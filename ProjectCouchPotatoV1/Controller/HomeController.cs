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

        //GET Requests

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

        [Route("searchmovie")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Search(string name)
        {
            var data = await _tmdbService.GetMovieDataReview(name);
            return View(data);
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
            _dbContext.Watchlists.Add(watchlist);
            await _dbContext.SaveChangesAsync();
            return Ok(watchlist);
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


    }
}