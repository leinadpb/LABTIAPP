using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LABTIAPP.Models
{
    public class Teacher
    {
        public int TeacherId { get; set; }

        public string Name { get; set; }
        public string Lastname { get; set; }
        public string Career { get; set; }
        public string Master { get; set; }
        public string Doctorade { get; set; }

        public string EmployeeCode { get; set; }
        
        public ICollection<Subject> Subjects { get; set; }

        public string FullName { get; set; }

        public Teacher()
        {
            FullName = Name + " " + Lastname;
        }

    }
}
