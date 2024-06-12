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
    /// Логика взаимодействия для TaskAdmin.xaml
    /// </summary>
    public partial class TaskAdmin : Page
    {
        ObservableCollection<CurrentGroup> groups;
        ObservableCollection<CurrentIndivid> currentIndivids;
        public TaskAdmin()
        {
            InitializeComponent();
            UpdateDataGrid();
        }
        private void UpdateDataGrid()
        {
            using(var context = new PracticeStudyCenterEntities())
            {
                groups = new ObservableCollection<CurrentGroup>(context.Groups
                    .Select(r => new CurrentGroup
                    {
                        GroupsID = r.GroupsID,
                        NameGroup = r.NameGroup,
                        UserID = (int)r.UserID,
                        CourseID = (int)r.CourseID,
                        UserName = r.Users.FirstName,
                        CourseName = r.Course.Name,
                    }).ToList());

                dtgrGroups.ItemsSource = groups;

                currentIndivids = new ObservableCollection<CurrentIndivid>(context.IndividualLessons
                    .Select(r => new CurrentIndivid
                    {
                        IndividualLessonsID = r.IndividualLessonsID,
                        StudentID = (int)r.StudentID,
                        StudentName = r.Users.FirstName,
                        TeacherID = (int)r.UserID,
                        TeacherName = r.Users.FirstName,
                        CourseID = (int)r.CourseID,
                        CourseName = r.Course.Name
                    }).ToList());

                dtgrIndivid.ItemsSource = currentIndivids;
            }
        }
    }
}
