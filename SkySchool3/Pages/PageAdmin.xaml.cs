using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SkySchool3.Classes;
using SkySchool3.Pages;

namespace SkySchool3.Pages
{
    /// <summary>
    /// Логика взаимодействия для PageAdmin.xaml
    /// </summary>
    public partial class PageAdmin : Page, IDisposable
    {
        public static SkySchool3Entities _context;

        public PageAdmin()
        {
            InitializeComponent();

            _context = new SkySchool3Entities();
            TBLogin.Text = Manager.CurrentUser.Login.ToString();
            TBName.Text = Manager.CurrentUser.Full_Name.ToString();
            TBParol.Text = "**********";
            TBRole.Text = Manager.CurrentUser.Role.ToString();
            DGridDisciplines.ItemsSource = _context.List_of_Discipline.Where(h => h.User.Login == TBLogin.Text).ToList();
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                _context.ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                DGridDisciplines.ItemsSource = _context.List_of_Discipline.Where(h => h.User.Login == TBLogin.Text).ToList();
            }
        }

        private void ImgOut_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (MessageBox.Show("Вы точно хотите выйти из профиля?", "Внимание",
                 MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Manager.MainFrame.Navigate(new PageAuthorization());
            }
        }

        private void CheckPass_Checked(object sender, RoutedEventArgs e)
        {
            if (TBParol.Text == Manager.CurrentUser.Password.ToString())
            {
                TBParol.Text = "**********";
            }
        }

        private void CheckPass_Unchecked(object sender, RoutedEventArgs e)
        {
            if (TBParol.Text == "**********")
            {
                TBParol.Text = Manager.CurrentUser.Password.ToString();
            }
        }

        private void MyReport_Click(object sender, RoutedEventArgs e)
        {
            //Manager.MainFrame.Navigate(new PageInfoReport());
        }

        private void MyPlan_Click(object sender, RoutedEventArgs e)
        {
            //Manager.MainFrame.Navigate(new PageInfoPlan());
        }

        private void Users_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new PageInfoUser());
        }

        private void Duscpilines_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new PageInfoDiscipline());
        }

        private void Groups_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new PageInfoGroup());
        }

        private void Students_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new PageInfoStudent());
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
