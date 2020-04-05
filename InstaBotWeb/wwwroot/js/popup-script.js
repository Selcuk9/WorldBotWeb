let more = document.getElementById('more-info'),
    modal = document.querySelector('.fixed-overlay'),
    btnClose = document.querySelector('.popup-close');


more.addEventListener('click', function () {
    showModal();
});
btnClose.addEventListener('click', function () {
    closeModal();
});
modal.addEventListener('click', function (event) {
    closeModal(event);
});

function showModal() {
    modal.style.display = "block";
}

function closeModal(event) {
    modal.style.display = "none";
}