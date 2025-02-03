/*$(document).ready(function () {*/
// Load Add/Edit Form in Modal
function loadAddForm() {
    $.get("/City/Add/", function (data) {
        $("#modalContent").html(data);
        $("#cityModal").modal("show");
    });
}

function loadEditForm(cityId) {
    $.get("/City/Edit/" + cityId, function (data) {
        $("#modalContent").html(data);
        $("#cityModal").modal("show");
    });
}


// Submit Add/Edit Form
function saveCity() {
    var formData = $("#cityForm").serialize();
    $.post("/City/Save", formData, function (response) {
        if (response.success) {
            location.reload();  // Refresh list after saving
        } else {
            alert("Failed to save city.");
        }
    });
}

// Delete City
function loadDeleteCityConfirm(cityId) {

    $.get("/City/DeleteConfirm/" + cityId, function (data) {
        $("#modalContent").html(data);
        $("#cityModal").modal("show");
    });

}


function deleteCity(cityId) {
    //if (!confirm("Are you sure you want to delete this city?")) return;
    $.post("/City/Delete/" + cityId, function (response) {
        if (response.success) {
            $("#row-" + cityId).remove(); // Remove deleted city row
            location.reload();  // Refresh list after delete
        } else {
            alert("Failed to delete city.");
        }
    });
}
/*});*/



