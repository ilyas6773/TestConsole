using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TestConsole
{
    public class Engine
    {
        public static string _desktop =  "C:\\Users\\Ilyas Tolegenov\\Desktop\\";

        public static string currentFilepath = "";

        private static string[] files = File.ReadAllLines(_desktop + "files.txt");
        public static List<Employee> LoadDb(List<Employee> employees)
        {
            List<string> files1 = files.ToList();

            if (new FileInfo(_desktop + "files.txt").Length == 0)
            {
                files1.Add(_desktop + "default.txt");
                if (File.Exists(files1[0]))
                {
                    currentFilepath = _desktop + "default.txt";
                    using (StreamWriter writer = new StreamWriter(_desktop + "files.txt"))
                    {
                        writer.WriteLine(currentFilepath);
                    }
                }
                else
                {
                    currentFilepath = _desktop + "default.txt";
                    using FileStream fs = File.Create(_desktop + "default.txt");
                }
            }
            else currentFilepath = files1[0];

            List<string> lines = File.ReadAllLines(currentFilepath).ToList();

            foreach (var line in lines)
            {
                string[] entries = line.Split(',');
                employees.Add(new Employee { Name = entries[0], Salary = decimal.Parse(entries[1]), EmploymentDate = DateTime.Parse(entries[2]) });
            }

            return employees;
        }

        public static string[] GetFiles()
        {
            return files;
        }

        public static bool OpenFile(string input, List<Employee> employees)
        {
            
            string[] ext = input.Split('.');
            if (ext[1] == "txt")
            {
                List<string> lines = File.ReadAllLines(_desktop + input).ToList();

                employees[0].ResetId();
                employees.Clear();

                foreach (var line in lines)
                {
                    string[] entries = line.Split(',');
                    employees.Add(new Employee { Name = entries[0], Salary = decimal.Parse(entries[1]), EmploymentDate = DateTime.Parse(entries[2]) });
                }

                currentFilepath = _desktop + input;
                Program.employees = employees;
                return true;
            }
            else if (ext[1] == "json")
            {
                employees[0].ResetId();
                using (StreamReader r = new StreamReader(_desktop + input))
                {
                    string json = r.ReadToEnd();
                    employees = JsonConvert.DeserializeObject<List<Employee>>(json);
                }

                currentFilepath = _desktop + input;
                List<Employee> temp = new(employees);
                Program.employees = employees;
                return true;

            }

            return false;
        }

        public static bool SaveAsTxt(string input, List<Employee> employees)
        {
            using FileStream fs = File.Create(_desktop + input + ".txt");
            fs.Close();
            List<string> output = new();
            foreach (var employee in employees)
            {
                output.Add($"{employee.Name}, {employee.Salary}, {employee.EmploymentDate}");
            }
            File.WriteAllLines(_desktop + input + ".txt", output);
            return true;
        }

        public static bool SaveAsJson(string input, List<Employee> employees)
        {
            string json = System.Text.Json.JsonSerializer.Serialize(employees);
            File.WriteAllText(_desktop + input + ".json", json);
            return true;
        }

        
    }
}
