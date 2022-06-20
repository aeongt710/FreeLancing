using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FreeLancing.Data;
using FreeLancing.Models;
using FreeLancing.Utility;

namespace FreeLancing.Areas.Tag.Controllers
{
    public class CustomTagsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CustomTagsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Tag/CustomTags
        public async Task<IActionResult> Index(int? pageNumber)
        {
            //var list=await _context.CustomTags.ToListAsync();

            int pageSize = 3;
            return View(await PaginatedList<CustomTag>.CreateAsync(_context.CustomTags.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Tag/CustomTags/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customTag = await _context.CustomTags
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customTag == null)
            {
                return NotFound();
            }

            return View(customTag);
        }

        // GET: Tag/CustomTags/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tag/CustomTags/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TagText")] CustomTag customTag)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customTag);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(customTag);
        }

        // GET: Tag/CustomTags/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customTag = await _context.CustomTags.FindAsync(id);
            if (customTag == null)
            {
                return NotFound();
            }
            return View(customTag);
        }

        // POST: Tag/CustomTags/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TagText")] CustomTag customTag)
        {
            if (id != customTag.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customTag);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomTagExists(customTag.Id))
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
            return View(customTag);
        }

        // GET: Tag/CustomTags/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customTag = await _context.CustomTags
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customTag == null)
            {
                return NotFound();
            }

            return View(customTag);
        }

        // POST: Tag/CustomTags/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customTag = await _context.CustomTags.FindAsync(id);
            _context.CustomTags.Remove(customTag);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomTagExists(int id)
        {
            return _context.CustomTags.Any(e => e.Id == id);
        }
    }
}
