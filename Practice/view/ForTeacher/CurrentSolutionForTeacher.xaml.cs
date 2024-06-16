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
    /// Логика взаимодействия для CurrentSolutionForTeacher.xaml
    /// </summary>
    public partial class CurrentSolutionForTeacher : Window
    {
        private CurrentSolutions solution;
        public CurrentSolutionForTeacher(CurrentSolutions solution)
        {
            InitializeComponent();
            this.solution = solution;
            DataContext = solution;
            StatusComboBox.SelectedItem = solution.Grade ? StatusComboBox.Items[0] : StatusComboBox.Items[1];
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
        private void ViewFile_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new PracticeStudyCenterEntities())
            {
                var currentSolution = context.Solutions.FirstOrDefault(s => s.SolutionID == solution.SolutionID);
                if (currentSolution != null && currentSolution.SolutionFile != null)
                {
                    string tempFilePath = System.IO.Path.GetTempFileName();
                    File.WriteAllBytes(tempFilePath, currentSolution.SolutionFile);
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(tempFilePath) { UseShellExecute = true });
                }
                else
                {
                    MessageBox.Show("Файл не найден.");
                }
            }
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new PracticeStudyCenterEntities())
            {
                var currentSolution = context.Solutions.FirstOrDefault(s => s.SolutionID == solution.SolutionID);
                if (currentSolution != null)
                {
                    currentSolution.TeacherComment = TeacherCommentTextBox.Text;
                    currentSolution.Grade = (StatusComboBox.SelectedItem as ComboBoxItem).Content.ToString() == "Сделано";
                    context.SaveChanges();
                    MessageBox.Show("Изменения сохранены.");
                    this.Close();
                }
            }
        }
    }
}
