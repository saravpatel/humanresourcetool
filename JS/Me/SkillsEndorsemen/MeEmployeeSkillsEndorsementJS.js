function DataTableDesign() {
    // document.getElementById("AskSomeBodyEndorsement").disabled = true;
    var tableTechnical = $('#TechnicalSkillsTables').DataTable({
        "sDom": '<"top"i>rt<"bottom"flp><"clear">'
    });
    var tableGeneral = $('#GeneralSkillsTables').DataTable({
        "sDom": '<"top"i>rt<"bottom"flp><"clear">'
    });
    var tableGenerallist = $('#EndrosementTableList').DataTable({
        "sDom": '<"top"i>rt<"bottom"flp><"clear">'
    });
    $('#tableDivResource').find('.dataTables_filter').hide();
    $('#tableDivResource').find('.dataTables_info').hide();
    $('#tableDivResource').find('.dataTables_length').hide();
    $('#tableDivResource').find('#TechnicalSkillsTables_paginate').hide();
    $('#tableDivResource').find('#GeneralSkillsTables_paginate').hide();
    $('#AssignskillsListRecords').find('.dataTables_filter').hide();
    $('#AssignskillsListRecords').find('.dataTables_info').hide();

}
$(document).ready(function () {
    DataTableDesign();
});

$("#tableDivResource").on('click', '#btn-submit-AddTechnicalSkills', function () {

    var flag = 0;
    var hiddenId = $("#page_content").find("#UserId").val();
    var employeesId = $("#page_content_inner").find("#EmployeeUserId").val();
    var technicalSkilId = $("#drpTechnical").val();
    $('#TechnicalSkillsTables').find('td').each(function () {

        if ($(this).text().trim() == $("#drpTechnical option:selected").text().trim()) {
            flag = 1;
            $(".toast-info-exist").show();
            setTimeout(function () { $(".toast-info-exist").hide(); }, 1500);
            return false;
        }
    });

    if (flag == 1) {
        return false;
    }
    else {
        $(".hrtoolLoader").show();
        var hdnTechID = $('#hdnTechID').val();
        if (hdnTechID == undefined) {
            hdnTechID = 0;
        }
        var iserror = false;
        if (technicalSkilId == "0") {
            iserror = true;
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
            return false;
        }
        if (iserror) {
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
            return false;
        }
        else {
            $.ajax({
                type: "POST",
                url: constantSet.SaveTechnicalUrl,
                data: { Id: technicalSkilId, EmployeeId: employeesId, TechID: hdnTechID },
                success: function (data) {
                    $("#page_content").find("#tableDivResource").html('')
                    $("#page_content").find("#tableDivResource").html(data);
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
        location.reload();
    }
});
$("#tableDivResource").on('click', '#btn-submit-AddGeneralSkills', function () {

    var flag = 0;
    var hiddenId = $("#page_content").find("#UserId").val();
    var GeneralSkilId = $("#drpGenerl").val();
    var employeesId = $("#EmployeeUserId").val();
    $('#GeneralSkillsTables').find('td').each(function () {

        if ($(this).text().trim() == $("#drpGenerl option:selected").text().trim()) {
            flag = 1;
            $(".toast-info-exist").show();
            setTimeout(function () { $(".toast-info-exist").hide(); }, 1500);
            return false;
        }
    });
    if (flag == 1) {
        return false;
    }
    else {
        var generalID = $('#hdnGeneralID').val();
        if (generalID == undefined) {
            generalID = 0;
        }
        $(".hrtoolLoader").show();
        var iserror = false;
        if (GeneralSkilId == "0") {
            iserror = true;
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
            return false;
        }
        if (iserror) {
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
            return false;
        }
        else {
            $.ajax({
                type: "POST",
                url: constantSet.SaveGeneralUrl,
                data: { Id: GeneralSkilId, EmployeeId: employeesId, GeneralID: generalID },
                success: function (data) {
                    $("#page_content").find("#tableDivResource").html('')
                    $("#page_content").find("#tableDivResource").html(data);
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
        location.reload();
    }

});

//general skills set
$(".btn-edit-GeneralSkills").on('click', function () {
   
    var EmployeeId = $("#page_content_inner").find("#EmployeeUserId").val();
    var id = $("#tableDivResource").find("#GeneralSkillsTables tbody").find('.selected').attr("id");
    $.ajax({
        url: constantSet.GetDetailOfGeneralDetail,
        data: { Id: id, EmployeeId: EmployeeId },
        success: function (data) {
            $("#tableDivResource").find('#AddGeneralSkillsBody').html('');
            $("#tableDivResource").find('#AddGeneralSkillsBody').html(data);

        }
    });
});

$(".btn-add-GeneralSkills").on('click', function () {
    debugger;
    var EmployeeId = $("#page_content_inner").find("#EmployeeUserId").val();
    var id = $("#drpGenerl").val();
    if (id == undefined) {
        id = 0;
    }
    var EmpId = $('#EmployeeUserId').val();
    $.ajax({
        url: constantSet.GetDetailOfGeneralDetail,
        data: { Id: id, EmployeeId: EmployeeId },
        success: function (data) {
            $("#tableDivResource").find('#AddGeneralSkillsBody').html('');
            $("#tableDivResource").find('#AddGeneralSkillsBody').html(data);

        }
    });
})
$('#tableDivResource').on('click', '.dataTr', function () {
    if ($(this).hasClass('dataTr')) {
        $('#GeneralSkillsTables tbody').find('tr.selected').removeClass('selected');
        $(this).addClass('selected');
        $("#tableDivResource").find(".btn-delete-GeneralSkills").removeAttr('disabled');
        $("#tableDivResource").find(".btn-edit-GeneralSkills").removeAttr('disabled');
    }
});

//technical skillset
$("#tableDivResource").on('click', '.btn-edit-TechnicalSkills', function () {
    //var techid = $("#tableDivResource").find("#TechnicalSkillsTables tbody").find('.selected').attr("id");
    //$.ajax({
    //    url: constantSet.ViewSkillslUrl,
    //    data: { TechanicalId: techid },
    //    success: function (data) {
    //        $("#tableDivResource").find('#ViewSkillsDetailsBody').html('');
    //        $("#tableDivResource").find('#ViewSkillsDetailsBody').html(data);
    //        $(".hrtoolLoader").hide();
    //        $(".modal-backdrop").hide();
    //    }
    //});
    var techID = $("#tableDivResource").find("#TechnicalSkillsTables tbody").find('.selected').attr("id");
    var EmpId = $('#EmployeeUserId').val();
    $.ajax({
        url: constantSet.GetDetailOfTechnicalDetail,
        data: { Id: techID, EmployeeId: EmpId },
        success: function (data) {
            $("#tableDivResource").find('#AddTechnicalSkillsBody').html('');
            $("#tableDivResource").find('#AddTechnicalSkillsBody').html(data);

        }
    });
})
$(".btn-add-TechnicalSkills").on('click', function () {

    //var techid = $("#tableDivResource").find("#TechnicalSkillsTables tbody").find('.selected').attr("id");
    //$.ajax({
    //    url: constantSet.ViewSkillslUrl,
    //    data: { TechanicalId: techid },
    //    success: function (data) {
    //        $("#tableDivResource").find('#ViewSkillsDetailsBody').html('');
    //        $("#tableDivResource").find('#ViewSkillsDetailsBody').html(data);
    //        $(".hrtoolLoader").hide();
    //        $(".modal-backdrop").hide();
    //    }
    //});
    var techID = $("#tableDivResource").find("#TechnicalSkillsTables tbody").find('.selected').attr("id");
    if (techID == undefined) {
        techID = 0;

    }
    var EmpId = $('#EmployeeUserId').val();
    $.ajax({
        url: constantSet.GetDetailOfTechnicalDetail,
        data: { Id: techID, EmployeeId: EmpId },
        success: function (data) {
            $("#tableDivResource").find('#AddTechnicalSkillsBody').html('');
            $("#tableDivResource").find('#AddTechnicalSkillsBody').html(data);

        }
    });
})
$('#tableDivResource').on('click', '.dataTrTechnical', function () {

    if ($(this).hasClass('dataTrTechnical')) {
        $('#TechnicalSkillsTables tbody').find('tr.selected').removeClass('selected');
        $(this).addClass('selected');
        $("#tableDivResource").find(".btn-delete-TechnicalSkills").removeAttr('disabled');
        $("#tableDivResource").find(".btn-edit-TechnicalSkills").removeAttr('disabled');
    }
});
$("#page_content").on('click', '#AskForEndorsement', function () {
    $.ajax({
        url: constantSet.AskForEndorsementUrl,
        data: {},
        success: function (data) {
            $("#page_content").find('#AskForEndorsementBody').html('');
            $("#page_content").find('#AskForEndorsementBody').html(data);
            $('[data-toggle="tooltip"]').tooltip();
            $("#page_content").find("#AskForEndorsementModel").find('#wizard').smartWizard({
                onLeaveStep: leaveAStepCallback,
                onFinish: onFinishCallback
            });
            $("#page_content").find('#AskForEndorsementBody').find("#drpEmployee").selectList();
            $("#page_content").find('#AskForEndorsementBody').find('.buttonNext').addClass('btn btn-warning');
            $("#page_content").find('#AskForEndorsementBody').find('.buttonPrevious').addClass('btn btn-warning');
            $("#page_content").find('#AskForEndorsementBody').find('.buttonFinish').addClass('btn btn-success');
            $("#page_content").find('#AskForEndorsementBody').find('.buttonPrevious').hide();
            $("#page_content").find('#AskForEndorsementBody').find('.buttonFinish').hide();
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
        }
    });
});
function leaveAStepCallback(obj, context) {
    if (context.fromStep == 1) {
        var iserror = false;
        var set = $('.Listskilss.active').length;
        if (set == 0) {
            iserror = true;
            $("#page_content").find('#AskForEndorsementBody').find("#Validskillss").show();
        }
        if (iserror) {
            return false;
        }
        else {

            $("#page_content").find('#AskForEndorsementBody').find('.buttonNext').hide();
            $("#page_content").find('#AskForEndorsementBody').find('.buttonPrevious').show();
            $("#page_content").find('#AskForEndorsementBody').find('.buttonFinish').show();
            return true;
        }
    }
    else {
        $("#page_content").find('#AskForEndorsementBody').find('.buttonNext').show();
        $("#page_content").find('#AskForEndorsementBody').find('.buttonPrevious').hide();
        $("#page_content").find('#AskForEndorsementBody').find('.buttonFinish').hide();
        return true;
    }
};
function onFinishCallback() {
    //var Skils = $("#page_content").find('input[name=SkillSet]:checked').val();
    var Skils = $(".Listskilss.active").attr('id');
    var AssignSkillsId = $("#selectID").val();
    //$.each($("#page_content").find("#drpEmployee").parent().find(".selectlist-item"), function () {
    //    AssignSkillsId.push($("#page_content").find("#drpEmployee").parent().find(".selectlist-select").find('option:contains(' + $(this).html().trim() + ')')[0].value);
    //})

    // var Assignskills = AssignSkillsId.join(',');
    var CurrentId = $("#page_content").find('#EmployeeUserId').val();
    var Endrosement = $("#AskForEndorsementBody").find("#EditEndrosemenetId").val();
    var model = {
        Id: Skils,
        AssignUser: AssignSkillsId,
        EmployeeUserId: CurrentId,
        EndrosementId: Endrosement,
        AssignSkillsId: $("#AssignSkillsId").val()
    }
    $.ajax({
        type: "POST",
        data: JSON.stringify(model),
        url: constantSet.AssignskillsUrl,
        contentType: "application/json",
        success: function (data) {
            $("#EndrosementId").val(data.EndrosementId);
            $("#page_content").find("#AssignskillsListRecords").html('');
            $("#page_content").find("#AssignskillsListRecords").html(data);
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
            $("#AskForEndorsementModel").hide();
            if (Endrosement > 0) {
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
$("#page_content").on('click', '.ViewRecordOfSkills', function () {

    $(".hrtoolLoader").show();
    var Gender = $('input[name=Skills]:checked').val();
    var user = $("#page_content_inner").find("#EmployeeUserId").val();
    var techid = $(this).attr("data-id");
    // var techid = $("#tableDivResource").find("#TechnicalSkillsTables tbody").find('.selected').attr("id");
    if (Gender == "1") {
        var Generalid = $(this).attr("data-id");
        $.ajax({
            url: constantSet.GenerallUrl,
            data: { GeneralId: Generalid },
            success: function (data) {
                $("#page_content").find("#ViewTask_Details").find('#ViewSkillsDetailsBody').html('');
                $("#page_content").find("#ViewTask_Details").find('#ViewSkillsDetailsBody').html(data);
                $(".hrtoolLoader").hide();
                $(".modal-backdrop").hide();
            }
        });
    }
    else {
        $.ajax({
            url: constantSet.technicalUrl,
            data: { TechanicalId: techid },
            success: function (data) {
                $("#page_content").find("#ViewTask_Details").find('#ViewSkillsDetailsBody').html('');
                $("#page_content").find("#ViewTask_Details").find('#ViewSkillsDetailsBody').html(data);
                $(".hrtoolLoader").hide();
                $(".modal-backdrop").hide();
            }
        });
    }

});
$('#AssignskillsListRecords').on('click', '.dataTr', function () {
    if ($(this).hasClass('dataTr')) {
        $('#EndrosementTableList tbody').find('tr.selected').removeClass('selected');
        $(this).addClass('selected');
        $("#AssignskillsListRecords").find(".btn-edit-Endrosement").removeAttr('disabled');
        $("#AssignskillsListRecords").find(".btn-delete-Endrosement").removeAttr('disabled');
    }
});
$('#AssignskillsListRecords').on('click', '.AddComments', function () {
    var End_ID = $(this).attr("data-id");
    $.ajax({
        url: constantSet.AddCommentsUrl,
        data: { Id: End_ID },
        success: function (data) {
            $("#EndrosementCommentId").val(End_ID);
            $("#page_content").find('#AddEndrosmentCommentsBody').html('');
            $("#page_content").find('#AddEndrosmentCommentsBody').html(data);
            $('[data-toggle="tooltip"]').tooltip();
            $("#page_content").find('#AssignskillsListRecords').find('div#froala-editor').froalaEditor({
                toolbarButtons: ['bold', 'italic', 'underline', 'strikeThrough', 'subscript', 'superscript', 'fontFamily', 'fontSize', '|', 'specialCharacters', 'color', 'emoticons', 'inlineStyle', 'paragraphStyle', '|', 'paragraphFormat', 'align', 'formatOL', 'formatUL', 'outdent', 'indent', '-', 'quote', 'insertHR', 'insertLink', 'insertImage', 'insertVideo', 'insertFile', 'insertTable', '|', 'undo', 'redo', 'clearFormatting', 'selectAll', 'html', 'applyFormat', 'removeFormat', 'fullscreen', 'print', 'help'],
                //toolbarButtons: ['bold', 'italic', 'underline', 'formatUL'],
                pluginsEnabled: null
            });
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
        }
    });
});
$('#AssignskillsListRecords').on('click', '#btn-submit-AddComments', function () {
    $(".hrtoolLoader").show();
    var EndrosmentId = $("#EndrosementCommentId").val();
    var employeesId = $("#page_content_inner").find("#EmployeeUserId").val();
    var body = $("#page_content").find('#AddEndrosmentCommentsBody').find('div#froala-editor').froalaEditor('html.get');
    CommentId = $("#page_content").find('#AddEndrosmentCommentsBody').find("#CommentId").val();
    var model = {
        EmployeeUserId: employeesId,
        EndrosementId: EndrosmentId,
        Comments: body,
        Id: CommentId
    }
    $.ajax({
        type: "POST",
        data: JSON.stringify(model),
        url: constantSet.SaveCommentUrl,
        contentType: "application/json",
        success: function (data) {

            $("#page_content").find('#AssignskillsListRecords').html('');
            $("#page_content").find('#AssignskillsListRecords').html(data);
            $('[data-toggle="tooltip"]').tooltip();
            $("#page_content").find('#AssignskillsListRecords').find('div#froala-editor').froalaEditor({
                toolbarButtons: ['bold', 'italic', 'underline', 'strikeThrough', 'subscript', 'superscript', 'fontFamily', 'fontSize', '|', 'specialCharacters', 'color', 'emoticons', 'inlineStyle', 'paragraphStyle', '|', 'paragraphFormat', 'align', 'formatOL', 'formatUL', 'outdent', 'indent', '-', 'quote', 'insertHR', 'insertLink', 'insertImage', 'insertVideo', 'insertFile', 'insertTable', '|', 'undo', 'redo', 'clearFormatting', 'selectAll', 'html', 'applyFormat', 'removeFormat', 'fullscreen', 'print', 'help'],
                //toolbarButtons: ['bold', 'italic', 'underline', 'formatUL'],
                pluginsEnabled: null
            });
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();

            if (CommentId > 0) {
                $(".toast-info").show();
                setTimeout(function () { $(".toast-info").hide(); }, 1500);
            }
            else {
                $(".toast-success").show();
                setTimeout(function () { $(".toast-success").hide(); }, 1500);
            }
        }
    });
});
$("#page_content").find('#AssignskillsListRecords').on('click', '.EditCommentRecords', function () {

    var IdRecord = $(this).attr('data-id');
    $("#CommentId").val(IdRecord)
    $.ajax({
        url: constantSet.EditCommentUrl,
        data: { Id: IdRecord },
        success: function (data) {

            $("#page_content").find('#AddEndrosmentCommentsBody').html('');
            $("#page_content").find('#AddEndrosmentCommentsBody').html(data);
            $("#page_content").find('#AssignskillsListRecords').find('div#froala-editor').froalaEditor({
                toolbarButtons: ['bold', 'italic', 'underline', 'strikeThrough', 'subscript', 'superscript', 'fontFamily', 'fontSize', '|', 'specialCharacters', 'color', 'emoticons', 'inlineStyle', 'paragraphStyle', '|', 'paragraphFormat', 'align', 'formatOL', 'formatUL', 'outdent', 'indent', '-', 'quote', 'insertHR', 'insertLink', 'insertImage', 'insertVideo', 'insertFile', 'insertTable', '|', 'undo', 'redo', 'clearFormatting', 'selectAll', 'html', 'applyFormat', 'removeFormat', 'fullscreen', 'print', 'help'],
                //toolbarButtons: ['bold', 'italic', 'underline', 'formatUL'],
                pluginsEnabled: null
            });
            var htmlString = $("#comment_comments_" + IdRecord).html();
            $("#page_content").find('#AssignskillsListRecords').find('div#froala-editor').froalaEditor('html.set', htmlString);
            $('[data-toggle="tooltip"]').tooltip();

            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
        }
    });
});
$('#AssignskillsListRecords').on('click', '.toggles', function () {
    var thisIs = $(this).attr("data-id");
    $("#Comment_" + thisIs).toggle();
    $(this).toggleClass('class1')
})
$("#page_content").find('#AssignskillsListRecords').on('click', '.DeleteCommentRecords', function () {

    var IdRecord = $(this).attr('data-id');
    var employeesId = $("#page_content_inner").find("#EmployeeUserId").val();
    $("#CommentId").val(IdRecord)
    $.Zebra_Dialog("Are you sure want to delete this records?", {
        'type': false,
        'title': 'Delete Comment Record',
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
                        url: constantSet.DeleteCommentUrl,
                        data: { Id: IdRecord, Employee: employeesId },
                        success: function (data) {
                            $("#page_content").find('#AssignskillsListRecords').html('');
                            $("#page_content").find('#AssignskillsListRecords').html(data);
                            $('[data-toggle="tooltip"]').tooltip();
                            $("#page_content").find('#AssignskillsListRecords').find('div#froala-editor').froalaEditor({
                                toolbarButtons: ['bold', 'italic', 'underline', 'strikeThrough', 'subscript', 'superscript', 'fontFamily', 'fontSize', '|', 'specialCharacters', 'color', 'emoticons', 'inlineStyle', 'paragraphStyle', '|', 'paragraphFormat', 'align', 'formatOL', 'formatUL', 'outdent', 'indent', '-', 'quote', 'insertHR', 'insertLink', 'insertImage', 'insertVideo', 'insertFile', 'insertTable', '|', 'undo', 'redo', 'clearFormatting', 'selectAll', 'html', 'applyFormat', 'removeFormat', 'fullscreen', 'print', 'help'],
                                //toolbarButtons: ['bold', 'italic', 'underline', 'formatUL'],
                                pluginsEnabled: null
                            });
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
$("#AssignskillsListRecords").on('click', '.btn-edit-Endrosement', function () {
    $("#page_content").find("#EditAskForEndorsementModelSkill").show();
    var id = $("#AssignskillsListRecords").find("#EndrosementTableList tbody").find('.selected').attr("id");
    $.ajax({
        url: constantSet.EditAssignskillsUrl,
        data: { Id: id },
        success: function (data) {
            $("#EditEndrosemenetId").val(data.EndrosementId)
            $("#page_content").find('#EditAskForEndorsementModelSkillBody').html('');
            $("#page_content").find('#EditAskForEndorsementModelSkillBody').html(data);
            $('[data-toggle="tooltip"]').tooltip();
            $("#page_content").find("#EditAskForEndorsementModelSkillBody").find('#wizard').smartWizard({
                onLeaveStep: EditleaveAStepCallback,
                onFinish: EditonFinishCallback
            });
            $("#page_content").find('#EditAskForEndorsementModelSkillBody').find('div#froala-editor').froalaEditor({
                toolbarButtons: ['bold', 'italic', 'underline', 'strikeThrough', 'subscript', 'superscript', 'fontFamily', 'fontSize', '|', 'specialCharacters', 'color', 'emoticons', 'inlineStyle', 'paragraphStyle', '|', 'paragraphFormat', 'align', 'formatOL', 'formatUL', 'outdent', 'indent', '-', 'quote', 'insertHR', 'insertLink', 'insertImage', 'insertVideo', 'insertFile', 'insertTable', '|', 'undo', 'redo', 'clearFormatting', 'selectAll', 'html', 'applyFormat', 'removeFormat', 'fullscreen', 'print', 'help'],
                //toolbarButtons: ['bold', 'italic', 'underline', 'formatUL'],
                pluginsEnabled: null
            });
            var htmlString = $("#EditEndrosemeneComments").val();
            $("#page_content").find('#EditAskForEndorsementModelSkillBody').find('div#froala-editor').froalaEditor('html.set', htmlString);
            $("#page_content").find('#EditAskForEndorsementModelSkillBody').find("#drpEmployee").selectList();
            $("#page_content").find('#EditAskForEndorsementModelSkillBody').find('.buttonNext').addClass('btn btn-warning');
            $("#page_content").find('#EditAskForEndorsementModelSkillBody').find('.buttonPrevious').addClass('btn btn-warning');
            $("#page_content").find('#EditAskForEndorsementModelSkillBody').find('.buttonFinish').addClass('btn btn-success');
            $("#page_content").find('#EditAskForEndorsementModelSkillBody').find('.buttonPrevious').hide();
            $("#page_content").find('#EditAskForEndorsementModelSkillBody').find('.buttonFinish').hide();
            var htmlString = $("#EditCommnent").val();
            $("#page_content").find('div#froala-editor').froalaEditor('html.set', htmlString);
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
        }
    });
});

function EditleaveAStepCallback(obj, context) {
    if (context.fromStep == 1) {
        var iserror = false;
        var set = $('.Listskilss.active').length;
        if (set == 0) {
            iserror = true;
            $("#page_content").find("#Validskillss").show();
        }
        if (iserror) {
            return false;
        }
        else {
            if (context.toStep = 2) {
                $("#page_content").find('#EditAskForEndorsementModelSkillBody').find('.buttonNext').show();
                $("#page_content").find('#EditAskForEndorsementModelSkillBody').find('.buttonPrevious').show();
                $("#page_content").find('#EditAskForEndorsementModelSkillBody').find('.buttonFinish').hide();
            }
            else {
                $("#page_content").find('#EditAskForEndorsementModelSkillBody').find('.buttonNext').hide();
                $("#page_content").find('#EditAskForEndorsementModelSkillBody').find('.buttonPrevious').show();
                $("#page_content").find('#EditAskForEndorsementModelSkillBody').find('.buttonFinish').show()
            }
            $("#page_content").find('#EditAskForEndorsementModelSkillBody').find("#lbl-error-StatusList").hide();
            $("#page_content").find('#EditAskForEndorsementModelSkillBody').find("#lbl-error-EmployeeList").hide();
            $("#page_content").find('#EditAskForEndorsementModelSkillBody').find("#lbl-error-CategoryList").hide();
            return true;
        }
    }
    if (context.fromStep == 2) {
        if (context.toStep == 1) {
            $("#page_content").find('#EditAskForEndorsementModelSkillBody').find('.buttonNext').show();
            $("#page_content").find('#EditAskForEndorsementModelSkillBody').find('.buttonPrevious').show();
            $("#page_content").find('#EditAskForEndorsementModelSkillBody').find('.buttonFinish').hide();
        }
        else {
            $("#page_content").find('#EditAskForEndorsementModelSkillBody').find('.buttonNext').hide();
            $("#page_content").find('#EditAskForEndorsementModelSkillBody').find('.buttonPrevious').show();
            $("#page_content").find('#EditAskForEndorsementModelSkillBody').find('.buttonFinish').show();
        }
        return true;
    }
    if (context.fromStep == 3) {

        if (context.toStep == 2) {
            $("#page_content").find('#EditAskForEndorsementModelSkillBody').find('.buttonNext').show();
            $("#page_content").find('#EditAskForEndorsementModelSkillBody').find('.buttonPrevious').show();
            $("#page_content").find('#EditAskForEndorsementModelSkillBody').find('.buttonFinish').hide();
        }
        else {
            $("#page_content").find('#EditAskForEndorsementModelSkillBody').find('.buttonNext').hide();
            $("#page_content").find('#EditAskForEndorsementModelSkillBody').find('.buttonPrevious').show();
            $("#page_content").find('#EditAskForEndorsementModelSkillBody').find('.buttonFinish').show();
        }
        return true;
    }
    else {
        if (context.toStep == 2) {
            $("#page_content").find('#EditAskForEndorsementModelSkillBody').find('.buttonNext').show();
            $("#page_content").find('#EditAskForEndorsementModelSkillBody').find('.buttonPrevious').show();
            $("#page_content").find('#EditAskForEndorsementModelSkillBody').find('.buttonFinish').hide();
        }
        else {
            $("#page_content").find('#EditAskForEndorsementModelSkillBody').find('.buttonNext').show();
            $("#page_content").find('#EditAskForEndorsementModelSkillBody').find('.buttonPrevious').hide();
            $("#page_content").find('#EditAskForEndorsementModelSkillBody').find('.buttonFinish').hide();
        }
        return true;
    }
};
function EditonFinishCallback() {
    var AssignSkillsId = $('#selectIDCustomer').val();
    var assignskillName = $("#CustomerCare").val();
    var isError = false;
    if (assignskillName == "" && AssignSkillsId == "") {
        isError = true;
        $("#page_content").find("#ValidEmployee").show();
    }
    //var Skils = $("#page_content").find('input[name=SkillSet]:checked').val();
    var Skils = $(".Listskilss.active").attr('id');

    //$.each($("#page_content").find("#drpEmployee").parent().find(".selectlist-item"), function () {
    //    AssignSkillsId.push($("#page_content").find("#drpEmployee").parent().find(".selectlist-select").find('option:contains(' + $(this).html().trim() + ')')[0].value);
    //})
    //  var Assignskills = AssignSkillsId.join(',');
    var CurrentId = $("#page_content").find('#EmployeeUserId').val();
    var Endrosement = $("#EditAskForEndorsementModelSkillBody").find("#EditEndrosemenetId").val();
    var comment = $("#page_content").find('#EditAskForEndorsementModelSkillBody').find('div#froala-editor').froalaEditor('html.get');
    var model = {
        Id: Skils,
        AssignUser: AssignSkillsId,
        EmployeeUserId: CurrentId,
        EndrosementId: Endrosement,
        AssignSkillsId: $("#AssignSkillsId").val(),
        comments: comment
    }
    if (!isError) {
        $.ajax({
            type: "POST",
            data: JSON.stringify(model),
            url: constantSet.AssignskillsUrl,
            contentType: "application/json",
            success: function (data) {
                $("#EndrosementId").val(data.EndrosementId);
                $("#page_content").find("#AssignskillsListRecords").html('');
                $("#page_content").find("#AssignskillsListRecords").html(data);
                $("#page_content").find("#EditAskForEndorsementModelSkill").hide();
                $(".hrtoolLoader").hide();
                $(".modal-backdrop").hide();
                if (Endrosement > 0) {
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
}
$("#page_content").find('#AssignskillsListRecords').on('click', '.btn-delete-Endrosement', function () {

    var IdRecord = $("#AssignskillsListRecords").find("#EndrosementTableList tbody").find('.selected').attr("id");
    var employeesId = $("#page_content_inner").find("#EmployeeUserId").val();
    $("#CommentId").val(IdRecord)
    $.Zebra_Dialog("Are you sure want to delete this records?", {
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
                        url: constantSet.DeleteEndrosment,
                        data: { Id: IdRecord, Employee: employeesId },
                        success: function (data) {
                            $("#EndrosementId").val(data.EndrosementId);
                            $("#page_content").find('#AssignskillsListRecords').html('');
                            $("#page_content").find('#AssignskillsListRecords').html(data);
                            $('[data-toggle="tooltip"]').tooltip();
                            $("#page_content").find('#AssignskillsListRecords').find('div#froala-editor').froalaEditor({
                                toolbarButtons: ['bold', 'italic', 'underline', 'strikeThrough', 'subscript', 'superscript', 'fontFamily', 'fontSize', '|', 'specialCharacters', 'color', 'emoticons', 'inlineStyle', 'paragraphStyle', '|', 'paragraphFormat', 'align', 'formatOL', 'formatUL', 'outdent', 'indent', '-', 'quote', 'insertHR', 'insertLink', 'insertImage', 'insertVideo', 'insertFile', 'insertTable', '|', 'undo', 'redo', 'clearFormatting', 'selectAll', 'html', 'applyFormat', 'removeFormat', 'fullscreen', 'print', 'help'],
                                //toolbarButtons: ['bold', 'italic', 'underline', 'formatUL'],
                                pluginsEnabled: null
                            });
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
$("#page_content").on('click', '.btn-delete-TechnicalSkills', function () {

    var techID = $("#tableDivResource").find("#TechnicalSkillsTables tbody").find('.selected').attr("id");
    var EmpId = $('#EmployeeUserId').val();
    var IdRecord = $(this).attr('data-id');
    $("#CommentId").val(IdRecord)
    $.Zebra_Dialog("are you sure want to Delete this Record?", {
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
                        url: constantSet.DeleteDataTechnical,
                        data: { Id: 0, EmployeeID: $('#EmployeeUserId').val(), techID: techID },
                        success: function (data) {
                            $("#tableDiv").html('');
                            $("#tableDiv").html(data);
                            $(".hrtoolLoader").hide();
                            $(".modal-backdrop").hide();
                            $(".toast-error").show();
                            setTimeout(function () { $(".toast-error").hide(); }, 1500);
                            window.location.reload();

                        }
                    });
                }
            }]
    });
});
$("#page_content").on('click', '.btn-delete-GeneralSkills', function () {
    var IdRecord = $(this).attr('data-id');
    var generalid = $("#tableDivResource").find("#GeneralSkillsTables tbody").find('.selected').attr("id");
    $("#CommentId").val(IdRecord)
    $.Zebra_Dialog("are you sure want to Delete this Record?", {
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
                        url: constantSet.DeleteDataGeneral,
                        data: { Id: 0, EmployeeID: $('#EmployeeUserId').val(), generalid: generalid },
                        success: function (data) {
                            $("#tableDiv").html('');
                            $("#tableDiv").html(data);
                            $(".hrtoolLoader").hide();
                            $(".modal-backdrop").hide();
                            $(".toast-error").show();
                            setTimeout(function () { $(".toast-error").hide(); }, 1500);
                            window.location.reload();

                        }
                    });
                }
            }]
    });
});
$("#page_content").on('click', '.Listskilss', function () {
    $("#page_content").find("#Validskillss").hide();

    $("#page_content").find(".Listskilss").css('border-color', 'rgba(237, 237, 237, 1)');
    var id = $(this).attr("id");
    $("#SelectGeneralskill_" + id).prop('checked', true);
    $("#page_content").find(".Listskilss").removeClass("active");
    $(this).addClass('active');
    $(this).css('border-color', 'rgba(74, 127, 173, 1)');
});
$("#page_content").on('change', 'input[type=radio][name=Skills]', function () {
    //$("#page_content").find("#AskForEndorsementModel").find("#AskForEndorsementBody").find('').change(function () {
    var test = $(this).val();
    $("div.desc").hide();
    $("#page_content").find("#technicalDiv" + test).show();
});
$("#page_content").find('#AssignskillsListRecords').on('click', '.btn-Refresh-Endrosement', function () {

    window.location.reload();
});
$("#page_content").on('click', '#AskSomeBodyEndorsement', function () {
    $.ajax({
        url: constantSet.AskSomebodyEndorsementUrl,
        data: {},
        success: function (data) {

            $("#page_content").find('#EditAskForEndorsementModelSkillBody').html('');
            $("#page_content").find('#EditAskForEndorsementModelSkillBody').html(data);
            $('[data-toggle="tooltip"]').tooltip();
            $("#page_content").find("#EditAskForEndorsementModelSkill").find('#wizard').smartWizard({
                onLeaveStep: EditleaveAStepCallback,
                onFinish: EditonFinishCallback
            });
            $("#page_content").find('#EditAskForEndorsementModelSkill').find("#drpEmployee").selectList();
            $("#page_content").find('#EditAskForEndorsementModelSkill').find('.buttonNext').addClass('btn btn-warning');
            $("#page_content").find('#EditAskForEndorsementModelSkill').find('.buttonPrevious').addClass('btn btn-warning');
            $("#page_content").find('#EditAskForEndorsementModelSkill').find('.buttonFinish').addClass('btn btn-success');
            $("#page_content").find('#EditAskForEndorsementModelSkill').find('.buttonPrevious').hide();
            $("#page_content").find('#EditAskForEndorsementModelSkill').find('.buttonFinish').hide();
            $("#page_content").find('#EditAskForEndorsementModelSkill').find('div#froala-editor').froalaEditor({
                toolbarButtons: ['bold', 'italic', 'underline', 'strikeThrough', 'subscript', 'superscript', 'fontFamily', 'fontSize', '|', 'specialCharacters', 'color', 'emoticons', 'inlineStyle', 'paragraphStyle', '|', 'paragraphFormat', 'align', 'formatOL', 'formatUL', 'outdent', 'indent', '-', 'quote', 'insertHR', 'insertLink', 'insertImage', 'insertVideo', 'insertFile', 'insertTable', '|', 'undo', 'redo', 'clearFormatting', 'selectAll', 'html', 'applyFormat', 'removeFormat', 'fullscreen', 'print', 'help'],
                //toolbarButtons: ['bold', 'italic', 'underline', 'formatUL'],
                pluginsEnabled: null
            });
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
        }
    });
});
$("#page_content").on('click', '#FilterbySkillsEndorsedbyyou', function () {
    var Empid = $('#EmployeeUserId').val();
    $.ajax({
        url: constantSet.getResourceListBy,
        data: { EmpId: Empid },
        success: function (data) {
            $("#AssignskillsListRecords").html('');
            $("#AssignskillsListRecords").html(data);
        }
    });
});
//$("#page_content").on("change", '#drpJobtitle', function () {
//    var Empid = $('#EmployeeUserId').val();
//    var JobId=$('#drpJobtitle').val();
//    $.ajax({
//        url: constantSet.getFilterByJobTitle,
//        data: { EmpID: Empid, jobId: JobId },
//        success: function (data) {
//            $("#AssignskillsListRecords").html('');
//            $("#AssignskillsListRecords").html(data);
//        }
//    });
//});

//$("#page_content").on("change", '#empNameText', function () {
//    var Empid = $('#EmployeeUserId').val();
//    var SelID = $('#selectID').val();
//    $.ajax({
//        url: constantSet.getFilterByName,
//        data: { Id: Empid, EmpId: SelID },
//        success: function (data) {
//            $("#AssignskillsListRecords").html('');
//            $("#AssignskillsListRecords").html(data);
//        }
//    });
//})

//$("#page_content").on("change", '#drpSkill', function () {
//    var Empid = $('#EmployeeUserId').val();
//    var Skid = $('#drpSkill').val();
//    $.ajax({
//        url: constantSet.getFilterBySkillId,
//        data: { EmplId: Empid, ESkillId: Skid },
//        success: function (data) {
//            $("#AssignskillsListRecords").html('');
//            $("#AssignskillsListRecords").html(data);
//        }
//    });
//})

//$("#page_content").on("change", '#drpPool', function () {
//    var Empid = $('#EmployeeUserId').val();
//    var pid = $('#drpPool').val();
//    $.ajax({
//        url: constantSet.skillFilterByPool,
//        data: { Id: Empid, PoolId: pid },
//        success: function (data) {
//            $("#AssignskillsListRecords").html('');
//            $("#AssignskillsListRecords").html(data);
//        }
//    });
//})

function FilterByAll() {
    var JobId = $('#drpJobtitle').val();
    var Skid = $('#drpSkill').val();
    var Empid = $('#EmployeeUserId').val();
    var pid = $('#drpPool').val();
    var SelID = $('#selectID').val();
    $.ajax({
        url: constantSet.getSkillFilterByAll,
        data: { EmpID: Empid, jobId: JobId, skiId: Skid, poolId: pid, selEmpId: SelID },
        success: function (data) {
            $("#AssignskillsListRecords").html('');
            $("#AssignskillsListRecords").html(data);
        }
    });
}