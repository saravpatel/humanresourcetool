$("#page_content_inner").on("click", "#isSubmittingData", function () {
    $('.hrtoolLoader').show();
    var isError = false;
    var fromamouts = $("#FromAmount").val();
    if (fromamouts == "") {
        isError = true;
        $("#validationmessage").show();
        $("#validationmessage").html("The To Currency is required.");
        $(".hrtoolLoader").hide();
        $(".modal-backdrop").hide();
    }
    if (isError) {
        return false;
    }
    else {
        var model = {
            SelectedFromCurrency: $('#fromCurrency').val(),
            SelectedToCurrency: $('#ToCurrency').val(),
            FromAmount: $("#FromAmount").val(),
            ToAmount: $("#ToAmount").val()
        }
        $.ajax({
            type: "POST",
            data: JSON.stringify(model),
            url: constant.CurrencyConvertUrl,
            contentType: "application/json",
            success: function (data) {
                $("#ToAmount").val(data);
                $(".hrtoolLoader").hide();
                $(".modal-backdrop").hide();
            },
        });
    }
});
$("#page_content_inner").on("click", "#ChangeCurrenyRate", function () {

    $.ajax({
        type: "POST",
        url: constant.ApplyLiveRateUrl,
        success: function (data) {
            $(".close").click();
            $("#ModelCurrency").hide();
        },
    });
});
$("#page_content_inner").on("click", "#ExportCurreny", function () {
    $.ajax({
        type: "POST",
        url: constant.ExportCurrencyUrl,
        success: function (data) {

           
        },
    });
});
$('#page_content_inner').on("click", ".btnChangeDisplayOrder", function () {
    
    var Id = $(this).attr('data-id');
    $("#FixedRate_" + Id).prop("disabled", false).focus();
    $("#FixedId_" + Id).prop("disabled", false).focus();
    var isAnySave = 0;
    $(".btnChangeDisplayOrder").each(function () {
        if ($(this).text() == "SAVE") {
            isAnySave = 1;
            $(".toast-success").show();
            setTimeout(function () { $(".toast-success").hide(); }, 1500);
        }
    });
    if (isAnySave == 1) {
        if ($(this).text() == "SAVE") {
            $("#change_" + Id).html('CHANGE');
            $("#FixedRate_" + Id).prop("disabled", true).focus();
            $("#FixedId_" + Id).prop("disabled", true).focus();
            var Id = $(this).attr('data-id');
            var fixrate = $("#FixedRate_" + Id).val();
            if ($($("#FixedId_" + Id)).is(':checked')) {
                var isfixed = true;
            }
            else {
                var isfixed = false;
            }
            $.ajax({
                data: { id: Id, FixedRate: fixrate, IsFixed: isfixed },
                url: constant.ChangeCurrencyUrl,
                success: function (result) {
                    if (result == "success") {
                        $(".toast-success").show();
                        setTimeout(function () { $(".toast-success").hide(); }, 1500);
                        window.location.reload();
                    }
                }

            })
        }
        else {
            $("#change_" + Id).html('SAVE');
        }
    }
    else { $("#change_" + Id).html('SAVE'); }
});