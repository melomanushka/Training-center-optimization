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

namespace Practice.view.ForAdmin
{
    /// <summary>
    /// Логика взаимодействия для CourseAdmin.xaml
    /// </summary>
    public partial class CourseAdmin : Page
    {
        ObservableCollection<CurrentCourse> courses;
        public CourseAdmin()
        {
            InitializeComponent();
            UpdateDataGrid();
        }
        private void UpdateDataGrid()
        {
            using (var context = new PracticeStudyCenterEntities())
            {
                courses = new ObservableCollection<CurrentCourse>(context.Course
                    .Select(r => new CurrentCourse
                    {
                        CourseId = r.CourseID,
                        Name = r.Name,
                        Description = r.Description,
                        Hours = (int)r.Hours,
                        TypeCourseID = (int)r.TypeCourseID,
                        TypeCourseName = r.TypeCourse.TypeCourseName
                    }).ToList());

                dtgrCourse.ItemsSource = courses;
            }
        }
    }
}
