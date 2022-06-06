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
    public partial class PageInfoDiscipline : Page, IDisposable
    {
        private readonly SkySchool3Entities _context;

        public PageInfoDiscipline()
        {
            InitializeComponent();

            _context = new SkySchool3Entities();

            DGridDisciplines.ItemsSource = _context.Discipline.OrderBy(h => h.Name).ToList();
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
                DGridDisciplines.ItemsSource = _context.Discipline.ToList();
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPageDiscipline(new Discipline()));
        }

        private async void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            var DisciplunesForRemoving = DGridDisciplines.SelectedItems.Cast<Discipline>().ToList();

            if (MessageBox.Show($"Вы точно хотите удалить следующи {DisciplunesForRemoving.Count()} элементов?", "Внимание",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    _context.Discipline.RemoveRange(DisciplunesForRemoving);
                    await _context.SaveChangesAsync();
                    MessageBox.Show("Данные удалены!");

                    DGridDisciplines.ItemsSource = _context.Discipline.ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPageDiscipline((sender as Button).DataContext as Discipline));
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}