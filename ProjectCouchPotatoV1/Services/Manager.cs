using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProjectCouchPotatoV1.Migrations;
using ProjectCouchPotatoV1.Models;
using System.Net.Http.Headers;

namespace ProjectCouchPotatoV1.Search
{
    public interface ITMDBService
    {
      //  Task<Movie> GetMovieById(string id);
        Task<Movie> GetMovieByName(string name);
        Task<List<Movie>> GetAutoCompleteSuggestions(string query);
    }

    public class TMDBService : ITMDBService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<TMDBService> _logger;
        private readonly MovieDbContext _dbContext;

        public TMDBService(HttpClient httpClient, ILogger<TMDBService> logger, MovieDbContext dbContext)
        {
            _httpClient = httpClient;
            _logger = logger;
            _dbContext = dbContext; 
        }

        //public async Task<Movie> GetMovieById(string id)
        //{
        //    Movie movie;

        //    using (var client = new HttpClient())
        //    {
        //        {
        //            string apiKey = "77a90b37fb8e26f0b6a321afcc05bb85";
        //            string movieId = id;

        //            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

        //            var requestUri = $"https://api.themoviedb.org/3/movie/{movieId}?api_key={apiKey}";

        //            var response = await client.GetAsync(requestUri);
        //            response.EnsureSuccessStatusCode();

        //            var body = await response.Content.ReadAsStringAsync();

        //            movie = Newtonsoft.Json.JsonConvert.DeserializeObject<Movie>(body);

        //            movie.MovieId = movieId;
        //            movie.Title = movie.Title; 
        //            movie.Overview = movie.Overview;
        //            movie.poster_path = movie.poster_path;

        //            _logger.LogInformation($"Id: {movie.MovieId}, Title: {movie.Title}, Overview: {movie.Overview}, Poster {movie.poster_path}");

        //            return movie;
        //        }
        //    }
        //}

        public async Task<Movie> GetMovieByName(string name)
        {
            Movie movie;

            using (var client = new HttpClient())
            {
                string apiKey = "77a90b37fb8e26f0b6a321afcc05bb85";
                string movieName = Uri.EscapeDataString(name);
              

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

                var requestUri = $"https://api.themoviedb.org/3/search/movie?api_key={apiKey}&query={movieName}";

                var response = await client.GetAsync(requestUri);
                response.EnsureSuccessStatusCode();

                var body = await response.Content.ReadAsStringAsync();

                var searchResult = Newtonsoft.Json.JsonConvert.DeserializeObject<MovieSearch>(body);

                if (searchResult?.Results?.Count > 0)
                {
                    movie = searchResult.Results[0]; 
                    movie.MovieId = movie.Id.ToString();

                    _logger.LogInformation($"Id: {movie.MovieId}, Title: {movie.Title}, Overview: {movie.Overview}, Poster {movie.poster_path}");

                    return movie;
                }
                else
                {
                    _logger.LogInformation($"No movie found with the name: {name}");
                    return null;
                }
            }
        }

        public async Task<List<Movie>> GetAutoCompleteSuggestions(string query)
        {
            using (var client = new HttpClient())
            {
                string apiKey = "77a90b37fb8e26f0b6a321afcc05bb85";
                string encodedQuery = Uri.EscapeDataString(query);

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

                var requestUri = $"https://api.themoviedb.org/3/search/movie?api_key={apiKey}&query={encodedQuery}";

                var response = await client.GetAsync(requestUri);
                response.EnsureSuccessStatusCode();

                var body = await response.Content.ReadAsStringAsync();

                var searchResult = Newtonsoft.Json.JsonConvert.DeserializeObject<AutoComplete>(body);

                if (searchResult?.Results?.Count > 0)
                {
                    var movies = searchResult.Results;

                    foreach (var movie in movies)
                    {
                        movie.Title = movie.Id.ToString();
                        _logger.LogInformation($"Id: {movie.MovieId}, Title: {movie.Title}, Overview: {movie.Overview}, Poster {movie.poster_path}");
                    }

                    return movies;
                }
                else
                {
                    _logger.LogInformation($"No movie found with the name: {query}");
                    return null;
                }
            }
        }


    }
}


