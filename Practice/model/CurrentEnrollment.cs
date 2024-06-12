using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice.model
{
    public class CurrentEnrollment
    {
        public int ContractID { get; set; }
        public DateTime StarttDate { get; set; }
        public DateTime EndtDate { get; set; }
        public int CreatorID { get; set; }
        public string CreatorName { get; set; }
        public int StudentID { get; set; }
        public string StudentName { get; set; }
        public int CourseID { get; set; }
        public string CourseName { get; set; }
        public decimal TotalCost { get; set; }
        public decimal AmountPaid { get; set; }
        public int PaymentTypeID { get; set; }
        public string PaymentTypeName { get; set; }
        public DateTime ContractDate { get; set; }
    }
}
