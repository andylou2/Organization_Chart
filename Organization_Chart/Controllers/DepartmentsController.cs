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
    [RoutePrefix("api/departments")]
    public class DepartmentsController : ApiController
    {
        //private Organization_ChartContext db = new Organization_ChartContext();
        private OrgContext db = new OrgContext();

        // GET: api/OrgChart
        public IQueryable<DepartmentDTO> GetOrg(int id)
        {
            var childdepartments = GetAllChildDepartments(id);
            return childdepartments;
        }

        // Get: children departments
        public IQueryable<DepartmentDTO> GetAllChildDepartments(int id)
        {
            var childdepartments = from dep in db.Departments
                                   where dep.ParentDepartmentID == id
                                   select new DepartmentDTO()
                                   {
                                       ID = dep.ID,
                                       Name = dep.Name,
                                       ParentDepartmentID = dep.ParentDepartmentID,
                                       ParentDepartment = dep.ParentDepartment,

                                   };

            if (childdepartments.Count() == 0)
            {
                return null;
            }
            foreach (var dept in childdepartments)
            {
                dept.Departments = GetAllChildDepartments(dept.ID).ToList();
            }
            return childdepartments;
        }

        //public IQueryable<DepartmentDTO> GetChildDepartments(int id)
        //{
        //    var childdepartments = GetChildDepartments
        //    if (childdepartments.Count() == 0)
        //    {
        //        return null;
        //    }
        //    foreach (var dept in childdepartments)
        //    {
        //        dept.Departments = GetAllChildDepartments(dept.ID).ToList();
        //    }
        //    return childdepartments;
        //}

        // GET: api/Departments
        public IQueryable<DepartmentDTO> GetDepartments()
        {
            var departments = from dep in db.Departments
                              select new DepartmentDTO()
                              {
                                  ID = dep.ID,
                                  Name = dep.Name,
                                  ParentDepartmentID = dep.ParentDepartmentID,
                                  ParentDepartment = dep.ParentDepartment,
                                  Employees = dep.Employees
                              };

            return departments;
        }

        // GET: api/Departments/5
        [AcceptVerbs("GET", "POST")]
        [Route("getdepartment")]
        [ResponseType(typeof(DepartmentDTO))]
        public async Task<IHttpActionResult> GetDepartment(int id)
        {
            var department = await db.Departments.Include(dep => dep.Employees).Select(dep =>
                new DepartmentDTO()
                {
                    ID = dep.ID,
                    Name = dep.Name,
                    ParentDepartmentID = dep.ParentDepartmentID,
                    ParentDepartment = dep.ParentDepartment,
                    Employees = dep.Employees
                }).SingleOrDefaultAsync(x => x.ID == id);
            if (department == null)
            {
                return NotFound();
            }

            return Ok(department);
        }

        // PUT: api/Departments/5
        [AcceptVerbs("GET", "POST", "PUT")]
        [Route("updatedepartment")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutDepartment(int id, DepartmentDTO department)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != department.ID)
            {
                return BadRequest();
            }

            db.Entry(department).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DepartmentExists(id))
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

        // POST: api/Departments
        [AcceptVerbs("GET", "POST")]
        [Route("createdepartment", Name = "GetDepartmentById")]
        [ResponseType(typeof(DepartmentDTO))]
        public async Task<IHttpActionResult> PostDepartment(DepartmentDTO department)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var dep = new Department()
            {
                Name = department.Name,
                ParentDepartmentID = department.ParentDepartmentID,
                ParentDepartment = db.Departments.FirstOrDefault(x => x.ID == department.ParentDepartmentID)
            };

            db.Departments.Add(dep);
            await db.SaveChangesAsync();

            //db.Entry(department).Reference(x => x.ParentDepartmentID).Load();

            //var dto = new DepartmentDTO()
            //{
            //    ID = department.ID,
            //    Name = department.Name,
            //    ParentDepartmentID = department.ParentDepartmentID,
            //    ParentDepartment = db.Departments.FirstOrDefault(x => x.ID == department.ParentDepartmentID).Name,
            //    Employees = department.Employees
            //};

            return CreatedAtRoute("GetDepartmentByID", new { id = dep.ID }, department);
        }

        // DELETE: api/Departments/5
        [AcceptVerbs("GET", "POST", "DELETE")]
        [Route("deletedepartment")]
        [ResponseType(typeof(Department))]
        public async Task<IHttpActionResult> DeleteDepartment(int id)
        {
            Department department = await db.Departments.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }
    
            var childdepartments = db.Departments.Where(d => d.ParentDepartmentID == department.ID).ToList();
            
            if (department.ParentDepartmentID == null && childdepartments.Count() == 0)
            {
                return BadRequest(ModelState);
            }

            foreach (var dept in childdepartments)
            {
                dept.ParentDepartmentID = department.ParentDepartmentID;
            }
            await db.SaveChangesAsync();

            db.Departments.Remove(department);
            await db.SaveChangesAsync();

            return Ok(department);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DepartmentExists(int id)
        {
            return db.Departments.Count(e => e.ID == id) > 0;
        }
    }
}