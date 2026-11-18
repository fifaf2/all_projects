<?php
require 'dbconnect.php';

if (!isset($_COOKIE["saveLogin"])) {
    echo "<script>window.location.href='../signIn.php';</script>";
    exit();
}

$basket_id = intval($_POST['basket_id']);
$count = intval($_POST['count']);
if ($count < 1) $count = 1;

mysqli_query($conn, "UPDATE basket SET count = $count WHERE id = $basket_id");

echo "<script>window.location.href='../cart.php';</script>";
?>
