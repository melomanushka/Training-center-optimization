using Practice.model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для CourseTeacher.xaml
    /// </summary>
    public partial class CourseTeacher : Page
    {
        ObservableCollection<CurrentCourse> courses;
        public CourseTeacher()
        {
            InitializeComponent();
            UpdateDataGrid();
        }
        private void UpdateDataGrid()
        {
            using (var context = new PracticeStudyCenterEntities())
            {
                courses = new ObservableCollection<CurrentCourse>(context.Groups
                    .Where(g => g.UserID == CurrentUser.ID)
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

            }
        }

        private void ButtonShowCourse_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                var group = button.CommandParameter as CurrentCourse;
                if (group != null)
                {
                    CurrentCourseForTeacher currentGroupForTeacher = new CurrentCourseForTeacher(group);
                    currentGroupForTeacher.Closed += EditWindow_Closed;
                    currentGroupForTeacher.ShowDialog();
                }
            }
        }
        private void EditWindow_Closed(object sender, EventArgs e)
        {
            UpdateDataGrid();
        }

    }
}
