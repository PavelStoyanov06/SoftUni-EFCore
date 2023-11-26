using SoftUni.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftUni
{
    public class StartUp
    {
        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            string result = string.Empty;
            var employees = context.Employees.ToList().OrderBy(x => x.Salary);

            foreach (var employee in employees)
            {
                result += $"{employee.FirstName} {employee.LastName} {employee.MiddleName} {employee.JobTitle} {employee.Salary:f2}{Environment.NewLine}";
            }

            return result;
        }

        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            StringBuilder content = new StringBuilder();

            var employees = context.Employees
                .Where(x => x.Salary > 50000)
                .OrderBy(x => x.FirstName)
                .ToList();

            foreach (var employee in employees)
            {
                content.AppendLine($"{employee.FirstName} - {employee.Salary:F2}");
            }

            return content.ToString().TrimEnd();
        }

        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            StringBuilder content = new StringBuilder();

            var employees = context.Employees
                .Where(x => x.Department.DepartmentId == 6)
                .OrderBy(x => x.Salary)
                .ThenByDescending(x => x.FirstName);

            foreach (var employee in employees)
            {
                content.AppendLine($"{employee.FirstName} {employee.LastName} " +
                                   $"from {employee.Department.Name} - ${employee.Salary:F2}");
            }

            return content.ToString().TrimEnd();
        }
    }
}
