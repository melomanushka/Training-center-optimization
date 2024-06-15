using Practice.model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
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
    /// Логика взаимодействия для CurrentContractEdit.xaml
    /// </summary>
    public partial class CurrentContractEdit : Window
    {
        int idStudent = 0;
        int idCreator = 0;
        int idContract = 0;
        public CurrentContractEdit(CurrentEnrollment enrollment)
        {
            InitializeComponent();
            idStudent = enrollment.StudentID;
            idCreator = enrollment.CreatorID;
            idContract = enrollment.ContractID;

            ComboBoxShowUsers();

            datePicker.SelectedDate = enrollment.ContractDate;
            Startdate.SelectedDate = enrollment.StarttDate;
            Enddate.SelectedDate = enrollment.EndtDate;
            txbAmount.Text = enrollment.AmountPaid.ToString();
            txbTotal.Text = enrollment.TotalCost.ToString();

            cmbxCourse.ItemsSource = PracticeStudyCenterEntities.GetContext().Course.ToList();
            cmbxCourse.SelectedItem = PracticeStudyCenterEntities.GetContext().Course.FirstOrDefault(r => r.CourseID == enrollment.CourseID);

            cmbxType.ItemsSource = PracticeStudyCenterEntities.GetContext().PaymentType.ToList();
            cmbxType.SelectedItem = PracticeStudyCenterEntities.GetContext().PaymentType.FirstOrDefault(s => s.PaymentTypeID == enrollment.PaymentTypeID);
        }
        private void ComboBoxShowUsers()
        {
            using (var context = new PracticeStudyCenterEntities())
            {
                var students = context.Users
                    .Where(user => user.StatusID == 3)
                    .Select(user => new UserFullName
                    {
                        UserID = user.UserID,
                        FullName = user.LastName + " " + user.FirstName + " " + user.MiddleName
                    }).ToList();
                var userStudents = new ObservableCollection<UserFullName>(students);

                var creators = context.Users
                    .Where(user => user.StatusID == 1)
                    .Select(user => new UserFullName
                    {
                        UserID = user.UserID,
                        FullName = user.LastName + " " + user.FirstName + " " + user.MiddleName
                    }).ToList();
                var usersCreator = new ObservableCollection<UserFullName>(creators);

                cmbxCreator.ItemsSource = usersCreator;
                cmbxCreator.DisplayMemberPath = "FullName";
                cmbxCreator.SelectedValuePath = "UserID";

                var currentTeacher = usersCreator.FirstOrDefault(t => t.UserID == idCreator);
                if (currentTeacher != null)
                {
                    cmbxCreator.SelectedItem = currentTeacher;
                }

                cmbxStudent.ItemsSource = userStudents;
                cmbxStudent.DisplayMemberPath = "FullName";
                cmbxStudent.SelectedValuePath = "UserID";

                var currentStudent = userStudents.FirstOrDefault(s => s.UserID == idStudent);
                if (currentStudent != null)
                {
                    cmbxStudent.SelectedItem = currentStudent;
                }
            }
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
                using (var context = new PracticeStudyCenterEntities())
                {
                // Получаем выбранный UserID студента
                int selectedUserId = (int)cmbxStudent.SelectedValue;

                // Ищем StudentID по UserID
                var student = context.Student.FirstOrDefault(s => s.UserID == selectedUserId);
                if (student == null)
                {
                    MessageBox.Show("Не удалось найти студента для обновления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                int currentCourse = cmbxCourse.SelectedIndex + 1;
                int currentType = cmbxType.SelectedIndex + 1;
                    var contract = context.Contracts.FirstOrDefault(c => c.ContractID == idContract);
                    if (contract != null)
                    {
                    contract.StudentID = student.StudentID;
                    contract.CreatorID = (int)cmbxCreator.SelectedValue;
                        contract.CourseID = currentCourse;
                        contract.PaymentTypeID = currentType;
                        contract.StartDate = Startdate.SelectedDate;
                        contract.EndDate = Enddate.SelectedDate;
                        contract.TotalCost = decimal.Parse(txbTotal.Text);
                        contract.AmountPaid = decimal.Parse(txbAmount.Text);
                        contract.ContractDate = datePicker.SelectedDate;

                        context.SaveChanges();
                        MessageBox.Show("Данные успешно сохранены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                   this.Close();
                }
                    else
                    {
                        MessageBox.Show("Не удалось найти договор для обновления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
    }
}
