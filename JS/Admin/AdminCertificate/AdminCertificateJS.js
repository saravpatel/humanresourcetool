$(document).ready(function () {
    DataTableDesign();
});
function DataTableDesign() {
    $('#AdminCertificateTable tfoot tr').appendTo('#AdminCertificateTable thead');
    var table = $('#AdminCertificateTable').DataTable({
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

    $("#tableDiv").on('keyup', '.SearchCertificate', function () {
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

    $("#tableDiv").on('keyup', '.SearchBody', function () {
        table.column(8).search(this.value).draw();
    });

    $("#tableDiv").on('keyup', '.SearchNumber', function () {
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

$("#tableDiv").on('click', '.btn-add-AdminCertificate', function () {
    $.ajax({
        url: constantAdminCertificate.addEdit,
        data: { Id: 0 },
        success: function (data) {
            $("#tableDiv").find('#AdminCertificateBody').html('');
            $("#tableDiv").find('#AdminCertificateBody').html(data);

            $("#tableDiv").find('#AdminCertificateBody').find("#txt_ValidFrom").Zebra_DatePicker({
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {
                    $("#tableDiv").find('#AdminCertificateBody').find("#lbl-error-ValidFrom").hide();
                    $("#lbl-error-GreaterEndDate").hide();
                    var validFrom = $("#txt_ValidFrom").val();
                    var expiryDate = $("#txt_ExpiryDate").val();
                    calculateDateDiff(validFrom, expiryDate);
                }
            });

            $("#tableDiv").find('#AdminCertificateBody').find("#txt_ExpiryDate").Zebra_DatePicker({
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {
                    $("#tableDiv").find('#AdminCertificateBody').find("#lbl-error-ExpiryDate").hide();
                    $("#lbl-error-GreaterEndDate").hide();
                    var validFrom = $("#txt_ValidFrom").val();
                    var expiryDate = $("#txt_ExpiryDate").val();
                    calculateDateDiff(validFrom, expiryDate);
                }
            });

            $('[data-toggle="tooltip"]').tooltip();

            $("#tableDiv").find('#AdminCertificateBody').find('#wizard').smartWizard({
                onLeaveStep: leaveAStepCallback,
                onFinish: onFinishCallback
            });

            $("#tableDiv").find('#AdminCertificateBody').find('.buttonNext').addClass('btn btn-warning');
            $("#tableDiv").find('#AdminCertificateBody').find('.buttonPrevious').addClass('btn btn-warning');
            $("#tableDiv").find('#AdminCertificateBody').find('.buttonFinish').addClass('btn btn-success');

            $("#tableDiv").find('#AdminCertificateBody').find('.buttonPrevious').hide();
            $("#tableDiv").find('#AdminCertificateBody').find('.buttonFinish').hide();

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
                    url: constantAdminCertificate.ImageUrl,
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
    var IsCustomer = $("#tableDiv").find("#AdminCertificateModal").find("#IsCustomerHidden").val();
    if (context.fromStep == 1) {
        var isError = false;
        var name = $("#tableDiv").find('#AdminCertificateBody').find('#txt_Name').val().trim();
        var certificateType = $("#tableDiv").find('#AdminCertificateBody').find('#drp-CertificateType').val();
        var body = $("#tableDiv").find('#AdminCertificateBody').find('#txt_Body').val().trim();
        var number = $("#tableDiv").find('#AdminCertificateBody').find('#txt_Number').val().trim();
        var assignTo = $("#tableDiv").find('#AdminCertificateBody').find('#AssignToID').val();
        var inRelationTo = $("#tableDiv").find('#AdminCertificateBody').find('#InRelationTo').val();
        var validFrom = $("#tableDiv").find('#AdminCertificateBody').find('#txt_ValidFrom').val().trim();
        var expiringDate = $("#tableDiv").find('#AdminCertificateBody').find('#txt_ExpiryDate').val().trim();
        //var status = $("#tableDiv").find('#AdminCertificateBody').find('#drp-Status').val();

        if (name == "") { isError = true; $("#tableDiv").find('#AdminCertificateBody').find("#lbl-error-Name").show(); }
        if (certificateType == "0") { isError = true; $("#tableDiv").find('#AdminCertificateBody').find("#lbl-error-CertificateType").show(); }
        if (body == "") { isError = true; $("#tableDiv").find('#AdminCertificateBody').find("#lbl-error-Body").show(); }
        if (number == "") { isError = true; $("#tableDiv").find('#AdminCertificateBody').find("#lbl-error-Number").show(); }
        if (assignTo == "") { isError = true; $("#tableDiv").find('#AdminCertificateBody').find("#lbl-error-AssignTo").show(); }
        if (inRelationTo == "") { isError = true; $("#tableDiv").find('#AdminCertificateBody').find("#lbl-error-InRelationTo").show(); }
        if (validFrom == "") { isError = true; $("#tableDiv").find('#AdminCertificateBody').find("#lbl-error-ValidFrom").show(); }
        if (expiringDate == "") { isError = true; $("#tableDiv").find('#AdminCertificateBody').find("#lbl-error-ExpiryDate").show(); }
        //if (status == "0") { isError = true; $("#tableDiv").find('#AdminCertificateBody').find("#lbl-error-Status").show(); }
    
        if (IsCustomer == 1)
        {          
                if (isError) {
                    return false;
                }
                else {
                    $("#tableDiv").find('#AdminCertificateBody').find('.buttonNext').hide();
                    $("#tableDiv").find('#AdminCertificateBody').find('.buttonPrevious').show();
                    //$("#tableDiv").find('#AdminCertificateBody').find('.buttonFinish').show()
                }
                return true;       
        }
        else
            {
            if (isError) {
                return false;
            }
            else {
                $("#tableDiv").find('#AdminCertificateBody').find('.buttonNext').hide();
                $("#tableDiv").find('#AdminCertificateBody').find('.buttonPrevious').show();
                $("#tableDiv").find('#AdminCertificateBody').find('.buttonFinish').show()
            }
            return true;
        }
    }
    else {
        $("#tableDiv").find('#AdminCertificateBody').find('.buttonNext').show();
        $("#tableDiv").find('#AdminCertificateBody').find('.buttonPrevious').hide();
        $("#tableDiv").find('#AdminCertificateBody').find('.buttonFinish').hide()
        return true;
    }
    
}

function onFinishCallback(obj, context) {
    var Id = $("#tableDiv").find("#AdminCertificateTable tbody").find('.selected').attr("id");   
    var name = $("#tableDiv").find('#AdminCertificateBody').find('#txt_Name').val().trim();
    var certificateType = $("#tableDiv").find('#AdminCertificateBody').find('#drp-CertificateType').val();
    var body = $("#tableDiv").find('#AdminCertificateBody').find('#txt_Body').val().trim();
    var number = $("#tableDiv").find('#AdminCertificateBody').find('#txt_Number').val().trim();
    var assignTo = $("#tableDiv").find('#AdminCertificateBody').find('#AssignToID').val();
    var inRelationTo = $("#tableDiv").find('#AdminCertificateBody').find('#InRelationTo').val();
    var validFrom = $("#tableDiv").find('#AdminCertificateBody').find('#txt_ValidFrom').val().trim();
    var expiringDate = $("#tableDiv").find('#AdminCertificateBody').find('#txt_ExpiryDate').val().trim();
    var status = $("#tableDiv").find('#AdminCertificateBody').find('#drp-Status').val();
    var alterBeforeDays = $("#tableDiv").find('#AdminCertificateBody').find('#txt_AlertBeforeDays').val().trim();
    var description = $("#tableDiv").find('#AdminCertificateBody').find('#textArea_Description').val().trim();
    var mandatory = $("#tableDiv").find('#AdminCertificateBody').find('#chk_Mandatory').prop("checked");
    var validate = $("#tableDiv").find('#AdminCertificateBody').find('#chk_Validate').prop("checked");

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
        Id: Id,
        Name: name,
        TypeId: certificateType,
        Body: body,
        Number: number,
        AssignToId: assignTo,
        InRelationToId: inRelationTo,
        ValidFrom: validFrom,
        ExpiryDate: expiringDate,
        StatusId: status,
        AlertBeforeDays: alterBeforeDays,
        Description: description,
        Mandatory: mandatory,
        Validate: validate,
        jsonDocumentListString: JsondocumentListJoinString,
        FilterSearch: currentFilter
    }

    $.ajax({
        url: constantAdminCertificate.SaveData,
        type: 'POST',
        data: JSON.stringify(model),
        contentType: "application/json",
        success: function (data) {
            $.ajax({
                url: constantAdminCertificate.updateCount,
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
        $('#AdminCertificateTable tbody').find('tr.selected').removeClass('selected');
        $(this).addClass('selected');
        $("#tableDiv").find(".btn-edit-AdminCertificate").removeAttr('disabled');
        $("#tableDiv").find(".btn-delete-AdminCertificate").removeAttr('disabled');
    }
});
function calculateDateDiff(stratDate, endDate) {
    if (stratDate != "" || endDate != "") {
        if (StartDateValidation(stratDate, endDate)) {
            $("#tableDiv").find("#lbl-error-GreaterEndDate").show();
            $("#tableDiv").find("#txt_ExpiryDate").val('');
        }
    }
}
$("#tableDiv").on('click', '.btn-edit-AdminCertificate', function () {
    var id = $("#tableDiv").find("#AdminCertificateTable tbody").find('.selected').attr("id");
    $.ajax({
        url: constantAdminCertificate.addEdit,
        data: { Id: id },
        success: function (data) {
            $("#tableDiv").find('#AdminCertificateBody').html('');
            $("#tableDiv").find('#AdminCertificateBody').html(data);
            $("#tableDiv").find('#AdminCertificateBody').find("#txt_ValidFrom").Zebra_DatePicker({
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {
                    var validFrom = $("#txt_ValidFrom").val();
                    var expiryDate = $("#txt_ExpiryDate").val();                
                    calculateDateDiff(validFrom, expiryDate);
                    $("#lbl-error-GreaterEndDate").hide();
                    $("#tableDiv").find('#AdminCertificateBody').find("#lbl-error-ValidFrom").hide();                   
                }
            });
            $("#tableDiv").find('#AdminCertificateBody').find("#txt_ExpiryDate").Zebra_DatePicker({
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {
                    $("#tableDiv").find('#AdminCertificateBody').find("#lbl-error-ExpiryDate").hide();
                    var validFrom = $("#txt_ValidFrom").val();
                    var expiryDate = $("#txt_ExpiryDate").val();
                    $("#lbl-error-GreaterEndDate").hide();
                    calculateDateDiff(validFrom, expiryDate);
                }
            });
            $('[data-toggle="tooltip"]').tooltip();
            $("#tableDiv").find('#AdminCertificateBody').find('#wizard').smartWizard({
                onLeaveStep: leaveAStepCallback,
                onFinish: onFinishCallback
            });

            $("#tableDiv").find('#AdminCertificateBody').find('.buttonNext').addClass('btn btn-warning');
            $("#tableDiv").find('#AdminCertificateBody').find('.buttonPrevious').addClass('btn btn-warning');
            $("#tableDiv").find('#AdminCertificateBody').find('.buttonFinish').addClass('btn btn-success');

            $("#tableDiv").find('#AdminCertificateBody').find('.buttonPrevious').hide();
            $("#tableDiv").find('#AdminCertificateBody').find('.buttonFinish').hide();

        }
    });
});

$("#tableDiv").on('click', '.btn-delete-AdminCertificate', function () {
    
    var id = $("#tableDiv").find("#AdminCertificateTable tbody").find('.selected').attr("id");
    var currentFilter = $(".in-submenu").find('.active').attr('class').replace('active', '').trim();
    $.Zebra_Dialog("Are you sure you would like to delete this record?", {
        'type': false,
        'title': 'Delete Certificate Record',
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
                        url: constantAdminCertificate.DeleteData,
                        data: { Id: id, search: currentFilter },
                        success: function (data) {
                            $.ajax({
                                url: constantAdminCertificate.updateCount,
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

$("#tableDiv").on('click', '.btn-Refresh-AdminCertificate', function () {
    window.location.reload();
});

$("#tableDiv").on('click', '.btn-ClearSorting-AdminCertificate', function () {
    window.location.reload();
});

$("#tableDiv").on('click', '.btn-clearFilter-AdminCertificate', function () {
    window.location.reload();
});

$(".listCertification").on('click', '.filter_Class', function () {
    //var currentFilter = $(this).find('a').attr('class');
    //$(".in-submenu").find('.active').removeClass("active");
    //$(this).find('a').addClass('active');
      var currentFilter = $(this).find('a').attr('class');
      $('.userCeritificate li.active').removeClass('active');
    $(this).closest('li').addClass('active');
    $(".hrtoolLoader").show();
    $.ajax({
        url: constantAdminCertificate.ListData,
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