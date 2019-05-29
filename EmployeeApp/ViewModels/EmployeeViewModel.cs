using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeeApp.ViewModels
{
    public class EmployeeViewModel
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int RoleID { get; set; }
        public string RoleName { get; set; }
    }
}