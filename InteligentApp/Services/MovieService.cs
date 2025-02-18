using InteligentApp.Components.Models;

namespace InteligentApp.Services
{
    public class MovieService : IMovieService
    {
        private const string CSV_PATH = "wwwroot/movies.csv";

        public List<Movie> GetMovies()
        {
            List<Movie> movies = new List<Movie>();

            if (File.Exists(CSV_PATH))
            {
                IEnumerable<string> lines = File.ReadAllLines(CSV_PATH).Skip(1);
                foreach (var line in lines)
                {
                    string[] values = line.Split(',');
                    if (values.Length == 3)
                    {
                        movies.Add(new Movie
                        {
                            Title = values[0].Trim('"'),
                            Year = int.Parse(values[1]),
                            Genre = values[2]
                        });
                    }
                }
            }
            return movies;
        }
    }

    public interface IMovieService
    {
        List<Movie> GetMovies();
    }
}