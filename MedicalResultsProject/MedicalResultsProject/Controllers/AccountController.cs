using MedicalResultsProject.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MedicalResultsProject.Controllers
{
    //Контроллер для работы с аккаунтом пользователя
    public class AccountController : Controller
    {
        //Метод отображения страницы авторизации
        public ActionResult Login()
        {
            //Возвращает представление данной страницы Login.cshtml
            return View();
        }
        //ПОСТ запрос на сервер который срабатывает при отправке формы на странице Login.cshtml и обрабатывается процесс авторизации
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model)
        {
            //Если форма модели заполнена верно
            if (ModelState.IsValid)
            {
                //Создаем объект пользователя
                User user = null;
                using (MyDbContext db = new MyDbContext())
                {
                    //хэшируем введенный пароль
                    var md5 = MD5.Create();
                    var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(model.Password));
                    string hashedPassword = Convert.ToBase64String(hash);
                    //Ищем пользователя в БД по введенному паролю и логину (используются LINQ запросы)
                    user = db.Users.FirstOrDefault(u => u.Username == model.Username && u.PasswordHash == hashedPassword);

                }
                //Если все ввели верно и пользователь был найден
                if (user != null)
                {
                    //Записываем данные о пользователе в файлы cookie для дальнейшей автоматической авторизации
                    FormsAuthentication.SetAuthCookie(model.Username, true);
                    //Перенаправляем на страницу Home
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    //Иначе отображаем пользователю ошибку
                    ModelState.AddModelError("", "Пользователя с таким логином не существует или пароль введен неверно.");
                }
            }
            //В ПОСТ запросе отправляем введенную модель
            return View(model);
        }

        //Метод отображения страницы регистрации
        public ActionResult Register()
        {
            //Возвращает представление данной страницы Register.cshtml
            return View();
        }
        //ПОСТ запрос на сервер который срабатывает при отправке формы на странице Register.cshtml и обрабатывается процесс авторизации
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            //Если форма модели заполнена верно
            if (ModelState.IsValid)
            {
                //Проверяем существует ли такой пользователь в БД
                User user = null;
                using (MyDbContext db = new MyDbContext())
                {
                    user = db.Users.FirstOrDefault(u => u.Username == model.Username);
                }
                //Если пользователь не существует
                if (user == null)
                {
                    //Создаем регулярное выражение для проверка ввода email
                    string cond = @"(\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)";
                    string email = model.Email;
                    //Если email введен в нужном формате
                    if (Regex.IsMatch(email, cond))
                    {
                        //Проверяем, чтобы длина пароля была от 6 до 15 символов
                        if (model.Password.Length >= 6 && model.Password.Length <= 15)
                        {
                            // создаем нового пользователя
                            using (MyDbContext db = new MyDbContext())
                            {
                                //Если все верно, хэшируем пароль и добавляем всю информацию к объекту User
                                var md5 = MD5.Create();
                                var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(model.Password));
                                string hashedPassword = Convert.ToBase64String(hash);


                                User ourUser = new User
                                {
                                    Username = model.Username,
                                    PasswordHash = hashedPassword,
                                    Email = model.Email,
                                    Name = model.Name,
                                    Surname = model.Surname,
                                    Age = model.Age,
                                    Height = model.Height,
                                    Weight = model.Weight
                                };
                                //Добавляем пользователя в БД
                                db.Users.Add(ourUser);
                                //Сохраняем изменения
                                db.SaveChanges();
                                //Проверяем, добавился ли пользователь в БД
                                user = db.Users.Where(u => u.Username == model.Username && u.PasswordHash == hashedPassword).FirstOrDefault();
                            }
                            //Если пользователь удачно добавлен в бд
                            if (user != null)
                            {
                                //Авторизируем пользователя и записываем информацию в cookie
                                FormsAuthentication.SetAuthCookie(model.Username, true);
                                //Перенаправляем на страницу Home
                                return RedirectToAction("Index", "Home");
                            }
                        }
                        else
                        {
                            //Если пароль меньше 6 или больше 15 символов, выводим ошибку
                            ModelState.AddModelError("", "Пароль должен быть от 6 до 15 символов.");
                        }
                    }
                    else
                    {
                        //Если email введен некорректно, выводим ошибку
                        ModelState.AddModelError("", "Email введен некорректно, повторите ввод. Пример: temp@gmail.com");
                    }

                }
                else
                {
                    //Если пользователь уже существует, выводим ошибку
                    ModelState.AddModelError("", "Пользователь с таким логином уже существует");
                }
            }
            //В ПОСТ запросе отправляем введенную модель
            return View(model);
        }
        //Метод для выхода из аккаунта пользователя
        public ActionResult Logoff()
        {
            //Удаляем файлы cookie
            FormsAuthentication.SignOut();
            //Перенаправляем на страницу Home
            return RedirectToAction("Index", "Home");
        }

        //Метод редактирование общей информации пользователя (все, кроме пароля)
        [Authorize]
        [HttpGet]
        public ActionResult EditGeneralInfo()
        {
            //Определяем модель редактирования
            ChangeGeneralInfoModel model = new ChangeGeneralInfoModel();
            //Заполняем модель редактирования данными пользователя
            using (MyDbContext db = new MyDbContext())
            {
                User user = db.Users.FirstOrDefault(u => u.Username == User.Identity.Name);
                model.Name = user.Name;
                model.Surname = user.Surname;
                model.Email = user.Email;
                model.Age = user.Age;
                model.Weight = user.Weight;
                model.Height = user.Height;
            }
            //Отправляет модель в представление
            return View(model);
        }

        //Пост запрос для сохранения изменений общей информации
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditGeneralInfo(ChangeGeneralInfoModel model)
        {
            //Проверяем валидность модели
            if (ModelState.IsValid)
            {
                string cond = @"(\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)";
                string email = model.Email;
                //Проверяем, что был введен Email
                if (Regex.IsMatch(email, cond))
                {
                    User user = null;
                    using (MyDbContext db = new MyDbContext())
                    {
                        //Ищем пользователя в БД и меняем его данные на данные из модели
                        user = db.Users.FirstOrDefault(u => u.Username == User.Identity.Name);
                        user.Email = model.Email;
                        user.Name = model.Name;
                        user.Surname = model.Surname;
                        user.Age = model.Age;
                        user.Height = model.Height;
                        user.Weight = model.Weight;
                        //Модифицируем уже существующее значение (заменяем на новое)
                        db.Entry(user).State = EntityState.Modified;
                        db.SaveChanges();
                    }

                    if (user != null)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Еmail введен некорректно, повторите ввод. Пример: temp@gmail.com");
                }
            }
            return View(model);
        }
        //Метод для редактирования пароля
        [Authorize]
        [HttpGet]
        public ActionResult EditPassword()
        {
            return View();
        }
        //Пост запрос для записи нового пароля в БД
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPassword(ChangePasswordModel model)
        {
            //Проверяем валидность модели
            if (ModelState.IsValid)
            {
                User user = null;
                using (MyDbContext db = new MyDbContext())
                {
                    user = db.Users.FirstOrDefault(u => u.Username == User.Identity.Name);
                    var md5 = MD5.Create();
                    var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(model.OldPassword));
                    string hashedPassword = Convert.ToBase64String(hash);
                    //Проверяем, что старый пароль введен верно
                    if (hashedPassword == user.PasswordHash)
                    {
                        //Проверяем, что новый пароль не меньше 6 символов и не больше 15 
                        if (model.NewPassword.Length >= 6 && model.NewPassword.Length <= 15)
                        {
                            //Проверяем что новый пароль и его повторение равны
                            if (model.NewPassword == model.RetryPassword)
                            {
                                //Перезаписываем новый пароль в БД
                                hash = md5.ComputeHash(Encoding.UTF8.GetBytes(model.NewPassword));
                                hashedPassword = Convert.ToBase64String(hash);
                                user.PasswordHash = hashedPassword;
                                db.Entry(user).State = EntityState.Modified;
                                db.SaveChanges();
                                return RedirectToAction("Index", "Home");
                            }
                            else
                            {
                                ModelState.AddModelError("", "Пароли не совпадают, повторите попытку!");
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("", "Пароль должен быть от 6 до 15 символов.");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Старый пароль введен неверно, повторите попытку!");
                    }
                }
            }

            return View(model);
        }
    }
}