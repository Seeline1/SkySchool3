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
    public partial class PageInfoStudent : Page, IDisposable
    {
        public readonly SkySchool3Entities _context;

        public PageInfoStudent()
        {
            InitializeComponent();

            _context = new SkySchool3Entities();

            DGridStudents.ItemsSource = _context.Student.OrderBy(h => h.ID_Group).ToList();
        }

        private void ImgHome_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Manager.MainFrame.GoBack();
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                _context.ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                DGridStudents.ItemsSource = _context.Student.ToList();
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPageStudent(new Student()));
        }

        private async void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            var StudentsForRemoving = DGridStudents.SelectedItems.Cast<Student>().ToList();

            if (MessageBox.Show($"Вы точно хотите удалить следующи {StudentsForRemoving.Count()} элементов?", "Внимание",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    _context.Student.RemoveRange(StudentsForRemoving);
                    await _context.SaveChangesAsync();
                    MessageBox.Show("Данные удалены!");

                    DGridStudents.ItemsSource = _context.Student.ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPageStudent((sender as Button).DataContext as Student));
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
