$(document).ready(function () {
    DataTableDesign();
});

function DataTableDesign() {
    $('#AdminVisaTable tfoot tr').appendTo('#AdminVisaTable thead');
    var table = $('#AdminVisaTable').DataTable({
        "sDom": '<"top"i>rt<"bottom"flp><"clear">',
        "scrollX": true,
        "bSort": false
    });
    $('#tableDiv').find('.dataTables_filter').hide();
    $('#tableDiv').find('.dataTables_info').hide();
    $("#tableDiv").on('keyup', '.SearchAssigned', function () {
        table.column(0).search(this.value).draw();
    });
    $("#tableDiv").on('keyup', '.SearchJobTitle', function () {
        table.column(1).search(this.value).draw();
    });
    $("#tableDiv").on('keyup', '.SearchPool', function () {
        table.column(2).search(this.value).draw();
    });
    $("#tableDiv").on('keyup', '.SearchFunction', function () {
        table.column(3).search(this.value).draw();
    });
    $("#tableDiv").on('keyup', '.SearchCountry', function () {
        table.column(4).search(this.value).draw();
    });
    $("#tableDiv").on('keyup', '.SearchType', function () {
        table.column(5).search(this.value).draw();
    });
    $("#tableDiv .SearchDueDate").Zebra_DatePicker({
        showButtonPanel: false,
        format: 'd-M-Y',
        onSelect: function () {
            var date = $("#tableDiv").find('.SearchDueDate').val();
            table.column(6).search(date).draw();
        }
    });

    $("#tableDiv").on('keyup', '.SearchExpireinDays', function () {
        table.column(7).search(this.value).draw();
    });

    $("#tableDiv").on('keyup', '.SearchNumber', function () {
        table.column(8).search(this.value).draw();
    });

    $("#tableDiv").on('keyup', '.SearchAgency', function () {
        table.column(9).search(this.value).draw();
    });

    $("#tableDiv").on('keyup', '.SearchRelation', function () {
        table.column(10).search(this.value).draw();
    });

    $("body").on('click', '.dp_clear', function () {
        var date = $("#tableDiv").find('.SearchDueDate').val();
        table.column(6).search(date).draw();
    });
}
function calculateDateDiff(stratDate, endDate) {
    if (stratDate != "" || endDate != "") {
        if (StartDateValidation(stratDate, endDate)) {
            $("#tableDiv").find("#lbl-error-GreaterEndDate").show();
            $("#tableDiv").find("#txt_ExpiryDate").val('');
        }
    }
}
$("#tableDiv").on('click', '.btn-add-AdminVisa', function () {
    $.ajax({
        url: constantAdminVisa.addEdit,
        data: { Id: 0 },
        success: function (data) {
            $("#tableDiv").find('#AdminVisaBody').html('');
            $("#tableDiv").find('#AdminVisaBody').html(data);
            $("#AdminVisaModal").find('.adminvisaTitle').text("Add Visa");
            $("#tableDiv").find('#AdminVisaBody').find("#txt_ValidFrom").Zebra_DatePicker({
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {
                    $("#tableDiv").find('#AdminVisaBody').find("#lbl-error-ValidFrom").hide();
                    $("#lbl-error-GreaterEndDate").hide();
                    var fromDate = $("#txt_ValidFrom").val();
                    var expDate = $("#txt_ExpiryDate").val();
                    calculateDateDiff(fromDate,expDate);
                }
            });

            $("#tableDiv").find('#AdminVisaBody').find("#txt_ExpiryDate").Zebra_DatePicker({
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {
                    $("#tableDiv").find('#AdminVisaBody').find("#lbl-error-ExpiryDate").hide();
                    $("#lbl-error-GreaterEndDate").hide();
                    var fromDate = $("#txt_ValidFrom").val();
                    var expDate = $("#txt_ExpiryDate").val();
                    calculateDateDiff(fromDate, expDate);
                }
            });

            $('[data-toggle="tooltip"]').tooltip();

            $("#tableDiv").find('#AdminVisaBody').find('#wizard').smartWizard({
                onLeaveStep: leaveAStepCallback,
                onFinish: onFinishCallback
            });

            $("#tableDiv").find('#AdminVisaBody').find('.buttonNext').addClass('btn btn-warning');
            $("#tableDiv").find('#AdminVisaBody').find('.buttonPrevious').addClass('btn btn-warning');
            $("#tableDiv").find('#AdminVisaBody').find('.buttonFinish').addClass('btn btn-success');

            $("#tableDiv").find('#AdminVisaBody').find('.buttonPrevious').hide();
            $("#tableDiv").find('#AdminVisaBody').find('.buttonFinish').hide();

        }
    });
});

$("#tableDiv").on('click', '.file-deleteicon', function () {
    $(this).parent().remove();
});

$("#tableDiv").on('change', '#fileToUpload', function (e) {
    $(".hrtoolLoader").show();
    var files = e.target.files;
    var imageData = "";
    if (files.length > 0) {
        if (window.FormData !== undefined) {
            imageData = new FormData();
            for (var x = 0; x < files.length; x++) {
                imageData.append("file" + x, files[x]);
            }
            setTimeout(function () {
                $.ajax({
                    type: "POST",
                    url: constantAdminVisa.ImageUrl,
                    contentType: false,
                    processData: false,
                    data: imageData,
                    success: function (result) {
                        var string = '<div class="row ListData"><div class="col-lg-1 icon"><i class="fa fa-paperclip"></i></div><div class="col-lg-10 attach-disc"><label class="fileName" data-newFileName="' + result.NewFileName + '">' + result.originalFileName + '</label><br /><input type="text" class="ImageDescription form-control" placeholder="Add Description Here"/></div><div class="col-lg-1 file-deleteicon"><i class="fa fa-trash-o"></i></div></div>';
                        var isEmpty = $("#tableDiv").find("#filesList").html();
                        if (isEmpty = "") {
                            $("#tableDiv").find("#filesList").html(string);
                        }
                        else {
                            $("#tableDiv").find("#filesList").append(string);
                        }
                        $(".hrtoolLoader").hide();
                        $(".modal-backdrop").hide();
                    }
                });
            }, 500);
        }
    }
    else {
        $(".hrtoolLoader").hide();
        $(".modal-backdrop").hide();
    }
});

function leaveAStepCallback(obj, context) {
    var IsCustomer = $("#tableDiv").find("#AdminVisaBody").find("#IsCustomerHidden").val();
    if (context.fromStep == 1) {
        var isError = false;
        var country = $("#tableDiv").find('#AdminVisaBody').find('#drp-Country').val();
        var visaType = $("#tableDiv").find('#AdminVisaBody').find('#drp-VisaType').val();
        var serviceAgency = $("#tableDiv").find('#AdminVisaBody').find('#drp-ServiceAgency').val();
        var visaNumber = $("#tableDiv").find('#AdminVisaBody').find('#txt_VisaNumber').val().trim();
        var assignTo = $("#tableDiv").find('#AdminVisaBody').find('#drp-AssignTo').val();
        var inRelationTo = $("#tableDiv").find('#AdminVisaBody').find('#drp-InRelationTo').val();
        var validFrom = $("#tableDiv").find('#AdminVisaBody').find('#txt_ValidFrom').val().trim();
        var expiringDate = $("#tableDiv").find('#AdminVisaBody').find('#txt_ExpiryDate').val().trim();
        //var status = $("#tableDiv").find('#AdminVisaBody').find('#drp-Status').val();

        if (country == "0") { isError = true; $("#tableDiv").find('#AdminVisaBody').find("#lbl-error-Country").show(); }
        if (visaType == "0") { isError = true; $("#tableDiv").find('#AdminVisaBody').find("#lbl-error-VisaType").show(); }
        if (serviceAgency == "0") { isError = true; $("#tableDiv").find('#AdminVisaBody').find("#lbl-error-ServiceAgency").show(); }
        if (visaNumber == "") { isError = true; $("#tableDiv").find('#AdminVisaBody').find("#lbl-error-VisaNumber").show(); }
        if (assignTo == "0") { isError = true; $("#tableDiv").find('#AdminVisaBody').find("#lbl-error-AssignTo").show(); }
        if (inRelationTo == "0") { isError = true; $("#tableDiv").find('#AdminVisaBody').find("#lbl-error-InRelationTo").show(); }
        if (validFrom == "") { isError = true; $("#tableDiv").find('#AdminVisaBody').find("#lbl-error-ValidFrom").show(); }
        if (expiringDate == "") { isError = true; $("#tableDiv").find('#AdminVisaBody').find("#lbl-error-ExpiryDate").show(); }
        //if (status == "0") { isError = true; $("#tableDiv").find('#AdminVisaBody').find("#lbl-error-Status").show(); }

        if (IsCustomer == 1) {
            if (isError) {
                return false;
            }
            else {
                $("#tableDiv").find('#AdminVisaBody').find('.buttonNext').hide();
                $("#tableDiv").find('#AdminVisaBody').find('.buttonPrevious').show();
             //   $("#tableDiv").find('#AdminVisaBody').find('.buttonFinish').show()

                return true;
            }
        }
        else
        {
            if (isError) {
                return false;
            }
            else {
                $("#tableDiv").find('#AdminVisaBody').find('.buttonNext').hide();
                $("#tableDiv").find('#AdminVisaBody').find('.buttonPrevious').show();
                $("#tableDiv").find('#AdminVisaBody').find('.buttonFinish').show()

                return true;
            }
        }
    }
    else {

        $("#tableDiv").find('#AdminVisaBody').find('.buttonNext').show();
        $("#tableDiv").find('#AdminVisaBody').find('.buttonPrevious').hide();
        $("#tableDiv").find('#AdminVisaBody').find('.buttonFinish').hide()
        return true;
    }
}

function onFinishCallback(obj, context) {
    var id = $("#tableDiv").find('#AdminVisaBody').find('#HiddenId').val();
    var country = $("#tableDiv").find('#AdminVisaBody').find('#drp-Country').val();
    var visaType = $("#tableDiv").find('#AdminVisaBody').find('#drp-VisaType').val();
    var serviceAgency = $("#tableDiv").find('#AdminVisaBody').find('#drp-ServiceAgency').val();
    var visaNumber = $("#tableDiv").find('#AdminVisaBody').find('#txt_VisaNumber').val().trim();
    var assignTo = $("#tableDiv").find('#AdminVisaBody').find('#drp-AssignTo').val();
    var inRelationTo = $("#tableDiv").find('#AdminVisaBody').find('#drp-InRelationTo').val();
    var validFrom = $("#tableDiv").find('#AdminVisaBody').find('#txt_ValidFrom').val().trim();
    var expiringDate = $("#tableDiv").find('#AdminVisaBody').find('#txt_ExpiryDate').val().trim();
    var status = $("#tableDiv").find('#AdminVisaBody').find('#drp-Status').val();
    var alterBeforeDays = $("#tableDiv").find('#AdminVisaBody').find('#txt_AlertBeforeDays').val().trim();
    var description = $("#tableDiv").find('#AdminVisaBody').find('#textArea_Description').val().trim();

    var currentFilter = $(".in-submenu").find('.active').attr('class').replace('active', '').trim();

    var documentList = [];
    $.each($("#tableDiv").find('#filesList').find(".ListData"), function () {
        var originalName = $(this).find(".fileName").html().trim();
        var newName = $(this).find(".fileName").attr("data-newfilename");
        var description = $(this).find(".ImageDescription").val();
        var oneData = {
            originalName: originalName,
            newName: newName,
            description: description
        }
        documentList.push(oneData);
    });
    var JsondocumentListJoinString = JSON.stringify(documentList);

    var model = {
        Id: id,
        CountryId: country,
        VisaTypeId: visaType,
        ServiceAgencyId: serviceAgency,
        VisaNumber: visaNumber,
        AssignToId: assignTo,
        InRelationToId: inRelationTo,
        ValidFrom: validFrom,
        ExpiryDate: expiringDate,
        StatusId: status,
        AlertBeforeDays: alterBeforeDays,
        Description: description,
        jsonDocumentListString: JsondocumentListJoinString,
        FilterSearch: currentFilter
    }

    $.ajax({
        url: constantAdminVisa.SaveData,
        type: 'POST',
        data: JSON.stringify(model),
        contentType: "application/json",
        success: function (data) {
            $.ajax({
                url: constantAdminVisa.updateCount,
                success: function (result) {
                    $(".in-submenu").html("");
                    $(".in-submenu").html(result);

                    $("#tableDiv").html("");
                    $("#tableDiv").html(data);

                    DataTableDesign();

                    $(".hrtoolLoader").hide();
                    $(".modal-backdrop").hide();

                    if (id > 0) {
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
}

$('#tableDiv').on('click', '.dataTr', function () {
    if ($(this).hasClass('dataTr')) {
        $('#AdminVisaTable tbody').find('tr.selected').removeClass('selected');
        $(this).addClass('selected');
        $("#tableDiv").find(".btn-edit-AdminVisa").removeAttr('disabled');
        $("#tableDiv").find(".btn-delete-AdminVisa").removeAttr('disabled');
    }
});

$("#tableDiv").on('click', '.btn-edit-AdminVisa', function () {
    var id = $("#tableDiv").find("#AdminVisaTable tbody").find('.selected').attr("id");
    $.ajax({
        url: constantAdminVisa.addEdit,
        data: { Id: id },
        success: function (data) {
            $("#tableDiv").find('#AdminVisaBody').html('');
            $("#tableDiv").find('#AdminVisaBody').html(data);
            $("#AdminVisaModal").find('.adminvisaTitle').text("Edit Visa");
            $("#tableDiv").find('#AdminVisaBody').find("#txt_ValidFrom").Zebra_DatePicker({
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {
                    $("#tableDiv").find('#AdminVisaBody').find("#lbl-error-ValidFrom").hide();
                    $("#lbl-error-GreaterEndDate").hide();
                    var fromDate = $("#txt_ValidFrom").val();
                    var expDate = $("#txt_ExpiryDate").val();
                    calculateDateDiff(fromDate, expDate);
                }
            });

            $("#tableDiv").find('#AdminVisaBody').find("#txt_ExpiryDate").Zebra_DatePicker({
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {
                    $("#tableDiv").find('#AdminVisaBody').find("#lbl-error-ExpiryDate").hide();
                    $("#lbl-error-GreaterEndDate").hide();
                    var fromDate = $("#txt_ValidFrom").val();
                    var expDate = $("#txt_ExpiryDate").val();
                    calculateDateDiff(fromDate, expDate);
                }
            });

            $('[data-toggle="tooltip"]').tooltip();

            $("#tableDiv").find('#AdminVisaBody').find('#wizard').smartWizard({
                onLeaveStep: leaveAStepCallback,
                onFinish: onFinishCallback
            });

            $("#tableDiv").find('#AdminVisaBody').find('.buttonNext').addClass('btn btn-warning');
            $("#tableDiv").find('#AdminVisaBody').find('.buttonPrevious').addClass('btn btn-warning');
            $("#tableDiv").find('#AdminVisaBody').find('.buttonFinish').addClass('btn btn-success');

            $("#tableDiv").find('#AdminVisaBody').find('.buttonPrevious').hide();
            $("#tableDiv").find('#AdminVisaBody').find('.buttonFinish').hide();

        }
    });
});

$("#tableDiv").on('click', '.btn-delete-AdminVisa', function () {
    
    var id = $("#tableDiv").find("#AdminVisaTable tbody").find('.selected').attr("id");
    var currentFilter = $(".in-submenu").find('.active').attr('class').replace('active', '').trim();
    $.Zebra_Dialog("Are you sure you would like to delete this record?", {
        'type': false,
        'title': 'Delete Visa Record',
        'width': 350,
        'buttons': [
            {
                caption: 'Cancel',
                callback: function () { }
            }, {
                caption: 'Ok',
                callback: function () {
                    $(".hrtoolLoader").show();
                    $.ajax({
                        url: constantAdminVisa.DeleteData,
                        data: { Id: id, search: currentFilter },
                        success: function (data) {
                            $.ajax({
                                url: constantAdminVisa.updateCount,
                                success: function (result) {
                                    $(".in-submenu").html("");
                                    $(".in-submenu").html(result);

                                    $("#tableDiv").html("");
                                    $("#tableDiv").html(data);

                                    DataTableDesign();

                                    $(".hrtoolLoader").hide();
                                    $(".modal-backdrop").hide();

                                    $(".toast-error").show();
                                    setTimeout(function () { $(".toast-error").hide(); }, 1500);
                                }
                            });
                        }
                    });
                }
            }]
    });
});

$("#tableDiv").on('click', '.btn-Refresh-AdminVisa', function () {
    window.location.reload();
});

$("#tableDiv").on('click', '.btn-ClearSorting-AdminVisa', function () {
    window.location.reload();
});

$("#tableDiv").on('click', '.btn-clearFilter-AdminVisa', function () {
    window.location.reload();
});

$(".listVisa").on('click', '.filter_Class', function () {    
    var currentFilter = $(this).find('a').attr('class');  
    $('.userVisa li.active').removeClass('active');
    $(this).closest('li').addClass('active');
    $(".hrtoolLoader").show();
    $.ajax({
        url: constantAdminVisa.ListData,
        data: { search: currentFilter },
        success: function (data) {
            $("#tableDiv").html("");
            $("#tableDiv").html(data);

            DataTableDesign();

            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
        }
    });
});