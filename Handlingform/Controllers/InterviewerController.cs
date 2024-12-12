using Handlingform.Data;
using Handlingform.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Handlingform.Controllers
{
    public class InterviewerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InterviewerController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Interviewer
        public IActionResult Index()
        {
            var interviews = _context.Interviews.ToList();
            return View(interviews);
        }

        // GET: Interviewer/Edit/5
        public IActionResult Edit(int id)
        {
            var interview = _context.Interviews.Find(id);
            if (interview == null)
            {
                return NotFound();
            }
            return View(interview);
        }

        // POST: Interviewer/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Interview interview)
        {
            if (id != interview.InterviewId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(interview);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(interview);
        }
    }
}
