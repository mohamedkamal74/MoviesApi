using Microsoft.EntityFrameworkCore;
using MoviesApi.Data;

namespace MoviesApi.Services
{
    public class GenreService : IGenreService
    {
        private readonly ApplicationDbContext _context;

        public GenreService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<genre> Create(genre genre)
        {
            await _context.genres.AddAsync(genre);
            _context.SaveChanges();
            return genre;
        }

        public genre Delete(genre genre)
        {
            _context.genres.Remove(genre);
            _context.SaveChanges();
            return genre;
        }

        public async Task<IEnumerable<genre>> GetAll()
        {
            return await _context.genres.OrderBy(g => g.Name).ToListAsync();
        }

        public async Task<genre> GetById(byte id)
        {
            return await _context.genres.SingleOrDefaultAsync(g => g.Id == id);
        }

        public Task<bool> IsValidGenre(byte id)
        {
            return _context.genres.AnyAsync(g => g.Id ==id);
            
        }

        public genre Update(genre genre)
        {
            _context.genres.Update(genre);
            _context.SaveChanges();
            return genre;
        }
    }
}
