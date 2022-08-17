using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MedicalResultsProject.Models
{
    //Вспомогательная модель для авторизации пользователя
    public class LoginModel
    {
        [Required(ErrorMessage = "Введите логин")]
        public string Username { get; set; }        //Хранит в себе логин пользователя
        [Required(ErrorMessage = "Введите пароль")]
        public string Password { get; set; }        //Хранит в себе пароль пользователя
    }
    //Вспомогательная модель для регистрации пользователя
    public class RegisterModel
    {
        [Required(ErrorMessage = "Введите логин")]
        public string Username { get; set; }        //Хранит в себе логин пользователя

        [Required(ErrorMessage = "Введите пароль")]
        public string Password { get; set; }        //Хранит в себе пароль пользователя

        [Required(ErrorMessage = "Введите email")]
        public string Email { get; set; }           //Хранит в себе Email пользователя

        [Required(ErrorMessage = "Введите имя")]
        public string Name { get; set; }            //Хранит в себе имя пользователя

        [Required(ErrorMessage = "Введите фамилию")]
        public string Surname { get; set; }         //Хранит в себе фамилию пользователя

        [Required(ErrorMessage = "Введите возраст")]
        [Range(typeof(int), "0", "100", ErrorMessage = "Диапазон значений должен быть от 0 до 100")]    //Атрибут определяет диапазон возможных значений
        public int Age { get; set; }                //Хранит в себе возраст пользователя

        [Required(ErrorMessage = "Введите рост")]
        [Range(typeof(int), "0", "200", ErrorMessage = "Диапазон значений должен быть от 0 до 200")]
        public int Height { get; set; }             //Хранит в себе рост пользователя

        [Required(ErrorMessage = "Введите вес")]
        [Range(typeof(int), "0", "200", ErrorMessage = "Диапазон значений должен быть от 0 до 200")]
        public int Weight { get; set; }             //Хранит в себе вес пользователя
    }


    public class ChangeGeneralInfoModel
    {
        [Required(ErrorMessage = "Введите email")]
        public string Email { get; set; }           //Хранит в себе Email пользователя

        [Required(ErrorMessage = "Введите имя")]
        public string Name { get; set; }            //Хранит в себе имя пользователя

        [Required(ErrorMessage = "Введите фамилию")]
        public string Surname { get; set; }         //Хранит в себе фамилию пользователя

        [Required(ErrorMessage = "Введите возраст")]
        [Range(typeof(int), "0", "100", ErrorMessage = "Диапазон значений должен быть от 0 до 100")]    //Атрибут определяет диапазон возможных значений
        public int Age { get; set; }                //Хранит в себе возраст пользователя

        [Required(ErrorMessage = "Введите рост")]
        [Range(typeof(int), "0", "200", ErrorMessage = "Диапазон значений должен быть от 0 до 200")]
        public int Height { get; set; }             //Хранит в себе рост пользователя

        [Required(ErrorMessage = "Введите вес")]
        [Range(typeof(int), "0", "200", ErrorMessage = "Диапазон значений должен быть от 0 до 200")]
        public int Weight { get; set; }             //Хранит в себе вес пользователя
    }

    //Модель для редактирования пароля пользователя
    public class ChangePasswordModel
    {
        public string OldPassword { get; set; } //Хранит в себе старый пароль

        public string NewPassword { get; set; } //Хранит в себе новый пароль
        public string RetryPassword { get; set; } //Хранит в себе повторение нового пароля
    }

    //Модель статистики
    public class Statistics
    {
        [Required(ErrorMessage = "Выберите дату")]
        [DataType(DataType.Date, ErrorMessage = "Дата должна быть в формате 01.01.2000")]
        public DateTime firstDate { get; set; } //Хранит в себе начальную дату
        [Required(ErrorMessage = "Выберите дату")]
        [DataType(DataType.Date, ErrorMessage = "Дата должна быть в формате 01.01.2000")]
        public DateTime secondDate { get; set; }    //Хранит в себе конечную дату

        public string selectedAnalysis { get; set; }    //Хранит в себе id выбранного анализа
    }

    //Модель статистики  для отрисовки графиков
    public class SelectionStats
    {
        public List<string> Dates { get; set; } //Хранит в себе список дат
        public List<string> Parameters { get; set; }    //Хранит в себе список названий параметров
        public List<List<decimal>> NormalValue { get; set; }    //Хранит в себе список списков нормальных значений
        public List<List<decimal>> Results { get; set; }    //Хранит в себе список списков значений для каждого параметра
    }
}