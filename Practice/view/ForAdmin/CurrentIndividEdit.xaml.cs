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

namespace Practice.view.ForAdmin
{
    /// <summary>
    /// Логика взаимодействия для CurrentIndividEdit.xaml
    /// </summary>
    public partial class CurrentIndividEdit : Window
    {
        int idStudent = 0;
        int idTeacher = 0;
        int individID = 0;

        public CurrentIndividEdit(CurrentIndivid individ)
        {
            InitializeComponent();
            idStudent = individ.StudentID;
            idTeacher = individ.TeacherID;
            individID = individ.IndividualLessonsID;
            
            txblNumber.Text = individ.IndividualLessonsID.ToString();

            LoadData(individ);
        }
        private void LoadData(CurrentIndivid individ)
        {
            ComboBoxShowUsers();

            using (var context = new PracticeStudyCenterEntities())
            {
                cmbxCourse.ItemsSource = context.Course.ToList();
                cmbxCourse.SelectedItem = context.Course.FirstOrDefault(s => s.CourseID == individ.CourseID);
            }
        }
        private void ComboBoxShowUsers()
        {
            using (PracticeStudyCenterEntities context = new PracticeStudyCenterEntities())
            {
                // Получение UserID студента
                var studentUserID = context.Student
                    .Where(s => s.StudentID == idStudent)
                    .Select(s => s.UserID)
                    .FirstOrDefault();

                // Получение информации о студентах
                var studentsList = (from user in context.Users
                                    where user.StatusID == 3
                                    select new UserFullName
                                    {
                                        UserID = user.UserID,
                                        FullName = user.LastName + " " + user.FirstName + " " + user.MiddleName
                                    }).ToList();

                var students = new ObservableCollection<UserFullName>(studentsList);

                // Заполнение ComboBox для студентов
                cmbxStudent.ItemsSource = students;
                cmbxStudent.DisplayMemberPath = "FullName";
                cmbxStudent.SelectedValuePath = "UserID";

                // Установка текущего студента
                var currentStudent = students.FirstOrDefault(d => d.UserID == studentUserID);
                if (currentStudent != null)
                {
                    cmbxStudent.SelectedItem = currentStudent;
                }

                // Получение информации о преподавателях
                var teachersList = (from user in context.Users
                                    where user.StatusID == 2
                                    select new UserFullName
                                    {
                                        UserID = user.UserID,
                                        FullName = user.LastName + " " + user.FirstName + " " + user.MiddleName
                                    }).ToList();

                var teachers = new ObservableCollection<UserFullName>(teachersList);

                // Заполнение ComboBox для преподавателей
                cmbxTeacher.ItemsSource = teachers;
                cmbxTeacher.DisplayMemberPath = "FullName";
                cmbxTeacher.SelectedValuePath = "UserID";

                // Установка текущего преподавателя
                var currentTeacher = teachers.FirstOrDefault(t => t.UserID == idTeacher);
                if (currentTeacher != null)
                {
                    cmbxTeacher.SelectedItem = currentTeacher;
                }
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            using (var context = new PracticeStudyCenterEntities())
            {
                // Найти текущее занятие по individID
                var currentIndivid = context.IndividualLessons.FirstOrDefault(il => il.IndividualLessonsID == individID);
                if (currentIndivid != null)
                {
                    // Обновить значения на основе выбранных значений в ComboBox
                    currentIndivid.StudentID = (int)cmbxStudent.SelectedValue;
                    currentIndivid.UserID = (int)cmbxTeacher.SelectedValue;
                    currentIndivid.CourseID = (int)cmbxCourse.SelectedValue;

                    // Сохранить изменения в базе данных
                    context.SaveChanges();
                }
            }
        }
    }
}
