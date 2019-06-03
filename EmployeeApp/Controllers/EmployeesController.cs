using EmployeeApp.Models;
using EmployeeApp.ViewModels;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace EmployeeApp.Controllers
{
    public class EmployeesController : Controller
    {
        // GET: Employees
        public ActionResult Index()
        {
            List<EmployeeViewModel> list = new List<EmployeeViewModel>();
            using (EmployeeAppEntities ea = new EmployeeAppEntities())
            {
                var v = (from a in ea.Employees
                         join b in ea.EmployeeRoles on a.RoleID equals b.ID
                         select new EmployeeViewModel
                         {
                             ID = a.ID,
                             FirstName = a.FirstName,
                             LastName = a.LastName,
                             DateOfBirth = a.DateOfBirth,
                             RoleID = a.RoleID,
                             RoleName = b.RoleName
                         });
                list = v.ToList();
            }
            return View(list);
        }

        //Save Employee
        [HttpPost]
        public ActionResult SaveEmployee(int id, string propertyName, string value)
        {
            var status = false;
            var message = "";

            //Update data to database
            using(EmployeeAppEntities ea = new EmployeeAppEntities())
            {
                var user = ea.Employees.Find(id);

                object updateValue = value;
                bool isValid = true;

                if (propertyName == "RoleID")
                {
                    int newRoleID = 0;
                    if(int.TryParse(value,out newRoleID))
                    {
                        updateValue = newRoleID;
                        //Update value field
                        value = ea.EmployeeRoles.Where(e => e.ID == newRoleID).First().RoleName;
                    }
                    else
                    {
                        isValid = false;
                    }
                }else if(propertyName == "DateOfBirth")
                {
                    DateTime dateOfBirth;
                    if(DateTime.TryParseExact(value,"dd-MM-yyyy",new CultureInfo("en-US"), DateTimeStyles.None, out dateOfBirth))
                    {
                        updateValue = dateOfBirth;
                    }
                    else
                    {
                        isValid = false;
                    }
                }
                if(user != null && isValid)
                {
                    ea.Entry(user).Property(propertyName).CurrentValue = updateValue;
                    ea.SaveChanges();
                    status = true;
                }
                else
                {
                    message = "Error!";
                }
            }

            var response = new { value = value, status = status, message = message };
            JObject o = JObject.FromObject(response);
            return Content(o.ToString());

           
        }

        //GET GetEmployeeRoles
        public ActionResult GetEmployeeRoles(int id)
        {
            //{'E':'Letter E','F':'Letter F','G':'Letter G', 'selected':'F'}
            int selectedRoleID = 0;
            StringBuilder sb = new StringBuilder();
            using(EmployeeAppEntities ea = new EmployeeAppEntities())
            {
                var roles = ea.EmployeeRoles.OrderBy(e => e.RoleName).ToList();
                foreach(var item in roles)
                {
                    sb.Append(string.Format("'{0}':'{1}',", item.ID, item.RoleName));
                }

                selectedRoleID = ea.Employees.Where(e => e.ID == id).First().RoleID;
            }

            sb.Append(string.Format("'selected': '{0}'", selectedRoleID));
            return Content("{" + sb.ToString() + "}");
        }
    }
}