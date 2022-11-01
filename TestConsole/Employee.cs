using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace TestConsole
{
    internal class Employee
    {
        static int nextId;

        public int Id { get; private set; }

        public void ResetId()
        {
            nextId = 0;
        }
        public Employee()
        {
            Id = Interlocked.Increment(ref nextId);
        }

        public string Name { get; set; } = "MissingName";
        public decimal Salary { get; set; } = 0000;
        public DateTime EmploymentDate { get; set; } = DateTime.Now;
    }
}
