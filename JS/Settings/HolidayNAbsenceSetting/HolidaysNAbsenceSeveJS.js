$("#btn-save-HolidaySetting").click(function () {
    $(".hrtoolLoader").show();
    var isError = false;
    var WorkingHours = parseFloat($("#txt_NormalWorkingHours").val().trim()).toFixed(2);
    var WorkPattern = parseInt($("#drp_DefaultWorkPattern")["0"].value);
    var AnnualLeave = parseFloat($("#txt_MaximumDurationofConsecutiveAnnualLeave").val().trim()).toFixed(2);
    var CarryOverDays = parseFloat($("#txt_MaxHolidayCarryOverDays").val().trim()).toFixed(2);
    var CarryOverHours = parseFloat($("#txt_MaxHolidayCarryOverHours").val().trim()).toFixed(2);
    var HolidayYear = parseInt($("#drp_Holiday_Year")["0"].value);
    var PublicHolidayTemplate = parseInt($("#drp_DefaultPublicHolidayTemplate")["0"].value);

    var TotalHolidayEntitlement = $("#txt_DoestheTotalHolidayEntitlement")['0'].checked;
    var HolidayEntitlement = parseFloat($("#txt_HolidayEntitlement").val().trim()).toFixed(2);
    var HolidayReturn = $("#chk_HolidayReturnToWorkForm")['0'].checked;
    var AuthoriseHolidays = $("#chk_AuthoriseHolidaysFromEmail")['0'].checked;
    var TOILPeriod = parseFloat($("#txt_TOILPeriod").val().trim()).toFixed(2);
    var BradfordFactorAlerts = $("#chk_EnableBradfordFactorAlerts")['0'].checked;
    var CalculationPeriod = parseInt($("#drp_CalculationPeriod")["0"].value);
    var pubHoliday = $("#drp_DefaultPublicHolidayTemplate").val();
    if (pubHoliday == "AddNew_DefaultPublicHolidayTemplate") {
        isError = true;
        $("#lbl-error-ValidDefaultPublicHoliday").show();
    }
    var defaultWorkPattrn = $("#drp_DefaultWorkPattern").val();
    if (defaultWorkPattrn == "AddNew_WorkPattern") {
        isError = true;
        $("#lbl-error-ValidDefaultWorkPattern").show();
    }
    if (WorkPattern == "0") { isError = true; $("#page_content").find("#lbl-error-DefaultWorkPattern").show(); };
    if (HolidayYear == "0") { isError = true; $("#page_content").find("#lbl-error-Holiday_Year").show(); };
    if (PublicHolidayTemplate == "0") { isError = true; $("#page_content").find("#lbl-error-DefaultPublicHoliday").show(); }
    if (CalculationPeriod == "0") { isError = true; $("#page_content").find("#lbl-error-CalculationPeriod").show(); }

    if (isError) {

        $(".hrtoolLoader").hide();
        $(".modal-backdrop").hide();
        return false;

    }
    else {

        var Model = {
            Id: $("#hidden-Id").val(),
            WorkingHours: WorkingHours,
            WorkPattern: WorkPattern,
            AnnualLeave: AnnualLeave,
            CarryOverDays: CarryOverDays,
            CarryOverHours: CarryOverHours,
            HolidayYear: HolidayYear,
            PublicHolidayTemplate: PublicHolidayTemplate,
            TotalHolidayEntitlement: TotalHolidayEntitlement,
            HolidayEntitlement: HolidayEntitlement,
            HolidayReturn: HolidayReturn,
            AuthoriseHolidays: AuthoriseHolidays,
            TOILPeriod: TOILPeriod,
            BradfordFactorAlerts: BradfordFactorAlerts,
            CalculationPeriod: CalculationPeriod,
        }

        $.ajax({
            type: "POST",
            url: constantHolidaySetting.saveSettings,
            data: { Model: JSON.stringify(Model) },
            success: function (data) {
                window.location.reload();
            }

        });
    }

});
