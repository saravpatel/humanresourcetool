//var table;
var edit = false;
var save = false;
var updatedArray = [];
$(document).ready(function () {
    DataTableDesign();
    $('#otherSettingTable_wrapper').find('#otherSettingTable_filter').hide();
    $('#otherSettingTable_wrapper').find('#otherSettingTable_info').hide();
    //table = $('#otherSettingTable').DataTable({
    //    bFilter: false,
    //    bInfo: false,
    //    dom: 'frtlip'
    //});
});
function DataTableDesign() {
    var table = $('#otherSettingTable').DataTable({
        "sDom": '<"top"i>rt<"bottom"flp><"clear">'
    });
    //$('#otherSettingTable').find('.dataTables_filter').hide();
    //$('#otherSettingTable').find('.dataTables_info').hide();
    $('#otherSettingTable tfoot tr').appendTo('#otherSettingTable thead');
    $("#otherSettingTable thead .SystemListName").keyup(function () {
        table.column(0).search(this.value).draw();
    });
}
function EditSystemListValue(count) {
    var ListValue = $('#Value_' + count).text();
    $('#txt-value').val(ListValue);
    $('#hidden').val(count);
    $("#tableDiv").find("#btn-add-value").hide();
    $("#tableDiv").find("#btn-edit-value").show();
    $("#tableDiv").find("#btn-cancel-value").show();
}
$("#tableDiv").on('click', '#btn-cancel-value', function () {
    $("#tableDiv").find("#btn-add-value").show();
    $("#tableDiv").find("#btn-edit-value").hide();
    $("#tableDiv").find("#btn-cancel-value").hide();
    $("#tableDiv").find("#txt-value").val('');
});
$("#tableDiv").on('click', '#btn-edit-value', function () {
    edit = true;
    save = false;
    var value = $("#tableDiv").find("#txt-value").val().trim();
    var test = $("#tableDiv").find("#hidden").val();
    var id = $("#tableDiv").find("#hidden_" + test).val();
    if (value != "") {
        $("#tableDiv").find("#table-value-setting tbody tr td").find("#Value_" + test).text(value);
        $("#tableDiv").find("#txt-value").val("");
        $("#tableDiv").find("#lbl-error-SystemListValueName").hide();
        var updateObject = { ListValue: "", Id: "" };
        updateObject = {
            ListValue: $("#tableDiv").find("#table-value-setting tbody tr td").find("#Value_" + test).text(),
            Id: id
        }
        updatedArray.push(updateObject);
    }
    else {
        return;
    }
});

$('#tableDiv').on('click', '.dataTr', function () {
    if ($(this).hasClass('dataTr')) {
        $('#otherSettingTable tbody').find('tr.selected').removeClass('selected');
        $(this).addClass('selected');
        $("#tableDiv").find(".btn-edit-otherSetting").removeAttr('disabled');
    }
});
$('#tableDiv').on('dblclick', '.dataTr', function () {
    if ($(this).hasClass('dataTr')) {
        $('#otherSettingTable tbody').find('tr.selected').removeClass('selected');
        $(this).addClass('selected');
        $(".btn-edit-otherSetting").click();
    }
});
$("#tableDiv").on('click', '.btn-add-otherSetting', function () {
    $.ajax({
        url: constantOtherSetting.addEdit,
        data: { Id: 0 },
        success: function (data) {
            $("#tableDiv").find('#otherSettingBody').html('');
            $("#tableDiv").find('#otherSettingBody').html(data);
            $("#tableDiv").find("#otherSettingModal").find("#btn-submit-othersetting").html("Add");
            $('[data-toggle="tooltip"]').tooltip();
            $("#otherSettingModal").find("#lbl-error-style").hide();
        }
    });
});
$("#tableDiv").on('click', '.btn-edit-otherSetting', function () {
    var id = $("#tableDiv").find("#otherSettingTable tbody").find('.selected').attr("id");
    var isError = false;

    $.ajax({
        url: constantOtherSetting.addEdit,
        data: { Id: id },
        success: function (data) {
            $("#tableDiv").find('#otherSettingBody').html('');
            $("#tableDiv").find('#otherSettingBody').html(data);
            $('[data-toggle="tooltip"]').tooltip();
            var keyName = $("#tableDiv").find("#txt-SystemListName").val().trim();
            $("#tableDiv").find("#txt-SystemListName").attr("disabled", "disabled");
            $("#tableDiv").find("#otherSettingModal").find(".modal-title").html("");
            $("#tableDiv").find("#otherSettingModal").find(".modal-title").html(keyName);
            $("#tableDiv").find("#otherSettingModal").find("#btn-submit-othersetting").html("Save");
            $("#otherSettingModal").find("#lbl-error-style").hide();
        }
    });
});

$("#tableDiv").on('click', '#btn-add-value', function () {
    edit = false;
    save = true;
    var value = $("#tableDiv").find("#txt-value").val().trim();
    if (value != "") {
        var trString = '<tr class="valuetr_0"><td><label data-valueid="0">' + value + '</label></td><td></td></tr>';
        if ($("#tableDiv").find('#table-value-setting tbody tr').length > 0) {
            $("#tableDiv").find('#table-value-setting tbody tr:last').after(trString);
        }
        else {
            $("#tableDiv").find('#table-value-setting tbody').html(trString);
        }
        $("#tableDiv").find("#txt-value").val("");
        $("#tableDiv").find("#lbl-error-SystemListValueName").hide();
    }
    else {
        return;
    }
});

$("#tableDiv").on('click', '#btn-submit-othersetting', function () {
    var isError = false;
    var id = $("#tableDiv").find("#hidden-Id").val();
    var keyName = $("#tableDiv").find("#txt-SystemListName").val().trim();
    var keyValueArray = [];
    var updateArrayValue;
    var flag = false;
    var test = $("#tableDiv").find("#hidden").val();
    var value = $("#tableDiv").find('#table-value-setting tbody tr td').find("#Value_" + test).text();
    if (keyName == "") {
        isError = true;
        $("#tableDiv").find("#lbl-error-style").show();
    }
    var keyValue = $("#tableDiv").find('#table-value-setting tbody tr').length;
    if (keyValue == 0) {
        isError = true;
        $("#tableDiv").find("#lbl-error-SystemListValueName").show();
    }
    else {
        var pushValue;
        $.each($("#tableDiv").find('#table-value-setting tbody').find(".valuetr_0"), function () {
            pushValue = $(this).find('label').html().trim();
            keyValueArray.push(pushValue);
        });
        if (edit == true) {
            flag = true;
            updateArrayValue = JSON.stringify(updatedArray);
        }
        if (save == true) {
            flag = false;
            updateArrayValue = JSON.stringify(keyValueArray);
        }
    }
    if (!isError) {
        $.ajax({
            url: constantOtherSetting.saveData,
            data: {

                Id: id,
                ListName: keyName,
                ListValue: updateArrayValue,
                Flag: flag
            },
            success: function (data) {
                $("#tableDiv").html('');
                $("#tableDiv").html(data);
                updatedArray = [];
                updateArrayValue = [];
                if (id > 0) {
                    $(".toast-info").show();
                    setTimeout(function () { $(".toast-info").hide(); }, 1500);
                }
                else {
                    $(".toast-success").show();
                    setTimeout(function () { $(".toast-success").hide(); }, 500);
                }
                DataTableDesign();
                $(".hrtoolLoader").hide();
                $(".modal-backdrop").hide();
            }
        });
    }
});
$('.select-btn').click(function () {
    debugger;
    //var id = $(this).attr('data-id');
});

function DeleteSystemListValue(id) {
    if (confirm("are you sure want to Delete this Value!?")) {       
        $.ajax({
            url: constantOtherSetting.deleteData,
            data: { Id: id },
            success: function (data) {                
                $("#tableDiv").html('');
                $("#tableDiv").html(data);
                updatedArray = [];
                updateArrayValue = [];
                if (id > 0) {
                    $(".toast-info").show();
                    setTimeout(function () { $(".toast-info").hide(); }, 1500);
                }
                else {
                    $(".toast-success").show();
                    setTimeout(function () { $(".toast-success").hide(); }, 500);
                }
                DataTableDesign();
                $(".hrtoolLoader").hide();
                $(".modal-backdrop").hide();
            }
        });
    }
}

