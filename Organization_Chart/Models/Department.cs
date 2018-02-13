using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Organization_Chart.Models
{
    public class Department
    {
        public Department()
        {

        }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public String Name { get; set; }

        [ForeignKey("ParentDepartment")]
        public int? ParentDepartmentID { get; set; }

        public virtual Department ParentDepartment { get; set; }

        public virtual ICollection<Employee> Employees { get; set; } 
        public virtual ICollection<Department> Departments { get; set; }
    }
}