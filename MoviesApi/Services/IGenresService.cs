namespace MoviesApi.Services
{
    public interface IGenreService
    {
        Task<IEnumerable<genre>> GetAll();
        Task<genre> GetById(byte id);
        Task<genre> Create(genre genre);
        genre Update(genre genre);
        genre Delete(genre genre);
        Task<bool> IsValidGenre(byte id );
    }
}
