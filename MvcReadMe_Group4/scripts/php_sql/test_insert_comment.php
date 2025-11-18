<?php
// test_insert_comment.php bookId userId commentText
require_once __DIR__ . '/config.php';
if ($argc < 4) {
    echo "Usage: php test_insert_comment.php <bookId> <userId> <commentText>\n";
    exit(1);
}
$bookId = (int)$argv[1];
$userId = (int)$argv[2];
$text = $argv[3];

$ins = $mysqli->prepare('INSERT INTO comments (UserId, BookId, CommentText, CreatedAt, IsHidden) VALUES (?, ?, ?, ?, 0)');
$now = date('Y-m-d H:i:s');
$ins->bind_param('iiss', $userId, $bookId, $text, $now);
if (!$ins->execute()) {
    echo "Insert failed: " . $ins->error . "\n";
    exit(1);
}
echo "Inserted comment id: " . $mysqli->insert_id . "\n";
$ins->close();
$mysqli->close();
?>