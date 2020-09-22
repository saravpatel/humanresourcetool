$(document).ready(function () {
    DataTableDesign();
    NewsDatePic();
    
});
function searchByDateTime()
{
    var fromDate = $("#fromDate").val();
    var ToDate = $("#ToDate").val();
    $.ajax({
        url: constantAdminNews.searchByFromToDate,
        data: { fromDate: fromDate, ToDate: ToDate },
        success: function (data) {
            $("#tableDiv").html('');
            $("#tableDiv").html(data);
            NewsDatePic();
            $("#fromDate").val(fromDate);
            $("#ToDate").val(ToDate);

        }
    });

}

function NewsDatePic()
{
    
    $("#page_content").find("#fromDate").Zebra_DatePicker({
        showButtonPanel: false,
        default_position: 'below',
        format: 'd-m-Y',
        onSelect: function () {
            searchByDateTime();
        }
    });
    $("#page_content").find("#ToDate").Zebra_DatePicker({
        showButtonPanel: false,
        default_position: 'below',
        format: 'd-m-Y',
        onSelect: function () {
            searchByDateTime();
        }
    });    
}
function DataTableDesign() {
    var table = $('#AdminNewsTable').DataTable({
        "sDom": '<"top"i>rt<"bottom"flp><"clear">'
    });
    $('#tableDiv').find('.dataTables_filter').hide();
    $('#tableDiv').find('.dataTables_info').hide();

    $('#AdminNewsTable tfoot tr').appendTo('#AdminNewsTable thead');
    $("#AdminNewsTable thead .SearchType").keyup(function () {        
        table.column(0).search(this.value).draw();
    });
    $("#AdminNewsTable thead .SearchDescription").keyup(function () {
        table.column(0).search(this.value).draw();
    });
    $("#AdminNewsTable thead .SearchName").keyup(function () {
        table.column(1).search(this.value).draw();
    });
    $("#AdminNewsTable thead .SearchCategory").keyup(function () {
        table.column(2).search(this.value).draw();
    });
    $("#AdminNewsTable thead .SearchBusiness").keyup(function () {
        table.column(3).search(this.value).draw();
    });
    $(".SearchDate").Zebra_DatePicker({
        showButtonPanel: false,
        format: 'd-m-Y',
        onSelect: function () {
            var date = $("#AdminNewsTable").find("thead").find('.SearchDate').val();
                   table.column(3).search(date).draw();
        }
    });

    $("body").on('click', '.dp_clear', function () {
        var date = $("#AdminNewsTable").find("thead").find('.SearchDate').val();
        table.column(0).search(date).draw();
    });
}

$("#tableDiv").on('click', '.btn-add-AdminNews', function () {
    $.ajax({
        url: constantAdminNews.addEdit,
        data: { Id: 0 },
        success: function (data) {
            $('#AdminNewsModal').modal('toggle');
            $('#AdminNewsModal').modal('show');
            $("#tableDiv").find('#AdminNewsBody').html('');
            $("#tableDiv").find('#AdminNewsBody').html(data);
            $("#AdminNewsModal").find('.adminNewsTitle').text("Add News");
            $("#tableDiv").find('#btn-submit-AdminNews').html("ADD");
            $('[data-toggle="tooltip"]').tooltip();

            $("#tableDiv").find('#AdminNewsBody').find('div#froala-editor').froalaEditor({
                toolbarButtons: ['bold', 'italic', 'underline', 'strikeThrough', 'subscript', 'superscript', 'fontFamily', 'fontSize', '|', 'specialCharacters', 'color', 'emoticons', 'inlineStyle', 'paragraphStyle', '|', 'paragraphFormat', 'align', 'formatOL', 'formatUL', 'outdent', 'indent', '-', 'quote', 'insertHR', 'insertLink', 'insertImage', 'insertVideo', 'insertFile', 'insertTable', '|', 'undo', 'redo', 'clearFormatting', 'selectAll', 'html', 'applyFormat', 'removeFormat', 'fullscreen', 'print', 'help'],
                //toolbarButtons: ['bold', 'italic', 'underline', 'formatUL'],
                pluginsEnabled: null
            })
        }
    });
});

$("#tableDiv").on('click', '.btn-edit-AdminNews', function () {
    var id = $("#tableDiv").find("#AdminNewsTable tbody").find('.selected').attr("id");
   // var LoginUserId = $("#page_content_inner").find("#LoginUserId").val();
    $.ajax({
        url: constantAdminNews.addEdit,
        data: { Id: id },
        success: function (data) {
            
            if (data == "Error") {
                $(".hrtoolLoader").hide();
                $(".modal-backdrop").hide();
                $(".toast-customer-error").show();
                $(".toast-customer-error").find(".custom").html('');
                $(".toast-customer-error").find(".custom").html("Not Authority To Edit Record.");
                setTimeout(function () { $(".toast-customer-error").hide(); }, 1500);
            }
            else {

                $('#AdminNewsModal').modal('toggle');
                $('#AdminNewsModal').modal('show');
                $("#tableDiv").find('#AdminNewsBody').html('');
                $("#tableDiv").find('#AdminNewsBody').html(data);
                $("#AdminNewsModal").find('.adminNewsTitle').text("Edit News");
                $("#tableDiv").find('#btn-submit-AdminNews').html("Save");
                $('[data-toggle="tooltip"]').tooltip();
                $("#tableDiv").find('#AdminNewsBody').find('div#froala-editor').froalaEditor({
                    toolbarButtons: ['bold', 'italic', 'underline', 'strikeThrough', 'subscript', 'superscript', 'fontFamily', 'fontSize', '|', 'specialCharacters', 'color', 'emoticons', 'inlineStyle', 'paragraphStyle', '|', 'paragraphFormat', 'align', 'formatOL', 'formatUL', 'outdent', 'indent', '-', 'quote', 'insertHR', 'insertLink', 'insertImage', 'insertVideo', 'insertFile', 'insertTable', '|', 'undo', 'redo', 'clearFormatting', 'selectAll', 'html', 'applyFormat', 'removeFormat', 'fullscreen', 'print', 'help'],
                    //toolbarButtons: ['bold', 'italic', 'underline', 'formatUL'],
                    pluginsEnabled: null
                })
                var htmlString = $("#EditNewRecords").val();
                $("#tableDiv").find('#AdminNewsBody').find('div#froala-editor').froalaEditor('html.set', htmlString);
            }
        }
    });
});

$("#tableDiv").on('click', '#btn-submit-AdminNews', function () {

    var id = $("#tableDiv").find('#AdminNewsBody').find('#hiddenId').val();
    var subject = $("#tableDiv").find('#AdminNewsBody').find('#txt_Subject').val().trim();
    var body = $("#tableDiv").find('#AdminNewsBody').find('div#froala-editor').froalaEditor('html.get');
    var employeeAccess = $("#tableDiv").find('#AdminNewsBody').find('#check_EmployeeAccess').prop("checked");
    var managerAccess = $("#tableDiv").find('#AdminNewsBody').find('#check_ManagerAccess').prop("checked");
    var customerAccess = $("#tableDiv").find('#AdminNewsBody').find('#check_CustomerAccess').prop("checked");
    var Spacificmanager = $("#tableDiv").find('#AdminNewsBody').find('#check_SpecificManager').prop("checked");
    var Spacificcustomer = $("#tableDiv").find('#AdminNewsBody').find('#check_SpecificCustomer').prop("checked");
    var SpacificWork = $("#tableDiv").find('#AdminNewsBody').find('#check_SpecificWorker').prop("checked");
    var notifyEmployees = $("#tableDiv").find('#AdminNewsBody').find('#chk_NotifyEmployees').prop("checked");
    var allowCustomer = $("#tableDiv").find('#AdminNewsBody').find('#chk_AllowComments').prop("checked");
    var ManagerID = $("#tableDiv").find('#AdminNewsBody').find('#drpManager').val();
    var CustomerID = $("#tableDiv").find('#AdminNewsBody').find('#drpCustomer').val();
    var WorkerID = $("#tableDiv").find('#AdminNewsBody').find('#drpWorker').val();
    var isError = false;
    var count = $("input[name='grpChk']:checked").size();
    if (subject == "")
    {
        isError = true;
        $("#lbl-error-Subject").show();
        $(".hrtoolLoader").hide();
        $(".modal-backdrop").hide();
    }
    if (body == "")
    {

        isError = true;
        $("#lbl-error-Body").show();
        $(".hrtoolLoader").hide();
        $(".modal-backdrop").hide();
    }
    //if (count == 0) {
    //    if (Spacificcustomer == false || SpacificWork == false || Spacificmanager == false) {
    //        isError = true;
    //        $("#lbl-error-chkbox").show();
    //        $(".hrtoolLoader").hide();
    //        $(".modal-backdrop").hide();
    //    }

    //}
    if (Spacificcustomer == true) {
        
        if (CustomerID == 0) {
            isError = true;
            $("#lbl-error-drpdown").show();
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
        }
    }
    if (SpacificWork == true) {
        if (WorkerID == 0) {
            isError = true;
            $("#lbl-error-drpdown").show();
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
        }
    }
    if (Spacificmanager == true) {
        if (ManagerID == 0) {
            isError = true;
            $("#lbl-error-drpdown").show();
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
        }
    }

    if(!isError)
        {
        var model = {
            Id: id,
            Subject: subject,
            Description: body,
            EmployeeAccess: employeeAccess,
            ManagerAccess: managerAccess,
            CustomerAccess: customerAccess,
            SpecificCustomer: Spacificcustomer,
            SpecificManager: Spacificmanager,
            SpecificWorker: SpacificWork,
            WorkerID: WorkerID,
            ManagerID: ManagerID,
            CustomerID: CustomerID,
            NotifyEmployeeViaEmail: notifyEmployees,
            AllowCustomer: allowCustomer
        }

        $(".hrtoolLoader").show();

        $.ajax({
            url: constantAdminNews.saveData,
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

$('#page_content').on('keyup', 'div#froala-editor', function () {
    $("#tableDiv").find('#AdminNewsBody').find("#lbl-error-Body").hide();
});

$('#tableDiv').on('click', '.dataTr', function () {
    if ($(this).hasClass('dataTr')) {
        $('#AdminNewsTable tbody').find('tr.selected').removeClass('selected');
        $(this).addClass('selected');
        $("#tableDiv").find(".btn-edit-AdminNews").removeAttr('disabled');
        $("#tableDiv").find(".btn-delete-AdminNews").removeAttr('disabled');     
    }
});

$('#tableDiv').on('dblclick', '.dataTr', function (event) {
    
    if ($(this).hasClass('dataTr')) {
        $('#AdminNewsTable tbody').find('tr.selected').removeClass('selected');
        $(this).addClass('selected');
        $(".btn-edit-AdminNews").click();
    }
});
//btn-Employee-Access 
$("#page_content").on('change', '#check_EmployeeAccess', function () {
    if ($(this).is(':checked') == true) {
        //$("#btn-Employee-Access").find("#check_SpecificWorker").prop('checked', false);
        //$("#btn-Manager-Access").find("#check_SpecificManager").prop('checked', false);
        //$("#btn-Customer-Access").find("#check_SpecificCustomer").prop('checked', false);
        if ($("#btn-Employee-Access").find("#SpecificWorker_Id").val() == undefined || $("#btn-Customer-Access").find("#SpecificCustomer_Id").val() == "") {
            $("#btn-Employee-Access").find("#Worker_Display").addClass("hide");
            $('#drpWorker').val("");
        }
        else {
            $("#Worker_Show").addClass("hide");
            $('#drpWorker').val("");
        }
        if ($("#btn-Manager-Access").find("#SpecificManager_Id").val() == undefined || $("#btn-Customer-Access").find("#SpecificCustomer_Id").val() == "") {
            $("#btn-Manager-Access").find("#Manager_Display").addClass("hide");
            $('#drpManager').val("");
            $("#empManagerID").val("");

        }
        else {
            $("#Manager_Show").addClass("hide");
            $('#drpManager').val("");
            $("#empManagerID").val("");

        }
        if ($("#btn-Customer-Access").find("#SpecificCustomer_Id").val() == undefined || $("#btn-Customer-Access").find("#SpecificCustomer_Id").val() == "") {

            $("#btn-Customer-Access").find("#Customer_Display").addClass("hide");
            $('#drpCustomer').val("");
            $("#empCustomerID").val("");

        } else {
            $("#Customer_Show").addClass("hide");
            $('#drpCustomer').val("");
            $("#empCustomerID").val("");
        }

    }
    else {
    }
});
$("#page_content").on('change', '#check_SpecificWorker', function () {
    debugger;
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
        $("#empNameText").val("");
        $("#drpWorker").val("");
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
            $('#drpWorker').val("");
            $("#empNameText").val("");
        }
        else {
            $("#Worker_Show").addClass("hide");
            $('#drpWorker').val("");
            $("#empNameText").val("");
        }
        if ($("#btn-Manager-Access").find("#SpecificManager_Id").val() == undefined || $("#btn-Customer-Access").find("#SpecificCustomer_Id").val() == "") {
            $("#btn-Manager-Access").find("#Manager_Display").addClass("hide");
            $('#drpManager').val("");
            $("#empManagerID").val("");
        }
        else {
            $("#Manager_Show").addClass("hide");
            $('#drpManager').val("");
            $("#empManagerID").val("");
        }
        if ($("#btn-Customer-Access").find("#SpecificCustomer_Id").val() == undefined || $("#btn-Customer-Access").find("#SpecificCustomer_Id").val() == "") {

            $("#btn-Customer-Access").find("#Customer_Display").addClass("hide");
            $('#drpCustomer').val("");
            $("#empCustomerID").val("");

        } else {
            $("#Customer_Show").addClass("hide");
            $('#drpCustomer').val("");
            $("#empCustomerID").val("");
        }
    }
    else {
        // $("#Customer_Display").removeClass("hide");

    }
});
$("#page_content").on('change', '#check_SpecificManager', function () {

    if ($(this).is(':checked') == true) {
        //$("#btn-Employee-Access").find("#check_EmployeeAccess").prop('checked', false);
        //$("#btn-Manager-Access").find("#check_ManagerAccess").prop('checked', false);
        //$("#btn-Customer-Access").find("#check_CustomerAccess").prop('checked', false);
        if ($("#btn-Manager-Access").find("#SpecificManager_Id").val() == undefined || $("#btn-Customer-Access").find("#SpecificCustomer_Id").val() == "") {
            $("#Manager_Display").removeClass("hide");
            $("#empManagerID").show();
            $("#empManagerID").val("");
            $("#drpManager").val("");
        }
        else
        {
            $("#Manager_Show").removeClass("hide");
            $("#empManagerID").val("");
            $("#drpManager").val("");
        }
    }
    else {
        $("#Manager_Display").addClass("hide");
        $("#Manager_Show").addClass("hide");
        $("#empManagerID").val("");
        $("#drpManager").val("");
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
            $('#drpWorker').val("");
            $("#empNameText").val("");
        }
        else {
            $("#Worker_Show").addClass("hide");
            $('#drpWorker').val("");
            $("#empNameText").val("");
        }
        if ($("#btn-Manager-Access").find("#SpecificManager_Id").val() == undefined || $("#btn-Customer-Access").find("#SpecificCustomer_Id").val() == "") {
            $("#btn-Manager-Access").find("#Manager_Display").addClass("hide");
            $('#drpManager').val("");
            $("#empManagerID").val("");
        }
        else {
            $("#Manager_Show").addClass("hide");
            $('#drpManager').val("");
            $("#empManagerID").val("");
        }
        if ($("#btn-Customer-Access").find("#SpecificCustomer_Id").val() == undefined || $("#btn-Customer-Access").find("#SpecificCustomer_Id").val() == "") {

            $("#btn-Customer-Access").find("#Customer_Display").addClass("hide");
            $('#drpCustomer').val("");
            $("#empCustomerID").val("");

        } else {
            $("#Customer_Show").addClass("hide");
            $('#drpCustomer').val("");
            $("#empCustomerID").val("");
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
            $("#Customer_Display").removeClass("hide");
            $("#empCustomerID").show();
            $("#empCustomerID").val("");
            $("#drpCustomer").val("");
        }
        else {
            $("#Customer_Show").removeClass("hide");
            $("#empCustomerID").val("");
            $("#drpCustomer").val("");
        }       
    }   
    else {
        $("#Customer_Show").addClass("hide");
        $("#Customer_Display").addClass("hide");       
    }
});

$('#tableDiv').on('click', '.AddComments', function () {

    var End_ID = $(this).attr("data-id");
    $.ajax({
        url: constantAdminNews.AddCommentsUrl,
        data: { Id: End_ID },
        success: function (data) {

            $("#tableDiv").find('#AddNewsCommentsRecord').html('');
            $("#tableDiv").find('#AddNewsCommentsRecord').html(data);
            $("#tableDiv").find("#AddNewsCommentsModel").find(".commentTitle").text("Add Comments");
            $('[data-toggle="tooltip"]').tooltip();
            $("#tableDiv").find('#AddNewsCommentsRecord').find('div#froala-editor-comment').froalaEditor({
                toolbarButtons: ['bold', 'italic', 'underline', 'strikeThrough', 'subscript', 'superscript', 'fontFamily', 'fontSize', '|', 'specialCharacters', 'color', 'emoticons', 'inlineStyle', 'paragraphStyle', '|', 'paragraphFormat', 'align', 'formatOL', 'formatUL', 'outdent', 'indent', '-', 'quote', 'insertHR', 'insertLink', 'insertImage', 'insertVideo', 'insertFile', 'insertTable', '|', 'undo', 'redo', 'clearFormatting', 'selectAll', 'html', 'applyFormat', 'removeFormat', 'fullscreen', 'print', 'help'],
                //toolbarButtons: ['bold', 'italic', 'underline', 'formatUL'],
                pluginsEnabled: null
            });
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
        }
    });
});

$('#tableDiv').on('click', '#btn-submit-AddComments', function () {
    
    $(".hrtoolLoader").show();
    var Id = $("#tableDiv").find("#CommentId").val();
    var Newsidrecord = $("#tableDiv").find("#NewsId").val();
    var body = $("#tableDiv").find('#AddNewsCommentsRecord').find('div#froala-editor-comment').froalaEditor('html.get');
    var notifyEmployees = $("#tableDiv").find('#AddNewsCommentsRecord').find('#chk_NotifyEmployees').prop("checked");
    CommentId = $("#tableDiv").find('#AddEndrosmentCommentsBody').find("#CommentId").val();
    if (body == "") {
        $("#tableDiv").find('#AddNewsCommentsModel').find("#lbl-error-Comment").show();
        $(".hrtoolLoader").hide();
        $(".modal-backdrop").hide();
    }
    else {
        var model = {
            Id: Id,
            NewsId: Newsidrecord,
            Comments: body,
            NotifyEmployeeViaEmail: notifyEmployees
        }
        $.ajax({
            type: "POST",
            data: JSON.stringify(model),
            url: constantAdminNews.SaveCommentUrl,
            contentType: "application/json",
            success: function (data) {

                $("#tableDiv").html('');
                $("#tableDiv").html(data);
                $('[data-toggle="tooltip"]').tooltip();
                $("#page_content").find('#AssignskillsListRecords').find('div#froala-editor-comment').froalaEditor({
                    toolbarButtons: ['bold', 'italic', 'underline', 'strikeThrough', 'subscript', 'superscript', 'fontFamily', 'fontSize', '|', 'specialCharacters', 'color', 'emoticons', 'inlineStyle', 'paragraphStyle', '|', 'paragraphFormat', 'align', 'formatOL', 'formatUL', 'outdent', 'indent', '-', 'quote', 'insertHR', 'insertLink', 'insertImage', 'insertVideo', 'insertFile', 'insertTable', '|', 'undo', 'redo', 'clearFormatting', 'selectAll', 'html', 'applyFormat', 'removeFormat', 'fullscreen', 'print', 'help'],
                    //toolbarButtons: ['bold', 'italic', 'underline', 'formatUL'],
                    pluginsEnabled: null
                });
                DataTableDesign();
                if (Id > 0) {
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

$('#page_content').on('keyup', 'div#froala-editor-comment', function () {
    $("#tableDiv").find('#AddNewsCommentsModel').find("#lbl-error-Comment").hide();
});


$('#tableDiv').on('click', '.toggles', function () {

    var thisIs = $(this).attr("data-id");
    $("#Comment_" + thisIs).toggle();
    $(this).toggleClass('class1')
})

$("#tableDiv").on('click', '.EditCommentRecords', function () {

    var IdRecord = $(this).attr('data-id');
    $("#CommentId").val(IdRecord)
    $.ajax({
        url: constantAdminNews.EditCommentUrl,
        data: { Id: IdRecord },
        success: function (data) {

            $("#tableDiv").find('#AddNewsCommentsRecord').html('');
            $("#tableDiv").find('#AddNewsCommentsRecord').html(data);
            $("#tableDiv").find("#AddNewsCommentsModel").find(".commentTitle").text("Edit Comments");
            $('[data-toggle="tooltip"]').tooltip();
            $("#tableDiv").find('#AddNewsCommentsRecord').find('div#froala-editor-comment').froalaEditor({
                toolbarButtons: ['bold', 'italic', 'underline', 'strikeThrough', 'subscript', 'superscript', 'fontFamily', 'fontSize', '|', 'specialCharacters', 'color', 'emoticons', 'inlineStyle', 'paragraphStyle', '|', 'paragraphFormat', 'align', 'formatOL', 'formatUL', 'outdent', 'indent', '-', 'quote', 'insertHR', 'insertLink', 'insertImage', 'insertVideo', 'insertFile', 'insertTable', '|', 'undo', 'redo', 'clearFormatting', 'selectAll', 'html', 'applyFormat', 'removeFormat', 'fullscreen', 'print', 'help'],
                //toolbarButtons: ['bold', 'italic', 'underline', 'formatUL'],
                pluginsEnabled: null
            });
            var htmlString = $("#comment_comments_" + IdRecord).html();
            $("#tableDiv").find('#AddNewsCommentsRecord').find('div#froala-editor-comment').froalaEditor('html.set', htmlString);


            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
        }
    });
});

$("#tableDiv").on('click', '.DeleteCommentRecords', function () {
    
    var IdRecord = $(this).attr('data-id');
    var LoginUserId = $("#page_content_inner").find("#LoginUserId").val();
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
                        url: constantAdminNews.DeleteCommentUrl,
                        data: { Id: IdRecord },
                        success: function (data) {
                            if (data == "Error") {
                                $(".hrtoolLoader").hide();
                                $(".modal-backdrop").hide();
                                $(".toast-customer-error").show();
                                $(".toast-customer-error").find(".custom").html('');
                                $(".toast-customer-error").find(".custom").html("Not Authority To Delete Record.");
                                setTimeout(function () { $(".toast-customer-error").hide(); }, 1500);
                            }
                            else {
                                $("#tableDiv").html('');
                                $("#tableDiv").html(data);
                                DataTableDesign();
                                $(".hrtoolLoader").hide();
                                $(".modal-backdrop").hide();
                                $(".toast-error").show();
                                setTimeout(function () { $(".toast-error").hide(); }, 1500);
                            }
                        }
                    });
                }
            }]
    });
});

$("#tableDiv").on('click', '.btn-delete-AdminNews', function () {
    var LoginUserId = $("#page_content_inner").find("#LoginUserId").val();
    var IdRecord = $("#tableDiv").find("#AdminNewsTable tbody").find('.selected').attr("id");
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
                        url: constantAdminNews.DeleteData,
                        data: { Id: IdRecord, LoginUserId: LoginUserId },
                        success: function (data) {
                            if (data == "Error")
                            {
                                $(".hrtoolLoader").hide();
                                $(".modal-backdrop").hide();
                                $(".toast-customer-error").show();
                                $(".toast-customer-error").find(".custom").html('');
                                $(".toast-customer-error").find(".custom").html("Not Authority To Delete Record.");
                                setTimeout(function () { $(".toast-customer-error").hide(); }, 1500);
                            }
                            else {
                                $("#tableDiv").html('');
                                $("#tableDiv").html(data);
                                $(".hrtoolLoader").hide();
                                DataTableDesign();
                                $(".modal-backdrop").hide();
                                $(".toast-error").show();
                                setTimeout(function () { $(".toast-error").hide(); }, 1500);
                            }
                        }
                    });
                }
            }]
    });
});

$("#tableDiv").on('click', '.btn-Refresh-AdminNews', function () {
    window.location.reload();
});
$("#tableDiv").on('click', '.btn-ClearSorting-AdminNews', function () {
    window.location.reload();
});
$("#tableDiv").on('click', '.btn-clearFilter-AdminNews', function () {
    window.location.reload();
})


