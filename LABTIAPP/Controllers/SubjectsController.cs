using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LABTIAPP.Models;
using LABTIAPP.Data;
using Microsoft.AspNetCore.Authorization;

namespace LABTIAPP.Controllers
{
    [Authorize]
    public class SubjectsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private int MinHour = 7;
        private int MaxHour = 22;

        public SubjectsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Subjects
        public async Task<IActionResult> Index()
        {
            var labContext = _context.Subjects.Include(s => s.Color).Include(s => s.Day).Include(s => s.Room).Include(s => s.Teacher);
            return View(await labContext.ToListAsync());
        }

        // GET: Subjects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subject = await _context.Subjects
                .Include(s => s.Color)
                .Include(s => s.Day)
                .Include(s => s.Room)
                .Include(s => s.Teacher)
                .SingleOrDefaultAsync(m => m.SubjectId == id);
            if (subject == null)
            {
                return NotFound();
            }

            return View(subject);
        }

        // GET: Subjects/Create
        public IActionResult Create()
        {
            ViewData["ColorId"] = new SelectList(_context.Colors, "ColorId", "Name");
            ViewData["DayId"] = new SelectList(_context.Days, "DayId", "DayName");
            ViewData["RoomId"] = new SelectList(_context.Rooms, "RoomId", "RoomName");
            ViewData["TeacherId"] = new SelectList(_context.Teachers, "TeacherId", "FullName");
            return View();
        }

        // POST: Subjects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int SubjectId, int InitDate, int FiniDate, string Title, string KeyCode, int ColorId, int TeacherId, int RoomId, int DayId)
        {
            //[Bind("SubjectId,InitDate,FiniDate,Title,KeyCode,TeacherId,RoomId,DayId")] Subject subject
            bool Error = false;

            if (InitDate < MinHour)
            {
                ViewBag.InitDateBelowMinimun = "La hora de inicio debe ser mayor o igual que " + MinHour.ToString();
                Error = true;
            }
            else if (FiniDate > MaxHour)
            {
                ViewBag.FiniDateOverMaximun = "La hora de fin debe ser menor o igual que " + MaxHour.ToString();
                Error = true;
            }
            Subject subject = new Subject()
            {
                SubjectId = SubjectId,
                InitDate = InitDate,
                FiniDate = FiniDate,
                Title = Title,
                KeyCode = KeyCode,
                TeacherId = TeacherId,
                RoomId = RoomId,
                DayId = DayId,
                ColorId = ColorId
            };

            if (!Error)
            {

                bool AddSubject = true;
                //Verify theres is no conflict beetween a new subject
                foreach (Subject s in _context.Subjects.Include(s => s.Day).Include(s => s.Room))
                {
                    if (DayId == s.Day.DayId && s.Room.RoomId == RoomId && (InitDate >= s.InitDate && InitDate <= s.FiniDate))
                    {
                        AddSubject = false;
                        break;
                    }
                }
                if (AddSubject)
                {
                    if (ModelState.IsValid)
                    {
                        _context.Add(subject);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                }
                else
                {
                    ViewBag.SubjectMatchToOther = "La asignatura provoca un choque con otra ya existente.";
                }

                //SelectList(Items, Value, Text, IsSelected)
            }
            ViewData["Colors"] = new SelectList(_context.Colors, "ColorId", "Name");
            ViewData["Days"] = new SelectList(_context.Days, "DayId", "DayName");
            ViewData["Rooms"] = new SelectList(_context.Rooms, "RoomId", "RoomName");
            ViewData["Teachers"] = new SelectList(_context.Teachers, "TeacherId", "FullName");
            ViewData["ColorId"] = new SelectList(_context.Colors, "ColorId", "Name");
            ViewData["DayId"] = new SelectList(_context.Days, "DayId", "DayName");
            ViewData["RoomId"] = new SelectList(_context.Rooms, "RoomId", "RoomName");
            ViewData["TeacherId"] = new SelectList(_context.Teachers, "TeacherId", "FullName");
            return View(subject);
        }

        // GET: Subjects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subject = await _context.Subjects.SingleOrDefaultAsync(m => m.SubjectId == id);
            if (subject == null)
            {
                return NotFound();
            }
            ViewData["ColorId"] = new SelectList(_context.Colors, "ColorId", "Name", subject.ColorId);
            ViewData["DayId"] = new SelectList(_context.Days, "DayId", "DayName", subject.DayId);
            ViewData["RoomId"] = new SelectList(_context.Rooms, "RoomId", "RoomName", subject.RoomId);
            ViewData["TeacherId"] = new SelectList(_context.Teachers, "TeacherId", "FullName", subject.TeacherId);
            return View(subject);
        }

        // POST: Subjects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SubjectId,InitDate,FiniDate,Title,KeyCode,ColorId,TeacherId,RoomId,DayId")] Subject subject)
        {
            if (id != subject.SubjectId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(subject);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubjectExists(subject.SubjectId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ColorId"] = new SelectList(_context.Colors, "ColorId", "ColorId", subject.ColorId);
            ViewData["DayId"] = new SelectList(_context.Days, "DayId", "DayId", subject.DayId);
            ViewData["RoomId"] = new SelectList(_context.Rooms, "RoomId", "RoomId", subject.RoomId);
            ViewData["TeacherId"] = new SelectList(_context.Teachers, "TeacherId", "TeacherId", subject.TeacherId);
            return View(subject);
        }

        // GET: Subjects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subject = await _context.Subjects
                .Include(s => s.Color)
                .Include(s => s.Day)
                .Include(s => s.Room)
                .Include(s => s.Teacher)
                .SingleOrDefaultAsync(m => m.SubjectId == id);
            if (subject == null)
            {
                return NotFound();
            }

            return View(subject);
        }

        // POST: Subjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var subject = await _context.Subjects.SingleOrDefaultAsync(m => m.SubjectId == id);
            _context.Subjects.Remove(subject);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SubjectExists(int id)
        {
            return _context.Subjects.Any(e => e.SubjectId == id);
        }
    }
}
