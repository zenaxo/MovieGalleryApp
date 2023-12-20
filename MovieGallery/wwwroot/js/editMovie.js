
const editForm = document.getElementById('editMovieForm');
const movieIdInput = document.getElementById('movieId');


function setLoading(isLoading) {

    const submitBtn = document.querySelector("#submitBtn");
    const loadingIcon = document.createElement('i');
    loadingIcon.classList = 'fa-solid fa-circle-notch fa-spin';

    if (isLoading) {
        submitBtn.insertBefore(loadingIcon, submitBtn.firstChild);
        submitBtn.disabled = true;
    } else {
        submitBtn.removeChild(submitBtn.firstChild);
        isLoading = false;
        submitBtn.disabled = false;
    }
}

editForm.addEventListener('submit', async function(event) {
    event.preventDefault();
    setLoading(true);

    const formData = new FormData(editForm);

    try {
        const response = await fetch('/Movies/Edit', {
            method: 'POST',
            body: formData
        });

        if (response.ok) {
            console.log('Form submitted successfully!');
            window.location.href = `/Movies/Details/${movieIdInput.value}`
        } else {
            console.error('Form submission failed.');
            setLoading(false);
        }
    } catch (error) {
        console.error('An error occured:', error);
        setLoading(false);
    }

})

$(document).ready(function () {
    $(".producers-container").on("click", ".producer-delete-btn", function (event) {
        event.preventDefault();

        var validationContainer = $("#producerValidationContainer");
        validationContainer.empty();

        var producerId = $(this).data("producer-id");
        var movieId = $(this).data("movie-id");

        $.ajax({
            url: '/Movies/DeleteProducer',
            type: 'POST',
            data: { producerId: producerId, movieId: movieId },
            success: function (result) {
                $(".producers-container").html(result);
            },
            error: function (xhr, status, error) {
                console.error('Error:', error);
                console.log('XHR:', xhr);
                console.log('Status:', status);
                alert('An error occurred while deleting the producer.');
            }
        });
    });

    $(".producers-container").on("click", ".add-producer-btn", function (event) {
        event.preventDefault();

        var movieId = $(this).data("movie-id");
        var firstName = $("#FirstName").val();
        var lastName = $("#LastName").val();
        var producerName = {
            FirstName: firstName,
            LastName: lastName
        };
        var validationContainer = $("#producerValidationContainer");
        validationContainer.empty();

        if (firstName === '' || lastName === '') {
            validationContainer.text('Please enter both first name and last name')
            return;
        }

        $.ajax({
            url: '/Movies/AddProducer',
            type: 'POST',
            data: { movieId: movieId, producerName: producerName },
            success: function (result) {
                $(".producers-container").html(result);
            },
            error: function (xhr, status, error) {
                console.error('Error:', error);
                console.log('XHR:', xhr);
                console.log('Status:', status);
                alert('An error occurred while adding the producer.');
            }
        });
    });
});

$(document).ready(function () {
    $(".actors-container").on("click", ".actor-delete-btn", function (event) {
        event.preventDefault();

        var actorValidationContainer = $("#actorValidationContainer");
        actorValidationContainer.empty();

        var actorId = $(this).data("actor-id");
        var movieId = $(this).data("movie-id");

        $.ajax({
            url: '/Movies/DeleteActor',
            type: 'POST',
            data: { actorId: actorId, movieId: movieId },
            success: function (result) {
                $(".actors-container").html(result);
            },
            error: function (xhr, status, error) {
                console.error('Error:', error);
                console.log('XHR:', xhr);
                console.log('Status:', status);
                alert('An error occurred while deleting the actor.');
            }
        });
    });

    $(".actors-container").on("click", ".add-actor-btn", function (event) {
        event.preventDefault();

        var movieId = $(this).data("movie-id");
        var firstName = $("#ActorFirstName").val();
        var lastName = $("#ActorLastName").val();
        var actorName = {
            FirstName: firstName,
            LastName: lastName
        };
        var actorValidationContainer = $("actorValidationContainer");
        actorValidationContainer.empty();

        if (firstName === '' || lastName === '') {
            actorValidationContainer.text('Please enter both first name and last name')
            return;
        }

        $.ajax({
            url: '/Movies/AddActor',
            type: 'POST',
            data: { movieId: movieId, actorName: actorName },
            success: function (result) {
                $(".actors-container").html(result);
            },
            error: function (xhr, status, error) {
                console.error('Error:', error);
                console.log('XHR:', xhr);
                console.log('Status:', status);
                alert('An error occurred while adding the actor.');
            }
        });
    });
});