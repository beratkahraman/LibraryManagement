using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryManagementApi.Data;
using LibraryManagementApi.Models;

namespace LibraryManagementApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly AppDbContext _db;
        public BooksController(AppDbContext db) => _db = db;

        // GET: api/books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetAll()
        {
            var books = await _db.Books
                                 .Include(b => b.Author)
                                 .ToListAsync();
            return Ok(books);
        }

        // GET: api/books/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetById(int id)
        {
            var book = await _db.Books
                                .Include(b => b.Author)
                                .FirstOrDefaultAsync(b => b.Id == id);
            if (book == null) return NotFound();
            return Ok(book);
        }

        // POST: api/books
        [HttpPost]
        public async Task<ActionResult<Book>> Create(Book book)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Yazar var mý kontrolü
            var authorExists = await _db.Authors.AnyAsync(a => a.Id == book.AuthorId);
            if (!authorExists)
                return BadRequest($"Author with Id={book.AuthorId} does not exist.");

            _db.Books.Add(book);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = book.Id }, book);
        }

        // PUT: api/books/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Book book)
        {
            if (id != book.Id)
                return BadRequest("ID mismatch");
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Yazar var mý kontrolü
            var authorExists = await _db.Authors.AnyAsync(a => a.Id == book.AuthorId);
            if (!authorExists)
                return BadRequest($"Author with Id={book.AuthorId} does not exist.");

            _db.Entry(book).State = EntityState.Modified;
            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Kitap gerçekten var mý
                var exists = await _db.Books.AnyAsync(b => b.Id == id);
                if (!exists)
                    return NotFound();
                throw;
            }

            return NoContent();
        }

        // DELETE: api/books/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var book = await _db.Books.FindAsync(id);
            if (book == null)
                return NotFound();

            _db.Books.Remove(book);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
