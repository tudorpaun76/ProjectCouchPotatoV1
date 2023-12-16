using Microsoft.AspNetCore.Mvc;
using RestSharp;
using System.Net.Http.Headers;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using ProjectCouchPotatoV1.Models;

namespace ProjectCouchPotatoV1.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class HomeController : Controller
    {
        public static async Task<Movie> Index()
        {
            Movie movie = new Movie();

            using (var client = new HttpClient())
            {
                {
                    string apiKey = "39f647a07cb46f9fade1f0097a756aed";

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

                    // Replace "movie_id" with the actual ID of the movie you want to retrieve
                    string movieId = "725201";

                    var requestUri = $"https://api.themoviedb.org/3/movie/{movieId}?language=en-US";

                    var response = await client.GetAsync(requestUri);
                    response.EnsureSuccessStatusCode();

                    var body = await response.Content.ReadAsStringAsync();

                    movie = Newtonsoft.Json.JsonConvert.DeserializeObject<Movie>(body);

                    movie.Id = movieId;
                    movie.Title = movie.Title;  // Assuming Name corresponds to Title in your JSON response
                    movie.Overview = movie.Overview;

                    Console.WriteLine($"Id: {movie.Id}, Title: {movie.Title}, Overview: {movie.Overview}");


                    return movie; 
                }
            }
        }

        static async Task Main()
        {
            Movie movie = await Index();
            Console.WriteLine($"Id: {movie.Id}, Title: {movie.Title}, Overview: {movie.Overview}");
            Console.WriteLine("Testing 123");
        }


    }
}


