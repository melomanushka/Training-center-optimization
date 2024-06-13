using Practice.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Practice.view.ForAdmin
{
    /// <summary>
    /// Логика взаимодействия для CurrentUserEdit.xaml
    /// </summary>
    public partial class CurrentUserEdit : Window
    {
        int id = 0;
        public CurrentUserEdit(CurrentUserForTable currentUser)
        {
            InitializeComponent();
            id = currentUser.ID;
            txbName.Text =  currentUser.FirstName;
            txbLast.Text = currentUser.LastName;
            txbMiddle.Text = currentUser.MiddleName;
            txbEmail.Text = currentUser.Email;
            txbPhone.Text = currentUser.PhoneNumber;
            txbLogin.Text = currentUser.Login;
            txbPassword.Text = currentUser.Password;
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            using (PracticeStudyCenterEntities context = new PracticeStudyCenterEntities())
            {
                var user = context.Users.FirstOrDefault(s => s.UserID == id);
                if (user != null)
                {
                    user.FirstName = txbName.Text;
                    user.LastName = txbLast.Text;
                    user.MiddleName = txbMiddle.Text;
                    user.Email = txbEmail.Text;
                    user.PhoneNumber = txbPhone.Text;
                    user.Login = txbLogin.Text;
                    user.Password = txbPassword.Text;

                    context.SaveChanges();

                    MessageBox.Show("Данные успешно обновлены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Элемент с указанным кодом не найден!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
