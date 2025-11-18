<?php
// create_comments.php
// Creates a "comments" table in MySQL (PascalCase columns to match EF-created tables)

require_once __DIR__ . '/config.php';

$dry = true;
if (in_array('--apply', $argv ?? [])) $dry = false;

$sql = <<<SQL
CREATE TABLE IF NOT EXISTS `comments` (
  `CommentId` bigint(20) NOT NULL AUTO_INCREMENT,
  `UserId` bigint(20) DEFAULT NULL,
  `BookId` bigint(20) DEFAULT NULL,
  `CommentText` text,
  `CreatedAt` datetime DEFAULT NULL,
  `IsHidden` tinyint(1) DEFAULT 0,
  PRIMARY KEY (`CommentId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
SQL;

if ($dry) {
    echo "DRY RUN: Would execute create comments table SQL:\n";
    echo $sql . "\n";
    echo "Run with --apply to create the table.\n";
    exit;
}

if ($mysqli->query($sql) === TRUE) {
    echo "comments table created or already exists.\n";
} else {
    echo "Failed creating comments table: " . $mysqli->error . "\n";
}

$mysqli->close();

?>