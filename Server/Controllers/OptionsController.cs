using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LCPECommerce.Server.Data;
using LCPECommerce.Shared.Models;
using Microsoft.Data.SqlClient;

namespace LCPECommerce.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OptionsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OptionsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Options
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Options>>> GetOption()
        {
            return await _context.Option.ToListAsync();
        }

        // GET: api/Options/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Options>> GetOptions(int id)
        {
            var options = await _context.Option.FindAsync(id);

            if (options == null)
            {
                return NotFound();
            }

            return options;
        }

        // PUT: api/Options/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOptions(int id, Options options)
        {
            if (id != options.Id)
            {
                return BadRequest();
            }

            _context.Entry(options).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OptionsExists(id))
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

        // POST: api/Options
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Options>> PostOptions(Options options)
        {
            _context.Option.Add(options);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOptions", new { id = options.Id }, options);
        }

        // DELETE: api/Options/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOptions(int id)
        {
            var options = await _context.Option.FindAsync(id);
            if (options == null)
            {
                return NotFound();
            }

            _context.Option.Remove(options);
            await _context.SaveChangesAsync();
            ResetAI();

            return NoContent();
        }

        private void ResetAI()
        {
            var entityType = _context.Model.FindEntityType(typeof(Options));
            var tableName = entityType.GetTableName();
            var param = new SqlParameter("@tblname", tableName);
            var idlast = _context.Option.OrderByDescending(a => a.Id).FirstOrDefault();
            int rsid = (_context.Option.Count() > 0) ? idlast.Id : 0;

            _context.Database.ExecuteSqlRaw("DBCC CHECKIDENT(@tblname, RESEED, " + rsid + ")", param);
        }

        private bool OptionsExists(int id)
        {
            return _context.Option.Any(e => e.Id == id);
        }
    }
}
