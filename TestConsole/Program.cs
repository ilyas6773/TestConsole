using System.Text.Json;
using Newtonsoft.Json;

namespace TestConsole
{
    class Program
    {
        private static string _desktop = "C:\\Users\\Ilyas Tolegenov\\Desktop\\";

        public enum Commands
        {
            help,
            add,
            list,
            remove,
            edit,
            min,
            max,
            average,
            sum,
            saveastxt,
            listfiles,
            saveasjson,
            open
        }
        static void Main(string[] args)
        {
            string[] files = File.ReadAllLines(_desktop + "files.txt");
            List<String> files1 = files.ToList();

            string currentFilepath = "";
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

            List<Employee> employees = new List<Employee>();

            LoadDb(currentFilepath, employees);

            Console.WriteLine("Welcome to test task");

            while (true)
            {
                string input = Console.ReadLine();
                string[] input1 = input.Split(' ');
                switch (input1.Length)
                {
                    case 1:
                    {
                        if (input1[0] == nameof(Commands.help))
                        {
                            Console.WriteLine("add [...]; list [...]; remove[...]; edit[...]; min, max, average, sum");
                        }

                        if (input1[0] == nameof(Commands.min))
                        {
                            MinSalary(employees);
                        }

                        if (input1[0] == nameof(Commands.max))
                        {
                            MaxSalary(employees);
                        }
                        if (input1[0] == nameof(Commands.average))
                        {
                            AvgSalary(employees);
                        }
                        if (input1[0] == nameof(Commands.sum))
                        {
                            SumSalary(employees);
                        }
                        if (input1[0] == nameof(Commands.list))
                        {
                            GetRecord(employees);
                        }

                        if (input1[0] == nameof(Commands.listfiles))
                        {
                            GetFiles(files);
                        }
                        if (input1[0] == "exit")
                            Environment.Exit(1);
                        break;
                    }
                    case 2:
                    {
                        if (input1[0] == nameof(Commands.remove))
                        {
                            RemoveRecord(Int32.Parse(input1[1])-1, employees, currentFilepath);
                        }

                        if (input1[0] == nameof(Commands.saveastxt))
                        {
                            SaveAsTxt(input1[1], employees);
                        }

                        if (input1[0] == nameof(Commands.saveasjson))
                        {
                            SaveAsJson(input1[1], employees);
                        }

                        if (input1[0] == nameof(Commands.open))
                        {
                            employees = OpenFile(input1[1], employees, currentFilepath);
                        }
                        break;
                    }
                    case 4:
                    {
                        if (input1[0] == nameof(Commands.add))
                        {
                            AddRecord(input1, employees, currentFilepath);
                        }
                        break;
                    }
                    case 5:
                    {
                        if (input1[0] == nameof(Commands.edit))
                        {
                                EditRecord(Int32.Parse(input1[1])-1, input1, employees, currentFilepath);
                        }
                        break;
                    }
                }
            }
        }

        static List<Employee> LoadDb(String filepath, List<Employee> employees)
        {
            List<string> lines = File.ReadAllLines(filepath).ToList();

            foreach (var line in lines)
            {
                string[] entries = line.Split(',');
                employees.Add(new Employee{ Name = entries[0], Salary = decimal.Parse(entries[1]), EmploymentDate = DateTime.Parse(entries[2])});
            }

            return employees;
        }

        static void AddRecord(string[] input, List<Employee> employees, String filepath)
        {
            try
            {
                employees.Add(new Employee { Name = input[1], Salary = decimal.Parse(input[2]), EmploymentDate = DateTime.Parse(input[3]) });
            }
            catch (Exception e)
            {
                employees.Add(new Employee());
            }

            List<string> output = new List<string>();
            foreach (var employee in employees)
            {
                output.Add($"{employee.Name}, {employee.Salary}, {employee.EmploymentDate}");
            }
            File.WriteAllLines(filepath, output);

            Console.WriteLine("Item Added");
        }

        static void RemoveRecord(int input, List<Employee> employees, String filepath)
        {
            employees.Remove(employees[input]);
            List<string> output = new List<string>();

            foreach (var employee in employees)
            {
                output.Add($"{employee.Name}, {employee.Salary}, {employee.EmploymentDate}");
            }
            File.WriteAllLines(filepath, output);

            List<string> lines = File.ReadAllLines(filepath).ToList();

            employees[0].ResetId();
            employees.Clear();
            foreach (var line in lines)
            {
                string[] entries = line.Split(',');
                employees.Add(new Employee { Name = entries[0], Salary = decimal.Parse(entries[1]), EmploymentDate = DateTime.Parse(entries[2]) });
            }

            Console.WriteLine("Item Removed");
        }

        static void EditRecord(int x, string[] input, List<Employee> employees, String filepath)
        {
            
            employees[x] = new Employee { Name = input[2], Salary = decimal.Parse(input[3]), EmploymentDate = DateTime.Parse(input[4]) };

            List<string> output = new List<string>();
            foreach (var employee in employees)
            {
                output.Add($"{employee.Name}, {employee.Salary}, {employee.EmploymentDate}");
            }
            File.WriteAllLines(filepath, output);
            Console.WriteLine("Item Edited");
        }

        static void SaveAsTxt(string input, List<Employee> employees)
        {
            using FileStream fs = File.Create(_desktop + input+".txt");
            fs.Close();
            List<string> output = new List<string>();
            foreach (var employee in employees)
            {
                output.Add($"{employee.Name}, {employee.Salary}, {employee.EmploymentDate}");
            }
            File.WriteAllLines(_desktop+input+".txt", output);

            Console.WriteLine($"Item Saved as {input}.txt");

        }

        static void SaveAsJson(string input, List<Employee> employees)
        {
            string json = System.Text.Json.JsonSerializer.Serialize(employees);
            File.WriteAllText(_desktop + input+".json", json);
        }

        static List<Employee> OpenFile(string input, List<Employee> employees, string filepath)
        {
            if (!File.Exists(_desktop + input))
            {
                Console.WriteLine("Wrong Filename or file does not exist");
                return employees;
            }
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

                filepath = _desktop + input;
                Console.WriteLine("Read successfully");
                return employees;
            }
            else if (ext[1] == "json")
            {
                employees[0].ResetId();
                using (StreamReader r = new StreamReader(_desktop + input))
                {
                    string json = r.ReadToEnd();
                    employees = JsonConvert.DeserializeObject<List<Employee>>(json);
                }
                filepath = _desktop + input;
                Console.WriteLine("Read successfully");
                List<Employee> temp = new List<Employee>(employees);
                return employees;

            }
            Console.WriteLine("Read failed");
            return employees;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////
        static void GetRecord(List<Employee> employees)
        {
            foreach (var employee in employees)
            {
                Console.WriteLine($"{employee.Id} {employee.Name} {employee.Salary} {employee.EmploymentDate}");
            }
        }

        static void GetFiles(string[] files)
        {
            foreach (var line in files)
            {
                string[] entries = line.Split("\\");
                Console.WriteLine(entries[entries.Length-1]);
            }
        }

        static void MinSalary(List<Employee> employees)
        {
            decimal lowest = employees.Min(employees => employees.Salary);

            foreach (var employee in employees)
            {
                if (employee.Salary == lowest)
                    Console.WriteLine("The lowest salary is " + employee.Name + " " + lowest);
            }
        }

        static void MaxSalary(List<Employee> employees)
        {
            decimal highest = employees.Max(employees => employees.Salary);
            Console.WriteLine(highest.ToString());

            foreach (var employee in employees)
            {
                if (employee.Salary == highest)
                    Console.WriteLine("The highest salary is" + " " + employee.Name + " " + highest);
            }
        }

        static void AvgSalary(List<Employee> employees)
        {
            decimal avg = employees.Average(employees => employees.Salary);
            Console.WriteLine("The average salary is" + " " + avg);
        }

        static void SumSalary(List<Employee> employees)
        {
            decimal sum = employees.Sum(employees => employees.Salary);

            Console.WriteLine("The total salary is" + " " + sum);

        }
    }
}