using ADOLib.Interface;
using ADOLib.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ADOCRUD.Controllers
{
    public class EmployeeController : Controller
    {

        IEmployeesRepository _iEmployeesRepository;
        IDepartmentsRepository _iDepartmentsRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        private string UploadedFile(IFormFile file)
        {
            try
            {
                string uniqueFileName = null;

                if (file != null)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                }
                return uniqueFileName;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public EmployeeController(IEmployeesRepository iEmployeesRepository, IDepartmentsRepository iDepartmentsRepository, IWebHostEnvironment webHostEnvironment)
        {
            _iEmployeesRepository = iEmployeesRepository;
            _iDepartmentsRepository = iDepartmentsRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            ViewBag.Departments = _iDepartmentsRepository.GetDepartmentsByQuery();
            var employees = _iEmployeesRepository.GetEmployeesByQuery();
            return View(employees);
        }

        public IActionResult Create()
        {
            ViewBag.Departments = _iDepartmentsRepository.GetDepartmentsByQuery();
            return View();
        }

        public IActionResult Details(int id)
        {
            Employee employee = _iEmployeesRepository.GetEmployeesById(id);
            return View(employee);
        }

        [HttpPost]
        public IActionResult Create(IFormCollection form)
        {
            try
            {
                string uniqueFileName = UploadedFile(form.Files["ProfileImage"]);

                Employee model = new Employee();
                model.Name = form.ContainsKey("Name") ? form["Name"].First() : null;
                model.Address = form.ContainsKey("Address") ? form["Address"].First() : null;
                model.DeptId = form.ContainsKey("DeptId") ? Convert.ToInt32(form["DeptId"].First()) : 0;
                model.ImagePath = uniqueFileName;

                if (ModelState.IsValid)
                {
                    if(_iEmployeesRepository.AddEmployee(model))
                        return RedirectToAction("Index");
                }               
            }
            catch (Exception ex)
            {

            }
            ViewBag.Departments = _iDepartmentsRepository.GetDepartmentsByQuery();
            return View();
        }

        public IActionResult Edit(int id)
        {
            Employee employee = _iEmployeesRepository.GetEmployeesById(id);
            ViewBag.Departments = _iDepartmentsRepository.GetDepartmentsByQuery();
            return View("Create", employee);
        }

        [HttpPost]
        public IActionResult Edit(IFormCollection form)
        {
            Employee model = new Employee();
            try
            {
                string uniqueFileName = UploadedFile(form.Files["ProfileImage"]);
                model.EmpId = form.ContainsKey("EmpId") ? Convert.ToInt32(form["EmpId"].First()) : 0;
                model.Name = form.ContainsKey("Name") ? form["Name"].First() : null;
                model.Address = form.ContainsKey("Address") ? form["Address"].First() : null;
                model.DeptId = form.ContainsKey("DeptId") ? Convert.ToInt32(form["DeptId"].First()) : 0;
                model.ImagePath = uniqueFileName ?? (form.ContainsKey("ImagePath") ? form["ImagePath"].First() : null);

                if (ModelState.IsValid)
                {
                    if (_iEmployeesRepository.UpdateEmployee(model))
                        return RedirectToAction("Index");
                }              
            }
            catch (Exception ex)
            {

            }
            ViewBag.Departments = _iDepartmentsRepository.GetDepartmentsByQuery();
            return View("Create", model);
        }

      
        public IActionResult Delete(int id)
        {
            //Employee employee = _iEmployeesRepository.GetEmployeesById(empid);
            //if (employee != null)
            if(id > 0)
            {
                _iEmployeesRepository.DeleteEmployee(id);
            }
            return RedirectToAction("Index");
        }
    }
}
