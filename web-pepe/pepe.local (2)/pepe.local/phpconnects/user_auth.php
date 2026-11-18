<?php
require 'dbconnect.php';

if (isset($_POST['loginUser']) && isset($_POST['passwordUser'])) {
    $login = $_POST['loginUser'];
    $password = $_POST['passwordUser'];

    $query_user = mysqli_query($conn, "SELECT * FROM users WHERE login = '$login' AND password = '$password' AND status = 'Активен'");

    if ($query_user) {
        if (mysqli_num_rows($query_user) > 0) {
            $user = mysqli_fetch_assoc($query_user);

            setcookie("saveLogin", $user['login'], time() + 3600, '/');

            echo "<script>
                window.location.href='../profile.php';
            </script>";
        } else {
            echo "<script>
                alert('Логин или пароль не верные');
                window.location.href = '../signIn.php';
            </script>";
        }
    }
}
?>?>