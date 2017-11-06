using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LABTIAPP.Models
{
    public class Day
    {
        public int DayId { get; set; }

        public string DayName { get; set; }
        public int DayPosition { get; set; }

        public List<Subject> Subjects { get; set; }


    }
}
