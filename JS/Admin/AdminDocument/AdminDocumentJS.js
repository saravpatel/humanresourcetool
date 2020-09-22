$(document).ready(function () {
    DataTableDesign();
});

function DataTableDesign() {
    $('#DocumentModalTable tfoot tr').appendTo('#DocumentModalTable thead');
    var table = $("#tableDiv").find('#DocumentModalTable').DataTable({
        "sDom": '<"top"i>rt<"bottom"flp><"clear">',
        "scrollX": true,
        "bSort": false
    });
    $('#tableDiv').find('.dataTables_filter').hide();
    $('#tableDiv').find('.dataTables_info').hide();
    $("#tableDiv").on('keyup', '.SearchType', function () {
        table.column(0).search(this.value).draw();
    });
    $("#tableDiv").on('keyup', '.SearchDescription', function () {
        table.column(1).search(this.value).draw();
    });
    $("#tableDiv").on('keyup', '.SearchName', function () {
        table.column(2).search(this.value).draw();
    });
    $("#tableDiv").on('keyup', '.SearchCategory', function () {
        table.column(3).search(this.value).draw();
    });
    $("#tableDiv").on('keyup', '.SearchBusiness', function () {
        table.column(4).search(this.value).draw();
    });

    $("#tableDiv").on('keyup', '.SearchDivision', function () {
        table.column(5).search(this.value).draw();
    });
    $("#tableDiv").on('keyup', '.SearchPool', function () {
        table.column(6).search(this.value).draw();
    });
    $("#tableDiv").on('keyup', '.SearchFunction', function () {
        table.column(7).search(this.value).draw();
    });
    $("#tableDiv .SearchCreated").Zebra_DatePicker({
        //direction: false,
        showButtonPanel: false,
        format: 'd-m-Y',
        default_position: 'below',
        onSelect: function () {
            var date = $("#tableDiv").find('.SearchCreated').val();
            table.column(8).search(date).draw();
        }
    });
    $("body").on('click', '.dp_clear', function () {
        var date = $("#DocumentModalTable").find("thead").find('.SearchCreated').val();
        table.column(8).search(date).draw();
    });
}

$('#tableDiv').on('click', '.dataTr', function () {
    if ($(this).hasClass('dataTr')) {
        $('#DocumentModalTable tbody').find('tr.selected').removeClass('selected');
        $(this).addClass('selected');
        $("#tableDiv").find(".btn-edit-Document").removeAttr('disabled');
        $("#tableDiv").find(".btn-delete-Document").removeAttr('disabled');
    }
});

$('#tableDiv').on('dblclick', '.dataTr', function (event) {

    if ($(this).hasClass('dataTr')) {
        $('#DocumentModalTable tbody').find('tr.selected').removeClass('selected');
        $(this).addClass('selected');
        $(".btn-edit-Document").click();
    }
});

$("#tableDiv").on('click', '.btn-Refresh-Document', function () {
    window.location.reload();
});

$("#tableDiv").on('click', '.btn-ClearSorting-Document', function () {
    window.location.reload();
});

$("#tableDiv").on('click', '.btn-clearFilter-Document', function () {
    window.location.reload();
});
$("#DocumentModal").find("#AddDocumentBody").on("change", "#drpBusiness", function () {

    var value = $(this).val();
    if (value != "0") {
        $.ajax({
            url: AdminconstantDocument.bindDiv,
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

$("#DocumentModal").find("#AddDocumentBody").on("change", "#drpDivision", function () {
    var value = $(this).val();
    if (value != "0") {
        $.ajax({
            url: AdminconstantDocument.bindpool,
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
                    url: AdminconstantDocument.bindFuncation,
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

$("#tableDiv").on('click', '.btn-add-Document', function () {
    $(".hrtoolLoader").show();
    $.ajax({
        type: "POST",
        url: AdminconstantDocument.Open,
        data: { Id: 0 },
        success: function (data) {
            $("#AddDocumentBody").html('');
            $('#AddDocumentBody').html(data);
            $('#DocumentModal').find('.DocumentTitle').text("Add Document");
            $('#AddDocumentBody').find('#btn-submit-Document').html('ADD');
            $('[data-toggle="tooltip"]').tooltip();
            //$('#AddDocumentBody').find("#IpAddress").val(details["ip"]);
            //alert($("#IpAddress").val());
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
            //$.ajax({
            //    url: 'https://api.ipify.org/?format=json',
            //    success: function (details) {
            //        $("#AddDocumentBody").html('');
            //        $('#AddDocumentBody').html(data);
            //        $('#DocumentModal').find('.DocumentTitle').text("Add Document");
            //        $('#AddDocumentBody').find('#btn-submit-Document').html('ADD');
            //        $('#AddDocumentBody').find("#IpAddress").val(details["ip"]);
            //        //alert($("#IpAddress").val());
            //        $(".hrtoolLoader").hide();
            //        $(".modal-backdrop").hide();
            //    },
            //    error: function () {
            //        window.location.reload();
            //    }
            //});
        }
    });
});

$("#tableDiv").on('click', '.btn-edit-Document', function () {

    $(".hrtoolLoader").show();
    var id = $("#tableDiv").find("#DocumentModalTable tbody").find('.selected').attr("id");
    $.ajax({
        type: "POST",
        url: AdminconstantDocument.Open,
        data: { Id: id },
        success: function (data) {
            $("#AddDocumentBody").html('');
            $('#AddDocumentBody').html(data);
            $('#DocumentModal').find('.DocumentTitle').text("Edit Document");
            $('#AddDocumentBody').find('#btn-submit-Document').html('Save');
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
        }
    });
});

$("#DocumentModal").find("#AddDocumentBody").on("click", ".btn-Document", function () {
    var isError = false;
    $("#DocumentModal").find("#AddDocumentBody").find("#lbl-error-Description").hide();
    $("#DocumentModal").find("#AddDocumentBody").find("#lbl-error-DisplayText").hide();
    $("#DocumentModal").find("#AddDocumentBody").find("#lbl-error-LinkURL").hide();

    $("#DocumentModal").find("#AddDocumentBody").find("#txt_DisplayText").val('');
    $("#DocumentModal").find("#AddDocumentBody").find("#txt_LinkURL").val('');

    $("#DocumentModal").find("#AddDocumentBody").find("#Linkbutton").addClass("hide");
    $("#DocumentModal").find("#AddDocumentBody").find(".btn-Link").removeClass("active");


    $("#DocumentModal").find("#AddDocumentBody").find("#DocumentButton").removeClass("hide");
    $("#DocumentModal").find("#AddDocumentBody").find(".btn-Document").addClass("active");

});

$("#DocumentModal").find("#AddDocumentBody").on("click", ".btn-Link", function () {
    var isError = false;
    $("#DocumentModal").find("#AddDocumentBody").find("#lbl-error-Description").hide();
    $("#DocumentModal").find("#AddDocumentBody").find("#lbl-error-DisplayText").hide();
    $("#DocumentModal").find("#AddDocumentBody").find("#lbl-error-LinkURL").hide();

    $("#DocumentModal").find("#AddDocumentBody").find("#txt_FilepathName")["0"].innerText = '';
    $("#DocumentModal").find("#AddDocumentBody").find("#FilePathName").val('');
    $("#DocumentModal").find("#AddDocumentBody").find("#txt_Description").val('');

    $("#DocumentModal").find("#AddDocumentBody").find("#DocumentButton").addClass("hide");
    $("#DocumentModal").find("#AddDocumentBody").find(".btn-Document").removeClass("active");
    $("#DocumentModal").find("#AddDocumentBody").find("#Linkbutton").removeClass("hide");
    $("#DocumentModal").find("#AddDocumentBody").find(".btn-Link").addClass("active");

});

//btn-Employee-Access 
$("#page_content").on('change', '#check_EmployeeAccess', function () {

    if ($(this).is(':checked') == true) {
        //$("#btn-Employee-Access").find("#check_SpecificWorker").prop('checked', false);
        //$("#btn-Manager-Access").find("#check_SpecificManager").prop('checked', false);
        //$("#btn-Customer-Access").find("#check_SpecificCustomer").prop('checked', false);
        if ($("#btn-Employee-Access").find("#SpecificWorker_Id").val() == undefined || $("#btn-Customer-Access").find("#SpecificCustomer_Id").val() == "") {
            $("#btn-Employee-Access").find("#Worker_Display").addClass("hide");
            $('#selectID').val("");
        }
        else {
            $("#Worker_Show").addClass("hide");
            $('#selectID').val("");
        }
        if ($("#btn-Manager-Access").find("#SpecificManager_Id").val() == undefined || $("#btn-Customer-Access").find("#SpecificCustomer_Id").val() == "") {
            $("#btn-Manager-Access").find("#Manager_Display").addClass("hide");
            $('#empManagerID').val("");

        }
        else {
            $("#Manager_Show").addClass("hide");
            $('#empManagerID').val("");

        }
        if ($("#btn-Customer-Access").find("#SpecificCustomer_Id").val() == undefined || $("#btn-Customer-Access").find("#SpecificCustomer_Id").val() == "") {

            $("#btn-Customer-Access").find("#Customer_Display").addClass("hide");
            $('#selectCustomerID').val("");


        } else {
            $("#Customer_Show").addClass("hide");
            $('#selectCustomerID').val("");

        }
        $("#empNameText").hide();

    }
    else {
    }
});

$("#page_content").on('change', '#check_SpecificWorker', function () {
    if ($(this).is(':checked') == true) {
        //$("#btn-Employee-Access").find("#check_EmployeeAccess").prop('checked', false);
        //$("#btn-Manager-Access").find("#check_ManagerAccess").prop('checked', false);
        //$("#btn-Customer-Access").find("#check_CustomerAccess").prop('checked', false);
        if ($("#btn-Manager-Access").find("#SpecificWorker_Id").val() == undefined || $("#btn-Customer-Access").find("#SpecificCustomer_Id").val() == "") {
            $("#Worker_Display").removeClass("hide");
            $("#empNameText").show();
            $("#empNameText").val("");
            $("#drpWorker").val("");
        }
    }
    else {
        $("#empNameText").hide();
        $("#Worker_Display").addClass("hide");
        $("#empNameText").val("");
        $("#drpWorker").val("");
    }
    if ($(this).is(':checked') == false) {
        $("#empNameText").hide();
        $('#selectID').val('');
    }
});

//btn-Manager-Access 
$("#page_content").on('change', '#check_ManagerAccess', function () {
    if ($(this).is(':checked') == true) {
        //$("#btn-Employee-Access").find("#check_SpecificWorker").prop('checked', false);
        //$("#btn-Manager-Access").find("#check_SpecificManager").prop('checked', false);
        //$("#btn-Customer-Access").find("#check_SpecificCustomer").prop('checked', false);
        if ($("#btn-Employee-Access").find("#SpecificWorker_Id").val() == undefined || $("#btn-Customer-Access").find("#SpecificCustomer_Id").val() == "") {
            $("#btn-Employee-Access").find("#Worker_Display").addClass("hide");
            $('#selectID').val("");
        }
        else {
            $("#Worker_Show").addClass("hide");
            $('#selectID').val("");
        }
        if ($("#btn-Manager-Access").find("#SpecificManager_Id").val() == undefined || $("#btn-Customer-Access").find("#SpecificCustomer_Id").val() == "") {
            $("#btn-Manager-Access").find("#Manager_Display").addClass("hide");
            $('#selectManagerID').val("");

        }
        else {
            $("#Manager_Show").addClass("hide");
            $('#selectManagerID').val("");

        }
        if ($("#btn-Customer-Access").find("#SpecificCustomer_Id").val() == undefined || $("#btn-Customer-Access").find("#SpecificCustomer_Id").val() == "") {

            $("#btn-Customer-Access").find("#Customer_Display").addClass("hide");
            $('#selectCustomerID').val("");


        } else {
            $("#Customer_Show").addClass("hide");
            $('#selectCustomerID').val("");

        }
    }
    else {
        // $("#Customer_Display").removeClass("hide");
        $("#empNameText").hide();

    }
});

$("#page_content").on('change', '#check_SpecificManager', function () {

    if ($(this).is(':checked') == true) {
        //$("#btn-Employee-Access").find("#check_EmployeeAccess").prop('checked', false);
        //$("#btn-Manager-Access").find("#check_ManagerAccess").prop('checked', false);
        //$("#btn-Customer-Access").find("#check_CustomerAccess").prop('checked', false);
        if ($("#btn-Manager-Access").find("#SpecificManager_Id").val() == undefined || $("#btn-Customer-Access").find("#SpecificCustomer_Id").val() == "") {
            $("#empManagerID").show();
            $("#Manager_Display").removeClass("hide");
            $("#empManagerID").val("");
            $("#drpManager").val("");
        }
        else {
            $("#empManagerID").show();
            $("#empManagerID").val("");
            $("#drpManager").val("");
        }
    }
    else {
        $("#empManagerID").hide();
        $("#Manager_Display").addClass("hide");
        $("#empManagerID").val("");
        $("#drpManager").val("");
        $('#selectManagerID').val('');
    }
});

//btn-Customer-Access
$("#page_content").on('change', '#check_CustomerAccess', function () {

    if ($(this).is(':checked') == true) {
        //$("#btn-Employee-Access").find("#check_SpecificWorker").prop('checked', false);
        //$("#btn-Manager-Access").find("#check_SpecificManager").prop('checked', false);
        //$("#btn-Customer-Access").find("#check_SpecificCustomer").prop('checked', false);
        if ($("#btn-Employee-Access").find("#SpecificWorker_Id").val() == undefined || $("#btn-Customer-Access").find("#SpecificCustomer_Id").val() == "") {
            $("#btn-Employee-Access").find("#Worker_Display").addClass("hide");
            $('#selectID').val("");
        }
        else {
            $("#Worker_Show").addClass("hide");
            $('#selectID').val("");
        }
        if ($("#btn-Manager-Access").find("#SpecificManager_Id").val() == undefined || $("#btn-Customer-Access").find("#SpecificCustomer_Id").val() == "") {
            $("#btn-Manager-Access").find("#Manager_Display").addClass("hide");
            $('#selectManagerID').val("");

        }
        else {
            $("#Manager_Show").addClass("hide");
            $('#selectManagerID').val("");

        }
        if ($("#btn-Customer-Access").find("#SpecificCustomer_Id").val() == undefined || $("#btn-Customer-Access").find("#SpecificCustomer_Id").val() == "") {

            $("#btn-Customer-Access").find("#Customer_Display").addClass("hide");
            $('#selectCustomerID').val("");


        } else {
            $("#Customer_Show").addClass("hide");
            $('#selectCustomerID').val("");

        }
    }
    else {
    }
});

$("#page_content").on('change', '#check_SpecificCustomer', function () {

    if ($(this).is(':checked') == true) {
        //$("#btn-Employee-Access").find("#check_EmployeeAccess").prop('checked', false);
        //$("#btn-Manager-Access").find("#check_ManagerAccess").prop('checked', false);
        //$("#btn-Customer-Access").find("#check_CustomerAccess").prop('checked', false);
        if ($("#btn-Manager-Access").find("#SpecificCustomer_Id").val() == undefined || $("#btn-Customer-Access").find("#SpecificCustomer_Id").val() == "") {
            $("#Customer_Show").removeClass("hide");
            $("#Customer_Display").removeClass("hide");
            $("#empCustomerID").show();
            $("#empCustomerID").val("");
            $("#drpCustomer").val("");
        }
        else {
            $("#Customer_Show").removeClass("hide");
            $("#Customer_Display").removeClass("hide");
            $("#empCustomerID").show();
            $("#empCustomerID").val("");
            $("#drpCustomer").val("");
        }
    }
    else {
        $("#empCustomerID").hide();
        $("#Customer_Display").addClass("hide");
        $("#Customer_Show").addClass("hide");
        $("#selectCustomerID").val('');
    }
});

function errorMessage() {
    $("#DocumentModal").find("#AddDocumentBody").find("#lbl-error-fileToUpload").hide();
    $(".hrtoolLoader").hide();
    $(".modal-backdrop").hide();
}
//Save Document
$("#page_content").on('click', '#btn-submit-Document', function () {
    var isError = false;
    debugger;
    var id = $("#DocumentModal").find("#AddDocumentBody").find("#Document_ID").val();
    var IpAddress = $("#DocumentModal").find("#AddDocumentBody").find("#IpAddress").val();
    var DocumentOriginalPath = $("#DocumentModal").find("#AddDocumentBody").find("#txt_FilepathName")["0"].innerText;
    var DocumentPath = $("#DocumentModal").find("#AddDocumentBody").find("#FilePathName").val();
    var Description = $("#DocumentModal").find("#AddDocumentBody").find("#txt_Description").val();
    var BusinessID = $("#DocumentModal").find("#AddDocumentBody").find("#drpBusiness").val();
    var DivisionID = $("#DocumentModal").find("#AddDocumentBody").find("#drpDivision").val();
    var PoolID = $("#DocumentModal").find("#AddDocumentBody").find("#drpPool").val();
    var FunctionID = $("#DocumentModal").find("#AddDocumentBody").find("#drpFunction").val();
    var Category = $("#DocumentModal").find("#AddDocumentBody").find("#drpCategory").val();
    var EmployeeAccess = $('#check_EmployeeAccess').is(":checked");
    var ManagerAccess = $('#check_ManagerAccess').is(":checked");
    var CustomerAccess = $('#check_CustomerAccess').is(":checked");
    var SpecificWorker = $('#check_SpecificWorker').is(":checked");
    var WorkerID = $("#DocumentModal").find("#AddDocumentBody").find("#selectID").val();
    var SpecificManager = $('#check_SpecificManager').is(":checked");
    var ManagerID = $("#DocumentModal").find("#AddDocumentBody").find("#selectManagerID").val();
    var SpecificCustomer = $('#check_SpecificCustomer').is(":checked");
    var CustomerID = $("#DocumentModal").find("#AddDocumentBody").find("#selectCustomerID").val();
    var SignatureRequire = $('#check_Signature').is(":checked");
    var LinkDisplayText = $("#DocumentModal").find("#AddDocumentBody").find("#txt_DisplayText").val();
    var LinkURL = $("#DocumentModal").find("#AddDocumentBody").find("#txt_LinkURL").val();
    var count = $("input[name='grpChk']:checked").length;

    if (count == 0) {

        if (Description == "") {
            isError = true;
            $("#DocumentModal").find("#AddDocumentBody").find("#bl-error-Description").show();
            $("#lbl-error-Description").show();            
        }
    }
    if (SpecificCustomer == true) {
        if (CustomerID == 0) {
            isError = true;
            $("#DocumentModal").find("#AddDocumentBody").find("#lbl-error-drpdown").show();
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
        }
    }
    if (SpecificWorker == true) {
        if (WorkerID == 0) {
            isError = true;
            $("#DocumentModal").find("#AddDocumentBody").find("#lbl-error-drpdown").show();
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
        }
    }
    if (SpecificManager == true) {
        if (ManagerID == 0) {
            isError = true;
            $("#DocumentModal").find("#AddDocumentBody").find("#lbl-error-drpdown").show();
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
        }
    }
    if ($("#DocumentModal").find(".btn-Document").hasClass('active')) {
        if (Description == "") {
            isError = true;
            $("#DocumentModal").find("#AddDocumentBody").find("#lbl-error-Description").show();
        }
        if (DocumentPath == "" || DocumentOriginalPath == "") {
            isError = true;
            $("#DocumentModal").find("#AddDocumentBody").find("#lbl-error-fileToUpload").show();
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
        }
    }
    else if ($("#DocumentModal").find(".btn-Link").hasClass('active')) {
        var weburl = checkUrl(LinkURL);
        if (!weburl) {
            isError = true;
            $("#DocumentModal").find("#AddDocumentBody").find("#lbl-error-WebsiteValid").show();
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
        }
        if (LinkDisplayText == "") {
            isError = true;
            $("#DocumentModal").find("#AddDocumentBody").find("#lbl-error-DisplayText").show();
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
        }
        if (LinkURL == "") {
            isError = true;
            $("#DocumentModal").find("#AddDocumentBody").find("#lbl-error-LinkURL").show();
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
        }
    }
    if (!isError) {
        var model = {
            Id: id,
            IpAddress: IpAddress,
            DocumentOriginalPath: DocumentOriginalPath,
            DocumentPath: DocumentPath,
            LinkDisplayText: LinkDisplayText,
            LinkURL: LinkURL,
            Description: Description,
            BusinessID: BusinessID,
            DivisionID: DivisionID,
            PoolID: PoolID,
            FunctionID: FunctionID,
            Category: Category,
            EmployeeAccess: EmployeeAccess,
            ManagerAccess: ManagerAccess,
            CustomerAccess: CustomerAccess,
            SpecificWorker: SpecificWorker,
            WorkerID: WorkerID,
            SpecificManager: SpecificManager,
            ManagerID: ManagerID,
            SpecificCustomer: SpecificCustomer,
            CustomerID: CustomerID,
            SignatureRequire: SignatureRequire,
        }
        $.ajax({
            url: AdminconstantDocument.AddEdit,
            type: 'POST',
            data: JSON.stringify(model),
            contentType: "application/json",
            success: function (data) {

                if (data == "True") {
                    $(".hrtoolLoader").hide();
                    $(".modal-backdrop").hide();
                    $(".toast-success").show();
                    setTimeout(function () {
                        $(".toast-success").hide();
                        window.location.href = AdminconstantDocument.Index;
                    }, 1500);
                }
            }
        });
    }
});

//delete Document 
$("#tableDiv").on('click', '.btn-delete-Document', function () {
    var id = $("#tableDiv").find("#DocumentModalTable tbody").find('.selected').attr("id");
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
                        url: AdminconstantDocument.deleteDocument,
                        data: { Id: id },
                        success: function (data) {
                            if (data == "True") {
                                $(".hrtoolLoader").hide();
                                $(".modal-backdrop").hide();
                                $(".toast-success").show();
                                setTimeout(function () {
                                    $(".toast-success").hide();
                                    window.location.href = AdminconstantDocument.Index;
                                }, 1500);

                            }
                        }
                    });
                }
            }]
    });
});
//FileUploder
$("#page_content").on('change', '#fileToUpload', function (e) {
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
                    url: AdminconstantDocument.ImageData,
                    contentType: false,
                    processData: false,
                    data: imageData,
                    success: function (result) {
                        $('#DocumentModal').find('#txt_FilepathName').html("");
                        $('#DocumentModal').find('#txt_FilepathName').html(result.originalFileName);
                        $('#DocumentModal').find('#FilePathName').val(result.NewFileName);
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

//validation
$("#tableDiv").on('keyup', '#txt_Description', function (e) {
    var isError = false;
    $("#DocumentModal").find("#AddDocumentBody").find("#lbl-error-Description").hide();


});
$("#tableDiv").on('keyup', '#txt_DisplayText', function (e) {
    var isError = false;
    $("#DocumentModal").find("#AddDocumentBody").find("#lbl-error-DisplayText").hide();


});
$("#tableDiv").on('keyup', '#txt_LinkURL', function (e) {
    var isError = false;
    $("#DocumentModal").find("#AddDocumentBody").find("#lbl-error-LinkURL").hide();
    $("#DocumentModal").find("#AddDocumentBody").find("#lbl-error-WebsiteValid").hide();
});
