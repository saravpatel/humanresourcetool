
$(document).ready(function () {
    //$("#page_content").find(".Datepiker").Zebra_DatePicker({
    //    showButtonPanel: false,
    //    format: 'd-m-Y',
    //    onSelect: function () {
    //        iserror = false;
    //        $("#ValidNextProbationReviewDate").hide();
    //        $("#lbl-error-GreaterEndDate").hide();
    //        var startdate = $("#ProbationEndDate").val();
    //        var enddate = $("#NextProbationReviewDate").val();
    //        calculateDateDiff(startdate, enddate);
    //    }
    //});
    $("#page_content").find("#ProbationEndDate").Zebra_DatePicker({
        showButtonPanel: false,
        format: 'd-m-Y',
        onSelect: function () {
            iserror = false;
            $("#ValidProbationEndDate").hide();
            $("#lbl-error-GreaterEndDate").hide();
            var startdate = $("#ProbationEndDate").val();
            var enddate = $("#NextProbationReviewDate").val();
            calculateDateDiff(startdate, enddate);
            var FixedTermDate = $("#FixedTermEndDate").val();
            calculateFixedTermDateDiff(startdate, FixedTermDate);
        }
    });
    $("#page_content").find("#NextProbationReviewDate").Zebra_DatePicker({
        showButtonPanel: false,
        format: 'd-m-Y',
        onSelect: function () {
            iserror = false;
            $("#ValidNextProbationReviewDate").hide();
            $("#lbl-error-GreaterEndDate").hide();
            var startdate = $("#ProbationEndDate").val();
            var enddate = $("#NextProbationReviewDate").val();
            calculateDateDiff(startdate, enddate);
        }
    });
    $("#page_content").find("#FixedTermEndDate").Zebra_DatePicker({
        showButtonPanel: false,
        format: 'd-m-Y',
        onSelect: function()
        {
            $("#ValidFixedTermEndDate").hide();
            var startdate = $("#ProbationEndDate").val();
            var enddate = $("#FixedTermEndDate").val();
            calculateFixedTermDateDiff(startdate, enddate);
        }
    });
});

function calculateFixedTermDateDiff(stratDate, endDate) {
    if (stratDate != "" || endDate != "") {
        if (StartDateValidation(stratDate, endDate)) {
            $("#ValidFixedTermEndDate").show();
            $("#FixedTermEndDate").val('');
        }

    }
}

function calculateDateDiff(stratDate, endDate) {
    if (stratDate != "" || endDate != "") {
        if (StartDateValidation(stratDate, endDate)) {
            $("#lbl-error-GreaterEndDate").show();
            $("#NextProbationReviewDate").val('');
        }
        
    }
}
$("#btn-save").click(function () {
    var iserror = false;
    var employee = $("#currntEmployeeId").val();
    var probationEndDate = $("#ProbationEndDate").val();
    var nextProbationReviewDate = $("#NextProbationReviewDate").val();
    var noticePeriod = $("#drpNoticePeriod").val();
    var fixedTermEndDate = $("#FixedTermEndDate").val();
    var methodofRecruitmentSetup = $("#MethodofRecruitmentSetup").val();
    var recruitmentCost = $("#RecruitmentCost").val();
    var thisYearHolidays = $("#Thisyear").val();
    var nextYearHolidays = $("#Nextyear").val();
    var holidayEnti = $("#HelpmecalculateEmployeeMent").find("#FullTimeEntitlement").val();
    var ActivityId = $("#selectActivitytype").val();
    var RecovryRate=$("#RecovryRate").val();
    if (probationEndDate == "") {
        iserror = true;
        $("#ValidProbationEndDate").show();
        $("#ValidProbationEndDate").html("The Probation End Date is required.");
    }
    //if (nextProbationReviewDate == "") {
    //    iserror = true;
    //    $("#ValidNextProbationReviewDate").show();
    //    $("#ValidNextProbationReviewDate").html("The Next Probation Review Date is required.");
    //}
    if (noticePeriod == 0) {
        iserror = true;
        $("#ValidNoticePeriod").show();
        $("#ValidNoticePeriod").html("The Notice Period is required.");
    }

    //if (fixedTermEndDate == "") {
    //    iserror = true;
    //    $("#ValidFixedTermEndDate").show();
    //    $("#ValidFixedTermEndDate").html("The fixed Term End Date is required.");
    //}
    if (iserror) {
        $(".hrtoolLoader").hide();
        $(".modal-backdrop").hide();
        return false;
    }
    else {
        $(".hrtoolLoader").show();
        $.ajax({
            type: 'POST',
            url: constantEmployment.updateEmployment,
            data: { EmployeeId: employee, ProbationEndDate: probationEndDate, NextProbationReviewDate: nextProbationReviewDate, NoticePeriod: noticePeriod, FixedTermEndDate: fixedTermEndDate, MethodofRecruitmentSetup: methodofRecruitmentSetup, RecruitmentCost: recruitmentCost, ThisYearHolidays: thisYearHolidays, NextYearHolidays: nextYearHolidays, holidayEnti: holidayEnti, ActivityTypeId: ActivityId, rate: RecovryRate },
            success: function (data) {
                
                $(".hrtoolLoader").hide();
                $(".modal-backdrop").hide();

                $(".toast-info").show();
                setTimeout(function () { $(".toast-info").hide(); }, 1500);
            }
        });
    }
});
$("#page_content_inner").on('click', '#btn-submit-Helpmecalculate', function () {  
    $(".hrtoolLoader").show();
    var iserror = false;
    var FullTimeEntitlement = $("#HelpmecalculateEmployeeMent").find("#FullTimeEntitlement").val();
    if (FullTimeEntitlement == "") {
        iserror = true;
        $("#ValidFullTimeEntitlement").show();
        $("#ValidFullTimeEntitlement").html("FullTime Entitlement is required.");
        $(".hrtoolLoader").hide();
        $(".modal-backdrop").hide();
    }
    var DaysPerWeek = $("#HelpmecalculateEmployeeMent").find("#DaysPerWeek").val();
    var holidayEnti = $("#HelpmecalculateEmployeeMent").find("#FullTimeEntitlement").val();
    if (DaysPerWeek == "") {
        iserror = true;
        $("#ValidDaysPerWeek").show();
        $("#ValidDaysPerWeek").html("Days Per Week is required.");
        $(".hrtoolLoader").hide();
        $(".modal-backdrop").hide();
    }

    var EntitlementIncludesPublicHoliday = $("#HelpmecalculateEmployeeMent").find("#EntitlementIncludesPublicHoliday").val();
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

        var EmployeeID = $("#page_content_inner").find("#currntEmployeeId").val();

        var model =
            {
                EmployeeID:EmployeeID,
                //StartDate: startDate,
                //CountryId: jobContry,
                FullTimeEntitlement: FullTimeEntitlement,
                DaysPerWeek: DaysPerWeek,
                IncludePublicHolidays: EntitlementIncludesPublicHoliday
            }

        $.ajax({
            type: "POST",
            data: JSON.stringify(model),
            url: constantEmployment.HelpCalculate,
            contentType: "application/json",
            success: function (result) {
                
                $("#page_content").find("#Thisyear").val(result[0].Text);
                $("#page_content").find("#Nextyear").val(result[1].Text);
                $(".hrtoolLoader").hide();
                $(".modal-backdrop").hide();
                $("#HelpmecalculateEmployeeMent").hide();
                $(".toast-success").show();
                setTimeout(function () { $(".toast-success").hide(); }, 1500);
            }
        });
    }

});