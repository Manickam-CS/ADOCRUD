using ADOLib.Interface;
using ADOLib.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ADOCRUD.Controllers
{
    public class DepartmentController : Controller
    {
        IDepartmentsRepository _iDepartmentsRepository;

        public DepartmentController(IDepartmentsRepository iDepartmentsRepository)
        {
            _iDepartmentsRepository = iDepartmentsRepository;
        }

        public IActionResult Index()
        {
            var departments = _iDepartmentsRepository.GetDepartmentsByQuery();
            return View(departments);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Department model)
        {
            try
            {
               if( _iDepartmentsRepository.AddDepartment(model))
                    return RedirectToAction("Index");             

            }
            catch { }
            return View();
        }

        public IActionResult Edit(int id)
        {
            Department department = _iDepartmentsRepository.GetDepartmentsById(id);
            return View("Create", department);
        }

        [HttpPost]
        public IActionResult Edit(Department model)
        {
            try
            {
                if( _iDepartmentsRepository.UpdateDepartment(model))
                    return RedirectToAction("Index");
            }
            catch { }
            return View("Create", model);
        }

        public IActionResult Delete(int id)
        {
            if(_iDepartmentsRepository.DeleteDepartment(id))
                return RedirectToAction("Index");
            return View();
        }

    }
}
