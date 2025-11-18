<?php
// Minimal autoloader for manually-installed TCPDF in vendor/tecnickcom/tcpdf
// This is only a lightweight fallback when Composer isn't used.

$possiblePaths = [
    __DIR__ . '/tecnickcom/tcpdf/tcpdf.php',
    __DIR__ . '/tecnickcom/tcpdf/tcpdf/tcpdf.php',
    __DIR__ . '/tecnickcom/TCPDF-master/tcpdf.php',
    __DIR__ . '/tecnickcom/TCPDF-master/tcpdf/tcpdf.php'
];

$found = false;
foreach ($possiblePaths as $p) {
    if (file_exists($p)) {
        require_once $p;
        $found = true;
        break;
    }
}

if (!$found) {
    http_response_code(500);
    echo "TCPDF not found. Please place the TCPDF library under vendor/tecnickcom/tcpdf and try again.";
    exit;
}
