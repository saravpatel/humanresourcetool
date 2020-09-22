$(document).ready(function () {
    employeeResourceInfo();
    //DataTableDesign();
    $('#DocumentModalTable_paginate').show();
    $("#DocumentModalTable_next").enabled = true;
    $("#DocumentModalTable_previous").enabled = true;
});
function DataTableDesign() {
   $('#DocumentModalTable tfoot tr').appendTo('#DocumentModalTable thead');
    var table = $('#DocumentModalTable').DataTable({
        "sDom": '<"top"i>rt<"bottom"flp><"clear">',
        "scrollX":true
    });
    $('#tableDiv').find('.dataTables_filter').hide();
    $('#tableDiv').find('.dataTables_info').hide();

    $("#tableDiv").on('keyup', '.SearchName', function () {
        table.column(0).search(this.value).draw();
    });
    $("#tableDiv").on('keyup', '.SearchLocation', function () {
        table.column(1).search(this.value).draw();
    });
    $("#tableDiv").on('keyup', '.SearchBusiness', function () {
        table.column(2).search(this.value).draw();
    });
    $("#tableDiv").on('keyup', '.SearchDivision', function () {
        table.column(3).search(this.value).draw();
    });
    $("#tableDiv").on('keyup', '.SearchPool', function () {
        table.column(4).search(this.value).draw();
    });
    $("#tableDiv").on('keyup', '.SearchFunction', function () {
        table.column(4).search(this.value).draw();
    });   
}
function employeeResourceInfo()
{
    var Employeeid = $('#employeeId').val();
    $.ajax({
        type: "POST",
        data: { EmployeeId: Employeeid },
        url: constantMeResources.getResourceEmployeeInfo,
        success: function (result) {
            $('#serdata').html(result);
            $('#serdata').show();
            DataTableDesign();
        }
    });
}
function openModal()
{
    var Employeeid = $('#employeeId').val();
    $.ajax({
        type: "POST",
        data: { EmployeeId: Employeeid },
        url: constantMeResources.openLeaverWizard,
        success: function (data) {
            $('#MeOverviewLeaverTask').modal('show');
            $("#page_content").find("#MeOverviewLeaverTask").find("#AddMeOverViewBody").html('');
            $("#page_content").find("#MeOverviewLeaverTask").find("#AddMeOverViewBody").html(data);
            $("#MeOverviewLeaverTask").find('#AddMeOverViewBody').find("#finalemploymentDate").Zebra_DatePicker({
                //direction: false,
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {
                    $('#MeOverviewLeaverTask').find('#ValidfinalemploymentDate').hide();
                }
            });
            $("#MeOverviewLeaverTask").find('#AddMeOverViewBody').find("#finalworkingDate").Zebra_DatePicker({
                //direction: false,
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {
                    $('#MeOverviewLeaverTask').find('#ValidfinalworkingDate').hide();

                }
            });
            $("#MeOverviewLeaverTask").find('#AddMeOverViewBody').find("#finalresignDate").Zebra_DatePicker({
                //direction: false,
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {
                    $('#MeOverviewLeaverTask').find('#ValidfinalResignDate').hide();
                }
            });
            $("#MeOverviewLeaverTask").find('#wizard').smartWizard({
                onLeaveStep: leaveAStepCallbackCoustomer,
                //onFinish: onFinishCallbackCoustomer
            });
            $("#MeOverviewLeaverTask").find('#AddMeOverViewBody').find('.buttonNext').addClass('btn btn-warning');
            $("#MeOverviewLeaverTask").find('#AddMeOverViewBody').find('.buttonPrevious').addClass('btn btn-warning');
            $("#MeOverviewLeaverTask").find('#AddMeOverViewBody').find('.buttonFinish').addClass('btn btn-success');
            $("#MeOverviewLeaverTask").find('#AddMeOverViewBody').find('.buttonPrevious').show();
            $("#MeOverviewLeaverTask").find('#AddMeOverViewBody').find('.buttonFinish').show();
        }
    });
}
function leaveAStepCallbackCoustomer(obj, context) {
    if (context.fromStep == 1) {
        var isError = false;
        var reasonId = $('#drpReason').val();
        var finalEmpDate = $('#finalemploymentDate').val();
        var finalWorkDate = $('#finalworkingDate').val();
        var reDate = $('#finalresignDate').val();
        if (reasonId == 0 || finalEmpDate == "" || finalWorkDate == "" || reDate == "")
        {
            if (reasonId == 0) {
                isError = true;
                $('#MeOverviewLeaverTask').find('#ValidReasonForLeaving').show();
            }
            if(finalEmpDate=="")
            {
                isError = true;
                $('#MeOverviewLeaverTask').find('#ValidfinalemploymentDate').show();
            }
            if (finalWorkDate == "") {
                $('#MeOverviewLeaverTask').find('#ValidfinalworkingDate').show();
                isError = true;
            }
            if (reDate == "") {
                $('#MeOverviewLeaverTask').find('#ValidfinalResignDate').show();
                isError = true;
            }
        }
        if (isError) {
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
            return false;
        }
        else {
            if (context.toStep == 2) {
                $("#MeOverviewLeaverTask").find('#AddMeOverViewBody').find('.buttonNext').show();
                $("#MeOverviewLeaverTask").find('#AddMeOverViewBody').find('.buttonPrevious').show();
                $("#MeOverviewLeaverTask").find('#AddMeOverViewBody').find('.buttonFinish').hide();
            }
            else {
                $("#MeOverviewLeaverTask").find('#AddMeOverViewBody').find('.buttonNext').hide();
                $("#MeOverviewLeaverTask").find('#AddMeOverViewBody').find('.buttonPrevious').show();
                $("#MeOverviewLeaverTask").find('#AddMeOverViewBody').find('.buttonFinish').show()
            }
        }
        return true;
     }       
    if (context.fromStep == 2) {
        if (context.toStep = 3) {
        }
        else {
        }      
        return true;
    }
    if (context.fromStep == 3) {      
        return true;
    }
    if (context.fromStep == 4) {
        return true;
    }
}
$("#page_content").on('click', '#btn-submit-AddNewTask', function () {
    var Id = $("#AddTaskbody").find("#taskid").val();
    var iserror = false;
    var isEmpty = $("#AddTaskbody").find("#NewTaskList").val();
    var NewTitle = $("#AddTaskbody").find("#NewTaskTitle").val();
    var NewDiscrtion = $("#AddTaskbody").find("#NewTaskDescription").val();
    var Assignto = $("#AddTaskbody").find("#drpAssign").val();
    var DueDateto = $("#AddTaskbody").find("#DueDate").val();
    var Statusto = $("#AddTaskbody").find("#drpStatus").val();
    var AlertBefore = $("#AddTaskbody").find("#AlertBeforeDays").val();
    if (NewTitle == "") {
        iserror = true;
        $("#ValidNewTaskTitle").show();
        $("#ValidNewTaskTitle").html("Title is required.");
    }
    if (iserror) {
        $(".hrtoolLoader").hide();
        $(".modal-backdrop").hide();
        return false;
    }
    else {
        var model =
            {
                Id: Id,
                Title: NewTitle,
                Description: NewDiscrtion,
                Assign: Assignto,
                DueDate: DueDateto,
                Status: Statusto,
                AlertBeforeDays: AlertBefore,
                IdRecord: Id,
            }
        $.ajax({
            type: "POST",
            data: JSON.stringify(model),
            url: constantMeResources.AddNewTask,
            contentType: "application/json",
            success: function (result) {
                if (result.Id > 0) {
                    $("#MeOverviewLeaverTask").find('#AddMeOverViewBody').find("#step-5").find("#NewTaskList").append('');
                    $("#MeOverviewLeaverTask").find('#AddMeOverViewBody').find("#step-5").find("#NewTaskList").append(result);
                    $("#MeOverviewLeaverTask").find('#AddMeOverViewBody').find("#closeAddNewtask").click();
                    $(".hrtoolLoader").hide();
                    $(".modal-backdrop").hide();
                    if (Id > 0) {
                        $(".toast-info").show();
                        setTimeout(function () { $(".toast-info").hide(); }, 1500);
                    }
                    else {
                        $(".toast-success").show();
                        setTimeout(function () { $(".toast-success").hide(); }, 1500);
                    }
                }
            }
        });
    }
});
$("#page_content").on('click', '#ShowAddNewTask', function () {
    $.ajax({
        url: constantMeResources.ShowTaskData,
        data: {},
        success: function (partialView) {
            $("#page_content").find('#MeOverviewLeaverTask').find("#AddTaskbody").html('');
            $("#page_content").find('#MeOverviewLeaverTask').find("#AddTaskbody").append(partialView);
            $("#page_content").find(".Datepiker").Zebra_DatePicker({
                showButtonPanel: false,
                format: 'd-m-Y',
            });
        }
    });
});
$('#page_content').on('click', '#closeAddNewtask', function () {
    $('#AddNewTask').hide();
});
