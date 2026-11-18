using Microsoft.AspNetCore.Mvc;
using PepeApi.Data;
using PepeApi.Models;

namespace PepeApi.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class UsersController : ControllerBase
{
    private readonly PepeDb db;

    public UsersController(PepeDb db)
    {
        this.db = db;
    }

    [HttpPost]
    public IActionResult PostAuthorizationUser(string? loginUser, string? passwordUser)
    {
        loginUser = (loginUser ?? "").Trim();
        passwordUser = passwordUser ?? "";

        if (loginUser == "" || passwordUser == "")
        {
            return BadRequest(new { message = "Введите логин и пароль" });
        }

        try
        {
            var user = (
                from u in db.Users
                where u.Login == loginUser && u.Password == passwordUser && u.Status == "Активен"
                select u
            ).FirstOrDefault();

            if (user == null)
            {
                return BadRequest(new { message = "Логин или пароль не верные" });
            }

            return Ok(user);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "Ошибка базы данных: " + ex.Message });
        }
    }

    [HttpPost]
    public IActionResult PostRegistrationUser(
        string? surnameUser,
        string? nameUser,
        string? patronymicUser,
        string? loginUser,
        string? passwordUser)
    {
        surnameUser = (surnameUser ?? "").Trim();
        nameUser = (nameUser ?? "").Trim();
        patronymicUser = (patronymicUser ?? "").Trim();
        loginUser = (loginUser ?? "").Trim();
        passwordUser = passwordUser ?? "";

        if (surnameUser == "" || nameUser == "" || loginUser == "" || passwordUser == "")
        {
            return BadRequest(new { message = "Заполните фамилию, имя, логин и пароль" });
        }

        if (loginUser.Length > 20 || passwordUser.Length > 20)
        {
            return BadRequest(new { message = "Логин и пароль не длиннее 20 символов" });
        }

        try
        {
            var old = (
                from u in db.Users
                where u.Login == loginUser
                select u
            ).FirstOrDefault();

            if (old != null && old.Status == "Активен")
            {
                return BadRequest(new { message = "Пользователь существует" });
            }

            if (old != null)
            {
                old.Surname = surnameUser;
                old.Name = nameUser;
                old.Patronymic = patronymicUser;
                old.Status = "Активен";
                old.Role = "Клиент";
                old.Password = passwordUser;
            }
            else
            {
                db.Users.Add(new User
                {
                    Surname = surnameUser,
                    Name = nameUser,
                    Patronymic = patronymicUser,
                    Status = "Активен",
                    Role = "Клиент",
                    Login = loginUser,
                    Password = passwordUser
                });
            }

            db.SaveChanges();
            return Ok(new { message = "Регистрация завершена успешно!" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "Ошибка базы данных: " + ex.Message });
        }
    }

    [HttpPut]
    public IActionResult PutUpdateUser(
        int userId,
        string? surnameUser,
        string? nameUser,
        string? patronymicUser,
        string? loginUser,
        string? passwordUser)
    {
        surnameUser = (surnameUser ?? "").Trim();
        nameUser = (nameUser ?? "").Trim();
        patronymicUser = (patronymicUser ?? "").Trim();
        loginUser = (loginUser ?? "").Trim();
        passwordUser = passwordUser ?? "";

        if (userId <= 0)
        {
            return BadRequest(new { message = "Вы не авторизованы" });
        }

        if (surnameUser == "" || nameUser == "" || loginUser == "" || passwordUser == "")
        {
            return BadRequest(new { message = "Данные для обновления не получены" });
        }

        if (loginUser.Length > 20 || passwordUser.Length > 20)
        {
            return BadRequest(new { message = "Логин и пароль не длиннее 20 символов" });
        }

        try
        {
            bool taken = (
                from u in db.Users
                where u.Login == loginUser && u.Id != userId && u.Status == "Активен"
                select u
            ).Any();

            if (taken)
            {
                return BadRequest(new { message = "Логин " + loginUser + " уже занят другим пользователем!" });
            }

            var user = (
                from u in db.Users
                where u.Id == userId && u.Status == "Активен"
                select u
            ).FirstOrDefault();

            if (user == null)
            {
                return BadRequest(new { message = "Пользователь не найден" });
            }

            user.Surname = surnameUser;
            user.Name = nameUser;
            user.Patronymic = patronymicUser;
            user.Login = loginUser;
            user.Password = passwordUser;
            db.SaveChanges();

            return Ok(user);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "Ошибка базы данных: " + ex.Message });
        }
    }

    [HttpPost]
    public IActionResult PostDeleteUser(int userId)
    {
        if (userId <= 0)
        {
            return BadRequest(new { message = "Вы не авторизованы" });
        }

        try
        {
            var user = (
                from u in db.Users
                where u.Id == userId
                select u
            ).FirstOrDefault();

            if (user == null)
            {
                return BadRequest(new { message = "Пользователь не найден" });
            }

            user.Status = "Удалён";
            db.SaveChanges();
            return Ok(new { message = "Ваш аккаунт был деактивирован." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "Ошибка базы данных: " + ex.Message });
        }
    }
}
