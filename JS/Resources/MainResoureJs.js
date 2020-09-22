var IsWorker = false;
var IsCustomer = false;
var bol = false;
$(document).ready(function () {
    DataTableDesign();
    DataTable2Design();
    $("#checkWorker").val("W");
    $('#ResoureListtable_paginate').show();
    $("#ResoureListtable_next").enabled = true;
    $("#ResoureListtable_previous").enabled = true;
});
function DataTableDesign() {
    $('#tableDivResourceList tfoot tr').appendTo('#ResoureListtable thead');
    var table = $("#ResoureListtable").DataTable({
        "sDom": '<"top"i>rt<"bottom"flp><"clear">',
        "scrollX":true
    });
    $("#tableDivResourceList").find(".dataTables_filter").hide();
    $("#tableDivResourceList").find(".dataTables_info").hide();
    $("#tableDivResourceList thead .FirstName").keyup(function () {
        table.column(0).search(this.value).draw();
    });
    $("#tableDivResourceList thead .JobTitleName").keyup(function () {
        table.column(1).search(this.value).draw();
    });
    $("#tableDivResourceList thead .CompanyName").keyup(function () {
        table.column(2).search(this.value).draw();
    });
    $("#tableDivResourceList thead .BusinessName").keyup(function () {
        table.column(3).search(this.value).draw();
    });
    $("#tableDivResourceList thead .DivisionName").keyup(function () {
        table.column(4).search(this.value).draw();
    });
    $("#tableDivResourceList thead .PoolName").keyup(function () {
        table.column(5).search(this.value).draw();
    });
    $("#tableDivResourceList thead .FunctionName").keyup(function () {
        table.column(6).search(this.value).draw();
    });
    $("#tableDivResourceList thead .ResourceTypeName").keyup(function () {
        table.column(7).search(this.value).draw();
    });
    $("#tableDivResourceList thead .OverallScore").keyup(function () {
        table.column(8).search(this.value).draw();
    });
    $("#tableDivResourceList thead .CoreStrengths").keyup(function () {
        table.column(9).search(this.value).draw();
    });
    $("#tableDivResourceList thead .TotalSkill").keyup(function () {
        table.column(10).search(this.value).draw();
    });
    $("#tableDivResourceList thead .NumberofSkillsEndorsed").keyup(function () {
        table.column(11).search(this.value).draw();
    });
    $("#tableDivResourceList thead .NoOfEndorsmntReceive").keyup(function () {
        table.column(12).search(this.value).draw();
    });
    $("#tableDivResourceList thead .NationalityName").keyup(function () {
        table.column(13).search(this.value).draw();
    });
    $("#tableDivResourceList thead .ContryName").keyup(function () {
        table.column(14).search(this.value).draw();
    });
    $("#tableDivResourceList thead .LengthofService").keyup(function () {
        table.column(15).search(this.value).draw();
    });

}
function DataTable2Design() {
    $('#tableDivCustomerList tfoot tr').appendTo('#CustomerListtable thead');
    var tableCoustomer = $("#page_content").find("#CustomerListtable").DataTable({
        "sDom": '<"top"i>rt<"bottom"flp><"clear">',
        "scrollX":true
    });

    $("#tableDivCustomerList").find(".dataTables_filter").hide();
    $("#tableDivCustomerList").find(".dataTables_info").hide();
    $("#tableDivCustomerList thead .FirstName").keyup(function () {
        table.column(0).search(this.value).draw();
    });
    $("#tableDivCustomerList thead .JobTitleName").keyup(function () {
        table.column(1).search(this.value).draw();
    });
    $("#tableDivCustomerList thead .CompanyName").keyup(function () {
        table.column(2).search(this.value).draw();
    });
    $("#tableDivCustomerList thead .BusinessName").keyup(function () {
        table.column(3).search(this.value).draw();
    });
    $("#tableDivCustomerList thead .DivisionName").keyup(function () {
        table.column(4).search(this.value).draw();
    });
    $("#tableDivCustomerList thead .PoolName").keyup(function () {
        table.column(5).search(this.value).draw();
    });
    $("#tableDivCustomerList thead .FunctionName").keyup(function () {
        table.column(6).search(this.value).draw();
    });
    $("#tableDivCustomerList thead .ResourceTypeName").keyup(function () {
        table.column(7).search(this.value).draw();
    });
    $("#tableDivCustomerList thead .OverallScore").keyup(function () {
        table.column(8).search(this.value).draw();
    });
    $("#tableDivCustomerList thead .CoreStrengths").keyup(function () {
        table.column(9).search(this.value).draw();
    });
    $("#tableDivCustomerList thead .TotalSkill").keyup(function () {
        table.column(10).search(this.value).draw();
    });
    $("#tableDivCustomerList thead .NumberofSkillsEndorsed").keyup(function () {
        table.column(11).search(this.value).draw();
    });
   
}
function ValidateEmail(email) {
    var expr = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return expr.test(email);
};
function calculateDateDiff(stratDate, endDate) {
    $("#ValidProbationEndDate").hide();
    if (stratDate != "" || endDate != "") {
        if (StartDateValidation(stratDate, endDate)) {
            $("#lbl-error-GreaterEndDate").show();
            $("#NextProbationReviewDate").val('');
        }
        
    }
}
//Worker
function calculateFixedTermEdDate(stratDate, endDate)
{
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
function calculateprobEndDate(stratDate, endDate)
{
    if (stratDate != "" || endDate != "") {
        if (StartDateValidation(stratDate, endDate)) {
            $("#lbl-error-ValidProbationEndDate").show();
            $("#ProbationEndDate").val('');
        }
        else
        {
            $("#lbl-error-ValidProbationEndDate").hide();
        }

    }
}
$('#page_content').on('click', '#AddWorkerresource', function () {
    $(".hrtoolLoader").show();
    $("#checkWorker").val("W")
    $('#AddCustomersresourece').removeClass('active');
    $("#page_content").find('#tableDivResourceList').html('');
    $.ajax({
        url: constantSetResoure.WorkerResource,
        data: {},
        success: function (partialView) {
           
            $('#AddWorkerresource').addClass('active');
            
            $("#page_content").find('#tableDivResourceList').append(partialView);
            $("#page_content").find('#tableDivCustomerList').html('');
            DataTableDesign();
            $(".ResourcListCount").show();
            $(".CustomerListCount").hide();
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();

        }
    });
});
function validDOB(dob)
{
    //var today = new Date();
    //var thisYear = 0;
    //debugger;
  
    var now = new Date();
    var a = dob.split(" ");
    var d = a[0].split("-");
    var date = new Date();
    var age = now.getFullYear() - d[2];  
    if(isNaN(age)||age < 18)
    {
        $("#page_content").find("#validAge").show();
        $("#page_content").find("#DateOfBirth").val('');
    }
    else {
        $("#page_content").find("#validAge").hide();
    }
}
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
    $("#page_content").find('#AddResoureceBody').find("#FixedTermEndDate").Zebra_DatePicker({
        format: 'd-m-Y',
        show_icon: true,
        always_visible: Isvisible,
        onSelect: function () {
            $("#page_content").find('#ValidFixedTermEndDate').hide();
            var startDate = $("#StartDate").val();
            var proEndDate = $("#FixedTermEndDate").val();
            calculateFixedTermEdDate(startDate, proEndDate);
        }
    });
}
$("#tableDivResourceList").on('click', '.btn-add-Resoure', function () {
    IsWorker = true;
    $(".hrtoolLoader").show();
    $.ajax({
        url: constantSetResoure.addEdit,
        data: { Id: 0 },
        success: function (data) {
            $("#tableDivResourceList").find('#AddResoureceBody').html('');
            $("#tableDivResourceList").find('#AddResoureceBody').html(data);
            $("#tableDivResourceList").find("#btn-submit-Resoure").html("Add");
            $('[data-toggle="tooltip"]').tooltip();
            $("#tableDivResourceList").find('#wizard').smartWizard({
                onLeaveStep: leaveAStepCallback,
                onFinish: onFinishCallback
            });
            $("#page_content").find("#tableDivResourceList").find("#ProbationEndDate").Zebra_DatePicker({
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {
                    //$("#page_content").find("#tableDivResourceList").find("#ValidProbationEndDate").hide();
                    $("#ValidProbationEndDate").hide();
                    $("#page_content").find("#tableDivResourceList").find("#lbl-error-GreaterEndDate").hide();
                    var stDate = $("#StartDate").val();
                    var startdate = $("#page_content").find("#tableDivResourceList").find("#ProbationEndDate").val();
                    var enddate = $("#page_content").find("#tableDivResourceList").find("#NextProbationReviewDate").val();
                    calculateDateDiff(startdate, enddate);
                    calculateprobEndDate(stDate, startdate);
                }
            });
            $("#page_content").find("#NextProbationReviewDate").Zebra_DatePicker({
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {
                    $("#page_content").find("#ValidNextProbationReviewDate").hide();
                    $("#page_content").find("#lbl-error-GreaterEndDate").hide();
                    var startdate = $("#page_content").find("#ProbationEndDate").val();
                    var enddate = $("#page_content").find("#NextProbationReviewDate").val();
                    calculateDateDiff(startdate, enddate);
                }
            });
            $("#tableDivResourceList").find('#AddResoureceBody').find('.buttonNext').addClass('btn btn-warning');
            $("#tableDivResourceList").find('#AddResoureceBody').find('.buttonPrevious').addClass('btn btn-warning');
            $("#tableDivResourceList").find('#AddResoureceBody').find('.buttonFinish').addClass('btn btn-success');
            $("#tableDivResourceList").find('#AddResoureceBody').find('.buttonPrevious').hide();
            $("#tableDivResourceList").find('#AddResoureceBody').find('.buttonFinish').hide();
            $("#page_content").find("#tableDivResourceList").find('#AddResoureceBody').find("#DateOfBirth").Zebra_DatePicker({
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {
                    $("#page_content").find('#ValidDateOfBirth').hide();
                    var dob = $("#page_content").find("#tableDivResourceList").find('#AddResoureceBody').find("#DateOfBirth").val();
                    validDOB(dob);
                }
            });
            $("#page_content").find("#StartDate").Zebra_DatePicker({
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {
                    $("#page_content").find("#tableDivResourceList").find('#AddResoureceBody').find('#ValidStartDate').hide();
                    var startDate = $("#StartDate").val();
                    var proEndDate = $("#ProbationEndDate").val();
                    calculateprobEndDate(startDate, proEndDate);
                    var proEndFixDate = $("#FixedTermEndDate").val();
                    calculateFixedTermEdDate(startDate, proEndFixDate);
                }
            });
            
            $("#page_content").find('#AddResoureceBody').find("#FixedTermEndDate").Zebra_DatePicker({
                showButtonPanel: false,
                format: 'd-m-Y',                
                onSelect: function () {
                    $("#page_content").find("#tableDivResourceList").find('#ValidFixedTermEndDate').hide();
                    $("#page_content").find("#tableDivResourceList").find("#lbl-error-ValidFDEndDate").hide();
                    var startDate = $("#StartDate").val();
                    var proEndDate = $("#FixedTermEndDate").val();
                    calculateFixedTermEdDate(startDate, proEndDate);
                }
            });

            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();

            //searchCopyFrom();

        }
    });
});
$("#tableDivResourceList").on('click', '.btn-edit-Resoure', function () {
    var sessionValue = $("#hdnSession").data('value');    
    EmployeeId = $("#page_content").find("#ResoureListtable tbody").find('.selected').attr("id");   
    if (EmployeeId == undefined)
    {
        alert("Please Select Employee.");
    }
    //var link = '@Url.Action("Index", "EmployeeResume", new {' + EmployeeId + ' })';
    else
        {
    if (sessionValue == "True")
    {
        var baseUrl = constantSetResoure.redirectUrlCustomer;
        baseUrl = baseUrl.replace("newId", EmployeeId);
        window.location.href = baseUrl;
     }
    else{
    var baseUrl = constantSetResoure.redirectURL;
    baseUrl = baseUrl.replace("newId", EmployeeId);
    window.location.href = baseUrl;
        }
    }
});
$("#page_content").on('click', '.dataTr', function () {

    if ($(this).hasClass('dataTr')) {
        $("#page_content").find('#ResoureListtable tbody').find('tr.selected').removeClass('selected');
        $(this).addClass('selected');
        $("#page_content").find(".btn-edit-Resoure").removeAttr('disabled');
        $("#page_content").find(".btn-delete-Resoure").removeAttr('disabled');
    }
});
$('#page_content').on('dblclick', ' .dataTr', function (event) {
    var sessionValue = $("#hdnSession").data('value');
   var EmployeeId = $("#page_content").find("#ResoureListtable tbody").find('.selected').attr("id");
    if (sessionValue == "True") {
        var baseUrl = constantSetResoure.redirectUrlCustomer;
        baseUrl = baseUrl.replace("newId", EmployeeId);
        window.location.href = baseUrl;
    }
    else {
        if ($(this).hasClass('dataTr')) {
            EmployeeId = $(this)["0"].id;
            var baseUrl = constantSetResoure.redirectURL;
            baseUrl = baseUrl.replace("newId", EmployeeId);
            window.location.href = baseUrl;
        }
    }
});
function chkForSSO() {
    bol = validateSSO();

}
function validateSSO()
{
    $("#page_content").find('#ValidSSONo').hide();
    var ssoNo = $("#page_content").find("#SSO").val();
    var Id = ssoNo;
    $.ajax({
        url: constantSetResoure.validSSo,
        data: { ID: Id },
        success: function (data) {
           
            bol = JSON.stringify(data);
            if (bol==='true') {
                $('#ValidSSONo').show();
            }
            else {
               $('#ValidSSONo').hide();
            }
        }
    });
    return bol;
}

function chkForEmailId() {
    bol = validateEmailId();

}
function validateEmailId() {
    $("#page_content").find('#ValidEmail').hide();
    var EmaiId = $("#page_content").find("#Email").val();
    var Id = EmaiId;
    $.ajax({
        url: constantSetResoure.validEmaiId,
        data: { ID: Id },
        success: function (data) {

            bol = JSON.stringify(data);
            if (bol === 'true') {
                $('#ValidEmailId').show();
            }
            else {
                $('#ValidEmailId').hide();
            }
        }
    });
    return bol;
}
function leaveAStepCallback(obj, context) {
    debugger;
        if (context.fromStep == 1) {
        var iserror = false;
        if (bol == "true") {
            iserror = true;
            $("#page_content").find("#ValidSSONo").show();
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
            $("#page_content").find('#AddResoureceBody').find('.buttonNext').show();
            $("#page_content").find('#AddResoureceBody').find('.buttonPrevious').show();
            $("#page_content").find('#AddResoureceBody').find('.buttonFinish').hide();
            searchCopyFrom();
            return true;
        }
    }
       if (context.fromStep == 2) {
        var iserror = false;
        if (context.toStep == 1)
        {       
            return true;
        }
        else{
            $("#page_content").find('#AddResoureceBody').find('.buttonPrevious').show();
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
                $("#page_content").find('#AddResoureceBody').find('.buttonPrevious').show();
                return false;
            }
            else {
                $("#page_content").find('#AddResoureceBody').find('.buttonNext').show();
                $("#page_content").find('#AddResoureceBody').find('.buttonPrevious').show();
                $("#page_content").find('#AddResoureceBody').find('.buttonFinish').hide();
                return true;
            }
            $("#page_content").find('#AddResoureceBody').find("#ValidJobContry").hide();
            $("#page_content").find('#AddResoureceBody').find("#ValidLocation").hide();
        }
    }
    if (context.fromStep == 3) {
        if (context.toStep == 2) {
            return true;
        }
        var iserror = false;
        var StartDate = $("#page_content").find("#StartDate").val();
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
        var NextProbationReviewDate = $("#page_content").find("#NextProbationReviewDate").val();
        //if (NextProbationReviewDate == "") {
        //    iserror = true;
        //    $("#ValidNextProbationReviewDate").show();
        //    $("#ValidNextProbationReviewDate").html("The Next Probation Review Date is required.");
        //}
        var NoticePeriod = $("#page_content").find("#drpNoticePeriod").val();
        if (NoticePeriod == 0) {
            iserror = true;
            $("#ValidNoticePeriod").show();
            $("#ValidNoticePeriod").html("The Notice Period is required.");
        }
        var NextProbationReviewDate = $("#page_content").find("#NextProbationReviewDate").val();
        //if (NextProbationReviewDate == "") {
        //    iserror = true;
        //    $("#ValidNextProbationReviewDate").show();
        //    $("#ValidNextProbationReviewDate").html("The Next Probation Review Date is required.");
        //}
        var FixedTermEndDate = $("#page_content").find("#FixedTermEndDate").val();
        //if (bol == "false")
        //{
        //    $("#FixedTermEndDate").val('');
        //    iserror = false;
        //}
        //else if (bol == "true") {
        //    iserror = true;
        //    if (FixedTermEndDate == "") {
        //        iserror = true;
        //        $("#ValidFixedTermEndDate").show();
        //        $("#ValidFixedTermEndDate").html("The Fixed Term End Date is required.");
        //    }
        //}
        if (iserror) {
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
            return false;
        }
        else {
            $("#page_content").find('#AddResoureceBody').find('.buttonNext').show();
            $("#page_content").find('#AddResoureceBody').find('.buttonPrevious').show();
            $("#page_content").find('#AddResoureceBody').find('.buttonFinish').hide();
            
            return true;
        }
    }
    if (context.fromStep == 4) {
        if (context.toStep == 3) {
            return true;
        }
        var iserror = false;
        var test = $("#page_content").find('#SWIFT_Code').val();
        var drpCountry = $("#page_content").find("#drpCountry").val();
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
            $("#page_content").find('#AddResoureceBody').find('.buttonNext').show();
            $("#page_content").find('#AddResoureceBody').find('.buttonPrevious').show();
            $("#page_content").find('#AddResoureceBody').find('.buttonFinish').hide();
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
            $("#page_content").find('#AddResoureceBody').find('.buttonNext').show();
            $("#page_content").find('#AddResoureceBody').find('.buttonPrevious').show();
            $("#page_content").find('#AddResoureceBody').find('.buttonFinish').show();
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
            $("#page_content").find('#AddResoureceBody').find('.buttonNext').show();
            $("#page_content").find('#AddResoureceBody').find('.buttonPrevious').show();
            $("#page_content").find('#AddResoureceBody').find('.buttonFinish').hide();
            return true;
        }
    }
    else {

        $("#page_content").find('#AddResoureceBody').find('.buttonNext').show();
        $("#page_content").find('#AddResoureceBody').find('.buttonPrevious').show();
        $("#page_content").find('#AddResoureceBody').find('.buttonFinish').hide();
    }
}
function onFinishCallback() {
    debugger;
    $(".hrtoolLoader").show();
    //var Gender = $('input[name=Gender]:checked').val();
    var Gender = $("#AddResoureceBody").find('.selectGender input:checked').val();
    var iserror = false;
    var hiddenId = $("#tableDivResourceList").find("#hidden-Id").val();
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
    var SWIFT_Code = $("#page_content").find("#SWIFT_Code").val();
    var IDD = $("#page_content").find("#hidden-Id").val();
    var includePubHoliday = $("#EntitlementIncludesPublicHoliday").val();
    var holidayEt = $("#FullTimeEntitlement").val();
    //HolidayEntitlement
    var model = {
        //Step 1
        ApplicantID: $("#page_content").find("#Applicant_ID").val(),
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

        Reportsto: $("#page_content").find("#selectReportToID").val(),
        AdditionalReportsto: $("#page_content").find("#AddRespID").val(),
        HRResponsible: $("#page_content").find("#HRResID").val(),
        JobTitle: $("#page_content").find("#drpJobTitle").val(),
        JobCountrID: $("#page_content").find("#drpJobContry").val(),
        Location: $("#page_content").find("#drpLocation").val(),
        BusinessID: $("#page_content").find("#drpBusiness").val(),
        DivisionID: $("#page_content").find("#drpDivision").val(),
        PoolID: $("#page_content").find("#drpPool").val(),
        FunctionID: $("#page_content").find("#drpFunction").val(),

        //Step 3
        StartDate: $("#page_content").find("#StartDate").val(),
        ResourceType: $("#page_content").find("#drpResourceType").val(),
        ProbationEndDate: $("#page_content").find("#ProbationEndDate").val(),
        NextProbationReviewDate: $("#page_content").find("#NextProbationReviewDate").val(),
        NoticePeriodID: $("#page_content").find("#drpNoticePeriod").val(),
        FixedTermEndDate: $("#page_content").find("#FixedTermEndDate").val(),
        MethodofRecruitmentSetup: $("#page_content").find("#MethodofRecruitmentSetup").val(),
        RecruitmentCost: $("#page_content").find("#RecruitmentCost").val(),
        curruncyID: $("#page_content").find("#drpCurruncy").val(),
        HolidaysThisYear: $("#page_content").find("#Thisyear").val(),
        HolidaysNextYear: $("#page_content").find("#Nextyear").val(),
        workPatternID: $("#page_content").find("#drpworkPattern").val(),
        IncludePublicHoliday: includePubHoliday,
        HolidayEntit: holidayEt,
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
        SWIFT_Code: $("#page_content").find('#SWIFT_Code').val(),
        IBAN_Number: $("#page_content").find('#IBAN_Number').val(),
        //step 5
        JsonNewtaskList: SelectJsonTask,
        // step 6 
        //done
    }
    $.ajax({
        type: "POST",
        data: JSON.stringify(model),
        url: constantSetResoure.SaveResoure,
        contentType: "application/json",
        success: function (result) {
            $("#page_content").find("#AddCustomersresourece").removeClass("active");
            $("#page_content").find("#AddWorkerresource").addClass("active");
            $("#AddWorkerresource").click();
            $(".hrtoolLoader").hide();
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
function validateSSN(elementValue) {
    var ssnPattern = /^[0-9]{3}\-?[0-9]{2}\-?[0-9]{4}$/;
    return ssnPattern.test(elementValue);
}
$("#page_content").on('click', '#btn-submit-AddNewTask', function () {
    
    //var IdRecord = $("#page_content").find("div.NewTaskList:last").attr('data-id');
    var IdRecord = $("#page_content").find('#NewTaskList').children().last().attr('data-id');
    var Id = $("#AddResoure").find("#AddTaskbody").find("#taskid").val();
    var Temp = $("#page_content").find("#tableDivResourceList").find("#NewTask_" + Id).find("#Tempvalue").val();
    if (IdRecord != null) {
        IdRecord++;
    }
    var iserror = false;
    var isEmpty = $("#page_content").find("#NewTaskList").val().trim();
    var NewTitle = $("#AddResoure").find("#NewTaskTitle").val();
    var NewDiscrtion = $("#AddResoure").find("#NewTaskDescription").val();
    var Assignto = $("#AddResoure").find("#drpAssign").val();
    var DueDateto = $("#AddResoure").find("#DueDate").val();
    var Statusto = $("#AddResoure").find("#drpStatus").val();
    var AlertBefore = $("#AddResoure").find("#AlertBeforeDays").val();

    if (NewTitle == "") {
        iserror = true;
        $("#ValidNewTaskTitle").show();
        $("#ValidNewTaskTitle").html("Title is required.");
    }
    if (iserror) {
        $(".hrtoolLoader").hide();
        $(".modal-backdrop").hide();
        return false;
    }
    else {
        var model =
            {
                Id: IdRecord,
                Title: NewTitle,
                Description: NewDiscrtion,
                Assign: Assignto,
                DueDate: DueDateto,
                Status: Statusto,
                AlertBeforeDays: AlertBefore,
                IdRecord: Id,
                IsTemp:Temp
            }
        $.ajax({
            type: "POST",
            data: JSON.stringify(model),
            url: constantSetResoure.AddNewTask,
            contentType: "application/json",
            success: function (result) {
                
                if (result.IdRecord > 0) {

                    $("#tableDivResourceList").find('#AddResoureceBody').find("#step-5").find("#NewTaskList").append('');
                }
                else {
                    $("#tableDivResourceList").find('#AddResoureceBody').find("#step-5").find("#NewTaskList").append(result);
                    $("#tableDivResourceList").find('#AddResoureceBody').find("#closeAddNewtask").click();
                    $(".hrtoolLoader").hide();
                    $(".modal-backdrop").hide();
                    if (Id > 0) {
                        $(".toast-info").show();
                        setTimeout(function () { $(".toast-info").hide(); }, 1500);
                    }
                    else {
                        $(".toast-success").show();
                        setTimeout(function () { $(".toast-success").hide(); }, 1500);
                    }
                }
            }
        });
    }
});
$("#page_content").on("change", "#drpBusiness", function () {

    var value = $(this).val();
    if (value != "0") {
        $.ajax({
            url: constantSetResoure.bindDiv,
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
            url: constantSetResoure.bindpool,
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
                    url: constantSetResoure.bindFuncation,
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
    $("#drpState").empty();
    $('#drpTown').empty();
    if (value != "0") {
        $.ajax({
            url: constantSetResoure.BindStateUrl,
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
                    url: constantSetResoure.BindAirPortIDUrl,
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
            url: constantSetResoure.BindCityIDUrl,
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
    
    $(".hrtoolLoader").show();
    var iserror = false;
    var FullTimeEntitlement = $("#AddResoure").find("#FullTimeEntitlement").val();
    if (FullTimeEntitlement == "") {
        iserror = true;
        $("#ValidFullTimeEntitlement").show();
        $("#ValidFullTimeEntitlement").html("FullTime Entitlement is required.");
        $(".hrtoolLoader").hide();
        $(".modal-backdrop").hide();
    }
    var DaysPerWeek = $("#AddResoure").find("#DaysPerWeek").val();
    if (DaysPerWeek == "") {
        iserror = true;
        $("#ValidDaysPerWeek").show();
        $("#ValidDaysPerWeek").html("Days Per Week is required.");
        $(".hrtoolLoader").hide();
        $(".modal-backdrop").hide();
    }
    var futureStater;
    var EntitlementIncludesPublicHoliday = $("#AddResoure").find("#EntitlementIncludesPublicHoliday").val();
    if (EntitlementIncludesPublicHoliday == "") {
        iserror = true;
        $("#EntitlementIncludes").show();
        $("#EntitlementIncludes").html("Entitlement Includes Public Holidays required.");
        $(".hrtoolLoader").hide();
        $(".modal-backdrop").hide();
    }
    if ($("#EntitlementIncludesPublicHoliday").prop("checked") == true) {
        futureStater = "ON";
    }
    else {
        futureStater = "OFF";
    }
    if (iserror) {
        $(".hrtoolLoader").hide();
        $(".modal-backdrop").hide();
        return false;
    }
    else {
        var startDate = $("#page_content").find("#StartDate").val();
        var jobContry = $("#page_content").find("#drpJobContry").val();
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
            url: constantSetResoure.HelpCalculate,
            contentType: "application/json",
            success: function (result) {
                
                $("#page_content").find("#Thisyear").val(result[0].Text);
                $("#page_content").find("#Nextyear").val(result[1].Text);
                $(".hrtoolLoader").hide();
                $(".modal-backdrop").hide();
                $("#Helpmecalculate").hide();
                $(".toast-success").show();
                setTimeout(function () { $(".toast-success").hide(); }, 1500);
            }
        });
    }

});
$("#page_content").on('click', '.btn-Refresh-Resoure', function () {
    window.location.reload();
});
$("#page_content").on('click', '.btn-ClearSorting-Resoure', function () {
    window.location.reload();
});
$("#page_content").on('click', '.btn-clearFilter-Resoure', function () {
    window.location.reload();
});
$("#tableDivProject").on('click', '.btn-delete-Project', function () {
    var id = $("#page_content").find("#ResoureListtable tbody").find('.selected').attr("id");

    $.Zebra_Dialog(" are you sure want to Delete this Record?", {
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
                        url: constantSet.DeleteProjectUrl,
                        data: { Id: id },
                        success: function (data) {
                            $("#tableDivProject").find('#addProjectBody').html('');
                            $("#tableDivProject").find('#addProjectBody').html(data);

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
$("#page_content").on('click', '.btn-delete-Resoure', function () {
    var IdRecord = $("#page_content").find("#ResoureListtable tbody").find('.selected').attr("id");
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
                        url: constantSetResoure.DeleteWorkerlist,
                        data: { Id: IdRecord },
                        success: function (data) {
                            $("#page_content").find("#tableDivResourceList").html('');
                            $("#page_content").find("#tableDivResourceList").html(data);
                            $("#page_content").find("#AddCustomersresourece").removeClass("active");
                            $("#page_content").find("#AddWorkerresource").addClass("active");
                            DataTableDesign();
                            $('[data-toggle="tooltip"]').tooltip();
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
//Coustomer Record
$("#page_content").on('click', '.dataTrCoustomer', function () {

    if ($(this).hasClass('dataTrCoustomer')) {
        $("#page_content").find('#CustomerListtable tbody').find('tr.selected').removeClass('selected');
        $(this).addClass('selected');
        $("#page_content").find(".btn-edit-Customer").removeAttr('disabled');
        $("#page_content").find(".btn-delete-Customer").removeAttr('disabled');
    }
});

$('#page_content').on('dblclick', '.dataTrCoustomer', function (event) {
    

    var sessionValue = $("#hdnSession").data('value');
    var EmployeeId = $("#page_content").find("#CustomerListtable tbody").find('.selected').attr("id");
    if (sessionValue == "True") {
        var baseUrl = constantSetResoure.redirectUrlCustomer;
        baseUrl = baseUrl.replace("newId", EmployeeId);
        window.location.href = baseUrl;
    }
    else {
        if ($(this).hasClass('dataTrCoustomer')) {
            EmployeeId = $(this)["0"].id;
            var baseUrl = constantSetResoure.redirectURL;
            baseUrl = baseUrl.replace("newId", EmployeeId);
            window.location.href = baseUrl;
        }
    }


    //if ($(this).hasClass('dataTrCoustomer')) {
    //    EmployeeId = $(this)["0"].id;
    //    var baseUrl = constantSetResoure.redirectURLEditCustomer;
    //    baseUrl = baseUrl.replace("newId", EmployeeId);
    //    window.location.href = baseUrl;
    //}
});

$('#page_content').on('click', '#AddCustomersresourece', function () {
//    IsWorker = false;
    IsCustomer = true;
    $(".hrtoolLoader").show();
    $("#checkWorker").val("C");
    $('#AddWorkerresource').removeClass('active');
    $("#page_content").find('#tableDivResourceList').html('');
    $.ajax({
        url: constantSetResoure.CustomerResource,
        data: {},
        success: function (partialView) {
            $('#AddCustomersresourece').addClass('active');
            $("#page_content").find('#tableDivResourceList').html('');
            $("#page_content").find('#tableDivCustomerList').html('');
            $("#page_content").find('#tableDivCustomerList').html(partialView);
            
            DataTable2Design();
            $(".CustomerListCount").show();
            $(".ResourcListCount").hide();
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();

        }
    });
});

$("#page_content").on('click', '.btn-add-Customer', function () {
    
    $(".hrtoolLoader").show();
    $.ajax({
        url: constantSetResoure.AddEditCoustomer,
        data: { Id: 0 },
        success: function (data) {
            $("#tableDivCustomerList").find('#AddCoustmerBody').html('');
            $("#tableDivCustomerList").find('#AddCoustmerBody').html(data);
            $("#tableDivCustomerList").find("#btn-submit-Resoure").html("Add");
            $('[data-toggle="tooltip"]').tooltip();
            $("#tableDivCustomerList").find('#wizard').smartWizard({
                onLeaveStep: leaveAStepCallbackCoustomer,
                onFinish: onFinishCallbackCoustomer
            });
            //$("#page_content").find("#AddCustomersresourece").addClass("active");

            $("#tableDivCustomerList").find('#AddCoustmerBody').find('.buttonNext').addClass('btn btn-warning');
            $("#tableDivCustomerList").find('#AddCoustmerBody').find('.buttonPrevious').addClass('btn btn-warning');
            $("#tableDivCustomerList").find('#AddCoustmerBody').find('.buttonFinish').addClass('btn btn-success');
            $("#tableDivCustomerList").find('#AddCoustmerBody').find('.buttonPrevious').hide();
            $("#tableDivCustomerList").find('#AddCoustmerBody').find('.buttonFinish').hide();            
            $("#page_content").find("#DateOfBirth").Zebra_DatePicker({
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {
                    $("#page_content").find('#ValidDateOfBirth').hide();
                    var dob = $("#page_content").find("#DateOfBirth").val();
                    validDOB(dob);

                }
            });
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();

        }
    });
});

$("#page_content").on('click', '.btn-edit-Customer', function () {  
    EmployeeId = $("#page_content").find("#CustomerListtable tbody").find('.selected').attr("id");
    var baseUrl = constantSetResoure.redirectURLEditCustomer;
    baseUrl = baseUrl.replace("newId", EmployeeId);
    window.location.href = baseUrl;
});

function leaveAStepCallbackCoustomer(obj, context) {

    

    if (context.fromStep == 1) {
        var iserror = false;

        var ssovalues = "C" + $("#page_content").find("#SSO").val();
        if (ssovalues == "0")
        {
            iserror = true;
            $("#ValidTitle").show();
            $("#ValidTitle").html("The Title is required.");
        }

       
        var Title = $("#tableDivCustomerList").find("#drpTitle").val();
        if (Title == "0") {
            iserror = true;
            $("#ValidTitle").show();
            $("#ValidTitle").html("The Title is required.");
        }
        var FirstName = $("#tableDivCustomerList").find("#FirstName").val();
        if (FirstName == "") {
            iserror = true;
            $("#ValidFirstName").show();
            $("#ValidFirstName").html("The First Name is required.");
        }
        var Lastname = $("#tableDivCustomerList").find("#LastName").val();
        if (Lastname == "") {
            iserror = true;
            $("#ValidLastName").show();
            $("#ValidLastName").html("The Last Name is required.");
        }
        var Email = $("#tableDivCustomerList").find("#Email").val();
        var CustomerCareName = $("#CustomerCare").val();
        if (CustomerCareName == "")
        {
            iserror = true;
            $("#ValidCustomerCare").show();
        }
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
        if (Gender == "1" || Gender == "0")
        { }
        else
        {
            iserror = true;
            $("#ValidGender").show();
            $("#ValidGender").html("The Gender is required.");
        }
        //var JobTitle = $("#tableDivCustomerList").find("#drpJobTitle").val();
        //if (JobTitle == "0") {
        //    iserror = true;
        //    $("#ValidJobTitle").show();
        //    $("#ValidJobTitle").html("The Job Title is required.");
        //}
        var DateOfBirth = $("#tableDivCustomerList").find("#DateOfBirth").val();
        if (DateOfBirth == "") {
            iserror = true;
            $("#ValidDateOfBirth").show();
            $("#ValidDateOfBirth").html("The Date Of Birth is required.");
        }

        if (iserror) {
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
            return false;
        }
        else {
            if (context.toStep = 2) {
                $("#page_content").find('#AddCoustmerBody').find('.buttonNext').show();
                $("#page_content").find('#AddCoustmerBody').find('.buttonPrevious').show();
                $("#page_content").find('#AddCoustmerBody').find('.buttonFinish').hide();

            }
            else {
                $("#page_content").find('#AddCoustmerBody').find('.buttonNext').hide();
                $("#page_content").find('#AddCoustmerBody').find('.buttonPrevious').show();
                $("#page_content").find('#AddCoustmerBody').find('.buttonFinish').show()
            }
            $("#page_content").find('#AddCoustmerBody').find("#lbl-error-StatusList").hide();           
            return true;
        }
        $("#page_content").find('#AddCoustmerBody').find("#lbl-error-EmployeeList").hide();
        $("#page_content").find('#AddCoustmerBody').find("#lbl-error-CategoryList").hide();
    }
    if (context.fromStep == 2) {

        if (context.toStep == 1) {
            $("#page_content").find('#AddCoustmerBody').find('.buttonNext').show();
            $("#page_content").find('#AddCoustmerBody').find('.buttonPrevious').hide();
            $("#page_content").find('#AddCoustmerBody').find('.buttonFinish').hide();
        }
        else {
            $("#page_content").find('#AddCoustmerBody').find('.buttonNext').hide();
            $("#page_content").find('#AddCoustmerBody').find('.buttonPrevious').show();
            $("#page_content").find('#AddCoustmerBody').find('.buttonFinish').show();
        }
        return true;
    }
    else {
        if (context.toStep == 2) {
            $("#page_content").find('#AddCoustmerBody').find('.buttonNext').show();
            $("#page_content").find('#AddCoustmerBody').find('.buttonPrevious').show();
            $("#page_content").find('#AddCoustmerBody').find('.buttonFinish').hide();
        }
        else {
            $("#page_content").find('#AddCoustmerBody').find('.buttonNext').show();
            $("#page_content").find('#AddCoustmerBody').find('.buttonPrevious').hide();
            $("#page_content").find('#AddCoustmerBody').find('.buttonFinish').hide();
        }

        return true;
    }

}

function onFinishCallbackCoustomer() {
    $(".hrtoolLoader").show();
    var Gender = $('input[name=Gender]:checked').val();
    var iserror = false;
    var hiddenId = $("#tableDivCustomerList").find("#hidden-Id").val();
    var check = $("#checkWorker").val();
    var ssovalues = "C" + $("#page_content").find("#SSO").val();
    var customerCare = $("#selectID").val();
    var model = {
        //Step 1
        CheckRecord: check,
        Id: $("#page_content").find("#hidden-Id").val(),
        Title: $("#page_content").find("#drpTitle").val(),
        FirstName: $("#page_content").find("#FirstName").val(),
        LastName: $("#page_content").find("#LastName").val(),
        OtherNames: $("#page_content").find("#OtherName").val(),
        KnownAs: $("#page_content").find("#Knownas").val(),
        SSO: ssovalues,
        UserNameEmail: $("#page_content").find("#Email").val(),
        Gender: Gender,
        DateOfBirth: $("#page_content").find("#DateOfBirth").val(),
        Nationality: $("#page_content").find("#drpNationality").val(),
        PhotoPath: $("#page_content").find('#fileToUpload').val(),
        Picture: $("#page_content").find('#ImageData').val(),
        WorkPhone: $("#page_content").find("#WorkPhone").val(),
        WorkMobile: $("#page_content").find("#WorkMobile").val(),
        CustomerCare: customerCare,
        SelectCustomerCompanyId: $("#page_content").find("#CoustomerCompanyListrecord").val()
    }
    $.ajax({
        type: "POST",
        data: JSON.stringify(model),
        url: constantSetResoure.SaveCoustomer,
        contentType: "application/json",
        success: function (result) {
                $("#page_content").find("#AddWorkerresource").removeClass("active");
                $("#page_content").find("#AddCustomersresourece").addClass("active");
                $("#AddCustomersresourece").click();
                $(".hrtoolLoader").hide();
                $(".modal-backdrop").hide();
            if (hiddenId > 0) {
                $(".toast-info").show();
                setTimeout(function () { $(".toast-info").hide(); }, 2500);
            }
            else {
                $(".toast-success").show();
                setTimeout(function () { $(".toast-success").hide(); }, 2500);
            }
            
        }
    });
}

$("#page_content").on('click', '.btn-delete-Coustomer', function () {
    var IdRecord = $("#page_content").find("#CustomerListtable tbody").find('.selected').attr("id");
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
                        url: constantSetResoure.DeleteCoustomerlist,
                        data: { Id: IdRecord },
                        success: function (data) {
                            $("#page_content").find("#tableDivCustomerList").html('');
                            $("#page_content").find("#tableDivCustomerList").html(result);
                            DataTable2Design();
                            $('[data-toggle="tooltip"]').tooltip();
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
$("#page_content").on('click', '.btn-Refresh-Coustomer', function () {
    window.location.reload();
});
$("#page_content").on('click', '.btn-ClearSorting-Coustomer', function () {
    window.location.reload();
});
$("#page_content").on('click', '.btn-clearFilter-Coustomer', function () {
    window.location.reload();
});
//copy
$("#page_content").on('click', '#CopySegment', function () {

    var CopyId = $("#tableDivResourceList").find("#selectID").val();
    $.ajax({
        url: constantSetResoure.copyrecordUrl,
        data: { EmployeeId: CopyId },
        success: function (data) {
           
            $("#tableDivResourceList").find('#AddResoureceBody').find("#step-2").html('');
            $("#tableDivResourceList").find('#AddResoureceBody').find("#step-2").html(data);
            $("#page_content").find(".Datepiker").Zebra_DatePicker({
                showButtonPanel: false,
                format: 'd-m-Y',
            });
            $("#tableDivResourceList").find('#AddResoureceBody').find("#step-2").find("#StartDate").val("");
            $('[data-toggle="tooltip"]').tooltip();

            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();

        }
    });

});

$("#page_content").on('click', '#ShowAddNewTask', function () {
    
    $.ajax({
        url: constantSetResoure.ShowAddNewTaskUrl,
        data: {},
        success: function (partialView) {
            $("#page_content").find('#tableDivResourceList').find("#AddTaskbody").html('');
            $("#page_content").find('#tableDivResourceList').find("#AddTaskbody").append(partialView);
            $("#page_content").find(".Datepiker").Zebra_DatePicker({
                showButtonPanel: false,
                format: 'd-m-Y',
            });


        }
    });
});

$("#page_content").on('change', '#fileToUpload', function (e) {
    
    var files = e.target.files;
    imageData = "";
    if (files.length > 0) {
        if (window.FormData !== undefined) {
            imageData = new FormData();
            for (var x = 0; x < files.length; x++) {
                imageData.append("file" + x, files[x]);
            }
        }
        $.ajax({
                type: "POST",
                url: constantSetResoure.UploadImage,
                contentType: false,
                processData: false,
                data: imageData,
                success: function (result) {
                    debugger;
                    $("#ImageData").val(result)
                    var newhref = "/Upload/Resources/" + result;
                    if (IsCustomer == true) {
                        $("#tableDivCustomerList").find(".editpic").attr("src", newhref);
                    }
                    else if (IsWorker == true) {
                        $("#tableDivResourceList").find(".editpic").attr("src", newhref);
                    }
                },
                error: function (xhr, status, p3, p4) {
                    var err = "Error " + " " + status + " " + p3 + " " + p4;
                    if (xhr.responseText && xhr.responseText[0]== "{")
                        err = JSON.parse(xhr.responseText).Message;
                    console.log(err);
                    $(".nav-md").find('.loader').hide();
                    $(".nav-md").find('.opacityDiv').hide();
                }
            });
    }
    else {
        $(".hrtoolLoader").hide();
        $(".modal-backdrop").hide();
    }
    });

$("#page_content").on('click', '.btn-delete-Customer', function () {
    var IdRecord = $("#page_content").find("#CustomerListtable tbody").find('.selected').attr("id");
    $.Zebra_Dialog(" are you sure want to Delete this Record?", {
        'type': false,
        'title': 'Delete this Record',
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
                        url: constantSetResoure.DeleteWorkerlist,
                        data: { Id: IdRecord },
                        success: function (data) {
                            $("#page_content").find("#AddWorkerresource").removeClass("active");
                            $("#page_content").find("#AddCustomersresourece").addClass("active");
                            $("#AddCustomersresourece").click();
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


$("#page_content").on('click', '.btn-Excel-Resoure', function () {



});

$("#page_content").on('click', '#FindAddess', function () {
    var isError = false;
    // var hiddenId = $("#tableDivTraining").find("#hidden-Id").val();
    var HouseNumber = $("#tableDivResourceList").find("#HouseNumber").val();
    var postCode = $("#tableDivResourceList").find("#Postcode").val();
    if (postCode == "") {
        $("#tableDivResourceList").find("#PostCode").show();
    }
    else {
            $.post('http://maps.googleapis.com/maps/api/geocode/json?address=' + postCode + '&sensor=false', function (r) {    
            var lat = r['results'][0]['geometry']['location']['lat'];
            var lng = r['results'][0]['geometry']['location']['lng'];
            $.post('http://maps.googleapis.com/maps/api/geocode/json?latlng=' + lat + ',' + lng + '&sensor=false', function (address) {
                
                $("#tableDivResourceList").find("#Address").val(HouseNumber + ", " + address['results'][0]['address_components'][1]['long_name'] + "\n" + address['results'][0]['address_components'][2]['long_name'] + "\n" + address['results'][0]['address_components'][4]['long_name'] + "\n" + address['results'][0]['address_components'][5]['long_name'] + "\n" + address['results'][0]['address_components'][6]['long_name'] + " " + address['results'][0]['address_components'][7]['long_name']);
            });
        });
        }
});

$('#page_content').on('keyup', '#Postcode', function () {
    var isError = false;
    $("#tableDivResourceList").find("#PostCode").hide();
});

$('#page_content').on('change', '#ProbationEndDate', function () {
    
    $("#ValidProbationEndDate").hide();
});


