using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice.model
{
    public class CurrentIndivid
    {
        public int IndividualLessonsID {  get; set; }
        public int StudentID { get; set; }
        public string StudentName { get; set; }
        public int TeacherID { get; set; }
        public string TeacherName { get; set; }
        public int CourseID { get; set; }
        public string CourseName { get; set; }
    }
}
