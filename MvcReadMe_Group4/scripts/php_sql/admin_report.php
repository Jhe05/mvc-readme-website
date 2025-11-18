<?php
// admin_report.php
// Generates a PDF report (users) using TCPDF and opens it in the browser.

// Require Composer autoloader (assumes TCPDF installed via Composer)
$autoload = __DIR__ . '/vendor/autoload.php';
if (!file_exists($autoload)) {
    header('Content-Type: text/plain; charset=utf-8');
    echo "Composer autoloader not found. Run `composer require tecnickcom/tcpdf` in the php_sql directory and ensure `vendor/autoload.php` exists.";
    exit;
}
require_once $autoload;

// Reuse existing config which sets up $mysqli (and CORS during development)
require_once __DIR__ . '/config.php';

// Determine report mode/date range. Default is 'today' (reset every day = daily report)
$mode = isset($_GET['mode']) ? $_GET['mode'] : 'today';
if ($mode === 'today') {
    $start = date('Y-m-d') . ' 00:00:00';
    $end = date('Y-m-d') . ' 23:59:59';
} else if (isset($_GET['date'])) {
    $d = preg_replace('/[^0-9\-]/', '', $_GET['date']);
    $start = $d . ' 00:00:00';
    $end = $d . ' 23:59:59';
} else {
    // default to all time
    $start = null;
    $end = null;
}

// Detect optional columns and tables
$hasUsersCreatedAt = false;
$colCheck = $mysqli->query("SHOW COLUMNS FROM `users` LIKE 'created_at'");
if ($colCheck && $colCheck->num_rows > 0) $hasUsersCreatedAt = true;

$hasRatingComment = false;
$colCheck2 = $mysqli->query("SHOW COLUMNS FROM `book_ratings` LIKE 'comment'");
if ($colCheck2 && $colCheck2->num_rows > 0) $hasRatingComment = true;

// Fetch users — simple query: just username and email (no role column)
$userCols = ['UserName AS name', 'Email AS email'];
if ($hasUsersCreatedAt) $userCols[] = 'created_at';
$sqlUsers = 'SELECT ' . implode(', ', $userCols) . ' FROM `users` ORDER BY Id ASC';
$resUsers = $mysqli->query($sqlUsers);
if (!$resUsers) {
    // Non-fatal: if users can't be queried (schema mismatch or other issue), continue with other sections
    // We'll render an empty users table instead of failing the whole report.
    $resUsers = null;
}

// Fetch ratings (optionally filtered by date)
$ratingCols = ['br.id', 'br.book_id', 'b.Title AS book_title', 'br.user_id', 'u.UserName AS username', 'br.rating', 'br.created_at'];
if ($hasRatingComment) $ratingCols[] = 'br.comment';
$sqlRatings = 'SELECT ' . implode(', ', $ratingCols) . ' FROM book_ratings br LEFT JOIN books b ON br.book_id = b.Id LEFT JOIN users u ON br.user_id = u.Id';
if ($start && $end) {
    $sqlRatings .= " WHERE br.created_at BETWEEN '" . $mysqli->real_escape_string($start) . "' AND '" . $mysqli->real_escape_string($end) . "'";
}
$sqlRatings .= ' ORDER BY br.created_at DESC';
$resRatings = $mysqli->query($sqlRatings);
if (!$resRatings) {
    header('Content-Type: text/plain; charset=utf-8');
    echo "DB query failed (ratings): " . $mysqli->error;
    exit;
}

// Fetch book reads: total from books.NumberOfReads and today's reads from bookreads (if available)
$sqlBooks = 'SELECT Id, Title, NumberOfReads FROM books ORDER BY Title';
$resBooks = $mysqli->query($sqlBooks);
if (!$resBooks) {
    header('Content-Type: text/plain; charset=utf-8');
    echo "DB query failed (books): " . $mysqli->error;
    exit;
}

// Prepare a map of today's reads per book (bookreads.ReadDate is text in this DB)
$todayReads = [];
if ($start && $end) {
    $todayPrefix = date('Y-m-d');
    // Try multiple table name variants because EF migrations and MySQL may have different casing (BookReads vs bookreads)
    $candidateTables = ['bookreads', 'BookReads'];
    foreach ($candidateTables as $tbl) {
        $sql = "SELECT BookId, SUM(ReadCount) AS cnt FROM `" . $mysqli->real_escape_string($tbl) . "` WHERE ReadDate LIKE CONCAT(?, '%') GROUP BY BookId";
        $stmt = $mysqli->prepare($sql);
        if ($stmt) {
            $stmt->bind_param('s', $todayPrefix);
            $stmt->execute();
            $r = $stmt->get_result();
            while ($rr = $r->fetch_assoc()) {
                $todayReads[(int)$rr['BookId']] = (int)$rr['cnt'];
            }
            $stmt->close();
            if (!empty($todayReads)) break; // stop if we found rows
        }
    }
}

// --- OPTIONAL: Also read from the ASP.NET application's SQLite DB so recent app actions (users/ratings/reads)
// are included in the report even if the web app writes to SQLite and not MySQL.
// Path to the SQLite DB used by the ASP.NET app (adjust if your file is different).
$sqlitePath = 'C:\\Users\\Trana\\Documents\\GitHub\\mvc-readme-website\\MvcReadMe_Group4\\MvcReadMe_Group4Context-f1f35934-980f-45be-b759-23b26e9dae99.db';
$sqliteUsers = [];
$sqliteRatings = [];
$sqliteTodayReads = [];
if (class_exists('SQLite3') && file_exists($sqlitePath)) {
    try {
        $sdb = new SQLite3($sqlitePath, SQLITE3_OPEN_READONLY);

        // Users from SQLite — just username and email (no role)
        $uquery = 'SELECT UserName AS name, Email AS email, (CASE WHEN EXISTS(SELECT 1 FROM pragma_table_info("users") WHERE name="created_at") THEN created_at ELSE NULL END) AS created_at FROM users';
        $ures = $sdb->query($uquery);
        while ($row = $ures->fetchArray(SQLITE3_ASSOC)) {
            $sqliteUsers[] = $row;
        }

        // Ratings from SQLite (filter by date if requested)
        $rquery = 'SELECT br.Id as id, br.BookId as book_id, b.Title as book_title, br.UserId as user_id, u.UserName as username, br.Rating as rating, br.created_at as created_at';
        // Check for comment column
        $hasComment = false;
        $colinfo = $sdb->query("PRAGMA table_info('book_ratings')");
        while ($ci = $colinfo->fetchArray(SQLITE3_ASSOC)) {
            if (strtolower($ci['name']) === 'comment') { $hasComment = true; break; }
        }
        if ($hasComment) $rquery .= ', br.comment as comment';
        $rquery .= ' FROM book_ratings br LEFT JOIN books b ON br.BookId = b.Id LEFT JOIN users u ON br.UserId = u.Id';
        if ($start && $end) {
            $rquery .= " WHERE br.created_at BETWEEN '" . SQLite3::escapeString($start) . "' AND '" . SQLite3::escapeString($end) . "'";
        }
        $rquery .= ' ORDER BY br.created_at DESC';
        $rres = $sdb->query($rquery);
        while ($rr = $rres->fetchArray(SQLITE3_ASSOC)) {
            $sqliteRatings[] = $rr;
        }

        // Today's reads from SQLite bookreads table (ReadDate stored as text)
        if ($start && $end) {
            $todayPrefix = date('Y-m-d');
            $brs = $sdb->query("SELECT BookId, SUM(ReadCount) AS cnt FROM bookreads WHERE ReadDate LIKE '" . SQLite3::escapeString($todayPrefix) . "%' GROUP BY BookId");
            while ($br = $brs->fetchArray(SQLITE3_ASSOC)) {
                $sqliteTodayReads[(int)$br['BookId']] = (int)$br['cnt'];
            }
        }

        $sdb->close();
    } catch (Exception $e) {
        // ignore SQLite errors but continue with MySQL data
    }
}

// (Debug block removed) Use the report endpoint normally. If you need diagnostics, re-add a guarded debug mode.

$pdfClass = '\\TCPDF';
if (!class_exists($pdfClass)) {
    header('Content-Type: text/plain; charset=utf-8');
    echo "TCPDF class not found even after autoload; ensure tecnickcom/tcpdf is installed via Composer and vendor/autoload.php is present.";
    exit;
}

// Create new PDF document
$pdf = new $pdfClass(PDF_PAGE_ORIENTATION, PDF_UNIT, PDF_PAGE_FORMAT, true, 'UTF-8', false);
$pdf->SetCreator('Admin Dashboard');
$pdf->SetAuthor('Admin');
$pdf->SetTitle('Admin Dashboard Report');
$pdf->SetSubject('Daily Admin Report');
$pdf->setPrintHeader(false);
$pdf->setPrintFooter(false);
$pdf->SetMargins(12, 18, 12);
$pdf->SetAutoPageBreak(TRUE, 15);
$pdf->AddPage();

// Header
$pdf->SetFont('helvetica', 'B', 16);
$pdf->Cell(0, 0, 'Admin Dashboard Report', 0, 1, 'C', 0, '', 0);
$pdf->Ln(2);
$pdf->SetFont('helvetica', '', 10);
$pdf->Cell(0, 0, 'Generated: ' . date('Y-m-d H:i:s'), 0, 1, 'C', 0, '', 0);
$pdf->Ln(6);

// Build HTML body: Users section, Ratings section, Books/Reads section
$html = '<style>table{border-collapse:collapse;font-size:11px;}th{background-color:#7d4bc8;color:#fff;padding:6px 8px;text-align:left;}td{padding:6px 8px;border-bottom:1px solid #eee;}</style>';

// Users will be appended below only if user data is available

// Merge MySQL users and SQLite users (dedupe by email if present, else username)
$allUsers = [];
if ($resUsers) {
    while ($u = $resUsers->fetch_assoc()) {
        $key = !empty($u['email']) ? strtolower($u['email']) : strtolower($u['name']);
        $allUsers[$key] = [
            'name' => $u['name'] ?? '',
            'email' => $u['email'] ?? '',
            'created_at' => $u['created_at'] ?? null
        ];
    }
} else {
    // users not available; leave $allUsers empty so report still generates
}
foreach ($sqliteUsers as $su) {
    $key = !empty($su['email']) ? strtolower($su['email']) : strtolower($su['name']);
    if (!isset($allUsers[$key])) {
        $allUsers[$key] = [
            'name' => $su['name'] ?? '',
            'email' => $su['email'] ?? '',
            'created_at' => $su['created_at'] ?? null
        ];
    }
}if (!empty($allUsers)) {
    // Render users table header (no role column)
    $html .= '<h3>Users</h3>';
    $html .= '<table width="100%"><thead><tr><th>Name</th><th>Email</th>';
    if ($hasUsersCreatedAt) $html .= '<th>Date Created</th>';
    $html .= '</tr></thead><tbody>';

    foreach ($allUsers as $u) {
        $html .= '<tr>';
        $html .= '<td>' . htmlspecialchars($u['name']) . '</td>';
        $html .= '<td>' . htmlspecialchars($u['email']) . '</td>';
        if ($hasUsersCreatedAt) $html .= '<td>' . htmlspecialchars($u['created_at']) . '</td>';
        $html .= '</tr>';
    }

    $html .= '</tbody></table><br/>';
} else {
    // No users data available — skip users table
}

// Free-form comments from a dedicated comments table (MySQL + SQLite)
$comments = [];

// Try to locate a comments-like table and adapt to column name variations.
$candidateTables = ['comments', 'Comments'];
$foundTable = null;
foreach ($candidateTables as $ct) {
    $tblCheck = $mysqli->query("SHOW TABLES LIKE '" . $mysqli->real_escape_string($ct) . "'");
    if ($tblCheck && $tblCheck->num_rows > 0) { $foundTable = $ct; break; }
}

if ($foundTable) {
    // Inspect columns to map names (case variants)
    $cols = [];
    $colRes = $mysqli->query("SHOW COLUMNS FROM `" . $mysqli->real_escape_string($foundTable) . "`");
    if ($colRes) {
        while ($cr = $colRes->fetch_assoc()) {
            $cols[] = $cr['Field'];
        }
        $colRes->free();
    }

    // Helper to pick first matching column from candidates
    $pick = function($candidates) use ($cols) {
        foreach ($candidates as $cand) {
            foreach ($cols as $col) {
                if (strcasecmp($col, $cand) === 0) return $col;
            }
        }
        return null;
    };

    $idCol = $pick(['CommentId','id','Id']);
    $bookCol = $pick(['BookId','book_id','bookId']);
    $userIdCol = $pick(['UserId','user_id','userId']);
    $userNameCol = $pick(['UserName','username','name','user_name']);
    $commentCol = $pick(['CommentText','comment','Comment']);
    $createdCol = $pick(['CreatedAt','created_at','createdAt','created']);

    // Build SELECT with safe aliases
    $sel = [];
    $sel[] = ($idCol ? ('c.`' . $idCol . '` AS id') : 'NULL AS id');
    $sel[] = ($bookCol ? ('c.`' . $bookCol . '` AS book_id') : 'NULL AS book_id');
    $sel[] = "b.Title AS book_title";
    if ($userIdCol) $sel[] = ('c.`' . $userIdCol . '` AS user_id'); else $sel[] = 'NULL AS user_id';
    // For username, try to join users if userId exists; otherwise try to read username column directly
    $joinUsers = $userIdCol ? true : false;
    if ($userNameCol && !$joinUsers) {
        $sel[] = ('c.`' . $userNameCol . '` AS username');
    } else if ($joinUsers) {
        $sel[] = 'u.UserName AS username';
    } else {
        $sel[] = "NULL AS username";
    }
    $sel[] = ($commentCol ? ('c.`' . $commentCol . '` AS comment') : 'NULL AS comment');
    $sel[] = ($createdCol ? ('c.`' . $createdCol . '` AS created_at') : 'NULL AS created_at');

    $sqlC = 'SELECT ' . implode(', ', $sel) . ' FROM `' . $mysqli->real_escape_string($foundTable) . '` c LEFT JOIN books b ON c.' . ($bookCol ? ('`' . $bookCol . '`') : '0') . ' = b.Id';
    if ($joinUsers) $sqlC .= ' LEFT JOIN users u ON c.`' . $userIdCol . '` = u.Id';

    if ($createdCol && $start && $end) {
        $sqlC .= " WHERE c.`" . $mysqli->real_escape_string($createdCol) . "` BETWEEN '" . $mysqli->real_escape_string($start) . "' AND '" . $mysqli->real_escape_string($end) . "'";
    }
    $sqlC .= ' ORDER BY ' . ($createdCol ? ('c.`' . $createdCol . '` DESC') : 'c.`' . ($idCol ?? 'id') . '` DESC');

    $resC = $mysqli->query($sqlC);
    if ($resC) {
        while ($r = $resC->fetch_assoc()) {
            // normalize username if missing and we have user_id
            if ((empty($r['username']) || $r['username'] === null) && !empty($r['user_id'])) {
                $r['username'] = 'User ' . $r['user_id'];
            }
            $comments[] = $r;
        }
        $resC->free();
    }
}

// Also include comments from the ASP.NET SQLite DB if available
if (class_exists('SQLite3') && file_exists($sqlitePath)) {
    try {
        $sdb = new SQLite3($sqlitePath, SQLITE3_OPEN_READONLY);
        // Check if comments table exists
        $tblInfo = $sdb->query("SELECT name FROM sqlite_master WHERE type='table' AND name='comments'");
        if ($tblInfo && $tblInfo->fetchArray()) {
            $cquery = 'SELECT c.CommentId as id, c.BookId as book_id, b.Title as book_title, c.UserId as user_id, u.UserName as username, c.CommentText as comment, c.CreatedAt as created_at FROM comments c LEFT JOIN books b ON c.BookId = b.Id LEFT JOIN users u ON c.UserId = u.Id';
            if ($start && $end) {
                $cquery .= " WHERE c.CreatedAt BETWEEN '" . SQLite3::escapeString($start) . "' AND '" . SQLite3::escapeString($end) . "'";
            }
            $cquery .= ' ORDER BY c.CreatedAt DESC';
            $cres = $sdb->query($cquery);
            while ($cr = $cres->fetchArray(SQLITE3_ASSOC)) {
                $comments[] = $cr;
            }
        }
        $sdb->close();
    } catch (Exception $e) {
        // ignore
    }
}

// If debug query param present, output diagnostics to help troubleshoot missing comments
if (isset($_GET['debug']) && ($_GET['debug'] === '1' || $_GET['debug'] === 'true')) {
    header('Content-Type: application/json');
    $diag = [];
    $diag['mode'] = $mode;
    $diag['start'] = $start;
    $diag['end'] = $end;
    // MySQL comments-like tables
    $tbls = [];
    $res = $mysqli->query("SHOW TABLES");
    if ($res) {
        while ($r = $res->fetch_row()) {
            $tbls[] = $r[0];
        }
        $res->free();
    }
    $diag['tables'] = array_values($tbls);
    // find tables containing 'comment'
    $matches = array_values(array_filter($tbls, function($t){ return stripos($t,'comment') !== false; }));
    $diag['tables_with_comment'] = $matches;

    // For each candidate table, attempt to get column listing and count
    $tblInfo = [];
    foreach ($matches as $t) {
        $info = ['table' => $t, 'columns' => [], 'count' => null, 'sample' => []];
        $cres = $mysqli->query("SHOW COLUMNS FROM `" . $mysqli->real_escape_string($t) . "`");
        if ($cres) {
            while ($c = $cres->fetch_assoc()) $info['columns'][] = $c['Field'];
            $cres->free();
        }
        // try count
        $cres2 = $mysqli->query("SELECT COUNT(*) AS cnt FROM `" . $mysqli->real_escape_string($t) . "`");
        if ($cres2) {
            $r2 = $cres2->fetch_assoc(); $info['count'] = isset($r2['cnt']) ? (int)$r2['cnt'] : null; $cres2->free();
        }
        // sample up to 10 rows
        $sres = $mysqli->query("SELECT * FROM `" . $mysqli->real_escape_string($t) . "` LIMIT 10");
        if ($sres) {
            while ($s = $sres->fetch_assoc()) $info['sample'][] = $s;
            $sres->free();
        }
        $tblInfo[] = $info;
    }
    $diag['comment_table_info'] = $tblInfo;

    // include the comments array we already collected (may be empty)
    $diag['collected_comments_count'] = count($comments);
    $diag['collected_comments_sample'] = array_slice($comments,0,20);

    echo json_encode($diag, JSON_PRETTY_PRINT | JSON_UNESCAPED_SLASHES);
    exit;
}

// Emit comments section if any
if (!empty($comments)) {
    // Fill missing usernames and book titles by looking up users/books (cache to avoid repeated queries)
    $userCache = [];
    $bookCache = [];
    foreach ($comments as &$citem) {
        // username
        if ((empty($citem['username']) || $citem['username'] === null) && !empty($citem['user_id'])) {
            $uid = (int)$citem['user_id'];
            if (!isset($userCache[$uid])) {
                $urow = null;
                $ures = $mysqli->query("SELECT UserName FROM users WHERE Id = " . intval($uid) . " LIMIT 1");
                if ($ures) { $urow = $ures->fetch_assoc(); $ures->free(); }
                $userCache[$uid] = $urow && !empty($urow['UserName']) ? $urow['UserName'] : ('User ' . $uid);
            }
            $citem['username'] = $userCache[$uid];
        }
        // book title
        if ((empty($citem['book_title']) || $citem['book_title'] === null) && !empty($citem['book_id'])) {
            $bid = (int)$citem['book_id'];
            if (!isset($bookCache[$bid])) {
                $brow = null;
                $bres = $mysqli->query("SELECT Title FROM books WHERE Id = " . intval($bid) . " LIMIT 1");
                if ($bres) { $brow = $bres->fetch_assoc(); $bres->free(); }
                $bookCache[$bid] = $brow && !empty($brow['Title']) ? $brow['Title'] : ('Book ' . $bid);
            }
            $citem['book_title'] = $bookCache[$bid];
        }
    }
    unset($citem);

    $html .= '<h3>Free-form Comments</h3>';
    $html .= '<table width="100%"><thead><tr><th>Book</th><th>User</th><th>Comment</th><th>When</th></tr></thead><tbody>';
    foreach ($comments as $c) {
        $html .= '<tr>';
        $html .= '<td>' . htmlspecialchars($c['book_title'] ?? ('#' . ($c['book_id'] ?? ''))) . '</td>';
        $html .= '<td>' . htmlspecialchars($c['username'] ?? ('User ' . ($c['user_id'] ?? ''))) . '</td>';
        $html .= '<td>' . htmlspecialchars($c['comment'] ?? '') . '</td>';
        $html .= '<td>' . htmlspecialchars($c['created_at'] ?? '') . '</td>';
        $html .= '</tr>';
    }
    $html .= '</tbody></table><br/>';
}

// Ratings (separate comments from rating entries)
$html .= '<h3>Ratings ' . ($start ? '(Filtered: ' . htmlspecialchars($start) . ' to ' . htmlspecialchars($end) . ')' : '') . '</h3>';
$html .= '<table width="100%"><thead><tr><th>Book</th><th>User</th><th>Rating</th><th>When</th></tr></thead><tbody>';

// combine MySQL and SQLite ratings; collect rating comments separately
$allRatings = [];
$ratingComments = [];
while ($r = $resRatings->fetch_assoc()) {
    $allRatings[] = $r;
    if (isset($r['comment']) && $r['comment'] !== null && trim($r['comment']) !== '') {
        $ratingComments[] = $r;
    }
}
foreach ($sqliteRatings as $sr) {
    $allRatings[] = $sr;
    if (isset($sr['comment']) && $sr['comment'] !== null && trim($sr['comment']) !== '') {
        $ratingComments[] = $sr;
    }
}

// Ratings table rows (no comments column)
foreach ($allRatings as $r) {
    $html .= '<tr>';
    $html .= '<td>' . htmlspecialchars($r['book_title'] ?? ('#' . ($r['book_id'] ?? ''))) . '</td>';
    $html .= '<td>' . htmlspecialchars($r['username'] ?? ('User ' . ($r['user_id'] ?? ''))) . '</td>';
    $html .= '<td>' . htmlspecialchars($r['rating'] ?? '') . '</td>';
    $html .= '<td>' . htmlspecialchars($r['created_at'] ?? '') . '</td>';
    $html .= '</tr>';
}
$html .= '</tbody></table><br/>';

// If there are comments attached to ratings, print them in a separate table
if (!empty($ratingComments)) {
    $html .= '<h3>Rating Comments ' . ($start ? '(Filtered: ' . htmlspecialchars($start) . ' to ' . htmlspecialchars($end) . ')' : '') . '</h3>';
    $html .= '<table width="100%"><thead><tr><th>Book</th><th>User</th><th>Comment</th><th>When</th></tr></thead><tbody>';
    foreach ($ratingComments as $rc) {
        $html .= '<tr>';
        $html .= '<td>' . htmlspecialchars($rc['book_title'] ?? ('#' . ($rc['book_id'] ?? ''))) . '</td>';
        $html .= '<td>' . htmlspecialchars($rc['username'] ?? ('User ' . ($rc['user_id'] ?? ''))) . '</td>';
        $html .= '<td>' . htmlspecialchars($rc['comment'] ?? '') . '</td>';
        $html .= '<td>' . htmlspecialchars($rc['created_at'] ?? '') . '</td>';
        $html .= '</tr>';
    }
    $html .= '</tbody></table><br/>';
}

// Books / Reads
$html .= '<h3>Books & Reads (today)</h3>';
$html .= '<table width="100%"><thead><tr><th>Title</th><th>Total Reads</th><th>Today Reads</th></tr></thead><tbody>';
while ($b = $resBooks->fetch_assoc()) {
    $bookId = (int)$b['Id'];
    $total = isset($b['NumberOfReads']) ? (int)$b['NumberOfReads'] : 0;
    // include today's reads from MySQL and optionally from the ASP.NET app's SQLite DB
    $today = (isset($todayReads[$bookId]) ? $todayReads[$bookId] : 0) + (isset($sqliteTodayReads[$bookId]) ? $sqliteTodayReads[$bookId] : 0);
    $html .= '<tr>';
    $html .= '<td>' . htmlspecialchars($b['Title']) . '</td>';
    $html .= '<td>' . $total . '</td>';
    $html .= '<td>' . $today . '</td>';
    $html .= '</tr>';
}
$html .= '</tbody></table>';

$pdf->writeHTML($html, true, false, true, false, '');

// Output inline
$pdf->Output('admin_report_' . date('Ymd') . '.pdf', 'I');

// Close
$resUsers->free();
$resRatings->free();
$resBooks->free();
$mysqli->close();

?>
