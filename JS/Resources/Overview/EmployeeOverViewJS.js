$(document).ready(function () {
    employeeResourceInfo();
    $('#DocumentModalTable_paginate').show();
    $("#DocumentModalTable_next").enabled = true;
    $("#DocumentModalTable_previous").enabled = true;  
});
function DataTableDesign() {
    var table = $('#DocumentModalTable').DataTable({
        "sDom": '<"top"i>rt<"bottom"flp><"clear">'
    });
    $('#tableDiv').find('.dataTables_filter').hide();
    $('#tableDiv').find('.dataTables_info').hide();
    $('#serdata').find('#tableDiv').find('#DocumentModalTable thead tr').appendTo('#DocumentModalTable thead');
    $('#serdata').find('#tableDiv').find('#DocumentModalTable tfoot tr').find('.SearchName').keyup(function () {
        table.column(0).search(this.value).draw();
    });
    $("#DocumentModalTable tfoot .SearchLocation").keyup(function () {
        table.column(1).search(this.value).draw();
    });
    $("#DocumentModalTable tfoot .SearchBusiness").keyup(function () {
        table.column(2).search(this.value).draw();
    });
    $("#DocumentModalTable tfoot .SearchDivision").keyup(function () {
        table.column(3).search(this.value).draw();
    });
    $("#DocumentModalTable tfoot .SearchPool").keyup(function () {
        table.column(4).search(this.value).draw();
    });
    $("#DocumentModalTable tfoot .SearchFunction").keyup(function () {
        table.column(5).search(this.value).draw();
    });
}


function employeeResourceInfo() {
    var Employeeid = $('#employeeId').val();
    $.ajax({
        type: "POST",
        data: { EmployeeId: Employeeid },
        url: constantMeResources.getResourceEmployeeInfo,
        success: function (result) {
            $('#serdata').html(result);
            $('#serdata').show();
            DataTableDesign();
        }
    });
}