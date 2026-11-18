<?php
require "dbconnect.php";
$action = $_POST["action"];

if (!isset($_COOKIE["saveLogin"])) {
    die("Ошибка: Вы не авторизованы");
}

$currentLogin = $_COOKIE["saveLogin"];

if ($action === "update") {
    if (isset($_POST["loginUser"]) && isset($_POST["passwordUser"])) {
        $newLogin = $_POST["loginUser"];
        $password = $_POST["passwordUser"];
        $name = $_POST["nameUser"];
        $surname = $_POST["surnameUser"];
        $patronymic = $_POST["patronymicUser"];

        if ($newLogin !== $currentLogin) {
            $check_login = mysqli_query(
                $conn,
                "SELECT * FROM users WHERE login = '$newLogin'",
            );
            if (mysqli_num_rows($check_login) > 0) {
                $user = mysqli_fetch_assoc($check_login);
                if ($user["status"] === "Активен") {
                    die(
                        "<script>alert('$user Ошибка: Логин $newLogin уже занят другим пользователем!');window.location.href = '../profile.php'</script>"
                    );
                }
            }
        }

        $sql = "UPDATE users SET
            surname = '$surname',
            name = '$name',
            patronymic = '$patronymic',
            login = '$newLogin',
            password = '$password'
            WHERE login = '$currentLogin'";

        if (mysqli_query($conn, $sql)) {
            if ($newLogin !== $currentLogin) {
                setcookie("saveLogin", $newLogin, time() + 3600, "/");
            }
            echo "<script>
                    alert('Данные профиля успешно обновлены!');
                    location.href='../profile.php';
                </script>";
        } else {
            echo "Ошибка БД: " . mysqli_error($conn);
        }
    } else {
        echo "<p>Данные для обновления не получены</p>";
    }
} elseif ($action === "delete") {
    if (isset($_COOKIE["saveLogin"])) {
        $loginToDelete = $_COOKIE["saveLogin"];

        $sql = "update users set status = 'Удалён' where login = '$loginToDelete'";

        if (mysqli_query($conn, $sql)) {
            setcookie("saveLogin", "", time() - 3600, "/");

            echo "<script>
                        alert('Ваш аккаунт был деактивирован.');
                        location.href='../index.php';
                    </script>";
        }
    } else {
        echo "Ошибка при удалении: " . mysqli_error($conn);
    }
} elseif ($action === "quit") {
    if (isset($_COOKIE["saveLogin"])) {
        setcookie("saveLogin", "", time() - 3600, "/");

        echo "<h1>Вы вышли из аккаунта. Возвращайитесь!</h1>";
        echo "<script>location.href='../index.php';</script>";
    } else {
        echo "Ошибка: Пользователь не найден.";
    }
}
?>
