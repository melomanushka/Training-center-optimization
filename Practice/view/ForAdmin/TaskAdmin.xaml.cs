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
            ComboBoxShowUsers();

            cmbxCourse.ItemsSource = PracticeStudyCenterEntities.GetContext().Course.ToList();
            cmbxGroups.ItemsSource = PracticeStudyCenterEntities.GetContext().Groups.ToList();
        }
        private void ComboBoxShowUsers()
        {
            using (PracticeStudyCenterEntities context = new PracticeStudyCenterEntities())
            {
                var usersList = (from user in context.Users
                                 where user.StatusID == 3
                                 select new UserFullName
                                 {
                                     UserID = user.UserID,
                                     FullName = user.LastName + " " + user.FirstName + " " + user.MiddleName
                                 }).ToList();

                var users = new ObservableCollection<UserFullName>(usersList);

                cmbxStudent.ItemsSource = users;
                cmbxStudent.DisplayMemberPath = "FullName"; // Устанавливаем отображаемое значение
                cmbxStudent.SelectedValuePath = "UserID";  // Устанавливаем значение, возвращаемое при выборе
                
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
            }
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
                        UserName = r.Users.FirstName + " " + r.Users.LastName + " " + r.Users.MiddleName,
                        CourseName = r.Course.Name,
                    }).ToList());

                dtgrGroups.ItemsSource = groups;

                currentIndivids = new ObservableCollection<CurrentIndivid>(context.IndividualLessons
                    .Select(r => new CurrentIndivid
                    {
                        IndividualLessonsID = r.IndividualLessonsID,
                        StudentID = (int)r.StudentID,
                        StudentName = r.Student.Users.FirstName + " " + r.Student.Users.LastName + " " + r.Student.Users.MiddleName,
                        TeacherID = (int)r.UserID,
                        TeacherName = r.Users.FirstName + " " + r.Users.LastName + " " + r.Users.MiddleName,
                        CourseID = (int)r.CourseID,
                        CourseName = r.Course.Name
                    }).ToList());

                dtgrIndivid.ItemsSource = currentIndivids;
            }
        }

        //ПОМИМО ГРУППЫ ДОЛЖНА УДАЛЯТЬ В СТУДЕНТГРУППА ВСЕ СТРОКИ ГДЕ НАХОДИЛАСЬ ЭТА ГРУППА TODO
        private void btnDelGroup(object sender, RoutedEventArgs e)
        {
            CurrentGroup group = (CurrentGroup)dtgrGroups.SelectedItem;
            if (group != null)
            {
                MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить этот элемент?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    using(PracticeStudyCenterEntities context = new PracticeStudyCenterEntities())
                    {
                        var itemToRemove = context.Groups.FirstOrDefault(s => s.GroupsID == group.GroupsID);
                        if (itemToRemove != null)
                        {
                            context.Groups.Remove(itemToRemove);
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
            }
            else
            {
                MessageBox.Show("Выберите элемент для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
            
        private void dtgrGroups_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            CurrentGroup group = ( CurrentGroup)dtgrGroups.SelectedItem;
            if (group != null)
            {
                CurrentGroupEdit currentGroupEdit = new CurrentGroupEdit(group);
                currentGroupEdit.Closed += EditWindow_Closed;
                currentGroupEdit.ShowDialog();
            }
        }
        private void EditWindow_Closed(object sender, EventArgs e)
        {
            UpdateDataGrid();
        }

        private void ButtonDelIndivid_Click(object sender, RoutedEventArgs e)
        {
            CurrentIndivid current = (CurrentIndivid)dtgrIndivid.SelectedItem;
            if (current != null)
            {
                MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить этот элемент?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    using (PracticeStudyCenterEntities context = new PracticeStudyCenterEntities())
                    {
                        var itemToRemove = context.IndividualLessons.FirstOrDefault(d => d.IndividualLessonsID == current.IndividualLessonsID);
                        if (itemToRemove != null)
                        {
                            context.IndividualLessons.Remove(itemToRemove);
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
            }
        }

        private void dtgrIndivid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            CurrentIndivid individ = (CurrentIndivid)dtgrIndivid.SelectedItem;
            if (individ != null)
            {
                CurrentIndividEdit currentIndividEdit = new CurrentIndividEdit(individ);
                currentIndividEdit.Closed += EditWindow_Closed;
                currentIndividEdit.ShowDialog();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new PracticeStudyCenterEntities())
            {
                // Проверяем, выбрана ли группа
                var selectedGroup = cmbxGroups.SelectedItem as Groups;
                var selectedStudent = cmbxStudent.SelectedItem as UserFullName;
                var selectedTeacher = cmbxTeacher.SelectedItem as UserFullName;
                var selectedCourse = cmbxCourse.SelectedItem as Course;

                if (selectedCourse == null || selectedTeacher == null)
                {
                    MessageBox.Show("Пожалуйста, выберите курс и преподавателя.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (selectedGroup != null)
                {
                    // Добавление новой группы
                    Groups newGroup = new Groups
                    {
                        NameGroup = selectedGroup.NameGroup,
                        UserID = selectedTeacher.UserID,
                        CourseID = selectedCourse.CourseID
                    };
                    context.Groups.Add(newGroup);
                    context.SaveChanges();
                    MessageBox.Show("Группа успешно создана");
                    this.TabControl1.SelectedIndex = 0;
                }
                else if (selectedStudent != null)
                {
                    // Проверка, является ли выбранный пользователь студентом
                    var student = context.Student.FirstOrDefault(s => s.UserID == selectedStudent.UserID);
                    if (student == null)
                    {
                        MessageBox.Show("Выбранный пользователь не является студентом.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    // Добавление нового индивидуального занятия
                    IndividualLessons newIndividLesson = new IndividualLessons
                    {
                        StudentID = student.StudentID,
                        UserID = selectedTeacher.UserID,
                        CourseID = selectedCourse.CourseID
                    };
                    context.IndividualLessons.Add(newIndividLesson);
                    context.SaveChanges();
                    MessageBox.Show("Индивидуальное занятие успешно создано");
                    this.TabControl1.SelectedIndex = 1;
                }
                else
                {
                    MessageBox.Show("Пожалуйста, выберите группу или студента для индивидуального занятия.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                UpdateDataGrid();

                // Очистка полей
                cmbxCourse.SelectedItem = null;
                cmbxGroups.SelectedItem = null;
                cmbxStudent.SelectedItem = null;
                cmbxTeacher.SelectedItem = null;
            }
        }

    }
}
