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

    const currentData = document.getElementById('moviesContainer');
    currentData.innerHTML = '';

    for (let i = 0; i < 9; i++) {
        currentData.appendChild(createSkeleton());
    }

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
            container.innerHTML = currentData;
            console.error('Failed to update movie list:', response.statusText);
        }
    } catch (error) {
        container.innerHTML = currentData;
        console.error('An error occurred while updating movie list:', error);
    }
}

function createSkeleton() {

    var cardElement = document.createElement('div');
    cardElement.className = 'card loading';

    // Create the image element
    var imageElement = document.createElement('div');
    imageElement.className = 'image';

    // Create the content element
    var contentElement = document.createElement('div');
    contentElement.className = 'content';

    // Create the stripe elements
    var stripeElement1 = document.createElement('div');
    stripeElement1.className = 'stripe';

    var stripeElement2 = document.createElement('div');
    stripeElement2.className = 'stripe small';

    // Append elements to their respective parent elements
    contentElement.appendChild(stripeElement1);
    contentElement.appendChild(stripeElement2);

    cardElement.appendChild(imageElement);
    cardElement.appendChild(contentElement);

    return cardElement;
}
