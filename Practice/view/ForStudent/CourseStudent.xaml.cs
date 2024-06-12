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

namespace Practice.view.ForStudent
{
    /// <summary>
    /// Логика взаимодействия для CourseStudent.xaml
    /// </summary>
    public partial class CourseStudent : Page
    {
        ObservableCollection<CurrentCourse> courses;
        ObservableCollection<CurrentCourse> courses2;

        public CourseStudent()
        {
            InitializeComponent();
            UpdateDataGrid();
        }
        private void UpdateDataGrid()
        {
            using (var context = new PracticeStudyCenterEntities())
            {
                var student = context.Student.SingleOrDefault(s => s.UserID == CurrentUser.ID);
                int studentId = student.StudentID;

                courses = new ObservableCollection<CurrentCourse>(context.IndividualLessons
                    .Where(g => g.StudentID == studentId)
                    .Select(g => new CurrentCourse
                    {
                        CourseId = g.Course.CourseID,
                        Name = g.Course.Name,
                        Description = g.Course.Description,
                        Hours = (int)g.Course.Hours,
                        TypeCourseID = (int)g.Course.TypeCourseID,
                        TypeCourseName = g.Course.TypeCourse.TypeCourseName
                    }).ToList());

                ItemList.ItemsSource = courses;

                courses2 = new ObservableCollection<CurrentCourse>(context.StudentGroup
                    .Where(sg => sg.StudentID == studentId)
                    .Select(sg => sg.Groups.Course)
                    .Select(c => new CurrentCourse
                    {
                        CourseId = c.CourseID,
                        Name = c.Name,
                        Description = c.Description,
                        Hours = (int)c.Hours,
                        TypeCourseID = (int)c.TypeCourseID,
                        TypeCourseName = c.TypeCourse.TypeCourseName
                    }).ToList());

                ItemList2.ItemsSource = courses2;
            }
        }
    }
}
