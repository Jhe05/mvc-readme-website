<?php
/**
 * test_insert_favorite.php
 * Quick test to insert a favorite row into MySQL and report any error.
 * Usage: php test_insert_favorite.php [user_id] [book_id]
 */
$user = isset($argv[1]) ? intval($argv[1]) : 2;
$book = isset($argv[2]) ? intval($argv[2]) : 1;
require_once __DIR__ . '/config.php';
if (!($mysqli instanceof mysqli)) { echo "No MySQL connection\n"; exit(1); }

echo "Inserting favorite user_id=$user book_id=$book\n";
$stmt = $mysqli->prepare('INSERT INTO favorites (UserId, BookId, CreatedAt) VALUES (?, ?, NOW())');
if (!$stmt) { echo "Prepare failed: " . $mysqli->error . "\n"; exit(1); }
$stmt->bind_param('ii', $user, $book);
if (!$stmt->execute()) {
    echo "Execute failed: " . $stmt->error . "\n";
    exit(1);
}
echo "Inserted with id " . $stmt->insert_id . "\n";
$stmt->close();
$mysqli->close();

?>
