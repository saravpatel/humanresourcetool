
$("#drp_DefaultPublicHolidayTemplate").change(function () {
    $("#page_content").find("#lbl-error-DefaultPublicHoliday").hide();
    $("#lbl-error-ValidDefaultPublicHoliday").hide();    
    var value = $(this).val();
    if (value == "AddNew_DefaultPublicHolidayTemplate") {
        $("#page_content").find("#EditPublicHoliday").hide();
        $.ajax({
            url: constantHolidaySetting.publicHoliday,
            data: { Id: 0 },
            success: function (data) {

                $('#DefaultPublicHolidayTemplateModal').modal('show');
                $('#DefaultPublicHolidayTemplateBody').html(data);
                $('#DefaultPublicHolidayTemplateBody').find('#publiHolidayList').DataTable({
                    bFilter: false,
                    bInfo: false,
                    dom: 'frtlip'
                });
            }
        });
    }
    if (value == "0") {
        $("#page_content").find("#EditPublicHoliday").hide();

    }
    else
    {
        $("#page_content").find("#EditPublicHoliday").show();
    }

});

$('#DefaultPublicHolidayTemplateBody').on('click', '.btn-Add-newHoliday', function () {

    var countryId = $('#DefaultPublicHolidayTemplateBody').find("#countryId").val();

    var HtmlString = '<tr class="dataTr newTr" id=""><td style="width:35%;"><input type="text" class="form-control HolidayName" placeholder="Holiday Name" id="HolidayName_" value=""/></td>';
    HtmlString += '<td style="width:35%;"><input type="text" class="form-control HolidayDate" placeholder="Holiday Date" id="HolidayDate_"/></td>';
    HtmlString += '<td style="width:30%;"><button class="btn btn-danger deleteTr">Delete</button></td></tr>';

    if ($('#DefaultPublicHolidayTemplateBody').find('#publiHolidayList tbody').find(".dataTr").length > 0) {
        $('#DefaultPublicHolidayTemplateBody').find('#publiHolidayList tbody').find(".dataTr:last").after(HtmlString);
    }
    else {
        $('#DefaultPublicHolidayTemplateBody').find('#publiHolidayList tbody').html("");
        $('#DefaultPublicHolidayTemplateBody').find('#publiHolidayList tbody').html(HtmlString);
    }
    $('#DefaultPublicHolidayTemplateBody').find('#publiHolidayList tbody').find(".newTr:last").find(".HolidayDate").Zebra_DatePicker({
        format: 'd-m-Y'
    });
});

$('#DefaultPublicHolidayTemplateBody').on('click', '.deleteTr', function () {
    if ($(this).parent().parent().hasClass('newTr')) {
        $(this).parent().parent().remove();
    }
    else {
        var Id = $(this).attr('id');
        $.ajax({
            url: constantHolidaySetting.deleteHoliday,
            data: { Id: Id },
            success: function (data) {
                $(this).parent().parent().remove();
            }
        });
    }
});

$('#DefaultPublicHolidayTemplateModal').on('click', '#btn-submit-DefaultPublicHolidayTemplate', function () {
    var isError = false;
    var Id = $('#DefaultPublicHolidayTemplateModal').find("#countryId").val();
    var countryName = $('#DefaultPublicHolidayTemplateModal').find("#txt_PublicHoidayCountryName").val().trim();
    if (countryName == "") {
        isError = true;
    }
    var holidayData = [];
    $.each($('#DefaultPublicHolidayTemplateBody').find('#publiHolidayList tbody').find(".newTr"), function () {
        var holidayName = $(this).find(".HolidayName").val().trim();
        var holidayDate = $(this).find(".HolidayDate").val().trim();
        if (holidayName != "" && holidayDate != "") {
            var newReord = {
                Name: holidayName,
                Date: holidayDate
            }
            holidayData.push(newReord);
        }
    });

    if (isError) {
        return false;
    }
    else {
        $.ajax({
            url: constantHolidaySetting.savePublicHoliday,
            data: { CountryId: Id, CountryName: countryName, HolidayData: JSON.stringify(holidayData) },
            success: function (data) {
                window.location.reload();
            }
        });
    }
});

$(".DefaultPublicHolidayTemplate_Edit").click(function () {
    var value = $("#drp_DefaultPublicHolidayTemplate").val();
    if (value != "AddNew_DefaultPublicHolidayTemplate" && value != "0") {
        $.ajax({
            url: constantHolidaySetting.publicHoliday,
            data: { Id: value },
            success: function (data) {
                $('#DefaultPublicHolidayTemplateBody').html(data);
                $('#DefaultPublicHolidayTemplateBody').find('#publiHolidayList').DataTable({
                    bFilter: false,
                    bInfo: false,
                    dom: 'frtlip'
                });
                $.each($('#DefaultPublicHolidayTemplateBody').find('.HolidayDate'), function () {
                    $(this).Zebra_DatePicker({
                        format: 'd-m-Y'
                    });
                });
            }
        });
    }
});

$('#DefaultPublicHolidayTemplateModal').on('click', '.btn-update-holiday', function () {
    var Id = $(this).attr('id');
    var name = $('#DefaultPublicHolidayTemplateModal').find("#HolidayName_" + Id).val().trim();
    var date = $('#DefaultPublicHolidayTemplateModal').find("#HolidayDate_" + Id).val().trim();
    $.ajax({
        url: constantHolidaySetting.updateHoliday,
        data: { Id: Id, Name: name, Date: date },
        success: function (data) {
        }
    });
});

//Bradford Factor
$(".btn-info").click(function () {
    $.ajax({
        url: constantHolidaySetting.BradfordFactor,
        success: function (data) {
            $("#BradfordFactorBody").html("");
            $("#BradfordFactorBody").html(data);
        }
    });
});

$("#btn-submit-BradfordFactor").click(function () {
    var Id = $("#BradfordFactorBody").find("#BradfordFactorId").val();
    var LowerValue1 = $("#BradfordFactorBody").find("#LowerValue1").val();
    var UpperValue1 = $("#BradfordFactorBody").find("#UpperValue1").val();
    var Alert1 = $("#BradfordFactorBody").find("#Alert1").val();
    var LowerValue2 = $("#BradfordFactorBody").find("#LowerValue2").val();
    var UpperValue2 = $("#BradfordFactorBody").find("#UpperValue2").val();
    var Alert2 = $("#BradfordFactorBody").find("#Alert2").val();
    var LowerValue3 = $("#BradfordFactorBody").find("#LowerValue3").val();
    var UpperValue3 = $("#BradfordFactorBody").find("#UpperValue3").val();
    var Alert3 = $("#BradfordFactorBody").find("#Alert3").val();
    var LowerValue4 = $("#BradfordFactorBody").find("#LowerValue4").val();
    var UpperValue4 = $("#BradfordFactorBody").find("#UpperValue4").val();
    var Alert4 = $("#BradfordFactorBody").find("#Alert4").val();

    $.ajax({
        url: constantHolidaySetting.SaveBradfordFactor,
        data: { Id: Id, LowerValue1: LowerValue1, UpperValue1: UpperValue1, Alert1: Alert1, LowerValue2: LowerValue2, UpperValue2: UpperValue2, Alert2: Alert2, LowerValue3: LowerValue3, UpperValue3: UpperValue3, Alert3: Alert3, LowerValue4: LowerValue4, UpperValue4: UpperValue4, Alert4: Alert4 },
        success: function (data) {
            window.location.reload();
        }
    });
});

$('#page_content_inner').on('keyup', '#UpperValue1', function () {

    var value = 0;
    value = $(this).val();
    value++;
    $("#BradfordFactorBody").find("#LowerValue2").val(value);
});
$('#page_content_inner').on('keyup', '#UpperValue2', function () {
    var value = 0;
    value = $(this).val();
    value++;
    $("#BradfordFactorBody").find("#LowerValue3").val(value);
});
$('#page_content_inner').on('keyup', '#UpperValue3', function () {
    var value = 0;
    value = $(this).val();
    value++;
    $("#BradfordFactorBody").find("#LowerValue4").val(value);
});

//Work patten

$("#drp_DefaultWorkPattern").change(function () {
    $("#lbl-error-ValidDefaultWorkPattern").hide();
    $("#page_content").find("#lbl-error-DefaultWorkPattern").hide();
    var value = $(this).val();
    if (value == "AddNew_WorkPattern") {
        $("#page_content").find("#EditWorkPattern").hide();
        $.ajax({
            url: constantHolidaySetting.workPatten,
            data: { Id: 0 },
            success: function (data) {
                $('#DefaultWorkPatternModal').modal('show');
                $('#DefaultWorkPatternBody').html('');
                $('#DefaultWorkPatternBody').html(data);

                $('#DefaultWorkPatternBody').find(".timeMask").mask("00:00");
            }
        });
    }
    if (value == "0") {
        $("#page_content").find("#EditWorkPattern").hide();
    }
    else
    {
        $("#page_content").find("#EditWorkPattern").show();
    }
});




$('#DefaultWorkPatternBody').on('change', '#AllowRotating', function () {

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
            //var workPatternId = $('#DefaultWorkPatternBody').find("#WorkPattenId").val();
            var workPatternId = parseInt($(".LastIndex:last").attr('data-name')) + 1;
            if (workPatternId.toString() == "NaN") {
                workPatternId = 0;
            }
            $.ajax({
                url: constantHolidaySetting.TrueIsRotating,
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
                url: constantHolidaySetting.FalseIsRotating,
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
        $.ajax({
            url: constantHolidaySetting.TrueIsRotating,
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

$("#btn-submit-DefaultWorkPattern").click(function () {
    var Id = $('#DefaultWorkPatternBody').find("#WorkPattenId").val();
    var Name = $('#DefaultWorkPatternBody').find("#txt_Name").val();
    var allowRoatating = $('#DefaultWorkPatternBody').find('#AllowRotating').prop('checked');
    var monStart = $('#DefaultWorkPatternBody').find("#FalseIsRotatingDiv").find("#MondayHours").val();
    var monHrStart = monStart.replace(":", ".");
    var monEnd = $('#DefaultWorkPatternBody').find("#FalseIsRotatingDiv").find("#MondayEnd").val();
    var monHrEnd = monEnd.replace(":", ".");
    var tueStart = $('#DefaultWorkPatternBody').find("#FalseIsRotatingDiv").find("#TuesdayStart").val();
    var tueHrStart = tueStart.replace(":", ".");
    var tueEnd = $('#DefaultWorkPatternBody').find("#FalseIsRotatingDiv").find("#TuesdayEnd").val();
    var tueHrEnd = tueEnd.replace(":", ".");
    var WedStart = $('#DefaultWorkPatternBody').find("#FalseIsRotatingDiv").find("#WednessdayStart").val();
    var WedHrstart = WedStart.replace(":", ".");
    var WebEnd = $('#DefaultWorkPatternBody').find("#FalseIsRotatingDiv").find("#WednessdayEnd").val();
    var WedHrEnd = WebEnd.replace(":", ".");
    var ThurStart = $('#DefaultWorkPatternBody').find("#FalseIsRotatingDiv").find("#ThursdayStart").val();
    var ThurHrStart = ThurStart.replace(":", ".");
    var ThurEnd = $('#DefaultWorkPatternBody').find("#FalseIsRotatingDiv").find("#ThursdayEnd").val();
    var ThurHrEnd = ThurEnd.replace(":", ".");
    var FriStart = $('#DefaultWorkPatternBody').find("#FalseIsRotatingDiv").find("#FridayStart").val();
    var FriHrStart = FriStart.replace(":", ".");
    var FriEnd = $('#DefaultWorkPatternBody').find("#FalseIsRotatingDiv").find("#FridayEnd").val();
    var FriHrEnd = FriEnd.replace(":", ".");
    var SatStat = $('#DefaultWorkPatternBody').find("#FalseIsRotatingDiv").find("#SaturdayStart").val();
    var SatHrStrt = SatStat.replace(":", ".");
    var SatEnd = $('#DefaultWorkPatternBody').find("#FalseIsRotatingDiv").find("#SaturdayEnd").val();
    var SatHrEnd = SatEnd.replace(":", ".");
    var SunStar = $('#DefaultWorkPatternBody').find("#FalseIsRotatingDiv").find("#SundayStart").val();
    var SunHrStrt = SunStar.replace(":", ".");
    var sunEnd = $('#DefaultWorkPatternBody').find("#FalseIsRotatingDiv").find("#SundayEnd").val();
    var SunHrEnd = sunEnd.replace(":", ".");
    if (Name == "") {
        IsError = true;
        $("#DefaultWorkPatternBody").find("#lbl-error-WorkpattenName").show();
    }    
    else {
    if (!allowRoatating) {
        var model = {
            Id: Id,
            Name: Name,
            IsRotating: allowRoatating,
            MondayHours: $('#DefaultWorkPatternBody').find("#FalseIsRotatingDiv").find("#MondayHours").val().trim(),
            MondayDays: $('#DefaultWorkPatternBody').find("#FalseIsRotatingDiv").find("#MondayDays").val().trim(),
            MondayStart: monHrStart,
            MondayEnd: monHrEnd,
            MondayBreakMins: $('#DefaultWorkPatternBody').find("#FalseIsRotatingDiv").find("#MondayBreakMins").val().trim(),
            TuesdayHours: $('#DefaultWorkPatternBody').find("#FalseIsRotatingDiv").find("#TuesdayHours").val().trim(),
            TuesdayDays: $('#DefaultWorkPatternBody').find("#FalseIsRotatingDiv").find("#TuesdayDays").val().trim(),
            TuesdayStart: tueHrStart,
            TuesdayEnd: tueHrEnd,
            TuesdayBreakMins: $('#DefaultWorkPatternBody').find("#FalseIsRotatingDiv").find("#TuesdayBreakMins").val().trim(),
            WednessdayHours: $('#DefaultWorkPatternBody').find("#FalseIsRotatingDiv").find("#WednessdayHours").val().trim(),
            WednessdayDays: $('#DefaultWorkPatternBody').find("#FalseIsRotatingDiv").find("#WednessdayDays").val().trim(),
            WednessdayStart: WedHrstart,
            WednessdayEnd: WedHrEnd,
            WednessdayBreakMins: $('#DefaultWorkPatternBody').find("#FalseIsRotatingDiv").find("#WednessdayBreakMins").val().trim(),
            ThursdayHours: $('#DefaultWorkPatternBody').find("#FalseIsRotatingDiv").find("#ThursdayHours").val().trim(),
            ThursdayDays: $('#DefaultWorkPatternBody').find("#FalseIsRotatingDiv").find("#ThursdayDays").val().trim(),
            ThursdayStart: ThurHrStart,
            ThursdayEnd: ThurHrEnd,
            ThursdayBreakMins: $('#DefaultWorkPatternBody').find("#FalseIsRotatingDiv").find("#ThursdayBreakMins").val().trim(),
            FridayHours: $('#DefaultWorkPatternBody').find("#FalseIsRotatingDiv").find("#FridayHours").val().trim(),
            FridayDays: $('#DefaultWorkPatternBody').find("#FalseIsRotatingDiv").find("#FridayDays").val().trim(),
            FridayStart:FriHrStart,
            FridayEnd: FriHrEnd,
            FridayBreakMins: $('#DefaultWorkPatternBody').find("#FalseIsRotatingDiv").find("#FridayBreakMins").val().trim(),
            SaturdayHours: $('#DefaultWorkPatternBody').find("#FalseIsRotatingDiv").find("#SaturdayHours").val().trim(),
            SaturdayDays: $('#DefaultWorkPatternBody').find("#FalseIsRotatingDiv").find("#SaturdayDays").val().trim(),
            SaturdayStart:SatHrStrt ,
            SaturdayEnd: SatHrEnd,
            SaturdayBreakMins: $('#DefaultWorkPatternBody').find("#FalseIsRotatingDiv").find("#SaturdayBreakMins").val().trim(),
            SundayHours: $('#DefaultWorkPatternBody').find("#FalseIsRotatingDiv").find("#SundayHours").val().trim(),
            SundayDays: $('#DefaultWorkPatternBody').find("#FalseIsRotatingDiv").find("#SundayDays").val().trim(),
            SundayStart: SunHrStrt,
            SundayEnd: SunHrEnd,
            SundayBreakMins: $('#DefaultWorkPatternBody').find("#FalseIsRotatingDiv").find("#SundayBreakMins").val().trim(),
        }
        $.ajax({
            url: constantHolidaySetting.SaveFalseRoatingData,
            data: { modelString: JSON.stringify(model) },
            success: function (data) {
                window.location.reload();
            }
        });
    }
    else {
        var tableData = [];
        $.each($('#DefaultWorkPatternBody').find("#TrueIsRotatingDiv").find(".RotatingTable"), function () {
            var monStart = $(this).find("#MondayStart").val();
            var monHrStart = monStart.replace(":", ".");
            var monEnd = $(this).find("#MondayEnd").val().trim();
            var monHrEnd = monEnd.replace(":", ".");
            var tueStart = $(this).find("#TuesdayStart").val().trim();
            var tueHrStart = tueStart.replace(":", ".");
            var tueEnd = $(this).find("#TuesdayEnd").val().trim(); 
            var tueHrEnd = tueEnd.replace(":", ".");
            var WedStart = $(this).find("#WednessdayStart").val();
            var WedHrstart = WedStart.replace(":", ".");
            var WebEnd = $(this).find("#WednessdayEnd").val();
            var WedHrEnd = WebEnd.replace(":", ".");
            var ThurStart = $(this).find("#ThursdayStart").val();
            var ThurHrStart = ThurStart.replace(":", ".");
            var ThurEnd =  $(this).find("#ThursdayEnd").val();
            var ThurHrEnd = ThurEnd.replace(":", ".");
            var FriStart =$(this).find("#FridayStart").val();
            var FriHrStart = FriStart.replace(":", ".");
            var FriEnd =  $(this).find("#FridayEnd").val();
            var FriHrEnd = FriEnd.replace(":", ".");
            var SatStat =$(this).find("#SaturdayStart").val();
            var SatHrStrt = SatStat.replace(":", ".");
            var SatEnd =  $(this).find("#SaturdayEnd").val();
            var SatHrEnd = SatEnd.replace(":", ".");
            var SunStar = $(this).find("#SundayStart").val();
            var SunHrStrt = SunStar.replace(":", ".");
            var sunEnd = $(this).find("#SundayEnd").val();
            var SauHrEnd = sunEnd.replace(":", ".");
            var model = {
                MondayHours: $(this).find("#MondayHours").val().trim(),
                MondayDays: $(this).find("#MondayDays").val().trim(),
                MondayStart: monHrStart,
                MondayEnd: monEnd,
                MondayBreakMins: $(this).find("#MondayBreakMins").val().trim(),
                TuesdayHours: $(this).find("#TuesdayHours").val().trim(),
                TuesdayDays: $(this).find("#TuesdayDays").val().trim(),
                TuesdayStart: tueStart,
                TuesdayEnd:tueHrEnd,
                TuesdayBreakMins: $(this).find("#TuesdayBreakMins").val().trim(),
                WednessdayHours: $(this).find("#WednessdayHours").val().trim(),
                WednessdayDays: $(this).find("#WednessdayDays").val().trim(),
                WednessdayStart: WedHrstart,
                WednessdayEnd: WebEnd,
                WednessdayBreakMins: $(this).find("#WednessdayBreakMins").val().trim(),
                ThursdayHours: $(this).find("#ThursdayHours").val().trim(),
                ThursdayDays: $(this).find("#ThursdayDays").val().trim(),
                ThursdayStart: ThurHrStart,
                ThursdayEnd: ThurHrEnd,
                ThursdayBreakMins: $(this).find("#ThursdayBreakMins").val().trim(),
                FridayHours: $(this).find("#FridayHours").val().trim(),
                FridayDays: $(this).find("#FridayDays").val().trim(),
                FridayStart: FriHrStart,
                FridayEnd: FriHrEnd,
                FridayBreakMins: $(this).find("#FridayBreakMins").val().trim(),
                SaturdayHours: $(this).find("#SaturdayHours").val().trim(),
                SaturdayDays: $(this).find("#SaturdayDays").val().trim(),
                SaturdayStart: SatHrStrt,
                SaturdayEnd: SatHrEnd,
                SaturdayBreakMins: $(this).find("#SaturdayBreakMins").val().trim(),
                SundayHours: $(this).find("#SundayHours").val().trim(),
                SundayDays: $(this).find("#SundayDays").val().trim(),
                SundayStart: SunHrStrt,
                SundayEnd: SauHrEnd,
                SundayBreakMins: $(this).find("#SundayBreakMins").val().trim(),
            }
            tableData.push(model);
        });
        $.ajax({
            url: constantHolidaySetting.SaveTrueRoatingData,
            data: { Id: Id, Name: Name, IsRotating: allowRoatating, modelString: JSON.stringify(tableData) },
            type: 'post',
            success: function (data) {
                window.location.reload();
            }
        });
    }}
});

$(".DefaultWorkPattern_Edit").click(function () {
    
    var value = $("#drp_DefaultWorkPattern").val();
    if (value != "AddNew_WorkPattern" || value != "0") {
        $.ajax({
            url: constantHolidaySetting.workPatten,
            data: { Id: value },
            success: function (data) {
                
                $('#DefaultWorkPatternBody').html('');
                $('#DefaultWorkPatternBody').html(data);
                $('#DefaultWorkPatternBody').find(".timeMask").mask('00:00');
            }
        });
    }
});


// Holiday Year List 
$('#page_content').on('change', '#drp_Holiday_Year', function () {
   $("#page_content").find("#lbl-error-Holiday_Year").hide();
});

// Calculation Period List
$('#page_content').on('change', '#drp_CalculationPeriod', function () {
    $("#page_content").find("#lbl-error-CalculationPeriod").hide();
});