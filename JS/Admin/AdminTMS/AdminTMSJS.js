var table, imageData, FileUploadData;
var bol = false;
function checkForData(data) {
    var id = data.value;
    var Isvisible = false;
   
    if (id == 4019) {
        $("#FixedTermEndDate").val('');
        Isvisible = true;
    }
    else {
        Isvisible = false;
    }
    $("#FixedTermEndDate").Zebra_DatePicker({
        format: 'd-m-Y',
        show_icon: true,
        always_visible: Isvisible,
        onSelect: function () {
            $('#ValidFixedTermEndDate').hide();
            var startDate = $("#StartDate").val();
            var proEndDate = $("#FixedTermEndDate").val();
            calculateFixedTermEdDate(startDate, proEndDate);
        }
    });
}

function calculateFixedTermEdDate(stratDate, endDate) {
    if (stratDate != "" || endDate != "") {
        if (StartDateValidation(stratDate, endDate)) {
            $("#lbl-error-ValidFDEndDate").show();
            $("#FixedTermEndDate").val('');
        }
        else {
            $("#lbl-error-ValidFDEndDate").hide();
        }

    }
}
function calculateprobEndDate(stratDate, endDate) {    
    if (stratDate != "" || endDate != "") {
        if (StartDateValidation(stratDate, endDate)) {
            $("#lbl-error-ValidProbationEndDate").show();
            $("#ProbationEndDate").val('');
        }
        else {
            $("#lbl-error-ValidProbationEndDate").hide();
        }

    }
}
function calculateDateDiff(stratDate, endDate) {
    $("#ValidProbationEndDate").hide();
    if (stratDate != "" || endDate != "") {
        if (StartDateValidation(stratDate, endDate)) {
            $("#lbl-error-GreaterEndDate").show();
            $("#NextProbationReviewDate").val('');
        }

    }
}
function validDOB(dob) {   
    var now = new Date();
    var a = dob.split(" ");
    var d = a[0].split("-");
    var date = new Date();
    var age = now.getFullYear() - d[2];
    if (isNaN(age) || age < 18) {
        $("#page_content").find("#validAge").show();
        $("#page_content").find("#DateOfBirth").val('');
    }
    else {
        $("#page_content").find("#validAge").hide();
    }
}

$(document).ready(function () {
    $("#drp-RejectionReasonStepID").hide();
    DataTableDesign();
   DataTable2Design();  
});

function DataTableDesign() {
    $('#VacancyModalTable tfoot tr').appendTo('#VacancyModalTable thead');
    var table = $('#VacancyModalTable').DataTable({
        "sDom": '<"top"i>rt<"bottom"flp><"clear">',
        "scrollX":true
    });
    $('#contantBody').find('#tableDiv').find('.dataTables_filter').hide();
    $('#contantBody').find('#tableDiv').find('.dataTables_info').hide();

    $("#contantBody thead .SearchName").keyup(function () {
        table.column(0).search(this.value).draw();
    });
    $("#contantBody thead .SearchRecruitment").keyup(function () {
        table.column(1).search(this.value).draw();
    });
    $("#contantBody thead .SearchHiring").keyup(function () {
        table.column(2).search(this.value).draw();
    });
    $("#contantBody thead .SearchLocation").keyup(function () {
        table.column(3).search(this.value).draw();
    });
    $("#contantBody thead .SearchBusiness").keyup(function () {
        table.column(4).search(this.value).draw();
    });
    $("#contantBody thead .SearchDivision").keyup(function () {
        table.column(5).search(this.value).draw();
    });
    $("#contantBody thead .SearchPool").keyup(function () {
        table.column(6).search(this.value).draw();
    });
    $("#contantBody thead .SearchFunction").keyup(function () {
        table.column(7).search(this.value).draw();
    });
    $("#contantBody thead .SearchCreated").keyup(function () {
        table.column(8).search(this.value).draw();
    });
    $("#contantBody thead .SearchCreated").Zebra_DatePicker({
        //direction: false,
        showButtonPanel: false,
        format: 'd-m-Y',
        onSelect: function () {
            var date = $("#contantBody").find("thead").find('.SearchCreated').val();
            table.column(8).search(date).draw();
        }
    });

    $("body").on('click', '.dp_clear', function () {
        var date = $("#contantBody").find("thead").find('.SearchCreated').val();
        table.column(8).search(date).draw();
    });
    $("#contantBody thead .SearchStatus").keyup(function () {
        table.column(9).search(this.value).draw();
    });

}

function DataTable2Design() {
    $('#TalentModalTable tfoot tr').appendTo('#TalentModalTable thead');
    var table2 = $('#TalentModalTable').DataTable({
        "sDom": '<"top"i>rt<"bottom"flp><"clear">',
        "scrollX":true
    });
    $('#contantBody').find("#tableDiv").find('.dataTables_filter').hide();
    $('#contantBody').find("#tableDiv").find('.dataTables_info').hide();

    $("#contantBody thead .TSearchName").keyup(function () {
        table2.column(0).search(this.value).draw();
    });
    $("#contantBody thead .TSearchVacancy").keyup(function () {
        table2.column(1).search(this.value).draw();
    });
    $("#contantBody thead .TSearchRecruitment").keyup(function () {
        table2.column(2).search(this.value).draw();
    });
    $("#contantBody thead .TSearchStarRating").keyup(function () {
        table2.column(3).search(this.value).draw();
    });
    $("#contantBody thead .TSearchBusiness").keyup(function () {
        table2.column(4).search(this.value).draw();
    });
    $("#contantBody thead .TSearchDivision").keyup(function () {
        table2.column(5).search(this.value).draw();
    });
    $("#contantBody thead .TSearchPool").keyup(function () {
        table2.column(6).search(this.value).draw();
    });
    $("#contantBody thead .TSearchFunction").keyup(function () {
        table2.column(7).search(this.value).draw();
    });
    $("#contantBody thead .TSearchStatus").keyup(function () {
        table2.column(8).search(this.value).draw();
    });
    $("#contantBody thead .TSearchCreated").keyup(function () {
        table2.column(9).search(this.value).draw();
    });
    $("#contantBody thead .TSearchCreated").Zebra_DatePicker({
        //direction: false,
        showButtonPanel: false,
        format: 'd-M-Y',
        onSelect: function () {
            var date = $("#contantBody").find("thead").find('.TSearchCreated').val();
            table2.column(9).search(date).draw();
        }
    });


    $("body").on('click', '.dp_clear', function () {
        var date = $("#contantBody").find("thead").find('.TSearchCreated').val();
        table2.column(9).search(date).draw();
    });
}

//Talent (Pool) View 
$('#contantBody').on('click', '.datasTr', function () {
    
    if ($(this).hasClass('datasTr')) {
        $('#TalentModalTable tbody').find('tr.selected').removeClass('selected');
        $(this).addClass('selected');
        $("#tableDiv").find(".btn-edit-Talent").removeAttr('disabled');
        $("#tableDiv").find(".btn-delete-Talent").removeAttr('disabled');
    }
});
$("#contantBody").on('click', '.btn-delete-Talent', function () {
    var IdRecord = $("#TalentModalTable tbody").find('.selected').attr("id");
    $.Zebra_Dialog("Are you sure want to delete this records?", {
        'type': false,
        'title': 'Delete Record',
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
                        url: constantTMS.DeleteAppliRecord,
                        data: { Id: IdRecord },
                        success: function (data) {
                            $("#contantBody").html('');
                            $('#contantBody').html(data);
                            DataTable2Design();
                            $(".TMSIndex, .Vacancy, .Applicant").removeClass('active');
                            $(".Pool").addClass('active');
                            $(".hrtoolLoader").hide();
                            $(".modal-backdrop").hide();
                            $(".toast-error").show();
                            setTimeout(function () { $(".toast-error").hide(); }, 1500);
                        }
                    });
                }
            }]
    });
})
$('#contantBody').find("#TalentModalTable").on('click', '.btn-Refresh-Talent', function () {
    window.location.reload();
});

$('#contantBody').find("#TalentModalTable").on('click', '.btn-ClearSorting-Talent', function () {
    window.location.reload();
});

$('#contantBody').find("#TalentModalTable").on('click', '.btn-clearFilter-Talent', function () {
    window.location.reload();
});

//table Vacancy
$('#contantBody').on('click', '.dataTr', function () {
    if ($(this).hasClass('dataTr')) {
        $('#VacancyModalTable tbody').find('tr.selected').removeClass('selected');
        $(this).addClass('selected');
        $("#contantBody").find(".btn-edit-Vacancy").removeAttr('disabled');
        $("#contantBody").find(".btn-delete-Vacancy").removeAttr('disabled');
    }
});

$("#contantBody").on('click', '.btn-Refresh-Vacancy', function () {
    window.location.reload();
});

$("#contantBody").on('click', '.btn-ClearSorting-Vacancy', function () {
    window.location.reload();
});

$("#contantBody").on('click', '.btn-clearFilter-Vacancy', function () {
    window.location.reload();
});

//TAB Click TMS Index
//$("#TMSTabPanel").on('click', '.btn-click-TMSIndex', function () {
//    $(".hrtoolLoader").show();
//    $.ajax({
//        url: constantTMS.Index,
//        success: function (data) {
//            $("#contantBody").html('');
//            $('#contantBody').html(data);
//            $(".Vacancy, .Applicant, .Pool").removeClass('active');
//            $(".TMSIndex").addClass('active');
//            $(".hrtoolLoader").hide();
//            $(".modal-backdrop").hide();
//        }
//    });

//});

//TAB Click Vacancy
$("#TMSTabPanel").on('click', '.btn-click-Vacancy', function () {
    $(".hrtoolLoader").show();
    $.ajax({
        url: constantTMS.Vacancy,
        success: function (data) {
            $("#contantBody").html('');
            $('#contantBody').html(data);
            DataTableDesign();
            $(".TMSIndex, .Applicant, .Pool").removeClass('active');
            $(".Vacancy").addClass('active');
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
        }
    });
});

//TAB Click Applicant
$("#TMSTabPanel").on('click', '.btn-click-Applicant', function () {
    
    $(".hrtoolLoader").show();
    $.ajax({
        url: constantTMS.Applicant,
        success: function (data) {
            $("#contantBody").html('');
            $('#contantBody').html(data);


            $(".TMSIndex, .Vacancy, .Pool").removeClass('active');
            $(".Applicant").addClass('active');
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
        }
    });
});

//TAB Click Pool
$("#TMSTabPanel").on('click', '.btn-click-Pool', function () {
    $(".hrtoolLoader").show();
    $.ajax({
        url: constantTMS.Pool,
        success: function (data) {
            
            $("#contantBody").html('');
            $('#contantBody').html(data);
            DataTable2Design();
            $(".TMSIndex, .Vacancy, .Applicant").removeClass('active');
            $(".Pool").addClass('active');
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
        }
    });
});

//Add Vacancy 
$("#contantBody").on('click', '.btn-add-Vacancy', function () {
    $(".hrtoolLoader").show();
    $.ajax({
        url: constantTMS.AddVacancy,
        data: { Id: 0 },
        success: function (data) {
            
            $("#AddVacancyLogBody").html('');
            $('#AddVacancyLogBody').html(data);

            $('[data-toggle="tooltip"]').tooltip();
            $("#contantBody").find('#VacancyModal').find(".vacancyTitle").text("Add Vacancy");
            $("#contantBody").find('#AddVacancyLogBody').find('#wizard').smartWizard({
                onLeaveStep: onVacanyDetailsCallback,
                onFinish: onFinishVacancyCallback
            });
            $("#AddVacancyLogBody").find("#txt_ClosingDate").Zebra_DatePicker({
                //direction: false,
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {
                    //var date = $("#AddVacancyLogBody").find("txt_ClosingDate").val();
                }
            });

            $("#contantBody").find('#AddVacancyLogBody').find('.buttonNext').addClass('btn btn-warning');
            $("#contantBody").find('#AddVacancyLogBody').find('.buttonPrevious').addClass('btn btn-warning');
            $("#contantBody").find('#AddVacancyLogBody').find('.buttonFinish').addClass('btn btn-success');

            $("#contantBody").find('#AddVacancyLogBody').find('.buttonPrevious').hide();
            $("#contantBody").find('#AddVacancyLogBody').find('.buttonFinish').hide();

            $("#contantBody").find('#AddVacancyLogBody').find('div#froala-editor').froalaEditor({
                toolbarButtons: ['bold', 'italic', 'underline', 'strikeThrough', 'subscript', 'superscript', 'fontFamily', 'fontSize', '|', 'specialCharacters', 'color', 'inlineStyle', 'paragraphStyle', '|', 'paragraphFormat', 'align', 'formatOL', 'formatUL', 'outdent', 'indent', '-', 'quote', 'insertHR', 'insertLink', 'insertImage', 'insertVideo', 'insertFile', 'insertTable', '|', 'undo', 'redo', 'clearFormatting', 'selectAll', 'html', 'applyFormat', 'removeFormat', 'fullscreen', 'print', 'help'],
              //  toolbarButtons: ['bold', 'italic', 'underline', 'formatUL'],
                pluginsEnabled: null
            })
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
        }
    });
});

$("#contantBody").find('#AddVacancyLogBody').change('div#froala-editor', function (e, editor) {
    
    // Do something here.
});

function onVacanyDetailsCallback(obj,context) {
    //$("#tableDiv").find('#AdminCaseLogBody').find
    
    if (context.fromStep == 1) {        
        var isError = false;
        var title = $("#contantBody").find('#AddVacancyLogBody').find('#txt_Vacancy').val().trim();
        var summary = $("#contantBody").find('#AddVacancyLogBody').find('#txt_Summary').val().trim();
        var status = $("#contantBody").find('#AddVacancyLogBody').find('#drp-StatusId').val();
        var recruitment = $("#contantBody").find('#AddVacancyLogBody').find('#drp-RecruitmentProcessId').val();
        var closeDate = $("#contantBody").find('#AddVacancyLogBody').find('#txt_ClosingDate').val().trim();
        if (title == "") { isError = true; $("#contantBody").find('#AddVacancyLogBody').find("#lbl-error-Vacancy").show(); }
        if (summary == "") { isError = true; $("#contantBody").find('#AddVacancyLogBody').find("#lbl-error-Summary").show(); }
        if (status == "0") { isError = true; $("#contantBody").find('#AddVacancyLogBody').find("#lbl-error-StatusList").show(); }
        if (recruitment == "0") { isError = true; $("#contantBody").find('#AddVacancyLogBody').find("#lbl-error-RecruitmentProcess").show(); }
        if (closeDate == "") { isError = true; $("#contantBody").find('#AddVacancyLogBody').find("#lbl-error-ClosingDate").show(); }

        if (isError) {
            return false;
        }
        else {
            if (context.toStep = 2) {
                $("#contantBody").find('#AddVacancyLogBody').find('.buttonNext').show();
                $("#contantBody").find('#AddVacancyLogBody').find('.buttonPrevious').show();
                $("#contantBody").find('#AddVacancyLogBody').find('.buttonFinish').hide();
            }
            else {
                $("#contantBody").find('#AddVacancyLogBody').find('.buttonNext').hide();
                $("#contantBody").find('#AddVacancyLogBody').find('.buttonPrevious').show();
                $("#contantBody").find('#AddVacancyLogBody').find('.buttonFinish').show()
            }
            $("#contantBody").find('#AddVacancyLogBody').find("#lbl-error-Vacancy").hide();
            $("#contantBody").find('#AddVacancyLogBody').find("#lbl-Exits-Vacancy").hide();
            $("#contantBody").find('#AddVacancyLogBody').find("#lbl-error-Summary").hide();
            $("#contantBody").find('#AddVacancyLogBody').find("#lbl-error-StatusList").hide();
            $("#contantBody").find('#AddVacancyLogBody').find("#lbl-error-RecruitmentProcess").hide();
            $("#contantBody").find('#AddVacancyLogBody').find("#lbl-error-ClosingDate").hide();

            return true;
        }
    }
    if (context.fromStep == 2) {
        
        var isError = false;
        //var JobDescription = $("#contantBody").find('#AddVacancyLogBody').find('#froala-editor').val();
        var JobDescription = $("#contantBody").find('#AddVacancyLogBody').find('#froala-editor').html();        
        var HiringLead = $("#contantBody").find('#AddVacancyLogBody').find('#drp-HiringLeadId').val();
        var drpBusiness = $("#drp-BusinessId").val();        
        if (JobDescription == "") { isError = true; $("#contantBody").find('#AddVacancyLogBody').find("#lbl-error-JobDescription").show(); }        
        if (HiringLead == "" || HiringLead=="0") { isError = true; $("#contantBody").find('#AddVacancyLogBody').find("#lbl-error-HiringLeadList").show(); }
        if (isError) {
            return false;
        }
        else {
            if (context.toStep == 1) {
                $("#contantBody").find('#AddVacancyLogBody').find('.buttonNext').show();
                $("#contantBody").find('#AddVacancyLogBody').find('.buttonPrevious').hide();
                $("#contantBody").find('#AddVacancyLogBody').find('.buttonFinish').hide();
            }
            else {
                $("#contantBody").find('#AddVacancyLogBody').find('.buttonNext').hide();
                $("#contantBody").find('#AddVacancyLogBody').find('.buttonPrevious').show();
                $("#contantBody").find('#AddVacancyLogBody').find('.buttonFinish').show();
            }
            $("#contantBody").find('#AddVacancyLogBody').find("#lbl-error-JobDescription").hide();
            $("#contantBody").find('#AddVacancyLogBody').find("#lbl-error-HiringLeadList").hide();
            return true;
        }

    }
    else {

        if (context.toStep == 2) {
            $("#contantBody").find('#AddVacancyLogBody').find('.buttonNext').show();
            $("#contantBody").find('#AddVacancyLogBody').find('.buttonPrevious').show();
            $("#contantBody").find('#AddVacancyLogBody').find('.buttonFinish').hide();
        }
        else {
            $("#contantBody").find('#AddVacancyLogBody').find('.buttonNext').show();
            $("#contantBody").find('#AddVacancyLogBody').find('.buttonPrevious').hide();
            $("#contantBody").find('#AddVacancyLogBody').find('.buttonFinish').hide();
        }

        return true;
    }

}


function onFinishVacancyCallback(obj,context) {
    $(".hrtoolLoader").show();
    var id = $("#contantBody").find('#AddVacancyLogBody').find('#VacancyHiddenId').val();
    var title = $("#contantBody").find('#AddVacancyLogBody').find('#txt_Vacancy').val().trim();
    var summary = $("#contantBody").find('#AddVacancyLogBody').find('#txt_Summary').val().trim();
    var status = $("#contantBody").find('#AddVacancyLogBody').find('#drp-StatusId').val();
    var recruitment = $("#contantBody").find('#AddVacancyLogBody').find('#drp-RecruitmentProcessId').val();
    var closeDate = $("#contantBody").find('#AddVacancyLogBody').find('#txt_ClosingDate').val().trim();
    var SalaryRange = $("#contantBody").find('#AddVacancyLogBody').find('#txt_SalaryRange').val().trim();
    var Location = $("#contantBody").find('#AddVacancyLogBody').find('#txt_Location').val().trim();
    var Business = $("#contantBody").find('#AddVacancyLogBody').find('#drp-BusinessId').val();
    var Division = $("#contantBody").find('#AddVacancyLogBody').find('#drp-DivisionId').val();
    var Pool = $("#contantBody").find('#AddVacancyLogBody').find('#drp-PoolId').val();
    var Function = $("#contantBody").find('#AddVacancyLogBody').find('#drp-FunctionId').val();
    var JobDescription = $("#contantBody").find('#AddVacancyLogBody').find('#froala-editor').froalaEditor('html.get');

    var HiringLead = $("#contantBody").find('#AddVacancyLogBody').find('#drp-HiringLeadId').val();
    var MustUploadCoverLetter = $('#check_MustUploadCoverLetter').is(":checked");
    var MustUploadResumeCV = $('#check_MustUploadResumeCV').is(":checked");
    var ApplicationFormPathOriginal = $("#contantBody").find('#AddVacancyLogBody').find('#FilePathName')["0"].innerText;
    var ApplicationFormPath = $("#contantBody").find('#AddVacancyLogBody').find('#txt_FilepathName').val();
    var Question1On = $('#check_Question1On').is(":checked");
    var Question1Text = $("#contantBody").find('#AddVacancyLogBody').find('#txt_Question1Text').val().trim();
    var Question2On = $('#check_Question2On').is(":checked");
    var Question2Text = $("#contantBody").find('#AddVacancyLogBody').find('#txt_Question2Text').val().trim();
    var Question3On = $('#check_Question3On').is(":checked");
    var Question3Text = $("#contantBody").find('#AddVacancyLogBody').find('#txt_Question3Text').val().trim();
    var Question4On = $('#check_Question4On').is(":checked");
    var Question4Text = $("#contantBody").find('#AddVacancyLogBody').find('#txt_Question4Text').val().trim();
    var Question5On = $('#check_Question5On').is(":checked");
    var Question5Text = $("#contantBody").find('#AddVacancyLogBody').find('#txt_Question5Text').val().trim(); //drp-SourceId
    var Source = $("#contantBody").find('#AddVacancyLogBody').find('#drp-SourceId').val();
    var model = {
        Id: id,
        Title: title,
        Summary: summary,
        StatusID: status,
        ClosingDate: closeDate,
        RecruitmentProcessID: recruitment,
        Salary: SalaryRange,
        Location: Location,
        BusinessID: Business,
        DivisionID: Division,
        PoolID: Pool,
        FunctionID: Function,
        JobDescription: JobDescription,
        HiringLeadID: HiringLead,
        MustUploadCoverLetter: MustUploadCoverLetter,
        MustUploadResumeCV: MustUploadResumeCV,
        ApplicationFormPathOriginal: ApplicationFormPathOriginal,
        ApplicationFormPath: ApplicationFormPath,
        Question1On: Question1On,
        Question1Text: Question1Text,
        Question2On: Question2On,
        Question2Text: Question2Text,
        Question3On: Question3On,
        Question3Text: Question3Text,
        Question4On: Question4On,
        Question4Text: Question4Text,
        Question5On: Question5On,
        Question5Text: Question5Text,
        SourceID: Source
    }

    $.ajax({
        url: constantTMS.SaveVacancy,
        type: 'POST',
        data: JSON.stringify(model),
        contentType: "application/json",
        success: function (data) {
            
            if (data == "Error") {
                isError = true;
                $("#contantBody").find('#AddVacancyLogBody').find("#lbl-Exist-Vacancy").show();
                $(".hrtoolLoader").hide();
                $(".modal-backdrop").hide();
            }
            $("#contantBody").html("");
            $("#contantBody").html(data);
            DataTableDesign();
            $(".TMSIndex, .Applicant, .Pool").removeClass('active');
            $(".Vacancy").addClass('active');
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

$("#contantBody").on("change", "#drp-BusinessId", function () {
    
    var value = $(this).val();
    if (value != "0") {
        $.ajax({
            url: constantTMS.bindDiv,
            data: { businessId: value },
            success: function (data) {
                $("#drp-DivisionId").html('');
                var toAppend = '';
                toAppend += "<option value='0'>--Select Divion--</option>";
                $.each(data, function (index, item) {
                    toAppend += "<option value='" + item.Id + "'>" + item.Name + "</option>";
                })
                $("#drp-DivisionId").html(toAppend);
                if ($("#drp-DivisionId").val() == 0) {
                    $("#drp-DivisionId").val(0);
                    $('#drp-PoolId').val(0);
                    $('#drp-FunctionId').val(0);
                }
            }
        });
    }
    else {
        $('#drp-DivisionId').empty();
        // Bind new values to dropdown
        $('#drp-DivisionId').each(function () {
            // Create option
            var option = $("<option />");
            option.attr("value", '0').text('--Select Division--');
            $('#drp-DivisionId').append(option);
        });
        $('#drp-PoolId').empty();
        // Bind new values to dropdown
        $('#drp-PoolId').each(function () {
            // Create option
            
            var option = $("<option />");
            option.attr("value", '0').text('--Select Pool--');
            $('#drp-PoolId').append(option);
        });
        $('#drp-FunctionId').empty();
        // Bind new values to dropdown
        $('#drp-FunctionId').each(function () {
            
            // Create option
            var option = $("<option />");
            option.attr("value", '0').text('--Select Function--');
            $('#drp-FunctionId').append(option);
        });


    }
});

$("#contantBody").on("change", "#drp-DivisionId", function () {
    
    var value = $(this).val();
    if (value != "0") {
        $.ajax({
            url: constantTMS.bindpool,
            data: { DivisionId: value },
            success: function (data) {
                $("#drp-PoolId").html('');
                var toAppend = '';
                toAppend += "<option value='0'>--Select Pool--</option>";
                $.each(data, function (index, item) {
                    toAppend += "<option value='" + item.Id + "'>" + item.Name + "</option>";
                })
                $("#drp-PoolId").html(toAppend);
                if ($("#drp-PoolId").val() == 0) {
                    $("#drp-PoolId").val(0);
                }
                $.ajax({
                    url: constantTMS.bindFuncation,
                    data: { DivisionId: value },
                    success: function (data) {
                        $("#drp-FunctionId").html('');
                        var toAppend = '';
                        toAppend += "<option value='0'>--Select Function--</option>";
                        $.each(data, function (index, item) {
                            toAppend += "<option value='" + item.Id + "'>" + item.Name + "</option>";
                        })
                        $("#drp-FunctionId").html(toAppend);
                        if ($("#drp-FunctionId").val() == 0) {
                            $("#drp-FunctionId").val(0);
                        }
                    }
                });
            }
        });
    }
    else {

        $('#drp-PoolId').empty();
        // Bind new values to dropdown
        $('#drp-PoolId').each(function () {
            // Create option
            var option = $("<option />");
            option.attr("value", '0').text('--Select Pool--');
            $('#drp-PoolId').append(option);
        });
        $('#drp-FunctionId').empty();
        // Bind new values to dropdown
        $('#drp-FunctionId').each(function () {
            // Create option
            var option = $("<option />");
            option.attr("value", '0').text('--Select Function--');
            $('#drp-FunctionId').append(option);
        });

    }
});

//Edit Vacancy
$("#contantBody").on('click', '.btn-edit-Vacancy', function () {
    $(".hrtoolLoader").show();
    var id = $("#contantBody").find("#VacancyModalTable tbody").find('.selected').attr("id");
    $.ajax({
        url: constantTMS.AddVacancy,
        data: { Id: id },
        success: function (data) {
            $("#AddVacancyLogBody").html('');
            $('#AddVacancyLogBody').html(data);

            $('[data-toggle="tooltip"]').tooltip();
            $("#contantBody").find('#VacancyModal').find(".vacancyTitle").text("Edit Vacancy");
            $("#contantBody").find('#AddVacancyLogBody').find('#wizard').smartWizard({
                        
                onLeaveStep: onVacanyDetailsCallback,
                onFinish: onFinishVacancyCallback
            });
            $("#AddVacancyLogBody").find("#txt_ClosingDate").Zebra_DatePicker({
                //direction: false,
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {
                    //var date = $("#AddVacancyLogBody").find("txt_ClosingDate").val();
                }
            });

            $("#contantBody").find('#AddVacancyLogBody').find('.buttonNext').addClass('btn btn-warning');
            $("#contantBody").find('#AddVacancyLogBody').find('.buttonPrevious').addClass('btn btn-warning');
            $("#contantBody").find('#AddVacancyLogBody').find('.buttonFinish').addClass('btn btn-success');

            $("#contantBody").find('#AddVacancyLogBody').find('.buttonPrevious').hide();
            $("#contantBody").find('#AddVacancyLogBody').find('.buttonFinish').hide();

            $("#contantBody").find('#AddVacancyLogBody').find('div#froala-editor').froalaEditor({
                toolbarButtons: ['bold', 'italic', 'underline', 'strikeThrough', 'subscript', 'superscript', 'fontFamily', 'fontSize', '|', 'specialCharacters', 'color','inlineStyle', 'paragraphStyle', '|', 'paragraphFormat', 'align', 'formatOL', 'formatUL', 'outdent', 'indent', '-', 'quote', 'insertHR', 'insertLink', 'insertImage', 'insertVideo', 'insertFile', 'insertTable', '|', 'undo', 'redo', 'clearFormatting', 'selectAll', 'html', 'applyFormat', 'removeFormat', 'fullscreen', 'print', 'help'],
                //toolbarButtons: ['bold', 'italic', 'underline', 'formatUL'],
                pluginsEnabled: null
            });
            
            var job = $("#contantBody").find('#AddVacancyLogBody').find('#txt_JobDescription').val();
            $("#contantBody").find('#AddVacancyLogBody').find('#froala-editor').froalaEditor('html.set', job);
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
        }
    });
});

//Delete Vacancy
$("#contantBody").on('click', '.btn-delete-Vacancy', function () {
    var id = $("#contantBody").find("#VacancyModalTable tbody").find('.selected').attr("id");
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
                        url: constantTMS.DeleteVacancy,
                        data: { Id: id },
                        success: function (data) {
                            
                            $("#contantBody").html("");
                            $("#contantBody").html(data);
                            $(".TMSIndex, .Applicant, .Pool").removeClass('active');
                            $(".Vacancy").addClass('active');
                            $(".toast-error").show();
                            setTimeout(function () { $(".toast-error").hide(); }, 1500);
                            DataTableDesign();
                            $(".hrtoolLoader").hide();
                            $(".modal-backdrop").hide();
                        }
                    });
                }
            }]
    });
});

//Checkbox Event
$("#contantBody").on('click', '#check_Question1On', function () {
    if ($(this).is(':checked') == true) {
        $("#txt_Question1Text").attr('readonly', false)
    }
    else {
        $("#txt_Question1Text").attr('readonly', true)
    }
});
$("#contantBody").on('click', '#check_Question2On', function () {
    if ($(this).is(':checked') == true) {
        $("#txt_Question2Text").attr('readonly', false)
    }
    else {
        $("#txt_Question2Text").attr('readonly', true)
    }
});
$("#contantBody").on('click', '#check_Question3On', function () {
    if ($(this).is(':checked') == true) {
        $("#txt_Question3Text").attr('readonly', false)
    }
    else {
        $("#txt_Question3Text").attr('readonly', true)
    }
});
$("#contantBody").on('click', '#check_Question4On', function () {
    if ($(this).is(':checked') == true) {
        $("#txt_Question4Text").attr('readonly', false)
    }
    else {
        $("#txt_Question4Text").attr('readonly', true)
    }
});
$("#contantBody").on('click', '#check_Question5On', function () {
    if ($(this).is(':checked') == true) {
        $("#txt_Question5Text").attr('readonly', false)
    }
    else {
        $("#txt_Question5Text").attr('readonly', true)
    }
});

//FileUploder
$("#contantBody").on('change', '#fileToUpload', function (e) {
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
                    url: constantTMS.ImageData,
                    contentType: false,
                    processData: false,
                    data: imageData,
                    success: function (result) {
                        
                        $('#AddVacancyLogBody').find('#FilePathName').html("");
                        $('#AddVacancyLogBody').find('#FilePathName').html(result.originalFileName);
                        $('#AddVacancyLogBody').find('#txt_FilepathName').val(result.NewFileName);


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

//Vacancy user Link
function gotoApplicantOverview(Id) {
    $(".hrtoolLoader").hide();
    $.ajax({
        url: constantTMS.SelectVacancy,
        data: { Id: Id },
        success: function (data) {            
            $("#contantBody").html('');
            $('#contantBody').html(data);
            $("#TMSTabPanel").find(".TMSIndex, .Applicant, .Pool").removeClass('active');
            $("#TMSTabPanel").find(".Vacancy").addClass('active');
            var updateOutput = function (e) {
                var list = e.length ? e : $(e.target),
                        output = list.data('output');
                if (window.JSON) {
                    output.val(window.JSON.stringify(list.nestable('serialize')));//, null, 2));
                }
                else {
                    output.val('JSON browser support required for this demo.');
                }
            };
            $.each($('.ApplicantNestable'), function () {
                $(this).nestable({
                    maxDepth: 1,
                    group: 1,
                    callback: function (l, e) {
                        
                        var stepId = $(l).attr("data-stepId");
                        var stepName = $(l).attr("data-stepName");
                        var applicantId = $(e).attr("data-applicantId");
                        var VacancyId = $("#contantBody").find("#drp-SelectVacancyId")["0"].value;
                        var model = {
                            StepID: stepId,
                            StepName: stepName,
                            ApplicantID: applicantId,
                            VacancyID: VacancyId
                        }
                        $.ajax({
                            url: constantTMS.StepMove,
                            type: 'POST',
                            data: JSON.stringify(model),
                            contentType: "application/json",
                            success: function (data) {
                                
                                if (data.isRedirect == undefined) {
                                    BindData(data);
                                }
                                else {
                                    
                                    $('#AddResoure').modal('show');
                                    $.ajax({
                                        url: constantTMS.EditResource,
                                        data: { Id: data.redirectUrl.RouteValues[2].Value, VacancyID: data.redirectUrl.RouteValues[3].Value },
                                        success: function (data) {
                                          
                                            $('#AddResoureceBody').html('');
                                            $('#AddResoureceBody').html(data);
                                            $("#btn-submit-Resoure").html("Add");
                                            $('[data-toggle="tooltip"]').tooltip();
                                            $('#wizard').smartWizard({
                                                onLeaveStep: leaveAStepCallback,
                                                onFinish: onFinishCallback
                                            });
                                            $(".TMSIndex, .Vacancy, .Pool").removeClass('active');
                                            $(".Applicant").addClass('active');
                                            $('#AddResoureceBody').find('.buttonNext').addClass('btn btn-warning');
                                            $('#AddResoureceBody').find('.buttonPrevious').addClass('btn btn-warning');
                                            $('#AddResoureceBody').find('.buttonFinish').addClass('btn btn-success');
                                            $('#AddResoureceBody').find('.buttonPrevious').hide();
                                            $('#AddResoureceBody').find('.buttonFinish').hide();

                                            $("#page_content").find("#DateOfBirth").Zebra_DatePicker({
                                                showButtonPanel: false,
                                                format: 'd-m-Y',
                                                onSelect: function () {
                                                    $("#page_content").find('#ValidDateOfBirth').hide();
                                                    validDOB(dob);
                                                }
                                            });
                                            $("#page_content").find("#StartDate").Zebra_DatePicker({
                                                showButtonPanel: false,
                                                format: 'd-m-Y',
                                                onSelect: function () {
                                                    $("#page_content").find('#ValidStartDate').hide();
                                                    var startDate = $("#StartDate").val();
                                                    var proEndDate = $("#ProbationEndDate").val();
                                                    calculateprobEndDate(startDate, proEndDate);
                                                    var proEndFixDate = $("#FixedTermEndDate").val();
                                                    calculateFixedTermEdDate(startDate, proEndFixDate);
                                                }
                                            });
                                            $("#page_content").find("#ProbationEndDate").Zebra_DatePicker({
                                                showButtonPanel: false,
                                                format: 'd-m-Y',
                                                onSelect: function () {
                                                    $("#page_content").find('#ValidProbationEndDate').hide();
                                                }
                                            });
                                            $("#page_content").find("#NextProbationReviewDate").Zebra_DatePicker({
                                                showButtonPanel: false,
                                                format: 'd-m-Y',
                                                onSelect: function () {
                                                    $("#page_content").find('#ValidNextProbationReviewDate').hide();                                                    
                                                    $("#page_content").find("#lbl-error-GreaterEndDate").hide();
                                                    var startdate = $("#page_content").find("#ProbationEndDate").val();
                                                    var enddate = $("#page_content").find("#NextProbationReviewDate").val();
                                                    calculateDateDiff(startdate, enddate);
                                                }
                                            });

                                            $("#page_content").find("#FixedTermEndDate").Zebra_DatePicker({
                                                showButtonPanel: false,
                                                format: 'd-m-Y',
                                                onSelect: function () {
                                                    $("#page_content").find('#ValidFixedTermEndDate').hide();
                                                    $("#page_content").find('#ValidFixedTermEndDate').hide();
                                                    $("#page_content").find('#ValidFixedTermEndDate').hide();
                                                    $("#page_content").find("#lbl-error-ValidFDEndDate").hide();
                                                    var startDate = $("#StartDate").val();
                                                    var proEndDate = $("#FixedTermEndDate").val();
                                                    calculateFixedTermEdDate(startDate, proEndDate);
                                                }
                                            });
                                        }
                                    });


                                    $(".hrtoolLoader").hide();
                                    $(".modal-backdrop").hide();
                                }

                            }
                        });
                    }
                });
            });
            function changeTable() {
            }
            updateOutput($('.ApplicantNestable').data('output', $('.ApplicantNestable-output')));
            //$("#contantBody").find(".vacancyProcess").html(data.RecruitmentProcess);
            //$("#contantBody").find(".vacancyStatus").text(data.Status);
            //$("#contantBody").find(".vacancyCloseDate").text(data.ClosingDate);
            //$("#contantBody").find(".vacancyDescription").text(data.Summary);
            $("#contantBody").find("#VacancyDetailsShow").removeClass("hide");
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();

        }
    });
}
function AcceptedAddresource()
{
    var stepId = $('#ApplicantModel').find('#drp-SelectStepID').val();;
    //var stepName = $(l).attr("data-stepName");    
    var applicantId = $("#ApplicantHiddenId").val();
    var VacancyId = $("#contantBody").find("#drp-SelectVacancyId")["0"].value;
    var model = {
        StepID: stepId,
        //StepName: stepName,
        ApplicantID: applicantId,
        VacancyID: VacancyId
    }
    $.ajax({
        url: constantTMS.StepMove,
        type: 'POST',
        data: JSON.stringify(model),
        contentType: "application/json",
        success: function (data) {
            if (data.isRedirect == undefined) {
                BindData(data);
            }
            else {
                $("#ApplicantModel").modal('hide');                    
                $.ajax({
                    url: constantTMS.EditResource,
                    data: { Id: data.redirectUrl.RouteValues[2].Value, VacancyID: data.redirectUrl.RouteValues[3].Value },
                    success: function (data) {
                        $('#AddResoureceBody').html('');
                        $('#AddResoureceBody').html(data);
                        $('#AddResoure').modal('show');
                        $("#btn-submit-Resoure").html("Add");
                        $('[data-toggle="tooltip"]').tooltip();
                        $('#wizard').smartWizard({
                            onLeaveStep: leaveAStepCallback,
                            onFinish: onFinishCallback
                        });
                        $('#AddResoureceBody').find('.buttonNext').addClass('btn btn-warning');
                        $('#AddResoureceBody').find('.buttonPrevious').addClass('btn btn-warning');
                        $('#AddResoureceBody').find('.buttonFinish').addClass('btn btn-success');
                        $('#AddResoureceBody').find('.buttonPrevious').hide();
                        $('#AddResoureceBody').find('.buttonFinish').hide();
                        $("#page_content").find("#DateOfBirth").Zebra_DatePicker({
                            showButtonPanel: false,
                            format: 'd-m-Y',
                            onSelect: function () {
                                $("#page_content").find('#ValidDateOfBirth').hide();
                                validDOB(dob);
                            }
                        });
                        $("#page_content").find("#StartDate").Zebra_DatePicker({
                            showButtonPanel: false,
                            format: 'd-m-Y',
                            onSelect: function () {
                                $("#page_content").find('#ValidStartDate').hide();                                
                                var startDate = $("#StartDate").val();
                                var proEndDate = $("#ProbationEndDate").val();
                                calculateprobEndDate(startDate, proEndDate);
                                var proEndFixDate = $("#FixedTermEndDate").val();
                                calculateFixedTermEdDate(startDate, proEndFixDate);
                            }
                        });
                        $("#page_content").find("#ProbationEndDate").Zebra_DatePicker({
                            showButtonPanel: false,
                            format: 'd-m-Y',
                            onSelect: function () {                                
                                $("#ValidProbationEndDate").hide();
                                $("#page_content").find("#lbl-error-GreaterEndDate").hide();
                                var stDate = $("#StartDate").val();
                                var startdate = $("#page_content").find("#ProbationEndDate").val();
                                var enddate = $("#page_content").find("#NextProbationReviewDate").val();
                                calculateDateDiff(startdate, enddate);
                                calculateprobEndDate(stDate, startdate);
                            }
                        });
                        $("#page_content").find("#NextProbationReviewDate").Zebra_DatePicker({
                            showButtonPanel: false,
                            format: 'd-m-Y',
                            onSelect: function () {
                                $("#page_content").find('#ValidNextProbationReviewDate').hide();
                                $("#ValidProbationEndDate").hide();
                                $("#lbl-error-GreaterEndDate").hide();
                                var stDate = $("#StartDate").val();
                                var startdate = $("#page_content").find("#ProbationEndDate").val();
                                var enddate = $("#page_content").find("#NextProbationReviewDate").val();
                                calculateDateDiff(startdate, enddate);
                                calculateprobEndDate(stDate, startdate);
                            }
                        });

                        $("#page_content").find("#FixedTermEndDate").Zebra_DatePicker({
                            showButtonPanel: false,
                            format: 'd-m-Y',
                            onSelect: function () {
                                $("#page_content").find('#ValidFixedTermEndDate').hide();
                                $("#page_content").find("#lbl-error-ValidFDEndDate").hide();
                                var startDate = $("#StartDate").val();
                                var proEndDate = $("#FixedTermEndDate").val();
                                calculateFixedTermEdDate(startDate, proEndDate);
                            }
                        });
                    }
                });

                $(".hrtoolLoader").hide();
                $(".modal-backdrop").hide();
            }

        }
    });
}
function redirectToApplicantPage()
{
    var value = $("#drp-SelectVacancyId").val();
    if (value != "0") {
        $.ajax({
            url: constantTMS.SelectVacancy,
            data: { Id: value },
            success: function (data) {
                $("#contantBody").html('');
                $('#contantBody').html(data);
                var updateOutput = function (e) {
                    var list = e.length ? e : $(e.target),
                            output = list.data('output');
                    if (window.JSON) {
                        output.val(window.JSON.stringify(list.nestable('serialize')));//, null, 2));
                    }
                    else {
                        output.val('JSON browser support required for this demo.');
                    }
                };
                $.each($('.ApplicantNestable'), function () {
                    $(this).nestable({
                        maxDepth: 1,
                        group: 1,
                        callback: function (l, e) {                            
                            var stepId = $(l).attr("data-stepId");
                            var stepName = $(l).attr("data-stepName");
                            var applicantId = $(e).attr("data-applicantId");
                            var VacancyId = $("#contantBody").find("#drp-SelectVacancyId")["0"].value;
                            var model = {
                                StepID: stepId,
                                StepName: stepName,
                                ApplicantID: applicantId,
                                VacancyID: VacancyId
                            }
                            $.ajax({
                                url: constantTMS.StepMove,
                                type: 'POST',
                                data: JSON.stringify(model),
                                contentType: "application/json",
                                success: function (data) {
                                    if (data.isRedirect == undefined) {
                                        BindData(data);
                                    }
                                    else {
                                        $('#AddResoure').modal('show');
                                        $.ajax({
                                            url: constantTMS.EditResource,
                                            data: { Id: data.redirectUrl.RouteValues[2].Value, VacancyID: data.redirectUrl.RouteValues[3].Value },
                                            success: function (data) {
                                               $('#AddResoureceBody').html('');
                                                $('#AddResoureceBody').html(data);
                                                $("#btn-submit-Resoure").html("Add");
                                                $('[data-toggle="tooltip"]').tooltip();
                                                $('#wizard').smartWizard({
                                                    onLeaveStep: leaveAStepCallback,
                                                    onFinish: onFinishCallback
                                                });
                                                $('#AddResoureceBody').find('.buttonNext').addClass('btn btn-warning');
                                                $('#AddResoureceBody').find('.buttonPrevious').addClass('btn btn-warning');
                                                $('#AddResoureceBody').find('.buttonFinish').addClass('btn btn-success');
                                                $('#AddResoureceBody').find('.buttonPrevious').hide();
                                                $('#AddResoureceBody').find('.buttonFinish').hide();

                                                $("#page_content").find("#DateOfBirth").Zebra_DatePicker({
                                                    showButtonPanel: false,
                                                    format: 'd-m-Y',
                                                    onSelect: function () {
                                                        $("#page_content").find('#ValidDateOfBirth').hide();
                                                        validDOB(dob);
                                                    }
                                                });
                                                $("#page_content").find("#StartDate").Zebra_DatePicker({
                                                    showButtonPanel: false,
                                                    format: 'd-m-Y',
                                                    onSelect: function () {
                                                        $("#page_content").find('#ValidStartDate').hide();
                                                        var startDate = $("#StartDate").val();
                                                        var proEndDate = $("#ProbationEndDate").val();
                                                        calculateprobEndDate(startDate, proEndDate);
                                                        var proEndFixDate = $("#FixedTermEndDate").val();
                                                        calculateFixedTermEdDate(startDate, proEndFixDate);
                                                    }
                                                });
                                                $("#page_content").find("#ProbationEndDate").Zebra_DatePicker({
                                                    showButtonPanel: false,
                                                    format: 'd-m-Y',
                                                    onSelect: function () {
                                                        $("#page_content").find('#ValidProbationEndDate').hide();
                                                    }
                                                });
                                                $("#page_content").find("#NextProbationReviewDate").Zebra_DatePicker({
                                                    showButtonPanel: false,
                                                    format: 'd-m-Y',
                                                    onSelect: function () {
                                                        $("#page_content").find('#ValidNextProbationReviewDate').hide();
                                                        $("#ValidProbationEndDate").hide();
                                                        $("#lbl-error-GreaterEndDate").hide();
                                                        var stDate = $("#StartDate").val();
                                                        var startdate = $("#page_content").find("#ProbationEndDate").val();
                                                        var enddate = $("#page_content").find("#NextProbationReviewDate").val();
                                                        calculateDateDiff(startdate, enddate);
                                                        calculateprobEndDate(stDate, startdate);
                                                        
                                                    }
                                                });

                                                $("#page_content").find("#FixedTermEndDate").Zebra_DatePicker({
                                                    showButtonPanel: false,
                                                    format: 'd-m-Y',
                                                    onSelect: function () {
                                                        $("#page_content").find('#ValidFixedTermEndDate').hide();
                                                        $("#page_content").find("#lbl-error-ValidFDEndDate").hide();
                                                        var startDate = $("#StartDate").val();
                                                        var proEndDate = $("#FixedTermEndDate").val();
                                                        calculateFixedTermEdDate(startDate, proEndDate);
                                                    }
                                                });
                                            }
                                        });
                                        $(".hrtoolLoader").hide();
                                        $(".modal-backdrop").hide();
                                    }

                                }
                            });
                        }
                    });
                });
                function changeTable() {
                }
                updateOutput($('.ApplicantNestable').data('output', $('.ApplicantNestable-output')));
                //$("#contantBody").find(".vacancyProcess").html(data.RecruitmentProcess);
                //$("#contantBody").find(".vacancyStatus").text(data.Status);
                //$("#contantBody").find(".vacancyCloseDate").text(data.ClosingDate);
                //$("#contantBody").find(".vacancyDescription").text(data.Summary);
                $("#contantBody").find("#VacancyDetailsShow").removeClass("hide");
                $(".hrtoolLoader").hide();
                $(".modal-backdrop").hide();

            }
        });
    }
    else {
        $.ajax({
            url: constantTMS.Applicant,
            success: function (data) {
                $("#contantBody").html('');
                $('#contantBody').html(data);
                $(".TMSIndex, .Vacancy, .Pool").removeClass('active');
                $(".Applicant").addClass('active');
                //$(this).find('option').removeAttr("selected");
                $(".hrtoolLoader").hide();
                $(".modal-backdrop").hide();
            }
        });
    }
}

//Applicant Select Vacancy
$("#contantBody").on("change", "#drp-SelectVacancyId", function () {
    var value = $(this).val();
    if (value != "0") {
        $.ajax({
            url: constantTMS.SelectVacancy,
            data: { Id: value },
            success: function (data) {
                $("#contantBody").html('');
                $('#contantBody').html(data);
                var updateOutput = function (e) {
                    var list = e.length ? e : $(e.target),
                            output = list.data('output');
                    if (window.JSON) {
                        output.val(window.JSON.stringify(list.nestable('serialize')));//, null, 2));
                    }
                    else {
                        output.val('JSON browser support required for this demo.');
                    }
                };
                $.each($('.ApplicantNestable'), function () {
                    $(this).nestable({
                        maxDepth: 1,
                        group: 1,
                        callback: function (l, e) {
                            
                            var stepId = $(l).attr("data-stepId");
                            var stepName = $(l).attr("data-stepName");
                            var applicantId = $(e).attr("data-applicantId");
                            var VacancyId = $("#contantBody").find("#drp-SelectVacancyId")["0"].value;
                            var model = {
                                StepID: stepId,
                                StepName: stepName,
                                ApplicantID: applicantId,
                                VacancyID: VacancyId
                            }
                            $.ajax({
                                url: constantTMS.StepMove,
                                type: 'POST',
                                data: JSON.stringify(model),
                                contentType: "application/json",
                                success: function (data) {
                                    
                                    if (data.isRedirect == undefined) {
                                        BindData(data);
                                    }
                                    else {
                                        
                                        $('#AddResoure').modal('show');
                                        $.ajax({
                                            url: constantTMS.EditResource,
                                            data: { Id: data.redirectUrl.RouteValues[2].Value, VacancyID: data.redirectUrl.RouteValues[3].Value },
                                            success: function (data) {
                                                $('#AddResoureceBody').html('');
                                                $('#AddResoureceBody').html(data);
                                                $("#btn-submit-Resoure").html("Add");
                                                $('[data-toggle="tooltip"]').tooltip();
                                                $('#wizard').smartWizard({
                                                    onLeaveStep: leaveAStepCallback,
                                                    onFinish: onFinishCallback
                                                });
                                                $('#AddResoureceBody').find('.buttonNext').addClass('btn btn-warning');
                                                $('#AddResoureceBody').find('.buttonPrevious').addClass('btn btn-warning');
                                                $('#AddResoureceBody').find('.buttonFinish').addClass('btn btn-success');
                                                $('#AddResoureceBody').find('.buttonPrevious').hide();
                                                $('#AddResoureceBody').find('.buttonFinish').hide();

                                                $("#page_content").find("#DateOfBirth").Zebra_DatePicker({
                                                    showButtonPanel: false,
                                                    format: 'd-m-Y',
                                                    onSelect: function () {
                                                        $("#page_content").find('#ValidDateOfBirth').hide();
                                                        validDOB(dob);
                                                    }
                                                });
                                                $("#page_content").find("#StartDate").Zebra_DatePicker({
                                                    showButtonPanel: false,
                                                    format: 'd-m-Y',
                                                    onSelect: function () {
                                                        $("#page_content").find('#ValidStartDate').hide();
                                                        var startDate = $("#StartDate").val();
                                                        var proEndDate = $("#ProbationEndDate").val();
                                                        calculateprobEndDate(startDate, proEndDate);
                                                        var proEndFixDate = $("#FixedTermEndDate").val();
                                                        calculateFixedTermEdDate(startDate, proEndFixDate);
                                                    }
                                                });
                                                $("#page_content").find("#ProbationEndDate").Zebra_DatePicker({
                                                    showButtonPanel: false,
                                                    format: 'd-m-Y',
                                                    onSelect: function () {
                                                        $("#page_content").find('#ValidProbationEndDate').hide();
                                                        $("#ValidProbationEndDate").hide();
                                                        $("#page_content").find("#lbl-error-GreaterEndDate").hide();
                                                        var stDate = $("#StartDate").val();
                                                        var startdate = $("#page_content").find("#ProbationEndDate").val();
                                                        var enddate = $("#page_content").find("#NextProbationReviewDate").val();
                                                        calculateDateDiff(startdate, enddate);
                                                        calculateprobEndDate(stDate, startdate);
                                                    }
                                                });
                                                $("#page_content").find("#NextProbationReviewDate").Zebra_DatePicker({
                                                    showButtonPanel: false,
                                                    format: 'd-m-Y',
                                                    onSelect: function () {
                                                        $("#page_content").find('#ValidNextProbationReviewDate').hide();
                                                        $("#ValidProbationEndDate").hide();
                                                        $("#lbl-error-GreaterEndDate").hide();
                                                        var stDate = $("#StartDate").val();
                                                        var startdate = $("#page_content").find("#ProbationEndDate").val();
                                                        var enddate = $("#page_content").find("#NextProbationReviewDate").val();
                                                        calculateDateDiff(startdate, enddate);
                                                        calculateprobEndDate(stDate, startdate);
                                                    }
                                                });

                                                $("#page_content").find("#FixedTermEndDate").Zebra_DatePicker({
                                                    showButtonPanel: false,
                                                    format: 'd-m-Y',
                                                    onSelect: function () {
                                                        $("#page_content").find('#ValidFixedTermEndDate').hide();                                                                                                                
                                                        $("#page_content").find("#lbl-error-ValidFDEndDate").hide();
                                                        var startDate = $("#StartDate").val();
                                                        var proEndDate = $("#FixedTermEndDate").val();
                                                        calculateFixedTermEdDate(startDate, proEndDate);
                                                    }
                                                });
                                            }
                                        });


                                        $(".hrtoolLoader").hide();
                                        $(".modal-backdrop").hide();
                                    }

                                }
                            });
                        }
                    });
                });
                function changeTable() {
                }
                updateOutput($('.ApplicantNestable').data('output', $('.ApplicantNestable-output')));
                //$("#contantBody").find(".vacancyProcess").html(data.RecruitmentProcess);
                //$("#contantBody").find(".vacancyStatus").text(data.Status);
                //$("#contantBody").find(".vacancyCloseDate").text(data.ClosingDate);
                //$("#contantBody").find(".vacancyDescription").text(data.Summary);
                $("#contantBody").find("#VacancyDetailsShow").removeClass("hide");
                $(".hrtoolLoader").hide();
                $(".modal-backdrop").hide();

            }
        });
    }
    else {
        $.ajax({
            url: constantTMS.Applicant,
            success: function (data) {
                $("#contantBody").html('');
                $('#contantBody').html(data);
                $(".TMSIndex, .Vacancy, .Pool").removeClass('active');
                $(".Applicant").addClass('active');
                //$(this).find('option').removeAttr("selected");
                $(".hrtoolLoader").hide();
                $(".modal-backdrop").hide();
            }
        });
    }
});


$("#page_content").on("change", "#drpBusiness", function () {
    
    var value = $(this).val();
    if (value != "0") {
        $.ajax({
            url: constantTMS.bindDiv,
            data: { businessId: value },
            success: function (data) {
                
                $("#drpDivision").html('');
                var toAppend = '';
                toAppend += "<option value='0'>--Select--</option>";
                $.each(data, function (index, item) {
                    toAppend += "<option value='" + item.Id + "'>" + item.Name + "</option>";
                })
                $("#drpDivision").html(toAppend);
                if ($("#drpDivision").val() == 0) {
                    $("#drpDivision").val(0);
                    $('#drpPool').val(0);
                    $('#drpFunction').val(0);
                }
            }
        });
    }
    else {
        $('#drpDivision').empty();
        // Bind new values to dropdown
        $('#drpDivision').each(function () {
            // Create option
            var option = $("<option />");
            option.attr("value", '0').text('--Select--');
            $('#drpDivision').append(option);
        });
        $('#drpPool').empty();
        // Bind new values to dropdown
        $('#drpPool').each(function () {
            // Create option
            var option = $("<option />");
            option.attr("value", '0').text('--Select--');
            $('#drpPool').append(option);
        });
        $('#drpFunction').empty();
        // Bind new values to dropdown
        $('#drpFunction').each(function () {
            // Create option
            var option = $("<option />");
            option.attr("value", '0').text('--Select--');
            $('#drpFunction').append(option);
        });


    }
});
$("#page_content").on("change", "#drpDivision", function () {
    var value = $(this).val();
    if (value != "0") {
        $.ajax({
            url: constantTMS.bindpool,
            data: { DivisionId: value },
            success: function (data) {
                $("#drp-Pool").html('');
                var toAppend = '';
                toAppend += "<option value='0'>--Select--</option>";
                $.each(data, function (index, item) {
                    toAppend += "<option value='" + item.Id + "'>" + item.Name + "</option>";
                })
                $("#drpPool").html(toAppend);
                if ($("#drpPool").val() == 0) {
                    $("#drpPool").val(0);
                }
                $.ajax({
                    url: constantTMS.bindFuncation,
                    data: { DivisionId: value },
                    success: function (data) {
                        $("#drp-Function").html('');
                        var toAppend = '';
                        toAppend += "<option value='0'>--Select--</option>";
                        $.each(data, function (index, item) {
                            toAppend += "<option value='" + item.Id + "'>" + item.Name + "</option>";
                        })
                        $("#drpFunction").html(toAppend);
                        if ($("#drpFunction").val() == 0) {
                            $("#drpFunction").val(0);
                        }
                    }
                });
            }
        });
    }
    else {

        $('#drpPool').empty();
        // Bind new values to dropdown
        $('#drpPool').each(function () {
            // Create option
            var option = $("<option />");
            option.attr("value", '0').text('--Select--');
            $('#drpPool').append(option);
        });
        $('#drpFunction').empty();
        // Bind new values to dropdown
        $('#drpFunction').each(function () {
            // Create option
            var option = $("<option />");
            option.attr("value", '0').text('--Select--');
            $('#drpFunction').append(option);
        });


    }
});
$("#page_content").on("change", "#drpCountry", function () {
    var value = $(this).val();
    if (value != "0") {
        $.ajax({
            url: constantTMS.BindStateUrl,
            data: { countryId: value },
            success: function (data) {
                
                $("#drpState").html('');
                var toAppend = '';
                toAppend += "<option value='0'>--Select--</option>";
                $.each(data, function (index, item) {
                    toAppend += "<option value='" + item.Value + "'>" + item.Text + "</option>";
                })
                $("#drpState").html(toAppend);
                if ($("#drpState").val() == 0) {
                    $("#drpState").val(0);

                }

                $.ajax({
                    url: constantTMS.BindAirPortIDUrl,
                    data: { countryId: value },
                    success: function (data) {
                        
                        $("#drpAirport").html('');
                        var toAppend = '';
                        toAppend += "<option value='0'>--Select--</option>";
                        $.each(data, function (index, item) {
                            toAppend += "<option value='" + item.Value + "'>" + item.Text + "</option>";
                        })
                        $("#drpAirport").html(toAppend);
                        if ($("#drpAirport").val() == 0) {
                            $("#drpAirport").val(0);

                        }
                    }
                });

            }
        });
    }
    else {
        $("#drpState").empty();
        // Bind new values to dropdown
        $('#drpState').each(function () {
            // Create option
            var option = $("<option />");
            option.attr("value", '0').text('--Select--');
            $('#drpState').append(option);
        });
        $('#drpTown').empty();
        // Bind new values to dropdown
        $('#drpTown').each(function () {
            // Create option
            var option = $("<option />");
            option.attr("value", '0').text('--Select--');
            $('#drpTown').append(option);
        });
    }
});
$("#page_content").on("change", "#drpState", function () {
    
    var value = $(this).val();
    if (value != "0") {
        $.ajax({
            url: constantTMS.BindCityIDUrl,
            data: { stateId: value },
            success: function (data) {
                
                $("#drpTown").html('');
                var toAppend = '';
                toAppend += "<option value='0'>--Select--</option>";
                $.each(data, function (index, item) {
                    toAppend += "<option value='" + item.Value + "'>" + item.Text + "</option>";
                })
                $("#drpTown").html(toAppend);
                if ($("#drpTown").val() == 0) {
                    $("#drpTown").val(0);
                }
            }
        });
    }
    else {

        $('#drpTown').empty();
        // Bind new values to dropdown
        $('#drpTown').each(function () {
            // Create option
            var option = $("<option />");
            option.attr("value", '0').text('--Select--');
            $('#drpTown').append(option);
        });
    }
});

$("#page_content").on('click', '#btn-submit-Helpmecalculate', function () {
    var iserror = false;
    var FullTimeEntitlement = $("#FullTimeEntitlement").val();
    
    if (FullTimeEntitlement == "") {
        iserror = true;
        $("#ValidFullTimeEntitlement").show();
        $("#ValidFullTimeEntitlement").html("FullTime Entitlement is required.");
        $(".hrtoolLoader").hide();
        $(".modal-backdrop").hide();
    }
    var DaysPerWeek = $("#DaysPerWeek").val();
    if (DaysPerWeek == "") {
        iserror = true;
        $("#ValidDaysPerWeek").show();
        $("#ValidDaysPerWeek").html("Days Per Week is required.");
        $(".hrtoolLoader").hide();
        $(".modal-backdrop").hide();
    }

    var EntitlementIncludesPublicHoliday = $("#EntitlementIncludesPublicHoliday").val();
    if (EntitlementIncludesPublicHoliday == "") {
        iserror = true;
        $("#EntitlementIncludes").show();
        $("#EntitlementIncludes").html("Entitlement Includes Public Holidays required.");
        $(".hrtoolLoader").hide();
        $(".modal-backdrop").hide();
    }
    if (iserror) {
        $(".hrtoolLoader").hide();
        $(".modal-backdrop").hide();
        return false;
    }
    else {
        var startDate = $("#StartDate").val();
        var jobContry = $("#drpJobContry").val();
        var model =
            {
                StartDate: startDate,
                CountryId: jobContry,
                FullTimeEntitlement: FullTimeEntitlement,
                DaysPerWeek: DaysPerWeek,
                IncludePublicHolidays: EntitlementIncludesPublicHoliday
            }
        $.ajax({
            type: "POST",
            data: JSON.stringify(model),
            url: constantTMS.HelpCalculate,
            contentType: "application/json",
            success: function (result) {

                $("#Thisyear").val(result[0].Text);
                $("#Nextyear").val(result[1].Text);
                $(".hrtoolLoader").hide();
                $(".modal-backdrop").hide();
                $("#Helpmecalculate").hide();
                $(".toast-success").show();
                setTimeout(function () { $(".toast-success").hide(); }, 1500);
            }
        });
    }
});
$("#page_content").on('click', '#CopySegment', function () {

    var CopyId = $("#selectID").val();
    $.ajax({
        url: constantTMS.copyrecordUrl,
        data: { EmployeeId: CopyId },
        success: function (data) {

            $('#AddResoureceBody').find("#step-2").html('');
            $('#AddResoureceBody').find("#step-2").html(data);
            $("#page_content").find(".Datepiker").Zebra_DatePicker({
                showButtonPanel: false,
                format: 'd-m-Y',
            });
            $('#AddResoureceBody').find("#step-2").find("#StartDate").val("");
            $('[data-toggle="tooltip"]').tooltip();

            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();

        }
    });

});
function chkForSSO() {
    bol = validateSSO();
}
function validateSSO() {
    $('#ValidSSONo').hide();
    var ssoNo = $("#SSO").val();
    var Id = ssoNo;
    $.ajax({
        url: constantTMS.validSSo,
        data: { ID: Id },
        success: function (data) {
            bol = JSON.stringify(data);          
        }
    });
    return bol;
}
function ValidateEmail(email) {
    var expr = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return expr.test(email);
};

function leaveAStepCallback(obj, context) {
    if (context.fromStep == 1) {
        var iserror = false;
        if (bol == "true") {
            iserror = true;
            $("#ValidSSONo").show();
        }
        else if (bol == "false") {
            iserror = false;
        }
        var Title = $("#page_content").find("#drpTitle").val();
        if (Title == "0") {
            iserror = true;
            $("#ValidTitle").show();
            $("#ValidTitle").html("The Title is required.");
        }
        var FirstName = $("#page_content").find("#FirstName").val();
        if (FirstName == "") {
            iserror = true;
            $("#ValidFirstName").show();
            $("#ValidFirstName").html("The First Name is required.");
        }
        var Lastname = $("#page_content").find("#LastName").val();
        if (Lastname == "") {
            iserror = true;
            $("#ValidLastName").show();
            $("#ValidLastName").html("The Last Name is required.");
        }
        var Email = $("#page_content").find("#Email").val();
        var validSSNumber = $("#page_content").find("#NINumber").val();
        if (Email != "") {
            if (ValidateEmail(Email)) {
            }
            else {
                $("#ValidEmail").show();
                $("#ValidEmail").html("Invalid email address!");
                iserror = true;
            }
        }
        else {
            iserror = true;
            $("#ValidEmail").show();
            $("#ValidEmail").html("The Email is required.");
        }
        var Gender = $("#page_content").find('input[name=Gender]:checked').val();
        if (Gender != "") {

        }
        else {
            iserror = true;
            $("#ValidGender").show();
            $("#ValidGender").html("The Gender is required.");
        }

        var DateOfBirth = $("#page_content").find("#DateOfBirth").val();
        if (DateOfBirth == "") {
            iserror = true;
            $("#ValidDateOfBirth").show();
            $("#ValidDateOfBirth").html("The Date Of Birth is required.");
        }
        var Nationality = $("#page_content").find("#drpNationality").val();
        if (Nationality == "0") {
            iserror = true;
            $("#ValidNationality").show();
            $("#ValidNationality").html("The Nationality is required..");
        }

        if (iserror) {
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
            return false;
        }
        else {
            iserror = false;
            $('#AddResoureceBody').find('.buttonNext').show();
            $('#AddResoureceBody').find('.buttonPrevious').show();
            $('#AddResoureceBody').find('.buttonFinish').hide();
            //searchCopyFrom();
            return true;
        }
    }
    if (context.fromStep == 2) {
        var iserror = false;
        if (context.toStep == 1) {
            return true;
        }
        else {
            $('#AddResoureceBody').find('.buttonPrevious').show();
            var JobTitle = $("#page_content").find("#drpJobTitle").val();
            if (JobTitle == "0") {
                iserror = true;
                $("#ValidJobTitle").show();
                $("#ValidJobTitle").html("The Job Title is required.");
            }
            var Location = $("#page_content").find("#drpLocation").val();
            if (Location == "0") {
                iserror = true;
                $("#ValidLocation").show();
                $("#ValidLocation").html("The Location is required.");
            }
            var Jobcountry = $("#page_content").find("#drpJobContry").val();
            if (Jobcountry == "0") {
                iserror = true;
                $("#ValidJobContry").show();
                $("#ValidJobContry").html("The Job Country is required.");
            }
            if (iserror) {
                iserror = false;
                $('#AddResoureceBody').find('.buttonPrevious').show();
                return false;
            }
            else {
                $('#AddResoureceBody').find('.buttonNext').show();
                $('#AddResoureceBody').find('.buttonPrevious').show();
                $('#AddResoureceBody').find('.buttonFinish').hide();
                return true;
            }
            $('#AddResoureceBody').find("#ValidJobContry").hide();
            $('#AddResoureceBody').find("#ValidLocation").hide();
        }
    }
    if (context.fromStep == 3) {
        if (context.toStep == 2) {
            return true;
        }
        var iserror = false;
        var StartDate = $("#StartDate").val();
        if (StartDate == "") {
            iserror = true;
            $("#ValidStartDate").show();
            $("#ValidStartDate").html("The Start Date is required.");
        }
        var ResourceType = $("#page_content").find("#drpResourceType").val();
        if (ResourceType == 0) {
            iserror = true;
            $("#ValidResourceType").show();
        }
        var ProbationEndDate = $("#page_content").find("#ProbationEndDate").val();
        if (ProbationEndDate == "") {
            iserror = true;
            $("#ValidProbationEndDate").show();
            $("#ValidProbationEndDate").html("The Probation End Date is required.");
        }
        var NextProbationReviewDate = $("#NextProbationReviewDate").val();       
        var NoticePeriod = $("#drpNoticePeriod").val();
        if (NoticePeriod == 0) {
            iserror = true;
            $("#ValidNoticePeriod").show();
            $("#ValidNoticePeriod").html("The Notice Period is required.");
        }
        var NextProbationReviewDate = $("#NextProbationReviewDate").val();
      
        var FixedTermEndDate = $("#FixedTermEndDate").val();
       
        if (iserror) {
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
            return false;
        }
        else {
            $('#AddResoureceBody').find('.buttonNext').show();
            $('#AddResoureceBody').find('.buttonPrevious').show();
            $('#AddResoureceBody').find('.buttonFinish').hide();

            return true;
        }
    }
    if (context.fromStep == 4) {
        if (context.toStep == 3) {
            return true;
        }
        var iserror = false;
        var test = $('#SWIFT_Code').val();
        var drpCountry = $("#drpCountry").val();
        if (drpCountry == "0") {
            iserror = true;
            $("#ValidCountry").show();
            $("#ValidCountry").html("The Country is required.");
        }
        var drpState = $("#page_content").find("#drpState").val();
        if (drpState == "0") {
            iserror = true;
            $("#ValidState").show();
            $("#ValidState").html("The State is required.");
        }
        var drpTown = $("#page_content").find("#drpTown").val();
        if (drpTown == "0") {
            iserror = true;
            $("#ValidTown").show();
            $("#ValidTown").html("The Town is required.");
        }

        if (iserror) {
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
            return false;
        }
        else {
            $('#AddResoureceBody').find('.buttonNext').show();
            $('#AddResoureceBody').find('.buttonPrevious').show();
            $('#AddResoureceBody').find('.buttonFinish').hide();
            return true;
        }

    }
    if (context.fromStep == 5) {
        if (context.toStep == 4) {
            return true;
        }
        var iserror = false;
        if (iserror) {
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
            return false;
        }
        else {
            $('#AddResoureceBody').find('.buttonNext').show();
            $('#AddResoureceBody').find('.buttonPrevious').show();
            $('#AddResoureceBody').find('.buttonFinish').show();
            return true;
        }
    }
    if (context.fromStep == 6) {
        if (context.toStep == 5) {
            return true;
        }
        var iserror = false;
        if (iserror) {
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
            return false;
        }
        else {
            $('#AddResoureceBody').find('.buttonNext').show();
            $('#AddResoureceBody').find('.buttonPrevious').show();
            $('#AddResoureceBody').find('.buttonFinish').hide();
            return true;
        }
    }
    else {
        $('#AddResoureceBody').find('.buttonNext').show();
        $('#AddResoureceBody').find('.buttonPrevious').show();
        $('#AddResoureceBody').find('.buttonFinish').hide();
    }
}
function onFinishCallback() {
    
    $(".hrtoolLoader").show();
    //var Gender = $('input[name=Gender]:checked').val();
    var Gender = $("#AddResoureceBody").find('.selectGender input:checked').val();
    var iserror = false;
    var hiddenId = $("#hidden-Id").val();
    var gethtml = $("#page_content").find("#NewTaskList").clone().html();
    var selected = [];
    
    $("#page_content").find("#NewTaskList").find('.NewtaskListrecord').each(function () {
        
        var divId = $(this).attr('data-id');
        if ($("#NewTaskcheckbox_" + divId).prop('checked') == true) {
            var oneData = {
                Title: $(this).find(".NewTitle").val().trim(),
                Description: $(this).find(".NewDiscrtion").val().trim(),
                Assign: $(this).find(".Assignto").val().trim(),
                DueDate: $(this).find(".DueDateto").val().trim(),
                Status: $(this).find(".Statusto").val().trim(),
                AlertBeforeDays: $(this).find(".AlertBefore").val().trim()
            }
            selected.push(oneData);
        }
    });
    var SelectJsonTask = JSON.stringify(selected);
    
    var check = $("#checkWorker").val();
    var ssovalues = "W" + $("#page_content").find("#SSO").val();

    var model = {
        //Step 1
        ApplicantID: $("#page_content").find("#Applicant_ID").val(),
        VacancyID: $("#page_content").find("#Vacancy_ID").val(),
        CheckRecord: check,
        Id: $("#page_content").find("#hidden-Id").val(),
        Title: $("#page_content").find("#drpTitle").val(),
        FirstName: $("#page_content").find("#FirstName").val(),
        LastName: $("#page_content").find("#LastName").val(),
        OtherNames: $("#page_content").find("#OtherName").val(),
        KnownAs: $("#page_content").find("#Knownas").val(),
        SSO: ssovalues,
        UserNameEmail: $("#page_content").find("#Email").val(),
        IMAddress: $("#page_content").find("#IMAddress").val(),
        Gender: Gender,
        DateOfBirth: $("#page_content").find("#DateOfBirth").val(),
        Nationality: $("#page_content").find("#drpNationality").val(),
        NIN_SSN: $("#page_content").find("#NINumber").val(),
        PhotoPath: $("#page_content").find('#fileToUpload').val(),
        Picture: $("#page_content").find("#ImageData").val(),
        //Step 2

        StartDate: $("#page_content").find("#StartDate").val(),
        ResourceType: $("#page_content").find("#drpResourceType").val(),
        Reportsto: $("#page_content").find("#drpReportsto").val(),
        AdditionalReportsto: $("#page_content").find("#drpAdditionalReportsto").val(),
        HRResponsible: $("#page_content").find("#drpHRResponsible").val(),
        JobTitle: $("#page_content").find("#drpJobTitle").val(),
        JobCountrID: $("#page_content").find("#drpJobContry").val(),
        Location: $("#page_content").find("#drpLocation").val(),
        BusinessID: $("#page_content").find("#drpBusiness").val(),
        DivisionID: $("#page_content").find("#drpDivision").val(),
        PoolID: $("#page_content").find("#drpPool").val(),
        FunctionID: $("#page_content").find("#drpFunction").val(),

        //Step 3
        ProbationEndDate: $("#page_content").find("#ProbationEndDate").val(),
        NextProbationReviewDate: $("#page_content").find("#NextProbationReviewDate").val(),
        NoticePeriodID: $("#page_content").find("#drpNoticePeriod").val(),
        FixedTermEndDate: $("#page_content").find("#FixedTermEndDate").val(),
        MethodofRecruitmentSetup: $("#page_content").find("#MethodofRecruitmentSetup").val(),
        RecruitmentCost: $("#page_content").find("#RecruitmentCost").val(),
        HolidaysThisYear: $("#page_content").find("#Thisyear").val(),
        HolidaysNextYear: $("#page_content").find("#Nextyear").val(),

        //Step 4
        CountryId: $("#page_content").find("#drpCountry").val(),
        StateId: $("#page_content").find("#drpState").val(),
        CityyId: $("#page_content").find("#drpTown").val(),
        AirportId: $("#page_content").find("#drpAirport").val(),
        PostalCode: $("#page_content").find("#Postcode").val(),
        Address: $("#page_content").find("#Address").val(),
        WorkPhone: $("#page_content").find("#WorkPhone").val(),
        WorkMobile: $("#page_content").find("#WorkMobile").val(),
        PersonalPhone: $("#page_content").find("#PersonalPhone").val(),
        PersonalMobile: $("#page_content").find("#PersonalMobile").val(),
        PersonalEmail: $("#page_content").find("#PersonalEmail").val(),
        BankName: $("#page_content").find("#BankName").val(),
        BankCode: $("#page_content").find("#BankCode").val(),
        AccountNumber: $("#page_content").find("#AccountNumber").val(),
        OtherAccountInformation: $("#page_content").find('#OtherAccountInformation').val(),
        AccountName: $("#page_content").find("#AccountName").val(),
        BankAddress: $("#page_content").find('#BankAddress').val(),

        //step 5
        JsonNewtaskList: SelectJsonTask,
        // step 6 
        //done


    }
    $.ajax({
        type: "POST",
        data: JSON.stringify(model),
        url: constantTMS.SaveResoure,
        contentType: "application/json",
        success: function (result) {
            
            if (result.isRedirect) {
                $.ajax({
                    url: constantTMS.SelectVacancy,
                    data: { Id: result.redirectUrl.RouteValues[2].Value },
                    success: function (data) {
                        $("#contantBody").html('');
                        $('#contantBody').html(data);
                        var updateOutput = function (e) {
                            var list = e.length ? e : $(e.target),
                                    output = list.data('output');
                            if (window.JSON) {
                                output.val(window.JSON.stringify(list.nestable('serialize')));//, null, 2));
                            }
                            else {
                                output.val('JSON browser support required for this demo.');
                            }
                        };
                        $.each($('.ApplicantNestable'), function () {
                            $(this).nestable({
                                maxDepth: 1,
                                group: 1,
                                callback: function (l, e) {
                                    
                                    var stepId = $(l).attr("data-stepId");
                                    var stepName = $(l).attr("data-stepName");
                                    var applicantId = $(e).attr("data-applicantId");
                                    var VacancyId = $("#contantBody").find("#drp-SelectVacancyId")["0"].value;
                                    var model = {
                                        StepID: stepId,
                                        StepName: stepName,
                                        ApplicantID: applicantId,
                                        VacancyID: VacancyId
                                    }
                                    $.ajax({
                                        url: constantTMS.StepMove,
                                        type: 'POST',
                                        data: JSON.stringify(model),
                                        contentType: "application/json",
                                        success: function (data) {
                                            
                                            if (data.isRedirect == undefined) {
                                                BindData(data);
                                            }
                                            else {

                                                $('#AddResoure').modal('show');
                                                $.ajax({
                                                    url: constantTMS.EditResource,
                                                    data: { Id: data.redirectUrl.RouteValues[2].Value, VacancyID: data.redirectUrl.RouteValues[3].Value },
                                                    success: function (data) {
                                                        $('#AddResoureceBody').html('');
                                                        $('#AddResoureceBody').html(data);
                                                        $("#btn-submit-Resoure").html("Add");
                                                        $('[data-toggle="tooltip"]').tooltip();
                                                        $('#wizard').smartWizard({
                                                            onLeaveStep: leaveAStepCallback,
                                                            onFinish: onFinishCallback
                                                        });
                                                        $('#AddResoureceBody').find('.buttonNext').addClass('btn btn-warning');
                                                        $('#AddResoureceBody').find('.buttonPrevious').addClass('btn btn-warning');
                                                        $('#AddResoureceBody').find('.buttonFinish').addClass('btn btn-success');
                                                        $('#AddResoureceBody').find('.buttonPrevious').hide();
                                                        $('#AddResoureceBody').find('.buttonFinish').hide();

                                                        $("#page_content").find("#DateOfBirth").Zebra_DatePicker({
                                                            showButtonPanel: false,
                                                            format: 'd-m-Y',
                                                            onSelect: function () {
                                                                $("#page_content").find('#ValidDateOfBirth').hide();
                                                                validDOB(dob);
                                                            }
                                                        });
                                                        $("#page_content").find("#StartDate").Zebra_DatePicker({
                                                            showButtonPanel: false,
                                                            format: 'd-m-Y',
                                                            onSelect: function () {
                                                                $("#page_content").find('#ValidStartDate').hide();
                                                                var startDate = $("#StartDate").val();
                                                                var proEndDate = $("#ProbationEndDate").val();
                                                                calculateprobEndDate(startDate, proEndDate);
                                                                var proEndFixDate = $("#FixedTermEndDate").val();
                                                                calculateFixedTermEdDate(startDate, proEndFixDate);
                                                            }
                                                        });
                                                        $("#page_content").find("#ProbationEndDate").Zebra_DatePicker({
                                                            showButtonPanel: false,
                                                            format: 'd-m-Y',
                                                            onSelect: function () {                                                                
                                                                $("#ValidProbationEndDate").hide();
                                                                $("#lbl-error-GreaterEndDate").hide();
                                                                var stDate = $("#StartDate").val();
                                                                var startdate = $("#page_content").find("#ProbationEndDate").val();
                                                                var enddate = $("#page_content").find("#NextProbationReviewDate").val();
                                                                calculateDateDiff(startdate, enddate);
                                                                calculateprobEndDate(stDate, startdate);
                                                            }
                                                        });
                                                        $("#page_content").find("#NextProbationReviewDate").Zebra_DatePicker({
                                                            showButtonPanel: false,
                                                            format: 'd-m-Y',
                                                            onSelect: function () {
                                                                $("#page_content").find("#ValidNextProbationReviewDate").hide();
                                                                $("#ValidProbationEndDate").hide();
                                                                $("#lbl-error-GreaterEndDate").hide();
                                                                var stDate = $("#StartDate").val();
                                                                var startdate = $("#page_content").find("#ProbationEndDate").val();
                                                                var enddate = $("#page_content").find("#NextProbationReviewDate").val();
                                                                calculateDateDiff(startdate, enddate);
                                                                calculateprobEndDate(stDate, startdate);
                                                            }
                                                        });

                                                        $("#page_content").find("#FixedTermEndDate").Zebra_DatePicker({
                                                            showButtonPanel: false,
                                                            format: 'd-m-Y',
                                                            onSelect: function () {
                                                                $("#page_content").find('#ValidFixedTermEndDate').hide();                                                                
                                                                $("#page_content").find("#lbl-error-ValidFDEndDate").hide();
                                                                var startDate = $("#StartDate").val();
                                                                var proEndDate = $("#FixedTermEndDate").val();
                                                                calculateFixedTermEdDate(startDate, proEndDate);
                                                            }
                                                        });
                                                    }
                                                });


                                                $(".hrtoolLoader").hide();
                                                $(".modal-backdrop").hide();
                                            }

                                        }
                                    });

                                    // l is the main container 
                                    // e is the element that was moved 
                                },
                            });
                        });
                        function changeTable() {
                        }
                        updateOutput($('.ApplicantNestable').data('output', $('.ApplicantNestable-output')));
                        //$("#contantBody").find(".vacancyProcess").html(data.RecruitmentProcess);
                        //$("#contantBody").find(".vacancyStatus").text(data.Status);
                        //$("#contantBody").find(".vacancyCloseDate").text(data.ClosingDate);
                        //$("#contantBody").find(".vacancyDescription").text(data.Summary);
                        $("#contantBody").find("#VacancyDetailsShow").removeClass("hide");
                        $(".hrtoolLoader").hide();
                        $(".modal-backdrop").hide();

                    }
                });
            }
        }
    });
}

//Add Edit Applicant
$("#contantBody").on('click', '.btn-add-Applicant', function () {
    
    $(".hrtoolLoader").show();
    var VacancyId = $("#contantBody").find("#drp-SelectVacancyId")["0"].value;
    $.ajax({
        url: constantTMS.AddEditApplicant,
        data: { Id: 0, VacancyId: VacancyId },
        success: function (data) {
            
            $("#AddApplicantBody").html('');
            $('#AddApplicantBody').html(data);

            $('[data-toggle="tooltip"]').tooltip();

           // $("#contantBody").find('#ApplicantModel').find(".applicantTitle").text("Add Vacancy");
            $("#contantBody").find('#AddApplicantBody').find('#wizard').smartWizard({
                onLeaveStep: onApplicantCallback,
                onFinish: onApplicantFinishCallback
            });
            $("#AddApplicantBody").find("#txt_DateOfBitrh").Zebra_DatePicker({
                //direction: false,
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {
                    validDOB();
                    //var date = $("#AddVacancyLogBody").find("txt_ClosingDate").val();
                }
            });
            var $range = $(".Competency_Score");
            $(".Competency_Score").ionRangeSlider({
                type: "single",
                min: 0,
                max: 5
            });
            $range.on("change", function () {
                var $this = $(this),
                    value = $this.prop("value");
                $("#contantBody").find(".Competency_Score").val(value);
                //alert("Value: " + value);
            });

            $("#contantBody").find('#AddApplicantBody').find('.buttonNext').addClass('btn btn-warning');
            $("#contantBody").find('#AddApplicantBody').find('.buttonPrevious').addClass('btn btn-warning');
            $("#contantBody").find('#AddApplicantBody').find('.buttonFinish').addClass('btn btn-success');

            $("#contantBody").find('#AddApplicantBody').find('.buttonPrevious').hide();
            $("#contantBody").find('#AddApplicantBody').find('.buttonFinish').hide();

            $("#contantBody").find('#AddApplicantBody').find('div#Applicantfroala-editor').froalaEditor({
                toolbarButtons: ['bold', 'italic', 'underline', 'strikeThrough', 'subscript', 'superscript', 'fontFamily', 'fontSize', '|', 'specialCharacters', 'color',  'inlineStyle', 'paragraphStyle', '|', 'paragraphFormat', 'align', 'formatOL', 'formatUL', 'outdent', 'indent', '-', 'quote', 'insertHR', 'insertLink', 'insertImage', 'insertVideo', 'insertFile', 'insertTable', '|', 'undo', 'redo', 'clearFormatting', 'selectAll', 'html', 'applyFormat', 'removeFormat', 'fullscreen', 'print', 'help'],
                //toolbarButtons: ['bold', 'italic', 'underline', 'formatUL'],
                pluginsEnabled: null
            });
            $("#contantBody").find("#AddApplicantBody").find("#drp-GeneralSkills").selectList();
            $("#contantBody").find("#AddApplicantBody").find("#drp-TechnicalSkills").selectList();
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
        }
    });
});
function ValidateEmail(email) {
    var expr = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return expr.test(email);
};
function validDOB()
{
    var dob = $("#contantBody").find('#AddApplicantBody').find('#txt_DateOfBitrh').val();
    var now = new Date();
    var a = dob.split(" ");
    var d = a[0].split("-");
    var date = new Date();
    var age = now.getFullYear() - d[2];  
    if(isNaN(age)||age < 18)
    {
        $("#contantBody").find('#AddApplicantBody').find("#lbl-error-validdateOfBirth").show();
        $("#contantBody").find('#AddApplicantBody').find("#txt_DateOfBitrh").val('');
    }
    else {
        $("#contantBody").find('#AddApplicantBody').find("#lbl-error-validdateOfBirth").hide();
    }
}

function onApplicantCallback(obj, context) {
    if (context.fromStep == 1) {
        var isError = false;
        var FirstName = $("#contantBody").find('#AddApplicantBody').find('#txt_FirstName').val().trim();
        var LastName = $("#contantBody").find('#AddApplicantBody').find('#txt_LastName').val().trim();
        var Email = $("#contantBody").find('#AddApplicantBody').find('#txt_Email').val();
        //if (Email != "") { var EmailValid = isValidEmailAddress(Email); if (!EmailValid) { isError = true; $("#contantBody").find('#AddApplicantBody').find("#lbl-error-EmailValid").show(); } }
        var GenderID = $("#contantBody").find('#AddApplicantBody').find('#drp-selGender').val();
        var DateOfBitrh = $("#contantBody").find('#AddApplicantBody').find('#txt_DateOfBitrh').val();

        if (FirstName == "") { isError = true; $("#contantBody").find('#AddApplicantBody').find("#lbl-error-FirstName").show(); }
        if (LastName == "") { isError = true; $("#contantBody").find('#AddApplicantBody').find("#lbl-error-LastName").show(); }
        if (Email != "") {
            if (ValidateEmail(Email)) {
            }
            else
            {
                isError = true;
                $("#contantBody").find('#AddApplicantBody').find("#lbl-error-EmailValid").show();
            }
        }
        if (GenderID == "0") { isError = true; $("#contantBody").find('#AddApplicantBody').find("#lbl-error-GenderList").show(); }
        if (DateOfBitrh == "") { isError = true; $("#contantBody").find('#AddApplicantBody').find("#lbl-error-dateOfBirth").show(); }
       

        if (isError) {
            return false;
        }
        else {
            if (context.toStep = 2) {
                $("#contantBody").find('#AddApplicantBody').find('.buttonNext').show();
                $("#contantBody").find('#AddApplicantBody').find('.buttonPrevious').show();
                $("#contantBody").find('#AddApplicantBody').find('.buttonFinish').hide();
            }
            else {
                $("#contantBody").find('#AddApplicantBody').find('.buttonNext').hide();
                $("#contantBody").find('#AddApplicantBody').find('.buttonPrevious').show();
            }
            $("#contantBody").find('#AddApplicantBody').find("#lbl-error-FirstName").hide();
            $("#contantBody").find('#AddApplicantBody').find("#lbl-Exits-LastName").hide();
            $("#contantBody").find('#AddApplicantBody').find("#lbl-error-Email").hide();
            $("#contantBody").find('#AddApplicantBody').find("#lbl-error-EmailValid").hide();
            $("#contantBody").find('#AddApplicantBody').find("#lbl-error-GenderList").hide();
            $("#contantBody").find('#AddApplicantBody').find("#lbl-error-dateOfBirth").hide();

            return true;
        }
    }
    if (context.fromStep == 2) {
        var isError = false;
        var CoverLetterPathOriginal = $("#contantBody").find('#ApplicantModel').find('#CoverLetterName').val();
        var CoverLetterPath = $("#contantBody").find('#ApplicantModel').find('#txt_CoverLetterName').val();
        var DownloadApplicationFormLink = $("#contantBody").find('#ApplicantModel').find('#DownloadLink').attr('href');
        var UploadApplicationFormPathOriginal = $("#contantBody").find('#ApplicantModel').find('#UploadApplicationFormName').val();
        var UploadApplicationFormPath = $("#contantBody").find('#ApplicantModel').find('#txt_UploadApplicationFormName').val();
        var ResumePathOriginal = $("#contantBody").find('#ApplicantModel').find('#ResumePathName').val();
        var ResumePath = $("#contantBody").find('#ApplicantModel').find('#txt_ResumePathName').val();
        var Question1Answer = $("#contantBody").find('#ApplicantModel').find('#txt_Question1Answer').val();
        var Question2Answer = $("#contantBody").find('#ApplicantModel').find('#txt_Question2Answer').val();
        var Question3Answer = $("#contantBody").find('#ApplicantModel').find('#txt_Question3Answer').val();
        var Question4Answer = $("#contantBody").find('#ApplicantModel').find('#txt_Question4Answer').val();
        var Question5Answer = $("#contantBody").find('#ApplicantModel').find('#txt_Question5Answer').val();
        var IsUploadCoverLetter = $("#contantBody").find('#ApplicantModel').find('#hiddenIsCoverLetter').val();
        var IsUploadCVOrResume = $("#contantBody").find('#ApplicantModel').find('#hiddenIsCVOrResume').val();
        var hdnOuestion1On = $("#contantBody").find('#ApplicantModel').find("#hiddenIsQue1On").val();
        var hdnOuestion2On = $("#contantBody").find('#ApplicantModel').find("#hiddenIsQue2On").val();
        var hdnOuestion3On = $("#contantBody").find('#ApplicantModel').find("#hiddenIsQue3On").val();
        var hdnOuestion4On = $("#contantBody").find('#ApplicantModel').find("#hiddenIsQue4On").val();
        var hdnOuestion5On = $("#contantBody").find('#ApplicantModel').find("#hiddenIsQue5On").val();
        if (hdnOuestion1On == 1) {
            if (Question1Answer == "") {
                isError = true;
                $("#lbl-error-Question1Answer").show();
            }
        }
        if (hdnOuestion2On == 1) {
            if (Question2Answer == "") {
                isError = true;
                $("#lbl-error-Question2Answer").show();
            }
        }
        if (hdnOuestion3On == 1) {
            if (Question3Answer == "") {
                isError = true;
                $("#lbl-error-Question3Answer").show();
            }
        }
        if (hdnOuestion4On == 1) {
            if (Question4Answer == "") {
                isError = true;
                $("#lbl-error-Question4Answer").show();
            }
        }
        if (hdnOuestion5On == 1) {
            if (Question5Answer == "") {
                isError = true;
                $("#lbl-error-Question5Answer").show();
            }
        }
        if (IsUploadCoverLetter == 1) {
            if (CoverLetterPath == "") {
                isError = true;
                $("#lbl-error-CoverLetter").show();
            }
        }
        if (IsUploadCVOrResume == 1) {
            if (ResumePath == "") {
                isError = true;
                $("#lbl-error-ResumePathName").show();
            }
        }
        if (UploadApplicationFormPath == "") {
            isError = true;
            $("#lbl-error-UploadApplicationForm").show();
        }
        if (isError) {
            return false;
        }
        else {
            if (context.toStep == 3) {
                $("#contantBody").find('#AddApplicantBody').find('.buttonNext').show();
                $("#contantBody").find('#AddApplicantBody').find('.buttonPrevious').show();
                $("#contantBody").find('#AddApplicantBody').find('.buttonFinish').hide();
            }
            else {
                $("#contantBody").find('#AddApplicantBody').find('.buttonNext').show();
                $("#contantBody").find('#AddApplicantBody').find('.buttonPrevious').show();
                //$("#contantBody").find('#AddApplicantBody').find('.buttonFinish').show();
            }
            //$("#contantBody").find('#AddApplicantBody').find("#lbl-error-JobDescription").hide();
            //$("#contantBody").find('#AddApplicantBody').find("#lbl-error-HiringLeadList").hide();

            return true;

        }

    }
    if (context.fromStep == 3) {
        
        var isError = false;
        // var JobDescription = $("#contantBody").find('#AddVacancyLogBody').find('#froala-editor').val();
        //  var HiringLead = $("#contantBody").find('#AddVacancyLogBody').find('#drp-HiringLeadId').val();
        // if (JobDescription == "") { isError = true; $("#contantBody").find('#AddVacancyLogBody').find("#lbl-error-JobDescription").show(); }
        // if (HiringLead == "0") { isError = true; $("#contantBody").find('#AddVacancyLogBody').find("#lbl-error-HiringLeadList").show(); }
        if (isError) {
            return false;
        }
        else {
            if (context.toStep == 4) {
                $("#contantBody").find('#AddApplicantBody').find('.buttonNext').show();
                $("#contantBody").find('#AddApplicantBody').find('.buttonPrevious').show();
                $("#contantBody").find('#AddApplicantBody').find('.buttonFinish').hide();
            }
            else {
                $("#contantBody").find('#AddApplicantBody').find('.buttonNext').show();
                $("#contantBody").find('#AddApplicantBody').find('.buttonPrevious').show();
                //$("#contantBody").find('#AddApplicantBody').find('.buttonFinish').show();
            }
            //$("#contantBody").find('#AddApplicantBody').find("#lbl-error-JobDescription").hide();
            //$("#contantBody").find('#AddApplicantBody').find("#lbl-error-HiringLeadList").hide();

            return true;

        }

    }
    if (context.fromStep == 4) {
        
        var isError = false;
        // var JobDescription = $("#contantBody").find('#AddVacancyLogBody').find('#froala-editor').val();
        //  var HiringLead = $("#contantBody").find('#AddVacancyLogBody').find('#drp-HiringLeadId').val();
        // if (JobDescription == "") { isError = true; $("#contantBody").find('#AddVacancyLogBody').find("#lbl-error-JobDescription").show(); }
        // if (HiringLead == "0") { isError = true; $("#contantBody").find('#AddVacancyLogBody').find("#lbl-error-HiringLeadList").show(); }
        if (isError) {
            return false;
        }
        else {
            if (context.toStep == 5) {
                $("#contantBody").find('#AddApplicantBody').find('.buttonNext').show();
                $("#contantBody").find('#AddApplicantBody').find('.buttonPrevious').show();
                $("#contantBody").find('#AddApplicantBody').find('.buttonFinish').hide();
            }
            else {
                $("#contantBody").find('#AddApplicantBody').find('.buttonNext').show();
                $("#contantBody").find('#AddApplicantBody').find('.buttonPrevious').show();
                //$("#contantBody").find('#AddApplicantBody').find('.buttonFinish').show();
            }
            //$("#contantBody").find('#AddApplicantBody').find("#lbl-error-JobDescription").hide();
            //$("#contantBody").find('#AddApplicantBody').find("#lbl-error-HiringLeadList").hide();

            return true;

        }

    }
    if (context.fromStep == 5) {
        
        var isError = false;
        // var JobDescription = $("#contantBody").find('#AddVacancyLogBody').find('#froala-editor').val();
        //  var HiringLead = $("#contantBody").find('#AddVacancyLogBody').find('#drp-HiringLeadId').val();
        // if (JobDescription == "") { isError = true; $("#contantBody").find('#AddVacancyLogBody').find("#lbl-error-JobDescription").show(); }
        // if (HiringLead == "0") { isError = true; $("#contantBody").find('#AddVacancyLogBody').find("#lbl-error-HiringLeadList").show(); }
        if (isError) {
            return false;
        }
        else {
            if (context.toStep == 6) {
                $("#contantBody").find('#AddApplicantBody').find('.buttonNext').show();
                $("#contantBody").find('#AddApplicantBody').find('.buttonPrevious').show();
                $("#contantBody").find('#AddApplicantBody').find('.buttonFinish').hide();
            }
            else {
                $("#contantBody").find('#AddApplicantBody').find('.buttonNext').show();
                $("#contantBody").find('#AddApplicantBody').find('.buttonPrevious').show();
                // $("#contantBody").find('#AddApplicantBody').find('.buttonFinish').show();
            }
            //$("#contantBody").find('#AddApplicantBody').find("#lbl-error-JobDescription").hide();
            //$("#contantBody").find('#AddApplicantBody').find("#lbl-error-HiringLeadList").hide();

            return true;

        }

    }
    if (context.fromStep == 6) {
        
        var isError = false;
        // var JobDescription = $("#contantBody").find('#AddVacancyLogBody').find('#froala-editor').val();
        //  var HiringLead = $("#contantBody").find('#AddVacancyLogBody').find('#drp-HiringLeadId').val();
        // if (JobDescription == "") { isError = true; $("#contantBody").find('#AddVacancyLogBody').find("#lbl-error-JobDescription").show(); }
        // if (HiringLead == "0") { isError = true; $("#contantBody").find('#AddVacancyLogBody').find("#lbl-error-HiringLeadList").show(); }
        if (isError) {
            return false;
        }
        else {
            if (context.toStep == 7) {
                $("#contantBody").find('#AddApplicantBody').find('.buttonNext').hide();
                $("#contantBody").find('#AddApplicantBody').find('.buttonPrevious').show();
                $("#contantBody").find('#AddApplicantBody').find('.buttonFinish').show();
            }
            else {
                $("#contantBody").find('#AddApplicantBody').find('.buttonNext').show();
                $("#contantBody").find('#AddApplicantBody').find('.buttonPrevious').show();
                $("#contantBody").find('#AddApplicantBody').find('.buttonFinish').hide();
            }
            //$("#contantBody").find('#AddApplicantBody').find("#lbl-error-JobDescription").hide();
            //$("#contantBody").find('#AddApplicantBody').find("#lbl-error-HiringLeadList").hide();

            return true;

        }

    }
    else {

        if (context.toStep == 6) {
            $("#contantBody").find('#AddApplicantBody').find('.buttonNext').show();
            $("#contantBody").find('#AddApplicantBody').find('.buttonPrevious').show();
            $("#contantBody").find('#AddApplicantBody').find('.buttonFinish').hide();
        }
        else {
            $("#contantBody").find('#AddApplicantBody').find('.buttonNext').show();
            $("#contantBody").find('#AddApplicantBody').find('.buttonPrevious').hide();
            $("#contantBody").find('#AddApplicantBody').find('.buttonFinish').hide();
        }
        return true;
    }
}

function onApplicantFinishCallback(obj, context) {
    $(".hrtoolLoader").show();

    var Id = $("#contantBody").find('#ApplicantModel').find('#ApplicantHiddenId').val();
    var VacancyID = $("#contantBody").find('#ApplicantModel').find('#ApplicantVacancyId').val();
    var FirstName = $("#contantBody").find('#ApplicantModel').find('#txt_FirstName').val().trim();
    var LastName = $("#contantBody").find('#ApplicantModel').find('#txt_LastName').val().trim();
    var Email = $("#contantBody").find('#ApplicantModel').find('#txt_Email').val().trim();
    var GenderID = $("#contantBody").find('#ApplicantModel').find('.selectGender input:checked').val();
    var DateOfbirth = $("#contantBody").find("#ApplicantModel").find("#txt_DateOfBitrh").val();
    var PostalCode = $("#contantBody").find("#ApplicantModel").find("#txt_PostalCode").val();
    var Address = $("#contantBody").find("#ApplicantModel").find("#AddressText").val().trim();
    var OtherContactDetails = $("#contantBody").find("#ApplicantModel").find("#txt_OtherContactDetails").val().trim();
    var SelectStepID = $("#contantBody").find('#ApplicantModel').find('#drp-SelectStepID').val();
    var CoverLetterPathOriginal = $("#contantBody").find('#ApplicantModel').find('#CoverLetterName').val();
    //var CoverLetterPathOriginal = $("#contantBody").find('#ApplicantModel').find('#CoverLetterName')["0"].innerText;
    var CoverLetterPath = $("#contantBody").find('#ApplicantModel').find('#txt_CoverLetterName').val();
    var DownloadApplicationFormLink = $("#contantBody").find('#ApplicantModel').find('#DownloadLink').attr('href');
    //var UploadApplicationFormPathOriginal = $("#contantBody").find('#ApplicantModel').find('#UploadApplicationFormName')["0"].innerText;
    var UploadApplicationFormPathOriginal = $("#contantBody").find('#ApplicantModel').find('#UploadApplicationFormName').val();
    var UploadApplicationFormPath = $("#contantBody").find('#ApplicantModel').find('#txt_UploadApplicationFormName').val();
    //var ResumePathOriginal = $("#contantBody").find('#ApplicantModel').find('#ResumePathName')["0"].innerText;
    var ResumePathOriginal = $("#contantBody").find('#ApplicantModel').find('#ResumePathName').val();
    var ResumePath = $("#contantBody").find('#ApplicantModel').find('#txt_ResumePathName').val();
    var Question1Answer = $("#contantBody").find('#ApplicantModel').find('#txt_Question1Answer').val();
    var Question2Answer = $("#contantBody").find('#ApplicantModel').find('#txt_Question2Answer').val();
    var Question3Answer = $("#contantBody").find('#ApplicantModel').find('#txt_Question3Answer').val();
    var Question4Answer = $("#contantBody").find('#ApplicantModel').find('#txt_Question4Answer').val();
    var Question5Answer = $("#contantBody").find('#ApplicantModel').find('#txt_Question5Answer').val();
    var SourceID = $("#contantBody").find('#ApplicantModel').find('#drp-SourceId').val();
    //CompetecyList_UL
    // var JobDescription = $("#contantBody").find('#AddVacancyLogBody').find('#froala-editor').froalaEditor('html.get');
    jsonCompetencySegmentObj = [];
    var id = 0;
    $.each($("#CompetecyList_UL").find("li"), function () {

        var competencyName = $(this).find(".Competency_NAME").val();
        var Score = $(this).find(".Competency_Score").val();
        id++;
        var oneData = {
            Id: id,
            CompetencyName: competencyName,
            Score: Score
        }
        jsonCompetencySegmentObj.push(oneData);
    });
    var AllCompetencySegmentJson = JSON.stringify(jsonCompetencySegmentObj);

    var commentList = [];
    $.each($("#contantBody").find('#CommentList').find(".seccomments"), function () {
        var commentBy = $(this).find(".postedby").html().trim();
        var comment = $(this).find(".sickComments").html().trim();
        var commentTime = $(this).find(".commentTime").html().trim();
        var newReord = {
            commentBy: commentBy,
            comment: comment,
            commentTime: commentTime
        }
        commentList.push(newReord);
    });
    var JsoncommentListJoinString = JSON.stringify(commentList);

    var documentList = [];
    $.each($("#contantBody").find('#filesList').find(".ListData"), function () {
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

    GeneralSkills = [];
    $.each($("#contantBody").find("#drp-GeneralSkills").parent().find(".selectlist-item"), function () {
        GeneralSkills.push($("#contantBody").find("#drp-GeneralSkills").parent().find(".selectlist-select").find('option:contains(' + $(this).text().trim() + ')')[0].value);
    });
    TechnicalSkills = [];
    $.each($("#contantBody").find("#drp-TechnicalSkills").parent().find(".selectlist-item"), function () {
        TechnicalSkills.push($("#contantBody").find("#drp-TechnicalSkills").parent().find(".selectlist-select").find('option:contains(' + $(this).text().trim() + ')')[0].value);
    });
    var Cost = $("#contantBody").find('#ApplicantModel').find('#txt_Cost').val().trim();
    var RejectResnCmt=$("#rejectionComments").val();
    var RejectReasonId=$("#drp-RejectionReasonStepID").val();
    var model = {
        Id: Id,
        VacancyID: VacancyID,
        FirstName: FirstName,
        LastName: LastName,
        Email: Email,
        GenderID: GenderID,
        DateOfBirth: DateOfbirth,
        PostalCode: PostalCode,
        Address: Address,
        OtherContactDetails: OtherContactDetails,
        StatusID: SelectStepID,
        CoverLetterPathOriginal: CoverLetterPathOriginal,
        CoverLetterPath: CoverLetterPath,
        DownloadApplicationFormLink: DownloadApplicationFormLink,
        UploadApplicationFormPathOriginal: UploadApplicationFormPathOriginal,
        UploadApplicationFormPath: UploadApplicationFormPath,
        ResumePathOriginal: ResumePathOriginal,
        ResumePath: ResumePath,
        Question1Answer: Question1Answer,
        Question2Answer: Question2Answer,
        Question3Answer: Question3Answer,
        Question4Answer: Question4Answer,
        Question5Answer: Question5Answer,
        SourceID: SourceID,
        CompatencyJSV: AllCompetencySegmentJson,
        CommentJSV: JsoncommentListJoinString,
        DocumentJSV: JsondocumentListJoinString,
        GeneralSkillsJSV: GeneralSkills.join(','),
        TechnicalSkillsJSV: TechnicalSkills.join(','),
        Cost: Cost,
        RejectReasonId:RejectReasonId,
            RejectReasonComment:RejectResnCmt
    }

    $.ajax({
        url: constantTMS.SaveApplicant,
        type: 'POST',
        data: JSON.stringify(model),
        contentType: "application/json",
        success: function (data) {
            redirectToApplicantPage();
            //if (data == "Error") {
            //    isError = true;
            //    $("#contantBody").find('#AddVacancyLogBody').find("#lbl-Exist-Vacancy").show();
            //    $(".hrtoolLoader").hide();
            //    $(".modal-backdrop").hide();
            //}
            //$("#contantBody").html("");
            //$("#contantBody").html(data);
            //$(".TMSIndex, .Applicant, .Pool").removeClass('active');
            //$(".Applicant").addClass('active');
            //$(".hrtoolLoader").hide();
            //$(".modal-backdrop").hide();

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

//Cover Letter fileUpload
$("#contantBody").on('change', '#CoverLetterfileToUpload', function (e) {
    var files = e.target.files;
    var FileUploadData = "";
    if (files.length > 0) {
        if (window.FormData !== undefined) {
            FileUploadData = new FormData();
            for (var x = 0; x < files.length; x++) {
                FileUploadData.append("file" + x, files[x]);
            }
            setTimeout(function () {
                $.ajax({
                    type: "POST",
                    url: constantTMS.FileUpload,
                    contentType: false,
                    processData: false,
                    data: FileUploadData,
                    success: function (result) {
                        
                        $("#contantBody").find('#AddApplicantBody').find('#CoverLetterName').html("");
                        $("#contantBody").find('#AddApplicantBody').find('#CoverLetterName').html(result.originalFileName);
                        $("#contantBody").find('#AddApplicantBody').find('#txt_CoverLetterName').val(result.NewFileName);


                    }
                });
            }, 500);
        }
    } else {
        $(".hrtoolLoader").hide();
        $(".modal-backdrop").hide();
    }
});

//Application Form FileUpload  
$("#contantBody").on('change', '#UploadApplicationFormfileToUpload', function (e) {
    $("#lbl-error-UploadApplicationForm").hide();
    var files = e.target.files;
    var FileUploadData = "";
    if (files.length > 0) {
        if (window.FormData !== undefined) {
            FileUploadData = new FormData();
            for (var x = 0; x < files.length; x++) {
                FileUploadData.append("file" + x, files[x]);
            }
            setTimeout(function () {
                $.ajax({
                    type: "POST",
                    url: constantTMS.FileUpload,
                    contentType: false,
                    processData: false,
                    data: FileUploadData,
                    success: function (result) {
                        
                        $('#AddApplicantBody').find('#UploadApplicationFormName').html("");
                        $('#AddApplicantBody').find('#UploadApplicationFormName').html(result.originalFileName);
                        $('#AddApplicantBody').find('#txt_UploadApplicationFormName').val(result.NewFileName);


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

//Resume FileUpload 
$("#contantBody").on('change', '#ResumePathNamefileToUpload', function (e) {
    $("#lbl-error-ResumePathName").hide();
    var files = e.target.files;
    var FileUploadData = "";
    if (files.length > 0) {
        if (window.FormData !== undefined) {
            FileUploadData = new FormData();
            for (var x = 0; x < files.length; x++) {
                FileUploadData.append("file" + x, files[x]);
            }
            setTimeout(function () {
                $.ajax({
                    type: "POST",
                    url: constantTMS.FileUpload,
                    contentType: false,
                    processData: false,
                    data: FileUploadData,
                    success: function (result) {
                        
                        $('#AddApplicantBody').find('#ResumePathName').html("");
                        $('#AddApplicantBody').find('#ResumePathName').html(result.originalFileName);
                        $('#AddApplicantBody').find('#txt_ResumePathName').val(result.NewFileName);


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

//Document FileUpload
$("#contantBody").on('click', '.file-deleteicon', function () {
    $(this).parent().remove();
});

$("#contantBody").on('change', '#DocumentfileToUpload', function (e) {
    var files = e.target.files;
    var FileUploadData = "";
    if (files.length > 0) {
        if (window.FormData !== undefined) {
            FileUploadData = new FormData();
            for (var x = 0; x < files.length; x++) {
                FileUploadData.append("file" + x, files[x]);
            }
            setTimeout(function () {
                $.ajax({
                    type: "POST",
                    url: constantTMS.FileUpload,
                    contentType: false,
                    processData: false,
                    data: FileUploadData,
                    success: function (result) {
                        var string = '<div class="row ListData"><div class="col-lg-1 icon"><i class="fa fa-paperclip"></i></div><div class="col-lg-10 attach-disc"><label class="fileName" data-newFileName="' + result.NewFileName + '">' + result.originalFileName + '</label><br /><input type="text" class="ImageDescription form-control" placeholder="Add Description Here"/></div><div class="col-lg-1 file-deleteicon"><i class="fa fa-trash-o"></i></div></div>';
                        var isEmpty = $("#contantBody").find('#AddApplicantBody').find("#filesList").html();
                        if (isEmpty = "") {
                            $('#AddApplicantBody').find("#filesList").html(string);
                        }
                        else {
                            $('#AddApplicantBody').find("#filesList").append(string);
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

$("#contantBody").on('click', '.deleteComment', function () {
    $(this).parent().parent().remove();
});

$("#contantBody").on('click', '.editComment', function () {
    $("#contantBody").find('#AddApplicantBody').find('#btnAddComment').hide();
    $("#contantBody").find('#AddApplicantBody').find('#btnEditComment').show();
    $("#contantBody").find('#AddApplicantBody').find('#btnEditComment').attr("data-editId", $(this).parent().parent().attr("id"));
    var htmlString = $(this).parent().parent().find(".sickComments").html();
    $("#contantBody").find('#AddApplicantBody').find('div#Applicantfroala-editor').froalaEditor('html.set', htmlString);
});

$("#contantBody").on('click', '#btnEditComment', function () {
    var editDiv = $(this).attr("data-editId");
    var data = $("#contantBody").find('#AddApplicantBody').find('div#Applicantfroala-editor').froalaEditor('html.get');
    $("#contantBody").find("#CommentList").find("#" + editDiv).find(".sickComments").html("");
    $("#contantBody").find("#CommentList").find("#" + editDiv).find(".sickComments").html(data);

    if ($('div#Applicantfroala-editor').data('froala.editor')) {
        $('div#Applicantfroala-editor').froalaEditor('destroy');
        $('div#Applicantfroala-editor').html('');
    }
    if (!$('div#Applicantfroala-editor').data('froala.editor')) {
        $('div#Applicantfroala-editor').froalaEditor({
            //toolbarButtons: ['bold', 'italic', 'underline', 'strikeThrough', 'subscript', 'superscript', 'fontFamily', 'fontSize', '|', 'specialCharacters', 'color', 'emoticons', 'inlineStyle', 'paragraphStyle', '|', 'paragraphFormat', 'align', 'formatOL', 'formatUL', 'outdent', 'indent', '-', 'quote', 'insertHR', 'insertLink', 'insertImage', 'insertVideo', 'insertFile', 'insertTable', '|', 'undo', 'redo', 'clearFormatting', 'selectAll', 'html', 'applyFormat', 'removeFormat', 'fullscreen', 'print', 'help'],
            toolbarButtons: ['bold', 'italic', 'underline', 'formatUL'],
            pluginsEnabled: null
        });
    }

    $("#contantBody").find('#btnAddComment').show();
    $("#contantBody").find('#btnEditComment').hide();
});

$("#page_content_inner").on('click', '#btnAddComment', function () {
    
    var data = $("#page_content_inner").find('#AddApplicantBody').find('div#Applicantfroala-editor').froalaEditor('html.get');
    var isEmpty = $("#page_content_inner").find("#CommentList").html().trim();
    var currentTime = getCurrentDateTime();
    if (isEmpty == "") {
        var appendDataString = '<div class="seccomments row" id="comment_1" data-commentid="0"><div class="col-lg-10"><p><span class="black postedby">' + constantTMS.userFullName + '</span> - <span class="commentTime">' + currentTime + '</sapn></p><div class="sickComments">' + data + '</div></div><div class="col-lg-2" style=""><i class="fa fa-pencil editComment"></i><i class="fa fa-trash-o deleteComment"></i></div></div>';
        $("#page_content_inner").find("#CommentList").html(appendDataString);
    }
    else {
        var lastCommentid = $("#page_content_inner").find(".seccomments:first").attr('id').split('_')[1];
        var newId = parseInt(lastCommentid) + 1;
        var appendDataString = '<div class="seccomments row" id="comment_' + newId + '" data-commentid="0"><div class="col-lg-10"><p><span class="black postedby">' + constantTMS.userFullName + '</span> - <span class="commentTime">' + currentTime + '</sapn></p><div class="sickComments">' + data + '</div></div><div class="col-lg-2" style=""><i class="fa fa-pencil editComment"></i><i class="fa fa-trash-o deleteComment"></i></div></div>';
        $("#page_content_inner").find("#CommentList").prepend(appendDataString);
    }

    if ($('div#Applicantfroala-editor').data('froala.editor')) {
        $('div#Applicantfroala-editor').froalaEditor('destroy');
        $('div#Applicantfroala-editor').html('');
    }
    if (!$('div#Applicantfroala-editor').data('froala.editor')) {
        $('div#Applicantfroala-editor').froalaEditor({
            toolbarButtons: ['bold', 'italic', 'underline', 'strikeThrough', 'subscript', 'superscript', 'fontFamily', 'fontSize', '|', 'specialCharacters', 'color', 'inlineStyle', 'paragraphStyle', '|', 'paragraphFormat', 'align', 'formatOL', 'formatUL', 'outdent', 'indent', '-', 'quote', 'insertHR', 'insertLink', 'insertImage', 'insertVideo', 'insertFile', 'insertTable', '|', 'undo', 'redo', 'clearFormatting', 'selectAll', 'html', 'applyFormat', 'removeFormat', 'fullscreen', 'print', 'help'],
           // toolbarButtons: ['bold', 'italic', 'underline', 'formatUL'],
            pluginsEnabled: null
        });
    }
});

//Edit Applicant Pool View 
$("#page_content_inner").find("#contantBody").on('click', '.btn-edit-Talent', function () {
    $(".hrtoolLoader").show();
    var id = $("#tableDiv").find("#TalentModalTable tbody").find('.selected').attr("id");
    $.ajax({
        url: constantTMS.EditApplicant,
        data: { Id: id },
        success: function (data) {
            $("#AddApplicantBody").html('');
            $('#AddApplicantBody').html(data);
            $('[data-toggle="tooltip"]').tooltip();
            $.each($("#page_content_inner").find('.sickComments'), function () {
                var uneacapeString = $(this).attr("data-commentstring");
                $(this).html(uneacapeString);
            });
            $("#contantBody").find('#ApplicantModel').find(".applicantTitle").text("");
            $("#contantBody").find('#ApplicantModel').find(".applicantTitle").text("Edit Applicant");
            //$("#ApplicantModel").find("#AddApplicantBody").find(".applicantTitle").text("Edit Applicant");
            $("#ApplicantModel").find("#AddApplicantBody").find('#wizard').smartWizard({
                onLeaveStep: onEditApplicantCallback,
                onFinish: onEditApplicantFinishCallback
            });
            $("#AddApplicantBody").find("#txt_DateOfBitrh").Zebra_DatePicker({
                //direction: false,
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {
                    validDOB();
                    //var date = $("#AddVacancyLogBody").find("txt_ClosingDate").val();
                }
            });
            $("#flagEdit").val(1);
            $.each($("#ApplicantModel").find("#AddApplicantBody").find(".Competency_Score"), function () {
                var id = $(this).attr("id");
                var $range = $(this);
                $(this).ionRangeSlider({
                    type: "single",
                    min: 0,
                    max: 5
                });

                $range.on("change", function () {
                    var $this = $(this),
                        value = $this.prop("value");
                    $("#" + id).val(value);
                });
            });

            $("#ApplicantModel").find('#AddApplicantBody').find('.buttonNext').addClass('btn btn-warning');
            $("#ApplicantModel").find('#AddApplicantBody').find('.buttonPrevious').addClass('btn btn-warning');
            $("#ApplicantModel").find('#AddApplicantBody').find('.buttonFinish').addClass('btn btn-success');

            $("#ApplicantModel").find('#AddApplicantBody').find('.buttonPrevious').hide();
            $("#ApplicantModel").find('#AddApplicantBody').find('.buttonFinish').hide();

            $("#ApplicantModel").find('#AddApplicantBody').find('div#Applicantfroala-editor').froalaEditor({
                toolbarButtons: ['bold', 'italic', 'underline', 'strikeThrough', 'subscript', 'superscript', 'fontFamily', 'fontSize', '|', 'specialCharacters', 'color', 'inlineStyle', 'paragraphStyle', '|', 'paragraphFormat', 'align', 'formatOL', 'formatUL', 'outdent', 'indent', '-', 'quote', 'insertHR', 'insertLink', 'insertImage', 'insertVideo', 'insertFile', 'insertTable', '|', 'undo', 'redo', 'clearFormatting', 'selectAll', 'html', 'applyFormat', 'removeFormat', 'fullscreen', 'print', 'help'],
               // toolbarButtons: ['bold', 'italic', 'underline', 'formatUL'],
                pluginsEnabled: null
            });
            $("#ApplicantModel").find('#AddApplicantBody').find("#drp-GeneralSkills").selectList();
            $("#ApplicantModel").find('#AddApplicantBody').find("#drp-TechnicalSkills").selectList();
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
        }
    });
});

function onEditApplicantCallback(obj, context) {
   
    if (context.fromStep == 1) {
        var isError = false;
        var SelectStepID = $("#drp-SelectStepID option:selected").text();
        if (SelectStepID == "Accepted") {
            AcceptedAddresource();
        }
        var VacancyID = $("#ApplicantModel").find("#AddApplicantBody").find("#ApplicantVacancyId").val();
        var FirstName = $("#ApplicantModel").find("#AddApplicantBody").find('#txt_FirstName').val().trim();
        var LastName = $("#ApplicantModel").find("#AddApplicantBody").find('#txt_LastName').val().trim();
        var Email = $("#ApplicantModel").find("#AddApplicantBody").find('#txt_Email').val();
        //if (Email != "") { var EmailValid = isValidEmailAddress(Email); if (!EmailValid) { isError = true;  $("#ApplicantModel").find("#AddApplicantBody").find("#lbl-error-EmailValid").show(); } }
        var GenderID = $("#ApplicantModel").find("#AddApplicantBody").find('#drp-selGender').val();
        var DateOfBitrh = $("#ApplicantModel").find("#AddApplicantBody").find('#txt_DateOfBitrh').val();
        if (FirstName == "") { isError = true;  $("#ApplicantModel").find("#AddApplicantBody").find("#lbl-error-FirstName").show(); }
        if (LastName == "") { isError = true;  $("#ApplicantModel").find("#AddApplicantBody").find("#lbl-error-LastName").show(); }
        if (Email == "") { isError = true;  $("#ApplicantModel").find("#AddApplicantBody").find("#lbl-error-Email").show(); }
        if (GenderID == "0") { isError = true;  $("#ApplicantModel").find("#AddApplicantBody").find("#lbl-error-GenderList").show(); }
        if (DateOfBitrh == "") { isError = true;  $("#ApplicantModel").find("#AddApplicantBody").find("#lbl-error-dateOfBirth").show(); }
        //var SelectStepID = $('#ApplicantModel').find('#drp-SelectStepID').Text;
       
        $.ajax({
            url: constantTMS.EmailCheck,
            data: { Id: 0, VacancyId: VacancyID },
            success: function (data) {
                if (data == "Error") {
                    isError = true;
                    $('#ApplicantModel').find("#AddApplicantBody").find("#lbl-error-EmailValid").show();

                }
                else {
                    $('#ApplyJobBody').find('.buttonPrevious').show();
                    $('#ApplyJobBody').find('.buttonFinish').show();

                    $('#ApplyJobBody').find("#lbl-error-FirstName").hide();
                    $('#ApplyJobBody').find("#lbl-Exits-LastName").hide();
                    $('#ApplyJobBody').find("#lbl-error-Email").hide();
                    $('#ApplyJobBody').find("#lbl-error-EmailValid").hide();
                    $('#ApplyJobBody').find("#lbl-error-GenderList").hide();
                    $('#ApplyJobBody').find("#lbl-error-dateOfBirth").hide();

                    return true;
                }
            }
        });

        //if (isError) {
        //    return false;
        //}
        if (isError) {
            return false;
        }
        else {
            if (context.toStep = 2) {
                $("#ApplicantModel").find("#AddApplicantBody").find('.buttonNext').show();
                $("#ApplicantModel").find("#AddApplicantBody").find('.buttonPrevious').show();
                $("#ApplicantModel").find("#AddApplicantBody").find('.buttonFinish').hide();
            }
            else {
                $("#ApplicantModel").find("#AddApplicantBody").find('.buttonNext').hide();
                $("#ApplicantModel").find("#AddApplicantBody").find('.buttonPrevious').show();
            }
            $("#ApplicantModel").find("#AddApplicantBody").find("#lbl-error-FirstName").hide();
            $("#ApplicantModel").find("#AddApplicantBody").find("#lbl-Exits-LastName").hide();
            $("#ApplicantModel").find("#AddApplicantBody").find("#lbl-error-Email").hide();
            $("#ApplicantModel").find("#AddApplicantBody").find("#lbl-error-EmailValid").hide();
            $("#ApplicantModel").find("#AddApplicantBody").find("#lbl-error-GenderList").hide();
            $("#ApplicantModel").find("#AddApplicantBody").find("#lbl-error-dateOfBirth").hide();

            return true;
        }
    }
    if (context.fromStep == 2) {
        var isError = false;
        var isError = false;
        var CoverLetterPathOriginal = $("#contantBody").find('#ApplicantModel').find('#CoverLetterName').val();
        var CoverLetterPath = $("#contantBody").find('#ApplicantModel').find('#txt_CoverLetterName').val();
        var DownloadApplicationFormLink = $("#contantBody").find('#ApplicantModel').find('#DownloadLink').attr('href');
        var UploadApplicationFormPathOriginal = $("#contantBody").find('#ApplicantModel').find('#UploadApplicationFormName').val();
        var UploadApplicationFormPath = $("#contantBody").find('#ApplicantModel').find('#txt_UploadApplicationFormName').val();
        var ResumePathOriginal = $("#contantBody").find('#ApplicantModel').find('#ResumePathName').val();
        var ResumePath = $("#contantBody").find('#ApplicantModel').find('#txt_ResumePathName').val();
        var Question1Answer = $("#contantBody").find('#ApplicantModel').find('#txt_Question1Answer').val();
        var Question2Answer = $("#contantBody").find('#ApplicantModel').find('#txt_Question2Answer').val();
        var Question3Answer = $("#contantBody").find('#ApplicantModel').find('#txt_Question3Answer').val();
        var Question4Answer = $("#contantBody").find('#ApplicantModel').find('#txt_Question4Answer').val();
        var Question5Answer = $("#contantBody").find('#ApplicantModel').find('#txt_Question5Answer').val();
        var IsUploadCoverLetter = $("#contantBody").find('#ApplicantModel').find('#hiddenIsCoverLetter').val();
        var IsUploadCVOrResume = $("#contantBody").find('#ApplicantModel').find('#hiddenIsCVOrResume').val();
        var hdnOuestion1On = $("#contantBody").find('#ApplicantModel').find("#hiddenIsQue1On").val();
        var hdnOuestion2On = $("#contantBody").find('#ApplicantModel').find("#hiddenIsQue2On").val();
        var hdnOuestion3On = $("#contantBody").find('#ApplicantModel').find("#hiddenIsQue3On").val();
        var hdnOuestion4On = $("#contantBody").find('#ApplicantModel').find("#hiddenIsQue4On").val();
        var hdnOuestion5On = $("#contantBody").find('#ApplicantModel').find("#hiddenIsQue5On").val();
        if (hdnOuestion1On == 1) {
            if (Question1Answer == "") {
                isError = true;
                $("#lbl-error-Question1Answer").show();
            }
        }
        if (hdnOuestion2On == 1) {
            if (Question2Answer == "") {
                isError = true;
                $("#lbl-error-Question2Answer").show();
            }
        }
        if (hdnOuestion3On == 1) {
            if (Question3Answer == "") {
                isError = true;
                $("#lbl-error-Question3Answer").show();
            }
        }
        if (hdnOuestion4On == 1) {
            if (Question4Answer == "") {
                isError = true;
                $("#lbl-error-Question4Answer").show();
            }
        }
        if (hdnOuestion5On == 1) {
            if (Question5Answer == "") {
                isError = true;
                $("#lbl-error-Question5Answer").show();
            }
        }
        if (IsUploadCoverLetter == 1) {
            if (CoverLetterPath == "") {
                isError = true;
                $("#lbl-error-CoverLetter").show();
            }
        }
        if (IsUploadCVOrResume == 1) {
            if (ResumePath == "") {
                isError = true;
                $("#lbl-error-ResumePathName").show();
            }
        }
        if (UploadApplicationFormPath == "") {
            isError = true;
            $("#lbl-error-UploadApplicationForm").show();
        }
        if (isError) {
            return false;
        }
        else {
            if (context.toStep == 3) {
                $("#ApplicantModel").find("#AddApplicantBody").find('.buttonNext').show();
                $("#ApplicantModel").find("#AddApplicantBody").find('.buttonPrevious').show();
                $("#ApplicantModel").find("#AddApplicantBody").find('.buttonFinish').hide();
            }
            else {
                $("#ApplicantModel").find("#AddApplicantBody").find('.buttonNext').show();
                $("#ApplicantModel").find("#AddApplicantBody").find('.buttonPrevious').show();
                // $("#ApplicantModel").find("#AddApplicantBody").find('.buttonFinish').show();
            }
            // $("#ApplicantModel").find("#AddApplicantBody").find("#lbl-error-JobDescription").hide();
            // $("#ApplicantModel").find("#AddApplicantBody").find("#lbl-error-HiringLeadList").hide();

            return true;

        }

    }
    if (context.fromStep == 3) {
        
        var isError = false;
        // var JobDescription = $("#contantBody").find('#AddVacancyLogBody').find('#froala-editor').val();
        //  var HiringLead = $("#contantBody").find('#AddVacancyLogBody').find('#drp-HiringLeadId').val();
        // if (JobDescription == "") { isError = true; $("#contantBody").find('#AddVacancyLogBody').find("#lbl-error-JobDescription").show(); }
        // if (HiringLead == "0") { isError = true; $("#contantBody").find('#AddVacancyLogBody').find("#lbl-error-HiringLeadList").show(); }
        if (isError) {
            return false;
        }
        else {
            if (context.toStep == 4) {
                $("#ApplicantModel").find("#AddApplicantBody").find('.buttonNext').show();
                $("#ApplicantModel").find("#AddApplicantBody").find('.buttonPrevious').show();
                $("#ApplicantModel").find("#AddApplicantBody").find('.buttonFinish').hide();
            }
            else {
                $("#ApplicantModel").find("#AddApplicantBody").find('.buttonNext').show();
                $("#ApplicantModel").find("#AddApplicantBody").find('.buttonPrevious').show();
                // $("#ApplicantModel").find("#AddApplicantBody").find('.buttonFinish').show();
            }
            // $("#ApplicantModel").find("#AddApplicantBody").find("#lbl-error-JobDescription").hide();
            // $("#ApplicantModel").find("#AddApplicantBody").find("#lbl-error-HiringLeadList").hide();

            return true;

        }

    }
    if (context.fromStep == 4) {
        
        var isError = false;
        // var JobDescription = $("#contantBody").find('#AddVacancyLogBody').find('#froala-editor').val();
        //  var HiringLead = $("#contantBody").find('#AddVacancyLogBody').find('#drp-HiringLeadId').val();
        // if (JobDescription == "") { isError = true; $("#contantBody").find('#AddVacancyLogBody').find("#lbl-error-JobDescription").show(); }
        // if (HiringLead == "0") { isError = true; $("#contantBody").find('#AddVacancyLogBody').find("#lbl-error-HiringLeadList").show(); }
        if (isError) {
            return false;
        }
        else {
            if (context.toStep == 5) {
                $("#ApplicantModel").find("#AddApplicantBody").find('.buttonNext').show();
                $("#ApplicantModel").find("#AddApplicantBody").find('.buttonPrevious').show();
                $("#ApplicantModel").find("#AddApplicantBody").find('.buttonFinish').hide();
            }
            else {
                $("#ApplicantModel").find("#AddApplicantBody").find('.buttonNext').show();
                $("#ApplicantModel").find("#AddApplicantBody").find('.buttonPrevious').show();
                // $("#ApplicantModel").find("#AddApplicantBody").find('.buttonFinish').show();
            }
            // $("#ApplicantModel").find("#AddApplicantBody").find("#lbl-error-JobDescription").hide();
            // $("#ApplicantModel").find("#AddApplicantBody").find("#lbl-error-HiringLeadList").hide();

            return true;

        }

    }
    if (context.fromStep == 5) {
        
        var isError = false;
        // var JobDescription = $("#contantBody").find('#AddVacancyLogBody').find('#froala-editor').val();
        //  var HiringLead = $("#contantBody").find('#AddVacancyLogBody').find('#drp-HiringLeadId').val();
        // if (JobDescription == "") { isError = true; $("#contantBody").find('#AddVacancyLogBody').find("#lbl-error-JobDescription").show(); }
        // if (HiringLead == "0") { isError = true; $("#contantBody").find('#AddVacancyLogBody').find("#lbl-error-HiringLeadList").show(); }
        if (isError) {
            return false;
        }
        else {
            if (context.toStep == 6) {
                $("#ApplicantModel").find("#AddApplicantBody").find('.buttonNext').show();
                $("#ApplicantModel").find("#AddApplicantBody").find('.buttonPrevious').show();
                $("#ApplicantModel").find("#AddApplicantBody").find('.buttonFinish').hide();
            }
            else {
                $("#ApplicantModel").find("#AddApplicantBody").find('.buttonNext').show();
                $("#ApplicantModel").find("#AddApplicantBody").find('.buttonPrevious').show();
                //  $("#ApplicantModel").find("#AddApplicantBody").find('.buttonFinish').show();
            }
            // $("#ApplicantModel").find("#AddApplicantBody").find("#lbl-error-JobDescription").hide();
            // $("#ApplicantModel").find("#AddApplicantBody").find("#lbl-error-HiringLeadList").hide();

            return true;

        }

    }
    if (context.fromStep == 6) {
        
        var isError = false;
        // var JobDescription = $("#contantBody").find('#AddVacancyLogBody').find('#froala-editor').val();
        //  var HiringLead = $("#contantBody").find('#AddVacancyLogBody').find('#drp-HiringLeadId').val();
        // if (JobDescription == "") { isError = true; $("#contantBody").find('#AddVacancyLogBody').find("#lbl-error-JobDescription").show(); }
        // if (HiringLead == "0") { isError = true; $("#contantBody").find('#AddVacancyLogBody').find("#lbl-error-HiringLeadList").show(); }
        if (isError) {
            return false;
        }
        else {
            if (context.toStep == 7) {
                $("#ApplicantModel").find("#AddApplicantBody").find('.buttonNext').hide();
                $("#ApplicantModel").find("#AddApplicantBody").find('.buttonPrevious').show();
                $("#ApplicantModel").find("#AddApplicantBody").find('.buttonFinish').show();
            }
            else {
                $("#ApplicantModel").find("#AddApplicantBody").find('.buttonNext').show();
                $("#ApplicantModel").find("#AddApplicantBody").find('.buttonPrevious').show();
                $("#ApplicantModel").find("#AddApplicantBody").find('.buttonFinish').hide();
            }
            // $("#ApplicantModel").find("#AddApplicantBody").find("#lbl-error-JobDescription").hide();
            // $("#ApplicantModel").find("#AddApplicantBody").find("#lbl-error-HiringLeadList").hide();

            return true;

        }

    }
    else {

        if (context.toStep == 6) {
            $("#ApplicantModel").find("#AddApplicantBody").find('.buttonNext').show();
            $("#ApplicantModel").find("#AddApplicantBody").find('.buttonPrevious').show();
            $("#ApplicantModel").find("#AddApplicantBody").find('.buttonFinish').hide();
        }
        else {
            $("#ApplicantModel").find("#AddApplicantBody").find('.buttonNext').show();
            $("#ApplicantModel").find("#AddApplicantBody").find('.buttonPrevious').hide();
            $("#ApplicantModel").find("#AddApplicantBody").find('.buttonFinish').hide();
        }
        return true;
    }
}

function onEditApplicantFinishCallback(obj, context) {  
    $(".hrtoolLoader").show();
    var Id = $("#contantBody").find('#ApplicantModel').find('#ApplicantHiddenId').val();
    var VacancyID = $("#contantBody").find('#ApplicantModel').find('#ApplicantVacancyId').val();
    var FirstName = $("#contantBody").find('#ApplicantModel').find('#txt_FirstName').val().trim();
    var LastName = $("#contantBody").find('#ApplicantModel').find('#txt_LastName').val().trim();
    var Email = $("#contantBody").find('#ApplicantModel').find('#txt_Email').val().trim();
    var GenderID = $("#contantBody").find('#ApplicantModel').find('.selectGender input:checked').val();
    var DateOfbirth = $("#contantBody").find("#ApplicantModel").find("#txt_DateOfBitrh").val();
    var PostalCode = $("#contantBody").find("#ApplicantModel").find("#txt_PostalCode").val();
    var Address = $("#contantBody").find("#ApplicantModel").find("#AddressText").val().trim();
    var OtherContactDetails = $("#contantBody").find("#ApplicantModel").find("#txt_OtherContactDetails").val().trim();
    var SelectStepID = $("#contantBody").find('#ApplicantModel').find('#drp-SelectStepID').val();
    var CoverLetterPathOriginal = $("#contantBody").find('#ApplicantModel').find('#CoverLetterName').val();
    //var CoverLetterPathOriginal = $("#contantBody").find('#ApplicantModel').find('#CoverLetterName')["0"].innerText;
    var CoverLetterPath = $("#contantBody").find('#ApplicantModel').find('#txt_CoverLetterName').val();
    var DownloadApplicationFormLink = $("#contantBody").find('#ApplicantModel').find('#DownloadLink').attr('href');
    var UploadApplicationFormPathOriginal = $("#contantBody").find('#ApplicantModel').find('#UploadApplicationFormName')["0"].innerText;
    var UploadApplicationFormPath = $("#contantBody").find('#ApplicantModel').find('#txt_UploadApplicationFormName').val();
//    var ResumePathOriginal = $("#contantBody").find('#ApplicantModel').find('#ResumePathName')["0"].innerText;
    var ResumePathOriginal = $("#contantBody").find('#ApplicantModel').find('#ResumePathName').val();

    var ResumePath = $("#contantBody").find('#ApplicantModel').find('#txt_ResumePathName').val();
    var Question1Answer = $("#contantBody").find('#ApplicantModel').find('#txt_Question1Answer').val();
    var Question2Answer = $("#contantBody").find('#ApplicantModel').find('#txt_Question2Answer').val();
    var Question3Answer = $("#contantBody").find('#ApplicantModel').find('#txt_Question3Answer').val();
    var Question4Answer = $("#contantBody").find('#ApplicantModel').find('#txt_Question4Answer').val();
    var Question5Answer = $("#contantBody").find('#ApplicantModel').find('#txt_Question5Answer').val();
    var SourceID = $("#contantBody").find('#ApplicantModel').find('#drp-SourceId').val();
    var flagEdit = $("#flagEdit").val();
    //CompetecyList_UL
    // var JobDescription = $("#contantBody").find('#AddVacancyLogBody').find('#froala-editor').froalaEditor('html.get');
    jsonCompetencySegmentObj = [];
    var id = 0;
    $.each($("#CompetecyList_UL").find("li"), function () {

        var competencyName = $(this).find(".Competency_NAME").val();
        var Score = $(this).find(".Competency_Score").val();
        id++;
        var oneData = {
            Id: id,
            CompetencyName: competencyName,
            Score: Score
        }
        jsonCompetencySegmentObj.push(oneData);
    });
    var AllCompetencySegmentJson = JSON.stringify(jsonCompetencySegmentObj);

    var commentList = [];
    $.each($("#contantBody").find('#CommentList').find(".seccomments"), function () {
        var commentBy = $(this).find(".postedby").html().trim();
        var comment = $(this).find(".sickComments").html().trim();
        var commentTime = $(this).find(".commentTime").html().trim();
        var newReord = {
            commentBy: commentBy,
            comment: comment,
            commentTime: commentTime
        }
        commentList.push(newReord);
    });
    var JsoncommentListJoinString = JSON.stringify(commentList);

    var documentList = [];
    var documentId = 0;
    $.each($("#contantBody").find('#filesList').find(".ListData"), function () {
        debugger;
        documentId = documentId + 1;
        var originalName = $(this).find(".fileName").html().trim();
        var newName = $(this).find(".fileName").attr("data-newfilename");
        var description = $(this).find(".ImageDescription").val();
        var docId = $("#doc_" + documentId).val();        
        var oneData = {
            originalName: originalName,
            newName: newName,
            description: description,
            Id: docId
        }                
        documentList.push(oneData);        
    });
    var JsondocumentListJoinString = JSON.stringify(documentList);

    GeneralSkills = [];
    $.each($("#contantBody").find("#drp-GeneralSkills").parent().find(".selectlist-item"), function () {
        GeneralSkills.push($("#contantBody").find("#drp-GeneralSkills").parent().find(".selectlist-select").find('option:contains(' + $(this).text().trim() + ')')[0].value);
    });
    TechnicalSkills = [];
    $.each($("#contantBody").find("#drp-TechnicalSkills").parent().find(".selectlist-item"), function () {
        TechnicalSkills.push($("#contantBody").find("#drp-TechnicalSkills").parent().find(".selectlist-select").find('option:contains(' + $(this).text().trim() + ')')[0].value);
    });
    var Cost = $("#contantBody").find('#ApplicantModel').find('#txt_Cost').val().trim();
    var RejectResnCmt = $("#rejectionComments").val();
    var RejectReasonId = $("#drp-RejectionReasonStepID").val();
    var model = {
        Id: Id,
        VacancyID: VacancyID,
        flagEdit: flagEdit,
        FirstName: FirstName,
        LastName: LastName,
        Email: Email,
        GenderID: GenderID,
        DateOfBirth: DateOfbirth,
        PostalCode: PostalCode,
        Address: Address,
        OtherContactDetails: OtherContactDetails,
        StatusID: SelectStepID,
        CoverLetterPathOriginal: CoverLetterPathOriginal,
        CoverLetterPath: CoverLetterPath,
        DownloadApplicationFormLink: DownloadApplicationFormLink,
        UploadApplicationFormPathOriginal: UploadApplicationFormPathOriginal,
        UploadApplicationFormPath: UploadApplicationFormPath,
        ResumePathOriginal: ResumePathOriginal,
        ResumePath: ResumePath,
        Question1Answer: Question1Answer,
        Question2Answer: Question2Answer,
        Question3Answer: Question3Answer,
        Question4Answer: Question4Answer,
        Question5Answer: Question5Answer,
        SourceID: SourceID,
        CompatencyJSV: AllCompetencySegmentJson,
        CommentJSV: JsoncommentListJoinString,
        DocumentJSV: JsondocumentListJoinString,
        GeneralSkillsJSV: GeneralSkills.join(','),
        TechnicalSkillsJSV: TechnicalSkills.join(','),
        Cost: Cost,
        RejectReasonId: RejectReasonId,
        RejectReasonComment: RejectResnCmt,        

    }
    $.ajax({
        url: constantTMS.SaveApplicant,
        type: 'POST',
        data: JSON.stringify(model),
        contentType: "application/json",
        success: function (data) {
            redirectToApplicantPage();
            $("#ApplicantModel").modal('hide');
            if (data == "Error") {
                isError = true;
                $("#contantBody").find('#AddVacancyLogBody').find("#lbl-Exist-Vacancy").show();
                $(".hrtoolLoader").hide();
                $(".modal-backdrop").hide();
            }
            if (flagEdit == 1)
            {
                $("#contantBody").html("");
                $("#contantBody").html(data);
                DataTable2Design();
            }
            //$("#contantBody").html("");
            //$("#contantBody").html(data);
            //$(".TMSIndex, .Vacancy, .Pool").removeClass('active');
            //$(".Applicant").addClass('active');
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
$("#page_content_inner").on("change", "#drp-SelectStepID", function () {
    var seletedStepId = $("#drp-SelectStepID").val();    
    if(seletedStepId==1)
    {
        $.ajax({
            url: constantTMS.RejectionReasonList,
            data: {},
            success: function (data) {                
                $.each(data, function (index, item) {
                    $("#drp-RejectionReasonStepID").append($("<option></option>").val(item.Value).html(item.Text));
                });
                $("#drp-RejectionReasonStepID").append($("<option></option>").val(0).html("+ Add New"));
                $("#div_RejectionReason").show();
                $("#div_RejectionComment").show();                
                //$("#drp-RejectionReasonStepID").enabled = true;
            }
        });
    }        
    else
    {
        $("#div_RejectionComment").hide();
        $("#div_RejectionReason").hide();
    }
  
})
$("#page_content_inner").on("change", "#drp-RejectionReasonStepID", function () {
    var drpdata = $("#drp-RejectionReasonStepID").val();
    if (drpdata == 0) {
         $.ajax({
             url: constantTMS.AddApplicantRejectionReason,
            data: {},
            success: function (data) {
                debugger;
                $("#ApplicantRejectReasonModel").find("#ApplicantRejectReasonModelBody").html('');
                $("#ApplicantRejectReasonModel").find("#ApplicantRejectReasonModelBody").html(data);
                $("#ApplicantRejectReasonModel").modal('show');

            }
        })
    }
})

$("#page_content_inner").on('click', "#btn-submit-RejectReason", function () {
    var reasonName = $("#page_content_inner").find("#txt_RejectReasonName").val();
    var UserId = $("#UserId").val();
    var isError = false;
    if(reasonName=="")
    {
        isError = true;
        $("#lbl-error-RejectReasonName").show();
    }
    if(!isError)
    {
        $.ajax({
            url: constantTMS.AddRejectReason,
            data: { Name: reasonName },
            success: function (data) {
                $("#ApplicantRejectReasonModel").modal('close');
                $(".toast-success").show();
                setTimeout(function () { $(".toast-success").hide(); }, 1500);
            }
        })
    }

})
//Edit Applicant
function EditApplicants(Id) {
    $(".hrtoolLoader").show();
    $.ajax({
        url: constantTMS.EditApplicant,
        data: { Id: Id },
        success: function (data) {
            
            $("#AddApplicantBody").html('');
            $('#AddApplicantBody').html(data);

            $('[data-toggle="tooltip"]').tooltip();

            $.each($("#page_content_inner").find('.sickComments'), function () {
                var uneacapeString = $(this).attr("data-commentstring");
                $(this).html(uneacapeString);
            });
            $("#contantBody").find('#ApplicantModel').find(".applicantTitle").text("");
            $("#contantBody").find('#ApplicantModel').find(".applicantTitle").text("Edit Applicant");
            //$("#ApplicantModel").find("#AddApplicantBody").find(".applicantTitle").text("Edit Applicant");
            $("#ApplicantModel").find("#AddApplicantBody").find('#wizard').smartWizard({
                onLeaveStep: onEditApplicantCallback,
                onFinish: onEditApplicantFinishCallback
            });
            $("#AddApplicantBody").find("#txt_DateOfBitrh").Zebra_DatePicker({
                //direction: false,
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {
                    validDOB();
                    //var date = $("#AddVacancyLogBody").find("txt_ClosingDate").val();
                }
            });

            $.each($("#ApplicantModel").find("#AddApplicantBody").find(".Competency_Score"), function () {
                var id = $(this).attr("id");
                var $range = $(this);
                $(this).ionRangeSlider({
                    type: "single",
                    min: 0,
                    max: 5
                });

                $range.on("change", function () {
                    var $this = $(this),
                        value = $this.prop("value");
                    $("#" + id).val(value);
                });
            });

            $("#ApplicantModel").find('#AddApplicantBody').find('.buttonNext').addClass('btn btn-warning');
            $("#ApplicantModel").find('#AddApplicantBody').find('.buttonPrevious').addClass('btn btn-warning');
            $("#ApplicantModel").find('#AddApplicantBody').find('.buttonFinish').addClass('btn btn-success');

            $("#ApplicantModel").find('#AddApplicantBody').find('.buttonPrevious').hide();
            $("#ApplicantModel").find('#AddApplicantBody').find('.buttonFinish').hide();

            $("#ApplicantModel").find('#AddApplicantBody').find('div#Applicantfroala-editor').froalaEditor({
                //toolbarButtons: ['bold', 'italic', 'underline', 'strikeThrough', 'subscript', 'superscript', 'fontFamily', 'fontSize', '|', 'specialCharacters', 'color', 'emoticons', 'inlineStyle', 'paragraphStyle', '|', 'paragraphFormat', 'align', 'formatOL', 'formatUL', 'outdent', 'indent', '-', 'quote', 'insertHR', 'insertLink', 'insertImage', 'insertVideo', 'insertFile', 'insertTable', '|', 'undo', 'redo', 'clearFormatting', 'selectAll', 'html', 'applyFormat', 'removeFormat', 'fullscreen', 'print', 'help'],
                toolbarButtons: ['bold', 'italic', 'underline', 'formatUL'],
                pluginsEnabled: null
            });
            $("#ApplicantModel").find('#AddApplicantBody').find("#drp-GeneralSkills").selectList();
            $("#ApplicantModel").find('#AddApplicantBody').find("#drp-TechnicalSkills").selectList();
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
        }
    });
}
//Delete Applicant
function deleteApplicantHandler(Id) {
    
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
                        url: constantTMS.DeleteApplicant,
                        data: { Id: Id },
                        success: function (data) {                            
                            //$("#contantBody").html("");
                            //$("#contantBody").html(data);
                            redirectToApplicantPage();
                            $(".TMSIndex, .Vacancy, .Pool").removeClass('active');
                            $(".Applicant").addClass('active');
                            $(".toast-error").show();
                            setTimeout(function () { $(".toast-error").hide(); }, 1500);
                            DataTableDesign();
                            $(".hrtoolLoader").hide();
                            $(".modal-backdrop").hide();
                        }
                    });
                }
            }]
    });
}

//Toggle Click
$("#page_content_inner").on('click', '.OpenToggle', function () {
    
    $(this).parent().parent().find(".performance-grid, .CloseToggle").removeClass("hide");
    $(this).parent().find(".OpenToggle").addClass('hide');
});

$("#page_content_inner").on('click', '.CloseToggle', function () {
    
    $(this).parent().parent().find(".performance-grid, .CloseToggle").addClass('hide');
    $(this).parent().find(".OpenToggle").removeClass('hide');
});

//moveAllReject
$("#page_content_inner").on('click', '.btn-move-AllReject', function () {
    
    $(".hrtoolLoader").show();
    var Id = $(this).attr('id');
    var VacancyId = $("#contantBody").find("#drp-SelectVacancyId")["0"].value;
    var model = {
        StepID: Id,
        VacancyID: VacancyId
    }
    $.ajax({
        url: constantTMS.RejectStepMove,
        type: 'POST',
        data: JSON.stringify(model),
        contentType: "application/json",
        success: function (data) {
            $("#contantBody").html('');
            $('#contantBody').html(data);
            var updateOutput = function (e) {
                var list = e.length ? e : $(e.target),
                        output = list.data('output');
                if (window.JSON) {
                    // output.val(window.JSON.stringify(list.nestable('serialize')));//, null, 2));
                }
                else {
                    output.val('JSON browser support required for this demo.');
                }
            };
            $.each($('.ApplicantNestable'), function () {
                $(this).nestable({
                    maxDepth: 1,
                    group: 1,
                    callback: function (l, e) {
                        
                        var stepId = $(l).attr("data-stepId");
                        var stepName = $(l).attr("data-stepName");
                        var applicantId = $(e).attr("data-applicantId");
                        var VacancyId = $("#contantBody").find("#drp-SelectVacancyId")["0"].value;
                        var model = {

                            StepID: stepId,
                            StepName: stepName,
                            ApplicantID: applicantId,
                            VacancyID: VacancyId

                        }

                        $.ajax({
                            url: constantTMS.StepMove,
                            type: 'POST',
                            data: JSON.stringify(model),
                            contentType: "application/json",
                            success: function (data) {
                            }
                        });

                        // l is the main container 
                        // e is the element that was moved 
                    }

                });
            });
            function changeTable() {
            }
            updateOutput($('.ApplicantNestable').data('output', $('.ApplicantNestable-output')));
            //$("#contantBody").find(".vacancyProcess").html(data.RecruitmentProcess);
            //$("#contantBody").find(".vacancyStatus").text(data.Status);
            //$("#contantBody").find(".vacancyCloseDate").text(data.ClosingDate);
            //$("#contantBody").find(".vacancyDescription").text(data.Summary);
            $("#contantBody").find("#VacancyDetailsShow").removeClass("hide");
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
        }
    });
});
//move To Talent 
$("#page_content_inner").on('click', '.btn-move-AllTalent', function () {
    $(".hrtoolLoader").show();
    var Id = $(this).attr('id');
    var VacancyId = $("#contantBody").find("#drp-SelectVacancyId")["0"].value;
    var model = {
        StepID: Id,
        VacancyID: VacancyId
    }
    $.ajax({
        url: constantTMS.TalentStepMove,
        type: 'POST',
        data: JSON.stringify(model),
        contentType: "application/json",
        success: function (data) {
            $("#contantBody").html('');
            $('#contantBody').html(data);
            var updateOutput = function (e) {
                var list = e.length ? e : $(e.target),
                        output = list.data('output');
                if (window.JSON) {
                    output.val(window.JSON.stringify(list.nestable('serialize')));//, null, 2));
                }
                else {
                    output.val('JSON browser support required for this demo.');
                }
            };
            $.each($('.ApplicantNestable'), function () {
                $(this).nestable({
                    maxDepth: 1,
                    group: 1,
                    callback: function (l, e) {
                        
                        var stepId = $(l).attr("data-stepId");
                        var stepName = $(l).attr("data-stepName");
                        var applicantId = $(e).attr("data-applicantId");
                        var VacancyId = $("#contantBody").find("#drp-SelectVacancyId")["0"].value;
                        var model = {

                            StepID: stepId,
                            StepName: stepName,
                            ApplicantID: applicantId,
                            VacancyID: VacancyId

                        }

                        $.ajax({
                            url: constantTMS.StepMove,
                            type: 'POST',
                            data: JSON.stringify(model),
                            contentType: "application/json",
                            success: function (data) {
                            }
                        });

                        // l is the main container 
                        // e is the element that was moved 
                    }

                });
            });
            function changeTable() {
            }
            updateOutput($('.ApplicantNestable').data('output', $('.ApplicantNestable-output')));
            //$("#contantBody").find(".vacancyProcess").html(data.RecruitmentProcess);
            //$("#contantBody").find(".vacancyStatus").text(data.Status);
            //$("#contantBody").find(".vacancyCloseDate").text(data.ClosingDate);
            //$("#contantBody").find(".vacancyDescription").text(data.Summary);
            $("#contantBody").find("#VacancyDetailsShow").removeClass("hide");
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
        }
    });
});

function btnClose() {
    $(".hrtoolLoader").show();
    var value = $("#Vacancy_ID").val();
    $.ajax({
        url: constantTMS.SelectVacancy,
        data: { Id: value },
        success: function (data) {
            $("#contantBody").html('');
            $('#contantBody').html(data);
            var updateOutput = function (e) {
                var list = e.length ? e : $(e.target),
                        output = list.data('output');
                if (window.JSON) {
                    output.val(window.JSON.stringify(list.nestable('serialize')));//, null, 2));
                }
                else {
                    output.val('JSON browser support required for this demo.');
                }
            };
            $.each($('.ApplicantNestable'), function () {
                $(this).nestable({
                    maxDepth: 1,
                    group: 1,
                    callback: function (l, e) {
                        
                        var stepId = $(l).attr("data-stepId");
                        var stepName = $(l).attr("data-stepName");
                        var applicantId = $(e).attr("data-applicantId");
                        var VacancyId = $("#contantBody").find("#drp-SelectVacancyId")["0"].value;
                        var model = {
                            StepID: stepId,
                            StepName: stepName,
                            ApplicantID: applicantId,
                            VacancyID: VacancyId
                        }
                        $.ajax({
                            url: constantTMS.StepMove,
                            type: 'POST',
                            data: JSON.stringify(model),
                            contentType: "application/json",
                            success: function (data) {
                                
                                if (data.isRedirect == undefined) {
                                    BindData(data);
                                }
                                else {

                                    $('#AddResoure').modal('show');
                                    $.ajax({
                                        url: constantTMS.EditResource,
                                        data: { Id: data.redirectUrl.RouteValues[2].Value, VacancyID: data.redirectUrl.RouteValues[3].Value },
                                        success: function (data) {
                                            $('#AddResoureceBody').html('');
                                            $('#AddResoureceBody').html(data);
                                            $("#btn-submit-Resoure").html("Add");
                                            $('[data-toggle="tooltip"]').tooltip();
                                            $('#wizard').smartWizard({
                                                onLeaveStep: leaveAStepCallback,
                                                onFinish: onFinishCallback
                                            });
                                            $('#AddResoureceBody').find('.buttonNext').addClass('btn btn-warning');
                                            $('#AddResoureceBody').find('.buttonPrevious').addClass('btn btn-warning');
                                            $('#AddResoureceBody').find('.buttonFinish').addClass('btn btn-success');
                                            $('#AddResoureceBody').find('.buttonPrevious').hide();
                                            $('#AddResoureceBody').find('.buttonFinish').hide();

                                            $("#page_content").find("#DateOfBirth").Zebra_DatePicker({
                                                showButtonPanel: false,
                                                format: 'd-m-Y',
                                                onSelect: function () {
                                                    $("#page_content").find('#ValidDateOfBirth').hide();
                                                    validDOB(dob);
                                                }
                                            });
                                            $("#page_content").find("#StartDate").Zebra_DatePicker({
                                                showButtonPanel: false,
                                                format: 'd-m-Y',
                                                onSelect: function () {                                                    
                                                    $("#page_content").find('#ValidStartDate').hide();
                                                    var startDate = $("#StartDate").val();
                                                    var proEndDate = $("#ProbationEndDate").val();
                                                    calculateprobEndDate(startDate, proEndDate);
                                                    var proEndFixDate = $("#FixedTermEndDate").val();
                                                    calculateFixedTermEdDate(startDate, proEndFixDate);
                                                }
                                            });
                                            $("#page_content").find("#ProbationEndDate").Zebra_DatePicker({
                                                showButtonPanel: false,
                                                format: 'd-m-Y',
                                                onSelect: function () {
                                                    $("#page_content").find('#ValidProbationEndDate').hide();
                                                }
                                            });
                                            $("#page_content").find("#NextProbationReviewDate").Zebra_DatePicker({
                                                showButtonPanel: false,
                                                format: 'd-m-Y',
                                                onSelect: function () {
                                                    $("#page_content").find("#ValidNextProbationReviewDate").hide();
                                                    $("#ValidProbationEndDate").hide();
                                                    $("#lbl-error-GreaterEndDate").hide();
                                                    var stDate = $("#StartDate").val();
                                                    var startdate = $("#page_content").find("#ProbationEndDate").val();
                                                    var enddate = $("#page_content").find("#NextProbationReviewDate").val();
                                                    calculateDateDiff(startdate, enddate);
                                                    calculateprobEndDate(stDate, startdate);
                                                }
                                            });

                                            $("#page_content").find("#FixedTermEndDate").Zebra_DatePicker({
                                                showButtonPanel: false,
                                                format: 'd-m-Y',
                                                onSelect: function () {
                                                    $("#page_content").find('#ValidFixedTermEndDate').hide();                                                    
                                                    $("#page_content").find("#lbl-error-ValidFDEndDate").hide();
                                                    var startDate = $("#StartDate").val();
                                                    var proEndDate = $("#FixedTermEndDate").val();
                                                    calculateFixedTermEdDate(startDate, proEndDate);
                                                }
                                            });
                                        }
                                    });

                                    $(".hrtoolLoader").hide();
                                    $(".modal-backdrop").hide();
                                }

                            }
                        });

                        // l is the main container 
                        // e is the element that was moved 
                    }



                });
            });
            function changeTable() {
            }
            updateOutput($('.ApplicantNestable').data('output', $('.ApplicantNestable-output')));
            //$("#contantBody").find(".vacancyProcess").html(data.RecruitmentProcess);
            //$("#contantBody").find(".vacancyStatus").text(data.Status);
            //$("#contantBody").find(".vacancyCloseDate").text(data.ClosingDate);
            //$("#contantBody").find(".vacancyDescription").text(data.Summary);
            $("#contantBody").find("#VacancyDetailsShow").removeClass("hide");
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();

        }
    });
}

function BindData(data) {
    
    $(".hrtoolLoader").show();
    $("#contantBody").html('');
    $('#contantBody').html(data);
    $("#TMSTabPanel").find(".TMSIndex, .Applicant, .Pool").removeClass('active');
    $("#TMSTabPanel").find(".Vacancy").addClass('active');
    //$("#drp-SelectVacancyId").find('option').removeAttr("selected");

    var updateOutput = function (e) {
        var list = e.length ? e : $(e.target),
                output = list.data('output');
        if (window.JSON) {
            output.val(window.JSON.stringify(list.nestable('serialize')));//, null, 2));
        }
        else {
            output.val('JSON browser support required for this demo.');
        }
    };
    $.each($('.ApplicantNestable'), function () {
        $(this).nestable({
            maxDepth: 1,
            group: 1,
            callback: function (l, e) {
                
                var stepId = $(l).attr("data-stepId");
                var stepName = $(l).attr("data-stepName");
                var applicantId = $(e).attr("data-applicantId");
                var VacancyId = $("#contantBody").find("#drp-SelectVacancyId")["0"].value;
                var model = {
                    StepID: stepId,
                    StepName: stepName,
                    ApplicantID: applicantId,
                    VacancyID: VacancyId
                }
                $.ajax({
                    url: constantTMS.StepMove,
                    type: 'POST',
                    data: JSON.stringify(model),
                    contentType: "application/json",
                    success: function (data) {
                        
                        if (data.isRedirect == undefined) {
                            BindData(data);
                        }
                        else {
                            
                            $('#AddResoure').modal('show');
                            $.ajax({
                                url: constantTMS.EditResource,
                                data: { Id: data.redirectUrl.RouteValues[2].Value, VacancyID: data.redirectUrl.RouteValues[3].Value },
                                success: function (data) {
                                    $("#TMSTabPanel").find(".TMSIndex, .Applicant, .Pool").removeClass('active');
                                    $("#TMSTabPanel").find(".Vacancy").addClass('active');
                                    $('#AddResoureceBody').html('');
                                    $('#AddResoureceBody').html(data);
                                    $("#btn-submit-Resoure").html("Add");
                                    $('[data-toggle="tooltip"]').tooltip();
                                    $('#wizard').smartWizard({
                                        onLeaveStep: leaveAStepCallback,
                                        onFinish: onFinishCallback
                                    });
                                    $('#AddResoureceBody').find('.buttonNext').addClass('btn btn-warning');
                                    $('#AddResoureceBody').find('.buttonPrevious').addClass('btn btn-warning');
                                    $('#AddResoureceBody').find('.buttonFinish').addClass('btn btn-success');
                                    $('#AddResoureceBody').find('.buttonPrevious').hide();
                                    $('#AddResoureceBody').find('.buttonFinish').hide();

                                    $("#page_content").find("#DateOfBirth").Zebra_DatePicker({
                                        showButtonPanel: false,
                                        format: 'd-m-Y',
                                        onSelect: function () {
                                            $("#page_content").find('#ValidDateOfBirth').hide();
                                            validDOB(dob);
                                        }
                                    });
                                    $("#page_content").find("#StartDate").Zebra_DatePicker({
                                        showButtonPanel: false,
                                        format: 'd-m-Y',
                                        onSelect: function () {
                                            $("#page_content").find('#ValidStartDate').hide();
                                            var startDate = $("#StartDate").val();
                                            var proEndDate = $("#ProbationEndDate").val();
                                            calculateprobEndDate(startDate, proEndDate);
                                            var proEndFixDate = $("#FixedTermEndDate").val();
                                            calculateFixedTermEdDate(startDate, proEndFixDate);
                                        }
                                    });
                                    $("#page_content").find("#ProbationEndDate").Zebra_DatePicker({
                                        showButtonPanel: false,
                                        format: 'd-m-Y',
                                        onSelect: function () {
                                            $("#page_content").find('#ValidProbationEndDate').hide();
                                        }
                                    });
                                    $("#page_content").find("#NextProbationReviewDate").Zebra_DatePicker({
                                        showButtonPanel: false,
                                        format: 'd-m-Y',
                                        onSelect: function () {                                            
                                            $("#page_content").find("#ValidNextProbationReviewDate").hide();
                                             $("#ValidProbationEndDate").hide();
                                            $("#lbl-error-GreaterEndDate").hide();
                                            var stDate = $("#StartDate").val();
                                            var startdate = $("#page_content").find("#ProbationEndDate").val();
                                            var enddate = $("#page_content").find("#NextProbationReviewDate").val();
                                            calculateDateDiff(startdate, enddate);
                                            calculateprobEndDate(stDate, startdate);
                                        }
                                    });

                                    $("#page_content").find("#FixedTermEndDate").Zebra_DatePicker({
                                        showButtonPanel: false,
                                        format: 'd-m-Y',
                                        onSelect: function () {
                                            $("#page_content").find('#ValidFixedTermEndDate').hide();
                                            $("#page_content").find("#lbl-error-ValidFDEndDate").hide();
                                            var startDate = $("#StartDate").val();
                                            var proEndDate = $("#FixedTermEndDate").val();
                                            calculateFixedTermEdDate(startDate, proEndDate);
                                        }
                                    });
                                }
                            });


                            $(".hrtoolLoader").hide();
                            $(".modal-backdrop").hide();
                        }

                    }
                });
            }
        });
    });
    function changeTable() {
    }
    updateOutput($('.ApplicantNestable').data('output', $('.ApplicantNestable-output')));
    //$("#contantBody").find(".vacancyProcess").html(data.RecruitmentProcess);
    //$("#contantBody").find(".vacancyStatus").text(data.Status);
    //$("#contantBody").find(".vacancyCloseDate").text(data.ClosingDate);
    //$("#contantBody").find(".vacancyDescription").text(data.Summary);
    $("#contantBody").find("#VacancyDetailsShow").removeClass("hide");
    $(".hrtoolLoader").hide();
    $(".modal-backdrop").hide();
}

$("#page_content_inner").on('click', '#FindAddress', function () {
    
    $(".hrtoolLoader").show();
    var isError = false;
    // var hiddenId = $("#tableDivTraining").find("#hidden-Id").val();
    var HouseNumber = $("#AddApplicantBody").find("#txt_HouseNumber").val();
    var postCode = $("#AddApplicantBody").find("#txt_PostalCode").val();

    if (postCode == "") {
        isError = true;
        $("#AddApplicantBody").find("#lbl-error-PostCode").show();
        $(".hrtoolLoader").hide();
        $(".modal-backdrop").hide();
    }
    else {

        $.post('http://maps.googleapis.com/maps/api/geocode/json?address=' + postCode + '&sensor=false', function (r) {
            
            var lat = r['results'][0]['geometry']['location']['lat'];
            var lng = r['results'][0]['geometry']['location']['lng'];
            $.post('http://maps.googleapis.com/maps/api/geocode/json?latlng=' + lat + ',' + lng + '&sensor=false', function (address) {
                
                $("#AddApplicantBody").find("#AddressText").val(HouseNumber + ", " + address['results'][0]['address_components'][1]['long_name'] + "\n" + address['results'][0]['address_components'][2]['long_name'] + "\n" + address['results'][0]['address_components'][4]['long_name'] + "\n" + address['results'][0]['address_components'][5]['long_name'] + "\n" + address['results'][0]['address_components'][6]['long_name'] + " " + address['results'][0]['address_components'][7]['long_name']);
            });
        });
        $(".hrtoolLoader").hide();
        $(".modal-backdrop").hide();
    }
});

$('#AddApplicantBody').on('keyup', '#txt_PostalCode', function () {
    var isError = false;
    $("#AddApplicantBody").find("#lbl-error-PostCode").hide();
});


$("#page_content_inner").on('click', "#removeCoverLetter", function () {
    document.getElementById('CoverLetterName').innerHTML = '';
    $("#txt_CoverLetterName").val('');
})
$("#page_content_inner").on('click', "#removeApplicationForm", function () {
    document.getElementById('UploadApplicationFormName').innerHTML = '';
    $("#txt_UploadApplicationFormName").val('');
})
$("#page_content_inner").on('click', "#removeUploadResumeCV", function () {
    document.getElementById('ResumePathName').innerHTML = '';
    $("#txt_ResumePathName").val('');
})

$("#page_content_inner").on('change', "#drp-RecriProcesId", function () {
    var selected =[];
    debugger;
    $('#TalentModalTable tbody tr').each(function () {
        //selected.push({
        //    JsonObject1: $(this).closest('tr').attr('id'),

        //});
        selected.push($(this).closest('tr').attr('id'));

    });
    var sdata = selected.join();
    var seleProcessId = $("#drp-RecriProcesId").val();
    $.ajax({
        url: constantTMS.QuickFilterByTalentPool,
        type: 'POST',
        data: { seleProcessId: seleProcessId ,seleId:sdata},        
        success: function (data) {
            $("#contantBody").html('');
            $('#contantBody').html(data);
            DataTable2Design();
            $(".TMSIndex, .Vacancy, .Applicant").removeClass('active');
            $(".Pool").addClass('active');
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
        }
    });
})


$('#page_content').on('dblclick', ' .dataTr', function (event) {
    var id = $("#contantBody").find("#VacancyModalTable tbody").find('.selected').attr("id");
    $.ajax({
        url: constantTMS.AddVacancy,
        data: { Id: id },
        success: function (data) {
            $("#AddVacancyLogBody").html('');
            $('#AddVacancyLogBody').html(data);
            $("#VacancyModal").modal('show');
            $('[data-toggle="tooltip"]').tooltip();
            $("#contantBody").find('#VacancyModal').find(".vacancyTitle").text("Edit Vacancy");
            $("#contantBody").find('#AddVacancyLogBody').find('#wizard').smartWizard({                
               noForwardJumping: false,
                onLeaveStep: onVacanyDetailsCallback,
                onFinish: onFinishVacancyCallback,
            });
            $("#AddVacancyLogBody").find("#txt_ClosingDate").Zebra_DatePicker({
                //direction: false,
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {
                    //var date = $("#AddVacancyLogBody").find("txt_ClosingDate").val();
                }
            });

            $("#contantBody").find('#AddVacancyLogBody').find('.buttonNext').addClass('btn btn-warning');
            $("#contantBody").find('#AddVacancyLogBody').find('.buttonPrevious').addClass('btn btn-warning');
            $("#contantBody").find('#AddVacancyLogBody').find('.buttonFinish').addClass('btn btn-success');

            $("#contantBody").find('#AddVacancyLogBody').find('.buttonPrevious').hide();
            $("#contantBody").find('#AddVacancyLogBody').find('.buttonFinish').hide();

            $("#contantBody").find('#AddVacancyLogBody').find('div#froala-editor').froalaEditor({
                toolbarButtons: ['bold', 'italic', 'underline', 'strikeThrough', 'subscript', 'superscript', 'fontFamily', 'fontSize', '|', 'specialCharacters', 'color','inlineStyle', 'paragraphStyle', '|', 'paragraphFormat', 'align', 'formatOL', 'formatUL', 'outdent', 'indent', '-', 'quote', 'insertHR', 'insertLink', 'insertImage', 'insertVideo', 'insertFile', 'insertTable', '|', 'undo', 'redo', 'clearFormatting', 'selectAll', 'html', 'applyFormat', 'removeFormat', 'fullscreen', 'print', 'help'],
                //toolbarButtons: ['bold', 'italic', 'underline', 'formatUL'],
                pluginsEnabled: null
            });

            var job = $("#contantBody").find('#AddVacancyLogBody').find('#txt_JobDescription').val();
            $("#contantBody").find('#AddVacancyLogBody').find('#froala-editor').froalaEditor('html.set', job);
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
        }
    });
});
//function deleteDoc(res)
//{    
//    $.ajax({
//        url: constantTMS.deleteDoc,
//        data: { Id: res},
//        success: function (data) {

//        }          
//}
$("#drpRecProcessList").on("change", function () {
    var RecId = $("#drpRecProcessList").val();
    var FunctionId = $("#drpTMSFunction").val();
    var PoolId = $("#drpTMSPool").val();
    var BusiId = $("#drpTMSBusiness").val();
    var DivId = $("#drpTMSDivision").val();
    $.ajax({
        url: constantTMS.constantFilterPi,
        data: { RecId: RecId, BusinessId: BusiId, DivisionId: DivId, PoolId: PoolId, FunctionId: FunctionId },
        success: function (data) {
            debugger
            drawchart(data);
        }
    })
})
$("#page_content").on("change", "#drpTMSBusiness", function () {
    var value = $(this).val();
    if (value != "0") {
        $.ajax({
            url: constantTMS.filDiv,
            data: { businessId: value },
            success: function (data) {
                $("#drpTMSDivision").html('');
                var toAppend = '';
                toAppend += "<option value='0'>--Select--</option>";
                $.each(data, function (index, item) {
                    toAppend += "<option value='" + item.Id + "'>" + item.Name + "</option>";
                })
                $("#drpTMSDivision").html(toAppend);
                var RecId = $("#drpRecProcessList").val();
                if ($("#drpTMSDivision").val() == 0) {
                    $("#drpTMSDivision").val(0);
                    $('#drpTMSPool').val(0);
                    $('#drpTMSFunction').val(0);
                }
                var eid = $("#selectID").val();
                $.ajax({
                    url: constantTMS.constantFilterPi,
                    data: { RecId: RecId, businessId: value },
                    success: function (data) {
                    debugger
                    drawchart(data);
                    }
                })
            }
        });
    }
    else {
        $('#drpTMSDivision').empty();
        // Bind new values to dropdown
        $('#drpTMSDivision').each(function () {
            // Create option
            var option = $("<option />");
            option.attr("value", '0').text('--Select--');
            $('#drpTMSDivision').append(option);
        });
        $('#drpTMSPool').empty();
        // Bind new values to dropdown
        $('#drpTMSPool').each(function () {
            // Create option
            var option = $("<option />");
            option.attr("value", '0').text('--Select--');
            $('#drpTMSPool').append(option);
        });
        $('#drpTMSFunction').empty();
        // Bind new values to dropdown
        $('#drpTMSFunction').each(function () {
            // Create option
            var option = $("<option />");
            option.attr("value", '0').text('--Select--');
            $('#drpTMSFunction').append(option);
        });


    }
});

$("#page_content").on("change", "#drpTMSDivision", function () {
    var value = $(this).val();
    var RecId = $("#drpRecProcessList").val();
    if (value != "0") {
        $.ajax({
            url: constantTMS.getPool,
            data: { divisionId: value },
            success: function (data) {
                $("#drpTMSPool").html('');
                var toAppend = '';
                toAppend += "<option value='0'>--Select--</option>";
                $.each(data, function (index, item) {
                    toAppend += "<option value='" + item.Id + "'>" + item.Name + "</option>";
                })
                $("#drpTMSPool").html(toAppend);
                if ($("#drpTMSPool").val() == 0) {
                    $("#drpTMSPool").val(0);
                }
                $.ajax({
                    url: constantTMS.getfunction,
                    data: { divisionId: value },
                    success: function (data) {
                        $("#drpTMSFunction").html('');
                        var toAppend = '';
                        toAppend += "<option value='0'>--Select--</option>";
                        $.each(data, function (index, item) {
                            toAppend += "<option value='" + item.Id + "'>" + item.Name + "</option>";
                        })
                        $("#drpTMSFunction").html(toAppend);
                        if ($("#drpTMSFunction").val() == 0) {
                            $("#drpTMSFunction").val(0);
                        }
                        var FunctionId = $("#drpTMSFunction").val();
                        var PoolId = $("#drpTMSPool").val();
                        var BusiId = $("#drpTMSBusiness").val();
                        var eid = $("#selectID").val();
                        $.ajax({
                            url: constantTMS.constantFilterPi,
                            data: { RecId: RecId, BusinessId: BusiId, DivisionId: value, PoolId: PoolId, FunctionId: FunctionId },
                            success: function (data) {
                                debugger
                                drawchart(data);
                            }
                        })
                    }
                });
            }
        });
    }
    else {

        $('#drpTMSPool').empty();
        // Bind new values to dropdown
        $('#drpTMSPool').each(function () {
            // Create option
            var option = $("<option />");
            option.attr("value", '0').text('--Select--');
            $('#drpTMSPool').append(option);
        });
        $('#drpTMSFunction').empty();
        // Bind new values to dropdown
        $('#drpTMSFunction').each(function () {
            // Create option
            var option = $("<option />");
            option.attr("value", '0').text('--Select--');
            $('#drpTMSFunction').append(option);
        });


    }
});



$("#page_content").on("change", "#drpTMSPool", function () {
    var eid = $("#selectID").val();
    var DivisionId = $("#drpTMSDivision").val();
 var FunctionId = $("#drpTMSFunction").val();
 var PoolId = $("#drpTMSPool").val();
 var RecId = $("#drpRecProcessList").val();
 var BusiId = $("#drpTMSBusiness").val();
 $.ajax({
           url: constantTMS.constantFilterPi,
           data: { RecId: RecId, BusinessId: BusiId, DivisionId: DivisionId, PoolId: PoolId, FunctionId: FunctionId },
                   success: function (data) {
                   debugger
                   drawchart(data);
                   }
           })                   
});

$("#page_content").on("change", "#drpTMSFunction", function () {
    var eid = $("#selectID").val();
    var RecId = $("#drpRecProcessList").val();
    var DivisionId = $("#drpTMSDivision").val();
    var FunctionId = $("#drpTMSFunction").val();
    var PoolId = $("#drpTMSPool").val();
    var BusiId = $("#drpTMSBusiness").val();
    $.ajax({
        url: constantTMS.constantFilterPi,
        data: { RecId: RecId, BusinessId: BusiId, DivisionId: DivisionId, PoolId: PoolId, FunctionId: FunctionId },
        success: function (data) {
            debugger
            drawchart(data);
        }
    })
});



