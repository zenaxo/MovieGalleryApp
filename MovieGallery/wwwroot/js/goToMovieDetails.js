document.addEventListener("DOMContentLoaded", function () {
    var movieImages = document.querySelectorAll(".movie-image");

    movieImages.forEach(function (image) {
        image.addEventListener("click", function () {
            var movieId = image.getAttribute("data-movie-id");
            window.location.href = "/Movies/Details/" + movieId;
        });
    });
});