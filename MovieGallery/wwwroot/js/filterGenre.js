//function filterGenre(genre) {
//    $.ajax({
//        url: '/Movies',
//        type: 'GET',
//        data: { filterOption: genre },
//        success: function (result) {
//            $('#moviesContainer').html(result);
//            updateGenreTrigger(genre);
//            removeGenreFromList(genre);
//        },
//        error: function (xhr, status, error) {
//            console.error('Error:', error);
//            console.log('XHR:', xhr);
//            console.log('Status:', status);
//        }
//    });
//}

//function updateGenreTrigger(genre) {
//    $('#genreTrigger').text(genre);
//}

//function removeGenreFromList(genre) {
//    // Add the selected genre to the list of selected genres
//    if (!ViewBag.SelectedGenres) {
//        ViewBag.SelectedGenres = [];
//    }
//    ViewBag.SelectedGenres.push(genre);

//    // Remove the selected genre button from the genre list
//    $('.genre-list button:contains("' + genre + '")').remove();
//}