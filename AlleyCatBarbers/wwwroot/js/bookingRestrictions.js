//document.addEventListener("DOMContentLoaded", function () {
//    var today = new Date().Date;
//    var minDate = today.toISOString(); 

//    // Make max date 30 days from today
//    var maxDateObj = new Date(today);
//    maxDateObj.setDate(today.getDate() + 30);
//    var maxDate = maxDateObj.toISOString()

//    var bookingDateInput = document.getElementById("bookingDate");
//    bookingDateInput.setAttribute("min", minDate);
//    bookingDateInput.setAttribute("max", maxDate);
//});


// Restrict date picker to today and the next 30 days
document.addEventListener("DOMContentLoaded", function () {
    var today = new Date();
    var minDate = today.toISOString().split('T')[0]; // Format date as yyyy-MM-dd
    var maxDateObj = new Date(today);
    maxDateObj.setDate(today.getDate() + 30);
    var maxDate = maxDateObj.toISOString().split('T')[0]; // Format date as yyyy-MM-dd

    var datePicker = document.getElementById("bookingDate");
    datePicker.attr('min', minDate);
    datePicker.attr('max', maxDate);
});