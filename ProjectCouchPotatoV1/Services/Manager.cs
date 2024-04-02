using Hangfire;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using ProjectCouchPotatoV1.Migrations;
using ProjectCouchPotatoV1.Models;
using System.Net.Http.Headers;

namespace ProjectCouchPotatoV1.Search
{
    public interface ITMDBService
    {
        Task<Review> GetMovieDataReview(string name);

        Task<Watchlist> GetMovieDataWatchlist(string name);

        Task<List<AutoCompleteResult>> GetSearchResults(string name);
        
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
            RecurringJob.AddOrUpdate(() => ScrapeMoviesFromApi(1), Cron.Daily);
            RecurringJob.AddOrUpdate(() => AddScrapedMoviesToDatabase(), Cron.Daily);
        }

        public async Task<Review> GetMovieDataReview(string name)
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

        public async Task<Watchlist> GetMovieDataWatchlist(string name)
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

        public async Task<List<AutoCompleteResult>> GetSearchResults(string name)
        {
            var searchResult = await SearchMovieAsync(name);
            List<AutoCompleteResult> movies = new List<AutoCompleteResult>();

            if (searchResult?.autoCompleteResults?.Count > 0)
            {
                foreach (var movie in searchResult.autoCompleteResults)
                {
                    movie.MovieId = movie.Id.ToString();
                    _logger.LogInformation($"Id: {movie.MovieId}, Title: {movie.Title}");
                    movies.Add(movie);
                }
            }
            else
            {
                _logger.LogInformation($"No movies found with the name: {name}");
            }

            return movies;
        }


        public async Task<List<MovieData>> ScrapeMoviesFromApi(int pageCount)
        {
            string apiKey = "77a90b37fb8e26f0b6a321afcc05bb85";
            List<MovieData> scrapedMovies = new List<MovieData>();

            for (int page = 1; page <= pageCount; page++)
            {
                string requestUri = $"https://api.themoviedb.org/3/movie/top_rated?api_key={apiKey}&page={page}";

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var response = await client.GetAsync(requestUri);
                    response.EnsureSuccessStatusCode();

                    var body = await response.Content.ReadAsStringAsync();
                    var json = JObject.Parse(body);

                    var Movies = json["results"].ToObject<List<MovieData>>();

                    scrapedMovies.AddRange(Movies.Select(movie => new MovieData
                    {
                        MovieId = movie.Id.ToString(),
                        Title = movie.Title,
                        Overview = movie.Overview,
                        poster_path = movie.poster_path
                    }));

                }
            }
            return scrapedMovies;
        }

        public async Task AddScrapedMoviesToDatabase()
        {
            try
            {
                int pageCount = 10;
                var topMovies = await ScrapeMoviesFromApi(pageCount);

                foreach (var movie in topMovies)
                {
                    _dbContext.MoviesList.Add(movie);
                }
                await _dbContext.SaveChangesAsync();

                _logger.LogInformation("Top movies scraped and saved successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while scraping and saving top movies: {ex.Message}");
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

                var requestUri = $"https://api.themoviedb.org/3/search/movie?api_key={apiKey}&query={movieName}&page=1&include_adult=false";


                var response = await client.GetAsync(requestUri);
                response.EnsureSuccessStatusCode();

                var body = await response.Content.ReadAsStringAsync();

                var searchResult = new MovieSearch();

                var json = JObject.Parse(body);

                searchResult.Results = json["results"].ToObject<List<Review>>();

                searchResult.Watchlists = json["results"].ToObject<List<Watchlist>>();

                searchResult.autoCompleteResults = json["results"].ToObject<List<AutoCompleteResult>>();

                return searchResult;


            }
        }

    }
}
