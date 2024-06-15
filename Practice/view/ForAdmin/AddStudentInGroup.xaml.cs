using Practice.model;
using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для AddStudentInGroup.xaml
    /// </summary>
    public partial class AddStudentInGroup : Window
    {
        private int groupId;

        public AddStudentInGroup(int groupId)
        {
            InitializeComponent();
            this.groupId = groupId;
            LoadStudents();
        }

        private void LoadStudents()
        {
            using (PracticeStudyCenterEntities context = new PracticeStudyCenterEntities())
            {
                var studentsNotInGroup = (from student in context.Student
                                          join user in context.Users on student.UserID equals user.UserID
                                          where !(from sg in context.StudentGroup
                                                  where sg.GroupID == groupId
                                                  select sg.StudentID).Contains(student.StudentID)
                                          select new UserFullName
                                          {
                                              UserID = student.StudentID,
                                              FullName = user.LastName + " " + user.FirstName + " " + user.MiddleName
                                          }).ToList();

                cmbxSt.ItemsSource = studentsNotInGroup;
                cmbxSt.DisplayMemberPath = "FullName";
                cmbxSt.SelectedValuePath = "UserID";
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void Button_ClickClose(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var selectedStudentId = (int)cmbxSt.SelectedValue;
            if (selectedStudentId != 0)
            {
                using (PracticeStudyCenterEntities context = new PracticeStudyCenterEntities())
                {
                    var studentGroup = new StudentGroup
                    {
                        StudentID = selectedStudentId,
                        GroupID = groupId
                    };

                    context.StudentGroup.Add(studentGroup);
                    context.SaveChanges();
                    MessageBox.Show("Студент успешно добавлен в группу.");
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Выберите студента из списка.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
