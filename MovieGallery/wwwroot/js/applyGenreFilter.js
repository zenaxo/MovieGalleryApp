function applyGenreFilter() {
    var selectedGenre = $("#genreFilter").val();
    var isSortedByAverageRating = $("#isSortedByAverageRating").is(":checked");

    // Use AJAX or redirect to update the URL with both filter and sorting options
    window.location.href = "?filterOption=" + selectedGenre + "&isSortedByAverageRating=" + isSortedByAverageRating;
}