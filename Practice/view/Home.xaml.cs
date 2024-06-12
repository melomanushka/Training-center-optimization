using Practice.view.ForTeacher;
using Practice.view.ForStudent;
using Practice.view.ForAdmin;
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
using Practice.model;

namespace Practice.view
{
    /// <summary>
    /// Логика взаимодействия для Home.xaml
    /// </summary>
    public partial class Home : Window
    {
        public Home()
        {
            InitializeComponent();
            PagesNavigation.Content = new HomeTeacher();
            txblName.Text = CurrentUser.GetFullName();
            txblEmail.Text = CurrentUser.Email;
            if (CurrentUser.Status != 1)
            {
                rdEnrollment.Visibility = Visibility.Collapsed;
            }
        }
        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;
            if (radioButton != null && radioButton.IsChecked == true)
            {
                switch (radioButton.Content.ToString())
                {
                    case " Главная":
                        if (CurrentUser.Status == 1)
                        {
                            LoadPage(new HomeAdmin());
                        }
                        else if (CurrentUser.Status == 2)
                        {
                            LoadPage(new HomeTeacher());
                        }
                        else
                        {
                            LoadPage(new HomeStudent());
                        }
                        break;
                    case " Курсы":
                        if (CurrentUser.Status == 1)
                        {
                            LoadPage(new CourseAdmin());
                        }
                        else if (CurrentUser.Status == 2)
                        {
                            LoadPage(new CourseTeacher());
                        }
                        else
                        {
                            LoadPage(new CourseStudent());
                        }
                        break;
                    case " Обучение":
                        if (CurrentUser.Status == 1)
                        {
                            LoadPage(new TaskAdmin());
                        }
                        else if (CurrentUser.Status == 2)
                        {
                            LoadPage(new TaskTeacher());
                        }
                        else
                        {
                            LoadPage(new TaskStudent());
                        }
                        break;
                    case " Сертификаты":
                        if (CurrentUser.Status == 1)
                        {
                            LoadPage(new ReportAdmin());
                        }
                        else if (CurrentUser.Status == 2)
                        {
                            LoadPage(new ReportTaecher());
                        }
                        else
                        {
                            LoadPage(new ReportStudent());
                        }
                        break;
                    case " Договоры":
                        LoadPage(new ContractsPage());
                        break;
                    case " Помощь":
                        LoadPage(new HelpPage());
                        break;
                }
            }
        }
        private void LoadPage(Page page)
        {
            PagesNavigation.Content = page;
        }
        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }

        private void btn_maximize_Click(object sender, RoutedEventArgs e)
        {
            SwitchWindowState();
        }

        private void btn_minimize_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).WindowState = WindowState.Minimized;
        }

        private void DockPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount >= 2)
            {
                SwitchWindowState();
                return;
            }
            if (Window.GetWindow(this).WindowState == WindowState.Maximized)
            {
                return;
            }
            else
            {
                if (e.LeftButton == MouseButtonState.Pressed) Window.GetWindow(this).DragMove();
            }
        }

        private void MaximizeWindow()
        {
            Window.GetWindow(this).WindowState = WindowState.Maximized;
        }

        private void RestoreWindow()
        {
            Window.GetWindow(this).WindowState = WindowState.Normal;
        }

        private void SwitchWindowState()
        {
            if (Window.GetWindow(this).WindowState == WindowState.Normal) MaximizeWindow();
            else RestoreWindow();
        }

        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            rdHome.IsChecked = true;
        }

        private void rdClose_Checked(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
