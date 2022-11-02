using System.Text.Json;
using Newtonsoft.Json;

namespace TestConsole
{
    class Program
    {
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

        public static List<Employee> employees = new List<Employee>();

        static void Main(string[] args)
        {

            employees = Engine.LoadDb(employees);

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
                            string[] output = DataAccessor.MinSalary(employees);
                                Console.WriteLine($"{output[0]} has the lowest salary of {output[1]}");
                        }

                        if (input1[0] == nameof(Commands.max))
                        {
                            string[] output = DataAccessor.MaxSalary(employees);
                            Console.WriteLine($"{output[0]} has the highest salary of {output[1]}");
                        }
                        if (input1[0] == nameof(Commands.average))
                        {
                            Console.WriteLine($"The average Salary is {DataAccessor.AvgSalary(employees)}");
                        }
                        if (input1[0] == nameof(Commands.sum))
                        {
                            Console.WriteLine($"The total Salary is {DataAccessor.SumSalary(employees)}");
                        }
                        if (input1[0] == nameof(Commands.list))
                        { 
                            if (employees.Count == 0)
                            {
                                Console.WriteLine("No records");
                                return;
                            }
                            foreach (var employee in employees)
                            {
                                Console.WriteLine($"{employee.Id} {employee.Name} {employee.Salary} {employee.EmploymentDate}");
                            }
                        }

                        if (input1[0] == nameof(Commands.listfiles))
                        {
                            foreach (var line in Engine.GetFiles())
                            {
                                string[] entries = line.Split("\\");
                                Console.WriteLine(entries[entries.Length - 1]);
                            }
                        }
                        if (input1[0] == "exit")
                            Environment.Exit(1);
                        break;
                    }
                    case 2:
                    {
                        if (input1[0] == nameof(Commands.remove))
                        {
                            if (Int32.Parse(input1[1]) > employees.Count)
                            {
                                Console.WriteLine("Invalid Id");
                                break;
                            }
                            if (DataAccessor.RemoveRecord(Int32.Parse(input1[1]) - 1, employees))
                            {
                                Console.WriteLine($"Record #{input1[1]} removed");
                            }
                        }

                        if (input1[0] == nameof(Commands.saveastxt))
                        {
                            if (ContainProhibitedSymbols(input1[1]))
                                break;

                            if (Engine.SaveAsTxt(input1[1], employees))
                            {
                                Console.WriteLine($"Saved Successfully as {input1}.txt");
                            }
                        }

                        if (input1[0] == nameof(Commands.saveasjson))
                        {
                            if (ContainProhibitedSymbols(input1[1]))
                                break;

                            if (Engine.SaveAsJson(input1[1], employees))
                            {
                                Console.WriteLine($"Saved Successfully as {input1}.json");
                            }
                        }

                        if (input1[0] == nameof(Commands.open))
                        {
                            if (!File.Exists(Engine._desktop + input1[1]))
                            {
                                Console.WriteLine("Wrong Filename or file does not exist");
                                break;
                            }

                            if (Engine.OpenFile(input1[1], employees))
                            {
                                Console.WriteLine("Read Successful");
                            }
                            else
                            {
                                Console.WriteLine("Read Failed");
                            }
                        }
                        break;
                    }
                    case 4:
                    {
                        if (input1[0] == nameof(Commands.add))
                        {
                            if (DataAccessor.AddRecord(input1, employees))
                            {
                                Console.WriteLine("Item Added");
                            }
                        }
                        break;
                    }
                    case 5:
                    {
                        if (input1[0] == nameof(Commands.edit))
                        {
                            if (DataAccessor.EditRecord(Int32.Parse(input1[1]) - 1, input1, employees))
                            {
                                Console.WriteLine("Item Edited");
                            }
                        }
                        break;
                    }
                }
            }
        }
        static bool ContainProhibitedSymbols(string input)
        {
            Dictionary<int, char> dict = ProhibitedChars();
            foreach (char c in input)
            {
                if (dict.ContainsValue(c))
                {
                    Console.WriteLine("Invalid Character");

                    Console.WriteLine("Try Avoiding these characters: ");

                    foreach (var value in dict)
                    {
                        Console.Write(dict + ", ");
                    }
                    return true;
                }
            }
            return false;
        }

        static private Dictionary<int, char> ProhibitedChars()
        {
            Dictionary<int, char> dict = new Dictionary<int, char>();
            dict.Add(1, '%');
            dict.Add(2, '#');
            dict.Add(3, '&');
            dict.Add(4, '{');
            dict.Add(5, '}');
            dict.Add(6, '\\');
            dict.Add(7, '<');
            dict.Add(8, '>');
            dict.Add(9, '*');
            dict.Add(10, '?');
            dict.Add(11, '/');
            dict.Add(12, ' ');
            dict.Add(13, '$');
            dict.Add(14, '!');
            dict.Add(15, '\'');
            dict.Add(16, ':');
            dict.Add(17, '@');
            dict.Add(18, '+');
            dict.Add(19, '`');
            dict.Add(20, '|');
            dict.Add(21, '=');
            return dict;
        }
    }
}