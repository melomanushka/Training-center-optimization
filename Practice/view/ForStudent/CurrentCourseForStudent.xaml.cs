using Microsoft.Win32;
using Practice.model;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace Practice.view.ForStudent
{
    /// <summary>
    /// Логика взаимодействия для CurrentCourseForStudent.xaml
    /// </summary>
    public partial class CurrentCourseForStudent : Window
    {
        private CurrentCourse course;
        private byte[] solutionFileContent;

        public CurrentCourseForStudent(CurrentCourse course)
        {
            InitializeComponent();
            this.course = course;
            DataContext = course;
            txblCourse.Text = course.Name;
            LoadAssignments();
        }

        private void LoadAssignments()
        {
            using (var context = new PracticeStudyCenterEntities())
            {
                var assignments = context.Assignments
                    .Where(a => a.CourseID == course.CourseId)
                    .Select(a => new
                    {
                        a.AssignmentID,
                        a.Title
                    }).ToList();

                AssignmentListBox.ItemsSource = assignments;
            }
        }

        private void SelectFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                SolutionFilePathTextBox.Text = openFileDialog.FileName;
                solutionFileContent = File.ReadAllBytes(openFileDialog.FileName);
            }
        }

        private void AddSolution_Click(object sender, RoutedEventArgs e)
        {
            if (AssignmentListBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите задание.");
                return;
            }

            if (solutionFileContent == null)
            {
                MessageBox.Show("Выберите файл решения.");
                return;
            }

            int assignmentId = (int)AssignmentListBox.SelectedValue;
            string studentComment = StudentCommentTextBox.Text;

            using (var context = new PracticeStudyCenterEntities())
            {
                int studentId = context.Student.SingleOrDefault(s => s.UserID == CurrentUser.ID).StudentID;
                int teacherId = (int)context.IndividualLessons
                    .Where(il => il.CourseID == course.CourseId && il.StudentID == studentId)
                    .Select(il => il.UserID)
                    .FirstOrDefault();

                if (teacherId == 0)
                {
                    MessageBox.Show("Преподаватель для данного курса не найден.");
                    return;
                }

                var solution = new Solutions
                {
                    AssignmentID = assignmentId,
                    StudentID = studentId,
                    StudentComment = studentComment,
                    TeacherID = teacherId,
                    SolutionFile = solutionFileContent,
                    TeacherComment = null,
                    Grade = false
                };

                context.Solutions.Add(solution);
                context.SaveChanges();
                MessageBox.Show("Решение добавлено.");
                this.Close();
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
