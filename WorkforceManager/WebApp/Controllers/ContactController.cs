using App.DAL;
using App.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Controllers;

public class ContactController : Controller
{
    private readonly AppDbContext _context;

    public ContactController(AppDbContext context)
    {
        _context = context;
    }

    // GET: Contact
    public async Task<IActionResult> Index()
    {
        var applicationDbContext = _context.Contacts.Include(contact => contact.Client);
        
        return View(await applicationDbContext.ToListAsync());
    }

    // GET: Contact/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null)
            return NotFound();

        var contact = await _context.Contacts
            .Include(c => c.Client)
            .FirstOrDefaultAsync(c => c.Id == id);
        
        if (contact == null)
            return NotFound();

        return View(contact);
    }

    // GET: Contact/Create
    public IActionResult Create()
    {
        ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Name");
        return View();
    }

    // POST: Contact/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Content,Info,Type,ClientId,Id")] Contact contact)
    {
        if (ModelState.IsValid)
        {
            contact.Id = Guid.NewGuid();
            _context.Add(contact);
            await _context.SaveChangesAsync();
            
            return RedirectToAction(nameof(Index));
        }

        ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Name", contact.ClientId);
        return View(contact);
    }

    // GET: Contact/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null)
            return NotFound();

        var contact = await _context.Contacts.FindAsync(id);
        if (contact == null)
            return NotFound();
        
        ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Name", contact.ClientId);
        return View(contact);
    }

    // POST: Contact/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, [Bind("Content,Info,Type,ClientId,Id")] Contact contact)
    {
        if (id != contact.Id)
            return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(contact);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContactExists(contact.Id))
                    return NotFound();
                
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Name", contact.ClientId);
        return View(contact);
    }

    // GET: Contact/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null)
            return NotFound();

        var contact = await _context.Contacts
            .Include(c => c.Client)
            .FirstOrDefaultAsync(c => c.Id == id);
        
        if (contact == null)
            return NotFound();

        return View(contact);
    }

    // POST: Contact/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var contact = await _context.Contacts.FindAsync(id);
        if (contact != null)
            _context.Contacts.Remove(contact);

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool ContactExists(Guid id)
    {
        return (_context.Contacts?.Any(contact => contact.Id == id)).GetValueOrDefault();
    }
}