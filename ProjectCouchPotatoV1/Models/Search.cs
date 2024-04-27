using Newtonsoft.Json;

namespace ProjectCouchPotatoV1.Models
{
    public class MovieSearch
    {
        public List<Review> Results { get; set; } = new List<Review>();

        public List<Watchlist> Watchlists { get; set; } = new List<Watchlist>();

        public List<AutoCompleteResult> autoCompleteResults { get; set; } = new List<AutoCompleteResult>();

        public List<Popular> Popular { get; set; } = new List<Popular>();

        public List<Upcoming> Upcoming { get; set; } = new List<Upcoming>();

        public List<MovieAvoid> MovieToAvoid { get; set; } = new List<MovieAvoid>();
    }

}
