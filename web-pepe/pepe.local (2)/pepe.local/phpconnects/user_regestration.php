<?php
require "dbconnect.php";
echo "<pre>";
print_r($_POST);
echo "</pre>";
if (isset($_POST["loginUser"]) && isset($_POST["passwordUser"])) {
    echo "<p>Загружены данные в post</p>";
    $login = $_POST["loginUser"];
    $password = $_POST["passwordUser"];
    $name = $_POST["nameUser"];
    $surname = $_POST["surnameUser"];
    $patronomic = $_POST["patronymicUser"];
    $query_user = mysqli_query(
        $conn,
        "select * from users where login = '$login'",
    );
    if ($query_user) {
        if (mysqli_num_rows($query_user) > 0) {
            $user = mysqli_fetch_assoc($query_user);

            if ($user["status"] === "Активен") {
                echo "<h1>Такой пользователь уже существует.</h1>
                <script>
                    alert('Пользователь существует');
                    window.location.href='../signIn.php';
                </script>";
            } else {
                $user = [
                    $login,
                    $password,
                    $name,
                    $surname,
                    $patronomic,
                    ($status = "Активен"),
                    ($role = "Клиент"),
                ];
                echo "<pre>";
                print_r($user);
                echo "</pre>";

                mysqli_query(
                    $conn,
                    "INSERT INTO pepedb.users (surname, name, patronymic, status, role, login, password)
                 VALUES ('$user[2]', '$user[3]', '$user[4]', 'Активен', 'Клиент', '$user[0]',  '$user[1]')",
                );

                echo "<h1>Регистрация пользователя прошла успешно</h1>
                        <script>
                            alert('Регистрация завершена успешно!');
                            window.location.href = '../signIn.php';
                        </script>";
            }
        } else {
            $user = [
                $login,
                $password,
                $name,
                $surname,
                $patronomic,
                ($status = "Активен"),
                ($role = "Клиент"),
            ];
            echo "<pre>";
            print_r($user);
            echo "</pre>";

            mysqli_query(
                $conn,
                "INSERT INTO pepedb.users (surname, name, patronymic, status, role, login, password)
             VALUES ('$user[2]', '$user[3]', '$user[4]', 'Активен', 'Клиент', '$user[0]',  '$user[1]')",
            );

            echo "<h1>Регистрация пользователя прошла успешно</h1>
                    <script>
                        alert('Регистрация завершена успешно!');
                        window.location.href = '../signIn.php';
                    </script>";
        }
    }
} else {
    echo "<h1>Регистрация пользователя прошла успешно</h1>
            <script>
                alert('Регистрация завершена успешно!');
                window.location.href = '../signIn.php';
            </script>";
}
?>
