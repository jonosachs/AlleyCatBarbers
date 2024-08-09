$(document).ready(function () {
    var serviceIdDropdown = $('#serviceId');
    var datePicker = $('#bookingDate');
    var timeSlotDropdown = $('#timeSlot');
    var formMode = $('#editBookingForm').data('mode');

    // Initialize dropdowns
    if (formMode === 'edit') {
        console.log("Edit mode");

        // Set the date and time values from hidden fields
        var hiddenDate = $('#hiddenDate').val();
        var hiddenTimeSlot = $('#hiddenTimeSlot').val();

        if (hiddenDate) {
            datePicker.val(hiddenDate);
            datePicker.datepicker("setDate", hiddenDate);  // Ensure the datepicker is updated
        }

        if (hiddenTimeSlot) {
            timeSlotDropdown.val(hiddenTimeSlot);
            fetchTimeSlots(hiddenDate)
            console.log("Setting time slot to:", hiddenTimeSlot); // Debugging
        }

        datePicker.prop('disabled', false);
        timeSlotDropdown.prop('disabled', false);
    } else {
        datePicker.val('Choose..');
        datePicker.prop('disabled', true);
        timeSlotDropdown.prop('disabled', true);
    }

    // Event handler for service dropdown change
    serviceIdDropdown.change(function () {

        var selectedService = $(this).val();

        if (selectedService) {
            datePicker.prop('disabled', false);
        } else {
            datePicker.prop('disabled', true);
        }

    });

    // Initialize date picker
    datePicker.datepicker({
        dateFormat: 'yy-mm-dd',
        minDate: 0, // Today
        maxDate: '+30D', // 30 days from today
        beforeShowDay: function (date) {
            var today = new Date();
            today.setHours(0, 0, 0, 0);
            var maxDate = new Date();
            maxDate.setDate(today.getDate() + 30);

            if (date < today || date > maxDate) {
                return [false, "ui-state-disabled", "Unavailable"];
            } else {
                return [true, "", ""];
            }
        },
        onSelect: function (dateText) {
            fetchTimeSlots(dateText);
        }
    });

    function fetchTimeSlots(dateText) {

        $.ajax({
            url: '/Bookings/GetAvailableTimeSlots',
            type: 'GET',
            data: { date: dateText, currentBookingTimeSlot: hiddenTimeSlot },
            success: function (data) {
                    
                timeSlotDropdown.empty();
                timeSlotDropdown.append('<option value="">Choose..</option>');
                    
                $.each(data, function (index, value) {
                    timeSlotDropdown.append('<option value="' + value + '">' + value + '</option>');
                });

                if (formMode === 'edit') {
                    timeSlotDropdown.val(hiddenTimeSlot);
                }

                timeSlotDropdown.prop('disabled', false); // Enable the dropdown after populating
            },
            error: function (xhr, status, error) {
                console.error("Error fetching time slots: ", error);
                timeSlotDropdown.prop('disabled', true); // Keep it disabled if there's an error
            }
        });
    }


});
