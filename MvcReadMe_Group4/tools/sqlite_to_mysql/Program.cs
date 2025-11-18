using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.Json;
using Microsoft.Data.Sqlite;
using MySqlConnector;

// Minimal SQLite -> MySQL transfer tool.
// Usage:
// dotnet run --project tools/sqlite_to_mysql -- "C:\path\to\db.db" "Server=127.0.0.1;User ID=root;Password=;Database=mvcreadme_db;"

class TransferProgram
{
    static int Main(string[] args)
    {
        try
        {
            var cwd = Directory.GetCurrentDirectory();

            string? sqlitePath = null;
            string? mysqlConn = null;

            if (args.Length >= 1 && !string.IsNullOrWhiteSpace(args[0])) sqlitePath = args[0];
            if (args.Length >= 2 && !string.IsNullOrWhiteSpace(args[1])) mysqlConn = args[1];

            if (sqlitePath == null)
            {
                // look up to 4 parent folders for appsettings.json
                var dir = new DirectoryInfo(cwd);
                for (int i = 0; i < 5 && dir != null; i++)
                {
                    var af = Path.Combine(dir.FullName, "appsettings.json");
                    if (File.Exists(af))
                    {
                        try
                        {
                            using var fs = File.OpenRead(af);
                            var doc = JsonDocument.Parse(fs);
                            if (doc.RootElement.TryGetProperty("ConnectionStrings", out var cs))
                            {
                                if (cs.TryGetProperty("MvcReadMe_Group4Context", out var val))
                                {
                                    var ds = val.GetString();
                                    // expecting "Data Source=xxx.db"
                                    if (!string.IsNullOrEmpty(ds))
                                    {
                                        var parts = ds.Split('=', 2);
                                        if (parts.Length == 2)
                                        {
                                            sqlitePath = parts[1].Trim().Trim('"');
                                            // if relative path, make absolute
                                            if (!Path.IsPathRooted(sqlitePath))
                                                sqlitePath = Path.GetFullPath(Path.Combine(dir.FullName, sqlitePath));
                                        }
                                    }
                                }
                            }
                        }
                        catch { }
                        break;
                    }
                    dir = dir.Parent;
                }
            }

            if (mysqlConn == null)
            {
                // default to local root with database name mvcreadme_db
                mysqlConn = "Server=127.0.0.1;Port=3306;User ID=root;Password=;Database=mvcreadme_db;SslMode=None;";
            }

            if (string.IsNullOrEmpty(sqlitePath) || !File.Exists(sqlitePath))
            {
                Console.Error.WriteLine($"SQLite file not found or not provided. Tried: {sqlitePath}");
                return 2;
            }

            Console.WriteLine($"Using SQLite: {sqlitePath}");
            Console.WriteLine($"Using MySQL connection: {mysqlConn}");

            using var sqliteConn = new SqliteConnection($"Data Source={sqlitePath}");
            sqliteConn.Open();

            var tables = new List<string>();
            using (var cmd = sqliteConn.CreateCommand())
            {
                cmd.CommandText = "SELECT name FROM sqlite_master WHERE type='table' AND name NOT LIKE 'sqlite_%';";
                using var rdr = cmd.ExecuteReader();
                while (rdr.Read()) tables.Add(rdr.GetString(0));
            }

            if (!tables.Any())
            {
                Console.WriteLine("No user tables found in SQLite DB.");
                return 0;
            }

            // Ensure database exists (connect to server without Database first)
            MySqlConnection? mysqlConnObj = null;
            try
            {
                var csb = new MySqlConnectionStringBuilder(mysqlConn);
                var db = csb.Database;
                if (string.IsNullOrEmpty(db)) db = "mvcreadme_db";

                // connect to server without a default database to create the database safely
                var serverCsb = new MySqlConnectionStringBuilder(mysqlConn);
                serverCsb.Database = "";
                using (var serverConn = new MySqlConnection(serverCsb.ConnectionString))
                {
                    serverConn.Open();
                    using var cmd = serverConn.CreateCommand();
                    cmd.CommandText = $"CREATE DATABASE IF NOT EXISTS `{db}` CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;";
                    cmd.ExecuteNonQuery();
                }

                // now open the connection specifying the database
                mysqlConnObj = new MySqlConnection(mysqlConn);
                mysqlConnObj.Open();

                using (var cmd = mysqlConnObj.CreateCommand())
                {
                    cmd.CommandText = $"USE `{db}`;";
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Failed to ensure/create target database: " + ex.Message);
                return 3;
            }

            foreach (var t in tables)
            {
                Console.WriteLine($"Processing table: {t}");
                var columns = new List<(string name, string type, bool pk)>();
                using (var cmd = sqliteConn.CreateCommand())
                {
                    cmd.CommandText = $"PRAGMA table_info('{t.Replace("'", "''")}')";
                    using var rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        var name = rdr.GetString(1);
                        var type = rdr.GetString(2);
                        var pk = rdr.GetInt32(5) != 0;
                        columns.Add((name, type, pk));
                    }
                }

                // Build DROP + CREATE
                using (var cmd = mysqlConnObj.CreateCommand())
                {
                    cmd.CommandText = $"DROP TABLE IF EXISTS `{t}`;";
                    cmd.ExecuteNonQuery();

                    var colDefs = new List<string>();
                    foreach (var c in columns)
                    {
                        var mysqlType = MapSqliteTypeToMySql(c.type, c.pk);
                        var def = $"`{c.name}` {mysqlType}";
                        if (c.pk && mysqlType.StartsWith("BIGINT", StringComparison.OrdinalIgnoreCase))
                            def += " PRIMARY KEY AUTO_INCREMENT";
                        colDefs.Add(def);
                    }

                    // if no primary key detected, don't set PK automatically
                    var createSql = $"CREATE TABLE `{t}` ({string.Join(", ", colDefs)}) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;";
                    cmd.CommandText = createSql;
                    cmd.ExecuteNonQuery();
                }

                // Copy rows
                using (var sCmd = sqliteConn.CreateCommand())
                {
                    sCmd.CommandText = $"SELECT * FROM `{t}`;";
                    using var rdr = sCmd.ExecuteReader();
                    var colNames = Enumerable.Range(0, rdr.FieldCount).Select(i => rdr.GetName(i)).ToArray();

                    // Prepare insert statement
                    var placeholders = string.Join(", ", colNames.Select((c, i) => "@p" + i));
                    var colsEsc = string.Join(", ", colNames.Select(c => "`" + c + "`"));
                    var insertSql = $"INSERT INTO `{t}` ({colsEsc}) VALUES ({placeholders});";

                    using var trans = mysqlConnObj.BeginTransaction();
                    using var iCmd = mysqlConnObj.CreateCommand();
                    iCmd.Transaction = trans;
                    iCmd.CommandText = insertSql;

                    // create parameters
                    iCmd.Parameters.Clear();
                    for (int i = 0; i < colNames.Length; i++)
                    {
                        iCmd.Parameters.Add(new MySqlParameter("@p" + i, MySqlDbType.VarChar));
                    }

                    int inserted = 0;
                    while (rdr.Read())
                    {
                        for (int i = 0; i < colNames.Length; i++)
                        {
                            var val = rdr.IsDBNull(i) ? null : rdr.GetValue(i);
                            iCmd.Parameters[i].Value = val ?? DBNull.Value;
                        }
                        iCmd.ExecuteNonQuery();
                        inserted++;
                    }
                    trans.Commit();
                    Console.WriteLine($"Inserted {inserted} rows into `{t}`.");
                }
            }

            Console.WriteLine("Transfer complete.");
            return 0;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Unhandled error: " + ex);
            return 100;
        }
    }

    static string MapSqliteTypeToMySql(string sqliteType, bool pk)
    {
        if (string.IsNullOrWhiteSpace(sqliteType)) return pk ? "BIGINT" : "TEXT";
        var t = sqliteType.ToLowerInvariant();
        if (t.Contains("int")) return "BIGINT";
        if (t.Contains("char") || t.Contains("clob") || t.Contains("text")) return "TEXT";
        if (t.Contains("blob")) return "BLOB";
        if (t.Contains("real") || t.Contains("floa") || t.Contains("doub")) return "DOUBLE";
        return "TEXT";
    }
}
