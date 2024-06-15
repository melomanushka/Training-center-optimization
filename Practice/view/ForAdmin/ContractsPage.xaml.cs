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
    /// Логика взаимодействия для ContractsPage.xaml
    /// </summary>
    public partial class ContractsPage : Page
    {
        ObservableCollection<CurrentEnrollment> enrollments;

        public ContractsPage()
        {
            InitializeComponent();
            UpdateDataGrid();
            ComboBoxShowUsers();

            //cmbxStudent.ItemsSource = PracticeStudyCenterEntities.GetContext().Users.ToList();
            cmbxCourse.ItemsSource = PracticeStudyCenterEntities.GetContext().Course.ToList();
            cmbxPayment.ItemsSource = PracticeStudyCenterEntities.GetContext().PaymentType.ToList();
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
            }
        }
        private void UpdateDataGrid()
        {
            using (var context = new PracticeStudyCenterEntities())
            {
                enrollments = new ObservableCollection<CurrentEnrollment>(context.Contracts
                    .Select(r => new CurrentEnrollment
                    {
                        ContractID = r.ContractID,
                        StarttDate = (DateTime)r.StartDate,
                        EndtDate = (DateTime)r.EndDate,
                        CreatorID = r.CreatorID.HasValue ? r.CreatorID.Value : 0,
                        CreatorName = r.Users.FirstName + " " + r.Users.LastName + " " + r.Users.MiddleName,
                        StudentID = r.Student.UserID.HasValue ? r.Student.UserID.Value : 0,
                        StudentName = r.Student.Users.FirstName + " " + r.Student.Users.LastName + " " + r.Student.Users.MiddleName,
                        CourseID = r.CourseID.HasValue ? r.CourseID.Value : 0,
                        CourseName = r.Course.Name,
                        PaymentTypeID = r.PaymentTypeID.HasValue ? r.PaymentTypeID.Value : 0,
                        PaymentTypeName = r.PaymentType.PaymentTypeName,
                        TotalCost = (int)r.TotalCost,
                        AmountPaid = (int)r.AmountPaid,
                        ContractDate = (DateTime)r.ContractDate,
                    }).ToList());

                dtgrEnrollment.ItemsSource = enrollments;
            }
        }

        private void TabItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            CurrentEnrollment enrollment = (CurrentEnrollment)dtgrEnrollment.SelectedItem;
            if (enrollment != null)
            {
                CurrentContractEdit currentContractEdit = new CurrentContractEdit(enrollment);
                currentContractEdit.Closed += EditWindow_Closed;
                currentContractEdit.ShowDialog();
            }
        }
        private void EditWindow_Closed(object sender, EventArgs e)
        {
            UpdateDataGrid();
        }
        private void ButtonADDContract_Click(object sender, RoutedEventArgs e)
        {
            using (PracticeStudyCenterEntities context = new PracticeStudyCenterEntities())
            {
                try
                {
                    // Преобразование значений текстовых полей в decimal
                    decimal totalCost;
                    decimal amountPaid;

                    if (!decimal.TryParse(txbTotal.Text, out totalCost))
                    {
                        MessageBox.Show("Неверный формат общей стоимости.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    if (!decimal.TryParse(txbAmount.Text, out amountPaid))
                    {
                        MessageBox.Show("Неверный формат суммы оплаты.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    // Получаем выбранный UserID студента
                    int selectedUserId = (int)cmbxStudent.SelectedValue;

                    // Ищем StudentID по UserID
                    var student = context.Student.FirstOrDefault(s => s.UserID == selectedUserId);
                    if (student == null)
                    {
                        MessageBox.Show("Не удалось найти студента.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    Contracts contracts = new Contracts()
                    {
                        ContractDate = ContractDate.SelectedDate,
                        CourseID = cmbxCourse.SelectedIndex + 1,
                        CreatorID = CurrentUser.ID,
                        StudentID = student.StudentID, // Используем найденный StudentID
                        PaymentTypeID = cmbxPayment.SelectedIndex + 1,
                        TotalCost = totalCost,
                        AmountPaid = amountPaid,
                        StartDate = StartDate.SelectedDate,
                        EndDate = EndDate.SelectedDate,
                    };

                    context.Contracts.Add(contracts);
                    context.SaveChanges();
                    MessageBox.Show("Контракт успешно добавлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.TabControl.SelectedIndex = 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Произошла ошибка при добавлении контракта: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                ClearFormFields();
                UpdateDataGrid();
            }
        }
        private void ClearFormFields()
        {
            // Очистка ComboBox
            cmbxStudent.SelectedIndex = -1;
            cmbxCourse.SelectedIndex = -1;
            cmbxPayment.SelectedIndex = -1;

            // Очистка TextBox
            txbTotal.Clear();
            txbAmount.Clear();

            // Очистка DatePicker
            ContractDate.SelectedDate = null;
            StartDate.SelectedDate = null;
            EndDate.SelectedDate = null;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CurrentEnrollment currentEnrollment = (CurrentEnrollment)dtgrEnrollment.SelectedItem;
            if (currentEnrollment != null)
            {
                MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить этот элемент?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    using(PracticeStudyCenterEntities context = new PracticeStudyCenterEntities())
                    {
                        var itemToRemove = context.Contracts.FirstOrDefault(r => r.ContractID == currentEnrollment.ContractID);
                        if (itemToRemove != null)
                        {
                            context.Contracts.Remove(itemToRemove);
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

            }
        }
    }
}
