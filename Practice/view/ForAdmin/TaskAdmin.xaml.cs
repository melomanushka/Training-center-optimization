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
                        UserName = r.Users.FirstName,
                        CourseName = r.Course.Name,
                    }).ToList());

                dtgrGroups.ItemsSource = groups;

                currentIndivids = new ObservableCollection<CurrentIndivid>(context.IndividualLessons
                    .Select(r => new CurrentIndivid
                    {
                        IndividualLessonsID = r.IndividualLessonsID,
                        StudentID = (int)r.StudentID,
                        StudentName = r.Student.Users.FirstName,
                        TeacherID = (int)r.UserID,
                        TeacherName = r.Users.FirstName,
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
    }
}
