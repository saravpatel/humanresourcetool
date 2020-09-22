var table, imageData;
$(document).ready(function () {
    //$('#AddAssetListRecord tfoot tr').appendTo('#AddAssetListRecord thead');
    //var table = $('#AddAssetListRecord').DataTable({
    //    "sDom": '<"top"i>rt<"bottom"flp><"clear">',
    //    "scrollX": true
    //});
    //$('#tableDiv').find('.dataTables_filter').hide();
    //$('#tableDiv').find('.dataTables_info').hide();
    //$('#tableDiv thead .SearchName').keyup(function () {
    //    alert(1);
    //    table.column(0).search(this.value).draw();
    //});
    //$('#tableDiv thead .SearchAssetType').keyup(function () {
    //    table.column(1).search(this.value).draw();
    //});
    //$("#tableDiv thead .SearchAssetType2").keyup(function () {
    //    table.column(2).search(this.value).draw();
    //});
    //$("#tableDiv thead .SearchAssetOwner").keyup(function () {
    //    table.column(3).search(this.value).draw();
    //});
});
$("#btn-Close-AssetsOtherSetting").on('click', function () {
    $("#AddAssesetModel").modal('show');
})
$("#tableDiv").on('change', '#Assettype2', function (e) {
    $("#validatiAsset2").hide();
    var value = $(this).val();
    var assId = $("#AssetsTypeId_2").val();
    var SetId = $("#Assettype1").val();
    if (value == "AddOtherSettings2") {
        $.ajax({
            url: constant.AddeditOtherSetting,
            data: { Id: assId },
            success: function (data) {
                debugger;
                $("#AddAssesetModel").modal('hide');
                $("#otherSettingModal").find('#otherSettingBody').html('');
                $("#otherSettingModal").find('#otherSettingBody').html(data);
                $("#otherSettingModal").modal('show');
                $("#tableDiv").find("#otherSettingModal").find("#btn-submit-othersetting").html("Add");
                $('[data-toggle="tooltip"]').tooltip();
                $("#otherSettingModal").find("#lbl-error-style").hide();
            }
        });
    }
})

$("#tableDiv").on('change', '#Assettype1', function (e) {
    $("#validatiAsset1").hide();
    var value = $(this).val();
    var assId = $("#AssetsTypeId_1").val();
    var SetId = $("#Assettype1").val();
    if (value == "AddOtherSettings") {
        $.ajax({
            url: constant.AddeditOtherSetting,
            data: { Id: assId },
            success: function (data) {
                debugger;
                $("#AddAssesetModel").modal('hide');
                $("#otherSettingModal").find('#otherSettingBody').html('');
                $("#otherSettingModal").find('#otherSettingBody').html(data);
                $("#otherSettingModal").modal('show');
                $("#tableDiv").find("#otherSettingModal").find("#btn-submit-othersetting").html("Add");
                $('[data-toggle="tooltip"]').tooltip();
                $("#otherSettingModal").find("#lbl-error-style").hide();
            }
        });
    }
})
function EditSystemListValue(count) {
    var ListValue = $('#Value_' + count).text();
    $('#txt-value').val(ListValue);
    $('#hidden').val(count);
    $("#tableDiv").find("#btn-add-value").hide();
    $("#tableDiv").find("#btn-edit-value").show();
    $("#tableDiv").find("#btn-cancel-value").show();
}
function AddOtherSettingAssets()
{
    edit = false;
    save = true;
    var value = $("#txt-value").val();
    if (value != "") {
        var trString = '<tr class="valuetr_0"><td><label data-valueid="0">' + value + '</label></td><td></td></tr>';
        if ($('#table-value-setting tbody tr').length > 0) {
            $('#table-value-setting tbody tr:last').after(trString);
        }
        else {
            $('#table-value-setting tbody').html(trString);
        }
        $("#txt-value").val("");
        $("#lbl-error-SystemListValueName").hide();
    }
    else {
        return;
    }
}
function AddOtherSettingValue_Asset()
{
    var isError = false;
    var id = $("#hidden-Id").val();
    var keyName = $("#txt-SystemListName").val().trim();
    var keyValueArray = [];
    var updateArrayValue;
    var flag = false;
    var test = $("#hidden").val();
    var value = $('#table-value-setting tbody tr td').find("#Value_" + test).text();
    if (keyName == "") {
        isError = true;
        $("#lbl-error-style").show();
    }
    var keyValue = $('#table-value-setting tbody tr').length;
    if (keyValue == 0) {
        isError = true;
        $("#lbl-error-SystemListValueName").show();
    }
    else {
        var pushValue;
        $.each($('#table-value-setting tbody').find(".valuetr_0"), function () {
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
            url: constant.saveOtherSetting,
            data: {
                Id: id,
                ListName: keyName,
                ListValue: updateArrayValue,
                Flag: flag
            },
            success: function (data) {
                $("#otherSettingModal").modal('hide');
                $("#AddAssesetModel").find('#addAssetsBody').html('');
                $("#AddAssesetModel").find('#addAssetsBody').html(data);
                $("#AddAssesetModel").modal('show');
                $("#tableDiv").find("#AddAssesetModel").find(".AssetTitle").text('Add Asset');
                $("#tableDiv").find("#addAssetsBody").find("#btn-submit-AddAssetsRecord").html("Add");
                $('[data-toggle="tooltip"]').tooltip();
                $("#tableDiv").find("#addTechnicalSkillsBody").find("#drp-TechnicalSkills").selectList();
            }
        });
    }
}



$("#tableDiv").on('change', '#fileToUpload', function (e) {
    
    var files = e.target.files;
    imageData = "";
    if (files.length > 0) {
        if (window.FormData !== undefined) {
            imageData = new FormData();
            for (var x = 0; x < files.length; x++) {
                imageData.append("file" + x, files[x]);
            }
        }
    }
    else {
        $(".hrtoolLoader").hide();
        $(".modal-backdrop").hide();
    }
});

function readURL(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $("#tableDiv").find('.editpic')
                .attr('src', e.target.result);
        };
        reader.readAsDataURL(input.files[0]);
    }
}

$("#tableDiv").on('click', '#btn-submit-AddAssetsRecord', function () {    
    $(".hrtoolLoader").show();
    var isError = false;
    var id = $("#tableDiv").find("#hidden-Id").val();
    var AssetName = $("#AssetName").val();
    if (AssetName == "") {
        isError = true;
        $("#validationmessageName").show();
        $("#validationmessageName").html("Name is required");
    }
    var Asset1 = $("#Assettype1").val();
    if (Asset1 == "0") {
        isError = true;
        $("#validationmessageAsset1").show();
        $("#validationmessageAsset1").html("AssetType is required");
    }
    var Asset2 = $("#Assettype2").val();
    if (Asset2 == "0") {
        isError = true;
        $("#validationmessageAsset2").show();
        $("#validationmessageAsset2").html("AssetType2 is required");
    }
    var SetId = $("#Assettype1").val();
    if (SetId == "AddOtherSettings") {
        isError = true;
        $("#validatiAsset1").show();
    }
    var SetId2 = $("#Assettype2").val();
    if (SetId2 == "AddOtherSettings2")
    {
        isError = true;
        $("#validatiAsset2").show();
    }
    var Owner = $("#Ownar").val();
    if (Owner == "0") {
        isError = true;
        $("#validationmessageowner").show();
        $("#validationmessageowner").html("The Asset Owner is required.");
    }
    var ajaxUrl = constant.saveAssetsData + '?Id=' + id + '&Name=' + AssetName + '&Assets1=' + Asset1 + '&Assets2=' + Asset2 + '&OwnerId=' + Owner;
    if (isError) {
        $(".hrtoolLoader").hide();
        return false;
    }
    else {
        $.ajax({
            type: "POST",
            url: ajaxUrl,
            contentType: false,
            processData: false,
            data: imageData,
            success: function (result) {
                $("#tableDiv").html('');
                $("#tableDiv").html(result);
                table = $("#tableDiv").find('#AddAssetListRecord').DataTable({
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

$("#tableDiv").on('click', '.btn-add-addAssets', function () {
    $.ajax({
        url: constant.addEdit,
        data: { Id: 0 },
        success: function (data) {
            $("#tableDiv").find('#addAssetsBody').html('');
            $("#tableDiv").find('#addAssetsBody').html(data);
            $("#tableDiv").find("#AddAssesetModel").find(".AssetTitle").text('Add Asset');
            $("#tableDiv").find("#addAssetsBody").find("#btn-submit-AddAssetsRecord").html("Add");
            $('[data-toggle="tooltip"]').tooltip();
            $("#tableDiv").find("#addTechnicalSkillsBody").find("#drp-TechnicalSkills").selectList();
        }
    });
});

$("#tableDiv").on('click', '.btn-edit-addAssets', function () {
    var id = $("#tableDiv").find("#AddAssetListRecord tbody").find('.selected').attr("id");
    $.ajax({
        url: constant.addEdit,
        data: { Id: id },
        success: function (data) {
            $("#tableDiv").find('#addAssetsBody').html('');
            $("#tableDiv").find('#addAssetsBody').html(data);
            $("#tableDiv").find("#AddAssesetModel").find(".AssetTitle").text('Edit Asset');
            $("#tableDiv").find("#addAssetsBody").find("#btn-submit-AddAssetsRecord").html("Save");
            $('[data-toggle="tooltip"]').tooltip();
            $("#tableDiv").find("#addAssetsBody").find("#drp-TechnicalSkills").selectList();
        }
    });
});

$('#tableDiv').on('click', '.dataTr', function () {
    if ($(this).hasClass('dataTr')) {
        $('#AddAssetListRecord tbody').find('tr.selected').removeClass('selected');
        $(this).addClass('selected');
        $("#tableDiv").find(".btn-edit-addAssets").removeAttr('disabled');
        $("#tableDiv").find(".btn-delete-addAssets").removeAttr('disabled');
    }
});

$("#tableDiv").on('click', '.btn-delete-addAssets', function () {
    var id = $("#tableDiv").find("#AddAssetListRecord tbody").find('.selected').attr("id");
    $.Zebra_Dialog(" are you sure want to Delete this Records!?", {
        'type': false,
        'title': 'Delete Asset Record',
        'width': 350,
        'buttons': [
            {
                caption: 'Cancel',
                callback: function () { }
            }, {
                caption: 'Ok',
                callback: function () {
                    $.ajax({
                        url: constant.DeleteAsseteUrl,
                        data: { Id: id },
                        success: function (data) {
                            window.location.reload();
                            $("#tableDiv").find('#addAssetsBody').html('');
                            $("#tableDiv").find('#addAssetsBody').html(data);
                            $("#tableDiv").find("#addAssetsBody").find("#btn-submit-AddAssetsRecord").html("Add");
                            $('[data-toggle="tooltip"]').tooltip();

                        }
                    });
                }
            }]
    });
});

$("#tableDiv").on('click', '.btn-Refresh-addAssets', function () {

    window.location.reload();
});
$("#tableDiv").on('click', '.btn-ClearSorting-addAssets', function () {

    window.location.reload();
});
$("#tableDiv").on('click', '.btn-clearFilter-addassets', function () {

    window.location.reload();
});