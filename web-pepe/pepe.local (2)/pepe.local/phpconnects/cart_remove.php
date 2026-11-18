<?php
require 'dbconnect.php';

if (!isset($_COOKIE["saveLogin"])) {
    echo "<script>window.location.href='../signIn.php';</script>";
    exit();
}

$basket_id = intval($_POST['basket_id']);
mysqli_query($conn, "DELETE FROM basket WHERE id = $basket_id");

echo "<script>window.location.href='../cart.php';</script>";
?>
