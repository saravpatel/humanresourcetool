$(document).ready(function () {
    for (i = 1; i <= 12; i++) {
        var dlength = $("#month_" + i).find(".timeSheetIcon").length;
        if (dlength == "") {
            $("#timesheetMonthCount_" + i).text(0+" Days");
        }
        else {
            $("#timesheetMonthCount_" + i).text(dlength + " Days");
        }
    }
});

var bol = false;
var valError = false;
var BusId;
var DivId;
var PoolId;
var FunctionId;
//Filter
function GetDivisonByBusinessId(Buisenessid) {
    BusId = Buisenessid.value;
    $.ajax({
        url: AdminPlanner.GetDivisonByBusinessID,
        //type: 'POST',
        data: { BusinessID: Buisenessid.value },
        //contentType: "application/json",
        success: function (data) {
            var toAppend = '<option value="0">-- Select --</option>';
            $.each(data, function (index, item) {
                toAppend += "<option value='" + item.Key + "'>" + item.Value + "</option>";
            });
            $('#drpDivision').html('');
            $('#drpFunction').html('');
            $('#drpPool').html('');
            $('#drpDivision').html(toAppend);
        }
    });
}
function GetPoolByDivisonID(DivisonID) {
    DivId = DivisonID.value;
    $.ajax({
        url: AdminPlanner.GetPoolByDivisonID,
        //type: 'POST',
        data: { BusinessId: BusId, DivisonID: DivisonID.value },
        //contentType: "application/json",
        success: function (data) {
            debugger;
            var toAppend = '<option value="0">-- Select --</option>';
            $.each(data, function (index, item) {
                toAppend += "<option value='" + item.Key + "'>" + item.Value + "</option>";
            });
            $('#drpFunction').html('');
            $('#drpPool').html('');
            $('#drpPool').html(toAppend);
        }
    });
}
function GetFunctionByPoolId(PoolID) {
    PoolId = PoolID.value;
    $.ajax({
        url: AdminPlanner.GetFunctionByPoolId,
        //type: 'POST',
        data: { BusinessId: BusId, DivisonID: DivId },
        //contentType: "application/json",
        success: function (data) {
            var toAppend = '<option value="0">-- Select --</option>';
            $.each(data, function (index, item) {
                toAppend += "<option value='" + item.Key + "'>" + item.Value + "</option>";
            });
            $('#drpFunction').html('');
            $('#drpFunction').html(toAppend);
        }
    });
}
function GetResourceByFunctionAnyPool(functionId) {
    FunctionId = functionId.value;
    $.ajax({
        url: AdminPlanner.GetResourceByFunctionAndPoolID,
        //type: 'POST',
        data: { PoolId: PoolId, FunctionID: FunctionId },
        //contentType: "application/json",
        success: function (data) {
            var toAppend = '<option value="0">-- Select --</option>';
            $.each(data, function (index, item) {
                toAppend += "<option value='" + item.Key + "'>" + item.Value + "</option>";
            });
            $('#drpResource').html('');
            $('#drpResource').html(toAppend);
        }
    });
}
function ChangePreviousMonth(Year, IsTimesheet, IsTravel, IsAnualLeave, IsOtherLeave, IsSickLeave, IsLateLeave, IsPublicHolidays, IsMatPatLeave) {
    Year = Year - 1;
    GetResultByFilter(Year, IsTimesheet, IsTravel, IsAnualLeave, IsOtherLeave, IsSickLeave, IsLateLeave, IsPublicHolidays, IsMatPatLeave)
}
function ChangeNextMonth(Year, IsTimesheet, IsTravel, IsAnualLeave, IsOtherLeave, IsSickLeave, IsLateLeave, IsPublicHolidays, IsMatPatLeave) {
    Year = Year + 1;
    GetResultByFilter(Year, IsTimesheet, IsTravel, IsAnualLeave, IsOtherLeave, IsSickLeave, IsLateLeave, IsPublicHolidays, IsMatPatLeave);
}
function GetAllTimeSheetLeave(Year, IsTimesheet, IsTravel, IsAnualLeave, IsOtherLeave, IsSickLeave, IsLateLeave, IsPublicHolidays, IsMatPatLeave) {
    GetResultByFilter(Year, IsTimesheet, IsTravel, IsAnualLeave, IsOtherLeave, IsSickLeave, IsLateLeave, IsPublicHolidays, IsMatPatLeave);
}
function GetAllTravelLeave(Year, IsTimesheet, IsTravel, IsAnualLeave, IsOtherLeave, IsSickLeave, IsLateLeave, IsPublicHolidays, IsMatPatLeave) {  
    GetResultByFilter(Year, IsTimesheet, IsTravel, IsAnualLeave, IsOtherLeave, IsSickLeave, IsLateLeave, IsPublicHolidays, IsMatPatLeave);
}
function GetAllAnualLeave(Year, IsTimesheet, IsTravel, IsAnualLeave, IsOtherLeave, IsSickLeave, IsLateLeave, IsPublicHolidays, IsMatPatLeave)
{
    GetResultByFilter(Year, IsTimesheet, IsTravel, IsAnualLeave, IsOtherLeave, IsSickLeave, IsLateLeave, IsPublicHolidays, IsMatPatLeave);
}
function GetAllOtherLeave(Year, IsTimesheet, IsTravel, IsAnualLeave, IsOtherLeave, IsSickLeave, IsLateLeave, IsPublicHolidays, IsMatPatLeave)
{
    GetResultByFilter(Year, IsTimesheet, IsTravel, IsAnualLeave, IsOtherLeave, IsSickLeave, IsLateLeave, IsPublicHolidays, IsMatPatLeave);
}
function GetAllSickLeave(Year, IsTimesheet, IsTravel, IsAnualLeave, IsOtherLeave, IsSickLeave, IsLateLeave, IsPublicHolidays, IsMatPatLeave) {
    GetResultByFilter(Year, IsTimesheet, IsTravel, IsAnualLeave, IsOtherLeave, IsSickLeave, IsLateLeave, IsPublicHolidays, IsMatPatLeave);
}
function GetAllLateLeave(Year, IsTimesheet, IsTravel, IsAnualLeave, IsOtherLeave, IsSickLeave, IsLateLeave, IsPublicHolidays, IsMatPatLeave) {
     GetResultByFilter(Year, IsTimesheet, IsTravel, IsAnualLeave, IsOtherLeave, IsSickLeave, IsLateLeave, IsPublicHolidays, IsMatPatLeave);
}
function GetAllMaternityLeave(Year, IsTimesheet, IsTravel, IsAnualLeave, IsOtherLeave, IsSickLeave, IsLateLeave, IsPublicHolidays, IsMatPatLeave) {
     GetResultByFilter(Year, IsTimesheet, IsTravel, IsAnualLeave, IsOtherLeave, IsSickLeave, IsLateLeave, IsPublicHolidays, IsMatPatLeave);
}
function GetAllPublicHolidays(Year, IsTimesheet, IsTravel, IsAnualLeave, IsOtherLeave, IsSickLeave, IsLateLeave, IsPublicHolidays, IsMatPatLeave) {
    GetResultByFilter(Year, IsTimesheet, IsTravel, IsAnualLeave, IsOtherLeave, IsSickLeave, IsLateLeave, IsPublicHolidays, IsMatPatLeave);
}
function ApplyFilter(Year) {
    var IsTimesheet=$("#hdnTimeSheet").val();
    var IsTravel=$("#hdnTravel").val();
    var IsAnualLeave=$("#hdnAnualLeave").val();
    var IsOtherLeave= $("#hdnOtherLeave").val();
    var IsSickLeave=$("#hdnSickLeave").val();
    var IsLateLeave=$("#hdnLateLevae").val();
    var IsPublicHolidays= $("#hdnPublicHolidays").val();
    var IsMatPatLeave=$("#hdnMatPatLeave").val();
    var ChangeNext = {
        "Year": $("#drpYear").val(),
        "IsTimeSheet": $("#hdnTimeSheet").val() == 1 ? true : false,
        "IsTravel": $("#hdnTravel").val() == 1 ? true : false,
        "IsAnualLeave": $("#hdnAnualLeave").val() == 1 ? true : false,
        "IsOtherLeave": $("#hdnOtherLeave").val() == 1 ? true : false,
        "IsSickLeave": $("#hdnSickLeave").val() == 1 ? true : false,
        "IsLate": $("#hdnLateLevae").val() == 1 ? true : false,
        "IsPublicHolidays": $("#hdnPublicHolidays").val() == 1 ? true : false,
        "IsMatPatLeave": $("#hdnMatPatLeave").val() == 1 ? true : false,
        "BusinessId": $("#drpBusiness").val() == null ? 0 : $("#drpBusiness").val(),
        "DivisionId": $("#drpDivision").val() == null ? 0 : $("#drpDivision").val(),
        "PoolId": $("#drpPool").val() == null ? 0 : $("#drpPool").val(),
        "FunctionId": $("#drpFunction").val() == null ? 0 : $("#drpFunction").val(),
        "ResourceId": $("#drpResource").val() == null ? 0 : $("#drpResource").val(),
        "ProjectId": $("#drpProject").val() == null ? 0 : $("#drpProject").val(),
    }
    $.ajax({
        url: AdminPlanner.GetResultByFilter,
        type: 'POST',
        data: JSON.stringify({ _requestModel: ChangeNext }),
        contentType: "application/json",
        success: function (data) {
            debugger;
            $("#projectCalendar").html('');
            $("#projectCalendar").html(data);
            $(".nav-tabs a").each(function () {
                if (IsTimesheet == 1) {
                    $('.nav-tabs li.active').removeClass('active');
                    $("#TimesheetLeaveLink").closest('li').addClass('active');
                }
                else if (IsTravel == 1) {
                    $('.nav-tabs li.active').removeClass('active');
                    $("#TravelLeaveLink").closest('li').addClass('active');
                }
                else if (IsAnualLeave == 1) {
                    $('.nav-tabs li.active').removeClass('active');
                    $("#AnualLeaveLink").closest('li').addClass('active');
                }
                else if (IsOtherLeave == 1) {
                    $('.nav-tabs li.active').removeClass('active');
                    $("#OtherLeaveLink").closest('li').addClass('active');
                }
                else if (IsSickLeave == 1) {
                    $('.nav-tabs li.active').removeClass('active');
                    $("#SickLeaveLink").closest('li').addClass('active');
                }
                else if (IsLateLeave == 1) {
                    $('.nav-tabs li.active').removeClass('active');
                    $("#LateLeaveLink").closest('li').addClass('active');
                }
                else if (IsPublicHolidays == 1) {
                    $('.nav-tabs li.active').removeClass('active');
                    $("#PublicHolidayLink").closest('li').addClass('active');
                }
                else if (IsMatPatLeave == 1) {
                    $('.nav-tabs li.active').removeClass('active');
                    $("#Mat_PatLeaveLink").closest('li').addClass('active');
                }
            })
        }
    });
}
function GetResultByFilter(Year, IsTimesheet, IsTravel, IsAnualLeave, IsOtherLeave, IsSickLeave, IsLateLeave, IsPublicHolidays, IsMatPatLeave) {
    var ChangeNext = {
        "Year": Year,
        "IsTimeSheet": IsTimesheet == 1 ? true : false,
        "IsTravel": IsTravel == 1 ? true : false,
        "IsAnualLeave": IsAnualLeave == 1 ? true : false,
        "IsOtherLeave": IsOtherLeave == 1 ? true : false,
        "IsSickLeave": IsSickLeave == 1 ? true : false,
        "IsLate": IsLateLeave == 1 ? true : false,
        "IsPublicHolidays": IsPublicHolidays == 1 ? true : false,
        "IsMatPatLeave": IsMatPatLeave == 1 ? true : false,
        "BusinessId": $("#drpBusiness").val() == null ? 0 : $("#drpBusiness").val(),
        "DivisionId": $("#drpDivision").val() == null ? 0 : $("#drpDivision").val(),
        "PoolId": $("#drpPool").val() == null ? 0 : $("#drpPool").val(),
        "FunctionId": $("#drpFunction").val() == null ? 0 : $("#drpFunction").val(),
        "ResourceId": $("#drpResource").val() == null ? 0 : $("#drpResource").val(),
        "ProjectId": $("#drpProject").val() == null ? 0 : $("#drpProject").val()
    }
    $.ajax({
        url: AdminPlanner.GetResultByFilter,
        type: 'POST',
        data: JSON.stringify({ _requestModel: ChangeNext }),
        contentType: "application/json",
        success: function (data) {
            debugger;
            $("#EmployeeSchedulDrillDownModel").find("#EmployeeSchedulDrillDownModelBody").html('');
            $("#EmployeeFilterDataModal").find("#EmployeeFilterDataModalBody").html('');
            $("#ResourceList").html('');
            $("#projectCalendar").html('');
            $("#projectCalendar").html(data);
            var travell = $("#hdnTravel").val();
            var timesheett = $("#hdnTimeSheet").val();
            if (travell == 1) {
                for (i = 1; i <= 12; i++) {
                    var dlength = $("#month_" + i).find(".travelSheetIcon").length;
                    if (dlength == "") {
                        $("#travelMonthCount_" + i).text(0+" Days");
                    }
                    else {
                        $("#travelMonthCount_" + i).text(dlength + " Days");
                    }
                }
            }          
            else if (timesheett == 1) {
                for (i = 1; i <= 12; i++) {
                    var dlength = $("#month_" + i).find(".timeSheetIcon").length;
                    if (dlength == "") {
                        $("#timesheetMonthCount_" + i).text(0+" Days");
                    }
                    else {
                        $("#timesheetMonthCount_" + i).text(dlength + " Days");
                    }
                }
            }
            $(".nav-tabs a").each(function () {
                if(IsTimesheet==1)
                {                    
                        $('.nav-tabs li.active').removeClass('active');
                        $("#TimesheetLeaveLink").closest('li').addClass('active');                    
                }
                else if(IsTravel==1)
                {
                    $('.nav-tabs li.active').removeClass('active');
                    $("#TravelLeaveLink").closest('li').addClass('active');
                }
                else if(IsAnualLeave==1)
                {
                    $('.nav-tabs li.active').removeClass('active');
                    $("#AnualLeaveLink").closest('li').addClass('active');
                }
                else if (IsOtherLeave==1)
                {
                    $('.nav-tabs li.active').removeClass('active');
                    $("#OtherLeaveLink").closest('li').addClass('active');
                }
                else if(IsSickLeave==1)
                {
                    $('.nav-tabs li.active').removeClass('active');
                    $("#SickLeaveLink").closest('li').addClass('active');
                }
                else if(IsLateLeave==1)
                {
                    $('.nav-tabs li.active').removeClass('active');
                    $("#LateLeaveLink").closest('li').addClass('active');
                }
                else if(IsPublicHolidays==1)
                {
                    $('.nav-tabs li.active').removeClass('active');
                    $("#PublicHolidayLink").closest('li').addClass('active');
                }
                else if(IsMatPatLeave==1)
                {
                    $('.nav-tabs li.active').removeClass('active');
                    $("#Mat_PatLeaveLink").closest('li').addClass('active');
                }
            })
        }
    });
}
function FilterByMonth(IsTimeSheet,IsTravel,YearId,MonthId)
{
    var Year = $("#drpYear").val();
    var ChangeNext = {
        "Year": YearId,
        "IsTimeSheet": IsTimeSheet,
        "IsTravel": IsTravel,
        "BusinessId": $("#drpBusiness").val() == null ? 0 : $("#drpBusiness").val(),
        "DivisionId": $("#drpDivision").val() == null ? 0 : $("#drpDivision").val(),
        "PoolId": $("#drpPool").val() == null ? 0 : $("#drpPool").val(),
        "FunctionId": $("#drpFunction").val() == null ? 0 : $("#drpFunction").val(),
        "ResourceId": $("#drpResource").val() == null ? 0 : $("#drpResource").val(),
        "ProjectId": $("#drpProject").val() == null ? 0 : $("#drpProject").val(),
        "MonthId": MonthId,
    }
    $.ajax({
        url: AdminPlanner.GetResultByFilterMonth,
        type: 'POST',
        data: JSON.stringify({ _requestModel: ChangeNext }),
        contentType: "application/json",
        success: function (data) {
            $("#projectCalendar").html('');
            $("#ResourceList").html('');
            $("#EmployeeFilterDataModal").find("#EmployeeFilterDataModalBody").html('');
            $("#EmployeeFilterDataModal").find("#EmployeeFilterDataModalBody").html(data);
            $('.icnTravel').addClass('big-calender-span').removeClass('big-calender-span-big');
            $('.icnTimesheet').addClass('big-calender-span').removeClass('big-calender-span-big');
        }
    });
}

function publicHolidaySrch(YearId,MonthId,DayId)
{
    var yearId = YearId;
    var monthId = MonthId;
    var dayId = DayId;
    $.ajax({
            url: AdminPlanner.addEdit_PublicHoliday,
            data: { Date: dayId, Month: monthId, Year: yearId },
            success: function (data) {
                $("#EmployeePlanner_PublicholidayModel").find("#EmployeePlanner_PublicHolidayBody").html("");
                $("#EmployeePlanner_PublicholidayModel").find("#EmployeePlanner_PublicHolidayBody").html(data);
                $("#EmployeePlanner_PublicholidayModel").modal('show');
            }
        });

 }
function resetAll()
{
    $("#drpBusiness").val('');
    $("#drpDivision").val('');
    $("#drpPool").val('');
    $("#drpFunction").val('');
    $("#drpResource").val('');
    $("#drpProject").val('');
}
function backToTimesheetPlanner()
{
    var Year = $("#hdnYear").val();
    var day = $("#hdnMonth").val();
    debugger;
    var ismonth = $("#istimesheetMonth").val();
    if (ismonth == 0)
    {
        FilterByMonth(1, 0, Year, day);
    }
    else if (ismonth == 1) {
        GetResultByFilter(Year, 1, 0, 0, 0, 0, 0, 0, 0);
    }
}
function backToTravelPlanner()
{
    var Year = $("#hdnYear").val();
    var month = $("#hdnMonth").val();
    var ismonth = $("#istravelMonth").val();
    if (ismonth == 0)
    {
        FilterByMonth(0, 1, Year, month);
    }
    else if (ismonth == 1)
    {
        GetResultByFilter(Year, 0, 1, 0, 0, 0, 0, 0, 0);
    }
    
}
function backToPlannerFromMonth()
{
    var isTravel = $("#hdnTravel").val();
    var isTimesheet = $("#hdnTimeSheet").val();
    var Year = $("#hdnYear").val();
    if (isTravel == "1") {
        GetResultByFilter(Year, 0, 1, 0, 0,0,0,0,0);
    }
    else if (isTimesheet == "1") {
        GetResultByFilter(Year, 1,0, 0, 0, 0, 0, 0, 0);
    }
}
//Timesheet
function TimeSheetStepCallback(obj, context) {
    var isError = false;   
    if (context.fromStep == 1) {              
       
        var isEditTimesheet=$("#isEditTimesheet").val();
        $.each($("#EmployeeProjectPlanner_TimeSheetModel").find("#EmployeeProjectPlanner_TimeSheetBody").find(".Timesheet_Detail_Div"), function () {
            if (bol == "true") {
                isError = false;
            }
            else if (bol == "false") {
                debugger;
                isError = true;
                var r = confirm("Your timesheet hours are not matching with your work pattern. Do you want to proceed?");
                if (r == true) {
                    isError = false;
                } else {
                    isError = true;
                }
            }
            var inTimeHr = $(this).find("#drp-HourseListSDTimesheet").val();
            var inTimeMin = $(this).find("#drp-MinutesListSDTimesheet").val();
            if (inTimeHr == 0) {
                isError = true;
                $(this).find("#lbl-error-InTime").show();
            }
            var endTimeHr = $(this).find("#drp-HourseListEDTimesheet").val();
            var endTimeMin = $(this).find("#drp-MinutesListEDTimesheet").val();
            if (inTimeHr == 0) {
                isError = true;
                $(this).find("#lbl-error-EndTime").show();
            }
            var eid = $("#selectID").val();
            var ename = $("#empNameText").val();
            if (ename == "") {
                isError = true;
                $("#lbl-error-Resource").show();
            }
            if (eid == "" || eid == 0) {
                isError = true;
                $("#lbl-error-Resource").show();
            }
            var date = $("#EmployeeProjectPlanner_TimeSheetModel").find("#EmployeeProjectPlanner_TimeSheetBody").find("#txt_Date").val().trim();
            if (date == "") {
                isError = true;
                $("#EmployeeProjectPlanner_TimeSheetModel").find("#EmployeeProjectPlanner_TimeSheetBody").find("#lbl-error-Date").show();
            }
            var costCode = $(this).find("#drp-CostCode").val();
            if (costCode == "0") {
                isError = true;
                $(this).find("#lbl-error-CostCode").show();
            }

        });
        if (isError) {
            return false;
        }
        else {
            if (isEditTimesheet == 1)
            {
                $("#EmployeeProjectPlanner_TimeSheetModel").find("#EmployeeProjectPlanner_TimeSheetBody").find('.buttonNext').hide();
                $("#EmployeeProjectPlanner_TimeSheetModel").find("#EmployeeProjectPlanner_TimeSheetBody").find('.buttonPrevious').show();
                $("#EmployeeProjectPlanner_TimeSheetModel").find("#EmployeeProjectPlanner_TimeSheetBody").find('.buttonFinish').show();
            }
            else
            {
                $("#EmployeeProjectPlanner_TimeSheetModel").find("#EmployeeProjectPlanner_TimeSheetBody").find('.buttonNext').hide();
                $("#EmployeeProjectPlanner_TimeSheetModel").find("#EmployeeProjectPlanner_TimeSheetBody").find('.buttonPrevious').show();
                $("#EmployeeProjectPlanner_TimeSheetModel").find("#EmployeeProjectPlanner_TimeSheetBody").find('.buttonFinish').hide();
            }
            return true;
        }
    }
    else {
        $("#EmployeeProjectPlanner_TimeSheetModel").find("#EmployeeProjectPlanner_TimeSheetBody").find('.buttonNext').show();
        $("#EmployeeProjectPlanner_TimeSheetModel").find("#EmployeeProjectPlanner_TimeSheetBody").find('.buttonPrevious').hide();
        $("#EmployeeProjectPlanner_TimeSheetModel").find("#EmployeeProjectPlanner_TimeSheetBody").find('.buttonFinish').hide();
        return true;
    }
}
function TimeSheetonFinishCallback(obj, context) {
    var eid = $("#selectID").val();
    var id = $("#EmployeeProjectPlanner_TimeSheetModel").find("#EmployeeProjectPlanner_TimeSheetBody").find("#HiddenId").val();
    var yearid = $("#EmployeeProjectPlanner_TimeSheetModel").find("#EmployeeProjectPlanner_TimeSheetBody").find("#HiddenYearId").val();
    var monthid = $("#EmployeeProjectPlanner_TimeSheetModel").find("#EmployeeProjectPlanner_TimeSheetBody").find("#HiddenMonthId").val();
    var dayid = $("#EmployeeProjectPlanner_TimeSheetModel").find("#EmployeeProjectPlanner_TimeSheetBody").find("#HiddendayId").val();
    var date = $("#EmployeeProjectPlanner_TimeSheetModel").find("#EmployeeProjectPlanner_TimeSheetBody").find("#txt_Date").val().trim();
    var DetailDiv = [];
    $.each($("#EmployeeProjectPlanner_TimeSheetModel").find("#EmployeeProjectPlanner_TimeSheetBody").find(".Timesheet_Detail_Div"), function () {
        var inTimeHr = $(this).find("#drp-HourseListSDTimesheet").val();
        var inTimeMin = $(this).find("#drp-MinutesListSDTimesheet").val();
        var endTimeHr = $(this).find("#drp-HourseListEDTimesheet").val();
        var endTimeMin = $(this).find("#drp-MinutesListEDTimesheet").val();
        var project = $(this).find("#drp-Project").val();
        var costCode = $(this).find("#drp-CostCode").val();
        var customer = $(this).find("#selectID").val();
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
    $.each($("#EmployeeProjectPlanner_TimeSheetModel").find("#EmployeeProjectPlanner_TimeSheetBody").find('#filesList').find(".ListData"), function () {
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
    var comment = $("#EmployeeProjectPlanner_TimeSheetModel").find("#EmployeeProjectPlanner_TimeSheetBody").find("#text_Comments").val().trim();
    var model = {
        Id: id,
        EmployeeId: eid,
        Date: date,
        Comment: comment,
        yearId: yearid,
        monthId: monthid,
        day: dayid,
        jsonDocumentList: JsondocumentListJoinString,
        jsonDetailList: jsonDetailString,
        HolidayCountryID: $("#drp-publicHolidayCountry").val(),
        timehseetDrillDown:$("#timesheetdrillDown").val(),
        isMonth: $("#isTimesheetmonth").val()
    }
    $.ajax({
        url: AdminPlanner.SaveData_TimeSheet,
        type: 'POST',
        data: JSON.stringify(model),
        contentType: "application/json",
        success: function (data) {
            $("#EmployeeProjectPlanner_TimeSheetModel").modal('hide');
            //$("#EmployeeProjectPlanner_TimeSheetModel").html("");
            //$("#EmployeeProjectPlanner_TimeSheetModel").html(data);
            //$("#EmployeeProjectPlanner_TimeSheetModel").find('.close').click();
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
            var isTimesheetDrillDown = $("#timesheetdrillDown").val();
            var ismonth = $("#isTimesheetmonth").val();
            if (isTimesheetDrillDown == 1)
            {
                TTimesheetDrillDownData(1, yearid, monthid, dayid,isTimesheetDrillDown);
            }
            else
            {                
                    if (ismonth == 0) {
                        $("#projectCalendar").html('');
                        $("#projectCalendar").html(data);
                    }
                    else if (ismonth == 1) {
                        $("#projectCalendar").html('');
                        $("#ResourceList").html('');
                        $("#EmployeeFilterDataModal").find("#EmployeeFilterDataModalBody").html('');
                        $("#EmployeeFilterDataModal").find("#EmployeeFilterDataModalBody").html(data);
                    }
                }            
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
function TimesheetData(IsTimeSheet, YearId, MonthId, DayId, isEditTimesheet, isMonth) {
    $.ajax({
        url: AdminPlanner.getAdminTimesheet,
        data: { Id: 0, IsTimeseet: IsTimeSheet, Year: YearId, month: MonthId, Day: DayId, isEditTimesheet: isEditTimesheet, isMonth: isMonth },
        success: function (data) {
            $("#EmployeeProjectPlanner_TimeSheetModel").find("#EmployeeProjectPlanner_TimeSheetBody").html("");
            $("#EmployeeProjectPlanner_TimeSheetModel").find("#EmployeeProjectPlanner_TimeSheetBody").html(data);
            $("#EmployeeProjectPlanner_TimeSheetModel").find("#EmployeeProjectPlanner_TimeSheetBody").find('#wizard').smartWizard({
                onLeaveStep: TimeSheetStepCallback,
                onFinish: TimeSheetonFinishCallback
            });
            $('#btn_AddNew_TimesheetDetail').click();
            $("#EmployeeProjectPlanner_TimeSheetModel").modal('show');
            $("#EmployeeProjectPlanner_TimeSheetModel").find("#EmployeeProjectPlanner_TimeSheetBody").find('.buttonNext').addClass('btn btn-warning');
            $("#EmployeeProjectPlanner_TimeSheetModel").find("#EmployeeProjectPlanner_TimeSheetBody").find('.buttonPrevious').addClass('btn btn-warning');
            $("#EmployeeProjectPlanner_TimeSheetModel").find("#EmployeeProjectPlanner_TimeSheetBody").find('.buttonFinish').addClass('btn btn-success');
            $("#EmployeeProjectPlanner_TimeSheetModel").find("#EmployeeProjectPlanner_TimeSheetBody").find('.buttonPrevious').hide();
            $("#EmployeeProjectPlanner_TimeSheetModel").find("#EmployeeProjectPlanner_TimeSheetBody").find('.buttonFinish').hide();
            $("#EmployeeProjectPlanner_TimeSheetModel").find("#EmployeeProjectPlanner_TimeSheetBody").find("#txt_Date").Zebra_DatePicker({
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {
                    $("#EmployeeProjectPlanner_TimeSheetModel").find("#EmployeeProjectPlanner_TimeSheetBody").find("#lbl-error-Date").hide();
                }
            });
            
        }
    });
}
$("#page_content").on("click", "#btn_AddNew_TimesheetDetail", function () {
    $.ajax({
        url: AdminPlanner.addEdit_TimeSheet_Detail,
        success: function (data) {
            var html = $("#EmployeeProjectPlanner_TimeSheetModel").find("#EmployeeProjectPlanner_TimeSheetBody").find("#TimeSheet_detail").html();
            if (html == "") {
                $("#EmployeeProjectPlanner_TimeSheetModel").find("#EmployeeProjectPlanner_TimeSheetBody").find("#TimeSheet_detail").html(data);

            }
            else {
                $("#EmployeeProjectPlanner_TimeSheetModel").find("#EmployeeProjectPlanner_TimeSheetBody").find("#TimeSheet_detail").append(data);
            }
            $("#EmployeeProjectPlanner_TimeSheetModel").find('.Timesheet_Detail_Div:first').find('.timesheet_delete_icon').parent().hide();
        }
    });
});
$("#page_content").on('change', '#TimeSheetFileToUpload', function (e) {
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
                    url: AdminPlanner.TimeSheetImageData,
                    contentType: false,
                    processData: false,
                    data: imageData,
                    success: function (result) {
                        var string = '<div class="row ListData"><div class="col-lg-1 icon"><i class="fa fa-paperclip"></i></div><div class="col-lg-10 attach-disc"><label class="fileName" data-newFileName="' + result.NewFileName + '">' + result.originalFileName + '</label><br /><input type="text" class="ImageDescription form-control" placeholder="Add Description Here"/></div><div class="col-lg-1 file-deleteicon"><i class="fa fa-trash-o"></i></div></div>';
                        var isEmpty = $("#EmployeeProjectPlanner_TimeSheetModel").find("#EmployeeProjectPlanner_TimeSheetBody").find("#filesList").html();
                        if (isEmpty = "") {
                            $("#EmployeeProjectPlanner_TimeSheetModel").find("#EmployeeProjectPlanner_TimeSheetBody").find("#filesList").html(string);
                        }
                        else {
                            $("#EmployeeProjectPlanner_TimeSheetModel").find("#EmployeeProjectPlanner_TimeSheetBody").find("#filesList").append(string);
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
function HoursList() {
    $("#lbl-error-InTime").hide();
    $("#lbl-error-EndTime").hide();
    var St = $('#drp-HourseListSDTimesheet').val();
    var Sm = $('#drp-MinutesListSDTimesheet').val();
    var Et = $('#drp-HourseListEDTimesheet').val();
    var Em = $('#drp-MinutesListEDTimesheet').val();
    Diff = ((new Date("1991-1-1 " + Et + "" + ":" + "" + Em + "") - new Date("1991-1-1 " + St + "" + ":" + "" + Sm + "")) / 1000 / 60 / 60).toFixed(2);
    debugger;
    if (Diff <= 0) {
        $("#lbl-error-validtimeSD").show();
    }
    else {
        $("#lbl-error-validtimeSD").hide();
    }
    if (St == 0) {
        $("#lbl-error-InTime").show();
        $("#lbl-error-EndTime").hide();
    }
    else {
        $("#lbl-error-InTime").hide();
    }
    if (Et == 0) {
        $("#lbl-error-InTime").hide();
        $("#lbl-error-EndTime").show();
    }
    else {
        $("#lbl-error-EndTime").hide();
    }
    debugger;
    bol = getWorkPattData();
}
function getWorkPattData() {
    var DetailDiv = [];
    var eid = $("#selectID").val();
    var date = $("#EmployeeProjectPlanner_TimeSheetModel").find("#EmployeeProjectPlanner_TimeSheetBody").find("#txt_Date").val();
    $.each($("#EmployeeProjectPlanner_TimeSheetModel").find("#EmployeeProjectPlanner_TimeSheetBody").find(".Timesheet_Detail_Div"), function () {
        var inTimeHr = $(this).find("#drp-HourseListSDTimesheet").val();
        var inTimeMin = $(this).find("#drp-MinutesListSDTimesheet").val();
        var endTimeHr = $(this).find("#drp-HourseListEDTimesheet").val();
        var endTimeMin = $(this).find("#drp-MinutesListEDTimesheet").val();
        var oneData = {
            InTimeHr: inTimeHr,
            InTimeMin: inTimeMin,
            EndTimeHr: endTimeHr,
            EndTimeMin: endTimeMin,
        }
        DetailDiv.push(oneData);
    });
    var jsonDetailString = JSON.stringify(DetailDiv);
    var model = {
        EmployeeId: eid,
        Date: date,
        jsonDetailList: jsonDetailString
    }
    $.ajax({
        url: AdminPlanner.workPattenData,
        type: 'POST',
        data: model,
        success: function (data) {
            debugger;
            bol = JSON.stringify(data);
        }
    });
    return bol;
}
$("#EmployeeProjectPlanner_TimeSheetModel").on("click", ".timesheet_delete_icon", function () {
    if ($("#EmployeeProjectPlanner_TimeSheetModel").find(".timesheet_delete_icon").length > 1) {
        $(this).parent().parent().parent().parent().remove();
    }
});
function TTimesheetDrillDownData(IsTimeSheet, YearId, MonthId, DayId,isMonth) {
    var busId = BusId;
    var divId = DivId;
    var poolId = PoolId;
    var FunId = FunctionId;
    var day = DayId;
    var Month = MonthId;
    var year = YearId;
    var projectId = $("#drpProject").val();
    var resourceId = $("#drpResource").val();
    $.ajax({
        url: AdminPlanner.timesheetDrillDown,
        type: 'POST',
        data: { IsTimeSheet: IsTimeSheet, Day: day, month: Month, Year: year, BusiId: busId, DiviId: divId, PoolId: poolId, FunctId: FunId, ProjectId: projectId, isMonth: isMonth, resourceId: resourceId },
        success: function (data) {
            $("#EmployeeSchedulDrillDownModel").find("#EmployeeSchedulDrillDownModelBody").html('');
            $("#EmployeeSchedulDrillDownModel").find("#EmployeeSchedulDrillDownModelBody").html(data);
            //$("#EmployeeFilterDataModal").find("#EmployeeFilterDataModalBody").html('');
            //$("#projectCalendarYear_Month").html('');
            //$("#projectCalendar").html('');
            //$("#ResourceList").show();
            $("#PartialMonthDay").hide();
            $("#ProjectCalanderMonth").hide();
            $("#ResourceList").html(data);
        }
    });
}
function EditResourceTimsheet(EmployeeId)
{
    var EmpID = EmployeeId;    
    var hdnYear = $("#hdnYear").val();
    var hdnMonth = $("#hdnMonth").val();
    var hdnDay = $("#hdnDay").val();
    $.ajax({
        url: AdminPlanner.editResourceTimesheet,
        type: 'POST',
        data: { EmpId: EmployeeId, Year: hdnYear, Month: hdnMonth, Day: hdnDay },
        success: function (data) {
            $("#EmployeeProjectPlanner_TimeSheetModel").find("#EmployeeProjectPlanner_TimeSheetBody").html("");
            $("#EmployeeProjectPlanner_TimeSheetModel").find("#EmployeeProjectPlanner_TimeSheetBody").html(data);

            $("#EmployeeProjectPlanner_TimeSheetModel").find("#EmployeeProjectPlanner_TimeSheetBody").find('#wizard').smartWizard({
                onLeaveStep: TimeSheetStepCallback,
                onFinish: TimeSheetonFinishCallback
            });
            $('#btn_AddNew_TimesheetDetail').click();
            $("#EmployeeProjectPlanner_TimeSheetModel").modal('show');
            $("#EmployeeProjectPlanner_TimeSheetModel").find("#EmployeeProjectPlanner_TimeSheetBody").find('.buttonNext').addClass('btn btn-warning');
            $("#EmployeeProjectPlanner_TimeSheetModel").find("#EmployeeProjectPlanner_TimeSheetBody").find('.buttonPrevious').addClass('btn btn-warning');
            $("#EmployeeProjectPlanner_TimeSheetModel").find("#EmployeeProjectPlanner_TimeSheetBody").find('.buttonFinish').addClass('btn btn-success');
            $("#EmployeeProjectPlanner_TimeSheetModel").find("#EmployeeProjectPlanner_TimeSheetBody").find('.buttonPrevious').hide();
            $("#EmployeeProjectPlanner_TimeSheetModel").find("#EmployeeProjectPlanner_TimeSheetBody").find('.buttonFinish').hide();
            $("#EmployeeProjectPlanner_TimeSheetModel").find("#EmployeeProjectPlanner_TimeSheetBody").find("#txt_Date").Zebra_DatePicker({
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {
                    $("#EmployeeProjectPlanner_TimeSheetModel").find("#EmployeeProjectPlanner_TimeSheetBody").find("#lbl-error-Date").hide();
                }
            });
        }
    });
}
function TravelleaveStepCallback(obj, context) {
    if (context.fromStep == 1) {
        var isEditTravel = $("#isEditTravel").val();
        var isError = false;
        var selectedRadio = $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find('input[name=lessThanADay]:checked').attr("id");
        var Eid = $("#selectResourceID").val();
        if (Eid == "") {
            isError = true;
            $("#lbl-error-Resource").show();
        }
        var EmpName = $("#ResourceNameText").val();
        if (EmpName == "") {
            isError = true;
            $("#lbl-error-Resource").show();
        }
        if (selectedRadio == "adayormore") {

            var fromCountry = $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-FromCountryId").val();
            var fromState = $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-FromStateId").val();
            var fromTown = $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-FromTownId").val();
            var fromAirpoet = $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-FromAirportId").val();
            var toCountry = $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-ToCountryId").val();
            var toState = $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-ToStateId").val();
            var toTown = $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-ToTownId").val();
            var toAirport = $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-ToAirportId").val();

            var reasonForLeave = $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-ReasonForTravelId").val();
            var startDate = $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#txt_StartDate").val().trim();
            var endDate = $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#txt_EndDate").val().trim();
            var comments = $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#text_Comments").val().trim();
            var duration = $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#txt_Duration").val().trim();

            var type = $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-Type").val();
            var customer = $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#selectID").val();
            var project = $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-Project").val();
            var costCode = $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-CostCode").val();

            var isLessThenADay = false;

            if (fromCountry == "0") { isError = true; $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#lbl-error-FromCountryId").show(); }
            if (fromState == "0") { isError = true; $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#lbl-error-FromStateId").show(); }
            if (fromTown == "0") { isError = true; $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#lbl-error-FromTownId").show(); }
            if (toCountry == "0") { isError = true; $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#lbl-error-ToCountryId").show(); }
            if (toState == "0") { isError = true; $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#lbl-error-ToStateId").show(); }
            if (toTown == "0") { isError = true; $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#lbl-error-ToTownId").show(); }

            if (reasonForLeave == "0") { isError = true; $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#lbl-error-ReasonForTravelId").show(); }
            if (startDate == "") { isError = true; $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#lbl-error-StartDate").show(); }
            if (endDate == "") { isError = true; $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#lbl-error-EndDate").show(); }
            if (comments == "") { isError = true; $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#lbl-error-Comments").show(); }

            if (type == "0") { isError = true; $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#lbl-error-Type").show(); }
            if (costCode == "0") { isError = true; $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#lbl-error-CostCode").show(); }

            if (isError) {
                return false;
            }
            else {
                if (isEditTravel == 1)
                {
                    $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find('.buttonNext').hide();
                    $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find('.buttonPrevious').show();
                    $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find('.buttonFinish').show();
                }
                else
                {
                    $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find('.buttonNext').hide();
                    $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find('.buttonPrevious').show();
                    $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find('.buttonFinish').hide();
                }
                return true;
            }
        }
        else {
            var fromCountry = $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-FromCountryId").val();
            var fromState = $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-FromStateId").val();
            var fromTown = $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-FromTownId").val();
            var fromAirpoet = $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-FromAirportId").val();
            var toCountry = $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-ToCountryId").val();
            var toState = $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-ToStateId").val();
            var toTown = $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-ToTownId").val();
            var toAirport = $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-ToAirportId").val();
            var reasonForLeave = $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-ReasonForTravelId").val();
            var startDate = $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#txt_LessThenStartDate").val().trim();
            var isLessThenADay = true;
            var comments = $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#text_Comments").val().trim();
            var type = $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-Type").val();
            var customer = $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#selectID").val();
            var project = $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-Project").val();
            var costCode = $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-CostCode").val();
            var drphrSD = $('#drp-TravelHourseListSD').val();
            var drpminSD = $('#drp-MinutesTravelListSD').val();
            var drphrED = $('#drp-TravelHourseListED').val();
            var drminED = $('#drp-TravelMinutesListED').val();
            debugger;
            if (valError == true) {
                isError = true;
                $("#lbl-error-validtimeSD").show();
            }
            else if (valError == false) {
                isError = false;
            }
            if (drphrSD == 0) {
                isError = true; $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#lbl-error-InTimeSD").show();
            }

            if (drphrED == 0) {
                isError = true; $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#lbl-error-EndTime").show();
            }

            if (fromCountry == "0") { isError = true; $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#lbl-error-FromCountryId").show(); }
            if (fromState == "0") { isError = true; $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#lbl-error-FromStateId").show(); }
            if (fromTown == "0") { isError = true; $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#lbl-error-FromTownId").show(); }
            if (toCountry == "0") { isError = true; $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#lbl-error-ToCountryId").show(); }
            if (toState == "0") { isError = true; $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#lbl-error-ToStateId").show(); }
            if (toTown == "0") { isError = true; $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#lbl-error-ToTownId").show(); }

            if (reasonForLeave == "0") { isError = true; $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#lbl-error-ReasonForTravelId").show(); }
            if (startDate == "") { isError = true; $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#lbl-error-LessThenStartDate").show(); }
            if (comments == "") { isError = true; $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#lbl-error-Comments").show(); }

            if (type == "0") { isError = true; $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#lbl-error-Type").show(); }
            if (costCode == "0") { isError = true; $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#lbl-error-CostCode").show(); }
            if (isError) {
                return false;
            }
            else {
                if (isEditTravel == 1) {
                    $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find('.buttonNext').hide();
                    $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find('.buttonPrevious').show();
                    $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find('.buttonFinish').show();
                }
                else {
                    $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find('.buttonNext').hide();
                    $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find('.buttonPrevious').show();
                    $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find('.buttonFinish').hide();
                }
                return true;
            }
        }
    }
    else {
        $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find('.buttonNext').show();
        $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find('.buttonPrevious').hide();
        $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find('.buttonFinish').hide();
        return true;
    }
}
function TravelLeaveonFinishCallback(obj, context) {

    var id = $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#HiddenId").val();
    var yearid = $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#HiddenYearId").val();
    var monthid = $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#HiddenMonthId").val();
    var dayid = $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#HiddendayId").val();
    var fromCountry = $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-FromCountryId").val();
    var fromState = $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-FromStateId").val();
    var fromTown = $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-FromTownId").val();
    var fromAirpoet = $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-FromAirportId").val();
    var toCountry = $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-ToCountryId").val();
    var toState = $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-ToStateId").val();
    var toTown = $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-ToTownId").val();
    var toAirport = $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-ToAirportId").val();
    var type = $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-Type").val();
    var customer = $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#selectID").val();
    var project = $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-Project").val();
    var costCode = $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-CostCode").val();
    var selectedRadio = $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find('input[name=lessThanADay]:checked').attr("id");
    var documentList = [];
    $.each($("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find('#filesList').find(".ListData"), function () {
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
        var reasonForLeave = $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-ReasonForTravelId").val();
        var startDate = $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#txt_StartDate").val().trim();
        var endDate = $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#txt_EndDate").val().trim();
        var comments = $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#text_Comments").val().trim();
        var duration = $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#txt_Duration").val().trim();
        var isLessThenADay = false;
        SaveTravelLeave(id, isLessThenADay, reasonForLeave, startDate, endDate, duration, comments, yearid, monthid, dayid, 0, 0, 0, 0, 0, JsondocumentListJoinString, fromCountry, fromState, fromTown, fromAirpoet, toCountry, toState, toTown, toAirport, type, customer, project, costCode);
    }
    else {
        var reasonForLeave = $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-ReasonForTravelId").val();
        var startDate = $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#txt_LessThenStartDate").val().trim();
        var hourSD = $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-TravelHourseListSD").val();
        var minSD = $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-MinutesTravelListSD").val();
        var hourED = $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-TravelHourseListED").val();
        var minED = $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-TravelMinutesListED").val();
        var durationHr = $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#txt_DurationHours").val();
        var isLessThenADay = true;
        var comments = $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#text_Comments").val().trim();
        SaveTravelLeave(id, isLessThenADay, reasonForLeave, startDate, "", 0, comments, yearid, monthid, dayid, hourSD, minSD, hourED, minED, durationHr, JsondocumentListJoinString, fromCountry, fromState, fromTown, fromAirpoet, toCountry, toState, toTown, toAirport, type, customer, project, costCode);
    }
}
function SaveTravelLeave(id, isLessThenADay, reasonForLeaveId, startDate, endDate, duration, comments, yearid, monthid, dayid, hourSD, minSD, hourED, minED, durationHr, jsonString, fromCountry, fromState, fromTown, fromAirpoet, toCountry, toState, toTown, toAirport, type, customer, project, costCode) {
    var EmpId = $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#selectResourceID").val();
    var model = {
        Id: id,
        EmployeeId: EmpId,
        FromCountryId: fromCountry,
        FromStateId: fromState,
        FromTownId: fromTown,
        FromAirportId: fromAirpoet,
        ToCountryId: toCountry,
        ToStateId: toState,
        ToTownId: toTown,
        ToAirportId: toAirport,
        IsLessThenADay: isLessThenADay,
        ReasonForTravelId: reasonForLeaveId,
        StartDate: startDate,
        EndDate: endDate,
        Duration: duration,
        Comment: comments,
        yearId: yearid,
        monthId: monthid,
        day: dayid,
        jsonDocumentList: jsonString,
        InTimeHr: hourSD,
        InTimeMin: minSD,
        EndTimeHr: hourED,
        EndTimeMin: minED,
        DurationHr: durationHr,
        Type: type,
        Customer: customer,
        Project: project,
        CostCode: costCode,
        IsTravellDrillDown: $("#isTravelDrillDown").val(),
        isMonth: $("#isTravelmonth").val()
        //        HolidayCountryID: $("#drp-publicHolidayCountry").val()
    }
    $.ajax({
        url: AdminPlanner.SaveData_TravelLeaveData,
        type: 'POST',
        data: JSON.stringify(model),
        contentType: "application/json",
        success: function (data) {
            debugger;
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
            $(".toast-success").show();
            $("#EmployeeProjectPlanner_TravelLeaveModel").modal('hide');
            var ismonth = $("#isTravelmonth").val();
            var isTravelDrilldown = $("#isTravelDrillDown").val();
            if (isTravelDrilldown == 1)
            {
                TravelDrillDownData(1, yearid, monthid, dayid, isTravelDrilldown);
            }
            else if (isTravelDrilldown == 0) {
                if (ismonth == 0) {
                    $("#projectCalendar").html('');
                    $("#projectCalendar").html(data);
                }
                else if (ismonth == 1) {
                    $("#projectCalendar").html('');
                    $("#ResourceList").html('');
                    $("#EmployeeFilterDataModal").find("#EmployeeFilterDataModalBody").html('');
                    $("#EmployeeFilterDataModal").find("#EmployeeFilterDataModalBody").html(data);
                }
            }
            setTimeout(function () { $(".toast-success").hide(); }, 1500);
        }
    });
}
function TravelData(IsTravel, YearId, MonthId, DayId, isTravelDrillDown, isMonth) {
    var travel = IsTravel;
    var day = DayId;
    var Month = MonthId;
    var year = YearId;
    $.ajax({
        url: AdminPlanner.getAdmin_ProjectPlannerTravel,
        type: 'POST',
        data: { Id: 0, Date: day, Month: Month, Year: year, isTravelDrillDown: isTravelDrillDown, isMonth: isMonth },
        success: function (data) {
            debugger;
            $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").html('');
            $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").html(data);
            $("#EmployeeProjectPlanner_TravelLeaveModel").modal('show');
            $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find('#wizard').smartWizard({
                onLeaveStep: TravelleaveStepCallback,
                onFinish: TravelLeaveonFinishCallback
            });
            $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find('.buttonNext').addClass('btn btn-warning');
            $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find('.buttonPrevious').addClass('btn btn-warning');
            $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find('.buttonFinish').addClass('btn btn-success');
            $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find('.buttonPrevious').hide();
            $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find('.buttonFinish').hide();
            $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#txt_StartDate").Zebra_DatePicker({
                //direction: false,
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {
                    $("#lbl-error-StartDate").hide();
                    $("#lbl-error-GreaterEndDate").hide();
                    calculateTravelDateDiff();
                    //calculateAnnualDateDiffTravelLeave();
                }
            });
            $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#txt_EndDate").Zebra_DatePicker({
                //direction: false,
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {
                    $("#lbl-error-EndDate").hide();
                    $("#lbl-error-GreaterEndDate").hide();
                    calculateTravelDateDiff();
                }
            });
            $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#txt_LessThenStartDate").Zebra_DatePicker({
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {
                    $("#lbl-error-LessThenStartDate").hide();
                }
            });
            //if (Id > 0) {
            //    var selectedRadio = $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find('input[name=lessThanADay]:checked').attr("id");
            //    if (selectedRadio == "adayormore") {
            //        $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#div_ADayOrMore").show();
            //        $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#div_LessThenADay").hide();
            //    }
            //    else {
            //        $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#div_LessThenADay").show();
            //        $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody  ").find("#div_ADayOrMore").hide();
            //    }
            //}            
        }
    });
}
function TravelDrillDownData(IsTravel, YearId, MonthId, DayId,isMonth) {
    var busId = BusId;
    var divId = DivId;
    var poolId = PoolId;
    var FunId = FunctionId;
    var day = DayId;
    var Month = MonthId;
    var year = YearId;
    var projectId = $("#drpProject").val();
    var resourceId = $("#drpResource").val();
    $.ajax({
        url: AdminPlanner.getAdminProjectPlannerDrillDown,
        type: 'POST',
        data: { isTravel: IsTravel, Day: day, month: Month, Year: year, BusiId: busId, DiviId: divId, PoolId: poolId, FunctId: FunId, ProjectId: projectId, isMonth: isMonth, resourceId: resourceId },
        success: function (data) {
            $("#EmployeeSchedulDrillDownModel").find("#EmployeeSchedulDrillDownModelBody").html('');
            $("#EmployeeSchedulDrillDownModel").find("#EmployeeSchedulDrillDownModelBody").html(data);
            //$("#projectCalendar").html('');
            //$("#EmployeeFilterDataModal").find("#EmployeeFilterDataModalBody").html('');
            //$("#projectCalendarYear_Month").html('');
            //$("#ResourceList").show();
            $("#PartialMonthDay").hide();
            $("#ProjectCalanderMonth").hide();
            $("#ResourceList").html(data);


        }
    });
}
function EditResourceTravel(EmployeeId) {
    var EmpID = EmployeeId;
    var hdnYear = $("#hdnYear").val();
    var hdnMonth = $("#hdnMonth").val();
    var hdnDay = $("#hdnDay").val();
    $.ajax({
        url: AdminPlanner.editResourceTravel,
        type: 'POST',
        data: { EmpId: EmployeeId, Year: hdnYear, Month: hdnMonth, Day: hdnDay },
        success: function (data) {
            $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").html('');
            $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").html(data);
            $("#EmployeeProjectPlanner_TravelLeaveModel").modal('show');
            $("#EmployeeProjectPlanner_TravelLeaveodel").find("#EmployeeProjectPlanner_TravelLeaveBody").find('.close').click();
            $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find('#wizard').smartWizard({
                onLeaveStep: TravelleaveStepCallback,
                //onFinish: TravelLeaveonFinishCallback
            });
            $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find('.buttonNext').addClass('btn btn-warning');
            $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find('.buttonPrevious').addClass('btn btn-warning');
            $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find('.buttonFinish').addClass('btn btn-success');

            $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find('.buttonPrevious').hide();
            $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find('.buttonFinish').hide();
            $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#txt_StartDate").Zebra_DatePicker({
                //direction: false,
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {
                    $("#lbl-error-StartDate").hide();
                    $("#lbl-error-GreaterEndDate").hide();
                    calculateTravelDateDiff();
                    //calculateAnnualDateDiffTravelLeave();
                }
            });
            $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#txt_EndDate").Zebra_DatePicker({
                //direction: false,
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {
                    $("#lbl-error-EndDate").hide();
                    $("#lbl-error-GreaterEndDate").hide();
                    calculateTravelDateDiff();
                }
            });
            $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#txt_LessThenStartDate").Zebra_DatePicker({
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {
                    $("#lbl-error-LessThenStartDate").hide();
                }
            });
            if (EmployeeId > 0) {
                var selectedRadio = $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find('input[name=lessThanADay]:checked').attr("id");
                if (selectedRadio == "adayormore") {
                    $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#div_ADayOrMore").show();
                    $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#div_LessThenADay").hide();
                }
                else {
                    $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#div_LessThenADay").show();
                    $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody  ").find("#div_ADayOrMore").hide();
                }
            }
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
        }
    });
}

$('#page_content').on('click', '#btnbookTravel', function () {
    var hdnYear = $("#hdnYear").val();
    var hdnMonth = $("#hdnMonth").val();
    var hdnDay = $("#hdnDay").val();
    var isMonth = $("#istravelMonth").val();
    var isTravelDrillDown = 1;
    TravelData(1, hdnYear, hdnMonth, hdnDay, isTravelDrillDown, isMonth);
})
$('#page_content').on('click', '#btnTimesheet', function () {
    var hdnYear = $("#hdnYear").val();
    var hdnMonth = $("#hdnMonth").val();
    var hdnDay = $("#hdnDay").val();
    var isEditTimesheet = 1;
    var isMonth = $("#istimesheetMonth").val();
    TimesheetData(1, hdnYear, hdnMonth, hdnDay,isEditTimesheet,isMonth);
})


$("#page_content").on("change", ".Travel_leave_radio", function () {
    var Id = $(this).attr("id");
    if (Id == "adayormore") {
        $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#div_ADayOrMore").show();
        $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#div_LessThenADay").hide();
    }
    else {
        $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#div_LessThenADay").show();
        $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#div_ADayOrMore").hide();
    }
});
$("#page_content").on("change", "#drp-FromCountryId", function () {
    $("#drp-FromStateId").val('');
    $("#drp-FromTownId").val('');
    var value = $(this).val();
    if (value != "0") {
        $.ajax({
            url: AdminPlanner.BindState,
            data: { countryId: value },
            success: function (data) {
                $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-FromStateId").html('');
                var toAppend = '';
                $.each(data, function (index, item) {
                    toAppend += "<option value='" + item.Value + "'>" + item.Text + "</option>";
                })
                $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-FromStateId").html(toAppend);

                $.ajax({
                    url: AdminProjectPlanner.BindAirport,
                    data: { countryId: value },
                    success: function (data) {
                        $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-FromAirportId").html('');
                        $("#drpAirport").html('');
                        var toAppendAirport = '';
                        $.each(data, function (index, item) {
                            toAppendAirport += "<option value='" + item.Value + "'>" + item.Text + "</option>";
                        })
                        $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-FromAirportId").html(toAppendAirport);
                    }
                });

            }
        });
    }
    else {
        $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-FromStateId").html('');
        var toAppend = "<option value='0'>-- Select --</option>";
        $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-FromStateId").html(toAppend);
        $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-FromAirportId").html('');
        $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-FromAirportId").html(toAppend);
    }
});
$("#page_content").on("change", "#drp-FromStateId", function () {
    var value = $(this).val();
    if (value != "0") {
        $.ajax({
            url: AdminPlanner.BindCity,
            data: { stateId: value },
            success: function (data) {
                $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-FromTownId").html('');
                var toAppend = '';
                $.each(data, function (index, item) {
                    toAppend += "<option value='" + item.Value + "'>" + item.Text + "</option>";
                })
                $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-FromTownId").html(toAppend);
            }
        });
    }
    else {
        $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-FromTownId").html('');
        var toAppend = "<option value='0'>-- Select --</option>";
        $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-FromTownId").html(toAppend);
    }
});
$("#page_content").on("change", "#drp-ToCountryId", function () {
    $("#drp-ToStateId").val('');
    $("#drp-ToTownId").val('');
    var value = $(this).val();
    if (value != "0") {
        $.ajax({
            url: AdminPlanner.BindState,
            data: { countryId: value },
            success: function (data) {
                $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-ToStateId").html('');
                var toAppend = '';
                $.each(data, function (index, item) {
                    toAppend += "<option value='" + item.Value + "'>" + item.Text + "</option>";
                })
                $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-ToStateId").html(toAppend);

                $.ajax({
                    url: AdminPlanner.BindAirport,
                    data: { countryId: value },
                    success: function (data) {
                        $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-ToAirportId").html('');
                        var toAppendAirport = '';
                        $.each(data, function (index, item) {
                            toAppendAirport += "<option value='" + item.Value + "'>" + item.Text + "</option>";
                        })
                        $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-ToAirportId").html(toAppendAirport);
                    }
                });

            }
        });
    }
    else {
        $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-ToStateId").html('');
        var toAppend = "<option value='0'>-- Select --</option>";
        $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-ToStateId").html(toAppend);
        $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-ToAirportId").html('');
        $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-ToAirportId").html(toAppend);
    }
});
$("#page_content").on("change", "#drp-ToStateId", function () {
    var value = $(this).val();
    if (value != "0") {
        $.ajax({
            url: AdminPlanner.BindCity,
            data: { stateId: value },
            success: function (data) {
                $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-ToTownId").html('');
                var toAppend = '';
                $.each(data, function (index, item) {
                    toAppend += "<option value='" + item.Value + "'>" + item.Text + "</option>";
                })
                $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-ToTownId").html(toAppend);
            }
        });
    }
    else {
        $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-ToTownId").html('');
        var toAppend = "<option value='0'>-- Select --</option>";
        $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-ToTownId").html(toAppend);
    }
});
$("#page_content").on('change', '#TravelLeaveFileToUpload', function (e) {
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
                    url: AdminPlanner.ImageData_TravelLeave,
                    contentType: false,
                    processData: false,
                    data: imageData,
                    success: function (result) {
                        var string = '<div class="row ListData"><div class="col-lg-1 icon"><i class="fa fa-paperclip"></i></div><div class="col-lg-10 attach-disc"><label class="fileName" data-newFileName="' + result.NewFileName + '">' + result.originalFileName + '</label><br /><input type="text" class="ImageDescription form-control" placeholder="Add Description Here"/></div><div class="col-lg-1 file-deleteicon"><i class="fa fa-trash-o"></i></div></div>';
                        var isEmpty = $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#filesList").html();
                        if (isEmpty = "") {
                            $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#filesList").html(string);
                        }
                        else {
                            $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#filesList").append(string);
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
$("#page_content").on('change', '#drp-TravelHourseListSD', function (e) {
    $("#lbl-error-InTimeSD").hide();
    $("#lbl-error-DurationHours").hide();
    $("#lbl-error-validtimeSD").hide();
    debugger;
    var St = $(this).val();
    var Sm = $("#drp-MinutesTravelListSD").val();
    var Et = $("#drp-TravelHourseListED").val();
    var Em = $("#drp-TravelMinutesListED").val();
    var Diff = ((new Date("1991-1-1 " + Et + "" + ":" + "" + Em + "") - new Date("1991-1-1 " + St + "" + ":" + "" + Sm + "")) / 1000 / 60 / 60).toFixed(2);
    if (isNaN(Diff) || Diff <= 0) {
        debugger;
        $("#txt_DurationHours").val('');
        $("#lbl-error-validtimeSD").show();

        valError = true;
    }
    else {
        $("#txt_DurationHours").val(Diff);
        valError = false;
    }
    return valError;
});
$("#page_content").on('change', '#drp-MinutesTravelListSD', function (e) {
    $("#lbl-error-InTimeSD").hide();
    $("#lbl-error-validtimeSD").hide();
    $("#lbl-error-DurationHours").hide();
    var St = $("#drp-TravelHourseListSD").val();
    var Sm = $(this).val();
    var Et = $("#drp-TravelHourseListED").val();
    var Em = $("#drp-TravelMinutesListED").val();
    var Diff = ((new Date("1991-1-1 " + Et + "" + ":" + "" + Em + "") - new Date("1991-1-1 " + St + "" + ":" + "" + Sm + "")) / 1000 / 60 / 60).toFixed(2);
    if (isNaN(Diff) || Diff <= 0) {
        $("#txt_DurationHours").val('');
        $("#lbl-error-validtimeSD").show();
        valError = true;
    }
    else {
        $("#txt_DurationHours").val(Diff);
        valError = false;
    }
    return valError;
});
$("#page_content").on('change', '#drp-TravelHourseListED', function (e) {
    $("#lbl-error-DurationHours").hide();
    $("#lbl-error-EndTime").hide();
    $("#lbl-error-validtimeSD").hide();
    var St = $("#drp-TravelHourseListSD").val();
    var Sm = $("#drp-MinutesTravelListSD").val();
    var Et = $(this).val();
    var Em = $("#drp-TravelMinutesListED").val();
    var Diff = ((new Date("1991-1-1 " + Et + "" + ":" + "" + Em + "") - new Date("1991-1-1 " + St + "" + ":" + "" + Sm + "")) / 1000 / 60 / 60).toFixed(2);
    if (isNaN(Diff) || Diff <= 0) {
        $("#txt_DurationHours").val('');
        $("#lbl-error-validtimeSD").show();
        valError = true;
    }
    else {
        $("#txt_DurationHours").val(Diff);
        valError = false;
    }
    return valError;

});
$("#page_content").on('change', '#drp-TravelMinutesListED', function (e) {
    $("#lbl-error-EndTime").hide();
    $("#lbl-error-validtimeSD").hide();
    var St = $("#drp-TravelHourseListSD").val();
    var Sm = $("#drp-MinutesTravelListSD").val();
    var Et = $("#drp-TravelHourseListED").val();
    var Em = $(this).val();
    var Diff = ((new Date("1991-1-1 " + Et + "" + ":" + "" + Em + "") - new Date("1991-1-1 " + St + "" + ":" + "" + Sm + "")) / 1000 / 60 / 60).toFixed(2);
    if (isNaN(Diff) || Diff <= 0) {
        $("#txt_DurationHours").val('');
        $("#lbl-error-validtimeSD").show();
        valError = true;
    }
    else {
        $("#txt_DurationHours").val(Diff);
        valError = false;
    }
    return valError;
});

