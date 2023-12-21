using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectCouchPotatoV1.Models
{
    public class Movie
    {
        [Key]
        public int Identifier { get; set; }
        public string MovieId { get; set; }
        public string Title { get; set; }
        public string Overview { get; set; }
        public string poster_path { get; set; }
        public string review { get; set; }
    }
}
