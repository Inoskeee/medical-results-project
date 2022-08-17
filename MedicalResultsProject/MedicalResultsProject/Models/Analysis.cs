using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MedicalResultsProject.Models
{
    //Модель анализа, атрибут [Table] означает таблицу, в которой будет храниться модель в БД
    [Table("Analyzes")]
    public class Analysis
    {
        [Key]
        public int Id { get; set; }                 //Ключевое значение Id анализа
        public int UserId { get; set; }             //Хранит в себе Id пользователя, кому принадлежит анализ

        [Required(ErrorMessage = "Введите название анализа")]   //Атрибут означает, что значение обязательное для ввода
        public string AnalysisName { get; set; }    //Хранит в себе название анализа

        [Required(ErrorMessage ="Выберите дату анализа")]
        [DataType(DataType.Date, ErrorMessage = "Дата должна быть в формате 01.01.2000")]   //Атрибут определяет тип значения, которое будет вводиться
        public string Date { get; set; }            //Хранит в себе дату анализа
        public string Diagnosis { get; set; }       //Хранит в себе диагноз
        [Required]
        public List<ValuesOfAnalysis> ValuesOfAnalyzes { get; set; }    //Хранит в себе список параметров анализа
    }
}