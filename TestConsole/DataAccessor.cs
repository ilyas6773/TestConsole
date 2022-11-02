using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TestConsole
{
    public class DataAccessor
    {

        public static bool AddRecord(string[] input, List<Employee> employees)
        {
            try
            {
                employees.Add(new Employee { Name = input[1], Salary = decimal.Parse(input[2]), EmploymentDate = DateTime.Parse(input[3]) });
            }
            catch (Exception e)
            {
                employees.Add(new Employee());
            }

            List<string> output = new();
            foreach (var employee in employees)
            {
                output.Add($"{employee.Name}, {employee.Salary}, {employee.EmploymentDate}");
            }
            File.WriteAllLines(Engine.currentFilepath, output);

            return true;
        }

        public static bool RemoveRecord(int input, List<Employee> employees)
        {
            employees.Remove(employees[input]);
            List<string> output = new List<string>();

            foreach (var employee in employees)
            {
                output.Add($"{employee.Name}, {employee.Salary}, {employee.EmploymentDate}");
            }
            File.WriteAllLines(Engine.currentFilepath, output);

            List<string> lines = File.ReadAllLines(Engine.currentFilepath).ToList();

            employees[0].ResetId();
            employees.Clear();
            foreach (var line in lines)
            {
                string[] entries = line.Split(',');
                employees.Add(new Employee { Name = entries[0], Salary = decimal.Parse(entries[1]), EmploymentDate = DateTime.Parse(entries[2]) });
            }

            return true;
        }

        public static bool EditRecord(int x, string[] input, List<Employee> employees)
        {

            employees[x] = new Employee { Name = input[2], Salary = decimal.Parse(input[3]), EmploymentDate = DateTime.Parse(input[4]) };

            List<string> output = new();
            foreach (var employee in employees)
            {
                output.Add($"{employee.Name}, {employee.Salary}, {employee.EmploymentDate}");
            }

            File.WriteAllLines(Engine.currentFilepath, output);
            return true;
        }

        public static String[] MinSalary(List<Employee> employees)
        {
            decimal lowest = employees.Min(employees => employees.Salary);

            string[] output = new string[2];

            foreach (var employee in employees)
            {
                if (employee.Salary == lowest)
                {
                    output[0] = employee.Name;
                    output[1] = lowest.ToString();
                    break;
                }
            }
            return output;
        }

        public static String[] MaxSalary(List<Employee> employees)
        {
            decimal highest = employees.Max(employees => employees.Salary);
            string[] output = new string[2];

            foreach (var employee in employees)
            {
                if (employee.Salary == highest)
                {
                    output[0] = employee.Name;
                    output[1] = highest.ToString();
                    break;
                }
            }
            return output;
        }

        public static decimal AvgSalary(List<Employee> employees)
        {
            return employees.Average(employees => employees.Salary);

        }

        public static decimal SumSalary(List<Employee> employees)
        {
            return employees.Sum(employees => employees.Salary);
        }
    }
}
