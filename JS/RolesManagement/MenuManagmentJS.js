
$(document).ready(function () {
    table = $('#AddMenuListRecordTable').DataTable({
        bFilter: false,
        bInfo: false,
        dom: 'frtlip'
    });

});

$("#tableDivMenu").on('click', '.btn-add-menu', function () {
    
    $.ajax({
        url: constantMenu.addEdit,
        data: { Id: 0 },
        success: function (data) {
            
            $("#tableDivMenu").find('#addMenuBody').html('');
            $("#tableDivMenu").find('#addMenuBody').html(data);
            $("#tableDivMenu").find("#addMenuBody").find("#btn-submit-AddMenuRecord").html("Add");
            $('[data-toggle="tooltip"]').tooltip();
        }
    });
});

$("#tableDivMenu").on('click', '.btn-edit-menu', function () {
    var id = $("#tableDivMenu").find("#AddMenuListRecordTable tbody").find('.selected').attr("id");
    
    $.ajax({
        url: constantMenu.addEdit,
        data: { Id: id },
        success: function (data) {
            $("#tableDivMenu").find('#addMenuBody').html('');
            $("#tableDivMenu").find('#addMenuBody').html(data);
            $("#tableDivMenu").find("#addMenuBody").find("#btn-submit-AddMenuRecord").html("Add");
        }
    });
});

$("#tableDivMenu").on('click', '#btn-submit-AddMenuRecord', function () {
    
    var iserror = false;
    var model = {
        Id: $("#tableDivMenu").find("#hidden-Id").val(),
        MenuName: $("#tableDivMenu").find("#MenuName").val(),
        ActionName: $("#tableDivMenu").find("#ActionName").val(),
        ControllerName: $("#tableDivMenu").find("#ControllerName").val(),
        SubmenuId: $("#tableDivMenu").find("#SubmenuId").val(),
        MenuKeyValues: $("#tableDivMenu").find("#menuKey").val()
    }
    if (model.MenuName == "") {
        iserror = true;
        $("#validationmessageName").show();
        $("#validationmessageName").html("Menu Name is required");
    }
    if (iserror) {
        return false;
    }
    else {
        $.ajax({
            type: "POST",
            data: JSON.stringify(model),
            url: constantMenu.saveMenuData,
            contentType: "application/json",
            success: function (result) {
                
                $("#tableDivMenu").html('');
                $("#tableDivMenu").html(result);
                table = $("#tableDivMenu").find('#AddMenuListRecordTable').DataTable({
                    bFilter: false,
                    bInfo: false,
                    dom: 'frtlip'
                });
                $(".hrtoolLoader").hide();
                $(".modal-backdrop").hide();
            }
        });
    }
});

$('#tableDivMenu').on('click', '.dataTr', function () {
    
    if ($(this).hasClass('dataTr')) {
        $('#AddMenuListRecordTable tbody').find('tr.selected').removeClass('selected');
        $(this).addClass('selected');
        $("#tableDivMenu").find(".btn-edit-menu").removeAttr('disabled');
        $("#tableDivMenu").find(".btn-delete-menu").removeAttr('disabled');
    }
});
$("#tableDivMenu").on('click', '.btn-Refresh-menu', function () {

    window.location.reload();
});
$("#tableDivMenu").on('click', '.btn-ClearSorting-menu', function () {

    window.location.reload();
});
$("#tableDivMenu").on('click', '.btn-clearFilter-menu', function () {

    window.location.reload();
});

$("#tableDivMenu").on('click', '.btn-delete-menu', function () {
    var id = $("#tableDivMenu").find("#AddMenuListRecordTable tbody").find('.selected').attr("id");
    $.Zebra_Dialog(" are you sure want to Delete this Records!?", {
        'type': false,
        'title': 'Delete Project Record',
        'width': 350,
        'buttons': [
            {
                caption: 'Cancel',
                callback: function () { }
            }, {
                caption: 'Ok',
                callback: function () {
                    $.ajax({
                        url: constantMenu.DeleteMenuUrl,
                        data: { Id: id },
                        success: function (data) {
                            window.location.reload();
                            $("#tableDivMenu").find('#addMenuBody').html('');
                            $("#tableDivMenu").find('#addMenuBody').html(data);
                            $("#tableDivMenu").find("#addMenuBody").find("#btn-submit-ProjectRecord").html("Add");
                            $('[data-toggle="tooltip"]').tooltip();

                        }
                    });
                }
            }]
    });
});