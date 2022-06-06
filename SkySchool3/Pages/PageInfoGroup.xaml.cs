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
    /// Логика взаимодействия для PageInfoGruppa.xaml
    /// </summary>
    public partial class PageInfoGroup : Page, IDisposable
    {
        private readonly SkySchool3Entities _context;

        public PageInfoGroup()
        {
            InitializeComponent();

            _context = new SkySchool3Entities();

            DGridGroups.ItemsSource = _context.Group.OrderBy(h => h.Group_Number).ToList();
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                _context.ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                DGridGroups.ItemsSource = _context.Group.ToList();
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPageGroup());
        }

        private async void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            var GrForRemoving = DGridGroups.SelectedItems.Cast<Group>().ToList();

            if (MessageBox.Show($"Вы точно хотите удалить следующи {GrForRemoving.Count()} элементов?", "Внимание",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    _context.Group.RemoveRange(GrForRemoving);
                    await _context.SaveChangesAsync();
                    MessageBox.Show("Данные удалены!");

                    DGridGroups.ItemsSource = _context.Group.ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void ImgHome_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Manager.MainFrame.GoBack();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
