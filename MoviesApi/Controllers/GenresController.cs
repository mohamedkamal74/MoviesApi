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
            var genres = await _context.genres.ToListAsync();
            return Ok(genres);
        }
        [HttpPost]
        public async Task<IActionResult> Createasync(CreateGenredto dto)
        {
            var grnre = new genre { Name = dto.Name };
            await _context.genres.AddAsync(grnre);
            _context.SaveChanges();
            return Ok(grnre);
        }
    }
}
