namespace MoviesApi.Dtos
{
    public class Genredto
    {
        [MaxLength(100)]
        public string Name { get; set; }
    }
}
