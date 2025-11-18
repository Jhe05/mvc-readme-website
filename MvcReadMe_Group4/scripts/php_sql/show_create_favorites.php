<?php
require_once __DIR__ . '/config.php';
if (!($mysqli instanceof mysqli)) { echo "No MySQL connection\n"; exit(1); }
$res = $mysqli->query("SHOW CREATE TABLE `favorites`");
if (!$res) { echo "Query failed: " . $mysqli->error . "\n"; exit(1); }
$row = $res->fetch_assoc();
echo $row['Create Table'] . "\n";
$mysqli->close();

?>
