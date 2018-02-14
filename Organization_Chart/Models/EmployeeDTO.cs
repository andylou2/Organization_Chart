using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Organization_Chart.Models
{
    public class EmployeeDTO
    {
        public int ID { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String Email { get; set; }
        public Department Department { get; set; }
    }
}