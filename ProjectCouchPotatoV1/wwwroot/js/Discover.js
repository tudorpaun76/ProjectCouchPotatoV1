const app = Vue.createApp({
    data() {
        return {
            searchQuery: '',
            searchResults: [],
            popular: [],
            upcoming: [],
            loaded: true
        };
    },
    watch: {
        searchQuery() {
            if (this.searchQuery.length > 2) {
                this.fetchSearchResults();
            } else {
                this.searchResults = [];
            }
        }
    },
    methods: {
        searchMovie(title) {
            this.populateSearchField(title);
            this.fetchSearchResults();
        },
        populateSearchField(title) {
            this.searchQuery = title;
        },
        fetchSearchResults() {
            axios.get(`/movie/searchresult?name=${this.searchQuery}`)
                .then(response => {
                    this.searchResults = response.data;
                })
                .catch(error => {
                    console.error('Error fetching search results:', error);
                })
                .finally(() => {
                    this.loaded = false;
                });
        },
        fetchPopularMovies() {
            axios.get(`/movie/popularmovies`)
                .then(response => {
                    this.popular = response.data;
                })
                .catch(error => {
                    console.error('Error fetching search results:', error);
                })
                .finally(() => {
                    this.loaded = false;
                });
        },
        fetchUpcomingMovies() {
            axios.get(`/movie/upcomingmovies`)
                .then(response => {
                    this.upcoming = response.data;
                })
                .catch(error => {
                    console.error('Error fetching search results:', error);
                })
                .finally(() => {
                    this.loaded = false;
                });
        }
    },
    mounted() {
        this.fetchPopularMovies();
        this.fetchUpcomingMovies();
    }
});

app.mount('#app');