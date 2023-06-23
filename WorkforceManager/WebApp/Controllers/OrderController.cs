using App.DAL;
using App.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Controllers;

public class OrderController : Controller
{
    private readonly AppDbContext _context;

    public OrderController(AppDbContext context)
    {
        _context = context;
    }

    // GET: Order
    public async Task<IActionResult> Index()
    {
        var applicationDbContext = _context.Orders.Include(order => order.Client);
        
        return View(await applicationDbContext.ToListAsync());
    }

    // GET: Order/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null)
            return NotFound();

        var order = await _context.Orders
            .Include(o => o.Client)
            .FirstOrDefaultAsync(o => o.Id == id);
        
        if (order == null)
            return NotFound();

        return View(order);
    }

    // GET: Order/Create
    public IActionResult Create()
    {
        ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Name");
        return View();
    }

    // POST: Order/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Name,TotalTasks,Start,End,ClientId,Id")] Order order)
    {
        if (ModelState.IsValid)
        {
            order.Id = Guid.NewGuid();
            _context.Add(order);
            await _context.SaveChangesAsync();
            
            return RedirectToAction(nameof(Index));
        }

        ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Name", order.ClientId);
        return View(order);
    }

    // GET: Order/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null)
            return NotFound();

        var order = await _context.Orders.FindAsync(id);
        if (order == null)
            return NotFound();
        
        ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Name", order.ClientId);
        return View(order);
    }

    // POST: Order/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, [Bind("Name,TotalTasks,Start,End,ClientId,Id")] Order order)
    {
        if (id != order.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(order);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(order.Id))
                    return NotFound();
                
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Name", order.ClientId);
        return View(order);
    }

    // GET: Order/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null)
            return NotFound();

        var order = await _context.Orders
            .Include(o => o.Client)
            .FirstOrDefaultAsync(o => o.Id == id);
        
        if (order == null)
            return NotFound();

        return View(order);
    }

    // POST: Order/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var order = await _context.Orders.FindAsync(id);
        if (order != null)
            _context.Orders.Remove(order);

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool OrderExists(Guid id)
    {
        return (_context.Orders?.Any(order => order.Id == id)).GetValueOrDefault();
    }
}