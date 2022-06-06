using System;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Windows;
//using SkySchool3;
using SkySchool3.Classes;
using SkySchool3.Pages;

namespace SkySchool3
{
    /// <summary>
    /// Логика взаимодействия для WindowSPAdd.xaml
    /// </summary>
    public partial class WindowAddListDiscipline : Window, IDisposable
    {
        public readonly SkySchool3Entities _context;
        private List_of_Discipline _currentSP;
        private int iduser = 0;

        public WindowAddListDiscipline(int id)
        {
            InitializeComponent();
            iduser = id;
            _currentSP = new List_of_Discipline();
            _context = new SkySchool3Entities();

            DataContext = _currentSP;
            ComboBoxDiscipline.ItemsSource = _context.Discipline.ToList();
            ComboBoxType.ItemsSource = _context.Lesson_Type.ToList();
        }

        private async void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            Discipline discipline = ComboBoxDiscipline.SelectedItem as Discipline;
            Lesson_Type type = ComboBoxType.SelectedItem as Lesson_Type;

            if (discipline != null)
            {
                if (type != null)
                {
                    List_of_Discipline tempListDiscipline = await _context.List_of_Discipline.FirstOrDefaultAsync(p => p.ID_User.Equals(iduser) && p.ID_Discipline.Equals(discipline.ID_Discipline) && p.ID_Lesson_Type.Equals(type.ID_Lesson_Type));

                    if (tempListDiscipline != null)
                    {
                        errors.AppendLine("Эта дисциплина уже закреплена за преподавателем");
                    }
                }
                else
                {
                    errors.AppendLine("Выберите вид занятия");
                }
            }
            else
            {
                errors.AppendLine("Выберите дисциплину");
            }

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            _context.List_of_Discipline.Add(new List_of_Discipline()
            {
                ID_User = iduser,
                Discipline = ComboBoxDiscipline.SelectedItem as Discipline,
                Lesson_Type = ComboBoxType.SelectedItem as Lesson_Type
            });

            try
            {
                await _context.SaveChangesAsync();
                MessageBox.Show("Информация сохранена!");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
