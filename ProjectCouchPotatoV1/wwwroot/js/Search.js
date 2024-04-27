const app = Vue.createApp({
    data() {
        return {
            movie: {
                MovieId: "@Model.MovieId",
                Title: "@Model.Title",
                Overview: "@Model.Overview",
                poster_path: "@Model.poster_path",
                ReviewText: ""
            },
            showSuccess: false,

        };
    },
    methods: {
    }
});

app.mount('#app');