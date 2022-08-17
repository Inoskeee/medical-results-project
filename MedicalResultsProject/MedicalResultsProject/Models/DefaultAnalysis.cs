using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MedicalResultsProject.Models
{
    [Table("DefaultAnalyzes")]
    public class DefaultAnalysis
    {
        [Key]
        public int Id { get; set; }                 //Ключевое значение Id анализа

        public int UserId { get; set; }
        public string AnalysisName { get; set; }    //Хранит в себе название анализа
        public List<TypesOfAnalysis> AnalysisTypes { get; set; }    //Хранит в себе список параметров анализа
        public bool isHidden { get; set; }
    }
}