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

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetMovieById(string id)
        {
            var data = await _tmdbService.GetMovieById(id);
            return View(data);
        }

        [HttpPost]
        [Route("submit")]
        [AllowAnonymous]
        public async Task<IActionResult> SaveMovieToDatabase([FromBody] Movie movie)
        {
            _dbContext.Movies.Add(movie);
            await _dbContext.SaveChangesAsync();
            return Ok(movie);
        }

        [HttpGet]
        [Route("getmovies")]
        [AllowAnonymous]
        public IActionResult GetMovies()
        {
            var movie = _dbContext.Movies.ToList();
            return Ok(movie);
        }

    }
}