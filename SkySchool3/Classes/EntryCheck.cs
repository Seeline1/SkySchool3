using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using SkySchool3.Classes;
using SkySchool3.Pages;

namespace SkySchool3.Classes
{
    class EntryCheck
    {
        public static async Task<bool> Login(string login, string password)
        {
            if (login.Length > 0)
            {
                if (password.Length > 0)
                {
                    using (SkySchool3Entities db = new SkySchool3Entities())
                    {
                        Manager.Currentuser = await db.User.FirstOrDefaultAsync(p => p.Login.Equals(login) && p.Password.Equals(password));
                    }

                    if (Manager.Currentuser != null)
                    {
                        MessageBox.Show("Пользователь успешно авторизовался");
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("Пользователь не найден");
                        return false;
                    }
                }
                else
                {
                    MessageBox.Show("Введите пароль");
                    return false;
                }
            }
            else
            {
                MessageBox.Show("Введите логин");
                return false;
            }
        }
    }
}
