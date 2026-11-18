<?php
require 'dbconnect.php';

if (!isset($_COOKIE["saveLogin"])) {
    echo "<script>window.location.href='../signIn.php';</script>";
    exit();
}

$login = $_COOKIE["saveLogin"];
$user_query = mysqli_query($conn, "SELECT id FROM users WHERE login = '$login'");
$user = mysqli_fetch_assoc($user_query);

if (!$user) {
    exit();
}

$user_id = $user['id'];
$product_id = intval($_POST['product_id']);
$count = intval($_POST['count']);

if ($count < 1) $count = 1;

// Проверяем есть ли уже этот товар в корзине
$check = mysqli_query($conn, "SELECT * FROM basket WHERE user_id = $user_id AND product_id = $product_id");

if (mysqli_num_rows($check) > 0) {
    // Увеличиваем количество
    $existing = mysqli_fetch_assoc($check);
    $new_count = $existing['count'] + $count;
    mysqli_query($conn, "UPDATE basket SET count = $new_count WHERE user_id = $user_id AND product_id = $product_id");
} else {
    // Добавляем новый товар
    mysqli_query($conn, "INSERT INTO basket (user_id, product_id, count) VALUES ($user_id, $product_id, $count)");
}

echo "<script>window.location.href='../cart.php';</script>";
?>
