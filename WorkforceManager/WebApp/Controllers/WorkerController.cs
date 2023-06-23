using App.DAL;
using App.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Controllers;

public class WorkerController : Controller
{
    private readonly AppDbContext _context;

    public WorkerController(AppDbContext context)
    {
        _context = context;
    }

    // GET: Worker
    public async Task<IActionResult> Index()
    {
        var applicationDbContext =
            _context.Workers.Include(worker => worker.AppUser).Include(worker => worker.Assignment);

        return View(await applicationDbContext.ToListAsync());
    }

    // GET: Worker/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null)
            return NotFound();

        var worker = await _context.Workers
            .Include(w => w.AppUser)
            .Include(w => w.Assignment)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (worker == null)
            return NotFound();

        return View(worker);
    }

    // GET: Worker/Create
    public IActionResult Create()
    {
        ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "FirstName");
        ViewData["AssignmentId"] = new SelectList(_context.Assignments, "Id", "Description");

        return View();
    }

    // POST: Worker/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("AssignmentId,AppUserId,Id")] Worker worker)
    {
        if (ModelState.IsValid)
        {
            worker.Id = Guid.NewGuid();
            _context.Add(worker);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "FirstName", worker.AppUserId);
        ViewData["AssignmentId"] = new SelectList(_context.Assignments, "Id", "Description", worker.AssignmentId);

        return View(worker);
    }

    // GET: Worker/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null)
            return NotFound();

        var worker = await _context.Workers.FindAsync(id);
        if (worker == null)
            return NotFound();

        ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "FirstName", worker.AppUserId);
        ViewData["AssignmentId"] = new SelectList(_context.Assignments, "Id", "Description", worker.AssignmentId);

        return View(worker);
    }

    // POST: Worker/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, [Bind("AssignmentId,AppUserId,Id")] Worker worker)
    {
        if (id != worker.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(worker);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WorkerExists(worker.Id))
                    return NotFound();

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "FirstName", worker.AppUserId);
        ViewData["AssignmentId"] = new SelectList(_context.Assignments, "Id", "Description", worker.AssignmentId);

        return View(worker);
    }

    // GET: Worker/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null)
            return NotFound();

        var worker = await _context.Workers
            .Include(w => w.AppUser)
            .Include(w => w.Assignment)
            .FirstOrDefaultAsync(w => w.Id == id);

        if (worker == null)
            return NotFound();

        return View(worker);
    }

    // POST: Worker/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var worker = await _context.Workers.FindAsync(id);
        if (worker != null)
            _context.Workers.Remove(worker);

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool WorkerExists(Guid id)
    {
        return (_context.Workers?.Any(worker => worker.Id == id)).GetValueOrDefault();
    }
}