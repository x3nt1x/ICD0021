using App.DAL;
using App.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Controllers;

public class CommentController : Controller
{
    private readonly AppDbContext _context;

    public CommentController(AppDbContext context)
    {
        _context = context;
    }

    // GET: Comment
    public async Task<IActionResult> Index()
    {
        var applicationDbContext = _context.Comments.Include(comment => comment.AppUser).Include(comment => comment.Assignment);
        
        return View(await applicationDbContext.ToListAsync());
    }

    // GET: Comment/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null)
            return NotFound();

        var comment = await _context.Comments
            .Include(c => c.AppUser)
            .Include(c => c.Assignment)
            .FirstOrDefaultAsync(m => m.Id == id);
        
        if (comment == null)
            return NotFound();

        return View(comment);
    }

    // GET: Comment/Create
    public IActionResult Create()
    {
        ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "FirstName");
        ViewData["AssignmentId"] = new SelectList(_context.Assignments, "Id", "Description");
        
        return View();
    }

    // POST: Comment/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Content,Date,AssignmentId,AppUserId,Id")] Comment comment)
    {
        if (ModelState.IsValid)
        {
            comment.Id = Guid.NewGuid();
            _context.Add(comment);
            await _context.SaveChangesAsync();
            
            return RedirectToAction(nameof(Index));
        }

        ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "FirstName", comment.AppUserId);
        ViewData["AssignmentId"] = new SelectList(_context.Assignments, "Id", "Description", comment.AssignmentId);
        
        return View(comment);
    }

    // GET: Comment/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null)
            return NotFound();

        var comment = await _context.Comments.FindAsync(id);
        if (comment == null)
            return NotFound();
        
        ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "FirstName", comment.AppUserId);
        ViewData["AssignmentId"] = new SelectList(_context.Assignments, "Id", "Description", comment.AssignmentId);
        
        return View(comment);
    }

    // POST: Comment/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, [Bind("Content,Date,AssignmentId,AppUserId,Id")] Comment comment)
    {
        if (id != comment.Id)
            return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(comment);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommentExists(comment.Id))
                    return NotFound();
                
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "FirstName", comment.AppUserId);
        ViewData["AssignmentId"] = new SelectList(_context.Assignments, "Id", "Description", comment.AssignmentId);
       
        return View(comment);
    }

    // GET: Comment/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null)
            return NotFound();

        var comment = await _context.Comments
            .Include(c => c.AppUser)
            .Include(c => c.Assignment)
            .FirstOrDefaultAsync(c => c.Id == id);
        
        if (comment == null)
            return NotFound();

        return View(comment);
    }

    // POST: Comment/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var comment = await _context.Comments.FindAsync(id);
        if (comment != null)
            _context.Comments.Remove(comment);

        await _context.SaveChangesAsync();
        
        return RedirectToAction(nameof(Index));
    }

    private bool CommentExists(Guid id)
    {
        return (_context.Comments?.Any(comment => comment.Id == id)).GetValueOrDefault();
    }
}