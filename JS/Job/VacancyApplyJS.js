var table, imageData, FileUploadData;
function validDOB() {
    var dob = $("#txt_DateOfBitrh").val();
    var now = new Date();
    var a = dob.split(" ");
    var d = a[0].split("-");
    var date = new Date();
    var age = now.getFullYear() - d[2];
    if (isNaN(age) || age < 18) {
        $("#lbl-error-validdateOfBirth").show();
        $("txt_DateOfBitrh").val('');
    }
    else {        
        $("#lbl-error-validdateOfBirth").hide();
    }
}

$(document).ready(function () {
    var url = window.location.href;
    $("#txt_Link").val(url);
});

$(".editable").each(function () {
    
    this.contentEditable = true;
});

$("#page_content_inner").on('click', '.jobapply', function () {
    $(".hrtoolLoader").show();
    var VacancyId = $("#page_content_inner").find("#JobVacancyID").val();
    $.ajax({
        url: ConstantJob.Open,
        data: { Id: 0, VacancyId: VacancyId },
        success: function (data) {
            
            $("#ApplyJobBody").html('');
            $('#ApplyJobBody').html(data);

            $('[data-toggle="tooltip"]').tooltip();
            $('#ApplyJobBody').find('#wizard').smartWizard({
                onLeaveStep: onApplicantCallback,
                onFinish: onApplicantFinishCallback
            });
            $("#ApplyJobBody").find("#txt_DateOfBitrh").Zebra_DatePicker({
                //direction: false,
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {
                    $("#AddVacancyLogBody").find("lbl-error-dateOfBirth").hide();
                    $("#AddVacancyLogBody").find("lbl-error-validdateOfBirth").hide();
                    validDOB();
                }
            });

            $('#ApplyJobBody').find('.buttonNext').show();
            $('#ApplyJobBody').find('.buttonPrevious').hide();
            $('#ApplyJobBody').find('.buttonFinish').hide();

            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
        }
    });
});

function onApplicantCallback(obj, context) {
    if (context.fromStep == 1) {        
        var isError = false;
        var Id = $('#ApplyModel').find('#ApplicantHiddenId').val();
        var VacancyID = $('#ApplyModel').find('#ApplicantVacancyId').val();
        var FirstName = $('#ApplyJobBody').find('#txt_FirstName').val();
        var LastName = $('#ApplyJobBody').find('#txt_LastName').val();
        var Email = $('#ApplyJobBody').find('#txt_Email').val();
        if (Email != "") {
            var EmailValid = isValidEmailAddress(Email);
            if (!EmailValid) {
                isError = true; $('#ApplyJobBody').find("#lbl-error-EmailValid").show();
            }
            else {
                $.ajax({
                    url: ConstantJob.EmailCheck,
                    data: { Email: Email, Id: Id, VacancyId: VacancyID },
                    success: function (data) {
                        if (data == "Error") {
                            isError = true;
                            $('#ApplyModel').find("#ApplyJobBody").find("#lbl-error-EmailValid").show();

                        }
                    }
                });

            }
        }
        var GenderID = $('#ApplyJobBody').find('#drp-selGender').val();
        var DateOfBitrh = $('#ApplyJobBody').find('#txt_DateOfBitrh').val();
        if (FirstName == "") { isError = true; $('#ApplyJobBody').find("#lbl-error-FirstName").show(); }
        if (LastName == "") { isError = true; $('#ApplyJobBody').find("#lbl-error-LastName").show(); }
        if (Email == "") { isError = true; $('#ApplyJobBody').find("#lbl-error-Email").show(); }
        if (DateOfBitrh == "") { isError = true; $('#ApplyJobBody').find("#lbl-error-dateOfBirth").show(); }

        if (isError) {
            return false;
        }
        else {
            if (context.toStep = 2) {
                $('#ApplyJobBody').find('.buttonNext').show();
                $('#ApplyJobBody').find('.buttonPrevious').show();
                $("#ApplyJobBody").find('.buttonFinish').hide();
            }
            else {
                $("#ApplyJobBody").find('.buttonNext').hide();
                $("#ApplyJobBody").find('.buttonPrevious').show();
                $("#ApplyJobBody").find('.buttonFinish').show()
            }         
            $('#ApplyJobBody').find("#lbl-error-FirstName").hide();
            $('#ApplyJobBody').find("#lbl-Exits-LastName").hide();
            $('#ApplyJobBody').find("#lbl-error-Email").hide();
            $('#ApplyJobBody').find("#lbl-error-EmailValid").hide();
            $('#ApplyJobBody').find("#lbl-error-GenderList").hide();
            $('#ApplyJobBody').find("#lbl-error-dateOfBirth").hide();
            $('#ApplyModel').find("#ApplyJobBody").find("#lbl-error-EmailValid").hide();
            return true;

        }
    }
    else if(context.fromStep==2)
    {        
        var isError = false;
        if (context.toStep == 1) {
            return true;
        }
        else {
            var IsUploadCoverLetter = $('#ApplyModel').find('#hiddenUploadCoverLetter').val();
            var IsUploadCVOrResume = $('#ApplyModel').find("#hiddenUploadResumeOrCV").val();
            var Id = $('#ApplyModel').find('#ApplicantHiddenId').val();
            var VacancyID = $('#ApplyModel').find('#ApplicantVacancyId').val();
            //var CoverLetterPathOriginal = $('#ApplyModel').find('#CoverLetterName')["0"].innerText;
            var CoverLetterPathOriginal = $('#ApplyModel').find('#CoverLetterName').val();
            var CoverLetterPath = $('#ApplyModel').find('#txt_CoverLetterName').val();
            var DownloadApplicationFormLink = $('#ApplyModel').find('#DownloadLink').attr('href');
            //var UploadApplicationFormPathOriginal = $('#ApplyModel').find('#UploadApplicationFormName')["0"].innerText;
            var UploadApplicationFormPathOriginal = $('#ApplyModel').find('#UploadApplicationFormName').val();
            var UploadApplicationFormPath = $('#ApplyModel').find('#txt_UploadApplicationFormName').val();
            //var ResumePathOriginal = $('#ApplyModel').find('#ResumePathName')["0"].innerText;
            var ResumePathOriginal = $('#ApplyModel').find('#ResumePathName').val();
            var ResumePath = $('#ApplyModel').find('#txt_ResumePathName').val();
            var Question1Answer = $('#ApplyModel').find('#txt_Question1Answer').val();
            var Question2Answer = $('#ApplyModel').find('#txt_Question2Answer').val();
            var Question3Answer = $('#ApplyModel').find('#txt_Question3Answer').val();
            var Question4Answer = $('#ApplyModel').find('#txt_Question4Answer').val();
            var Question5Answer = $('#ApplyModel').find('#txt_Question5Answer').val();
            var hdnOuestion1On = $('#ApplyModel').find("#hiddenOuestion1On").val();
            var hdnOuestion2On = $('#ApplyModel').find("#hiddenOuestion2On").val();
            var hdnOuestion3On = $('#ApplyModel').find("#hiddenOuestion3On").val();
            var hdnOuestion4On = $('#ApplyModel').find("#hiddenOuestion4On").val();
            var hdnOuestion5On = $('#ApplyModel').find("#hiddenOuestion5On").val();
            if (hdnOuestion1On == 1) {
                if (Question1Answer == "") {
                    isError = true;
                    $('#ApplyModel').find("#lbl-error-Question1Answer").show();
                }
            }
            if (hdnOuestion2On == 1) {
                if (Question2Answer == "") {
                    isError = true;
                    $('#ApplyModel').find("#lbl-error-Question2Answer").show();
                }
            }
            if (hdnOuestion3On == 1) {
                if (Question3Answer == "") {
                    isError = true;
                    $('#ApplyModel').find("#lbl-error-Question3Answer").show();
                }
            }
            if (hdnOuestion4On == 1) {
                if (Question4Answer == "") {
                    isError = true;
                    $('#ApplyModel').find("#lbl-error-Question4Answer").show();
                }
            }
            if (hdnOuestion5On == 1) {
                if (Question5Answer == "") {
                    isError = true;
                    $('#ApplyModel').find("#lbl-error-Question5Answer").show();
                }
            }
            var SourceID = $('#ApplyModel').find('#drp-SourceId').val();
            if (IsUploadCoverLetter == 1) {
                if (CoverLetterPath == "") {
                    isError = true;
                    $('#ApplyModel').find("#lbl-error-CoverLetter").show();
                }
            }
            if (IsUploadCVOrResume == 1) {
                if (ResumePath == "") {
                    isError = true;
                    $('#ApplyModel').find("#lbl-error-ResumePathName").show();
                }
            }
            if (UploadApplicationFormPath == "") {
                isError = true;
                $('#ApplyModel').find("#lbl-error-UploadApplicationForm").show();
            }
            if (isError) {
                return false;
            }
            else {
                if (context.toStep == 1) {
                    $('#ApplyJobBody').find('.buttonNext').show();
                    $('#ApplyJobBody').find('.buttonPrevious').hide();
                    $('#ApplyJobBody').find('.buttonFinish').hide();
                }
                else {
                    $('#ApplyJobBody').find('.buttonNext').hide();
                    $('#ApplyJobBody').find('.buttonPrevious').show();
                    $('#ApplyJobBody').find('.buttonFinish').show();
                }
                return true;
            }
        }
    } 
    else{                                
        if (context.toStep == 2) {
            $('#ApplyJobBody').find('.buttonNext').show();
            $('#ApplyJobBody').find('.buttonPrevious').show();
            $('#ApplyJobBody').find('.buttonFinish').hide();
        }
        else {
            $('#ApplyJobBody').find('.buttonNext').show();
            $('#ApplyJobBody').find('.buttonPrevious').hide();
            $('#ApplyJobBody').find('.buttonFinish').hide();
        }
        return true;
    }

}

function onApplicantFinishCallback(obj, context) {
    $(".hrtoolLoader").show();
    var isError = false;
    var IsUploadCoverLetter = $('#ApplyModel').find('#hiddenUploadCoverLetter').val();
    var IsUploadCVOrResume = $('#ApplyModel').find("#hiddenUploadResumeOrCV").val();
    var Id = $('#ApplyModel').find('#ApplicantHiddenId').val();
    var VacancyID = $('#ApplyModel').find('#ApplicantVacancyId').val();
    var FirstName = $('#ApplyModel').find('#txt_FirstName').val();
    var LastName = $('#ApplyModel').find('#txt_LastName').val();
    var Email = $('#ApplyModel').find('#txt_Email').val();
    var GenderID = $('#ApplyModel').find('.selectGender input:checked').val();
    var DateOfbirth = $("#ApplyModel").find("#txt_DateOfBitrh").val();
    var PostalCode = $("#ApplyModel").find("#txt_PostalCode").val();
    var Address = $("#ApplyModel").find("#AddressText").val();
    var OtherContactDetails = $("#ApplyModel").find("#txt_OtherContactDetails").val();
    var SelectStepID = $('#ApplyModel').find('#SelectSetpId').val();
    //var CoverLetterPathOriginal = $('#ApplyModel').find('#CoverLetterName')["0"].innerText;
    var CoverLetterPathOriginal = $('#ApplyModel').find('#CoverLetterName').val();
    var CoverLetterPath = $('#ApplyModel').find('#txt_CoverLetterName').val();
    var DownloadApplicationFormLink = $('#ApplyModel').find('#DownloadLink').attr('href');
    //var UploadApplicationFormPathOriginal = $('#ApplyModel').find('#UploadApplicationFormName')["0"].innerText;
    var UploadApplicationFormPathOriginal = $('#ApplyModel').find('#UploadApplicationFormName').val();
    var UploadApplicationFormPath = $('#ApplyModel').find('#txt_UploadApplicationFormName').val();
    //var ResumePathOriginal = $('#ApplyModel').find('#ResumePathName')["0"].innerText;
    var ResumePathOriginal = $('#ApplyModel').find('#ResumePathName').val();
    var ResumePath = $('#ApplyModel').find('#txt_ResumePathName').val();
    var Question1Answer = $('#ApplyModel').find('#txt_Question1Answer').val();
    var Question2Answer = $('#ApplyModel').find('#txt_Question2Answer').val();
    var Question3Answer = $('#ApplyModel').find('#txt_Question3Answer').val();
    var Question4Answer = $('#ApplyModel').find('#txt_Question4Answer').val();
    var Question5Answer = $('#ApplyModel').find('#txt_Question5Answer').val();
    var hdnOuestion1On=$('#ApplyModel').find("#hiddenOuestion1On").val();
    var hdnOuestion2On=$('#ApplyModel').find("#hiddenOuestion2On").val();
    var hdnOuestion3On=$('#ApplyModel').find("#hiddenOuestion3On").val();
    var hdnOuestion4On=$('#ApplyModel').find("#hiddenOuestion4On").val();
    var hdnOuestion5On=$('#ApplyModel').find("#hiddenOuestion5On").val();

    if(hdnOuestion1On==1)
    {
        if(Question1Answer=="")
        {
            isError = true;
            $('#ApplyModel').find("#lbl-error-Question1Answer").show();
        }        
    }
    if(hdnOuestion2On==1)
    {
        if(Question2Answer=="")
        {
            isError = true;
            $('#ApplyModel').find("#lbl-error-Question2Answer").show();
        }
    }
    if(hdnOuestion3On==1)
    {
        if(Question3Answer=="")
        {
            isError = true;
            $('#ApplyModel').find("#lbl-error-Question3Answer").show();
        }
    }
    if(hdnOuestion4On==1)
    {
        if(Question4Answer=="")
        {
            isError = true;
            $('#ApplyModel').find("#lbl-error-Question4Answer").show();
        }
    }
    if(hdnOuestion5On==1)
    {
        if(Question5Answer=="")
        {
            isError = true;
            $('#ApplyModel').find("#lbl-error-Question5Answer").show();
        }
    }
    var SourceID = $('#ApplyModel').find('#drp-SourceId').val();
    if (IsUploadCoverLetter == 1)
    {        
        if (CoverLetterPath == "")
        {     
            isError = true;
            $('#ApplyModel').find("#lbl-error-CoverLetter").show();
        }        
    }
    if (IsUploadCVOrResume == 1)
    {
        if (ResumePath == "")
        {
            isError = true;
            $('#ApplyModel').find("#lbl-error-ResumePathName").show();
        }
    }
    if (UploadApplicationFormPath == "")
    {
        isError = true;
        $('#ApplyModel').find("#lbl-error-UploadApplicationForm").show();
    }
       var model = {
        Id: Id,
        VacancyID: VacancyID,
        FirstName: FirstName,
        LastName: LastName,
        Email: Email,
        GenderID: GenderID,
        DateOfBirth: DateOfbirth,
        PostalCode: PostalCode,
        Address: Address,
        OtherContactDetails: OtherContactDetails,
        StatusID: SelectStepID,
        CoverLetterPathOriginal: CoverLetterPathOriginal,
        CoverLetterPath: CoverLetterPath,
        DownloadApplicationFormLink: DownloadApplicationFormLink,
        UploadApplicationFormPathOriginal: UploadApplicationFormPathOriginal,
        UploadApplicationFormPath: UploadApplicationFormPath,
        ResumePathOriginal: ResumePathOriginal,
        ResumePath: ResumePath,
        Question1Answer: Question1Answer,
        Question2Answer: Question2Answer,
        Question3Answer: Question3Answer,
        Question4Answer: Question4Answer,
        Question5Answer: Question5Answer,
        SourceID: SourceID,

    }
    if (!isError) {
        $.ajax({
            url: ConstantJob.Add,
            type: 'POST',
            data: JSON.stringify(model),
            contentType: "application/json",
            success: function (data) {

                if (data == "Error") {
                    isError = true;
                    $('#ApplyModel').find("#ApplyJobBody").find("#lbl-error-EmailValid").show();
                    $(".hrtoolLoader").hide();
                    $(".modal-backdrop").hide();
                    setTimeout(function () { $(".toast-error").hide(); }, 1500);

                }
                else {
                    //$("#page_content_inner").html("");
                    //$("#page_content_inner").html(data);
                    $(".hrtoolLoader").hide();
                    $(".modal-backdrop").hide();
                    $(".toast-info").show();
                    setTimeout(function () { $(".toast-info").hide(); }, 1500);
                    $('#ApplyModel').find("#modelClose").click();

                }
            }
        });
    }
}

function isValidEmailAddress(emailAddress) {
    var pattern = /^([a-z\d!#$%&'*+\-\/=?^_`{|}~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]+(\.[a-z\d!#$%&'*+\-\/=?^_`{|}~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]+)*|"((([ \t]*\r\n)?[ \t]+)?([\x01-\x08\x0b\x0c\x0e-\x1f\x7f\x21\x23-\x5b\x5d-\x7e\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|\\[\x01-\x09\x0b\x0c\x0d-\x7f\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))*(([ \t]*\r\n)?[ \t]+)?")@(([a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|[a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF][a-z\d\-._~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]*[a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])\.)+([a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|[a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF][a-z\d\-._~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]*[a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])\.?$/i;
    return pattern.test(emailAddress);
};

//Cover Letter fileUpload
$("#ApplyJobBody").on('change', '#CoverLetterfileToUpload', function (e) {
    $("#lbl-error-CoverLetter").hide();
    var files = e.target.files;
    var FileUploadData = "";
    if (files.length > 0) {
        if (window.FormData !== undefined) {
            FileUploadData = new FormData();
            for (var x = 0; x < files.length; x++) {
                FileUploadData.append("file" + x, files[x]);
            }
            setTimeout(function () {
                $.ajax({
                    type: "POST",
                    url: ConstantJob.FileUpload,
                    contentType: false,
                    processData: false,
                    data: FileUploadData,
                    success: function (result) {                        
                        $("#ApplyJobBody").find('#CoverLetterName').html("");
                        $("#ApplyJobBody").find('#CoverLetterName').html(result.originalFileName);
                        $("#ApplyJobBody").find('#txt_CoverLetterName').val(result.NewFileName);
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

//Application Form FileUpload  
$("#ApplyJobBody").on('change', '#UploadApplicationFormfileToUpload', function (e) {
    $("#lbl-error-UploadApplicationForm").hide();
    var files = e.target.files;
    var FileUploadData = "";
    if (files.length > 0) {
        if (window.FormData !== undefined) {
            FileUploadData = new FormData();
            for (var x = 0; x < files.length; x++) {
                FileUploadData.append("file" + x, files[x]);
            }
            setTimeout(function () {
                $.ajax({
                    type: "POST",
                    url: ConstantJob.FileUpload,
                    contentType: false,
                    processData: false,
                    data: FileUploadData,
                    success: function (result) {
                        
                        $("#ApplyJobBody").find("#UploadApplicationFormName").html("");
                        $("#ApplyJobBody").find("#UploadApplicationFormName").html(result.originalFileName);
                        $("#ApplyJobBody").find("#txt_UploadApplicationFormName").val(result.NewFileName);


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

//Resume FileUpload 
$("#ApplyJobBody").on('change', '#ResumePathNamefileToUpload', function (e) {
    $("#lbl-error-ResumePathName").hide();
    var files = e.target.files;
    var FileUploadData = "";
    if (files.length > 0) {
        if (window.FormData !== undefined) {
            FileUploadData = new FormData();
            for (var x = 0; x < files.length; x++) {
                FileUploadData.append("file" + x, files[x]);
            }
            setTimeout(function () {
                $.ajax({
                    type: "POST",
                    url: ConstantJob.FileUpload,
                    contentType: false,
                    processData: false,
                    data: FileUploadData,
                    success: function (result) {
                        
                        $("#ApplyJobBody").find("#ResumePathName").html("");
                        $("#ApplyJobBody").find("#ResumePathName").html(result.originalFileName);
                        $("#ApplyJobBody").find("#txt_ResumePathName").val(result.NewFileName);


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


$("#ApplyModel").on('click', '#FindAddress', function () {
    
    $(".hrtoolLoader").show();
    var isError = false;
    // var hiddenId = $("#tableDivTraining").find("#hidden-Id").val();
    var HouseNumber = $("#ApplyModel").find("#txt_HouseNumber").val();
    var postCode = $("#ApplyModel").find("#txt_PostalCode").val();

    if (postCode == "") {
        isError = true;
        $("#ApplyModel").find("#lbl-error-PostCode").show();
        $(".hrtoolLoader").hide();
        $(".modal-backdrop").hide();
    }
    else {

        $.post('http://maps.googleapis.com/maps/api/geocode/json?address=' + postCode + '&sensor=false', function (r) {
            
            var lat = r['results'][0]['geometry']['location']['lat'];
            var lng = r['results'][0]['geometry']['location']['lng'];
            $.post('http://maps.googleapis.com/maps/api/geocode/json?latlng=' + lat + ',' + lng + '&sensor=false', function (address) {
                
                $("#ApplyModel").find("#AddressText").val(HouseNumber + ", " + address['results'][0]['address_components'][1]['long_name'] + "\n" + address['results'][0]['address_components'][2]['long_name'] + "\n" + address['results'][0]['address_components'][4]['long_name'] + "\n" + address['results'][0]['address_components'][5]['long_name'] + "\n" + address['results'][0]['address_components'][6]['long_name'] + " " + address['results'][0]['address_components'][7]['long_name']);
            });
        });
        $(".hrtoolLoader").hide();
        $(".modal-backdrop").hide();
    }
});

$('#ApplyModel').on('keyup', '#txt_PostalCode', function () {
    var isError = false;
    $("#ApplyModel").find("#lbl-error-PostCode").hide();
});

//validation
$('#ApplyModel').on('keyup', '#txt_FirstName', function () {
    var isError = false;
    $("#ApplyModel").find("#lbl-error-FirstName").hide();
});

$('#ApplyModel').on('keyup', '#txt_LastName', function () {
    var isError = false;
    $("#ApplyModel").find("#lbl-error-LastName").hide();
});

$('#ApplyModel').on('keyup', '#txt_Email', function () {
    var isError = false;
    $("#ApplyModel").find("#lbl-error-Email").hide();
    $("#ApplyModel").find("#lbl-error-EmailValid").hide();
});

$('#ApplyModel').on('change', '#txt_DateOfBitrh', function () {
    var isError = false;
    $("#ApplyModel").find("#lbl-error-dateOfBirth").hide();
});

$("#ApplyModel").on('click', "#removeCoverLetter", function () {
    document.getElementById('CoverLetterName').innerHTML = '';
    $("#txt_CoverLetterName").val('');
})
$("#ApplyModel").on('click', "#removeApplicationForm", function () {
    document.getElementById('UploadApplicationFormName').innerHTML = '';
    $("#txt_UploadApplicationFormName").val('');
})
$("#ApplyModel").on('click', "#removeUploadResumeCV", function () {
    document.getElementById('ResumePathName').innerHTML = '';
    $("#txt_ResumePathName").val('');
})