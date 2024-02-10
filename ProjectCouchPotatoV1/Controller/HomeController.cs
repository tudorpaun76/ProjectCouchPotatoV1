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

namespace ProjectCouchPotatoV1.Controllers
{
    [Route("movie")]
    [ApiController]
    public class HomeController : Controller
    {
        private readonly ITMDBService _tmdbService;
        private readonly ILogger<TMDBService> _logger;
        private readonly MovieDbContext _dbContext;

        public HomeController(ITMDBService tmdbService, MovieDbContext dbContext)
        {
            _tmdbService = tmdbService;
            _dbContext = dbContext;
        }

        [Route("review")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetMovieByName(string name)
        {
            var data = await _tmdbService.GetMovieByName(name);
            return View(data);
        }

        [Route("searchwatchlist")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> SearchWatchlist(string name)
        {
            var data = await _tmdbService.GetMovieByName(name);
            return View(data);
        }

        [Route("search")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Search(string name)
        {
            var data = await _tmdbService.GetMovieByName(name);
            return View(data);
        }

        [HttpPost]
        [Route("submit")]
        [AllowAnonymous]
        public async Task<IActionResult> SaveMovieToDatabase([FromBody] Review review)
        {
            _dbContext.Reviews.Add(review);
            await _dbContext.SaveChangesAsync();
            return Ok(review);
        }

        [HttpGet]
        [Route("getmovies")]
        [AllowAnonymous]
        public IActionResult GetMovies()
        {
            var movie = _dbContext.Reviews.ToList();
            return Ok(movie);
        }

        [HttpPost]
        [Route("watchlist")]
        [AllowAnonymous]
        public async Task<IActionResult> SaveWatchlistToDatabase([FromBody] Watchlist watchlist)
        {
            _dbContext.Watchlists.Add(watchlist);
            await _dbContext.SaveChangesAsync();
            return Ok(watchlist);
        }

        [HttpGet]
        [Route("getwatchlist")]
        [AllowAnonymous]
        public IActionResult GetWatchlist ()
        {
            var watchlist = _dbContext.Watchlists.ToList();
            return Ok(watchlist);
        }

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