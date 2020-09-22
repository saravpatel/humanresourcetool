function DataTableDesign() {
    var tableGenerallist = $('#EndrosementTableList').DataTable({
        "sDom": '<"top"i>rt<"bottom"flp><"clear">'
    });
    $('#AssignskillsListRecords').find('.dataTables_filter').hide();
    $('#AssignskillsListRecords').find('.dataTables_info').hide();

}

$(document).ready(function () {
    DataTableDesign();
});

$("#page_content").on('click', '#AskForEndorsement', function () {
    debugger;
    $.ajax({
        url: constantSet.AskForEndorsementUrl,
        data: {},
        success: function (data) {
            
            $("#page_content").find('#AskForEndorsementBody').html('');
            $("#page_content").find('#AskForEndorsementBody').html(data);
            fillSkillDataAskForEndorsement(1);
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
        debugger;
        var iserror = false;
        var set = $('.Listskilss.active').length;
        if (set == 0) {
            iserror = true;
            $("#Validskillss").show();
            
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

        //var iserror = false;
        //if (iserror) {
        //    $(".hrtoolLoader").hide();
        //    $(".modal-backdrop").hide();
        //    return false;
        //}
        //else {
        //    $("#page_content").find('#AskForEndorsementBody').find('.buttonNext').hide();
        //    $("#page_content").find('#AskForEndorsementBody').find('.buttonPrevious').show();
        //    $("#page_content").find('#AskForEndorsementBody').find('.buttonFinish').show();
        //    return true;
        //}
    }
    else {
        $("#page_content").find('#AskForEndorsementBody').find('.buttonNext').show();
        $("#page_content").find('#AskForEndorsementBody').find('.buttonPrevious').hide();
        $("#page_content").find('#AskForEndorsementBody').find('.buttonFinish').hide();
        return true;
    }
};
function fillSkillDataAskForEndorsement(Id) {
    $.ajax({
        url: constantSet.bindSkillData,
        data: { id: Id },
        success: function (data) {
            $("#page_content").find("#AskForEndorsementBody").find('#technicalDiv1').html('');
            $("#page_content").find("#AskForEndorsementBody").find('#technicalDiv1').html(data);
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
        }
    });
}
function fillSkillDataEndorsesomebody(Id) {
    $.ajax({
        url: constantSet.bindSkillData,
        data: { id: Id },
        success: function (data) {
            $("#page_content").find('#EditAskForEndorsementModelSkillBody').find('#technicalDiv1').html('');
            $("#page_content").find('#EditAskForEndorsementModelSkillBody').find('#technicalDiv1').html(data);
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
        }
    });
}
function onFinishCallback() {
    //var Skils = $("#page_content").find('input[name=Skills]:checked').val();
    var Skils = $(".Listskilss.active").attr('id');
    //var AssignSkillsId = [];
    //$.each($("#page_content").find("#drpEmployee").parent().find(".selectlist-item"), function () {
    //    AssignSkillsId.push($("#page_content").find("#drpEmployee").parent().find(".selectlist-select").find('option:contains(' + $(this).html().trim() + ')')[0].value);
    //})
    //var Assignskills = AssignSkillsId.join(',');
    var CurrentId = $("#page_content").find('#EmployeeUserId').val();
    var Endrosement = $("#AskForEndorsementBody").find("#EditEndrosemenetId").val();
    var email = $("#AskForEndorsementBody").find("#emailid").val();
    var eid = $("#drpEmployee").val();
    var EmpName = $("#name").val();
    var isError = false;
    var emailvalidation = isValidEmailAddress(email);
    debugger;
    if (($('#InvitationMail').prop("checked") == true)) {
        if (EmpName == "") {
            isError = true; $("#AskForEndorsementBody").find("#lbl-error-Name").show();
        }
        if (email == "") {
            isError = true; $("#AskForEndorsementBody").find("#lbl-error-Email").show();
        }
        if (!emailvalidation) {
            isError = true; $("#AskForEndorsementBody").find("#lbl-error-ValidEmail").show();
        }
    }
    else {
        if (eid =="") {
            isError = true;
            $("#AskForEndorsementBody").find("#ValidEmployee").show();
        }
    }
    if (eid == "") {
        isError = true;
        $("#AskForEndorsementBody").find("#ValidEmployee").show();
    }
        
    if(!isError) {
        var model = {
            Id: Skils,
            AssignUser: $("#drpEmployee").val(),
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
            }
        });
    }
}

$("#page_content").on('click', '.Listskilss', function () {
    $("#Validskillss").hide();

    $("#page_content").find(".Listskilss").css('border-color', 'rgba(237, 237, 237, 1)');
    var id = $(this).attr("id");
    $("#SelectGeneralskill_" + id).prop('checked', true);
    $("#page_content").find(".Listskilss").removeClass("active");
    $(this).addClass('active');
    $(this).css('border-color', 'rgba(74, 127, 173, 1)');
});
$("#page_content").on('click', '#cancelDetailViewModel', function () {
    var flag = $("#isModalEdit").val();
    if (flag == 1) {
        $("#EditAskForEndorsementModelSkill").modal('show');
    }
    else {
        $("#AskForEndorsementModel").modal('show');
    }
    $("#ViewTask_Details").modal('hide');
})
$("#page_content").on('click', '.ViewRecordOfSkills', function () {
    var Gender = $('input[name=Skills]:checked').val();
    var user = $("#page_content_inner").find("#EmployeeUserId").val();
    var flag = $("#isModalEdit").val();
    var techid = $(this).attr("data-id");
    // var techid = $("#tableDivResource").find("#TechnicalSkillsTables tbody").find('.selected').attr("id");
    if (Gender == "1") {
        var Generalid = $(this).attr("data-id");
        $.ajax({
            url: constantSet.GenerallUrl,
            data: { GeneralId: Generalid },
            success: function (data) {
                $("#ViewTask_Details").find('#ViewSkillsDetailsBody').html('');
                $("#ViewTask_Details").find('#ViewSkillsDetailsBody').html(data);
                $("#AskForEndorsementModel").modal('hide');
                $("#EditAskForEndorsementModelSkill").modal('hide');
                $("#ViewTask_Details").modal('show');
                $(".hrtoolLoader").hide();
                
                //$(".modal-backdrop").hide();
            }
        });
    }
    else {
        $.ajax({
            url: constantSet.technicalUrl,
            data: { TechanicalId: techid },
            success: function (data) {
                $("#ViewTask_Details").find('#ViewSkillsDetailsBody').html('');
                $("#ViewTask_Details").find('#ViewSkillsDetailsBody').html(data);
                $("#AskForEndorsementModel").modal('hide');
                $("#EditAskForEndorsementModelSkill").modal('hide');
                $("#ViewTask_Details").modal('show');
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
                        data: { Id: IdRecord },
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
    
    var id = $("#AssignskillsListRecords").find("#EndrosementTableList tbody").find('.selected').attr("id");
    $.ajax({
        url: constantSet.EditAssignskillsUrl,
        data: { Id: id },
        success: function (data) {
            $("#EditEndrosemenetId").val(data.EndrosementId)
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
$("#page_content").find('#AssignskillsListRecords').on('click', '.btn-delete-Endrosement', function () {
    
    var IdRecord = $("#AssignskillsListRecords").find("#EndrosementTableList tbody").find('.selected').attr("id");
    $("#CommentId").val(IdRecord)
    $.Zebra_Dialog(" are you sure want to Delete this Records!?", {
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
                        data: { Id: IdRecord },
                        success: function (data) {
                            $("#EndrosementId").val(data.EndrosementId);
                            $("#page_content").find("#AssignskillsListRecords").html('');
                            $("#page_content").find("#AssignskillsListRecords").html(data);
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
$("#page_content").on('click', '#AskSomeBodyEndorsement', function () {
    $.ajax({
        url: constantSet.AskForEndorsementUrl,
        data: {},
        success: function (data) {
            $("#page_content").find('#AskForEndorsementBody').html('');
            $("#page_content").find('#AskForEndorsementBody').html(data);
            $('[data-toggle="tooltip"]').tooltip();
            fillSkillDataEndorsesomebody(1);
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

$("#page_content").on("click", "#ResetAskSomeBodyEndorsement", function () {
    window.location.reload();
});
$("#page_content").on("click", "#FilterAskForEndorsement", function () {
   
    var SkillsetId = $("#drpSkillsSet").val();
    var ResourceId = $("#drpResource").val();
    var BusinessID = $("#drpBusiness").val();
    if (BusinessID == null) {
        BusinessID = 0;
    }
    var DivisionID = $("#drpDivision").val();
    if (DivisionID == null) {
        DivisionID = 0;
    }
    var PoolID = $("#drpPool").val();
    if (PoolID == null) {
        PoolID = 0;
    }
    var FunctionID = $("#drpFunction").val();
    if (FunctionID == null) {
        FunctionID = 0;
    }
    $.ajax({
        url: constantSet.FilterSkillsEndrocement,
        data: { SkillsSet: SkillsetId, Resourece: ResourceId, Bussiness: BusinessID, Pool: PoolID, Funcation: FunctionID },
        success: function (data) {
            
            $("#EditEndrosemenetId").val(data.EndrosementId)
            $("#page_content").find('#AssignskillsListRecords').html('');
            $("#page_content").find('#AssignskillsListRecords').html(data);

            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
        }
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
            $('#EditAskForEndorsementModelSkillBody').html('');
            $("#EditAskForEndorsementModelSkill").find('#EditAskForEndorsementModelSkillBody').html(data);
            $('[data-toggle="tooltip"]').tooltip();
            $("#EditAskForEndorsementModelSkill").find("#EditAskForEndorsementModelSkillBody").find('#wizard').smartWizard({
                onLeaveStep: EditleaveAStepCallback,
                onFinish: EditonFinishCallback
            });
            $("#EditAskForEndorsementModelSkill").find('#EditAskForEndorsementModelSkillBody').find('div#froala-editor').froalaEditor({
                toolbarButtons: ['bold', 'italic', 'underline', 'strikeThrough', 'subscript', 'superscript', 'fontFamily', 'fontSize', '|', 'specialCharacters', 'color', 'emoticons', 'inlineStyle', 'paragraphStyle', '|', 'paragraphFormat', 'align', 'formatOL', 'formatUL', 'outdent', 'indent', '-', 'quote', 'insertHR', 'insertLink', 'insertImage', 'insertVideo', 'insertFile', 'insertTable', '|', 'undo', 'redo', 'clearFormatting', 'selectAll', 'html', 'applyFormat', 'removeFormat', 'fullscreen', 'print', 'help'],
                //toolbarButtons: ['bold', 'italic', 'underline', 'formatUL'],
                pluginsEnabled: null
            });            
            var htmlString = $("#EditEndrosemeneComments").val();
            $("#EditAskForEndorsementModelSkill").find('#EditAskForEndorsementModelSkillBody').find('div#froala-editor').froalaEditor('html.set', htmlString);
            //$('#EditAskForEndorsementModelSkillBody').find("#drpEmployee").selectList();
            $("#EditAskForEndorsementModelSkill").find('#EditAskForEndorsementModelSkillBody').find('.buttonNext').addClass('btn btn-warning');
            $("#EditAskForEndorsementModelSkill").find('#EditAskForEndorsementModelSkillBody').find('.buttonPrevious').addClass('btn btn-warning');
            $("#EditAskForEndorsementModelSkill").find('#EditAskForEndorsementModelSkillBody').find('.buttonFinish').addClass('btn btn-success');
            $("#EditAskForEndorsementModelSkill").find('#EditAskForEndorsementModelSkillBody').find('.buttonPrevious').hide();
            $("#EditAskForEndorsementModelSkill").find('#EditAskForEndorsementModelSkillBody').find('.buttonFinish').hide();
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
        }
    });
});
function EditleaveAStepCallback(obj, context) {
    if (context.fromStep == 1) {
        debugger;
        var iserror = false;
        var Skils = $(".Listskilss").attr('id');
        if (Skils == "" || Skils == null || Skils == undefined)
        {
            iserror = true;
            $("#Validskillss").show();
        }
        var set = $("#page_content").find('input[type=radio][name=GeneralSkillSet]:checked').val();
        if (set == undefined) {
            iserror = true;
            $("#Validskillss").show();
        }
        if (iserror) {
            return false;
        }
        else {
            if (context.toStep = 2) {
                $("#page_content").find('#EditAskForEndorsementModelSkillBody').find('.buttonNext').show();
                $('.buttonPrevious').show();
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
        debugger;
        if (context.toStep == 1) {
            $("#page_content").find('#EditAskForEndorsementModelSkillBody').find('.buttonNext').show();
            $('.buttonPrevious').hide();
            $('.buttonFinish').hide();
        }
        else {
            $('.buttonNext').hide();
            $('.buttonPrevious').show();
            $('.buttonFinish').show();
        }
        return true;
    }
    if (context.fromStep == 3) {
        debugger;
        if (context.toStep == 2) {
            $('.buttonNext').show();
            $('.buttonPrevious').show();
            $('.buttonFinish').hide();
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
            $('.buttonFinish').hide();
        }
        else {
            $("#page_content").find('#EditAskForEndorsementModelSkillBody').find('.buttonNext').show();
            $('.buttonPrevious').hide();
            $('.buttonFinish').hide();
        }
        return true;
    }
};
function EditonFinishCallback() {
    //var Skils = $("#page_content").find('input[name=GeneralSkillSet]:checked').val();
    //var Skils = $(".Listskilss.active").attr('id');
    var Skils = $(".Listskilss.active").attr('id');
    var eid = $("#drpEmployeeSkill").val();
    var isError = false;
    if (eid == "")
    {
        isError = true;
        $('#EditAskForEndorsementModelSkillBody').find("#ValidEmployee").show();
    }
    //var AssignSkillsId = [];
    //$.each($("#page_content").find("#drpEmployee").parent().find(".selectlist-item"), function () {
    //    AssignSkillsId.push($("#page_content").find("#drpEmployee").parent().find(".selectlist-select").find('option:contains(' + $(this).html().trim() + ')')[0].value);
    //})
    //var Assignskills = AssignSkillsId.join(',');
    var CurrentId = $("#page_content").find('#EmployeeUserId').val();
    var Endrosement = $("#EditAskForEndorsementModelSkillBody").find("#EditEndrosemenetId").val();
    var comment = $("#page_content").find('#EditAskForEndorsementModelSkillBody').find('div#froala-editor').froalaEditor('html.get');
    var model = {
        Id: Skils,
        AssignUser: $("#drpEmployeeSkill").val(),
        EmployeeUserId: CurrentId,
        EndrosementId: Endrosement,
        AssignSkillsId: $("#AssignSkillsId").val(),
        comments: comment        
    }
    //url: constantSet.AssignskillsUrl,
    if (!isError) {
        $.ajax({
            type: "POST",
            data: JSON.stringify(model),      
            url: constantSet.bindEndorPreviewData,
            contentType: "application/json",
            success: function (data) {
                $("#PreviewRecipientEndorsementModelSkill").find("#PreviewRecipientEndorsementModelSkillBody").html('');
                $("#PreviewRecipientEndorsementModelSkill").find("#PreviewRecipientEndorsementModelSkillBody").html(data);
                $("#PreviewRecipientEndorsementModelSkill").modal('show');
                $('#PreviewRecipientEndorsementModelSkill').find('div#froala-editor').froalaEditor({
                    //toolbarButtons: ['bold', 'italic', 'underline', 'strikeThrough', 'subscript', 'superscript', 'fontFamily', 'fontSize', '|', 'specialCharacters', 'color', 'emoticons', 'inlineStyle', 'paragraphStyle', '|', 'paragraphFormat', 'align', 'formatOL', 'formatUL', 'outdent', 'indent', '-', 'quote', 'insertHR', 'insertLink', 'insertImage', 'insertVideo', 'insertFile', 'insertTable', '|', 'undo', 'redo', 'clearFormatting', 'selectAll', 'html', 'applyFormat', 'removeFormat', 'fullscreen', 'print', 'help'],
                    ////toolbarButtons: ['bold', 'italic', 'underline', 'formatUL'],
                    //pluginsEnabled: null
                    contenteditable:false
                });
                var htmlString = $("#EditEndrosemenComments").val();
                $("#PreviewRecipientEndorsementModelSkill").find("div#froala-editor").froalaEditor('html.set', htmlString);
                //$("#EndrosementId").val(data.EndrosementId);
                //$("#page_content").find("#AssignskillsListRecords").html('');
                //$("#page_content").find("#AssignskillsListRecords").html(data);
                //$("#page_content").find("#EditAskForEndorsementModelSkill").hide();
                //$(".hrtoolLoader").hide();
                //$(".modal-backdrop").hide();
                //if (Endrosement > 0) {
                //    $(".toast-info").show();
                //    setTimeout(function () { $(".toast-info").hide(); }, 1500);
                //}
                //else {
                //    $(".toast-success").show();
                //    setTimeout(function () { $(".toast-success").hide(); }, 1500);
                //}

            }
        });
    }
}
$("#page_content").on('click', '#btn-Project-PostModel', function () {
    var Skils = $(".Listskilss.active").attr('id');
    var eid = $("#drpEmployeeSkill").val();
    var isError = false;   
    var CurrentId = $("#page_content").find('#EmployeeUserId').val();
    var Endrosement = $("#EditAskForEndorsementModelSkillBody").find("#EditEndrosemenetId").val();
    var comment = $("#page_content").find('#EditAskForEndorsementModelSkillBody').find('div#froala-editor').froalaEditor('html.get');
    var model = {
        Id: Skils,
        AssignUser: $("#drpEmployeeSkill").val(),
        EmployeeUserId: CurrentId,
        EndrosementId: Endrosement,
        AssignSkillsId: $("#AssignSkillsId").val(),
        comments: comment    
    }
   
    $.ajax({
        type: "POST",
        data: JSON.stringify(model),
        url: constantSet.AssignskillsUrl,
        contentType: "application/json",
        success: function (data) {
            debugger;
            $("#page_content").find("#AssignskillsListRecords").html('');
            $("#page_content").find("#AssignskillsListRecords").html(data);
            $("#PreviewRecipientEndorsementModelSkill").modal('hide');
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
            $("#page_content").find('#EditAskForEndorsementModelSkill').find('div#froala-editor').froalaEditor({
                toolbarButtons: ['bold', 'italic', 'underline', 'strikeThrough', 'subscript', 'superscript', 'fontFamily', 'fontSize', '|', 'specialCharacters', 'color', 'emoticons', 'inlineStyle', 'paragraphStyle', '|', 'paragraphFormat', 'align', 'formatOL', 'formatUL', 'outdent', 'indent', '-', 'quote', 'insertHR', 'insertLink', 'insertImage', 'insertVideo', 'insertFile', 'insertTable', '|', 'undo', 'redo', 'clearFormatting', 'selectAll', 'html', 'applyFormat', 'removeFormat', 'fullscreen', 'print', 'help'],
                //toolbarButtons: ['bold', 'italic', 'underline', 'formatUL'],
                pluginsEnabled: null
            });
            $("#page_content").find('#EditAskForEndorsementModelSkill').find("#drpEmployee").selectList();
            $("#page_content").find('#EditAskForEndorsementModelSkill').find('.buttonNext').addClass('btn btn-warning');
            $("#page_content").find('#EditAskForEndorsementModelSkill').find('.buttonPrevious').addClass('btn btn-warning');
            $("#page_content").find('#EditAskForEndorsementModelSkill').find('.buttonFinish').addClass('btn btn-success');
            $("#page_content").find('#EditAskForEndorsementModelSkill').find('.buttonPrevious').hide();
            $("#page_content").find('#EditAskForEndorsementModelSkill').find('.buttonFinish').hide();
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
        }
    });
});

$("#page_content").on("change", "#drpDivision", function () {
    var value = $(this).val();
    if (value != "0") {
        $.ajax({
            url: constantSet.bindpool,
            data: { DivisionId: value },
            success: function (data) {
                $("#drp-Pool").html('');
                var toAppend = '';
                toAppend += "<option value='0'>All</option>";
                $.each(data, function (index, item) {
                    toAppend += "<option value='" + item.Id + "'>" + item.Name + "</option>";
                })
                $("#drpPool").html(toAppend);
                if ($("#drpPool").val() == 0) {
                    $("#drpPool").val(0);
                }
                $.ajax({
                    url: constantSet.bindFuncation,
                    data: { DivisionId: value },
                    success: function (data) {
                        $("#drp-Function").html('');
                        var toAppend = '';
                        toAppend += "<option value='0'>All</option>";
                        $.each(data, function (index, item) {
                            toAppend += "<option value='" + item.Id + "'>" + item.Name + "</option>";
                        })
                        $("#drpFunction").html(toAppend);
                        if ($("#drpFunction").val() == 0) {
                            $("#drpFunction").val(0);
                        }
                    }
                });
            }
        });
    }
    else {

        $('#drpPool').empty();
        // Bind new values to dropdown
        $('#drpPool').each(function () {
            // Create option
            var option = $("<option />");
            option.attr("value", '0').text('--Select--');
            $('#drpPool').append(option);
        });
        $('#drpFunction').empty();
        // Bind new values to dropdown
        $('#drpFunction').each(function () {
            // Create option
            var option = $("<option />");
            option.attr("value", '0').text('--Select--');
            $('#drpFunction').append(option);
        });
    }
});
$("#page_content").on("change", "#drpBusiness", function () {
    var value = $(this).val();
    if (value != "0") {
        $.ajax({
            url: constantSet.bindDiv,
            data: { businessId: value },
            success: function (data) {
                $("#drpDivision").html('');
                var toAppend = '';
                toAppend += "<option value='0'>All</option>";
                $.each(data, function (index, item) {
                    toAppend += "<option value='" + item.Id + "'>" + item.Name + "</option>";
                })
                $("#drpDivision").html(toAppend);
                if ($("#drpDivision").val() == 0)
                {
                    $("#drpDivision").val(0);
                    $('#drpPool').val(0);
                    $('#drpFunction').val(0);
                }
            }
        });
    }
    else {
        $('#drpDivision').empty();
        // Bind new values to dropdown
        $('#drpDivision').each(function () {
            // Create option
            var option = $("<option />");
            option.attr("value", '0').text('--Select--');
            $('#drpDivision').append(option);
        });
        $('#drpPool').empty();
        // Bind new values to dropdown
        $('#drpPool').each(function () {
            // Create option
            var option = $("<option />");
            option.attr("value", '0').text('--Select--');
            $('#drpPool').append(option);
        });
        $('#drpFunction').empty();
        // Bind new values to dropdown
        $('#drpFunction').each(function () {
            // Create option
            var option = $("<option />");
            option.attr("value", '0').text('--Select--');
            $('#drpFunction').append(option);
        });
    }
});