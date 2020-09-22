var table, imageData;
$(document).ready(function () {
    DataTableDesign();
});
var bol = false;

function DataTableDesign() {
    $('#tableDiv tfoot tr').appendTo('#CustomerCompanyTable thead');
    var table = $('#CustomerCompanyTable').DataTable({
        "sDom": '<"top"i>rt<"bottom"flp><"clear">',
        "scrollX":true
    });
    $('#tableDiv').find('.dataTables_filter').hide();
    $('#tableDiv').find('.dataTables_info').hide();

    $("#tableDiv thead .SearchCustomerName").keyup(function () {
        table.column(0).search(this.value).draw();
    });
    $("#tableDiv thead .SearchCustomerParent").keyup(function () {
        table.column(1).search(this.value).draw();
    });
    $("#tableDiv thead .SearchCustomerCurrency").keyup(function () {
        table.column(2).search(this.value).draw();
    });
    $("#tableDiv thead .SearchCompanyUsers").keyup(function () {
        table.column(3).search(this.value).draw();
    });
}

//table 
$('#tableDiv').on('click', '.dataTr', function () {
    if ($(this).hasClass('dataTr')) {
        $('#CustomerCompanyTable tbody').find('tr.selected').removeClass('selected');
        $(this).addClass('selected');
        $("#tableDiv").find(".btn-edit-CustomerCompany").removeAttr('disabled');
        $("#tableDiv").find(".btn-delete-CustomerCompany").removeAttr('disabled');
    }
});

$('#tableDiv').on('dblclick', '.dataTr', function () {

    if ($(this).hasClass('dataTr')) {
        $(".btn-edit-CustomerCompany").click();
        //$('#CustomerCompanyTable tbody').find('tr.selected').removeClass('selected');
        //$(this).addClass('selected');

    }
});

$("#tableDiv").on('click', '.btn-Refresh-CustomerCompany', function () {
    window.location.reload();
});

$("#tableDiv").on('click', '.btn-ClearSorting-CustomerCompany', function () {
    window.location.reload();
});

$("#tableDiv").on('click', '.btn-clearFilter-CustomerCompany', function () {
    window.location.reload();
});

//FileUpload
$("#tableDiv").on('change', '#fileToUpload', function (e) {
    
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
                    url: constantCmpCustomer.ImageUrl,
                    contentType: false,
                    processData: false,
                    data: imageData,
                    success: function (result) {

                        $('#AddEditCustomerCompany').find('#CompanyLogo').html("");
                        $('#AddEditCustomerCompany').find('#CompanyLogo').html(result.originalFileName);
                        $('#AddEditCustomerCompany').find('#txt_CompanyLogo').val(result.NewFileName);

                        $('#tableDiv').find('#CompanyLogo').html("");
                        $('#tableDiv').find('#CompanyLogo').html(result.originalFileName);
                        $('#tableDiv').find('#txt_CompanyLogo').val(result.NewFileName);

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

//Delete Customer
$("#tableDiv").on('click', '.btn-delete-CustomerCompany', function () {
    var id = $("#tableDiv").find("#CustomerCompanyTable tbody").find('.selected').attr("id");
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
                        url: constantCmpCustomer.DeleteCmp,
                        data: { Id: id },
                        success: function (data) {
                            $("#tableDiv").html('');
                            $("#tableDiv").html(data);
                            DataTableDesign();
                            $(".toast-error").show();
                            setTimeout(function () { $(".toast-error").hide(); }, 1500);
                            $(".hrtoolLoader").hide();
                            $(".modal-backdrop").hide();
                            // location.reload();

                        }
                    });
                }
            }]
    });

});

//Open PopUp  
$("#tableDiv").on('click', '.btn-add-CustomerCompany', function () {
    $(".hrtoolLoader").show();
    $.ajax({
        url: constantCmpCustomer.addEdit,
        data: { Id: 0 },
        success: function (data) {
            $("#tableDiv").find('#AddEditCustomerCompany').html('');
            $("#tableDiv").find('#AddEditCustomerCompany').html(data);
            $("#tableDiv").find('.customerCompanyTitle').text("Company Information");

            $('[data-toggle="tooltip"]').tooltip();

            $("#tableDiv").find('#AddEditCustomerCompany').find('#wizard').smartWizard({
                onLeaveStep: CustomerCallback,
                onFinish: onFinishCallback
            });

            $("#tableDiv").find('#AddEditCustomerCompany').find('.buttonNext').addClass('btn btn-warning');
            $("#tableDiv").find('#AddEditCustomerCompany').find('.buttonPrevious').addClass('btn btn-warning');
            $("#tableDiv").find('#AddEditCustomerCompany').find('.buttonFinish').addClass('btn btn-success');

            $("#tableDiv").find('#AddEditCustomerCompany').find('.buttonPrevious').hide();
            $("#tableDiv").find('#AddEditCustomerCompany').find('.buttonFinish').hide();
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
        }
    });
});
function chkForSSO() {
    bol = validateSSO();
}
function validateSSO() {
    $("#tableDiv").find('#ValidSSONo').hide();
    var ssoNo = $("#tableDiv").find("#txt_CustomerNumber").val();
    var Id = ssoNo;
    $.ajax({
        url: constantCmpCustomer.validSSo,
        data: { ID: Id },
        success: function (data) {
            alert(data);
            bol = JSON.stringify(data);           
        }
    });
    return bol;
}

function CustomerCallback(obj, context) {
    //$("#tableDiv").find('#AddEditCustomerCompany').find
    var isError = false;
    if (context.fromStep == 1) {
        if (bol == "true") {
            isError = true;
            $("#tableDiv").find("#ValidSSONo").show();
        }
        else if (bol == "false") {
            isError = false;
        }
        var CompanyName = $("#tableDiv").find('#AddEditCustomerCompany').find('#txt_CompanyName').val().trim();
        var ShortName = $("#tableDiv").find('#AddEditCustomerCompany').find('#txt_ShortName').val().trim();
        var ParentCompany = $("#tableDiv").find('#AddEditCustomerCompany').find('#txt_ParentCompany').val();
        var Website = $("#tableDiv").find('#AddEditCustomerCompany').find('#txt_Website').val();
        var CurrencyID = $("#tableDiv").find('#AddEditCustomerCompany').find('#drpCurrency').val();
        var PhoneNumber = $("#tableDiv").find('#AddEditCustomerCompany').find('#txt_PhoneNumber').val();
        var Email = $("#tableDiv").find('#AddEditCustomerCompany').find('#txt_Email').val();
        var SecondaryPhoneNumber = $("#tableDiv").find('#AddEditCustomerCompany').find('#txt_SecondaryPhoneNumber').val();
        var BusinessAddress = $("#tableDiv").find('#AddEditCustomerCompany').find('#txt_BusinessAddress').val().trim();
        var MailingAddress = $("#tableDiv").find('#AddEditCustomerCompany').find('#txt_MailingAddress').val().trim();
        var OriginalCompanyLogo = $("#tableDiv").find('#AddEditCustomerCompany').find('#CompanyLogo')["0"].innerText;
        var CompanyLogo = $("#tableDiv").find('#AddEditCustomerCompany').find('#txt_CompanyLogo').val();
        var GeneralNotes = $("#tableDiv").find('#AddEditCustomerCompany').find('#txt_GeneralNotes').val().trim();

        var emailvalidation=isValidEmailAddress(Email);
        var weburl=checkUrl(Website);
        
        if (!emailvalidation) { isError = true; $("#tableDiv").find('#AddEditCustomerCompany').find("#lbl-error-Email").show(); }
      //  if (!weburl) { isError = true; $("#tableDiv").find('#AddEditCustomerCompany').find("#lbl-error-WebsiteValid").show(); }
        if (CompanyName == "") { isError = true; $("#tableDiv").find('#AddEditCustomerCompany').find("#lbl-error-CompanyName").show(); }
        //  if (ParentCompany == "") { isError = true; $("#tableDiv").find('#AddEditCustomerCompany').find("#lbl-error-ParentCompany").show(); }
        if (Website != "") {
            if (!weburl) { isError = true; $("#tableDiv").find('#AddEditCustomerCompany').find("#lbl-error-WebsiteValid").show(); }
        }
        //if (ParentCompany == "") {
        //    isError = true; $("#tableDiv").find('#AddEditCustomerCompany').find("#lbl-error-ParentCompany").show();
        //}
        //if (Website == "") {
        //    isError = true; $("#tableDiv").find('#AddEditCustomerCompany').find("#lbl-error-Website").show();
        //}
        if (isError) {
            return false;
        }
        else {
            if (context.toStep = 2) {
                $("#tableDiv").find('#AddEditCustomerCompany').find('.buttonNext').show();
                $("#tableDiv").find('#AddEditCustomerCompany').find('.buttonPrevious').show();
                $("#tableDiv").find('#AddEditCustomerCompany').find('.buttonFinish').hide();
            }
            else {
                $("#tableDiv").find('#AddEditCustomerCompany').find('.buttonNext').hide();
                $("#tableDiv").find('#AddEditCustomerCompany').find('.buttonPrevious').show();
                $("#tableDiv").find('#AddEditCustomerCompany').find('.buttonFinish').show()
            }
            $("#tableDiv").find('#AddEditCustomerCompany').find("#lbl-error-Email").hide(); 
          //  $("#tableDiv").find('#AddEditCustomerCompany').find("#lbl-error-WebsiteValid").hide();
            $("#tableDiv").find('#AddEditCustomerCompany').find("#lbl-error-CompanyName").hide();
            $("#tableDiv").find('#AddEditCustomerCompany').find("#lbl-error-ParentCompany").hide();
            $("#tableDiv").find('#AddEditCustomerCompany').find("#lbl-error-Website").hide();
            $("#tableDiv").find('#AddEditCustomerCompany').find("#lbl-error-WebsiteValid").hide(); 
         //   $("#tableDiv").find('#AddEditCustomerCompany').find("#lbl-error-ParentCompany").hide();
            return true;
        }
    }
    if (context.fromStep == 2) {
        if (context.toStep == 1) {
            return true;
        }
        else {

            var isError = false;
            var VAT_GST_Number = $("#tableDiv").find('#AddEditCustomerCompany').find('#txt_VAT_GST_Number').val();
            var CreditLimit = $("#tableDiv").find('#AddEditCustomerCompany').find('#txt_CreditLimit').val();
            var PaymentTerms = $("#tableDiv").find('#AddEditCustomerCompany').find('#txt_PaymentTerms').val().trim();
            var CreditStatus = $("#tableDiv").find('#AddEditCustomerCompany').find('#txt_CreditStatus').val().trim();
            var SalesRegions = $("#tableDiv").find('#AddEditCustomerCompany').find('#txt_SalesRegions').val().trim();

            if (VAT_GST_Number == 0) { isError = true; $("#tableDiv").find('#AddEditCustomerCompany').find("#lbl-error-VAT_GST_Number").show(); }
            if (CreditLimit == 0) { isError = true; $("#tableDiv").find('#AddEditCustomerCompany').find("#lbl-error-CreditLimit").show(); }
            if (PaymentTerms == "") { isError = true; $("#tableDiv").find('#AddEditCustomerCompany').find("#lbl-error-PaymentTerms").show(); }

            if (isError) {
                return false;
            }
            else {
                if (context.toStep == 1) {
                    $("#tableDiv").find('#AddEditCustomerCompany').find('.buttonNext').show();
                    $("#tableDiv").find('#AddEditCustomerCompany').find('.buttonPrevious').hide();
                    $("#tableDiv").find('#AddEditCustomerCompany').find('.buttonFinish').hide();
                }
                else {
                    $("#tableDiv").find('#AddEditCustomerCompany').find('.buttonNext').hide();
                    $("#tableDiv").find('#AddEditCustomerCompany').find('.buttonPrevious').show();
                    $("#tableDiv").find('#AddEditCustomerCompany').find('.buttonFinish').show();
                }
                $("#tableDiv").find('#AddEditCustomerCompany').find("#lbl-error-VAT_GST_Number").hide();
                $("#tableDiv").find('#AddEditCustomerCompany').find("#lbl-error-CreditLimit").hide();
                $("#tableDiv").find('#AddEditCustomerCompany').find("#lbl-error-PaymentTerms").hide();
                return true;
            }
        }
    }
    else {
        if (context.toStep == 2) {
            $("#tableDiv").find('#AddEditCustomerCompany').find('.buttonNext').show();
            $("#tableDiv").find('#AddEditCustomerCompany').find('.buttonPrevious').show();
            $("#tableDiv").find('#AddEditCustomerCompany').find('.buttonFinish').hide();
        }
        else {
            $("#tableDiv").find('#AddEditCustomerCompany').find('.buttonNext').show();
            $("#tableDiv").find('#AddEditCustomerCompany').find('.buttonPrevious').hide();
            $("#tableDiv").find('#AddEditCustomerCompany').find('.buttonFinish').hide();
        }

        return true;
    }

}

function onFinishCallback(obj, context) {
    $(".hrtoolLoader").show();
    //step 1
    var id = $("#tableDiv").find('#AddEditCustomerCompany').find('#CustomerCompanyId').val();
    var CompanyName = $("#tableDiv").find('#AddEditCustomerCompany').find('#txt_CompanyName').val().trim();
    var ShortName = $("#tableDiv").find('#AddEditCustomerCompany').find('#txt_ShortName').val().trim();
    var ParentCompany = $("#tableDiv").find('#AddEditCustomerCompany').find('#txt_ParentCompany').val().trim();
    var Website = $("#tableDiv").find('#AddEditCustomerCompany').find('#txt_Website').val();
    var CurrencyID = $("#tableDiv").find('#AddEditCustomerCompany').find('#drpCurrency').val();
    var PhoneNumber = $("#tableDiv").find('#AddEditCustomerCompany').find('#txt_PhoneNumber').val();
    var Email = $("#tableDiv").find('#AddEditCustomerCompany').find('#txt_Email').val();
    var SecondaryPhoneNumber = $("#tableDiv").find('#AddEditCustomerCompany').find('#txt_SecondaryPhoneNumber').val();
    var BusinessAddress = $("#tableDiv").find('#AddEditCustomerCompany').find('#txt_BusinessAddress').val().trim();
    var MailingAddress = $("#tableDiv").find('#AddEditCustomerCompany').find('#txt_MailingAddress').val().trim();
    var OriginalCompanyLogo = $("#tableDiv").find('#AddEditCustomerCompany').find('#CompanyLogo')["0"].innerText;
    var CompanyLogo = $("#tableDiv").find('#AddEditCustomerCompany').find('#txt_CompanyLogo').val();
    var GeneralNotes = $("#tableDiv").find('#AddEditCustomerCompany').find('#txt_GeneralNotes').val().trim();

    //step 2
    var VAT_GST_Number = $("#tableDiv").find('#AddEditCustomerCompany').find('#txt_VAT_GST_Number').val();
    var CreditLimit = $("#tableDiv").find('#AddEditCustomerCompany').find('#txt_CreditLimit').val();
    var PaymentTerms = $("#tableDiv").find('#AddEditCustomerCompany').find('#txt_PaymentTerms').val().trim();
    var CreditStatus = $("#tableDiv").find('#AddEditCustomerCompany').find('#txt_CreditStatus').val().trim();
    var SalesRegions = $("#tableDiv").find('#AddEditCustomerCompany').find('#txt_SalesRegions').val().trim();

    //step3
    var BankName = $("#tableDiv").find('#AddEditCustomerCompany').find('#txt_BankName').val().trim();
    var BankSortCode = $("#tableDiv").find('#AddEditCustomerCompany').find('#txt_BankSortCode').val().trim();
    var AccountNumber = $("#tableDiv").find('#AddEditCustomerCompany').find('#txt_AccountNumber').val().trim();
    var IBAN = $("#tableDiv").find('#AddEditCustomerCompany').find('#txt_IBAN').val().trim();
    var AccountName = $("#tableDiv").find('#AddEditCustomerCompany').find('#txt_AccountName').val().trim();
    var BankAddress = $("#tableDiv").find('#AddEditCustomerCompany').find('#txt_BankAddress').val().trim();
    var TaxGroup = $("#tableDiv").find('#AddEditCustomerCompany').find('#txt_TaxGroup').val().trim();

    var model = {
        Id: id,
        CompanyName: CompanyName,
        ShortName: ShortName,
        ParentCompany: ParentCompany,
        Website:Website,
        CurrencyID: CurrencyID,
        PhoneNumber: PhoneNumber,
        Email: Email,
        SecondaryPhoneNumber: SecondaryPhoneNumber,
        BusinessAddress: BusinessAddress,
        MailingAddress: MailingAddress,
        OriginalCompanyLogo: OriginalCompanyLogo,
        CompanyLogo: CompanyLogo,
        GeneralNotes: GeneralNotes,
        VAT_GST_Number: VAT_GST_Number,
        CreditLimit: CreditLimit,
        PaymentTerms: PaymentTerms,
        CreditStatus: CreditStatus,
        SalesRegions: SalesRegions,
        BankName: BankName,
        BankSortCode: BankSortCode,
        AccountNumber: AccountNumber,
        IBAN: IBAN,
        AccountName: AccountName,
        BankAddress: BankAddress,
        TaxGroup: TaxGroup,

    }

    $.ajax({
        url: constantCmpCustomer.SaveCmp,
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

}

$("#tableDiv").on('click', '.btn-edit-CustomerCompany', function () {
    $(".hrtoolLoader").show();
    var id = $("#tableDiv").find("#CustomerCompanyTable tbody").find('.selected').attr("id");
    $.ajax({
        url: constantCmpCustomer.addEdit,
        data: { Id: id },
        success: function (data) {
            $("#tableDiv").html('');
            $("#tableDiv").html(data);
            //$("#tableDiv").find('.customerCompanyTitle').text("Edit Customer");

            //$('[data-toggle="tooltip"]').tooltip();

            //$("#tableDiv").find('#AddEditCustomerCompany').find('#Customerwizard').smartWizard({
            //    onLeaveStep: CustomerCallback,
            //    onFinish: onFinishCallback
            //});

            //$("#tableDiv").find('#AddEditCustomerCompany').find('.buttonNext').addClass('btn btn-warning');
            //$("#tableDiv").find('#AddEditCustomerCompany').find('.buttonPrevious').addClass('btn btn-warning');
            //$("#tableDiv").find('#AddEditCustomerCompany').find('.buttonFinish').addClass('btn btn-success');

            //$("#tableDiv").find('#AddEditCustomerCompany').find('.buttonPrevious').hide();
            //$("#tableDiv").find('#AddEditCustomerCompany').find('.buttonFinish').hide();
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
        }
    });
});

//btn-Save-CustomerCompany
$("#tableDiv").on('click', '#btn-Save-CustomerCompany', function () {
    var isError = false;
    //step 1
    var id = $("#tableDiv").find('#CustomerCompanyId').val();
    var CompanyName = $("#tableDiv").find('#txt_CompanyName').val().trim();
    var ShortName = $("#tableDiv").find('#txt_ShortName').val().trim();
    var ParentCompany = $("#tableDiv").find('#txt_ParentCompany').val().trim();
    var Website = $("#tableDiv").find('#txt_Website').val();
    var CurrencyID = $("#tableDiv").find('#drpCurrency').val();
    var PhoneNumber = $("#tableDiv").find('#txt_PhoneNumber').val();
    var Email = $("#tableDiv").find('#txt_Email').val();
    var SecondaryPhoneNumber = $("#tableDiv").find('#txt_SecondaryPhoneNumber').val();
    var BusinessAddress = $("#tableDiv").find('#txt_BusinessAddress').val().trim();
    var MailingAddress = $("#tableDiv").find('#txt_MailingAddress').val().trim();
    var OriginalCompanyLogo = $("#tableDiv").find('#CompanyLogo')["0"].innerText;
    var CompanyLogo = $("#tableDiv").find('#txt_CompanyLogo').val();
    var GeneralNotes = $("#tableDiv").find('#txt_GeneralNotes').val().trim();

    //step 2
    var VAT_GST_Number = $("#tableDiv").find('#txt_VAT_GST_Number').val();
    var CreditLimit = $("#tableDiv").find('#txt_CreditLimit').val();
    var PaymentTerms = $("#tableDiv").find('#txt_PaymentTerms').val().trim();
    var CreditStatus = $("#tableDiv").find('#txt_CreditStatus').val().trim();
    var SalesRegions = $("#tableDiv").find('#txt_SalesRegions').val().trim();

    //step3
    var BankName = $("#tableDiv").find('#txt_BankName').val().trim();
    var BankSortCode = $("#tableDiv").find('#txt_BankSortCode').val().trim();
    var AccountNumber = $("#tableDiv").find('#txt_AccountNumber').val().trim();
    var IBAN = $("#tableDiv").find('#txt_IBAN').val().trim();
    var AccountName = $("#tableDiv").find('#txt_AccountName').val().trim();
    var BankAddress = $("#tableDiv").find('#txt_BankAddress').val().trim();
    var TaxGroup = $("#tableDiv").find('#txt_TaxGroup').val().trim();

    var emailvalidation=isValidEmailAddress(Email);
    var weburl=checkUrl(Website);     
    if (!emailvalidation) { isError = true; $("#tableDiv").find("#lbl-error-Email").show(); }
    if (!weburl) { isError = true; $("#tableDiv").find("#lbl-error-WebsiteValid").show(); }
    if (CompanyName == "" || ParentCompany == "" || VAT_GST_Number == 0 || CreditLimit == 0 || PaymentTerms == "") {
    
    if (CompanyName == "") { isError = true; $("#tableDiv").find("#lbl-error-CompanyName").show(); }
    if (ParentCompany == "") { isError = true; $("#tableDiv").find("#lbl-error-ParentCompany").show(); }

    if (VAT_GST_Number == 0) { isError = true; $("#tableDiv").find("#lbl-error-VAT_GST_Number").show(); }
    if (CreditLimit == 0) { isError = true; $("#tableDiv").find("#lbl-error-CreditLimit").show(); }
    if (PaymentTerms == "") { isError = true; $("#tableDiv").find("#lbl-error-PaymentTerms").show(); }
    }
    if(!isError) {
        var model = {
            Id: id,
            CompanyName: CompanyName,
            ShortName: ShortName,
            ParentCompany: ParentCompany,
            Website: Website,
            CurrencyID: CurrencyID,
            PhoneNumber: PhoneNumber,
            Email: Email,
            SecondaryPhoneNumber: SecondaryPhoneNumber,
            BusinessAddress: BusinessAddress,
            MailingAddress: MailingAddress,
            OriginalCompanyLogo: OriginalCompanyLogo,
            CompanyLogo: CompanyLogo,
            GeneralNotes: GeneralNotes,
            VAT_GST_Number: VAT_GST_Number,
            CreditLimit: CreditLimit,
            PaymentTerms: PaymentTerms,
            CreditStatus: CreditStatus,
            SalesRegions: SalesRegions,
            BankName: BankName,
            BankSortCode: BankSortCode,
            AccountNumber: AccountNumber,
            IBAN: IBAN,
            AccountName: AccountName,
            BankAddress: BankAddress,
            TaxGroup: TaxGroup,

        }

        $.ajax({
            url: constantCmpCustomer.SaveCmp,
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
    }

});

//Validation 

$("#tableDiv").on('keyup', '#txt_Email', function () {
    iserror = false;
    $("#tableDiv").find("#lbl-error-Email").hide();

});
$("#tableDiv").on('keyup', '#txt_CompanyName', function () {
    iserror = false;
    $("#tableDiv").find("#lbl-error-CompanyName").hide();

});

//$("#tableDiv").on('keyup', '#txt_Website', function () {
//    iserror = false;
//    $("#tableDiv").find("#lbl-error-WebsiteValid").hide();

//});
//$("#tableDiv").on('keyup', '#txt_ParentCompany', function () {
//    iserror = false;
//    $("#tableDiv").find("#lbl-error-ParentCompany").hide();

//});
//$("#tableDiv").on('keyup', '#txt_PaymentTerms', function () {
//    iserror = false;
//    $("#tableDiv").find("#lbl-error-ParentCompany").hide();

//});
//$("#tableDiv").on('change', '#txt_VAT_GST_Number', function () {
//    iserror = false;
//    $("#tableDiv").find("#lbl-error-VAT_GST_Number").hide();

//});
//$("#tableDiv").on('change', '#txt_CreditLimit', function () {
//    iserror = false;
//    $("#tableDiv").find("#lbl-error-CreditLimit").hide();

//});