const container = document.getElementById('dynamicContent');

container.addEventListener('change', function (event) {
    const target = event.target;

    if (target.id === 'genreFilterSelect') {
        sortByGenre();
    }
});
container.addEventListener('click', function (event) {
    const target = event.target;

    if (target.id === 'topRatedFilter') {
        sortByAverageRating();
    } else if (target.id === 'dateFilter') {
        sortByDate();
    }
});
async function sortByDate() {
    const isSortedByDate = document.getElementById('isSortedByDate').value === 'true' ? false : true;
    const isSortedByAverageRating = false;
    const filterOption = document.getElementById('genreFilterSelect').value;

    const options = {
        FilterOption: filterOption.toString(),
        IsSortedByAverageRating: isSortedByAverageRating,
        IsSortedByDate: isSortedByDate,
    };
    await updateMovieList(options);
}
async function sortByAverageRating() {
    const isSortedByAverageRating = document.getElementById('isSortedByAverageRating').value === 'true' ? false : true;
    const isSortedByDate = false;
    const filterOption = document.getElementById('genreFilterSelect').value;

    const options = {
        FilterOption: filterOption.toString(),
        IsSortedByAverageRating: isSortedByAverageRating,
        IsSortedByDate: isSortedByDate,
    };
    await updateMovieList(options);
}
async function sortByGenre() {

    const filterOption = document.getElementById('genreFilterSelect').value;
    const isSortedByAverageRating = document.getElementById('isSortedByAverageRating').value === 'true' ? true : false;
    const isSortedByDate = document.getElementById('isSortedByDate').value === 'true' ? true : false;

    const options = {
        FilterOption: filterOption.toString(),
        IsSortedByAverageRating: isSortedByAverageRating,
        IsSortedByDate: isSortedByDate,
    };
    await updateMovieList(options);
}

async function updateMovieList(options) {
    try {
        const response = await fetch('/Movies/UpdateMovieList', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(options),
        });

        if (response.ok) {
            const data = await response.text();
            container.innerHTML = data;
        } else {
            console.error('Failed to update movie list:', response.statusText);
        }
    } catch (error) {
        console.error('An error occurred while updating movie list:', error);
    }
}
