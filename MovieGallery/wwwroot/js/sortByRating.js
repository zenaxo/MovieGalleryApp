function sortByRating() {
    // Add logic to handle sorting by rating
    var isSortedByRating = true; // Set a flag indicating sorting by rating

    // You may use AJAX to make a request to your controller action with the sorting parameter
    // Example using jQuery AJAX:
    $.ajax({
        url: "/YourController/Index", // Update with the correct controller and action
        type: "GET",
        data: {
            filterOption: $("#genreFilter").val(),
            isSortedByAverageRating: isSortedByRating
        },
        success: function (data) {
            // Update the movie list on the page with the sorted data
            // This may involve replacing the current movie list with the new data
            $(".movies-disp").html(data);
        },
        error: function (error) {
            console.error("Error sorting by rating:", error);
            // Handle errors if necessary
        }
    });
}