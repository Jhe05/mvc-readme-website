<?php
// get_average_rating.php
// GET param: book_id

header('Content-Type: application/json');
// CORS & preflight handled in config.php
require_once __DIR__ . '/config.php';

$book_id = isset($_GET['book_id']) ? (int)$_GET['book_id'] : 0;
if ($book_id <= 0) { echo json_encode(['success' => false, 'error' => 'Invalid book_id']); exit; }

$stmt = $mysqli->prepare('SELECT AVG(rating) AS avg_rating, COUNT(*) AS cnt FROM book_ratings WHERE book_id = ?');
$stmt->bind_param('i', $book_id);
$stmt->execute();
$res = $stmt->get_result()->fetch_assoc();
$avg = isset($res['avg_rating']) ? (float)$res['avg_rating'] : 0.0;
$cnt = isset($res['cnt']) ? (int)$res['cnt'] : 0;
$stmt->close();

echo json_encode(['success' => true, 'avg' => $avg, 'count' => $cnt]);
