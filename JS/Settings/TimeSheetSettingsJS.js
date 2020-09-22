
$("#btn-save").click(function () {
    var iserror = false;
    $(".hrtoolLoader").show();
    var project_id = $("#Project").val();
    if (project_id == "0") {
        iserror = true;
        $("#lbl-error-Assignment").show();
        $("#lbl-error-Assignment").html("The List Project is required.");
    }
    var Frequency_id = $("#Frequency").val();
    if (Frequency_id == "0") {
        iserror = true;
        $("#lbl-error-Frequency").show();
        $("#lbl-error-Frequency").html("The List Frequency is required.");
    }
    var Detail_id = $("#Detail").val();
    if (Detail_id == "0") {
        iserror = true;
        $("#lbl-error-Detail").show();
        $("#lbl-error-Detail").html("The List Detail is required");
    }
    if (iserror) {
        $(".hrtoolLoader").hide();
        $(".modal-backdrop").hide();
        return false;
    }
    else {
        $.ajax({
            url: constantTimeSetting.saveData,
            data: { Project: project_id, Frequency: Frequency_id, Detail: Detail_id },
            success: function (data) {
                $(".hrtoolLoader").hide();
                $(".modal-backdrop").hide();
                $(".toast-success").show();
                setTimeout(function () { $(".toast-success").hide(); }, 1500);
                $("#Project").val('');
                $("#Frequency").val('');
                $("#Detail").val('');
            }
        });
    }
});