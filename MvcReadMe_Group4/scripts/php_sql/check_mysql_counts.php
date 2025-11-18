<?php
require_once __DIR__ . '/config.php';
if (!($mysqli instanceof mysqli)) { echo "No MySQL connection\n"; exit(1); }
$tables = ['users','books','favorites'];
foreach ($tables as $t) {
    $res = $mysqli->query("SELECT COUNT(*) AS cnt FROM `" . $mysqli->real_escape_string($t) . "`");
    if ($res) {
        $r = $res->fetch_assoc();
        echo "$t: " . $r['cnt'] . "\n";
    } else {
        echo "$t: ERROR - " . $mysqli->error . "\n";
    }
}

// show sample users and books ids
$u = $mysqli->query("SELECT Id, UserName, Email FROM users ORDER BY Id LIMIT 10");
if ($u) {
    echo "\nSample users:\n";
    while ($r = $u->fetch_assoc()) {
        echo "Id=" . $r['Id'] . " UserName=" . $r['UserName'] . " Email=" . $r['Email'] . "\n";
    }
}
$b = $mysqli->query("SELECT Id, Title FROM books ORDER BY Id LIMIT 10");
if ($b) {
    echo "\nSample books:\n";
    while ($r = $b->fetch_assoc()) {
        echo "Id=" . $r['Id'] . " Title=" . $r['Title'] . "\n";
    }
}
$mysqli->close();

?>
