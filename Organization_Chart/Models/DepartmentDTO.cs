using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Organization_Chart.Models
{
    public class DepartmentDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int? ParentDepartmentID { get; set; }
        public string ParentDepartment { get; set; }
        public ICollection<Employee> Employees { get; set; }
        public ICollection<DepartmentDTO> Departments { get; set; }
    }
}