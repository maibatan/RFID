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
    public class DetailsController : ControllerBase
    {
        private readonly AppContext _context;
        public DetailsController(AppContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Detail>>> Get()
        {
            return await _context.Details
                        .Include(detail => detail.AssetItemNavigation)
                        .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Detail>> Get(int id)
        {
            var expectedDetail = await _context.Details
                              .Include(detail => detail.AssetItemNavigation)
                              .Where(detail => detail.Id == id)
                              .FirstAsync();

            if (expectedDetail == null)
            {
                return NotFound();
            }

            return expectedDetail;
        }
        [HttpPost]
        public async Task<ActionResult<Detail>> Post(Detail detail)
        {
            _context.Details.Add(detail);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ExpectedDetailExists(detail.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction(nameof(Get), new { id = detail.Id }, detail);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Detail detail)
        {
            if (id != detail.Id)
            {
                return BadRequest();
            }

            _context.Entry(detail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExpectedDetailExists(id))
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
        public async Task<ActionResult<Detail>> Delete(int id)
        {
            var detail = await _context.Details.FindAsync(id);
            if (detail == null)
            {
                return NotFound();
            }

            _context.Details.Remove(detail);
            await _context.SaveChangesAsync();
            return detail;
        }

        private bool ExpectedDetailExists(int id)
        {
            return _context.Details.Any(e => e.Id == id);
        }
    }
}
