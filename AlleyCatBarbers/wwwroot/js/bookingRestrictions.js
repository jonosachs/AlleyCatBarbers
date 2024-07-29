document.addEventListener("DOMContentLoaded", function () {
    var today = new Date();
    var minDate = today.toISOString().substring(0, 16); // "YYYY-MM-DDTHH:MM"

    // Make max date 30 days from today
    var maxDateObj = new Date(today);
    maxDateObj.setDate(today.getDate() + 30);
    var maxDate = maxDateObj.toISOString().substring(0, 16); // "YYYY-MM-DDTHH:MM"

    var bookingDateInput = document.getElementById("bookingDate");
    bookingDateInput.setAttribute("min", minDate);
    bookingDateInput.setAttribute("max", maxDate);
    bookingDateInput.setAttribute("step", "900"); // 15 mins
});
