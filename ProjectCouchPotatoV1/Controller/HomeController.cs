using Microsoft.AspNetCore.Mvc;
using RestSharp;
using System.Net.Http.Headers;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using ProjectCouchPotatoV1.Models;
using ProjectCouchPotatoV1.Search;
using Microsoft.AspNetCore.Authorization;

namespace ProjectCouchPotatoV1.Controllers
{
    [Route("movie")]
    [ApiController]
    public class HomeController : Controller
    {
        private readonly ITMDBService _tmdbService;
        private readonly ILogger<TMDBService> _logger;

        public HomeController(ITMDBService tmdbService)
        {
            _tmdbService = tmdbService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetMovieById(string id)
        {
            var data = await _tmdbService.GetMovieById(id);

            return View(data);
        }

    }
}