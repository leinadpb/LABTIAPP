using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LABTIAPP.Models
{
    public class Subject
    {
        public int SubjectId { get; set; }

        public int InitDate { get; set; }
        public int FiniDate { get; set; }
        public string Title { get; set; }
        public string KeyCode { get; set; }

        public int ColorId { get; set; }
        public Color Color { get; set; }

        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; }

        public int RoomId { get; set; }
        public Room Room { get; set; }

        public int DayId { get; set; }
        public Day Day { get; set; }



    }
}
