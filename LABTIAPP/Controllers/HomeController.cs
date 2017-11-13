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
using Microsoft.AspNetCore.Authorization;

namespace LABTIAPP.Controllers
{
    public class HomeController : Controller
    {
        string Red = "#410502";
        string Empty = "#365768";
        // string Yellow = "#702300";
        string Green = "#003C01";
        string[] Rooms = { "FD401", "FD402", "FD403", "FD404", "FD405", "FD411", "FD412", "FD413", "FD414", "FD415" };

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

        [Authorize]
        public IActionResult Admin()
        {
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

            var subs = _db.Subjects.Include(s => s.Day).Include(s => s.Room).Include(s => s.Color);
            int h = DateTime.Now.Hour;
            DateTime MidDay = DateTime.Parse("12:00 PM");

            //int d = (int)DateTime.Now.DayOfWeek;
            //int day = ((int)DateTime.Now.DayOfWeek == 0) ? 7 : (int)DateTime.Now.DayOfWeek; //Ln - 1 ..... Do - 7
            int day = (int)DateTime.Now.DayOfWeek; // Sunday - 0
            if (day == 0) day = 7;
            DateTime date = DateTime.Now;
            ViewBag.TodayDate = string.Format("{0:f}", date);

            if(DateTime.Now > MidDay)
            {
                switch (h)
                {
                    case 1:
                        h = 13;
                        break;
                    case 2:
                        h = 14;
                        break;
                    case 3:
                        h = 15;
                        break;
                    case 4:
                        h = 16;
                        break;
                    case 5:
                        h = 17;
                        break;
                    case 6:
                        h = 18;
                        break;
                    case 7:
                        h = 19;
                        break;
                    case 8:
                        h = 20;
                        break;
                    case 9:
                        h = 21;
                        break;
                    case 10:
                        h = 22;
                        break;
                    case 11:
                        h = 23;
                        break;
                }
            }

            foreach (string r in Rooms)
            {
                var subjects = subs.Where(s => s.Room.RoomName.Equals(r));

                foreach (Subject s in subjects)
                {
                    

                    if ((h >= s.InitDate && h < s.FiniDate) && s.Day.DayPosition == day)
                    {
                        ViewData[r + "Color"] = Red;
                        break;
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
            
            ViewBag.SelectedRoom = room;
            var subs = _db.Subjects.Include(s => s.Day).Include(s => s.Room).Include(s => s.Color);
            DateTime MidDay = DateTime.Parse("12:00 PM");

            int h = DateTime.Now.Hour;
            int day = (int)DateTime.Now.DayOfWeek; // Sunday - 0
            if (day == 0) day = 7;
            DateTime date = DateTime.Now;
            ViewBag.TodayDate = string.Format("{0:f}", date);

            if (DateTime.Now > MidDay)
            {
                switch (h)
                {
                    case 1:
                        h = 13;
                        break;
                    case 2:
                        h = 14;
                        break;
                    case 3:
                        h = 15;
                        break;
                    case 4:
                        h = 16;
                        break;
                    case 5:
                        h = 17;
                        break;
                    case 6:
                        h = 18;
                        break;
                    case 7:
                        h = 19;
                        break;
                    case 8:
                        h = 20;
                        break;
                    case 9:
                        h = 21;
                        break;
                    case 10:
                        h = 22;
                        break;
                    case 11:
                        h = 23;
                        break;
                }
            }

            foreach (string r in Rooms)
            {
                var subjects = subs.Where(s => s.Room.RoomName.Equals(r));

                foreach (Subject s in subjects)
                {
                    
                    if ((h >= s.InitDate && h < s.FiniDate) && s.Day.DayPosition == day)
                    {
                        ViewData[r + "Color"] = Red;
                        break;
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
