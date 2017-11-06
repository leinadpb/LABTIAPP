using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LABTIAPP.Models;
using LABTIAPP.Data;
using System.Diagnostics;

namespace LABTIAPP.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;

        public HomeController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var labContext = _db.Subjects.Include(s => s.Day).Include(s => s.Room).Include(s => s.Teacher);
            ViewBag.Subjects = labContext;
            ViewBag.TotalSubjects = labContext.Count();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(int room)
        {
            int[] aulas = new int[] { 401, 402, 403, 404, 405, 411, 412, 413, 414, 415};

            if(room == 0)
            {
                ViewBag.RoomNotDefined = "Introduzca un aula válida.";
            }

            if(!aulas.Contains(room))
            {
                ViewBag.RoomNotFound = "El aula selecciona no existe en el LABTI.";
            }

            string StrRoom = "FD" + room.ToString();

            var labContext = _db.Subjects.Include(s => s.Day).Include(s => s.Room).Include(s => s.Teacher).Where(s => s.Room.RoomName.Equals(StrRoom));
            ViewBag.Subjects = labContext;
            ViewBag.TotalSubjects = labContext.Count();
            return View(await labContext.ToListAsync());
        }
        //Get
        public IActionResult Schedule()
        {
            string Red = "#410502";
            string Empty = "#365768";
            // string Yellow = "#702300";
            string Green = "#003C01";
            string[] Rooms = { "FD401", "FD402", "FD403", "FD404", "FD405", "FD411", "FD412", "FD413", "FD414", "FD415" };

            var subs = _db.Subjects.Include(s => s.Day).Include(s => s.Room).Include(s => s.Color);
            
            foreach (string r in Rooms)
            {
                var subjects = subs.Where(s => s.Room.RoomName.Equals(r));

                foreach (Subject s in subjects)
                {
                    int h = DateTime.Now.Hour;
                    //int d = (int)DateTime.Now.DayOfWeek;
                    int day = ((int)DateTime.Now.DayOfWeek == 0) ? 7 : (int)DateTime.Now.DayOfWeek; //Ln - 1 ..... Do - 7

                    if ((h >= s.InitDate && h < s.FiniDate) && s.Day.DayId == day)
                    {
                        ViewData[r + "Color"] = Red;
                    }
                    else
                    {
                        ViewData[r + "Color"] = Green;

                    }
                }
                if (subjects.Count() == 0)
                {
                    ViewData[r + "Color"] = Empty;
                }
            }

            return View(_db.Subjects.Include(s => s.Day).Include(s => s.Room).Include(s => s.Teacher).Include(s => s.Color).OrderBy(s => s.Day.DayId));
        }
        [HttpPost]
        public IActionResult Schedule(string room)
        {
            string Red = "#410502";
            string Empty = "#365768";
            // string Yellow = "#702300";
            string Green = "#003C01";
            string[] Rooms = { "FD401", "FD402", "FD403", "FD404", "FD405", "FD411", "FD412", "FD413", "FD414", "FD415" };
            ViewBag.SelectedRoom = room;
            var subs = _db.Subjects.Include(s => s.Day).Include(s => s.Room).Include(s => s.Color);

            foreach (string r in Rooms)
            {
                var subjects = subs.Where(s => s.Room.RoomName.Equals(r));

                foreach (Subject s in subjects)
                {
                    int h = DateTime.Now.Hour;
                    //int d = (int)DateTime.Now.DayOfWeek;
                    int day = ((int)DateTime.Now.DayOfWeek == 0) ? 7 : (int)DateTime.Now.DayOfWeek; //Ln - 1 ..... Do - 7

                    if ((s.InitDate >= h && h <= s.FiniDate) && s.Day.DayId == day)
                    {
                        ViewData[r + "Color"] = Red;
                    }
                    else
                    {
                        ViewData[r + "Color"] = Green;

                    }
                }
                if (subjects.Count() == 0)
                {
                    ViewData[r + "Color"] = Empty;
                }
            }



            var SubjectList = _db.Subjects.Include(s => s.Day).Include(s => s.Room).Include(s => s.Teacher).Include(s => s.Color)
                .Where(s => s.Room.RoomName.Equals("FD" + room));
            ViewBag.Subjects = SubjectList;



            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
