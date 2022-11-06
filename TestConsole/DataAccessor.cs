using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TestConsole
{
    public static class DataAccessor
    {
        private static List<Employee> _Stack = Engine.LoadDb();
        public static bool AddRecord(string name, decimal salary, DateTime employmentDate)
        {
            try
            {
                _Stack.Add(new Employee { Name = name, Salary = salary, EmploymentDate = employmentDate });
            }
            catch (Exception e)
            {
                _Stack.Add(new Employee());
            }

            List<string> output = new ();
            foreach (var employee in _Stack)
            {
                output.Add($"{employee.Name}, {employee.Salary}, {employee.EmploymentDate}");
            }
            File.WriteAllLines(Engine.currentFilepath, output);

            return true;
        }

        public static bool RemoveRecord(int input)
        {
            if (input > _Stack.Count)
            {
                return false;
            }
            _Stack.Remove(_Stack[input]);
            List<string> output = new List<string>();

            foreach (var employee in _Stack)
            {
                output.Add($"{employee.Name}, {employee.Salary}, {employee.EmploymentDate}");
            }
            File.WriteAllLines(Engine.currentFilepath, output);

            List<string> lines = File.ReadAllLines(Engine.currentFilepath).ToList();

            _Stack[0].ResetId();
            _Stack.Clear();
            foreach (var line in lines)
            {
                string[] entries = line.Split(',');
                _Stack.Add(new Employee { Name = entries[0], Salary = decimal.Parse(entries[1]), EmploymentDate = DateTime.Parse(entries[2]) });
            }

            return true;
        }

        public static bool EditRecord(int x, string name, decimal salary, DateTime employmentDate)
        {

            _Stack[x] = new Employee { Name = name, Salary = salary, EmploymentDate = employmentDate};

            List<string> output = new();
            foreach (var employee in _Stack)
            {
                output.Add($"{employee.Name}, {employee.Salary}, {employee.EmploymentDate}");
            }

            File.WriteAllLines(Engine.currentFilepath, output);
            return true;
        }

        public static String[] MinSalary()
        {
            decimal lowest = _Stack.Min(employees => employees.Salary);

            string[] output = new string[2];

            foreach (var employee in _Stack)
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

        public static String[] MaxSalary()
        {
            decimal highest = _Stack.Max(employees => employees.Salary);
            string[] output = new string[2];

            foreach (var employee in _Stack)
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

        public static decimal AvgSalary()
        {
            return _Stack.Average(employees => employees.Salary);

        }

        public static decimal SumSalary()
        {
            return _Stack.Sum(employees => employees.Salary);
        }

        public static List<Employee> GetList()
        {
            return _Stack;
        }

        public static void SetList(List<Employee> input)
        {
            _Stack = input;
        }

        public static void ClearList()
        {
            _Stack.Clear();
        }

        public static void Reset()
        {
            _Stack[0].ResetId();
        }

        public static void AddToList(string name, decimal salary, DateTime employmentDate)
        {
            _Stack.Add(new Employee { Name = name, Salary = salary, EmploymentDate = employmentDate});
        }

        public static string[] maxEmploy()
        {
            DateTime x = _Stack.Min(employee => employee.EmploymentDate);
            string[] output = new string[2];
            foreach (var employee in _Stack)
            {
                if (employee.EmploymentDate == x)
                {
                    output[0] = employee.Name;
                    output[1] = employee.EmploymentDate.ToString();
                }

            }
            return output;
        }

        public static string[] minEmploy()
        {
            DateTime x = _Stack.Max(employee => employee.EmploymentDate);
            string[] output = new string[2];
            foreach (var employee in _Stack)
            {
                if (employee.EmploymentDate == x)
                {
                    output[0] = employee.Name;
                    output[1] = employee.EmploymentDate.ToString();
                }

            }
            return output;
        }

        public static double avgEmploy()
        {
            List<DateTime> dates = new();
            foreach (var employee in _Stack)
            {
                dates.Add(employee.EmploymentDate);
            }
            var avg = dates.Zip(dates.Skip(1), (dt1, dt2) => dt2 - dt1)
                          .Aggregate(0d, (a, dt) => a + dt.Days)
                      / (dates.Count - 1);

            return avg;
        }
    }
}
