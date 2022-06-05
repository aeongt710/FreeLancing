using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FreeLancing.Data;
using FreeLancing.Models;

namespace FreeLancing.Areas.Organization.Views.Jobs
{
    [Area("Organization")]
    public class JobsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public JobsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Organization/Jobs
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Jobs.Include(j => j.Organization).Include(j => j.Tag);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Organization/Jobs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var job = await _context.Jobs
                .Include(j => j.Organization)
                .Include(j => j.Tag)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (job == null)
            {
                return NotFound();
            }

            return View(job);
        }

        // GET: Organization/Jobs/Create
        public IActionResult Create()
        {
            ViewData["OrganizationId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["TagId"] = new SelectList(_context.Tags, "Id", "TagText");
            return View();
        }

        // POST: Organization/Jobs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,Salary,Durtion,IsAssigned,SubmittedText,IsSubmitted,IsCompleted,TagId,OrganizationId")] Job job)
        {
            if (ModelState.IsValid)
            {
                _context.Add(job);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["OrganizationId"] = new SelectList(_context.Users, "Id", "Id", job.OrganizationId);
            ViewData["TagId"] = new SelectList(_context.Tags, "Id", "TagText", job.TagId);
            return View(job);
        }

        // GET: Organization/Jobs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var job = await _context.Jobs.FindAsync(id);
            if (job == null)
            {
                return NotFound();
            }
            ViewData["OrganizationId"] = new SelectList(_context.Users, "Id", "Id", job.OrganizationId);
            ViewData["TagId"] = new SelectList(_context.Tags, "Id", "TagText", job.TagId);
            return View(job);
        }

        // POST: Organization/Jobs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Salary,Durtion,IsAssigned,SubmittedText,IsSubmitted,IsCompleted,TagId,OrganizationId")] Job job)
        {
            if (id != job.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(job);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JobExists(job.Id))
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
            ViewData["OrganizationId"] = new SelectList(_context.Users, "Id", "Id", job.OrganizationId);
            ViewData["TagId"] = new SelectList(_context.Tags, "Id", "TagText", job.TagId);
            return View(job);
        }

        // GET: Organization/Jobs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var job = await _context.Jobs
                .Include(j => j.Organization)
                .Include(j => j.Tag)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (job == null)
            {
                return NotFound();
            }

            return View(job);
        }

        // POST: Organization/Jobs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var job = await _context.Jobs.FindAsync(id);
            _context.Jobs.Remove(job);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JobExists(int id)
        {
            return _context.Jobs.Any(e => e.Id == id);
        }
    }
}
