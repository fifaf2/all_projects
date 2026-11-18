<!doctype html>
<html lang="en">

<head>
  <meta charset="UTF-8" />
  <meta name="viewport" content="width=device-width, initial-scale=1.0" />
  <title>Document</title>
  <link rel="stylesheet" href="./styles/main.css" />
  <link rel="stylesheet" href="./styles/header.css" />
  <link rel="stylesheet" href="./styles/forms.css" />
  <link rel="stylesheet" href="./styles/body.css" />
</head>

<body>
  <?php include "./components/header.php"; ?>
  <main>
    <form action="./phpconnects/user_regestration.php" method="POST">
      <h1>Регистрация</h1>
      <input type="text" name="surnameUser" placeholder="Фамилия*" required>
      <input type="text" name="nameUser" placeholder="Имя*" required>
      <input type="text" name="patronymicUser" placeholder="Отчество">
      <input type="text" name="loginUser" placeholder="Логин*" required>
      <input type="password" name="passwordUser" placeholder="Пароль" required>
      <p>Согласие на обработку персональных данных <input type="checkbox" name="soglasieUser" required></p>
      <button type="submit">Зарегистрироваться</button>
      <p>Есть аккаунт?<a href="signIn.php">Войти</a></p>
    </form>
  </main>
</body>

</html>
