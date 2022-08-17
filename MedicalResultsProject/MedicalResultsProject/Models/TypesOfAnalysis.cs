using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MedicalResultsProject.Models
{
    //Модель типа параметра
    [Table("TypesOfAnalyzes")]
    public class TypesOfAnalysis
    {
        [Key]
        public int Id { get; set; }                     //Ключевое значение Id параметра
        public int UserId { get; set; }                 //Хранит в себе Id пользователя владельца, по умолчанию 0, если данный тип общедоступен

        [ForeignKey("DefaultAnalysis")]
        public int AnalysisId { get; set; } //Хранит в себе Id анализа по умолчанию, по умолчанию 0, если данный тип общедоступен
        public DefaultAnalysis DefaultAnalysis { get; set; }

        [Required(ErrorMessage = "Введите название типа анализа")]
        public string TypeName { get; set; }            //Хранит в себе название типа
        public List<NormalValues> NormalValue { get; set; }     //Хранит в себе нормальное значение типа
    }
}