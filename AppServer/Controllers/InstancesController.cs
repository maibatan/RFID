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
    public class InstancesController : ControllerBase
    {
        private readonly AppContext _context;
        public InstancesController(AppContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Instance>>> Get()
        {
            return await _context.Instances.Include(instance=>instance.AssetItemNavigation).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Instance>> Get(string id)
        {
            var instance = await _context.Instances
                           .Include(instance => instance.AssetItemNavigation)
                           .Include(instance => instance.DepartmentNavigation)
                           .Where(instance=> instance.TagId == id).FirstAsync();

            if (instance == null)
            {
                return NotFound();
            }

            return instance;
        }
        [HttpPost]
        public async Task<ActionResult<Instance>> Post(Instance instance)
        {
            _context.Instances.Add(instance);
            if(InstanceTagExists(instance.TagId))
            {
                return Conflict();
            }
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (InstanceExists(instance.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction(nameof(Get), new { id = instance.TagId }, instance);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Instance instance)
        {
            if (id != instance.Id)
            {
                return BadRequest();
            }
            _context.Entry(instance).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InstanceExists(id))
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
        public async Task<ActionResult<Instance>> Delete(int id)
        {
            var instance = await _context.Instances.FindAsync(id);
            if (instance == null)
            {
                return NotFound();
            }

            _context.Instances.Remove(instance);
            await _context.SaveChangesAsync();
            return instance;
        }

        private bool InstanceExists(int id)
        {
            return _context.Instances.Any(e => e.Id == id);
        }
        private bool InstanceTagExists(string tagId)
        {
            return _context.Instances.Any(e => e.TagId == tagId);
        }
    }
}
