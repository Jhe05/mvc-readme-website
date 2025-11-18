<?php
// debug_cli.php - run admin_report.php with debug=1 from CLI
$_GET['mode'] = 'today';
$_GET['debug'] = '1';
require_once __DIR__ . '/admin_report.php';
