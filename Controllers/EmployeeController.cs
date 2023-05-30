using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Data;
using Microsoft.EntityFrameworkCore;
using ContactManager.Models;

namespace ContactManager.Controllers
{
    public class EmployeeController : Controller
    {
        private EmployeeContext context;
        private Converter converter = new Converter();
        
        private string fileName;

        public EmployeeController(EmployeeContext context) {
           this.context=context;
        }
        
        // GET: /Employee/
        public ActionResult Index()
        {
            return View();
        }

         public ActionResult GetAll()
        {
            var list = context.Employees.ToList();
            return PartialView("_GetAll", list);
        }

        public ActionResult SortEmployees(string column)
        {
            IList<Employee> emps = null;
            if (column.Equals("Name")) {emps = context.Employees.AsEnumerable().OrderBy(e => e.Name).ToList<Employee>(); }
            else if (column.Equals("DateOfBirth")) {emps = context.Employees.AsEnumerable().OrderBy(e => e.DateOfBirth).ToList<Employee>(); }
            else if (column.Equals("Married")) {emps = context.Employees.AsEnumerable().OrderBy(e => e.Married).ToList<Employee>(); }
            else if (column.Equals("Phone")) {emps = context.Employees.AsEnumerable().OrderBy(e => e.Phone).ToList<Employee>(); }
            else if (column.Equals("Salary")) {emps = context.Employees.AsEnumerable().OrderBy(e => e.Salary).ToList<Employee>(); }
            else if (column.Equals("NoSorting")) {emps = context.Employees.ToList<Employee>(); }
            return PartialView("_GetAll", emps);
        }

        private IList<Employee> GetEmployees()
        {
            return context.Employees.ToList();
        }

        private void AddEmployee(Employee emp)
        {
            context.Employees.Add(emp);
            context.SaveChanges();
        }

        [HttpPost]
        public ActionResult Upload(IFormFile uploadedFile)
        {
            IFormFile file=null;
            try
             {   
                file = Request.Form.Files.FirstOrDefault();
                if (file==null) throw new NullReferenceException();
       
            }
            catch (Exception ex) { return Content("No file chosen"); }

            if (file != null && file.Length > 0)
            {
                fileName = file.FileName;
            }

            try
            {
                IList<Employee> emps = converter.convertFromCSV(file);
                foreach (Employee e in emps)
                {
                    AddEmployee(e);
                }
            }
            catch (Exception ex)  { return Content("Conversion error"); }

            return Content("File " + fileName + " uploaded");
        }

        [HttpPost]     //Called with post jquery.Load method
        public ActionResult Edit(Employee newEmployee)
        {
            Employee e = context.Employees.Find(newEmployee.EmployeeId);
            e.Name = newEmployee.Name;
            e.DateOfBirth = Convert.ToDateTime(newEmployee.DateOfBirth).Date;
            e.Married = newEmployee.Married;
            e.Phone=newEmployee.Phone;
            e.Salary=Convert.ToDecimal(newEmployee.Salary);
            try
            {
                context.Entry(e).State = EntityState.Modified;
                context.SaveChanges();
                return PartialView("_GetAll", GetEmployees());
            }
            catch (Exception ex) { 
                return Content("There is no such employee");
            }
        }

        [HttpPost]   //Called with post jquery.Load method
        public ActionResult Delete(int id)
        {
            Console.WriteLine("iddd: "+id);
            Employee e = context.Employees.Find(id);
            try
            {
                context.Employees.Remove(e);
                context.SaveChanges();
                return PartialView("_GetAll", GetEmployees());
            }
            catch (Exception ex)
            {
                return Content("There is no such employee");
            }
        }

        





    }
}