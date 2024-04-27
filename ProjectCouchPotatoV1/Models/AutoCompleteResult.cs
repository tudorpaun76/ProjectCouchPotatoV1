namespace ProjectCouchPotatoV1.Models
{
    public class AutoCompleteResult
    {
        public int Id { get; set; }
        public string MovieId { get; set; }
        public string Title { get; set; }
        public string Overview { get; set; }
        public string poster_path { get; set; }

        public string vote_average { get; set; }

        public string release_date { get; set; }

    }
}
