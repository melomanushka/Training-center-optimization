using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice.model
{
    internal class CurrentUser
    {
        public static int ID { get; set; }
        public static string Login { get; set; }
        public static int Status { get; set; }
        public static string FirstName { get; set; }
        public static string LastName { get; set; }
        public static string MidleName { get; set; }
        public static string Password { get; set; }
        public static string Email { get; set; }
        public static string PassportNumber { get; set; }
        public static DateTime PassportIssueDate { get; set; }
        public static string PassportIssuedBy { get; set; }
        public static string PassportSeries { get; set; }
        public static string PhoneNumber { get; set; }
        public static string GetFullName()
        {
            return $"{FirstName} {LastName}";
        }
    }
}
