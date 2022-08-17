using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MedicalResultsProject.Models
{
    //Модель типа параметра
    [Table("NormalValues")]
    public class NormalValues
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("TypesOfAnalysis")]
        public int TypeId { get; set; } //Хранит в себе Id анализа по умолчанию, по умолчанию 0, если данный тип общедоступен
        public TypesOfAnalysis TypesOfAnalysis { get; set; }

        public int minAge { get; set; }
        public int maxAge { get; set; }

        [Required(ErrorMessage = "Введите нормальное значение типа анализа")]
        //[Range(typeof(decimal), "0.0", "500.0", ErrorMessage = "Диапазон значений должен быть от 0.0 до 500.0")]
        [RegularExpression(@"^[0-9]+([.][0-9]{1,3})?$", ErrorMessage = "Введите числовое значение")]    //Проверка регулярным выражением того, что введено число
        public decimal minValue { get; set; }

        [Required(ErrorMessage = "Введите нормальное значение типа анализа")]
        //[Range(typeof(decimal), "0.0", "500.0", ErrorMessage = "Диапазон значений должен быть от 0.0 до 500.0")]
        [RegularExpression(@"^[0-9]+([.][0-9]{1,3})?$", ErrorMessage = "Введите числовое значение")]    //Проверка регулярным выражением того, что введено число
        public decimal maxValue { get; set; }

    }
}