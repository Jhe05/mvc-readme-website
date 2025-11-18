<?php
// report_cli.php - generate the PDF report to stdout (no debug)
$_GET['mode'] = 'today';
require_once __DIR__ . '/admin_report.php';
