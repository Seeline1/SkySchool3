using OfficeOpenXml;
using OfficeOpenXml.Drawing.Chart;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkySchool3.Classes
{
    public class ModelReport
    {
        public Report Report { get; set; }
        public List<string> StudList = new List<string>();
    }

    public class ModelPlan
    {
        public Plan Plan { get; set; }
    }

    public class StudListItem
    {
        public string FIO { get; set; }
        //public int Mark { get; set; }
        //public string Prisutstvie { get; set; }
    }

    public class Report
    {
        public string Discipline { get; set; }
        public string LessonType { get; set; }
        public string FullNameTeacher { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string Group { get; set; }
        public string FullNameStudent { get; set; }
        public string Present { get; set; }
        public string Assessment { get; set; }
        public string Discription { get; set; }
    }

    public class Plan
    {
        public string Discipline { get; set; }
        public string LessonType { get; set; }
        public string FullNameTeacher { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string Group { get; set; }
        public string Discription { get; set; }
    }

    public class ModelReporter
    {
        public ModelReport GetReport()
        {
            return new ModelReport
            {
                Report = new Report { },
                StudList = new List<string>()
            };
        }
    }

    public class ModelPlaner
    {
        public ModelPlan GetReport()
        {
            return new ModelPlan
            {
                Plan = new Plan { }
            };
        }
    }

    public class ReportExcelGenerator
    {
        public byte[] Generate(ModelReport report)
        {
            var package = new ExcelPackage();
            var sheet = package.Workbook.Worksheets
                .Add("Report");

            sheet.Cells[1, 1].Value = "Дисциплина:";
            sheet.Cells[1, 2].Value = report.Report.Discipline;

            sheet.Cells[2, 1].Value = "Тип занятия:";
            sheet.Cells[2, 2].Value = report.Report.LessonType;

            sheet.Cells[3, 1].Value = "Преподаватель:";
            sheet.Cells[3, 2].Value = report.Report.FullNameTeacher;

            sheet.Cells[4, 1].Value = "День занятия:";
            sheet.Cells[4, 2].Value = report.Report.Date;

            sheet.Cells[5, 1].Value = "Время занятия:";
            sheet.Cells[5, 2].Value = report.Report.Time;

            sheet.Cells[6, 1].Value = "Описание занятия:";
            sheet.Cells[6, 2].Value = report.Report.Discription;
            sheet.Cells[6, 2].Style.WrapText = true;

            sheet.Cells[8, 1].Value = "Группа:";
            sheet.Cells[8, 2].Value = "Студент:";
            sheet.Cells[8, 3].Value = "Присутствие:";
            sheet.Cells[8, 4].Value = "Оценка:";

            sheet.Cells[9, 1].Value = report.Report.Group;
            sheet.Cells[9, 2].Value = report.Report.FullNameStudent;
            sheet.Cells[9, 3].Value = report.Report.Present;
            sheet.Cells[9, 4].Value = report.Report.Assessment;

            int row = 9;
            int column = 1;
            foreach (var item in report.StudList)
            {
                sheet.Cells[row, column].Value = report.Report.Group;
                sheet.Cells[row, column + 1].Value = item;
                row++;
            }

            sheet.Cells["A8:D8"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            sheet.Column(1).Width = 18;
            sheet.Column(2).Width = 35;
            sheet.Column(3).Width = 15;
            sheet.Column(4).Width = 10;

            sheet.Protection.IsProtected = true;
            return package.GetAsByteArray();
        }
    }

    public class PlanExcelGenerator
    {
        public byte[] Generate(ModelPlan report)
        {
            var package = new ExcelPackage();
            var sheet = package.Workbook.Worksheets
                .Add("Plan");

            sheet.Cells[1, 1].Value = "Дисциплина:";
            sheet.Cells[1, 2].Value = report.Plan.Discipline;

            sheet.Cells[2, 1].Value = "Тип занятия:";
            sheet.Cells[2, 2].Value = report.Plan.LessonType;

            sheet.Cells[3, 1].Value = "Преподаватель:";
            sheet.Cells[3, 2].Value = report.Plan.FullNameTeacher;

            sheet.Cells[4, 1].Value = "День занятия:";
            sheet.Cells[4, 2].Value = report.Plan.Date;

            sheet.Cells[5, 1].Value = "Время занятия:";
            sheet.Cells[5, 2].Value = report.Plan.Time;

            sheet.Cells[6, 1].Value = "Группа:";
            sheet.Cells[6, 2].Value = report.Plan.Group;

            sheet.Cells[7, 1].Value = "Описание занятия:";
            sheet.Cells[7, 2].Value = report.Plan.Discription;
            sheet.Cells[7, 2].Style.WrapText = true;

            sheet.Column(1).Width = 18;
            sheet.Column(2).Width = 35;

            sheet.Protection.IsProtected = true;
            return package.GetAsByteArray();
        }
    }
}