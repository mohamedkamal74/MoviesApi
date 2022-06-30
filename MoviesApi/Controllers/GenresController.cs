using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesApi.Data;

namespace MoviesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public GenresController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllasync()
        {
            var genres = await _context.genres.OrderBy(g=>g.Name).ToListAsync();
            return Ok(genres);
        }
        [HttpPost]
        public async Task<IActionResult> Createasync(GenreDto dto)
        {
            var grnre = new genre { Name = dto.Name };
            await _context.genres.AddAsync(grnre);
            _context.SaveChanges();
            return Ok(grnre);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult>Updateasync(int id,[FromBody] GenreDto dto)
        {
            var genre = await _context.genres.SingleOrDefaultAsync(g => g.Id == id);
            if (genre == null)
                return NotFound($"No genre was found with Id:{id}");
            genre.Name=dto.Name;
            _context.SaveChanges();
            return Ok(genre);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Deleteasync(int id)
        {
            var genre = await _context.genres.SingleOrDefaultAsync(g => g.Id == id);
            if (genre == null)
                return NotFound($"No genre was found with Id:{id}");
            _context.genres.Remove(genre);
            _context.SaveChanges();
            return Ok(genre);

        }
    }
}
