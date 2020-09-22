$(document).ready(function () {
    DataTableDesign();
});

function DataTableDesign() {
    $('#AdminCreateRoleTable tfoot tr').appendTo('#AdminCreateRoleTable thead');
    var table = $('#AdminCreateRoleTable').DataTable({
        "sDom": '<"top"i>rt<"bottom"flp><"clear">',
        "scrollX":true
    });
    $('#tableDiv').find('.dataTables_filter').hide();
    $('#tableDiv').find('.dataTables_info').hide();


    $("#tableDiv").on('keyup', '.SearchName', function () {
        table.column(0).search(this.value).draw();
    });

    $("#tableDiv").on('keyup', '.SearchDescription', function () {
        table.column(1).search(this.value).draw();
    });

    $("#tableDiv").on('keyup', '.SearchisActive', function () {
        table.column(2).search(this.value).draw();
    });
}

$("#tableDiv").on('click', '.btn-add-CreateRole', function () {
    $(".hrtoolLoader").show();
    var RoleId = 0;
    $.ajax({
        url: constantCreateRole.addEdit,
        data: { Id: RoleId },
        type: "POST",
        success: function (data) {
            $("#tableDiv").html('');
            $("#tableDiv").html(data);

            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
        }
    });
});

$("#tableDiv").on('click', '.btn-edit-CreateRole', function () {
    var id = $("#tableDiv").find("#AdminCreateRoleTable tbody").find('.selected').attr("id");
    $(".hrtoolLoader").show();
    $.ajax({
        url: constantCreateRole.addEdit,
        data: { Id: id },
        type: "POST",
        success: function (data) {
            $("#tableDiv").html('');
            $("#tableDiv").html(data);

            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
        }
    });
});

$('#tableDiv').on('click', '.dataTr', function () {
    if ($(this).hasClass('dataTr')) {
        $('#AdminCreateRoleTable tbody').find('tr.selected').removeClass('selected');
        $(this).addClass('selected');
        $("#tableDiv").find(".btn-edit-CreateRole").removeAttr('disabled');
    }
});

$('#tableDiv').on('click', '#btn-backToList', function () {
    $(".hrtoolLoader").show();
    $.ajax({
        url: constantCreateRole.ListData,
        success: function (data) {
            $("#tableDiv").html('');
            $("#tableDiv").html(data);
            DataTableDesign();
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
        }
    });
});


$('#tableDiv').on('click', '#btn-save', function () {
    var id = $('#tableDiv').find("#hidden_id").val().trim();
    var name = $('#tableDiv').find("#txt_Name").val().trim();
    var description = $('#tableDiv').find("#textArea_Description").val().trim();
    var active = $('#tableDiv').find("#chk_Active").prop('checked');

    var isError = false;
    if (name == "") {
        $('#tableDiv').find("#lbl-error-Name").show();
        isError = true;
    }

    var selectedId = "";
    $.each($('#tableDiv').find('#menuTable').find('td').find(".checkbox"), function (index, item) {
        if ($(this).prop('checked')) {
            if (selectedId == "") {
                selectedId = $(this).attr('id');
            }
            else {
                selectedId += ',' + $(this).attr('id');
            }
        }
    });
    var model = {
        Id: id,
        Name: name,
        Description: description,
        Active: active,
        SelectedList: selectedId
    }

    if (isError) {
        return false;
    }
    else {
        $(".hrtoolLoader").show();
        $.ajax({
            url: constantCreateRole.SaveData,
            type: 'POST',
            data: JSON.stringify(model),
            contentType: "application/json",
            success: function (data) {
                $("#tableDiv").html('');
                $("#tableDiv").html(data);
                DataTableDesign();
                $(".hrtoolLoader").hide();
                $(".modal-backdrop").hide();

                if (id != "") {
                    $(".toast-info").show();
                    setTimeout(function () { $(".toast-info").hide(); }, 1500);
                }
                else {
                    $(".toast-success").show();
                    setTimeout(function () { $(".toast-success").hide(); }, 1500);
                }
            }
        });
    }
});