function DataTableDesign() {
    $('#ProjectListtable tfoot tr').appendTo('#ProjectListtable thead');
    var table = $('#ProjectListtable').DataTable({
        "sDom": '<"top"i>rt<"bottom"flp><"clear">',
        "scrollX": true
    });
    $('#tableDivProject').find('.dataTables_filter').hide();
    $('#tableDivProject').find('.dataTables_info').hide();
    $("#tableDivProject thead .SearchName").keyup(function () {
        table.column(0).search(this.value).draw();
    });
    $("#tableDivProject thead .SearchCountryName").keyup(function () {
        table.column(1).search(this.value).draw();
    });
    $("#tableDivProject thead .SearchLocationName").keyup(function () {
        table.column(2).search(this.value).draw();
    });
    $("#tableDivProject thead .SearchProjectOwner").keyup(function () {
        table.column(3).search(this.value).draw();
    });
}

$(document).ready(function () {
    DataTableDesign();
});

$("#tableDivProject").on('click', '.btn-add-Project', function () {
    //$(".hrtoolLoader").show();
    $.ajax({
        url: constantSet.addEdit,
        data: { Id: 0 },
        success: function (data) {
            $("#tableDivProject").find('#addProjectBody').html('');
            $("#tableDivProject").find('#addProjectBody').html(data);
            $("#AddProjectModel").find('.projectTitle').text("Add Project");
            $("#tableDivProject").find("#btn-submit-ProjectRecord").html("Add");
            $('[data-toggle="tooltip"]').tooltip();
            $("#tableDivProject").find("#addProjectBody").find("#drp-GeneralSkills").selectList();
            $("#tableDivProject").find("#addProjectBody").find("#drp-TechnicalSkills").selectList();
            $("#tableDivProject").find("#addProjectBody").find("#drp-Customers").selectList();
            $("#validationmessagetodate").hide();
            // $(".hrtoolLoader").hide();

            $("#FromDate").Zebra_DatePicker({
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {
                    $("#tableDivProject").find("#validationmessagefromdate").hide();
                    $("#tableDivProject").find("#lbl-error-GreaterEndDate").hide();
                    var startdate = $("#FromDate").val();
                    var enddate = $("#Todate").val();
                    calculateDateDiff(startdate, enddate);

                }
            });
            $("#Todate").Zebra_DatePicker({
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {
                    $("#tableDivProject").find("#validationmessagetodate").hide();
                    $("#tableDivProject").find("#lbl-error-GreaterEndDate").hide();
                    var startdate = $("#FromDate").val();
                    var enddate = $("#Todate").val();
                    calculateDateDiff(startdate, enddate);
                }
            });

        }
    });
});
function calculateDateDiff(stratDate, endDate) {
    if (stratDate != "" || endDate != "") {
        if (StartDateValidation(stratDate, endDate)) {
            $("#tableDivProject").find("#lbl-error-GreaterEndDate").show();
            $("#tableDivProject").find("#Todate").val('');
        }
        //else {
        //    var days = DaysCount(stratDate, endDate);
        //    $("#EmployeeBenefitBody").find("#txt_Duration").val(days);
        //}
    }
}

$("#tableDivProject").on('click', '.btn-edit-Project', function () {
    // $(".hrtoolLoader").show();
    var id = $("#tableDivProject").find("#ProjectListtable tbody").find('.selected').attr("id");
    $.ajax({
        url: constantSet.addEdit,
        data: { Id: id },
        success: function (data) {
            $("#tableDivProject").find('#addProjectBody').html('');
            $("#tableDivProject").find('#addProjectBody').html(data);
            $("#AddProjectModel").find('.projectTitle').text("Edit Project");
            $("#tableDivProject").find("#btn-submit-ProjectRecord").html("Save");
            $("#tableDivProject").find("#addProjectBody").find("#drp-GeneralSkills").selectList();
            $("#tableDivProject").find("#addProjectBody").find("#drp-TechnicalSkills").selectList();
            $("#tableDivProject").find("#addProjectBody").find("#drp-Customers").selectList();
            $('[data-toggle="tooltip"]').tooltip();

            $("#FromDate").Zebra_DatePicker({
                //direction: false,`                        
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {
                    $("#tableDivProject").find("#validationmessagefromdate").hide();
                    $("#tableDivProject").find("#lbl-error-GreaterEndDate").hide();
                    var startdate = $("#FromDate").val();
                    var enddate = $("#Todate").val();
                    calculateDateDiff(startdate, enddate);
                }
            });
            $("#Todate").Zebra_DatePicker({
                //direction: false,
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {
                    $("#tableDivProject").find("#validationmessagetodate").hide();
                    $("#tableDivProject").find("#lbl-error-GreaterEndDate").hide();
                    var startdate = $("#FromDate").val();
                    var enddate = $("#Todate").val();
                    calculateDateDiff(startdate, enddate);
                }
            });
            // $(".hrtoolLoader").hide();
            // $(".modal-backdrop").hide();
        }
    });
});

$("#tableDivProject").on('click', '#btn-submit-ProjectRecord', function () {
    var iserror = false;
    GeneralSkillId = [];
    $.each($("#tableDivProject").find("#drp-GeneralSkills").parent().find(".selectlist-item"), function () {
        GeneralSkillId.push($("#tableDivProject").find("#drp-GeneralSkills").parent().find(".selectlist-select").find('option:contains(' + $(this).html().trim() + ')')[0].value);
    });
    technicalSkillId = [];
    $.each($("#tableDivProject").find("#drp-TechnicalSkills").parent().find(".selectlist-item"), function () {
        technicalSkillId.push($("#tableDivProject").find("#drp-TechnicalSkills").parent().find(".selectlist-select").find('option:contains(' + $(this).html().trim() + ')')[0].value);
    });
    CustomereId = [];
    $.each($("#tableDivProject").find("#drp-Customers").parent().find(".selectlist-item"), function () {
        CustomereId.push($("#tableDivProject").find("#drp-Customers").parent().find(".selectlist-select").find('option:contains(' + $(this).html().trim() + ')')[0].value);
    });
    var ProjectName = $("#tableDivProject").find("#AssetName").val();
    var hiddenId = $("#tableDivProject").find("#hidden-Id").val();
    var model = {
        Id: $("#tableDivProject").find("#hidden-Id").val(),
        Name: $("#tableDivProject").find("#AssetName").val(),
        Country: $("#tableDivProject").find("#CountryId").val(),
        Location: $("#tableDivProject").find("#LocationId").val(),
        Block: $("#tableDivProject").find("#BlockId").val(),
        TaxZone: $("#tableDivProject").find("#TaxZoneId").val(),
        AssetType: $("#tableDivProject").find("#AssetTypeId").val(),
        FromDate: $("#tableDivProject").find("#FromDate").val(),
        ToDate: $("#tableDivProject").find("#Todate").val(),
        OperatorCompany: $("#tableDivProject").find("#CompanyOparater").val(),
        ProjectOwner: $("#tableDivProject").find("#ProjectOwner").val(),
        Description: $("#tableDivProject").find("#txt-SystemListDescription").val(),
        GeneralSkillsCSV: GeneralSkillId.join(','),
        TechnicalSkillsCSV: technicalSkillId.join(','),
        CustomersCSV: CustomereId.join(',')
    }
    if (ProjectName == "" || model.Country == 0 || model.Location == 0 || model.Block == 0 || model.FromDate == ""
        || model.ToDate == "" || model.GeneralSkillsCSV == "" || model.TechnicalSkillsCSV == "") {
        iserror = true;
        if (ProjectName == "") {
            $("#tableDivProject").find("#lbl-error-projectname").show();
            //$(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
        }
        if (model.Country == "0") {
            $("#validationmessageCountry").show();
            $("#validationmessageCountry").html("Select Country");
        }
        if (model.Location == 0) {
            $("#validationmessageLocation").show();
            $("#validationmessageLocation").html("Select Location");
        }
        if (model.Block == 0) {
            $("#validationmessageBlock").show();
            $("#validationmessageBlock").html("Select Block");
        }
        if (model.FromDate == "") {
            $("#validationmessagefromdate").show();
            $("#validationmessagefromdate").html("From Date is required");
        }
        if (model.ToDate == "") {
            $("#validationmessagetodate").show();
            $("#validationmessagetodate").html("ToDate is required");
        }
        if (model.GeneralSkillsCSV == "") {
            $("#validationmessaggeneralskill").show();
            $("#validationmessaggeneralskill").html("Select General Skills");
        }
        if (model.TechnicalSkillsCSV == "") {
            $("#validationmessatechnicalskill").show();
            $("#validationmessatechnicalskill").html("Select Technical Skills");
        }
        //if (iserror) {
        //    $(".hrtoolLoader").hide();
        //    $(".modal-backdrop").hide();
        //    return false;
        //}
    }
    else {
        $.ajax({
            type: "POST",
            data: JSON.stringify(model),
            url: constantSet.SaveProject,
            contentType: "application/json",
            success: function (result) {
                $("#tableDivProject").html('');
                $("#tableDivProject").html(result);

                DataTableDesign();
                // $(".hrtoolLoader").hide();
                $(".modal-backdrop").hide();
                if (hiddenId > 0) {
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

$('#tableDivProject').on('click', '.dataTr', function () {
    if ($(this).hasClass('dataTr')) {
        $('#ProjectListtable tbody').find('tr.selected').removeClass('selected');
        $(this).addClass('selected');
        $("#tableDivProject").find(".btn-edit-Project").removeAttr('disabled');
        $("#tableDivProject").find(".btn-delete-Project").removeAttr('disabled');
    }
});

$("#tableDivProject").on('click', '.btn-Refresh-Project', function () {
    window.location.reload();
});

$("#tableDivProject").on('click', '.btn-ClearSorting-Project', function () {
    window.location.reload();
});

$("#tableDivProject").on('click', '.btn-clearFilter-Project', function () {
    window.location.reload();
});

$("#tableDivProject").on('click', '.btn-delete-Project', function () {
    var id = $("#tableDivProject").find("#ProjectListtable tbody").find('.selected').attr("id");
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
                    // $(".hrtoolLoader").show();
                    $.ajax({
                        url: constantSet.DeleteProjectUrl,
                        data: { Id: id },
                        success: function (data) {
                            $("#tableDivProject").html('');
                            $("#tableDivProject").html(data);
                            DataTableDesign();
                            //  $(".hrtoolLoader").hide();
                            $(".modal-backdrop").hide();
                            $(".toast-error").show();
                            setTimeout(function () { $(".toast-error").hide(); }, 1500);
                        }
                    });
                }
            }]
    });
});