using System;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SkySchool3.Classes;
using SkySchool3.Pages;

namespace SkySchool3.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddEditPageDis1.xaml
    /// </summary>
    public partial class AddEditPageDiscipline : Page, IDisposable
    {
        private readonly SkySchool3Entities _context;
        private Discipline _currentDis;
        private string dis = "";

        public AddEditPageDiscipline(Discipline selectedDis)
        {
            InitializeComponent();

            _currentDis = new Discipline();
            _context = new SkySchool3Entities();

            if (selectedDis != null)
            {
                _currentDis = selectedDis;
            }

            DataContext = _currentDis;
            TxtDis.Text = _currentDis.Name;
            dis = _currentDis.Name;
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        private async void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(TxtDis.Text))
            {
                errors.AppendLine("Укажите название");
            }

            Discipline tempDis = await _context.Discipline.FirstOrDefaultAsync(p => p.Name.Equals(_currentDis.Name));
            if (TxtDis.Text == dis) { }
            else if (tempDis != null)
            {
                errors.AppendLine("Такая дисциплина уже существует");
            }

            if (TxtDis.Text.Length <= 100)
            {
                bool ru = true;

                for (int i = 0; i < TxtDis.Text.Length; i++)
                {
                    if (TxtDis.Text[i] >= 'A' && TxtDis.Text[i] <= 'Z')
                    {
                        ru = false;
                    }

                    if (TxtDis.Text[i] >= 'a' && TxtDis.Text[i] <= 'z')
                    {
                        ru = false;
                    }
                }

                if (!ru)
                {
                    errors.AppendLine("Доступна только русская раскладка");
                }
            }
            else
            {
                errors.AppendLine("Название должно содержать до 100 символов");
            }

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            if (_currentDis.ID_Discipline == 0)
            {
                _context.Discipline.Add(new Discipline()
                { Name = TxtDis.Text });
            }
            else
            {
                var disciplineToUpdate = await _context.Discipline.FirstOrDefaultAsync(x => x.ID_Discipline == _currentDis.ID_Discipline);
                disciplineToUpdate.ID_Discipline = _currentDis.ID_Discipline;
            }

            try
            {
                await _context.SaveChangesAsync();
                MessageBox.Show("Информация сохранена!");
                Manager.MainFrame.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void ImgBck_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Manager.MainFrame.GoBack();
        }
    }
}