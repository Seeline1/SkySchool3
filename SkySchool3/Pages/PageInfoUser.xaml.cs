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
    /// Логика взаимодействия для PageInfo.xaml
    /// </summary>
    public partial class PageInfoUser : Page, IDisposable
    {
        private readonly SkySchool3Entities _context;

        public PageInfoUser()
        {
            InitializeComponent();

            _context = new SkySchool3Entities();

            DGridUsers.ItemsSource = _context.User.OrderBy(h => h.Full_Name).ToList();
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                _context.ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                DGridUsers.ItemsSource = _context.User.ToList();
            }
        }

        private void ImgHome_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Manager.MainFrame.GoBack();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPageUser(new User()));
        }

        private async void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            var UsersForRemoving = DGridUsers.SelectedItems.Cast<User>().ToList();

            if (MessageBox.Show($"Вы точно хотите удалить следующи {UsersForRemoving.Count()} элементов?", "Внимание",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    _context.User.RemoveRange(UsersForRemoving);
                    await _context.SaveChangesAsync();
                    MessageBox.Show("Данные удалены!");

                    DGridUsers.ItemsSource = _context.User.ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPageUser((sender as Button).DataContext as User));
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
