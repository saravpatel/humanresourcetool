$(document).ready(function () {
    DataTableDesign();
});

function DataTableDesign() {
    $('#EmployeeCaseLogModalTable tfoot tr').appendTo('#EmployeeCaseLogModalTable thead');
    var table = $('#EmployeeCaseLogModalTable').DataTable({
        "sDom": '<"top"i>rt<"bottom"flp><"clear">',
        "scrollX":true
    });
    $('#tableDiv').find('.dataTables_filter').hide();
    $('#tableDiv').find('.dataTables_info').hide();

    $("#tableDiv thead .SearchCaseId").keyup(function () {
        table.column(0).search(this.value).draw();
    });
    $("#tableDiv thead .SearchSummary").keyup(function () {
        table.column(1).search(this.value).draw();
    });
    $("#tableDiv thead .SearchStatus").keyup(function () {
        table.column(2).search(this.value).draw();
    });
    $("#tableDiv thead .SearchCategory").keyup(function () {
        table.column(3).search(this.value).draw();
    });
    $("#tableDiv thead .SearchCreatedBy").keyup(function () {
        table.column(4).search(this.value).draw();
    });
    $("#tableDiv thead .SearchCreated").Zebra_DatePicker({
        //direction: false,
        showButtonPanel: false,
        format: 'd-M-Y',
        onSelect: function () {
            var date = $("#EmployeeCaseLogModalTable").find("thead").find('.SearchCreated').val();
            table.column(5).search(date).draw();
        }
    });

    $("body").on('click', '.dp_clear', function () {
        var date = $("#EmployeeCaseLogModalTable").find("thead").find('.SearchCreated').val();
        table.column(5).search(date).draw();
    });
}

$("#tableDiv").on('click', '.btn-Refresh-EmployeeCaseLog', function () {
    window.location.reload();
});

$("#tableDiv").on('click', '.btn-ClearSorting-EmployeeCaseLog', function () {
    window.location.reload();
});

$("#tableDiv").on('click', '.btn-clearFilter-EmployeeCaseLog', function () {
    window.location.reload();
});

$("#tableDiv").on('click', '.btn-add-EmployeeCaseLog', function () {
    $.ajax({
        url: constantEmployeeCaseLog.addEdit,
        data: { Id: 0 },
        success: function (data) {
            $("#tableDiv").find('#EmployeeCaseLogBody').html('');
            $("#tableDiv").find('#EmployeeCaseLogBody').html(data);

            $('[data-toggle="tooltip"]').tooltip();

            $("#tableDiv").find('#EmployeeCaseLogBody').find('#wizard').smartWizard({
                onLeaveStep: leaveAStepCallback,
                onFinish: onFinishCallback
            });            

            $("#tableDiv").find('#EmployeeCaseLogBody').find('.buttonNext').addClass('btn btn-warning');
            $("#tableDiv").find('#EmployeeCaseLogBody').find('.buttonPrevious').addClass('btn btn-warning');
            $("#tableDiv").find('#EmployeeCaseLogBody').find('.buttonFinish').addClass('btn btn-success');

            $("#tableDiv").find('#EmployeeCaseLogBody').find('.buttonPrevious').hide();
            $("#tableDiv").find('#EmployeeCaseLogBody').find('.buttonFinish').hide();


            $("#tableDiv").find('#EmployeeCaseLogBody').find('div#froala-editor').froalaEditor({
                //toolbarButtons: ['bold', 'italic', 'underline', 'strikeThrough', 'subscript', 'superscript', 'fontFamily', 'fontSize', '|', 'specialCharacters', 'color', 'emoticons', 'inlineStyle', 'paragraphStyle', '|', 'paragraphFormat', 'align', 'formatOL', 'formatUL', 'outdent', 'indent', '-', 'quote', 'insertHR', 'insertLink', 'insertImage', 'insertVideo', 'insertFile', 'insertTable', '|', 'undo', 'redo', 'clearFormatting', 'selectAll', 'html', 'applyFormat', 'removeFormat', 'fullscreen', 'print', 'help'],
                toolbarButtons: ['bold', 'italic', 'underline', 'formatUL'],
                pluginsEnabled: null
            })

        }
    });
});

$("#tableDiv").on('click', '.deleteComment', function () {
    $(this).parent().parent().remove();
});

$("#tableDiv").on('click', '.editComment', function () {
    $("#tableDiv").find('#btnAddComment').hide();
    $("#tableDiv").find('#btnEditComment').show();
    $("#tableDiv").find('#btnEditComment').attr("data-editId", $(this).parent().parent().attr("id"));
    var htmlString = $(this).parent().parent().find(".sickComments").html();
    $("#tableDiv").find('#EmployeeCaseLogBody').find('div#froala-editor').froalaEditor('html.set', htmlString);
});

$("#tableDiv").on('click', '#btnEditComment', function () {
    var editDiv = $(this).attr("data-editId");
    var data = $("#tableDiv").find('#EmployeeCaseLogBody').find('div#froala-editor').froalaEditor('html.get');
    $("#tableDiv").find("#CommentList").find("#" + editDiv).find(".sickComments").html("");
    $("#tableDiv").find("#CommentList").find("#" + editDiv).find(".sickComments").html(data);

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

    $("#tableDiv").find('#btnAddComment').show();
    $("#tableDiv").find('#btnEditComment').hide();
});

$("#tableDiv").on('click', '#btnAddComment', function () {
    var data = $("#tableDiv").find('#EmployeeCaseLogBody').find('div#froala-editor').froalaEditor('html.get');
    var isEmpty = $("#tableDiv").find("#CommentList").html().trim();
    var currentTime = getCurrentDateTime();
    if (isEmpty == "") {
        var appendDataString = '<div class="seccomments row" id="comment_1" data-commentid="0"><div class="col-lg-10"><p><span class="black postedby">' + constantEmployeeCaseLog.userFullName + '</span> - <span class="commentTime">' + currentTime + '</sapn></p><div class="sickComments">' + data + '</div></div><div class="col-lg-2" style=""><i class="fa fa-pencil editComment"></i><i class="fa fa-trash-o deleteComment"></i></div></div>';
        $("#tableDiv").find("#CommentList").html(appendDataString);
    }
    else {
        var lastCommentid = $("#tableDiv").find(".seccomments:first").attr('id').split('_')[1];
        var newId = parseInt(lastCommentid) + 1;
        var appendDataString = '<div class="seccomments row" id="comment_' + newId + '" data-commentid="0"><div class="col-lg-10"><p><span class="black postedby">' + constantEmployeeCaseLog.userFullName + '</span> - <span class="commentTime">' + currentTime + '</sapn></p><div class="sickComments">' + data + '</div></div><div class="col-lg-2" style=""><i class="fa fa-pencil editComment"></i><i class="fa fa-trash-o deleteComment"></i></div></div>';
        $("#tableDiv").find("#CommentList").prepend(appendDataString);
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
                    url: constantEmployeeCaseLog.ImageUrl,
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
    //$("#tableDiv").find('#AdminCaseLogBody').find
    if (context.fromStep == 1) {
        var isError = false;
        var status = $("#tableDiv").find('#EmployeeCaseLogBody').find('#drp-StatusId').val();
        var employee = $("#currntEmployeeId").val();
        var category = $("#tableDiv").find('#EmployeeCaseLogBody').find('#drp-CategoryId').val();
        if (status == "0") { isError = true; $("#tableDiv").find('#EmployeeCaseLogBody').find("#lbl-error-StatusList").show(); }        
        if (category == "0") { isError = true; $("#tableDiv").find('#EmployeeCaseLogBody').find("#lbl-error-CategoryList").show(); }

        if (isError) {
            return false;
        }
        else {
            if (context.toStep = 2) {                
                $("#tableDiv").find('#EmployeeCaseLogBody').find('.buttonNext').show();
                $("#tableDiv").find('#EmployeeCaseLogBody').find('.buttonPrevious').show();
                $("#tableDiv").find('#EmployeeCaseLogBody').find('.buttonFinish').hide();
            }
            else {
                $("#tableDiv").find('#EmployeeCaseLogBody').find('.buttonNext').hide();
                $("#tableDiv").find('#EmployeeCaseLogBody').find('.buttonPrevious').show();
                $("#tableDiv").find('#EmployeeCaseLogBody').find('.buttonFinish').show()
            }
            $("#tableDiv").find('#EmployeeCaseLogBody').find("#lbl-error-StatusList").hide();
            $("#tableDiv").find('#EmployeeCaseLogBody').find("#lbl-error-CategoryList").hide();
            return true;
        }
    }
    if (context.fromStep == 2) {        
        if (context.toStep == 1) {
            $("#tableDiv").find('#EmployeeCaseLogBody').find('.buttonNext').show();
            $("#tableDiv").find('#EmployeeCaseLogBody').find('.buttonPrevious').hide();
            $("#tableDiv").find('#EmployeeCaseLogBody').find('.buttonFinish').hide();
        }
        else {
            $("#tableDiv").find('#EmployeeCaseLogBody').find('.buttonNext').hide();
            $("#tableDiv").find('#EmployeeCaseLogBody').find('.buttonPrevious').show();
            $("#tableDiv").find('#EmployeeCaseLogBody').find('.buttonFinish').show();
        }
        return true;
    }
    else {
        if (context.toStep == 2) {            
            $("#tableDiv").find('#EmployeeCaseLogBody').find('.buttonNext').show();
            $("#tableDiv").find('#EmployeeCaseLogBody').find('.buttonPrevious').show();
            $("#tableDiv").find('#EmployeeCaseLogBody').find('.buttonFinish').hide();
        }
        else {
            $("#tableDiv").find('#EmployeeCaseLogBody').find('.buttonNext').show();
            $("#tableDiv").find('#EmployeeCaseLogBody').find('.buttonPrevious').hide();
            $("#tableDiv").find('#EmployeeCaseLogBody').find('.buttonFinish').hide();
        }

        return true;
    }

}

function onFinishCallback(obj, context) {
    $(".hrtoolLoader").show();

    var id = $("#tableDiv").find('#EmployeeCaseLogBody').find('#CaseHiddenId').val();
    var status = $("#tableDiv").find('#EmployeeCaseLogBody').find('#drp-StatusId').val();
    var employee = $("#currntEmployeeId").val();
    var category = $("#tableDiv").find('#EmployeeCaseLogBody').find('#drp-CategoryId').val();
    var summary = $("#tableDiv").find('#EmployeeCaseLogBody').find('#SummaryText').val();

    var commentList = [];
    $.each($("#tableDiv").find('#CommentList').find(".seccomments"), function () {
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
        StatusId: status,
        EmployeeId: employee,
        CategoryId: category,
        Summary: summary,
        CommentListString: JsoncommentListJoinString,
        DocumentListString: JsondocumentListJoinString
    }

    $.ajax({
        url: constantEmployeeCaseLog.saveData,
        type: 'POST',
        data: JSON.stringify(model),
        contentType: "application/json",
        success: function (data) {
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
    location.reload();
}

$('#tableDiv').on('click', '.dataTr', function () {
    if ($(this).hasClass('dataTr')) {
        $('#EmployeeCaseLogModalTable tbody').find('tr.selected').removeClass('selected');
        $(this).addClass('selected');
        $("#tableDiv").find(".btn-edit-EmployeeCaseLog").removeAttr('disabled');
        $("#tableDiv").find(".btn-delete-EmployeeCaseLog").removeAttr('disabled');
    }
});

$("#tableDiv").on('click', '.btn-edit-EmployeeCaseLog', function () {
    var id = $("#tableDiv").find("#EmployeeCaseLogModalTable tbody").find('.selected').attr("id");
    $.ajax({
        url: constantEmployeeCaseLog.addEdit,
        data: { Id: id },
        success: function (data) {
            $("#tableDiv").find('#EmployeeCaseLogBody').html('');
            $("#tableDiv").find('#EmployeeCaseLogBody').html(data);

            $.each($("#tableDiv").find('.sickComments'), function () {
                var uneacapeString = $(this).attr("data-commentstring");
                $(this).html(uneacapeString);
            });

            $('[data-toggle="tooltip"]').tooltip();

            $("#tableDiv").find('#EmployeeCaseLogBody').find('#wizard').smartWizard({
                onLeaveStep: leaveAStepCallback,
                onFinish: onFinishCallback
            });

            $("#tableDiv").find('#EmployeeCaseLogBody').find('.buttonNext').addClass('btn btn-warning');
            $("#tableDiv").find('#EmployeeCaseLogBody').find('.buttonPrevious').addClass('btn btn-warning');
            $("#tableDiv").find('#EmployeeCaseLogBody').find('.buttonFinish').addClass('btn btn-success');

            $("#tableDiv").find('#EmployeeCaseLogBody').find('.buttonPrevious').hide();
            $("#tableDiv").find('#EmployeeCaseLogBody').find('.buttonFinish').hide();


            $("#tableDiv").find('#EmployeeCaseLogBody').find('div#froala-editor').froalaEditor({
                //toolbarButtons: ['bold', 'italic', 'underline', 'strikeThrough', 'subscript', 'superscript', 'fontFamily', 'fontSize', '|', 'specialCharacters', 'color', 'emoticons', 'inlineStyle', 'paragraphStyle', '|', 'paragraphFormat', 'align', 'formatOL', 'formatUL', 'outdent', 'indent', '-', 'quote', 'insertHR', 'insertLink', 'insertImage', 'insertVideo', 'insertFile', 'insertTable', '|', 'undo', 'redo', 'clearFormatting', 'selectAll', 'html', 'applyFormat', 'removeFormat', 'fullscreen', 'print', 'help'],
                toolbarButtons: ['bold', 'italic', 'underline', 'formatUL'],
                pluginsEnabled: null
            })

        }
    });
});

$("#tableDiv").on('click', '.btn-delete-EmployeeCaseLog', function () {
    var id = $("#tableDiv").find("#EmployeeCaseLogModalTable tbody").find('.selected').attr("id");
    $.Zebra_Dialog("Are you sure you would like to delete this record?", {
        'type': false,
        'title': 'Delete Case Record',
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
                        url: constantEmployeeCaseLog.DeleteData,
                        data: { Id: id },
                        success: function (data) {
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
            }]
    });
});