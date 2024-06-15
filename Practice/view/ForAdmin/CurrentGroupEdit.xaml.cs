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
    /// Логика взаимодействия для CurrentGroupEdit.xaml
    /// </summary>
    public partial class CurrentGroupEdit : Window
    {
        ObservableCollection<CurrentUserForTable> students;
        int idGroup = 0;
        int idTeacher = 0;
        public CurrentGroupEdit(CurrentGroup group)
        {
            InitializeComponent();

            idGroup = group.GroupsID;
            idTeacher = group.UserID;

            UpdateDataGrid();
            ComboBoxShowUsers();

            txbName.Text = group.NameGroup;

            cmbxCourse.ItemsSource = PracticeStudyCenterEntities.GetContext().Course.ToList();
            cmbxCourse.SelectedItem = PracticeStudyCenterEntities.GetContext().Course.FirstOrDefault(r => r.CourseID == group.CourseID);

        }
        private void ComboBoxShowUsers()
        {
            using (PracticeStudyCenterEntities context = new PracticeStudyCenterEntities())
            {
                var usersListTeacher = (from user in context.Users
                                        where user.StatusID == 2
                                        select new UserFullName
                                        {
                                            UserID = user.UserID,
                                            FullName = user.LastName + " " + user.FirstName + " " + user.MiddleName
                                        }).ToList();

                var usersTeacher = new ObservableCollection<UserFullName>(usersListTeacher);

                cmbxTeacher.ItemsSource = usersTeacher;
                cmbxTeacher.DisplayMemberPath = "FullName"; // Устанавливаем отображаемое значение
                cmbxTeacher.SelectedValuePath = "UserID";  // Устанавливаем значение, возвращаемое при выборе

                // Выбираем текущего преподавателя
                var currentTeacher = usersTeacher.FirstOrDefault(t => t.UserID == idTeacher);
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
        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void UpdateDataGrid()
        {
            using (PracticeStudyCenterEntities context = new PracticeStudyCenterEntities())
            {
                students = new ObservableCollection<CurrentUserForTable>(context.StudentGroup
                    .Where(r => r.GroupID == idGroup)
                    .Select(r => new CurrentUserForTable
                    {
                        ID = (int)r.StudentID,
                        FirstName = r.Student.Users.FirstName,
                        LastName = r.Student.Users.LastName,
                        MiddleName = r.Student.Users.MiddleName,
                        Password = r.Student.Users.Password,
                        Email = r.Student.Users.Email,
                        PhoneNumber = r.Student.Users.PhoneNumber,
                        PassportIssueDate = (DateTime)r.Student.PassportIssueDate,
                        PassportSeries = r.Student.PassportSeries,
                        PassportNumber = r.Student.PassportNumber,
                        PassportIssuedBy = r.Student.PassportIssuedBy,
                    }).ToList());

                dtgrStudent.ItemsSource = students;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            CurrentUserForTable student = (CurrentUserForTable)dtgrStudent.SelectedItem;
            if (student != null)
            {
                MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить этот элемент?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    using (PracticeStudyCenterEntities context = new PracticeStudyCenterEntities())
                    {
                        var itemToremove = context.StudentGroup.FirstOrDefault(s => s.StudentID == student.ID || s.GroupID == idGroup);
                        if (itemToremove != null)
                        {
                            context.StudentGroup.Remove(itemToremove);
                            context.SaveChanges();
                            MessageBox.Show("Данные успешно удалены");

                            UpdateDataGrid();
                        }
                        else
                        {
                            MessageBox.Show("Элемент не найден.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Выберите элемент для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int id = idGroup;
            if (id != 0)
            {
                AddStudentInGroup studentInGroup = new AddStudentInGroup(id);
                studentInGroup.Closed += EditWindow_Closed; // изменено на метод как делегат
                studentInGroup.ShowDialog();
            }
            else
            {
                MessageBox.Show("Что-то он не нашёл нужную группу");
            }
        }
        private void EditWindow_Closed(object sender, EventArgs e)
        {
            UpdateDataGrid();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            using (PracticeStudyCenterEntities context = new PracticeStudyCenterEntities())
            {
                var group = context.Groups.FirstOrDefault(g => g.GroupsID == idGroup);
                if (group != null)
                {
                    group.NameGroup = txbName.Text;
                    group.UserID = (int)cmbxTeacher.SelectedValue;
                    group.CourseID = ((Course)cmbxCourse.SelectedItem).CourseID;

                    context.SaveChanges();
                    MessageBox.Show("Данные успешно обновлены.");
                }
                else
                {
                    MessageBox.Show("Группа не найдена.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
