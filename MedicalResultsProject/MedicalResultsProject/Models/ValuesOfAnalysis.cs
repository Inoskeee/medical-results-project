using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MedicalResultsProject.Models
{
    //Модель параметров анализа
    [Table("ValuesOfAnalyzes")]
    public class ValuesOfAnalysis
    {
        [Key]
        public int Id { get; set; }                 //Ключевое значение Id параметра

        [ForeignKey("Analysis")]
        public int AnalysisId { get; set; }         //Ключевое значение Id анализа, которому принадлежит параметр
        public Analysis Analysis { get; set; }

        [ForeignKey("TypesOfAnalysis")]
        public int TypeId { get; set; }             //Ключевое значение Id типа параметра, которому принадлежит параметр
        public TypesOfAnalysis TypesOfAnalysis{ get; set; }

        [Required(ErrorMessage ="Введите значение параметра")]
        [Range(typeof(decimal), "0", "500", ErrorMessage ="Значение должно быть от 0 до 500")]
        [RegularExpression(@"^[0-9]+([.][0-9]{1,3})?$", ErrorMessage = "Введите числовое значение")]    //Проверка регулярным выражением, что вводится число
        public decimal Value { get; set; }          //Хранит в себе значение параметра
    }
}