using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MedicalResultsProject.Models
{
    //Модель пользователя
    public class User
    {
        [Key]
        public int Id { get; set; }                 //Ключевое значение Id пользователя
        public string Username { get; set; }        //Логин пользователя
        public string PasswordHash { get; set; }    //Хэшированный пароль пользователя
        public string Email { get; set; }           //Email пользователя
        public string Name { get; set; }            //Имя пользователя
        public string Surname { get; set; }         //Фамилия пользователя
        public int Age { get; set; }                //Возраст пользователя
        public int Height { get; set; }             //Рост пользователя
        public int Weight { get; set; }             //Вес пользователя
        public List<Analysis> Analyzes { get; set; }    //Список анализов пользователя
    }
}



