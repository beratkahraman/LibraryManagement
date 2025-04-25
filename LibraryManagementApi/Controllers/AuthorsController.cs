using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryManagementApi.Data;
using LibraryManagementApi.Models;

namespace LibraryManagementApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorsController : ControllerBase
    {
        private readonly AppDbContext _db;
        public AuthorsController(AppDbContext db) => _db = db;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Author>>> GetAll() =>
            Ok(await _db.Authors.Include(a => a.Books).ToListAsync());

        [HttpGet("{id}")]
        public async Task<ActionResult<Author>> GetById(int id)
        {
            var author = await _db.Authors.Include(a => a.Books)
                                          .FirstOrDefaultAsync(a => a.Id == id);
            return author is not null ? Ok(author) : NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<Author>> Create(Author author)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            _db.Authors.Add(author);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = author.Id }, author);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Author author)
        {
            if (id != author.Id)
                return BadRequest("ID mismatch");
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _db.Entry(author).State = EntityState.Modified;
            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // önce var mý diye kontrol et
                var exists = await _db.Authors.AnyAsync(a => a.Id == id);
                if (!exists)
                    return NotFound();

                // eðer hala concurrency hatasý varsa yeniden fýrlat
                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var author = await _db.Authors.Include(a => a.Books)
                                          .FirstOrDefaultAsync(a => a.Id == id);
            if (author is null) return NotFound();
            if (author.Books.Any()) return BadRequest("Cannot delete author with existing books.");
            _db.Authors.Remove(author);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
