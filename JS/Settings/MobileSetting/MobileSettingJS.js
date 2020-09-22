
$("#btn-save").click(function () {
    $(".hrtoolLoader").show();
    var allowMobileUse = $("#allowMobileUse").prop('checked');
    var showPhoneNumbers = $("#ShowPhoneNumbers").prop('checked');

    $.ajax({
        url: constantMobileSetting.saveData,
        data: { AllowMobileUse: allowMobileUse, ShowPhoneNumber: showPhoneNumbers },
        success: function (data) {            
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();

            $(".toast-success").show();
            setTimeout(function () { $(".toast-success").hide(); }, 1500);
        }
    });
});