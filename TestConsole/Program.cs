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
            open,
            q,
            minemploy,
            maxemploy,
            avgemploy
        }

        static void Main(string[] args)
        {

            Engine.LoadDb();

            Console.WriteLine("Welcome to test task");

            while (true)
            {
                string input = Console.ReadLine();
                string[] input1 = input.Split(' ');
                Commands cmd;
                if (!Enum.TryParse(input1[0], out cmd))
                {
                    Console.WriteLine("Incorrect command format. Please type help for more info");
                    continue;
                }
                switch (cmd)
                {
                    case Commands.help:
                    {
                        Console.WriteLine("add [...]; list [...]; remove[...]; edit[...]; min, max, average, sum, saveastxt[...], listfiles[...], saveasjson[...], open[...], q");
                        break;
                    }
                    case Commands.min:
                    {
                        string[] output = DataAccessor.MinSalary();
                        Console.WriteLine($"{output[0]} has the lowest salary of {output[1]}");
                        break;
                    }
                    case Commands.max:
                    {
                        string[] output = DataAccessor.MaxSalary();
                        Console.WriteLine($"{output[0]} has the highest salary of {output[1]}");
                        break;
                    }
                    case Commands.average:
                    {
                        Console.WriteLine($"The average Salary is {DataAccessor.AvgSalary()}");
                        break;
                    }
                    case Commands.sum:
                    {
                        Console.WriteLine($"The total Salary is {DataAccessor.SumSalary()}");
                        break;
                    }
                    case Commands.list:
                    {
                        if (DataAccessor.GetList().Count == 0)
                        {
                            Console.WriteLine("The list is empty");
                            break;
                        }
                        foreach (var employee in DataAccessor.GetList())
                        {
                            Console.WriteLine($"{employee.Id} {employee.Name} {employee.Salary} {employee.EmploymentDate}");
                        }
                        break;
                    }
                    case Commands.listfiles:
                    {
                        foreach (var line in Engine.GetFiles())
                        {
                            string[] entries = line.Split("\\");
                            Console.WriteLine(entries[entries.Length - 1]);
                        }
                        break;
                    }
                    case Commands.remove:
                    {
                        if (input1.Length != 2)
                        {
                            Console.WriteLine("Not enough arguments. Correct form: remove [Id]");
                            break;
                        }

                        if (Int32.TryParse(input1[1], out int x))
                        {
                            Console.WriteLine(DataAccessor.RemoveRecord(Int32.Parse(input1[1]))
                                ? $"Record #{input1[1]} removed"
                                : "No such ID");
                        }
                        else Console.WriteLine("Write correct value");
                        break;
                    }
                    case Commands.saveastxt:
                    {
                        if (input1.Length != 2)
                        {
                            Console.WriteLine("Not enough arguments. Correct form: saveastxt [filename]");
                            break;
                        }

                        if (ContainProhibitedSymbols(input1[1]))
                                break;
                        if (Engine.SaveAsTxt(input1[1]))
                        {
                            Console.WriteLine($"Saved Successfully as {input1}.txt");
                        }
                        break;
                    }
                    case Commands.saveasjson:
                    {
                        if (input1.Length != 2)
                        {
                            Console.WriteLine("Not enough arguments. Correct form: saveasjson [filename]");
                            break;
                        }

                        if (ContainProhibitedSymbols(input1[1]))
                            break;
                        if (Engine.SaveAsJson(input1[1]))
                        {
                            Console.WriteLine($"Saved Successfully as {input1}.txt");
                        }
                        
                        break;
                    }
                    case Commands.open:
                    {
                        if (input1.Length != 2)
                        {
                            Console.WriteLine(
                                "Incorrect number of arguments. Correct form: open [filename.extenstion]");
                            break;
                        }

                        if (!File.Exists(Engine._desktop + input1[1]))
                        {
                            Console.WriteLine("Wrong Filename or file does not exist");
                            break;
                        }

                        Console.WriteLine(Engine.OpenFile(input1[1]) ? "Read Successful" : "Read Failed");
                        break;
                    }
                    case Commands.add:
                    {
                        if (input1.Length != 4)
                        {
                            Console.WriteLine("Incorrect number of arguments. Correct form: edit [name] [salary] [employmentDate]");
                            break;
                        }
                        if (Decimal.TryParse(input1[2], out decimal x) &&
                            DateTime.TryParse(input1[3], out DateTime y))
                        {
                            if (DataAccessor.AddRecord(input1[1], Decimal.Parse(input1[2]),
                                    DateTime.Parse(input1[3])))
                            {
                                Console.WriteLine("Item Added");
                            }
                        }
                        else
                            Console.WriteLine("Salary or EmploymentDate value is incorrect format");
                        break;
                    }
                    case Commands.edit:
                    {
                        if (input1.Length != 5)
                        {
                            Console.WriteLine("Incorrect number of arguments. Correct form: edit [Id] [name] [salary] [employmentDate]");
                            break;
                        }
                        if (Int32.TryParse(input1[1], out int z) &&
                            Decimal.TryParse(input1[3], out decimal x) &&
                            DateTime.TryParse(input1[4], out DateTime y))
                        {
                            if (DataAccessor.EditRecord(Int32.Parse(input1[1]) - 1, input1[2], Decimal.Parse(input1[3]), DateTime.Parse(input1[4])))
                            {
                                Console.WriteLine("Item Edited");
                            }
                        }
                        else
                            Console.WriteLine("ID or Salary or EmploymentDate value is incorrect format");
                        break;
                    }
                    case Commands.q:
                        Environment.Exit(1);
                        break;
                    case Commands.maxemploy:
                    {
                        string[] output = DataAccessor.maxEmploy();
                            Console.WriteLine($"{output[0]} has been employed the longest {DateTime.Now - DateTime.Parse(output[1])} days");
                        break;
                    }
                    case Commands.minemploy:
                    {
                        string[] output = DataAccessor.minEmploy();
                        Console.WriteLine($"{output[0]} has been employed the least {DateTime.Now - DateTime.Parse(output[1])} days");
                        break;
                    }
                    case Commands.avgemploy:
                    {
                        Console.WriteLine($"The average employment duration is {DataAccessor.avgEmploy()} days");
                        break;
                    }
                    default:
                        Console.WriteLine("Incorrect command format. For more info type help");
                        break;
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