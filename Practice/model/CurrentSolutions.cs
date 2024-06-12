using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice.model
{
    public class CurrentSolutions
    {
        public int SolutionID { get; set; }
        public string StydentName { get; set; }
        public string StydentLastName { get; set; }
        public string StydentMiddleName { get; set; }
        public int AssignmentID {  get; set; }
        public string AssignmentName { get; set; }
        public string AssignmentDesc { get; set; }
        public int TeacherID { get; set; }
        public string TeacherName { get;set; }
        public bool Grade {  get; set; }
        public string StudentComment { get; set;}
        public string TeacherComment { get; set;}
    }
}
