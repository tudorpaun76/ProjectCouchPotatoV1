using Microsoft.AspNetCore.Mvc;
using RestSharp;
using System.Net.Http.Headers;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using ProjectCouchPotatoV1.Models;
using ProjectCouchPotatoV1.Search;
using Microsoft.AspNetCore.Authorization;
using ProjectCouchPotatoV1.Migrations;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using ProjectCouchPotatoV1.Areas.Identity.Data;
using System.Security.Claims;
using Microsoft.Ajax.Utilities;

namespace ProjectCouchPotatoV1.Controllers
{
    [Route("movie")]
    [ApiController]
    public class HomeController : Controller
    {
        private readonly ITMDBService _tmdbService;
        private readonly ILogger<TMDBService> _logger;
        private readonly MovieDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HomeController(ITMDBService tmdbService, MovieDbContext dbContext, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _tmdbService = tmdbService;
            _dbContext = dbContext;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        [Route("discover")]
        [AllowAnonymous]
        public IActionResult Discover()
        {
            return View();
        }

        [HttpGet]
        [Route("information")]
        [AllowAnonymous]
        public IActionResult Information()
        {
            return View();
        }

        //GET Requests

        [Route("movierandom")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> RandomMovie()
        {
            try
            {
                var randomMovieId = await _dbContext.MoviesList
                    .OrderBy(x => Guid.NewGuid()) 
                    .Select(x => x.Id)
                    .FirstOrDefaultAsync();

                var randomMovie = await _dbContext.MoviesList
                    .FirstOrDefaultAsync(m => m.Id == randomMovieId);

                if (randomMovie == null)
                {
                    return NotFound(); 
                }

                return Ok(randomMovie);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Route("searchreview")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> SearchReview(string name)
        {
            var data = await _tmdbService.GetMovieDataReview(name);
            return View(data);
        }

        [Route("searchwatchlist")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> SearchWatchlist(string name)
        {
            var data = await _tmdbService.GetMovieDataWatchlist(name);
            return View(data);
        }


        [Route("searchavoid")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> SearchAvoid(string name)
        {
            var data = await _tmdbService.GetMovieDataAvoid(name);
            return View(data);
        }

        [Route("searchmovie")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Search(string name)
        {
            var data = await _tmdbService.GetMovieDataReview(name);
            return View(data);
        }


        [Route("popularmovies")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> PopularMovies()
        {
            var data = await _tmdbService.GetPopularMovies();
            return Ok(data);
        }

        [Route("upcomingmovies")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> UpcomingMovies()
        {
            var data = await _tmdbService.GetUpcomingMovies();
            return Ok(data);
        }


        [HttpGet]
        [Route("getmovies")]
        [AllowAnonymous]
        public IActionResult GetMovies()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var movies = _dbContext.Reviews.Where(r => r.UserId == userId).ToList();
            return Ok(movies);
        }

        [HttpGet]
        [Route("getwatchlist")]
        [AllowAnonymous]
        public IActionResult GetWatchlist()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var movies = _dbContext.Watchlists.Where(r => r.UserId == userId).ToList();
            return Ok(movies);
        }

        [HttpGet]
        [Route("getavoidmovies")]
        [AllowAnonymous]
        public IActionResult GetAvoidMovies()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var movies = _dbContext.MovieToAvoid.Where(r => r.UserId == userId).ToList();
            return Ok(movies);
        }

        [Route("searchresult")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetSearchResult(string name)
        {
            var data = await _tmdbService.GetSearchResults(name);
            return Ok(data);
        }

        //POST Requests

        [HttpPost]
        [Route("review")]
        [AllowAnonymous]
        public async Task<IActionResult> SaveReviewToDatabase([FromBody] Review review)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            review.UserId = userId;

            var existingReview = await _dbContext.Reviews
                .FirstOrDefaultAsync(r => r.MovieId == review.MovieId && r.UserId == userId);

            if (existingReview != null)
            {
                return Conflict("A review for this movie by the current user already exists.");
            }

            _dbContext.Reviews.Add(review);
            await _dbContext.SaveChangesAsync();
            return Ok(review);
        }

        [HttpPost]
        [Route("watchlist")]
        [AllowAnonymous]
        public async Task<IActionResult> SaveWatchlistToDatabase([FromBody] Watchlist watchlist)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            watchlist.UserId = userId;

            var existingWatchlist = await _dbContext.Watchlists
                .FirstOrDefaultAsync(w => w.MovieId == watchlist.MovieId && w.UserId == userId);

            if (existingWatchlist != null)
            {
                return Conflict("The movie is already in your watchlist.");
            }

            _dbContext.Watchlists.Add(watchlist);
            await _dbContext.SaveChangesAsync();
            return Ok(watchlist);
        }


        [HttpPost]
        [Route("avoid")]
        [AllowAnonymous]
        public async Task<IActionResult> SaveMovieAvoidToDatabase([FromBody] MovieAvoid avoid)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            avoid.UserId = userId;

            var existingAvoid = await _dbContext.MovieToAvoid
                .FirstOrDefaultAsync(a => a.MovieId == avoid.MovieId && a.UserId == userId);

            if (existingAvoid != null)
            {
                return Conflict("The movie is already marked to avoid.");
            }

            _dbContext.MovieToAvoid.Add(avoid);
            await _dbContext.SaveChangesAsync();
            return Ok(avoid);
        }


        [HttpPut]
        [Route("editreview")]
        public async Task<IActionResult> EditReview([FromBody] Review updatedReview, int movieId)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var existingReview = await _dbContext.Reviews.FirstOrDefaultAsync(r => r.UserId == userId && r.Id == movieId);

            if (existingReview == null)
            {
                return NotFound("Review not found.");
            }

            existingReview.ReviewText = updatedReview.ReviewText;

            _dbContext.Reviews.Update(existingReview);
            await _dbContext.SaveChangesAsync();

            return Ok(existingReview);
        }


        //DELETE Requests

        [HttpDelete]
        [Route("deletereview")]
        [AllowAnonymous]
        public IActionResult DeleteReview(int movieid)
        {
            var movie = _dbContext.Reviews.Find(movieid);
            if (movie == null)
            {
                return NotFound();
            }
            _dbContext.Reviews.Remove(movie);
            _dbContext.SaveChanges();

            return Ok(movie);
        }

        [HttpDelete]
        [Route("deletewatchlist")]
        [AllowAnonymous]
        public IActionResult DeleteWatchlist(int movieid)
        {
            var movie = _dbContext.Watchlists.Find(movieid);
            if (movie == null)
            {
                return NotFound();
            }
            _dbContext.Watchlists.Remove(movie);
            _dbContext.SaveChanges();

            return Ok(movie);
        }

        [HttpDelete]
        [Route("deleteavoid")]
        [AllowAnonymous]
        public IActionResult DeleteAvoid(int movieid)
        {
            var movie = _dbContext.MovieToAvoid.Find(movieid);
            if (movie == null)
            {
                return NotFound();
            }
            _dbContext.MovieToAvoid.Remove(movie);
            _dbContext.SaveChanges();

            return Ok(movie);
        }


    }
}