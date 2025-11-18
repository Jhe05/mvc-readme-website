<?php
// admin_notifications.php
// Returns recent admin notifications

header('Content-Type: application/json');
// CORS & preflight handled in config.php
require_once __DIR__ . '/config.php';

$limit = isset($_GET['limit']) ? (int)$_GET['limit'] : 25;
if ($limit <= 0 || $limit > 200) $limit = 25;

$stmt = $mysqli->prepare('SELECT id, message, created_at FROM admin_notifications ORDER BY created_at DESC LIMIT ?');
$stmt->bind_param('i', $limit);
$stmt->execute();
$res = $stmt->get_result();
$rows = [];
while ($r = $res->fetch_assoc()) {
    $rows[] = $r;
}
$stmt->close();

echo json_encode(['success' => true, 'notifications' => $rows]);
