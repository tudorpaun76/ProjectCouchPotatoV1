using ProjectCouchPotatoV1.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectCouchPotatoV1.Models
{
    public class Review
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string MovieId { get; set; }
        public string Title { get; set; }
        public string Overview { get; set; }
        public string poster_path { get; set; }
        public string ReviewText { get; set; }
        public string vote_average { get; set; }
        public string vote_count{ get; set; }
        public string release_date { get; set; }
        public string original_language { get; set; }

        [NotMapped]
        public List<Trailer> Trailers { get; set; }

        [NotMapped]
        public List<Cast> Cast { get; set; }
    }

    public class Trailer
    {
        public string key { get; set; }

    }

    public class Cast 
    {
        public string name { get; set; }
        public string character { get; set; }
        public string profile_path { get; set; }
    }
}
