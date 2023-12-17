using Microsoft.Extensions.Logging;
using ProjectCouchPotatoV1.Models;
using System.Net.Http.Headers;

namespace ProjectCouchPotatoV1.Search
{
    public interface ITMDBService
    {
        Task<Movie> GetMovieById(string id);
    }

    public class TMDBService : ITMDBService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<TMDBService> _logger;

        public TMDBService(HttpClient httpClient, ILogger<TMDBService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<Movie>GetMovieById(string id)
        {
            Movie movie;
            using (var client = new HttpClient())
            {
                {
                    string apiKey = "77a90b37fb8e26f0b6a321afcc05bb85";
                    string movieId = id;

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

                    var requestUri = $"https://api.themoviedb.org/3/movie/{movieId}?api_key={apiKey}";

                    var response = await client.GetAsync(requestUri);
                    response.EnsureSuccessStatusCode();

                    var body = await response.Content.ReadAsStringAsync();

                    movie = Newtonsoft.Json.JsonConvert.DeserializeObject<Movie>(body);

                    movie.Id = movieId;
                    movie.Title = movie.Title; 
                    movie.Overview = movie.Overview;
                    movie.poster_path = movie.poster_path;

                    _logger.LogInformation($"Id: {movie.Id}, Title: {movie.Title}, Overview: {movie.Overview}, Poster {movie.poster_path}");


                    return movie;
                }
            }
        }
    }
}


