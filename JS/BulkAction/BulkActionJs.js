$(document).ready(function () {
    $('input[id=SchadulId]').prop('checked', true);
});
$("#headerSch").on("click", function () {
    $('input[id=SchadulId]').prop('checked', true);
})

$("#headerTimesheet").on("click", function () {
    $('input[id=TimesheetId]').prop('checked', true);
})

$("#headerTravel").on("click", function () {
    $('input[id=TravelId]').prop('checked', true);
})
$("#headerBenefit").on("click", function () {
    $('input[id=BenifitId]').prop('checked', true);
})
$("#headerActivity").on("click", function () {
    $('input[id=ActivityTypeId]').prop('checked', true);
})
$("#headerSalary").on("click", function () {
    $('input[id=SalaryId]').prop('checked', true);
})
$("#headerAccessSetUp").on("click", function () {
    $('input[id=AccessSetupId]').prop('checked', true);
})
$("#headerEntiThisYear").on("click", function () {
    $('input[id=HolidayEntiYearId]').prop('checked', true);
})
$("#headerEntiNextYear").on("click", function () {
    $('input[id=HolidayEntitNextYearId]').prop('checked', true);
})
$("#headerTraining").on("click", function () {
    $('input[id=TrainingId]').prop('checked', true);
})
$("#headerResumeCV").on("click", function () {
    $('input[id=SendCVResumeId]').prop('checked', true);
})
$("#headerHoliday").on("click", function () {
    $('input[id=HolidayAccrualsId]').prop('checked', true);
})
$("#headerHolidayBook").on("click", function () {
    $('input[id=BookHolidayId]').prop('checked', true);
})
$("#headerResoureSett").on("click", function () {
    $('input[id=ResourceSettingsId]').prop('checked', true);
})
$("#headerUploadRes").on("click", function () {
    $('input[id=UploadResourcesId]').prop('checked', true);
})
$("#page_content").on("click", "#btnClearFilter", function () {
    $("#busId").val("");
    $("#divsionID").html('');
    $("#poolID").html('');
    $("#functionID").html('');
    var toAppend = '';
    toAppend += "<option value='0'>--Select--</option>";    
    $("#busId").val(0);    
    $("#divsionID").html(toAppend);
    $("#poolID").html(toAppend);
    $("#functionID").html(toAppend);
    $("#jobID").val(0);
    $("#countryID").val(0);
})
function DataTable4Design() {
    var table = $('#salaryDeductionTempTable').DataTable({
        "sDom": '<"top"i>rt<"bottom"flp><"clear">',
        "bPaginate": false
    });
    $('.dataTables_filter').hide();
    $('.dataTables_info').hide();

}
function TotalSalarySet() {
    var amount = $("#Amount").val();
    if (amount == "") {
        amount = 0;
    }
    var CurrencyID = $("#drpCurrency").val();
    var Currencytext = $("#drpCurrency option[value=" + CurrencyID + "]").text().split("-")[1];
    var SalaryTypeID = $("#drpSalary").val();
    var SalaryType = $("#drpSalary option[value=" + SalaryTypeID + "]").text();
    var Total = $('#EmployeeTotalSalaryAmount').val();
    if (Total == "" || Total == undefined) {
        var Total = amount;
    }
    if (SalaryType == "Hourly") {
        $('#TotalSalary').val("Total Salary is" + " " + Currencytext + " " + Total + " per hour");
    }
    else {
        var data = "Total" + " " + SalaryType + " " + "Salary is" + " " + Currencytext + " " + Total;
        $("#TotalSalary").val(data);
        //$('#TotalSalary').val("Total" + " " + SalaryType + " " + "Salary is" + " " + Currencytext + " " + Total);
    }
    $('#EmployeeTotalSalaryAmount').val(Total);
}
$("#checkAll").change(function () {
    $("input:checkbox").prop('checked', $(this).prop("checked"));
});
function AddNewFiledTraining()
{
    
    $("#findBtn").find("#AddNewFieldModel").css("display", "block");
    $.ajax({
        url: bulkAction.getTrainingFiledType,
        data: { },
        success: function (data) {
            debugger;
           $("#AddNewFieldModel").find('#AddNewFieldList').html('');
            $("#AddNewFieldModel").find('#AddNewFieldList').html(data);
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
        }
    });

}
function ValidExpiryDateSelect()
{
    var expDate = $("#txt_ExpiryDate").val();
    var DateAwa = $("#txt_DateAwarded").val();
    if (expDate != "" || DateAwa != "") {
        if (StartDateValidation(DateAwa, expDate)) {
            $("#lbl-error-ValidExpiryDate").show();
            $("#txt_ExpiryDate").val('');
        }
        else {
            $("#lbl-error-ValidExpiryDate").hide();
        }
    }
}
function ValidExpiryDateTraining()
{
    var expDate = $("#EndDate").val();
    var DateAwa = $("#ExpiryDate").val();
    if (expDate != "" || DateAwa != "") {
        if (StartDateValidation(expDate, DateAwa)) {
            $("#valdExpiryDate").show();
            $("#ExpiryDate").val('');
        }
        else {
            $("#valdExpiryDate").hide();
        }
    }
}
//$("#empData").on('click', '.btn-add-EmployeeBenefit', function () {
function serchEmployeeData() {
    var arrayOfValues = [];
    var tableControl = document.getElementById('tblData');
    $('input:checkbox:checked', tableControl).each(function () {
        arrayOfValues.push($(this).closest('tr').find('td:first').attr('id'));
    }).get();
     if ($('#tblData').find('input[type="checkbox"]:checked').length > 0)
     {            
        $('#EmployeeBenefitModal').modal('show')
        var selected_Id = $('input[type="radio"]:checked').attr('id');
        var URL;
        var id = $('input[type="radio"]:checked').attr('id');
        $("#EmployeeBenefitBody").html('');
        $('#hideFooter').show();
        $('#hideHeadder').show();
        if (selected_Id == 'SchadulId') {
            URL = bulkAction.getEmpResult;
            $('#hideFooter').hide();
            $('#hideHeadder').hide();
        }
        else if (selected_Id == 'TravelId') {
            URL = bulkAction.getTravel;
        }
        else if (selected_Id == 'TimesheetId') {
            URL = bulkAction.getTimesheetData;
        }
        else if (selected_Id == 'BenifitId') {
            URL = bulkAction.getBenifit;
        }
        else if (selected_Id == 'SalaryId') {
            URL = bulkAction.getSalary;
            $('#hideFooter').hide();
            $('#hideHeadder').hide();
        }
        else if (selected_Id == 'ActivityTypeId')
        {
            URL = bulkAction.getActivityType;
            $('#hideHeadder').hide();
            $('#hideFooter').hide();
        }
        else if (selected_Id == 'HolidayEntiYearId')
        {
            URL = bulkAction.getHolidayThisYear;
            $('#hideFooter').hide();
            $('#hideHeadder').hide();
        }
        else if (selected_Id == 'HolidayEntitNextYearId')
        {
            URL = bulkAction.getHolidayNextYear;
            $('#hideFooter').hide();
            $('#hideHeadder').hide();
        }
        else if (selected_Id == 'BookHolidayId')
        {
            URL = bulkAction.getholiday;
            $('#hideFooter').hide();
            $('#hideHeadder').hide();
        }
        else if (selected_Id == 'TrainingId')
        {
            $('#showHeadder').show();
            $('#hideHeadder').hide();
            URL = bulkAction.getTrainingList;
        }
        else if (selected_Id == 'UploadResourcesId') {
            URL = bulkAction.getBulkResourceFile;
        }
        else if (selected_Id == 'SendCVResumeId')
        {
            URL = bulkAction.SendResumeCVTocustomer;
        }
        else if (selected_Id == 'ResourceSettingsId')
        {
            URL = bulkAction.getBulkemployeesetting;
        }
        else if (selected_Id == 'AccessSetupId')
        {
            URL = bulkAction.getBulkAccessSetup;
        }
       $.ajax({
            url: URL,
            data: { empId: arrayOfValues.join()},
            success: function (data) {
                $("#EmployeeBenefitModal").find('#EmployeeBenefitBody').html('');
                $("#EmployeeBenefitModal").find('#EmployeeBenefitBody').html(data);
                $('[data-toggle="tooltip"]').tooltip();
                $("#EmployeeBenefitModal").find('#EmployeeBenefitBody').find("#txt_StartDate").Zebra_DatePicker({
                    //direction: false,
                    showButtonPanel: false,
                    format: 'd-m-Y',
                    onSelect: function () {
                        $("#EmployeeBenefitModal").find('#EmployeeBenefitBody').find("#lbl-error-StartDate").hide();
                        $("#EmployeeBenefitModal").find('#EmployeeBenefitBody') .find("#lbl-error-GreaterEndDate").hide();
                        $("#EmployeeBenefitBody").find("#lbl-error-GreaterEndDate").hide();
                        var startdate = $("#txt_StartDate").val();
                        var enddate = $("#txt_EndDate").val();
                        calculateDateDiff(startdate, enddate);
                    }
                });
                $("#EmployeeBenefitModal").find('#EmployeeBenefitBody').find("#txt_EndDate").Zebra_DatePicker({
                    //direction: false,
                    showButtonPanel: false,
                    format: 'd-m-Y',
                    onSelect: function () {
                        $("#EmployeeBenefitModal").find('#EmployeeBenefitBody').find("#lbl-error-EndDate").hide();
                        $("#EmployeeBenefitBody").find("#lbl-error-GreaterEndDate").hide();
                        var startdate = $("#txt_StartDate").val();
                        var enddate = $("#txt_EndDate").val();
                        calculateDateDiff(startdate, enddate);
                    }
                });
                $("#EmployeeBenefitModal").find('#EmployeeBenefitBody').find("#txt_Date").Zebra_DatePicker({
                    //direction: false,
                    showButtonPanel: false,
                    format: 'd-m-Y',
                    onSelect: function () {
                        $("#EmployeeBenefitModal").find('#EmployeeBenefitBody').find("#lbl-error-Date").hide();
                    }
                });
                $("#EmployeeBenefitModal").find('#EmployeeBenefitBody').find("#Effective").Zebra_DatePicker({
                    //direction: false,
                    showButtonPanel: false,
                    format: 'd-m-Y',
                    onSelect: function () {
                        $("#EmployeeBenefitModal").find('#EmployeeBenefitBody').find("#lbl-error-DateAwarded").hide();
                        $("#EmployeeBenefitModal").find('#EmployeeBenefitBody').find("#validationmessagefromdate").hide();

                    }
                });
                $("#EmployeeBenefitModal").find('#EmployeeBenefitBody').find("#txt_DateAwarded").Zebra_DatePicker({
                    //direction: false,
                    showButtonPanel: false,
                    format: 'd-m-Y',
                    onSelect: function () {
                        $("#EmployeeBenefitModal").find('#EmployeeBenefitBody').find("#lbl-error-DateAwarded").hide();
                        $("#EmployeeBenefitModal").find('#EmployeeBenefitBody').find("#lbl-error-ValidExpiryDate").hide();

                        ValidExpiryDateSelect();
                    }
                });
                $("#EmployeeBenefitModal").find('#EmployeeBenefitBody').find("#txt_ExpiryDate").Zebra_DatePicker({
                    //direction: false,
                    showButtonPanel: false,
                    format: 'd-m-Y',
                    onSelect: function () {
                        $("#EmployeeBenefitModal").find('#EmployeeBenefitBody').find("#lbl-error-ExpiryDate").hide();
                        $("#EmployeeBenefitModal").find('#EmployeeBenefitBody').find("#lbl-error-ValidExpiryDate").hide();
                        ValidExpiryDateSelect();
                    }
                });
                $("#EmployeeBenefitModal").find('#EmployeeBenefitBody').find("#StartDate").Zebra_DatePicker({
                    //direction: false,
                    showButtonPanel: false,
                    format: 'd-m-Y',
                    onSelect: function () {
                        $("#EmployeeBenefitModal").find('#EmployeeBenefitBody').find("#validationmessageStartDate").hide();
                        $("#EmployeeBenefitBody").find("#lbl-error-GreaterEndDate").hide();
                        var startdate = $("#StartDate").val();
                        var enddate = $("#EndDate").val();
                        calculateDateDiff(startdate, enddate);
                    }
                });
                $("#EmployeeBenefitModal").find('#EmployeeBenefitBody').find("#EndDate").Zebra_DatePicker({
                    //direction: false,
                    showButtonPanel: false,
                    format: 'd-m-Y',
                    onSelect: function () {
                        $("#EmployeeBenefitModal").find('#EmployeeBenefitBody').find("#validationmessageEndDate").hide();
                        $("#EmployeeBenefitBody").find("#lbl-error-GreaterEndDate").hide();
                        $("#valdExpiryDate").hide();
                        var startdate = $("#StartDate").val();
                        var enddate = $("#EndDate").val();
                        calculateDateDiff(startdate, enddate);
                        ValidExpiryDateTraining();
                    }
                });
                $("#EmployeeBenefitModal").find('#EmployeeBenefitBody').find("#ExpiryDate").Zebra_DatePicker({
                    //direction: false,
                    showButtonPanel: false,
                    format: 'd-m-Y',
                    onSelect: function () {
                        $("#EmployeeBenefitModal").find('#EmployeeBenefitBody').find("#validationmessageExpiryDate").hide();
                        $("#valdExpiryDate").hide();
                        ValidExpiryDateTraining();
                    }
                });
                $("#EmployeeBenefitModal").find('#EmployeeBenefitBody').find("#txt_LessThenStartDate").Zebra_DatePicker({
                    //direction: false,
                    showButtonPanel: false,
                    format: 'd-m-Y',
                    onSelect: function () {
                        $("#EmployeeBenefitModal").find('#EmployeeBenefitBody').find("#lbl-error-LessThenStartDate").hide();
                    }
                });
                $("#EmployeeBenefitModal").find('#wizard').smartWizard({
                    onLeaveStep: leaveAStepCallback,
                    onFinish: onFinishCallback
                });
                //$("#EmployeeBenefitModal").find(".actionBar").css("overflow", "hidden");
                $("#EmployeeBenefitModal").find('#EmployeeBenefitBody').find('.buttonNext').addClass('btn btn-warning');
                $("#EmployeeBenefitModal").find('#EmployeeBenefitBody').find('.buttonPrevious').addClass('btn btn-warning');
                $("#EmployeeBenefitModal").find('#EmployeeBenefitBody').find('.buttonFinish').addClass('btn btn-success');
                $("#EmployeeBenefitModal").find('#EmployeeBenefitBody').find('.buttonPrevious').hide();
                $("#EmployeeBenefitModal").find('#EmployeeBenefitBody').find('.buttonFinish').hide();
                $(".actionBar").scroll("hide");
                $(".hrtoolLoader").hide();
                $(".modal-backdrop").hide();
                spinner();
                var $range = $("#EmployeeBenefitModal").find("#example_id");
                $("#EmployeeBenefitModal").find("#example_id").ionRangeSlider({
                    type: "single",
                    min: 0,
                    max: 100
                });

                $range.on("change", function () {
                    var $this = $(this),
                        value = $this.prop("value");
                    $("#EmployeeBenefitModal").find("#Progress").val(value);
                    //alert("Value: " + value);
                });
                //if (id > 0) {
                
                //    var Importance = $('input[name=Importance]:checked').val();
                //    if (Importance > 0) {
                
                //        $("#EmployeeBenefitModal").find('#EmployeeBenefitBody').find("#validationmessageImportance").hide();
                //    }
                //}
            }
        });
    }
     else {         
         var selected_Id = $('input[type="radio"]:checked').attr('id');
         if (selected_Id == 'UploadResourcesId') {
             URL = bulkAction.getBulkResourceFile;
             $.ajax({
                 url: URL,
                 data: { empId: arrayOfValues.join() },
                 success: function (data) {
                     $("#EmployeeBenefitModal").find('#EmployeeBenefitBody').html('');
                     $("#EmployeeBenefitModal").find('#EmployeeBenefitBody').html(data);
                     $("#EmployeeBenefitModal").modal('show');
                     $('[data-toggle="tooltip"]').tooltip();
                     $(".actionBar").scroll("hide");
                     $(".hrtoolLoader").hide();
                     $(".modal-backdrop").hide();                                      
                 }
             });

         }
         else {
             $('#EmployeeBenefitModal').modal('hide');
             toastr.warning('Please Select Employee');
         }
     }
}
//});

function leaveAStepCallback(obj, context) 
{
    var selected_Id = $('input[type="radio"]:checked').attr('id');
    if (selected_Id == "TrainingId") {
    if (context.fromStep == 1) {
        var iserror = false;
        var TrainingNameId = $("#findBtn").find("#drp-Training").val();
        if (TrainingNameId == "0") {
            iserror = true;
            $("#validationmessageTrainingId").show();
            $("#validationmessageTrainingId").html("The Training is required.");
        }
        var Importance = $('input[name=Importance]:checked').val();
        if (Importance == undefined) {
            iserror = true;
            $("#validationmessageImportance").show();
            $("#validationmessageImportance").html("The Importance is required.");
        }
        var StartDate = $("#findBtn").find("#StartDate").val();
        if (StartDate == "") {
            iserror = true;
            $("#validationmessageStartDate").show();
            $("#validationmessageStartDate").html("The Start Date is required.");
        }
        var EndDate = $("#findBtn").find("#EndDate").val();
        if (EndDate == "") {
            iserror = true;
            $("#validationmessageEndDate").show();
            $("#validationmessageEndDate").html("The End Date is required.");
        }
        var ExpiryDate = $("#findBtn").find("#ExpiryDate").val();
        if (ExpiryDate == "") {
            iserror = true;
            $("#validationmessageExpiryDate").show();
            $("#validationmessageExpiryDate").html("The Expiry Date is required.");
        }

        $.each($("#findBtn").find('.customField'), function () {          
            var id = $(this).context.id.split('_')[1];
            var valid = $(this).find("#IsMandatory_" + id + "").val();
            if (valid == "true") {
                var text = $(this).find("#txtCustome_" + id + "").val();
                if (text == "" || text == "0") {
                    iserror = true;
                    $(this).find("#validationmessage_" + id + "").show();
                    $(this).find("#validationmessageDatePicaker").show();
                }
            }
        });

        if (iserror) {
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
            return false;
        }
        else {
            $.each($("#findBtn").find('.customField'), function () {
                var id = $(this).context.id.split('_')[1];
                var dropdownAvailable = $(this).find('select').length;
                if (dropdownAvailable > 0) {
                    var value = $(this).find("#drpCustome_" + id + "").val();
                    $(this).find("#drpCustome_" + id + "").val(value).find("option[value=" + value + "]").attr('selected', true);
                }
                else {
                    var text = $(this).find("#txtCustome_" + id + "").val();
                    var textAreaAvailable = $(this).find('textarea').length;
                    if (textAreaAvailable > 0) {
                        $(this).find("#txtCustome_" + id + "").html(text);
                    }
                    else {
                        $(this).find("#txtCustome_" + id + "").attr("value", text);
                    }
                }
            });
            $("#EmployeeBenefitModal").find('#EmployeeBenefitBody').find('.buttonNext').hide();
            $("#EmployeeBenefitModal").find('#EmployeeBenefitBody').find('.buttonPrevious').show();
            $("#EmployeeBenefitModal").find('#EmployeeBenefitBody').find('.buttonFinish').show();
            return true;
        }
    }
    else {
        $("#EmployeeBenefitModal").find('#EmployeeBenefitBody').find('.buttonNext').show();
        $("#EmployeeBenefitModal").find('#EmployeeBenefitBody').find('.buttonPrevious').hide();
        $("#EmployeeBenefitModal").find('#EmployeeBenefitBody').find('.buttonFinish').hide();
        return true;
    }
    }
    else if(selected_Id=="BenifitId")
    {
        if (context.fromStep == 1) {
            var isError = false;
            var beniftid = $('#drp-Benifits').val();
            var dateAward = $('#txt_DateAwarded').val();
            var expDate = $('#txt_ExpiryDate').val();
            var Value = $('#txt_FixedAmount').val();            
            var Comment = $('#textArea_comments').val();
            var recoveryOnTer = $('#chk_RecoverOnTermination').is(":checked");
                if (beniftid == 0 || dateAward == "" || expDate == "") {
                
                    if (beniftid == 0) {
                        isError = true;
                    $("#lbl-error-BenifitsList").show();
                    $(".hrtoolLoader").hide();
                    $(".modal-backdrop").hide();
                }
                    if (dateAward == "") {
                        isError = true;
                    $("#lbl-error-DateAwarded").show();
                    $(".hrtoolLoader").hide();
                    $(".modal-backdrop").hide();
                }
                    if (expDate == "") {
                        isError = true;
                    $("#lbl-error-ExpiryDate").show();
                    $(".hrtoolLoader").hide();
                    $(".modal-backdrop").hide();
                }
                }
                 if(!isError) {
                    $("#EmployeeBenefitModal").find('#EmployeeBenefitBody').find('.buttonNext').hide();
                    $("#EmployeeBenefitModal").find('#EmployeeBenefitBody').find('.buttonPrevious').show();
                    $("#EmployeeBenefitModal").find('#EmployeeBenefitBody').find('.buttonFinish').show();
                    return true;
                }
        }
        else {
            $("#EmployeeBenefitModal").find('#EmployeeBenefitBody').find('.buttonNext').show();
            $("#EmployeeBenefitModal").find('#EmployeeBenefitBody').find('.buttonPrevious').hide();
            $("#EmployeeBenefitModal").find('#EmployeeBenefitBody').find('.buttonFinish').hide();
            return true;
        }
    }
    else if (selected_Id == "TravelId") {
        if (context.fromStep == 1) {
            var isError = false;
            var FromCountryID = $('#drp-FromCountryId').val();
            var FromStateId = $('#drp-FromStateId').val();
            var FromTownId = $('#drp-FromTownId').val();
            var ToCountryID = $('#drp-ToCountryId').val();
            var ToStateId = $('#drp-ToStateId').val();
            var ToTownId = $('#drp-ToTownId').val();
            var ReasonTravelId = $('#drp-ReasonForTravelId').val();
            var StartDate = $('#txt_StartDate').val();
            var endDate = $('#txt_EndDate').val();
            var drphrSD = $('#drp-HourseListSD').val();
            var drpminSD = $('#drp-MinutesListSD').val();
            var stdateMin = $('#txt_LessThenStartDate').val();
            var drphrED = $('#drp-HourseListED').val();
            var drminED = $('#drp-MinutesListED').val();
            var durationHR = $('#txt_DurationHours').val();
            var duration = $('#txt_Duration').val();
            var comment = $('#text_Comments').val();
            var type = $('#drp-Type').val();
            var costcode = $('#drp-CostCode').val();
            var LstDate = $("#txt_LessThenStartDate").val();
            if (FromCountryID == 0 || FromStateId == 0 || FromTownId == 0 || ToCountryID == 0 || ToStateId == 0 || ToTownId == 0 || ReasonTravelId == 0 || costcode == 0||
                            comment == "" || type == 0) {
                if (FromCountryID == 0) {
                    isError = true;
                    $("#lbl-error-FromCountryId").show();
                    $(".hrtoolLoader").hide();
                    $(".modal-backdrop").hide();
                }
                if (FromStateId == 0) {
                    isError = true;
                    $("#lbl-error-FromStateId").show();
                    $(".hrtoolLoader").hide();
                    $(".modal-backdrop").hide();
                }
                if (FromTownId == 0) {
                    isError = true;
                    $("#lbl-error-FromTownId").show();
                    $(".hrtoolLoader").hide();
                    $(".modal-backdrop").hide();
                }
                if (ToCountryID == 0) {
                    isError = true;
                    $("#lbl-error-ToCountryId").show();
                    $(".hrtoolLoader").hide();
                    $(".modal-backdrop").hide();
                }
                if (ToStateId == 0) {
                    isError = true;
                    $("#lbl-error-ToStateId").show();
                    $(".hrtoolLoader").hide();
                    $(".modal-backdrop").hide();
                }
                if (ToTownId == 0) {
                    isError = true;
                    $("#lbl-error-ToTownId").show();
                    $(".hrtoolLoader").hide();
                    $(".modal-backdrop").hide();
                }
                if (ReasonTravelId == 0) {
                    isError = true;
                    $("#lbl-error-ReasonForTravelId").show();
                    $(".hrtoolLoader").hide();
                    $(".modal-backdrop").hide();
                }
                if (costcode == 0) {
                    isError = true;
                    $("#lbl-error-CostCode").show();
                    $(".hrtoolLoader").hide();
                    $(".modal-backdrop").hide();
                }
                if (comment == "") {
                    isError = true;
                    $("#lbl-error-Comments").show();
                    $(".hrtoolLoader").hide();
                    $(".modal-backdrop").hide();
                }
                if (type == 0) {
                    isError = true;
                    $("#lbl-error-Type").show();
                    $(".hrtoolLoader").hide();
                    $(".modal-backdrop").hide();
                }
            }
            if ($('#adayormore').is(':checked')) {
                
                // var fromdrpCuntry = $("#findBtn").find("#drp-FromCountryId").val();
                if (StartDate == "" || endDate == "") {
                    if (StartDate == "") {
                        isError = true;
                        $("#lbl-error-StartDate").show();
                        $(".hrtoolLoader").hide();
                        $(".modal-backdrop").hide();
                    }
                    if (endDate == "") {
                        isError = true;
                        $("#lbl-error-EndDate  ").show();
                        $(".hrtoolLoader").hide();
                        $(".modal-backdrop").hide();
                    }
                    
                }
            }
            else if ($('#lessThanADay').is(':checked')) {
                if(LstDate=="")
                {
                    isError = true;
                    $("#lbl-error-LessThenStartDate").show();
                    $(".hrtoolLoader").hide();
                    $(".modal-backdrop").hide();
                }
                if (drphrSD == 0 || drpminSD == 0) {
                    iserror = true;
                    $("#lbl-error-InTimeSD").show();
                    $(".hrtoolLoader").hide();
                    $(".modal-backdrop").hide();
                }
                if (drphrED == 0 || drminED == 0) {
                    iserror = true;
                    $("#lbl-error-InTimeED").show();
                    $(".hrtoolLoader").hide();
                    $(".modal-backdrop").hide();
                }
            }
            if (!isError) {
                $("#EmployeeBenefitModal").find('#EmployeeBenefitBody').find('.buttonNext').hide();
                $("#EmployeeBenefitModal").find('#EmployeeBenefitBody').find('.buttonPrevious').show();
                $("#EmployeeBenefitModal").find('#EmployeeBenefitBody').find('.buttonFinish').show();
                return true;
            }
        }

        else {
            $("#EmployeeBenefitModal").find('#EmployeeBenefitBody').find('.buttonNext').show();
            $("#EmployeeBenefitModal").find('#EmployeeBenefitBody').find('.buttonPrevious').hide();
            $("#EmployeeBenefitModal").find('#EmployeeBenefitBody').find('.buttonFinish').hide();
            return true;
        }
    }
    else if (selected_Id == "TimesheetId") {
        if (context.fromStep == 1) {
            var isError = false;
            var date = $('#txt_Date').val();
            var inTimeHr = $('#txt_InTimeHr').val();
            var InTimeMin = $('#txt_InTimeMin').val();
            var OutTimeHr = $('#txt_EndTimeHr').val();
            var OutTimeMin = $('#txt_EndTimeMin').val();
            var Project = $('#drp-Project').val();
            var CostCode = $('#drp-CostCode').val();
            var Customer = $('#drp-Customer').val();
            var AssetId = $('#drp-Asset').val();
            var comment = $('#text_Comments').val();
            if (date == "" || inTimeHr == 0 || InTimeMin == 0 || OutTimeHr == 0 || OutTimeMin == 0 || CostCode == 0) {
                if (date == "") {
                    isError = true;
                    $("#lbl-error-Date").show();
                    $(".hrtoolLoader").hide();
                    $(".modal-backdrop").hide();
                }
                if (inTimeHr == 0) {
                    isError = true;
                    $("#lbl-error-InTime").show();
                    $(".hrtoolLoader").hide();
                    $(".modal-backdrop").hide();
                }
                if (InTimeMin == 0) {
                    isError = true;
                    $("#lbl-error-InTime").show();
                    $(".hrtoolLoader").hide();
                    $(".modal-backdrop").hide();
                }
                if (OutTimeHr == 0) {
                    $("#llbl-error-EndTime").show();
                    $(".hrtoolLoader").hide();
                    $(".modal-backdrop").hide();
                }
                if (OutTimeMin == 0) {
                    isError = true;
                    $("#lbl-error-EndTime").show();
                    $(".hrtoolLoader").hide();
                    $(".modal-backdrop").hide();
                }
                if (CostCode == 0) {
                    isError = true;
                    $("#lbl-error-CostCode").show();
                    $(".hrtoolLoader").hide();
                    $(".modal-backdrop").hide();
                }
            }
            if (!isError) {
                $("#EmployeeBenefitModal").find('#EmployeeBenefitBody').find('.buttonNext').hide();
                $("#EmployeeBenefitModal").find('#EmployeeBenefitBody').find('.buttonPrevious').show();
                $("#EmployeeBenefitModal").find('#EmployeeBenefitBody').find('.buttonFinish').show();
                return true;
            }
        }

        else {
            $("#EmployeeBenefitModal").find('#EmployeeBenefitBody').find('.buttonNext').show();
            $("#EmployeeBenefitModal").find('#EmployeeBenefitBody').find('.buttonPrevious').hide();
            $("#EmployeeBenefitModal").find('#EmployeeBenefitBody').find('.buttonFinish').hide();
            return true;
        }

    }

}
function onFinishCallback(arrayOfValues) {
    var arrayOfValues = [];
    var tableControl = document.getElementById('tblData');
    $('input:checkbox:checked', tableControl).each(function () {
        arrayOfValues.push($(this).closest('tr').find('td:first').attr('id'));
    }).get();
    var empData = arrayOfValues.join();
    var selected_Id = $('input[type="radio"]:checked').attr('id');
    if (selected_Id == "TrainingId") {
        var mandatory = $('#MandatoryId').val();
        if ($("#MandatoryId").is(':checked')) {
            mandatory = 1;
        }
        else if ($("#OptionalId").is(':checked')) {
            mandatory = 0;
        }
        var Importance = $('input[name=Importance]:checked').val();
        var training = $('#drp-Training').val();
        var startdate = $('#StartDate').val();
        var enddate = $('#EndDate').val();
        var expdate = $('#ExpiryDate').val();
        var provider=$('#Provider').val();
        //var count = $("input[name='Importance']:checked").size();
        var desc = $('#txt-SystemListDescription').val();
        var status= $('#drp-TrainingStatus').val();
        var cost= $('#Cost').val();
        var notes=$('#Notes').val();
        var progress = $("#Progress").val();
        var documentList = [];
        $.each($("#EmployeeBenefitModal").find('#filesList').find(".ListData"), function () {
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
            EmployeeId: empData,
            TrainingNameId: training,
            Description:desc,
            Status:status,
            StartDate:startdate,
            EndDate: enddate,
            ExpiryDate:expdate,
            Provider: provider,
            Cost: cost,
            Notes:notes,
            Importance: Importance,
            Progress:progress,
          //  CustomFieldsJSON: $("#tableDivTraining").find('#CutomiseFiled').val(),
            TraingDocumentList: JsondocumentListJoinString
        }
        $.ajax({
            url: bulkAction.AddEditTraining,
            type: 'POST',
            data: JSON.stringify(model),
            contentType: "application/json",
            success: function () {
                toastr.success('Data Inserted Successfully');
                $('#EmployeeBenefitModal').modal('hide');
            }
        });
    }
    else if (selected_Id == "BenifitId")
    {
        var beniftid = $('#drp-Benifits').val();
        var dateAward = $('#txt_DateAwarded').val();
        var expDate = $('#txt_ExpiryDate').val();
        var Value = $('#txt_FixedAmount').val();
        var Comment = $('#textArea_comments').val();
        var CurruncyId = $("#drpCurrency").val();
        var recoveryOnTer = $('#chk_RecoverOnTermination').is(":checked");
       // var Currency = $("#drpCurrency").val();
        var documentList = [];
        $.each($("#EmployeeBenefitModal").find('#filesList').find(".ListData"), function () {
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
            BenefitID: beniftid,
            EmployeeId: empData,
            DateAwarded: dateAward,
            ExpiryDate: expDate,
            FixedAmount: Value,
            Currency: CurruncyId,
            Comments: Comment,
            RecoverOnTermination: recoveryOnTer,
            DocumentListString: JsondocumentListJoinString
        }
        $.ajax({
                    url: bulkAction.SaveBenifit,
                    type: 'POST',
                    data: JSON.stringify(model),
                    contentType: "application/json",
                    success: function (data) {
                        toastr.success('Data Inserted Successfully');
                        $('#EmployeeBenefitModal').modal('hide');

                    }
                })
    }
    else if (selected_Id == "TravelId") {   
        var fromCountry = $("#EmployeeBenefitModal").find("#drp-FromCountryId").val();
        var fromState = $("#EmployeeBenefitModal").find("#drp-FromStateId").val();
        var fromTown = $("#EmployeeBenefitModal").find("#drp-FromTownId").val();
        var fromAirpoet = $("#EmployeeBenefitModal").find("#drp-FromAirportId").val();
        var toCountry = $("#EmployeeBenefitModal").find("#drp-ToCountryId").val();
        var toState = $("#EmployeeBenefitModal").find("#drp-ToStateId").val();
        var toTown = $("#EmployeeBenefitModal").find("#drp-ToTownId").val();
        var toAirport = $("#EmployeeBenefitModal").find("#drp-ToAirportId").val();
        var type = $("#EmployeeBenefitModal").find("#drp-Type").val();
        var customer = $("#EmployeeBenefitModal").find("#drp-Customer").val();
        var project = $("#EmployeeBenefitModal").find("#drp-Project").val();
        var costCode = $("#EmployeeBenefitModal").find("#drp-CostCode").val();

        var selectedRadio = $("#EmployeeBenefitModal").find('input[name=lessThanADay]:checked').attr("id");
        var documentList = [];
        $.each($("#EmployeeBenefitModal").find('#filesList').find(".ListData"), function () {
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

        if (selectedRadio == "adayormore") {
            var reasonForLeave = $("#EmployeeBenefitModal").find("#drp-ReasonForTravelId").val();
            
            var startDate = $("#EmployeeBenefitModal").find("#txt_StartDate").val().trim();
            var endDate = $("#EmployeeBenefitModal").find("#txt_EndDate").val().trim();
            var comments = $("#EmployeeBenefitModal").find("#text_Comments").val().trim();
            var duration = $("#EmployeeBenefitModal").find("#txt_Duration").val().trim();
            var isLessThenADay = false;
            SaveTravelLeave(isLessThenADay, reasonForLeave, startDate, endDate, duration, comments,0, 0,0,0,0, JsondocumentListJoinString, fromCountry, fromState, fromTown, fromAirpoet, toCountry, toState, toTown, toAirport, type, customer, project, costCode);
        }
        else {
            var reasonForLeave = $("#EmployeeBenefitModal").find("#drp-ReasonForTravelId").val();
            var startDate = $("#EmployeeBenefitModal").find("#txt_LessThenStartDate").val().trim();
            var drphrSD = $('#drp-HourseListSD').val();
            var drpminSD = $('#drp-MinutesListSD').val();
            var drphrED = $('#drp-HourseListED').val();
            var drminED = $('#drp-MinutesListED').val();
            var durationHR = $('#txt_DurationHours').val();
            var isLessThenADay = true;
            var comments = $("#EmployeeBenefitModal").find("#text_Comments").val().trim();
            SaveTravelLeave(isLessThenADay, reasonForLeave, startDate, "", 0, comments, drphrSD, drpminSD, drphrED, drminED, durationHR, JsondocumentListJoinString, fromCountry, fromState, fromTown, fromAirpoet, toCountry, toState, toTown, toAirport, type, customer, project, costCode);
        }
    }
    else if (selected_Id == "TimesheetId") {
        var DetailDiv = [];
        var date = $('#txt_DateTime').val();
        var comment = $('#text_Comments').val();
        $.each($("#EmployeeBenefitModal").find(".Timesheet_Detail_Div"), function () {
            var inTimeHr = $(this).find("#txt_InTimeHr").val();
            var inTimeMin = $(this).find("#txt_InTimeMin").val();
            var endTimeHr = $(this).find("#txt_EndTimeHr").val();
            var endTimeMin = $(this).find("#txt_EndTimeMin").val();
            var project = $(this).find("#drp-Project").val();
            var costCode = $(this).find("#drp-CostCode").val();
            var customer = $(this).find("#drp-Customer").val();
            var asset = $(this).find("#drp-Asset").val();
            var oneData = {
                InTimeHr: inTimeHr,
                InTimeMin: inTimeMin,
                EndTimeHr: endTimeHr,
                EndTimeMin: endTimeMin,
                Project: project,
                CostCode: costCode,
                Customer: customer,
                Asset: asset
            }

            DetailDiv.push(oneData);
        });
        var jsonDetailString = JSON.stringify(DetailDiv);
        var documentList = [];
        $.each($("#EmployeeBenefitModal").find('#filesList').find(".ListData"), function () {
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

        var comment = $("#EmployeeBenefitModal").find("#text_Comments").val().trim();
        var model = {
            EmployeeId: empData,
            Date: date,
            Comment: comment,
            jsonDocumentList: JsondocumentListJoinString,
            jsonDetailList: jsonDetailString
        }
        $.ajax({
            url: bulkAction.AddEditTimeSheet,
            type: 'POST',
            data: JSON.stringify(model),
            contentType: "application/json",
            success: function () {
                toastr.success('Data Inserted Successfully');
                $('#txt_Date').val("");
                $('#txt_InTimeHr').val("");
                $('#txt_InTimeMin').val("");
                $('#txt_EndTimeHr').val("");
                $('#txt_EndTimeMin').val("");
                $('#drp-Project').val("");
                $('#drp-CostCode').val("");
                $('#drp-Customer').val("");
                $('#drp-Asset').val("");
                $('#text_Comments').val("");
                $('#EmployeeBenefitModal').modal('hide');
            }
        })
    }
}
function SaveTravelLeave(isLessThenADay, reasonForLeave, startDate, endDate, duration, comments, drphrSD, drpminSD, drphrED, drminED, durationHR, jsonString, fromCountry, fromState, fromTown, fromAirpoet, toCountry, toState, toTown, toAirport, type, customer, project, costCode)
{
 
    var arrayOfValues = [];
    var tableControl = document.getElementById('tblData');
    $('input:checkbox:checked', tableControl).each(function () {
        arrayOfValues.push($(this).closest('tr').find('td:first').attr('id'));
    }).get();
    var empData = arrayOfValues.join();
    var model = {
        EmployeeId: empData,
        FromCountryId: fromCountry,
        FromStateId: fromState,
        FromTownId: fromTown,
        FromAirportId: fromAirpoet,
        ToCountryId: toCountry,
        ToStateId: toState,
        ToTownId: toTown,
        ToAirportId: toAirport,
        IsLessThenADay: isLessThenADay,
        ReasonForTravelId: reasonForLeave,
        StartDate: startDate,
        EndDate: endDate,
        Duration: duration,
        Comment: comments,
        jsonDocumentList: jsonString,
        InTimeHr: drphrSD,
        InTimeMin: drpminSD,
        EndTimeHr: drphrED,
        EndTimeMin: drminED,
        durationHr: durationHR,
        Type: type,
        Customer: customer,
        Project: project,
        CostCode: costCode
    }
    $.ajax({
        url: bulkAction.saveTravel,
        type: 'POST',
        data: JSON.stringify(model),
        contentType: "application/json",
        success: function (data) {
            toastr.success('Data Inserted Successfully');
            $('#EmployeeBenefitModal').modal('hide');
        }
    });
}
//Scheduling

function calculateDateDiff(stratDate, endDate) {
    if (stratDate != "" || endDate != "") {

        if (StartDateValidation(stratDate, endDate)) {
            $("#EmployeeBenefitBody").find("#lbl-error-GreaterEndDate").show();
            $("#EmployeeBenefitBody").find("#txt_EndDate").val('');
            $("#EndDate").val('');
        }
        else {
            var days = DaysCount(stratDate, endDate);
            $("#EmployeeBenefitBody").find("#txt_Duration").val(days);
        }
    }
}
function ISLessThenADay() {
    $('#div_ADayOrMore').show();
    $('#div_LessThenADay').hide();
}
function ISDayOrMore()
{
    $('#div_ADayOrMore').hide();
    $('#div_LessThenADay').show();
}
function TravelIsDayOrMore()
{
    $("#div_LessThenADay").hide();
    $("#div_ADayOrMore").show();
}
function TravelIsLessThanDay()
{
    $("#div_LessThenADay").show();
    $("#div_ADayOrMore").hide();
}
//Timesheet
function minmax(value, min, max) {
    
    if (parseInt(value) < min || isNaN(parseInt(value)))
        return 0;
    else if (parseInt(value) > max) {
        toastr.warning('Please Enter valid Time');
        return 0;
    }
    else return value;
}
$("#EmployeeBenefitModal").on("click", ".timesheet_delete_icon", function () {
    $(this).parent().parent().parent().parent().remove();
});
function spinner() {
    $('.spinner .btn:first-of-type').on('click', function () {
        var errorMessage = $(this).parent().parent().parent().parent().find(".field-validation-error").length;
        if (errorMessage > 0) {
            $(this).parent().parent().parent().parent().find(".field-validation-error").hide();
        }
        var btn = $(this);
        var input = btn.closest('.spinner').find('input');
        if (input.val() == "") {
            input.val("0");
        }
        if (input.attr('max') == undefined || parseInt(input.val()) < parseInt(input.attr('max'))) {
            input.val(parseInt(input.val(), 10) + 1);
        } else {
            btn.next("disabled", true);
        }
    });
    $('.spinner .btn:last-of-type').on('click', function () {
        var btn = $(this);
        var input = btn.closest('.spinner').find('input');
        if (input.val() == "") {
            input.val("0");
        }
        if (input.attr('min') == undefined || parseInt(input.val()) > parseInt(input.attr('min'))) {
            input.val(parseInt(input.val(), 10) - 1);
        } else {
            btn.prev("disabled", true);
        }
    });
}


$("#btn_AddNew_TimesheetDetail").on("click",function () {
    $.ajax({
        url: bulkAction.getTimeSheetDetails,
        success: function (data) {
            var html = $("#EmployeeBenefitModal").find("#TimeSheet_detail").html();
            if (html == "") {
                $("#EmployeeBenefitModal").find("#TimeSheet_detail").html(data);
            }
            else {
                $("#EmployeeBenefitModal").find("#TimeSheet_detail").append(data);
            }
            spinner();
            $("#EmployeeBenefitModal").find('.Timesheet_Detail_Div:first').find('.timesheet_delete_icon').parent().hide();
        }
    });
});
$("#findBtn").find('#btn-submit-AdminCaseLog').on("click", function () {
    var arrayOfValues = [];
    var tableControl = document.getElementById('tblData');
    $('input:checkbox:checked', tableControl).each(function () {
        arrayOfValues.push($(this).closest('tr').find('td:first').attr('id'));
    }).get();
    addUserInfo(arrayOfValues);
});

$("#findBtn").find('#btnBulkEmpTrainig').on("click", function () {

    var arrayOfValues = [];
    var tableControl = document.getElementById('tblData');
    $('input:checkbox:checked', tableControl).each(function () {
        arrayOfValues.push($(this).closest('tr').find('td:first').attr('id'));
    }).get();
    addTrainingInfo(arrayOfValues);
});
$("#EmployeeBenefitModal").on('change', '#drp-TrainingStatus', function () {
    var TaxtFiled = $("select#drp-TrainingStatus option:selected").text();
    if (TaxtFiled == "In-progress") {
        var $range = $("#example_id");
        $("#example_id").ionRangeSlider({
            type: "single",
            min: 0,
            max: 100
        });

        $range.on("change", function () {
            var $this = $(this),
                value = $this.prop("value");
            $("#tableDivTraining").find("#Progress").val(value);
            //alert("Value: " + value);
        });
        $("#Progresshideshow").css("display", "block");
    }
    else {

        $("#Progresshideshow").css("display", "none");
    }

});


$("#findBtn").find('#btnBulkTimesheetData').on("click", function () {
    
    var arrayOfValues = [];
    var tableControl = document.getElementById('tblData');
    $('input:checkbox:checked', tableControl).each(function () {
        arrayOfValues.push($(this).closest('tr').find('td:first').attr('id'));
    }).get();
    addTimeSheetInfo(arrayOfValues);
});
$("#findBtn").find('#btnBulkTravelData').on("click", function () {
    var arrayOfValues = [];
    var tableControl = document.getElementById('tblData');
    $('input:checkbox:checked', tableControl).each(function () {
        arrayOfValues.push($(this).closest('tr').find('td:first').attr('id'));
    }).get();
    addUserTravelInfo(arrayOfValues);
});
$("#findBtn").find('#btnBulkBenifitData').on("click", function () {
    var arrayOfValues = [];
    var tableControl = document.getElementById('tblData');
    $('input:checkbox:checked', tableControl).each(function () {
        arrayOfValues.push($(this).closest('tr').find('td:first').attr('id'));
    }).get();
    addBenifitInfo(arrayOfValues);
});
$("#findBtn").find('#btnBulkSalaryData').on("click", function () {
    var arrayOfValues = [];
    var tableControl = document.getElementById('tblData');
    $('input:checkbox:checked', tableControl).each(function () {
        arrayOfValues.push($(this).closest('tr').find('td:first').attr('id'));
    }).get();
    addSalaryInfo(arrayOfValues);
}); 

$("#AddNewFieldModel").on('click', '.btnclose', function () {

    // $("#AddNewFieldModel").css("display", "none");
    $("#AddNewFieldModel").modal('hide');
});

$("#findBtn").on('click', '#btn-submit-FiledType', function () {
    debugger;
    lastid = $("#lastId").val();
    lastid++;
    $("#lastId").val(lastid);
    var iserror = false;
    var TaxtFiled = $("#drp-FiledType option:selected").text();
    var Id = $("#drp-FiledType").val();
    var values = $("#Labletext").val();
    var Mandatory = $("#check_MandatoryField").is(":checked");
    if (values == "") {
        iserror = true;
    } 

    if (Id == 0)
    { iserror = true }
    if (iserror) {
        return false;
    }
    else {

        if (TaxtFiled == "Text Field") {

                    var textarea = $("#EmployeeBenefitModal").find("#CoustomeAdd").append("<div class='row marbot10 customField'data-id='" + lastid + "' id='coustome_" + lastid + "'><div class='form-group'><label class='col-md-3 FieldName'>" + values + "</label><div class='col-md-6'><textarea class='form-control textarea-resizeNone' id='txtCustome_" + lastid + "' placeholder=" + values + " value=''></textarea></div><div class='col-md-3'><a class='Removephone' id='Remove_" + lastid + "' data-id=" + lastid + "><span><button type='button' class='close' aria-label='Close'><span aria-hidden='true'>&times;</span></button></span></a><input type='hidden' id='IsMandatory_" + lastid + "' value='" + Mandatory + "' /><span class='field-validation-error' id='validationmessage_" + lastid + "' data-valmsg-for='Name' data-valmsg-replace='true' style='display:none'>This Field Are Required.</span></div></div></div>");
                    //$("#AddNewFieldModel").css("display", "none");
                    //$('#AddNewFieldModel').modal('hide');
                    $(this).remove();
//                     $('#login').modal('hide');


        }
        if (TaxtFiled == "Text Box") {

            var textbox = $("#EmployeeBenefitModal").find("#CoustomeAdd").append("<div class='row marbot10 customField' data-id='" + lastid + "'  id='coustome_" + lastid + "' }><div class='form-group'><label class='col-md-3 FieldName'>" + values + "</label><div class='col-md-6'><input class='form-control' id='txtCustome_" + lastid + "' placeholder=" + values + " value=''></input></div><div class='col-md-3'><a class='Removephone' id='Remove_" + lastid + "' data-id=" + lastid + "><span><button type='button' class='close' aria-label='Close'><span aria-hidden='true'>&times;</span></button></span></a><input type='hidden' id='IsMandatory_" + lastid + "' value='" + Mandatory + "' /><span class='field-validation-error' id='validationmessage_" + lastid + "' data-valmsg-for='Name' data-valmsg-replace='true' style='display:none'>This Field Are Required.</span></div></div></div>");
            $("#AddNewFieldModel").css("display", "none");
        
        }
        if (TaxtFiled == "Date Field") {

            var DateFiled = $("#EmployeeBenefitModal").find("#CoustomeAdd").append("<div class='row marbot10 customField' data-id=" + lastid + "  id='coustome_" + lastid + "'><div class='form-group'><label class='col-md-3 FieldName'>" + values + "</label><div class='col-md-6'><input type='text' class='form-control DatePicker' id='txtCustome_" + lastid + "' placeholder=" + values + " value=''></input></div><div class='col-md-3'><a class='Removephone' id='Remove_" + lastid + "' data-id=" + lastid + "><span><button type='button' class='close' aria-label='Close'><span aria-hidden='true'>&times;</span></button></span></a><input type='hidden' id='IsMandatory_" + lastid + "' value='" + Mandatory + "' /><span class='field-validation-error' id='validationmessageDatePicaker' data-valmsg-for='Name' data-valmsg-replace='true' style='display:none'>This Field Are Required.</span></div></div></div>");

            $("#AddNewFieldModel").css("display", "none");
            $(".DatePicker").Zebra_DatePicker({
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {
                    var fromDate = $(".DatePicker").val();
                    $("#validationmessageDatePicaker").hide();
                }
            });
          
        }
        if (TaxtFiled == "Number Field") {

            var Number = $("#EmployeeBenefitModal").find("#CoustomeAdd").append("<div class='row marbot10 customField' data-id=" + lastid + "  id='coustome_" + lastid + "'><div class='form-group'><label class='col-md-3 FieldName'>" + values + "</label><div class='col-md-6'><input type='number' min='0' class='form-control' id='txtCustome_" + lastid + "' placeholder=" + values + " value=''/></div><div class='col-md-3'><a class='Removephone' id='Remove_" + lastid + "' data-id=" + lastid + "><span><button type='button' class='close' aria-label='Close'><span aria-hidden='true'>&times;</span></button></span></a><input type='hidden' id='IsMandatory_" + lastid + "' value='" + Mandatory + "' /><span class='field-validation-error' id='validationmessage_" + lastid + "' data-valmsg-for='Name' data-valmsg-replace='true' style='display:none' >This Field Are Required.</span></div></div></div>");

            $("#AddNewFieldModel").css("display", "none");
          
        }
        if (TaxtFiled == "Drop Down") {
            var listitem = $("#Additemlist").val();
            var arr = listitem.split(',');
            var dropDownString = "<div class='row marbot10 customField' data-id=" + lastid + "  id='coustome_" + lastid + "'>";
            dropDownString += "<div class='form-group'>";
            dropDownString += "<label class='col-md-3 FieldName'>" + values + "</label>";
            dropDownString += "<div class='col-md-6'><select class='form-control' id='drpCustome_" + lastid + "'><option value='0'>-- Select --</option></select></div>";
            dropDownString += "<div class='col-md-3'>";
            dropDownString += "<a class='Removephone' id='Remove_" + lastid + "' data-id=" + lastid + "><span><button type='button' class='close' aria-label='Close'><span aria-hidden='true'>&times;</span></button></span></a>";
            dropDownString += "<input type='hidden' id='IsMandatory_" + lastid + "' value='" + Mandatory + "' />";
            dropDownString += "<span class='field-validation-error' id='validationmessage_" + lastid + "' data-valmsg-for='Name' data-valmsg-replace='true' style='display:none'>This Field Are Required.</span>";
            dropDownString += "</div>";
            dropDownString += "</div>";
            dropDownString += "</div>";


            //var Dropdown = $("#tableDivTraining").find("#CoustomeAdd").append("<div class='row marbot10' data-id=" + lastid + "  id='coustome_" + lastid + "'><div class='form-group'><label class='col-md-3'>" + values + "</label><div class='col-md-6'><select class='form-control' id='drpCustome_" + lastid + "'>"
            //    + "<option>-- Select --</option>" +
            //    +"</select>"+
            //    "</div><div class='col-md-3'><a class='Removephone' id='Remove_" + lastid + "' data-id=" + lastid + "><span><button type='button' class='close' aria-label='Close'><span aria-hidden='true'>&times;</span></button></span></a><input type='hidden' id='IsMandatory_" + lastid + "' value='" + Mandatory + "' /><span class='field-validation-error' id='validationmessage_" + values + "' data-valmsg-for='Name' data-valmsg-replace='true' style='display:none'>This Field Are Required.</span></div></div></div>");

            $("#EmployeeBenefitModal").find("#CoustomeAdd").append(dropDownString);
            var optionString = "";
            for (var i = 0; i < arr.length; i++) {
                optionString += "<option value='" + arr[i] + "'>" + arr[i] + "</option>";
            }

            $("#EmployeeBenefitModal").find("#CoustomeAdd").find("#drpCustome_" + lastid).append(optionString);
            //for (var i = 0; i < arr.length; i++) {

            //    $('<option>').val(arr[i]).html(arr[i]).appendTo('#drpCustome_' + lastid);

            //}


            //     var element = document.getElementById("coustome_" + lastid + "");
            //     var html = element.outerHTML;
            //     var data = { html: html };
            //     var json = JSON.stringify(Dropdown);
            //    customfiledtype.push(json)
        }
    }
    //$('#CutomiseFiled').val(customfiledtype);
    $(this).parent().find(".btnclose").click();


});

$("#findBtn").find('#btnBulkActivityData').on("click", function () {
    var arrayOfValues = [];
    var tableControl = document.getElementById('tblData');
    $('input:checkbox:checked', tableControl).each(function () {
        arrayOfValues.push($(this).closest('tr').find('td:first').attr('id'));
    }).get();
    addActivityTypeInfo(arrayOfValues);
});
$("#EmployeeBenefitModal").on('click', '.Removephone', function () {
    $(this).parent().parent().parent().remove();
});
$("#findBtn").on('click', '.btnclose', function () {
    $("#AddNewFieldModel").css("display", "none");
});
function test() {
    if ($('#tblData').find('input[type="checkbox"]:checked').length > 0)
    {
        $('#empData').show();
    }

}
function addTrainingInfo(arrayOfValues) {
  
    var empData = arrayOfValues.join();
    var mandatory = $('#MandatoryId').val();
    if($("#MandatoryId").is(':checked')) {
        mandatory = 1;
    }
    else if ($("#OptionalId").is(':checked')) {
        mandatory = 0;
    }
    var training = $('#drp-Training').val(); 
    var startdate=$('#StartDate').val();
    var enddate=$('#EndDate').val();
    var expdate = $('#ExpiryDate').val();
    var mandetoryFiled = $("#MandatoryId").is(':checked');
    var count = $("input[name='Importance']:checked").size();      
     if (training == 0 || startdate == "" || enddate == "" || expdate == "" || count== 0 ) {
        //if (training == 0) {
        //    $("#findBtn").find("#validationmessageTrainingId").show();
        //    $(".hrtoolLoader").hide();
        //    $(".modal-backdrop").hide();
        //}
        //if (startdate == "") {
        //    $("#findBtn").find("#validationmessageStartDate").show();
        //    $(".hrtoolLoader").hide();
        //    $(".modal-backdrop").hide();
        //}
        //if (enddate == "") {
        //    $("#findBtn").find("#validationmessageEndDate").show();
        //    $(".hrtoolLoader").hide();
        //    $(".modal-backdrop").hide();
        //}
        //if (expdate == "") {
        //    $("#findBtn").find("#validationmessageExpiryDate").show();
        //    $(".hrtoolLoader").hide();
        //    $(".modal-backdrop").hide();
        //}
        //if (count == 0) {
        //    $("#findBtn").find("#validationmessageImportance").show();
        //    $(".hrtoolLoader").hide();
        //    $(".modal-backdrop").hide();
        //}
    }
    else{
     $.ajax({
         url: bulkAction.AddEditTraining,
        type: 'POST',
        data: {
            empId:empData,
            TrainingNameId:$('#drp-Training').val(),
            Description:$('#txt-SystemListDescription').val(),
            Importance: mandatory,
            status:$('#drp-TrainingStatus').val(),
            startDate: startdate,
            emdDate: enddate,
            expDate: expdate,
            provider:$('#Provider').val(),
            cost:$('#Cost').val(),
            note:$('#Notes').val(),
            //CustomFieldsJSON:$('#').val(),
            attachment:$('#fileToUpload').val()
        },
        success: function (data) {
            toastr.success('Data Inserted Successfully');
            $('#drp-Training').val(""),
            $('#txt-SystemListDescription').val(""),
            $('#MandatoryId').val(""),
            $('#drp-TrainingStatus').val(""),
            $('#StartDate').val(""),
            $('#EndDate').val(""),
            $('#ExpiryDate').val(""),
            $('#Provider').val(""),
            $('#Cost').val(""),
            $('#Notes').val(""),
            //CustomFieldsJSON:$('#').val(),
            $('#fileToUpload').val("")

        }

    })}
}
function addEmpHolidayThisyearInfo(count) {
    var empDataArr = [];
    //table.find('tbody').find('tr').each(function (i) {
    //    var $tds = $(this).find('td');
    //    empId = $tds.eq(0).text();
    //    empid[i] = $tds.eq(0).text();
    //    arr[i] = $tds.eq(3).text();   
    //    //arr[i] = $tds.eq(2).text()
    //    //alert('Row '+ (i + 1) + ':\nId: ' + empId);
    //});
    
    for (var i = 1; i <= count ; i++) {
        var empData = {
            'EmpId': $('#hidden_' + i).val(),
            'Year': $('#Value_' + i).val()
        };
        empDataArr.push(empData);
    }
   
    empDataArr = JSON.stringify({ '_employeeRequest': empDataArr });

    $.ajax({
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        url: bulkAction.AddEditHolidayThisYear,
        type: 'POST',
        data: empDataArr    ,
        success: function (data) {
            toastr.success('Data Inserted Successfully');
            $('#EmployeeBenefitModal').modal('hide');

        }
    });
}
function addEmpHolidayNextyearInfo(count) {
    var empDataArr = [];   

    for (var i = 1; i <= count ; i++) {
        var empData = {
            'EmpId': $('#hidden_' + i).val(),
            'Year': $('#Value_' + i).val()
        };
        empDataArr.push(empData);
    }

    empDataArr = JSON.stringify({ '_employeeRequest': empDataArr });

    $.ajax({
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        url: bulkAction.AddEditHolidayNextYear,
        type: 'POST',
        data: empDataArr,
        success: function (data) {
            
            toastr.success('Data Inserted Successfully');
            $('#EmployeeBenefitModal').modal('hide');

        }
    });
}
function addActivityTypeInfo(arrayOfValues) {
    
    var isError = false;
    var empData = arrayOfValues.join();
    var Year=$('#YearText').val();
    var ActivityType=$('#ActivityTypeText').val();
    var curruncy = $('#selCurrency').val();
    var workUnit = $('#selWorkUnit').val();
    if (Year == 0 || ActivityType == "" || curruncy == 0 || workUnit == 0) {
        isError = true;
        if (Year == 0) {
            $("#findBtn").find("#lbl-error-Year").show();
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
        }
        if (ActivityType == "") {
            $("#findBtn").find("#lbl-error-ActivityTypes").show();
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
        }
        if (curruncy == 0) {
            $("#findBtn").find("#lbl-error-CurrencyList").show();
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
        }
        if (workUnit == 0) {
            $("#findBtn").find("#lbl-error-WorkUnitList").show();
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
        }

    }else{
        $.ajax({
            url: bulkAction.AddEditActivityType,
            type: 'POST',
            data: {
                EmpId: empData,
                Year:$('#YearText').val(),
                name: $('#ActivityTypeText').val(),
                currencyId: $('#selCurrency').val(),
                workUnitId: $('#selWorkUnit').val(),
                WorkerRate: $('#WorkerRateText').val(),
                customerRate: $('#CustomerRateText').val()
            },
            success: function (data) {
               toastr.success('Data Inserted Successfully');
                $('#YearText').val(""),
               $('#ActivityTypeText').val(""),
                 $('#selCurrency').val(""),
               $('#selWorkUnit').val(""),
                 $('#WorkerRateText').val(""),
                $('#CustomerRateText').val("")
                $('#EmployeeBenefitModal').modal('hide');

                //$(".toast-success").show();
                //setTimeout(function () { $(".toast-success").hide(); }, 1500);
            }
        })}
}
function addSalaryInfo(arrayOfValues)
{
    var isError = false;
    var empData = arrayOfValues.join();
    var EffFromDate = $('#Effective').val();
    var salaryType = $('#drpSalary').val();
    var paymentFreq = $('#drpPayment').val();
    var ammount = $('#Amount').val();
    var curruncy = $('#drpCurrency').val();
    var totalSal = $('#TotalSalary').val();
    var reasonForChange = $('#drpResonforChange').val();
    var Comment = $('#Comments').val();
    if (EffFromDate == "" || reasonForChange==0 || ammount==0||curruncy==0) {
        if (EffFromDate == "") {
            isError = true;
            $("#findBtn").find("#validationmessagefromdate").show();
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
        }
        
        if (ammount == 0) {
            isError = true;
            $("#findBtn").find("#validationmessageAmount").show();
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
        }
        if (curruncy == 0) {
            isError = true;
            $("#findBtn").find("#validationmessageCurruncy").show();
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
        }
        if (reasonForChange == 0) {
            isError = true;
            $("#findBtn").find("#validationmessageReason").show();
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
        }
    }
    else {
        $.ajax({
            url: bulkAction.AddEditSalary,
            type: 'POST',
            data: {

                EmployeeId: empData,
                EffectiveFrom: EffFromDate,
                SalaryType: salaryType,
                PaymentFrequancy: paymentFreq,
                Ammount: ammount,
                Curruncy: curruncy,
                TotalSalary: totalSal,
                ReasonForChange: reasonForChange,
                comment: Comment
            },
            success: function () {
               
               toastr.success('Data Inserted Successfully');
                $('#Effective').val("");
                $('#drpSalary').val("");
                $('#drpPayment').val("");
                $('#Amount').val("");
                $('#drpCurrency').val("");
                $('#TotalSalary').val("");
                $('#drpResonforChange').val("");
                $('#Comments').val("");
                $('#EmployeeBenefitModal').modal('hide');

                //$(".toast-success").show();
                //setTimeout(function () { $(".toast-success").hide(); }, 1500);
            }

        })
    }
}

$('#drpCurrency').change(function () {
    var Id = $('#drpCurrency').val();
    
    var ammount = $('#Amount').val();
    $.ajax({
        url: bulkAction.getCur,
        type: 'POST',
        data: {
            id: Id,
            amount: ammount,
        },
        success: function (result) {
            $('#TotalSalary').val(result);
        }
    })
});
$("#drp-ToCountryId").change(function () {
    $('#drp-ToStateId').empty();
    $('#drp-ToTownId').empty();
    $('#drp-ToAirportId').empty();
        TofillState();
        TofillAirport();
    });
$("#drp-ToStateId").change(function () {
    $('#drp-ToTownId').empty();
    $('#drp-ToAirportId').empty();
        TofillTown();
    });
$("#drp-FromCountryId").change(function () {
    $('#drp-FromStateId').empty();
    $('#drp-FromTownId').empty();
    $('#drp-FromAirportId').empty();
        FromfillState();
        FromfillAirport();
    });
$("#drp-FromStateId").change(function () {
        FromfillTown();
    });
function FromfillState()
    {
        var id = $('#drp-FromCountryId').val();
      
        $.ajax({
            url: bulkAction.BindState,
            contentType: "GET",
            dataType: "JSON",
            data: {
                countryId: id
            },
            success: function (country) {
                $("#drp-FromStateId").html(""); // clear before appending new list
                //$("#drp-FromStateId").append('<option value="0">Select State</option>');
                $.each(country, function (i, country) {
                    $("#drp-FromStateId").append(('<option value="' + country.Value + '">' + country.Text + '</option>'));
                })
            }
        })
    }
function FromfillTown() {
        var id = $('#drp-FromStateId').val();   
        $.ajax({
            url: bulkAction.BindCity,
            contentType: "GET",
            dataType: "JSON",
            data: {
                stateId: id
            },
            success: function (country) {
                $("#drp-FromTownId").html(""); // clear before appending new list
                //$("#drp-FromStateId").append('<option value="0">Select State</option>');
                $.each(country, function (i, country) {
                    $("#drp-FromTownId").append(('<option value="' + country.Value + '">' + country.Text + '</option>'));
                })
            }
        })
    }
function FromfillAirport() {
        var id = $('#drp-FromCountryId').val();
        $.ajax({
            url: bulkAction.BindAirport,
            contentType: "GET",
            dataType: "JSON",
            data: {
                countryId: id
            },
            success: function (country) {
                $("#drp-FromAirportId").html(""); // clear before appending new list
                //$("#drp-FromStateId").append('<option value="0">Select State</option>');
                $.each(country, function (i, country) {
                    $("#drp-FromAirportId").append(('<option value="' + country.Value + '">' + country.Text + '</option>'));
                })
            }
        })
    }
function TofillState()
    {
        var id = $('#drp-ToCountryId').val();
    
        $.ajax({
            url: bulkAction.BindState,
            contentType: "GET",
            dataType: "JSON",
            data: {
                countryId: id
            },
            success: function (country) {
                $("#drp-ToStateId").html(""); // clear before appending new list
                //$("#drp-FromStateId").append('<option value="0">Select State</option>');
                $.each(country, function (i, country) {
                    $("#drp-ToStateId").append(('<option value="' + country.Value + '">' + country.Text + '</option>'));
                })
            }
        })
    }
function TofillTown() {
var id = $('#drp-ToStateId').val();
       $.ajax({
            url: bulkAction.BindCity,
            contentType: "GET",
            dataType: "JSON",
            data: {
                stateId: id
            },
            success: function (country) {
                $("#drp-ToTownId").html(""); // clear before appending new list
                //$("#drp-FromStateId").append('<option value="0">Select State</option>');
                $.each(country, function (i, country) {
                    $("#drp-ToTownId").append(('<option value="' + country.Value + '">' + country.Text + '</option>'));
                })
            }
        })
}
function TofillAirport() {
        var id = $('#drp-ToCountryId').val();
        $.ajax({
            url: bulkAction.BindAirport,
            contentType: "GET",
            dataType: "JSON",
            data: {
                countryId: id
            },
            success: function (country) {
                $("#drp-ToAirportId").html(""); // clear before appending new list
                //$("#drp-FromStateId").append('<option value="0">Select State</option>');
                $.each(country, function (i, country) {
                    $("#drp-ToAirportId").append(('<option value="' + country.Value + '">' + country.Text + '</option>'));
                })
            }
        })
}
$("#page_content").on('change', '#drp-HourseListSD', function (e) {
    $("#lbl-error-InTimeSD").hide();
    $("#lbl-error-DurationHours").hide();
    $("#lbl-error-validtimeSD").hide();
    var St = $(this).val();
    var Sm = $("#drp-MinutesListSD").val();
    var Et = $("#drp-HourseListED").val();
    var Em = $("#drp-MinutesListED").val();
    var Diff = ((new Date("1991-1-1 " + Et + "" + ":" + "" + Em + "") - new Date("1991-1-1 " + St + "" + ":" + "" + Sm + "")) / 1000 / 60 / 60).toFixed(2);
    if (Diff <= 0) {
        $("#txt_DurationHours").val('');
        $("#lbl-error-validtimeSD").show();
    }
    else { $("#txt_DurationHours").val(Diff); }
    
});
$("#page_content").on('change', '#drp-MinutesListSD', function (e) {
    $("#lbl-error-InTimeSD").hide();
    $("#lbl-error-validtimeSD").hide();
    $("#lbl-error-DurationHours").hide();
    var St = $("#drp-HourseListSD").val();
    var Sm = $(this).val();
    var Et = $("#drp-HourseListED").val();
    var Em = $("#drp-MinutesListED").val();
    var Diff = ((new Date("1991-1-1 " + Et + "" + ":" + "" + Em + "") - new Date("1991-1-1 " + St + "" + ":" + "" + Sm + "")) / 1000 / 60 / 60).toFixed(2);
    if (Diff <= 0) {
        $("#txt_DurationHours").val('');
        $("#lbl-error-validtimeSD").show();
    }
    else { $("#txt_DurationHours").val(Diff); }

});
//drp-HourseListED   txt_DurationHours
$("#page_content").on('change', '#drp-HourseListED', function (e) {
    $("#lbl-error-DurationHours").hide();
    $("#lbl-error-InTimeED").hide();
    $("#lbl-error-validtimeSD").hide();
    var St = $("#drp-HourseListSD").val();
    var Sm = $("#drp-MinutesListSD").val();
    var Et = $(this).val();
    var Em = $("#drp-MinutesListED").val();
    var Diff = ((new Date("1991-1-1 " + Et + "" + ":" + "" + Em + "") - new Date("1991-1-1 " + St + "" + ":" + "" + Sm + "")) / 1000 / 60 / 60).toFixed(2);
    
    if (Diff <= 0) {
        $("#txt_DurationHours").val('');
        $("#lbl-error-validtimeSD").show();
    }
    else { $("#txt_DurationHours").val(Diff); }

    
  
});
//drp-MinutesListED
$("#page_content").on('change', '#drp-MinutesListED', function (e) {
    $("#lbl-error-DurationHours").hide();
    $("#lbl-error-InTimeED").hide();
    $("#lbl-error-validtimeSD").hide();
    var St = $("#drp-HourseListSD").val();
    var Sm = $("#drp-MinutesListSD").val();
    var Et = $("#drp-HourseListED").val();
    var Em = $(this).val();
    var Diff = ((new Date("1991-1-1 " + Et + "" + ":" + "" + Em + "") - new Date("1991-1-1 " + St + "" + ":" + "" + Sm + "")) / 1000 / 60 / 60).toFixed(2);   
    if (Diff <= 0) {
        $("#txt_DurationHours").val('');
        $("#lbl-error-validtimeSD").show();
    }
    else { $("#txt_DurationHours").val(Diff); }

});

function addUserInfo(arrayOfValues)
{
    //var id = $(this).attr("data-id");
    //alert(id);
    var iserror = false;
    var isLessDay=false;
    var isDayorMore=false;
        if ($('#adayormore').is(':checked')) {
            isDayorMore = true;          
        }
        if($('#lessThanADay').is(':checked')){
            isLessDay = true; }
        var EmpData = arrayOfValues.join();
        //var alen = arrayOfValues.len;      
        var stDate = $('#txt_StartDate').val();
        var edDate = $('#txt_EndDate').val();
        var duration = $('#txt_Duration').val();
        var customer = $('#drp-Customer').val();
        var pro = $('#drp-Project').val();
        var ass = $('#drp-Asset').val();
        var cmt = $('#text_Comments').val();
        var drphrSD = $('#drp-HourseListSD').val();
        var drpminSD = $('#drp-MinutesListSD').val();
        var stdateMin = $('#txt_LessThenStartDate').val();
       var drphrED = $('#drp-HourseListED').val();
       var drminED = $('#drp-MinutesListED').val();
       var durationHR = $('#txt_DurationHours').val();
       
        if ($('#lessThanADay').is(':checked'))
        {
            if (drphrSD == 0 || drpminSD == 0)
            {
                iserror = true;               
                $("#findBtn").find("#lbl-error-InTimeSD").show();
                $(".hrtoolLoader").hide();
                $(".modal-backdrop").hide();
            }
            if (drphrED == 0 || drminED == 0)
            {
                iserror = true;
                $("#findBtn").find("#lbl-error-InTimeED").show();
                $(".hrtoolLoader").hide();
                $(".modal-backdrop").hide();
            }
            if (stdateMin == "") {
                iserror = true;
                $("#findBtn").find("#lbl-error-LessThenStartDate").show();
                $(".hrtoolLoader").hide();
                $(".modal-backdrop").hide();
            }
            if (cmt == "") {
                iserror = true;
                $("#findBtn").find("#lbl-error-Comments").show();
                $(".hrtoolLoader").hide();
                $(".modal-backdrop").hide();
            }
        }
       else if ($('#adayormore').is(':checked'))
       {                        
                if (stDate == "") {
                    iserror = true;
                    $("#lbl-error-StartDate").show();
                    $("#lbl-error-StartDate").html("The Start Date is required.");
                    //$(".hrtoolLoader").hide();
                    //$(".modal-backdrop").hide();
                }
                if (edDate == "") {
                    iserror = true;
                    $("#findBtn").find("#lbl-error-EndDate").show();
                    $(".hrtoolLoader").hide();
                    $(".modal-backdrop").hide();
                }
                if (cmt == "") {
                    iserror = true;
                    $("#findBtn").find("#lbl-error-Comments").show();
                    $(".hrtoolLoader").hide();
                    $(".modal-backdrop").hide();
                }
            
        }
        if(!iserror)
        {
         
            $.ajax({
                url: bulkAction.getSchedul,
                type: 'POST',
                data: {
                    EmpID: EmpData,
                    IsLessDay: isLessDay,
                    IsDayorMore: isDayorMore,
                    startDate: stDate,
                    EndDate: edDate,
                    duation: duration,
                    customer: customer,
                    project: pro,
                    asset: ass,
                    comment: cmt,
                    HrSD:  drphrSD ,
                    MinSD:drpminSD ,
                    CDate: stdateMin,
                    HrED: drphrED,
                    MinED: drminED,
                    durationHr: durationHR

               },
                success: function () {
                    toastr.success('Data Inserted Successfully');
                    $('#txt_StartDate').val("");
                    $('#txt_EndDate').val("");
                    $('#txt_Duration').val("");
                    $('#drp-Customer').val("");
                    $('#drp-Project').val("");
                    $('#drp-Asset').val("");
                    $('#text_Comments').val("");
                    $('#EmployeeBenefitModal').modal('hide');
                }
            })
        }
    
    }     
$('#btnDisplay').on("click", function () {
        {
            var bid = $('#busId').val();
            var did = $('#divsionID').val();
            var pid = $('#poolID').val();
            var fid = $('#functionID').val();
            var jid = $('#jobID').val();
            var cid = $('#countryID').val();            
            if (did == 0) {
                did = 0;
            }
            if (pid == 0) {
                pid = 0;
            }
            if (fid == 0) {
                fid = 0;
            }
            if (jid == 0) {
                jid = 0;
            }
            if (cid == 0) {
                cid = 0;
            }
            
            $.ajax({
                type: 'POST',
                url: bulkAction.getSearchList,
                data: {
                    busiId: bid,
                    divId: did,
                    poolId: pid,
                    funId: fid,
                    jobId: jid,
                    counId: cid
                },
                success: function (result) {
                    $('#serdata').html(result);
                    $('#serdata').show();
                }
            })
        }
    });

$('#divsionID').change(function () {
        fillPool();
        fillFunction();
    });
//$('#busId').change(function () {
//        fillDiv();    
//    });
function fillJobTitle() {
        var catId = $('#CountryOfR').val();

        $.ajax({
            url: bulkAction.fillJob,
            contentType: "GET",
            dataType: "JSON",
            data: {
                //  businessId: BusiId
            },
            success: function (jobtitle) {
                $("#jobID").html(""); // clear before appending new list
                $("#jobID").append('<option value="0">Select Job Title</option>');
                $.each(jobtitle, function (i, jobtitle) {
                    $("#jobID").append(('<option value="' + jobtitle.Id + '">' + jobtitle.Name + '</option>'));
                })
            }
        })
    }
function filCountry() {
        // var catId = $('#CountryOfR').val();
        $.ajax({
            url:bulkAction.fillCountry,
            contentType: "GET",
            dataType: "JSON",
            data: {
                //  businessId: BusiId
            },
            success: function (country) {
                $("#countryID").html(""); // clear before appending new list
                $("#countryID").append('<option value="0">Select Country</option>');
                $.each(country, function (i, country) {
                    $("#countryID").append(('<option value="' + country.Id + '">' + country.Name + '</option>'));
                })
            }
        })
    }
//function fillFunction() {

//        var divId = $('#divsionID').val();
//        $.ajax({
//            url: bulkAction.getfunction,
//            contentType: "GET",
//            dataType: "JSON",
//            data: {
//                divisionId: divId
//            },
//            success: function (functionVal) {

//                $("#functionID").html(""); // clear before appending new list
//                $("#functionID").append('<option value="0">Select Function</option>');

//                $.each(functionVal, function (i, functionVal) {
//                    $("#functionID").append(
//                        $('<option></option>').val(functionVal.Id).html(functionVal.Name));
//                });
//            }

//        })
//    }
//function fillPool() {
       
//        var divId = $('#divsionID').val();
       
//        $.ajax({
//            url: bulkAction.getPool,
//            contentType: "GET",
//            dataType: "JSON",
//            data: {
//                divisionId: divId
//            },
//            success: function (pool) {

//                $("#poolID").html(""); // clear before appending new list
//                $("#poolID").append('<option value="0">Select Pool</option>');
//                $.each(pool, function (i, pool) {
//                    $("#poolID").append(
//                        $('<option></option>').val(pool.Id).html(pool.Name));
//                });
//            }

//        })
//    }
//function fillDiv() {
//        //var BusiId = $('#BusinessName').val();
//        var BusiId= $('#busId').val();
//        $.ajax({
//            url: bulkAction.filDiv,
//            contentType: "GET",
//            dataType: "JSON",
//            data: {
//                businessId: BusiId
//            },
//            success: function (division) {
//                $("#divsionID").html(""); // clear before appending new list
//                $("#divsionID").append('<option value="0">Selected Division</option>');
//                $.each(division, function (i, division) {
//                    $("#divsionID").append(('<option value="' + division.Id + '">' + division.Name + '</option>'));
//                });
//            }

//        })
//    }

//$(function () {
//        $('#searchData').hide();
//});
$(document).ready(function () {
    //$('#searchData').hide();
    $('#AccessSetupId')
});
$('a').click(function () {
    $('#searchData').show();
});
// Book Holiday

function savebookHoliday()
{
    
    var isError=false;
    var selectedRadio = $('input[name=lessThanADay]:checked').attr("id");
    if (selectedRadio == "adayormore") {
        var startDate = $("#txt_StartDate").val();
        var endDate = $("#txt_EndDate").val();
        var comments = $("#text_Comments").val();
        var duration = $("#txt_Duration").val();
        var isLessThenADay = false;
        if (startDate == "") { isError = true; $("#lbl-error-StartDate").show(); }
        if (endDate == "") { isError = true; $("#lbl-error-EndDate").show(); }
        if (comments == "") { isError = true; $("#lbl-error-Comments").show(); }
        if (isError) {
            return false;
        }
        else {
            SaveAnnualLeave(isLessThenADay, startDate, endDate, duration, comments,"");
        }
    }
    else {
        var startDate = $("#txt_LessThenStartDate").val();
        var duration = '0.5';
        var isLessThenADay = true;
        var partOfDay = $("#drp-PartOfDay").val();
        var comments = $("#text_Comments").val();
        if (startDate == "") { isError = true; $("#lbl-error-LessThenStartDate").show(); }
        if (comments == "") { isError = true; $("#lbl-error-Comments").show(); }
        if (isError) {
            return false;
        }
        else {
            SaveAnnualLeave(isLessThenADay, startDate, "", duration, comments, partOfDay);
        }
    }
}
function SaveAnnualLeave(isLessThenADay, startDate, endDate, duration, comments, partOfDay) {
    var arrayOfValues = [];
    var tableControl = document.getElementById('tblData');
    $('input:checkbox:checked', tableControl).each(function () {
        arrayOfValues.push($(this).closest('tr').find('td:first').attr('id'));
    }).get();
    var empData = arrayOfValues.join();
    var model = {       
        EmployeeId: empData,
        IsLessThenADay: isLessThenADay,
        startDate: startDate,
        endDate: endDate,
        Duration: duration,
        comment: comments,
        PartOfDay: partOfDay,
    }

    $.ajax({
        url: bulkAction.saveBookHoliday,
        type: 'POST',
        data: JSON.stringify(model),
        contentType: "application/json",
        success: function (data) {
            toastr.success('Data Inserted Successfully');
            $('#EmployeeBenefitModal').modal('hide');
        }
    });
}
$("#page_content").on("change", ".Annual_leave_radio", function () {
    var Id = $(this).attr("id");
    if (Id == "adayormore") {
        $("#div_ADayOrMore").show();
        $("#div_LessThenADay").hide();
    }
    else {
        $("#div_LessThenADay").show();
        $("#div_ADayOrMore").hide();
    }
});


$("#EmployeeBenefitBody").on('click', '.btn-add-salaryDeduction', function () {        
    var IsError = false;
        var hiddenId = $("#TempSalaryID").val();
        var EmployeeID = $("#EmployeeID").val();
        var TotalSalary = $('#EmployeeTotalSalaryAmount').val();
        var EffectiveFrom = $("#Effective").val();
        var SalaryTypeID = $("#drpSalary").val();
        var PaymentFrequencyID = $("#drpPayment").val();
        var CurrencyID = $("#drpCurrency").val();
        var Amount = $("#Amount").val();
        var ReasonforChange = $("#drpResonforChange").val();
        var SalaryId = $("#SalaryIdTemp").val();
        var SalaryComments = $("#Comments").val();
        var model = {
            //Id: 8,
            Id: hiddenId,
            EmployeeId: EmployeeID,
            EmployeeSalaryID: SalaryId,
            EffectiveFrom: EffectiveFrom,
            SalaryTypeID: SalaryTypeID,
            PaymentFrequencyID: PaymentFrequencyID,
            CurrencyID: CurrencyID,
            Amount: Amount,
            TotalSalary: TotalSalary,
            ReasonforChange: ReasonforChange,
            Comments: SalaryComments,
        }
        if (TotalSalary == "0") {
            IsError = true;
            $("#validationmessageAmount").show();
            $("#validationmessageAmount").html("Amount required");
        }
        //if (Amount == "0")
        //{
        //    IsError = true;
        //    $("#validationmessageAmount").show();
        //    $("#validationmessageAmount").html("Amount required");
        //}
        if (IsError) {
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
            $("#btn-close-SalaryDeductionTemp").click();
        }
        else {      
            $('#addsalaryDeductionTemp').modal('toggle');
            $("#addsalaryDeductionTemp").modal('show');
            $.ajax({
                type: "POST",
                data: JSON.stringify(model),
                url: bulkAction.SaveSalary,
                contentType: "application/json",
                success: function (data) {
                    $("#addsalaryDeduction").find('#AddsalaryDeductionDeductionBody').html('');
                    $("#addsalaryDeduction").find('#AddsalaryDeductionDeductionBody').html(data);
                    $("#addsalaryDeduction").modal('show');
                    $("#EmployeeTotalSalaryTemp").val(Amount);
                    $("#tableDivDeductionSalary").find(".salaryDeductionDeductionTitle").html("ADD Deduction");
                    $("#tableDivDeductionSalary").find("#btn-submit-salaryDeduction").html("Add");
                    $('[data-toggle="tooltip"]').tooltip();
                    $(".hrtoolLoader").hide();
                    $(".modal-backdrop").hide();

                }
            });
        }
  
});
$("#EmployeeBenefitBody").on('click', '#btn-submit-salaryDeduction', function () {

    $(".hrtoolLoader").show();
    var iserror = false;
    var Id = $("#SalaryDeductionIDTemp").val();
    var SalaryId = $("#SalaryIdTemp").val();
    var model = {
        Id: Id,
        EmployeeID: $("#EmployeeID").val(),
        EmployeeSalaryID: SalaryId,
        DeductionID: $("#drp-SalaryDeductionTemp").val(),
        FixedAmount: $("#FixedAmountTemp").val(),
        PercentOfSalary: $("#txt_PercentOfSalaryTemp").val(),
        IncludeInSalary: $("#check_IncludeSalaryTemp").is(":checked"),
        Comments: $("#CommentsTemp").val(),

    }
    if (model.DeductionID == "0") {
        iserror = true;
        $("#lbl-error-DeductionList").show();
    }
    if (model.FixedAmount == "" || model.FixedAmount == "0" || model.FixedAmount == "0.00") {
        iserror = true;
        $("#validationmessageFixedAmount").show();
        $("#validationmessageFixedAmount").html("Fiexd amount is required.");
    }
    if (model.PercentOfSalary == "0" || model.PercentOfSalary == "" || model.PercentOfSalary == "0.00") {
        iserror = true;
        $("#validationmessagePercentOfSalary").show();
        $("#validationmessagePercentOfSalary").html("Percent of salary is required.");
    }
    if (iserror) {
        $(".hrtoolLoader").hide();
        $(".modal-backdrop").hide();
        return false;
    }
    else {
        $.ajax({
            type: "POST",
            data: JSON.stringify(model),
            url: bulkAction.SaveSalarytDeductionTemp,
            contentType: "application/json",
            success: function (data) {
                debugger;
                $('#AddSalaryBody').html('');
                $('#AddSalaryBody').html(data);
                $(".salaryTitle").html("Edit Salary");
                $("#btn-submit-Salary").html("Save");
                $("#addsalaryDeduction").modal('hide');
                //var CurrencyID = $("#tableDivSalary").find("#drpCurrency").val();
                //var SalaryTypeID = $("#tableDivSalary").find("#drpSalary").val();
                //var Currencytext = $("#drpCurrency option[value=" + CurrencyID + "]").text().split('-')[1];
                //var SalaryType = $("#drpSalary option[value=" + SalaryTypeID + "]").text().split('-')[0];
                //var Total = $("#tableDivSalary").find('#TotalSalary').val().split(' ')[4];
                //if (Total == "$") {
                //    Total = 0;
                //}
                //$("#tableDivSalary").find('#TotalSalary').val("Total" + " " + SalaryType + " " + "Salary is" + " " + Currencytext + " " + Total);
                 TotalSalarySet();
                //DataTable2Design();
                //DataTable4Design();
                $('[data-toggle="tooltip"]').tooltip();
                $("#Effective").Zebra_DatePicker({
                    //direction: false,`
                    showButtonPanel: false,
                    format: 'd-m-Y',
                    onSelect: function () {
                        var fromDate = $('#Effective').val();
                        $("#validationmessagefromdate").hide();
                    }
                });
                $(".hrtoolLoader").hide();
                $(".modal-backdrop").hide();
            }
        });
    }
});

$("#btn-Show-SalaryEntitlementAdd").on('click',function () {
    $(".hrtoolLoader").show();
    var IsError = false;
    var arrayOfValues = [];
    var tableControl = document.getElementById('tblData');
    $('input:checkbox:checked', tableControl).each(function () {
        arrayOfValues.push($(this).closest('tr').find('td:first').attr('id'));
    }).get();
    var empData = arrayOfValues.join();
    var hiddenId = $("#TempSalaryID").val();
    var EmployeeID = $("#EmployeeID").val();   
    var TotalSalary = $('#EmployeeTotalSalaryAmount').val();
    var EffectiveFrom = $("#Effective").val();
    var SalaryTypeID = $("#drpSalary").val();
    var PaymentFrequencyID = $("#drpPayment").val();
    var CurrencyID = $("#drpCurrency").val();
    var Amount = $("#Amount").val();
    var ReasonforChange = $("#drpResonforChange").val();
    var SalaryComments = $("#Comments").val();
    
    var model = {
        Id: hiddenId,
        EmployeeId: empData,
        EffectiveFrom: EffectiveFrom,
        SalaryTypeID: SalaryTypeID,
        PaymentFrequencyID: PaymentFrequencyID,
        CurrencyID: CurrencyID,
        Amount: Amount,
        TotalSalary: TotalSalary,
        ReasonforChange: ReasonforChange,
        Comments: SalaryComments,
    }
    if (TotalSalary == "0" &&TotalSalary=="") {
        IsError = true;
        $("#validationmessageAmount").show();
        $("#validationmessageAmount").html("Amount required");

    }
    if (IsError) {
        $(".hrtoolLoader").hide();
        $(".modal-backdrop").hide();
        $("#btn-close-SalaryDeductionTemp").click();
    }
    else {
          
        $.ajax({
            type: "POST",
            data: JSON.stringify(model),
            url: bulkAction.SaveEntitlement,
            contentType: "application/json",
            success: function (data) {
                debugger;
                $("#addsalaryEntitlement").find('#AddSalaryEntitlementBody').html('');
                $("#addsalaryEntitlement").find('#AddSalaryEntitlementBody').html(data);
                $('#addsalaryEntitlement').modal('show');
                //$("#addsalaryEntitlementTemp").find(".salaryEntitlementTempTitle").html("ADD Entitlement");
                //$("#addsalaryEntitlementTemp").find("#btn-submit-AddSalaryEntitlementTempBody").html("Add");
                //$("#addsalaryEntitlementTemp").find('#AddSalaryEntitlementTempBody').find("#SalaryEntitlementIDTemp").val(0);
                $("#SalaryIdTemp").val(hiddenId);
                $("#EmployeeTotalSalaryEntitlementTemp").val(Amount);
                $('[data-toggle="tooltip"]').tooltip();
                $(".hrtoolLoader").hide();
                $(".modal-backdrop").hide();

            }
        });
    }
});

$("#EmployeeBenefitBody").on('click', '#btn-submit-SalaryEntitlement', function () {
    
    $(".hrtoolLoader").show();
    var iserror = false;
    var Id = $("#SalaryEntitlementIDTemp").val();
    var SalaryId = $("#SalaryIdTemp").val();
    var model = {
        Id: Id,
        EmployeeID: $("#EmployeeID").val(),
        EmployeeSalaryID: SalaryId,
        EntitlementID: $("#drp-SalaryEntitlementETemp").val(),
        FixedAmount: $("#FixedAmountETemp").val(),
        PercentOfSalary: $("#txt_PercentOfSalaryETemp").val(),
        IncludeInSalary: $("#check_IncludeSalaryETemp").is(":checked"),
        Comments: $("#CommentsETemp").val(),
    }
    if (model.EntitlementID == "0") {
        iserror = true;
        $("#lbl-error-EntitlementList").show();
    }
    if (model.FixedAmount == "" || model.FixedAmount == "0" || model.FixedAmount == "0.00") {
        iserror = true;
        $("#validationmessageFixedAmount").show();
        $("#validationmessageFixedAmount").html("Fixed Amount is required.");
    }
    if (model.FixedAmount == "" || model.PercentOfSalary == "0" || model.PercentOfSalary == "0.00") {
        iserror = true;
        $("#validationmessagePercentOfSalary").show();
        $("#validationmessagePercentOfSalary").html("Percent of salary is required.");
    }
    if (iserror) {
        $(".hrtoolLoader").hide();
        $(".modal-backdrop").hide();
        return false;
    }
    else {
        $.ajax({
            type: "POST",
            data: JSON.stringify(model),
            url: bulkAction.SaveSalaryEntitlement,
            contentType: "application/json",
            success: function (data) {
                $('#AddSalaryBody').html('');
                $('#AddSalaryBody').html(data);
                //$("#tableDivSalary").find(".salaryTitle").html("Add Salary");
                //$("#tableDivSalary").find("#btn-submit-Salary").html("ADD");
                $('#addsalaryEntitlement').modal('hide');
                //var CurrencyID = $("#tableDivSalary").find("#drpCurrency").val();
                //var SalaryTypeID = $("#tableDivSalary").find("#drpSalary").val();
                //var Currencytext = $("#drpCurrency option[value=" + CurrencyID + "]").text().split('-')[1];
                //var SalaryType = $("#drpSalary option[value=" + SalaryTypeID + "]").text().split('-')[0];
                //var Total = $("#tableDivSalary").find('#TotalSalary').val().split(' ')[4];
                //if (Total == "$") {
                //    Total = 0;
                //}
                //$("#tableDivSalary").find('#TotalSalary').val("Total" + " " + SalaryType + " " + "Salary is" + " " + Currencytext + " " + Total);
                TotalSalarySet();
                //DataTable4Design();
                //DataTable5Design();
                $('[data-toggle="tooltip"]').tooltip();
                $("#Effective").Zebra_DatePicker({
                    //direction: false,`
                    showButtonPanel: false,
                    format: 'd-m-Y',
                    onSelect: function () {
                        var fromDate = $('#Effective').val();
                        $("#validationmessagefromdate").hide();
                    }
                });
                $(".hrtoolLoader").hide();
                $(".modal-backdrop").hide();
            }
        });
    }
});
$("#EmployeeBenefitBody").on('click', '#btn-Cancel-SalaryEntitlement', function () {    
    $('#addsalaryEntitlement').modal('hide');
});
$('#EmployeeBenefitBody').on('keyup', '#txt_PercentOfSalaryTemp', function () {
    
    $("#validationmessageFixedAmount").hide();
    $("#validationmessagePercentOfSalary").hide();
    var total = $("#EmployeeTotalSalaryTemp").val().replace(/,/g, '');
    var fixed = $(this).val();
    if (fixed == "" || fixed == "00" || fixed == "0.00") {
        $("#validationmessagePercentOfSalary").show();
        $("#validationmessagePercentOfSalary").html("Invalid inputs.");
        if (fixed != "") {
            $(this).val('0');
        }
        $('#FixedAmountTemp').val('0');
    }

    else {
        var value = ((total * fixed) / 100).toFixed(2);
        $("#FixedAmountTemp").val(value);
    }


});
$('#EmployeeBenefitBody').on('keyup', '#FixedAmountTemp', function (event) {
    debugger;    
    $("#validationmessageFixedAmount").hide();
    $("#validationmessagePercentOfSalary").hide();
    var total = $("#EmployeeTotalSalaryTemp").val().replace(/,/g, '');
    var fixed = $("#FixedAmountTemp").val();
    if (fixed == "00" || fixed == "0.00") {

        $("#validationmessageFixedAmount").show();
        $("#validationmessageFixedAmount").html("Invalid inputs.");
        if (fixed != "") {
            $(this).val('0');
        }
        $('#txt_PercentOfSalaryTemp').val('0');
    }
    else {
        var value = ((fixed * 100) / total).toFixed(2);
        $("#txt_PercentOfSalaryTemp").val(value);
    }
});
$('#EmployeeBenefitBody').on('keyup', '#FixedAmountETemp', function () {
    $("#validationmessageFixedAmount").hide();
    $("#validationmessagePercentOfSalary").hide();
    var total = $("#EmployeeTotalSalaryEntitlementTemp").val().replace(/,/g, '');
    var fixed = $(this).val();
    if (fixed == "00" || fixed == "0.00") {
        $("#validationmessageFixedAmount").show();
        $("#validationmessageFixedAmount").html("Invalid inputs.");
        $(this).val('0');
        $('#txt_PercentOfSalaryETemp').val('0');
    }
    else {
        var value = ((fixed * 100) / total).toFixed(2);
        $("#txt_PercentOfSalaryETemp").val(value);
    }
});
// Bulk Resources

$("#EmployeeBenefitBody").on('change', '#CoverLetterfileToUpload', function (e) {    
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
                    url: bulkAction.FileUpload,
                    contentType: false,
                    processData: false,
                    data: FileUploadData,
                    success: function (result) {
                        $('#EmployeeBenefitBody').html("");
                        $('#EmployeeBenefitBody').html(result);                        
                    }
                });
            }, 500);
        }
    } else {
        $(".hrtoolLoader").hide();
        $(".modal-backdrop").hide();
    }
});

$("#EmployeeBenefitBody").on('click', "#removeResourecSheet", function () {
    document.getElementById('ResourceSheetName').innerHTML = '';
    $("#txt_ResourceSheetName").val('');
})
function ValidateEmail(email) {
    var expr = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return expr.test(email);
};
$("#EmployeeBenefitBody").on("click", "#sendResumeCV", function () {
    var isError = false;
    var arrayOfValues = [];
    var tableControl = document.getElementById('tblData');
    $('input:checkbox:checked', tableControl).each(function () {
        arrayOfValues.push($(this).closest('tr').find('td:first').attr('id'));
    }).get();
    var empData = arrayOfValues.join();
    var emailText = $("#customerEmailText").val();
    var arrayofEmail = [];
    if (emailText != "") {
        debugger;
        var data = emailText.split(',');        
        for (var i = 0; i < data.length; i++) {
            if (ValidateEmail(data[i])) {
                arrayofEmail.push(data[i]);
            }
            else {
                $("#ValidEmail").show();
                $("#ValidEmail").html("Invalid email address!");
                isError = true;
            }            
        }        
    }
    if (!isError)
        {
    $.ajax({
        type: "POST",
        url: bulkAction.sendResumecv,       
        data: { Id:empData, mail: emailText },
        success: function (result) {
            toastr.success('Send Resume/CV Successfully');
            $('#EmployeeBenefitModal').modal('hide');
        }
    });}
})
//EmployeeResourceSetting
$("#EmployeeBenefitBody").on("change", "#drpBulkBusiness", function () {

    var value = $(this).val();
    if (value != "0") {
        $.ajax({
            url: bulkAction.filDiv,
            data: { businessId: value },
            success: function (data) {

                $("#drpBulkDivision").html('');
                var toAppend = '';
                toAppend += "<option value='0'>--Select--</option>";
                $.each(data, function (index, item) {
                    toAppend += "<option value='" + item.Id + "'>" + item.Name + "</option>";
                })
                $("#drpBulkDivision").html(toAppend);
                if ($("#drpBulkDivision").val() == 0) {
                    $("#drpBulkDivision").val(0);
                    $('#drpBulkPool').val(0);
                    $('#drpBulkFunction').val(0);
                }
            }
        });
    }
    else {
        $('#drpBulkDivision').empty();
        // Bind new values to dropdown
        $('#drpBulkDivision').each(function () {
            // Create option
            var option = $("<option />");
            option.attr("value", '0').text('--Select--');
            $('#drpBulkDivision').append(option);
        });
        $('#drpBulkPool').empty();
        // Bind new values to dropdown
        $('#drpBulkPool').each(function () {
            // Create option
            var option = $("<option />");
            option.attr("value", '0').text('--Select--');
            $('#drpBulkPool').append(option);
        });
        $('#drpBulkFunction').empty();
        // Bind new values to dropdown
        $('#drpBulkFunction').each(function () {
            // Create option
            var option = $("<option />");
            option.attr("value", '0').text('--Select--');
            $('#drpBulkFunction').append(option);
        });


    }
});
$("#EmployeeBenefitBody").on("change", "#drpBulkDivision", function () {
    var value = $(this).val();
    if (value != "0") {
        $.ajax({
            url: bulkAction.getPool,
            data: { divisionId: value },
            success: function (data) {
                $("#drpBulkPool").html('');
                var toAppend = '';
                toAppend += "<option value='0'>--Select--</option>";
                $.each(data, function (index, item) {
                    toAppend += "<option value='" + item.Id + "'>" + item.Name + "</option>";
                })
                $("#drpBulkPool").html(toAppend);
                if ($("#drpBulkPool").val() == 0) {
                    $("#drpBulkPool").val(0);
                }
                $.ajax({
                    url: bulkAction.getfunction,
                    data: { divisionId: value },
                    success: function (data) {
                        $("#drpBulkFunction").html('');
                        var toAppend = '';
                        toAppend += "<option value='0'>--Select--</option>";
                        $.each(data, function (index, item) {
                            toAppend += "<option value='" + item.Id + "'>" + item.Name + "</option>";
                        })
                        $("#drpBulkFunction").html(toAppend);
                        if ($("#drpBulkFunction").val() == 0) {
                            $("#drpBulkFunction").val(0);
                        }
                    }
                });
            }
        });
    }
    else {

        $('#drpBulkPool').empty();
        // Bind new values to dropdown
        $('#drpBulkPool').each(function () {
            // Create option
            var option = $("<option />");
            option.attr("value", '0').text('--Select--');
            $('#drpBulkPool').append(option);
        });
        $('#drpBulkFunction').empty();
        // Bind new values to dropdown
        $('#drpBulkFunction').each(function () {
            // Create option
            var option = $("<option />");
            option.attr("value", '0').text('--Select--');
            $('#drpBulkFunction').append(option);
        });


    }
});

$("#EmployeeBenefitBody").on("click", "#btnResourceSetting", function () {
    var arrayOfValues = [];
    var tableControl = document.getElementById('tblData');
    $('input:checkbox:checked', tableControl).each(function () {
        arrayOfValues.push($(this).closest('tr').find('td:first').attr('id'));
    }).get();
    var empData = arrayOfValues.join();
    var model = {
        //Step 1
        EmpId: empData,                
        CompanyId: $("#drpBulkCompany").val(),
        LocationId: $("#drpBulkLocation").val(),
        BusinessId: $("#drpBulkBusiness").val(),
        DivisonId: $("#drpBulkDivision").val(),
        PoolId: $("#drpBulkPool").val(),
        FunctionId: $("#drpBulkFunction").val(),
        NoticePeriodId: $("#drpBulkNoticePeriod").val(),
        ResourceTypeId: $("#drpBulkResourceType").val(),
        ReportId: $("#selectReportToID").val(),
        AdditinalReportId: $("#AddRespID").val(),
        HRRepoId: $("#HRResID").val(),
        JobTitleId: $("#drpJobTitle").val(),
    }
    $.ajax({
        type: "POST",
        data: JSON.stringify(model),
        url: bulkAction.saveEmplSetting,
        contentType: "application/json",
        success: function (result) {
            toastr.success('Data Updated Successfully');
            $('#EmployeeBenefitModal').modal('hide');
        }
    });
})
$("#searchData").on("change", "#busId", function () {
    var value = $(this).val();
    if (value != "0") {
        $.ajax({
            url: bulkAction.filDiv,
            data: { businessId: value },
            success: function (data) {
                $("#divsionID").html('');
                var toAppend = '';
                toAppend += "<option value='0'>--Select--</option>";
                $.each(data, function (index, item) {
                    toAppend += "<option value='" + item.Id + "'>" + item.Name + "</option>";
                })
                $("#divsionID").html(toAppend);
                if ($("#divsionID").val() == 0) {
                    $("#divsionID").val(0);
                    $('#poolID').val(0);
                    $('#functionID').val(0);
                }
            }
        });
    }
    else {
        $('#divsionID').empty();
        // Bind new values to dropdown
        $('#divsionID').each(function () {
            // Create option
            var option = $("<option />");
            option.attr("value", '0').text('--Select--');
            $('#divsionID').append(option);
        });
        $('#poolID').empty();
        // Bind new values to dropdown
        $('#poolID').each(function () {
            // Create option
            var option = $("<option />");
            option.attr("value", '0').text('--Select--');
            $('#poolID').append(option);
        });
        $('#functionID').empty();
        // Bind new values to dropdown
        $('#functionID').each(function () {
            // Create option
            var option = $("<option />");
            option.attr("value", '0').text('--Select--');
            $('#functionID').append(option);
        });


    }
});
$("#searchData").on("change", "#divsionID", function () {
    var value = $(this).val();
    if (value != "0") {
        $.ajax({
            url: bulkAction.getPool,
            data: { divisionId: value },
            success: function (data) {
                $("#poolID").html('');
                var toAppend = '';
                toAppend += "<option value='0'>--Select--</option>";
                $.each(data, function (index, item) {
                    toAppend += "<option value='" + item.Id + "'>" + item.Name + "</option>";
                })
                $("#poolID").html(toAppend);
                if ($("#poolID").val() == 0) {
                    $("#poolID").val(0);
                }
                $.ajax({
                    url: bulkAction.getfunction,
                    data: { divisionId: value },
                    success: function (data) {
                        $("#functionID").html('');
                        var toAppend = '';
                        toAppend += "<option value='0'>--Select--</option>";
                        $.each(data, function (index, item) {
                            toAppend += "<option value='" + item.Id + "'>" + item.Name + "</option>";
                        })
                        $("#functionID").html(toAppend);
                        if ($("#functionID").val() == 0) {
                            $("#functionID").val(0);
                        }
                    }
                });
            }
        });
    }
    else {

        $('#poolID').empty();
        // Bind new values to dropdown
        $('#poolID').each(function () {
            // Create option
            var option = $("<option />");
            option.attr("value", '0').text('--Select--');
            $('#poolID').append(option);
        });
        $('#functionID').empty();
        // Bind new values to dropdown
        $('#functionID').each(function () {
            // Create option
            var option = $("<option />");
            option.attr("value", '0').text('--Select--');
            $('#functionID').append(option);
        });


    }
});

//Bulk AccessSetup

$("#page_content").on("click", "#btnBulkAccessSetup", function () {
    var arrayOfValues = [];
    var tableControl = document.getElementById('tblData');
    $('input:checkbox:checked', tableControl).each(function () {
        arrayOfValues.push($(this).closest('tr').find('td:first').attr('id'));
    }).get();
    var RoleId = $("#selectAccesProfile").val();
    var isError = false;
    if (RoleId == "")
    {
        isError = true;
        $("#lbl-error-empAccesProfileName").show();
    }
    var empData = arrayOfValues.join();
    var model = {
        //Step 1
        EmployeeId: empData,
        AspnetRoleId: RoleId
    }
    if (!isError) {
        $.ajax({
            type: "POST",
            data: JSON.stringify(model),
            url: bulkAction.saveBulkAspnetrole,
            contentType: "application/json",
            success: function (result) {
                toastr.success('Data Updated Successfully');
                $('#EmployeeBenefitModal').modal('hide');

            }
        });
    }
})

///////////////TempSalary
$("#page_content").on('click', '.btn-add-salaryDeductionTemp', function () {
    $(".hrtoolLoader").show();
    var IsError = false;
    var hiddenId = $("#TempSalaryID").val();
    var EmployeeID = $("#EmployeeID").val();
    //var tt = $("#tableDivSalary").find("#TotalSalary").val().split(' ')[5];
    //if (tt == null) {
    //   tt = $("#tableDivSalary").find("#TotalSalary").val().split(' ')[4];
    //}
    //var TotalSalary = tt;
    var TotalSalary = $('#EmployeeTotalSalaryAmount').val();
    var EffectiveFrom = $("#Effective").val();
    var SalaryTypeID = $("#drpSalary").val();
    var PaymentFrequencyID = $("#drpPayment").val();
    var CurrencyID = $("#drpCurrency").val();
    var Amount = $("#Amount").val();
    var ReasonforChange = $("#drpResonforChange").val();
    var SalaryComments = $("#Comments").val();

    var model = {
        Id: hiddenId,
        EmployeeId: EmployeeID,
        EffectiveFrom: EffectiveFrom,
        SalaryTypeID: SalaryTypeID,
        PaymentFrequencyID: PaymentFrequencyID,
        CurrencyID: CurrencyID,
        Amount: Amount,
        TotalSalary: TotalSalary,
        ReasonforChange: ReasonforChange,
        Comments: SalaryComments,
    }
    if (TotalSalary == "0") {
        IsError = true;
        $("#validationmessageAmount").show();
        $("#validationmessageAmount").html("Amount required");

    }
    if (IsError) {
        $(".hrtoolLoader").hide();
        $(".modal-backdrop").hide();
        $("#btn-close-SalaryDeductionTemp").click();
    }
    else {
        //$.ajax({
        //    url: constantSet.SaveSalary,
        //    data: { Id: 0, SalaryID: hiddenId },
        //    success: function (data) {
   //     $('#addsalaryDeductionTemp').modal('toggle');
     //s   $("#addsalaryDeductionTemp").modal('show');        
        $.ajax({
            type: "POST",
            data: JSON.stringify(model),
            url: bulkAction.SaveSalary,
            contentType: "application/json",
            success: function (data) {
                $("#addsalaryDeduction").find('#AddsalaryDeductionDeductionBody').html('');
                $("#addsalaryDeduction").find('#AddsalaryDeductionDeductionBody').html(data);
                $("#addsalaryDeduction").modal('show');
                $(".salaryDeductionTempTitle").html("ADD Deduction");
                $("#btn-submit-salaryDeductionTemp").html("Add");
                $('#AddsalaryDeductionDeductionBody').find("#SalaryDeductionIDTemp").val(0);
                $('#AddsalaryDeductionDeductionBody').find("#SalaryIdTemp").val(hiddenId);
                $('#AddsalaryDeductionDeductionBody').find("#EmployeeTotalSalaryTemp").val(Amount);
                $('[data-toggle="tooltip"]').tooltip();
                $(".hrtoolLoader").hide();
                $(".modal-backdrop").hide();

            }
        });
    }
});
$("#page_content").on('click', '.btn-add-SalaryEntitlementTemp', function () {

    $(".hrtoolLoader").show();
    var IsError = false;
    var hiddenId = $("#TempSalaryID").val();
    var EmployeeID = $("#EmployeeID").val();
    //var tt = $("#tableDivSalary").find("#TotalSalary").val().split(' ')[5];
    //if (tt == null) {
    //    tt = $("#tableDivSalary").find("#TotalSalary").val().split(' ')[4];
    //}
    //var TotalSalary = tt;
    var TotalSalary = $('#EmployeeTotalSalaryAmount').val();
    var EffectiveFrom = $("#Effective").val();
    var SalaryTypeID = $("#drpSalary").val();
    var PaymentFrequencyID = $("#drpPayment").val();
    var CurrencyID = $("#drpCurrency").val();
    var Amount = $("#Amount").val();
    var ReasonforChange = $("#drpResonforChange").val();
    var SalaryComments = $("#Comments").val();

    var model = {
        Id: hiddenId,
        EmployeeId: EmployeeID,
        EffectiveFrom: EffectiveFrom,
        SalaryTypeID: SalaryTypeID,
        PaymentFrequencyID: PaymentFrequencyID,
        CurrencyID: CurrencyID,
        Amount: Amount,
        TotalSalary: TotalSalary,
        ReasonforChange: ReasonforChange,
        Comments: SalaryComments,
    }
    if (TotalSalary == "0") {
        IsError = true;
        $("#validationmessageAmount").show();
        $("#validationmessageAmount").html("Amount required");

    }
    if (IsError) {
        $(".hrtoolLoader").hide();
        $(".modal-backdrop").hide();
        $("#btn-close-SalaryDeductionTemp").click();
    }
    else {
        //$.ajax({
        //    url: constantSet.SaveSalary,
        //    data: { Id: 0, SalaryID: hiddenId },
        //    success: function (data) {
       // $('#addsalaryEntitlement').modal('toggle');
       // $('#addsalaryEntitlement').modal('show');
        $.ajax({
            type: "POST",
            data: JSON.stringify(model),
            url: bulkAction.SaveEntitlement,
            contentType: "application/json",
            success: function (data) {
                $("#addsalaryEntitlementTemp").find("#AddSalaryEntitlementTempBody").html('');
                $("#addsalaryEntitlementTemp").find("#AddSalaryEntitlementTempBody").html(data);
                $("#addsalaryEntitlementTemp").modal('show');
               // $("#addsalaryEntitlement").find(".salaryEntitlementTempTitle").html("ADD Entitlement");
               // $("#addsalaryEntitlement").find("#btn-submit-SalaryEntitlementTemp").html("Add");
              //  $("#addsalaryEntitlement").find('#AddSalaryEntitlementBody').find("#SalaryEntitlementIDTemp").val(0);
                $("#addsalaryEntitlementTemp").find('#AddSalaryEntitlementTempBody').find("#SalaryIdTemp").val(hiddenId);
                $("#addsalaryEntitlementTemp").find('#AddSalaryEntitlementTempBody').find("#EmployeeTotalSalaryEntitlementTemp").val(Amount);
               // $('[data-toggle="tooltip"]').tooltip();
                $(".hrtoolLoader").hide();
                $(".modal-backdrop").hide();
            }
        });
    }
});
$("#page_content").on('click', '#btn-submit-SalaryEntitlementTemp', function () {

    $(".hrtoolLoader").show();
    var iserror = false;
    var Id = $("#SalaryEntitlementIDTemp").val();
    var SalaryId = $("#SalaryIdTemp").val();
    var model = {
        Id: Id,
        EmployeeID: $("#EmployeeID").val(),
        EmployeeSalaryID: SalaryId,
        EntitlementID: $("#drp-SalaryEntitlementETemp").val(),
        FixedAmount: $("#FixedAmountETemp").val(),
        PercentOfSalary: $("#txt_PercentOfSalaryETemp").val(),
        IncludeInSalary: $("#check_IncludeSalaryETemp").is(":checked"),
        Comments: $("#CommentsETemp").val(),

    }
    if (model.EntitlementID == "0") {
        iserror = true;
        $("#lbl-error-EntitlementList").show();
    }
    if (model.FixedAmount == "" || model.FixedAmount == "0" || model.FixedAmount == "0.00") {
        iserror = true;
        $("#validationmessageFixedAmount").show();
        $("#validationmessageFixedAmount").html("Fixed Amount is required.");
    }
    if (model.FixedAmount == "" || model.PercentOfSalary == "0" || model.PercentOfSalary == "0.00") {
        iserror = true;
        $("#validationmessagePercentOfSalary").show();
        $("#validationmessagePercentOfSalary").html("Percent of salary is required.");
    }
    if (iserror) {
        $(".hrtoolLoader").hide();
        $(".modal-backdrop").hide();
        return false;
    }
    else {
        $.ajax({
            type: "POST",
            data: JSON.stringify(model),
            url: bulkAction.saveSalarytEntitTemp,
            contentType: "application/json",
            success: function (data) {               
                $("#tableDivSalary").find('#AddSalaryBody').html('');
                $("#tableDivSalary").find('#AddSalaryBody').html(data);
                $("#tableDivSalary").find(".salaryTitle").html("Add Salary");
                $("#tableDivSalary").find("#btn-submit-Salary").html("ADD");
                $('#addsalaryEntitlement').modal('hide');
                //var CurrencyID = $("#tableDivSalary").find("#drpCurrency").val();
                //var SalaryTypeID = $("#tableDivSalary").find("#drpSalary").val();
                //var Currencytext = $("#drpCurrency option[value=" + CurrencyID + "]").text().split('-')[1];
                //var SalaryType = $("#drpSalary option[value=" + SalaryTypeID + "]").text().split('-')[0];
                //var Total = $("#tableDivSalary").find('#TotalSalary').val().split(' ')[4];
                //if (Total == "$") {
                //    Total = 0;
                //}
                //$("#tableDivSalary").find('#TotalSalary').val("Total" + " " + SalaryType + " " + "Salary is" + " " + Currencytext + " " + Total);
                TotalSalarySet();
                //DataTable4Design();
                //DataTable5Design();
                $('[data-toggle="tooltip"]').tooltip();
                $("#Effective").Zebra_DatePicker({
                    //direction: false,`
                    showButtonPanel: false,
                    format: 'd-m-Y',
                    onSelect: function () {
                        var fromDate = $('#Effective').val();
                        $("#validationmessagefromdate").hide();
                    }
                });
                $(".hrtoolLoader").hide();
                $(".modal-backdrop").hide();
            }
        });
    }
});
$("#page_content").on('change', "#drpCurrency", function () {
    TotalSalarySet();
});

$("#page_content").on("change", "#drpSalary", function () {
    TotalSalarySet();
});
$("#page_content").on('click', '#btn-Cancel-SalaryEntitlementTemp', function () {
    //$("#addsalaryEntitlementTemp").hide();
    $('#addsalaryEntitlementTemp').modal('hide');

});



