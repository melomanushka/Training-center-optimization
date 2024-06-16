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
    /// Логика взаимодействия для TaskTeacher.xaml
    /// </summary>
    public partial class TaskTeacher : Page
    {
        ObservableCollection<CurrentSolutions> solutions;
        public TaskTeacher()
        {
            InitializeComponent();
            UpdateDataGrid();
        }
        private void UpdateDataGrid()
        {
            using(var context = new PracticeStudyCenterEntities())
            {
                solutions = new ObservableCollection<CurrentSolutions>(context.Solutions
                    .Where(op => op.TeacherID == CurrentUser.ID)
                    .Select(op => new CurrentSolutions
                    {
                        SolutionID = op.SolutionID,
                        StudentComment = op.StudentComment,
                        TeacherComment = op.TeacherComment,
                        StydentName = op.Student.Users.FirstName,
                        StydentLastName = op.Student.Users.LastName,
                        StydentMiddleName = op.Student.Users.MiddleName,
                        TeacherName = op.Users.FirstName,
                        Grade = (bool)op.Grade,
                        AssignmentName = op.Assignments.Title,
                    }).ToList());

                ItemList.ItemsSource = solutions;
            }
        }
        private void ShowSolution_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                var user = button.CommandParameter as CurrentSolutions;
                if (user != null)
                {
                    using (var context = new PracticeStudyCenterEntities())
                    {
                        CurrentSolutionForTeacher currentSolutionForTeacher = new CurrentSolutionForTeacher(user);
                        currentSolutionForTeacher.Closed += EditWindow_Closed;
                        currentSolutionForTeacher.ShowDialog();
                    }
                }
            }
        }
        private void EditWindow_Closed(object sender, EventArgs e)
        {
            UpdateDataGrid();
        }

    }
}
