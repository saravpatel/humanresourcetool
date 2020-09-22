$(document).ready(function () {
    // getTravelByID();
    TimesheetTotalHours();
    ScheduleTotalHours();
    TravelTotalHours();
    AnualTotalHours();
    OtherTotalDays();
    TrainingTotalHours();
    upliftTotalHours();
    NewvacnyChk();
    SickTotalDays();
    MatPatTotalDays();
    if ($("#MatPatId").find("li").length === 0)
    {
        $("#MatPatId").hide();
    }
    if ($("#UpliftId").find("li").length === 0) {
        $("#UpliftId").hide();
    }
    if ($("#TimeSheetId").find("li").length === 0) {
        $("#TimeSheetId").hide();
    }
    if ($("#ScheduleId").find("li").length === 0) {
        $("#ScheduleId").hide();
    }
    if ($("#TravelId").find("li").length === 0) {
        $("#TravelId").hide();
    }
    if ($("#AnnualId").find("li").length === 0) {
        $("#AnnualId").hide();
    }
    if ($("#TrainingId").find("li").length === 0) {
        $("#TrainingId").hide();
    }
    if ($("#OtherId").find("li").length === 0) {
        $("#OtherId").hide();
    }
    if ($("#NewVacancyId").find("li").length === 0) {
        $("#NewVacancyId").hide();
    }
});
function TimesheetSearchdata() {
    //$('#DocumentModalTable thead tr').appendTo('#DocumentModalTable thead');
    // $('#DocumentModalTable tfoot tr').appendTo('#DocumentModalTable thead');
    var table = $('#DocumentModalTable').DataTable({
        "sDom": '<"top"i>rt<"bottom"flp><"clear">',
    });       
    $("#DocumentModalTable thead .SearchDay").keyup(function () {
        table.column(1).search(this.value).draw();
    });
    $("#DocumentModalTable .SearchDate").Zebra_DatePicker({
        //direction: false,
        showButtonPanel: false,
        format: 'd-m-Y',
        default_position: 'below',
        onSelect: function () {
            var date = $("#DocumentModalTable").find('.SearchDate').val();
            table.column(2).search(date).draw();
        }
    });
    //$("#DocumentModalTable thead .SearchDate").keyup(function () {
    //    table.column(2).search(this.value).draw();
    //});
    $("#DocumentModalTable thead .SearchHours").keyup(function () {
        table.column(3).search(this.value).draw();
    });
    $("#DocumentModalTable thead .SearchCostCode").keyup(function () {
        table.column(4).search(this.value).draw();
    });
    $("#DocumentModalTable thead .SearchProject").keyup(function () {
        table.column(5).search(this.value).draw();
    });
    $("#DocumentModalTable thead .SearchCustomer").keyup(function () {
        table.column(6).search(this.value).draw();
    });
    $("#DocumentModalTable thead .SearchAsset").keyup(function () {
        table.column(7).search(this.value).draw();
    });
    $("#DocumentModalTable thead .SearchStatus").keyup(function () {
        table.column(9).search(this.value).draw();
    });
     $(".dp_clear").on('click', function () {
         var date = $("#DocumentModalTable").find("thead").find('.SearchDate').val();
        table.column(2).search(date).draw();
    });
}
function ScheduleSerchdata() {
    var table = $('#DocumentModalTable1').DataTable({
        "sDom": '<"top"i>rt<"bottom"flp><"clear">'
    });
    $('#tableDiv').find('.dataTables_filter').hide();
    $('#tableDiv').find('.dataTables_info').hide();
    // $('#DocumentModalTable tfoot tr').appendTo('#DocumentModalTable thead');
    //$("#DocumentModalTable1 thead .SearchStartDate").keyup(function () {
    //    table.column(1).search(this.value).draw();
    //});
    $("#DocumentModalTable1 .SearchStartDate").Zebra_DatePicker({
        //direction: false,
        showButtonPanel: false,
        format: 'd-m-Y',
        default_position: 'below',
        onSelect: function () {
            var date = $("#DocumentModalTable1").find('.SearchStartDate').val();
            table.column(1).search(date).draw();
        }
    });
     $("#DocumentModalTable1 .SearchEndDate").Zebra_DatePicker({
        //direction: false,
        showButtonPanel: false,
        format: 'd-m-Y',
        default_position: 'below',
        onSelect: function () {
            var date = $("#DocumentModalTable1").find('.SearchEndDate').val();
            table.column(2).search(date).draw();
        }
    });
    //$("#DocumentModalTable1 thead .SearchEndDate").keyup(function () {
    //    table.column(2).search(this.value).draw();
    //});
    $("#DocumentModalTable1 thead .SearchHours").keyup(function () {
        table.column(4).search(this.value).draw();
    });
    $("#DocumentModalTable1 thead .SearchProject").keyup(function () {
        table.column(5).search(this.value).draw();
    });
    $("#DocumentModalTable1 thead .SearchCustomer").keyup(function () {
        table.column(6).search(this.value).draw();
    });
    $("#DocumentModalTable1 thead .SearchAsset").keyup(function () {
        table.column(7).search(this.value).draw();
    });
    $("#DocumentModalTable1 thead .SearchStatus").keyup(function () {
        table.column(9).search(this.value).draw();
    });
    $(".dp_clear").on('click', function () {
        var date = $("#DocumentModalTable1").find("thead").find('.SearchStartDate').val();
        table.column(1).search(date).draw();    
        var date1 = $("#DocumentModalTable1").find("thead").find('.SearchEndDate').val();
        table.column(2).search(date1).draw();
    });
}
function AnualSearchData() {
    var table = $('#DocumentModalTable3').DataTable({
        "sDom": '<"top"i>rt<"bottom"flp><"clear">'
    });   
    $('#tableDiv').find('.dataTables_filter').hide();
    $('#tableDiv').find('.dataTables_info').hide();
    //$("#DocumentModalTable3 thead .SearchStartDate").keyup(function () {
    //    table.column(1).search(this.value).draw();
    //});
    $("#DocumentModalTable3 .SearchStartDate").Zebra_DatePicker({
        //direction: false,
        showButtonPanel: false,
        format: 'd-m-Y',
        default_position: 'below',
        onSelect: function () {
            var date = $("#DocumentModalTable3").find('.SearchStartDate').val();
            table.column(1).search(date).draw();
        }
    });
    $("#DocumentModalTable3 .SearchEndDate").Zebra_DatePicker({
        //direction: false,
        showButtonPanel: false,
        format: 'd-m-Y',
        default_position: 'below',
        onSelect: function () {
            var date = $("#DocumentModalTable3").find('.SearchEndDate').val();
            table.column(2).search(date).draw();
        }
    });
    //$("#DocumentModalTable3 thead .SearchEndDate").keyup(function () {
    //    table.column(2).search(this.value).draw();
    //});
    $("#DocumentModalTable3 thead .SearchDays").keyup(function () {
        table.column(3).search(this.value).draw();
    });
    $("#DocumentModalTable3 thead .SearchStatus").keyup(function () {
        table.column(4).search(this.value).draw();
    });
    $(".dp_clear").on('click', function () {
        var date = $("#DocumentModalTable3").find("thead").find('.SearchStartDate').val();
        table.column(1).search(date).draw();
        var date1 = $("#DocumentModalTable3").find("thead").find('.SearchEndDate').val();
        table.column(2).search(date1).draw();
    });
}
function TravelSearchData() {
    var table = $('#DocumentModalTable2').DataTable({
        "sDom": '<"top"i>rt<"bottom"flp><"clear">'
    });

    $('#tableDiv').find('.dataTables_filter').hide();
    $('#tableDiv').find('.dataTables_info').hide();

    // $('#DocumentModalTable tfoot tr').appendTo('#DocumentModalTable thead');

    $("#DocumentModalTable2 thead .SearchType").keyup(function () {
        table.column(1).search(this.value).draw();
    });
    $("#DocumentModalTable2 thead .SearchFromCountry").keyup(function () {
        table.column(2).search(this.value).draw();
    });
    $("#DocumentModalTable2 thead .SearchFromTown").keyup(function () {
        table.column(3).search(this.value).draw();
    });
    $("#DocumentModalTable2 thead .SearchFromPlace").keyup(function () {
        table.column(5).search(this.value).draw();
    });
    $("#DocumentModalTable2 thead .SearchToCountry").keyup(function () {
        table.column(6).search(this.value).draw();
    });
    $("#DocumentModalTable2 thead .SearchToTown").keyup(function () {
        table.column(7).search(this.value).draw();
    });
    $("#DocumentModalTable2 thead .SearchToPlace").keyup(function () {
        table.column(8).search(this.value).draw();
    });
    $("#DocumentModalTable2 .SearchStartDate").Zebra_DatePicker({
        //direction: false,
        showButtonPanel: false,
        format: 'd-m-Y',
        default_position: 'below',
        onSelect: function () {
            var date = $("#DocumentModalTable2").find('.SearchStartDate').val();
            table.column(9).search(date).draw();
        }
    });
    $("#DocumentModalTable2 .SearchEndDate").Zebra_DatePicker({
        //direction: false,
        showButtonPanel: false,
        format: 'd-m-Y',
        default_position: 'below',
        onSelect: function () {
            var date = $("#DocumentModalTable2").find('.SearchEndDate').val();
            table.column(10).search(date).draw();
        }
    });
    //$("#DocumentModalTable2 thead .SearchStartDate").keyup(function () {
    //    table.column(9).search(this.value).draw();
    //});
    //$("#DocumentModalTable2 thead .SearchEndDate").keyup(function () {
    //    table.column(10).search(this.value).draw();
    //});
    $("#DocumentModalTable2 thead .SearchProject").keyup(function () {
        table.column(13).search(this.value).draw();
    });
    $("#DocumentModalTable2 thead .SearchCustomer").keyup(function () {
        table.column(14).search(this.value).draw();
    }); 
    $("#DocumentModalTable2 thead .SearchCostCode").keyup(function () {
        table.column(14).search(this.value).draw();
    });
    $("#DocumentModalTable2 thead .SearchStatus").keyup(function () {
             table.column(18).search(this.value).draw();
    });
    $(".dp_clear").on('click', function () {
        var date = $("#DocumentModalTable2").find("thead").find('.SearchStartDate').val();
        table.column(9).search(date).draw();
        var date1 = $("#DocumentModalTable2").find("thead").find('.SearchEndDate').val();
        table.column(10).search(date1).draw();
    });
}
function OtherSearchData() {
    var table = $('#DocumentModalTable12').DataTable({
        "sDom": '<"top"i>rt<"bottom"flp><"clear">'
    });
    $('#tableDiv').find('.dataTables_filter').hide();
    $('#tableDiv').find('.dataTables_info').hide();
    //$('#DocumentModalTable12 tfoot tr').appendTo('#DocumentModalTable thead');
    //$("#DocumentModalTable12 thead .SearchStartDate").keyup(function () {
    //    table.column(1).search(this.value).draw();
    //});
    $("#DocumentModalTable12 .SearchStartDate").Zebra_DatePicker({
        //direction: false,
        showButtonPanel: false,
        format: 'd-m-Y',
        default_position: 'below',
        onSelect: function () {
            var date = $("#DocumentModalTable12").find('.SearchStartDate').val();
            table.column(1).search(date).draw();
        }
    });
    $("#DocumentModalTable12 .SearchEndDate").Zebra_DatePicker({
        //direction: false,
        showButtonPanel: false,
        format: 'd-m-Y',
        default_position: 'below',
        onSelect: function () {
            var date = $("#DocumentModalTable12").find('.SearchEndDate').val();
            table.column(2).search(date).draw();
        }
    });
    //$("#DocumentModalTable12 thead .SearchEndDate").keyup(function () {
    //    table.column(2).search(this.value).draw();
    //});
    $("#DocumentModalTable12 thead .SearchDuration").keyup(function () {
        table.column(3).search(this.value).draw();
    });
    $("#DocumentModalTable12 thead .SearchReason").keyup(function () {
        table.column(4).search(this.value).draw();
    });
    $("#DocumentModalTable12 thead .SearchStatus").keyup(function () {
        table.column(4).search(this.value).draw();
    });
     $(".dp_clear").on('click', function () {
        var date = $("#DocumentModalTable12").find("thead").find('.SearchStartDate').val();
        table.column(1).search(date).draw();
        var date1 = $("#DocumentModalTable12").find("thead").find('.SearchEndDate').val();
        table.column(2).search(date1).draw();
    });
}
function TrainingSearchData() {
    var table = $('#DocumentModalTable13').DataTable({
        "sDom": '<"top"i>rt<"bottom"flp><"clear">'
    });

   $("#DocumentModalTable13 thead .SearchTrainingName").keyup(function () {
        debugger;
        table.column(1).search(this.value).draw();
    });
    $("#DocumentModalTable13 thead .SearchImportance").keyup(function () {
        table.column(2).search(this.value).draw();
    });
    //$("#DocumentModalTable13 thead .SearchStartDate").keyup(function () {
    //    table.column(3).search(this.value).draw();
    //});
    //$("#DocumentModalTable13 thead .SearchEndDate").keyup(function () {
    //    table.column(4).search(this.value).draw();
    //});
    $("#DocumentModalTable13 thead .SearchStatus").keyup(function () {
        table.column(9).search(this.value).draw();
    });
    $("#DocumentModalTable13 .SearchStartDate").Zebra_DatePicker({
        //direction: false,
        showButtonPanel: false,
        format: 'd-m-Y',
        default_position: 'below',
        onSelect: function () {
            var date = $("#DocumentModalTable13").find('.SearchStartDate').val();
            table.column(3).search(date).draw();
        }
    });
    $("#DocumentModalTable13 .SearchEndDate").Zebra_DatePicker({
        //direction: false,
        showButtonPanel: false,
        format: 'd-m-Y',
        default_position: 'below',
        onSelect: function () {
            var date = $("#DocumentModalTable13").find('.SearchEndDate').val();
            table.column(4).search(date).draw();
        }
    });
    $(".dp_clear").on('click', function () {
        var date = $("#DocumentModalTable13").find("thead").find('.SearchStartDate').val();
        table.column(3).search(date).draw();
        var date1 = $("#DocumentModalTable13").find("thead").find('.SearchEndDate').val();
        table.column(4).search(date1).draw();
    });
}
function NewVacancySearchData() {
    var table = $('#DocumentModalTable14').DataTable({
        "sDom": '<"top"i>rt<"bottom"flp><"clear">'
    });
    $('#tableDiv').find('.dataTables_filter').hide();
    $('#tableDiv').find('.dataTables_info').hide();

    //$('#DocumentModalTable tfoot tr').appendTo('#DocumentModalTable thead');

    $("#DocumentModalTable14 thead .SearchVacancy").keyup(function () {
        table.column(1).search(this.value).draw();
    });
    $("#DocumentModalTable14 thead .SearchClosingDate").keyup(function () {
        table.column(2).search(this.value).draw();
    });
    $("#DocumentModalTable14 thead .SearchRecruitmentProcess").keyup(function () {
        table.column(3).search(this.value).draw();
    });
    $("#DocumentModalTable14 thead .SearchStatus").keyup(function () {
        table.column(11).search(this.value).draw();
    });
    //$("#DocumentModalTable thead .SearchCreated").Zebra_DatePicker({
    //    //direction: false,
    //    showButtonPanel: false,
    //    format: 'd-m-Y',
    //    onSelect: function () {
    //        var date = $("#DocumentModalTable").find("thead").find('.SearchCreated').val();
    //        table.column(8).search(date).draw();
    //    }
    //});

    //$("body").on('click', '.dp_clear', function () {
    //    var date = $("#DocumentModalTable").find("thead").find('.SearchCreated').val();
    //    table.column(8).search(date).draw();
    //});
}
function UpliftSearchData() {
    var table = $('#DocumentModalTable11').DataTable({
        "sDom": '<"top"i>rt<"bottom"flp><"clear">'
    });

    $('#tableDiv').find('.dataTables_filter').hide();
    $('#tableDiv').find('.dataTables_info').hide();

    // $('#DocumentModalTable tfoot tr').appendTo('#DocumentModalTable thead');

    $("#DocumentModalTable11 thead .SearchDay").keyup(function () {
        table.column(1).search(this.value).draw();
    });
    //$("#DocumentModalTable11 thead .SearchDate").keyup(function () {
    //    table.column(2).search(this.value).draw();
    //});
    $("#DocumentModalTable11 .SearchDate").Zebra_DatePicker({
        //direction: false,
        showButtonPanel: false,
        format: 'd-m-Y',
        default_position: 'below',
        onSelect: function () {
            var date = $("#DocumentModalTable11").find('.SearchDate').val();
            table.column(2).search(date).draw();
        }
    });
    $("#DocumentModalTable11 thead .SearchUpliftPosition").keyup(function () {
        table.column(3).search(this.value).draw();
    });
    $("#DocumentModalTable11 thead .SearchHours").keyup(function () {
        table.column(4).search(this.value).draw();
    });
    $("#DocumentModalTable11 thead .SearchProject").keyup(function () {
        table.column(5).search(this.value).draw();
    });
    $("#DocumentModalTable11 thead .SearchCustomer").keyup(function () {
        table.column(6).search(this.value).draw();
    });
    $("#DocumentModalTable11 thead .SearchWorkerRate").keyup(function () {
        table.column(7).search(this.value).draw();
    }); 
    $("#DocumentModalTable11 thead .SearchStatus").keyup(function () {
        table.column(10).search(this.value).draw();
    });
    $(".dp_clear").on('click', function () {
        var date = $("#DocumentModalTable11").find("thead").find('.SearchDate').val();
        table.column(2).search(date).draw();
    });
}
function SickLeaveSearchData()
{
    var table = $('#DocumentModalTable12').DataTable({
        "sDom": '<"top"i>rt<"bottom"flp><"clear">'
    });

    $('#tableDiv').find('.dataTables_filter').hide();
    $('#tableDiv').find('.dataTables_info').hide();

    // $('#DocumentModalTable tfoot tr').appendTo('#DocumentModalTable thead');

    //$("#DocumentModalTable12 thead .SearchStartDate").keyup(function () {
    //    table.column(1).search(this.value).draw();
    //});
    $("#DocumentModalTable12 .SearchStartDate").Zebra_DatePicker({
        //direction: false,
        showButtonPanel: false,
        format: 'd-m-Y',
        default_position: 'below',
        onSelect: function () {
            var date = $("#DocumentModalTable12").find('.SearchStartDate').val();
            table.column(1).search(date).draw();
        }
    });
    //$("#DocumentModalTable12 thead .SearchEndDate").keyup(function () {
    //    table.column(2).search(this.value).draw();
    //});
    $("#DocumentModalTable12 .SearchEndDate").Zebra_DatePicker({
        //direction: false,
        showButtonPanel: false,
        format: 'd-m-Y',
        default_position: 'below',
        onSelect: function () {
            var date = $("#DocumentModalTable12").find('.SearchEndDate').val();
            table.column(2).search(date).draw();
        }
    });
    $("#DocumentModalTable12 thead .SearchDuration").keyup(function () {
        table.column(3).search(this.value).draw();
    });
    $("#DocumentModalTable12 thead .SearchSelfCertiRequired").keyup(function () {
        table.column(4).search(this.value).draw();
    });
    $("#DocumentModalTable12 thead .SearchInterviewRequired").keyup(function () {
        table.column(5).search(this.value).draw();
    });
    $("#DocumentModalTable12 thead .SearchType").keyup(function () {
        table.column(6).search(this.value).draw();
    });
    $("#DocumentModalTable12 thead .SearchDoctConsulted").keyup(function () {
        table.column(7).search(this.value).draw();
    });
    $("#DocumentModalTable12 thead .SearchIssueAtWork").keyup(function () {
        table.column(8).search(this.value).draw();
    });
    $("#DocumentModalTable12 thead .SearchStatus").keyup(function () {
        table.column(10).search(this.value).draw();
    });
    $(".dp_clear").on('click', function () {
        var date = $("#DocumentModalTable12").find("thead").find('.SearchStartDate').val();
        table.column(1).search(date).draw();
        var date1 = $("#DocumentModalTable12").find("thead").find('.SearchEndDate').val();
        table.column(2).search(date1).draw();
    });
}
function MatPatLeaveSearchData()
{
    var table = $('#DocumentModalTable12').DataTable({
        "sDom": '<"top"i>rt<"bottom"flp><"clear">'
    });

    $('#tableDiv').find('.dataTables_filter').hide();
    $('#tableDiv').find('.dataTables_info').hide();

    // $('#DocumentModalTable tfoot tr').appendTo('#DocumentModalTable thead');
    $("#DocumentModalTable12 .SearchdueDate").Zebra_DatePicker({
        //direction: false,
        showButtonPanel: false,
        format: 'd-m-Y',
        default_position: 'below',
        onSelect: function () {
            var date = $("#DocumentModalTable12").find('.SearchdueDate').val();
            table.column(1).search(date).draw();
        }
    });

    //$("#DocumentModalTable12 thead .SearchdueDate").keyup(function () {
    //    table.column(1).search(this.value).draw();
    //});
    $("#DocumentModalTable12 thead .SearchLink").keyup(function () {
        table.column(2).search(this.value).draw();
    });
    $("#DocumentModalTable12 thead .SearchStatus").keyup(function () {
        table.column(4).search(this.value).draw();
    });
     $(".dp_clear").on('click', function () {
         var date = $("#DocumentModalTable12").find("thead").find('.SearchdueDate').val();
        table.column(1).search(date).draw();     
    });
}
function getTravelByID(sParam) {   
    var url = window.location.href;
    var vars = {};
    var hashes = url.split("?")[1];
    var hash = hashes.split('&');
    for (var i = 0; i < hash.length; i++) {
        params = hash[i].split("=");
        vars[params[0]] = params[1];
    }
    var namesplit = hash[0].split("=")[1];
    var name = namesplit;
    var hidden_fields = params[1];
    if (name == "tavelId")       
    {
        $('#All').hide();
        $("#AllList").removeClass("active");
        $("#Travel").addClass("active");
        $.ajax({
            type: 'POST',
            url: constantApproval.TravelApprove,
            data: {
                EmpID: hidden_fields
            },
            success: function (result) {
                $('#Travellist').html();
                $('#Travellist').html(result);
                TravelSearchData();
               
            }
        })
    }
    else if(name=="anualId")
    {
        $('#All').hide();
        $("#AllList").removeClass("active");
        $("#Annual").addClass("active");
        $.ajax({
            type: 'POST',
            url: constantApproval.AnualLeaveApprove,
            data: {
                EmpID: hidden_fields
            },
            success: function (result) {              
                $('#Anualleavelist').html(result);
                $('#Anualleavelist').show();
                $('#All').hide();
                AnualSearchData();
            }
        });
    }
    else if(name=="sickId")
    {
        $('#All').hide();
        $("#AllList").removeClass("active");
        $("#Sick").addClass("active");
        $.ajax({
            type: 'POST',
            url: constantApproval.sickApprove,
            data: {
                EmpID: hidden_fields
            },
            success: function (result) {
                $('#sickApproval').html(result);
                $('#sickApproval').show();
                SickLeaveSearchData();
                $('#All').hide();
            }
        });
    }
    else if(name=="otherleaveId")
    {
        $('#All').hide();
        $("#AllList").removeClass("active");
        $("#Other").addClass("active");
        $.ajax({
            type: 'POST',
            url: constantApproval.OtherLeaveApprove,
            data: {
                EmpID: hidden_fields
            },
            success: function (result) {
                $('#otherLeaveApproval').html(result);
                $('#otherLeaveApproval').show();
                OtherSearchData();
                $('#All').hide();
            }
        });
    }
    else if(name=="matpatleaveId")
    {
        $('#All').hide();
        $("#AllList").removeClass("active");
        $("#MatPat").addClass("active");
        $.ajax({
            type: 'POST',
            url: constantApproval.MatPatApprove,
            data: {
                EmpID: hidden_fields
            },
            success: function (result) {
                $('#matpatApproval').html(result);
                $('#matpatApproval').show();
                MatPatLeaveSearchData();
            }
        });
    }
    else if(name=="scheduleId")
    {
        $("#AllList").removeClass("active");
        $("#Scheduling").addClass("active");
        $('#All').hide();
        $.ajax({
            type: 'POST',
            url: constantApproval.ScheduleApproval,
            data: {
                EmpID: hidden_fields
            },
            success: function (result) {
                $('#Schedulelist').html(result);
                $('#Schedulelist').show();
                ScheduleSerchdata();
            }
        });
    }
    else if(name=="timesheetId")
    {
        $("#AllList").removeClass("active");
        $("#TimeSheet").addClass("active");
        $('#All').hide();
        $.ajax({
            type: 'POST',
            url: constantApproval.TimesheetApprove,
            data: {
                EmpID: hidden_fields
            },
            success: function (result) {
                $('#timesheetlist').html(result);
                $('#timesheetlist').show();
                TimesheetSearchdata();             
            }
        });
    }
    else if(name=="upliftId")
    {
        $("#AllList").removeClass("active");
        $("#Uplift").addClass("active");
        $('#All').hide();
        $.ajax({
            type: 'POST',
            url: constantApproval.UpliftApprove,
            data: {
                EmpID: hidden_fields
            },
            success: function (result) {
                $('#upliftApproval').html(result);
                $('#upliftApproval').show();
                UpliftSearchData();                
            }
        });
    }
}

$("#ProjectPlannerTimeSheetId li").on('click', function () {
    $("#AllList").removeClass("active");
    $("#TimeSheet").addClass("active");
    var hidden_fields = $(this).find('input:hidden').val();
    $.ajax({
        type: 'POST',
        url: constantApproval.TimesheetApprove,
        data: {
            EmpID: hidden_fields
        },
        success: function (result) {
            $('#All').hide();
            $('#timesheetlist').html(result);
            $('#timesheetlist').show();
            TimesheetSearchdata();

        }
    });
});
$("#TimeSheetId li").on('click', function () {
    $("#AllList").removeClass("active");
    $("#TimeSheet").addClass("active");
    var hidden_fields = $(this).find('input:hidden').val();
    $.ajax({
        type: 'POST',
        url: constantApproval.TimesheetApprove,
        data: {
            EmpID: hidden_fields
        },
        success: function (result) {
            $('#All').hide();
            $('#timesheetlist').html(result);
            $('#timesheetlist').show();
            TimesheetSearchdata();

        }
    });
});
$("#TravelId li").on('click', function () {
    $("#AllList").removeClass("active");
    $("#Travel").addClass("active");
    var hidden_fields = $(this).find('input:hidden').val();
    $.ajax({
        type: 'POST',
        url: constantApproval.TravelApprove,
        data: {
            EmpID: hidden_fields
        },
        success: function (result) {
            $('#All').hide();
            $('#Travellist').html(result);
            $('#Travellist').show();
            TravelSearchData();

        }
    });
});
$("#AnnualId li").on('click', function () {
    $("#AllList").removeClass("active");
    $("#Annual").addClass("active");
    var hidden_fields = $(this).find('input:hidden').val();
    $.ajax({
        type: 'POST',
        url: constantApproval.AnualLeaveApprove,
        data: {
            EmpID: hidden_fields
        },
        success: function (result) {
            $('#All').hide();
            $('#Anualleavelist').html(result);
            $('#Anualleavelist').show();
            AnualSearchData();
        }
    });
});
$("#OtherId li").on('click', function () {
    $("#AllList").removeClass("active");
    $("#Other").addClass("active");
    var hidden_fields = $(this).find('input:hidden').val();
    $.ajax({
        type: 'POST',
        url: constantApproval.OtherLeaveApprove,
        data: {
            EmpID: hidden_fields
        },
        success: function (result) {
            $('#All').hide();
            $('#otherLeaveApproval').html(result);
            $('#otherLeaveApproval').show();
            OtherSearchData();
        }
    });
});
$("#UpliftId li").on('click', function () {
    $("#AllList").removeClass("active");
    $("#Uplift").addClass("active");
    var hidden_fields = $(this).find('input:hidden').val();
    $.ajax({
        type: 'POST',
        url: constantApproval.UpliftApprove,
        data: {
            EmpID: hidden_fields
        },
        success: function (result) {
            $('#All').hide();
            $('#upliftApproval').html(result);
            $('#upliftApproval').show();
            UpliftSearchData();

        }
    });
});




$("#MatPatId li").on('click', function () {
    $("#AllList").removeClass("active");
    $("#MatPat").addClass("active");
    var hidden_fields = $(this).find('input:hidden').val();
    $.ajax({
        type: 'POST',
        url: constantApproval.MatPatApprove,
        data: {
            EmpID: hidden_fields
        },
        success: function (result) {
            $('#All').hide();
            $('#matpatApproval').html(result);
            $('#matpatApproval').show();
            MatPatLeaveSearchData();
        }
    });
});


$("#SickLeaveId li").on('click', function () {
    $("#AllList").removeClass("active");
    $("#Sick").addClass("active");
    var hidden_fields = $(this).find('input:hidden').val();
    $.ajax({
        type: 'POST',
        url: constantApproval.sickApprove,
        data: {
            EmpID: hidden_fields
        },
        success: function (result) {
            $('#All').hide();
            $('#sickApproval').html(result);
            $('#sickApproval').show();
            SickLeaveSearchData();
        }
    });
});



$("#NewVacancyId li").on('click', function () {
    $("#AllList").removeClass("active");
    $("#Vacancy").addClass("active");
    var hidden_fields = $(this).find('input:hidden').val();
    $.ajax({
        type: 'POST',
        url: constantApproval.NewVacancyApprove,
        data: {
            EmpID: hidden_fields
        },
        success: function (result) {
            $('#All').hide();
            $('#newVacancyApproval').html(result);
            $('#newVacancyApproval').show();
            NewVacancySearchData();
        }
    });
});
$("#TrainingId li").on('click', function () {
    $("#AllList").removeClass("active");
    $("#Training").addClass("active");
    var hidden_fields = $(this).find('input:hidden').val();
    $.ajax({
        type: 'POST',
        url: constantApproval.TrainingRequestApprove,
        data: {
            EmpID: hidden_fields
        },
        success: function (result) {
            $('#All').hide();
            $('#trainingReqApproval').html(result);
            $('#trainingReqApproval').show();
            TrainingSearchData();
        }
    });
});
$("#ScheduleId li").on('click', function () {
    $("#AllList").removeClass("active");
    $("#Scheduling").addClass("active");
    var hidden_fields = $(this).find('input:hidden').val();
    $.ajax({
        type: 'POST',
        url: constantApproval.ScheduleApproval,
        data: {
            EmpID: hidden_fields
        },
        success: function (result) {
            $('#All').hide();
            $('#Schedulelist').html(result);
            $('#Schedulelist').show();
            ScheduleSerchdata();
        }
    });
});
$("#AllList").click(function () {
    location.reload();
    $(this).parent().addClass("Active");
    var tab = $(this).attr("href");
    $(tab).fadeIn();
});


function TimesheetTotalHours() {
    Number.prototype.padDigit = function () {
        return (this < 10) ? '0' + this : this;
    }
    $(document).on("click", "#approveCheckBox", function () {             //Calculate Total hours and days

        if ($('.cbCheck1').is(':checked')) {
            $('.cbCheck1').attr('checked', false);
            $("#totalhour").val('00:00');
            $('#totalday').val(0);
        }
        $('#Timeshetapprvebtn').show();
        $('#Timeshetrejectbtn').hide();
        var checked = $(this).is(":checked");
        if (checked) {
            $('#Timeshetapprvebtn').show();
            var initialhour = $('#totalhour').val();
            var getRow = $(this).parents('tr');
            var hour = (getRow.find('td:eq(3)').html());
            var InTime1 = initialhour.split(":");
            var t1 = initialhour;
            var mins = 0;
            var hrs = 0;
            t1 = t1.split(':');
            var t2 = hour.split(':');
            // console.log(Number(t1[1]) + Number(t2[1]))
            mins = Number(t1[1]) + Number(t2[1]);
            var minhrs = Math.floor(parseInt(mins / 60));
            hrs = Number(t1[0]) + Number(t2[0]) + minhrs;
            mins = mins % 60;
            t1 = hrs.padDigit() + ':' + mins.padDigit();
            // console.log(t1)

            $("#totalhour").val(t1);

            var initialdays = parseInt($('#totalday').val());
            initialdays = initialdays + 1;

            $('#totalday').val(initialdays);
        }
        else {
            $('#Timeshetapprvebtn').hide();

            var initialhour = $('#totalhour').val();
            var getRow = $(this).parents('tr');
            var hour = (getRow.find('td:eq(3)').html());
            var InTime1 = initialhour.split(":");
            var t1 = initialhour;
            var mins = 0;
            var hrs = 0;
            t1 = t1.split(':');
            var convminute = parseInt(t1[0]) * 60 + parseInt(t1[1]);

            var t2 = hour.split(':');

            var miutestoremove = parseInt(t2[0]) * 60 + parseInt(t2[1]);

            var result = convminute - miutestoremove;

            t1 = parseInt((result / 60)).padDigit() + ':' + parseInt((result % 60)).padDigit();
            //var t2 = hour.split(':');
            ////console.log(Number(t1[1]) + Number(t2[1]))
            //mins = Number(t1[1]) - Number(t2[1]);
            //var minhrs = Math.floor(parseInt(mins / 60));
            //hrs = Number(t1[0]) - Number(t2[0]) + minhrs;
            //mins = mins % 60;
            //t1 = hrs.padDigit() + ':' + mins.padDigit();
            //console.log(t1)

            $("#totalhour").val(t1);
            var initialdays = parseInt($('#totalday').val());
            initialdays = initialdays - 1;

            $('#totalday').val(initialdays);
        }
        //var inlhour = $('#totalhour').val();
        //var totday=$("#totalday").val();
        //if(inlhour=="00:00" || totday=="0")
        //{
        //    $('#Timeshetapprvebtn').hide();
        //}
    });

    $(document).on("click", "#RejectCheckBox", function () {             //Calculate Total hours and days
        if ($('.cbCheck').is(':checked')) {
            $('.cbCheck').attr('checked', false);
            $("#totalhour").val('00:00');
            $('#totalday').val(0);

        }
        $('#Timeshetrejectbtn').show();
        $('#Timeshetapprvebtn').hide();
        var checked = $(this).is(":checked");
        if (checked) {
            $('#Timeshetrejectbtn').show();
            var initialhour = $('#totalhour').val();

            var getRow = $(this).parents('tr');
            var hour = (getRow.find('td:eq(3)').html());
            var InTime1 = initialhour.split(":");
            var t1 = initialhour;
            var mins = 0;
            var hrs = 0;
            t1 = t1.split(':');
            var t2 = hour.split(':');
            //console.log(Number(t1[1]) + Number(t2[1]))
            mins = Number(t1[1]) + Number(t2[1]);
            var minhrs = Math.floor(parseInt(mins / 60));
            hrs = Number(t1[0]) + Number(t2[0]) + minhrs;
            mins = mins % 60;
            t1 = hrs.padDigit() + ':' + mins.padDigit();
            //console.log(t1)

            $("#totalhour").val(t1);
            var initialdays = parseInt($('#totalday').val());
            initialdays = initialdays + 1;

            $('#totalday').val(initialdays);

        }
        else {
            $('#Timeshetrejectbtn').hide();
            var initialhour = $('#totalhour').val();
            var getRow = $(this).parents('tr');
            var hour = (getRow.find('td:eq(3)').html());
            var InTime1 = initialhour.split(":");
            var t1 = initialhour;
            var mins = 0;
            var hrs = 0;
            t1 = t1.split(':');
            var convminute = parseInt(t1[0]) * 60 + parseInt(t1[1]);

            var t2 = hour.split(':');

            var miutestoremove = parseInt(t2[0]) * 60 + parseInt(t2[1]);

            var result = convminute - miutestoremove;

            t1 = parseInt((result / 60)).padDigit() + ':' + parseInt((result % 60)).padDigit();
            //var t2 = hour.split(':');
            ////console.log(Number(t1[1]) + Number(t2[1]))
            //mins = Number(t1[1]) - Number(t2[1]);
            //var minhrs = Math.floor(parseInt(mins / 60));
            //hrs = Number(t1[0]) - Number(t2[0]) + minhrs;
            //mins = mins % 60;
            //t1 = hrs.padDigit() + ':' + mins.padDigit();
            //console.log(t1)

            $("#totalhour").val(t1);
            var initialdays = parseInt($('#totalday').val());
            initialdays = initialdays - 1;

            $('#totalday').val(initialdays);
        }
        //var inlhour = $('#totalhour').val();
        //var totday = $("#totalday").val();
        //if (inlhour == "00:00" || totday == "0") {
        //    $('#Timeshetapprvebtn').hide();
        //    $('#Timeshetrejectbtn').hide();
        //}
    });
}

function ScheduleTotalHours() {
    Number.prototype.padDigit = function () {
        return (this < 10) ? '0' + this : this;
    }
    $(document).on("click", ".cbCheck2", function () {
        //Calculate Total hours and days
        if ($('.cbCheck3').is(':checked')) {
            $('.cbCheck3').attr('checked', false);
            $("#TotalScheduleHour").val('00:00');
          // $('#TotalScheduleDays').val(0);
        }
        $('#ScheduleApprove').show();
        $('#ScheduleReject').hide();
        var checked = $(this).is(":checked");
        if (checked) {
            $('#ScheduleApprove').show();
            var initialhour = $('#TotalScheduleHour').val();
            //alert(initialhour);
            var getRow = $(this).parents('tr');
            var hour = (getRow.find('td:eq(4)').html());
            var InTime1 = initialhour.split(":");
            var t1 = initialhour;
            var mins = 0;
            var hrs = 0;
            t1 = t1.split(':');
            var t2 = hour.split(':');
            console.log(Number(t1[1]) + Number(t2[1]))
            mins = Number(t1[1]) + Number(t2[1]);
            var minhrs = Math.floor(parseInt(mins / 60));
            hrs = Number(t1[0]) + Number(t2[0]) + minhrs;
            mins = mins % 60;
            t1 = hrs.padDigit() + ':' + mins.padDigit();
            console.log(t1)

            $("#TotalScheduleHour").val(t1);
            //var initialdays = parseInt($('#TotalScheduleDays').val());
            //var getRow1 = $(this).parents('tr');
            //var Day = (getRow1.find('td:eq(3)').html());
            //var totalday = parseInt(initialdays) + parseInt(Day);

          //  $('#TotalScheduleDays').val(totalday);
        }
        else {
            $('#ScheduleApprove').hide();
            var initialhour = $('#TotalScheduleHour').val();
            //alert(initialhour);
            var getRow = $(this).parents('tr');
            var hour = (getRow.find('td:eq(4)').html());
            var InTime1 = initialhour.split(":");
            var t1 = initialhour;
            var mins = 0;
            var hrs = 0;
            t1 = t1.split(':');
            var convminute = parseInt(t1[0]) * 60 + parseInt(t1[1]);

            var t2 = hour.split(':');

            var miutestoremove = parseInt(t2[0]) * 60 + parseInt(t2[1]);

            var result = convminute - miutestoremove;

            t1 = parseInt((result / 60)).padDigit() + ':' + parseInt((result % 60)).padDigit();
            //var t2 = hour.split(':');
            //console.log(Number(t1[1]) + Number(t2[1]))
            //mins = Number(t1[1]) - Number(t2[1]);
            //var minhrs = Math.floor(parseInt(mins / 60));
            //hrs = Number(t1[0]) - Number(t2[0]) + minhrs;
            //mins = mins % 60;
            //t1 = hrs.padDigit() + ':' + mins.padDigit();
            //console.log(t1)

            $("#TotalScheduleHour").val(t1);
            var initialdays = parseInt($('#TotalScheduleDays').val());
            var getRow1 = $(this).parents('tr');
            var Day = (getRow1.find('td:eq(3)').html());
            var totalday = parseInt(initialdays) - parseInt(Day);

          //  $('#TotalScheduleDays').val(totalday);
        }
        //var inithour = $('#TotalScheduleHour').val();
        //if(inithour=="00:00")
        //{
        //    $('#ScheduleApprove').hide();
        //}
    });
    $(document).on("click", ".cbCheck3", function () {
        debugger;//Calculate Total hours and days
        if ($('.cbCheck2').is(':checked')) {
            $('.cbCheck2').attr('checked', false);
            $("#TotalScheduleHour").val('00:00');
          //  $('#TotalScheduleDays').val(0);

        }
        $('#ScheduleReject').show();
        $('#ScheduleApprove').hide();
        var checked = $(this).is(":checked");
        if (checked) {
            $('#ScheduleReject').show();
            var initialhour = $('#TotalScheduleHour').val();
            //alert(initialhour);
            var getRow = $(this).parents('tr');
            var hour = (getRow.find('td:eq(4)').html());
            var InTime1 = initialhour.split(":");
            var t1 = initialhour;
            var mins = 0;
            var hrs = 0;
            t1 = t1.split(':');
            var t2 = hour.split(':');
            console.log(Number(t1[1]) + Number(t2[1]))
            mins = Number(t1[1]) + Number(t2[1]);
            var minhrs = Math.floor(parseInt(mins / 60));
            hrs = Number(t1[0]) + Number(t2[0]) + minhrs;
            mins = mins % 60;
            t1 = hrs.padDigit() + ':' + mins.padDigit();
            console.log(t1)

            $("#TotalScheduleHour").val(t1);
            var initialdays = parseInt($('#TotalScheduleDays').val());
            var getRow1 = $(this).parents('tr');
            var Day = (getRow1.find('td:eq(3)').html());
            var totalday = parseInt(initialdays) + parseInt(Day);

          //  $('#TotalScheduleDays').val(totalday);

        }
        else {
            $('#ScheduleReject').hide();
            var initialhour = $('#TotalScheduleHour').val();
            //alert(initialhour);
            var getRow = $(this).parents('tr');
            var hour = (getRow.find('td:eq(4)').html());
            var InTime1 = initialhour.split(":");
            var t1 = initialhour;
            var mins = 0;
            var hrs = 0;
            t1 = t1.split(':');
            var convminute = parseInt(t1[0]) * 60 + parseInt(t1[1]);

            var t2 = hour.split(':');

            var miutestoremove = parseInt(t2[0]) * 60 + parseInt(t2[1]);

            var result = convminute - miutestoremove;

            t1 = parseInt((result / 60)).padDigit() + ':' + parseInt((result % 60)).padDigit();
            //var t2 = hour.split(':');
            //console.log(Number(t1[1]) + Number(t2[1]))
            //mins = Number(t1[1]) - Number(t2[1]);
            //var minhrs = Math.floor(parseInt(mins / 60));
            //hrs = Number(t1[0]) - Number(t2[0]) + minhrs;
            //mins = mins % 60;
            //t1 = hrs.padDigit() + ':' + mins.padDigit();
            //console.log(t1)

            $("#TotalScheduleHour").val(t1);
            var initialdays = parseInt($('#TotalScheduleDays').val());
            var getRow1 = $(this).parents('tr');
            var Day = (getRow1.find('td:eq(3)').html());
            var totalday = parseInt(initialdays) - parseInt(Day);

         //   $('#TotalScheduleDays').val(totalday);
        }
        //if (inithour == "00:00") {
        //    $('#ScheduleReject').hide();
        //}
    });
}

function TravelTotalHours() {
    Number.prototype.padDigit = function () {
        return (this < 10) ? '0' + this : this;
    }  
    $(document).on("click", ".TravelApprvchk", function () {
        if ($('.TravelRejctchk').is(':checked')) {
            $('.TravelRejctchk').attr('checked', false);
            $('#TravelTotalDay').val(0);          
        }
        $('#TravelApprove').show();
        $('#TravelReject').hide();
        $('#TravelTotalHour').val('0:00');
        var checked = $(this).is(":checked");
        if (checked) {
            $('#TravelApprove').show();
            var initialhour = $('#TravelTotalHour').val();
            var getRow = $(this).parents('tr');
            var hour = (getRow.find('td:eq(11)').html());
            var InTime1 = initialhour.split(":");           
            var t1 = initialhour;
            var mins = 0;
            var hrs = 0;
            t1 = t1.split(':');
            var t2 = hour.split(':');
            console.log(Number(t1[1]) + Number(t2[1]))
            mins = Number(t1[1]) + Number(t2[1]);
            var minhrs = Math.floor(parseInt(mins / 60));
            hrs = Number(t1[0]) + Number(t2[0]) + minhrs;
            mins = mins % 60;
            t1 = hrs.padDigit() + ':' + mins.padDigit();
            $("#TravelTotalHour").val(t1);         
            var initialdays = parseInt($('#TravelTotalDay').val());
            var getRow1 = $(this).parents('tr');
            var Day = (getRow1.find('td:eq(10)').html());
            var totalday = parseInt(initialdays) + parseInt(Day);
            $('#TravelTotalDay').val(totalday);
        }
        else {
            $('#TravelApprove').hide();
            var initialhour = $('#TravelTotalHour').val();
            var getRow = $(this).parents('tr');
            var hour = (getRow.find('td:eq(11)').html());
            var InTime1 = initialhour.split(":");
            var t1 = initialhour;          
            var mins = 0;
            var hrs = 0;
            t1 = t1.split(':');
            var convminute = parseInt(t1[0]) * 60 + parseInt(t1[1]);
            var t2 = hour.split(':');
            var miutestoremove = parseInt(t2[0]) * 60 + parseInt(t2[1]);
            var result = convminute - miutestoremove;
            t1 = parseInt((result / 60)).padDigit() + ':' + parseInt((result % 60)).padDigit();
            var initialdays = parseInt($('#TravelTotalDay').val());
            var getRow1 = $(this).parents('tr');
            var Day = (getRow1.find('td:eq(10)').html());
            var totalday = parseInt(initialdays) - parseInt(Day);
            $('#TravelTotalDay').val(totalday);
        }
        var inihour = $('#TravelTotalHour').val();
        var inidays = parseInt($('#TravelTotalDay').val());        
    });
    $(document).on("click", ".TravelRejctchk", function () {
        //Calculate Total hours and days
        if ($('.TravelApprvchk').is(':checked')) {
            $('.TravelApprvchk').attr('checked', false);
            $("#TravelTotalHour").val('00:00');
            $('#TravelTotalDay').val(0);
        }
        $('#TravelTotalHour').val('0:00');
        $('#TravelReject').show();
        $('#TravelApprove').hide();
        var checked = $(this).is(":checked");
        if (checked) {
            $('#TravelReject').show();
            var initialhour = $('#TravelTotalHour').val();
            var getRow = $(this).parents('tr');
            var hour = (getRow.find('td:eq(11)').html());
            var InTime1 = initialhour.split(":");
            var t1 = initialhour;
            var mins = 0;
            var hrs = 0;
            t1 = t1.split(':');
            var t2 = hour.split(':');
            console.log(Number(t1[1]) + Number(t2[1]))
            mins = Number(t1[1]) + Number(t2[1]);
            var minhrs = Math.floor(parseInt(mins / 60));
            hrs = Number(t1[0]) + Number(t2[0]) + minhrs;
            mins = mins % 60;
            t1 = hrs.padDigit() + ':' + mins.padDigit();
            console.log(t1);
            $("#TravelTotalHour").val(t1);
            var initialdays = parseInt($('#TravelTotalDay').val());
            var getRow1 = $(this).parents('tr');
            var Day = (getRow1.find('td:eq(10)').html());
            var totalday = parseInt(initialdays) + parseInt(Day);
            $('#TravelTotalDay').val(totalday);
        }
        else {
            $('#TravelReject').hide();
            var initialhour = $('#TravelTotalHour').val();
            var getRow = $(this).parents('tr');
            var hour = (getRow.find('td:eq(11)').html());
            var InTime1 = initialhour.split(":");
            var t1 = initialhour;
            var mins = 0;
            var hrs = 0;
            t1 = t1.split(':');
            var convminute = parseInt(t1[0]) * 60 + parseInt(t1[1]);
            var t2 = hour.split(':');
            var miutestoremove = parseInt(t2[0]) * 60 + parseInt(t2[1]);
            var result = convminute - miutestoremove;
            t1 = parseInt((result / 60)).padDigit() + ':' + parseInt((result % 60)).padDigit();
            var initialdays = parseInt($('#TravelTotalDay').val());
            var getRow1 = $(this).parents('tr');
            var Day = (getRow1.find('td:eq(10)').html());
            var totalday = parseInt(initialdays) - parseInt(Day);
            $('#TravelTotalDay').val(totalday);
            $('#TravelTotalDay').val(totalday);
        }
        var inihour = $('#TravelTotalHour').val();
        var inidays = parseInt($('#TravelTotalDay').val());
       
    });
}


function AnualTotalHours() {
    Number.prototype.padDigit = function () {
        return (this < 10) ? '0' + this : this;
    }
    $(document).on("click", ".AnuApprovechk", function () {             //Calculate Total hours and days
        if ($('.AnuRejectchk').is(':checked')) {
            $('.AnuRejectchk').attr('checked', false);
            $("#Holidaysapprove").val(0);
            $('#RemainingHolidays').val(0);
        }
        $('#AnnualLeaveApprove').show();
        $('#AnnualLeaveReject').hide();
        var checked = $(this).is(":checked");
        if (checked) {
            $('#AnnualLeaveApprove').show();
            var initialhour = $('#Holidaysapprove').val();
            var getRow = $(this).parents('tr');
            var Days = (getRow.find('td:eq(3)').html());
          //  $("#Holidaysapprove").val(Days);
            var Totaldays = parseInt($('#TotalHolidays').val());
            var HolidayTaken = parseInt($('#HolidaysTaken').val());
            var HolidayApprove = parseInt($('#Holidaysapprove').val());
            var totalday = parseInt(Totaldays) - parseInt(HolidayTaken) - parseInt(HolidayApprove);
            if (totalday < 0) {
                totalday = 0;
            }
            else {
                totalday = totalday;
            }
            $('#RemainingHolidays').val(totalday);
        }
        else {
            $('#AnnualLeaveApprove').hide();
            //var initialhour = $('#Holidaysapprove').val();
            //var getRow = $(this).parents('tr');
            //var Days = (getRow.find('td:eq(3)').html());
            //$("#Holidaysapprove").val(Days);
            //var Totaldays = parseInt($('#TotalHolidays').val());
            //var HolidayTaken = parseInt($('#HolidaysTaken').val());
            //var HolidayApprove = parseInt($('#Holidaysapprove').val());
            //var totalday = parseInt(Totaldays) + parseInt(HolidayTaken) + parseInt(HolidayApprove);
            //$('#RemainingHolidays').val(totalday);
        }
        var inihour = $('#Holidaysapprove').val();
        var initotaldays = parseInt($('#TotalHolidays').val());
        
    });
    $(document).on("click", ".AnuRejectchk", function () {             //Calculate Total hours and days
        if ($('.AnuApprovechk').is(':checked')) {
            $('.AnuApprovechk').attr('checked', false);
            $("#Holidaysapprove").val(0);
            $('#RemainingHolidays').val(0);

        }
        $('#AnnualLeaveReject').show();
        $('#AnnualLeaveApprove').hide();
        var checked = $(this).is(":checked");
        if (checked) {
            $('#AnnualLeaveReject').show();
            var initialhour = $('#Holidaysapprove').val();

            var getRow = $(this).parents('tr');
            var Days = (getRow.find('td:eq(3)').html());


        //    $("#Holidaysapprove").val(Days);
            var Totaldays = parseInt($('#TotalHolidays').val());
            var HolidayTaken = parseInt($('#HolidaysTaken').val());
            var HolidayApprove = parseInt($('#Holidaysapprove').val());

            var totalday = parseInt(Totaldays) - parseInt(HolidayTaken) - parseInt(HolidayApprove);

          //  $('#RemainingHolidays').val(totalday);

        }
        else {
            $('#AnnualLeaveReject').hide();
            var initialhour = $('#Holidaysapprove').val();
            var getRow = $(this).parents('tr');
            var Days = (getRow.find('td:eq(3)').html());

           // $("#Holidaysapprove").val(Days);
            var Totaldays = parseInt($('#TotalHolidays').val());
            var HolidayTaken = parseInt($('#HolidaysTaken').val());
            var HolidayApprove = parseInt($('#Holidaysapprove').val());

            var totalday = parseInt(Totaldays) + parseInt(HolidayTaken) + parseInt(HolidayApprove);

           // $('#RemainingHolidays').val(totalday);
        }
        var inihour = $('#Holidaysapprove').val();
        var initotaldays = parseInt($('#TotalHolidays').val());
      
    });
}



function OtherTotalDays() {
    Number.prototype.padDigit = function () {
        return (this < 10) ? '0' + this : this;
    }
    $(document).on("click", ".othApprovechk", function () {
        //Calculate Total hours and days
        if ($('.othRejectchk').is(':checked')) {
                $('.othRejectchk').attr('checked', false);
                $("#OtherLeaveInTheYear").val();
                
        }
        $('#OtherLeaveApprove').show();
        $('#OtherLeaveReject').hide();
        var checked = $(this).is(":checked");
        if (checked) {
            var initialDays = $('#OtherLeaveApproved').val();
            $('#OtherLeaveApprove').show();

            //var getRow = $(this).parents('tr');
            //var Days = (getRow.find('td:eq(3)').html());
            //$("#OtherLeaveApproved").val(Days);


            //var Totaldays = parseInt($('#TotalHolidays').val());
            //var HolidayTaken = parseInt($('#HolidaysTaken').val());
            //var HolidayApprove = parseInt($('#Holidaysapprove').val());

            //var totalday = parseInt(Totaldays) - parseInt(HolidayTaken) - parseInt(HolidayApprove);

            //$('#RemainingHolidays').val(totalday);
        }
        else {
            var initialDays = $('#OtherLeaveApproved').val();
            var getRow = $(this).parents('tr');
            var Days = (getRow.find('td:eq(3)').html());
            $('#OtherLeaveApprove').hide();

        }
        

    });
    $(document).on("click", ".othRejectchk", function () {
      //Calculate Total hours and days
        if ($('.othApprovechk').is(':checked')) {
            $('.othApprovechk').attr('checked', false);
            $("#OtherLeaveInTheYear").val(0);
            $('#OtherLeaveApproved').val(0);
        }
        $('#OtherLeaveApprove').hide();
        $('#OtherLeaveReject').show();
        var checked = $(this).is(":checked");
        if (checked) {
            var initialdays = $('#OtherLeaveApproved').val();
            $('#OtherLeaveReject').show();

            //var getRow = $(this).parents('tr');
            //var Days = (getRow.find('td:eq(3)').html());

            //$("#OtherLeaveApproved").val(Days);
            //var Totaldays = parseInt($('#TotalHolidays').val());
            //var HolidayTaken = parseInt($('#HolidaysTaken').val());
            //var HolidayApprove = parseInt($('#Holidaysapprove').val());

            //var totalday = parseInt(Totaldays) - parseInt(HolidayTaken) - parseInt(HolidayApprove);

            //$('#RemainingHolidays').val(totalday);

        }
        else {
            var initialhour = $('#OtherLeaveApproved').val();
            var getRow = $(this).parents('tr');
            var Days = (getRow.find('td:eq(3)').html());
            $('#OtherLeaveReject').hide();

        }
        //var iniDays = $('#OtherLeaveApproved').val();
        //if (iniDays == "0") {         
        //    $('#OtherLeaveReject').hide();
        //}
    });
}


function TrainingTotalHours() {
    Number.prototype.padDigit = function () {
        return (this < 10) ? '0' + this : this;
    }
    $(document).on("click", ".TrinApprvchk", function () {             //Calculate Total hours and days
        if ($('.TraiRejctchk').is(':checked')) {
            $('.TraiRejctchk').attr('checked', false);
          //  $("#TotalTrainingDays").val(0);
            $('#TrainingApproved').val(0);
            $('#TrainingDaysApproved').val(0);
          //  $('#TrainingCosts').val(0);
        }
        $('#TrainingApprove').show();
        $('#TrainingReject').hide();
        var checked = $(this).is(":checked");
        if (checked) {
            var initialDays = $('#TrainingDaysApproved').val();
            $('#TrainingApprove').show();
            var getRow = $(this).parents('tr');
            var Days = (getRow.find('td:eq(5)').html());

        //    $("#TrainingDaysApproved").val(Days);

            //var Totaldays = parseInt($('#TotalHolidays').val());
            //var HolidayTaken = parseInt($('#HolidaysTaken').val());
            //var HolidayApprove = parseInt($('#Holidaysapprove').val());

            //var totalday = parseInt(Totaldays) - parseInt(HolidayTaken) - parseInt(HolidayApprove);

            //$('#RemainingHolidays').val(totalday);
        }
        else {
            var initialDays = $('#TrainingDaysApproved').val();
            var getRow = $(this).parents('tr');
            var Days = (getRow.find('td:eq(5)').html());
            $('#TrainingApprove').hide();

        }
        //var iniDays = $('#TrainingDaysApproved').val();
        //if(iniDays=="0")
        //{
        //    $('#TrainingApprove').hide();        
        //}
    });
    $(document).on("click", ".TraiRejctchk", function () {             //Calculate Total hours and days
        if ($('.TrinApprvchk').is(':checked')) {
            $('.TrinApprvchk').attr('checked', false);
         //   $("#TotalTrainingDays").val(0);
            $('#TrainingApproved').val(0);
          //  $('#TrainingDaysApproved').val(0);
            $('#TrainingCosts').val(0);

        }
        $('#TrainingApprove').hide();
        $('#TrainingReject').show();
        var checked = $(this).is(":checked");
        if (checked) {
            var initialhour = $('#TrainingDaysApproved').val();
            $('#TrainingReject').show();
            var getRow = $(this).parents('tr');
            var Days = (getRow.find('td:eq(5)').html());


           // $("#TrainingDaysApproved").val(Days);
            //var Totaldays = parseInt($('#TotalHolidays').val());
            //var HolidayTaken = parseInt($('#HolidaysTaken').val());
            //var HolidayApprove = parseInt($('#Holidaysapprove').val());

            //var totalday = parseInt(Totaldays) - parseInt(HolidayTaken) - parseInt(HolidayApprove);

            //$('#RemainingHolidays').val(totalday);

        }
        else {

            var initialhour = $('#TrainingDaysApproved').val();
            var getRow = $(this).parents('tr');
            var Days = (getRow.find('td:eq(5)').html());
            $('#TrainingReject').hide();

         //   $("#TrainingDaysApproved").val(Days);
            //var Totaldays = parseInt($('#TotalHolidays').val());
            //var HolidayTaken = parseInt($('#HolidaysTaken').val());
            //var HolidayApprove = parseInt($('#Holidaysapprove').val());

            //var totalday = parseInt(Totaldays) + parseInt(HolidayTaken) + parseInt(HolidayApprove);

            //$('#RemainingHolidays').val(totalday);
            //var iniDays = $('#TrainingDaysApproved').val();
            //if (iniDays == "0") {             
            //    $('#TrainingReject').hide();
            //}
        }
    });
}

function NewvacnyChk() {
    $(document).on("click", ".VacnyApprvchk", function () {
        if ($('.vacnyRejctchk').is(':checked')) {
            $('.vacnyRejctchk').attr('checked', false);
            $('#NewVacancyApprove').show();
            $('#NewVacancyReject').hide();

        }
        $('#NewVacancyApprove').show();
        $('#NewVacancyReject').hide();
    });
    $(document).on("click", ".vacnyRejctchk", function () {
        if ($('.VacnyApprvchk').is(':checked')) {
            $('.VacnyApprvchk').attr('checked', false);
            $('#NewVacancyApprove').hide();
            $('#NewVacancyReject').show();
        }
        $('#NewVacancyApprove').hide();
        $('#NewVacancyReject').show();
    });

}

function upliftTotalHours() {
    Number.prototype.padDigit = function () {
        return (this < 10) ? '0' + this : this;
    }
    $(document).on("click", ".UpliftApprvchk", function () {             //Calculate Total hours and days
        if ($('.UpliftRejctchk').is(':checked')) {
            $('.UpliftRejctchk').attr('checked', false);
           // $("#TotalHrUpliftInTheYear").val('00:00');
            $('#NoofUpliftApproved').val('00:00');
        }
        $('#Upliftapprove').show();
        $('#UpliftReject').hide();
        var checked = $(this).is(":checked");
        if (checked) {
            var initialhour = $('#NoofUpliftApproved').val();
            $('#Upliftapprove').show();
            var getRow = $(this).parents('tr');
            var hour = (getRow.find('td:eq(4)').html());
            var InTime1 = initialhour.split(":");
            var t1 = initialhour;
            var mins = 0;
            var hrs = 0;
            t1 = t1.split(':');
            var t2 = hour.split(':');
           console.log(Number(t1[1]) + Number(t2[1]))
            mins = Number(t1[1]) + Number(t2[1]);
            var minhrs = Math.floor(parseInt(mins / 60));
            hrs = Number(t1[0]) + Number(t2[0]) + minhrs;
            mins = mins % 60;
            t1 = hrs.padDigit() + ':' + mins.padDigit();
            $("#NoofUpliftApproved").val(t1);

        }
        else {
            $('#Upliftapprove').hide();
            var initialhour = $('#NoofUpliftApproved').val();
            var getRow = $(this).parents('tr');
            var hour = (getRow.find('td:eq(4)').html());
            var InTime1 = initialhour.split(":");
            var t1 = initialhour;
            var mins = 0;
            var hrs = 0;
            t1 = t1.split(':');
            var convminute = parseInt(t1[0]) * 60 + parseInt(t1[1]);

            var t2 = hour.split(':');

            var miutestoremove = parseInt(t2[0]) * 60 + parseInt(t2[1]);

            var result = convminute - miutestoremove;

            t1 = parseInt((result / 60)).padDigit() + ':' + parseInt((result % 60)).padDigit();
           
            $("#NoofUpliftApproved").val(t1);
            
        }
        //var inihour = $('#NoofUpliftApproved').val();
        //if(inihour=="00:00")
        //{
        //    $('#Upliftapprove').hide();            
        //}
    });
    $(document).on("click", ".UpliftRejctchk", function () {             //Calculate Total hours and days
        if ($('.UpliftApprvchk').is(':checked')) {
            $('.UpliftApprvchk').attr('checked', false);
           // $("#TotalHrUpliftInTheYear").val('00:00');
            $('#NoofUpliftApproved').val('00:00');
        }
        $('#Upliftapprove').hide();
        $('#UpliftReject').show();
        var checked = $(this).is(":checked");
        if (checked) {
            $('#UpliftReject').show();
            var initialhour = $('#NoofUpliftApproved').val();
            var getRow = $(this).parents('tr');
            var hour = (getRow.find('td:eq(4)').html());
            var InTime1 = initialhour.split(":");
            var t1 = initialhour;
            var mins = 0;
            var hrs = 0;
            t1 = t1.split(':');
            var t2 = hour.split(':');
            console.log(Number(t1[1]) + Number(t2[1]))
            mins = Number(t1[1]) + Number(t2[1]);
            var minhrs = Math.floor(parseInt(mins / 60));
            hrs = Number(t1[0]) + Number(t2[0]) + minhrs;
            mins = mins % 60;
            t1 = hrs.padDigit() + ':' + mins.padDigit();

            $("#NoofUpliftApproved").val(t1);
        }
        else {
            $('#UpliftReject').hide();

            var initialhour = $('#NoofUpliftApproved').val();
            var getRow = $(this).parents('tr');
            var hour = (getRow.find('td:eq(4)').html());
            var InTime1 = initialhour.split(":");
            var t1 = initialhour;
            var mins = 0;
            var hrs = 0;
            t1 = t1.split(':');
            var convminute = parseInt(t1[0]) * 60 + parseInt(t1[1]);

            var t2 = hour.split(':');

            var miutestoremove = parseInt(t2[0]) * 60 + parseInt(t2[1]);

            var result = convminute - miutestoremove;

            t1 = parseInt((result / 60)).padDigit() + ':' + parseInt((result % 60)).padDigit();

            $("#NoofUpliftApproved").val(t1);
        }
        //var inihour = $('#NoofUpliftApproved').val();
        //if (inihour == "00:00") {         
        //    $('#UpliftReject').hide();
        //}
    });
}

function SickTotalDays() {
    Number.prototype.padDigit = function () {
        return (this < 10) ? '0' + this : this;
    }
    $(document).on("click", ".othApprovechk", function () {
        if ($('.othRejectchk').is(':checked')) {
            $('.othRejectchk').attr('checked', false);
        }
        $('#SickLeaveApprovebtn').show();
        $('#sickLeaveRejectbtn').hide();
        var checked = $(this).is(":checked");
        if (checked) {
            var initialDays = $('#SickLeaveApprove').val();
            $('#SickLeaveApprovebtn').show();

        }
        else {
            var initialDays = $('#SickLeaveApprove').val();
            var getRow = $(this).parents('tr');
            var Days = (getRow.find('td:eq(3)').html());
            $('#SickLeaveApprovebtn').hide();

        }
        //var inday = $('#SickLeaveApprove').val();
        //if (inday == "0") {          
        //    $('#SickLeaveApprovebtn').hide();            
        //}

    });
    $(document).on("click", ".othRejectchk", function () {
        if ($('.othApprovechk').is(':checked')) {
            $('.othApprovechk').attr('checked', false);
            $('#SickLeaveApprove').val(0);
        }
        $('#SickLeaveApprovebtn').hide();
        $('#sickLeaveRejectbtn').show();
        var checked = $(this).is(":checked");
        if (checked) {
            var initialdays = $('#SickLeaveApprove').val();
            $('#sickLeaveRejectbtn').show();
        }
        else {
            var initialhour = $('#SickLeaveApprove').val();
            var getRow = $(this).parents('tr');
            var Days = (getRow.find('td:eq(3)').html());
            $('#sickLeaveRejectbtn').hide();

        }
        //var inday = $('#SickLeaveApprove').val();
        //if (inday == "0") {
        //    $('#sickLeaveRejectbtn').hide();
        //}
    });
   
}

function MatPatTotalDays()
{
    Number.prototype.padDigit = function () {
        return (this < 10) ? '0' + this : this;
    }
    $(document).on("click", ".othApprovechk", function () {
        if ($('.othRejectchk').is(':checked')) {
            $('.othRejectchk').attr('checked', false);
        }
        $('#MatPatApprovebtn').show();
        $('#MatPatReject').hide();
        var checked = $(this).is(":checked");
        if (checked) {
            $('#MatPatApprovebtn').show();

        }
        else {
            $('#MatPatApprovebtn').hide();

        }
        //var inday = $('#SickLeaveApprove').val();
        //if (inday == "0") {
        //    $('#SickLeaveApprovebtn').hide();
        //}

    });
    $(document).on("click", ".othRejectchk", function () {
        if ($('.othApprovechk').is(':checked')) {
            $('.othApprovechk').attr('checked', false);
            $('#SickLeaveApprove').val(0);
        }
        $('#MatPatApprovebtn').hide();
        $('#MatPatReject').show();
        var checked = $(this).is(":checked");
        if (checked) {
            $('#MatPatReject').show();
        }
        else {
            //var initialhour = $('#SickLeaveApprove').val();
            //var getRow = $(this).parents('tr');
            //var Days = (getRow.find('td:eq(3)').html());
            $('#MatPatReject').hide();
        }
        //var inday = $('#SickLeaveApprove').val();
        //if (inday == "0") {
        //    $('#sickLeaveRejectbtn').hide();
        //}
    });
}