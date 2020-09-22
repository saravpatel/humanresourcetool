$(document).ready(function () {
    DataTableDesign();
});

function DataTableDesign() {
    $('#EmployeeBenefitTable tfoot tr').appendTo('#EmployeeBenefitTable thead');
    var table = $('#EmployeeBenefitTable').DataTable({
        "sDom": '<"top"i>rt<"bottom"flp><"clear">',
        "scrollX": true
    });
    $('#tableDiv').find('.dataTables_filter').hide();
    $('#tableDiv').find('.dataTables_info').hide();


    $("#tableDiv thead .SearchBenefit").keyup(function () {
        table.column(0).search(this.value).draw();
    });
    $("#tableDiv thead .SearchValue").keyup(function () {
        table.column(3).search(this.value).draw();
    });

    $("#tableDiv thead .SearchDateAwarded").Zebra_DatePicker({
        //direction: false,
        showButtonPanel: false,
        format: 'd-M-Y',
        onSelect: function () {
            var date = $("#EmployeeBenefitTable").find("thead").find('.SearchDateAwarded').val();
            table.column(1).search(date).draw();
        }
    });

    $("#tableDiv thead .SearchExpiryDate").Zebra_DatePicker({
        //direction: false,
        showButtonPanel: false,
        format: 'd-M-Y',
        onSelect: function () {
            var date = $("#EmployeeBenefitTable").find("thead").find('.SearchExpiryDate').val();
            table.column(2).search(date).draw();
        }
    });

    $("body").on('click', '.dp_clear', function () {
        var DateAwarded = $("#tableDiv").find("thead").find('.SearchDateAwarded').val();
        if (DateAwarded == "") {
            table.column(1).search(DateAwarded).draw();
        }
        var ExpiryDate = $("#tableDiv").find("thead").find('.SearchExpiryDate').val();
        if (DateAwarded == "") {
            table.column(2).search(ExpiryDate).draw();
        }
    });
}
function calculateDateDiff(stratDate, endDate) {
    debugger;
    if (stratDate != "" || endDate != "") {
        if (StartDateValidation(stratDate, endDate)) {
            $("#lbl-error-validExpiryDate").show();
            $("#txt_ExpiryDate").val('');
        }
        else {
            $("#lbl-error-validExpiryDate").hide();
        }
    }
}
$("#tableDiv").on('click', '.btn-add-EmployeeBenefit', function () {
    $.ajax({
        url: constantEmployeeBenifits.addEdit,
        data: { Id: 0 },
        success: function (data) {
            $("#tableDiv").find('#EmployeeBenefitBody').html('');
            $("#tableDiv").find('#EmployeeBenefitBody').html(data);

            $("#tableDiv").find('#EmployeeBenefitBody').find("#txt_DateAwarded").Zebra_DatePicker({
                //direction: false,
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {
                    $("#tableDiv").find('#EmployeeBenefitBody').find("#lbl-error-DateAwarded").hide();
                    var stratDate = $("#txt_DateAwarded").val();
                    var endDate = $("#txt_ExpiryDate").val();
                    calculateDateDiff(stratDate, endDate);
                }
            });


            $("#tableDiv").find('#EmployeeBenefitBody').find("#txt_ExpiryDate").Zebra_DatePicker({
                //direction: false,
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {
                    $("#tableDiv").find('#EmployeeBenefitBody').find("#lbl-error-ExpiryDate").hide();
                    var stratDate = $("#txt_DateAwarded").val();
                    var endDate = $("#txt_ExpiryDate").val();
                    calculateDateDiff(stratDate, endDate);
                }
            });

            $('[data-toggle="tooltip"]').tooltip();

            $("#tableDiv").find('#EmployeeBenefitBody').find('#wizard').smartWizard({
                onLeaveStep: leaveAStepCallback,
                onFinish: onFinishCallback
            });
            $("#tableDiv").find('#EmployeeBenefitBody').find('#wizard').find('.stepContainer').css('height', '345px');

            $("#tableDiv").find('#EmployeeBenefitBody').find('.buttonNext').addClass('btn btn-warning');
            $("#tableDiv").find('#EmployeeBenefitBody').find('.buttonPrevious').addClass('btn btn-warning');
            $("#tableDiv").find('#EmployeeBenefitBody').find('.buttonFinish').addClass('btn btn-success');

            $("#tableDiv").find('#EmployeeBenefitBody').find('.buttonPrevious').hide();
            $("#tableDiv").find('#EmployeeBenefitBody').find('.buttonFinish').hide();

        }
    });
});


function leaveAStepCallback(obj, context) {
    if (context.fromStep == 1) {
        var isError = false;
        var benifits = $("#tableDiv").find('#EmployeeBenefitBody').find('#drp-Benifits').val();
        if (benifits == "0") { isError = true; $("#tableDiv").find('#EmployeeBenefitBody').find("#lbl-error-BenifitsList").show(); }
        var dateAwarded = $("#tableDiv").find('#EmployeeBenefitBody').find('#txt_DateAwarded').val().trim();
        if (dateAwarded == "") { isError = true; $("#tableDiv").find('#EmployeeBenefitBody').find("#lbl-error-DateAwarded").show(); }
        var dateExpiry = $("#tableDiv").find('#EmployeeBenefitBody').find('#txt_ExpiryDate').val().trim();
        if (dateExpiry == "") { isError = true; $("#tableDiv").find('#EmployeeBenefitBody').find("#lbl-error-ExpiryDate").show(); }

        if (isError) {
            return false;
        }
        else {
            $("#tableDiv").find('#EmployeeBenefitBody').find('.buttonNext').hide();
            $("#tableDiv").find('#EmployeeBenefitBody').find('.buttonPrevious').show();
            $("#tableDiv").find('#EmployeeBenefitBody').find('.buttonFinish').show()

            $("#tableDiv").find('#EmployeeBenefitBody').find("#lbl-error-BenifitsList").hide();
            $("#tableDiv").find('#EmployeeBenefitBody').find("#lbl-error-DateAwarded").hide();
            $("#tableDiv").find('#EmployeeBenefitBody').find("#lbl-error-ExpiryDate").hide();
            return true;
        }
    }
    else {

        $("#tableDiv").find('#EmployeeBenefitBody').find('.buttonNext').show();
        $("#tableDiv").find('#EmployeeBenefitBody').find('.buttonPrevious').hide();
        $("#tableDiv").find('#EmployeeBenefitBody').find('.buttonFinish').hide()
        return true;
    }
}

function onFinishCallback(obj, context) {
    var id = $("#tableDiv").find('#EmployeeBenefitBody').find('#CaseHiddenId').val();
    var employee = $("#tableDiv").parent().find('#currntEmployeeId').val();
    var benifits = $("#tableDiv").find('#EmployeeBenefitBody').find('#drp-Benifits').val();
    var Currency = $("#tableDiv").find("#drpCurrency").val();
    var dateAwarded = $("#tableDiv").find('#EmployeeBenefitBody').find('#txt_DateAwarded').val().trim();
    var dateExpiry = $("#tableDiv").find('#EmployeeBenefitBody').find('#txt_ExpiryDate').val().trim();
    var value = $("#tableDiv").find('#EmployeeBenefitBody').find('#txt_FixedAmount').val().trim();
    var comments = $("#tableDiv").find('#EmployeeBenefitBody').find('#textArea_comments').val().trim();
    var recoverOnTermination = $("#tableDiv").find('#EmployeeBenefitBody').find('#chk_RecoverOnTermination').prop("checked");

    var documentList = [];
    $.each($("#tableDiv").find('#filesList').find(".ListData"), function () {
        var originalName = $(this).find(".fileName").html().trim();
        var newName = $(this).find(".fileName").attr("data-newfilename");
        var description = $(this).find(".ImageDescription").val();
        var oneData = {
            originalName: originalName,
            newName: newName
        }
        documentList.push(oneData);
    });
    var JsondocumentListJoinString = JSON.stringify(documentList);

    var model = {
        Id: id,
        BenefitID: benifits,
        Currency:Currency,
        EmployeeId: employee,
        DateAwarded: dateAwarded,
        ExpiryDate: dateExpiry,
        FixedAmount: value,
        Comments: comments,
        RecoverOnTermination: recoverOnTermination,
        DocumentListString: JsondocumentListJoinString
    }

    $.ajax({
        url: constantEmployeeBenifits.SaveData,
        type: 'POST',
        data: JSON.stringify(model),
        contentType: "application/json",
        success: function (data) {
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
                    url: constantEmployeeBenifits.ImageUrl,
                    contentType: false,
                    processData: false,
                    data: imageData,
                    success: function (result) {
                        var string = '<div class="row ListData"><div class="col-lg-1 icon"><i class="fa fa-paperclip"></i></div><div class="col-lg-10 attach-disc"><label class="fileName" data-newFileName="' + result.NewFileName + '">' + result.originalFileName + '</label></div><div class="col-lg-1 file-deleteicon"><i class="fa fa-trash-o"></i></div></div>';
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


$('#tableDiv').on('click', '.dataTr', function () {
    if ($(this).hasClass('dataTr')) {
        $('#EmployeeBenefitTable tbody').find('tr.selected').removeClass('selected');
        $(this).addClass('selected');
        $("#tableDiv").find(".btn-edit-EmployeeBenefit").removeAttr('disabled');
        $("#tableDiv").find(".btn-delete-EmployeeBenefit").removeAttr('disabled');
    }
});

$("#tableDiv").on('click', '.btn-Refresh-EmployeeBenefit', function () {
    window.location.reload();
});

$("#tableDiv").on('click', '.btn-ClearSorting-EmployeeBenefit', function () {
    window.location.reload();
});

$("#tableDiv").on('click', '.btn-clearFilter-EmployeeBenefit', function () {
    window.location.reload();
});

$("#tableDiv").on('click', '.btn-edit-EmployeeBenefit', function () {
    var id = $("#tableDiv").find("#EmployeeBenefitTable tbody").find('.selected').attr("id");
    $.ajax({
        url: constantEmployeeBenifits.addEdit,
        data: { Id: id },
        success: function (data) {
            $("#tableDiv").find('#EmployeeBenefitBody').html('');
            $("#tableDiv").find('#EmployeeBenefitBody').html(data);

            $("#tableDiv").find('#EmployeeBenefitBody').find("#txt_DateAwarded").Zebra_DatePicker({
                //direction: false,
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {
                    $("#tableDiv").find('#EmployeeBenefitBody').find("#lbl-error-DateAwarded").hide();
                }
            });


            $("#tableDiv").find('#EmployeeBenefitBody').find("#txt_ExpiryDate").Zebra_DatePicker({
                //direction: false,
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {
                    $("#tableDiv").find('#EmployeeBenefitBody').find("#lbl-error-ExpiryDate").hide();
                }
            });

            $('[data-toggle="tooltip"]').tooltip();

            $("#tableDiv").find('#EmployeeBenefitBody').find('#wizard').smartWizard({
                onLeaveStep: leaveAStepCallback,
                onFinish: onFinishCallback
            });
            $("#tableDiv").find('#EmployeeBenefitBody').find('#wizard').find('.stepContainer').css('height', '345px');

            $("#tableDiv").find('#EmployeeBenefitBody').find('.buttonNext').addClass('btn btn-warning');
            $("#tableDiv").find('#EmployeeBenefitBody').find('.buttonPrevious').addClass('btn btn-warning');
            $("#tableDiv").find('#EmployeeBenefitBody').find('.buttonFinish').addClass('btn btn-success');

            $("#tableDiv").find('#EmployeeBenefitBody').find('.buttonPrevious').hide();
            $("#tableDiv").find('#EmployeeBenefitBody').find('.buttonFinish').hide();
        }
    });
});

$("#tableDiv").on('click', '.btn-delete-EmployeeBenefit', function () {
    var id = $("#tableDiv").find("#EmployeeBenefitTable tbody").find('.selected').attr("id");
    var employeeId = $("#tableDiv").parent().find('#currntEmployeeId').val();
    $.Zebra_Dialog("Are you sure you would like to delete this record?", {
        'type': false,
        'title': 'Delete Case Record',
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
                        url: constantEmployeeBenifits.DeleteData,
                        data: { Id: id, EmployeeId: employeeId },
                        success: function (data) {
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
            }]
    });
});