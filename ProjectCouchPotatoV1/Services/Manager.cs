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
        Task<Review> GetMovieByName(string name);
        Task<Watchlist> GetMovieWatchlist(string name);
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

        public async Task<Review> GetMovieByName(string name)
        {
            var searchResult = await SearchMovieAsync(name);

            if (searchResult?.Results?.Count > 0)
            {
                var movie = searchResult.Results[0];
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

        public async Task<Watchlist> GetMovieWatchlist(string name)
        {
            var searchResult = await SearchMovieAsync(name);

            if (searchResult?.Watchlists?.Count > 0)
            {
                var movie = searchResult.Watchlists[0];
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

        private async Task<MovieSearch> SearchMovieAsync(string name)
        {
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

                return searchResult;
            }
        }

    }
}


