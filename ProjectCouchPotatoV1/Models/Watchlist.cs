using ProjectCouchPotatoV1.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectCouchPotatoV1.Models
{
    public class Watchlist
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string MovieId { get; set; }
        public string Title { get; set; }
        public string Overview { get; set; }
        public string poster_path { get; set; }

    }
}
