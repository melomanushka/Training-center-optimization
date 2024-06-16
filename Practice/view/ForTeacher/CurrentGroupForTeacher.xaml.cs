using Practice.model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace Practice.view.ForTeacher
{
    /// <summary>
    /// Логика взаимодействия для CurrentGroupForTeacher.xaml
    /// </summary>
    public partial class CurrentGroupForTeacher : Window
    {
        ObservableCollection<CurrentUserForTeacher> students;
        int idGroup = 0;
        public CurrentGroupForTeacher(CurrentUserForTeacher currentUser)
        {
            InitializeComponent();
            idGroup = currentUser.GroupsID;
            UpdateDataGrid();
        }
        private void UpdateDataGrid()
        {
            using (PracticeStudyCenterEntities context = new PracticeStudyCenterEntities())
            {
                students = new ObservableCollection<CurrentUserForTeacher>(context.StudentGroup
                    .Where(r => r.GroupID == idGroup)
                    .Select(r => new CurrentUserForTeacher
                    {
                        UserName = r.Student.Users.FirstName,
                        UserLast = r.Student.Users.LastName,
                        UserMiddle = r.Student.Users.MiddleName,
                        Email = r.Student.Users.Email,
                        PhoneNumber = r.Student.Users.PhoneNumber,
                    }).ToList());

                dtgrStudents.ItemsSource = students;
            }
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
