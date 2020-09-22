$(document).ready(function () {
    
    $("#tableDiv").find('div#froala-editor').froalaEditor({
        //toolbarButtons: ['bold', 'italic', 'underline', 'strikeThrough', 'subscript', 'superscript', 'fontFamily', 'fontSize', '|', 'specialCharacters', 'color', 'emoticons', 'inlineStyle', 'paragraphStyle', '|', 'paragraphFormat', 'align', 'formatOL', 'formatUL', 'outdent', 'indent', '-', 'quote', 'insertHR', 'insertLink', 'insertImage', 'insertVideo', 'insertFile', 'insertTable', '|', 'undo', 'redo', 'clearFormatting', 'selectAll', 'html', 'applyFormat', 'removeFormat', 'fullscreen', 'print', 'help'],
        toolbarButtons: ['bold', 'italic', 'underline', 'formatUL'],
        pluginsEnabled: null
    });
    $(".cv_btn_last").addClass("activate").removeClass("deactivate");
    $(".btn-add-resume").addClass("hide");
    var text = $("#tableDiv").find("#resume_test").val();
    $("#tableDiv").find('div#froala-editor').froalaEditor('html.set', text);

});

//Add/Edit Button Click 
$(".add_edit_btn").on('click', function () {
    $(".fa-minus-circle, .fa-pencil, .heading_a, .resume_person_details, .resume_cv_details, .btn-add-resume").removeClass("hide");
    $("#resumetext, #resumetextArea").addClass("hide");
    $(".activate").addClass("deactivate").removeClass("activate");
    $(".add_edit_btn").addClass("activate").removeClass("deactivate");

});

//for Resume
$(".resume_btn").on('click', function () {
    
    $(".fa-minus-circle, .fa-pencil, .heading_a, .resume_person_details, .resume_cv_details").addClass("hide");
    $("#resumetext, #resumetextArea, .btn-add-resume").removeClass("hide");
    $(".activate").addClass("deactivate").removeClass("activate");
    $(".resume_btn").addClass("activate").removeClass("deactivate");

});

//For cv show 
$(".cv_btn_last").on('click', function () {
    $(".fa-minus-circle, .fa-pencil, #resumetext, #resumetextArea, .btn-add-resume").addClass("hide");
    $(".heading_a, .resume_person_details, .resume_cv_details").removeClass("hide");
    $(".activate").addClass("deactivate").removeClass("activate");
    $(".cv_btn_last").addClass("activate").removeClass("deactivate");
});


//Add/Edit Work Experience
$("#tableDiv").on('click', '#btn-submit-WorkExprience', function () {
    $(".hrtoolLoader").show();
    var IsError = false;
    var id = $("#tableDiv").find('#AddEditWorkExperience').find('#WorkExperienceId').val();
    var employeeId = $("#tableDiv").find('#AddEditWorkExperience').find('#Employeeid').val();
    var JobTitle = $("#tableDiv").find('#AddEditWorkExperience').find('#txt_JobTitle').val().trim();
    var CompanyName = $("#tableDiv").find('#AddEditWorkExperience').find('#txt_CompanyName').val().trim();
    var StartDate = $("#tableDiv").find('#AddEditWorkExperience').find('#datep_StartDate').val().trim();
    var EndDate = $("#tableDiv").find('#AddEditWorkExperience').find('#datep_EndDate').val().trim();
    var OtherInformation = $("#tableDiv").find('#AddEditWorkExperience').find('#txt_OtherInformation').val().trim();
    var ResumeText = $("#tableDiv").find('div#froala-editor').froalaEditor('html.get');

    if (JobTitle == "") { IsError = true; $("#tableDiv").find('#AddEditWorkExperience').find('#lbl-error-JobTitle').show(); }
    if (CompanyName == "") { IsError = true; $("#tableDiv").find('#AddEditWorkExperience').find('#lbl-error-CompanyName').show(); }

    if (IsError) {
        $(".hrtoolLoader").hide();
        $(".modal-backdrop").hide();
        return false;
    }
    else {
        var model = {
            Id: id,
            EmployeeID: employeeId,
            JobTitle: JobTitle,
            CompanyName: CompanyName,
            JobStartDate: StartDate,
            JobEndDate: EndDate,
            OtherInformation: OtherInformation

        }

        $.ajax({
            url: constantResume.SaveWorkExperirnce,
            type: 'POST',
            data: JSON.stringify(model),
            contentType: "application/json",
            success: function (data) {
                $("#tableDiv").html("");
                $("#tableDiv").html(data);
                $("#tableDiv").find('div#froala-editor').froalaEditor({
                    //toolbarButtons: ['bold', 'italic', 'underline', 'strikeThrough', 'subscript', 'superscript', 'fontFamily', 'fontSize', '|', 'specialCharacters', 'color', 'emoticons', 'inlineStyle', 'paragraphStyle', '|', 'paragraphFormat', 'align', 'formatOL', 'formatUL', 'outdent', 'indent', '-', 'quote', 'insertHR', 'insertLink', 'insertImage', 'insertVideo', 'insertFile', 'insertTable', '|', 'undo', 'redo', 'clearFormatting', 'selectAll', 'html', 'applyFormat', 'removeFormat', 'fullscreen', 'print', 'help'],
                    toolbarButtons: ['bold', 'italic', 'underline', 'formatUL'],
                    pluginsEnabled: null
                });
                $("#tableDiv").find(".btn-add-resume").addClass("hide");
                $("#tableDiv").find('#froala-editor').froalaEditor('html.set', ResumeText);
                $(".cv_btn_last").click();
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

//Work Experience Open Pop-up 
$("#tableDiv").on('click', '.btn-add-WorkExperience', function () {
    $(".hrtoolLoader").show();
    var employeeId = $("#EmployeeID").val();
    $.ajax({
        url: constantResume.AddWorkExperience,
        data: { Id: 0, employeeId: employeeId },
        success: function (data) {
            $("#tableDiv").find('#AddEditWorkExperience').html('');
            $("#tableDiv").find('#AddEditWorkExperience').html(data);

            $("#datep_StartDate").Zebra_DatePicker({
                //direction: false,
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {
                    var fromDate = $('#datep_StartDate').val();
                }
            }
);
            $("#datep_EndDate").Zebra_DatePicker({
                //direction: false,
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {
                    var fromDate = $('#datep_EndDate').val();
                }
            }
             );
            $('#datep_StartDate').Zebra_DatePicker({
                //direction: false,
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {
                    $("#lbl-error-StartDate").hide();
                    $("#lbl-error-EndDate").hide();
                    var fromDate = $('#datep_StartDate').val();
                    var toDate = $('#datep_EndDate').val();
                    if (fromDate != "") {
                        fromDate = fromDate.replace(/-/g, '/');
                        toDate = toDate.replace(/-/g, '/');
                        if (toDate != "") {
                            if (FromDateValidation(fromDate, toDate)) {
                                $("#lbl-error-StartDate").html("Start Date date is less than to End date");
                                $("#lbl-error-StartDate").show();
                                $("#datep_StartDate").val("");
                                return false;
                            }
                        }
                    }
                }
            });
            $('#datep_EndDate').Zebra_DatePicker({
                //direction: true,
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {
                    $("#lbl-error-EndDate").hide();
                    var fromDate = $('#datep_StartDate').val();
                    var toDate = $('#datep_EndDate').val();
                    if (fromDate == "") {
                        $("#lbl-error-EndDate").html("Start date is required");
                        $("#lbl-error-EndDate").show();
                        $("#datep_EndDate").val("");
                        return false;
                    }
                    else {
                        fromDate = fromDate.replace(/-/g, '/');
                        toDate = toDate.replace(/-/g, '/');
                        if (ToDateValidation(fromDate, toDate)) {
                            $("#lbl-error-EndDate").html("EndDate date is greater than Start date");
                            $("#lbl-error-EndDate").show();
                            $("#datep_EndDate").val("");
                            return false;
                        }
                    }
                }
            });


            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
        }
    });
});

//Work Experience Edit Pop-up
$("#tableDiv").on('click', '.btn-edit-WorkExperience', function () {
    $(".hrtoolLoader").show();

    var employeeId = $("#EmployeeID").val();
    var id = $(this).parent().find("#workExperienceID").val();
    $.ajax({
        url: constantResume.AddWorkExperience,
        data: { Id: id, employeeId: employeeId },
        success: function (data) {
            $("#tableDiv").find('#AddEditWorkExperience').html('');
            $("#tableDiv").find('#AddEditWorkExperience').html(data);
            $("#tableDiv").find(".workExperienceTitle")['0'].textContent = "Edit Work Experience";
            $("#tableDiv").find('#btn-submit-WorkExprience')['0'].textContent = "Save";
            $("#datep_StartDate").Zebra_DatePicker({
                //direction: false,
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {
                    var fromDate = $('#datep_StartDate').val();
                }
            }
);
            $("#datep_EndDate").Zebra_DatePicker({
                //direction: false,
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {
                    var fromDate = $('#datep_EndDate').val();
                }
            }
             );
            $('#datep_StartDate').Zebra_DatePicker({
                //direction: false,
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {
                    $("#lbl-error-StartDate").hide();
                    $("#lbl-error-EndDate").hide();
                    var fromDate = $('#datep_StartDate').val();
                    var toDate = $('#datep_EndDate').val();
                    if (fromDate != "") {
                        fromDate = fromDate.replace(/-/g, '/');
                        toDate = toDate.replace(/-/g, '/');
                        if (toDate != "") {
                            if (FromDateValidation(fromDate, toDate)) {
                                $("#lbl-error-StartDate").html("Start Date date is less than to End date");
                                $("#lbl-error-StartDate").show();
                                $("#datep_StartDate").val("");
                                return false;
                            }
                        }
                    }
                }
            });
            $('#datep_EndDate').Zebra_DatePicker({
                //direction: true,
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {
                    $("#lbl-error-EndDate").hide();
                    var fromDate = $('#datep_StartDate').val();
                    var toDate = $('#datep_EndDate').val();
                    if (fromDate == "") {
                        $("#lbl-error-EndDate").html("Start date is required");
                        $("#lbl-error-EndDate").show();
                        $("#datep_EndDate").val("");
                        return false;
                    }
                    else {
                        fromDate = fromDate.replace(/-/g, '/');
                        toDate = toDate.replace(/-/g, '/');
                        if (ToDateValidation(fromDate, toDate)) {
                            $("#lbl-error-EndDate").html("EndDate date is greater than Start date");
                            $("#lbl-error-EndDate").show();
                            $("#datep_EndDate").val("");
                            return false;
                        }
                    }
                }
            });
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
        }
    });
});

//Work Experience Delete Pop-up
$("#tableDiv").on('click', '.btn-delete-WorkExperience', function () {

    var employeeId = $("#EmployeeID").val();
    var id = $(this).parent().find("#workExperienceID").val();
    var ResumeText = $("#tableDiv").find('div#froala-editor').froalaEditor('html.get');
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
                        url: constantResume.DeleteWorkExperience,
                        data: { Id: id, EmployeeId: employeeId },
                        success: function (data) {
                            $("#tableDiv").html("");
                            $("#tableDiv").html(data);
                            $("#tableDiv").find('div#froala-editor').froalaEditor({
                                //toolbarButtons: ['bold', 'italic', 'underline', 'strikeThrough', 'subscript', 'superscript', 'fontFamily', 'fontSize', '|', 'specialCharacters', 'color', 'emoticons', 'inlineStyle', 'paragraphStyle', '|', 'paragraphFormat', 'align', 'formatOL', 'formatUL', 'outdent', 'indent', '-', 'quote', 'insertHR', 'insertLink', 'insertImage', 'insertVideo', 'insertFile', 'insertTable', '|', 'undo', 'redo', 'clearFormatting', 'selectAll', 'html', 'applyFormat', 'removeFormat', 'fullscreen', 'print', 'help'],
                                toolbarButtons: ['bold', 'italic', 'underline', 'formatUL'],
                                pluginsEnabled: null
                            });
                            $("#tableDiv").find(".btn-add-resume").addClass("hide");
                            $("#tableDiv").find('#froala-editor').froalaEditor('html.set', ResumeText);
                            $(".cv_btn_last").click();
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

//Validation
$("#tableDiv").on('keyup', '#txt_JobTitle', function (e) {
    var isError = false;
    $("#tableDiv").find('#AddEditWorkExperience').find('#lbl-error-JobTitle').hide();
});
$("#tableDiv").on('keyup', '#txt_CompanyName', function (e) {
    var isError = false;
    $("#tableDiv").find('#AddEditWorkExperience').find('#lbl-error-CompanyName').hide();
});



//Add/Edit Education
$("#tableDiv").on('click', '#btn-submit-Education', function () {
    $(".hrtoolLoader").show();
    var IsError = false;
    var id = $("#tableDiv").find('#AddEditEducation').find('#educationId').val();
    var employeeId = $("#tableDiv").find('#AddEditEducation').find('#Employeeid').val();
    var CourseName = $("#tableDiv").find('#AddEditEducation').find('#txt_CourseName').val().trim();
    var InstitutionName = $("#tableDiv").find('#AddEditEducation').find('#txt_InstitutionName').val().trim();
    var StartDate = $("#tableDiv").find('#AddEditEducation').find('#dp_StartDateE').val().trim();
    var EndDate = $("#tableDiv").find('#AddEditEducation').find('#dp_EndDateE').val().trim();
    var OtherInformation = $("#tableDiv").find('#AddEditEducation').find('#txt_OtherInformation').val().trim();

    var ResumeText = $("#tableDiv").find('div#froala-editor').froalaEditor('html.get');

    if (CourseName == "") { IsError = true; $("#tableDiv").find('#AddEditEducation').find('#lbl-error-CourseName').show(); }
    if (InstitutionName == "") { IsError = true; $("#tableDiv").find('#AddEditEducation').find('#lbl-error-InstitutionName').show(); }

    if (IsError) {
        $(".hrtoolLoader").hide();
        $(".modal-backdrop").hide();
        return false;
    }
    else {
        var model = {
            Id: id,
            EmployeeID: employeeId,
            CourseName: CourseName,
            InstitutionName: InstitutionName,
            StartDate: StartDate,
            EndDate: EndDate,
            OtherInformation: OtherInformation
        }
        $.ajax({
            url: constantResume.SaveEducation,
            type: 'POST',
            data: JSON.stringify(model),
            contentType: "application/json",
            success: function (data) {
                $("#tableDiv").html("");
                $("#tableDiv").html(data);
                $("#tableDiv").find('div#froala-editor').froalaEditor({
                    //toolbarButtons: ['bold', 'italic', 'underline', 'strikeThrough', 'subscript', 'superscript', 'fontFamily', 'fontSize', '|', 'specialCharacters', 'color', 'emoticons', 'inlineStyle', 'paragraphStyle', '|', 'paragraphFormat', 'align', 'formatOL', 'formatUL', 'outdent', 'indent', '-', 'quote', 'insertHR', 'insertLink', 'insertImage', 'insertVideo', 'insertFile', 'insertTable', '|', 'undo', 'redo', 'clearFormatting', 'selectAll', 'html', 'applyFormat', 'removeFormat', 'fullscreen', 'print', 'help'],
                    toolbarButtons: ['bold', 'italic', 'underline', 'formatUL'],
                    pluginsEnabled: null
                });
                $("#tableDiv").find(".btn-add-resume").addClass("hide");
                $("#tableDiv").find('#froala-editor').froalaEditor('html.set', ResumeText);
                $(".cv_btn_last").click();
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
                //location.reload();
            }
        });
    }
});

//Education Open Pop-up 
$("#tableDiv").on('click', '.btn-add-Education', function () {
    $(".hrtoolLoader").show();
    var employeeId = $("#EmployeeID").val();
    $.ajax({
        url: constantResume.AddEducation,
        data: { Id: 0, employeeId: employeeId },
        success: function (data) {
            $("#tableDiv").find('#AddEditEducation').html('');
            $("#tableDiv").find('#AddEditEducation').html(data);
            $("#dp_StartDateE").Zebra_DatePicker({
                //direction: false,
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {
                    var fromDate = $('#dp_StartDateE').val();
                }
            }
   );
            $("#dp_EndDateE").Zebra_DatePicker({
                //direction: false,
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {
                    var fromDate = $('#dp_EndDateE').val();
                }
            }
             );

            $('#dp_StartDateE').Zebra_DatePicker({
                //direction: false,
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {
                    $("#lbl-error-StartDate").hide();
                    $("#lbl-error-EndDate").hide();
                    var fromDate = $('#dp_StartDateE').val();
                    var toDate = $('#dp_EndDateE').val();
                    if (fromDate != "") {
                        fromDate = fromDate.replace(/-/g, '/');
                        toDate = toDate.replace(/-/g, '/');
                        if (toDate != "") {
                            if (FromDateValidation(fromDate, toDate)) {
                                $("#lbl-error-StartDate").html("Start Date date is less than to End date");
                                $("#lbl-error-StartDate").show();
                                $("#dp_StartDateE").val("");
                                return false;
                            }
                        }
                    }
                }
            });
            $('#dp_EndDateE').Zebra_DatePicker({
                //direction: true,
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {
                    $("#lbl-error-EndDate").hide();
                    var fromDate = $('#dp_StartDateE').val();
                    var toDate = $('#dp_EndDateE').val();
                    if (fromDate == "") {
                        $("#lbl-error-EndDate").html("Start date is required");
                        $("#lbl-error-EndDate").show();
                        $("#dp_EndDateE").val("");
                        return false;
                    }
                    else {
                        fromDate = fromDate.replace(/-/g, '/');
                        toDate = toDate.replace(/-/g, '/');
                        if (ToDateValidation(fromDate, toDate)) {
                            $("#lbl-error-EndDate").html("EndDate date is greater than Start date");
                            $("#lbl-error-EndDate").show();
                            $("#dp_EndDateE").val("");
                            return false;
                        }
                    }
                }
            });
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
        }
    });
});

//Education Edit Pop-up
$("#tableDiv").on('click', '.btn-edit-Education', function () {
    $(".hrtoolLoader").show();
    var employeeId = $("#EmployeeID").val();
    var id = $(this).parent().find("#EducationID").val();
    $.ajax({
        url: constantResume.AddEducation,
        data: { Id: id, employeeId: employeeId },
        success: function (data) {
            $("#tableDiv").find('#AddEditEducation').html('');
            $("#tableDiv").find('#AddEditEducation').html(data);
            $("#tableDiv").find(".educationTitle")['0'].textContent = "Edit Education";
            $("#tableDiv").find('#btn-submit-Education')['0'].textContent = "Save";
            $("#dp_StartDateE").Zebra_DatePicker({
                //direction: false,
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {
                    var fromDate = $('#dp_StartDateE').val();
                }
            }
   );
            $("#dp_EndDateE").Zebra_DatePicker({
                //direction: false,
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {
                    var fromDate = $('#dp_EndDateE').val();
                }
            }
             );

            $('#dp_StartDateE').Zebra_DatePicker({
                //direction: false,
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {
                    $("#lbl-error-StartDate").hide();
                    $("#lbl-error-EndDate").hide();
                    var fromDate = $('#dp_StartDateE').val();
                    var toDate = $('#dp_EndDateE').val();
                    if (fromDate != "") {
                        fromDate = fromDate.replace(/-/g, '/');
                        toDate = toDate.replace(/-/g, '/');
                        if (toDate != "") {
                            if (FromDateValidation(fromDate, toDate)) {
                                $("#lbl-error-StartDate").html("Start Date date is less than to End date");
                                $("#lbl-error-StartDate").show();
                                $("#dp_StartDateE").val("");
                                return false;
                            }
                        }
                    }
                }
            });
            $('#dp_EndDateE').Zebra_DatePicker({
                //direction: true,
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {
                    $("#lbl-error-EndDate").hide();
                    var fromDate = $('#dp_StartDateE').val();
                    var toDate = $('#dp_EndDateE').val();
                    if (fromDate == "") {
                        $("#lbl-error-EndDate").html("Start date is required");
                        $("#lbl-error-EndDate").show();
                        $("#dp_EndDateE").val("");
                        return false;
                    }
                    else {
                        fromDate = fromDate.replace(/-/g, '/');
                        toDate = toDate.replace(/-/g, '/');
                        if (ToDateValidation(fromDate, toDate)) {
                            $("#lbl-error-EndDate").html("EndDate date is greater than Start date");
                            $("#lbl-error-EndDate").show();
                            $("#dp_EndDateE").val("");
                            return false;
                        }
                    }
                }
            });
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
        }
    });
});

//Education Delete Pop-up
$("#tableDiv").on('click', '.btn-delete-Education', function () {
    
    var employeeId = $("#EmployeeID").val();

    var id = $(this).parent().find("#EducationID").val();
    var ResumeText = $("#tableDiv").find('div#froala-editor').froalaEditor('html.get');

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
                        url: constantResume.DeleteEducation,
                        data: { Id: id, EmployeeId: employeeId },
                        success: function (data) {
                            $("#tableDiv").html("");
                            $("#tableDiv").html(data);
                            $("#tableDiv").find('div#froala-editor').froalaEditor({
                                //toolbarButtons: ['bold', 'italic', 'underline', 'strikeThrough', 'subscript', 'superscript', 'fontFamily', 'fontSize', '|', 'specialCharacters', 'color', 'emoticons', 'inlineStyle', 'paragraphStyle', '|', 'paragraphFormat', 'align', 'formatOL', 'formatUL', 'outdent', 'indent', '-', 'quote', 'insertHR', 'insertLink', 'insertImage', 'insertVideo', 'insertFile', 'insertTable', '|', 'undo', 'redo', 'clearFormatting', 'selectAll', 'html', 'applyFormat', 'removeFormat', 'fullscreen', 'print', 'help'],
                                toolbarButtons: ['bold', 'italic', 'underline', 'formatUL'],
                                pluginsEnabled: null
                            });
                            $("#tableDiv").find(".btn-add-resume").addClass("hide");
                            $("#tableDiv").find('#froala-editor').froalaEditor('html.set', ResumeText);
                            $(".cv_btn_last").click();
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

//Validation 
$("#tableDiv").on('keyup', '#txt_CourseName', function (e) {
    var isError = false;
    $("#tableDiv").find('#AddEditEducation').find('#lbl-error-CourseName').hide();
});
$("#tableDiv").on('keyup', '#txt_InstitutionName', function (e) {
    var isError = false;
    $("#tableDiv").find('#AddEditEducation').find('#lbl-error-InstitutionName').hide();
});

//Add/Edit Qualification
$("#tableDiv").on('click', '#btn-submit-Qualification', function () {
    $(".hrtoolLoader").show();
    var IsError = false;
    var id = $("#tableDiv").find('#AddEditQualification').find('#QualificationId').val();
    var employeeId = $("#tableDiv").find('#AddEditQualification').find('#Employeeid').val();
    var Detail = $("#tableDiv").find('#AddEditQualification').find('#txt_Qualification').val().trim();
    var ResumeText = $("#tableDiv").find('div#froala-editor').froalaEditor('html.get');
    
    if (Detail == "") { IsError = true; $("#tableDiv").find('#AddEditQualification').find('#lbl-error-Qualification').show(); }
    
    if (IsError) {
        $(".hrtoolLoader").hide();
        $(".modal-backdrop").hide();
        return false;
    }
    else {
        var model = {
            Id: id,
            EmployeeID: employeeId,
            Detail: Detail

        }
        $.ajax({
            url: constantResume.SaveQualification,
            type: 'POST',
            data: JSON.stringify(model),
            contentType: "application/json",
            success: function (data) {
                $("#tableDiv").html("");
                $("#tableDiv").html(data);
                $("#tableDiv").find('div#froala-editor').froalaEditor({
                    //toolbarButtons: ['bold', 'italic', 'underline', 'strikeThrough', 'subscript', 'superscript', 'fontFamily', 'fontSize', '|', 'specialCharacters', 'color', 'emoticons', 'inlineStyle', 'paragraphStyle', '|', 'paragraphFormat', 'align', 'formatOL', 'formatUL', 'outdent', 'indent', '-', 'quote', 'insertHR', 'insertLink', 'insertImage', 'insertVideo', 'insertFile', 'insertTable', '|', 'undo', 'redo', 'clearFormatting', 'selectAll', 'html', 'applyFormat', 'removeFormat', 'fullscreen', 'print', 'help'],
                    toolbarButtons: ['bold', 'italic', 'underline', 'formatUL'],
                    pluginsEnabled: null
                });
                $("#tableDiv").find(".btn-add-resume").addClass("hide");
                $("#tableDiv").find('#froala-editor').froalaEditor('html.set', ResumeText);
                $(".cv_btn_last").click();
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
               // location.reload();
            }
        });
    }
});

//Qualification Open Pop-up 
$("#tableDiv").on('click', '.btn-add-Qualification', function () {
    $(".hrtoolLoader").show();
    var employeeId = $("#EmployeeID").val();
    $.ajax({
        url: constantResume.AddQualification,
        data: { Id: 0, employeeId: employeeId },
        success: function (data) {
            $("#tableDiv").find('#AddEditQualification').html('');
            $("#tableDiv").find('#AddEditQualification').html(data);

            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
        }
    });
});

//Qualification Edit Pop-up
$("#tableDiv").on('click', '.btn-edit-Qualification', function () {
    $(".hrtoolLoader").show();

    var employeeId = $("#EmployeeID").val();
    var id = $(this).parent().find("#QualificationID").val();
    $.ajax({
        url: constantResume.AddQualification,
        data: { Id: id, employeeId: employeeId },
        success: function (data) {
            $("#tableDiv").find('#AddEditQualification').html('');
            $("#tableDiv").find('#AddEditQualification').html(data);
            $("#tableDiv").find(".qualificationTitle")['0'].textContent = "Edit Qualification";
            $("#tableDiv").find('#btn-submit-Qualification')['0'].textContent = "Save";
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
        }
    });
});

//Qualification Delete Pop-up
$("#tableDiv").on('click', '.btn-delete-Qualification', function () {
    var employeeId = $("#EmployeeID").val();
    var id = $(this).parent().find("#QualificationID").val();
    var ResumeText = $("#tableDiv").find('div#froala-editor').froalaEditor('html.get');
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
                        url: constantResume.DeleteQualification,
                        data: { Id: id, EmployeeId: employeeId },
                        success: function (data) {
                            $("#tableDiv").html("");
                            $("#tableDiv").html(data);
                            $("#tableDiv").find('div#froala-editor').froalaEditor({
                                //toolbarButtons: ['bold', 'italic', 'underline', 'strikeThrough', 'subscript', 'superscript', 'fontFamily', 'fontSize', '|', 'specialCharacters', 'color', 'emoticons', 'inlineStyle', 'paragraphStyle', '|', 'paragraphFormat', 'align', 'formatOL', 'formatUL', 'outdent', 'indent', '-', 'quote', 'insertHR', 'insertLink', 'insertImage', 'insertVideo', 'insertFile', 'insertTable', '|', 'undo', 'redo', 'clearFormatting', 'selectAll', 'html', 'applyFormat', 'removeFormat', 'fullscreen', 'print', 'help'],
                                toolbarButtons: ['bold', 'italic', 'underline', 'formatUL'],
                                pluginsEnabled: null
                            });
                            $("#tableDiv").find(".btn-add-resume").addClass("hide");
                            $("#tableDiv").find('#froala-editor').froalaEditor('html.set', ResumeText);
                            $(".cv_btn_last").click();
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

//Validation Qualification
$("#tableDiv").on('keyup', '#txt_Qualification', function (e) {
    var isError = false;
    $("#tableDiv").find('#AddEditQualification').find('#lbl-error-Qualification').hide();

});


//Add/Edit Language
$("#tableDiv").on('click', '#btn-submit-Language', function () {
    $(".hrtoolLoader").show();
    var IsError = false;
    var id = $("#tableDiv").find('#AddEditLanguage').find('#LanguageId').val();
    var employeeId = $("#tableDiv").find('#AddEditLanguage').find('#Employeeid').val();
    var LanguageID = $("#tableDiv").find('#AddEditLanguage').find('#drp-LanguageId').val();
    var SpeakingID = $("#tableDiv").find('#AddEditLanguage').find('#drp-SpeakingId').val();
    var ListeningID = $("#tableDiv").find('#AddEditLanguage').find('#drp-ListeningId').val();
    var WritingID = $("#tableDiv").find('#AddEditLanguage').find('#drp-WritingId').val();
    var ReadingID = $("#tableDiv").find('#AddEditLanguage').find('#drp-ReadingId').val();
    var ResumeText = $("#tableDiv").find('div#froala-editor').froalaEditor('html.get');

    var model = {
        Id: id,
        EmployeeID: employeeId,
        LanguageID: LanguageID,
        SpeakingID: SpeakingID,
        ListeningID: ListeningID,
        WritingID: WritingID,
        ReadingID: ReadingID
    }
    $.ajax({
        url: constantResume.SaveLanguage,
        type: 'POST',
        data: JSON.stringify(model),
        contentType: "application/json",
        success: function (data) {
            if (data == "Error") {
                IsError = true;
                $("#tableDiv").find('#AddEditLanguage').find("#lbl-error-LanguageListExist").show();
            }
            else {
                $("#tableDiv").html("");
                $("#tableDiv").html(data);
                $("#tableDiv").find('div#froala-editor').froalaEditor({
                    //toolbarButtons: ['bold', 'italic', 'underline', 'strikeThrough', 'subscript', 'superscript', 'fontFamily', 'fontSize', '|', 'specialCharacters', 'color', 'emoticons', 'inlineStyle', 'paragraphStyle', '|', 'paragraphFormat', 'align', 'formatOL', 'formatUL', 'outdent', 'indent', '-', 'quote', 'insertHR', 'insertLink', 'insertImage', 'insertVideo', 'insertFile', 'insertTable', '|', 'undo', 'redo', 'clearFormatting', 'selectAll', 'html', 'applyFormat', 'removeFormat', 'fullscreen', 'print', 'help'],
                    toolbarButtons: ['bold', 'italic', 'underline', 'formatUL'],
                    pluginsEnabled: null
                });
                $("#tableDiv").find(".btn-add-resume").addClass("hide");
                $("#tableDiv").find('#froala-editor').froalaEditor('html.set', ResumeText);
                $(".cv_btn_last").click();
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
            //location.reload();
        }
    });

});

//Language Open Pop-up 
$("#tableDiv").on('click', '.btn-add-Language', function () {
    $(".hrtoolLoader").show();
    var employeeId = $("#EmployeeID").val();
    $.ajax({
        url: constantResume.AddLanguage,
        data: { Id: 0, employeeId: employeeId },
        success: function (data) {
            $("#tableDiv").find('#AddEditLanguage').html('');
            $("#tableDiv").find('#AddEditLanguage').html(data);
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
        }
    });
});

//Language Edit Pop-up
$("#tableDiv").on('click', '.btn-edit-Language', function () {
    $(".hrtoolLoader").show();

    var employeeId = $("#EmployeeID").val();
    var id = $(this).parent().find("#LanguageID").val();
    $.ajax({
        url: constantResume.AddLanguage,
        data: { Id: id, employeeId: employeeId },
        success: function (data) {
            $("#tableDiv").find('#AddEditLanguage').html('');
            $("#tableDiv").find('#AddEditLanguage').html(data);
            $("#tableDiv").find(".languageTitle")['0'].textContent = "Edit Language";
            $("#tableDiv").find('#btn-submit-Language')['0'].textContent = "Save";
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
        }
    });
});

//Language Delete Pop-up
$("#tableDiv").on('click', '.btn-delete-Language', function () {
    var employeeId = $("#EmployeeID").val();
    var id = $(this).parent().find("#LanguageID").val();
    var ResumeText = $("#tableDiv").find('div#froala-editor').froalaEditor('html.get');
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
                        url: constantResume.DeleteLanguage,
                        data: { Id: id, EmployeeId: employeeId },
                        success: function (data) {
                            $("#tableDiv").html("");
                            $("#tableDiv").html(data);
                            $("#tableDiv").find('div#froala-editor').froalaEditor({
                                //toolbarButtons: ['bold', 'italic', 'underline', 'strikeThrough', 'subscript', 'superscript', 'fontFamily', 'fontSize', '|', 'specialCharacters', 'color', 'emoticons', 'inlineStyle', 'paragraphStyle', '|', 'paragraphFormat', 'align', 'formatOL', 'formatUL', 'outdent', 'indent', '-', 'quote', 'insertHR', 'insertLink', 'insertImage', 'insertVideo', 'insertFile', 'insertTable', '|', 'undo', 'redo', 'clearFormatting', 'selectAll', 'html', 'applyFormat', 'removeFormat', 'fullscreen', 'print', 'help'],
                                toolbarButtons: ['bold', 'italic', 'underline', 'formatUL'],
                                pluginsEnabled: null
                            });
                            $("#tableDiv").find(".btn-add-resume").addClass("hide");
                            $("#tableDiv").find('#froala-editor').froalaEditor('html.set', ResumeText);
                            $(".cv_btn_last").click();
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

//Validation
$("#tableDiv").on('change', '#drp-LanguageId', function (e) {
    var isError = false;
    $("#tableDiv").find('#AddEditLanguage').find('#lbl-error-LanguageListExist').hide();

});

//Save Resume Text btn-add-resume 
$("#tableDiv").on('click', '.btn-add-resume', function () {
    $(".hrtoolLoader").show();
    var IsError = false;
    var id = $("#tableDiv").find('#resumeTextID').val();
    var employeeId = $("#EmployeeID").val();
    var ResumeText = $("#tableDiv").find('div#froala-editor').froalaEditor('html.get');

    if (ResumeText == "") { IsError = true; $("#tableDiv").find('#Resume_Editor').find('#lbl-error-Resume').show(); }
    
    if (IsError) {
        $(".hrtoolLoader").hide();
        $(".modal-backdrop").hide();
        return false;
    }
    else {
        var model = {
            Id: id,
            EmployeeID: employeeId,
            ResumeText: ResumeText
        }
        $.ajax({
            url: constantResume.SaveResume,
            type: 'POST',
            data: JSON.stringify(model),
            contentType: "application/json",
            success: function (data) {
                
                $("#tableDiv").html("");
                $("#tableDiv").html(data);
                $("#tableDiv").find('div#froala-editor').froalaEditor({
                    //toolbarButtons: ['bold', 'italic', 'underline', 'strikeThrough', 'subscript', 'superscript', 'fontFamily', 'fontSize', '|', 'specialCharacters', 'color', 'emoticons', 'inlineStyle', 'paragraphStyle', '|', 'paragraphFormat', 'align', 'formatOL', 'formatUL', 'outdent', 'indent', '-', 'quote', 'insertHR', 'insertLink', 'insertImage', 'insertVideo', 'insertFile', 'insertTable', '|', 'undo', 'redo', 'clearFormatting', 'selectAll', 'html', 'applyFormat', 'removeFormat', 'fullscreen', 'print', 'help'],
                    toolbarButtons: ['bold', 'italic', 'underline', 'formatUL'],
                    pluginsEnabled: null
                });
                $("#tableDiv").find(".btn-add-resume").addClass("hide");
                $("#tableDiv").find('#froala-editor').froalaEditor('html.set', ResumeText);
                $(".cv_btn_last").click();
                if (id > 0) {
                    $(".toast-info").show();
                    setTimeout(function () { $(".toast-info").hide(); }, 1500);
                }
                else {
                    $(".toast-success").show();
                    setTimeout(function () { $(".toast-success").hide(); }, 1500);
                }
                $(".hrtoolLoader").hide();
                $(".modal-backdrop").hide();

            }
        });
    }
});

//Validation
$("#tableDiv").on('keyup', '#froala-editor', function (e) {
    var isError = false;
    $("#tableDiv").find('#Resume_Editor').find('#lbl-error-Resume').hide();
});