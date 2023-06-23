using App.DAL;
using App.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Controllers;

public class ClientController : Controller
{
    private readonly AppDbContext _context;

    public ClientController(AppDbContext context)
    {
        _context = context;
    }

    // GET: Client
    public async Task<IActionResult> Index()
    {
        return View(await _context.Clients.ToListAsync());
    }

    // GET: Client/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null)
            return NotFound();

        var client = await _context.Clients.FirstOrDefaultAsync(c => c.Id == id);
        if (client == null)
            return NotFound();

        return View(client);
    }

    // GET: Client/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Client/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Name,TotalOrders,TotalTasks,Id")] Client client)
    {
        if (ModelState.IsValid)
        {
            client.Id = Guid.NewGuid();
            _context.Add(client);
            await _context.SaveChangesAsync();
            
            return RedirectToAction(nameof(Index));
        }

        return View(client);
    }

    // GET: Client/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null)
            return NotFound();

        var client = await _context.Clients.FindAsync(id);
        if (client == null)
            return NotFound();

        return View(client);
    }

    // POST: Client/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, [Bind("Name,TotalOrders,TotalTasks,Id")] Client client)
    {
        if (id != client.Id)
            return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(client);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientExists(client.Id))
                    return NotFound();

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        return View(client);
    }

    // GET: Client/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null)
            return NotFound();

        var client = await _context.Clients.FirstOrDefaultAsync(c => c.Id == id);
        if (client == null)
            return NotFound();

        return View(client);
    }

    // POST: Client/Delete/5
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
        return (_context.Clients?.Any(client => client.Id == id)).GetValueOrDefault();
    }
}