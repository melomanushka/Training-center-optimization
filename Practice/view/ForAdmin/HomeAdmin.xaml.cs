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
                    .Where(r => r.StatusID == 2)
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
    }
}
