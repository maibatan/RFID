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
    public class AssetItemsController : ControllerBase
    {
        private readonly AppContext _context;
        public AssetItemsController(AppContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AssetItem>>> Get()
        {
            return await _context.AssetItems.ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<AssetItem>> Get(int id)
        {
            var assetItem = await _context.AssetItems.FindAsync(id);


            if (assetItem == null)
            {
                return NotFound();
            }

            return assetItem;
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, AssetItem assetItem)
        {
            if (id != assetItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(assetItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AssetItemExists(id))
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

        [HttpPost]
        public async Task<ActionResult<AssetItem>> Post(AssetItem assetItem)
        {
            _context.AssetItems.Add(assetItem);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (AssetItemExists(assetItem.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction(nameof(Get), new { id = assetItem.Id }, assetItem);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<AssetItem>> Delete(int id)
        {
            var assetItem = await _context.AssetItems.FindAsync(id);
            if (assetItem == null)
            {
                return NotFound();
            }

            _context.AssetItems.Remove(assetItem);
            await _context.SaveChangesAsync();

            return assetItem;
        }

        private bool AssetItemExists(int id)
        {
            return _context.AssetItems.Any(e => e.Id == id);
        }
    }
}
