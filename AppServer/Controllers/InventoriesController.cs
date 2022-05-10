using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoriesController : ControllerBase
    {
        private readonly AppContext _context;
        public InventoriesController(AppContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Inventory>>> Get()
        {
            return  await _context.Inventories
                  .Include(inventory => inventory.Details).ThenInclude(detail => detail.AssetItemNavigation)
                  .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Inventory>> Get(int id)
        {
            var inventory = await _context.Inventories
                  .Include(inventory => inventory.Details).ThenInclude(detail => detail.AssetItemNavigation)
                 .Where(report=>report.Id==id).FirstAsync();

            if (inventory == null)
            {
                return NotFound();
            }

            return inventory;
        }
        [HttpPost]
        public async Task<ActionResult<Inventory>> Post(Inventory inventory)
        {
            _context.Inventories.Add(inventory);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (InventoryExists(inventory.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction(nameof(Get), new { id = inventory.Id }, inventory);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Inventory inventory)
        {
            if (id != inventory.Id)
            {
                return BadRequest();
            }

            _context.Entry(inventory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InventoryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<Inventory>> Delete(int id)
        {
            var inventory = await _context.Inventories.FindAsync(id);
            if (inventory == null)
            {
                return NotFound();
            }
            _context.Inventories.Remove(inventory);
            await _context.SaveChangesAsync();
            return inventory;
        }

        private bool InventoryExists(int id)
        {
            return _context.Inventories.Any(e => e.Id == id);
        }
    }
}
