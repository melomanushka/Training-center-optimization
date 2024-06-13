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
    /// Логика взаимодействия для CurrentUserStudentEdit.xaml
    /// </summary>
    public partial class CurrentUserStudentEdit : Window
    {
        int id = 0;

        public CurrentUserStudentEdit(CurrentUserForTable currentUser)
        {
            InitializeComponent();
            id = currentUser.ID;
            txbName.Text = currentUser.FirstName;
            txbLast.Text = currentUser.LastName;
            txbMiddle.Text = currentUser.MiddleName;
            txbEmail.Text = currentUser.Email;
            txbPhone.Text = currentUser.PhoneNumber;
            txbLogin.Text = currentUser.Login;
            txbPassword.Text = currentUser.Password;
            txbPasSer.Text = currentUser.PassportSeries;
            txbPasNum.Text = currentUser.PassportNumber;
            txbPasWhere.Text = currentUser.PassportIssuedBy;
            datePicker.SelectedDate = currentUser.PassportIssueDate;
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
                var userSt = context.Student.FirstOrDefault(s => s.StudentID == id);

                if (userSt != null)
                {
                    userSt.Users.FirstName = txbName.Text;
                    userSt.Users.LastName = txbLast.Text;
                    userSt.Users.MiddleName = txbMiddle.Text;
                    userSt.Users.Email = txbEmail.Text;
                    userSt.Users.PhoneNumber = txbPhone.Text;
                    userSt.Users.Login = txbLogin.Text;
                    userSt.Users.Password = txbPassword.Text;
                    userSt.PassportSeries = txbPasSer.Text;
                    userSt.PassportNumber = txbPasNum.Text;
                    userSt.PassportIssuedBy = txbPasWhere.Text;
                    userSt.PassportIssueDate = datePicker.SelectedDate;

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
