using System;
using System.Threading.Tasks;
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
    /// Логика взаимодействия для AddEditPageGruppa.xaml
    /// </summary>
    public partial class AddEditPageGroup : Page, IDisposable
    {
        private readonly SkySchool3Entities _context;
        private Group _currentGr;
        private int ng = 0;

        public AddEditPageGroup()
        {
            InitializeComponent();

            _currentGr = new Group();
            _context = new SkySchool3Entities();

            DataContext = _currentGr;
            TxtNG.Text = Convert.ToString(_currentGr.Group_Number);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        private async void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (TxtNG.Text.Length == 4)
            {
                for (int i = 0; i < TxtNG.Text.Length; i++)
                {
                    if (TxtNG.Text[i] >= '0' && TxtNG.Text[i] <= '9')
                    {
                        ng = Convert.ToInt32(TxtNG.Text);
                    }
                    else
                    {
                        errors.AppendLine("Номер группы должен состоять только из цифр");
                        break;
                    }
                }
            }
            else
            {
                errors.AppendLine("Номер группы должен состоять из 4 цифр");
            }

            Group tempGr = await _context.Group.FirstOrDefaultAsync(p => p.Group_Number.Equals(ng));
            if (tempGr != null)
            {
                MessageBox.Show("Группа с таким номером уже существует");
                return;
            }

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            _context.Group.Add(new Group()
            {
                Group_Number = Convert.ToInt32(TxtNG.Text)
            });

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
