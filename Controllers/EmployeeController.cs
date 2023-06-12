using System.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ContactManager.Models;
using ContactManager.Services;

namespace ContactManager.Controllers
{
    public class EmployeeController : Controller
    {
        private IEmployeeService employeeService;

        public EmployeeController(IEmployeeService empService) {
           this.employeeService=empService;
        }
        
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> GetAll()
        {
            var list = await employeeService.GetEmployees();
            return PartialView("_GetAll", list);
        }

        [HttpPost]
        public async Task<ActionResult> Upload(IFormFile uploadedFile)
        {
            IFormFile file=null;
            try
             {   
                file = Request.Form.Files.FirstOrDefault();
                if (file==null) return Content("No file chosen");
                await employeeService.Upload(file);
            }
            catch (Exception ex) { return Content(ex.Message); }
            return Content("File " + file.FileName + " uploaded"); 
        }

        [HttpPost]     //Called with post jquery.Load method
        public async Task<ActionResult> Edit(Employee newEmployee)
        {
            try
            {
                await employeeService.EditEmployee(newEmployee);
                var employees=await employeeService.GetEmployees();
                return PartialView("_GetAll", employees);
            }
            catch (Exception ex) { 
                return Content(ex.Message);
            }
        }

        [HttpPost]   //Called with post jquery.Load method
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await employeeService.DeleteEmployee(id);
                var employees=await employeeService.GetEmployees();
                return PartialView("_GetAll", employees);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

    }
}