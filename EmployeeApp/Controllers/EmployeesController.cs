using EmployeeApp.Models;
using EmployeeApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}