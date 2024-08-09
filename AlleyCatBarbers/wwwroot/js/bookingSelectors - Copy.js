
$(document).ready(function () {
    var serviceIdDropdown = $('#serviceId');
    var datePicker = $('#bookingDate');
    var timeSlotDropdown = $('#timeSlot');

    // Initialize dropdowns
    datePicker.prop('disabled', true);
    timeSlotDropdown.prop('disabled', true);


    // Restrict date picker to today and the next 30 days
    var today = new Date();
    var minDate = today.toISOString().split('T')[0]; // Format date as yyyy-MM-dd
    var maxDateObj = new Date(today);
    maxDateObj.setDate(today.getDate() + 30);
    var maxDate = maxDateObj.toISOString().split('T')[0]; // Format date as yyyy-MM-dd
    datePicker.attr('min', minDate);
    datePicker.attr('max', maxDate);



    serviceIdDropdown.change(function () {
        var selectedService = $(this).val();
        if (selectedService) {
            datePicker.prop('disabled', false);
        } 
    });

    datePicker.change(function () {
        var selectedDate = $(this).val();
        if (selectedDate) {
            $.ajax({
                url: '/Bookings/GetAvailableSlots',
                type: 'GET',
                data: { date: selectedDate },
                success: function (data) {
                    timeSlotDropdown.empty();
                    timeSlotDropdown.append('<option value="">Select a time</option>');
                    $.each(data, function (index, value) {
                        timeSlotDropdown.append('<option value="' + value + '">' + value + '</option>');
                    });
                    timeSlotDropdown.prop('disabled', false); // Enable the dropdown after populating
                },
                error: function (xhr, status, error) {
                    console.error("Error fetching time slots: ", error);
                    timeSlotDropdown.prop('disabled', true); // Keep it disabled if there's an error
                }
            });
        } else {
            timeSlotDropdown.empty();
            timeSlotDropdown.append('<option value="">Select a time</option>');
            timeSlotDropdown.prop('disabled', true); // Disable the dropdown if no date is selected
        }
    });
});
