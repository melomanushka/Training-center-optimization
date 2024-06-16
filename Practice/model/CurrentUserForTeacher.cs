using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice.model
{
    public class CurrentUserForTeacher
    {
        //forstudent
        public int StudentID { get; set; }
        public string UserName { get; set; }
        public string UserLast {  get; set; }
        public string UserMiddle { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int CourseID { get; set; }
        public string CourseName { get; set; } 

        //for groups
        public string GroupName { get; set; }
        public int GroupsID { get; set; }

    }
}
