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
    /// Логика взаимодействия для HomeAdmin.xaml
    /// </summary>
    public partial class HomeAdmin : Page
    {
        ObservableCollection<CurrentUserForTable> teachers;
        ObservableCollection<CurrentUserForTable> students;
        public HomeAdmin()
        {
            InitializeComponent();
            UpdateDataGrid();
            cmbxStatus.ItemsSource = PracticeStudyCenterEntities.GetContext().UserStatus.ToList();
        }
        private void UpdateDataGrid()
        {
            using (var context = new PracticeStudyCenterEntities())
            {
                teachers = new ObservableCollection<CurrentUserForTable>(context.Users
                    .Where(r => r.StatusID == 2 || r.StatusID == 1)
                    .Select(r => new CurrentUserForTable
                    {
                        ID = r.UserID,
                        FirstName = r.FirstName,
                        LastName = r.LastName,
                        MiddleName = r.MiddleName,
                        Login = r.Login,
                        Password = r.Password,
                        Email = r.Email,
                        PhoneNumber = r.PhoneNumber,
                        StatusName = r.UserStatus.StatusUserName,

                    }).ToList());

                dtgrTeacher.ItemsSource = teachers;

                students = new ObservableCollection<CurrentUserForTable>(context.Student
                    .Select(s => new CurrentUserForTable
                    {
                        ID = s.StudentID,
                        FirstName = s.Users.FirstName,
                        LastName = s.Users.LastName,
                        MiddleName = s.Users.MiddleName,
                        Login = s.Users.Login,
                        Password = s.Users.Password,
                        Email = s.Users.Email,
                        PassportNumber = s.PassportNumber,
                        PassportIssueDate = (DateTime)s.PassportIssueDate,
                        PassportIssuedBy = s.PassportIssuedBy,
                        PassportSeries = s.PassportSeries,
                        PhoneNumber = s.Users.PhoneNumber
                    }).ToList());

                dtgrStudent.ItemsSource = students;
            }
        }

        private void btnAddUser(object sender, RoutedEventArgs e)
        {
            using (var context = new PracticeStudyCenterEntities())
            {
                int status = cmbxStatus.SelectedIndex + 1;

                if (status == 1)
                {
                    Users users = new Users()
                    {
                        FirstName = txbName.Text,
                        LastName = txbLastName.Text,
                        MiddleName = txbMiddleName.Text,
                        Login = txbLogin.Text,
                        Password = txbPassword.Text,
                        Email = txbEmail.Text,
                        PhoneNumber = txbPhone.Text,
                        StatusID = status,
                        CompanyID = 1
                    };
                    context.Users.Add(users);
                    context.SaveChanges();

                    MessageBox.Show("Данные успешно сохранены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    
                    this.tbc2.SelectedIndex = 0;
                    
                    txbName.Text = string.Empty;
                    txbLastName.Text = string.Empty;
                    txbMiddleName.Text = string.Empty;
                    txbLogin.Text = string.Empty;
                    txbPassword.Text = string.Empty;
                    txbEmail.Text = string.Empty;
                    txbPhone.Text = string.Empty;
                    txbPassSer.Text = string.Empty;
                    txbPassWhere.Text = string.Empty;
                    txbPassNum.Text = string.Empty;
                }
                else if (status == 2)
                {
                    Users users = new Users()
                    {
                        FirstName = txbName.Text,
                        LastName = txbLastName.Text,
                        MiddleName = txbMiddleName.Text,
                        Login = txbLogin.Text,
                        Password = txbPassword.Text,
                        Email = txbEmail.Text,
                        PhoneNumber = txbPhone.Text,
                        StatusID = status,
                    };
                    context.Users.Add(users);
                    context.SaveChanges();
                    MessageBox.Show("Данные успешно сохранены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                    this.tbc2.SelectedIndex = 0;

                    txbName.Text = string.Empty;
                    txbLastName.Text = string.Empty;
                    txbMiddleName.Text = string.Empty;
                    txbLogin.Text = string.Empty;
                    txbPassword.Text = string.Empty;
                    txbEmail.Text = string.Empty;
                    txbPhone.Text = string.Empty;
                    txbPassSer.Text = string.Empty;
                    txbPassWhere.Text = string.Empty;
                    txbPassNum.Text = string.Empty;
                }
                else if (status == 3)
                {
                    Users users = new Users()
                    {
                        FirstName = txbName.Text,
                        LastName = txbLastName.Text,
                        MiddleName = txbMiddleName.Text,
                        Login = txbLogin.Text,
                        Password = txbPassword.Text,
                        Email = txbEmail.Text,
                        PhoneNumber = txbPhone.Text,
                        StatusID = status,
                    };
                    context.Users.Add(users);

                    Student student = new Student()
                    {
                        UserID = users.UserID,
                        PassportNumber = txbPassNum.Text,
                        PassportSeries = txbPassSer.Text,
                        PassportIssueDate = Date.SelectedDate.Value,
                        PassportIssuedBy = txbPassWhere.Text
                    };

                    context.Student.Add(student);
                    context.SaveChanges();

                    MessageBox.Show("Данные успешно сохранены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                    this.tbc2.SelectedIndex = 1;

                    txbName.Text = string.Empty;
                    txbLastName.Text = string.Empty;
                    txbMiddleName.Text = string.Empty;
                    txbLogin.Text = string.Empty;
                    txbPassword.Text = string.Empty;
                    txbEmail.Text = string.Empty;
                    txbPhone.Text = string.Empty;
                    txbPassSer.Text = string.Empty;
                    txbPassWhere.Text = string.Empty;
                    txbPassNum.Text = string.Empty;
                }
                else
                {
                    MessageBox.Show("Выберите статус пользователя.");
                }

                UpdateDataGrid();
            }
        }

        private void btnDelTeacher(object sender, RoutedEventArgs e)
        {
            CurrentUserForTable selectedItem = (CurrentUserForTable)dtgrTeacher.SelectedItem;

            if (selectedItem != null)
            {
                MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить этот элемент?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    using (PracticeStudyCenterEntities context = new PracticeStudyCenterEntities())
                    {
                        var itemToRemove = context.Users.FirstOrDefault(s => s.UserID == selectedItem.ID);

                        if (itemToRemove != null)
                        {
                            context.Users.Remove(itemToRemove);

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

        private void btnDelStudent(object sender, RoutedEventArgs e)
        {
            CurrentUserForTable selectedItem = (CurrentUserForTable)dtgrStudent.SelectedItem;

            if (selectedItem != null)
            {
                MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить этот элемент?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    using (PracticeStudyCenterEntities context = new PracticeStudyCenterEntities())
                    {
                        var itemToRemove = context.Users.FirstOrDefault(s => s.UserID == selectedItem.ID);
                        var itemToRemove2 = context.Student.FirstOrDefault(r => r.UserID == selectedItem.ID);

                        if (itemToRemove != null)
                        {
                            context.Users.Remove(itemToRemove);
                            context.Student.Remove(itemToRemove2);

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

        private void dtgrTeacher_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            CurrentUserForTable currentUser = (CurrentUserForTable)dtgrTeacher.SelectedItem;
            if (currentUser != null)
            {
                CurrentUserEdit currentUserEdit =new CurrentUserEdit(currentUser);
                currentUserEdit.Closed += EditWindow_Closed;
                currentUserEdit.ShowDialog();
            }
        }
        private void EditWindow_Closed(object sender, EventArgs e)
        {
            UpdateDataGrid();
        }

        private void dtgrStudent_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            CurrentUserForTable currentUser = (CurrentUserForTable)dtgrStudent.SelectedItem;
            if (currentUser != null)
            {
                CurrentUserStudentEdit currentUserEdit = new CurrentUserStudentEdit(currentUser);
                currentUserEdit.Closed += EditWindow_Closed;
                currentUserEdit.ShowDialog();
            }
        }
    }
}
