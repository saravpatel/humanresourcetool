$(document).ready(function () {
    $(function () {
        $('[data-toggle="tooltip"]').tooltip()
    })
});

$("body").on('keyup', '.form-control', function () {
    $(this).parent().parent().find('.field-validation-error').hide();
});

$("body").on('change', '.form-control', function () {
    var currentValue = $(this).val();
    if (currentValue != "0") {
        $(this).parent().parent().find(".field-validation-error").hide();
    }
});

function getCurrentDateTime() {
    var dt = new Date();
    var time = dt.getDay() + "/" + dt.getMonth() + "/" + dt.getFullYear() + " " + dt.getHours() + ":" + dt.getMinutes() + ":" + dt.getSeconds();
    return time;
}

function isValidEmailAddress(emailAddress) {
    var pattern = /^([a-z\d!#$%&'*+\-\/=?^_`{|}~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]+(\.[a-z\d!#$%&'*+\-\/=?^_`{|}~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]+)*|"((([ \t]*\r\n)?[ \t]+)?([\x01-\x08\x0b\x0c\x0e-\x1f\x7f\x21\x23-\x5b\x5d-\x7e\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|\\[\x01-\x09\x0b\x0c\x0d-\x7f\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))*(([ \t]*\r\n)?[ \t]+)?")@(([a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|[a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF][a-z\d\-._~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]*[a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])\.)+([a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|[a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF][a-z\d\-._~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]*[a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])\.?$/i;
    return pattern.test(emailAddress);
};

function FromDateValidation(FromDate, ToDate) {
    var isError = false;
    var splitFromDate = FromDate.split('/');
    var splitToDate = ToDate.split('/');
    if (parseInt(splitFromDate[2]) > parseInt(splitToDate[2])) {
        isError = true;
        return isError;
    }
    else if (parseInt(splitFromDate[2]) == parseInt(splitToDate[2])) {
        if (parseInt(splitFromDate[1]) > parseInt(splitToDate[1])) {
            isError = true;
            return isError;
        }
        else if (parseInt(splitFromDate[1]) == parseInt(splitToDate[1])) {
            if (parseInt(splitFromDate[0]) > parseInt(splitToDate[0])) {
                isError = true;
                return isError;
            }
            else {
                return isError;
            }
        }
        else {
            return isError;
        }
    }
    else {
        return isError;
    }
}

function ToDateValidation(FromDate, ToDate) {
    var isError = false;
    var splitFromDate = FromDate.split('/');
    var splitToDate = ToDate.split('/');
    if (parseInt(splitToDate[2]) < parseInt(splitFromDate[2])) {
        isError = true;
        return isError;
    }
    else if (parseInt(splitToDate[2]) == parseInt(splitFromDate[2])) {
        if (parseInt(splitToDate[1]) < parseInt(splitFromDate[1])) {
            isError = true;
            return isError;
        }
        else if (parseInt(splitToDate[1]) == parseInt(splitFromDate[1])) {
            if (parseInt(splitToDate[0]) < parseInt(splitFromDate[0])) {
                isError = true;
                return isError;
            }
            else {
                return isError;
            }
        }
        else {
            return isError;
        }
    }
    else {
        return isError;
    }
}

$("#sidebar_main_toggle").click(function () {
    //alert($(this).find('span').css('transform'));
    var isDisplay = $("#sidebar_main").css('display');
    if (isDisplay == "block") {
        $("#sidebar_main").css('display', 'none');
        $("#header_main").css('margin-left', '0px');
        $("#page_content").css('margin-left', '0px');
        $('.schedulingMonth').addClass('big-calender-span-big').removeClass('big-calender-span');
        $('.travelMonth').addClass('big-calender-span-big').removeClass('big-calender-span');
        $('.timesheetMonth').addClass('big-calender-span-big').removeClass('big-calender-span');        
        $('.upliftMonth').addClass('big-calender-span-big').removeClass('big-calender-span');        
        $('.plannerTravelMonth').addClass('big-calender-span-big').removeClass('big-calender-span');        
        $('.plannerTimesheetMonth').addClass('big-calender-span-big').removeClass('big-calender-span');
        $(this).find('.sSwitchIcon').css('transform', 'rotate(-180deg)')

    }
    else {
        $("#sidebar_main").css('display', 'block');
        $("#header_main").css('margin-left', '190px');
        $("#page_content").css('margin-left', '190px');
        $('.schedulingMonth').addClass('big-calender-span').removeClass('big-calender-span-big');
        $('.travelMonth').addClass('big-calender-span').removeClass('big-calender-span-big');
        $('.timesheetMonth').addClass('big-calender-span').removeClass('big-calender-span-big');
        $('.upliftMonth').addClass('big-calender-span').removeClass('big-calender-span-big');
        $('.plannerTravelMonth').addClass('big-calender-span').removeClass('big-calender-span-big');        
        $('.plannerTimesheetMonth').addClass('big-calender-span').removeClass('big-calender-span-big');
        $(this).find('.sSwitchIcon').css('transform', 'rotate(0)')


    }
})

function numberWithCommas(x) {
    var parts = x.toString().split(".");
    parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    return parts.join(".");
}

//date formate
function GetFormattedDate() {
    var todayTime = new Date();
    var month = (todayTime.getMonth() + 1);
    var day = (todayTime.getDate());
    var year = (todayTime.getFullYear());
    return day + "/" + month + "/" + year;
}

function checkUrl(url) {
    //regular expression for URL
    var pattern = /^(http|https)?:\/\/[a-zA-Z0-9-\.]+\.[a-z]{2,4}/;

    if (pattern.test(url)) {
        return true;
    } else {
        return false;
    }
}

function ToDateValidation(FromDate, ToDate) {
    var isError = false;
    var splitFromDate = FromDate.split('/');
    var splitToDate = ToDate.split('/');
    if (parseInt(splitToDate[2]) < parseInt(splitFromDate[2])) {
        isError = true;
        return isError;
    }
    else if (parseInt(splitToDate[2]) == parseInt(splitFromDate[2])) {
        if (parseInt(splitToDate[1]) < parseInt(splitFromDate[1])) {
            isError = true;
            return isError;
        }
        else if (parseInt(splitToDate[1]) == parseInt(splitFromDate[1])) {
            if (parseInt(splitToDate[0]) < parseInt(splitFromDate[0])) {
                isError = true;
                return isError;
            }
            else {
                return isError;
            }
        }
        else {
            return isError;
        }
    }
    else {
        return isError;
    }
}

function FromDateValidation(FromDate, ToDate) {
    var isError = false;
    var splitFromDate = FromDate.split('/');
    var splitToDate = ToDate.split('/');
    if (parseInt(splitFromDate[2]) > parseInt(splitToDate[2])) {
        isError = true;
        return isError;
    }
    else if (parseInt(splitFromDate[2]) == parseInt(splitToDate[2])) {
        if (parseInt(splitFromDate[1]) > parseInt(splitToDate[1])) {
            isError = true;
            return isError;
        }
        else if (parseInt(splitFromDate[1]) == parseInt(splitToDate[1])) {
            if (parseInt(splitFromDate[0]) > parseInt(splitToDate[0])) {
                isError = true;
                return isError;
            }
            else {
                return isError;
            }
        }
        else {
            return isError;
        }
    }
    else {
        return isError;
    }
}

function StartDateValidation(FromDate, ToDate) {
    var isError = false;
    var splitFromDate = FromDate.split('-');
    var splitToDate = ToDate.split('-');
    if (parseInt(splitFromDate[2]) > parseInt(splitToDate[2])) {
        isError = true;
        return isError;
    }
    else if (parseInt(splitFromDate[2]) == parseInt(splitToDate[2])) {
        if (parseInt(splitFromDate[1]) > parseInt(splitToDate[1])) {
            isError = true;
            return isError;
        }
        else if (parseInt(splitFromDate[1]) == parseInt(splitToDate[1])) {
            if (parseInt(splitFromDate[0]) > parseInt(splitToDate[0])) {
                isError = true;
                return isError;
            }
            else {
                return isError;
            }
        }
        else {
            return isError;
        }
    }
    else {
        return isError;
    }
}

function DaysCount(stratDate, endDate)
{
    var splitStartDate = stratDate.split('-');
    var splitEndDate = endDate.split('-');
    var oneDay = 24 * 60 * 60 * 1000;
    var firstDate = new Date(splitStartDate[2], splitStartDate[1], splitStartDate[0]);
    var secondDate = new Date(splitEndDate[2], splitEndDate[1], splitEndDate[0]);
    var diffDays = Math.round(Math.abs((firstDate.getTime() - secondDate.getTime()) / (oneDay)));
    diffDays = diffDays + 1;
    return diffDays;
}

function formatDate(date) {
    var d = new Date(date),
        month = '' + (d.getMonth() + 1),
        day = '' + d.getDate(),
        year = d.getFullYear();

    if (month.length < 2) month = '0' + month;
    if (day.length < 2) day = '0' + day;

    return [day, month, year].join('-');
}