 const app = Vue.createApp({
        data() {
            return {
                watchlist: {
                    MovieId: "@Model.MovieId",
                    Title: "@Model.Title",
                    Overview: "@Model.Overview",
                    poster_path: "@Model.poster_path",
                },
                showSuccess: false,

            };
        },
        methods: {
            submitWatchlist() {
                axios.post('/movie/watchlist', this.watchlist)
                    .then(response => {
                        this.watchlist = response.data;
                        this.submitSuccess = true;
                        console.log('Success:', response.data);
                    })
                    .catch(error => {
                        console.error('Error:', error);
                    });
            },
        }
    });

    app.mount('#app');