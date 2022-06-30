namespace MoviesApi.Dtos
{
    public class CreateGenredto
    {
        [MaxLength(100)]
        public string Name { get; set; }
    }
}
