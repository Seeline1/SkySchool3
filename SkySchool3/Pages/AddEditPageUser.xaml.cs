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
    /// Логика взаимодействия для AddEditPage.xaml
    /// </summary>
    public partial class AddEditPageUser : Page, IDisposable
    {
        private readonly SkySchool3Entities _context;
        private User _currentUser;
        private string log = "";

        public AddEditPageUser(User selectedUser)
        {
            InitializeComponent();

            _currentUser = new User();
            _context = new SkySchool3Entities();

            if (selectedUser != null)
            {
                _currentUser = selectedUser;
            }

            DataContext = _currentUser;
            TxtFullName.Text = _currentUser.Full_Name;
            TxtRole.Text = _currentUser.Role;
            TxtLogin.Text = _currentUser.Login;
            TxtPassword.Text = _currentUser.Password;
            DGridDisciplines.ItemsSource = _context.List_of_Discipline.Where(h => h.User.Login == _currentUser.Login).ToList();
            log = _currentUser.Login;
            BtnVisivle();
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                _context.ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                DGridDisciplines.ItemsSource = _context.List_of_Discipline.Where(h => h.User.Login == _currentUser.Login).ToList();
            }
        }

        private async void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            User tempUser = await _context.User.FirstOrDefaultAsync(p => p.Login.Equals(_currentUser.Login));

            if (TxtLogin.Text == log) { }
            else if (tempUser != null)
            {
                errors.AppendLine("Пользователь с таким логином уже существует");
            }

            if (TxtFullName.Text.Length < 150)
            {
                for (int i = 0; i < TxtFullName.Text.Length; i++)
                {
                    if (TxtFullName.Text[i] >= '0' && TxtFullName.Text[i] <= '9')
                    {
                        errors.AppendLine("ФИО не должно содержать цифр");
                    }
                }

                if (string.IsNullOrWhiteSpace(TxtFullName.Text))
                {
                    errors.AppendLine("Укажите ФИО");
                }
            }
            else
            {
                errors.AppendLine("ФИО должно содержать не более 150 символов");
            }

            if (TxtRole.Text == "user" || TxtRole.Text == "admin") { }
            else
            {
                errors.AppendLine("Укажите роль: \"admin\" или \"user\"");
            }

            if (TxtLogin.Text.Length == 11)
            {
                for (int i = 0; i < TxtLogin.Text.Length; i++)
                {
                    if (TxtLogin.Text[i] >= '0' && TxtLogin.Text[i] <= '9') { }
                    else
                    {
                        errors.AppendLine("Логин должен состоять только из цифр");
                        break;
                    }
                }
            }
            else
            {
                errors.AppendLine("Логин должен состоять из 11 цифр");
            }

            if (TxtPassword.Text.Length >= 6 && TxtPassword.Text.Length <= 15)
            {
                bool en = true;
                bool num = false;
                bool symbol = false;

                for (int i = 0; i < TxtPassword.Text.Length; i++)
                {
                    if (TxtPassword.Text[i] >= 'А' && TxtPassword.Text[i] <= 'Я')
                    {
                        en = false;
                    }

                    if (TxtPassword.Text[i] >= 'а' && TxtPassword.Text[i] <= 'я')
                    {
                        en = false;
                    }

                    if (TxtPassword.Text[i] >= '0' && TxtPassword.Text[i] <= '9')
                    {
                        num = true;
                    }

                    if (TxtPassword.Text[i] == '!' || TxtPassword.Text[i] == '@' || TxtPassword.Text[i] == '$')
                    {
                        symbol = true;
                    }
                }

                if (!en)
                {
                    errors.AppendLine("Доступна только английская раскладка");
                }

                if (!num)
                {
                    errors.AppendLine("Добавьте хотя бы одну цифру");
                }

                if (!symbol)
                {
                    errors.AppendLine("Добавьте один из следующих символов: \"!\" или \"@\" или \"$\"");
                }
            }
            else
            {
                errors.AppendLine("Пароль должен содержать от 6 до 15 символов");
            }

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            if (_currentUser.ID_User == 0)
            {
                _context.User.Add(new User()
                { Full_Name = TxtFullName.Text, Login = TxtLogin.Text, Password = TxtPassword.Text, Role = TxtRole.Text });
            }
            else
            {
                var userToUpdate = await _context.User.FirstOrDefaultAsync(x => x.ID_User == _currentUser.ID_User);
                userToUpdate.Login = TxtLogin.Text;
                userToUpdate.Password = TxtPassword.Text;
                userToUpdate.Role = TxtRole.Text;
                userToUpdate.Full_Name = TxtFullName.Text;
            }

            try
            {
                await _context.SaveChangesAsync();
                MessageBox.Show("Информация сохранена");
                Manager.MainFrame.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }

        }

        private void ImgAdd_MouseDown(object sender, MouseButtonEventArgs e)
        {
            WindowAddListDiscipline Windowspadd = new WindowAddListDiscipline(_currentUser.ID_User);
            Windowspadd.Show();
        }

        private async void ImgDel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var UserDisciplinesforRemoving = DGridDisciplines.SelectedItems.Cast<List_of_Discipline>().ToList();

            if (UserDisciplinesforRemoving.Count == 0)
            {
                MessageBox.Show("Выберите элементы для удаления");
            }
            else if (MessageBox.Show($"Вы точно хотите удалить следующи {UserDisciplinesforRemoving.Count()} элементов?", "Внимание",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    _context.List_of_Discipline.RemoveRange(UserDisciplinesforRemoving);
                    await _context.SaveChangesAsync();
                    MessageBox.Show("Данные удалены!");

                    DGridDisciplines.ItemsSource = _context.List_of_Discipline.Where(h => h.User.Login == _currentUser.Login).ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void ImgBck_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Manager.MainFrame.GoBack();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        private void ImgUpdate_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                _context.ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                DGridDisciplines.ItemsSource = _context.List_of_Discipline.Where(h => h.User.Login == _currentUser.Login).ToList();
            }
        }

        private void BtnVisivle()
        {
            if (_currentUser.ID_User == 0)
            {
                ImgAdd.Visibility = Visibility.Hidden;
                ImgDel.Visibility = Visibility.Hidden;
                ImgUpdate.Visibility = Visibility.Hidden;
            }
        }
    }
}