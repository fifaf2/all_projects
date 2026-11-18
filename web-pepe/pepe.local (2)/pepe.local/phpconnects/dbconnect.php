<?php
$server_name = "127.0.0.1";
$username = "root";
$password = "password";
$db_name = "pepedb";
$port = "3306";

$conn = new mysqli($server_name, $username, $password, $db_name, $port);

if ($conn->connect_error) {
    die("<p>Ошибка в подключений к базе данных</p>");
}
?>
