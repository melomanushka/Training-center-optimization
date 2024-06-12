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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Practice.view.ForTeacher
{
    /// <summary>
    /// Логика взаимодействия для HomeTeacher.xaml
    /// </summary>
    public partial class HomeTeacher : Page
    {
        ObservableCollection<CurrentUserForTeacher> students;
        ObservableCollection<CurrentUserForTeacher> groups;
        public HomeTeacher()
        {
            InitializeComponent();
            UpdateDataGrid();
        }
        private void UpdateDataGrid()
        {
            using (var context = new PracticeStudyCenterEntities())
            {
                students = new ObservableCollection<CurrentUserForTeacher>(context.IndividualLessons
                    .Where(il => il.UserID == CurrentUser.ID)
                    .Select(il => new CurrentUserForTeacher
                    {
                        StudentID = il.Student.StudentID,
                        UserName = il.Student.Users.FirstName,
                        UserLast = il.Student.Users.LastName,
                        UserMiddle = il.Student.Users.MiddleName,
                        CourseID = il.Course.CourseID,
                        CourseName = il.Course.Name,
                        Email = il.Student.Users.Email,
                        PhoneNumber = il.Student.Users.PhoneNumber
                    }).ToList());

                dtgrStudent.ItemsSource = students;

                groups = new ObservableCollection<CurrentUserForTeacher>(context.Groups
                    .Where(op => op.UserID == CurrentUser.ID)
                    .Select(op => new CurrentUserForTeacher
                    {
                        GroupName = op.NameGroup,
                        CourseName = op.Course.Name,
                    }).ToList());

                ItemList.ItemsSource = groups;
            }
        }
    }
}
