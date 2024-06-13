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
    /// Логика взаимодействия для CourseAdmin.xaml
    /// </summary>
    public partial class CourseAdmin : Page
    {
        ObservableCollection<CurrentCourse> courses;
        public CourseAdmin()
        {
            InitializeComponent();
            cmbxType.ItemsSource = PracticeStudyCenterEntities.GetContext().TypeCourse.ToList();
            UpdateDataGrid();
        }
        private void UpdateDataGrid()
        {
            using (var context = new PracticeStudyCenterEntities())
            {
                courses = new ObservableCollection<CurrentCourse>(context.Course
                    .Select(r => new CurrentCourse
                    {
                        CourseId = r.CourseID,
                        Name = r.Name,
                        Description = r.Description,
                        Hours = (int)r.Hours,
                        TypeCourseID = (int)r.TypeCourseID,
                        TypeCourseName = r.TypeCourse.TypeCourseName
                    }).ToList());

                dtgrCourse.ItemsSource = courses;
            }
        }

        private void dtnDelCourse(object sender, RoutedEventArgs e)
        {
            CurrentCourse currentCourse = (CurrentCourse)dtgrCourse.SelectedItem;
            if (currentCourse != null)
            {
                MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить этот элемент?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    using (PracticeStudyCenterEntities context = new PracticeStudyCenterEntities())
                    {
                        var itemToRemove = context.Course.FirstOrDefault(s => s.CourseID == currentCourse.CourseId);

                        if (itemToRemove != null)
                        {
                            context.Course.Remove(itemToRemove);

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
                MessageBox.Show("Элемент не найден.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnAddCourse(object sender, RoutedEventArgs e)
        {
            using(var context = new PracticeStudyCenterEntities())
            {
                int type = cmbxType.SelectedIndex + 1;
                Course course = new Course()
                {
                    Name = txbName.Text,
                    Description = txbDesc.Text,
                    Hours = int.Parse(txbNum.Text),
                    TypeCourseID = type,
                };

                context.Course.Add(course);
                context.SaveChanges();
                UpdateDataGrid();
                MessageBox.Show("Данные успешно сохранены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                this.TabControl1.SelectedIndex = 0;

                txbName.Text = string.Empty;
                txbDesc.Text = string.Empty;
                txbNum.Text = string.Empty;
            }
        }
    }
}
