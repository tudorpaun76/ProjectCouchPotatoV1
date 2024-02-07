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

        //[Route("search")]
        //[HttpGet]
        //[AllowAnonymous]
        //public async Task<IActionResult> GetMovieById(string id)
        //{
        //    var data = await _tmdbService.GetMovieById(id);
        //    return View(data);
        //}


        //[HttpGet]
        //[AllowAnonymous]
        //public async Task<IActionResult> GetAutoCompleteSuggestions(string query) 
        //{
        //    var data = await _tmdbService.GetAutoCompleteSuggestions(query);
        //    return View(data);
        //}

        [Route("search")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetMovieByName(string name)
        {
            var data = await _tmdbService.GetMovieByName(name);
            return View(data);
        }

        [HttpPost]
        [Route("submit")]
        [AllowAnonymous]
        public async Task<IActionResult> SaveMovieToDatabase([FromBody] Movie watchlist)
        {
            _dbContext.Reviews.Add(watchlist);
            await _dbContext.SaveChangesAsync();
            return Ok(watchlist);
        }

        [HttpGet]
        [Route("getmovies")]
        [AllowAnonymous]
        public IActionResult GetMovies()
        {
            var movie = _dbContext.Reviews.ToList();
            return Ok(movie);
        }

        [HttpDelete]
        [Route("delete")]
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



        //FIX THIS
        [HttpPut]
        [Route("update")]
        [AllowAnonymous]
        public IActionResult UpdateReview([FromBody] Movie updatedReview)
        {
            var movie = _dbContext.Reviews.Find(updatedReview);
            if (movie == null)
            {
                return NotFound();
            }
            _dbContext.Reviews.Update(movie);
            _dbContext.SaveChanges();
            return Ok(movie);
        }




    }
}