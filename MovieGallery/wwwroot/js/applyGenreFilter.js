function applyGenreFilter() {
    var selectedGenre = $("#genreFilter").val();
    var isSortedByAverageRating = $("#isSortedByAverageRating").is(":checked");
    window.location.href = "?filterOption=" + selectedGenre + "&isSortedByAverageRating=" + isSortedByAverageRating;
}