using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Organization_Chart.Models;

namespace Organization_Chart.Controllers
{
    [RoutePrefix("api/employees")]
    public class EmployeesController : ApiController
    {
        private OrgContext db = new OrgContext();

        // GET: api/Employees
        public IQueryable<EmployeeDTO> GetEmployees()
        {
            var employees = from emp in db.Employees
                            select new EmployeeDTO()
                            {
                                ID = emp.ID,
                                FirstName = emp.FirstName,
                                LastName = emp.LastName,
                                Email = emp.Email,
                                Department = emp.Department
                            };
            return employees;
        }

        // GET: api/Employees/5
        [AcceptVerbs("GET", "POST")]
        [Route("getemployee")]
        [ResponseType(typeof(EmployeeDTO))]
        public async Task<IHttpActionResult> GetEmployee(int id)
        {
            var employee = await db.Employees.Select(emp =>
                new EmployeeDTO()
                {
                    ID = emp.ID,
                    FirstName = emp.FirstName,
                    LastName = emp.LastName,
                    Email = emp.Email,
                    Department = emp.Department
                }).SingleOrDefaultAsync(x => x.ID == id);
            if (employee == null)
            {
                return NotFound();
            }

            return Ok(employee);
        }

        // PUT: api/Employees/5
        [AcceptVerbs("GET", "POST", "PUT")]
        [Route("updatedepartment")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutEmployee(int id, EmployeeDTO employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != employee.ID)
            {
                return BadRequest();
            }

            db.Entry(employee).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Employees
        [AcceptVerbs("GET", "POST")]
        [Route("createemployee", Name = "GetEmployeeById")]
        [ResponseType(typeof(EmployeeDTO))]
        public async Task<IHttpActionResult> PostEmployee(EmployeeDTO employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var emp = new Employee()
            {
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = employee.Email,
                Department = employee.Department
            };
            db.Employees.Add(emp);
            await db.SaveChangesAsync();

            return CreatedAtRoute("GetEmployeeByID", new { id = employee.ID }, employee);
        }

        // DELETE: api/Employees/5
        [AcceptVerbs("GET", "POST")]
        [Route("deleteemployee")]
        [ResponseType(typeof(Employee))]
        public async Task<IHttpActionResult> DeleteEmployee(int id)
        {
            Employee employee = await db.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            // TODO UPDATE DEPARTMENT ICOLLECTION??

            db.Employees.Remove(employee);
            await db.SaveChangesAsync();

            return Ok(employee);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EmployeeExists(int id)
        {
            return db.Employees.Count(e => e.ID == id) > 0;
        }
    }
}