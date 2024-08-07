document.addEventListener("DOMContentLoaded", function () {

    var userTable = document.getElementById('userTable');
    var reviewTable = document.getElementById('reviewTable');
  

    if (userTable) {
        $('#userTable').DataTable({
            responsive: true,
            paging: true,
            searching: true,
            info: true,
        });
    }

    if (reviewTable) {
        $('#reviewTable').DataTable({
            responsive: true,
            paging: true,
            searching: true,
            info: true,
        });
    }



});
