using Practice.model;
using Practice.view;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Practice
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string LogInPassword;

        public MainWindow()
        {
            InitializeComponent();
        }
        private void btnEnter_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new PracticeStudyCenterEntities())
            {
                var user1 = context.Users
                    .FirstOrDefault(u => u.Login == txbLogin.Text
                    && u.Password == LogInPassword);

                var user2 = context.Users
                    .FirstOrDefault(u => u.Login == txbLogin.Text
                    && u.Password == LogInPassword);

                if (user1 != null)
                {
                    CurrentUser.ID = user1.UserID;
                    CurrentUser.Login = user1.Login;
                    CurrentUser.Status = (int)user1.StatusID;
                    CurrentUser.FirstName = user1.FirstName;
                    CurrentUser.LastName = user1.LastName;
                    CurrentUser.MidleName = user1.MiddleName;
                    CurrentUser.Email = user1.Email;
                    CurrentUser.PhoneNumber = user1.PhoneNumber;
                    CurrentUser.Password = user1.Password;

                    Home home = new Home();
                    home.Show();
                    this.Close();
                }
                else if (user2 != null)
                {
                    CurrentUser.ID = user2.UserID;
                    CurrentUser.Login = user2.Login;
                    CurrentUser.Status = (int)user2.StatusID;
                    CurrentUser.FirstName = user2.FirstName;
                    CurrentUser.LastName = user2.LastName;
                    CurrentUser.MidleName = user2.MiddleName;
                    CurrentUser.Email = user2.Email;
                    CurrentUser.PhoneNumber = user2.PhoneNumber;
                    CurrentUser.Password = user2.Password;

                    Home home = new Home();
                    home.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Invalid username or password.");
                }
            }
        }
        private void PasswordBox1_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var passwordBox = (PasswordBox)sender;
            LogInPassword = passwordBox.Password;
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
