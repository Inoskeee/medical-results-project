using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MedicalResultsProject.Models
{
    //Класс отвечает за доступ к базе данных, хранит в себе все модели
    //Атрибут означает, что в качестве БД будет использован MySQL
    [DbConfigurationType(typeof(MySql.Data.EntityFramework.MySqlConnectionFactory))]
    public class MyDbContext : DbContext
    {
        //Конструктор класса
        public MyDbContext() : base("MyDbContextConnectionString") { }
        public DbSet<User> Users { get; set; }  //Таблица пользователей
        public DbSet<Analysis> Analyzes { get; set; }   //Таблица анализов
        public DbSet<ValuesOfAnalysis> ValuesOfAnalyzes { get; set; }   //Таблица значений анализа
        public DbSet<TypesOfAnalysis> TypesOfAnalyzes { get; set; }     //Таблица типов значений анализа
        public DbSet<DefaultAnalysis> DefaultAnalyzes { get; set; }     //Таблица значений анализов по умолчанию
        public DbSet<NormalValues> NormalValues { get; set; }     //Таблица типов значений нормальных значений
    }

    //Класс отвечает за инициализацию базы данные, если она еще не существует
    public class MRInitializer : CreateDatabaseIfNotExists<MyDbContext>
    {
        //Метод автоматически добавляет в базу данных типов параметров типы по умолчанию, указанные внутри метода
        protected override void Seed(MyDbContext db)
        {
            //db.TypesOfAnalyzes.Add(new TypesOfAnalysis { UserId = 0, TypeName = "Лейкоциты", NormalValue = (decimal)10.8 });
            //db.TypesOfAnalyzes.Add(new TypesOfAnalysis { UserId = 0, TypeName = "Эритроциты", NormalValue = (decimal)6.7 });
            //db.TypesOfAnalyzes.Add(new TypesOfAnalysis { UserId = 0, TypeName = "Гемоглобин", NormalValue = (decimal)232 });
            //db.TypesOfAnalyzes.Add(new TypesOfAnalysis { UserId = 0, TypeName = "Тромбоциты", NormalValue = (decimal)437 });
            db.DefaultAnalyzes.Add(new DefaultAnalysis { isHidden = true, UserId=0, AnalysisName = "DefaultAnalysis" });
            base.Seed(db);
            db.DefaultAnalyzes.Add(new DefaultAnalysis
            {
                isHidden = false,
                AnalysisName = "Общий анализ крови",
                UserId = 0,
                AnalysisTypes = new List<TypesOfAnalysis>()
                { new TypesOfAnalysis { UserId = 0, TypeName="Лейкоциты", NormalValue= new List<NormalValues>
                    {
                    new NormalValues{minAge=0, maxAge=1, minValue = 6, maxValue = (decimal)17.5},
                    new NormalValues{minAge=1, maxAge=2, minValue = 6, maxValue = 17},
                    new NormalValues{minAge=2, maxAge=4, minValue = (decimal)5.5, maxValue = (decimal)15.5},
                    new NormalValues{minAge=4, maxAge=6, minValue = 5, maxValue = 14},
                    new NormalValues{minAge=6, maxAge=10, minValue = (decimal)4.5, maxValue = 13},
                    new NormalValues{minAge=10, maxAge=16, minValue = (decimal)4.5, maxValue = 12},
                    new NormalValues{minAge=16, maxAge=18, minValue = (decimal)4.5, maxValue = 11},
                    new NormalValues{minAge=18, maxAge=150, minValue = 4, maxValue = 9},
                    }},
                    new TypesOfAnalysis { UserId = 0, TypeName = "Эритроциты", NormalValue = new List<NormalValues>
                    {
                    new NormalValues{minAge=0, maxAge=1, minValue = (decimal)3.8, maxValue = (decimal)4.9},
                    new NormalValues{minAge=1, maxAge=2, minValue = (decimal)3.8, maxValue = (decimal)4.9},
                    new NormalValues{minAge=2, maxAge=4, minValue = (decimal)3.8, maxValue = (decimal)4.9},
                    new NormalValues{minAge=4, maxAge=6, minValue = (decimal)3.8, maxValue = (decimal)4.9},
                    new NormalValues{minAge=6, maxAge=10, minValue = (decimal)3.8, maxValue = (decimal)4.9},
                    new NormalValues{minAge=10, maxAge=16, minValue = (decimal)3.8, maxValue = (decimal)4.9},
                    new NormalValues{minAge=16, maxAge=18, minValue = (decimal)3.5, maxValue = (decimal)4.7},
                    new NormalValues{minAge=18, maxAge=150, minValue = (decimal)3.5, maxValue = (decimal)4.7},
                    }},
                    new TypesOfAnalysis { UserId = 0, TypeName = "Тромбоциты", NormalValue = new List<NormalValues>
                    {
                    new NormalValues{minAge=0, maxAge=1, minValue = (decimal)160, maxValue = (decimal)390},
                    new NormalValues{minAge=1, maxAge=2, minValue = (decimal)150, maxValue = (decimal)400},
                    new NormalValues{minAge=2, maxAge=4, minValue = (decimal)150, maxValue = (decimal)400},
                    new NormalValues{minAge=4, maxAge=6, minValue = (decimal)150, maxValue = (decimal)400},
                    new NormalValues{minAge=6, maxAge=10, minValue = (decimal)180, maxValue = (decimal)450},
                    new NormalValues{minAge=10, maxAge=16, minValue = (decimal)150, maxValue = (decimal)450},
                    new NormalValues{minAge=16, maxAge=18, minValue = (decimal)150, maxValue = (decimal)450},
                    new NormalValues{minAge=18, maxAge=150, minValue = (decimal)150, maxValue = (decimal)450},
                    }},
                    new TypesOfAnalysis { UserId = 0, TypeName = "Гемоглобин", NormalValue = new List<NormalValues>
                    {
                    new NormalValues{minAge=0, maxAge=1, minValue = (decimal)105, maxValue = (decimal)140},
                    new NormalValues{minAge=1, maxAge=2, minValue = (decimal)100, maxValue = (decimal)140},
                    new NormalValues{minAge=2, maxAge=4, minValue = (decimal)100, maxValue = (decimal)140},
                    new NormalValues{minAge=4, maxAge=6, minValue = (decimal)100, maxValue = (decimal)140},
                    new NormalValues{minAge=6, maxAge=10, minValue = (decimal)115, maxValue = (decimal)145},
                    new NormalValues{minAge=10, maxAge=16, minValue = (decimal)112, maxValue = (decimal)152},
                    new NormalValues{minAge=16, maxAge=18, minValue = (decimal)115, maxValue = (decimal)153},
                    new NormalValues{minAge=18, maxAge=150, minValue = (decimal)120, maxValue = (decimal)155},
                    }}
                }
            });
            base.Seed(db);
            db.DefaultAnalyzes.Add(new DefaultAnalysis
            {
                isHidden = false,
                AnalysisName = "Биохимический анализ крови",
                UserId = 0,
                AnalysisTypes = new List<TypesOfAnalysis>()
                { new TypesOfAnalysis { UserId = 0,  TypeName="Общий белок", NormalValue= new List<NormalValues>
                    {
                    new NormalValues{minAge=0, maxAge=1, minValue = 56, maxValue = 75},
                    new NormalValues{minAge=1, maxAge=2, minValue = 56, maxValue = 75},
                    new NormalValues{minAge=2, maxAge=4, minValue = 56, maxValue = 75},
                    new NormalValues{minAge=4, maxAge=6, minValue = 60, maxValue = 80},
                    new NormalValues{minAge=6, maxAge=10, minValue = 60, maxValue = 80},
                    new NormalValues{minAge=10, maxAge=16, minValue = 60, maxValue = 80},
                    new NormalValues{minAge=16, maxAge=18, minValue = 64, maxValue = 83},
                    new NormalValues{minAge=18, maxAge=150, minValue = 82, maxValue = 81},
                    }},
                    new TypesOfAnalysis { UserId = 0, TypeName = "Креатинин крови", NormalValue =  new List<NormalValues>
                    {
                    new NormalValues{minAge=0, maxAge=1, minValue = 27, maxValue = 62},
                    new NormalValues{minAge=1, maxAge=2, minValue = 27, maxValue = 62},
                    new NormalValues{minAge=2, maxAge=4, minValue = 27, maxValue = 62},
                    new NormalValues{minAge=4, maxAge=6, minValue = 27, maxValue = 62},
                    new NormalValues{minAge=6, maxAge=10, minValue = 27, maxValue = 62},
                    new NormalValues{minAge=10, maxAge=16, minValue = 44, maxValue = 88},
                    new NormalValues{minAge=16, maxAge=18, minValue = 44, maxValue = 88},
                    new NormalValues{minAge=18, maxAge=150, minValue = 44, maxValue = 88},
                    }},
                    new TypesOfAnalysis { UserId = 0, TypeName = "Холестерин", NormalValue = new List<NormalValues>
                    {
                    new NormalValues{minAge=0, maxAge=1, minValue = (decimal)2.95, maxValue = (decimal)5.23},
                    new NormalValues{minAge=1, maxAge=2, minValue = (decimal)2.95, maxValue = (decimal)5.23},
                    new NormalValues{minAge=2, maxAge=4, minValue = (decimal)2.95, maxValue = (decimal)5.23},
                    new NormalValues{minAge=4, maxAge=6, minValue = (decimal)2.95, maxValue = (decimal)5.23},
                    new NormalValues{minAge=6, maxAge=10, minValue = (decimal)2.95, maxValue = (decimal)5.23},
                    new NormalValues{minAge=10, maxAge=16, minValue = (decimal)2.95, maxValue = (decimal)5.23},
                    new NormalValues{minAge=16, maxAge=18, minValue = (decimal)2.93, maxValue = (decimal)5.1},
                    new NormalValues{minAge=18, maxAge=150, minValue = (decimal)3.44, maxValue = (decimal)6.32},
                    }},
                    new TypesOfAnalysis { UserId = 0, TypeName = "Билирубин", NormalValue = new List<NormalValues>
                    {
                    new NormalValues{minAge=0, maxAge=1, minValue = (decimal)3.3, maxValue = (decimal)17},
                    new NormalValues{minAge=1, maxAge=2, minValue = (decimal)3.3, maxValue = (decimal)17},
                    new NormalValues{minAge=2, maxAge=4, minValue = (decimal)3.3, maxValue = (decimal)17},
                    new NormalValues{minAge=4, maxAge=6, minValue = (decimal)3.3, maxValue = (decimal)17},
                    new NormalValues{minAge=6, maxAge=10, minValue = (decimal)3.3, maxValue = (decimal)17},
                    new NormalValues{minAge=10, maxAge=16, minValue = (decimal)3.3, maxValue = (decimal)17},
                    new NormalValues{minAge=16, maxAge=18, minValue = (decimal)3.3, maxValue = (decimal)17},
                    new NormalValues{minAge=18, maxAge=150, minValue = (decimal)3.3, maxValue = (decimal)17},
                    }},
                    new TypesOfAnalysis { UserId = 0, TypeName = "Глюкоза", NormalValue = new List<NormalValues>
                    {
                    new NormalValues{minAge=0, maxAge=1, minValue = (decimal)3.3, maxValue = (decimal)5.5},
                    new NormalValues{minAge=1, maxAge=2, minValue = (decimal)3.3, maxValue = (decimal)5.5},
                    new NormalValues{minAge=2, maxAge=4, minValue = (decimal)3.3, maxValue = (decimal)5.5},
                    new NormalValues{minAge=4, maxAge=6, minValue = (decimal)3.3, maxValue = (decimal)5.5},
                    new NormalValues{minAge=6, maxAge=10, minValue = (decimal)3.3, maxValue = (decimal)5.5},
                    new NormalValues{minAge=10, maxAge=16, minValue = (decimal)3.3, maxValue = (decimal)5.5},
                    new NormalValues{minAge=16, maxAge=18, minValue = (decimal)3.3, maxValue = (decimal)5.5},
                    new NormalValues{minAge=18, maxAge=150, minValue = (decimal)3.3, maxValue = (decimal)5.5},
                    }}
                }
            });
            base.Seed(db);
        }
    }
}