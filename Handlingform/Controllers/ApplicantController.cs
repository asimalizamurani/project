using Handlingform.Data;
using Handlingform.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Handlingform.Controllers
{
    public class ApplicantController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ApplicantController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Applicant
        public IActionResult Index()
        {
            var applicants = _context.Applicants.ToList();
            return View(applicants);
        }

        // GET: Applicant/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Applicant/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Applicant applicant)
        {
            if (ModelState.IsValid)
            {
                _context.Add(applicant);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(applicant);
        }

        // GET: Applicant/Edit/5
        public IActionResult Edit(int id)
        {
            var applicant = _context.Applicants.Find(id);
            if (applicant == null)
            {
                return NotFound();
            }
            return View(applicant);
        }

        // POST: Applicant/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Applicant applicant)
        {
            if (id != applicant.ApplicantId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(applicant);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(applicant);
        }
    }
}
