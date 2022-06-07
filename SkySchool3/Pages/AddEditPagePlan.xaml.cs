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
    /// Логика взаимодействия для AddEditPagePlan.xaml
    /// </summary>
    public partial class AddEditPagePlan : Page, IDisposable
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

        public AddEditPagePlan()
        {
            InitializeComponent();

            _currentUser = new User();
            _context = new SkySchool3Entities();

            //_currentUser.ID_User = 1;
            //_currentUser.Login = "1";
            //_currentUser.Password = "2";
            //_currentUser.Role = "admin";
            //_currentUser.Full_Name = "dawda";
            log = Manager.CurrentUser.Login + "\\";
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

            var planData = new ModelPlaner().GetReport();
            planData.Plan.Discipline = ds.Name;
            planData.Plan.LessonType = tp.Type;
            planData.Plan.FullNameTeacher = Manager.CurrentUser.Full_Name;
            planData.Plan.Date = date;
            planData.Plan.Time = CBTime.Text;
            planData.Plan.Group = Convert.ToString(gr.Group_Number);
            planData.Plan.Discription = TxtDiscription.Text;
            var planExcel = new PlanExcelGenerator()
                .Generate(planData);
            string path = "plans\\" + log + "Plan" + date1 + ".xlsx";
            File.WriteAllBytes(path, planExcel);
            MessageBox.Show("Информация сохранена!");
            Manager.MainFrame.GoBack();
        }

        private void ImgBck_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Manager.MainFrame.GoBack();
        }
    }
}
