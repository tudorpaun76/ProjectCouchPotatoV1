using Newtonsoft.Json;

namespace ProjectCouchPotatoV1.Models
{
    public class MovieSearch
    {
        public List<Review> Results { get; set; } = new List<Review>();

        public List<Watchlist> Watchlists { get; set; } = new List<Watchlist>();

        public List<AutoCompleteResult> autoCompleteResults { get; set; } = new List<AutoCompleteResult>();
    }

}
