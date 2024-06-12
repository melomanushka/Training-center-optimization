using LiveCharts;
using LiveCharts.Wpf;
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

namespace Practice.view.ForStudent
{
    public partial class HomeStudent : Page
    {
        public ObservableCollection<Series> SeriesCollection { get; set; }

        public HomeStudent()
        {
            InitializeComponent();

            // Инициализация коллекции перед вызовом метода LoadData()
            SeriesCollection = new ObservableCollection<Series>();

            DataContext = this;
            LoadData();
        }

        private void LoadData()
        {
            using (var context = new PracticeStudyCenterEntities())
            {
                var student = context.Student.SingleOrDefault(s => s.UserID == CurrentUser.ID);
                if (student == null)
                {
                    MessageBox.Show("Студент не найден");
                    return;
                }

                int studentId = student.StudentID;

                // Получаем количество выполненных заданий через индивидуальные занятия
                var completedIndividualAssignments = context.Solutions
                    .Count(s => s.StudentID == studentId && s.Grade == true);

                // Получаем количество заданий через индивидуальные занятия
                var allIndividualAssignments = context.IndividualLessons
                    .Where(il => il.StudentID == studentId)
                    .SelectMany(il => il.Course.Assignments)
                    .Count();

                // Получаем количество невыполненных заданий через индивидуальные занятия
                var incompleteIndividualAssignments = allIndividualAssignments - completedIndividualAssignments;

                // Получаем количество выполненных заданий через групповые занятия
                var completedGroupAssignments = context.Solutions
                    .Count(s => s.StudentID == studentId && s.Grade == true);

                // Получаем количество заданий через групповые занятия
                var allGroupAssignments = context.StudentGroup
                    .Where(sg => sg.StudentID == studentId)
                    .SelectMany(sg => sg.Groups.Course.Assignments)
                    .Count();

                // Получаем количество невыполненных заданий через групповые занятия
                var incompleteGroupAssignments = allGroupAssignments - completedGroupAssignments;

                // Очищаем коллекцию перед добавлением новых значений
                SeriesCollection.Clear();

                // Добавляем данные о выполненных заданиях через индивидуальные занятия
                SeriesCollection.Add(new PieSeries
                {
                    Title = "Выполненные задания (индивидуальные)",
                    Values = new ChartValues<int> { completedIndividualAssignments },
                    DataLabels = true
                });

                // Добавляем данные о невыполненных заданиях через индивидуальные занятия
                SeriesCollection.Add(new PieSeries
                {
                    Title = "Невыполненные задания (индивидуальные)",
                    Values = new ChartValues<int> { incompleteIndividualAssignments },
                    DataLabels = true
                });

                // Добавляем данные о выполненных заданиях через групповые занятия
                SeriesCollection.Add(new PieSeries
                {
                    Title = "Выполненные задания (групповые)",
                    Values = new ChartValues<int> { completedGroupAssignments },
                    DataLabels = true
                });

                // Добавляем данные о невыполненных заданиях через групповые занятия
                SeriesCollection.Add(new PieSeries
                {
                    Title = "Невыполненные задания (групповые)",
                    Values = new ChartValues<int> { incompleteGroupAssignments },
                    DataLabels = true
                });
            }
        }

    }
}
