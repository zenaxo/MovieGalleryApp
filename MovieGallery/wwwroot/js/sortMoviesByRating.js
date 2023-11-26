function sortMovies() {
    isAsc = !isAsc;
    let sortString = isAsc ? 'ASC' : 'DESC';
    window.location.href = '@Url.Action("SortByRating", "Movies")' + '?order=' + sortString;
}