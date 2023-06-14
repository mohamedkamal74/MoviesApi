using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesApi.Data;
using MoviesApi.Services;

namespace MoviesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMoviesService _moviesService;
        private readonly IGenreService _genreService;
        private new List<string> _allowedExtention = new List<string> { ".jpg", ".png" };
        private long _maxallowedPosterSize =1048576;


        public MoviesController(IMoviesService moviesService,IGenreService genreService)
        {
            _moviesService = moviesService;
            _genreService = genreService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var movies = await _moviesService.GetAll();
            return Ok(movies);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var movie = await _moviesService.GetById(id);
            if (movie == null)
                return NotFound($"No Movie with this Id: {id}");
            return Ok(movie);
        }

        [HttpGet("GetByGenreId")]
        public async Task<IActionResult> GetByGenreIdAsync(byte genreId)
        {
            var movies = await _moviesService.GetAll(genreId);
            return Ok(movies);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromForm] MovieDto dto)
        {
            if(dto.Poster ==null)
                return BadRequest("Poster is required");
            if (!_allowedExtention.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                return BadRequest("only .jpg,.png movies are alloewd !");

            if(dto.Poster.Length>_maxallowedPosterSize)
                return BadRequest("max size for poster image is 1 mega byte !");

            var isValidGenre = await _genreService.IsValidGenre(dto.GenreId);
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
          await _moviesService.Create(movie);
            return Ok(movie);


        }
        [HttpPut("{id}")]
        public async Task<IActionResult>UpdateAsync(int id,[FromForm] MovieDto dto)
        {
            var movie = await _moviesService.GetById(id);
            if (movie == null)
                return NotFound($"No movie found with Id: {id}");
            var isValidGenre = await _genreService.IsValidGenre(dto.GenreId);
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

            _moviesService.Update(movie);
            return Ok(movie);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult>DeleteMovie(int id)
        {
            var movie = await _moviesService.GetById(id);
            if (movie == null)
                return NotFound($"No movie found with Id: {id}");
            _moviesService.Delete(movie);
            return Ok(movie);

        }
    }
}
