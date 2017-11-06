using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LABTIAPP.Models
{
    public class Room
    {
        public int RoomId { get; set; }

        public string RoomName { get; set; }

        public ICollection<Subject> Subjects { get; set; }

    }
}
