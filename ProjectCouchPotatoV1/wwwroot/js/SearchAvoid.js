const app = Vue.createApp({
    data() {
        return {
            avoid: {
                MovieId: "@Model.MovieId",
                Title: "@Model.Title",
                Overview: "@Model.Overview",
                poster_path: "@Model.poster_path",
            },
            showSuccess: false,

        };
    },
    methods: {
        submitAvoid() {
            axios.post('/movie/avoid', this.avoid)
                .then(response => {
                    this.avoid = response.data;
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