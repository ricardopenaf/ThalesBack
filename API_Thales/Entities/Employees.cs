using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Root
    {
        public string status { get; set; }
        public List<Employees> data { get; set; }
        public string message { get; set; }
    }

    public class Employees
    {
        public int id { get; set; }
        public string imageUrl { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string contactNumber { get; set; }
        public int age { get; set; }
        public string dob { get; set; }
        public double salary { get; set; }
        public string address { get; set; }
        public double annualSalary { get; set; }
    }
}
