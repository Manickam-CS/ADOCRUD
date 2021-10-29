using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ADOLib.Models
{
    public class Employee
    {
        [Key]
        public int EmpId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string ImagePath { get; set; }

        [Required]
        public int DeptId { get; set; }

        public string DepartmentName { get; set; }
    }
}
