//salary
function DataTableDesign() {
    var table = $('#salaryTable').DataTable({
        "sDom": '<"top"i>rt<"bottom"flp><"clear">',
        "bPaginate": false
    });
    $('#tableDivSalary').find('.dataTables_filter').hide();
    $('#tableDivSalary').find('.dataTables_info').hide();
    $("#tableDivSalary").find("#salaryTable").find("#Effective").Zebra_DatePicker({
        //direction: false,
        showButtonPanel: false,
        format: 'd-M-Y',
        onSelect: function () {
            var CreatedExpiryDate = $("#tableDivSalary").find("#salaryTable").find('#Effective').val();
        }
    });
}
//deduction
function DataTable2Design() {
    var table = $('#salaryDeductionTable').DataTable({
        "sDom": '<"top"i>rt<"bottom"flp><"clear">',
        "bPaginate": false
    });
    $('#tableDivDeductionSalary').find('.dataTables_filter').hide();
    $('#tableDivDeductionSalary').find('.dataTables_info').hide();
    $("#drpCustomerCompany").change();
}
//entitlements
function DataTable3Design() {
    var table = $('#salaryEntitlementTable').DataTable({
        "sDom": '<"top"i>rt<"bottom"flp><"clear">',
        "bPaginate": false
    });
    $('#tableDivEntitlementSalary').find('.dataTables_filter').hide();
    $('#tableDivEntitlementSalary').find('.dataTables_info').hide();


}
//Deduction Temp
function DataTable4Design() {
    var table = $('#salaryDeductionTempTable').DataTable({
        "sDom": '<"top"i>rt<"bottom"flp><"clear">',
        "bPaginate": false
    });
    $('#tableDivDeductionTempSalary').find('.dataTables_filter').hide();
    $('#tableDivDeductionTempSalary').find('.dataTables_info').hide();

}
//entitlements Temp
function DataTable5Design() {
    var table = $('#salaryEntitlementTempTable').DataTable({
        "sDom": '<"top"i>rt<"bottom"flp><"clear">',
        "bPaginate": false
    });
    $('#tableDivEntitlementTempSalary').find('.dataTables_filter').hide();
    $('#tableDivEntitlementTempSalary').find('.dataTables_info').hide();

}

$(document).ready(function () {
    DataTableDesign();
    DataTable2Design();
    DataTable3Design();
    DataTable4Design();
    DataTable5Design();
    $("#page_content").find("#DateOfBirth").Zebra_DatePicker({
        showButtonPanel: false,
        format: 'd-m-Y',
        onSelect: function () {
            var dob = $("#DateOfBirth").val();
            validDOB(dob);
        }
    });
    
    $("#page_content").find("#StartDate").Zebra_DatePicker({
        showButtonPanel: false,
        format: 'd-m-Y',
        onSelect:function(){
                   $("#page_content").find("#ValidStartDate").hide();
        }
    }); 
    $("#page_content").find("#ContinuousServiceDate").Zebra_DatePicker({
        showButtonPanel: false,
        format: 'd-m-Y',        
    });
    //$("#page_content").find(".Datepiker").Zebra_DatePicker({
    //    showButtonPanel: false,
    //    format: 'd-m-Y',
    //});


    $("#drpCustomerCompany").select();
});

//CustomerCompany Start
$("#tableDivResource").on("change", "#drpCustomerCompany", function () {
    var value = $(this).val();
    if (value != "0") {
        $.ajax({
            url: constantSet.CCDetails,
            data: { Id: value },
            success: function (data) {
                $("#tableDivResource").find(".CCDetails").removeClass('hide');
                $("#tableDivResource").find("#CCName").html(data.CompanyName);
                $("#tableDivResource").find("#CCShortName").html(data.ShortName);
                $("#tableDivResource").find("#CCParentCompany").html(data.ParentCompany);
                $("#tableDivResource").find("#CCWebsite").html(data.Website);

                $("#tableDivResource").find("#CCCustomerCurrency").html(data.IBAN);

                $("#tableDivResource").find("#CCCustomerNumber").html(data.PhoneNumber);
                $("#tableDivResource").find("#CCPhoneNumber").html(data.ShortName);
                $("#tableDivResource").find("#CCEmail").html(data.ParentCompany);
                $("#tableDivResource").find("#CCSecondaryPhoneNumber").html(data.SecondaryPhoneNumber);
                $("#tableDivResource").find("#CCBusinessAddress").html(data.CompanyName);
                $("#tableDivResource").find("#CCMailingAddress").html(data.ShortName);
                $("#tableDivResource").find("#CCGeneralNotes").html(data.ParentCompany);

            }
        });
    }
    else {
        $("#tableDivResource").find(".CCDetails").addClass('hide');
    }
});

//Find Address
$("#page_content").on('click', '#FindAddess', function () {

    var isError = false;
    // var hiddenId = $("#tableDivTraining").find("#hidden-Id").val();
    var HouseNumber = $("#tableDivResource").find("#HouseNumber").val();
    var postCode = $("#tableDivResource").find("#Postcode").val();

    if (postCode == "") {
        $("#tableDivResource").find("#ValidPostCode").show();
    }
    else {

        $.post('http://maps.googleapis.com/maps/api/geocode/json?address=' + postCode + '&sensor=false', function (r) {

            var lat = r['results'][0]['geometry']['location']['lat'];
            var lng = r['results'][0]['geometry']['location']['lng'];
            $.post('http://maps.googleapis.com/maps/api/geocode/json?latlng=' + lat + ',' + lng + '&sensor=false', function (address) {

                $("#tableDivResource").find("#Address").val(HouseNumber + "," + address['results'][0]['address_components'][1]['long_name'] + "\n" + address['results'][0]['address_components'][2]['long_name'] + "\n" + address['results'][0]['address_components'][4]['long_name'] + "\n" + address['results'][0]['address_components'][5]['long_name'] + "\n" + address['results'][0]['address_components'][6]['long_name'] + " " + address['results'][0]['address_components'][7]['long_name']);
            });
        });
    }
});

$('#page_content').on('keyup', '#Postcode', function () {
    var isError = false;
    $("#tableDivResource").find("#ValidPostCode").hide();
});

//CustomerCompany End


$("#tableDivSalary").on('click', '.btn-add-salary', function () {
    $(".hrtoolLoader").show();
    var EmployeeID = $("#page_content_inner").find("#EmployeeID").val();
    $.ajax({
        url: constantSet.addEdit,
        data: { Id: 0, EmployeeID: EmployeeID},
        success: function (data) {
            $("#tableDivSalary").find('#AddSalaryBody').html('');
            $("#tableDivSalary").find('#AddSalaryBody').html(data);
            $("#tableDivSalary").find(".salaryTitle").html("ADD Salary");
            $("#addsalaryContact").find("#btn-submit-Salary").html("Add");
            TotalSalarySet();
            DataTable4Design();
            DataTable5Design();
            $('[data-toggle="tooltip"]').tooltip();
            $("#Effective").Zebra_DatePicker({
                //direction: false,`
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {
                    var fromDate = $('#Effective').val();
                    $("#validationmessagefromdate").hide();
                }
            });
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();

        }
    });
});

$("#tableDivSalary").on('click', '.btn-edit-salary', function () {

    $(".hrtoolLoader").show();
    var current = $("#page_content_inner").find("#EmployeeID").val();
    var id = $("#tableDivSalary").find("#salaryTable tbody").find('.selected').attr("id");
    var hiddenId = $("#tableDivResource").find("#TempSalaryID").val();
    $.ajax({
        url: constantSet.addEdit,
        data: { Id: id, EmployeeID: current, Hid: hiddenId },
        success: function (data) {

            $("#tableDivSalary").find('#AddSalaryBody').html('');
            $("#tableDivSalary").find('#AddSalaryBody').html(data);
            $("#tableDivSalary").find(".salaryTitle").html("Edit Salary");
            $("#tableDivSalary").find("#btn-submit-Salary").html("Save");
            //var CurrencyID = $("#tableDivSalary").find("#drpCurrency").val();
            //var SalaryTypeID = $("#tableDivSalary").find("#drpSalary").val();
            //var Currencytext = $("#drpCurrency option[value=" + CurrencyID + "]").text().split('-')[1];
            //var SalaryType = $("#drpSalary option[value=" + SalaryTypeID + "]").text().split('-')[0];
            //var Total = $("#tableDivSalary").find('#TotalSalary').val().split(' ')[4];
            //if (Total == "$") {
            //    Total = 0;
            //}
            //$("#tableDivSalary").find('#TotalSalary').val("Total" + " " + SalaryType + " " + "Salary is" + " " + Currencytext + " " + Total);
            TotalSalarySet();
            DataTable2Design();
            DataTable3Design();
            $('[data-toggle="tooltip"]').tooltip();
            $("#Effective").Zebra_DatePicker({
                //direction: false,`
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {
                    var fromDate = $('#Effective').val();
                    $("#validationmessagefromdate").hide();
                }
            });
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
        }
    });
});

$('#tableDivSalary').on('click', '.dataTr', function () {
    if ($(this).hasClass('dataTr')) {
        $('#salaryTable tbody').find('tr.selected').removeClass('selected');
        $(this).addClass('selected');
        $("#tableDivSalary").find(".btn-edit-salary").removeAttr('disabled');
        $("#tableDivSalary").find(".btn-delete-salary").removeAttr('disabled');
    }
});

$("#tableDivResource").on('click', '#btn-submit-Salary', function () {
    $(".hrtoolLoader").show();
    var iserror = false;
    var hiddenId = $("#tableDivSalary").find("#hiddenId").val();
    var TempId = $("#tableDivResource").find("#TempSalaryID").val();
    var model = {
        Id: $("#tableDivSalary").find("#hiddenId").val(),
        EmployeeId: $("#page_content_inner").find("#EmployeeID").val(),
        EffectiveFrom: $("#tableDivSalary").find("#Effective").val(),
        SalaryTypeID: $("#tableDivSalary").find("#drpSalary").val(),
        PaymentFrequencyID: $("#tableDivSalary").find("#drpPayment").val(),
        CurrencyID: $("#tableDivSalary").find("#drpCurrency").val(),
        Amount: $("#tableDivSalary").find("#Amount").val(),
        TotalSalary: $("#tableDivSalary").find("#EmployeeTotalSalaryAmount").val(),
        ReasonforChange: $("#tableDivSalary").find("#drpResonforChange").val(),
        Comments: $("#tableDivSalary").find("#Comments").val(),
        TableId: TempId,
    }
    if (model.EffectiveFrom == "") {
        iserror = true;
        $("#validationmessagefromdate").show();
        $("#validationmessagefromdate").html("Effective From Date required");
    }
    if (model.Amount == "") {
        iserror = true;
        $("#validationmessageAmount").show();
        $("#validationmessageAmount").html("Amount required");
    }
    if (model.ReasonforChange == "0") {
        iserror = true;
        $("#validationmessageReason").show();
        $("#validationmessageReason").html("Select Reason for Change");
    }
    if (iserror) {
        $(".hrtoolLoader").hide();
        $(".modal-backdrop").hide();
        return false;
    }
    else {
        $.ajax({
            type: "POST",
            data: JSON.stringify(model),
            url: constantSet.SaveProject,
            contentType: "application/json",
            success: function (result) {
                $("#tableDivSalary").html('');
                $("#tableDivSalary").html(result);
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
});
function validDOB(dob) {
    $("#page_content").find("#validAge").hide();
    $("#page_content").find("#ValidDateOfBirth").hide();
    var now = new Date();
    var a = dob.split(" ");
    var d = a[0].split("-");
    var date = new Date();
    var age = now.getFullYear() - d[2];
    if (isNaN(age) || age < 18) {
        $("#page_content").find("#validAge").show();
        $("#page_content").find("#DateOfBirth").val('');
    }
    else {
        $("#page_content").find("#validAge").hide();
    }
}
$("#tableDivResource").on('click', '#SaveProfileRecord', function () {
    $(".hrtoolLoader").show();
    var Gender = $('input[name=Gender]:checked').val();
    var hiddenId = $("#tableDivTraining").find("#hidden-Id").val();
    var iserror = false;
    var Title = $("#page_content").find("#drpTitle").val();
    var ReportsTo = $("#page_content").find("#selectID").val();   
    var AddReTo = $("#page_content").find("#AddRespID").val();
    var HrReportTo = $("#page_content").find("#HRResID").val();
    if (Title == "0") {
        iserror = true; 
        $("#ValidTitle").show();
        $("#ValidTitle").html("The Title is required.");
    }
    var FirstName = $("#page_content").find("#FirstName").val();
    if (FirstName == "") {
        iserror = true;
        $("#ValidFirstName").show();
        $("#ValidFirstName").html("The First Name is required.");
    }
    var Lastname = $("#page_content").find("#LastName").val();
    if (Lastname == "") {
        iserror = true;
        $("#ValidLastName").show();
        $("#ValidLastName").html("The Last Name is required.");
    }

    var Email = $("#page_content").find("#Email").val();
    if (Email == "") {
        iserror = true;
        $("#ValidEmail").show();
        $("#ValidEmail").html("The Email is required.");
    }
    var Gender = $("#page_content").find('input[name=Gender]:checked').val();
    if (Gender == "1" || Gender == "0")
    { }
    else
    {
        iserror = true;
        $("#ValidGender").show();
        $("#ValidGender").html("The Gender is required.");
    }
    var DateOfBirth = $("#page_content").find("#DateOfBirth").val();
    if (DateOfBirth == "") {
        iserror = true;
        $("#ValidDateOfBirth").show();
        $("#ValidDateOfBirth").html("The Date Of Birth is required.");
    }

    var Nationality = $("#page_content").find("#drpNationality").val();
    if (DateOfBirth == "0") {
        iserror = true;
        $("#ValidNationality").show();
        $("#ValidNationality").html("The Nationality is required..");
    }
    var StartDate = $("#page_content").find("#StartDate").val();
    if (StartDate == "") {
        iserror = true;
        $("#ValidStartDate").show();
        $("#ValidStartDate").html("The Start Date is required.");
    }
    var ResourceType = $("#page_content").find("#drpResourceType").val();
    if (ResourceType == "0") {
        iserror = true;
        $("#ValidResourceType").show();
        $("#ValidResourceType").html("The Resource Type is required.");
    }
    //var JobTitle = $("#page_content").find("#drpJobTitle").val();
    //if (JobTitle == "0") {
    //    iserror = true;
    //    $("#ValidJobTitle").show();
    //    $("#ValidJobTitle").html("The Job Title is required.");
    //}
    var Location = $("#page_content").find("#drpLocation").val();
    if (Location == "0") {
        iserror = true;
        $("#ValidLocation").show();
        $("#ValidLocation").html("The Location is required.");
    }
    var ssovalues = $("#page_content").find("#SSO").val();
    var RoleType = $("#tableDivResource").find("#EmployeeRoleType").val();
    if (RoleType == "value") {
        var SelectCustomerCompanyId = $("#tableDivResource").find("#drpCustomerCompany").val();
        var Postcode = $("#tableDivResource").find("#Postcode").val();
        var ContactAddress = $("#tableDivResource").find("#Address").val();
        var WorkPhone = $("#tableDivResource").find("#WorkPhone").val();
        var WorkMobile = $("#tableDivResource").find("#Mobile").val();
        var CustomerCareID = $("#tableDivResource").find("#selectIDCustomer").val();
    }
    if (iserror) {
        $(".hrtoolLoader").hide();
        $(".modal-backdrop").hide();
        return false;
    }
    else {
        var model = {
            Id: $("#page_content").find("#UserId").val(),
            Title: $("#page_content").find("#drpTitle").val(),
            FirstName: $("#page_content").find("#FirstName").val(),
            LastName: $("#page_content").find("#LastName").val(),
            OtherNames: $("#page_content").find("#OtherName").val(),
            KnownAs: $("#page_content").find("#Knownas").val(),
            SSO: ssovalues,
            UserNameEmail: $("#page_content").find("#Email").val(),
            IMAddress: $("#page_content").find("#IMAddress").val(),
            Gender: Gender,
            DateOfBirth: $("#page_content").find("#DateOfBirth").val(),
            Nationality: $("#page_content").find("#drpNationality").val(),
            NIN_SSN: $("#page_content").find("#NINumber").val(),
            PhotoPath: $("#page_content").find('#fileToUpload').val(),
            StartDate: $("#page_content").find("#StartDate").val(),
            ContinuousServiceDate: $("#page_content").find("#ContinuousServiceDate").val(),
            ResourceType: $("#page_content").find("#drpResourceType").val(),
            Reportsto: ReportsTo,
            AdditionalReportsto: AddReTo,
            HRResponsible: HrReportTo,
            JobTitle: $("#page_content").find("#drpJobTitle").val(),
            Company: $("#page_content").find("#drpCompany").val(),
            Location: $("#page_content").find("#drpLocation").val(),
            BusinessID: $("#page_content").find("#drpBusiness").val(),
            DivisionID: $("#page_content").find("#drpDivision").val(),
            PoolID: $("#page_content").find("#drpPool").val(),
            FunctionID: $("#page_content").find("#drpFunction").val(),
            Picture: $("#page_content").find(".editpic").attr("data-Name"),
            SelectCustomerCompanyId: SelectCustomerCompanyId,
            PostalCode: Postcode,
            WorkPhone: WorkPhone,
            WorkMobile: WorkMobile,
            Address: ContactAddress,
            CustomerCareID: CustomerCareID,
        }
        $.ajax({
            type: "POST",
            data: JSON.stringify(model),
            url: constantSet.Saveprofile,
            contentType: "application/json",
            success: function (result) {
                //$("#page_content_inner").find("#tableDivResource").html('')
                //$("#page_content_inner").find("#tableDivResource").html(result);
                //DataTableDesign();
                window.location.reload();
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
});

$("#tableDivResource").on('change', '#fileToUpload', function (e) {
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
                    url: constantSet.ImageData,
                    contentType: false,
                    processData: false,
                    data: imageData,
                    success: function (result) {
                        var newhref = "/Upload/Resources/" + result.NewFileName;
                        $("#tableDivResource").find(".editpic").attr("src", newhref);
                        $("#tableDivResource").find(".editpic").attr("data-Name", result.NewFileName);
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

$("#tableDivSalary").on('click', '.btn-delete-salary', function () {
    var employeeId = $("#page_content_inner").find("#EmployeeID").val();
    var id = $("#tableDivSalary").find("#salaryTable tbody").find('.selected').attr("id");
    $.Zebra_Dialog("Are you sure you would like to delete this record?", {
        'type': false,
        'title': 'Delete Salary',
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
                        data: { Id: id, EmployeeID: employeeId },
                        success: function (data) {
                            $("#tableDivSalary").html('');
                            $("#tableDivSalary").html(data);

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

$("#tableDivResource").on("change", "#drpBusiness", function () {

    var value = $(this).val();
    if (value != "0") {
        $.ajax({
            url: constantSet.bindDiv,
            data: { businessId: value },
            success: function (data) {

                $("#drpDivision").html('');
                var toAppend = '';
                toAppend += "<option value='0'>--Select Division--</option>";
                $.each(data, function (index, item) {
                    toAppend += "<option value='" + item.Id + "'>" + item.Name + "</option>";
                })
                $("#drpDivision").html(toAppend);
                if ($("#drpDivision").val() == 0) {
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
            option.attr("value", '0').text('--Select Division--');
            $('#drpDivision').append(option);
        });
        $('#drpPool').empty();
        // Bind new values to dropdown
        $('#drpPool').each(function () {
            // Create option
            var option = $("<option />");
            option.attr("value", '0').text('--Select Pool--');
            $('#drpPool').append(option);
        });
        $('#drpFunction').empty();
        // Bind new values to dropdown
        $('#drpFunction').each(function () {
            // Create option
            var option = $("<option />");
            option.attr("value", '0').text('--Select Function--');
            $('#drpFunction').append(option);
        });


    }
});

$("#tableDivResource").on("change", "#drpDivision", function () {
    var value = $(this).val();
    if (value != "0") {
        $.ajax({
            url: constantSet.bindpool,
            data: { DivisionId: value },
            success: function (data) {
                $("#drp-Pool").html('');
                var toAppend = '';
                toAppend += "<option value='0'>--Select Pool--</option>";
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
                        toAppend += "<option value='0'>--Select Function--</option>";
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
            option.attr("value", '0').text('--Select Pool--');
            $('#drpPool').append(option);
        });
        $('#drpFunction').empty();
        // Bind new values to dropdown
        $('#drpFunction').each(function () {
            // Create option
            var option = $("<option />");
            option.attr("value", '0').text('--Select Function--');
            $('#drpFunction').append(option);
        });


    }
});

$("#tableDivResource").on('keyup', "#Effective", function () {


});

$("#tableDivResource").on('focusout', "#Amount", function (event) {    
    debugger;    
    // skip for arrow keys
    //if (event.which >= 37 && event.which <= 40) {
    //    event.preventDefault();
    //}
    //$(this).val(function (index, value) {
    //    value = value.replace(/,/g, '');
    //    return numberWithCommas(value);
    //});
    var amount = $(this).val();
    var title = $("#tableDivSalary").find(".salaryTitle").html();
    var CurrencyID = $("#tableDivSalary").find("#drpCurrency").val();
    // var Currencytext = $("#drpCurrency option[value=" + CurrencyID + "]").text().split('-')[1];
    //var text = $("#drpCurrency option[value=" + CurrencyID + "]").text().split("-")[1];
    // var TotalSalary = $("#tableDivSalary").find("#TotalSalary").val("Total Annual Salary is" + " " + text + " " + amount);
    var EmployeeID = $("#page_content_inner").find("#EmployeeID").val();
    var hiddenId = $("#tableDivResource").find("#TempSalaryID").val();
    var OriginalId = $("#tableDivSalary").find("#hiddenId").val();
    var TotalsSalary = amount;
    var EffectiveFrom = $("#tableDivSalary").find("#Effective").val();
    var SalaryTypeID = $("#tableDivSalary").find("#drpSalary").val();
    var PaymentFrequencyID = $("#tableDivSalary").find("#drpPayment").val();
    var CurrencyID = $("#tableDivSalary").find("#drpCurrency").val();
    var Amount = $("#tableDivSalary").find("#Amount").val();
    var ReasonforChange = $("#tableDivSalary").find("#drpResonforChange").val();
    var SalaryComments = $("#tableDivSalary").find("#Comments").val();
    var model = {
        Id: hiddenId,
        OriginalId: OriginalId,
        EmployeeId: EmployeeID,
        EffectiveFrom: EffectiveFrom,
        SalaryTypeID: SalaryTypeID,
        PaymentFrequencyID: PaymentFrequencyID,
        CurrencyID: CurrencyID,
        Amount: Amount,
        TotalSalary: TotalsSalary,
        ReasonforChange: ReasonforChange,
        Comments: SalaryComments,
    }
    $.ajax({
        type: "POST",
        data: JSON.stringify(model),
        url: constantSet.SaveChange,
        contentType: "application/json",
        success: function (data) {
            $("#tableDivSalary").find('#AddSalaryBody').html('');
            $("#tableDivSalary").find('#AddSalaryBody').html(data);
            if (title != "Edit Salary") {
                $("#tableDivSalary").find(".salaryTitle").html("ADD Salary");
                $("#addsalaryContact").find("#btn-submit-Salary").html("Add");
                DataTable4Design();
                DataTable5Design();
            }
            else {
                $("#tableDivSalary").find(".salaryTitle").html("Edit Salary");
                $("#addsalaryContact").find("#btn-submit-Salary").html("Save");
                DataTable2Design();
                DataTable3Design();

            }

            TotalSalarySet();

            $('[data-toggle="tooltip"]').tooltip();
            $("#Effective").Zebra_DatePicker({
                //direction: false,`
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {
                    var fromDate = $('#Effective').val();
                    $("#validationmessagefromdate").hide();
                }
            });
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
        }
    });

    // $(this).val(numberWithCommas($(this).val()));

});

$("#tableDivResource").on('change', "#drpCurrency", function () {
    TotalSalarySet();
});

$("#tableDivResource").on("change", "#drpSalary", function () {
    TotalSalarySet();
});

//btn-close-Salary  btn-Cancel-Salary

$("#tableDivResource").on('click', '#btn-close-Salary', function () {
    var hiddenId = $("#tableDivSalary").find("#TempSalaryID").val();
    var EmployeeID = $("#page_content_inner").find("#EmployeeID").val();
    $.ajax({
        url: constantSet.DeleteTemp,
        data: { Id: hiddenId, EmployeeID: EmployeeID },
        success: function (data) {
            window.location.reload();
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();

        }
    });

});

$("#tableDivResource").on('click', '#btn-Cancel-Salary', function () {
    var hiddenId = $("#tableDivSalary").find("#TempSalaryID").val();
    var EmployeeID = $("#page_content_inner").find("#EmployeeID").val();
    $.ajax({
        url: constantSet.DeleteTemp,
        data: { Id: hiddenId, EmployeeID: EmployeeID },
        success: function (data) {
            window.location.reload();
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();

        }
    });
});

//Deduction
$("#tableDivSalary").on('click', '.btn-add-salaryDeduction', function () {
    
    $(".hrtoolLoader").show();
    var IsError = false;
    var hiddenId = $("#tableDivSalary").find("#hiddenId").val();
    if (TotalSalary == "0") {
        IsError = true;
        $("#validationmessageAmount").show();
        $("#validationmessageAmount").html("Amount required");

    }
    if (IsError) {
        $(".hrtoolLoader").hide();
        $(".modal-backdrop").hide();
        $("#btn-close-SalaryDeductionTemp").click();
    }
    else {

        $('#addsalaryDeduction').modal('toggle');
        $('#addsalaryDeduction').modal('show');
        $.ajax({
            url: constantSet.addEditDeduction,
            data: { Id: 0, SalaryID: hiddenId },
            success: function (data) {
                $("#tableDivDeductionSalary").find('#AddsalaryDeductionDeductionBody').html('');
                $("#tableDivDeductionSalary").find('#AddsalaryDeductionDeductionBody').html(data);
                $("#tableDivDeductionSalary").find(".salaryDeductionDeductionTitle").html("ADD Deduction");
                $("#tableDivDeductionSalary").find("#btn-submit-salaryDeduction").html("Add");
                $('[data-toggle="tooltip"]').tooltip();
                $(".hrtoolLoader").hide();
                $(".modal-backdrop").hide();

            }
        });
    }
});

$("#tableDivResource").on('click', '#btn-submit-salaryDeduction', function () {

    $(".hrtoolLoader").show();
    var iserror = false;
    var Id = $("#tableDivDeductionSalary").find("#SalaryDeductionID").val();
    var SalaryId = $("#tableDivDeductionSalary").find("#SalaryId").val();
    var model = {
        Id: Id,
        EmployeeID: $("#page_content_inner").find("#EmployeeID").val(),
        EmployeeSalaryID: SalaryId,
        DeductionID: $("#tableDivDeductionSalary").find("#drp-SalaryDeduction").val(),
        FixedAmount: $("#tableDivDeductionSalary").find("#FixedAmount").val(),
        PercentOfSalary: $("#tableDivDeductionSalary").find("#txt_PercentOfSalary").val(),
        IncludeInSalary: $("#tableDivDeductionSalary").find("#check_IncludeSalary").is(":checked"),
        Comments: $("#tableDivDeductionSalary").find("#Comments").val(),

    }
    if (model.DeductionID == "0") {
        iserror = true;
        $("#lbl-error-DeductionList").show();
    }
    if (model.FixedAmount == "" || model.FixedAmount == "0" || model.FixedAmount == "0.00") {
        iserror = true;
        $("#validationmessageFixedAmount").show();
        $("#validationmessageFixedAmount").html("Fixed amount is required.");
    }
    if (model.PercentOfSalary == "0" || model.PercentOfSalary == "" || model.PercentOfSalary == "0.00") {
        iserror = true;
        $("#validationmessagePercentOfSalary").show();
        $("#validationmessagePercentOfSalary").html("Percent of salary is required.");
    }
    if (iserror) {
        $(".hrtoolLoader").hide();
        $(".modal-backdrop").hide();
        return false;
    }
    else {
        $.ajax({
            type: "POST",
            data: JSON.stringify(model),
            url: constantSet.SaveSalaryDeduction,
            contentType: "application/json",
            success: function (data) {
                $("#tableDivSalary").find('#AddSalaryBody').html('');
                $("#tableDivSalary").find('#AddSalaryBody').html(data);
                $("#tableDivSalary").find(".salaryTitle").html("Edit Salary");
                $("#tableDivSalary").find("#btn-submit-Salary").html("Save");
                //var CurrencyID = $("#tableDivSalary").find("#drpCurrency").val();
                //var SalaryTypeID = $("#tableDivSalary").find("#drpSalary").val();
                //var Currencytext = $("#drpCurrency option[value=" + CurrencyID + "]").text().split('-')[1];
                //var SalaryType = $("#drpSalary option[value=" + SalaryTypeID + "]").text().split('-')[0];
                //var Total = $("#tableDivSalary").find('#TotalSalary').val().split(' ')[4];
                //if (Total == "$") {
                //    Total = 0;
                //}
                //$("#tableDivSalary").find('#TotalSalary').val("Total" + " " + SalaryType + " " + "Salary is" + " " + Currencytext + " " + Total);
                TotalSalarySet();
                DataTable2Design();
                DataTable3Design();
                $('[data-toggle="tooltip"]').tooltip();
                $("#Effective").Zebra_DatePicker({
                    //direction: false,`
                    showButtonPanel: false,
                    format: 'd-m-Y',
                    onSelect: function () {
                        var fromDate = $('#Effective').val();
                        $("#validationmessagefromdate").hide();
                    }
                });
                $(".hrtoolLoader").hide();
                $(".modal-backdrop").hide();
            }
        });
    }
});

$('#tableDivSalary').on('click', '.dataDeductionTr', function () {
    if ($(this).hasClass('dataDeductionTr')) {
        $('#salaryDeductionTable tbody').find('tr.selected').removeClass('selected');
        $(this).addClass('selected');
        $("#tableDivDeductionSalary").find(".btn-edit-salaryDeduction").removeAttr('disabled');
        $("#tableDivDeductionSalary").find(".btn-delete-salaryDeduction").removeAttr('disabled');
    }
});

$("#tableDivResource").on('click', '#btn-Edit-SalaryDeduction', function () {
    
    $(".hrtoolLoader").show();
    $('#addsalaryDeduction').modal('show');
    var hiddenId = $("#tableDivSalary").find("#hiddenId").val();
    var id = $("#tableDivDeductionSalary").find("#salaryDeductionTable tbody").find('.selected').attr("id");
    $.ajax({
        url: constantSet.addEditDeduction,
        data: { Id: id, SalaryID: hiddenId },
        success: function (data) {

            $("#tableDivDeductionSalary").find('#AddsalaryDeductionDeductionBody').html('');
            $("#tableDivDeductionSalary").find('#AddsalaryDeductionDeductionBody').html(data);
            $("#tableDivDeductionSalary").find(".salaryDeductionDeductionTitle").html("Edit Deduction");
            $("#tableDivDeductionSalary").find("#btn-submit-salaryDeduction").html("Save");
            $('[data-toggle="tooltip"]').tooltip();
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();

        }
    });
});

$("#tableDivSalary").on('click', '.btn-edit-salaryDeduction', function () {
    $(".hrtoolLoader").show();
    var hiddenId = $("#tableDivSalary").find("#hiddenId").val();
    var id = $("#tableDivDeductionSalary").find("#salaryDeductionTable tbody").find('.selected').attr("id");
    $.ajax({
        url: constantSet.addEditDeduction,
        data: { Id: id, SalaryID: hiddenId },
        success: function (data) {
            $("#tableDivDeductionSalary").find('#AddsalaryDeductionDeductionBody').html('');
            $("#tableDivDeductionSalary").find('#AddsalaryDeductionDeductionBody').html(data);
            $("#tableDivDeductionSalary").find(".salaryDeductionDeductionTitle").html("Edit Deduction");
            $("#tableDivDeductionSalary").find("#btn-submit-salaryDeduction").html("Save");
            $('[data-toggle="tooltip"]').tooltip();
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();

        }
    });
});

$("#tableDivSalary").on('click', '.btn-delete-salaryDeduction', function () {

    var hiddenId = $("#tableDivSalary").find("#hiddenId").val();
    var id = $("#tableDivDeductionSalary").find("#salaryDeductionTable tbody").find('.selected').attr("id");
    $.Zebra_Dialog("Are you sure you would like to delete this record?", {
        'type': false,
        'title': 'Delete Salary',
        'width': 350,
        'buttons': [
            {
                caption: 'Cancel',
                callback: function () { }
            }, {
                caption: 'Ok',
                callback: function () {
                    $(".hrtoolLoader").show();
                    var model = {
                        Id: id,
                        EmployeeID: $("#page_content_inner").find("#EmployeeID").val(),
                        EmployeeSalaryID: hiddenId,
                    }
                    $.ajax({
                        type: "POST",
                        data: JSON.stringify(model),
                        url: constantSet.DeleteDeduction,
                        contentType: "application/json",
                        success: function (data) {
                            $("#tableDivSalary").find('#AddSalaryBody').html('');
                            $("#tableDivSalary").find('#AddSalaryBody').html(data);
                            $("#tableDivSalary").find(".salaryTitle").html("Edit Salary");
                            $("#tableDivSalary").find("#btn-submit-Salary").html("Save");
                            DataTable2Design();
                            DataTable3Design();
                            $('[data-toggle="tooltip"]').tooltip();
                            $("#Effective").Zebra_DatePicker({
                                //direction: false,`
                                showButtonPanel: false,
                                format: 'd-m-Y',
                                onSelect: function () {
                                    var fromDate = $('#Effective').val();
                                    $("#validationmessagefromdate").hide();
                                }
                            });
                            $(".hrtoolLoader").hide();
                            $(".modal-backdrop").hide();
                        }
                    });
                }
            }]
    });
});

$('#tableDivSalary').on('keyup', '#FixedAmount', function () {
    
    $("#validationmessageFixedAmount").hide();
    $("#validationmessagePercentOfSalary").hide();
    var total = $("#EmployeeTotalSalary").val().replace(/,/g, '');
    var fixed = $(this).val();
    if (fixed == "00" || fixed == "0.00") {
        $("#validationmessageFixedAmount").show();
        $("#validationmessageFixedAmount").html("Invalid inputs.");
        if (fixed != "") {
            $(this).val('0');
        }
        $('#tableDivResource').find('#txt_PercentOfSalary').val('0');
    }
    else {
        var value = ((fixed * 100) / total).toFixed(2);
        $("#txt_PercentOfSalary").val(value);
    }
});
//check_IncludeSalaryPercentage
$("#tableDivSalary").on("change", "#check_IncludeSalaryPercentage", function () {
    
    if ($(this).is(":checked")) {
        $('#FixedAmount').attr('readonly', true);
        $('#txt_PercentOfSalary').attr('readonly', false);
        $("#tableDivEntitlementSalary").find('#FixedAmount').attr('readonly', true);
        $("#tableDivEntitlementSalary").find('#txt_PercentOfSalary').attr('readonly', false);
        var value = $("#tableDivResource").find('#txt_PercentOfSalary').val();
        if (value == "0" || value == "0.00") {
            $('#txt_PercentOfSalary').val('');
        }
    }
    else {
        $('#FixedAmount').attr('readonly', false);
        $('#txt_PercentOfSalary').attr('readonly', true);
        $("#tableDivEntitlementSalary").find('#FixedAmount').attr('readonly', false);
        $("#tableDivEntitlementSalary").find('#txt_PercentOfSalary').attr('readonly', true);
    }
});

$('#tableDivSalary').on('keyup', '#txt_PercentOfSalary', function () {
    $("#validationmessageFixedAmount").hide();
    $("#validationmessagePercentOfSalary").hide();
    var total = $("#EmployeeTotalSalary").val().replace(/,/g, '');
    var fixed = $(this).val();

    if (fixed == "" || fixed == "00" || fixed == "0.00") {
        $("#validationmessagePercentOfSalary").show();
        $("#validationmessagePercentOfSalary").html("Invalid inputs.");
        if (fixed != "") {
            $(this).val('0');
        }
        $('#tableDivResource').find('#FixedAmountTemp').val('0');
    }

    else {
        var value = ((total * fixed) / 100).toFixed(2);
        $("#FixedAmount").val(value);
    }
});

$('#tableDivResource').on('change', '#drp-SalaryDeduction', function () {

    $('#tableDivResource').find('#lbl-error-DeductionList').hide();

});

//Temp Deduction 

$("#tableDivResource").on('click', '.btn-add-salaryDeductionTemp', function () {
    $(".hrtoolLoader").show();
    var IsError = false;
    var hiddenId = $("#tableDivSalary").find("#TempSalaryID").val();
    var EmployeeID = $("#page_content_inner").find("#EmployeeID").val();
    //var tt = $("#tableDivSalary").find("#TotalSalary").val().split(' ')[5];
    //if (tt == null) {
    //   tt = $("#tableDivSalary").find("#TotalSalary").val().split(' ')[4];
    //}
    //var TotalSalary = tt;
    var TotalSalary = $("#tableDivSalary").find('#EmployeeTotalSalaryAmount').val();
    var EffectiveFrom = $("#tableDivSalary").find("#Effective").val();
    var SalaryTypeID = $("#tableDivSalary").find("#drpSalary").val();
    var PaymentFrequencyID = $("#tableDivSalary").find("#drpPayment").val();
    var CurrencyID = $("#tableDivSalary").find("#drpCurrency").val();
    var Amount = $("#tableDivSalary").find("#Amount").val();
    var ReasonforChange = $("#tableDivSalary").find("#drpResonforChange").val();
    var SalaryComments = $("#tableDivSalary").find("#Comments").val();

    var model = {
        Id: hiddenId,
        EmployeeId: EmployeeID,
        EffectiveFrom: EffectiveFrom,
        SalaryTypeID: SalaryTypeID,
        PaymentFrequencyID: PaymentFrequencyID,
        CurrencyID: CurrencyID,
        Amount: Amount,
        TotalSalary: TotalSalary,
        ReasonforChange: ReasonforChange,
        Comments: SalaryComments,
    }
    if (TotalSalary == "0") {
        IsError = true;
        $("#validationmessageAmount").show();
        $("#validationmessageAmount").html("Amount required");

    }
    if (IsError) {
        $(".hrtoolLoader").hide();
        $(".modal-backdrop").hide();
        $("#btn-close-SalaryDeductionTemp").click();
    }
    else {
        //$.ajax({
        //    url: constantSet.SaveSalary,
        //    data: { Id: 0, SalaryID: hiddenId },
        //    success: function (data) {
        $('#addsalaryDeductionTemp').modal('toggle');
        $('#addsalaryDeductionTemp').modal('show');

        $.ajax({
            type: "POST",
            data: JSON.stringify(model),
            url: constantSet.SaveSalary,
            contentType: "application/json",
            success: function (data) {

                $("#tableDivDeductionTempSalary").find('#AddsalaryDeductionTempBody').html('');
                $("#tableDivDeductionTempSalary").find('#AddsalaryDeductionTempBody').html(data);
                $("#tableDivDeductionTempSalary").find(".salaryDeductionTempTitle").html("ADD Deduction");
                $("#tableDivDeductionTempSalary").find("#btn-submit-salaryDeductionTemp").html("Add");
                $("#tableDivDeductionTempSalary").find('#AddsalaryDeductionTempBody').find("#SalaryDeductionIDTemp").val(0);
                $("#tableDivDeductionTempSalary").find('#AddsalaryDeductionTempBody').find("#SalaryIdTemp").val(hiddenId);
                $("#tableDivDeductionTempSalary").find('#AddsalaryDeductionTempBody').find("#EmployeeTotalSalaryTemp").val(Amount);

                $('[data-toggle="tooltip"]').tooltip();
                $(".hrtoolLoader").hide();
                $(".modal-backdrop").hide();

            }
        });
    }
});

$("#tableDivResource").on('click', '#btn-submit-salaryDeductionTemp', function () {
    
    $(".hrtoolLoader").show();
    var iserror = false;
    var Id = $("#tableDivDeductionTempSalary").find("#SalaryDeductionIDTemp").val();
    var SalaryId = $("#tableDivDeductionTempSalary").find("#SalaryIdTemp").val();
    var model = {
        Id: Id,
        EmployeeID: $("#page_content_inner").find("#EmployeeID").val(),
        EmployeeSalaryID: SalaryId,
        DeductionID: $("#tableDivDeductionTempSalary").find("#drp-SalaryDeductionTemp").val(),
        FixedAmount: $("#tableDivDeductionTempSalary").find("#FixedAmountTemp").val(),
        PercentOfSalary: $("#tableDivDeductionTempSalary").find("#txt_PercentOfSalaryTemp").val(),
        IncludeInSalary: $("#tableDivDeductionTempSalary").find("#check_IncludeSalaryTemp").is(":checked"),
        Comments: $("#tableDivDeductionTempSalary").find("#CommentsTemp").val(),

    }
    if (model.DeductionID == "0") {
        iserror = true;
        $("#lbl-error-DeductionList").show();
    }
    if (model.FixedAmount == "" || model.FixedAmount == "0" || model.FixedAmount == "0.00") {
        iserror = true;
        $("#validationmessageFixedAmount").show();
        $("#validationmessageFixedAmount").html("Fiexd amount is required.");
    }
    if (model.PercentOfSalary == "0" || model.PercentOfSalary == "" || model.PercentOfSalary == "0.00") {
        iserror = true;
        $("#validationmessagePercentOfSalary").show();
        $("#validationmessagePercentOfSalary").html("Percent of salary is required.");
    }
    if (iserror) {
        $(".hrtoolLoader").hide();
        $(".modal-backdrop").hide();
        return false;
    }
    else {
        $.ajax({
            type: "POST",
            data: JSON.stringify(model),
            url: constantSet.SaveSalaryDeductionTemp,
            contentType: "application/json",
            success: function (data) {

                $("#tableDivSalary").find('#AddSalaryBody').html('');
                $("#tableDivSalary").find('#AddSalaryBody').html(data);
                $("#tableDivSalary").find(".salaryTitle").html("ADD Salary");
                $("#addsalaryContact").find("#btn-submit-Salary").html("Add");
                //var CurrencyID = $("#tableDivSalary").find("#drpCurrency").val();
                //var SalaryTypeID = $("#tableDivSalary").find("#drpSalary").val();
                //var Currencytext = $("#drpCurrency option[value=" + CurrencyID + "]").text().split('-')[1];
                //var SalaryType = $("#drpSalary option[value=" + SalaryTypeID + "]").text().split('-')[0];
                //var Total = $("#tableDivSalary").find('#TotalSalary').val().split(' ')[4];
                //if (Total == "$") {
                //    Total = 0;
                //}
                //$("#tableDivSalary").find('#TotalSalary').val("Total" + " " + SalaryType + " " + "Salary is" + " " + Currencytext + " " + Total);                
                TotalSalarySet();
                DataTable4Design();
                DataTable5Design();
                $('[data-toggle="tooltip"]').tooltip();
                $("#Effective").Zebra_DatePicker({
                    //direction: false,`
                    showButtonPanel: false,
                    format: 'd-m-Y',
                    onSelect: function () {
                        var fromDate = $('#Effective').val();
                        $("#validationmessagefromdate").hide();
                    }
                });
                $(".hrtoolLoader").hide();
                $(".modal-backdrop").hide();
            }
        });

    }
});

$('#tableDivResource').on('click', '.dataDeductionTempTr', function () {

    if ($(this).hasClass('dataDeductionTempTr')) {
        $('#salaryDeductionTempTable tbody').find('tr.selected').removeClass('selected');
        $(this).addClass('selected');
        $("#tableDivDeductionTempSalary").find(".btn-edit-salaryDeductionTemp").removeAttr('disabled');
        $("#tableDivDeductionTempSalary").find(".btn-delete-salaryDeductionTemp").removeAttr('disabled');
    }
});

$("#tableDivResource").on('click', '.btn-edit-SalaryDeductionTemp', function () {

    $(".hrtoolLoader").show();
    var hiddenId = $("#tableDivSalary").find("#hiddenId").val();
    var id = $("#tableDivDeductionTempSalary").find("#salaryDeductionTempTable tbody").find('.selected').attr("id");
    $.ajax({
        url: constantSet.addEditDeductionTemp,
        data: { Id: id, SalaryID: hiddenId },
        success: function (data) {
            $("#tableDivDeductionTempSalary").find('#AddsalaryDeductionTempBody').html('');
            $("#tableDivDeductionTempSalary").find('#AddsalaryDeductionTempBody').html(data);
            $("#tableDivDeductionTempSalary").find(".salaryDeductionTempTitle").html("Edit Deduction");
            $("#tableDivDeductionTempSalary").find("#btn-submit-salaryDeductionTemp").html("Save");
            $('[data-toggle="tooltip"]').tooltip();
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();

        }
    });
});

$("#tableDivResource").on('click', '.btn-delete-salaryDeductionTemp', function () {

    var hiddenId = $("#tableDivSalary").find("#TempSalaryID").val();
    var id = $("#tableDivDeductionTempSalary").find("#salaryDeductionTempTable tbody").find('.selected').attr("id");
    $.Zebra_Dialog("Are you sure you would like to delete this record?", {
        'type': false,
        'title': 'Delete Salary',
        'width': 350,
        'buttons': [
            {
                caption: 'Cancel',
                callback: function () { }
            }, {
                caption: 'Ok',
                callback: function () {
                    $(".hrtoolLoader").show();
                    var model = {
                        Id: id,
                        EmployeeID: $("#page_content_inner").find("#EmployeeID").val(),
                        EmployeeSalaryID: hiddenId,
                    }
                    $.ajax({
                        type: "POST",
                        data: JSON.stringify(model),
                        url: constantSet.DeleteDeductionTemp,
                        contentType: "application/json",
                        success: function (data) {
                            $("#tableDivSalary").find('#AddSalaryBody').html('');
                            $("#tableDivSalary").find('#AddSalaryBody').html(data);
                            $("#tableDivSalary").find(".salaryTitle").html("Add Salary");
                            $("#tableDivSalary").find("#btn-submit-Salary").html("ADD");
                            //var CurrencyID = $("#tableDivSalary").find("#drpCurrency").val();
                            //var SalaryTypeID = $("#tableDivSalary").find("#drpSalary").val();
                            //var Currencytext = $("#drpCurrency option[value=" + CurrencyID + "]").text().split('-')[1];
                            //var SalaryType = $("#drpSalary option[value=" + SalaryTypeID + "]").text().split('-')[0];
                            //var Total = $("#tableDivSalary").find('#TotalSalary').val().split(' ')[4];
                            //if (Total == "$") {
                            //    Total = 0;
                            //}
                            //$("#tableDivSalary").find('#TotalSalary').val("Total" + " " + SalaryType + " " + "Salary is" + " " + Currencytext + " " + Total);
                            TotalSalarySet();
                            DataTable4Design();
                            DataTable5Design();
                            $('[data-toggle="tooltip"]').tooltip();
                            $("#Effective").Zebra_DatePicker({
                                //direction: false,`
                                showButtonPanel: false,
                                format: 'd-m-Y',
                                onSelect: function () {
                                    var fromDate = $('#Effective').val();
                                    $("#validationmessagefromdate").hide();
                                }
                            });
                            $(".hrtoolLoader").hide();
                            $(".modal-backdrop").hide();
                        }
                    });
                }
            }]
    });
});

$("#tableDivResource").on('click', '#btn-close-SalaryDeductionTemp', function () {

    // $("#addsalaryDeductionTemp").hide();
    $('#addsalaryDeductionTemp').modal('hide');

});

$("#tableDivResource").on('click', '#btn-Cancel-SalaryDeductionTemp', function () {

    //$("#addsalaryDeductionTemp").hide();
    $('#addsalaryDeductionTemp').modal('hide');

});

$("#tableDivResource").on('click', '#btn-Edit-SalaryDeductionTemp', function () {

    //$("#addsalaryDeductionTemp").show();
    $('#addsalaryDeductionTemp').modal('show');
    $(".hrtoolLoader").show();
    var hiddenId = $("#tableDivSalary").find("#hiddenId").val();
    var id = $("#tableDivDeductionTempSalary").find("#salaryDeductionTempTable tbody").find('.selected').attr("id");
    $.ajax({
        url: constantSet.addEditDeductionTemp,
        data: { Id: id, SalaryID: hiddenId },
        success: function (data) {
            $("#tableDivDeductionTempSalary").find('#AddsalaryDeductionTempBody').html('');
            $("#tableDivDeductionTempSalary").find('#AddsalaryDeductionTempBody').html(data);
            $("#tableDivDeductionTempSalary").find(".salaryDeductionTempTitle").html("Edit Deduction");
            $("#tableDivDeductionTempSalary").find("#btn-submit-salaryDeductionTemp").html("Save");

            $('[data-toggle="tooltip"]').tooltip();
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();

        }
    });
});

$('#tableDivResource').on('keyup', '#FixedAmountTemp', function (event) {
    
    $("#validationmessageFixedAmount").hide();
    $("#validationmessagePercentOfSalary").hide();
    var total = $("#EmployeeTotalSalaryTemp").val().replace(/,/g, '');
    var fixed = $(this).val();

    if (fixed == "00" || fixed == "0.00") {
        $("#validationmessageFixedAmount").show();
        $("#validationmessageFixedAmount").html("Invalid inputs.");
        if (fixed != "") {
            $(this).val('0');
        }
        $('#tableDivResource').find('#txt_PercentOfSalaryTemp').val('0');
    }
    else {
        var value = ((fixed * 100) / total).toFixed(2);
        $("#txt_PercentOfSalaryTemp").val(value);
    }
});

$('#tableDivResource').on('change', '#drp-SalaryDeductionTemp', function () {

    $('#tableDivResource').find('#lbl-error-DeductionList').hide();

});
//check_IncludeSalaryPercentage
$("#tableDivResource").on("change", "#check_IncludeSalaryPercentageDTemp", function () {
    
    if ($(this).is(":checked")) {
        $('#FixedAmountTemp').attr('readonly', true);
        $('#txt_PercentOfSalaryTemp').attr('readonly', false);
        var value = $("#tableDivResource").find('#txt_PercentOfSalaryTemp').val();
        if (value == "0" || value == "0.00")
        {
            $('#txt_PercentOfSalaryTemp').val('');
        }
    }
    else {
        $('#FixedAmountTemp').attr('readonly', false);
        $('#txt_PercentOfSalaryTemp').attr('readonly', true);
        
    }
});

$('#tableDivResource').on('keyup', '#txt_PercentOfSalaryTemp', function () {
    
    $("#validationmessageFixedAmount").hide();
    $("#validationmessagePercentOfSalary").hide();
    var total = $("#EmployeeTotalSalaryTemp").val().replace(/,/g, '');
    var fixed = $(this).val();
    if (fixed == "" || fixed == "00" || fixed == "0.00" ) {
        $("#validationmessagePercentOfSalary").show();
        $("#validationmessagePercentOfSalary").html("Invalid inputs.");
        if (fixed != "") {
            $(this).val('0');
        }
        $('#tableDivResource').find('#FixedAmountTemp').val('0');
    }
        
    else {
        var value = ((total * fixed) / 100).toFixed(2);
        $("#FixedAmountTemp").val(value);
    }

  
});

//$('#tableDivResource').on('keydown', '#txt_PercentOfSalaryTemp', function () {
//    if (!$(this).data("value"))
//        $(this).data("value", this.value);
//});

//Temp Entitlement
$("#tableDivSalary").on('click', '.btn-add-SalaryEntitlementTemp', function () {

    $(".hrtoolLoader").show();
    var IsError = false;
    var hiddenId = $("#tableDivSalary").find("#TempSalaryID").val();
    var EmployeeID = $("#page_content_inner").find("#EmployeeID").val();
    //var tt = $("#tableDivSalary").find("#TotalSalary").val().split(' ')[5];
    //if (tt == null) {
    //    tt = $("#tableDivSalary").find("#TotalSalary").val().split(' ')[4];
    //}
    //var TotalSalary = tt;
    var TotalSalary = $("#tableDivSalary").find('#EmployeeTotalSalaryAmount').val();
    var EffectiveFrom = $("#tableDivSalary").find("#Effective").val();
    var SalaryTypeID = $("#tableDivSalary").find("#drpSalary").val();
    var PaymentFrequencyID = $("#tableDivSalary").find("#drpPayment").val();
    var CurrencyID = $("#tableDivSalary").find("#drpCurrency").val();
    var Amount = $("#tableDivSalary").find("#Amount").val();
    var ReasonforChange = $("#tableDivSalary").find("#drpResonforChange").val();
    var SalaryComments = $("#tableDivSalary").find("#Comments").val();

    var model = {
        Id: hiddenId,
        EmployeeId: EmployeeID,
        EffectiveFrom: EffectiveFrom,
        SalaryTypeID: SalaryTypeID,
        PaymentFrequencyID: PaymentFrequencyID,
        CurrencyID: CurrencyID,
        Amount: Amount,
        TotalSalary: TotalSalary,
        ReasonforChange: ReasonforChange,
        Comments: SalaryComments,
    }
    if (TotalSalary == "0") {
        IsError = true;
        $("#validationmessageAmount").show();
        $("#validationmessageAmount").html("Amount required");

    }
    if (IsError) {
        $(".hrtoolLoader").hide();
        $(".modal-backdrop").hide();
        $("#btn-close-SalaryDeductionTemp").click();
    }
    else {
        //$.ajax({
        //    url: constantSet.SaveSalary,
        //    data: { Id: 0, SalaryID: hiddenId },
        //    success: function (data) {
        $('#addsalaryEntitlementTemp').modal('toggle');
        $('#addsalaryEntitlementTemp').modal('show');
        $.ajax({
            type: "POST",
            data: JSON.stringify(model),
            url: constantSet.SaveEntitlement,
            contentType: "application/json",
            success: function (data) {
                $("#tableDivEntitlementTempSalary").find('#AddSalaryEntitlementTempBody').html('');
                $("#tableDivEntitlementTempSalary").find('#AddSalaryEntitlementTempBody').html(data);
                $("#tableDivEntitlementTempSalary").find(".salaryEntitlementTempTitle").html("ADD Entitlement");
                $("#tableDivEntitlementTempSalary").find("#btn-submit-SalaryEntitlementTemp").html("Add");
                $("#tableDivEntitlementTempSalary").find('#AddSalaryEntitlementTempBody').find("#SalaryEntitlementIDTemp").val(0);
                $("#tableDivEntitlementTempSalary").find('#AddSalaryEntitlementTempBody').find("#SalaryIdTemp").val(hiddenId);
                $("#tableDivEntitlementTempSalary").find('#AddSalaryEntitlementTempBody').find("#EmployeeTotalSalaryEntitlementTemp").val(Amount);
                $('[data-toggle="tooltip"]').tooltip();
                $(".hrtoolLoader").hide();
                $(".modal-backdrop").hide();

            }
        });
    }
});

$("#tableDivSalary").on('click', '.btn-edit-SalaryEntitlementTemp', function () {

    $(".hrtoolLoader").show();
    var hiddenId = $("#tableDivSalary").find("#TempSalaryID").val();
    var id = $("#tableDivEntitlementTempSalary").find("#salaryEntitlementTempTable tbody").find('.selected').attr("id");
    $.ajax({
        url: constantSet.addEditEntitlementTemp,
        data: { Id: id, SalaryID: hiddenId },
        success: function (data) {
            $("#tableDivEntitlementTempSalary").find('#AddSalaryEntitlementTempBody').html('');
            $("#tableDivEntitlementTempSalary").find('#AddSalaryEntitlementTempBody').html(data);
            $("#tableDivEntitlementTempSalary").find(".salaryEntitlementTempTitle").html("Edit Entitlement");
            $("#tableDivEntitlementTempSalary").find("#btn-submit-SalaryEntitlementTemp").html("Save");
            $('[data-toggle="tooltip"]').tooltip();
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();

        }
    });
});

$("#tableDivSalary").on('click', '.btn-delete-SalaryEntitlementTemp', function () {

    var hiddenId = $("#tableDivSalary").find("#hiddenId").val();
    var id = $("#tableDivEntitlementTempSalary").find("#salaryEntitlementTempTable tbody").find('.selected').attr("id");
    $.Zebra_Dialog("Are you sure you would like to delete this record?", {
        'type': false,
        'title': 'Delete Salary',
        'width': 350,
        'buttons': [
            {
                caption: 'Cancel',
                callback: function () { }
            }, {
                caption: 'Ok',
                callback: function () {
                    $(".hrtoolLoader").show();
                    var model = {
                        Id: id,
                        EmployeeID: $("#page_content_inner").find("#EmployeeID").val(),
                        EmployeeSalaryID: hiddenId,
                    }
                    $.ajax({
                        type: "POST",
                        data: JSON.stringify(model),
                        url: constantSet.DeleteEntitlementTemp,
                        contentType: "application/json",
                        success: function (data) {
                            $("#tableDivSalary").find('#AddSalaryBody').html('');
                            $("#tableDivSalary").find('#AddSalaryBody').html(data);
                            $("#tableDivSalary").find(".salaryTitle").html("ADD Salary");
                            $("#tableDivSalary").find("#btn-submit-Salary").html("ADD");
                            //var CurrencyID = $("#tableDivSalary").find("#drpCurrency").val();
                            //var SalaryTypeID = $("#tableDivSalary").find("#drpSalary").val();
                            //var Currencytext = $("#drpCurrency option[value=" + CurrencyID + "]").text().split('-')[1];
                            //var SalaryType = $("#drpSalary option[value=" + SalaryTypeID + "]").text().split('-')[0];
                            //var Total = $("#tableDivSalary").find('#TotalSalary').val().split(' ')[4];
                            //if (Total == "$") {
                            //    Total = 0;
                            //}
                            //$("#tableDivSalary").find('#TotalSalary').val("Total" + " " + SalaryType + " " + "Salary is" + " " + Currencytext + " " + Total);
                            TotalSalarySet();
                            DataTable4Design();
                            DataTable5Design();
                            $('[data-toggle="tooltip"]').tooltip();
                            $("#Effective").Zebra_DatePicker({
                                //direction: false,`
                                showButtonPanel: false,
                                format: 'd-m-Y',
                                onSelect: function () {
                                    var fromDate = $('#Effective').val();
                                    $("#validationmessagefromdate").hide();
                                }
                            });
                            $(".hrtoolLoader").hide();
                            $(".modal-backdrop").hide();
                        }
                    });
                }
            }]
    });
});

$("#tableDivSalary").on('click', '#btn-submit-SalaryEntitlementTemp', function () {

    $(".hrtoolLoader").show();
    var iserror = false;
    var Id = $("#tableDivEntitlementTempSalary").find("#SalaryEntitlementIDTemp").val();
    var SalaryId = $("#tableDivEntitlementTempSalary").find("#SalaryIdTemp").val();
    var model = {
        Id: Id,
        EmployeeID: $("#page_content_inner").find("#EmployeeID").val(),
        EmployeeSalaryID: SalaryId,
        EntitlementID: $("#tableDivEntitlementTempSalary").find("#drp-SalaryEntitlementETemp").val(),
        FixedAmount: $("#tableDivEntitlementTempSalary").find("#FixedAmountETemp").val(),
        PercentOfSalary: $("#tableDivEntitlementTempSalary").find("#txt_PercentOfSalaryETemp").val(),
        IncludeInSalary: $("#tableDivEntitlementTempSalary").find("#check_IncludeSalaryETemp").is(":checked"),
        Comments: $("#tableDivEntitlementTempSalary").find("#CommentsETemp").val(),

    }
    if (model.EntitlementID == "0") {
        iserror = true;
        $("#lbl-error-EntitlementList").show();
    }
    if (model.FixedAmount == "" || model.FixedAmount == "0" || model.FixedAmount == "0.00") {
        iserror = true;
        $("#validationmessageFixedAmount").show();
        $("#validationmessageFixedAmount").html("Fixed Amount is required.");
    }
    if (model.FixedAmount == "" || model.PercentOfSalary == "0" || model.PercentOfSalary == "0.00") {
        iserror = true;
        $("#validationmessagePercentOfSalary").show();
        $("#validationmessagePercentOfSalary").html("Percent of salary is required.");
    }
    if (iserror) {
        $(".hrtoolLoader").hide();
        $(".modal-backdrop").hide();
        return false;
    }
    else {
        $.ajax({
            type: "POST",
            data: JSON.stringify(model),
            url: constantSet.SaveSalaryEntitlementTemp,
            contentType: "application/json",
            success: function (data) {
                $("#tableDivSalary").find('#AddSalaryBody').html('');
                $("#tableDivSalary").find('#AddSalaryBody').html(data);
                $("#tableDivSalary").find(".salaryTitle").html("Add Salary");
                $("#tableDivSalary").find("#btn-submit-Salary").html("ADD");
                //var CurrencyID = $("#tableDivSalary").find("#drpCurrency").val();
                //var SalaryTypeID = $("#tableDivSalary").find("#drpSalary").val();
                //var Currencytext = $("#drpCurrency option[value=" + CurrencyID + "]").text().split('-')[1];
                //var SalaryType = $("#drpSalary option[value=" + SalaryTypeID + "]").text().split('-')[0];
                //var Total = $("#tableDivSalary").find('#TotalSalary').val().split(' ')[4];
                //if (Total == "$") {
                //    Total = 0;
                //}
                //$("#tableDivSalary").find('#TotalSalary').val("Total" + " " + SalaryType + " " + "Salary is" + " " + Currencytext + " " + Total);
                TotalSalarySet();
                DataTable4Design();
                DataTable5Design();
                $('[data-toggle="tooltip"]').tooltip();
                $("#Effective").Zebra_DatePicker({
                    //direction: false,`
                    showButtonPanel: false,
                    format: 'd-m-Y',
                    onSelect: function () {
                        var fromDate = $('#Effective').val();
                        $("#validationmessagefromdate").hide();
                    }
                });
                $(".hrtoolLoader").hide();
                $(".modal-backdrop").hide();
            }
        });
    }
});

$('#tableDivSalary').on('click', '.dataTrEntitlementTemp', function () {

    if ($(this).hasClass('dataTrEntitlementTemp')) {
        $('#salaryEntitlementTempTable tbody').find('tr.selected').removeClass('selected');
        $(this).addClass('selected');
        $("#tableDivEntitlementTempSalary").find(".btn-edit-SalaryEntitlementTemp").removeAttr('disabled');
        $("#tableDivEntitlementTempSalary").find(".btn-delete-SalaryEntitlementTemp").removeAttr('disabled');
    }
});

$("#tableDivResource").on('click', '#btn-close-SalaryEntitlementTemp', function () {

    // $("#addsalaryEntitlementTemp").hide();
    $('#addsalaryEntitlementTemp').modal('hide');
});

$("#tableDivResource").on('click', '#btn-Cancel-SalaryEntitlementTemp', function () {

    //$("#addsalaryEntitlementTemp").hide();
    $('#addsalaryEntitlementTemp').modal('hide');

});

$("#tableDivResource").on('click', '#btn-Edit-SalaryEntitlementTemp', function () {

    // $("#addsalaryEntitlementTemp").show();
    $('#addsalaryEntitlementTemp').modal('show');
    $(".hrtoolLoader").show();
    var hiddenId = $("#tableDivSalary").find("#TempSalaryID").val();
    var id = $("#tableDivEntitlementTempSalary").find("#salaryEntitlementTempTable tbody").find('.selected').attr("id");
    $.ajax({
        url: constantSet.addEditEntitlementTemp,
        data: { Id: id, SalaryID: hiddenId },
        success: function (data) {
            $("#tableDivEntitlementTempSalary").find('#AddSalaryEntitlementTempBody').html('');
            $("#tableDivEntitlementTempSalary").find('#AddSalaryEntitlementTempBody').html(data);
            $("#tableDivEntitlementTempSalary").find(".salaryEntitlementTempTitle").html("Edit Entitlement");
            $("#tableDivEntitlementTempSalary").find("#btn-submit-SalaryEntitlementTemp").html("Save");
            $('[data-toggle="tooltip"]').tooltip();
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();

        }
    });
});

$('#tableDivResource').on('keyup', '#FixedAmountETemp', function () {
    
    $('#tableDivResource').find("#validationmessageFixedAmount").hide();
    $('#tableDivResource').find("#validationmessagePercentOfSalary").hide();
    var total = $("#EmployeeTotalSalaryEntitlementTemp").val().replace(/,/g, '');
    var fixed = $(this).val();
    if (fixed == "00" || fixed == "0.00") {
        $("#validationmessageFixedAmount").show();
        $("#validationmessageFixedAmount").html("Invalid inputs.");
        $(this).val('0');
        $('#tableDivResource').find('#txt_PercentOfSalaryETemp').val('0');
    }
    else {
        var value = ((fixed * 100) / total).toFixed(2);
        $("#txt_PercentOfSalaryETemp").val(value);
    }
});

//check_IncludeSalaryPercentage
$("#tableDivResource").on("change", "#check_IncludeSalaryPercentageETemp", function () {
    $('#tableDivResource').find("#validationmessageFixedAmount").hide();
    $('#tableDivResource').find("#validationmessagePercentOfSalary").hide();
    if ($(this).is(":checked")) {
        $('#FixedAmountETemp').attr('readonly', true);
        $('#txt_PercentOfSalaryETemp').attr('readonly', false);
        var value = $('#txt_PercentOfSalaryETemp').val();
        if (value == "0" || value == "0.00") {
            $("#tableDivResource").find('#txt_PercentOfSalaryETemp').val('');
        }
    }
    else {
        $('#FixedAmountETemp').attr('readonly', false);
        $('#txt_PercentOfSalaryETemp').attr('readonly', true);
    }
});

$('#tableDivResource').on('keyup', '#txt_PercentOfSalaryETemp', function () {
    
    $('#tableDivResource').find("#validationmessageFixedAmount").hide();
    $('#tableDivResource').find("#validationmessagePercentOfSalary").hide();
    var total = $("#EmployeeTotalSalaryEntitlementTemp").val().replace(/,/g, '');
    var fixed = $(this).val();
    if (fixed == "" || fixed == "00" || fixed == "0.00") {
        $("#validationmessagePercentOfSalary").show();
        $("#validationmessagePercentOfSalary").html("Invalid inputs.");
        if (fixed != "") {
            $(this).val('0');
        }
        $('#tableDivResource').find('#FixedAmountETemp').val('0');
    }

    else {
        var value = ((total * fixed) / 100).toFixed(2);
        $("#FixedAmountETemp").val(value);
    }
});

$('#tableDivResource').on('change', '#drp-SalaryEntitlementETemp', function () {
    $('#tableDivSalary').find('#lbl-error-EntitlementList').hide();

});


//SalaryEntitlement
$("#tableDivSalary").on('click', '.btn-add-SalaryEntitlement', function () {
    $(".hrtoolLoader").show();
    var hiddenId = $("#tableDivSalary").find("#hiddenId").val();
    $.ajax({
        url: constantSet.addEditEntitlement,
        data: { Id: 0, SalaryID: hiddenId },
        success: function (data) {
            $("#tableDivEntitlementSalary").find('#AddSalaryEntitlementBody').html('');
            $("#tableDivEntitlementSalary").find('#AddSalaryEntitlementBody').html(data);
            $("#tableDivEntitlementSalary").find(".salaryEntitlementTitle").html("ADD Entitlement");
            $("#tableDivEntitlementSalary").find("#btn-submit-SalaryEntitlement").html("Add");
            $('[data-toggle="tooltip"]').tooltip();
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();

        }
    });
});

$("#tableDivSalary").on('click', '.btn-edit-SalaryEntitlement', function () {
    $(".hrtoolLoader").show();
    var hiddenId = $("#tableDivSalary").find("#hiddenId").val();
    var id = $("#tableDivEntitlementSalary").find("#salaryEntitlementTable tbody").find('.selected').attr("id");
    $.ajax({
        url: constantSet.addEditEntitlement,
        data: { Id: id, SalaryID: hiddenId },
        success: function (data) {
            $("#tableDivEntitlementSalary").find('#AddSalaryEntitlementBody').html('');
            $("#tableDivEntitlementSalary").find('#AddSalaryEntitlementBody').html(data);
            $("#tableDivEntitlementSalary").find(".salaryEntitlementTitle").html("Edit Entitlement");
            $("#tableDivEntitlementSalary").find("#btn-submit-SalaryEntitlement").html("Save");
            $('[data-toggle="tooltip"]').tooltip();
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();

        }
    });
});

$("#tableDivSalary").on('click', '.btn-delete-SalaryEntitlement', function () {

    var hiddenId = $("#tableDivSalary").find("#hiddenId").val();
    var id = $("#tableDivEntitlementSalary").find("#salaryEntitlementTable tbody").find('.selected').attr("id");
    $.Zebra_Dialog("Are you sure you would like to delete this record?", {
        'type': false,
        'title': 'Delete Salary',
        'width': 350,
        'buttons': [
            {
                caption: 'Cancel',
                callback: function () { }
            }, {
                caption: 'Ok',
                callback: function () {
                    $(".hrtoolLoader").show();
                    var model = {
                        Id: id,
                        EmployeeID: $("#page_content_inner").find("#EmployeeID").val(),
                        EmployeeSalaryID: hiddenId,
                    }
                    $.ajax({
                        type: "POST",
                        data: JSON.stringify(model),
                        url: constantSet.DeleteEntitlement,
                        contentType: "application/json",
                        success: function (data) {
                            $("#tableDivSalary").find('#AddSalaryBody').html('');
                            $("#tableDivSalary").find('#AddSalaryBody').html(data);
                            $("#tableDivSalary").find(".salaryTitle").html("Edit Salary");
                            $("#tableDivSalary").find("#btn-submit-Salary").html("Save");
                            //var CurrencyID = $("#tableDivSalary").find("#drpCurrency").val();
                            //var SalaryTypeID = $("#tableDivSalary").find("#drpSalary").val();
                            //var Currencytext = $("#drpCurrency option[value=" + CurrencyID + "]").text().split('-')[1];
                            //var SalaryType = $("#drpSalary option[value=" + SalaryTypeID + "]").text().split('-')[0];
                            //var Total = $("#tableDivSalary").find('#TotalSalary').val().split(' ')[4];
                            //if (Total == "$") {
                            //    Total = 0;
                            //}
                            //$("#tableDivSalary").find('#TotalSalary').val("Total" + " " + SalaryType + " " + "Salary is" + " " + Currencytext + " " + Total);
                            TotalSalarySet();
                            DataTable2Design();
                            DataTable3Design();
                            $('[data-toggle="tooltip"]').tooltip();
                            $("#Effective").Zebra_DatePicker({
                                //direction: false,`
                                showButtonPanel: false,
                                format: 'd-m-Y',
                                onSelect: function () {
                                    var fromDate = $('#Effective').val();
                                    $("#validationmessagefromdate").hide();
                                }
                            });
                            $(".hrtoolLoader").hide();
                            $(".modal-backdrop").hide();
                        }
                    });
                }
            }]
    });
});

$("#tableDivSalary").on('click', '#btn-submit-SalaryEntitlement', function () {

    $(".hrtoolLoader").show();
    var iserror = false;
    var Id = $("#tableDivEntitlementSalary").find("#SalaryEntitlementID").val();
    var SalaryId = $("#tableDivEntitlementSalary").find("#SalaryId").val();
    var model = {
        Id: Id,
        EmployeeID: $("#page_content_inner").find("#EmployeeID").val(),
        EmployeeSalaryID: SalaryId,
        EntitlementID: $("#tableDivEntitlementSalary").find("#drp-SalaryEntitlement").val(),
        FixedAmount: $("#tableDivEntitlementSalary").find("#FixedAmount").val(),
        PercentOfSalary: $("#tableDivEntitlementSalary").find("#txt_PercentOfSalary").val(),
        IncludeInSalary: $("#tableDivEntitlementSalary").find("#check_IncludeSalary").is(":checked"),
        Comments: $("#tableDivEntitlementSalary").find("#Comments").val(),

    }
    if (model.EntitlementID == "0") {
        iserror = true;
        $("#lbl-error-EntitlementList").show();
    }
    if (model.FixedAmount == "" || model.FixedAmount == "0" || model.FixedAmount == "0.00") {
        iserror = true;
        $("#validationmessageFixedAmount").show();
        $("#validationmessageFixedAmount").html("Fixed amount is required.");
    }
    if (model.PercentOfSalary == "0" || model.PercentOfSalary == "" || model.PercentOfSalary == "0.00") {
        iserror = true;
        $("#validationmessagePercentOfSalary").show();
        $("#validationmessagePercentOfSalary").html("Percent of salary is required.");
    }
    if (iserror) {
        $(".hrtoolLoader").hide();
        $(".modal-backdrop").hide();
        return false;
    }
    else {
        $.ajax({
            type: "POST",
            data: JSON.stringify(model),
            url: constantSet.SaveSalaryEntitlement,
            contentType: "application/json",
            success: function (data) {
                $("#tableDivSalary").find('#AddSalaryBody').html('');
                $("#tableDivSalary").find('#AddSalaryBody').html(data);
                $("#tableDivSalary").find(".salaryTitle").html("Edit Salary");
                $("#tableDivSalary").find("#btn-submit-Salary").html("Save");
                //var CurrencyID = $("#tableDivSalary").find("#drpCurrency").val();
                //var SalaryTypeID = $("#tableDivSalary").find("#drpSalary").val();
                //var Currencytext = $("#drpCurrency option[value=" + CurrencyID + "]").text().split('-')[1];
                //var SalaryType = $("#drpSalary option[value=" + SalaryTypeID + "]").text().split('-')[0];
                //var Total = $("#tableDivSalary").find('#TotalSalary').val().split(' ')[4];
                //if (Total == "$") {
                //    Total = 0;
                //}
                //$("#tableDivSalary").find('#TotalSalary').val("Total" + " " + SalaryType + " " + "Salary is" + " " + Currencytext + " " + Total);
                TotalSalarySet();
                DataTable2Design();
                DataTable3Design();
                $('[data-toggle="tooltip"]').tooltip();
                $("#Effective").Zebra_DatePicker({
                    //direction: false,`
                    showButtonPanel: false,
                    format: 'd-m-Y',
                    onSelect: function () {
                        var fromDate = $('#Effective').val();
                        $("#validationmessagefromdate").hide();
                    }
                });
                $(".hrtoolLoader").hide();
                $(".modal-backdrop").hide();
            }
        });
    }
});

$('#tableDivSalary').on('click', '.dataTrEntitlement', function () {
    if ($(this).hasClass('dataTrEntitlement')) {
        $('#salaryEntitlementTable tbody').find('tr.selected').removeClass('selected');
        $(this).addClass('selected');
        $("#tableDivEntitlementSalary").find(".btn-edit-SalaryEntitlement").removeAttr('disabled');
        $("#tableDivEntitlementSalary").find(".btn-delete-SalaryEntitlement").removeAttr('disabled');
    }
});

$('#tableDivSalary').on('change', '#drp-SalaryEntitlement', function () {
    $('#tableDivSalary').find('#lbl-error-EntitlementList').hide();

});

//check_IncludeSalaryPercentage
//$("#tableDivSalary").on("change", "#check_IncludeSalaryPercentage", function () {
//    
//    if ($(this).is(":checked")) {
//        $('#FixedAmount').attr('readonly', true);
//        $('#txt_PercentOfSalary').attr('readonly', false);
//    }
//    else {
//        $('#FixedAmount').attr('readonly', false);
//        $('#txt_PercentOfSalary').attr('readonly', true);
//    }
//});

//$('#tableDivSalary').on('keyup', '#txt_PercentOfSalary', function () {
//    
//    var total = $("#EmployeeTotalSalary").val().replace(/,/g, '');
//    var fixed = $(this).val();

//    var value = ((total * fixed) / 100).toFixed(2);

//    $("#FixedAmount").val(value);
//});


function TotalSalarySet() {
    

    var amount = $("#tableDivSalary").find("#Amount").val();
    if (amount == "") {
        amount = 0;
    }
    var CurrencyID = $("#tableDivSalary").find("#drpCurrency").val();
    var Currencytext = $("#drpCurrency option[value=" + CurrencyID + "]").text().split("-")[1];
    var SalaryTypeID = $("#tableDivSalary").find("#drpSalary").val();
    var SalaryType = $("#drpSalary option[value=" + SalaryTypeID + "]").text();
    var Total = $("#tableDivSalary").find('#EmployeeTotalSalaryAmount').val();
    if (Total == "" || Total == undefined) {
        var Total = amount;
    }
    if (SalaryType == "Hourly") {
        $("#tableDivSalary").find('#TotalSalary').val("Total Salary is" + " " + Currencytext + " " + Total + " per hour");
    }
    else {
        $("#tableDivSalary").find('#TotalSalary').val("Total" + " " + SalaryType + " " + "Salary is" + " " + Currencytext + " " + Total);

    }
    $("#tableDivSalary").find('#EmployeeTotalSalaryAmount').val(Total);
}

