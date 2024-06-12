using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice.model
{
    public class CurrentUserForTable
    {
        public int ID { get; set; }
        public string Login { get; set; }
        public int Status { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string PassportNumber { get; set; }
        public DateTime PassportIssueDate { get; set; }
        public string PassportIssuedBy { get; set; }
        public string PassportSeries { get; set; }
        public string PhoneNumber { get; set; }
    }
}
