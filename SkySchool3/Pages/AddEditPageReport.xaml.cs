using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using SkySchool3.Classes;
using SkySchool3.Pages;


namespace SkySchool3.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddEditPageReport.xaml
    /// </summary>
    public partial class AddEditPageReport : Page, IDisposable
    {
        private readonly SkySchool3Entities _context;
        private User _currentUser;
        private string date = "";
        private string date1 = "";
        private string[] itemsTime = { "8:00", "9:40", "11:20", "13:30", "15:10", "16:50", "18:20" };
        private int[] itemocenka = { 1, 2, 3, 4, 5 };
        private string log = "";
        //List<Ocenka> ocenka = new List<Ocenka>
        //{
        //    new Ocenka { ocenka ="1"},
        //    new Ocenka { ocenka ="2"},
        //    new Ocenka { ocenka ="3"},
        //    new Ocenka { ocenka ="4"},
        //    new Ocenka { ocenka ="5"},
        //};
        List<string> stud = new List<string>();

        public AddEditPageReport()
        {
            InitializeComponent();

            _currentUser = new User();
            _context = new SkySchool3Entities();

            //_currentUser.ID_User = 1;
            //_currentUser.Login = "1";
            //_currentUser.Password = "2";
            //_currentUser.Role = "admin";
            //_currentUser.Full_Name = "dawda";
            log = Manager.CurrentUser.Login + "\\";/*Manager.CurrentUser.Login;*/
            DGDis.ItemsSource = _context.List_of_Discipline.Where(h => h.User.Login == Manager.CurrentUser.Login).ToList();
            CBTime.ItemsSource = itemsTime;
            CBGrupp.ItemsSource = _context.Group.ToList();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            Group gr = CBGrupp.SelectedItem as Group;

            List_of_Discipline sd = DGDis.SelectedItem as List_of_Discipline;

            Discipline ds = _context.Discipline.FirstOrDefault(h => h.ID_Discipline == sd.ID_Discipline);

            Lesson_Type tp = _context.Lesson_Type.FirstOrDefault(h => h.ID_Lesson_Type == sd.ID_Lesson_Type);

            string a = calendar1.SelectedDate.Value.Day.ToString();
            string b = calendar1.SelectedDate.Value.Month.ToString();
            string c = calendar1.SelectedDate.Value.Year.ToString();
            date = a + "." + b + "." + c;

            string q = DateTime.Today.Day.ToString();
            string w = DateTime.Today.Month.ToString();
            string r = DateTime.Today.Year.ToString();
            string t = DateTime.Now.Hour.ToString();
            string y = DateTime.Now.Minute.ToString();
            date1 = q + "_" + w + "_" + r + "_" + t + "_" + y;

            stud = _context.Student.Where(h => h.ID_Group == gr.ID_Group).Select(x => x.Full_Name).ToList();
            var reportData = new ModelReporter().GetReport();
            reportData.Report.Discipline = ds.Name;
            reportData.Report.LessonType = tp.Type;
            reportData.Report.FullNameTeacher = Manager.CurrentUser.Full_Name;
            reportData.Report.Date = date;
            reportData.Report.Time = CBTime.Text;
            reportData.Report.Group = Convert.ToString(gr.Group_Number);

            var marks = DGStud.ItemsSource as List<StudentRowModel>;
            var marks2 = DGStud.Items.OfType<StudentRowModel>().ToList();

            //foreach (var student in stud)
            //{
            //    reportData.StudList.Add(new StudListItem()
            //    {
            //        FIO = student,
            //        Mark = 5,
            //        //Prisutstvie = DGStud.

            //    });
            //}
            reportData.StudList = stud;

            reportData.Report.Discription = TxtDiscription.Text;
            string dt = Convert.ToString(DateTime.Today);
            var reportExcel = new ReportExcelGenerator()
                .Generate(reportData);
            string path = "reports\\" + log + "Report" + date1 + ".xlsx";
            File.WriteAllBytes(path, reportExcel);
            MessageBox.Show("Информация сохранена!");
            Manager.MainFrame.GoBack();
        }

        private void ImgUpdate_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Group gr = CBGrupp.SelectedItem as Group;
            DGStud.ItemsSource = _context.Student.Where(h => h.ID_Group == gr.ID_Group).ToList();
            CBOcenka.ItemsSource = itemocenka;
        }

        private void ImgBck_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Manager.MainFrame.GoBack();
        }
    }

    public class StudentRowModel
    {
        public string FullName { get; set; }
        public bool Present { get; set; }
        public int Assessment { get; set; }
    }
}
