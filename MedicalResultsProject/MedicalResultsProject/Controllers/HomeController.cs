using MedicalResultsProject.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MedicalResultsProject.Controllers
{
    //Контроллер для работы с личным кабинетом пользователя
    public class HomeController : Controller
    {
        //Объявляем базу данных
        MyDbContext db = new MyDbContext();
        
        //Метод, отвечающий за загрузку главной страницы личного кабинета
        //Атрибут [Authorize] означает, что данный метод доступен только авторизованным пользователям
        [Authorize]
        public ActionResult Index()
        {
            //Ищем пользователя в БД
            User user = db.Users.Where(u => u.Username == User.Identity.Name).FirstOrDefault();
            //Записываем его во ViewBag для доступа к этим данным из файла Index.cshtml
            ViewBag.User = user;
            //Ищем анализы пользователя в БД
            user.Analyzes = db.Analyzes.AsEnumerable().TakeWhile(u => u.UserId == user.Id).ToList();
            //Перебираем списки значений и типов анализов в БД, значения и типы подключатся к данным анализа автоматически
            foreach (ValuesOfAnalysis value in db.ValuesOfAnalyzes) { }
            foreach(TypesOfAnalysis value in db.TypesOfAnalyzes) { }
            //Возвращаем представление Index.cshtml
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(DefaultAnalysis model)
        {
            model.AnalysisName = "";
            model.AnalysisTypes = new List<TypesOfAnalysis>();
            if (ModelState.IsValid)
            {
                //return RedirectToAction("../Home/AddDefaultAnalysis?id="+model.Id.ToString());
                return RedirectToAction("AddDefaultAnalysis", new {id = model.Id});
            }
            return View(model);
        }

        //Метод добавления нового типа параметра
        [Authorize]
        public ActionResult AddType()
        {
            //Возвращаем представление AddType.cshtml
            ViewBag.NormalValues = new List<string>()
            {
                "Дети до 1 года",
                "1-2 года",
                "2-4 года",
                "4-6 лет",
                "6-10 лет",
                "10-16 лет",
                "Дети старше 16 лет",
                "Взрослые"
            };
            List<NormalValues> normalValues = new List<NormalValues>()
            {
                new NormalValues{minAge=0, maxAge=1, minValue = 0, maxValue = 0},
                new NormalValues{minAge=1, maxAge=2, minValue = 0, maxValue = 0},
                new NormalValues{minAge=2, maxAge=4, minValue = 0, maxValue = 0},
                new NormalValues{minAge=4, maxAge=6, minValue = 0, maxValue = 0},
                new NormalValues{minAge=6, maxAge=10, minValue = 0, maxValue = 0},
                new NormalValues{minAge=10, maxAge=16, minValue = 0, maxValue = 0},
                new NormalValues{minAge=16, maxAge=18, minValue = 0, maxValue = 0},
                new NormalValues{minAge=18, maxAge=150, minValue = 0, maxValue = 0},
            };
            TypesOfAnalysis model = new TypesOfAnalysis
            {
                NormalValue = normalValues
            };
            return View(model);
        }

        //Запрос на добавление нового типа параметра
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddType(TypesOfAnalysis model)
        {
            //Если модель введена успешно
            if (ModelState.IsValid)
            {
                //Ищем текущего пользователя в БД
                User user = db.Users.Where(u => u.Username == User.Identity.Name).FirstOrDefault();
                //Ищем такой тип анализа в БД
                TypesOfAnalysis analysisType = db.TypesOfAnalyzes.FirstOrDefault(u => u.TypeName == model.TypeName && u.UserId == user.Id);
                List<NormalValues> normalValues = new List<NormalValues>()
                {
                    new NormalValues{minAge=0, maxAge=1, minValue = model.NormalValue[0].minValue, maxValue = model.NormalValue[0].maxValue},
                    new NormalValues{minAge=1, maxAge=2, minValue = model.NormalValue[1].minValue, maxValue = model.NormalValue[1].maxValue},
                    new NormalValues{minAge=2, maxAge=4, minValue = model.NormalValue[2].minValue, maxValue = model.NormalValue[2].maxValue},
                    new NormalValues{minAge=4, maxAge=6, minValue = model.NormalValue[3].minValue, maxValue = model.NormalValue[3].maxValue},
                    new NormalValues{minAge=6, maxAge=10, minValue = model.NormalValue[4].minValue, maxValue = model.NormalValue[4].maxValue},
                    new NormalValues{minAge=10, maxAge=16, minValue = model.NormalValue[5].minValue, maxValue = model.NormalValue[5].maxValue},
                    new NormalValues{minAge=16, maxAge=18, minValue = model.NormalValue[6].minValue, maxValue = model.NormalValue[6].maxValue},
                    new NormalValues{minAge=18, maxAge=150, minValue = model.NormalValue[7].minValue, maxValue = model.NormalValue[7].maxValue},
                };
                //Если такой тип анализа не существует
                if (analysisType == null)
                {
                    int analysisId = db.DefaultAnalyzes.Where(x => x.isHidden == true).FirstOrDefault().Id;
                    //Добавляем данные модели к объекту типа анализа
                    analysisType = new TypesOfAnalysis
                    {
                        TypeName = model.TypeName,
                        UserId = user.Id,
                        AnalysisId = analysisId,
                        NormalValue = normalValues
                    };
                    //Записываем данные о типе в БД и сохраняем изменения
                    
                    db.TypesOfAnalyzes.Add(analysisType);
                    db.SaveChanges();
                    //Перенаправляет пользователя на страницу добавления анализа
                    return RedirectToAction("../Home/AddAnalysis/status=2");
                }
            }
            //В ПОСТ запросе отправляем введенную модель
            return View(model);
        }

        //Метод добавления нового анализа. Включает в себя дополнительные параметры, которые передаются вместе с ссылкой.
        //Эти параметры отвечают за заполнение некоторых полей автоматически или по умолчанию.
        [Authorize]
        public ActionResult AddAnalysis(int status = 0, string analysisName = "", string date = "", string diagnosis = "")
        {
            //Передаем все параметры во ViewBag
            ViewBag.Status = status;
            ViewBag.AnalysisName = analysisName;
            ViewBag.Date = date;
            ViewBag.Diagnosis = diagnosis;
            //Возвращаем представление AddAnalysis.cshtml
            return View();
        }

        //Запрос на добавление нового анализа
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddAnalysis(Analysis model)
        {
            //Если все данные введены верно
            if (ModelState.IsValid)
            {
                //Ищем текущего пользователя в БД
                User user = db.Users.Where(u => u.Username == User.Identity.Name).FirstOrDefault();
                Analysis analysis = null;
                //На будущее тут можно добавить проверку на существование такого анализа
                if (analysis == null)
                {
                    //Добавляем данные из модели к данным объекта анализа
                    analysis = new Analysis
                    {
                        UserId = user.Id,
                        AnalysisName = model.AnalysisName,
                        Date = model.Date,
                        ValuesOfAnalyzes = model.ValuesOfAnalyzes,
                        Diagnosis = model.Diagnosis,
                    };
                    List<TypesOfAnalysis> typesOfAnalyses = new List<TypesOfAnalysis>();
                    foreach(var value in model.ValuesOfAnalyzes)
                    {
                        TypesOfAnalysis analysisType = db.TypesOfAnalyzes.Where(u => u.Id == value.TypeId).FirstOrDefault();
                        DefaultAnalysis defAnalysis = db.DefaultAnalyzes.Where(u => u.isHidden == true).FirstOrDefault();
                        if (analysisType.AnalysisId == defAnalysis.Id)
                        {
                            typesOfAnalyses.Add(analysisType);
                        }
                        else
                        {
                            List<NormalValues> normalValues = new List<NormalValues>();
                            foreach(NormalValues values in db.NormalValues.Where(u => u.TypeId == analysisType.Id).ToList())
                            {
                                normalValues.Add(new NormalValues
                                {
                                    maxAge = values.maxAge,
                                    maxValue = values.maxValue,
                                    minAge = values.minAge,
                                });
                            }
                            TypesOfAnalysis newType = new TypesOfAnalysis
                            {
                                UserId = user.Id,
                                TypeName = analysisType.TypeName,
                                NormalValue = normalValues
                            };
                            typesOfAnalyses.Add(newType);
                        }
                        
                    }
                    DefaultAnalysis defaultAnalysis = new DefaultAnalysis
                    {
                        isHidden = false,
                        UserId = user.Id,
                        AnalysisName = model.AnalysisName,
                        AnalysisTypes = typesOfAnalyses
                    };
                    //Записываем новый анализ в БД и сохраняем изменения
                    db.Analyzes.Add(analysis);
                    db.DefaultAnalyzes.Add(defaultAnalysis);
                    db.SaveChanges();
                    //Перенаправляем пользователя на главную страницу
                    return RedirectToAction("../Home");
                }
            }
            //В ПОСТ запросе отправляем введенную модель
            return View(model);
        }

        //Запрос на удаление анализа, на вход получаем id анализа
        [Authorize]
        [HttpGet]
        public ActionResult DeleteAnalysis(int id)
        {
            //Ищем анализ в БД
            Analysis analysis = db.Analyzes.Where(u => u.Id == id).FirstOrDefault();
            //Ищем значения анализа в БД
            foreach (ValuesOfAnalysis value in db.ValuesOfAnalyzes) 
            {
                if (value.AnalysisId == analysis.Id)
                {
                    //Удаляем значения этого анализа из БД
                    db.Entry(value).State = EntityState.Deleted;
                }
            }
            //Удаляем анализ из БД и сохраняем изменения
            db.Entry(analysis).State = EntityState.Deleted;
            db.SaveChanges();
            //Перенаправляем пользователя на главную страницу
            return Redirect("../Home");
        }

        //Метод получения подробной информации об анализе, на вход получаем id анализа
        [Authorize]
        public ActionResult InformationAnalysis(int id)
        {
            //Ищем пользователя в БД
            User user = db.Users.Where(u => u.Username == User.Identity.Name).FirstOrDefault();
            //Ищем анализ пользователя в БД
            Analysis analysis = db.Analyzes.Where(u => u.Id == id).FirstOrDefault();
            //Перебираем значения и типы анализов, данные добавятся к анализу автоматически
            foreach (ValuesOfAnalysis value in db.ValuesOfAnalyzes) { }
            foreach (TypesOfAnalysis value in db.TypesOfAnalyzes) { }
            List<NormalValues> normalValues = new List<NormalValues>();

            foreach(var value in analysis.ValuesOfAnalyzes)
            {
                normalValues.Add(db.NormalValues.Where(u=> u.TypeId == value.TypeId && (user.Age >= u.minAge && user.Age < u.maxAge)).FirstOrDefault());
            }

            //Добавляем анализ и пользователя во ViewBag
            ViewBag.Analysis = analysis;
            ViewBag.NormalValues = normalValues;
            ViewBag.User = user;
            //Возвращаем представление InformationAnalysis.cshtml
            return View();
        }

        [Authorize]
        public ActionResult UserProfile()
        {
            User user = db.Users.Where(u => u.Username == User.Identity.Name).FirstOrDefault();
            ViewBag.User = user;
            return View();
        }

        //Метод добавления нового уже существующего анализа. В параметры передается id анализа
        [Authorize]
        public ActionResult AddDefaultAnalysis(int id = 1)
        {
            //Ищем этот анализ в таблице уже существующих анализов
            DefaultAnalysis defaultAnalysis = db.DefaultAnalyzes.Where(u => u.Id == id).FirstOrDefault();
            //Ищем типы анализов, которые принадлежать данному анализу
            List<TypesOfAnalysis> typesOfAnalysis = db.TypesOfAnalyzes.Where(u => u.AnalysisId == id).ToList();
            //Добавляем имя анализа и типы анализа в ViewBag
            ViewBag.AnalysisName = defaultAnalysis.AnalysisName;
            ViewBag.AnalysisTypes = typesOfAnalysis;
            //Создаем список значений для каждого типа анализа
            List<ValuesOfAnalysis> valuesOfAnalyses = new List<ValuesOfAnalysis>();
            foreach (var item in typesOfAnalysis)
            {
                valuesOfAnalyses.Add(new ValuesOfAnalysis { TypeId = item.Id });
            };
            //Создаем модель анализа для передачи данных на страницу
            Analysis model = new Analysis()
            {
                AnalysisName = defaultAnalysis.AnalysisName,
                ValuesOfAnalyzes = valuesOfAnalyses
            };
            return View(model);
        }

        //Запрос на добавление нового анализа
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddDefaultAnalysis(Analysis model)
        {
            //Если все данные введены верно
            if (ModelState.IsValid)
            {
                //Ищем текущего пользователя в БД
                User user = db.Users.Where(u => u.Username == User.Identity.Name).FirstOrDefault();
                Analysis analysis = null;
                //На будущее тут можно добавить проверку на существование такого анализа
                if (analysis == null)
                {
                    //Добавляем данные из модели к данным объекта анализа
                    analysis = new Analysis
                    {
                        UserId = user.Id,
                        AnalysisName = model.AnalysisName,
                        Date = model.Date,
                        ValuesOfAnalyzes = model.ValuesOfAnalyzes,
                        Diagnosis = model.Diagnosis,
                    };
                    //Записываем новый анализ в БД и сохраняем изменения
                    db.Analyzes.Add(analysis);
                    db.SaveChanges();
                    //Перенаправляем пользователя на главную страницу
                    return RedirectToAction("../Home");
                }
            }
            DefaultAnalysis defaultAnalysis = db.DefaultAnalyzes.Where(u => u.AnalysisName == model.AnalysisName).FirstOrDefault();
            List<TypesOfAnalysis> typesOfAnalysis = db.TypesOfAnalyzes.Where(u => u.AnalysisId == defaultAnalysis.Id).ToList();
            ViewBag.AnalysisName = defaultAnalysis.AnalysisName;
            ViewBag.AnalysisTypes = typesOfAnalysis;
            //Если ошибка, в ПОСТ запросе отправляем введенную модель и отображаем ошибки
            return View(model);
        }

        //Метод для просмотра статистики. Принимает на вход 3 аргумента: Дату начала, дату конца и id выбранного анализа
        [Authorize]
        public ActionResult CheckStatistics(string firstDate = "", string secondDate = "", int selectedAnalysis = 1)
        {
            //Записываем все парамеры во ViewBag
            ViewBag.FirstDate = firstDate;
            ViewBag.SecondDate = secondDate;
            ViewBag.SelectedAnalysis = selectedAnalysis;
            //Подгружаем значения и типы анализов из БД
            foreach (ValuesOfAnalysis value in db.ValuesOfAnalyzes) { }
            foreach (TypesOfAnalysis value in db.TypesOfAnalyzes) { }
            //Ищем пользователя в БД
            User user = db.Users.Where(u => u.Username == User.Identity.Name).FirstOrDefault();
            //Ищем выбранный анализ в БД
            DefaultAnalysis defaultAnalysis = db.DefaultAnalyzes.Where(u=> u.Id == selectedAnalysis).FirstOrDefault();
            //Создаем список анализов, для хранения анализов, которые входят в введенный промежуток дат
            List<Analysis> dataAnalysis = new List<Analysis>();
            //Временная переменная для проверки даты
            DateTime myDate;
            //Проверяем, правильно ли введена дата начала и дата конца
            if (!DateTime.TryParse(firstDate, out myDate) || !DateTime.TryParse(secondDate, out myDate))
            {

            }
            else
            {
                //Если все введено верно, ищем все анализы выбранного типа у пользователя в БД
                dataAnalysis = db.Analyzes.Where(u => u.UserId == user.Id && u.AnalysisName == defaultAnalysis.AnalysisName).ToList();
                //Временный список для хранения анализов
                List<Analysis> tempList = new List<Analysis>();
                //Перебираем список анализов пользователя
                foreach (Analysis analysis in dataAnalysis)
                {
                    //Проверяем, входит ли дата анализа в выбранный промежуток
                    myDate = DateTime.Parse(analysis.Date);
                    DateTime myFirstDate = DateTime.Parse(firstDate);
                    DateTime mySecondDate = DateTime.Parse(secondDate);
                    if (myDate >= myFirstDate && myDate <= mySecondDate)
                    {
                        //Если входит, добавляем анализ в список
                        tempList.Add(analysis);
                    }
                }
                //Переопределяем список анализов
                dataAnalysis = tempList;
            }
            //Если у пользователя в выбранный промежуток есть такие анализы
            if (dataAnalysis.Count > 0)
            {
                //Определяем объект, в котором храняться сгруппированные данные
                SelectionStats stats = new SelectionStats()
                {
                    Dates = new List<string>(),
                    Parameters = new List<string>(),
                    NormalValue = new List<List<decimal>>(),
                    Results = new List<List<decimal>>()
                };
                //Перебираем список анализов
                foreach(var item in dataAnalysis)
                {
                    //Если такой даты еще нет в статистике
                    if (!stats.Dates.Contains(item.Date))
                    {
                        //Добавляем дату в статистику
                        stats.Dates.Add(item.Date);
                        //Определяем временный список значений
                        List<decimal> values = new List<decimal>();
                        //Записываем 
                        for (int i = 0; i < item.ValuesOfAnalyzes.Count; i++)
                        {
                            //В список результатов добавляем новый список пока размер меньше i+1
                            if(stats.Results.Count < (i + 1)){ stats.Results.Add(new List<decimal> { }); }
                            //Заполняем добавленный список значениями для каждого анализа
                            stats.Results[i].Add(item.ValuesOfAnalyzes[i].Value);
                        }
                        //Перебираем значения анализа
                        foreach (var value in item.ValuesOfAnalyzes)
                        {
                            //Ищем тип значения в БД
                            TypesOfAnalysis parametr = db.TypesOfAnalyzes.Where(u => u.Id == value.TypeId).FirstOrDefault();
                            User users = db.Users.Where(u => u.Username == User.Identity.Name).FirstOrDefault();
                            //Если список параметров не включает в себя такой параметр
                            if (!stats.Parameters.Contains(parametr.TypeName))
                            {
                                //Добавляем параметр в список параметров
                                stats.Parameters.Add(parametr.TypeName);
                                //Ищем нормальное значение параметра для возраста пользователя
                                NormalValues normalValue = db.NormalValues.Where(u => u.TypeId == parametr.Id && (users.Age >= u.minAge && users.Age < u.maxAge)).FirstOrDefault();
                                //Добавляем нормальное значение в список
                                stats.NormalValue.Add(new List<decimal> { normalValue.minValue, normalValue.maxValue});
                            }
                        }
                        stats.Results.Add(values);
                    }
                }
                //Добавляем всю статистику во ViewBag
                ViewBag.Dates = stats.Dates;
                ViewBag.Parameters = stats.Parameters;
                ViewBag.Values = stats.Results;
                ViewBag.NormalValue = stats.NormalValue;
            }
            else
            {
                //Добавляем пустые списки во ViewBag, если нет анализов за выбранный промежуток
                ViewBag.Dates = new List<string>();
                ViewBag.Parameters = new List<string>();
                ViewBag.Values = new List<List<decimal>>();
                ViewBag.NormalValue = new List<List<decimal>>();
            }
            return View();
        }
    }
}