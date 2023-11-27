function toggleDeleteWindow(movieId) {
    $.ajax({
        type: 'POST',
        url: '@Url.Action("ToggleDeleteWindow", "Movies")',
        data: { movieId: movieId },
        success: function () {
            // Refresh or update the UI as needed
            location.reload(); // Example: Reload the page
        },
        error: function (error) {
            console.log(error);
        }
    });
}