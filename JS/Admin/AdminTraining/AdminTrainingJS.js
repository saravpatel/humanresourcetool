var customfiledtype = [];
function DataTableDesign() {
    var table = $('#TrainingListtable').DataTable({
        "sDom": '<"top"i>rt<"bottom"flp><"clear">'
    });

    $('#tableDivTraining').find('.dataTables_filter').hide();
    $('#tableDivTraining').find('.dataTables_info').hide();

    $('#TrainingListtable tfoot tr').appendTo('#TrainingListtable thead');
    $("#TrainingListtable thead .EmployeeName").keyup(function () {
        table.column(0).search(this.value).draw();
    });
    $("#TrainingListtable thead .TrainingName").keyup(function () {
        table.column(1).search(this.value).draw();
    });
    $("#TrainingListtable thead .ImportanceName").keyup(function () {
        table.column(2).search(this.value).draw();
    });
    $("#TrainingListtable thead .StatusName").keyup(function () {
        table.column(3).search(this.value).draw();
    });
    $("#TrainingListtable thead .Progress").keyup(function () {
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
function calculateDateDiff(stratDate, endDate) {
    if (stratDate != "" || endDate != "") {
        if (StartDateValidation(stratDate, endDate)) {
            $("#tableDivTraining").find("#lbl-error-GreaterEndDate").show();
            $("#tableDivTraining").find("#EndDate").val('');
            $("#tableDivTraining").find("#ExpiryDate").val('');
        }
       }
}
function calculateExpDate(stratDate, endDate) {
    if (stratDate != "" || endDate != "") {
        if (StartDateValidation(stratDate, endDate)) {
            $("#tableDivTraining").find('#addTrainingBody').find("#lbl-error-GreaterExpDate").show();
            $("#tableDivTraining").find("#ExpiryDate").val('');
        }
    }
}
$(document).ready(function () {
    DataTableDesign();
    $(".DatePicker").Zebra_DatePicker({
        showButtonPanel: false,
        format: 'd-m-Y',
        onSelect: function () {
            var fromDate = $(".DatePicker").val();
            $("#validationmessageDatePicaker").hide();
        }
    });
});

$("#tableDivTraining").on('click', '.btn-add-Training', function () {
    $(".hrtoolLoader").show();
    var current = $("#CurrentUser").val();
    $.ajax({
        url: constantSet.addEdit,
        data: { Id: 0, EmployeeId: current },
        success: function (data) {
            $("#tableDivTraining").find('#addTrainingBody').html('');
            $("#tableDivTraining").find('#addTrainingBody').html(data);
            $("#tableDivTraining").find(".trainingTitle").html("Add Employee Training");
            // $("#tableDivTraining").find("#btn-submit-Training").html("Add");
            $('[data-toggle="tooltip"]').tooltip();
            $("#StartDate").Zebra_DatePicker({
                //direction: false,`
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {
                    $("#validationmessageStartDate").hide();
                    $("#addTrainingBody").find("#lbl-error-GreaterEndDate").hide();
                    var startdate = $("#StartDate").val();
                    var enddate = $("#EndDate").val();
                    calculateDateDiff(startdate, enddate);
                   
                }
            });
            $("#EndDate").Zebra_DatePicker({
                //direction: false,
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {
                    $("#validationmessageEndDate").hide();
                    $("#addTrainingBody").find("#lbl-error-GreaterEndDate").hide();
                    var startdate = $("#StartDate").val();
                    var enddate = $("#EndDate").val();
                    calculateDateDiff(startdate, enddate);
                    var fromDate = $("#ExpiryDate").val();
                    calculateExpDate(enddate, fromDate);
                }
            });
            $("#ExpiryDate").Zebra_DatePicker({
                //direction: false,
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {
                    $("#lbl-error-GreaterExpDate").hide();
                    $("#validationmessageExpiryDate").hide();
                    var fromDate = $("#ExpiryDate").val();
                    var enddate =$("#EndDate").val();
                    calculateExpDate(enddate, fromDate);
                }
            });
            $(".DatePicker").Zebra_DatePicker({
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {
                    var fromDate = $(".DatePicker").val();
                    $("#validationmessageDatePicaker").hide();
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

function onFinishCallback() {
    
    $(".hrtoolLoader").show();
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
    var JsonCustomFieldeListJoinString = JSON.stringify($(".CustomeFiled").html().trim());
    var model = {
        Id: $("#tableDivTraining").find("#hidden-Id").val(),
        EmployeeId: $("#tableDivTraining").find("#selectID").val(),
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
        CustomFieldsJSON: JsonCustomFieldeListJoinString,
        TraingDocumentList: JsondocumentListJoinString
    }
    $.ajax({
        type: "POST",
        data: JSON.stringify(model),
        url: constantSet.Savetraining,
        contentType: "application/json",
        success: function (result) {
            $("#tableDivTraining").html('');
            $("#tableDivTraining").html(result);
            DataTableDesign();
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
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
        var EmployeeId = $("#tableDivTraining").find("#selectID").val();

        if (EmployeeId == "") {
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

        $.each($("#tableDivTraining").find('.customField'), function () {
            var id = $(this).context.id.split('_')[1];
            var valid = $(this).find("#IsMandatory_" + id + "").val();
            if (valid == "true") {
                var text = $(this).find("#txtCustome_" + id + "").val();
                if (text == "" || text == "0") {
                    iserror = true;
                    $(this).find("#validationmessage_" + id + "").show();
                    $(this).find("#validationmessageDatePicaker").show();
                }               
            }           
        });

        if (iserror) {
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
            return false;
        }
        else {
            $.each($("#tableDivTraining").find('.customField'), function () {
                var id = $(this).context.id.split('_')[1];
                var dropdownAvailable = $(this).find('select').length;
                if (dropdownAvailable > 0) {
                    var value = $(this).find("#drpCustome_" + id + "").val();
                    $(this).find("#drpCustome_" + id + "").val(value).find("option[value=" + value + "]").attr('selected', true);
                }
                else {
                    var text = $(this).find("#txtCustome_" + id + "").val();
                    var textAreaAvailable = $(this).find('textarea').length;
                    if (textAreaAvailable > 0) {
                        $(this).find("#txtCustome_" + id + "").html(text);
                    }
                    else {
                        $(this).find("#txtCustome_" + id + "").attr("value", text);
                    }
                }
            });
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

$("#tableDivTraining").on('click', '.btn-edit-Training', function () {
    $(".hrtoolLoader").show();
    var current = $("#CurrentUser").val();
    var progress = $("#Progress").val();
    var id = $("#tableDivTraining").find("#TrainingListtable tbody").find('.selected').attr("id");
    $.ajax({
        url: constantSet.addEdit,
        data: { Id: id },
        success: function (data) {

            $("#tableDivTraining").find('#addTrainingBody').html('');
            $("#tableDivTraining").find('#addTrainingBody').html(data);

            if (id > 0) {
                var jsonString = $("#tableDivTraining").find('#addTrainingBody').find("#hiddenCustomJson").val();
                if (jsonString != "") {
                    $("#tableDivTraining").find('#addTrainingBody').find("#CoustomeAdd").html("");
                    $("#tableDivTraining").find('#addTrainingBody').find("#CoustomeAdd").html($.parseJSON(jsonString));

                    $(".DatePicker").Zebra_DatePicker({
                        showButtonPanel: false,
                        format: 'd-m-Y',
                        onSelect: function () {
                            var fromDate = $(".DatePicker").val();
                            $("#validationmessageDatePicaker").hide();
                        }
                    });
                }
            }

            $("#tableDivTraining").find(".trainingTitle").html("Edit Employee Training");
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
                //alert("Value: " + value);
            });

            var CustomeFiledType = $("#CutomiseFiled").val();

            if (CustomeFiledType == "") {

            }
            else {

                //var JSONObject = JSON.parse(CustomeFiledType)
                //$("#AddTrainingModel").find('#addTrainingBody').find("#CoustomeAdd").html('');
                //$("#AddTrainingModel").find('#addTrainingBody').find("#CoustomeAdd").show();
                //$("#AddTrainingModel").find('#addTrainingBody').find("#CoustomeAdd").append(JSONObject.html);
            }
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
        $('#TrainingListtable tbody').find('tr.selected').removeClass('selected');
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
    $.Zebra_Dialog(" are you sure want to Delete this Records!?", {
        'type': false,
        'title': 'Delete Project Record',
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
                        data: { Id: id },
                        success: function (data) {
                            
                            $("#tableDivTraining").html('');
                            $("#tableDivTraining").html(data);
                            DataTableDesign();
                            $(".hrtoolLoader").hide();
                            $(".modal-backdrop").hide();
                            $(".toast-error").show();
                            setTimeout(function () { $(".toast-error").hide(); }, 1500);
                        }
                    });
                }
            }]
    });
    
});

$("#tableDivTraining").on('click', '#btn_AddNewField', function () {
    $("#tableDivTraining").find("#AddNewFieldModel").css("display", "block");
    $(".hrtoolLoader").show();
    $.ajax({
        url: constantSet.FiledTypelistUrl,
        data: {},
        success: function (data) {
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
            //alert("Value: " + value);
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

$("#tableDivTraining").on('click', '#btn-submit-FiledType', function () {

    lastid = $("#lastId").val();
    lastid++;
    $("#lastId").val(lastid);
    var iserror = false;
    var TaxtFiled = $("select#drp-FiledType option:selected").text();
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


            $("#AddNewFieldModel").css("display", "none");
            // var element = document.getElementById("coustome_" + lastid + "");
            // var html = element.outerHTML;
            // var data = { html: html };
            //  var json = JSON.stringify(data);
            // customfiledtype.push(json)
        }
        if (TaxtFiled == "Text Box") {

            var textbox = $("#tableDivTraining").find("#CoustomeAdd").append("<div class='row marbot10 customField' data-id='" + lastid + "'  id='coustome_" + lastid + "' }><div class='form-group'><label class='col-md-3 FieldName'>" + values + "</label><div class='col-md-6'><input class='form-control' id='txtCustome_" + lastid + "' placeholder=" + values + " value=''></input></div><div class='col-md-3'><a class='Removephone' id='Remove_" + lastid + "' data-id=" + lastid + "><span><button type='button' class='close' aria-label='Close'><span aria-hidden='true'>&times;</span></button></span></a><input type='hidden' id='IsMandatory_" + lastid + "' value='" + Mandatory + "' /><span class='field-validation-error' id='validationmessage_" + lastid + "' data-valmsg-for='Name' data-valmsg-replace='true' style='display:none'>This Field Are Required.</span></div></div></div>");


            $("#AddNewFieldModel").css("display", "none");
            //  var element = document.getElementById("coustome_" + lastid + "");
            //  var html = element.outerHTML;
            //  var data = { html: html };
            //  var json = JSON.stringify(textbox);
            //  customfiledtype.push(json)
        }
        if (TaxtFiled == "Date Field") {

            var DateFiled = $("#tableDivTraining").find("#CoustomeAdd").append("<div class='row marbot10 customField' data-id=" + lastid + "  id='coustome_" + lastid + "'><div class='form-group'><label class='col-md-3 FieldName'>" + values + "</label><div class='col-md-6'><input type='text' class='form-control DatePicker' id='txtCustome_" + lastid + "' placeholder=" + values + " value=''></input></div><div class='col-md-3'><a class='Removephone' id='Remove_" + lastid + "' data-id=" + lastid + "><span><button type='button' class='close' aria-label='Close'><span aria-hidden='true'>&times;</span></button></span></a><input type='hidden' id='IsMandatory_" + lastid + "' value='" + Mandatory + "' /><span class='field-validation-error' id='validationmessageDatePicaker' data-valmsg-for='Name' data-valmsg-replace='true' style='display:none'>This Field Are Required.</span></div></div></div>");

            $("#AddNewFieldModel").css("display", "none");
            $(".DatePicker").Zebra_DatePicker({
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {
                    var fromDate = $(".DatePicker").val();
                    $("#validationmessageDatePicaker").hide();
                }
            });
            //   var element = document.getElementById("coustome_" + lastid + "");
            //    var html = element.outerHTML;
            //   var data = { html: html };
            //   var json = JSON.stringify(DateFiled);
            //   customfiledtype.push(json)
        }
        if (TaxtFiled == "Number Field") {

            var Number = $("#tableDivTraining").find("#CoustomeAdd").append("<div class='row marbot10 customField' data-id=" + lastid + "  id='coustome_" + lastid + "'><div class='form-group'><label class='col-md-3 FieldName'>" + values + "</label><div class='col-md-6'><input type='number' class='form-control' id='txtCustome_" + lastid + "' placeholder=" + values + " value=''/></div><div class='col-md-3'><a class='Removephone' id='Remove_" + lastid + "' data-id=" + lastid + "><span><button type='button' class='close' aria-label='Close'><span aria-hidden='true'>&times;</span></button></span></a><input type='hidden' id='IsMandatory_" + lastid + "' value='" + Mandatory + "' /><span class='field-validation-error' id='validationmessage_" + lastid + "' data-valmsg-for='Name' data-valmsg-replace='true' style='display:none'>This Field Are Required.</span></div></div></div>");

            $("#AddNewFieldModel").css("display", "none");
            //   var element = document.getElementById("coustome_" + lastid + "");
            //   var html = element.outerHTML;
            //   var data = { html: html };
            //   var json = JSON.stringify(Number);
            //   customfiledtype.push(json)
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

            $("#tableDivTraining").find("#CoustomeAdd").append(dropDownString);
            var optionString = "";
            for (var i = 0; i < arr.length; i++) {
                optionString += "<option value='" + arr[i] + "'>" + arr[i] + "</option>";
            }

            $("#tableDivTraining").find("#CoustomeAdd").find("#drpCustome_" + lastid).append(optionString);
            //for (var i = 0; i < arr.length; i++) {

            //    $('<option>').val(arr[i]).html(arr[i]).appendTo('#drpCustome_' + lastid);

            //}


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

$("#tableDivTraining").on('click', '.Removephone', function () {
    $(this).parent().parent().parent().remove();
});
