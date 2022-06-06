using System;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    public partial class AddEditPageStudent : Page, IDisposable
    {
        private readonly SkySchool3Entities _context;
        private Student _currentStud;
        private string fio = "";
        private int ng = 0;

        public AddEditPageStudent(Student selectedStud)
        {
            InitializeComponent();

            _currentStud = new Student();
            _context = new SkySchool3Entities();

            if (selectedStud != null)
            {
                _currentStud = selectedStud;
            }

            DataContext = _currentStud;
            TxtFullName.Text = _currentStud.Full_Name;

            ComboBoxNG.ItemsSource = _context.Group.ToList();

            fio = _currentStud.Full_Name;
            ng = _currentStud.ID_Group;
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        private async void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            Group grupp = ComboBoxNG.SelectedItem as Group;

            if (!string.IsNullOrWhiteSpace(_currentStud.Full_Name))
            {
                if (grupp != null)
                {
                    Student tempStud = await _context.Student.FirstOrDefaultAsync(p => p.Full_Name.Equals(_currentStud.Full_Name) && p.ID_Group.Equals(grupp.ID_Group));

                    if (TxtFullName.Text == fio && ComboBoxNG.Text == Convert.ToString(ng)) { }
                    else if (tempStud != null)
                    {
                        errors.AppendLine("Такой студент уже существует");
                    }
                }
                else
                {
                    errors.AppendLine("Укажите номер группы");
                }
            }
            else
            {
                errors.AppendLine("Укажите ФИО");
            }

            if (TxtFullName.Text.Length < 150)
            {
                for (int i = 0; i < TxtFullName.Text.Length; i++)
                {
                    if (TxtFullName.Text[i] >= '0' && TxtFullName.Text[i] <= '9')
                    {
                        errors.AppendLine("ФИО может содержать только буквы");
                        break;
                    }
                }
            }
            else
            {
                errors.AppendLine("Фио не может содержать более 150 символов");
            }

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            Group group = ComboBoxNG.SelectedItem as Group;

            if (_currentStud.ID_Student == 0)
            {
                _context.Student.Add(new Student()
                { Full_Name = TxtFullName.Text, ID_Group = group.ID_Group });
            }
            else
            {
                var studentToUpdate = await _context.Student.FirstOrDefaultAsync(x => x.ID_Student == _currentStud.ID_Student);
                studentToUpdate.Full_Name = TxtFullName.Text;
                studentToUpdate.ID_Group = group.ID_Group;
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