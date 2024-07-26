
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