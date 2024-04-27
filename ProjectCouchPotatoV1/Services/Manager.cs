using Hangfire;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using NuGet.Packaging;
using ProjectCouchPotatoV1.Migrations;
using ProjectCouchPotatoV1.Models;
using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;

namespace ProjectCouchPotatoV1.Search
{
    public interface ITMDBService
    {
        Task<Review> GetMovieDataReview(string name);

        Task<Watchlist> GetMovieDataWatchlist(string name);

        Task<List<AutoCompleteResult>> GetSearchResults(string name);

        Task<List<Popular>> GetPopularMovies();

        Task<List<Upcoming>> GetUpcomingMovies();

        Task<MovieAvoid> GetMovieDataAvoid(string name);

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
            //RecurringJob.AddOrUpdate(() => ScrapeMoviesFromApi(1), Cron.Daily);
            //RecurringJob.AddOrUpdate(() => AddScrapedMoviesToDatabase(), Cron.Daily);
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

        public async Task<MovieAvoid> GetMovieDataAvoid(string name)
        {
            var searchResult = await SearchMovieAsync(name);

            if (searchResult?.MovieToAvoid?.Count > 0)
            {
                var movie = searchResult.MovieToAvoid[0];
                movie.MovieId = movie.Id.ToString();

                _logger.LogInformation($"Id: {movie.MovieId}, Title: {movie.Title}, Overview: {movie.Overview}, Poster {movie.poster_path}");

                return movie;
            }
            else
            {
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


        //public async Task<List<MovieData>> ScrapeMoviesFromApi(int pageCount)
        //{
        //    string apiKey = "xxx";
        //    List<MovieData> scrapedMovies = new List<MovieData>();

        //    for (int page = 1; page <= pageCount; page++)
        //    {
        //        string requestUri = $"https://api.themoviedb.org/3/movie/top_rated?api_key={apiKey}&page={page}";

        //        using (var client = new HttpClient())
        //        {
        //            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //            var response = await client.GetAsync(requestUri);
        //            response.EnsureSuccessStatusCode();

        //            var body = await response.Content.ReadAsStringAsync();
        //            var json = JObject.Parse(body);

        //            var Movies = json["results"].ToObject<List<MovieData>>();

        //            scrapedMovies.AddRange(Movies.Select(movie => new MovieData
        //            {
        //                MovieId = movie.Id.ToString(),
        //                Title = movie.Title,
        //                Overview = movie.Overview,
        //                poster_path = movie.poster_path
        //            }));

        //        }
        //    }
        //    return scrapedMovies;
        //}

        //public async Task AddScrapedMoviesToDatabase()
        //{
        //    try
        //    {
        //        int pageCount = 10;
        //        var topMovies = await ScrapeMoviesFromApi(pageCount);

        //        foreach (var movie in topMovies)
        //        {
        //            _dbContext.MoviesList.Add(movie);
        //        }
        //        await _dbContext.SaveChangesAsync();

        //        _logger.LogInformation("Top movies scraped and saved successfully.");
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"Error occurred while scraping and saving top movies: {ex.Message}");
        //    }
        //}


        private async Task<MovieSearch> SearchMovieAsync(string name)
        {
            using (var client = new HttpClient())
            {
                IConfiguration config = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .Build();

                string apiKey = config.GetSection("API_KEY")["ApiKey"];
                string movieName = Uri.EscapeDataString(name);

                _logger.LogInformation(apiKey);

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

                var requestUri = $"https://api.themoviedb.org/3/search/movie?api_key={apiKey}&query={movieName}&page=1&include_adult=false&append_to_response=videos";


                var response = await client.GetAsync(requestUri);
                response.EnsureSuccessStatusCode();

                var body = await response.Content.ReadAsStringAsync();

                var searchResult = new MovieSearch();

                var json = JObject.Parse(body);

                searchResult.Results = json["results"].ToObject<List<Review>>();

                searchResult.Watchlists = json["results"].ToObject<List<Watchlist>>();

                searchResult.MovieToAvoid = json["results"].ToObject<List<MovieAvoid>>();

                searchResult.autoCompleteResults = json["results"].ToObject<List<AutoCompleteResult>>();

                foreach (var movie in searchResult.Results)
                {
                    var movieId = movie.Id;
                    var trailerRequestUri = $"https://api.themoviedb.org/3/movie/{movieId}/videos?api_key={apiKey}";
                    var trailerResponse = await client.GetAsync(trailerRequestUri);
                    trailerResponse.EnsureSuccessStatusCode();
                    var trailerBody = await trailerResponse.Content.ReadAsStringAsync();
                    var trailerJson = JObject.Parse(trailerBody);
                    movie.Trailers = trailerJson["results"].ToObject<List<Trailer>>();

                    var castRequestUri = $"https://api.themoviedb.org/3/movie/{movieId}/credits?api_key={apiKey}";
                    var castResponse = await client.GetAsync(castRequestUri);
                    castResponse.EnsureSuccessStatusCode();
                    var castBody = await castResponse.Content.ReadAsStringAsync();
                    var castJson = JObject.Parse(castBody);
                    movie.Cast = castJson["cast"].ToObject<List<Cast>>();
                }


                return searchResult;
            }
        }


        private async Task<MovieSearch> GetPopularMoviesAsync()
        {

            using (var client = new HttpClient())
            {
                IConfiguration config = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .Build();

                string apiKey = config.GetSection("API_KEY")["ApiKey"];
                _logger.LogInformation(apiKey);

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

                var requestUri = $"https://api.themoviedb.org/3/movie/popular?api_key={apiKey}";


                var response = await client.GetAsync(requestUri);
                response.EnsureSuccessStatusCode();

                var body = await response.Content.ReadAsStringAsync();

                var searchResult = new MovieSearch();

                var json = JObject.Parse(body);

                searchResult.Popular = json["results"].ToObject<List<Popular>>();

                return searchResult;

            }
        }

        public async Task<List<Popular>> GetPopularMovies()
        {
            var searchResult = await GetPopularMoviesAsync();
            var popularMovies = new List<Popular>();

            if (searchResult?.Popular?.Count > 0)
            {
                foreach (var movie in searchResult.Popular)
                {
                    movie.MovieId = movie.Id.ToString();
                    _logger.LogInformation($"Id: {movie.MovieId}, Title: {movie.Title}, Overview: {movie.Overview}, Poster {movie.poster_path}");
                    popularMovies.Add(movie);
                }

                return popularMovies;
            }
            else
            {
                _logger.LogInformation($"No movies found");
                return null;
            }
        }

        private async Task<MovieSearch> GetUpcomingMoviesAsync()
        {

            using (var client = new HttpClient())
            {
                IConfiguration config = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .Build();

                string apiKey = config.GetSection("API_KEY")["ApiKey"];
                _logger.LogInformation(apiKey);

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

                var requestUri = $"https://api.themoviedb.org/3/movie/upcoming?api_key={apiKey}";


                var response = await client.GetAsync(requestUri);
                response.EnsureSuccessStatusCode();

                var body = await response.Content.ReadAsStringAsync();

                var searchResult = new MovieSearch();

                var json = JObject.Parse(body);

                searchResult.Upcoming = json["results"].ToObject<List<Upcoming>>();

                return searchResult;

            }
        }

        public async Task<List<Upcoming>> GetUpcomingMovies()
        {
            var searchResult = await GetUpcomingMoviesAsync();
            var popularMovies = new List<Upcoming>();

            if (searchResult?.Upcoming?.Count > 0)
            {
                foreach (var movie in searchResult.Upcoming)
                {
                    movie.MovieId = movie.Id.ToString();
                    _logger.LogInformation($"Id: {movie.MovieId}, Title: {movie.Title}, Overview: {movie.Overview}, Poster {movie.poster_path}");
                    popularMovies.Add(movie);
                }

                return popularMovies;
            }
            else
            {
                _logger.LogInformation($"No movies found");
                return null;
            }
        }


    }
}
