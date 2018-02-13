namespace Organization_Chart.Migrations
{
    using Organization_Chart.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Organization_Chart.Models.OrgContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Organization_Chart.Models.OrgContext context)
        {
            //  This method will be called after migrating to the latest version.
            context.Departments.AddOrUpdate(x => new { x.Name }, new Department { Name = "Amazon" });
            var AmazonDepartment = context.Departments.Where(x => x.Name == "Amazon").First();

            context.Employees.AddOrUpdate(x => new { x.Email }, new Employee { FirstName = "Jeff", LastName = "Bezos", Email = "Jeff_Bezos@amazon.com", DepartmentID = AmazonDepartment.ID });

            context.Departments.AddOrUpdate(x => new { x.Name }, new Department { Name = "Sales", ParentDepartmentID = AmazonDepartment.ID });
            var SalesDepartment = context.Departments.Where(x => x.Name == "Sales").First();

            context.Departments.AddOrUpdate(x => new { x.Name }, new Department { Name = "Inside Sales", ParentDepartmentID = SalesDepartment.ID });
            context.Departments.AddOrUpdate(x => new { x.Name }, new Department { Name = "Key Account Sales", ParentDepartmentID = SalesDepartment.ID });

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
        }
    }
}
