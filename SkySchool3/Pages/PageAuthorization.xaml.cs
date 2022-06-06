using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SkySchool3.Classes;
using SkySchool3.Pages;

namespace SkySchool3.Pages
{
    /// <summary>
    /// Логика взаимодействия для PageAuth.xaml
    /// </summary>
    public partial class PageAuthorization : Page
    {
        public PageAuthorization()
        {
            InitializeComponent();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (PasswordBox.Password.Length > 0)
            {
                ImgShowHide.Visibility = Visibility.Visible;
            }
            else
            {
                ImgShowHide.Visibility = Visibility.Hidden;
            }
        }

        private void ImgShowHide_MouseLeave(object sender, MouseEventArgs e)
        {
            HidePassword();
        }

        private void ImgShowHide_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            ShowPassword();
        }

        private void ImgShowHide_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            HidePassword();
        }

        void ShowPassword()
        {
            TxtPassword.Visibility = Visibility.Visible;
            PasswordBox.Visibility = Visibility.Hidden;
            TxtPassword.Text = PasswordBox.Password;
        }
        void HidePassword()
        {
            TxtPassword.Visibility = Visibility.Hidden;
            PasswordBox.Visibility = Visibility.Visible;
            PasswordBox.Focus();
        }

        private async void ButtonEntry_Click(object sender, RoutedEventArgs e)
        {
            TxtPassword.Text = PasswordBox.Password;
            PasswordBox.Password = TxtPassword.Text;
            if (await EntryCheck.Login(TxtLogin.Text, TxtPassword.Text))
            {
                if (Manager.CurrentUser.Role == "admin")
                {
                    Manager.MainFrame.Navigate(new PageAdmin());
                }
                else
                {
                    Manager.MainFrame.Navigate(new PageUser());
                }
            }
        }
    }
}
