$(document).ready(function () {
    for (i = 1; i <= 12; i++) {
        var dlength = $("#month_" + i).find(".schedulingIcon").length;        
        if (dlength == "")
        {            
            $("#scheduleMonthCount_"+i).text(0+" Days");    
        }
        else
        {
            $("#scheduleMonthCount_" + i).text(dlength + " Days");
        }
    }
});
var bol = false;
var valError = false;
var BusId;
var DivId;
var PoolId;
var FunctionId;
function GetDivisonByBusinessId(Buisenessid) {
    BusId = Buisenessid.value;
    $.ajax({
        url: AdminProjectPlanner.GetDivisonByBusinessID,
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
        url: AdminProjectPlanner.GetPoolByDivisonID,
        //type: 'POST',
        data: { BusinessId: BusId, DivisonID : DivisonID.value },
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
        url: AdminProjectPlanner.GetFunctionByPoolId,
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
        url: AdminProjectPlanner.GetResourceByFunctionAndPoolID,
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
function ChangePreviousMonth(Year, IsSchedule, IsTravel, IsTimeSheet, IsUplift) {
    Year = Year - 1;
    GetResultByFilter(Year, IsSchedule, IsTravel, IsTimeSheet, IsUplift)
}
function ChangeNextMonth(Year, IsSchedule, IsTravel, IsTimeSheet, IsUplift) {
    Year = Year + 1;
    GetResultByFilter(Year, IsSchedule, IsTravel, IsTimeSheet, IsUplift);
}
function GetResultByFilter(Year, IsSchedule, IsTravel, IsTimeSheet, IsUplift) {
    var ChangeNext = {
        "Year": Year,
        "IsTimeSheet": IsTimeSheet == 1 ? true : false,
        "IsSchedule": IsSchedule == 1 ? true : false,
        "IsTravel": IsTravel == 1 ? true : false,
        "IsUplift": IsUplift == 1 ? true : false,
        "BusinessId": $("#drpBusiness").val() == null ? 0 : $("#drpBusiness").val(),
        "DivisionId": $("#drpDivision").val() == null ? 0 : $("#drpDivision").val(),
        "PoolId": $("#drpPool").val() == null ? 0 : $("#drpPool").val(),
        "FunctionId": $("#drpFunction").val() == null ? 0 : $("#drpFunction").val(),
        "ResourceId": $("#drpResource").val() == null ? 0 : $("#drpResource").val(),
        "ProjectId": $("#drpProject").val() == null ? 0 : $("#drpProject").val()
    }
    $.ajax({
        url: AdminProjectPlanner.GetResultByFilter,
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
                         $(".nav-tabs a").each(function () {
                if (IsSchedule == 1) {
                    $('.nav-tabs li.active').removeClass('active');
                    $("#ScheduleProjrctPlannerLink").closest('li').addClass('active');
                }
                else if(IsTravel == 1) {
                    $('.nav-tabs li.active').removeClass('active');
                    $("#TravelProjrctPlannerLink").closest('li').addClass('active');
                }
                else if (IsTimeSheet == 1) {
                    $('.nav-tabs li.active').removeClass('active');
                    $("#TimesheetProjrctPlannerLink").closest('li').addClass('active');
                }
                else if (IsUplift == 1) {
                    $('.nav-tabs li.active').removeClass('active');
                    $("#UpliftProjrctPlannerLink").closest('li').addClass('active');
                }                
            })

            var travell = $("#hdnTravel").val();
            var schedulee = $("#hdnSchedule").val();            
            var timesheett = $("#hdnTimeSheet").val();
            var upliftt=$("#hdnUplift").val();
            if (travell == 1)
            {
                for (i = 1; i <= 12; i++) {
                    var dlength = $("#month_" + i).find(".travelSheetIcon").length;
                    if (dlength == "") {
                        $("#travelMonthCount_" + i).text(0 + " Days");
                    }
                    else {
                        $("#travelMonthCount_" + i).text(dlength + " Days");
                    }
                }
            }
            else if(schedulee==1)
            {
                 for (i = 1; i <= 12; i++) {
                var dlength = $("#month_" + i).find(".schedulingIcon").length;        
                if (dlength == "")
                {            
                    $("#scheduleMonthCount_" + i).text(0 + " Days");
                }
                else
                {
                    $("#scheduleMonthCount_" + i).text(dlength + " Days");
                }
                 }
            }
            else if(timesheett==1)
            {
                for (i = 1; i <= 12; i++) {
                    var dlength = $("#month_" + i).find(".timeSheetIcon").length;
                    if (dlength == "") {
                        $("#timesheetMonthCount_" + i).text(0 + " Days");
                    }
                    else {
                        $("#timesheetMonthCount_" + i).text(dlength + " Days");
                    }
                }
            }
            else if(upliftt==1)
            {
                for (i = 1; i <= 12; i++) {
                    var dlength = $("#month_" + i).find(".upliftSheetIcon").length;
                    if (dlength == "") {
                        $("#upliftMonthCount_" + i).text(0 + " Days");
                    }
                    else {
                        $("#upliftMonthCount_" + i).text(dlength+" Days");
                    }
                }
            }

        }
    });
}
function GetAllTravelLeave(Year, IsSchedule, IsTravel, IsTimeSheet, IsUplift) {
    GetResultByFilter(Year, IsSchedule, IsTravel, IsTimeSheet, IsUplift);
}
function GetAllTimeSheetLeave(Year, IsSchedule, IsTravel, IsTimeSheet, IsUplift) {
    GetResultByFilter(Year, IsSchedule, IsTravel, IsTimeSheet, IsUplift);
}
function GetAllUpliftLeave(Year, IsSchedule, IsTravel, IsTimeSheet, IsUplift) {
    GetResultByFilter(Year, IsSchedule, IsTravel, IsTimeSheet, IsUplift);
}
function GetAllSchedulingLeave(Year, IsSchedule, IsTravel, IsTimeSheet, IsUplift) {
    GetResultByFilter(Year, IsSchedule, IsTravel, IsTimeSheet, IsUplift);
}
function GetAllSchedulingLeaveByMonth(Year,IsSchedule, IsTravel, IsTimeSheet, IsUplift) {
    GetResultByFilterByMonth(Year, IsSchedule, IsTravel, IsTimeSheet, IsUplift);
}
function GetAllTimeSheetLeaveByMonth(Year, IsSchedule, IsTravel, IsTimeSheet, IsUplift) {
    GetResultByFilterByMonth(Year, IsSchedule, IsTravel, IsTimeSheet, IsUplift);
}
function GetAllTravelLeaveByMonth(Year, IsSchedule, IsTravel, IsTimeSheet, IsUplift) {
    GetResultByFilterByMonth(Year, IsSchedule, IsTravel, IsTimeSheet, IsUplift);
}
function GetAllUpliftLeaveByMonth(Year, IsSchedule, IsTravel, IsTimeSheet, IsUplift) {
    GetResultByFilterByMonth(Year, IsSchedule, IsTravel, IsTimeSheet, IsUplift);
}
function GetResultByFilterByMonth(Year, IsSchedule, IsTravel, IsTimeSheet, IsUplift) {
    var ChangeNext = {
        "Year": Year,
        "IsTimeSheet": IsTimeSheet == 1 ? true : false,
        "IsSchedule": IsSchedule == 1 ? true : false,
        "IsTravel": IsTravel == 1 ? true : false,
        "IsUplift": IsUplift == 1 ? true : false,
        "BusinessId": $("#drpBusiness").val() == null ? 0 : $("#drpBusiness").val(),
        "DivisionId": $("#drpDivision").val() == null ? 0 : $("#drpDivision").val(),
        "PoolId": $("#drpPool").val() == null ? 0 : $("#drpPool").val(),
        "FunctionId": $("#drpFunction").val() == null ? 0 : $("#drpFunction").val(),
        "ResourceId": $("#drpResource").val() == null ? 0 : $("#drpResource").val(),
        "ProjectId": $("#drpProject").val() == null ? 0 : $("#drpProject").val(),
        "MonthId": $("#drpMonth").val() == null ? 0 : $("#drpMonth").val(),
    }
    $.ajax({
        url: AdminProjectPlanner.GetResultByFilterMonth,
        type: 'POST',
        data: JSON.stringify({ _requestModel: ChangeNext }),
        contentType: "application/json",
        success: function (data) {
            $("#EmployeeFilterDataModal").find("#EmployeeFilterDataModalBody").html('');
            $("#EmployeeFilterDataModal").find("#EmployeeFilterDataModalBody").html(data);
        }
    });
}
//function applyFilterMonth()
//{   
//    var Year = $("#drpYear").val();
//    var ChangeNext = {
//        "Year": Year,
//        "IsTimeSheet": $("#hdnTimeSheet").val() == 1 ? true : false,
//        "IsSchedule": $("#hdnSchedule").val() == 1 ? true : false,
//        "IsTravel": $("#hdnTravel").val() == 1 ? true : false,
//        "IsUplift": $("#hdnUplift").val() == 1 ? true : false,
//        "BusinessId": $("#drpBusiness").val() == null ? 0 : $("#drpBusiness").val(),
//        "DivisionId": $("#drpDivision").val() == null ? 0 : $("#drpDivision").val(),
//        "PoolId": $("#drpPool").val() == null ? 0 : $("#drpPool").val(),
//        "FunctionId": $("#drpFunction").val() == null ? 0 : $("#drpFunction").val(),
//        "ResourceId": $("#drpResource").val() == null ? 0 : $("#drpResource").val(),
//        "ProjectId": $("#drpProject").val() == null ? 0 : $("#drpProject").val(),
//        "MonthId": $("#drpMonth").val() == null ? 0 : $("#drpMonth").val(),

//    }
//    $.ajax({
//        url: AdminProjectPlanner.GetResultByFilterMonth,
//        type: 'POST',
//        data: JSON.stringify({ _requestModel: ChangeNext }),
//        contentType: "application/json",
//        success: function (data) {
//            $("#EmployeeFilterDataModal").find("#EmployeeFilterDataModalBody").html('');
//            $("#EmployeeFilterDataModal").find("#EmployeeFilterDataModalBody").html(data);
//        }
//    });
//}
function backToProjectPlannerFromMonth()
{
    var isScedule = $("#hdnSchedule").val();
    var isTravel = $("#hdnTravel").val();
    var isUplift = $("#hdnUplift").val();
    var isTimesheet = $("#hdnTimeSheet").val();
    var Year = $("#hdnYear").val();
    if(isScedule=="1")
    {
        GetResultByFilter(Year, 1,0,0,0);
    }
    else if(isTravel=="1")
    {
        GetResultByFilter(Year, 0,1, 0, 0);
    }
    else if(isTimesheet=="1")
    {
        GetResultByFilter(Year, 0, 0,1,0);
    }
    else if(isUplift=="1")
    {
        GetResultByFilter(Year, 0, 0, 0, 1);
    }
}
function backToTravelProjectPlanner() {
    var year = $("#hdnYear").val();
    var month =$("#hdnMonth").val();
    var isMonth=$("#ismonthtravelDrill").val();
    if (isMonth == 0)
    {
        FilterByMonth(0,1, 0, 0, year, month);
    }
    else if (isMonth == 1)
    {
        GetResultByFilter(year, 0, 1, 0, 0);
    }
}
function backToScheduleProjectPlanner()
{
    var year = $("#hdnYear").val();
    var month = $("#hdnMonth").val();
    var ismonth = $("#hdnSchedulingMonth").val();
    if(ismonth==0)
    {
        FilterByMonth(1, 0, 0, 0, year, month);
    }
    else if (ismonth == 1) {
        GetResultByFilter(year, 1, 0, 0, 0);
    }
}
function backToTimesheetProjectPlanner()
{
    var year = $("#hdnYear").val();
    var month = $("#hdnMonth").val();
    var ismonth = $("#ismonthtimesheetDrill").val();
    if (ismonth == 0)
    {
        FilterByMonth(0, 0, 1, 0, year, month);
    }
    else if (ismonth == 1)
    {
        GetResultByFilter(year, 0, 0, 1, 0);
    }
    
}
function backToUpliftProjectPlanner()
{
    var year = $("#hdnYear").val();
    var month = $("#hdnMonth").val();
    var ismonth = $("#isMonthUpliftDrill").val();
    if (ismonth == 0)
    {
        FilterByMonth(0, 0, 0, 1, year, month);
    }
    else if (ismonth == 1) {
        GetResultByFilter(year, 0, 0, 0, 1);
    }
}
function FilterByMonth(IsSchdule,IsTravel,IsTimeSheet,IsUplift,YearId,MonthId)
{
    var Year = $("#drpYear").val();
    var ChangeNext = {
        "Year": YearId,
        "IsTimeSheet": IsTimeSheet,
        "IsSchedule": IsSchdule,
        "IsTravel": IsTravel,
        "IsUplift": IsUplift,
        "BusinessId": $("#drpBusiness").val() == null ? 0 : $("#drpBusiness").val(),
        "DivisionId": $("#drpDivision").val() == null ? 0 : $("#drpDivision").val(),
        "PoolId": $("#drpPool").val() == null ? 0 : $("#drpPool").val(),
        "FunctionId": $("#drpFunction").val() == null ? 0 : $("#drpFunction").val(),
        "ResourceId": $("#drpResource").val() == null ? 0 : $("#drpResource").val(),
        "ProjectId": $("#drpProject").val() == null ? 0 : $("#drpProject").val(),
        "MonthId": MonthId,

    }
    $.ajax({
        url: AdminProjectPlanner.GetResultByFilterMonth,
        type: 'POST',
        data: JSON.stringify({ _requestModel: ChangeNext }),
        contentType: "application/json",
        success: function (data) {
            $("#projectCalendar").html('');            
            $("#ResourceList").html('');
            $("#EmployeeFilterDataModal").find("#EmployeeFilterDataModalBody").html('');
            $("#EmployeeFilterDataModal").find("#EmployeeFilterDataModalBody").html(data);
            $('.schedulingMonth').addClass('big-calender-span').removeClass('big-calender-span-big');
            $('.icnTravel').addClass('big-calender-span').removeClass('big-calender-span-big');
            $('.icnTimesheet').addClass('big-calender-span').removeClass('big-calender-span-big');
            $('.icnuplift').addClass('big-calender-span').removeClass('big-calender-span-big');            
        }
    });

}
function ApplyFilter() {
    var Year = $("#drpYear").val();
    var IsTimeSheet = $("#hdnTimeSheet").val();
    var IsSchedule=$("#hdnSchedule").val();
    var IsTravel=$("#hdnTravel").val();
    var IsUplift=$("#hdnUplift").val();
    var ChangeNext = {
        "Year": Year,
        "IsTimeSheet": $("#hdnTimeSheet").val() == 1 ? true : false,
        "IsSchedule": $("#hdnSchedule").val() == 1 ? true : false,
        "IsTravel": $("#hdnTravel").val() == 1 ? true : false,
        "IsUplift": $("#hdnUplift").val() == 1 ? true : false,
        "BusinessId": $("#drpBusiness").val() == null ? 0 : $("#drpBusiness").val(),
        "DivisionId": $("#drpDivision").val() == null ? 0 : $("#drpDivision").val(),
        "PoolId": $("#drpPool").val() == null ? 0 : $("#drpPool").val(),
        "FunctionId": $("#drpFunction").val() == null ? 0 : $("#drpFunction").val(),
        "ResourceId": $("#drpResource").val() == null ? 0 : $("#drpResource").val(),
        "ProjectId": $("#drpProject").val() == null ? 0 : $("#drpProject").val(),
    }
    $.ajax({
        url: AdminProjectPlanner.GetResultByFilter,
        type: 'POST',
        data: JSON.stringify({ _requestModel: ChangeNext }),
        contentType: "application/json",
        success: function (data) {
            $("#EmployeeFilterDataModal").find("#EmployeeFilterDataModalBody").html('');
            $("#projectCalendar").html('');
            $("#projectCalendar").html(data);
            $(".nav-tabs a").each(function () {
                if (IsSchedule == 1) {
                    $('.nav-tabs li.active').removeClass('active');
                    $("#ScheduleProjrctPlannerLink").closest('li').addClass('active');
                }
                else if (IsTravel == 1) {
                    $('.nav-tabs li.active').removeClass('active');
                    $("#TravelProjrctPlannerLink").closest('li').addClass('active');
                }
                else if (IsTimeSheet == 1) {
                    $('.nav-tabs li.active').removeClass('active');
                    $("#TimesheetProjrctPlannerLink").closest('li').addClass('active');
                }
                else if (IsUplift == 1) {
                    $('.nav-tabs li.active').removeClass('active');
                    $("#UpliftProjrctPlannerLink").closest('li').addClass('active');
                }
            })
        }
    });
}
function resetAll() {
    $("#drpBusiness").val('');
    $("#drpDivision").val('');
    $("#drpPool").val('');
    $("#drpFunction").val('');
    $("#drpResource").val('');
    $("#drpProject").val('');
}
function getProjectPlannerData(IsSchedule, IsTimeSheet,YearId , MonthId, DayId)
{
    $('[data-toggle="popover"]').popover(
    {
          title: "<center><input type='button' value='scheduling' id='btnsscheduling' onclick='schedulingdata(" + isschedule + "," + istimesheet + "," + yearid + "," + monthid + "," + dayid + ");' /></br><input type='button' value='drilldown' id='btndrilldown' onclick='drilldowndata(" + isschedule + "," + istimesheet + "," + yearid + "," + monthid + "," + dayid + ");'/></center>",
          content: "", html: true, placement: "right"
    });
}
function DrillDownData(IsSchedule ,IsTimeSheet,YearId, MonthId ,DayId,isMonth)
{
    var busId=BusId;
    var divId = DivId;
    var poolId = PoolId;
    var FunId = FunctionId;
    var Schedule = IsSchedule;
    var Timesheet = IsTimeSheet;
    var day = DayId;
    var Month = MonthId;
    var year = YearId;
    var projectId = $("#drpProject").val();
    var resourceId = $("#drpResource").val();
    $.ajax({
        url: AdminProjectPlanner.getProjectPlannerDrillDownData,
        type: 'POST',
        data: { isSchedule: Schedule, isTimesheet: Timesheet, Day: day, month: Month, Year: year, BusiId: busId, DiviId: divId, PoolId: poolId, FunctId: FunId, ProjectId: projectId, isMonth: isMonth, resourceId: resourceId },
        success: function (data) {
            $("#EmployeeSchedulDrillDownModel").find("#EmployeeSchedulDrillDownModelBody").html('');
            $("#EmployeeSchedulDrillDownModel").find("#EmployeeSchedulDrillDownModelBody").html(data);
            //$("#EmployeeFilterDataModal").find("#EmployeeFilterDataModalBody").hide();
            $("#PartialMonthDay").hide();
            $("#ProjectCalanderMonth").hide();
            
            $("#ResourceList").html(data);
        }
    });
}
//Scheduling
function EditResourceSchedule(EmployeeId)
{
    var EmpID = EmployeeId;
    var hdnYear = $("#hdnYear").val();
    var hdnMonth = $("#hdnMonth").val();
    var hdnDay = $("#hdnDay").val();
    $.ajax({
        url: AdminProjectPlanner.editScheduling,
        type: 'POST',
        data: { EmpId: EmployeeId, Year: hdnYear, Month: hdnMonth, Day: hdnDay },
        success: function (data) {
            $("#EmployeeBenefitModal").find("#EmployeeBenefitBody").html('');
            $("#EmployeeBenefitModal").find("#EmployeeBenefitBody").html(data);
            $("#EmployeeBenefitModal").modal('show');
            $("#EmployeeBenefitModal").find("#EmployeeBenefitBody").find('.close').click();
            if (EmployeeId > 0) {
                var selectedRadio = $("#EmployeeBenefitModal").find("#EmployeeBenefitBody").find('input[name=lessThanADay]:checked').attr("id");
                if (selectedRadio == "adayormore") {
                    $("#EmployeeBenefitModal").find("#EmployeeBenefitBody").find("#div_ADayOrMore").show();
                    $("#EmployeeBenefitModal").find("#EmployeeBenefitBody").find("#div_LessThenADay").hide();
                }
                else {
                    $("#EmployeeBenefitModal").find("#EmployeeBenefitBody").find("#div_LessThenADay").show();
                    $("#EmployeeBenefitModal").find("#EmployeeBenefitBody").find("#div_ADayOrMore").hide();
                }
            }
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
        }
    });
}
$("#page_content").on("click", "#btnScheduling", function () {
    var hdnYear = $("#hdnYear").val();
    var hdnMonth = $("#hdnMonth").val();
    var hdnDay = $("#hdnDay").val();
    var isScheduling = 1;
    var isMonth = $("#hdnSchedulingMonth").val();
    SchedulingData(1, 0, hdnYear, hdnMonth, hdnDay, isScheduling, isMonth);
})
function calculateDateDiff() {
    var startDate = $("#txt_StartDate").val().trim();
    var edDate = $("#txt_EndDate").val().trim();
    debugger;
    if (startDate != "" || edDate != "") {
        if (StartDateValidation(startDate, edDate)) {
            $("#lbl-error-GreaterEndDate").show();
            $("#txt_EndDate").val('');
            $("#txt_DurationDays").val('');
        }
        else {
            var days = DaysCount(startDate, edDate);
            $("#txt_DurationDays").val(days);
        }
    }
}
function SchedulingData(IsSchedule, IsTimeSheet, YearId, MonthId,DayId,isScheduling,isMonth)
{
    var Schedule = IsSchedule;
    var Timesheet = IsTimeSheet;
    var day = DayId;
    var Month = MonthId;
    var year = YearId;
    debugger;
    $.ajax({
        url: AdminProjectPlanner.getProjectPlannerData,
        type: 'POST',
        data: { isSchedule: Schedule, isTimesheet: Timesheet, Year: year, month: Month, Day: day, isScheduling: isScheduling, isMonth: isMonth },
        success: function (data) {
            debugger;
            $("#EmployeeBenefitModal").find("#EmployeeBenefitBody").html('');
            $("#EmployeeBenefitModal").find("#EmployeeBenefitBody").html(data);
            $("#EmployeeBenefitModal").modal('show');
            $("#EmployeeBenefitModal").find("#EmployeeBenefitBody").find("#txt_StartDate").Zebra_DatePicker({
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {
                    $("#lbl-error-StartDate").hide();
                    $("#lbl-error-GreaterEndDate").hide();
                    CheckForValidResourceDate();
                    calculateDateDiff();
                }
            });
            $("#EmployeeBenefitModal").find("#EmployeeBenefitBody").find("#txt_EndDate").Zebra_DatePicker({
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {
                    $("#lbl-error-EndDate").hide();
                    $("#lbl-error-GreaterEndDate").hide();
                    calculateDateDiff();
                }
            });            
            $("#EmployeeBenefitModal").find("#EmployeeBenefitBody").find("#txt_LessThenStartDate").Zebra_DatePicker({
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {
                    $("#lbl-error-EndExistStartDate").hide();
                    $("#lbl-error-LessThenStartDate").hide();
                    CheckForValidResourceLessThanDate();
                }
            });
        }
    });
}
function CheckForValidResourceLessThanDate()
{
    var stDate = $("#txt_LessThenStartDate").val();
    var EmpId = $("#selectResourceID").val();
    var EmpName = $("#ResourceNameText").val();
    $.ajax({
        url: AdminProjectPlanner.validateSchedulStDate,
        type: 'POST',
        data: { EmployeeID: EmpId, startDate: stDate },
        success: function (data) {
            debugger;
            valError = JSON.stringify(data);
        }
    });
    return valError;
}
function CheckForValidResourceDate()
{
    var stDate = $("#txt_StartDate").val();
    var EmpId = $("#selectResourceID").val();
    var EmpName = $("#ResourceNameText").val();
    var isErrorr = false;
    if (EmpId == "")
    {
        isErrorr = true;
        $("#lbl-error-Resource").show();
    }
    if (!isErrorr)
    {
        $.ajax({
            url: AdminProjectPlanner.validateSchedulStDate,
            type: 'POST',
            data: { EmployeeID: EmpId,startDate: stDate },
            success: function (data) {
                valError = JSON.stringify(data);
            }
        });
     }
     return valError;
   }
$("#page_content").on("change", ".Scheduling_radio", function () {
    var Id = $(this).attr("id");
    if (Id == "adayormore") {
        $("#EmployeeBenefitModal").find("#EmployeeBenefitBody").find("#div_ADayOrMore").show();
        $("#EmployeeBenefitModal").find("#EmployeeBenefitBody").find("#div_LessThenADay").hide();
    }
    else {
        $("#EmployeeBenefitModal").find("#EmployeeBenefitBody").find("#div_LessThenADay").show();
        $("#EmployeeBenefitModal").find("#EmployeeBenefitBody").find("#div_ADayOrMore").hide();
    }
});
$("#page_content").on("click", "#btn-submit-EmployeePlanner_Scheduling", function () {
    var IsError = false;
    var id = $("#HiddenId").val();
    var yearid = $("#HiddenYearId").val();
    var monthid = $("#HiddenMonthId").val();
    var dayid = $("#HiddendayId").val();
    var selectedRadio = $('input[name=lessThanADay]:checked').attr("id");
    if (selectedRadio == "adayormore") {
        var startDate = $("#txt_StartDate").val();
        var endDate = $("#txt_EndDate").val();
        var duration = $("#txt_DurationDays").val();
        var isLessThenADay = false;
        var isDayorMore = true;
        if (startDate == "") { IsError = true; $("#lbl-error-StartDate").show(); }
        if (endDate == "") { IsError = true; $("#lbl-error-EndDate").show(); }
        if (valError == "true") {
            IsError = true;
            $("#lbl-error-ExistStartDate").show();
        }
        else if (valError == "false") {
            IsError = false;
            $("#lbl-error-ExistStartDate").hide();
        }
    }
    else {
        var startDate = $("#txt_LessThenStartDate").val().trim();
        var InTimeHr = $("#drp-HourseListSD").val();
        var InTimeMin = $("#drp-MinutesListSD").val();
        var EndTimeHr = $("#drp-HourseListED").val();
        var EndTimeMin = $("#drp-MinutesListED").val();
        var DurationHr = $("#txt_DurationHours").val();
        var isLessThenADay = true;
        var isDayorMore = false;
        if (valError == "true") {
            IsError = true;
            $("#lbl-error-EndExistStartDate").show();
        }
        else if (valError == "false") {
            IsError = false;
            $("#lbl-error-EndExistStartDate").hide();
        }
        if (startDate == "") { IsError = true; $("#lbl-error-LessThenStartDate").show(); }     
        if (InTimeHr == 0)
        {
            IsError = true;
            $("#lbl-error-InTimeSD").show();
        }
        if(EndTimeHr==0)
        {
            IsError = true;
            $("#lbl-error-InTimeED").show();
        }
    }
    var comments = $("#text_Comments").val();
    var customer = $("#selectID").val();
    var customerName = $("#empNameText").val();
    var project = $("#drp-Project").val();
    var asset = $("#drp-Asset").val();
    var EmpId = $("#selectResourceID").val();
    var EmpName = $("#ResourceNameText").val();
    if (EmpId == 0||EmpId=="")
    {
        IsError = true; $("#lbl-error-Resource").show();
    }
    if (EmpName == "") {
        IsError = true; $("#lbl-error-Resource").show();
    }
    if (customerName == "") { IsError = true; $("#lbl-error-Customer").show(); }
    if (project == 0) { IsError = true; $("#lbl-error-Project").show(); }
    if (asset == 0 || asset == 0) { IsError = true; $("#lbl-error-Asset").show(); }
    if (IsError) {
        $(".hrtoolLoader").hide();
        $(".modal-backdrop").hide();
        return false;
    }
    else {
        var model = {
            Id: id,
            EmployeeId: EmpId,
            IsDayOrMore: isDayorMore,
            IsLessThenADay: isLessThenADay,
            StartDate: startDate,
            EndDate: endDate,
            DurationDays: duration,
            InTimeHr: InTimeHr,
            InTimeMin: InTimeMin,
            EndTimeHr: EndTimeHr,
            EndTimeMin: EndTimeMin,
            DurationHr: DurationHr,
            Project: project,
            Customer: customer,
            Asset: asset,
            Comments: comments,
            yearId: yearid,
            monthId: monthid,
            day: dayid,
            HolidayCountryID: $("#drp-publicHolidayCountry").val(),
            isScheduling: $("#isSchedulingDrillDown").val(),
            isMonth: $("#isSchedulingmonth").val()
        }
        $.ajax({
            url: AdminProjectPlanner.saveSchedulingData,
            type: 'POST',
            data: JSON.stringify(model),
            contentType: "application/json",
            success: function (data) {
                $(".hrtoolLoader").hide();
                $(".modal-backdrop").hide();
                $("#EmployeeBenefitModal").modal('hide');
                var isScheduling = $("#isSchedulingDrillDown").val();
                var ismonth = $("#isSchedulingmonth").val();
                debugger;
                if (isScheduling == 1) {
                    DrillDownData(1, 0, yearid, monthid, dayid, ismonth);
                }
                else if (isScheduling == 0) {
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
});
$("#EmployeeBenefitModal").on('change', '#drp-HourseListSD', function (e) {
    $("#lbl-error-validtimeSD").hide();
    $("#lbl-error-InTimeSD").hide();
    $("#lbl-error-DurationHours").hide();
    var St = $(this).val();
    var Sm = $("#drp-MinutesListSD").val();
    var Et = $("#drp-HourseListED").val();
    var Em = $("#drp-MinutesListED").val();
    var Diff = ((new Date("1991-1-1 " + Et + "" + ":" + "" + Em + "") - new Date("1991-1-1 " + St + "" + ":" + "" + Sm + "")) / 1000 / 60 / 60).toFixed(2);
    if (Diff <= 0) {
        $("#drp-HourseListED").val(0);
        $("#drp-MinutesListED").val(0);
        $("#txt_DurationHours").val(0);
        $("#lbl-error-validtimeSD").show();
    }
    else {
        $("#txt_DurationHours").val(Diff);
    }
});
$("#EmployeeBenefitModal").on('change', '#drp-MinutesListSD', function (e) {
    $("#lbl-error-validtimeSD").hide();
    $("#lbl-error-InTimeSD").hide();
    $("#lbl-error-DurationHours").hide();
    var St = $("#drp-HourseListSD").val();
    var Sm = $(this).val();
    var Et = $("#drp-HourseListED").val();
    var Em = $("#drp-MinutesListED").val();
    var Diff = ((new Date("1991-1-1 " + Et + "" + ":" + "" + Em + "") - new Date("1991-1-1 " + St + "" + ":" + "" + Sm + "")) / 1000 / 60 / 60).toFixed(2);
    if (Diff <= 0) {
        $("#drp-HourseListED").val(0);
        $("#drp-MinutesListED").val(0);
        $("#txt_DurationHours").val(0);
    }
    else {
        $("#txt_DurationHours").val(Diff);
    }

});
$("#EmployeeBenefitModal").on('change', '#drp-HourseListED', function (e) {
    $("#lbl-error-validtimeSD").hide();
    $("#lbl-error-InTimeED").hide();
    $("#lbl-error-DurationHours").hide();
    var St = $("#drp-HourseListSD").val();
    var Sm = $("#drp-MinutesListSD").val();
    var Et = $(this).val();
    var Em = $("#drp-MinutesListED").val();
    var Diff = ((new Date("1991-1-1 " + Et + "" + ":" + "" + Em + "") - new Date("1991-1-1 " + St + "" + ":" + "" + Sm + "")) / 1000 / 60 / 60).toFixed(2);
    if (Diff <= 0) {
        $("#drp-HourseListSD").val(0);
        $("#drp-MinutesListSD").val(0);
        $("#txt_DurationHours").val(0);
        $("#lbl-error-validtimeSD").show();
    }
    else {
        $("#txt_DurationHours").val(Diff);
    }
});
$("#EmployeeBenefitModal").on('change', '#drp-MinutesListED', function (e) {
    $("#lbl-error-validtimeSD").hide();
    $("#lbl-error-InTimeED").hide();
    $("#lbl-error-DurationHours").hide();
    var St = $("#drp-HourseListSD").val();
    var Sm = $("#drp-MinutesListSD").val();
    var Et = $("#drp-HourseListED").val();
    var Em = $(this).val();
    var Diff = ((new Date("1991-1-1 " + Et + "" + ":" + "" + Em + "") - new Date("1991-1-1 " + St + "" + ":" + "" + Sm + "")) / 1000 / 60 / 60).toFixed(2);
    if (Diff <= 0) {
        $("#drp-HourseListSD").val(0);
        $("#drp-MinutesListSD").val(0);
        $("#txt_DurationHours").val(0);
    }
    else {
        $("#txt_DurationHours").val(Diff);
    }
});
//Travel
function TravelData(IsTravel, YearId, MonthId, DayId, isTravelDrillDown, isMonth) {
    var travel = IsTravel;
    var day = DayId;
    var Month = MonthId;
    var year = YearId;
    $.ajax({
        url: AdminProjectPlanner.getAdmin_ProjectPlannerTravel,
        type: 'POST',
        data: { IsTravel: travel, Year: year, month: Month, Day: day, isTravelDrillDown: isTravelDrillDown, isMonth:isMonth },
        success: function (data) {
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
            //if (id > 0) {
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
function calculateTravelDateDiff()
{
    var startDate = $("#txt_StartDate").val().trim();
    var edDate = $("#txt_EndDate").val().trim();
    if (startDate != "" || edDate != "") {
        if (StartDateValidation(startDate, edDate)) {
            $("#lbl-error-GreaterEndDate").show();
            $("#txt_EndDate").val('');
            $("#txt_Duration").val('');
        }
        else {
            var days = DaysCount(startDate, edDate);
            $("#txt_Duration").val(days);
        }
    }
}
function TravelleaveStepCallback(obj, context) {

    if (context.fromStep == 1) {
        var EditTravel = $("#isEditTravel").val();
        var isError = false;
        var selectedRadio = $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find('input[name=lessThanADay]:checked').attr("id");
        var Eid = $("#selectResourceID").val();
        if (Eid == "")
        {
            isError = true;
            $("#lbl-error-Resource").show();
        }
        var EmpName = $("#ResourceNameText").val();
        if (EmpName == "")
        {
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
                if (EditTravel == 1)
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
            if (valError == true)
            {
                isError = true;
                $("#lbl-error-validtimeSD").show();
            }
            else if (valError == false)
            {
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
                if (EditTravel == 1) {
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
        isMonth: $("#isTravelmonth").val()
//        HolidayCountryID: $("#drp-publicHolidayCountry").val()
    }
    $.ajax({
        url: AdminProjectPlanner.SaveData_TravelLeaveData,
        type: 'POST',
        data: JSON.stringify(model),
        contentType: "application/json",
        success: function (data) {
            debugger;
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
            $(".toast-success").show();
            $("#EmployeeProjectPlanner_TravelLeaveModel").modal('hide');
            debugger;
            var ismonth = $("#isTravelmonth").val();
            var isTravelDrilldown = $("#isTravelDrillDown").val();
            if (isTravelDrilldown == 1) {
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
            url: AdminProjectPlanner.BindState,
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
            url: AdminProjectPlanner.BindCity,
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
            url: AdminProjectPlanner.BindState,
            data: { countryId: value },
            success: function (data) {
                $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-ToStateId").html('');
                var toAppend = '';
                $.each(data, function (index, item) {
                    toAppend += "<option value='" + item.Value + "'>" + item.Text + "</option>";
                })
                $("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-ToStateId").html(toAppend);

                $.ajax({
                    url: AdminProjectPlanner.BindAirport,
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
            url: AdminProjectPlanner.BindCity,
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
                    url: AdminProjectPlanner.ImageData_TravelLeave,
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
        valError=true;
    }
    else {
        $("#txt_DurationHours").val(Diff);
        valError=false;
    }
    return valError;
});
function TravelDrillDownData(IsTravel, YearId, MonthId, DayId, isMonth) {
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
        url: AdminProjectPlanner.getAdminProjectPlannerDrillDown,
        type: 'POST',
        data: { isTravel: IsTravel, Day: day, month: Month, Year: year, BusiId: busId, DiviId: divId, PoolId: poolId, FunctId: FunId, ProjectId: projectId, isMonth: isMonth, resourceId: resourceId },
        success: function (data) {
            $("#ResourceList").find("#EmployeeSchedulDrillDownModel").find("#EmployeeSchedulDrillDownModelBody").html('');
            $("#ResourceList").find("#EmployeeSchedulDrillDownModel").find("#EmployeeSchedulDrillDownModelBody").html(data);
            //$("#EmployeeFilterDataModal").find("#EmployeeFilterDataModalBody").html('');
            //$("#projectCalendarYear_Month").html('');
            //$("#projectCalendar").html('');
            $("#PartialMonthDay").hide();
            $("#ProjectCalanderMonth").hide();
            $("#ResourceList").html(data);            
        }
    });
}
$('#page_content').on('click', '#btnbookTravel', function () {
    var hdnYear = $("#hdnYear").val();
    var hdnMonth = $("#hdnMonth").val();
    var hdnDay = $("#hdnDay").val();
    var isTravelDrillDown = 1;
    debugger;
    var isMonth = $("#ismonthtravelDrill").val();
    TravelData(1, hdnYear, hdnMonth, hdnDay, isTravelDrillDown, isMonth);
})
function EditResourceTravel(TravelId,EmployeeId) {
    debugger;
    var EmpID = EmployeeId;
    var hdnYear = $("#hdnYear").val();
    var hdnMonth = $("#hdnMonth").val();
    var hdnDay = $("#hdnDay").val();
    $.ajax({
        url: AdminProjectPlanner.editResourceTravel,
        type: 'POST',
        data: { EmpId: EmployeeId, TravelId:TravelId,Year: hdnYear, Month: hdnMonth, Day: hdnDay },
        success: function (data) {
            debugger;
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
    bol = getWorkPattData();
}
function TimeSheetStepCallback(obj, context) {
    if (context.fromStep == 1) {
        var isError = false;
        var isEditTimesheet = $("#isEditTimesheet").val();
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
            if (eid == "" || eid == 0) {
                isError = true;
                $("#lbl-error-Resource").show();
            }
            if (ename == "") {
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
        timehseetDrillDown: $("#timesheetdrillDown").val(),
        isMonth: $("#isTimesheetmonth").val()
    }
    $.ajax({
        url: AdminProjectPlanner.SaveData_TimeSheet,
        type: 'POST',
        data: JSON.stringify(model),
        contentType: "application/json",
        success: function (data) {
            $("#EmployeeProjectPlanner_TimeSheetModel").modal('hide');
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
            debugger;
            var isTimesheetDrillDown = $("#timesheetdrillDown").val();
            var ismonth = $("#isTimesheetmonth").val();
            if (isTimesheetDrillDown == 1) {
                TTimesheetDrillDownData(1, yearid, monthid, dayid,isTimesheetDrillDown);
            }
            else {
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
            if(id > 0) {
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
                    url: AdminProjectPlanner.TimeSheetImageData,
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
function TimesheetData(IsTimeSheet, YearId, MonthId, DayId, isEditTimesheet, isMonth)
{
    $.ajax({
        url: AdminProjectPlanner.getAdminTimesheet,
        data: { Id: 0, IsTimeseet: IsTimeSheet, Year: YearId, month: MonthId, Day: DayId, timehseetDrillDown: isEditTimesheet, isMonth: isMonth },
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
        url: AdminProjectPlanner.addEdit_TimeSheet_Detail,
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
$("#page_content").on("click", ".timesheet_delete_icon", function () {
    if ($("#EmployeeProjectPlanner_TimeSheetModel").find(".timesheet_delete_icon").length > 1) {
        $(this).parent().parent().parent().parent().remove();
    }
});
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
        url: AdminProjectPlanner.workPattenData,
        type: 'POST',
        data: model,
        success: function (data) {
            debugger;
            bol = JSON.stringify(data);
        }
    });

    return bol;
}
function TTimesheetDrillDownData(IsTimeSheet, YearId, MonthId, DayId, isMonth)
{
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
        url: AdminProjectPlanner.timesheetDrillDown,
        type: 'POST',
        data: { IsTimeSheet: IsTimeSheet, Day: day, month: Month, Year: year, BusiId: busId, DiviId: divId, PoolId: poolId, FunctId: FunId, ProjectId: projectId, isMonth: isMonth, resourceId: resourceId },
        success: function (data) {
            $("#ResourceList").find("#EmployeeSchedulDrillDownModel").find("#EmployeeSchedulDrillDownModelBody").html('');
            $("#ResourceList").find("#EmployeeSchedulDrillDownModel").find("#EmployeeSchedulDrillDownModelBody").html(data);
            //$("#EmployeeFilterDataModal").find("#EmployeeFilterDataModalBody").html('');
            //$("#projectCalendarYear_Month").html('');
            //$("#projectCalendar").html('');
            $("#PartialMonthDay").hide();
            $("#ProjectCalanderMonth").hide();
            $("#ResourceList").html(data);
        }
    });
}
$("#page_content").on('click', '#btnTimesheet', function () {
    var hdnYear = $("#hdnYear").val();
    var hdnMonth = $("#hdnMonth").val();
    var hdnDay = $("#hdnDay").val();
    var isEditTimesheet = 1;
    var isMonth = $("#ismonthtimesheetDrill").val();
    TimesheetData(1, hdnYear, hdnMonth, hdnDay, isEditTimesheet, isMonth);
})
function EditResourceTimsheet(EmployeeId)
{
    var EmpID = EmployeeId;
    var hdnYear = $("#hdnYear").val();
    var hdnMonth = $("#hdnMonth").val();
    var hdnDay = $("#hdnDay").val();
    $.ajax({
        url: AdminProjectPlanner.editResourceTimesheet,
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

function UpliftStepCallback(obj, context) {
    
    if (context.fromStep == 1) {
        var isError = false;
        var isEditUplift=$("#isEditUplift").val();
        var date = $("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").find("#txt_Date").val().trim();
        var eid = $("#selectResourceID").val();
        var ename = $("#ResourceNameText").val();
        var JobTitle = $("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").find("#drpJobTitle").val().trim();
        var WorkerRatePer = $("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").find("#txt_workerRatePer").val().trim();
        var WorkerRate = $("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").find("#txt_workerRate").val().trim();
        var CustomerRatePer = $("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").find("#txt_CustomerRatePer").val().trim();
        var CustomerRate = $("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").find("#txt_CustomerRate").val().trim();
        var customer = $("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").find("#selectID").val();
        var Project = $("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").find("#drp-Project").val().trim();
        var Comments = $("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").find("#text_Comments").val().trim();
        $.each($("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").find(".Uplift_Detail_Div"), function () {
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

        });
        debugger;
        if (bol == "True" || bol == true) {
            isError = true;
        }
        else if (bol == "False" || bol == false)
        {
            isError = false;
        }
        if(ename=="")
        {
            isError = true;
            $("#lbl-error-Resource").show();
        }
        if (eid == "" || eid==0)
        {
            isError = true;
            $("#lbl-error-Resource").show();
        }        
        if (date == "") {
            isError = true;
            $("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").find("#lbl-error-Date").show();
        }
       
        if (isError) {
            return false;
        }
        else {
            if (isEditUplift == 1)
            {
                $("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").find('.buttonNext').hide();
                $("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").find('.buttonPrevious').show();
                $("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").find('.buttonFinish').show();
            }
            else
            {
                $("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").find('.buttonNext').hide();
                $("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").find('.buttonPrevious').show();
                $("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").find('.buttonFinish').hide();
            }
            return true;
        }
    }
    else {
        $("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").find('.buttonNext').show();
        $("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").find('.buttonPrevious').hide();
        $("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").find('.buttonFinish').hide();
        return true;
    }
}
function UpliftonFinishCallback(obj, context) {
    var isCustomer = $("#isCustomer").val();
    var id = $("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").find("#HiddenId").val();
    var yearid = $("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").find("#HiddenYearId").val();
    var monthid = $("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").find("#HiddenMonthId").val();
    var dayid = $("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").find("#HiddendayId").val();
    var date = $("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").find("#txt_Date").val().trim();
    var JobTitle = $("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").find("#drpJobTitle").val().trim();
    //var WorkerRatePer = $(".accordion_container").find("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").find("#txt_workerRatePer").val().trim();
    //var WorkerRate = $(".accordion_container").find("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").find("#txt_workerRate").val().trim();
    //var CustomerRatePer = $(".accordion_container").find("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").find("#txt_CustomerRatePer").val().trim();
    //var CustomerRate = $(".accordion_container").find("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").find("#txt_CustomerRate").val().trim();
    var customer = $("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").find("#selectID").val();
    var Project = $("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").find("#drp-Project").val().trim();
    var Comments = $("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").find("#text_Comments").val().trim();
    var DetailDiv = [];
    $.each($("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").find(".Uplift_Detail_Div"), function () {
        var inTimeHr = $(this).find("#drp-HourseListSDTimesheet").val();
        var inTimeMin = $(this).find("#drp-MinutesListSDTimesheet").val();
        var endTimeHr = $(this).find("#drp-HourseListEDTimesheet").val();
        var endTimeMin = $(this).find("#drp-MinutesListEDTimesheet").val();
        var WorkerRatePer = $(this).find("#txt_workerRatePer").val().trim();
        var WorkerRate = $(this).find("#txt_workerRate").val().trim();
        var CustomerRatePer = $(this).find("#txt_CustomerRatePer").val().trim();
        var CustomerRate = $(this).find("#txt_CustomerRate").val().trim();

        var oneData = {
            InTimeHr: inTimeHr,
            InTimeMin: inTimeMin,
            EndTimeHr: endTimeHr,
            EndTimeMin: endTimeMin,
            WorkerRatePer: WorkerRatePer,
            WorkerRate: WorkerRate,
            CustomerRatePer: CustomerRatePer,
            CustomerRate: CustomerRate
        }

        DetailDiv.push(oneData);
    });
    var jsonDetailString = JSON.stringify(DetailDiv);

    var documentList = [];
    $.each($("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").find('#filesList').find(".ListData"), function () {
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
    var comment = $("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").find("#text_Comments").val().trim();   
    var model = {
        Id: id,
        EmployeeId: $("#selectResourceID").val(),
        Date: date,
        Comment: comment,
        yearId: yearid,
        monthId: monthid,
        day: dayid,
        UpliftPostionId: JobTitle,
        //WorkerRate: WorkerRate,
        //WorkerRatePer: WorkerRatePer,
        //CustomerRate: CustomerRate,
        //CustomerRatePer: CustomerRatePer,
        CustomerId: customer,
        ProjectId: Project,
        jsonDocumentList: JsondocumentListJoinString,
        jsonDetailList: jsonDetailString,
        HolidayCountryID: $("#drp-publicHolidayCountry").val(),
        isUpliftDrillDown: $("#isUpliftDrillDown").val(),
        isMonth: $("#isMonthUplift").val()
    }
    $.ajax({
        url: AdminProjectPlanner.SaveData_Uplift,
        type: 'POST',
        data: JSON.stringify(model),
        contentType: "application/json",
        success: function (data) {
            //$("#EmployeeProjectPlanner_UpliftModel").find("#fixedrightcolumn_" + monthid).html("");
            //$("#EmployeeProjectPlanner_UpliftModel").find("#fixedrightcolumn_" + monthid).html(data);
            //$("#EmployeeProjectPlanner_UpliftModel").modal('hide');
            //$("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftModel").find('.close').click();
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
            debugger;
            var isUpliftDrillDown = $("#isUpliftDrillDown").val();
            var ismonth = $("#isMonthUplift").val();
            if (isUpliftDrillDown == 1) {
                UpliftDrillDownData(1, yearid, monthid, dayid);
            }
            else if (isUpliftDrillDown == 0) {
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
            $(".toast-success").show();
            setTimeout(function () { $(".toast-success").hide(); }, 1500);
        }
    });
}
$("#page_content").on("click", "#btn_AddNew_UpliftDetail", function () {
    $.ajax({
        url: AdminProjectPlanner.addEdit_Uplift_Detail,
        success: function (data) {
            var html = $("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").find("#Uplift_detail").html();
            if (html == "") {
                $("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").find("#Uplift_detail").html(data);
            }
            else {
                $("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").find("#Uplift_detail").append(data);
            }
            $('.Uplift_Detail_Div:first').find('.Uplift_delete_icon').parent().hide();
        }
    });
});
$("#page_content").on("click", ".Uplift_delete_icon", function () {
    $(this).parent().parent().parent().parent().remove();
});
function UpliftData(IsUplift, YearId, MonthId, DayId, isUpliftDrillDown,isMonth)
{
    var yearId = YearId;
    var monthId = MonthId;
    var dayId = DayId;
    $.ajax({
        url: AdminProjectPlanner.addEdit_Uplift,
        data: { Id: 0, Date: dayId, Month: monthId, Year: yearId, isUpliftDrillDown: isUpliftDrillDown, isMonth: isMonth },
        success: function (data) {
            $("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").html("");
            $("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").html(data);

            $("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").find('#wizard').smartWizard({
                onLeaveStep: UpliftStepCallback,
                onFinish: UpliftonFinishCallback
            });
            
                $("#btn_AddNew_UpliftDetail").click();
            

            $("#EmployeeProjectPlanner_UpliftModel").modal('show');

            $("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").find('.buttonNext').addClass('btn btn-warning');
            $("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").find('.buttonPrevious').addClass('btn btn-warning');
            $("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").find('.buttonFinish').addClass('btn btn-success');

            $("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").find('.buttonPrevious').hide();
            $("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").find('.buttonFinish').hide();

            $("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").find("#txt_Date").Zebra_DatePicker({
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {
                    $("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").find("#lbl-error-Date").hide();
                }
            });

        }
    });
}
$("#page_content").on('change', '#UpliftFileToUpload', function (e) {
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
                    url: AdminProjectPlanner.ImageData_Uplift,
                    contentType: false,
                    processData: false,
                    data: imageData,
                    success: function (result) {
                        var string = '<div class="row ListData"><div class="col-lg-1 icon"><i class="fa fa-paperclip"></i></div><div class="col-lg-10 attach-disc"><label class="fileName" data-newFileName="' + result.NewFileName + '">' + result.originalFileName + '</label><br /><input type="text" class="ImageDescription form-control" placeholder="Add Description Here"/></div><div class="col-lg-1 file-deleteicon"><i class="fa fa-trash-o"></i></div></div>';
                        var isEmpty = $("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").find("#filesList").html();
                        if (isEmpty = "") {
                            $("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").find("#filesList").html(string);
                        }
                        else {
                            $("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").find("#filesList").append(string);
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

function UpliftDrillDownData(IsUplift, YearId, MonthId, DayId, isMonth)
{
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
        url: AdminProjectPlanner.getUpliftDrillDownData,
        type: 'POST',
        data: { IsUplift: IsUplift, Day: day, month: Month, Year: year, BusiId: busId, DiviId: divId, PoolId: poolId, FunctId: FunId, ProjectId: projectId, isMonth: isMonth, resourceId: resourceId },
        success: function (data) {
            $("#ResourceList").find("#EmployeeSchedulDrillDownModel").find("#EmployeeSchedulDrillDownModelBody").html('');
            $("#ResourceList").find("#EmployeeSchedulDrillDownModel").find("#EmployeeSchedulDrillDownModelBody").html(data);
            //$("#EmployeeFilterDataModal").find("#EmployeeFilterDataModalBody").html('');
            //$("#projectCalendarYear_Month").html('');
            //$("#projectCalendar").html('');
            $("#PartialMonthDay").hide();
            $("#ProjectCalanderMonth").hide();
            $("#ResourceList").html(data);            
        }
    });
}
$('#page_content').on('click', '#btnUplift', function () {
    var hdnYear = $("#hdnYear").val();
    var hdnMonth = $("#hdnMonth").val();
    var hdnDay = $("#hdnDay").val();
    var isUpliftDrillDown = 1;
    var isMonth = $("#isMonthUpliftDrill").val();
    UpliftData(1, hdnYear, hdnMonth, hdnDay, isUpliftDrillDown, isMonth);
})
function EditResourceUplift(EmployeeId)
{
    var EmpID = EmployeeId;
    var hdnYear = $("#hdnYear").val();
    var hdnMonth = $("#hdnMonth").val();
    var hdnDay = $("#hdnDay").val();
    $.ajax({
        url: AdminProjectPlanner.Edit_Uplift,
        data: { EmpId: EmpID, Year: hdnYear, Month: hdnMonth, Day: hdnDay },
        success: function (data) {
            $("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").html("");
            $("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").html(data);
            $("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").find('#wizard').smartWizard({
                onLeaveStep: UpliftStepCallback,
                onFinish: UpliftonFinishCallback
            });
            $("#btn_AddNew_UpliftDetail").click();
            $("#EmployeeProjectPlanner_UpliftModel").modal('show');
            $("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").find('.buttonNext').addClass('btn btn-warning');
            $("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").find('.buttonPrevious').addClass('btn btn-warning');
            $("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").find('.buttonFinish').addClass('btn btn-success');
            $("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").find('.buttonPrevious').hide();
            $("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").find('.buttonFinish').hide();
            $("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").find("#txt_Date").Zebra_DatePicker({
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {
                    $("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").find("#lbl-error-Date").hide();
                }
            });

        }
    });
}
