$(document).ready(function () {
    DataTableDesign();
});

function DataTableDesign() {
    $('#AdminTaskTable tfoot tr').appendTo('#AdminTaskTable thead');
    var table = $('#AdminTaskTable').DataTable({
        "sDom": '<"top"i>rt<"bottom"flp><"clear">',
        "scrollX": true,
        "bSort": false
    });

    $('#tableDiv').find('.dataTables_filter').hide();
    $('#tableDiv').find('.dataTables_info').hide();

    $("#tableDiv").on('keyup', '.SearchTitle', function () {
        table.column(0).search(this.value).draw();
    });

    $("#tableDiv").on('keyup', '.SearchAssigned', function () {
        table.column(1).search(this.value).draw();
    });

    $("#tableDiv").on('keyup', '.SearchRelation', function () {
        table.column(2).search(this.value).draw();
    });


    $("#tableDiv").on('keyup', '.SearchCategory', function () {
        table.column(3).search(this.value).draw();
    });

    $("#tableDiv").on('keyup', '.SearchExpireinDays', function () {
        table.column(4).search(this.value).draw();
    });

    $("#tableDiv .SearchDueDate").Zebra_DatePicker({
        showButtonPanel: false,
        format: 'd-M-Y',
        onSelect: function () {
            var date = $("#tableDiv").find('.SearchDueDate').val();
            table.column(5).search(date).draw();
        }
    });

    $("body").on('click', '.dp_clear', function () {
        var date = $("#tableDiv").find('.SearchDueDate').val();
        table.column(5).search(date).draw();
    });
}

$("#tableDiv").on('click', '.btn-add-AdminTask', function () {
    $.ajax({
        url: constantAdminTask.addEdit,
        data: { Id: 0 },
        success: function (data) {
            $("#tableDiv").find('#AdminTaskBody').html('');
            $("#tableDiv").find('#AdminTaskBody').html(data);

            $("#tableDiv").find('#AdminTaskBody').find("#txt_DueDate").Zebra_DatePicker({
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {
                    $("#tableDiv").find('#AdminTaskBody').find("#lbl-error-DueDate").hide();
                }
            });

            $('[data-toggle="tooltip"]').tooltip();

            $("#tableDiv").find('#AdminTaskBody').find('#wizard').smartWizard({
                onLeaveStep: leaveAStepCallback,
                onFinish: onFinishCallback
            });

            $("#tableDiv").find('#AdminTaskBody').find('.buttonNext').addClass('btn btn-warning');
            $("#tableDiv").find('#AdminTaskBody').find('.buttonPrevious').addClass('btn btn-warning');
            $("#tableDiv").find('#AdminTaskBody').find('.buttonFinish').addClass('btn btn-success');

            $("#tableDiv").find('#AdminTaskBody').find('.buttonPrevious').hide();
            $("#tableDiv").find('#AdminTaskBody').find('.buttonFinish').hide();

        }
    });
});

$("#tableDiv").on('click', '.file-deleteicon', function () {
    $(this).parent().remove();
});

$("#tableDiv").on('change', '#fileToUpload', function (e) {
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
                    url: constantAdminTask.ImageUrl,
                    contentType: false,
                    processData: false,
                    data: imageData,
                    success: function (result) {
                        var string = '<div class="row ListData"><div class="col-lg-1 icon"><i class="fa fa-paperclip"></i></div><div class="col-lg-10 attach-disc"><label class="fileName" data-newFileName="' + result.NewFileName + '">' + result.originalFileName + '</label><br /><input type="text" class="ImageDescription form-control" placeholder="Add Description Here"/></div><div class="col-lg-1 file-deleteicon"><i class="fa fa-trash-o"></i></div></div>';
                        var isEmpty = $("#tableDiv").find("#filesList").html();
                        if (isEmpty = "") {
                            $("#tableDiv").find("#filesList").html(string);
                        }
                        else {
                            $("#tableDiv").find("#filesList").append(string);
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

function leaveAStepCallback(obj, context) {
    if (context.fromStep == 1) {
        var isError = false;
        var title = $("#tableDiv").find('#AdminTaskBody').find('#txt_Title').val().trim();
        var category = $("#tableDiv").find('#AdminTaskBody').find('#drp-Category').val();
        var assignTo = $("#tableDiv").find('#AdminTaskBody').find('#selectID').val();
        var inRelationTo = $("#tableDiv").find('#AdminTaskBody').find('#selectReportToID').val();
        var dueDate = $("#tableDiv").find('#AdminTaskBody').find('#txt_DueDate').val().trim();

        if (title == "") { isError = true; $("#tableDiv").find('#AdminTaskBody').find("#lbl-error-Title").show(); }
        if (category == "0") { isError = true; $("#tableDiv").find('#AdminTaskBody').find("#lbl-error-Category").show(); }
        if (assignTo == "0") { isError = true; $("#tableDiv").find('#AdminTaskBody').find("#lbl-error-AssignTo").show(); }
        if (inRelationTo == "0") { isError = true; $("#tableDiv").find('#AdminTaskBody').find("#lbl-error-InRelationTo").show(); }
        if (dueDate == "") { isError = true; $("#tableDiv").find('#AdminTaskBody').find("#lbl-error-DueDate").show(); }

        if (isError) {
            return false;
        }
        else {
            $("#tableDiv").find('#AdminTaskBody').find('.buttonNext').hide();
            $("#tableDiv").find('#AdminTaskBody').find('.buttonPrevious').show();
            $("#tableDiv").find('#AdminTaskBody').find('.buttonFinish').show()

            return true;
        }
    }
    else {

        $("#tableDiv").find('#AdminTaskBody').find('.buttonNext').show();
        $("#tableDiv").find('#AdminTaskBody').find('.buttonPrevious').hide();
        $("#tableDiv").find('#AdminTaskBody').find('.buttonFinish').hide()
        return true;
    }
}

function onFinishCallback(obj, context) {
    var id = $("#tableDiv").find('#AdminTaskBody').find('#HiddenId').val();
    var title = $("#tableDiv").find('#AdminTaskBody').find('#txt_Title').val().trim();
    var category = $("#tableDiv").find('#AdminTaskBody').find('#drp-Category').val();
    var assignTo = $("#tableDiv").find('#AdminTaskBody').find('#selectID').val();
    var inRelationTo = $("#tableDiv").find('#AdminTaskBody').find('#selectReportToID').val();
    var dueDate = $("#tableDiv").find('#AdminTaskBody').find('#txt_DueDate').val().trim();
    var status = $("#tableDiv").find('#AdminTaskBody').find('#drp-Status').val();
    var alterBeforeDays = $("#tableDiv").find('#AdminTaskBody').find('#txt_AlertBeforeDays').val().trim();
    var description = $("#tableDiv").find('#AdminTaskBody').find('#textArea_Description').val().trim();
    var currentFilter = $(".in-submenu").find('.active').attr('class').replace('active', '').trim();
    var documentList = [];
    $.each($("#tableDiv").find('#filesList').find(".ListData"), function () {
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
        Id: id,
        Title: title,
        CategoryId: category,
        AssignToId: assignTo,
        InRelationToId: inRelationTo,
        DueDate: dueDate,
        StatusId: status,
        AlertBeforeDays: alterBeforeDays,
        Description: description,
        jsonDocumentListString: JsondocumentListJoinString,
        FilterSearch: currentFilter
    }
    $.ajax({
        url: constantAdminTask.SaveData,
        type: 'POST',
        data: JSON.stringify(model),
        contentType: "application/json",
        success: function (data) {
            $.ajax({
                url: constantAdminTask.updateCount,
                success: function (result) {
                    $(".in-submenu").html("");
                    $(".in-submenu").html(result);

                    $("#tableDiv").html("");
                    $("#tableDiv").html(data);

                    DataTableDesign();

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
}

$('#tableDiv').on('click', '.dataTr', function () {
    if ($(this).hasClass('dataTr')) {
        $('#AdminTaskTable tbody').find('tr.selected').removeClass('selected');
        $(this).addClass('selected');
        $("#tableDiv").find(".btn-edit-AdminTask").removeAttr('disabled');
        $("#tableDiv").find(".btn-delete-AdminTask").removeAttr('disabled');
    }
});

$("#tableDiv").on('click', '.btn-edit-AdminTask', function () {
    var id = $("#tableDiv").find("#AdminTaskTable tbody").find('.selected').attr("id");
    $.ajax({
        url: constantAdminTask.addEdit,
        data: { Id: id },
        success: function (data) {
            $("#tableDiv").find('#AdminTaskBody').html('');
            $("#tableDiv").find('#AdminTaskBody').html(data);

            $("#tableDiv").find('#AdminTaskBody').find("#txt_DueDate").Zebra_DatePicker({
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {
                    $("#tableDiv").find('#AdminTaskBody').find("#lbl-error-DueDate").hide();
                }
            });

            $('[data-toggle="tooltip"]').tooltip();

            $("#tableDiv").find('#AdminTaskBody').find('#wizard').smartWizard({
                onLeaveStep: leaveAStepCallback,
                onFinish: onFinishCallback
            });

            $("#tableDiv").find('#AdminTaskBody').find('.buttonNext').addClass('btn btn-warning');
            $("#tableDiv").find('#AdminTaskBody').find('.buttonPrevious').addClass('btn btn-warning');
            $("#tableDiv").find('#AdminTaskBody').find('.buttonFinish').addClass('btn btn-success');

            $("#tableDiv").find('#AdminTaskBody').find('.buttonPrevious').hide();
            $("#tableDiv").find('#AdminTaskBody').find('.buttonFinish').hide();

        }
    });
});

$("#tableDiv").on('click', '.btn-delete-AdminTask', function () {
    var id = $("#tableDiv").find("#AdminTaskTable tbody").find('.selected').attr("id");
    var currentFilter = $(".in-submenu").find('.active').attr('class').replace('active', '').trim();
    $.Zebra_Dialog("Are you sure you would like to delete this record?", {
        'type': false,
        'title': 'Delete Visa Record',
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
                        url: constantAdminTask.DeleteData,
                        data: { Id: id, search: currentFilter },
                        success: function (data) {
                            $.ajax({
                                url: constantAdminTask.updateCount,
                                success: function (result) {
                                    $(".in-submenu").html("");
                                    $(".in-submenu").html(result);

                                    $("#tableDiv").html("");
                                    $("#tableDiv").html(data);

                                    DataTableDesign();

                                    $(".hrtoolLoader").hide();
                                    $(".modal-backdrop").hide();

                                    $(".toast-error").show();
                                    setTimeout(function () { $(".toast-error").hide(); }, 1500);

                                }
                            });
                        }
                    });
                }
            }]
    });
});

$("#tableDiv").on('click', '.btn-Refresh-AdminTask', function () {
    window.location.reload();
});

$("#tableDiv").on('click', '.btn-ClearSorting-AdminTask', function () {
    window.location.reload();
});

$("#tableDiv").on('click', '.btn-clearFilter-AdminTask', function () {
    window.location.reload();
});

$(".listTask").on('click', '.filter_Class', function () {
   // var currentFilter = $(this).find('a').attr('class');
    //$(".in-submenu").find('.active').removeClass("active");
    //$(this).find('a').addClass('active');
    var currentFilter = $(this).find('a').attr('class');
    $('.userTask li.active').removeClass('active');
    $(this).closest('li').addClass('active');
    $(".hrtoolLoader").show();
    $.ajax({
        url: constantAdminTask.ListData,
        data: { search: currentFilter },
        success: function (data) {
            $("#tableDiv").html("");
            $("#tableDiv").html(data);

            DataTableDesign();

            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
        }
    });
});