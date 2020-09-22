$(document).ready(function () {
    accordation();
    $(".accordion_container").find("#accordion_row_" + constantPlanner.currentMonth).click();
    var positionTop = $(".accordion_container").find("#accordion_table_" + constantPlanner.currentMonth).offset().top;
    $('html, body').animate({ scrollTop: positionTop - 110 }, 'slow');
});
var bol = false;
function minmax(value, min, max) {
    if (parseInt(value) < min || isNaN(parseInt(value)))
        return 0;
    else if (parseInt(value) > max) {
        toastr.warning('Please Enter valid Time');
        return 0;
    }
    else return value;
}
function getEmployeeWorkPatternExist()
{
    alert("Please Select Employee WorkPattern");
}
function DataTableDesign() {
    var table = $('#WorkPatternHistoryTable').DataTable({
        "sDom": '<"top"i>rt<"bottom"flp><"clear">',
        "bPaginate": false
    });
};

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
        url: constantPlanner.ListOfMonth,
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

function calculateAnnualDateDiff() {
    var stratDate = $(".accordion_container").find("#MeEmployeePlanner_AnnualLeaveModel").find("#MeEmployeePlanner_AnnualLeaveBody").find("#txt_StartDate").val();
    var endDate = $(".accordion_container").find("#MeEmployeePlanner_AnnualLeaveModel").find("#MeEmployeePlanner_AnnualLeaveBody").find("#txt_EndDate").val();
    if (stratDate != "" && endDate != "") {
        var splitStartDate = stratDate.split('-');
        var splitEndDate = endDate.split('-');
        var oneDay = 24 * 60 * 60 * 1000;
        var firstDate = new Date(splitStartDate[2], splitStartDate[1], splitStartDate[0]);
        var secondDate = new Date(splitEndDate[2], splitEndDate[1], splitEndDate[0]);
        var diffDays = Math.round(Math.abs((firstDate.getTime() - secondDate.getTime()) / (oneDay)));
        diffDays = diffDays + 1;
        $(".accordion_container").find("#MeEmployeePlanner_AnnualLeaveModel").find("#MeEmployeePlanner_AnnualLeaveBody").find("#txt_Duration").val(diffDays);
    }
}

$(".accordion_container").on("click", ".btn_AddEdit_AnnualLeave", function () {
    var yearId = $(this).attr("data-yearid");
    var monthId = $(this).attr("data-monthid");
    var dayId = $(this).attr("data-dayid");
    var id = $(this).attr("data-id");
    $.ajax({
        url: constantPlanner.addEdit_AnnualLeave,
        data: { Id: id, Date: dayId, Month: monthId, Year: yearId },
        success: function (data) {
            $(".accordion_container").find("#MeEmployeePlanner_AnnualLeaveModel").find("#MeEmployeePlanner_AnnualLeaveBody").html("");
            $(".accordion_container").find("#MeEmployeePlanner_AnnualLeaveModel").find("#MeEmployeePlanner_AnnualLeaveBody").html(data);

            $(".accordion_container").find("#MeEmployeePlanner_AnnualLeaveModel").find("#MeEmployeePlanner_AnnualLeaveBody").find("#txt_StartDate").Zebra_DatePicker({
                //direction: false,
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {

                    $("#page_content").find("#lbl-error-GreaterEndDate").hide();
                    var startdate = $(this).val();
                    var enddate = $("#page_content").find("#txt_EndDate").val();
                    calculateAnnualDateDiff(startdate, enddate);
                }
            });
            $(".accordion_container").find("#MeEmployeePlanner_AnnualLeaveModel").find("#MeEmployeePlanner_AnnualLeaveBody").find("#txt_EndDate").Zebra_DatePicker({
                //direction: false,
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {

                    $("#page_content").find("#lbl-error-GreaterEndDate").hide();
                    $("#page_content").find("#lbl-error-EndDate").hide();
                    var enddate = $(this).val();
                    var startdate = $("#page_content").find("#txt_StartDate").val();
                    calculateAnnualDateDiff(startdate, enddate);
                }
            });

            $(".accordion_container").find("#MeEmployeePlanner_AnnualLeaveModel").find("#MeEmployeePlanner_AnnualLeaveBody").find("#txt_LessThenStartDate").Zebra_DatePicker({
                //direction: false,
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {

                }
            });

            if (id > 0) {
                var selectedRadio = $(".accordion_container").find("#MeEmployeePlanner_AnnualLeaveModel").find("#MeEmployeePlanner_AnnualLeaveBody").find('input[name=lessThanADay]:checked').attr("id");
                if (selectedRadio == "adayormore") {
                    $(".accordion_container").find("#MeEmployeePlanner_AnnualLeaveModel").find("#MeEmployeePlanner_AnnualLeaveBody").find("#div_ADayOrMore").show();
                    $(".accordion_container").find("#MeEmployeePlanner_AnnualLeaveModel").find("#MeEmployeePlanner_AnnualLeaveBody").find("#div_LessThenADay").hide();
                }
                else {
                    $(".accordion_container").find("#MeEmployeePlanner_AnnualLeaveModel").find("#MeEmployeePlanner_AnnualLeaveBody").find("#div_LessThenADay").show();
                    $(".accordion_container").find("#MeEmployeePlanner_AnnualLeaveModel").find("#MeEmployeePlanner_AnnualLeaveBody").find("#div_ADayOrMore").hide();
                }
            }

        }
    });
});

$(".accordion_container").on("change", ".Annual_leave_radio", function () {
    var Id = $(this).attr("id");
    if (Id == "adayormore") {
        $(".accordion_container").find("#MeEmployeePlanner_AnnualLeaveModel").find("#MeEmployeePlanner_AnnualLeaveBody").find("#div_ADayOrMore").show();
        $(".accordion_container").find("#MeEmployeePlanner_AnnualLeaveModel").find("#MeEmployeePlanner_AnnualLeaveBody").find("#div_LessThenADay").hide();
    }
    else {
        $(".accordion_container").find("#MeEmployeePlanner_AnnualLeaveModel").find("#MeEmployeePlanner_AnnualLeaveBody").find("#div_LessThenADay").show();
        $(".accordion_container").find("#MeEmployeePlanner_AnnualLeaveModel").find("#MeEmployeePlanner_AnnualLeaveBody").find("#div_ADayOrMore").hide();
    }
});

$(".accordion_container").on("click", "#btn-submit-MeEmployeePlanner_AnnualLeave", function () {
    var isError = false;
    var id = $(".accordion_container").find("#MeEmployeePlanner_AnnualLeaveModel").find("#MeEmployeePlanner_AnnualLeaveBody").find("#HiddenId").val();
    var yearid = $(".accordion_container").find("#MeEmployeePlanner_AnnualLeaveModel").find("#MeEmployeePlanner_AnnualLeaveBody").find("#HiddenYearId").val();
    var monthid = $(".accordion_container").find("#MeEmployeePlanner_AnnualLeaveModel").find("#MeEmployeePlanner_AnnualLeaveBody").find("#HiddenMonthId").val();
    var dayid = $(".accordion_container").find("#MeEmployeePlanner_AnnualLeaveModel").find("#MeEmployeePlanner_AnnualLeaveBody").find("#HiddendayId").val();
    var selectedRadio = $(".accordion_container").find("#MeEmployeePlanner_AnnualLeaveModel").find("#MeEmployeePlanner_AnnualLeaveBody").find('input[name=lessThanADay]:checked').attr("id");
    if (selectedRadio == "adayormore") {
        var toil = $(".accordion_container").find("#MeEmployeePlanner_AnnualLeaveModel").find("#MeEmployeePlanner_AnnualLeaveBody").find("#chk_TOIL").prop('checked');
        var startDate = $(".accordion_container").find("#MeEmployeePlanner_AnnualLeaveModel").find("#MeEmployeePlanner_AnnualLeaveBody").find("#txt_StartDate").val().trim();
        var endDate = $(".accordion_container").find("#MeEmployeePlanner_AnnualLeaveModel").find("#MeEmployeePlanner_AnnualLeaveBody").find("#txt_EndDate").val().trim();
        var comments = $(".accordion_container").find("#MeEmployeePlanner_AnnualLeaveModel").find("#MeEmployeePlanner_AnnualLeaveBody").find("#text_Comments").val().trim();
        var duration = $(".accordion_container").find("#MeEmployeePlanner_AnnualLeaveModel").find("#MeEmployeePlanner_AnnualLeaveBody").find("#txt_Duration").val().trim();
        var isLessThenADay = false;
        if (startDate == "") { isError = true; $(".accordion_container").find("#MeEmployeePlanner_AnnualLeaveModel").find("#MeEmployeePlanner_AnnualLeaveBody").find("#lbl-error-StartDate").show(); }
        if (endDate == "") { isError = true; $(".accordion_container").find("#MeEmployeePlanner_AnnualLeaveModel").find("#MeEmployeePlanner_AnnualLeaveBody").find("#lbl-error-EndDate").show(); }
        if (comments == "") { isError = true; $(".accordion_container").find("#MeEmployeePlanner_AnnualLeaveModel").find("#MeEmployeePlanner_AnnualLeaveBody").find("#lbl-error-Comments").show(); }
        if (isError) {
            return false;
        }
        else {
            SaveAnnualLeave(id, isLessThenADay, startDate, endDate, duration, comments, toil, yearid, monthid, dayid);
        }
    }
    else {
        var toil = $(".accordion_container").find("#MeEmployeePlanner_AnnualLeaveModel").find("#MeEmployeePlanner_AnnualLeaveBody").find("#chk_TOIL").prop('checked');
        var startDate = $(".accordion_container").find("#MeEmployeePlanner_AnnualLeaveModel").find("#MeEmployeePlanner_AnnualLeaveBody").find("#txt_LessThenStartDate").val().trim();
        var duration = '0.5';
        var isLessThenADay = true;
        if (startDate == "") { isError = true; $(".accordion_container").find("#MeEmployeePlanner_AnnualLeaveModel").find("#MeEmployeePlanner_AnnualLeaveBody").find("#lbl-error-LessThenStartDate").show(); }
        if (isError) {
            return false;
        }
        else {
            SaveAnnualLeave(id, isLessThenADay, startDate, "", duration, "", toil, yearid, monthid, dayid);
        }
    }

});

function SaveAnnualLeave(id, isLessThenADay, startDate, endDate, duration, comments, toil, yearid, monthid, dayid) {
    $(".hrtoolLoader").show();
    var model = {
        Id: id,
        EmployeeId: $("#currentEmployeeId").val(),
        IsLessThenADay: isLessThenADay,
        StartDate: startDate,
        EndDate: endDate,
        Duration: duration,
        Comment: comments,
        TOIL: toil,
        yearId: yearid,
        monthId: monthid,
        day: dayid,
        HolidayCountry: $("#drp-publicHolidayCountry").val()
    }

    $.ajax({
        url: constantPlanner.SaveData_AnnualLeave,
        type: 'POST',
        data: JSON.stringify(model),
        contentType: "application/json",
        success: function (data) {
            $(".accordion_container").find("#fixedrightcolumn_" + monthid).html("");
            $(".accordion_container").find("#fixedrightcolumn_" + monthid).html(data);

            $(".accordion_container").find("#MeEmployeePlanner_AnnualLeaveModel").find('.close').click();

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

$(".accordion_container").on("click", "#btn-submit-MeEmployeePlanner_LateLeave", function () {
    var isError = false;
    var id = $(".accordion_container").find("#MeEmployeePlanner_LateLeaveModel").find("#MeEmployeePlanner_LateLeaveBody").find("#HiddenId").val();
    var yearid = $(".accordion_container").find("#MeEmployeePlanner_LateLeaveModel").find("#MeEmployeePlanner_LateLeaveBody").find("#HiddenYearId").val();
    var monthid = $(".accordion_container").find("#MeEmployeePlanner_LateLeaveModel").find("#MeEmployeePlanner_LateLeaveBody").find("#HiddenMonthId").val();
    var dayid = $(".accordion_container").find("#MeEmployeePlanner_LateLeaveModel").find("#MeEmployeePlanner_LateLeaveBody").find("#HiddendayId").val();
    var lateDate = $(".accordion_container").find("#MeEmployeePlanner_LateLeaveModel").find("#MeEmployeePlanner_LateLeaveBody").find("#txt_LateDate").val().trim();
    var lateHr = $(".accordion_container").find("#MeEmployeePlanner_LateLeaveModel").find("#MeEmployeePlanner_LateLeaveBody").find("#drp-HourseListLateLeave").val();
    var lateMin = $(".accordion_container").find("#MeEmployeePlanner_LateLeaveModel").find("#MeEmployeePlanner_LateLeaveBody").find("#drp-MinutesListSDLateLeave").val();
    var comments = $(".accordion_container").find("#MeEmployeePlanner_LateLeaveModel").find("#MeEmployeePlanner_LateLeaveBody").find("#text_Comments").val().trim();
    if (lateDate == "") { isError = true; $(".accordion_container").find("#MeEmployeePlanner_LateLeaveModel").find("#MeEmployeePlanner_LateLeaveBody").find("#lbl-error-LateDate").show(); }
    if (comments == "") { isError = true; $(".accordion_container").find("#MeEmployeePlanner_LateLeaveModel").find("#MeEmployeePlanner_LateLeaveBody").find("#lbl-error-Comments").show(); }
    if (lateHr == "" && lateMin == "") {
        isError = true;
        $(".accordion_container").find("#MeEmployeePlanner_LateLeaveModel").find("#MeEmployeePlanner_LateLeaveBody").find("#lbl-error-HowLate").show();
    }

    if (isError) {
        return false;
    }
    else {

        $(".hrtoolLoader").show();
        var model = {
            Id: id,
            EmployeeId: $("#currentEmployeeId").val(),
            LateDate: lateDate,
            LateHr: lateHr,
            LateMin: lateMin,
            Comment: comments,
            yearId: yearid,
            monthId: monthid,
            day: dayid,
            HolidayCountryID: $("#drp-publicHolidayCountry").val()
        }
        
        $.ajax({
            url: constantPlanner.SaveData_LateLeave,
            type: 'POST',
            data: JSON.stringify(model),
            contentType: "application/json",
            success: function (data) {
                $(".accordion_container").find("#fixedrightcolumn_" + monthid).html("");
                $(".accordion_container").find("#fixedrightcolumn_" + monthid).html(data);

                $(".accordion_container").find("#MeEmployeePlanner_LateLeaveModel").find('.close').click();

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

$(".accordion_container").on("click", ".btn_AddEdit_LateLeave", function () {
    var yearId = $(this).attr("data-yearid");
    var monthId = $(this).attr("data-monthid");
    var dayId = $(this).attr("data-dayid");
    var id = $(this).attr("data-id");
    $.ajax({
        url: constantPlanner.addEdit_LateLeave,
        data: { Id: id, Date: dayId, Month: monthId, Year: yearId },
        success: function (data) {
            $(".accordion_container").find("#MeEmployeePlanner_LateLeaveModel").find("#MeEmployeePlanner_LateLeaveBody").html("");
            $(".accordion_container").find("#MeEmployeePlanner_LateLeaveModel").find("#MeEmployeePlanner_LateLeaveBody").html(data);

            $(".accordion_container").find("#MeEmployeePlanner_LateLeaveModel").find("#MeEmployeePlanner_LateLeaveBody").find("#txt_LateDate").Zebra_DatePicker({
                //direction: false,
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () { }
            });
            spinner();
        }
    });
});

//Other Leave Start

$(".accordion_container").on("click", ".btn_AddEdit_OtherLeave", function () {
    var yearId = $(this).attr("data-yearid");
    var monthId = $(this).attr("data-monthid");
    var dayId = $(this).attr("data-dayid");
    var id = $(this).attr("data-id");
    $.ajax({
        url: constantPlanner.addEdit_OtherLeave,
        data: { Id: id, Date: dayId, Month: monthId, Year: yearId },
        success: function (data) {
            $(".accordion_container").find("#MeEmployeePlanner_OtherLeaveModel").find("#MeEmployeePlanner_OtherLeaveBody").html("");
            $(".accordion_container").find("#MeEmployeePlanner_OtherLeaveModel").find("#MeEmployeePlanner_OtherLeaveBody").html(data);

            $(".accordion_container").find("#MeEmployeePlanner_OtherLeaveModel").find("#MeEmployeePlanner_OtherLeaveBody").find('#wizard').smartWizard({
                onLeaveStep: OtherleaveStepCallback,
                onFinish: OtherLeaveonFinishCallback
            });

            $(".accordion_container").find("#MeEmployeePlanner_OtherLeaveModel").find("#MeEmployeePlanner_OtherLeaveBody").find('.buttonNext').addClass('btn btn-warning');
            $(".accordion_container").find("#MeEmployeePlanner_OtherLeaveModel").find("#MeEmployeePlanner_OtherLeaveBody").find('.buttonPrevious').addClass('btn btn-warning');
            $(".accordion_container").find("#MeEmployeePlanner_OtherLeaveModel").find("#MeEmployeePlanner_OtherLeaveBody").find('.buttonFinish').addClass('btn btn-success');

            $(".accordion_container").find("#MeEmployeePlanner_OtherLeaveModel").find("#MeEmployeePlanner_OtherLeaveBody").find('.buttonPrevious').hide();
            $(".accordion_container").find("#MeEmployeePlanner_OtherLeaveModel").find("#MeEmployeePlanner_OtherLeaveBody").find('.buttonFinish').hide();

            $(".accordion_container").find("#MeEmployeePlanner_OtherLeaveModel").find("#MeEmployeePlanner_OtherLeaveBody").find("#txt_StartDate").Zebra_DatePicker({
                //direction: false,
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {
                    $("#page_content").find("#lbl-error-GreaterEndDate").hide();
                    var startdate = $(this).val();
                    var enddate = $("#page_content").find("#txt_EndDate").val();
                    calculateAnnualDateDiff(startdate, enddate);
                    // calculateAnnualDateDiff();
                }
            });
            $(".accordion_container").find("#MeEmployeePlanner_OtherLeaveModel").find("#MeEmployeePlanner_OtherLeaveBody").find("#txt_EndDate").Zebra_DatePicker({
                //direction: false,
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {
                    $("#page_content").find("#lbl-error-GreaterEndDate").hide();
                    $("#page_content").find("#lbl-error-EndDate").hide();
                    var enddate = $(this).val();
                    var startdate = $("#page_content").find("#txt_StartDate").val();
                    calculateAnnualDateDiff(startdate, enddate);
                    //calculateAnnualDateDiff();
                }
            });

            $(".accordion_container").find("#MeEmployeePlanner_OtherLeaveModel").find("#MeEmployeePlanner_OtherLeaveBody").find("#txt_LessThenStartDate").Zebra_DatePicker({
                //direction: false,
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {

                }
            });

            if (id > 0) {
                var selectedRadio = $(".accordion_container").find("#MeEmployeePlanner_OtherLeaveModel").find("#MeEmployeePlanner_OtherLeaveBody").find('input[name=lessThanADay]:checked').attr("id");
                if (selectedRadio == "adayormore") {
                    $(".accordion_container").find("#MeEmployeePlanner_OtherLeaveModel").find("#MeEmployeePlanner_OtherLeaveBody").find("#div_ADayOrMore").show();
                    $(".accordion_container").find("#MeEmployeePlanner_OtherLeaveModel").find("#MeEmployeePlanner_OtherLeaveBody").find("#div_LessThenADay").hide();
                }
                else {
                    $(".accordion_container").find("#MeEmployeePlanner_OtherLeaveModel").find("#MeEmployeePlanner_OtherLeaveBody").find("#div_LessThenADay").show();
                    $(".accordion_container").find("#MeEmployeePlanner_OtherLeaveModel").find("#MeEmployeePlanner_OtherLeaveBody").find("#div_ADayOrMore").hide();
                }
            }

            spinner();

        }
    });
});

function calculateAnnualDateDiff(stratDate, endDate) {
    if (stratDate != "" || endDate != "") {
        if (StartDateValidation(stratDate, endDate)) {
            $("#page_content").find("#lbl-error-GreaterEndDate").show();
            $("#page_content").find("#txt_EndDate").val('');
        }
        else {
            var days = DaysCount(stratDate, endDate);
            $("#page_content").find("#txt_Duration").val(days);
        }
    }

    //var stratDate = $(".accordion_container").find("#MeEmployeePlanner_OtherLeaveModel").find("#MeEmployeePlanner_OtherLeaveBody").find("#txt_StartDate").val();
    //var endDate = $(".accordion_container").find("#MeEmployeePlanner_OtherLeaveModel").find("#MeEmployeePlanner_OtherLeaveBody").find("#txt_EndDate").val();
    //if (stratDate != "" && endDate != "") {
    //    var splitStartDate = stratDate.split('-');
    //    var splitEndDate = endDate.split('-');
    //    var oneDay = 24 * 60 * 60 * 1000;
    //    var firstDate = new Date(splitStartDate[2], splitStartDate[1], splitStartDate[0]);
    //    var secondDate = new Date(splitEndDate[2], splitEndDate[1], splitEndDate[0]);
    //    var diffDays = Math.round(Math.abs((firstDate.getTime() - secondDate.getTime()) / (oneDay)));
    //    diffDays = diffDays + 1;
    //    $(".accordion_container").find("#MeEmployeePlanner_OtherLeaveModel").find("#MeEmployeePlanner_OtherLeaveBody").find("#txt_Duration").val(diffDays);
    //}
}

$(".accordion_container").on("change", ".Other_leave_radio", function () {
    var Id = $(this).attr("id");
    if (Id == "adayormore") {
        $(".accordion_container").find("#MeEmployeePlanner_OtherLeaveModel").find("#MeEmployeePlanner_OtherLeaveBody").find("#div_ADayOrMore").show();
        $(".accordion_container").find("#MeEmployeePlanner_OtherLeaveModel").find("#MeEmployeePlanner_OtherLeaveBody").find("#div_LessThenADay").hide();
    }
    else {
        $(".accordion_container").find("#MeEmployeePlanner_OtherLeaveModel").find("#MeEmployeePlanner_OtherLeaveBody").find("#div_LessThenADay").show();
        $(".accordion_container").find("#MeEmployeePlanner_OtherLeaveModel").find("#MeEmployeePlanner_OtherLeaveBody").find("#div_ADayOrMore").hide();
    }
});

$(".accordion_container").on('click', '.file-deleteicon', function () {
    $(this).parent().remove();
});

$(".accordion_container").on('change', '#OtherLeaveFileToUpload', function (e) {
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
                    url: constantPlanner.ImageData_OtherLeave,
                    contentType: false,
                    processData: false,
                    data: imageData,
                    success: function (result) {
                        var string = '<div class="row ListData"><div class="col-lg-1 icon"><i class="fa fa-paperclip"></i></div><div class="col-lg-10 attach-disc"><label class="fileName" data-newFileName="' + result.NewFileName + '">' + result.originalFileName + '</label><br /><input type="text" class="ImageDescription form-control" placeholder="Add Description Here"/></div><div class="col-lg-1 file-deleteicon"><i class="fa fa-trash-o"></i></div></div>';
                        var isEmpty = $(".accordion_container").find("#MeEmployeePlanner_OtherLeaveModel").find("#MeEmployeePlanner_OtherLeaveBody").find("#filesList").html();
                        if (isEmpty = "") {
                            $(".accordion_container").find("#MeEmployeePlanner_OtherLeaveModel").find("#MeEmployeePlanner_OtherLeaveBody").find("#filesList").html(string);
                        }
                        else {
                            $(".accordion_container").find("#MeEmployeePlanner_OtherLeaveModel").find("#MeEmployeePlanner_OtherLeaveBody").find("#filesList").append(string);
                        }
                        $(".hrtoolLoader").hide();
                        $(".modal-backdrop").hide();
                    }
                });
            }, 500);
        }
    }
});

function OtherleaveStepCallback(obj, context) {
    if (context.fromStep == 1) {
        var isError = false;
        var selectedRadio = $(".accordion_container").find("#MeEmployeePlanner_OtherLeaveModel").find("#MeEmployeePlanner_OtherLeaveBody").find('input[name=lessThanADay]:checked').attr("id");
        if (selectedRadio == "adayormore") {
            var reasonForLeave = $(".accordion_container").find("#MeEmployeePlanner_OtherLeaveModel").find("#MeEmployeePlanner_OtherLeaveBody").find("#drp-ReasonForLeaveId").val();
            var startDate = $(".accordion_container").find("#MeEmployeePlanner_OtherLeaveModel").find("#MeEmployeePlanner_OtherLeaveBody").find("#txt_StartDate").val().trim();
            var endDate = $(".accordion_container").find("#MeEmployeePlanner_OtherLeaveModel").find("#MeEmployeePlanner_OtherLeaveBody").find("#txt_EndDate").val().trim();
            var comments = $(".accordion_container").find("#MeEmployeePlanner_OtherLeaveModel").find("#MeEmployeePlanner_OtherLeaveBody").find("#text_Comments").val().trim();
            var duration = $(".accordion_container").find("#MeEmployeePlanner_OtherLeaveModel").find("#MeEmployeePlanner_OtherLeaveBody").find("#txt_Duration").val().trim();
            var isLessThenADay = false;
            if (reasonForLeave == "0") { isError = true; $(".accordion_container").find("#MeEmployeePlanner_OtherLeaveModel").find("#MeEmployeePlanner_OtherLeaveBody").find("#lbl-error-ReasonForLeave").show(); }
            if (startDate == "") { isError = true; $(".accordion_container").find("#MeEmployeePlanner_OtherLeaveModel").find("#MeEmployeePlanner_OtherLeaveBody").find("#lbl-error-StartDate").show(); }
            if (endDate == "") { isError = true; $(".accordion_container").find("#MeEmployeePlanner_OtherLeaveModel").find("#MeEmployeePlanner_OtherLeaveBody").find("#lbl-error-EndDate").show(); }
            if (comments == "") { isError = true; $(".accordion_container").find("#MeEmployeePlanner_OtherLeaveModel").find("#MeEmployeePlanner_OtherLeaveBody").find("#lbl-error-Comments").show(); }
            if (isError) {
                return false;
            }
            else {
                $(".accordion_container").find("#MeEmployeePlanner_OtherLeaveModel").find("#MeEmployeePlanner_OtherLeaveBody").find('.buttonNext').hide();
                $(".accordion_container").find("#MeEmployeePlanner_OtherLeaveModel").find("#MeEmployeePlanner_OtherLeaveBody").find('.buttonPrevious').show();
                $(".accordion_container").find("#MeEmployeePlanner_OtherLeaveModel").find("#MeEmployeePlanner_OtherLeaveBody").find('.buttonFinish').show();
                return true;
            }
        }
        else {
            var reasonForLeave = $(".accordion_container").find("#MeEmployeePlanner_OtherLeaveModel").find("#MeEmployeePlanner_OtherLeaveBody").find("#drp-ReasonForLeaveId").val();
            var startDate = $(".accordion_container").find("#MeEmployeePlanner_OtherLeaveModel").find("#MeEmployeePlanner_OtherLeaveBody").find("#txt_LessThenStartDate").val().trim();
            var isLessThenADay = true;
            var comments = $(".accordion_container").find("#MeEmployeePlanner_OtherLeaveModel").find("#MeEmployeePlanner_OtherLeaveBody").find("#text_Comments").val().trim();
            if (reasonForLeave == "0") { isError = true; $(".accordion_container").find("#MeEmployeePlanner_OtherLeaveModel").find("#MeEmployeePlanner_OtherLeaveBody").find("#lbl-error-ReasonForLeave").show(); }
            if (startDate == "") { isError = true; $(".accordion_container").find("#MeEmployeePlanner_OtherLeaveModel").find("#MeEmployeePlanner_OtherLeaveBody").find("#lbl-error-LessThenStartDate").show(); }
            if (comments == "") { isError = true; $(".accordion_container").find("#MeEmployeePlanner_OtherLeaveModel").find("#MeEmployeePlanner_OtherLeaveBody").find("#lbl-error-Comments").show(); }
            if (isError) {
                return false;
            }
            else {
                $(".accordion_container").find("#MeEmployeePlanner_OtherLeaveModel").find("#MeEmployeePlanner_OtherLeaveBody").find('.buttonNext').hide();
                $(".accordion_container").find("#MeEmployeePlanner_OtherLeaveModel").find("#MeEmployeePlanner_OtherLeaveBody").find('.buttonPrevious').show();
                $(".accordion_container").find("#MeEmployeePlanner_OtherLeaveModel").find("#MeEmployeePlanner_OtherLeaveBody").find('.buttonFinish').show();
                return true;
            }
        }

    }
    else {
        $(".accordion_container").find("#MeEmployeePlanner_OtherLeaveModel").find("#MeEmployeePlanner_OtherLeaveBody").find('.buttonNext').show();
        $(".accordion_container").find("#MeEmployeePlanner_OtherLeaveModel").find("#MeEmployeePlanner_OtherLeaveBody").find('.buttonPrevious').hide();
        $(".accordion_container").find("#MeEmployeePlanner_OtherLeaveModel").find("#MeEmployeePlanner_OtherLeaveBody").find('.buttonFinish').hide();
        return true;
    }
}

function OtherLeaveonFinishCallback(obj, context) {
    var id = $(".accordion_container").find("#MeEmployeePlanner_OtherLeaveModel").find("#MeEmployeePlanner_OtherLeaveBody").find("#HiddenId").val();
    var yearid = $(".accordion_container").find("#MeEmployeePlanner_OtherLeaveModel").find("#MeEmployeePlanner_OtherLeaveBody").find("#HiddenYearId").val();
    var monthid = $(".accordion_container").find("#MeEmployeePlanner_OtherLeaveModel").find("#MeEmployeePlanner_OtherLeaveBody").find("#HiddenMonthId").val();
    var dayid = $(".accordion_container").find("#MeEmployeePlanner_OtherLeaveModel").find("#MeEmployeePlanner_OtherLeaveBody").find("#HiddendayId").val();
    var selectedRadio = $(".accordion_container").find("#MeEmployeePlanner_OtherLeaveModel").find("#MeEmployeePlanner_OtherLeaveBody").find('input[name=lessThanADay]:checked').attr("id");

    var documentList = [];
    $.each($(".accordion_container").find("#MeEmployeePlanner_OtherLeaveModel").find("#MeEmployeePlanner_OtherLeaveBody").find('#filesList').find(".ListData"), function () {
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
        var reasonForLeave = $(".accordion_container").find("#MeEmployeePlanner_OtherLeaveModel").find("#MeEmployeePlanner_OtherLeaveBody").find("#drp-ReasonForLeaveId").val();
        var startDate = $(".accordion_container").find("#MeEmployeePlanner_OtherLeaveModel").find("#MeEmployeePlanner_OtherLeaveBody").find("#txt_StartDate").val().trim();
        var endDate = $(".accordion_container").find("#MeEmployeePlanner_OtherLeaveModel").find("#MeEmployeePlanner_OtherLeaveBody").find("#txt_EndDate").val().trim();
        var comments = $(".accordion_container").find("#MeEmployeePlanner_OtherLeaveModel").find("#MeEmployeePlanner_OtherLeaveBody").find("#text_Comments").val().trim();
        var duration = $(".accordion_container").find("#MeEmployeePlanner_OtherLeaveModel").find("#MeEmployeePlanner_OtherLeaveBody").find("#txt_Duration").val().trim();
        var isLessThenADay = false;
        SaveOtherLeave(id, isLessThenADay, reasonForLeave, startDate, endDate, duration, comments, yearid, monthid, dayid, 0, 0, JsondocumentListJoinString);
    }
    else {
        var reasonForLeave = $(".accordion_container").find("#MeEmployeePlanner_OtherLeaveModel").find("#MeEmployeePlanner_OtherLeaveBody").find("#drp-ReasonForLeaveId").val();
        var startDate = $(".accordion_container").find("#MeEmployeePlanner_OtherLeaveModel").find("#MeEmployeePlanner_OtherLeaveBody").find("#txt_LessThenStartDate").val().trim();
        var hour = $(".accordion_container").find("#MeEmployeePlanner_OtherLeaveModel").find("#MeEmployeePlanner_OtherLeaveBody").find("#drp-HourseListSDOtherLeave").val();
        var min = $(".accordion_container").find("#MeEmployeePlanner_OtherLeaveModel").find("#MeEmployeePlanner_OtherLeaveBody").find("#drp-MinutesListSDOtherLeave").val();
        var isLessThenADay = true;
        var comments = $(".accordion_container").find("#MeEmployeePlanner_OtherLeaveModel").find("#MeEmployeePlanner_OtherLeaveBody").find("#text_Comments").val().trim();
        SaveOtherLeave(id, isLessThenADay, reasonForLeave, startDate, "", 0, comments, yearid, monthid, dayid, hour, min, JsondocumentListJoinString);
    }
}

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
        url: constantPlanner.SaveData_OtherLeave,
        type: 'POST',
        data: JSON.stringify(model),
        contentType: "application/json",
        success: function (data) {
            $(".accordion_container").find("#fixedrightcolumn_" + monthid).html("");
            $(".accordion_container").find("#fixedrightcolumn_" + monthid).html(data);

            $(".accordion_container").find("#MeEmployeePlanner_OtherLeaveModel").find('.close').click();

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

//Other Leave End

// Travel leave Start

$("#page_content").on("change", "#drpCountry", function () {
    $("#drpState").val('');
    $("#drpTown").val('');
    var value = $(this).val();
    if (value != "0") {
        $.ajax({
            url: constantPlanner.BindState,
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
                    url: constantPlanner.BindAirport,
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
            url: constantPlanner.BindCity,
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
$("#page_content").on('change', '#drp-TravelHourseListSD', function (e) {
    $("#lbl-error-InTimeSD").hide();
    $("#lbl-error-DurationHours").hide();
    $("#lbl-error-validtimeSD").hide();
    var St = $(this).val();
    var Sm = $("#drp-MinutesTravelListSD").val();
    var Et = $("#drp-TravelHourseListED").val();
    var Em = $("#drp-TravelMinutesListED").val();
    var Diff = ((new Date("1991-1-1 " + Et + "" + ":" + "" + Em + "") - new Date("1991-1-1 " + St + "" + ":" + "" + Sm + "")) / 1000 / 60 / 60).toFixed(2);
    if (Diff <= 0) {
        $("#txt_DurationHours").val('');
        $("#lbl-error-validtimeSD").show();
    }
    else { $("#txt_DurationHours").val(Diff); }

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
    if (Diff <= 0) {
        $("#txt_DurationHours").val('');
        $("#lbl-error-validtimeSD").show();
    }
    else { $("#txt_DurationHours").val(Diff); }

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
    if (Diff <= 0) {
        $("#txt_DurationHours").val('');
        $("#lbl-error-validtimeSD").show();
    }
    else { $("#txt_DurationHours").val(Diff); }

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
    if (Diff <= 0) {
        $("#txt_DurationHours").val('');
        $("#lbl-error-validtimeSD").show();
    }
    else { $("#txt_DurationHours").val(Diff); }

});
$(".accordion_container").on("click", ".btn_AddEdit_TravelLeave", function () {

    var yearId = $(this).attr("data-yearid");
    var monthId = $(this).attr("data-monthid");
    var dayId = $(this).attr("data-dayid");
    var id = $(this).attr("data-id");
    $.ajax({
        url: constantPlanner.addEdit_TravelLeave,
        data: { Id: id, Date: dayId, Month: monthId, Year: yearId },
        success: function (data) {
            $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").html("");
            $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").html(data);

            $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find('#wizard').smartWizard({
                onLeaveStep: TravelleaveStepCallback,
                onFinish: TravelLeaveonFinishCallback
            });

            $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find('.buttonNext').addClass('btn btn-warning');
            $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find('.buttonPrevious').addClass('btn btn-warning');
            $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find('.buttonFinish').addClass('btn btn-success');

            $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find('.buttonPrevious').hide();
            $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find('.buttonFinish').hide();

            $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find("#txt_StartDate").Zebra_DatePicker({
                //direction: false,
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {
                    $("#page_content").find("#lbl-error-GreaterEndDate").hide();

                    var startdate = $(this).val();
                    var enddate = $("#page_content").find("#txt_EndDate").val();
                    calculateAnnualDateDiff(startdate, enddate);
                    //calculateAnnualDateDiffTravelLeave();

                }
            });
            $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find("#txt_EndDate").Zebra_DatePicker({
                //direction: false,
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {
                    $("#page_content").find("#lbl-error-GreaterEndDate").hide();
                    $("#page_content").find("#lbl-error-EndDate").hide();
                    var enddate = $(this).val();
                    var startdate = $("#page_content").find("#txt_StartDate").val();
                    calculateAnnualDateDiff(startdate, enddate);
                    //calculateAnnualDateDiffTravelLeave();
                }
            });

            $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find("#txt_LessThenStartDate").Zebra_DatePicker({
                //direction: false,
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {

                }
            });

            if (id > 0) {
                var selectedRadio = $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find('input[name=lessThanADay]:checked').attr("id");
                if (selectedRadio == "adayormore") {
                    $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find("#div_ADayOrMore").show();
                    $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find("#div_LessThenADay").hide();
                }
                else {
                    $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find("#div_LessThenADay").show();
                    $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody  ").find("#div_ADayOrMore").hide();
                }
            }

            spinner();

        }
    });
});

function calculateAnnualDateDiffTravelLeave() {
    var stratDate = $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find("#txt_StartDate").val();
    var endDate = $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find("#txt_EndDate").val();
    if (stratDate != "" && endDate != "") {
        var splitStartDate = stratDate.split('-');
        var splitEndDate = endDate.split('-');
        var oneDay = 24 * 60 * 60 * 1000;
        var firstDate = new Date(splitStartDate[2], splitStartDate[1], splitStartDate[0]);
        var secondDate = new Date(splitEndDate[2], splitEndDate[1], splitEndDate[0]);
        var diffDays = Math.round(Math.abs((firstDate.getTime() - secondDate.getTime()) / (oneDay)));
        diffDays = diffDays + 1;
        $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find("#txt_Duration").val(diffDays);
    }
}

$(".accordion_container").on("change", ".Travel_leave_radio", function () {
    var Id = $(this).attr("id");
    if (Id == "adayormore") {
        $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find("#div_ADayOrMore").show();
        $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find("#div_LessThenADay").hide();
    }
    else {
        $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find("#div_LessThenADay").show();
        $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find("#div_ADayOrMore").hide();
    }
});


$("#page_content").on("change", "#drp-ToCountryId", function () {
    $("#drp-ToStateId").val('');
    $("#drp-ToTownId").val('');
    var value = $(this).val();
    if (value != "0") {
        $.ajax({
            url: constantPlanner.BindState,
            data: { countryId: value },
            success: function (data) {

                $("#drp-ToStateId").html('');
                var toAppend = '';
                toAppend += "<option value='0'>--Select--</option>";
                $.each(data, function (index, item) {
                    toAppend += "<option value='" + item.Value + "'>" + item.Text + "</option>";
                })
                $("#drp-ToStateId").html(toAppend);
                if ($("#drp-ToStateId").val() == 0) {
                    $("#drp-ToStateId").val(0);

                }

                $.ajax({
                    url: constantPlanner.BindAirport,
                    data: { countryId: value },
                    success: function (data) {

                        $("#drp-ToAirportId").html('');
                        var toAppend = '';
                        toAppend += "<option value='0'>--Select--</option>";
                        $.each(data, function (index, item) {
                            toAppend += "<option value='" + item.Value + "'>" + item.Text + "</option>";
                        })
                        $("#drp-ToAirportId").html(toAppend);
                        if ($("#drp-ToAirportId").val() == 0) {
                            $("#drp-ToAirportId").val(0);

                        }
                    }
                });

            }
        });
    }
    else {
        $("#drp-ToStateId").empty();
        // Bind new values to dropdown
        $('#drp-ToStateId').each(function () {
            // Create option
            var option = $("<option />");
            option.attr("value", '0').text('--Select--');
            $('#drp-ToStateId').append(option);
        });
        $('#drp-ToTownId').empty();
        // Bind new values to dropdown
        $('#drp-ToTownId').each(function () {
            // Create option
            var option = $("<option />");
            option.attr("value", '0').text('--Select--');
            $('#drp-ToTownId').append(option);
        });
    }
});

$("#page_content").on("change", "#drp-ToStateId", function () {
    var value = $(this).val();
    if (value != "0") {
        $.ajax({
            url: constantPlanner.BindCity,
            data: { stateId: value },
            success: function (data) {

                $("#drp-ToTownId").html('');
                var toAppend = '';
                toAppend += "<option value='0'>--Select--</option>";
                $.each(data, function (index, item) {
                    toAppend += "<option value='" + item.Value + "'>" + item.Text + "</option>";
                })
                $("#drp-ToTownId").html(toAppend);
                if ($("#drp-ToTownId").val() == 0) {
                    $("#drp-ToTownId").val(0);
                }
            }
        });
    }
    else {

        $('#drp-ToTownId').empty();
        // Bind new values to dropdown
        $('#drp-ToTownId').each(function () {
            // Create option
            var option = $("<option />");
            option.attr("value", '0').text('--Select--');
            $('#drp-ToTownId').append(option);
        });
    }
});

function TravelleaveStepCallback(obj, context) {
    if (context.fromStep == 1) {

        var isError = false;
        var selectedRadio = $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find('input[name=lessThanADay]:checked').attr("id");
        if (selectedRadio == "adayormore") {
            var fromCountry = $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find("#drpCountry").val();
            var fromState = $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find("#drpState").val();
            var fromTown = $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find("#drpTown").val();
            var fromAirpoet = $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find("#drpAirport").val();
            var toCountry = $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find("#drp-ToCountryId").val();
            var toState = $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find("#drp-ToStateId").val();
            var toTown = $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find("#drp-ToTownId").val();
            var toAirport = $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find("#drp-ToAirportId").val();

            var reasonForLeave = $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find("#drp-ReasonForTravelId").val();
            var startDate = $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find("#txt_StartDate").val().trim();
            var endDate = $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find("#txt_EndDate").val().trim();
            var comments = $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find("#text_Comments").val().trim();
            var duration = $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find("#txt_Duration").val().trim();

            var type = $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find("#drp-Type").val();
            var customer = $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find("#drp-Customer").val();
            var project = $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find("#drp-Project").val();
            var costCode = $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find("#drp-CostCode").val();

            var isLessThenADay = false;

            if (fromCountry == "0") { isError = true; $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find("#lbl-error-FromCountryId").show(); }
            if (fromState == "0") { isError = true; $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find("#lbl-error-FromStateId").show(); }
            if (fromTown == "0") { isError = true; $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find("#lbl-error-FromTownId").show(); }
            if (toCountry == "0") { isError = true; $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find("#lbl-error-ToCountryId").show(); }
            if (toState == "0") { isError = true; $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find("#lbl-error-ToStateId").show(); }
            if (toTown == "0") { isError = true; $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find("#lbl-error-ToTownId").show(); }

            if (reasonForLeave == "0") { isError = true; $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find("#lbl-error-ReasonForTravelId").show(); }
            if (startDate == "") { isError = true; $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find("#lbl-error-StartDate").show(); }
            if (endDate == "") { isError = true; $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find("#lbl-error-EndDate").show(); }
            if (comments == "") { isError = true; $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find("#lbl-error-Comments").show(); }

            if (type == "0") { isError = true; $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find("#lbl-error-Type").show(); }
            if (costCode == "0") { isError = true; $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find("#lbl-error-CostCode").show(); }

            if (isError) {
                return false;
            }
            else {
                $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find('.buttonNext').hide();
                $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find('.buttonPrevious').show();
                $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find('.buttonFinish').show();
                return true;
            }
        }
        else {
            var fromCountry = $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find("#drpCountry").val();
            var fromState = $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find("#drpState").val();
            var fromTown = $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find("#drpTown").val();
            var fromAirpoet = $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find("#drpAirport").val();
            var toCountry = $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find("#drp-ToCountryId").val();
            var toState = $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find("#drp-ToStateId").val();
            var toTown = $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find("#drp-ToTownId").val();
            var toAirport = $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find("#drp-ToAirportId").val();

            var reasonForLeave = $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find("#drp-ReasonForTravelId").val();
            var startDate = $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find("#txt_LessThenStartDate").val().trim();
            var drphrSD = $('#drp-TravelHourseListSD').val();
            var drpminSD = $('#drp-MinutesTravelListSD').val();
            var drphrED = $('#drp-TravelHourseListED').val();
            var drminED = $('#drp-TravelMinutesListED').val();

            var isLessThenADay = true;
            var comments = $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find("#text_Comments").val().trim();
            var type = $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find("#drp-Type").val();
            var customer = $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find("#drp-Customer").val();
            var project = $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find("#drp-Project").val();
            var costCode = $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find("#drp-CostCode").val();

            if (drphrSD == 0)
            {
                isError = true; $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find("#lbl-error-InTimeSD").show();
            }
           
            if (drphrED == 0)
            {
                isError = true; $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find("#lbl-error-EndTime").show();
            }
           
            if (fromCountry == "0") { isError = true; $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find("#lbl-error-FromCountryId").show(); }
            if (fromState == "0") { isError = true; $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find("#lbl-error-FromStateId").show(); }
            if (fromTown == "0") { isError = true; $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find("#lbl-error-FromTownId").show(); }
            if (toCountry == "0") { isError = true; $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find("#lbl-error-ToCountryId").show(); }
            if (toState == "0") { isError = true; $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find("#lbl-error-ToStateId").show(); }
            if (toTown == "0") { isError = true; $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find("#lbl-error-ToTownId").show(); }

            if (reasonForLeave == "0") { isError = true; $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find("#lbl-error-ReasonForTravelId").show(); }
            if (startDate == "") { isError = true; $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find("#lbl-error-LessThenStartDate").show(); }


            if (comments == "") { isError = true; $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find("#lbl-error-Comments").show(); }

            if (type == "0") { isError = true; $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find("#lbl-error-Type").show(); }
            if (costCode == "0") { isError = true; $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find("#lbl-error-CostCode").show(); }

            if (isError) {
                return false;
            }
            else {

                $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find('.buttonNext').hide();
                $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find('.buttonPrevious').show();
                $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find('.buttonFinish').show();
                return true;
            }
        }

    }
    else {
        $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find('.buttonNext').show();
        $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find('.buttonPrevious').hide();
        $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find('.buttonFinish').hide();
        return true;
    }
}

function TravelLeaveonFinishCallback(obj, context) {
    var id = $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find("#HiddenId").val();
    var yearid = $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find("#HiddenYearId").val();
    var monthid = $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find("#HiddenMonthId").val();
    var dayid = $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find("#HiddendayId").val();
    var fromCountry = $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find("#drpCountry").val();
    var fromState = $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find("#drpState").val();
    var fromTown = $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find("#drpTown").val();
    var fromAirpoet = $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find("#drpAirport").val();
    var toCountry = $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find("#drp-ToCountryId").val();
    var toState = $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find("#drp-ToStateId").val();
    var toTown = $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find("#drp-ToTownId").val();
    var toAirport = $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find("#drp-ToAirportId").val();
    var type = $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find("#drp-Type").val();
    var customer = $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find("#selectID").val();
    var project = $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find("#drp-Project").val();
    var costCode = $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find("#drp-CostCode").val();
    var selectedRadio = $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find('input[name=lessThanADay]:checked').attr("id");
    var documentList = [];
    $.each($(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find('#filesList').find(".ListData"), function () {
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
        var reasonForLeave = $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find("#drp-ReasonForTravelId").val();
        var startDate = $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find("#txt_StartDate").val().trim();
        var endDate = $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find("#txt_EndDate").val().trim();
        var comments = $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find("#text_Comments").val().trim();
        var duration = $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find("#txt_Duration").val().trim();
        var isLessThenADay = false;
        SaveTravelLeave(id, isLessThenADay, reasonForLeave, startDate, endDate, duration, comments, yearid, monthid, dayid,0,0,0,0, 0, JsondocumentListJoinString, fromCountry, fromState, fromTown, fromAirpoet, toCountry, toState, toTown, toAirport, type, customer, project, costCode);
    }
    else {
        var reasonForLeave = $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find("#drp-ReasonForTravelId").val();
        var startDate = $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find("#txt_LessThenStartDate").val().trim();
        var drphrSD = $('#drp-TravelHourseListSD').val();
        var drpminSD = $('#drp-MinutesTravelListSD').val();
        var drphrED = $('#drp-TravelHourseListED').val();
        var drminED = $('#drp-TravelMinutesListED').val();
        var durationHr = $('#txt_DurationHours').val();
        var isLessThenADay = true;
        var comments = $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find("#text_Comments").val().trim();
        SaveTravelLeave(id, isLessThenADay, reasonForLeave, startDate, "", 0, comments, yearid, monthid, dayid, drphrSD, drpminSD, drphrED, drminED, durationHr, JsondocumentListJoinString, fromCountry, fromState, fromTown, fromAirpoet, toCountry, toState, toTown, toAirport, type, customer, project, costCode);
    }
}

function SaveTravelLeave(id, isLessThenADay, reasonForLeaveId, startDate, endDate, duration, comments, yearid, monthid, dayid, drphrSD, drpminSD, drphrED, drminED, durationHr, jsonString, fromCountry, fromState, fromTown, fromAirpoet, toCountry, toState, toTown, toAirport, type, customer, project, costCode) {
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
        InTimeHr:drphrSD,
        InTimeMin: drpminSD,
        EndTimeHr :drphrED,
        EndTimeMin:drminED,
        DurationHr:  durationHr,
        Type: type,
        Customer: customer,
        Project: project,
        CostCode: costCode,
        HolidayCountryID: $("#drp-publicHolidayCountry").val()
    }

    $.ajax({
        url: constantPlanner.SaveData_TravelLeave,
        type: 'POST',
        data: JSON.stringify(model),
        contentType: "application/json",
        success: function (data) {
            $(".accordion_container").find("#fixedrightcolumn_" + monthid).html("");
            $(".accordion_container").find("#fixedrightcolumn_" + monthid).html(data);

            $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find('.close').click();

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
                    url: constantPlanner.ImageData_TravelLeave,
                    contentType: false,
                    processData: false,
                    data: imageData,
                    success: function (result) {
                        var string = '<div class="row ListData"><div class="col-lg-1 icon"><i class="fa fa-paperclip"></i></div><div class="col-lg-10 attach-disc"><label class="fileName" data-newFileName="' + result.NewFileName + '">' + result.originalFileName + '</label><br /><input type="text" class="ImageDescription form-control" placeholder="Add Description Here"/></div><div class="col-lg-1 file-deleteicon"><i class="fa fa-trash-o"></i></div></div>';
                        var isEmpty = $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find("#filesList").html();
                        if (isEmpty = "") {
                            $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find("#filesList").html(string);
                        }
                        else {
                            $(".accordion_container").find("#MeEmployeePlanner_TravelLeaveModel").find("#MeEmployeePlanner_TravelLeaveBody").find("#filesList").append(string);
                        }
                        $(".hrtoolLoader").hide();
                        $(".modal-backdrop").hide();
                    }
                });
            }, 500);
        }
    }
});

//Travel leave End

//TimeSheet Start


function TimeSheetStepCallback(obj, context) {
    if (context.fromStep == 1) {
        var isError = false;
        var date = $(".accordion_container").find("#MeEmployeePlanner_TimeSheetModel").find("#MeEmployeePlanner_TimeSheetBody").find("#txt_Date").val().trim();
        if (date == "") {
            isError = true;
            $(".accordion_container").find("#MeEmployeePlanner_TimeSheetModel").find("#MeEmployeePlanner_TimeSheetBody").find("#lbl-error-Date").show();
        }
        $.each($(".accordion_container").find("#MeEmployeePlanner_TimeSheetModel").find("#MeEmployeePlanner_TimeSheetBody").find(".Timesheet_Detail_Div"), function () {
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
            var inTimeHr = $(this).find("#drp-HourseListSDTimesheet").val();
            var inTimeMin = $(this).find("#drp-MinutesListSDTimesheet").val();
            if (inTimeHr == 0 ){
                isError = true;
                $(this).find("#lbl-error-InTime").show();
            }
            var endTimeHr = $(this).find("#drp-HourseListEDTimesheet").val();
            var endTimeMin = $(this).find("#drp-MinutesListEDTimesheet").val();
            if (inTimeHr == 0) {
                isError = true;
                $(this).find("#lbl-error-EndTime").show();
            }
            var costCode = $(this).find("#drp-CostCode").val();
            if (costCode == 0) {
                isError = true;
                $(this).find("#lbl-error-CostCode").show();
            }
        });
        //var bol = getWorkPattData();
            
        if (isError) {
            return false;
        }
        else {
            $(".accordion_container").find("#MeEmployeePlanner_TimeSheetModel").find("#MeEmployeePlanner_TimeSheetBody").find('.buttonNext').hide();
            $(".accordion_container").find("#MeEmployeePlanner_TimeSheetModel").find("#MeEmployeePlanner_TimeSheetBody").find('.buttonPrevious').show();
            $(".accordion_container").find("#MeEmployeePlanner_TimeSheetModel").find("#MeEmployeePlanner_TimeSheetBody").find('.buttonFinish').show();
           
            return true;
        }
    }
    else {

        $(".accordion_container").find("#MeEmployeePlanner_TimeSheetModel").find("#MeEmployeePlanner_TimeSheetBody").find('.buttonNext').show();
        $(".accordion_container").find("#MeEmployeePlanner_TimeSheetModel").find("#MeEmployeePlanner_TimeSheetBody").find('.buttonPrevious').hide();
        $(".accordion_container").find("#MeEmployeePlanner_TimeSheetModel").find("#MeEmployeePlanner_TimeSheetBody").find('.buttonFinish').hide();
        return true;
    }
}

function TimeSheetonFinishCallback(obj, context) {
    var id = $(".accordion_container").find("#MeEmployeePlanner_TimeSheetModel").find("#MeEmployeePlanner_TimeSheetBody").find("#HiddenId").val();
    var yearid = $(".accordion_container").find("#MeEmployeePlanner_TimeSheetModel").find("#MeEmployeePlanner_TimeSheetBody").find("#HiddenYearId").val();
    var monthid = $(".accordion_container").find("#MeEmployeePlanner_TimeSheetModel").find("#MeEmployeePlanner_TimeSheetBody").find("#HiddenMonthId").val();
    var dayid = $(".accordion_container").find("#MeEmployeePlanner_TimeSheetModel").find("#MeEmployeePlanner_TimeSheetBody").find("#HiddendayId").val();
    var date = $(".accordion_container").find("#MeEmployeePlanner_TimeSheetModel").find("#MeEmployeePlanner_TimeSheetBody").find("#txt_Date").val().trim();
    var DetailDiv = [];
    $.each($(".accordion_container").find("#MeEmployeePlanner_TimeSheetModel").find("#MeEmployeePlanner_TimeSheetBody").find(".Timesheet_Detail_Div"), function () {
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
    $.each($(".accordion_container").find("#MeEmployeePlanner_TimeSheetModel").find("#MeEmployeePlanner_TimeSheetBody").find('#filesList').find(".ListData"), function () {
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

    var comment = $(".accordion_container").find("#MeEmployeePlanner_TimeSheetModel").find("#MeEmployeePlanner_TimeSheetBody").find("#text_Comments").val().trim();

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
        url: constantPlanner.SaveData_TimeSheet,
        type: 'POST',
        data: JSON.stringify(model),
        contentType: "application/json",
        success: function (data) {
            $(".accordion_container").find("#fixedrightcolumn_" + monthid).html("");
            $(".accordion_container").find("#fixedrightcolumn_" + monthid).html(data);

            $(".accordion_container").find("#MeEmployeePlanner_TimeSheetModel").find('.close').click();

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
        url: constantPlanner.addEdit_TimeSheet,
        data: { Id: id, Date: dayId, Month: monthId, Year: yearId },
        success: function (data) {
            debugger;
            $(".accordion_container").find("#MeEmployeePlanner_TimeSheetModel").find("#MeEmployeePlanner_TimeSheetBody").html("");
            $(".accordion_container").find("#MeEmployeePlanner_TimeSheetModel").find("#MeEmployeePlanner_TimeSheetBody").html(data);
            $(".accordion_container").find("#MeEmployeePlanner_TimeSheetModel").find("#MeEmployeePlanner_TimeSheetBody").find('#wizard').smartWizard({
                onLeaveStep: TimeSheetStepCallback,
                onFinish: TimeSheetonFinishCallback
            });
            $(".accordion_container").find("#MeEmployeePlanner_TimeSheetModel").find("#MeEmployeePlanner_TimeSheetBody").find('.buttonNext').addClass('btn btn-warning');
            $(".accordion_container").find("#MeEmployeePlanner_TimeSheetModel").find("#MeEmployeePlanner_TimeSheetBody").find('.buttonPrevious').addClass('btn btn-warning');
            $(".accordion_container").find("#MeEmployeePlanner_TimeSheetModel").find("#MeEmployeePlanner_TimeSheetBody").find('.buttonFinish').addClass('btn btn-success');
            $(".accordion_container").find("#MeEmployeePlanner_TimeSheetModel").find("#MeEmployeePlanner_TimeSheetBody").find('.buttonPrevious').hide();
            $(".accordion_container").find("#MeEmployeePlanner_TimeSheetModel").find("#MeEmployeePlanner_TimeSheetBody").find('.buttonFinish').hide();
            $(".accordion_container").find("#MeEmployeePlanner_TimeSheetModel").find("#MeEmployeePlanner_TimeSheetBody").find("#txt_Date").Zebra_DatePicker({
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {
                //    (".accordion_container").find("#MeEmployeePlanner_TimeSheetModel").find("#MeEmployeePlanner_TimeSheetBody").find("#lbl-error-Date").hide();
                    
                }
            });
        }
    });
});

$(".accordion_container").on("click", "#btn_AddNew_TimesheetDetail", function () {

    $.ajax({
        url: constantPlanner.addEdit_TimeSheet_Detail,
        success: function (data) {
            var html = $(".accordion_container").find("#MeEmployeePlanner_TimeSheetModel").find("#MeEmployeePlanner_TimeSheetBody").find("#TimeSheet_detail").html();
            if (html == "") {
                $(".accordion_container").find("#MeEmployeePlanner_TimeSheetModel").find("#MeEmployeePlanner_TimeSheetBody").find("#TimeSheet_detail").html(data);
            }
            else {
                $(".accordion_container").find("#MeEmployeePlanner_TimeSheetModel").find("#MeEmployeePlanner_TimeSheetBody").find("#TimeSheet_detail").append(data);
            }
            spinner();
        }
    });
});
$("#page_content").on('change', '#drp-HourseListSDTimesheet', function (e) {
    $("#lbl-error-InTime").hide();
    $("#lbl-error-EndTime").hide();
    $("#lbl-error-ValidInTime").hide();   
    var st = $("#drp-HourseListSDTimesheet").val();
    var et = $("#drp-HourseListEDTimesheet").val();
    if (st<et) {
        $("#lbl-error-ValidInTime").show();
    }
    else {
        $("#lbl-error-ValidInTime").hide();
    }
});
$("#page_content").on('change', '#drp-MinutesListSDTimesheet', function (e) {
    $("#lbl-error-InTime").hide();
    $("#lbl-error-EndTime").hide();
    $("#lbl-error-ValidInTime").hide();
   
   
});
$("#page_content").on('change', '#drp-HourseListEDTimesheet', function (e) {
    $("#lbl-error-InTime").hide();
    $("#lbl-error-EndTime").hide();
    $("#lbl-error-ValidInTime").hide();
    
    var st = $("#drp-HourseListSDTimesheet").val();
    var et = $("#drp-HourseListEDTimesheet").val();
    if (st < et) {
        $("#lbl-error-ValidInTime").show();
    }
    else {
        $("#lbl-error-ValidInTime").hide();
    }
});
$("#page_content").on('change', '#drp-MinutesListEDTimesheet', function (e) {
    $("#lbl-error-InTime").hide();
    $("#lbl-error-EndTime").hide();
    $("#lbl-error-ValidInTime").hide();
    var st = $("#drp-HourseListSDTimesheet").val();
    var et = $("#drp-HourseListEDTimesheet").val();
    
});

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
    var date = $(".accordion_container").find("#MeEmployeePlanner_TimeSheetModel").find("#MeEmployeePlanner_TimeSheetBody").find("#txt_Date").val().trim();
    $.each($(".accordion_container").find("#MeEmployeePlanner_TimeSheetModel").find("#MeEmployeePlanner_TimeSheetBody").find(".Timesheet_Detail_Div"), function () {
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
        url: constantPlanner.workPattenData,
        type: 'POST',
        data: model ,
        success: function (data) {
            bol = JSON.stringify(data);         
        }
    });
    return bol;
}

$(".accordion_container").on("click", ".timesheet_delete_icon", function () {
    $(this).parent().parent().parent().parent().remove();
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
                    url: constantPlanner.ImageData_TimeSheet,
                    contentType: false,
                    processData: false,
                    data: imageData,
                    success: function (result) {
                        var string = '<div class="row ListData"><div class="col-lg-1 icon"><i class="fa fa-paperclip"></i></div><div class="col-lg-10 attach-disc"><label class="fileName" data-newFileName="' + result.NewFileName + '">' + result.originalFileName + '</label><br /><input type="text" class="ImageDescription form-control" placeholder="Add Description Here"/></div><div class="col-lg-1 file-deleteicon"><i class="fa fa-trash-o"></i></div></div>';
                        var isEmpty = $(".accordion_container").find("#MeEmployeePlanner_TimeSheetModel").find("#MeEmployeePlanner_TimeSheetBody").find("#filesList").html();
                        if (isEmpty = "") {
                            $(".accordion_container").find("#MeEmployeePlanner_TimeSheetModel").find("#MeEmployeePlanner_TimeSheetBody").find("#filesList").html(string);
                        }
                        else {
                            $(".accordion_container").find("#MeEmployeePlanner_TimeSheetModel").find("#MeEmployeePlanner_TimeSheetBody").find("#filesList").append(string);
                        }
                        $(".hrtoolLoader").hide();
                        $(".modal-backdrop").hide();
                    }
                });
            }, 500);
        }
    }
});


//TimeSheet End
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

//common modal popup close scroll event
$(".accordion_container").on("click", ".closeModel", function () {

    var monthId = $(this).parent().parent().find('#HiddenMonthId').val();
    var positionTop = $("#page_content").find(".accordion_container").find("#accordion_table_" + monthId).offset();
    $('html, body').animate({ scrollTop: positionTop - 110 }, 'slow');
});


//Public Holiday Start

$("#page_content").on("click", ".closeModel_Public", function () {
    var value = $("#page_content").find("#Employee_CountryID").val();
    $("#page_content").find("#drp-publicHolidayCountry").val(value);
});

$("#drp-publicHolidayCountry").change(function () {

    var value = $(this).val();
    if (value != "0") {
        $('#MeEmployeePlanner_Effactive_PublicholidayModal').modal('show');
        $.ajax({
            url: constantPlanner.BindPublicHolidayTemplate,
            data: { CountryId: value },
            success: function (data) {

                $('#MeEmployeePlanner_Effactive_PublicholidayModal').find("#MeEmployeePlanner_Effactive_PublicholidayBody").html("");
                $('#MeEmployeePlanner_Effactive_PublicholidayModal').find("#MeEmployeePlanner_Effactive_PublicholidayBody").html(data);
                $('#MeEmployeePlanner_Effactive_PublicholidayModal').find("#MeEmployeePlanner_Effactive_PublicholidayBody").find("#txt_EffectiveDate").Zebra_DatePicker({
                    direction: false,
                    showButtonPanel: false,
                    format: 'd-m-Y',
                    onSelect: function () {
                        $('#MeEmployeePlanner_Effactive_PublicholidayModal').find("#MeEmployeePlanner_Effactive_PublicholidayBody").find("#lbl-error-EffectiveDate").hide();
                    }
                });

            }
        });
    }
    else {
        return false;
    }
});

$("#btn-submit-MeEmployeePlanner-EffactiveDateSave").click(function () {

    $(".hrtoolLoader").show();
    $(".modal-backdrop").show();
    var effactiveDate = $('#MeEmployeePlanner_Effactive_PublicholidayModal').find("#MeEmployeePlanner_Effactive_PublicholidayBody").find("#txt_EffectiveDate").val().trim();
    var isError = false;
    if (effactiveDate == "") {
        isError = true;
        $('#MeEmployeePlanner_Effactive_PublicholidayModal').find("#MeEmployeePlanner_Effactive_PublicholidayBody").find("#lbl-error-EffectiveDate").show();
    }
    var countryId = $('#MeEmployeePlanner_Effactive_PublicholidayModal').find("#MeEmployeePlanner_Effactive_PublicholidayBody").find("#countryId").val();

    if (isError) {
        $(".hrtoolLoader").hide();
        $(".modal-backdrop").hide();
        return false;
    }
    else {
        $.ajax({
            url: constantPlanner.SavePublicHolidayTemplate,
            data: { CountryId: countryId, EffactiveDate: effactiveDate, EmployeeId: $("#currentEmployeeId").val(),HolidayCountryID:$("#drp-publicHolidayCountry").val() },
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
                    $("#page_content").find("#MeEmployeePlanner_Effactive_PublicholidayModal").find(".close").click();

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
        url: constantPlanner.addEdit_PublicHoliday,
        data: { Id: id, Date: dayId, Month: monthId, Year: yearId },
        success: function (data) {
            $(".accordion_container").find("#MeEmployeePlanner_PublicholidayModel").find("#MeEmployeePlanner_PublicHolidayBody").html("");
            $(".accordion_container").find("#MeEmployeePlanner_PublicholidayModel").find("#MeEmployeePlanner_PublicHolidayBody").html(data);
        }
    });
});
//public Holiday End


//Settings btn-PlannerSettings
$("#page_content").on("click", ".btn-PlannerSettings", function () {
    $(".hrtoolLoader").show();
    var Id = $("#page_content").find("#currentEmployeeId").val();
    $.ajax({
        url: constantPlanner.BindSettings,
        data: { Id: Id },
        success: function (data) {

            $("#MeEmployeePlanner_Settings").find("#MeEmployeePlanner_SettingsBody").html('');
            $("#MeEmployeePlanner_Settings").find("#MeEmployeePlanner_SettingsBody").html(data);
            $('[data-toggle="tooltip"]').tooltip();
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
        }
    });

});

//btn-sub-closeModel
$("#page_content").on("click", ".btn-sub-closeModel", function () {
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
        url: constantPlanner.SaveAnnualSettings,
        type: 'POST',
        data: JSON.stringify(model),
        contentType: "application/json",
        success: function (data) {



        }
    });

});

//ADD TOIL

$("#page_content").on("click", "#ADDLeaves", function () {

    var Id = $("#page_content").find("#EmployeeSettingsID").val();
    $.ajax({
        url: constantPlanner.AddEditTOIL,
        data: { Id: Id },
        success: function (data) {

            $("#MeEmployeePlanner_TOIL").find("#MeEmployeePlanner_TOILBody").html('');
            $("#MeEmployeePlanner_TOIL").find("#MeEmployeePlanner_TOILBody").html(data);
            $('[data-toggle="tooltip"]').tooltip();
            $("#page_content").find("#MeEmployeePlanner_Settings").find("#MeEmployeePlanner_TOIL").find("#txt_ExpiryDate").Zebra_DatePicker({
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

$("#page_content").on("click", "#DurationDays_Plus", function () {

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

$("#page_content").on("click", "#btn-submit-MeEmployeePlanner_TOIL", function () {

    $(".hrtoolLoader").show();
    $(".modal-backdrop").show();
    var IsError = false;
    var Id = $("#page_content").find("#TOIL_Id").val();
    var EmployeeId = $("#page_content").find("#EmployeeSettingsID").val();
    var Balance = $("#page_content").find("#Toil_Balance").val();
    var DurationDays = $("#page_content").find("#Toil_DurationDays").val();
    var AddEdit = $("#page_content").find("#DurationDays_Plus").html();
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
            url: constantPlanner.SaveTOIL,
            type: 'POST',
            data: JSON.stringify(model),
            contentType: "application/json",
            success: function (data) {
                $("#MeEmployeePlanner_Settings").find("#MeEmployeePlanner_SettingsBody").html('');
                $("#MeEmployeePlanner_Settings").find("#MeEmployeePlanner_SettingsBody").html(data);
                $('[data-toggle="tooltip"]').tooltip();
                $(".hrtoolLoader").hide();
                $(".modal-backdrop").hide();
            }
        });

    }
});

//Sick Leave  

$(".accordion_container").on('change', '#adayormore', function (e) {
    if ($(this).prop("checked")) {
        $("#div_HalfDayOrHours").hide();
        $(".IsHours").hide();
        $(".IsHalfDay").hide();
        $("#div_SickADayOrMore").show();
    }
    else {
        $("#div_HalfDayOrHours").hide();
        $(".IsHours").hide();
        $(".IsHalfDay").hide();
        $("#div_SickADayOrMore").show();
    }
});
//Ishalfday 
$(".accordion_container").on('change', '#Ishalfday', function (e) {
    if ($(this).prop("checked")) {
        $("#div_HalfDayOrHours").show();
        $(".IsHours").hide();
        $(".IsHalfDay").show();
        $("#div_SickADayOrMore").hide();
    }
    else {
        $("#div_HalfDayOrHours").hide();
        $(".IsHours").hide();
        $(".IsHalfDay").hide();
        $("#div_SickADayOrMore").show();
    }

});
//Ishours
$(".accordion_container").on('change', '#Ishours', function (e) {
    if ($(this).prop("checked")) {
        $("#div_HalfDayOrHours").show();
        $(".IsHours").show();
        $(".IsHalfDay").hide();
        $("#div_SickADayOrMore").hide();
    }
    else {
        $("#div_HalfDayOrHours").hide();
        $(".IsHours").hide();
        $(".IsHalfDay").hide();
        $("#div_SickADayOrMore").show();
    }

});

//check_SelfCertificationFormRequired
$(".accordion_container").on('change', '#check_SelfCertificationFormRequired', function (e) {
    if ($(this).prop("checked")) {
        $("#div_SelfCertificationFormRequired").show();
    }
    else {
        $("#div_SelfCertificationFormRequired").hide();
    }

});

//check_BackToWorkInterviewRequired
$(".accordion_container").on('change', '#check_BackToWorkInterviewRequired', function (e) {
    if ($(this).prop("checked")) {
        $("#div_BackToWorkInterviewRequired").show();
    }
    else {
        $("#div_BackToWorkInterviewRequired").hide();
    }

});

//check_DoctorConsulted
$(".accordion_container").on('change', '#check_DoctorConsulted', function (e) {
    if ($(this).prop("checked")) {
        $("#div_DoctorConsulted").show();
        $("#div_DoctorNotConsulte").hide();
    }
    else {
        $("#div_DoctorConsulted").hide();
        $("#div_DoctorNotConsulte").show();
    }

});
function validateMadicalCerDate(stDate, edDate)
{
    if (stDate != "" || edDate != "") {

        if (StartDateValidation(stDate, edDate)) {
            $("#page_content").find("#MeEmployeePlanner_SickLeaveModel").find("#lbl-error-MedicalCertificateDate").show();
            $("#page_content").find("#txt_MedicalCertificateIssuedDate").val('');
        }        
    }
}
$("#page_content").on("click", ".btn_AddEdit_SickLeave", function () {
    $(".hrtoolLoader").show();
    var yearId = $(this).attr("data-yearid");
    var monthId = $(this).attr("data-monthid");
    var dayId = $(this).attr("data-dayid");
    var id = $(this).attr("data-id");
    $.ajax({
        url: constantPlanner.addEdit_SickLeave,
        data: { Id: id, Date: dayId, Month: monthId, Year: yearId },
        success: function (data) {
            $(".accordion_container").find("#MeEmployeePlanner_SickLeaveModel").find("#MeEmployeePlanner_SickLeaveBody").html("");
            $(".accordion_container").find("#MeEmployeePlanner_SickLeaveModel").find("#MeEmployeePlanner_SickLeaveBody").html(data);

            $(".accordion_container").find("#MeEmployeePlanner_SickLeaveModel").find("#MeEmployeePlanner_SickLeaveBody").find('#wizard').smartWizard({
                onLeaveStep: SickLeavesStepCallback,
                onFinish: SickLeavesFinishCallback
            });

            $(".accordion_container").find("#MeEmployeePlanner_SickLeaveModel").find("#MeEmployeePlanner_SickLeaveBody").find('.buttonNext').addClass('btn btn-warning');
            $(".accordion_container").find("#MeEmployeePlanner_SickLeaveModel").find("#MeEmployeePlanner_SickLeaveBody").find('.buttonPrevious').addClass('btn btn-warning');
            $(".accordion_container").find("#MeEmployeePlanner_SickLeaveModel").find("#MeEmployeePlanner_SickLeaveBody").find('.buttonFinish').addClass('btn btn-success');
            $(".accordion_container").find("#MeEmployeePlanner_SickLeaveModel").find("#MeEmployeePlanner_SickLeaveBody").find('.buttonPrevious').hide();
            $(".accordion_container").find("#MeEmployeePlanner_SickLeaveModel").find("#MeEmployeePlanner_SickLeaveBody").find('.buttonFinish').hide();

            $(".accordion_container").find("#MeEmployeePlanner_SickLeaveModel").find("#MeEmployeePlanner_SickLeaveBody").find("#txt_StartDate").Zebra_DatePicker({
                //direction: false,
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {
                    $("#page_content").find("#lbl-error-GreaterEndDate").hide();

                    var startdate = $(this).val();
                    var enddate = $("#page_content").find("#txt_EndDate").val();
                    calculateAnnualDateDiff(startdate, enddate);
                    //calculateAnnualDateDiffTravelLeave();

                }
            });
            $(".accordion_container").find("#MeEmployeePlanner_SickLeaveModel").find("#MeEmployeePlanner_SickLeaveBody").find("#txt_EndDate").Zebra_DatePicker({
                //direction: false,
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {
                    $("#page_content").find("#lbl-error-GreaterEndDate").hide();
                    $("#page_content").find("#lbl-error-EndDate").hide();
                    var enddate = $(this).val();
                    var startdate = $("#page_content").find("#txt_StartDate").val();
                    calculateAnnualDateDiff(startdate, enddate);
                    //calculateAnnualDateDiffTravelLeave();
                }
            });

            $(".accordion_container").find("#MeEmployeePlanner_SickLeaveModel").find("#MeEmployeePlanner_SickLeaveBody").find("#txt_SelfCertificateReceivedDate").Zebra_DatePicker({
                //direction: false,
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {

                }
            });
            //txt_InterviewDate
            $(".accordion_container").find("#MeEmployeePlanner_SickLeaveModel").find("#MeEmployeePlanner_SickLeaveBody").find("#txt_InterviewDate").Zebra_DatePicker({
                //direction: false,
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {

                }
            });
            //txt_DateOfVisit
            $(".accordion_container").find("#MeEmployeePlanner_SickLeaveModel").find("#MeEmployeePlanner_SickLeaveBody").find("#txt_DateOfVisit").Zebra_DatePicker({
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {
                    $("#page_content").find("#MeEmployeePlanner_SickLeaveModel").find("#lbl-error-MedicalCertificateDate").hide();
                    var stDate = $("#txt_DateOfVisit").val();
                    var edDate = $("#txt_MedicalCertificateIssuedDate").val();
                    validateMadicalCerDate(stDate, edDate);
                }
            });
            //txt_MedicalCertificateIssuedDate
            $(".accordion_container").find("#MeEmployeePlanner_SickLeaveModel").find("#MeEmployeePlanner_SickLeaveBody").find("#txt_MedicalCertificateIssuedDate").Zebra_DatePicker({
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {
                    $("#page_content").find("#MeEmployeePlanner_SickLeaveModel").find("#lbl-error-MedicalCertificateDate").hide();
                    var stDate = $("#txt_DateOfVisit").val();
                    var edDate = $("#txt_MedicalCertificateIssuedDate").val();
                    validateMadicalCerDate(stDate, edDate);
                }
            });
            //txt_DateOfVisit
            //$(".accordion_container").find("#MeEmployeePlanner_SickLeaveModel").find("#MeEmployeePlanner_SickLeaveBody").find("#txt_DateOfVisit").Zebra_DatePicker({
            //    //direction: false,
            //    showButtonPanel: false,
            //    format: 'd-m-Y',
            //    onSelect: function () {

            //    }
            //});
            $(".accordion_container").find("#MeEmployeePlanner_SickLeaveModel").find("#MeEmployeePlanner_SickLeaveBody").find('div#froala-editor').froalaEditor({
                //toolbarButtons: ['bold', 'italic', 'underline', 'strikeThrough', 'subscript', 'superscript', 'fontFamily', 'fontSize', '|', 'specialCharacters', 'color', 'emoticons', 'inlineStyle', 'paragraphStyle', '|', 'paragraphFormat', 'align', 'formatOL', 'formatUL', 'outdent', 'indent', '-', 'quote', 'insertHR', 'insertLink', 'insertImage', 'insertVideo', 'insertFile', 'insertTable', '|', 'undo', 'redo', 'clearFormatting', 'selectAll', 'html', 'applyFormat', 'removeFormat', 'fullscreen', 'print', 'help'],
                toolbarButtons: ['bold', 'italic', 'underline', 'formatUL'],
                pluginsEnabled: null
            });

            if (id > 0) {

                //var selectedRadio = $(".accordion_container").find("#MeEmployeePlanner_SickLeaveModel").find("#MeEmployeePlanner_SickLeaveBody").find('input[name=IsHalfDay]:checked').attr("id");
                //var selectRadios = $(".accordion_container").find("#MeEmployeePlanner_SickLeaveModel").find("#MeEmployeePlanner_SickLeaveBody").find('input[name=IsHours]:checked').attr("id");

                //if (selectedRadio == "adayormore" || selectRadios == "adayormore") {
                //    $(".accordion_container").find("#MeEmployeePlanner_SickLeaveModel").find("#MeEmployeePlanner_SickLeaveBody").find("#div_SickADayOrMore").show();
                //    $(".accordion_container").find("#MeEmployeePlanner_SickLeaveModel").find("#MeEmployeePlanner_SickLeaveBody").find("#div_HalfDayOrHours").hide();
                //}
                //else {
                //    $(".accordion_container").find("#MeEmployeePlanner_SickLeaveModel").find("#MeEmployeePlanner_SickLeaveBody").find("#div_SickADayOrMore").show();
                //    $(".accordion_container").find("#MeEmployeePlanner_SickLeaveModel").find("#MeEmployeePlanner_SickLeaveBody  ").find("#div_HalfDayOrHours").hide();
                //}

                var aDayorMore = $("input[id='adayormore']:checked").val();
                var Ishalfday = $("input[id='Ishalfday']:checked").val();
                if (aDayorMore == undefined) {
                    $(".accordion_container").find("#MeEmployeePlanner_SickLeaveModel").find("#MeEmployeePlanner_SickLeaveBody").find("#div_SickADayOrMore").hide();
                    $(".accordion_container").find("#MeEmployeePlanner_SickLeaveModel").find("#MeEmployeePlanner_SickLeaveBody  ").find("#div_HalfDayOrHours").show();
                    if (Ishalfday == undefined) {
                        $(".accordion_container").find("#MeEmployeePlanner_SickLeaveModel").find("#MeEmployeePlanner_SickLeaveBody  ").find("#div_HalfDayOrHours").find("#div_IsHalfDay").hide();
                        $(".accordion_container").find("#MeEmployeePlanner_SickLeaveModel").find("#MeEmployeePlanner_SickLeaveBody  ").find("#div_HalfDayOrHours").find("#div_IsHours").show();
                    } else {
                        $(".accordion_container").find("#MeEmployeePlanner_SickLeaveModel").find("#MeEmployeePlanner_SickLeaveBody  ").find("#div_HalfDayOrHours").find("#div_IsHalfDay").show();
                        $(".accordion_container").find("#MeEmployeePlanner_SickLeaveModel").find("#MeEmployeePlanner_SickLeaveBody  ").find("#div_HalfDayOrHours").find("#div_IsHours").hide();
                    }
                }
                else {
                    $(".accordion_container").find("#MeEmployeePlanner_SickLeaveModel").find("#MeEmployeePlanner_SickLeaveBody").find("#div_SickADayOrMore").show();
                    $(".accordion_container").find("#MeEmployeePlanner_SickLeaveModel").find("#MeEmployeePlanner_SickLeaveBody  ").find("#div_HalfDayOrHours").hide();
                }
            }

            spinner();
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
        }
    });
});


function SickLeavesStepCallback(obj, context) {

    if (context.fromStep == 1) {
        var isError = false;
        var ADayorMore = $("#page_content").find("#adayormore").prop("checked");
        var IsHalfDay = $("#page_content").find("#Ishalfday").prop("checked");
        var IsHours = $("#page_content").find("#Ishours").prop("checked");
        var Reason = $("#page_content").find("#drp-ReasonSickLeaveList").val();
        var StartDate = $("#page_content").find("#txt_StartDate").val();
        if (StartDate == "") {
            isError = true;
            $("#page_content").find("#lbl-error-StartDate").show();
        }
        if (ADayorMore) {
            var EndDate = $("#page_content").find("#txt_EndDate").val();
            var Duration = $("#page_content").find("#txt_Duration").val();
            if (EndDate == "") {
                isError = true;
                $("#page_content").find("#lbl-error-EndDate").show();
            }
        }
        else {
            var PartOfDay = $("#page_content").find("#drp-PartOfDay").val();
            var DurationHours = $("#page_content").find("#txt_DurationHours").val();
            if (parseInt(DurationHours) > 0) {
                $("#page_content").find("#lbl-error-DurationHours").show();
            }

        }
        var EmergencyLeave = $("#page_content").find("#check_EmergencyLeave").prop("checked");

        if (isError) {
            return false;
        }
        else {
            if (context.toStep = 2) {
                $(".accordion_container").find("#MeEmployeePlanner_SickLeaveModel").find("#MeEmployeePlanner_SickLeaveBody").find('.buttonNext').show();
                $(".accordion_container").find("#MeEmployeePlanner_SickLeaveModel").find("#MeEmployeePlanner_SickLeaveBody").find('.buttonPrevious').show();
                $(".accordion_container").find("#MeEmployeePlanner_SickLeaveModel").find("#MeEmployeePlanner_SickLeaveBody").find('.buttonFinish').hide();
            }
            else {
                $(".accordion_container").find("#MeEmployeePlanner_SickLeaveModel").find("#MeEmployeePlanner_SickLeaveBody").find('.buttonNext').show();
                $(".accordion_container").find("#MeEmployeePlanner_SickLeaveModel").find("#MeEmployeePlanner_SickLeaveBody").find('.buttonPrevious').show();
                $(".accordion_container").find("#MeEmployeePlanner_SickLeaveModel").find("#MeEmployeePlanner_SickLeaveBody").find('.buttonFinish').hide();
            }

            return true;
        }
    }
    if (context.fromStep == 2) {
        var isError = false;
        var ConfirmedbyHR = $("#page_content").find("#check_ConfirmedByHR").prop("checked");
        var SelfCertificationFormRequired = $("#page_content").find("#check_SelfCertificationFormRequired").prop("checked");
        if (SelfCertificationFormRequired == true) {
            var SelfCertificateReceivedDate = $("#page_content").find("#txt_SelfCertificateReceivedDate").val();
        }

        var BackToWorkInterviewRequired = $("#page_content").find("#check_BackToWorkInterviewRequired").prop("checked");
        if (BackToWorkInterviewRequired == true) {
            var InterviewDate = $("#page_content").find("#txt_InterviewDate").val();
            var InterviewConductedBy = $("#page_content").find("#txt_InterviewConductedBy").val().trim();
        }
        var IsPaid = $("#page_content").find("#Paid").prop("checked");
        var IsPaidatotherrate = $("#page_content").find("#Paidatotherrate").prop("checked");
        var IsUnpaid = $("#page_content").find("#Unpaid").prop("checked");


        if (isError) {
            return false;
        }
        else {
            if (context.toStep == 3) {
                $(".accordion_container").find("#MeEmployeePlanner_SickLeaveModel").find("#MeEmployeePlanner_SickLeaveBody").find('.buttonNext').show();
                $(".accordion_container").find("#MeEmployeePlanner_SickLeaveModel").find("#MeEmployeePlanner_SickLeaveBody").find('.buttonPrevious').show();
                $(".accordion_container").find("#MeEmployeePlanner_SickLeaveModel").find("#MeEmployeePlanner_SickLeaveBody").find('.buttonFinish').hide();
            }
            else {
                $(".accordion_container").find("#MeEmployeePlanner_SickLeaveModel").find("#MeEmployeePlanner_SickLeaveBody").find('.buttonNext').show();
                $(".accordion_container").find("#MeEmployeePlanner_SickLeaveModel").find("#MeEmployeePlanner_SickLeaveBody").find('.buttonPrevious').show();
                $(".accordion_container").find("#MeEmployeePlanner_SickLeaveModel").find("#MeEmployeePlanner_SickLeaveBody").find('.buttonFinish').hide();
            }

            return true;
        }
    }
    if (context.fromStep == 3) {
        var isError = false;
        var DoctorConsulted = $("#page_content").find("#check_DoctorConsulted").prop("checked");
        if (DoctorConsulted == true) {
            var DoctorName = $("#page_content").find("#txt_DoctorName").val().trim();
            var DateOfVisit = $("#page_content").find("#txt_DateOfVisit").val();
            var TimeOfVisit = $("#page_content").find("#drp-Timeofvisit").val();
            var MedicalCertificateIssuedDate = $("#page_content").find("#txt_MedicalCertificateIssuedDate").val();
            var DoctorAdvice = $("#page_content").find("#txt_DoctorAdvice").val().trim();
            var MedicationPrescribed = $("#page_content").find("#txt_MedicationPrescribed").val().trim();

        } else {
            var WhyDoctorNotConsulted = $("#page_content").find("#txt_WhyDoctorNotConsulted").val().trim();
        }
        var IsDuaToAccident = $("#page_content").find("#check_IsDuaToAccident").prop("checked");


        if (isError) {
            return false;
        }
        else {

            if (context.toStep == 4) {
                $(".accordion_container").find("#MeEmployeePlanner_SickLeaveModel").find("#MeEmployeePlanner_SickLeaveBody").find('.buttonNext').show();
                $(".accordion_container").find("#MeEmployeePlanner_SickLeaveModel").find("#MeEmployeePlanner_SickLeaveBody").find('.buttonPrevious').show();
                $(".accordion_container").find("#MeEmployeePlanner_SickLeaveModel").find("#MeEmployeePlanner_SickLeaveBody").find('.buttonFinish').hide();
            }
            else {
                $(".accordion_container").find("#MeEmployeePlanner_SickLeaveModel").find("#MeEmployeePlanner_SickLeaveBody").find('.buttonNext').show();
                $(".accordion_container").find("#MeEmployeePlanner_SickLeaveModel").find("#MeEmployeePlanner_SickLeaveBody").find('.buttonPrevious').show();
                $(".accordion_container").find("#MeEmployeePlanner_SickLeaveModel").find("#MeEmployeePlanner_SickLeaveBody").find('.buttonFinish').hide();
            }
            return true;
        }
    }
    if (context.fromStep == 4) {
        var isError = false;
        if (isError) {
            return false;
        }
        else {
            if (context.toStep == 5) {
                $(".accordion_container").find("#MeEmployeePlanner_SickLeaveModel").find("#MeEmployeePlanner_SickLeaveBody").find('.buttonNext').hide();
                $(".accordion_container").find("#MeEmployeePlanner_SickLeaveModel").find("#MeEmployeePlanner_SickLeaveBody").find('.buttonPrevious').show();
                $(".accordion_container").find("#MeEmployeePlanner_SickLeaveModel").find("#MeEmployeePlanner_SickLeaveBody").find('.buttonFinish').show();
            }
            else {
                $(".accordion_container").find("#MeEmployeePlanner_SickLeaveModel").find("#MeEmployeePlanner_SickLeaveBody").find('.buttonNext').show();
                $(".accordion_container").find("#MeEmployeePlanner_SickLeaveModel").find("#MeEmployeePlanner_SickLeaveBody").find('.buttonPrevious').show();
                $(".accordion_container").find("#MeEmployeePlanner_SickLeaveModel").find("#MeEmployeePlanner_SickLeaveBody").find('.buttonFinish').hide();
            }
            return true;
        }
    }
    else {
        if (context.toStep == 4) {
            $(".accordion_container").find("#MeEmployeePlanner_SickLeaveModel").find("#MeEmployeePlanner_SickLeaveBody").find('.buttonNext').show();
            $(".accordion_container").find("#MeEmployeePlanner_SickLeaveModel").find("#MeEmployeePlanner_SickLeaveBody").find('.buttonPrevious').show();
            $(".accordion_container").find("#MeEmployeePlanner_SickLeaveModel").find("#MeEmployeePlanner_SickLeaveBody").find('.buttonFinish').hide();
        }
        else {
            $(".accordion_container").find("#MeEmployeePlanner_SickLeaveModel").find("#MeEmployeePlanner_SickLeaveBody").find('.buttonNext').hide();
            $(".accordion_container").find("#MeEmployeePlanner_SickLeaveModel").find("#MeEmployeePlanner_SickLeaveBody").find('.buttonPrevious').show();
            $(".accordion_container").find("#MeEmployeePlanner_SickLeaveModel").find("#MeEmployeePlanner_SickLeaveBody").find('.buttonFinish').show();
        }
        return true;
        //$(".accordion_container").find("#MeEmployeePlanner_SickLeaveModel").find("#MeEmployeePlanner_SickLeaveBody").find('.buttonNext').show();
        //$(".accordion_container").find("#MeEmployeePlanner_SickLeaveModel").find("#MeEmployeePlanner_SickLeaveBody").find('.buttonPrevious').hide();
        //$(".accordion_container").find("#MeEmployeePlanner_SickLeaveModel").find("#MeEmployeePlanner_SickLeaveBody").find('.buttonFinish').hide();
        //return true;
    }

}

function SickLeavesFinishCallback() {
    //Step 1
    $(".hrtoolLoader").show();
    var Id = $("#page_content").find("#SickLeaveID").val();
    var yearid = $(".accordion_container").find("#MeEmployeePlanner_SickLeaveModel").find("#MeEmployeePlanner_SickLeaveBody").find("#HiddenYearId").val();
    var monthid = $(".accordion_container").find("#MeEmployeePlanner_SickLeaveModel").find("#MeEmployeePlanner_SickLeaveBody").find("#HiddenMonthId").val();
    var dayid = $(".accordion_container").find("#MeEmployeePlanner_SickLeaveModel").find("#MeEmployeePlanner_SickLeaveBody").find("#HiddendayId").val();

    var ADayorMore = $("#page_content").find("#adayormore").prop("checked");
    var IsHalfDay = $("#page_content").find("#Ishalfday").prop("checked");
    var IsHours = $("#page_content").find("#Ishours").prop("checked");
    var Reason = $("#page_content").find("#drp-ReasonSickLeaveList").val();
    var StartDate = $("#page_content").find("#txt_StartDate").val();
    if (ADayorMore) {
        var EndDate = $("#page_content").find("#txt_EndDate").val();
        var Duration = $("#page_content").find("#txt_Duration").val();
    }
    else {
        var PartOfDay = $("#page_content").find("#drp-PartOfDay").val();
        var DurationHours = $("#page_content").find("#txt_DurationHours").val();
    }
    var EmergencyLeave = $("#page_content").find("#check_EmergencyLeave").prop("checked");
    //Step 2
    var ConfirmedbyHR = $("#page_content").find("#check_ConfirmedByHR").prop("checked");
    var SelfCertificationFormRequired = $("#page_content").find("#check_SelfCertificationFormRequired").prop("checked");
    if (SelfCertificationFormRequired == true) {
        var SelfCertificateReceivedDate = $("#page_content").find("#txt_SelfCertificateReceivedDate").val();
    }

    var BackToWorkInterviewRequired = $("#page_content").find("#check_BackToWorkInterviewRequired").prop("checked");
    if (BackToWorkInterviewRequired == true) {
        var InterviewDate = $("#page_content").find("#txt_InterviewDate").val();
        var InterviewConductedBy = $("#page_content").find("#txt_InterviewConductedBy").val().trim();
    }
    var IsPaid = $("#page_content").find("#Paid").prop("checked");
    var IsPaidatotherrate = $("#page_content").find("#Paidatotherrate").prop("checked");
    var IsUnpaid = $("#page_content").find("#Unpaid").prop("checked");
    //Step 3
    var DoctorConsulted = $("#page_content").find("#check_DoctorConsulted").prop("checked");
    if (DoctorConsulted == true) {
        var DoctorName = $("#page_content").find("#txt_DoctorName").val().trim();
        var DateOfVisit = $("#page_content").find("#txt_DateOfVisit").val();
        var TimeOfVisit = $("#page_content").find("#drp-Timeofvisit").val();
        var MedicalCertificateIssuedDate = $("#page_content").find("#txt_MedicalCertificateIssuedDate").val();
        var DoctorAdvice = $("#page_content").find("#txt_DoctorAdvice").val().trim();
        var MedicationPrescribed = $("#page_content").find("#txt_MedicationPrescribed").val().trim();

    } else {
        var WhyDoctorNotConsulted = $("#page_content").find("#txt_WhyDoctorNotConsulted").val().trim();
    }
    var IsDuaToAccident = $("#page_content").find("#check_IsDuaToAccident").prop("checked");
    //Step 4
    var commentList = [];
    $.each($(".accordion_container").find('#CommentList').find(".seccomments"), function () {
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

    //Step 5
    var documentList = [];
    $.each($(".accordion_container").find('#filesList').find(".ListData"), function () {
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
        EmployeeId: $("#currentEmployeeId").val(),
        yearId: yearid,
        monthId: monthid,
        day: dayid,
        IsADayOrMore: ADayorMore,
        IsHalfDay: IsHalfDay,
        IsHours: IsHours,
        Reason: Reason,
        StartDate: StartDate,
        EndDate: EndDate,
        DurationDays: Duration,
        PartOfDay: PartOfDay,
        DurationHours: DurationHours,
        EmergencyLeave: EmergencyLeave,
        ConfirmedbyHR: ConfirmedbyHR,
        SelfCertificationFormRequired: SelfCertificationFormRequired,
        SelfCertificateReceivedDate: SelfCertificateReceivedDate,
        BackToWorkInterviewRequired: BackToWorkInterviewRequired,
        InterviewDate: InterviewDate,
        InterviewConductedBy: InterviewConductedBy,
        IsPaid: IsPaid,
        IsPaidatotherrate: IsPaidatotherrate,
        IsUnpaid: IsUnpaid,
        DoctorConsulted: DoctorConsulted,
        DoctorName: DoctorName,
        DateOfVisit: DateOfVisit,
        TimeOfVisit: TimeOfVisit,
        MedicalCertificateIssuedDate: MedicalCertificateIssuedDate,
        DoctorAdvice: DoctorAdvice,
        MedicationPrescribed: MedicationPrescribed,
        WhyDoctorNotConsulted: WhyDoctorNotConsulted,
        IsDuaToAccident: IsDuaToAccident,
        CommentListString: JsoncommentListJoinString,
        DocumentListString: JsondocumentListJoinString,
        HolidayCountryID: $("#drp-publicHolidayCountry").val()
    }

    $.ajax({
        url: constantPlanner.saveData_SickLeave,
        type: 'POST',
        data: JSON.stringify(model),
        contentType: "application/json",
        success: function (data) {
            $(".accordion_container").find("#fixedrightcolumn_" + monthid).html("");
            $(".accordion_container").find("#fixedrightcolumn_" + monthid).html(data);

            $(".accordion_container").find("#MeEmployeePlanner_SickLeaveModel").find('.close').click();

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

$("#page_content").on('click', '.deleteComment', function () {
    $(this).parent().parent().remove();
});

$("#page_content").on('click', '.editComment', function () {
    $("#page_content").find('#btnAddComment').hide();
    $("#page_content").find('#btnEditComment').show();
    $("#page_content").find('#btnEditComment').attr("data-editId", $(this).parent().parent().attr("id"));
    var htmlString = $(this).parent().parent().find(".sickComments").html();
    $("#page_content").find('#MeEmployeePlanner_SickLeaveBody').find('div#froala-editor').froalaEditor('html.set', htmlString);
});

$("#page_content").on('click', '#btnEditComment', function () {
    var editDiv = $(this).attr("data-editId");
    var data = $("#page_content").find('#MeEmployeePlanner_SickLeaveBody').find('div#froala-editor').froalaEditor('html.get');
    $("#page_content").find("#CommentList").find("#" + editDiv).find(".sickComments").html("");
    $("#page_content").find("#CommentList").find("#" + editDiv).find(".sickComments").html(data);

    if ($('div#froala-editor').data('froala.editor')) {
        $('div#froala-editor').froalaEditor('destroy');
        $('div#froala-editor').html('');
    }
    if (!$('div#froala-editor').data('froala.editor')) {
        $('div#froala-editor').froalaEditor({
            //toolbarButtons: ['bold', 'italic', 'underline', 'strikeThrough', 'subscript', 'superscript', 'fontFamily', 'fontSize', '|', 'specialCharacters', 'color', 'emoticons', 'inlineStyle', 'paragraphStyle', '|', 'paragraphFormat', 'align', 'formatOL', 'formatUL', 'outdent', 'indent', '-', 'quote', 'insertHR', 'insertLink', 'insertImage', 'insertVideo', 'insertFile', 'insertTable', '|', 'undo', 'redo', 'clearFormatting', 'selectAll', 'html', 'applyFormat', 'removeFormat', 'fullscreen', 'print', 'help'],
            toolbarButtons: ['bold', 'italic', 'underline', 'formatUL'],
            pluginsEnabled: null
        });
    }

    $("#page_content").find('#btnAddComment').show();
    $("#page_content").find('#btnEditComment').hide();
});

$("#page_content").on('click', '#btnAddComment', function () {
    var data = $("#page_content").find('#MeEmployeePlanner_SickLeaveBody').find('div#froala-editor').froalaEditor('html.get');
    var isEmpty = $("#page_content").find("#CommentList").html().trim();
    var currentTime = getCurrentDateTime();
    if (isEmpty == "") {
        var appendDataString = '<div class="seccomments row" id="comment_1" data-commentid="0"><div class="col-lg-10"><p><span class="black postedby">' + constantPlanner.userFullName + '</span> - <span class="commentTime">' + currentTime + '</sapn></p><div class="sickComments">' + data + '</div></div><div class="col-lg-2" style=""><i class="fa fa-pencil editComment"></i><i class="fa fa-trash-o deleteComment"></i></div></div>';
        $("#page_content").find("#CommentList").html(appendDataString);
    }
    else {
        var lastCommentid = $("#page_content").find(".seccomments:first").attr('id').split('_')[1];
        var newId = parseInt(lastCommentid) + 1;
        var appendDataString = '<div class="seccomments row" id="comment_' + newId + '" data-commentid="0"><div class="col-lg-10"><p><span class="black postedby">' + constantPlanner.userFullName + '</span> - <span class="commentTime">' + currentTime + '</sapn></p><div class="sickComments">' + data + '</div></div><div class="col-lg-2" style=""><i class="fa fa-pencil editComment"></i><i class="fa fa-trash-o deleteComment"></i></div></div>';
        $("#page_content").find("#CommentList").prepend(appendDataString);
    }

    if ($('div#froala-editor').data('froala.editor')) {
        $('div#froala-editor').froalaEditor('destroy');
        $('div#froala-editor').html('');
    }
    if (!$('div#froala-editor').data('froala.editor')) {
        $('div#froala-editor').froalaEditor({
            //toolbarButtons: ['bold', 'italic', 'underline', 'strikeThrough', 'subscript', 'superscript', 'fontFamily', 'fontSize', '|', 'specialCharacters', 'color', 'emoticons', 'inlineStyle', 'paragraphStyle', '|', 'paragraphFormat', 'align', 'formatOL', 'formatUL', 'outdent', 'indent', '-', 'quote', 'insertHR', 'insertLink', 'insertImage', 'insertVideo', 'insertFile', 'insertTable', '|', 'undo', 'redo', 'clearFormatting', 'selectAll', 'html', 'applyFormat', 'removeFormat', 'fullscreen', 'print', 'help'],
            toolbarButtons: ['bold', 'italic', 'underline', 'formatUL'],
            pluginsEnabled: null
        });
    }
});

$("#page_content").on('click', '.file-deleteicon', function () {
    $(this).parent().remove();
});

$("#page_content").on('change', '#fileToUpload', function (e) {
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
                    url: constantPlanner.ImageData_SickLeave,
                    contentType: false,
                    processData: false,
                    data: imageData,
                    success: function (result) {
                        var string = '<div class="row ListData"><div class="col-lg-1 icon"><i class="fa fa-paperclip"></i></div><div class="col-lg-10 attach-disc"><label class="fileName" data-newFileName="' + result.NewFileName + '">' + result.originalFileName + '</label><br /><input type="text" class="ImageDescription form-control" placeholder="Add Description Here"/></div><div class="col-lg-1 file-deleteicon"><i class="fa fa-trash-o"></i></div></div>';
                        var isEmpty = $("#page_content").find("#filesList").html();
                        if (isEmpty = "") {
                            $("#page_content").find("#filesList").html(string);
                        }
                        else {
                            $("#page_content").find("#filesList").append(string);
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

//Work - Pattern

$("#page_content").on('click', '.Close_work', function () {
    var value = $("#page_content").find("#Employee_WorkPatternID").val();
    $("#page_content").find("#drp_DefaultWorkPattern").val(value);

});

$("#page_content").on('click', '.WorkPatternHistory', function () {
    $(".hrtoolLoader").show();
    var EmployeeId = $("#currentEmployeeId").val();
    $.ajax({
        url: constantPlanner.workPattenHistory,
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
    var value = $(this).val();
    if (value == "AddNew_WorkPattern") {
        $.ajax({
            url: constantPlanner.workPatten,
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
            url: constantPlanner.workPatten,
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
                url: constantPlanner.TrueIsRotating,
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
                url: constantPlanner.FalseIsRotating,
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
            url: constantPlanner.TrueIsRotating,
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
//btn-submit-DefaultWorkPattern
$("#page_content").on('click', '#btn-submit-DefaultWorkPattern', function (e) {
    $(".hrtoolLoader").show();
    var IsError = false;
    debugger;
    var Id = $("#page_content").find('#DefaultWorkPatternBody').find("#WorkPattenId").val();
    var Name = $("#page_content").find('#DefaultWorkPatternBody').find("#txt_Name").val();
    if (Name == "") {
        isError = true;
        if (Name == "") {
            $("#DefaultWorkPatternBody").find("#lbl-error-workpatten").show();
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
        }
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
                url: constantPlanner.SaveFalseRoatingData,
                data: { modelString: JSON.stringify(model) },
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
                url: constantPlanner.SaveTrueRoatingData,
                data: { Id: Id, Name: Name, IsRotating: allowRoatating, modelString: JSON.stringify(tableData) },
                type: 'post',
                success: function (data) {
                    window.location.reload();
                }
            });
        }
    }
});

//btn-submit-ReadOnlyWorkPattern
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
            url: constantPlanner.SaveEmployeeWorkPattern,
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

// Maternity Paternity Leave
function validateMatLeaveDate(stDate, edDate)
{
    if (stDate != "" || edDate != "") {

        if (StartDateValidation(stDate, edDate)) {
            $("#page_content").find("#MeEmployeePlanner_MaternityPaternityModel").find("#lbl-error-EndDate").show();
            $("#page_content").find("#MeEmployeePlanner_MaternityPaternityModel").find("#txt_ActualEndDate").val('');
        }
    }
}
$("#page_content").on("click", ".btn_AddEdit_MPLeave", function () {
    $(".hrtoolLoader").show();
    var yearId = $(this).attr("data-yearid");
    var monthId = $(this).attr("data-monthid");
    var dayId = $(this).attr("data-dayid");
    var id = $(this).attr("data-id");
    var EmployeeId = $("#currentEmployeeId").val();
    $.ajax({
        url: constantPlanner.addEdit_MPLeaves,
        data: { Id: id, Date: dayId, Month: monthId, Year: yearId, EmployeeID: EmployeeId },
        success: function (data) {
            $(".accordion_container").find("#MeEmployeePlanner_MaternityPaternityModel").find("#MeEmployeePlanner_MaternityPaternityBody").html("");
            $(".accordion_container").find("#MeEmployeePlanner_MaternityPaternityModel").find("#MeEmployeePlanner_MaternityPaternityBody").html(data);

            $(".accordion_container").find("#MeEmployeePlanner_MaternityPaternityModel").find("#MeEmployeePlanner_MaternityPaternityBody").find('#wizard').smartWizard({
                onLeaveStep: MaternityLeavesStepCallback,
                onFinish: MaternityLeavesFinishCallback
            });

            $.each($(".accordion_container").find("#MeEmployeePlanner_MaternityPaternityModel").find("#MeEmployeePlanner_MaternityPaternityBody").find('.MPComments'), function () {
                var uneacapeString = $(this).attr("data-commentstring");
                $(this).html(uneacapeString);
            });

            $(".accordion_container").find("#MeEmployeePlanner_MaternityPaternityModel").find("#MeEmployeePlanner_MaternityPaternityBody").find('.buttonNext').addClass('btn btn-warning');
            $(".accordion_container").find("#MeEmployeePlanner_MaternityPaternityModel").find("#MeEmployeePlanner_MaternityPaternityBody").find('.buttonPrevious').addClass('btn btn-warning');
            $(".accordion_container").find("#MeEmployeePlanner_MaternityPaternityModel").find("#MeEmployeePlanner_MaternityPaternityBody").find('.buttonFinish').addClass('btn btn-success');

            $(".accordion_container").find("#MeEmployeePlanner_MaternityPaternityModel").find("#MeEmployeePlanner_MaternityPaternityBody").find('.buttonPrevious').hide();
            $(".accordion_container").find("#MeEmployeePlanner_MaternityPaternityModel").find("#MeEmployeePlanner_MaternityPaternityBody").find('.buttonFinish').hide();

            $(".accordion_container").find("#MeEmployeePlanner_MaternityPaternityModel").find("#MeEmployeePlanner_MaternityPaternityBody").find("#txt_DueDate").Zebra_DatePicker({
                //direction: false,
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {
                    $("#page_content").find("#lbl-error-DueDate").hide();
                    var startdate = $(this).val();
                    var date = startdate.split("-")[1] + "-" + startdate.split("-")[0] + "-" + startdate.split("-")[2]
                    calculateMaternityDateDiff(date);
                    //calculateAnnualDateDiffTravelLeave();

                }
            });

            $(".accordion_container").find("#MeEmployeePlanner_MaternityPaternityModel").find("#MeEmployeePlanner_MaternityPaternityBody").find("#txt_ActualStartDate").Zebra_DatePicker({
                //direction: false,
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {
                    $("#page_content").find("#lbl-error-ActualStartDate").hide();

                }
            });

            $(".accordion_container").find("#MeEmployeePlanner_MaternityPaternityModel").find("#MeEmployeePlanner_MaternityPaternityBody").find("#txt_ActualEndDate").Zebra_DatePicker({
                //direction: false,
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {
                    $("#page_content").find("#lbl-error-ActualEndDate").hide();
                    $("#page_content").find("#MeEmployeePlanner_MaternityPaternityModel").find("#lbl-error-EndDate").hide();                    
                    var stDate = $("#txt_ActualStartDate").val();
                    var edDate = $("#txt_ActualEndDate").val();
                    validateMatLeaveDate(stDate,edDate);

                }
            });

            $(".accordion_container").find("#MeEmployeePlanner_MaternityPaternityModel").find("#MeEmployeePlanner_MaternityPaternityBody").find('div#froala-editor').froalaEditor({
                //toolbarButtons: ['bold', 'italic', 'underline', 'strikeThrough', 'subscript', 'superscript', 'fontFamily', 'fontSize', '|', 'specialCharacters', 'color', 'emoticons', 'inlineStyle', 'paragraphStyle', '|', 'paragraphFormat', 'align', 'formatOL', 'formatUL', 'outdent', 'indent', '-', 'quote', 'insertHR', 'insertLink', 'insertImage', 'insertVideo', 'insertFile', 'insertTable', '|', 'undo', 'redo', 'clearFormatting', 'selectAll', 'html', 'applyFormat', 'removeFormat', 'fullscreen', 'print', 'help'],
                toolbarButtons: ['bold', 'italic', 'underline', 'formatUL'],
                pluginsEnabled: null
            });

            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
        }
    });
});

$("#page_content").on('click', '.MPdeleteComment', function () {
    $(this).parent().parent().remove();
});

$("#page_content").on('click', '.MPeditComment', function () {
    $("#page_content").find('#MPbtnAddComment').hide();
    $("#page_content").find('#MPbtnEditComment').show();
    $("#page_content").find('#MPbtnEditComment').attr("data-editId", $(this).parent().parent().attr("id"));
    var htmlString = $(this).parent().parent().find(".MPComments").html();
    $("#page_content").find('#MeEmployeePlanner_MaternityPaternityBody').find('div#froala-editor').froalaEditor('html.set', htmlString);
});

$("#page_content").on('click', '#MPbtnEditComment', function () {
    var editDiv = $(this).attr("data-editId");
    var data = $("#page_content").find('#MeEmployeePlanner_MaternityPaternityBody').find('div#froala-editor').froalaEditor('html.get');
    $("#page_content").find("#MPCommentList").find("#" + editDiv).find(".MPComments").html("");
    $("#page_content").find("#MPCommentList").find("#" + editDiv).find(".MPComments").html(data);

    if ($('div#froala-editor').data('froala.editor')) {
        $('div#froala-editor').froalaEditor('destroy');
        $('div#froala-editor').html('');
    }
    if (!$('div#froala-editor').data('froala.editor')) {
        $('div#froala-editor').froalaEditor({
            //toolbarButtons: ['bold', 'italic', 'underline', 'strikeThrough', 'subscript', 'superscript', 'fontFamily', 'fontSize', '|', 'specialCharacters', 'color', 'emoticons', 'inlineStyle', 'paragraphStyle', '|', 'paragraphFormat', 'align', 'formatOL', 'formatUL', 'outdent', 'indent', '-', 'quote', 'insertHR', 'insertLink', 'insertImage', 'insertVideo', 'insertFile', 'insertTable', '|', 'undo', 'redo', 'clearFormatting', 'selectAll', 'html', 'applyFormat', 'removeFormat', 'fullscreen', 'print', 'help'],
            toolbarButtons: ['bold', 'italic', 'underline', 'formatUL'],
            pluginsEnabled: null
        });
    }

    $("#page_content").find('#MPbtnAddComment').show();
    $("#page_content").find('#MPbtnEditComment').hide();
});

$("#page_content").on('click', '#MPbtnAddComment', function () {
    var data = $("#page_content").find('#MeEmployeePlanner_MaternityPaternityBody').find('div#froala-editor').froalaEditor('html.get');
    var isEmpty = $("#page_content").find("#MPCommentList").html().trim();
    var currentTime = getCurrentDateTime();
    if (isEmpty == "") {
        var appendDataString = '<div class="seccomments row" id="comment_1" data-commentid="0"><div class="col-lg-10"><p><span class="black postedby">' + constantPlanner.userFullName + '</span> - <span class="commentTime">' + currentTime + '</sapn></p><div class="MPComments">' + data + '</div></div><div class="col-lg-2" style=""><i class="fa fa-pencil MPeditComment"></i><i class="fa fa-trash-o MPdeleteComment"></i></div></div>';
        $("#page_content").find("#MPCommentList").html(appendDataString);
    }
    else {
        var lastCommentid = $("#page_content").find(".seccomments:first").attr('id').split('_')[1];
        var newId = parseInt(lastCommentid) + 1;
        var appendDataString = '<div class="seccomments row" id="comment_' + newId + '" data-commentid="0"><div class="col-lg-10"><p><span class="black postedby">' + constantPlanner.userFullName + '</span> - <span class="commentTime">' + currentTime + '</sapn></p><div class="MPComments">' + data + '</div></div><div class="col-lg-2" style=""><i class="fa fa-pencil MPeditComment"></i><i class="fa fa-trash-o MPdeleteComment"></i></div></div>';
        $("#page_content").find("#MPCommentList").prepend(appendDataString);
    }

    if ($('div#froala-editor').data('froala.editor')) {
        $('div#froala-editor').froalaEditor('destroy');
        $('div#froala-editor').html('');
    }
    if (!$('div#froala-editor').data('froala.editor')) {
        $('div#froala-editor').froalaEditor({
            //toolbarButtons: ['bold', 'italic', 'underline', 'strikeThrough', 'subscript', 'superscript', 'fontFamily', 'fontSize', '|', 'specialCharacters', 'color', 'emoticons', 'inlineStyle', 'paragraphStyle', '|', 'paragraphFormat', 'align', 'formatOL', 'formatUL', 'outdent', 'indent', '-', 'quote', 'insertHR', 'insertLink', 'insertImage', 'insertVideo', 'insertFile', 'insertTable', '|', 'undo', 'redo', 'clearFormatting', 'selectAll', 'html', 'applyFormat', 'removeFormat', 'fullscreen', 'print', 'help'],
            toolbarButtons: ['bold', 'italic', 'underline', 'formatUL'],
            pluginsEnabled: null
        });
    }
});

$("#page_content").on('click', '.MPfile-deleteicon', function () {
    $(this).parent().remove();
});

$("#page_content").on('change', '#MPfileToUpload', function (e) {
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
                    url: constantPlanner.ImageData_MPLeave,
                    contentType: false,
                    processData: false,
                    data: imageData,
                    success: function (result) {
                        var string = '<div class="row ListData"><div class="col-lg-1 icon"><i class="fa fa-paperclip"></i></div><div class="col-lg-10 attach-disc"><label class="fileName" data-newFileName="' + result.NewFileName + '">' + result.originalFileName + '</label><br /><input type="text" class="ImageDescription form-control" placeholder="Add Description Here"/></div><div class="col-lg-1 file-deleteicon"><i class="fa fa-trash-o"></i></div></div>';
                        var isEmpty = $("#page_content").find("#filesList").html();
                        if (isEmpty = "") {
                            $("#page_content").find("#MPfilesList").html(string);
                        }
                        else {
                            $("#page_content").find("#MPfilesList").append(string);
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

function calculateMaternityDateDiff(date) {

    date = new Date(date);

    var thisWeekStartDay = date.getDate() - date.getDay();

    var thisWeekStart = new Date(date.setDate(thisWeekStartDay));
    $("#page_content").find("#MeEmployeePlanner_MaternityPaternityBody").find("#txt_ExptectedBirthWeekStartDate").val(formatDate(thisWeekStart));
    var thisWeekEnd = new Date(date.setDate(thisWeekStartDay + 6));
    $("#page_content").find("#MeEmployeePlanner_MaternityPaternityBody").find("#txt_ExptectedBirthWeekEndDate").val(formatDate(thisWeekEnd));

    var earliestDays = new Date(date.setDate(thisWeekStartDay - (11 * 7)));
    $("#page_content").find("#MeEmployeePlanner_MaternityPaternityBody").find("#txt_EarliestBirthWeekStartDate").val(formatDate(earliestDays));
    $("#page_content").find("#MeEmployeePlanner_MaternityPaternityBody").find("#txt_OrdinaryMaternityLeaveStartDate").val(formatDate(thisWeekStart));
    //var ordinaryStartdate = new Date(date.setDate(thisWeekStart));

    var ordinaryenddate = new Date(thisWeekStart.setDate(thisWeekStartDay + (26 * 7)));
    $("#page_content").find("#MeEmployeePlanner_MaternityPaternityBody").find("#txt_OrdinaryMaternityLeaveEndDate").val(formatDate(ordinaryenddate));

    var nextday = ordinaryenddate.getDate() - ordinaryenddate.getDay();

    var additionStartdays = new Date(ordinaryenddate.setDate(nextday + 1));
    $("#page_content").find("#MeEmployeePlanner_MaternityPaternityBody").find("#txt_AdditionalMaternityLeaveStartDate").val(formatDate(additionStartdays));

    var additionEnddate = new Date(ordinaryenddate.setDate(thisWeekStartDay + (26 * 7)));

    $("#page_content").find("#MeEmployeePlanner_MaternityPaternityBody").find("#txt_AdditionalMaternityLeaveEndDate").val(formatDate(additionEnddate));
}

function MaternityLeavesStepCallback(obj, context) {
    debugger;
    if (context.fromStep == 1) {
        var isError = false;

        var StartDate = $("#page_content").find("#MeEmployeePlanner_MaternityPaternityBody").find("#txt_DueDate").val();
        var ActualStartDate = $("#page_content").find("#MeEmployeePlanner_MaternityPaternityBody").find("#txt_ActualStartDate").val();
        var ActualEndDate = $("#page_content").find("#MeEmployeePlanner_MaternityPaternityBody").find("#txt_ActualEndDate").val();

        if (StartDate == "") { isError = true; $("#page_content").find("#lbl-error-DueDate").show(); }
        if (ActualStartDate == "") { isError = true; $("#page_content").find("#lbl-error-ActualStartDate").show(); }
        if (ActualEndDate == "") { isError = true; $("#page_content").find("#lbl-error-ActualEndDate").show(); }


        if (isError) {
            return false;
        }
        else {
            if (context.toStep == 2) {
                $(".accordion_container").find("#MeEmployeePlanner_MaternityPaternityModel").find("#MeEmployeePlanner_MaternityPaternityBody").find('.buttonNext').show();
                $(".accordion_container").find("#MeEmployeePlanner_MaternityPaternityModel").find("#MeEmployeePlanner_MaternityPaternityBody").find('.buttonPrevious').show();
                $(".accordion_container").find("#MeEmployeePlanner_MaternityPaternityModel").find("#MeEmployeePlanner_MaternityPaternityBody").find('.buttonFinish').hide();
            }
            else {
                $(".accordion_container").find("#MeEmployeePlanner_MaternityPaternityModel").find("#MeEmployeePlanner_MaternityPaternityBody").find('.buttonNext').show();
                $(".accordion_container").find("#MeEmployeePlanner_MaternityPaternityModel").find("#MeEmployeePlanner_MaternityPaternityBody").find('.buttonPrevious').hide();
                $(".accordion_container").find("#MeEmployeePlanner_MaternityPaternityModel").find("#MeEmployeePlanner_MaternityPaternityBody").find('.buttonFinish').hide();

            }

            return true;
        }
    }
    if (context.fromStep == 2) {
        //var isError = false;


        //if (isError) {
        //    return false;
        //}
        //else {
        if (context.toStep == 1) {
            $(".accordion_container").find("#MeEmployeePlanner_MaternityPaternityModel").find("#MeEmployeePlanner_MaternityPaternityBody").find('.buttonNext').show();
            $(".accordion_container").find("#MeEmployeePlanner_MaternityPaternityModel").find("#MeEmployeePlanner_MaternityPaternityBody").find('.buttonPrevious').hide();
            $(".accordion_container").find("#MeEmployeePlanner_MaternityPaternityModel").find("#MeEmployeePlanner_MaternityPaternityBody").find('.buttonFinish').hide();
        }
        else {
            $(".accordion_container").find("#MeEmployeePlanner_MaternityPaternityModel").find("#MeEmployeePlanner_MaternityPaternityBody").find('.buttonNext').hide();
            $(".accordion_container").find("#MeEmployeePlanner_MaternityPaternityModel").find("#MeEmployeePlanner_MaternityPaternityBody").find('.buttonPrevious').show();
            $(".accordion_container").find("#MeEmployeePlanner_MaternityPaternityModel").find("#MeEmployeePlanner_MaternityPaternityBody").find('.buttonFinish').show();

        }
        return true;
        //  }
    }

    else {
        if (context.toStep == 2) {
            $(".accordion_container").find("#MeEmployeePlanner_MaternityPaternityModel").find("#MeEmployeePlanner_MaternityPaternityBody").find('.buttonNext').show();
            $(".accordion_container").find("#MeEmployeePlanner_MaternityPaternityModel").find("#MeEmployeePlanner_MaternityPaternityBody").find('.buttonPrevious').show();
            $(".accordion_container").find("#MeEmployeePlanner_MaternityPaternityModel").find("#MeEmployeePlanner_MaternityPaternityBody").find('.buttonFinish').hide();
        }
        else {
            $(".accordion_container").find("#MeEmployeePlanner_MaternityPaternityModel").find("#MeEmployeePlanner_MaternityPaternityBody").find('.buttonNext').hide();
            $(".accordion_container").find("#MeEmployeePlanner_MaternityPaternityModel").find("#MeEmployeePlanner_MaternityPaternityBody").find('.buttonPrevious').show();
            $(".accordion_container").find("#MeEmployeePlanner_MaternityPaternityModel").find("#MeEmployeePlanner_MaternityPaternityBody").find('.buttonFinish').show();
        }

        return true;
    }

}

function MaternityLeavesFinishCallback() {
    $(".hrtoolLoader").show();
    var Id = $("#page_content").find("#MeEmployeePlanner_MaternityPaternityBody").find("#MaternityPaternityID").val();
    var yearid = $(".accordion_container").find("#MeEmployeePlanner_MaternityPaternityBody").find("#HiddenYearId").val();
    var monthid = $(".accordion_container").find("#MeEmployeePlanner_MaternityPaternityBody").find("#HiddenMonthId").val();
    var dayid = $(".accordion_container").find("#MeEmployeePlanner_MaternityPaternityBody").find("#HiddendayId").val();
    var DueDate = $("#page_content").find("#MeEmployeePlanner_MaternityPaternityBody").find("#txt_DueDate").val();
    var ActualStartDate = $("#page_content").find("#MeEmployeePlanner_MaternityPaternityBody").find("#txt_ActualStartDate").val();
    var ActualEndDate = $("#page_content").find("#MeEmployeePlanner_MaternityPaternityBody").find("#txt_ActualEndDate").val();
    var EarliestBirthWeekStartDate = $("#page_content").find("#MeEmployeePlanner_MaternityPaternityBody").find("#txt_EarliestBirthWeekStartDate").val();
    var ExptectedBirthWeekEndDate = $("#page_content").find("#MeEmployeePlanner_MaternityPaternityBody").find("#txt_ExptectedBirthWeekEndDate").val();
    var ExptectedBirthWeekStartDate = $("#page_content").find("#MeEmployeePlanner_MaternityPaternityBody").find("#txt_ExptectedBirthWeekStartDate").val();
    var OrdinaryMaternityLeaveStartDate = $("#page_content").find("#MeEmployeePlanner_MaternityPaternityBody").find("#txt_OrdinaryMaternityLeaveStartDate").val();
    var OrdinaryMaternityLeaveEndDate = $("#page_content").find("#MeEmployeePlanner_MaternityPaternityBody").find("#txt_OrdinaryMaternityLeaveEndDate").val();
    var AdditionalMaternityLeaveStartDate = $("#page_content").find("#MeEmployeePlanner_MaternityPaternityBody").find("#txt_AdditionalMaternityLeaveStartDate").val();
    var AdditionalMaternityLeaveEndDate = $("#page_content").find("#MeEmployeePlanner_MaternityPaternityBody").find("#txt_AdditionalMaternityLeaveEndDate").val();
    var Lengthofemployment = $("#page_content").find("#MeEmployeePlanner_MaternityPaternityBody").find("#txt_Lengthofemployment").val();
    //Step 2
    var commentList = [];
    $.each($(".accordion_container").find('#MPCommentList').find(".seccomments"), function () {
        var commentBy = $(this).find(".postedby").html().trim();
        var comment = $(this).find(".MPComments").html().trim();
        var commentTime = $(this).find(".commentTime").html().trim();
        var newReord = {
            commentBy: commentBy,
            comment: comment,
            commentTime: commentTime
        }
        commentList.push(newReord);
    });
    var JsoncommentListJoinString = JSON.stringify(commentList);

    //Step 3
    var documentList = [];
    $.each($(".accordion_container").find('#MPfilesList').find(".ListData"), function () {
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
        EmployeeId: $("#currentEmployeeId").val(),
        yearId: yearid,
        monthId: monthid,
        day: dayid,
        DueDate: DueDate,
        Lengthofemployment: Lengthofemployment,
        ExptectedBirthWeekStartDate: ExptectedBirthWeekStartDate,
        ExptectedBirthWeekEndDate: ExptectedBirthWeekEndDate,
        OrdinaryMaternityLeaveStartDate: OrdinaryMaternityLeaveStartDate,
        OrdinaryMaternityLeaveEndDate: OrdinaryMaternityLeaveEndDate,
        AdditionalMaternityLeaveStartDate: AdditionalMaternityLeaveStartDate,
        AdditionalMaternityLeaveEndDate: AdditionalMaternityLeaveEndDate,
        EarliestBirthWeekStartDate: EarliestBirthWeekStartDate,
        ActualStartDate: ActualStartDate,
        ActualEndDate: ActualEndDate,
        CommentListString: JsoncommentListJoinString,
        DocumentListString: JsondocumentListJoinString,
        HolidayCountryID: $("#drp-publicHolidayCountry").val()
    }

    $.ajax({
        url: constantPlanner.saveData_MaternityPaternity,
        type: 'POST',
        data: JSON.stringify(model),
        contentType: "application/json",
        success: function (data) {
            $(".accordion_container").find("#fixedrightcolumn_" + monthid).html("");
            $(".accordion_container").find("#fixedrightcolumn_" + monthid).html(data);

            $(".accordion_container").find("#MeEmployeePlanner_MaternityPaternityModel").find('.close').click();



            if (Id > 0) {
                $(".toast-info").show();
                setTimeout(function () { $(".toast-info").hide(); }, 1500);
            }
            else {
                $(".toast-success").show();
                setTimeout(function () { $(".toast-success").hide(); }, 1500);
            }
            window.location.reload();
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();

        }
    });

}


// Print Pdf
function showPrintOption() {

    $(".hrtoolLoader").show();
    var EmployeeId = $("#currentEmployeeId").val();
    $.ajax({
        url: constantPlanner.AddAbsencePdfView,
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
        $("#page_content").find("#txt_Duration").val('');
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

        window.location.href = constantPlanner.PrintPdf + '?EmployeeId=' + EmployeeId + '&yearId=' + yearid + '&monthId=' + monthid + '&day=' + dayid + '&StartDate=' + StartDate + '&EndDate=' + EndDate + '&Absence=' + Absence + '&Holidays=' + Holidays;
        setTimeout(function () {

            $("#page_content").find("#PrintPdfModel").find('.close').click();
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
        }, 5000);


        //"@Url.Action("PrintPdfAbsenceLeaves", "EmployeePlanner", new { EmployeeId = Model.EmployeeId, yearId = Model.yearId, monthId = Model.monthId, day = Model.day, StartDate = Model.StartDate.Trim(), EndDate = Model.EndDate.Trim(), Absence = Model.Absence, Holidays=Model.Holidays})"
        //$.ajax({
        //    url: constantPlanner.PrintPdf,
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