<?php
// rate_book.php
// POST params: book_id, user_id, rating
// Upserts the user's rating for the given book and inserts an admin notification

header('Content-Type: application/json');
// CORS & preflight handled in config.php
require_once __DIR__ . '/config.php';

$book_id = isset($_POST['book_id']) ? (int)$_POST['book_id'] : 0;
$user_id = isset($_POST['user_id']) ? (int)$_POST['user_id'] : 0;
$rating = isset($_POST['rating']) ? (int)$_POST['rating'] : 0;

if ($book_id <= 0 || $user_id <= 0 || $rating < 1 || $rating > 5) {
    echo json_encode(['success' => false, 'error' => 'Invalid parameters']);
    exit;
}

// Check if existing
$stmt = $mysqli->prepare('SELECT id, rating FROM book_ratings WHERE book_id = ? AND user_id = ?');
$stmt->bind_param('ii', $book_id, $user_id);
$stmt->execute();
$res = $stmt->get_result();
$now = date('Y-m-d H:i:s');
$action = 'created';
if ($row = $res->fetch_assoc()) {
    $existingId = (int)$row['id'];
    $existingRating = (int)$row['rating'];
    if ($existingRating === $rating) {
        // No change
        $stmt->close();
        // return current average
        $avgStmt = $mysqli->prepare('SELECT AVG(rating) AS avg_rating, COUNT(*) AS cnt FROM book_ratings WHERE book_id = ?');
        $avgStmt->bind_param('i', $book_id);
        $avgStmt->execute();
        $avgRes = $avgStmt->get_result()->fetch_assoc();
        echo json_encode(['success' => true, 'action' => 'none', 'avg' => (float)$avgRes['avg_rating'], 'count' => (int)$avgRes['cnt']]);
        exit;
    }
    // update
    $u = $mysqli->prepare('UPDATE book_ratings SET rating = ?, created_at = ? WHERE id = ?');
    $u->bind_param('isi', $rating, $now, $existingId);
    $u->execute();
    $u->close();
    $action = 'updated';
    $stmt->close();
} else {
    $stmt->close();
    // insert
    $i = $mysqli->prepare('INSERT INTO book_ratings (book_id, user_id, rating, created_at) VALUES (?, ?, ?, ?)');
    $i->bind_param('iiis', $book_id, $user_id, $rating, $now);
    $i->execute();
    $i->close();
    $action = 'created';
}

// compute new average
$avgStmt = $mysqli->prepare('SELECT AVG(rating) AS avg_rating, COUNT(*) AS cnt FROM book_ratings WHERE book_id = ?');
$avgStmt->bind_param('i', $book_id);
$avgStmt->execute();
$avgRes = $avgStmt->get_result()->fetch_assoc();
$avg = isset($avgRes['avg_rating']) ? (float)$avgRes['avg_rating'] : 0.0;
$cnt = isset($avgRes['cnt']) ? (int)$avgRes['cnt'] : 0;
$avgStmt->close();

// Build a notification message for admin
// Fetch username and book title for a nice message (best-effort)
$uname = 'User ' . $user_id;
$btitle = 'Book ' . $book_id;
$uStmt = $mysqli->prepare('SELECT UserName FROM users WHERE id = ?');
if ($uStmt) {
    $uStmt->bind_param('i', $user_id);
    $uStmt->execute();
    $r = $uStmt->get_result();
    if ($rr = $r->fetch_assoc()) $uname = $rr['UserName'];
    $uStmt->close();
}
$bStmt = $mysqli->prepare('SELECT Title FROM books WHERE id = ?');
if ($bStmt) {
    $bStmt->bind_param('i', $book_id);
    $bStmt->execute();
    $r2 = $bStmt->get_result();
    if ($rr2 = $r2->fetch_assoc()) $btitle = $rr2['Title'];
    $bStmt->close();
}

$message = sprintf('User %s rated %s with %d stars.', $mysqli->real_escape_string($uname), $mysqli->real_escape_string($btitle), $rating);

// Insert admin notification
$notStmt = $mysqli->prepare('INSERT INTO admin_notifications (message, created_at) VALUES (?, ?)');
if ($notStmt) {
    $notStmt->bind_param('ss', $message, $now);
    $notStmt->execute();
    $notStmt->close();
}

echo json_encode(['success' => true, 'action' => $action, 'avg' => $avg, 'count' => $cnt, 'message' => $message]);
