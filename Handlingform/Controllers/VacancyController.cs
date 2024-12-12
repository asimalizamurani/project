using Handlingform.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Handlingform.Data;

namespace Handlingform.Controllers
{
    public class VacancyController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VacancyController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Vacancy
        public IActionResult Index()
        {
            var vacancies = _context.Vacancies.ToList();
            return View(vacancies);
        }

        // GET: Vacancy/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Vacancy/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Vacancy vacancy)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vacancy);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(vacancy);
        }

        // GET: Vacancy/Edit/5
        public IActionResult Edit(int id)
        {
            var vacancy = _context.Vacancies.Find(id);
            if (vacancy == null)
            {
                return NotFound();
            }
            return View(vacancy);
        }

        // POST: Vacancy/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Vacancy vacancy)
        {
            if (id != vacancy.VacancyId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vacancy);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Vacancies.Any(v => v.VacancyId == id))
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(vacancy);
        }

        // GET: Vacancy/Delete/5
        public IActionResult Delete(int id)
        {
            var vacancy = _context.Vacancies.Find(id);
            if (vacancy == null)
            {
                return NotFound();
            }
            return View(vacancy);
        }

        // POST: Vacancy/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var vacancy = _context.Vacancies.Find(id);
            _context.Vacancies.Remove(vacancy);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}

