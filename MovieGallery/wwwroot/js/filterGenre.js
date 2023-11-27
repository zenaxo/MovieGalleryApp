function filterGenre(movieGenre) {
    var selectedGenre = movieGenre;  // Use the movie's genre instead of movie ID
    var isSortedByAverageRating = $("#isSortedByAverageRating").is(":checked");

    // Use AJAX or redirect to update the URL with both filter and sorting options
    window.location.href = "?filterOption=" + selectedGenre + "&isSortedByAverageRating=" + isSortedByAverageRating;
}