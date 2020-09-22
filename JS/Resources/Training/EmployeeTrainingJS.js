function DataTableDesign() {
    $('#TrainingListtable tfoot tr').appendTo('#TrainingListtable thead');
    var table = $('#TrainingListtable').DataTable({
        "sDom": '<"top"i>rt<"bottom"flp><"clear">',
        "scrollX":true
    });
    $('#tableDivTraining').find('.dataTables_filter').hide();
    $('#tableDivTraining').find('.dataTables_info').hide();
    $("#tableDivTraining thead .EmployeeName").keyup(function () {
        table.column(0).search(this.value).draw();
    });
    $("#tableDivTraining thead .TrainingName").keyup(function () {
        table.column(1).search(this.value).draw();
    });
    $("#tableDivTraining thead .ImportanceName").keyup(function () {
        table.column(2).search(this.value).draw();
    });
    $("#tableDivTraining thead .StatusName").keyup(function () {
        table.column(3).search(this.value).draw();
    });
    $("#tableDivTraining thead .Progress").keyup(function () {
        table.column(4).search(this.value).draw();
    });
    $("#tableDivTraining").find("#TrainingListtable").find("#CreatedStartDate").Zebra_DatePicker({
        //direction: false,
        showButtonPanel: false,
        format: 'd-M-Y',
        onSelect: function () {
            var CreatedStartDate = $("#tableDivTraining").find("#TrainingListtable").find('#CreatedStartDate').val();
            table.column(5).search(CreatedStartDate).draw();
        }
    });
    $("#tableDivTraining").find("#TrainingListtable").find("#CreatedEndDate").Zebra_DatePicker({
        //direction: false,
        showButtonPanel: false,
        format: 'd-M-Y',
        onSelect: function () {
            var CreatedEndDate = $("#tableDivTraining").find("#TrainingListtable").find('#CreatedEndDate').val();
            table.column(6).search(CreatedEndDate).draw();
        }
    });
    $("#tableDivTraining").find("#TrainingListtable").find("#CreatedExpiryDate").Zebra_DatePicker({
        //direction: false,
        showButtonPanel: false,
        format: 'd-M-Y',
        onSelect: function () {
            var CreatedExpiryDate = $("#tableDivTraining").find("#TrainingListtable").find('#CreatedExpiryDate').val();
            table.column(7).search(CreatedExpiryDate).draw();
        }
    });
}
$(document).ready(function () {
    DataTableDesign();
});
function calculateDateDiff(stratDate, endDate) {
    if (stratDate != "" || endDate != "") {

        if (StartDateValidation(stratDate, endDate)) {
            $("#tableDivTraining").find('#addTrainingBody').find("#lbl-error-GreaterEndDate").show();
            $("#tableDivTraining").find("#EndDate").val('');
        }
    }
}
function calculateExpDate(stratDate, endDate)
{
    if (stratDate != "" || endDate != "") {
        if (StartDateValidation(stratDate, endDate)) {
            $("#tableDivTraining").find('#addTrainingBody').find("#lbl-error-GreaterExpDate").show();
            $("#tableDivTraining").find("#ExpiryDate").val('');
        }
    }
}
$("#tableDivTraining").on('click', '.btn-add-Training', function () {

    $(".hrtoolLoader").show();
    var current = $("#CurrentUser").val();
    $.ajax({
        url: constantSet.addEdit,
        data: { Id: 0, EmployeeId: current },
        success: function (data) {
            $("#tableDivTraining").find('#addTrainingBody').html('');
            $("#tableDivTraining").find('#addTrainingBody').html(data);
            $("#tableDivTraining").find("#btn-submit-Training").html("Add");
            $('[data-toggle="tooltip"]').tooltip();
            $("#StartDate").Zebra_DatePicker({
                //direction: false,`
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {
                    $("#tableDivTraining").find('#addTrainingBody').find("#validationmessageStartDate").hide();
                    $("#tableDivTraining").find('#addTrainingBody').find("#lbl-error-GreaterEndDate").hide();
                    var startdate = $("#tableDivTraining").find('#addTrainingBody').find('#StartDate').val();
                    var enddate = $("#tableDivTraining").find('#addTrainingBody').find('#EndDate').val();
                    calculateDateDiff(startdate, enddate);

                }
            });
            $("#EndDate").Zebra_DatePicker({
                //direction: false,
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {
                    $("#tableDivTraining").find('#addTrainingBody').find("#validationmessageEndDate").hide();
                    $("#tableDivTraining").find('#addTrainingBody').find("#lbl-error-GreaterEndDate").hide();
                    var startdate = $("#tableDivTraining").find('#addTrainingBody').find('#StartDate').val();
                    var enddate = $("#tableDivTraining").find('#addTrainingBody').find('#EndDate').val();
                    calculateDateDiff(startdate, enddate);
                    var fromDate = $('#ExpiryDate').val();
                    calculateExpDate(enddate, fromDate);
                }
            });
            $("#ExpiryDate").Zebra_DatePicker({
                //direction: false,
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {
                    $("#tableDivTraining").find('#addTrainingBody').find("#validationmessageExpiryDate").hide();
                    $("#tableDivTraining").find('#addTrainingBody').find("#lbl-error-GreaterExpDate").hide();
                    var fromDate = $('#ExpiryDate').val();
                    var enddate = $("#tableDivTraining").find('#addTrainingBody').find('#EndDate').val();
                    calculateExpDate(enddate, fromDate);
                }
            });
            $("#AddTrainingModel").find('#wizard').smartWizard({
                onLeaveStep: leaveAStepCallback,
                onFinish: onFinishCallback
            });
            $("#AddTrainingModel").find(".actionBar").css("overflow", "hidden");

            $("#tableDivTraining").find('#addTrainingBody').find('.buttonNext').addClass('btn btn-warning');
            $("#tableDivTraining").find('#addTrainingBody').find('.buttonPrevious').addClass('btn btn-warning');
            $("#tableDivTraining").find('#addTrainingBody').find('.buttonFinish').addClass('btn btn-success');


            $("#tableDivTraining").find('#addTrainingBody').find('.buttonPrevious').hide();
            $("#tableDivTraining").find('#addTrainingBody').find('.buttonFinish').hide();
            $(".actionBar").scroll("hide");
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();

        }
    });
});
$("#tableDivTraining").on('click', '.btn-edit-Training', function () {
    
    $(".hrtoolLoader").show();
    var current = $("#CurrentUser").val();
    var id = $("#tableDivTraining").find("#TrainingListtable tbody").find('.selected').attr("id");
    $.ajax({
        url: constantSet.addEdit,
        data: { Id: id, EmployeeId: current },
        success: function (data) {
            $("#tableDivTraining").find('#addTrainingBody').html('');
            $("#tableDivTraining").find('#addTrainingBody').html(data);
            $("#tableDivTraining").find("#btn-submit-Training").html("Save");
            $('[data-toggle="tooltip"]').tooltip();
            $("#StartDate").Zebra_DatePicker({
                //direction: false,`
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {
                    var fromDate = $('#StartDate').val();
                }
            });
            $("#EndDate").Zebra_DatePicker({
                //direction: false,
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {
                    var fromDate = $('#EndDate').val();
                }
            });
            $("#ExpiryDate").Zebra_DatePicker({
                //direction: false,
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {
                    var fromDate = $('#ExpiryDate').val();
                }
            });
            $("#AddTrainingModel").find('#wizard').smartWizard({
                onLeaveStep: leaveAStepCallback,
                onFinish: onFinishCallback
            });
            var $range = $("#tableDivTraining").find("#example_id");
            $("#tableDivTraining").find("#example_id").ionRangeSlider({
                type: "single",
                min: 0,
                max: 100
            });

            $range.on("change", function () {
                var $this = $(this),
                    value = $this.prop("value");
                $("#tableDivTraining").find("#Progress").val(value);
            });
            $("#AddTrainingModel").find(".actionBar").css("overflow", "hidden");
            $("#tableDivTraining").find('#addTrainingBody').find('.buttonNext').addClass('btn btn-warning');
            $("#tableDivTraining").find('#addTrainingBody').find('.buttonPrevious').addClass('btn btn-warning');
            $("#tableDivTraining").find('#addTrainingBody').find('.buttonFinish').addClass('btn btn-success');

            $("#tableDivTraining").find('#addTrainingBody').find('.buttonNext').show();
            $("#tableDivTraining").find('#addTrainingBody').find('.buttonPrevious').hide();
            $("#tableDivTraining").find('#addTrainingBody').find('.buttonFinish').hide();
            $(".actionBar").scroll("hide");
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
        }
    });
});
$('#tableDivTraining').on('click', '.dataTr', function () {
    if ($(this).hasClass('dataTr')) {
        $('#ProjectListtable tbody').find('tr.selected').removeClass('selected');
        $(this).addClass('selected');
        $("#tableDivTraining").find(".btn-edit-Training").removeAttr('disabled');
        $("#tableDivTraining").find(".btn-delete-Training").removeAttr('disabled');
    }
});
$("#tableDivTraining").on('click', '.btn-Refresh-Training', function () {
    window.location.reload();
});
$("#tableDivTraining").on('click', '.btn-ClearSorting-Training', function () {
    window.location.reload();
});
$("#tableDivTraining").on('click', '.btn-clearFilter-Training', function () {
    window.location.reload();
});
$("#tableDivTraining").on('click', '.btn-delete-Training', function () {
    var id = $("#tableDivTraining").find("#TrainingListtable tbody").find('.selected').attr("id");
    var EId = $("#CurrentUser").val();  
    $.Zebra_Dialog(" Are you sure want to delete this records?", {
        'type': false,
        'title': 'Delete this Record',
        'width': 350,
        'buttons': [
            {
                caption: 'Cancel',
                callback: function () { }
            }, {
                caption: 'Ok',
                callback: function () {
                    $(".hrtoolLoader").show();
                    $.ajax({
                        url: constantSet.DeleteProjectUrl,
                        data: { Id: id,EmpId:EId },
                        success: function (data) {
                            debugger;
                            //$("#tableDivTraining").find('#addTrainingBody').html('');
                            //$("#tableDivTraining").find('#addTrainingBody').html(data);
                            $("#tableDivTraining").html('');
                            $("#tableDivTraining").html(data);
                            DataTableDesign();
                            $(".hrtoolLoader").hide();
                            $(".modal-backdrop").hide();
                            $(".toast-error").show();
                            setTimeout(function () { $(".toast-error").hide(); }, 1500);
                            location.reload();
                            //if (data == "True") {
                            //    $(".hrtoolLoader").hide();
                            //    $(".modal-backdrop").hide();
                            //    $(".toast-success").show();
                            //    setTimeout(function () {
                            //        $(".toast-success").hide();
                            //        window.location.href = constantSet.Index;
                            //    }, 1500);
                            //}
                        }
                    });
                }
            }]
    });
});
$("#tableDivTraining").on('click', '.Removephone', function () {
    $(this).parent().parent().parent().remove();
});
$("#tableDivTraining").on('click', '.btnclose', function () {
    $("#AddNewFieldModel").css("display", "none");
});
$("#tableDivTraining").on('click', '#btn_AddNewField', function () {

    $("#findBtn").find("#AddNewFieldModel").css("display", "block");
    
    $.ajax({
        url: constantSet.FiledTypelistUrl,
        data: {},
        success: function (data) {
            debugger;
            $("#tableDivTraining").find('#AddNewFieldList').html('');
            $("#tableDivTraining").find('#AddNewFieldList').html(data);
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
        }
    });
});

$("#tableDivTraining").on('click', '.btnclose', function () {
    $("#AddNewFieldModel").css("display", "none");
});
$("#tableDivTraining").on('click', '#btn-submit-FiledType', function () {
    debugger;
    lastid = $("#lastId").val();
    lastid++;
    $("#lastId").val(lastid);
    var iserror = false;
    var TaxtFiled = $("#drp-FiledType option:selected").text();
    var Id = $("#drp-FiledType").val();
    var values = $("#Labletext").val();
    var Mandatory = $("#check_MandatoryField").is(":checked");
    if (values == "") {
        iserror = true;
    }

    if (Id == 0)
    { iserror = true }
    if (iserror) {
        return false;
    }
    else {

        if (TaxtFiled == "Text Field") {

            var textarea = $("#tableDivTraining").find("#CoustomeAdd").append("<div class='row marbot10 customField'data-id='" + lastid + "' id='coustome_" + lastid + "'><div class='form-group'><label class='col-md-3 FieldName'>" + values + "</label><div class='col-md-6'><textarea class='form-control textarea-resizeNone' id='txtCustome_" + lastid + "' placeholder=" + values + " value=''></textarea></div><div class='col-md-3'><a class='Removephone' id='Remove_" + lastid + "' data-id=" + lastid + "><span><button type='button' class='close' aria-label='Close'><span aria-hidden='true'>&times;</span></button></span></a><input type='hidden' id='IsMandatory_" + lastid + "' value='" + Mandatory + "' /><span class='field-validation-error' id='validationmessage_" + lastid + "' data-valmsg-for='Name' data-valmsg-replace='true' style='display:none'>This Field Are Required.</span></div></div></div>");
            //$("#AddNewFieldModel").css("display", "none");
            //$('#AddNewFieldModel').modal('hide');
            //$(this).remove();
            $('#AddNewFieldModel').modal('hide');


        }
        if (TaxtFiled == "Text Box") {

            var textbox = $("#tableDivTraining").find("#CoustomeAdd").append("<div class='row marbot10 customField' data-id='" + lastid + "'  id='coustome_" + lastid + "' }><div class='form-group'><label class='col-md-3 FieldName'>" + values + "</label><div class='col-md-6'><input class='form-control' id='txtCustome_" + lastid + "' placeholder=" + values + " value=''></input></div><div class='col-md-3'><a class='Removephone' id='Remove_" + lastid + "' data-id=" + lastid + "><span><button type='button' class='close' aria-label='Close'><span aria-hidden='true'>&times;</span></button></span></a><input type='hidden' id='IsMandatory_" + lastid + "' value='" + Mandatory + "' /><span class='field-validation-error' id='validationmessage_" + lastid + "' data-valmsg-for='Name' data-valmsg-replace='true' style='display:none'>This Field Are Required.</span></div></div></div>");
            //            $("#AddNewFieldModel").css("display", "none");
            $('#AddNewFieldModel').modal('hide');


        }
        if (TaxtFiled == "Date Field") {

            var DateFiled = $("#tableDivTraining").find("#CoustomeAdd").append("<div class='row marbot10 customField' data-id=" + lastid + "  id='coustome_" + lastid + "'><div class='form-group'><label class='col-md-3 FieldName'>" + values + "</label><div class='col-md-6'><input type='text' class='form-control DatePicker' id='txtCustome_" + lastid + "' placeholder=" + values + " value=''></input></div><div class='col-md-3'><a class='Removephone' id='Remove_" + lastid + "' data-id=" + lastid + "><span><button type='button' class='close' aria-label='Close'><span aria-hidden='true'>&times;</span></button></span></a><input type='hidden' id='IsMandatory_" + lastid + "' value='" + Mandatory + "' /><span class='field-validation-error' id='validationmessageDatePicaker' data-valmsg-for='Name' data-valmsg-replace='true' style='display:none'>This Field Are Required.</span></div></div></div>");

            //$("#AddNewFieldModel").css("display", "none");
            $('#AddNewFieldModel').modal('hide');
            $(".DatePicker").Zebra_DatePicker({
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {
                    var fromDate = $(".DatePicker").val();
                    $("#validationmessageDatePicaker").hide();
                }
            });

        }
        if (TaxtFiled == "Number Field") {

            var Number = $("#tableDivTraining").find("#CoustomeAdd").append("<div class='row marbot10 customField' data-id=" + lastid + "  id='coustome_" + lastid + "'><div class='form-group'><label class='col-md-3 FieldName'>" + values + "</label><div class='col-md-6'><input type='number' min='0' class='form-control' id='txtCustome_" + lastid + "' placeholder=" + values + " value=''/></div><div class='col-md-3'><a class='Removephone' id='Remove_" + lastid + "' data-id=" + lastid + "><span><button type='button' class='close' aria-label='Close'><span aria-hidden='true'>&times;</span></button></span></a><input type='hidden' id='IsMandatory_" + lastid + "' value='" + Mandatory + "' /><span class='field-validation-error' id='validationmessage_" + lastid + "' data-valmsg-for='Name' data-valmsg-replace='true' style='display:none' >This Field Are Required.</span></div></div></div>");
            //$("#AddNewFieldModel").css("display", "none");
            $('#AddNewFieldModel').modal('hide');


        }
        if (TaxtFiled == "Drop Down") {
            var listitem = $("#Additemlist").val();
            var arr = listitem.split(',');
            var dropDownString = "<div class='row marbot10 customField' data-id=" + lastid + "  id='coustome_" + lastid + "'>";
            dropDownString += "<div class='form-group'>";
            dropDownString += "<label class='col-md-3 FieldName'>" + values + "</label>";
            dropDownString += "<div class='col-md-6'><select class='form-control' id='drpCustome_" + lastid + "'><option value='0'>-- Select --</option></select></div>";
            dropDownString += "<div class='col-md-3'>";
            dropDownString += "<a class='Removephone' id='Remove_" + lastid + "' data-id=" + lastid + "><span><button type='button' class='close' aria-label='Close'><span aria-hidden='true'>&times;</span></button></span></a>";
            dropDownString += "<input type='hidden' id='IsMandatory_" + lastid + "' value='" + Mandatory + "' />";
            dropDownString += "<span class='field-validation-error' id='validationmessage_" + lastid + "' data-valmsg-for='Name' data-valmsg-replace='true' style='display:none'>This Field Are Required.</span>";
            dropDownString += "</div>";
            dropDownString += "</div>";
            dropDownString += "</div>";


            //var Dropdown = $("#tableDivTraining").find("#CoustomeAdd").append("<div class='row marbot10' data-id=" + lastid + "  id='coustome_" + lastid + "'><div class='form-group'><label class='col-md-3'>" + values + "</label><div class='col-md-6'><select class='form-control' id='drpCustome_" + lastid + "'>"
            //    + "<option>-- Select --</option>" +
            //    +"</select>"+
            //    "</div><div class='col-md-3'><a class='Removephone' id='Remove_" + lastid + "' data-id=" + lastid + "><span><button type='button' class='close' aria-label='Close'><span aria-hidden='true'>&times;</span></button></span></a><input type='hidden' id='IsMandatory_" + lastid + "' value='" + Mandatory + "' /><span class='field-validation-error' id='validationmessage_" + values + "' data-valmsg-for='Name' data-valmsg-replace='true' style='display:none'>This Field Are Required.</span></div></div></div>");

            $("#EmployeeBenefitModal").find("#CoustomeAdd").append(dropDownString);
            var optionString = "";
            for (var i = 0; i < arr.length; i++) {
                optionString += "<option value='" + arr[i] + "'>" + arr[i] + "</option>";
            }

            $("#EmployeeBenefitModal").find("#CoustomeAdd").find("#drpCustome_" + lastid).append(optionString);
            //for (var i = 0; i < arr.length; i++) {

            //    $('<option>').val(arr[i]).html(arr[i]).appendTo('#drpCustome_' + lastid);

            //}

            $('#AddNewFieldModel').modal('hide');

            //     var element = document.getElementById("coustome_" + lastid + "");
            //     var html = element.outerHTML;
            //     var data = { html: html };
            //     var json = JSON.stringify(Dropdown);
            //    customfiledtype.push(json)
        }
    }
    //$('#CutomiseFiled').val(customfiledtype);
    $(this).parent().find(".btnclose").click();


});

$("#tableDivTraining").on('change', '#drp-FiledType', function () {
    var TaxtFiled = $("select#drp-FiledType option:selected").text();
    if (TaxtFiled == "Drop Down") {
        $(".AdddrpCustome").css("display", "block");
    }
    else {
        $(".AdddrpCustome").css("display", "none");
    }
});
$("#tableDivTraining").on('change', '#drp-TrainingStatus', function () {
    var TaxtFiled = $("select#drp-TrainingStatus option:selected").text();
    if (TaxtFiled == "In-progress") {
        var $range = $("#example_id");
        $("#example_id").ionRangeSlider({
            type: "single",
            min: 0,
            max: 100
        });

        $range.on("change", function () {
            var $this = $(this),
                value = $this.prop("value");
            $("#tableDivTraining").find("#Progress").val(value);
        });
        $("#Progresshideshow").css("display", "block");
    }
    else {

        $("#Progresshideshow").css("display", "none");
    }

});
$("#tableDivTraining").on('change', '#fileToUpload', function (e) {
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
                    url: constantSet.ImageUrl,
                    contentType: false,
                    processData: false,
                    data: imageData,
                    success: function (result) {
                        var string = '<div class="row ListData"><div class="col-lg-1 icon"><i class="fa fa-paperclip"></i></div><div class="col-lg-10"><label class="fileName" data-newFileName="' + result.NewFileName + '">' + result.originalFileName + '</label><br /><input type="text" class="ImageDescription form-control" placeholder="Add Description Here"/></div><div class="col-lg-1 file-deleteicon"><i class="fa fa-trash-o"></i></div></div>';
                        var isEmpty = $("#tableDivTraining").find("#filesList").html();
                        if (isEmpty = "") {
                            $("#tableDivTraining").find("#filesList").html(string);
                        }
                        else {
                            $("#tableDivTraining").find("#filesList").append(string);
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
$("#tableDivTraining").on('click', '.file-deleteicon', function () {
    $(this).parent().remove();
});

function onFinishCallback() {
    //$(".hrtoolLoader").show();
    var Importance = $('input[name=Importance]:checked').val();
    var iserror = false;
    var hiddenId = $("#tableDivTraining").find("#hidden-Id").val();
    var documentList = [];
    $.each($("#tableDivTraining").find('#filesList').find(".ListData"), function () {
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
        Id: $("#tableDivTraining").find("#hidden-Id").val(),
        EmployeeId: $("#tableDivTraining").find("#EmployeeId").val(),
        TrainingNameId: $("#tableDivTraining").find("#drp-Training").val(),
        Description: $("#tableDivTraining").find("#txt-SystemListDescription").val(),
        Status: $("#tableDivTraining").find("#drp-TrainingStatus").val(),
        StartDate: $("#tableDivTraining").find("#StartDate").val(),
        EndDate: $("#tableDivTraining").find("#EndDate").val(),
        ExpiryDate: $("#tableDivTraining").find("#ExpiryDate").val(),
        Provider: $("#tableDivTraining").find("#Provider").val(),
        Cost: $("#tableDivTraining").find("#Cost").val(),
        Notes: $("#tableDivTraining").find("#Notes").val(),
        Importance: Importance,
        Progress: $("#tableDivTraining").find("#Progress").val(),
        CustomFieldsJSON: $("#tableDivTraining").find('#CutomiseFiled').val(),
        TraingDocumentList: JsondocumentListJoinString
    }
    $.ajax({
        type: "POST",
        data: JSON.stringify(model),
        url: constantSet.Savetraining,
        contentType: "application/json",
        success: function (result) {
            location.reload();
            $(".hrtoolLoader").hide();
            //$(".modal-backdrop").hide();

            //$(".modal-backdrop.in").css('opacity', '0');
            $("#page_content").find("#tableDivTraining").html('');
            $("#page_content").find("#tableDivTraining").html(result);
          
            DataTableDesign();
            if (hiddenId > 0) {
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
function leaveAStepCallback(obj, context) {
    if (context.fromStep == 1) {
        var iserror = false;
        var EmployeeId = $("#tableDivTraining").find("#EmployeeId").val();
        if (EmployeeId == "0") {
            iserror = true;
            $("#validationmessageEmployeeName").show();
            $("#validationmessageEmployeeName").html("The Employee is required.");
        }
        var TrainingNameId = $("#tableDivTraining").find("#drp-Training").val();
        if (TrainingNameId == "0") {
            iserror = true;
            $("#validationmessageTrainingId").show();
            $("#validationmessageTrainingId").html("The Training is required.");
        }
        var Importance = $('input[name=Importance]:checked').val();
        if (Importance == undefined) {
            iserror = true;
            $("#validationmessageImportance").show();
            $("#validationmessageImportance").html("The Importance is required.");
        }
        var StartDate = $("#tableDivTraining").find("#StartDate").val();
        if (StartDate == "") {
            iserror = true;
            $("#validationmessageStartDate").show();
            $("#validationmessageStartDate").html("The Start Date is required.");
        }
        var EndDate = $("#tableDivTraining").find("#EndDate").val();
        if (EndDate == "") {
            iserror = true;
            $("#validationmessageEndDate").show();
            $("#validationmessageEndDate").html("The End Date is required.");
        }
        var ExpiryDate = $("#tableDivTraining").find("#ExpiryDate").val();
        if (ExpiryDate == "") {
            iserror = true;
            $("#validationmessageExpiryDate").show();
            $("#validationmessageExpiryDate").html("The Expiry Date is required.");
        }
        if (iserror) {
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
            return false;
        }
        else {
            $("#tableDivTraining").find('#addTrainingBody').find('.buttonNext').hide();
            $("#tableDivTraining").find('#addTrainingBody').find('.buttonPrevious').show();
            $("#tableDivTraining").find('#addTrainingBody').find('.buttonFinish').show();

            return true;
        }
    }
    else {
        $("#tableDivTraining").find('#addTrainingBody').find('.buttonNext').show();
        $("#tableDivTraining").find('#addTrainingBody').find('.buttonPrevious').hide();
        $("#tableDivTraining").find('#addTrainingBody').find('.buttonFinish').hide();
        return true;

    }
};