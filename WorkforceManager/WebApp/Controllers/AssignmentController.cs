using App.DAL;
using App.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Controllers;

public class AssignmentController : Controller
{
    private readonly AppDbContext _context;

    public AssignmentController(AppDbContext context)
    {
        _context = context;
    }

    // GET: Assignments
    public async Task<IActionResult> Index()
    {
        return View(await _context.Assignments.ToListAsync());
    }

    // GET: Assignments/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null)
            return NotFound();

        var assignment = await _context.Assignments.FirstOrDefaultAsync(c => c.Id == id);
        if (assignment == null)
            return NotFound();

        return View(assignment);
    }

    // GET: Assignments/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Assignments/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Assignment assignment)
    {
        if (ModelState.IsValid)
        {
            assignment.Id = Guid.NewGuid();
            _context.Add(assignment);
            await _context.SaveChangesAsync();
            
            return RedirectToAction(nameof(Index));
        }

        return View(assignment);
    }

    // GET: Assignments/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null)
            return NotFound();

        var assignment = await _context.Assignments.FindAsync(id);
        if (assignment == null)
            return NotFound();

        return View(assignment);
    }

    // POST: Assignments/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, Assignment assignment)
    {
        if (id != assignment.Id)
            return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(assignment);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientExists(assignment.Id))
                    return NotFound();

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        return View(assignment);
    }

    // GET: Assignments/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null)
            return NotFound();

        var assignment = await _context.Assignments.FirstOrDefaultAsync(c => c.Id == id);
        if (assignment == null)
            return NotFound();

        return View(assignment);
    }

    // POST: Assignments/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var client = await _context.Clients.FindAsync(id);
        if (client != null)
            _context.Clients.Remove(client);

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    
    private bool ClientExists(Guid id)
    {
        return (_context.Assignments?.Any(assignment => assignment.Id == id)).GetValueOrDefault();
    }
}