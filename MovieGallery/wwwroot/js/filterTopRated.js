function filterTopRated() {
    $.ajax({
        url: '/Movies',
        type: 'GET',
        data: { isSortedByAverageRating: true },
        success: function (result) {
            $('#moviesContainer').html(result);
            console.log(result);
            console.log('Update successful');
        },
        error: function (xhr, status, error) {
            console.error('Error:', error);
            console.log('XHR:', xhr);
            console.log('Status:', status);
        }
    });
}