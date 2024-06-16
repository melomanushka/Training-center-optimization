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

namespace Practice.view.ForTeacher
{
    /// <summary>
    /// Логика взаимодействия для CurrentCourseForTeacher.xaml
    /// </summary>
    public partial class CurrentCourseForTeacher : Window
    {
        int id = 0;
        byte[] currentFile = null;
        public CurrentCourseForTeacher(CurrentCourse course)
        {
            InitializeComponent();
            txblCourse.Text = course.Name;
            id = course.CourseId;
            UpdateDataGrid();
        }
        private void UpdateDataGrid()
        {
            using (PracticeStudyCenterEntities context = new PracticeStudyCenterEntities())
            {
                var tasks = context.Assignments
                    .Where(r => r.CourseID == id)
                    .Select(r => new CurrentAssigment
                    {
                        AssignmentID = r.AssignmentID,
                        Title = r.Title,
                        CourseID = (int)r.CourseID,
                    }).ToList();

                TaskListBox.ItemsSource = tasks;
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

        private void ButtonFindFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                currentFile = File.ReadAllBytes(openFileDialog.FileName);
                txblStatus.Text = "*Файл выбран*";
            }
        }

        private void ButtonAddFile_Click(object sender, RoutedEventArgs e)
        {
            using (PracticeStudyCenterEntities context = new PracticeStudyCenterEntities())
            {
                Assignments assignments = new Assignments()
                {
                    Title = txbTitle.Text,
                    FileContent = currentFile,
                    CourseID = id,
                };
                context.Assignments.Add(assignments);
                context.SaveChanges();
            }
            UpdateDataGrid();
            MessageBox.Show("Задание добавлено!");
        }

        private void ButtonOpenFile_Click(object sender, RoutedEventArgs e)
        {
            var file = (CurrentAssigment)TaskListBox.SelectedItem;
            if (file != null)
            {
                using(PracticeStudyCenterEntities context = new PracticeStudyCenterEntities())
                {
                    var currentFile = context.Assignments.FirstOrDefault(r => r.AssignmentID == file.AssignmentID);
                    if (currentFile != null && currentFile.FileContent != null)
                    {
                        string tempFilePath = System.IO.Path.GetTempFileName();
                        File.WriteAllBytes(tempFilePath, currentFile.FileContent);
                        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(tempFilePath) { UseShellExecute = true });
                    }
                    else
                    {
                        MessageBox.Show("Файл не найден.");
                    }
                }
            }
        }
        private void ButtonEditFile_Click(object sender, RoutedEventArgs e)
        {
            var selectedAssignment = (CurrentAssigment)TaskListBox.SelectedItem;
            if (selectedAssignment != null)
            {
                using (PracticeStudyCenterEntities context = new PracticeStudyCenterEntities())
                {
                    var assignmentToUpdate = context.Assignments.FirstOrDefault(r => r.AssignmentID == selectedAssignment.AssignmentID);
                    if (assignmentToUpdate != null)
                    {
                        assignmentToUpdate.Title = txbTitle.Text;

                        if (currentFile != null)
                        {
                            assignmentToUpdate.FileContent = currentFile;
                        }

                        context.SaveChanges();
                        MessageBox.Show("Задание обновлено!");
                        UpdateDataGrid();
                    }
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите задание для редактирования.");
            }
        }

        private void TaskListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedAssignment = (CurrentAssigment)TaskListBox.SelectedItem;
            if (selectedAssignment != null)
            {
                txbTitle.Text = selectedAssignment.Title;
                currentFile = null; // Reset current file to ensure correct file upload
                txblStatus.Text = "";
            }
        }

    }
}
