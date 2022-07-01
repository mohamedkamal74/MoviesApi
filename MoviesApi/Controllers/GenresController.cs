using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesApi.Data;
using MoviesApi.Services;

namespace MoviesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
       
        private readonly IGenreService _genreService;

        public GenresController(IGenreService genreService)
        {
           
            _genreService = genreService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllasync()
        {
            var genres = await _genreService.GetAll();
            return Ok(genres);
        }
        [HttpPost]
        public async Task<IActionResult> Createasync(GenreDto dto)
        {
            var grnre = new genre { Name = dto.Name };
           await _genreService.Create(grnre);
            return Ok(grnre);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult>Updateasync(byte id,[FromBody] GenreDto dto)
        {
            var genre = await _genreService.GetById(id);
            if (genre == null)
                return NotFound($"No genre was found with Id:{id}");
            genre.Name=dto.Name;
           _genreService.Update(genre);
            return Ok(genre);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Deleteasync(byte id)
        {
            var genre = await _genreService.GetById(id);
            if (genre == null)
                return NotFound($"No genre was found with Id:{id}");
           _genreService.Delete(genre);
            return Ok(genre);

        }
    }
}
