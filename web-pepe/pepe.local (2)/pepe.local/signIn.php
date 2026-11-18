<!doctype html>
<html lang="en">

<head>
  <meta charset="UTF-8" />
  <meta name="viewport" content="width=device-width, initial-scale=1.0" />
  <title>SignIn</title>
  <link rel="stylesheet" href="./styles/main.css" />
  <link rel="stylesheet" href="./styles/header.css" />
  <link rel="stylesheet" href="./styles/body.css" />
  <link rel="stylesheet" href="./styles/forms.css" />
</head>

<body>
  <?php include "./components/header.php"; ?>
  <main>
    <form action="phpconnects/user_auth.php" method="POST">
      <h1>Авторизация</h1>
      <input type="text" name="loginUser" placeholder="Логин*" required>
      <input type="password" name="passwordUser" placeholder="Пароль" required>
      <button>Войти</button>
      <p>Нет аккаунта?<a href="signUp.php">Зарегистрироваться</a></p>
    </form>
  </main>
</body>

</html>