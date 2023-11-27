$(document).ready(function () {
    $("#searchInput").on("input", function () {
        var searchTerm = $(this).val();

        if (searchTerm.length >= 3) {
            // Make an AJAX request to get search results
            $.ajax({
                type: "GET",
                url: "/Movies/Search",
                data: { title: searchTerm },
                success: function (data) {
                    displaySearchResults(data);
                }
            });
        } else {
            // Clear search results if the search term is less than 3 characters
            $("#searchResultsContainer").empty();
        }
    });

    // Function to display search results
    function displaySearchResults(results) {
        var container = $("#searchResultsContainer");
        container.empty();

        if (results && results.length > 0) {
            var ul = $("<ul>").addClass("list-group");

            results.forEach(function (movie) {
                console.log(movie);
                console.log("Movie Title:", movie.Title);
                var li = $("<li>").addClass("list-group-item");
                var link = $("<a>").attr("href", "/Movies/Details/" + movie.movieID).text(movie.title);

                li.append(link);
                ul.append(li);
            });

            container.append(ul);
        }
    }
});