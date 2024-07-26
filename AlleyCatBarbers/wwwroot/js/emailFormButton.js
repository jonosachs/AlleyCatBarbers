document.addEventListener('DOMContentLoaded', function () {
    var form = document.querySelector('form[data-email-sent]');
    var emailSent = form.getAttribute('data-email-sent');

    if (emailSent) {
        markButtonAsSent();
    }
});

function disableSubmitButton() {
    var button = document.getElementById("submitButton");
    button.disabled = true;
    button.value = "Sending...";
}

function enableSubmitButton() {
    var button = document.getElementById("submitButton");
    button.disabled = false;
    button.value = "Send";
}

function markButtonAsSent() {
    var button = document.getElementById("submitButton");
    button.disabled = true;
    button.value = "Send";
    button.classList.add('disabled');
}
