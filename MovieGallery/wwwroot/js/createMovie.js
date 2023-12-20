class Producer {
    constructor(FirstName, LastName) {
        this.FirstName = FirstName;
        this.LastName = LastName;
    }
}

class Actor {
    constructor(FirstName, LastName) {
        this.FirstName = FirstName;
        this.LastName = LastName;
    }
}

const errorList = document.querySelector('#validationMessages');

const titleInput = document.querySelector('#titleInput');
const genreInput = document.querySelector('#genreInput');
const releaseDateInput = document.querySelector('#releaseDateInput');
const imageFileInput = document.querySelector('#imageFileInput');
const backgroundFileInput = document.querySelector('#backgroundFileInput');
const movieDescriptionInput = document.querySelector('#movieDescriptionInput');
const numRatingsInput = document.querySelector('#numRatingsInput');
const ratingValueInput = document.querySelector('#ratingValueInput');

const pFirstNameInput = document.querySelector('#pFirstName');
const pLastNameInput = document.querySelector('#pLastName');
const producersContainer = document.querySelector('.producers-container');
const pValidationText = document.querySelector("#pValidationText");
pValidationText.textContent = '';
let Producers = [];


const aFirstNameInput = document.querySelector('#aFirstName');
const aLastNameInput = document.querySelector('#aLastName');
const actorsContainer = document.querySelector('.actors-container');
const aValidationText = document.querySelector("#aValidationText");
aValidationText.textContent = '';
let Actors = [];


const inputFields = document.querySelectorAll('.form-control');

const requiredInputFields = document.querySelectorAll('.required');

inputFields.forEach(inputField => {
    inputField.addEventListener("focus", () => clearValidation());
});

pFirstNameInput.addEventListener("focus", () => pValidationText.textContent = '');
pLastNameInput.addEventListener("focus", () => pValidationText.textContent = '');
aFirstNameInput.addEventListener("focus", () => aValidationText.textContent = '');
aLastNameInput.addEventListener("focus", () => aValidationText.textContent = '');

// Producer segment
function addProducer() {

    if (pFirstNameInput.value === '' || pLastNameInput.value === '') {
        pValidationText.textContent = 'Please enter First name and Last name';
        return;
    }
    const fistNameFirst = pFirstNameInput.value.charAt(0).toUpperCase();
    const lastNameFirst = pLastNameInput.value.charAt(0).toUpperCase();

    var FirstName = fistNameFirst + pFirstNameInput.value.slice(1);
    var LastName = lastNameFirst + pLastNameInput.value.slice(1);

    if (Producers.some(p => p.FirstName === FirstName && p.LastName === LastName)) {
        pValidationText.textContent = 'Producers need to be unique';
        return;
    }
    var producer = new Producer(FirstName, LastName);
    Producers.push(producer);

    pFirstNameInput.value = '';
    pLastNameInput.value = '';

    renderProducers();

}

function deleteProducer(index) {
    Producers.splice(index, 1);
    renderProducers();
}

function renderProducers() {
    pValidationText.textContent = '';
    producersContainer.innerHTML = '';

    Producers.forEach((producer, index) => {
        var btn = createProducerDeleteBtn(producer);
        producersContainer.appendChild(btn);
        btn.addEventListener("click", () => deleteProducer(index));
    });
}

function createProducerDeleteBtn(Producer) {
    const btn = document.createElement("button");
    btn.className = 'delete-button btn btn-danger';
    btn.textContent = `${Producer.FirstName} ${Producer.LastName}`;
    btn.innerHTML += `<i class="fa-solid fa-xmark" style="color: #ffffff;"></i>`;

    return btn;
}

document.querySelector("#addProducerBtn").addEventListener("click", addProducer);

// Actor segment

function addActor() {

    if (aFirstNameInput.value === '' || aLastNameInput.value === '') {
        aValidationText.textContent = 'Please enter First name and Last name';
        return;
    }
    const fistNameFirst = aFirstNameInput.value.charAt(0).toUpperCase();
    const lastNameFirst = aLastNameInput.value.charAt(0).toUpperCase();

    var FirstName = fistNameFirst + aFirstNameInput.value.slice(1);
    var LastName = lastNameFirst + aLastNameInput.value.slice(1);

    if (Actors.some(a => a.FirstName === FirstName && a.LastName === LastName)) {
        aValidationText.textContent = 'Actor names need to be unique'
        return;
    }
    var actor = new Actor(FirstName, LastName);
    Actors.push(actor);

    aFirstNameInput.value = '';
    aLastNameInput.value = '';

    renderActors();

}

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
function deleteActor(index) {
    Actors.splice(index, 1);
    renderActors();
}

function renderActors() {
    aValidationText.textContent = '';
    actorsContainer.innerHTML = '';

    Actors.forEach((actor, index) => {
        var btn = createActorDeleteBtn(actor);
        actorsContainer.appendChild(btn);
        btn.addEventListener("click", () => deleteActor(index));
    });
}

function createActorDeleteBtn(Actor) {
    const btn = document.createElement("button");
    btn.className = 'delete-button btn btn-danger';
    btn.textContent = `${Actor.FirstName} ${Actor.LastName}`;

    const deleteIcon = document.createElement('i');
    deleteIcon.classList = 'fa-solid fa-xmark';
    deleteIcon.style = 'color: #ffffff';


    btn.appendChild(deleteIcon);

    return btn;
}

document.querySelector("#addActorBtn").addEventListener("click", addActor);

document.querySelector("#postForm").addEventListener("submit", function (event) {
    event.preventDefault();
    setLoading(true);

    clearValidation();

    let errors = 0;
    let firstErrorInput;

    requiredInputFields.forEach(inputField => {
        if (inputField.value === '') {
            if (errors === 0) {
                firstErrorInput = inputField.parentNode;
            }
            const errormsg = document.createElement('p');
            const inputName = inputField.parentNode.firstElementChild.textContent.toLowerCase();
            errormsg.className = 'text text-danger validation';
            errormsg.textContent = `The ${inputName} field is required`;
            inputField.parentNode.insertBefore(errormsg, inputField.nextSibling);
            errors++;
        }
    });

    if (errors > 0) {
        firstErrorInput.scrollIntoView({ behavior: "smooth", block: "start" });
        setLoading(false);
        return;
    }

    submitForm();
});

async function submitForm() {
    const formData = new FormData();
    formData.append('Title', titleInput.value);
    formData.append('Genre', genreInput.value);
    formData.append('ReleaseDate', releaseDateInput.value);
    formData.append('ImageFile', imageFileInput.files[0]);
    formData.append('BackgroundFile', backgroundFileInput.files[0]);
    formData.append('MovieDescription', movieDescriptionInput.value);
    formData.append('NumRatings', numRatingsInput.value);
    formData.append('RatingValue', ratingValueInput.value);

    Producers.forEach((producer, index) => {
        formData.append(`Producers[${index}].FirstName`, producer.FirstName);
        formData.append(`Producers[${index}].LastName`, producer.LastName);
    });

    Actors.forEach((actor, index) => {
        formData.append(`Actors[${index}].FirstName`, actor.FirstName);
        formData.append(`Actors[${index}].LastName`, actor.LastName);
    });

    try {
        const response = await fetch('/Movies/Create', {
            method: 'POST',
            body: formData,
        });

        if (response.ok) {

            window.location.href = '/Movies';
            return;

        } else if (response.status === 400) {

            const errors = await response.json();
            handleValidationErrors(errors);

        } else {
            throw new Error('Failed to create movie');
        }
    } catch (error) {
        console.error(error);
    }
    setLoading(false);
}

function handleValidationErrors(errors) {
    console.error('Validation errors: ', errors);

    errorList.innerHTML = '';

    for (const field in errors) {
        if (errors.hasOwnProperty(field)) {
            errors[field].forEach(error => {
                const listItem = document.createElement('li');
                listItem.textContent = `${field}: ${error}`;
                errorList.appendChild(listItem);
            });
        }
    }
}

function clearValidation() {
    errorList.innerHTML = "";
    inputFields.forEach(inputField => {
        const nextSibling = inputField.nextElementSibling;
        if (nextSibling && nextSibling.classList.contains('validation')) {
            nextSibling.remove();
        }
    });
}

