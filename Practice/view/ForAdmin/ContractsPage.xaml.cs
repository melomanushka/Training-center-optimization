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
            cmbxStudent.ItemsSource = PracticeStudyCenterEntities.GetContext().Users.ToList();
            cmbxCourse.ItemsSource = PracticeStudyCenterEntities.GetContext().Course.ToList();
            cmbxPayment.ItemsSource = PracticeStudyCenterEntities.GetContext().PaymentType.ToList();
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
                        EndtDate = (DateTime)r.StartDate,
                        CreatorID = (int)r.CreatorID,
                        CreatorName = r.Users.FirstName,
                        StudentID = (int)r.Users.UserID,
                        StudentName = r.Users.FirstName,
                        CourseID = (int)r.CourseID,
                        CourseName = r.Course.Name,
                        PaymentTypeID = (int)r.PaymentTypeID,
                        PaymentTypeName = r.PaymentType.PaymentTypeName,
                        TotalCost = (int)r.TotalCost,
                        AmountPaid = (int)r.AmountPaid,
                        ContractDate = (DateTime)r.ContractDate,
                    }).ToList());

                dtgrEnrollment.ItemsSource = enrollments;
            }
        }
    }
}
