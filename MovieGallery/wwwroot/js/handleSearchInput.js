$(document).ready(function () {
    $("#searchInput").on("input", function () {
        var searchTerm = $(this).val();

        if (searchTerm.length >= 3) {
            $.ajax({
                type: "GET",
                url: "/Movies/Search",
                data: { title: searchTerm },
                success: function (data) {
                    displaySearchResults(data);
                }
            });
        } else {
            $("#searchResultsContainer").empty();
        }
    });

    function displaySearchResults(results) {
        var container = $("#searchResultsContainer");
        container.empty();

        if (results && results.length > 0) {
            var ul = $("<ul>").addClass("list-group");
            results.forEach(function (movie) {
                var li = $("<li>").addClass("list-group-item");
                var link = $("<a>").attr("href", "/Movies/Details/" + movie.movieID).text(movie.title);

                li.append(link);
                ul.append(li);
            });

            container.append(ul);
        }
    }
});
