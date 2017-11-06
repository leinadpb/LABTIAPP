using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LABTIAPP.Models
{
    public class Color
    {
        public int ColorId { get; set; }
        public string Value { get; set; }
        public string Name { get; set; }

        public ICollection<Subject> Subjects { get; set; }

    }
}
