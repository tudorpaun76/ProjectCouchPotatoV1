const app = Vue.createApp({
    data() {
        return {
            movies: [],
            watchlists: [],
            avoids: [],
            randomMovie: [],
            selectedMovie: null,
            isEditing: false,
            searchQuery: '',
            searchResults: [],
            loaded: true,
            updatedReview: ''
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
        fetchSearchResults() {
            axios.get(`/movie/searchresult?name=${this.searchQuery}`)
                .then(response => {
                    this.searchResults = response.data;
                })
                .catch(error => {
                    console.error('Error fetching search results:', error);
                });
        },
        populateSearchField(movieTitle, context) {
            this.searchQuery = movieTitle;
            if (context === 'movie') {
                document.getElementById('movieSearchForm').submit();
            } else if (context === 'searchreview') {
                document.getElementById('reviewSearchForm').submit();
            } else if (context === 'searchwatchlist') {
                document.getElementById('watchlistSearchForm').submit();
            } else if (context === 'searchavoid') {
                document.getElementById('avoidSearchForm').submit();
            } else {
                console.error('Invalid context provided.');
            }
        },
        fetchMovies() {
            axios.get('/movie/getmovies')
                .then(response => {
                    this.movies = response.data;
                    this.loaded = false;
                })
                .catch(error => {
                    console.error('Error getting movies:', error);
                })
                .finally(() => {
                    this.loaded = false;
                });
        },
        fetchWatchlists() {
            axios.get('/movie/getwatchlist')
                .then(response => {
                    this.watchlists = response.data;
                    this.loaded = false;
                })
                .catch(error => {
                    console.error('Error getting movies:', error);
                })
                .finally(() => {
                    this.loaded = false;
                });
        },
        fetchAvoid() {
            axios.get('/movie/getavoidmovies')
                .then(response => {
                    this.avoids = response.data;
                    this.loaded = false;
                })
                .catch(error => {
                    console.error('Error getting movies:', error);
                })
                .finally(() => {
                    this.loaded = false;
                });
        },
        fetchRandomMovie() {
            axios.get('/movie/movierandom')
                .then(response => {
                    console.log('Random Movie data:', response.data);
                    this.randomMovie = response.data;
                })
                .catch(error => {
                    console.error('Error getting movies:', error);
                })
        },
        openReview(movie) {
            this.selectedMovie = movie;
            this.$nextTick(() => {
                $('#reviewModal').modal('show');
            });
        },
        openWatchlist(watchlist) {
            this.selectedMovie = watchlist;
            this.$nextTick(() => {
                $('#watchlistModal').modal('show');
            });
        },
        openAvoid(avoid) {
            this.selectedMovie = avoid;
            this.$nextTick(() => {
                $('#avoidModal').modal('show');
            });
        },
        closeModal() {
            this.selectedMovie = null;
        },
        deleteReview(id) {
            axios.delete(`/movie/deletereview?movieid=${id}`)
                .then(response => {
                    console.log('Deleted movie:', response.data);
                    this.movies = this.movies.filter(movie => movie.id !== id);
                    this.closeModal();
                    $('#reviewModal').modal('hide');
                })
                .catch(error => {
                    console.error('Error deleting movie:', error);
                });
        },
        deleteWatchlist(id) {
            axios.delete(`/movie/deletewatchlist?movieid=${id}`)
                .then(response => {
                    console.log('Deleted movie:', response.data);
                    this.watchlists = this.watchlists.filter(watchlist => watchlist.id !== id);
                    this.closeModal();
                    $('#watchlistModal').modal('hide');
                })
                .catch(error => {
                    console.error('Error deleting movie:', error);
                });
        },
        deleteAvoid(id) {
            axios.delete(`/movie/deleteavoid?movieid=${id}`)
                .then(response => {
                    console.log('Deleted movie:', response.data);
                    this.avoids = this.avoids.filter(avoid => avoid.id !== id);
                    this.closeModal();
                    $('#avoidModal').modal('hide');
                })
                .catch(error => {
                    console.error('Error deleting movie:', error);
                });
        },
        editReview(id) {
            axios.put(`/movie/editreview?movieId=${id}`, { reviewText: this.selectedMovie.reviewText })
                .then(response => {
                    console.log('Updated review:', response.data);
                    this.selectedMovie.reviewText = response.data.reviewText;
                    this.closeModal();
                    $('#reviewModal').modal('hide');
                })
                .catch(error => {
                    console.error('Error updating review:', error);
                });
        },
    },
    mounted() {
        this.fetchMovies();
        this.fetchWatchlists();
        this.fetchAvoid();
        jQuery(document).ready(function ($) {
            $('#reviewModal').modal({ show: true });
            $('#watchlistModal').modal({ show: true });
            $('#avoidModal').modal({ show: true });
        });

    }
});

app.mount('#app');