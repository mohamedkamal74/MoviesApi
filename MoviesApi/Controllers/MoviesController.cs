using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesApi.Data;

namespace MoviesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private new List<string> _allowedExtention = new List<string> { ".jpg", ".png" };
        private long _maxallowedPosterSize =1048576;


        public MoviesController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var movies = await _context.Movies.Include(g=>g.genre).OrderByDescending(m=>m.Rate).ToListAsync();
            return Ok(movies);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var movie = await _context.Movies.Include(g=>g.genre).SingleOrDefaultAsync(m=>m.Id==id);
            if (movie == null)
                return NotFound($"No Movie with this Id: {id}");
            return Ok(movie);
        }

        [HttpGet("GetByGenreId")]
        public async Task<IActionResult> GetByGenreIdAsync(byte genreId)
        {
            var movies = await _context.Movies.Where(m=>m.GenreId==genreId).Include(g => g.genre).OrderByDescending(m => m.Rate).ToListAsync();
            return Ok(movies);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromForm] MovieDto dto)
        {
            if(dto.Poster ==null)
                return BadRequest("Pster is required");
            if (!_allowedExtention.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                return BadRequest("only .jpg,.png movies are alloewd !");

            if(dto.Poster.Length>_maxallowedPosterSize)
                return BadRequest("max size for poster image is 1 mega byte !");

            var isValidGenre = await _context.genres.AnyAsync(g => g.Id == dto.GenreId);
            if(!isValidGenre)
                return BadRequest("Invalid Genre Id !");


            using var datastream=new MemoryStream();
            await dto.Poster.CopyToAsync(datastream);
            var movie = new Movie
            {
                GenreId = dto.GenreId,
                Title = dto.Title,
                Poster= datastream.ToArray(),
                Rate = dto.Rate,
                StoreLine = dto.StoreLine,
                Year = dto.Year
            };
            await _context.Movies.AddAsync(movie);
            _context.SaveChanges();
            return Ok(movie);


        }
        [HttpPut("{id}")]
        public async Task<IActionResult>UpdateAsync(int id,[FromForm] MovieDto dto)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
                return NotFound($"No movie found with Id: {id}");
            var isValidGenre = await _context.genres.AnyAsync(g => g.Id == dto.GenreId);
            if (!isValidGenre)
                return BadRequest("Invalid Genre Id !");

            if(dto.Poster != null)
            {
                if (!_allowedExtention.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                    return BadRequest("only .jpg,.png movies are alloewd !");

                if (dto.Poster.Length > _maxallowedPosterSize)
                    return BadRequest("max size for poster image is 1 mega byte !");

                using var datastream = new MemoryStream();
                await dto.Poster.CopyToAsync(datastream);
                movie.Poster=datastream.ToArray();
            }
            movie.Title = dto.Title;
            movie.Year = dto.Year;
            movie.GenreId = dto.GenreId;
            movie.StoreLine = dto.StoreLine;
            movie.Rate = dto.Rate;

            _context.SaveChanges();
            return Ok(movie);

                

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult>DeleteMovie(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
                return NotFound($"No movie found with Id: {id}");
            _context.Movies.Remove(movie);
            _context.SaveChanges();
            return Ok(movie);

        }
    }
}
