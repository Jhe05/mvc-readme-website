MySQL / phpMyAdmin: Favorites table

This folder contains a small SQL file to create a `favorites` table you can import into phpMyAdmin or run in MySQL.

Files:
- `favorites.sql` — CREATE TABLE statement for `favorites` (and optional sample INSERT comment).

Instructions (phpMyAdmin):
1. Open phpMyAdmin and select the database you want to add the table to.
2. Click the "Import" tab.
3. Choose `favorites.sql` from this folder and click "Go".
4. You should see a success message and the new `favorites` table in the schema.

Notes & troubleshooting:
- The SQL assumes your users table is named `users` and books table is named `books`, with primary key column `id` on each. If your tables use different names or column names, edit `favorites.sql` and change the `REFERENCES` clauses accordingly.
- If your database uses MyISAM, change the engine to `MyISAM`, but InnoDB is recommended for foreign keys.
- If phpMyAdmin reports a foreign-key error during import (for example because referenced tables don't yet exist), you can:
  - Temporarily remove the `CONSTRAINT ... FOREIGN KEY` lines, import the table, then add foreign keys manually once both `users` and `books` exist; or
  - Import the SQL from the database root after creating `users` and `books`.

If you want, I can also create a small PHP admin script (e.g., `add_favorite.php`, `remove_favorite.php`) that uses mysqli to insert/delete rows — tell me and I'll add them under `scripts/php_sql/` as well.