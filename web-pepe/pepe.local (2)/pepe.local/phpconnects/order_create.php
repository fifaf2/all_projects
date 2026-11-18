<?php
require "dbconnect.php";

if (!isset($_COOKIE["saveLogin"])) {
    echo "<script>window.location.href='../signIn.php';</script>";
    exit();
}

$login = $_COOKIE["saveLogin"];
$user_query = mysqli_query(
    $conn,
    "SELECT id FROM users WHERE login = '$login'",
);
$user = mysqli_fetch_assoc($user_query);
$user_id = $user["id"];

$address = mysqli_real_escape_string($conn, $_POST["address"]);
$date_delivery = $_POST["date_delivery"];

$cart_query = mysqli_query(
    $conn,
    "
    SELECT basket.count, basket.product_id, products.cost
    FROM basket
    JOIN products ON basket.product_id = products.id
    WHERE basket.user_id = $user_id
",
);
$cart_items = mysqli_fetch_all($cart_query, MYSQLI_ASSOC);

if (empty($cart_items)) {
    echo "<script>window.location.href='../cart.php';</script>";
    exit();
}

$total = 0;
foreach ($cart_items as $item) {
    $total += $item["cost"] * $item["count"];
}

mysqli_query(
    $conn,
    "INSERT INTO orders (priceAll, userId, status, addressDelivery, dateDelivery)
     VALUES ($total, $user_id, 'Новый', '$address', '$date_delivery')",
);
$query = mysqli_query(
    $conn,
    "SELECT * FROM orders WHERE id = (SELECT MAX(id) FROM orders)",
);
$order = mysqli_fetch_assoc($query);
$order_id = $order["id"];

foreach ($cart_items as $item) {
    $product_id = $item["product_id"];
    $count = $item["count"];
    mysqli_query(
        $conn,
        "
        INSERT INTO orderItem (order_id, product_id, count)
        VALUES ($order_id, $product_id, $count)
    ",
    );
    // Уменьшаем остаток на складе
    mysqli_query(
        $conn,
        "
        UPDATE products SET count = count - $count WHERE id = $product_id
    ",
    );
}

// Очищаем корзину
mysqli_query($conn, "DELETE FROM basket WHERE user_id = $user_id");

echo "<script>
    alert('Заказ №$order_id успешно оформлен!');
    window.location.href='../profile.php';
</script>";
?>
