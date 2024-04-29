//const app = Vue.createApp({
//    data() {
//        return {
//            movie: {
//                MovieId: "@Model.MovieId",
//                Title: "@Model.Title",
//                Overview: "@Model.Overview",
//                poster_path: "@Model.poster_path",
//                ReviewText: "",
//                vote_average: "@Model.vote_average",
//                vote_count: "@Model.vote_count",
//                release_date: "@Model.release_date",
//                original_language: "@Model.original_language",
//            },
//            avoid: {
//                MovieId: "@Model.MovieId",
//                Title: "@Model.Title",
//                Overview: "@Model.Overview",
//                poster_path: "@Model.poster_path",
//            },
//            watchlist: {
//                MovieId: "@Model.MovieId",
//                Title: "@Model.Title",
//                Overview: "@Model.Overview",
//                poster_path: "@Model.poster_path",
//            },
//            showSuccess: false,
//            showModalSuccess: false,
//            dupeMessage: false
//        };
//    },
//    methods: {
//        submitForm() {
//            axios.post('/movie/review', this.movie)
//                .then(response => {
//                    this.movie = response.data;
//                    this.showModalSuccess = true;
//                    this.hideModalSuccessMessage();
//                    console.log('Success:', response.data);
//                })
//                .catch(error => {
//                    if (error.response.status === 409) {
//                        this.dupeMessage = true;
//                        this.hideDupeMessage();
//                    } else {
//                        console.error('Error:', error);
//                        this.errorMessage = "An error occurred while submitting the review.";
//                    }
//                });
//        },
//        submitAvoid() {
//            axios.post('/movie/avoid', this.avoid)
//                .then(response => {
//                    this.avoid = response.data;
//                    this.showSuccess = true;
//                    this.hideSuccessMessage();
//                    console.log('Success:', response.data);
//                })
//                .catch(error => {
//                    if (error.response.status === 409) {
//                        this.dupeMessage = true;
//                        this.hideDupeMessage();
//                    } else {
//                        console.error('Error:', error);
//                        this.errorMessage = "An error occurred while submitting the review.";
//                    }
//                });
//        },
//        submitWatchlist() {
//            axios.post('/movie/watchlist', this.watchlist)
//                .then(response => {
//                    this.watchlist = response.data;
//                    this.showSuccess = true;
//                    this.hideSuccessMessage();
//                    console.log('Success:', response.data);
//                })
//                .catch(error => {
//                    if (error.response.status === 409) {
//                        this.dupeMessage = true;
//                        this.hideDupeMessage();
//                    } else {
//                        console.error('Error:', error);
//                        this.errorMessage = "An error occurred while submitting the review.";
//                    }
//                });
//        },
//        hideSuccessMessage() {
//            setTimeout(() => {
//                this.showSuccess = false;
//            }, 3000);
//        },
//        hideDupeMessage() {
//            setTimeout(() => {
//                this.dupeMessage = false;
//            }, 3000);
//        },
//        hideSuccessMessage() {
//            setTimeout(() => {
//                this.showSuccess = false;
//            }, 3000);
//        },
//        hideModalSuccessMessage() {
//            setTimeout(() => {
//                this.showModalSuccess = false;
//            }, 3000);
//        },
//    },
//});

//app.mount('#app');