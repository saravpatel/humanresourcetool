$(document).ready(function () {
    accordation();
    $(".accordion_container").find("#accordion_row_" + constantProjectPlanner.currentMonth).click();
    var positionTop = $(".accordion_container").find("#accordion_table_" + constantProjectPlanner.currentMonth).offset().top;
    $('html, body').animate({ scrollTop: positionTop - 110 }, 'slow');
});
var bol = false;
var valError = false;
function accordation() {
    var acc = document.getElementsByClassName("accordion_row");
    var i;
    for (i = 0; i < acc.length; i++) {
        acc[i].onclick = function () {
            this.classList.toggle("active");
            var panel = this.nextElementSibling;
            if (panel.style.display === "block") {
                panel.style.display = "none";
            } else {
                panel.style.display = "block";
            }
        }
    }
}
function getEmployeeWorkPatternExist()
{
    alert("Please Select Employee WorkPattern");
}
$(".accordion_container").on("click", ".scrollright", function () {
    var monthId = $(this).attr("data-monthid");
    var leftPos = $(".accordion_container").find('#fixedrightcolumn_' + monthId).find('.fixedTable_inner').scrollLeft();
    $(".accordion_container").find('#fixedrightcolumn_' + monthId).find('.fixedTable_inner').animate({ scrollLeft: leftPos + 200 }, 500);
});

$(".accordion_container").on("click", ".scrollleft", function () {
    var monthId = $(this).attr("data-monthid");
    var leftPos = $(".accordion_container").find('#fixedrightcolumn_' + monthId).find('.fixedTable_inner').scrollLeft();
    $(".accordion_container").find('#fixedrightcolumn_' + monthId).find('.fixedTable_inner').animate({ scrollLeft: leftPos - 200 }, 500);
});

$(".changemonth_next").click(function () {
    var yearId = $(this).attr("data-yearid");
    var newYearId = parseInt(yearId) + 1;
    bindChangeYearList(newYearId);
});

$(".changemonth_previous").click(function () {
    var yearId = $(this).attr("data-yearid");
    var newYearId = parseInt(yearId) - 1;
    bindChangeYearList(newYearId);
});

function bindChangeYearList(yearId) {
    $(".hrtoolLoader").show();
    $.ajax({
        url: constantProjectPlanner.ListOfMonth,
        data: { year: yearId, EmployeeId: $("#currentEmployeeId").val(), HolidayCountryId: $("#drp-publicHolidayCountry").val() },
        success: function (data) {
            $(".changemonth_next").attr("data-yearid", yearId);
            $(".changemonth_previous").attr("data-yearid", yearId);
            var newString = "January " + yearId + " - December " + yearId;
            $(".yearsName").html("");
            $(".yearsName").html(newString);

            $(".accordion_container").html("");
            $(".accordion_container").html(data);

            accordation();

            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
        }
    });
}

//TimeSheet Start
function TimeSheetStepCallback(obj, context) {
    debugger;
    if (context.fromStep == 1) {
        var isError = false;
        
        //var bol = getWorkPattData();
        if (bol == "true") {
            isError = false;
        }
        else if (bol == "false") {
            isError = true;
            var r = confirm("Your timesheet hours are not matching with your work pattern. Do you want to proceed?");
            if (r == true) {
                isError = false;
            } else {
                isError = true;
            }
        }
        var date = $(".accordion_container").find("#EmployeeProjectPlanner_TimeSheetModel").find("#EmployeeProjectPlanner_TimeSheetBody").find("#txt_Date").val().trim();
        if (date == "") {
            isError = true;
            $(".accordion_container").find("#EmployeeProjectPlanner_TimeSheetModel").find("#EmployeeProjectPlanner_TimeSheetBody").find("#lbl-error-Date").show();
        }
        $.each($(".accordion_container").find("#EmployeeProjectPlanner_TimeSheetModel").find("#EmployeeProjectPlanner_TimeSheetBody").find(".Timesheet_Detail_Div"), function () {
            var inTimeHr = $(this).find("#drp-HourseListSDTimesheet").val();
            var inTimeMin = $(this).find("#drp-MinutesListSDTimesheet").val();
            if (inTimeHr == 0) {
                isError = true;
                $("#lbl-error-InTime").show();
            }
            var endTimeHr = $(this).find("#drp-HourseListEDTimesheet").val();
            var endTimeMin = $(this).find("#drp-MinutesListEDTimesheet").val();
            if (inTimeHr == 0 ) {
                isError = true;
                $(this).find("#lbl-error-EndTime").show();
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
            $(".accordion_container").find("#EmployeeProjectPlanner_TimeSheetModel").find("#EmployeeProjectPlanner_TimeSheetBody").find('.buttonNext').hide();
            $(".accordion_container").find("#EmployeeProjectPlanner_TimeSheetModel").find("#EmployeeProjectPlanner_TimeSheetBody").find('.buttonPrevious').show();
            $(".accordion_container").find("#EmployeeProjectPlanner_TimeSheetModel").find("#EmployeeProjectPlanner_TimeSheetBody").find('.buttonFinish').show();
            return true;
        }
    }
    else {
        $(".accordion_container").find("#EmployeeProjectPlanner_TimeSheetModel").find("#EmployeeProjectPlanner_TimeSheetBody").find('.buttonNext').show();
        $(".accordion_container").find("#EmployeeProjectPlanner_TimeSheetModel").find("#EmployeeProjectPlanner_TimeSheetBody").find('.buttonPrevious').hide();
        $(".accordion_container").find("#EmployeeProjectPlanner_TimeSheetModel").find("#EmployeeProjectPlanner_TimeSheetBody").find('.buttonFinish').hide();
        return true;
    }
}
function HoursList() {
    $("#lbl-error-InTime").hide();
    $("#lbl-error-EndTime").hide();
    var St = $('#drp-HourseListSDTimesheet').val();
    var Sm = $('#drp-MinutesListSDTimesheet').val();
    var Et = $('#drp-HourseListEDTimesheet').val();
    var Em = $('#drp-MinutesListEDTimesheet').val();
    Diff = ((new Date("1991-1-1 " + Et + "" + ":" + "" + Em + "") - new Date("1991-1-1 " + St + "" + ":" + "" + Sm + "")) / 1000 / 60 / 60).toFixed(2);
    if (Diff <= 0) {
        $("#lbl-error-validtimeSD").show();
        $("#drp-HourseListEDTimesheet").val('');
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

function getWorkPattData() {
    var DetailDiv = [];
    var eid = $("#currentEmployeeId").val();
    var date = $("#txt_Date").val();
    $.each($(".accordion_container").find("#EmployeeProjectPlanner_TimeSheetModel").find("#EmployeeProjectPlanner_TimeSheetBody").find(".Timesheet_Detail_Div"), function () {
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
        EmployeeId: $("#currentEmployeeId").val(),
        Date: date,
        jsonDetailList: jsonDetailString
    }
    $.ajax({
        url: constantProjectPlanner.workPattenData,
        type: 'POST',
        data: model,
        success: function (data) {
            debugger;
            bol = JSON.stringify(data);
        }
        
    });
    return bol;
}
function TimeSheetonFinishCallback(obj, context) {
    var id = $(".accordion_container").find("#EmployeeProjectPlanner_TimeSheetModel").find("#EmployeeProjectPlanner_TimeSheetBody").find("#HiddenId").val();
    var yearid = $(".accordion_container").find("#EmployeeProjectPlanner_TimeSheetModel").find("#EmployeeProjectPlanner_TimeSheetBody").find("#HiddenYearId").val();
    var monthid = $(".accordion_container").find("#EmployeeProjectPlanner_TimeSheetModel").find("#EmployeeProjectPlanner_TimeSheetBody").find("#HiddenMonthId").val();
    var dayid = $(".accordion_container").find("#EmployeeProjectPlanner_TimeSheetModel").find("#EmployeeProjectPlanner_TimeSheetBody").find("#HiddendayId").val();
    var date = $(".accordion_container").find("#EmployeeProjectPlanner_TimeSheetModel").find("#EmployeeProjectPlanner_TimeSheetBody").find("#txt_Date").val().trim();
    var DetailDiv = [];
    $.each($(".accordion_container").find("#EmployeeProjectPlanner_TimeSheetModel").find("#EmployeeProjectPlanner_TimeSheetBody").find(".Timesheet_Detail_Div"), function () {
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
    $.each($(".accordion_container").find("#EmployeeProjectPlanner_TimeSheetModel").find("#EmployeeProjectPlanner_TimeSheetBody").find('#filesList').find(".ListData"), function () {
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
    var comment = $(".accordion_container").find("#EmployeeProjectPlanner_TimeSheetModel").find("#EmployeeProjectPlanner_TimeSheetBody").find("#text_Comments").val().trim();
    var model = {
        Id: id,
        EmployeeId: $("#currentEmployeeId").val(),
        Date: date,
        Comment: comment,
        yearId: yearid,
        monthId: monthid,
        day: dayid,
        jsonDocumentList: JsondocumentListJoinString,
        jsonDetailList: jsonDetailString,
        HolidayCountryID: $("#drp-publicHolidayCountry").val()
    }
    $.ajax({
        url: constantProjectPlanner.SaveData_TimeSheet,
        type: 'POST',
        data: JSON.stringify(model),
        contentType: "application/json",
        success: function (data) {
            debugger;
            $(".accordion_container").find("#fixedrightcolumn_" + monthid).html("");
            $(".accordion_container").find("#fixedrightcolumn_" + monthid).html(data);
            $(".accordion_container").find("#EmployeeProjectPlanner_TimeSheetModel").find('.close').click();
            $("#timesheetCnt").html('0000');
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

$(".accordion_container").on("click", ".btn_AddEdit_TimeSheet", function () {
    var yearId = $(this).attr("data-yearid");
    var monthId = $(this).attr("data-monthid");
    var dayId = $(this).attr("data-dayid");
    var id = $(this).attr("data-id");
    $.ajax({
        url: constantProjectPlanner.addEdit_TimeSheet,
        data: { Id: id, Date: dayId, Month: monthId, Year: yearId },
        success: function (data) {
            $(".accordion_container").find("#EmployeeProjectPlanner_TimeSheetModel").find("#EmployeeProjectPlanner_TimeSheetBody").html("");
            $(".accordion_container").find("#EmployeeProjectPlanner_TimeSheetModel").find("#EmployeeProjectPlanner_TimeSheetBody").html(data);
            
            $(".accordion_container").find("#EmployeeProjectPlanner_TimeSheetModel").find("#EmployeeProjectPlanner_TimeSheetBody").find('#wizard').smartWizard({
                onLeaveStep: TimeSheetStepCallback,
                onFinish: TimeSheetonFinishCallback
            });

            $(".accordion_container").find("#EmployeeProjectPlanner_TimeSheetModel").find("#EmployeeProjectPlanner_TimeSheetBody").find('.buttonNext').addClass('btn btn-warning');
            $(".accordion_container").find("#EmployeeProjectPlanner_TimeSheetModel").find("#EmployeeProjectPlanner_TimeSheetBody").find('.buttonPrevious').addClass('btn btn-warning');
            $(".accordion_container").find("#EmployeeProjectPlanner_TimeSheetModel").find("#EmployeeProjectPlanner_TimeSheetBody").find('.buttonFinish').addClass('btn btn-success');

            $(".accordion_container").find("#EmployeeProjectPlanner_TimeSheetModel").find("#EmployeeProjectPlanner_TimeSheetBody").find('.buttonPrevious').hide();
            $(".accordion_container").find("#EmployeeProjectPlanner_TimeSheetModel").find("#EmployeeProjectPlanner_TimeSheetBody").find('.buttonFinish').hide();

            $(".accordion_container").find("#EmployeeProjectPlanner_TimeSheetModel").find("#EmployeeProjectPlanner_TimeSheetBody").find("#txt_Date").Zebra_DatePicker({
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {
                    (".accordion_container").find("#EmployeeProjectPlanner_TimeSheetModel").find("#EmployeeProjectPlanner_TimeSheetBody").find("#lbl-error-Date").hide();
                }
            });

        }
    });
});

$(".accordion_container").on("click", "#btn_AddNew_TimesheetDetail", function () {
    $.ajax({
        url: constantProjectPlanner.addEdit_TimeSheet_Detail,
        success: function (data) {
            var html = $(".accordion_container").find("#EmployeeProjectPlanner_TimeSheetModel").find("#EmployeeProjectPlanner_TimeSheetBody").find("#TimeSheet_detail").html();
            if (html == "") {
                $(".accordion_container").find("#EmployeeProjectPlanner_TimeSheetModel").find("#EmployeeProjectPlanner_TimeSheetBody").find("#TimeSheet_detail").html(data);
            }
            else {
                $(".accordion_container").find("#EmployeeProjectPlanner_TimeSheetModel").find("#EmployeeProjectPlanner_TimeSheetBody").find("#TimeSheet_detail").append(data);
            }
            spinner();
        }
    });
});

$(".accordion_container").on("click", ".timesheet_delete_icon", function () {
    $(this).parent().parent().parent().parent().remove();
});
$("#page_content").on('click', '.file-deleteicon', function () {
    $(this).parent().remove();
});
$(".accordion_container").on('change', '#TimeSheetFileToUpload', function (e) {
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
                    url: constantProjectPlanner.ImageData_TimeSheet,
                    contentType: false,
                    processData: false,
                    data: imageData,
                    success: function (result) {
                        var string = '<div class="row ListData"><div class="col-lg-1 icon"><i class="fa fa-paperclip"></i></div><div class="col-lg-10 attach-disc"><label class="fileName" data-newFileName="' + result.NewFileName + '">' + result.originalFileName + '</label><br /><input type="text" class="ImageDescription form-control" placeholder="Add Description Here"/></div><div class="col-lg-1 file-deleteicon"><i class="fa fa-trash-o"></i></div></div>';
                        var isEmpty = $(".accordion_container").find("#EmployeeProjectPlanner_TimeSheetModel").find("#EmployeeProjectPlanner_TimeSheetBody").find("#filesList").html();
                        if (isEmpty = "") {
                            $(".accordion_container").find("#EmployeeProjectPlanner_TimeSheetModel").find("#EmployeeProjectPlanner_TimeSheetBody").find("#filesList").html(string);
                        }
                        else {
                            $(".accordion_container").find("#EmployeeProjectPlanner_TimeSheetModel").find("#EmployeeProjectPlanner_TimeSheetBody").find("#filesList").append(string);
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

//TimeSheet End

// Travel leave Start

$(".accordion_container").on("click", ".btn_AddEdit_TravelLeave", function () {

    var yearId = $(this).attr("data-yearid");
    var monthId = $(this).attr("data-monthid");
    var dayId = $(this).attr("data-dayid");
    var id = $(this).attr("data-id");
    $.ajax({
        url: constantProjectPlanner.addEdit_TravelLeave,
        data: { Id: id, Date: dayId, Month: monthId, Year: yearId },
        success: function (data) {

            $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").html("");
            $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").html(data);

            $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find('#wizard').smartWizard({
                onLeaveStep: TravelleaveStepCallback,
                onFinish: TravelLeaveonFinishCallback
            });

            $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find('.buttonNext').addClass('btn btn-warning');
            $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find('.buttonPrevious').addClass('btn btn-warning');
            $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find('.buttonFinish').addClass('btn btn-success');

            $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find('.buttonPrevious').hide();
            $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find('.buttonFinish').hide();

            $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#txt_StartDate").Zebra_DatePicker({
                //direction: false,
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {
                    $("#lbl-error-GreaterEndDate").hide();
                    var startdate = $("#txt_StartDate").val();
                    var enddate = $("#txt_EndDate").val();
                    calculateDateDiff(startdate, enddate);
                    //calculateAnnualDateDiffTravelLeave();
                }
            });
            $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#txt_EndDate").Zebra_DatePicker({
                //direction: false,
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {
                    $("#lbl-error-GreaterEndDate").hide();

                      var startdate = $("#txt_StartDate").val();
                        var enddate = $("#txt_EndDate").val();
                        calculateDateDiff(startdate, enddate);
                    //calculateAnnualDateDiffTravelLeave();
                }
            });

            $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#txt_LessThenStartDate").Zebra_DatePicker({
                //direction: false,
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {

                }
            });

            if (id > 0) {
                var selectedRadio = $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find('input[name=lessThanADay]:checked').attr("id");
                if (selectedRadio == "adayormore") {
                    $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#div_ADayOrMore").show();
                    $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#div_LessThenADay").hide();
                }
                else {
                    $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#div_LessThenADay").show();
                    $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody  ").find("#div_ADayOrMore").hide();
                }
            }

            spinner();

        }
    });
});

function calculateAnnualDateDiffTravelLeave() {
    var stratDate = $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#txt_StartDate").val().trim();
    var endDate = $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#txt_EndDate").val().trim();
    if (stratDate != "" && endDate != "") {
        var splitStartDate = stratDate.split('-');
        var splitEndDate = endDate.split('-');
        var oneDay = 24 * 60 * 60 * 1000;
        var firstDate = new Date(splitStartDate[2], splitStartDate[1], splitStartDate[0]);
        var secondDate = new Date(splitEndDate[2], splitEndDate[1], splitEndDate[0]);
        var diffDays = Math.round(Math.abs((firstDate.getTime() - secondDate.getTime()) / (oneDay)));
        diffDays = diffDays + 1;
        $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#txt_Duration").val(diffDays);
    }
}

$(".accordion_container").on("change", ".Travel_leave_radio", function () {
    var Id = $(this).attr("id");
    if (Id == "adayormore") {
        $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#div_ADayOrMore").show();
        $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#div_LessThenADay").hide();
    }
    else {
        $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#div_LessThenADay").show();
        $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#div_ADayOrMore").hide();
    }
});

$(".accordion_container").on("change", "#drp-FromCountryId", function () {
    $("#drp-FromStateId").val('');
    $("#drp-FromTownId").val('');
    var value = $(this).val();
    if (value != "0") {
        $.ajax({
            url: constantProjectPlanner.BindState,
            data: { countryId: value },
            success: function (data) {
                $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-FromStateId").html('');
                var toAppend = '';
                $.each(data, function (index, item) {
                    toAppend += "<option value='" + item.Value + "'>" + item.Text + "</option>";
                })
                $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-FromStateId").html(toAppend);

                $.ajax({
                    url: constantProjectPlanner.BindAirport,
                    data: { countryId: value },
                    success: function (data) {
                        $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-FromAirportId").html('');
                        $("#drpAirport").html('');
                        var toAppendAirport = '';
                        $.each(data, function (index, item) {
                            toAppendAirport += "<option value='" + item.Value + "'>" + item.Text + "</option>";
                        })
                        $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-FromAirportId").html(toAppendAirport);
                    }
                });

            }
        });
    }
    else {
        $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-FromStateId").html('');
        var toAppend = "<option value='0'>--Select--</option>";
        $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-FromStateId").html(toAppend);
        $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-FromAirportId").html('');
        $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-FromAirportId").html(toAppend);
    }
});

$(".accordion_container").on("change", "#drp-FromStateId", function () {
    $("#lbl-error-FromTownId").hide();
    var value = $(this).val();
    if (value != "0") {
        $.ajax({
            url: constantProjectPlanner.BindCity,
            data: { stateId: value },
            success: function (data) {
                $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-FromTownId").html('');
                var toAppend = '';
                $.each(data, function (index, item) {
                    toAppend += "<option value='" + item.Value + "'>" + item.Text + "</option>";
                })
                $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-FromTownId").html(toAppend);
            }
        });
    }
    else {
        $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-FromTownId").html('');
        var toAppend = "<option value='0'>--Select--</option>";
        $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-FromTownId").html(toAppend);
    }
});

$(".accordion_container").on("change", "#drp-ToCountryId", function () {
    $("#drp-ToStateId").val('');
    $("#drp-ToTownId").val('');
    var value = $(this).val();
    if (value != "0") {
        $.ajax({
            url: constantProjectPlanner.BindState,
            data: { countryId: value },
            success: function (data) {
                $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-ToStateId").html('');
                var toAppend = '';
                $.each(data, function (index, item) {
                    toAppend += "<option value='" + item.Value + "'>" + item.Text + "</option>";
                })
                $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-ToStateId").html(toAppend);

                $.ajax({
                    url: constantProjectPlanner.BindAirport,
                    data: { countryId: value },
                    success: function (data) {
                        $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-ToAirportId").html('');
                        var toAppendAirport = '';
                        $.each(data, function (index, item) {
                            toAppendAirport += "<option value='" + item.Value + "'>" + item.Text + "</option>";
                        })
                        $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-ToAirportId").html(toAppendAirport);
                    }
                });

            }
        });
    }
    else {
        $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-ToStateId").html('');
        var toAppend = "<option value='0'>--Select--</option>";
        $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-ToStateId").html(toAppend);
        $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-ToAirportId").html('');
        $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-ToAirportId").html(toAppend);
    }
});

$(".accordion_container").on("change", "#drp-ToStateId", function () {
    $("#lbl-error-ToTownId").hide();
    var value = $(this).val();
    if (value != "0") {
        $.ajax({
            url: constantProjectPlanner.BindCity,
            data: { stateId: value },
            success: function (data) {
                $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-ToTownId").html('');
                var toAppend = '';
                $.each(data, function (index, item) {
                    toAppend += "<option value='" + item.Value + "'>" + item.Text + "</option>";
                })
                $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-ToTownId").html(toAppend);
            }
        });
    }
    else {
        $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-ToTownId").html('');
        var toAppend = "<option value='0'>--Select--</option>";
        $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-ToTownId").html(toAppend);
    }
});
$("#page_content").on('change', '#drp-TravelHourseListSD', function (e) {
    $("#lbl-error-InTimeSD").hide();
    $("#lbl-error-DurationHours").hide();
    $("#lbl-error-validtimeSD").hide();
    var St = $(this).val();
    var Sm = $("#drp-MinutesTravelListSD").val();
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
    else { $("#txt_DurationHours").val(Diff);
            valError = false;
        }
    return valError;


});
//drp-HourseListED   txt_DurationHours
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
        valError=true;

    }
    else {
        $("#txt_DurationHours").val(Diff);
        valError = false;;
    }
    return valError;

});
//drp-MinutesListED
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
        valError = true;;
    }
    else { $("#txt_DurationHours").val(Diff);
    valError=false;
    }
    return valError;
});
function TravelleaveStepCallback(obj, context) {
    if (context.fromStep == 1) {

        var isError = false;
        var selectedRadio = $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find('input[name=lessThanADay]:checked').attr("id");
        if (selectedRadio == "adayormore") {
            var fromCountry = $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-FromCountryId").val();
            var fromState = $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-FromStateId").val();
            var fromTown = $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-FromTownId").val();
            var fromAirpoet = $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-FromAirportId").val();
            var toCountry = $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-ToCountryId").val();
            var toState = $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-ToStateId").val();
            var toTown = $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-ToTownId").val();
            var toAirport = $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-ToAirportId").val();

            var reasonForLeave = $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-ReasonForTravelId").val();
            var startDate = $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#txt_StartDate").val().trim();
            var endDate = $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#txt_EndDate").val().trim();
            var comments = $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#text_Comments").val().trim();
            var duration = $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#txt_Duration").val().trim();

            var type = $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-Type").val();
            var customer = $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#selectID").val();
            var project = $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-Project").val();
            var costCode = $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-CostCode").val();

            var isLessThenADay = false;

            if (fromCountry == "0") { isError = true; $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#lbl-error-FromCountryId").show(); }
            if (fromState == "0") { isError = true; $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#lbl-error-FromStateId").show(); }
            if (fromTown == "0") { isError = true; $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#lbl-error-FromTownId").show(); }
            if (toCountry == "0") { isError = true; $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#lbl-error-ToCountryId").show(); }
            if (toState == "0") { isError = true; $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#lbl-error-ToStateId").show(); }
            if (toTown == "0") { isError = true; $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#lbl-error-ToTownId").show(); }

            if (reasonForLeave == "0") { isError = true; $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#lbl-error-ReasonForTravelId").show(); }
            if (startDate == "") { isError = true; $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#lbl-error-StartDate").show(); }
            if (endDate == "") { isError = true; $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#lbl-error-EndDate").show(); }
            if (comments == "") { isError = true; $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#lbl-error-Comments").show(); }

            if (type == "0") { isError = true; $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#lbl-error-Type").show(); }
            if (costCode == "0") { isError = true; $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#lbl-error-CostCode").show(); }

            if (isError) {
                return false;
            }
            else {
                $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find('.buttonNext').hide();
                $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find('.buttonPrevious').show();
                $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find('.buttonFinish').show();
                return true;
            }
        }
        else {
            var fromCountry = $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-FromCountryId").val();
            var fromState = $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-FromStateId").val();
            var fromTown = $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-FromTownId").val();
            var fromAirpoet = $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-FromAirportId").val();
            var toCountry = $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-ToCountryId").val();
            var toState = $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-ToStateId").val();
            var toTown = $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-ToTownId").val();
            var toAirport = $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-ToAirportId").val();

            var reasonForLeave = $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-ReasonForTravelId").val();
            var startDate = $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#txt_LessThenStartDate").val().trim();
            var isLessThenADay = true;
            var comments = $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#text_Comments").val().trim();
            var type = $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-Type").val();
            var customer = $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#selectID").val();
            var project = $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-Project").val();
            var costCode = $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-CostCode").val();
            var drphrSD = $('#drp-TravelHourseListSD').val();
            var drpminSD = $('#drp-MinutesTravelListSD').val();
            var drphrED = $('#drp-TravelHourseListED').val();
            var drminED = $('#drp-TravelMinutesListED').val();
            if (valError == true) {
                isError = true;
                $("#lbl-error-validtimeSD").show();
            }
            else if (valError == false) {
                isError = false;
            }
            if (drphrSD == 0) {
                isError = true; $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#lbl-error-InTimeSD").show();
            }

            if (drphrED == 0) {
                isError = true; $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#lbl-error-EndTime").show();
            }

            if (fromCountry == "0") { isError = true; $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#lbl-error-FromCountryId").show(); }
            if (fromState == "0") { isError = true; $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#lbl-error-FromStateId").show(); }
            if (fromTown == "0") { isError = true; $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#lbl-error-FromTownId").show(); }
            if (toCountry == "0") { isError = true; $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#lbl-error-ToCountryId").show(); }
            if (toState == "0") { isError = true; $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#lbl-error-ToStateId").show(); }
            if (toTown == "0") { isError = true; $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#lbl-error-ToTownId").show(); }
            if (reasonForLeave == "0") { isError = true; $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#lbl-error-ReasonForTravelId").show(); }
            if (startDate == "") { isError = true; $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#lbl-error-LessThenStartDate").show(); }
            if (comments == "") { isError = true; $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#lbl-error-Comments").show(); }
            if (type == "0") { isError = true; $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#lbl-error-Type").show(); }
            if (costCode == "0") { isError = true; $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#lbl-error-CostCode").show(); }
            if (isError) {
                return false;
            }
            else {

                $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find('.buttonNext').hide();
                $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find('.buttonPrevious').show();
                $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find('.buttonFinish').show();
                return true;
            }
        }

    }
    else {
        $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find('.buttonNext').show();
        $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find('.buttonPrevious').hide();
        $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find('.buttonFinish').hide();
        return true;
    }
}

function TravelLeaveonFinishCallback(obj, context) {
    var id = $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#HiddenId").val();
    var yearid = $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#HiddenYearId").val();
    var monthid = $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#HiddenMonthId").val();
    var dayid = $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#HiddendayId").val();
    var fromCountry = $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-FromCountryId").val();
    var fromState = $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-FromStateId").val();
    var fromTown = $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-FromTownId").val();
    var fromAirpoet = $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-FromAirportId").val();
    var toCountry = $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-ToCountryId").val();
    var toState = $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-ToStateId").val();
    var toTown = $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-ToTownId").val();
    var toAirport = $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-ToAirportId").val();
    var type = $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-Type").val();
    var customer = $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#selectID").val();
    var project = $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-Project").val();
    var costCode = $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-CostCode").val();


    var selectedRadio = $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find('input[name=lessThanADay]:checked').attr("id");

    var documentList = [];
    $.each($(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find('#filesList').find(".ListData"), function () {
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
        var reasonForLeave = $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-ReasonForTravelId").val();
        var startDate = $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#txt_StartDate").val().trim();
        var endDate = $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#txt_EndDate").val().trim();
        var comments = $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#text_Comments").val().trim();
        var duration = $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#txt_Duration").val().trim();
        var isLessThenADay = false;
        SaveTravelLeave(id, isLessThenADay, reasonForLeave, startDate, endDate, duration, comments, yearid, monthid, dayid, 0, 0,0,0,0, JsondocumentListJoinString, fromCountry, fromState, fromTown, fromAirpoet, toCountry, toState, toTown, toAirport, type, customer, project, costCode);
    }
    else {
        var reasonForLeave = $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-ReasonForTravelId").val();
        var startDate = $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#txt_LessThenStartDate").val().trim();
        var hourSD = $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-TravelHourseListSD").val();
        var minSD = $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-MinutesTravelListSD").val();
        var hourED = $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-TravelHourseListED").val();
        var minED = $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#drp-TravelMinutesListED").val();
        var durationHr = $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#txt_DurationHours").val();
        var isLessThenADay = true;
        var comments = $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#text_Comments").val().trim();
        SaveTravelLeave(id, isLessThenADay, reasonForLeave, startDate, "", 0, comments, yearid, monthid, dayid, hourSD, minSD, hourED, minED, durationHr,JsondocumentListJoinString, fromCountry, fromState, fromTown, fromAirpoet, toCountry, toState, toTown, toAirport, type, customer, project, costCode);
    }
}

function SaveTravelLeave(id, isLessThenADay, reasonForLeaveId, startDate, endDate, duration, comments, yearid, monthid, dayid, hourSD, minSD, hourED, minED, durationHr, jsonString, fromCountry, fromState, fromTown, fromAirpoet, toCountry, toState, toTown, toAirport, type, customer, project, costCode) {
    $(".hrtoolLoader").show();
    var model = {
        Id: id,
        EmployeeId: $("#currentEmployeeId").val(),
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
        HolidayCountryID: $("#drp-publicHolidayCountry").val()
    }

    $.ajax({
        url: constantProjectPlanner.SaveData_TravelLeave,
        type: 'POST',
        data: JSON.stringify(model),
        contentType: "application/json",
        success: function (data) {
            $(".accordion_container").find("#fixedrightcolumn_" + monthid).html("");
            $(".accordion_container").find("#fixedrightcolumn_" + monthid).html(data);

            $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find('.close').click();

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

$(".accordion_container").on('change', '#TravelLeaveFileToUpload', function (e) {
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
                    url: constantProjectPlanner.ImageData_TravelLeave,
                    contentType: false,
                    processData: false,
                    data: imageData,
                    success: function (result) {
                        var string = '<div class="row ListData"><div class="col-lg-1 icon"><i class="fa fa-paperclip"></i></div><div class="col-lg-10 attach-disc"><label class="fileName" data-newFileName="' + result.NewFileName + '">' + result.originalFileName + '</label><br /><input type="text" class="ImageDescription form-control" placeholder="Add Description Here"/></div><div class="col-lg-1 file-deleteicon"><i class="fa fa-trash-o"></i></div></div>';
                        var isEmpty = $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#filesList").html();
                        if (isEmpty = "") {
                            $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#filesList").html(string);
                        }
                        else {
                            $(".accordion_container").find("#EmployeeProjectPlanner_TravelLeaveModel").find("#EmployeeProjectPlanner_TravelLeaveBody").find("#filesList").append(string);
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

//Travel leave End
    
//common modal popup close scroll event
$(".accordion_container").on("click", ".closeModel", function () {
    var monthId = $(this).parent().parent().find('#HiddenMonthId').val();
    var positionTop = $(".accordion_container").find("#accordion_table_" + monthId).offset().top;
    $('html, body').animate({ scrollTop: positionTop - 110 }, 'slow');
});


//Settings btn-PlannerSettings
$("#page_content").on("click", ".btn-PlannerProjectSettings", function () {
    var Id = $("#page_content").find("#currentEmployeeId").val();
    $.ajax({
        url: constantProjectPlanner.BindSettings,
        data: { Id: Id },
        success: function (data) {
            $("#page_content").find("#MeEmployeeProjectPlanner_Settings").find("#MeEmployeeProjectPlanner_SettingsBody").html('');
            $("#page_content").find("#MeEmployeeProjectPlanner_Settings").find("#MeEmployeeProjectPlanner_SettingsBody").html(data);
            $('[data-toggle="tooltip"]').tooltip();
        }
    });

});

//btn-Project-closeModel
$("#page_content").on("click", ".btn-Project-closeModel", function () {

    var Id = $("#page_content").find("#currentEmployeeId").val();
    var model = {
        Id: $("#page_content").find("#EmployeeSettingsID").val(),
        HolidayYear: $("#page_content").find("#drp_Holiday_Year").val(),
        MeasuredIn: $("#page_content").find("#drp_MeasuredIn").val(),
        Thisyear: $("#page_content").find("#Thisyear").val(),
        Nextyear: $("#page_content").find("#Nextyear").val(),
        TOIL: $("#page_content").find("#TOIL").val(),
        CarriedOver: $("#page_content").find("#CarriedOver").val(),
        AuthoriseUserId: $("#page_content").find("#drp_authorisations").val(),
        EntitlementIncludesPublicHoliday: $("#check_EntitlementIncludes").is(":checked"),
        AutoApproveHolidays: $("#check_AutoApproveHolidays").is(":checked"),
        ExceedAllowance: $("#check_ExceedAllowance").is(":checked")
    }

    $.ajax({
        url: constantProjectPlanner.SaveAnnualSettings,
        type: 'POST',
        data: JSON.stringify(model),
        contentType: "application/json",
        success: function (data) {



        }
    });

});

//ADD TOIL

$("#page_content").on("click", "#ADDProjectLeaves", function () {

    var Id = $("#page_content").find("#EmployeeSettingsID").val();
    $.ajax({
        url: constantProjectPlanner.AddEditTOIL,
        data: { Id: Id },
        success: function (data) {

            $("#EmployeeProjectPlanner_TOIL").find("#EmployeeProjectPlanner_TOILBody").html('');
            $("#EmployeeProjectPlanner_TOIL").find("#EmployeeProjectPlanner_TOILBody").html(data);
            $('[data-toggle="tooltip"]').tooltip();
            $("#page_content").find("#EmployeeProjectPlanner_Settings").find("#EmployeeProjectPlanner_TOIL").find("#txt_ExpiryDate").Zebra_DatePicker({
                //direction: false,
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {
                    $("#page_content").find("#lbl-error-ExpiryDate").hide();
                    $("#page_content").find("#lbl-error-ExpiryDateOver").hide();

                }
            });

        }
    });

});

$("#page_content").on("click", "#ProjectDurationDays_Plus", function () {

    var value = $(this).html();
    if (value == "+") {
        $(this).html('');
        $(this).html('-');
    }
    else {
        $(this).html('');
        $(this).html('+');
    }

});

$("#page_content").on("click", "#btn-submit-EmployeeProjectPlanner_TOIL", function () {

    $(".hrtoolLoader").show();
    $(".modal-backdrop").show();
    var IsError = false;
    var Id = $("#page_content").find("#TOIL_Id").val();
    var EmployeeId = $("#page_content").find("#EmployeeSettingsID").val();
    var Balance = $("#page_content").find("#Toil_Balance").val();
    var DurationDays = $("#page_content").find("#Toil_DurationDays").val();
    var AddEdit = $("#page_content").find("#ProjectDurationDays_Plus").html();
    if (AddEdit == "+") {
        AddEdit = true;
    }
    else {
        AddEdit = false;
    }
    var SupportingComments = $("#page_content").find("#Txt_SupportingComments").val().trim();
    var ExpiryDate = $("#page_content").find("#txt_ExpiryDate").val().trim();
    if (ExpiryDate == "") {
        IsError = true;
        $("#page_content").find("#lbl-error-ExpiryDate").show();
    }
    if (ExpiryDate != "") {
        var fromDate = ExpiryDate;
        var toDate = $('#TOIL_StartDate').val();
        if (fromDate != "") {
            fromDate = fromDate.replace(/-/g, '/');
            toDate = toDate.replace(/-/g, '/');
            if (toDate != "") {
                if (!FromDateValidation(fromDate, toDate)) {
                    IsError = true;
                    $("#page_content").find("#lbl-error-ExpiryDateOver").show();
                }
            }
        }
    }

    if (IsError) {
        $(".hrtoolLoader").hide();
        $(".modal-backdrop").hide();
        return false;
    }
    else {
        var model = {
            Id: Id,
            EmployeeId: EmployeeId,
            DurationDays: DurationDays,
            ExpiryDate: ExpiryDate,
            SupportingComments: SupportingComments,
            AddEdit: AddEdit,
            Balance: Balance,
        }

        $.ajax({
            url: constantProjectPlanner.SaveTOIL,
            type: 'POST',
            data: JSON.stringify(model),
            contentType: "application/json",
            success: function (data) {
                $("#EmployeeProjectPlanner_Settings").find("#EmployeeProjectPlanner_SettingsBody").html('');
                $("#EmployeeProjectPlanner_Settings").find("#EmployeeProjectPlanner_SettingsBody").html(data);
                $('[data-toggle="tooltip"]').tooltip();
                $(".hrtoolLoader").hide();
                $(".modal-backdrop").hide();
            }
        });

    }
});

// Work Pattern
$("#page_content").on('click', '.Close_work', function () {
    var value = $("#page_content").find("#Employee_WorkPatternID").val();
    $("#page_content").find("#drp_DefaultWorkPattern").val(value);

});

$("#page_content").on('click', '.WorkPatternHistory', function () {
    $(".hrtoolLoader").show();
    var EmployeeId = $("#currentEmployeeId").val();
    $.ajax({
        url: constantProjectPlanner.workPattenHistory,
        data: { EmployeeId: EmployeeId },
        success: function (data) {

            $("#HistoryWorkPattern").find("#HistoryWorkPatternBody").html('');
            $("#HistoryWorkPattern").find("#HistoryWorkPatternBody").html(data);
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
        }
    });
});

$("#page_content").on('change', '#drp_DefaultWorkPattern', function (e) {
    $(".hrtoolLoader").show();
    var value = $(this).val();
    if (value == "AddNew_WorkPattern") {
        $.ajax({
            url: constantProjectPlanner.workPatten,
            data: { Id: 0 },
            success: function (data) {
                $("#DefaultWorkPatternModal").modal('show');

                $('#DefaultWorkPatternBody').html('');
                $('#DefaultWorkPatternBody').html(data);
                $('#DefaultWorkPatternBody').find(".timeMask").mask('00:00');
                $(".hrtoolLoader").hide();
                $(".modal-backdrop").hide();
            }
        });
    }
    if (value != "0") {
        $.ajax({
            url: constantProjectPlanner.workPatten,
            data: { Id: value },
            success: function (data) {
                $("#ReadOnlyWorkPatternModal").modal('show');
                $('#ReadOnlyWorkPatternBody').html('');
                $('#ReadOnlyWorkPatternBody').html(data);
                $("#page_content").find("#ReadOnlyWorkPatternModal").find("#ReadOnlyWorkPatternBody").find("#txt_Effectivefromdate").Zebra_DatePicker({
                    //direction: false,
                    showButtonPanel: false,
                    format: 'd-m-Y',
                    onSelect: function () {
                        $("#page_content").find("#lbl-error-Effectivefromdate").hide();

                    }
                });
                $('#ReadOnlyWorkPatternBody').find(".timeMask").mask('00:00');
                $(".hrtoolLoader").hide();
                $(".modal-backdrop").hide();
            }
        });
    }
});

$('#page_content').on('change', '#AllowRotating', function () {

    var chkProp = $(this).prop("checked");
    if (chkProp == true) {
        var isAvailable = $('#DefaultWorkPatternBody').find("#TrueIsRotatingDiv").length;
        if (isAvailable > 0) {
            $('#DefaultWorkPatternBody').find("#FalseIsRotatingDiv").hide();
            $('#DefaultWorkPatternBody').find("#TrueIsRotatingDiv").show();
            var newWeek = parseInt(isAvailable) + 1;
            $('#DefaultWorkPatternBody').find(".LastIndex:last").html('Week ' + newWeek);
            $('#DefaultWorkPatternBody').find(".LastIndex:last").attr('data-name', newWeek);
        }
        else {
            var workPatternId = $('#DefaultWorkPatternBody').find("#WorkPattenId").val();
            $.ajax({
                url: constantProjectPlanner.TrueIsRotating,
                data: { workPatternId: 0 },
                success: function (data) {
                    $('#DefaultWorkPatternBody').find("#FalseIsRotatingDiv").hide();
                    $('#DefaultWorkPatternBody').find('#workPatternDiv').append(data);
                    $('#DefaultWorkPatternBody').find('#workPatternDiv').find(".timeMask").mask('00:00');
                }
            });
        }
    }
    else {
        var isAvailable = $('#DefaultWorkPatternBody').find("#FalseIsRotatingDiv").length;
        if (isAvailable > 0) {
            $('#DefaultWorkPatternBody').find("#TrueIsRotatingDiv").hide();
            $('#DefaultWorkPatternBody').find("#FalseIsRotatingDiv").show();
            var newWeek = parseInt(isAvailable) + 1;
            $('#DefaultWorkPatternBody').find(".LastIndex:last").html('Week ' + newWeek);
            $('#DefaultWorkPatternBody').find(".LastIndex:last").attr('data-name', newWeek);
        }
        else {
            $.ajax({
                url: constantProjectPlanner.FalseIsRotating,
                success: function (data) {
                    $('#DefaultWorkPatternBody').find("#TrueIsRotatingDiv").hide();
                    $('#DefaultWorkPatternBody').find('#workPatternDiv').append(data);
                    $('#DefaultWorkPatternBody').find('#workPatternDiv').find(".timeMask").mask('00:00');
                }
            });
        }
    }
});


$('#DefaultWorkPatternBody').on('click', '#AddNewTable', function () {

    var isAvailable = $('#DefaultWorkPatternBody').find("#TrueIsRotatingDiv").find('table').length;
    if (isAvailable > 0) {

        var htmlString = $('#DefaultWorkPatternBody').find("#TrueIsRotatingDiv").find(".RotatingTable:last").parent().parent().clone();

        $('#DefaultWorkPatternBody').find("#TrueIsRotatingDiv").find(".RotatingTable:last").parent().parent().after(htmlString);
        var newWeek = parseInt(isAvailable) + 1;
        $('#DefaultWorkPatternBody').find(".LastIndex:last").html('Week ' + newWeek);
        $('#DefaultWorkPatternBody').find(".LastIndex:last").attr('data-name', newWeek);


    }
    else {
        //var workPatternId = $('#DefaultWorkPatternBody').find("#WorkPattenId").val();
        var workPatternId = parseInt($(".LastIndex:last").attr('data-name')) + 1;
        if (workPatternId.toString() == "NaN") {
            workPatternId = 0;
        }
        $.ajax({
            url: constantProjectPlanner.TrueIsRotating,
            data: { workPatternId: workPatternId },
            success: function (data) {
                $('#DefaultWorkPatternBody').find("#FalseIsRotatingDiv").hide();
                $('#DefaultWorkPatternBody').find('#workPatternDiv').html('');
                $('#DefaultWorkPatternBody').find('#workPatternDiv').append(data);
            }
        });
    }

});

$('#DefaultWorkPatternBody').on('click', '.deleteTable', function () {

    $(this).parent().parent().remove();
});

//Validation
$("#page_content").on('keyup', '#txt_Name', function () {
    $("#page_content").find("#DefaultWorkPatternBody").find("#lbl-error-WorkpattenName").hide();
});
$("#page_content").on('click', '#btn-submit-DefaultWorkPattern', function (e) {
    debugger;
    $(".hrtoolLoader").show();
    var IsError = false;
    var Id = $("#page_content").find('#DefaultWorkPatternBody').find("#WorkPattenId").val();
    var Name = $("#page_content").find('#DefaultWorkPatternBody').find("#txt_Name").val();
    if (Name == "") {
        IsError = true;
        $("#page_content").find("#DefaultWorkPatternBody").find("#lbl-error-WorkpattenName").show();
    }
    if (IsError) {

        $(".hrtoolLoader").hide();
        $(".modal-backdrop").hide();
        return false;
    }
    else {
        var allowRoatating = $('#DefaultWorkPatternBody').find('#AllowRotating').prop('checked');
        if (!allowRoatating) {
            var model = {
                Id: Id,
                Name: Name,
                IsRotating: allowRoatating,
                MondayHours: $('#DefaultWorkPatternBody').find("#FalseIsRotatingDiv").find("#MondayHours").val().trim(),
                MondayDays: $('#DefaultWorkPatternBody').find("#FalseIsRotatingDiv").find("#MondayDays").val().trim(),
                MondayStart: $('#DefaultWorkPatternBody').find("#FalseIsRotatingDiv").find("#MondayStart").val().trim(),
                MondayEnd: $('#DefaultWorkPatternBody').find("#FalseIsRotatingDiv").find("#MondayEnd").val().trim(),
                MondayBreakMins: $('#DefaultWorkPatternBody').find("#FalseIsRotatingDiv").find("#MondayBreakMins").val().trim(),
                TuesdayHours: $('#DefaultWorkPatternBody').find("#FalseIsRotatingDiv").find("#TuesdayHours").val().trim(),
                TuesdayDays: $('#DefaultWorkPatternBody').find("#FalseIsRotatingDiv").find("#TuesdayDays").val().trim(),
                TuesdayStart: $('#DefaultWorkPatternBody').find("#FalseIsRotatingDiv").find("#TuesdayStart").val().trim(),
                TuesdayEnd: $('#DefaultWorkPatternBody').find("#FalseIsRotatingDiv").find("#TuesdayEnd").val().trim(),
                TuesdayBreakMins: $('#DefaultWorkPatternBody').find("#FalseIsRotatingDiv").find("#TuesdayBreakMins").val().trim(),
                WednessdayHours: $('#DefaultWorkPatternBody').find("#FalseIsRotatingDiv").find("#WednessdayHours").val().trim(),
                WednessdayDays: $('#DefaultWorkPatternBody').find("#FalseIsRotatingDiv").find("#WednessdayDays").val().trim(),
                WednessdayStart: $('#DefaultWorkPatternBody').find("#FalseIsRotatingDiv").find("#WednessdayStart").val().trim(),
                WednessdayEnd: $('#DefaultWorkPatternBody').find("#FalseIsRotatingDiv").find("#WednessdayEnd").val().trim(),
                WednessdayBreakMins: $('#DefaultWorkPatternBody').find("#FalseIsRotatingDiv").find("#WednessdayBreakMins").val().trim(),
                ThursdayHours: $('#DefaultWorkPatternBody').find("#FalseIsRotatingDiv").find("#ThursdayHours").val().trim(),
                ThursdayDays: $('#DefaultWorkPatternBody').find("#FalseIsRotatingDiv").find("#ThursdayDays").val().trim(),
                ThursdayStart: $('#DefaultWorkPatternBody').find("#FalseIsRotatingDiv").find("#ThursdayStart").val().trim(),
                ThursdayEnd: $('#DefaultWorkPatternBody').find("#FalseIsRotatingDiv").find("#ThursdayEnd").val().trim(),
                ThursdayBreakMins: $('#DefaultWorkPatternBody').find("#FalseIsRotatingDiv").find("#ThursdayBreakMins").val().trim(),
                FridayHours: $('#DefaultWorkPatternBody').find("#FalseIsRotatingDiv").find("#FridayHours").val().trim(),
                FridayDays: $('#DefaultWorkPatternBody').find("#FalseIsRotatingDiv").find("#FridayDays").val().trim(),
                FridayStart: $('#DefaultWorkPatternBody').find("#FalseIsRotatingDiv").find("#FridayStart").val().trim(),
                FridayEnd: $('#DefaultWorkPatternBody').find("#FalseIsRotatingDiv").find("#FridayEnd").val().trim(),
                FridayBreakMins: $('#DefaultWorkPatternBody').find("#FalseIsRotatingDiv").find("#FridayBreakMins").val().trim(),
                SaturdayHours: $('#DefaultWorkPatternBody').find("#FalseIsRotatingDiv").find("#SaturdayHours").val().trim(),
                SaturdayDays: $('#DefaultWorkPatternBody').find("#FalseIsRotatingDiv").find("#SaturdayDays").val().trim(),
                SaturdayStart: $('#DefaultWorkPatternBody').find("#FalseIsRotatingDiv").find("#SaturdayStart").val().trim(),
                SaturdayEnd: $('#DefaultWorkPatternBody').find("#FalseIsRotatingDiv").find("#SaturdayEnd").val().trim(),
                SaturdayBreakMins: $('#DefaultWorkPatternBody').find("#FalseIsRotatingDiv").find("#SaturdayBreakMins").val().trim(),
                SundayHours: $('#DefaultWorkPatternBody').find("#FalseIsRotatingDiv").find("#SundayHours").val().trim(),
                SundayDays: $('#DefaultWorkPatternBody').find("#FalseIsRotatingDiv").find("#SundayDays").val().trim(),
                SundayStart: $('#DefaultWorkPatternBody').find("#FalseIsRotatingDiv").find("#SundayStart").val().trim(),
                SundayEnd: $('#DefaultWorkPatternBody').find("#FalseIsRotatingDiv").find("#SundayEnd").val().trim(),
                SundayBreakMins: $('#DefaultWorkPatternBody').find("#FalseIsRotatingDiv").find("#SundayBreakMins").val().trim(),
            }

            $.ajax({
                url: constantProjectPlanner.SaveFalseRoatingData,
                data: { moedl: JSON.stringify(model) },
                success: function (data) {
                    window.location.reload();
                }
            });
        }
        else {
            var tableData = [];
            $.each($('#DefaultWorkPatternBody').find("#TrueIsRotatingDiv").find(".RotatingTable"), function () {
                var model = {
                    MondayHours: $(this).find("#MondayHours").val().trim(),
                    MondayDays: $(this).find("#MondayDays").val().trim(),
                    MondayStart: $(this).find("#MondayStart").val().trim(),
                    MondayEnd: $(this).find("#MondayEnd").val().trim(),
                    MondayBreakMins: $(this).find("#MondayBreakMins").val().trim(),
                    TuesdayHours: $(this).find("#TuesdayHours").val().trim(),
                    TuesdayDays: $(this).find("#TuesdayDays").val().trim(),
                    TuesdayStart: $(this).find("#TuesdayStart").val().trim(),
                    TuesdayEnd: $(this).find("#TuesdayEnd").val().trim(),
                    TuesdayBreakMins: $(this).find("#TuesdayBreakMins").val().trim(),
                    WednessdayHours: $(this).find("#WednessdayHours").val().trim(),
                    WednessdayDays: $(this).find("#WednessdayDays").val().trim(),
                    WednessdayStart: $(this).find("#WednessdayStart").val().trim(),
                    WednessdayEnd: $(this).find("#WednessdayEnd").val().trim(),
                    WednessdayBreakMins: $(this).find("#WednessdayBreakMins").val().trim(),
                    ThursdayHours: $(this).find("#ThursdayHours").val().trim(),
                    ThursdayDays: $(this).find("#ThursdayDays").val().trim(),
                    ThursdayStart: $(this).find("#ThursdayStart").val().trim(),
                    ThursdayEnd: $(this).find("#ThursdayEnd").val().trim(),
                    ThursdayBreakMins: $(this).find("#ThursdayBreakMins").val().trim(),
                    FridayHours: $(this).find("#FridayHours").val().trim(),
                    FridayDays: $(this).find("#FridayDays").val().trim(),
                    FridayStart: $(this).find("#FridayStart").val().trim(),
                    FridayEnd: $(this).find("#FridayEnd").val().trim(),
                    FridayBreakMins: $(this).find("#FridayBreakMins").val().trim(),
                    SaturdayHours: $(this).find("#SaturdayHours").val().trim(),
                    SaturdayDays: $(this).find("#SaturdayDays").val().trim(),
                    SaturdayStart: $(this).find("#SaturdayStart").val().trim(),
                    SaturdayEnd: $(this).find("#SaturdayEnd").val().trim(),
                    SaturdayBreakMins: $(this).find("#SaturdayBreakMins").val().trim(),
                    SundayHours: $(this).find("#SundayHours").val().trim(),
                    SundayDays: $(this).find("#SundayDays").val().trim(),
                    SundayStart: $(this).find("#SundayStart").val().trim(),
                    SundayEnd: $(this).find("#SundayEnd").val().trim(),
                    SundayBreakMins: $(this).find("#SundayBreakMins").val().trim(),
                }
                tableData.push(model);
            });
            $.ajax({
                url: constantProjectPlanner.SaveTrueRoatingData,
                data: { Id: Id, Name: Name, IsRotating: allowRoatating, modelString: JSON.stringify(tableData) },
                type: 'post',
                success: function (data) {
                    window.location.reload();
                }
            });
        }
    }
});
$("#page_content").on('click', '#btn-submit-ReadOnlyWorkPattern', function (e) {

    $(".hrtoolLoader").show();
    var IsError = false;
    var Id = 0;
    var Name = $("#page_content").find('#ReadOnlyWorkPatternBody').find("#txt_Name").val();
    var WorkPatternId = $("#page_content").find('#ReadOnlyWorkPatternBody').find("#WorkPattenId").val();
    var EffectiveFrom = $("#page_content").find('#ReadOnlyWorkPatternBody').find("#txt_Effectivefromdate").val();
    var CurrentWeekWorkPatternDetailID = $("#page_content").find('#ReadOnlyWorkPatternBody').find('input[name="CurrentWeek"]:checked').val();
    if (CurrentWeekWorkPatternDetailID == undefined) {
        CurrentWeekWorkPatternDetailID = 0;
    }
    if (EffectiveFrom == "") {
        IsError = true;
        $("#page_content").find("#ReadOnlyWorkPatternBody").find("#lbl-error-Effectivefromdate").show();
    }
    if (IsError) {

        $(".hrtoolLoader").hide();
        $(".modal-backdrop").hide();
        return false;
    }
    else {
        var model = {
            Id: Id,
            EmployeeId: $("#currentEmployeeId").val(),
            WorkPatternId: WorkPatternId,
            EffectiveFrom: EffectiveFrom,
            CurrentWeekWorkPatternDetailID: CurrentWeekWorkPatternDetailID,
        }
        $.ajax({
            url: constantProjectPlanner.SaveEmployeeWorkPattern,
            type: 'POST',
            data: {
                Id: Id,
                EmployeeId: $("#currentEmployeeId").val(),
                WorkPatternId: WorkPatternId,
                EffectiveFrom: EffectiveFrom,
                CurrentWeekWorkPatternDetailID: CurrentWeekWorkPatternDetailID,
                HolidayCountryID: $("#drp-publicHolidayCountry").val()
            },
            success: function (data) {
                window.location.reload();
            }
        });
    }

});
//End 
//Public Holiday Country Start
$("#page_content").on("click", ".closeModel_Public", function () {
    var value = $("#page_content").find("#Employee_CountryID").val();
    $("#page_content").find("#drp-publicHolidayCountry").val(value);
});
$("#drp-publicHolidayCountry").change(function () {
    var value = $(this).val();
    if (value != "0") {
        $('#MeEmployeeProjectPlanner_Effactive_PublicholidayModal').modal('show');
        $.ajax({
            url: constantProjectPlanner.BindPublicHolidayTemplate,
            data: { CountryId: value },
            success: function (data) {
                $('#MeEmployeeProjectPlanner_Effactive_PublicholidayModal').find("#Effactive_PublicholidayModal").html("");
                $('#MeEmployeeProjectPlanner_Effactive_PublicholidayModal').find("#Effactive_PublicholidayModal").html(data);
                $('#MeEmployeeProjectPlanner_Effactive_PublicholidayModal').find("#Effactive_PublicholidayModal").find("#txt_EffectiveDate").Zebra_DatePicker({
                    direction: false,
                    showButtonPanel: false,
                    format: 'd-m-Y',
                    onSelect: function () {
                        $('#MeEmployeeProjectPlanner_Effactive_PublicholidayModal').find("#Effactive_PublicholidayModal").find("#lbl-error-EffectiveDate").hide();
                    }
                });              
            }
        });
    }
    else {
        return false;
    }
});
$("#btn-submit-MeEmployeeProjectPlanner-EffactiveDateSave").click(function () {
    $(".hrtoolLoader").show();
    $(".modal-backdrop").show();
    var effactiveDate = $('#MeEmployeeProjectPlanner_Effactive_PublicholidayModal').find("#Effactive_PublicholidayModal").find("#txt_EffectiveDate").val();
    var isError = false;
    if (effactiveDate == "") {
        isError = true;
        $('#EmployeeProjectPlanner_Effactive_PublicholidayModal').find("#Effactive_PublicholidayModal").find("#lbl-error-EffectiveDate").show();
    }
    var countryId = $('#MeEmployeeProjectPlanner_Effactive_PublicholidayModal').find("#Effactive_PublicholidayModal").find("#countryId").val();

    if (isError) {
        $(".hrtoolLoader").hide();
        $(".modal-backdrop").hide();
        return false;
    }
    else {
        $.ajax({
            url: constantProjectPlanner.SavePublicHolidayTemplate,
            data: { CountryId: countryId, EffactiveDate: effactiveDate, EmployeeId: $("#currentEmployeeId").val(), HolidayCountryID: $("#drp-publicHolidayCountry").val() },
            success: function (data) {

                if (data == "error") {
                    $(".hrtoolLoader").hide();
                    $(".modal-backdrop").hide();

                    $(".toast-warning").show();
                    $(".toast-warning").html("please select effective date grater then last effective date.");
                    setTimeout(function () { $(".toast-warning").hide(); }, 3000);
                }
                else {

                    var date = new Date();
                    var currentMonth = date.getMonth() + 1;
                    $("#page_content").find(".accordion_container").find("#fixedrightcolumn_" + currentMonth).html("");
                    $("#page_content").find(".accordion_container").find("#fixedrightcolumn_" + currentMonth).html(data);

                    $(".toast-success").show();
                    setTimeout(function () { $(".toast-success").hide(); }, 1500);
                    $("#page_content").find("#EmployeeProjectPlanner_Effactive_PublicholidayModal").find(".close").click();

                    $(".hrtoolLoader").hide();
                    $(".modal-backdrop").hide();
                    window.location.reload();
                }
            }
        });
    }
});
$(".accordion_container").on("click", ".btn_AddEdit_PublicHoliday", function () {
    var yearId = $(this).attr("data-yearid");
    var monthId = $(this).attr("data-monthid");
    var dayId = $(this).attr("data-dayid");
    var id = $(this).attr("data-id");
    $.ajax({
        url: constantProjectPlanner.addEdit_PublicHoliday,
        data: { Id: id, Date: dayId, Month: monthId, Year: yearId },
        success: function (data) {
            $(".accordion_container").find("#EmployeePlanner_PublicholidayModel").find("#EmployeePlanner_PublicHolidayBody").html("");
            $(".accordion_container").find("#EmployeePlanner_PublicholidayModel").find("#EmployeePlanner_PublicHolidayBody").html(data);
        }
    });
});
//public Holiday Country End
// Print Pdf
function showPrintOption() {

    $(".hrtoolLoader").show();
    var EmployeeId = $("#currentEmployeeId").val();
    $.ajax({
        url: constantProjectPlanner.AddAbsencePdfView,
        data: { EmployeeId: EmployeeId },
        success: function (data) {
            $("#page_content").find("#PrintPdfModel").find("#PrintPdfBody").html("");
            $("#page_content").find("#PrintPdfModel").find("#PrintPdfBody").html(data);

            $("#page_content").find("#PrintPdfModel").find("#PrintPdfBody").find("#txt_StartDate").Zebra_DatePicker({
                //direction: false,
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {

                    $("#page_content").find("#lbl-error-StartDate").hide();
                    constantPdf.StartDate = $(this).val().trim();

                }
            });

            $("#page_content").find("#PrintPdfModel").find("#PrintPdfBody").find("#txt_EndDate").Zebra_DatePicker({
                //direction: false,
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {

                    $("#page_content").find("#lbl-error-EndDate").hide();
                    $("#page_content").find("#lbl-error-GreaterEndDate").hide();
                    constantPdf.EndDate = $(this).val().trim();
                }
            });
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
        }
    });

};

$("#page_content").on('click', '#btn-submit-PrintPdf', function () {

    $(".hrtoolLoader").show();
    var IsError = false;
    var yearid = $("#page_content").find("#PrintPdfModel").find("#HiddenYearId").val();
    var monthid = $("#page_content").find("#PrintPdfModel").find("#HiddenMonthId").val();
    var dayid = $("#page_content").find("#PrintPdfModel").find("#HiddendayId").val();
    var StartDate = $("#page_content").find("#PrintPdfModel").find("#txt_StartDate").val();
    var EndDate = $("#page_content").find("#PrintPdfModel").find("#txt_EndDate").val();
    var CheckedText = $("#page_content").find("#PrintPdfModel").find('input[name=Absence]:checked').val();
    var EmployeeId = $("#currentEmployeeId").val();
    if (CheckedText == "Absences") {
        var Absence = true;
        var Holidays = false;
    }
    else {
        var Absence = false;
        var Holidays = true;
    }

    if (StartDateValidation(StartDate, EndDate)) {
        IsError = true;
        $("#page_content").find("#lbl-error-GreaterEndDate").show();
        $("#page_content").find("#txt_EndDate").val('');
    }
    if (StartDate == "") { IsError = true; $("#page_content").find("#PrintPdfModel").find("#lbl-error-StartDate").show(); }
    if (EndDate == "") { IsError = true; $("#page_content").find("#PrintPdfModel").find("#lbl-error-EndDate").show(); }
    if (IsError) {
        (".hrtoolLoader").hide();
        $(".modal-backdrop").hide();
        return false;
    }
    else {
        //var model = {
        //    EmployeeId: $("#currentEmployeeId").val(),
        //    yearId: yearid,
        //    monthId: monthid,
        //    day: dayid,
        //    StartDate: StartDate,
        //    EndDate: EndDate,
        //    Absence: Absence,
        //    Holidays: Holidays,
        //}

        window.location.href = constantProjectPlanner.PrintPdf + '?EmployeeId=' + EmployeeId + '&yearId=' + yearid + '&monthId=' + monthid + '&day=' + dayid + '&StartDate=' + StartDate + '&EndDate=' + EndDate + '&Absence=' + Absence + '&Holidays=' + Holidays;
        setTimeout(function () {

            $("#page_content").find("#PrintPdfModel").find('.close').click();
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
        }, 5000);


        //"@Url.Action("PrintPdfAbsenceLeaves", "EmployeePlanner", new { EmployeeId = Model.EmployeeId, yearId = Model.yearId, monthId = Model.monthId, day = Model.day, StartDate = Model.StartDate.Trim(), EndDate = Model.EndDate.Trim(), Absence = Model.Absence, Holidays=Model.Holidays})"
        //$.ajax({
        //    url: constantProjectPlanner.PrintPdf,
        //    type: 'POST',
        //    data: JSON.stringify(model),
        //    contentType: "application/json",
        //    success: function (data) {
        //        
        //        var newWin = window.open('', 'Print-Window');
        //        newWin.document.open();
        //        newWin.document.write('<html><body onload="window.print()">' + data + '</body></html>');
        //        newWin.document.close();
        //        //setTimeout(function () { newWin.close(); }, 10);
        //        //            //$("#page_content").find("#fixedrightcolumn_" + monthid).html("");
        //        //            //$("#page_content").find("#fixedrightcolumn_" + monthid).html(data);
        //        $("#page_content").find("#PrintPdfModel").find('.close').click();
        //        window.location.reload();
        //        $(".hrtoolLoader").hide();
        //        $(".modal-backdrop").hide();

        //    }
        //});
    }
});

//Validation
$("#page_content").on('change', 'input[name=Absence]:radio', function () {

    var value = $(this).val();
    if (value == "Absences") {
        constantPdf.Absence = true;
        constantPdf.Holidays = false;
    }
    else {
        constantPdf.Absence = false;
        constantPdf.Holidays = true;
    }

});

//Scheduling Start

$(".accordion_container").on("click", ".btn_AddEdit_Scheduling", function () {

    $(".hrtoolLoader").show();
    var yearId = $(this).attr("data-yearid");
    var monthId = $(this).attr("data-monthid");
    var dayId = $(this).attr("data-dayid");
    var id = $(this).attr("data-id");
    $.ajax({
        url: constantProjectPlanner.addEdit_Scheduling,
        data: { Id: id, Date: dayId, Month: monthId, Year: yearId },
        success: function (data) {
            $(".accordion_container").find("#EmployeeProjectPlanner_SchedulingModel").find("#EmployeeProjectPlanner_SchedulingBody").html("");
            $(".accordion_container").find("#EmployeeProjectPlanner_SchedulingModel").find("#EmployeeProjectPlanner_SchedulingBody").html(data);

            $(".accordion_container").find("#EmployeeProjectPlanner_SchedulingModel").find("#EmployeeProjectPlanner_SchedulingBody").find("#txt_StartDate").Zebra_DatePicker({
                //direction: false,
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {
                    $("#page_content").find("#lbl-error-GreaterEndDate").hide();
                    $("#page_content").find("#lbl-error-StartDate").hide();
                    var startdate = $(this).val();
                    var enddate = $("#page_content").find("#txt_EndDate").val();
                    calculateDateDiff(startdate, enddate);

                }
            });
            $(".accordion_container").find("#EmployeeProjectPlanner_SchedulingModel").find("#EmployeeProjectPlanner_SchedulingBody").find("#txt_EndDate").Zebra_DatePicker({
                //direction: false,
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {
                    $("#page_content").find("#lbl-error-GreaterEndDate").hide();
                    $("#page_content").find("#lbl-error-EndDate").hide();
                    var enddate = $(this).val();
                    var startdate = $("#page_content").find("#txt_StartDate").val();
                    calculateDateDiff(startdate, enddate);
                }
            });

            $(".accordion_container").find("#EmployeeProjectPlanner_SchedulingModel").find("#EmployeeProjectPlanner_SchedulingBody").find("#txt_LessThenStartDate").Zebra_DatePicker({
                //direction: false,
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {
                    $("#page_content").find("#lbl-error-LessThenStartDate").hide();

                }
            });

            if (id > 0) {
                var selectedRadio = $(".accordion_container").find("#EmployeeProjectPlanner_SchedulingModel").find("#EmployeeProjectPlanner_SchedulingBody").find('input[name=lessThanADay]:checked').attr("id");
                if (selectedRadio == "adayormore") {
                    $(".accordion_container").find("#EmployeeProjectPlanner_SchedulingModel").find("#EmployeeProjectPlanner_SchedulingBody").find("#div_ADayOrMore").show();
                    $(".accordion_container").find("#EmployeeProjectPlanner_SchedulingModel").find("#EmployeeProjectPlanner_SchedulingBody").find("#div_LessThenADay").hide();
                }
                else {
                    $(".accordion_container").find("#EmployeeProjectPlanner_SchedulingModel").find("#EmployeeProjectPlanner_SchedulingBody").find("#div_LessThenADay").show();
                    $(".accordion_container").find("#EmployeeProjectPlanner_SchedulingModel").find("#EmployeeProjectPlanner_SchedulingBody  ").find("#div_ADayOrMore").hide();
                }
            }

            spinner();

            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
        }
    });
});

$("#page_content").on("click", "#btn-submit-EmployeePlanner_Scheduling", function () {

    $(".hrtoolLoader").show();
    var IsError = false;
    var id = $(".accordion_container").find("#EmployeeProjectPlanner_SchedulingModel").find("#EmployeeProjectPlanner_SchedulingBody").find("#HiddenId").val();
    var yearid = $(".accordion_container").find("#EmployeeProjectPlanner_SchedulingModel").find("#EmployeeProjectPlanner_SchedulingBody").find("#HiddenYearId").val();
    var monthid = $(".accordion_container").find("#EmployeeProjectPlanner_SchedulingModel").find("#EmployeeProjectPlanner_SchedulingBody").find("#HiddenMonthId").val();
    var dayid = $(".accordion_container").find("#EmployeeProjectPlanner_SchedulingModel").find("#EmployeeProjectPlanner_SchedulingBody").find("#HiddendayId").val();
    var selectedRadio = $(".accordion_container").find("#EmployeeProjectPlanner_SchedulingModel").find("#EmployeeProjectPlanner_SchedulingBody").find('input[name=lessThanADay]:checked').attr("id");
    if (selectedRadio == "adayormore") {

        var startDate = $(".accordion_container").find("#EmployeeProjectPlanner_SchedulingModel").find("#EmployeeProjectPlanner_SchedulingBody").find("#txt_StartDate").val().trim();
        var endDate = $(".accordion_container").find("#EmployeeProjectPlanner_SchedulingModel").find("#EmployeeProjectPlanner_SchedulingBody").find("#txt_EndDate").val().trim();
        var duration = $(".accordion_container").find("#EmployeeProjectPlanner_SchedulingModel").find("#EmployeeProjectPlanner_SchedulingBody").find("#txt_DurationDays").val().trim();
        var isLessThenADay = false;
        var isDayorMore = true;

        if (startDate == "") { IsError = true; $(".accordion_container").find("#EmployeeProjectPlanner_SchedulingBody").find("#lbl-error-StartDate").show(); }
        if (endDate == "") { IsError = true; $(".accordion_container").find("#EmployeeProjectPlanner_SchedulingBody").find("#lbl-error-EndDate").show(); }
    }
    else {
        var startDate = $(".accordion_container").find("#EmployeeProjectPlanner_SchedulingModel").find("#EmployeeProjectPlanner_SchedulingBody").find("#txt_LessThenStartDate").val().trim();
        var InTimeHr = $(".accordion_container").find("#EmployeeProjectPlanner_SchedulingModel").find("#EmployeeProjectPlanner_SchedulingBody").find("#drp-HourseListSD").val();
        var InTimeMin = $(".accordion_container").find("#EmployeeProjectPlanner_SchedulingModel").find("#EmployeeProjectPlanner_SchedulingBody").find("#drp-MinutesListSD").val().trim();
        var EndTimeHr = $(".accordion_container").find("#EmployeeProjectPlanner_SchedulingModel").find("#EmployeeProjectPlanner_SchedulingBody").find("#drp-HourseListED").val().trim();
        var EndTimeMin = $(".accordion_container").find("#EmployeeProjectPlanner_SchedulingModel").find("#EmployeeProjectPlanner_SchedulingBody").find("#drp-MinutesListED").val().trim();
        var DurationHr = $(".accordion_container").find("#EmployeeProjectPlanner_SchedulingModel").find("#EmployeeProjectPlanner_SchedulingBody").find("#txt_DurationHours").val().trim();
        var isLessThenADay = true;
        var isDayorMore = false;

        if (startDate == "") { IsError = true; $(".accordion_container").find("#EmployeeProjectPlanner_SchedulingBody").find("#lbl-error-LessThenStartDate").show(); }
        //if (InTimeHr == "") { IsError = true; $(".accordion_container").find("#EmployeeProjectPlanner_SchedulingBody").find("#lbl-error-EndDate").show(); }
    }
    var comments = $(".accordion_container").find("#EmployeeProjectPlanner_SchedulingModel").find("#EmployeeProjectPlanner_SchedulingBody").find("#text_Comments").val().trim();

    var customer = $(".accordion_container").find("#EmployeeProjectPlanner_SchedulingModel").find("#EmployeeProjectPlanner_SchedulingBody").find("#selectID").val();
    var project = $(".accordion_container").find("#EmployeeProjectPlanner_SchedulingModel").find("#EmployeeProjectPlanner_SchedulingBody").find("#drp-Project").val();
    var asset = $(".accordion_container").find("#EmployeeProjectPlanner_SchedulingModel").find("#EmployeeProjectPlanner_SchedulingBody").find("#drp-Asset").val();
    if (customer == "0" || customer == "0") { IsError = true; $(".accordion_container").find("#EmployeeProjectPlanner_SchedulingBody").find("#lbl-error-Customer").show(); }
    //if (project == "0" || project == "0") { IsError = true; $(".accordion_container").find("#EmployeeProjectPlanner_SchedulingBody").find("#lbl-error-Project").show(); }
    if (asset == "0" || asset == "0") { IsError = true; $(".accordion_container").find("#EmployeeProjectPlanner_SchedulingBody").find("#lbl-error-Asset").show(); }
    if (IsError) {
        $(".hrtoolLoader").hide();
        $(".modal-backdrop").hide();
        return false;
    }
    else {
        var model = {
            Id: id,
            EmployeeId: $("#currentEmployeeId").val(),
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
            HolidayCountryID: $("#drp-publicHolidayCountry").val()
        }
        $.ajax({
            url: constantProjectPlanner.SaveData_Scheduling,
            type: 'POST',
            data: JSON.stringify(model),
            contentType: "application/json",
            success: function (data) {
                $(".accordion_container").find("#fixedrightcolumn_" + monthid).html("");
                $(".accordion_container").find("#fixedrightcolumn_" + monthid).html(data);

                $(".accordion_container").find("#EmployeeProjectPlanner_SchedulingModel").find('.close').click();

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

function SaveOtherLeave(id, isLessThenADay, reasonForLeaveId, startDate, endDate, duration, comments, yearid, monthid, dayid, hour, min, jsonString) {
    $(".hrtoolLoader").show();
    var model = {
        Id: id,
        EmployeeId: $("#currentEmployeeId").val(),
        IsLessThenADay: isLessThenADay,
        ReasonForLeaveId: reasonForLeaveId,
        StartDate: startDate,
        EndDate: endDate,
        Duration: duration,
        Comment: comments,
        yearId: yearid,
        monthId: monthid,
        day: dayid,
        jsonDocumentList: jsonString,
        Hour: hour,
        Min: min,
        HolidayCountryID: $("#drp-publicHolidayCountry").val()
    }

    $.ajax({
        url: constantProjectPlanner.SaveData_OtherLeave,
        type: 'POST',
        data: JSON.stringify(model),
        contentType: "application/json",
        success: function (data) {
            $(".accordion_container").find("#fixedrightcolumn_" + monthid).html("");
            $(".accordion_container").find("#fixedrightcolumn_" + monthid).html(data);

            $(".accordion_container").find("#EmployeePlanner_OtherLeaveModel").find('.close').click();

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

$(".accordion_container").on("change", ".Scheduling_radio", function () {

    var Id = $(this).attr("id");
    if (Id == "adayormore") {
        $(".accordion_container").find("#EmployeeProjectPlanner_SchedulingModel").find("#EmployeeProjectPlanner_SchedulingBody").find("#div_ADayOrMore").show();
        $(".accordion_container").find("#EmployeeProjectPlanner_SchedulingModel").find("#EmployeeProjectPlanner_SchedulingBody").find("#div_LessThenADay").hide();
    }
    else {
        $(".accordion_container").find("#EmployeeProjectPlanner_SchedulingModel").find("#EmployeeProjectPlanner_SchedulingBody").find("#div_LessThenADay").show();
        $(".accordion_container").find("#EmployeeProjectPlanner_SchedulingModel").find("#EmployeeProjectPlanner_SchedulingBody").find("#div_ADayOrMore").hide();
    }
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

function calculateDateDiff(stratDate, endDate) {
    if (stratDate != "" || endDate != "") {
        if (StartDateValidation(stratDate, endDate)) {
            $("#lbl-error-GreaterEndDate").show();
            $("#txt_EndDate").val('');
        }
        else {
            var days = DaysCount(stratDate, endDate);
            $("#page_content").find("#txt_DurationDays").val(days);
            $("#txt_Duration").val(days);
        }
    }
}

//drp-HourseListSD  drp-MinutesListSD txt_DurationHours
$("#page_content").on('change', '#drp-HourseListSD', function (e) {
    $("#lbl-error-validtimeSD").hide();
    $("#lbl-error-InTimeSD").hide();
    debugger;
    $("#page_content").find("#lbl-error-DurationHours").hide();
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
//drp-MinutesListSD
$("#page_content").on('change', '#drp-MinutesListSD', function (e) {
    $("#lbl-error-validtimeSD").hide();
    $("#lbl-error-InTimeSD").hide();
    $("#page_content").find("#lbl-error-DurationHours").hide();
    var St = $("#drp-HourseListSD").val();
    var Sm = $(this).val();
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
//drp-HourseListED   txt_DurationHours
$("#page_content").on('change', '#drp-HourseListED', function (e) {
    $("#lbl-error-validtimeSD").hide();
    $("#lbl-error-InTimeSD").hide();
    $("#page_content").find("#lbl-error-DurationHours").hide();
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
//drp-MinutesListED
$("#page_content").on('change', '#drp-MinutesListED', function (e) {
     $("#lbl-error-validtimeSD").hide();
    $("#lbl-error-InTimeSD").hide();
    $("#page_content").find("#lbl-error-DurationHours").hide();
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

//Validation

$("#page_content").on('change', '#drp-Customer', function (e) {
    $("#page_content").find(".accordion_container").find("#EmployeeProjectPlanner_SchedulingModel").find("#EmployeeProjectPlanner_SchedulingBody").find("#lbl-error-Customer").hide();
});
$("#page_content").on('change', '#drp-Project', function (e) {
    $("#page_content").find(".accordion_container").find("#EmployeeProjectPlanner_SchedulingModel").find("#EmployeeProjectPlanner_SchedulingBody").find("#lbl-error-Project").hide();
});
$("#page_content").on('change', '#drp-Asset', function (e) {
    $("#page_content").find(".accordion_container").find("#EmployeeProjectPlanner_SchedulingModel").find("#EmployeeProjectPlanner_SchedulingBody").find("#lbl-error-Asset").hide();
});

//Scheduling End

// Uplift Leave
function UpliftStepCallback(obj, context) {
    if (context.fromStep == 1) {
        var isError = false;
        var date = $(".accordion_container").find("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").find("#txt_Date").val().trim();
        var JobTitle = $(".accordion_container").find("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").find("#drpJobTitle").val().trim();
        var WorkerRatePer = $(".accordion_container").find("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").find("#txt_workerRatePer").val().trim();
        var WorkerRate = $(".accordion_container").find("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").find("#txt_workerRate").val().trim();
        var CustomerRatePer = $(".accordion_container").find("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").find("#txt_CustomerRatePer").val().trim();
        var CustomerRate = $(".accordion_container").find("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").find("#txt_CustomerRate").val().trim();
        var customer = $(".accordion_container").find("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").find("#selectID").val();
        var Project = $(".accordion_container").find("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").find("#drp-Project").val().trim();
        var Comments = $(".accordion_container").find("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").find("#text_Comments").val().trim();
        if (JobTitle == "0") {
            isError = true;
            $(".accordion_container").find("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").find("#ValidJobTitle").show();
        }
        if (date == "") {
            isError = true;
            $(".accordion_container").find("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").find("#lbl-error-Date").show();
        }
        $.each($(".accordion_container").find("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").find(".Uplift_Detail_Div"), function () {
            var inTimeHr = $(this).find("#drp-HourseListSDTimesheet").val().trim();
            var inTimeMin = $(this).find("#drp-MinutesListSDTimesheet").val().trim();
            if (inTimeHr == 0 && inTimeMin == 0) {
                isError = true;
                $(this).find("#lbl-error-InTime").show();
            }
            var endTimeHr = $(this).find("#drp-HourseListEDTimesheet").val().trim();
            var endTimeMin = $(this).find("#drp-MinutesListEDTimesheet").val().trim();
            if (inTimeHr == 0 && inTimeMin == 0) {
                isError = true;
                $(this).find("#lbl-error-EndTime").show();
            }

        });                
        if (isError) {
            return false;
        }
        else {
            $(".accordion_container").find("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").find('.buttonNext').hide();
            $(".accordion_container").find("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").find('.buttonPrevious').show();
            $(".accordion_container").find("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").find('.buttonFinish').show();
            return true;
        }
    }
    else {
        $(".accordion_container").find("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").find('.buttonNext').show();
        $(".accordion_container").find("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").find('.buttonPrevious').hide();
        $(".accordion_container").find("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").find('.buttonFinish').hide();
        return true;
    }
}

function UpliftonFinishCallback(obj, context) {
    var id = $(".accordion_container").find("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").find("#HiddenId").val();
    var yearid = $(".accordion_container").find("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").find("#HiddenYearId").val();
    var monthid = $(".accordion_container").find("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").find("#HiddenMonthId").val();
    var dayid = $(".accordion_container").find("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").find("#HiddendayId").val();
    var date = $(".accordion_container").find("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").find("#txt_Date").val().trim();
    var JobTitle = $(".accordion_container").find("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").find("#drpJobTitle").val().trim();
    //var WorkerRatePer = $(".accordion_container").find("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").find("#txt_workerRatePer").val().trim();
    //var WorkerRate = $(".accordion_container").find("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").find("#txt_workerRate").val().trim();
    //var CustomerRatePer = $(".accordion_container").find("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").find("#txt_CustomerRatePer").val().trim();
    //var CustomerRate = $(".accordion_container").find("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").find("#txt_CustomerRate").val().trim();
    var customer = $(".accordion_container").find("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").find("#selectID").val();
    var Project = $(".accordion_container").find("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").find("#drp-Project").val().trim();
    var Comments = $(".accordion_container").find("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").find("#text_Comments").val().trim();

    var DetailDiv = [];
    $.each($(".accordion_container").find("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").find(".Uplift_Detail_Div"), function () {
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
    $.each($(".accordion_container").find("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").find('#filesList').find(".ListData"), function () {
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

    var comment = $(".accordion_container").find("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").find("#text_Comments").val().trim();

    var model = {
        Id: id,
        EmployeeId: $("#currentEmployeeId").val(),
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
    }

    $.ajax({
        url: constantProjectPlanner.SaveData_Uplift,
        type: 'POST',
        data: JSON.stringify(model),
        contentType: "application/json",
        success: function (data) {
            $(".accordion_container").find("#fixedrightcolumn_" + monthid).html("");
            $(".accordion_container").find("#fixedrightcolumn_" + monthid).html(data);

            $(".accordion_container").find("#EmployeeProjectPlanner_UpliftModel").find('.close').click();

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

$(".accordion_container").on("click", ".btn_AddEdit_Uplift", function () {
    var yearId = $(this).attr("data-yearid");
    var monthId = $(this).attr("data-monthid");
    var dayId = $(this).attr("data-dayid");
    var id = $(this).attr("data-id");
    $.ajax({
        url: constantProjectPlanner.addEdit_Uplift,
        data: { Id: id, Date: dayId, Month: monthId, Year: yearId },
        success: function (data) {
            $(".accordion_container").find("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").html("");
            $(".accordion_container").find("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").html(data);

            $(".accordion_container").find("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").find('#wizard').smartWizard({
                onLeaveStep: UpliftStepCallback,
                onFinish: UpliftonFinishCallback
            });
            if (id <= 0) {
                $(".accordion_container").find("#btn_AddNew_UpliftDetail").click();
            }


            $(".accordion_container").find("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").find('.buttonNext').addClass('btn btn-warning');
            $(".accordion_container").find("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").find('.buttonPrevious').addClass('btn btn-warning');
            $(".accordion_container").find("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").find('.buttonFinish').addClass('btn btn-success');

            $(".accordion_container").find("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").find('.buttonPrevious').hide();
            $(".accordion_container").find("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").find('.buttonFinish').hide();

            $(".accordion_container").find("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").find("#txt_Date").Zebra_DatePicker({
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {
                    (".accordion_container").find("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").find("#lbl-error-Date").hide();
                }
            });

        }
    });
});

$(".accordion_container").on("click", "#btn_AddNew_UpliftDetail", function () {
    $.ajax({
        url: constantProjectPlanner.addEdit_Uplift_Detail,
        success: function (data) {
            var html = $(".accordion_container").find("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").find("#Uplift_detail").html();
            if (html == "") {
                $(".accordion_container").find("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").find("#Uplift_detail").html(data);

            }
            else {
                $(".accordion_container").find("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").find("#Uplift_detail").append(data);
            }
            spinner();
            $(".accordion_container").find('.Uplift_Detail_Div:first').find('.Uplift_delete_icon').parent().hide();
        }
    });
});

$(".accordion_container").on("click", ".file-deleteicon", function () {
    $(this).parent().remove();
});

$(".accordion_container").on("click", ".Uplift_delete_icon", function () {
    $(this).parent().parent().parent().parent().remove();
});

$(".accordion_container").on('change', '#UpliftFileToUpload', function (e) {
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
                    url: constantProjectPlanner.ImageData_Uplift,
                    contentType: false,
                    processData: false,
                    data: imageData,
                    success: function (result) {
                        var string = '<div class="row ListData"><div class="col-lg-1 icon"><i class="fa fa-paperclip"></i></div><div class="col-lg-10 attach-disc"><label class="fileName" data-newFileName="' + result.NewFileName + '">' + result.originalFileName + '</label><br /><input type="text" class="ImageDescription form-control" placeholder="Add Description Here"/></div><div class="col-lg-1 file-deleteicon Uplift_delete_icon"><i class="fa fa-trash-o"></i></div></div>';
                        var isEmpty = $(".accordion_container").find("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").find("#filesList").html();
                        if (isEmpty = "") {
                            $(".accordion_container").find("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").find("#filesList").html(string);
                        }
                        else {
                            $(".accordion_container").find("#EmployeeProjectPlanner_UpliftModel").find("#EmployeeProjectPlanner_UpliftBody").find("#filesList").append(string);
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
//Uplift end