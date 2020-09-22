
$(document).ready(function () {

   // TimesheetSearch();
  //  ScheduleSerch();
 //   TravelSearch();
 //   TrainingSearch();
 //   NewVacancySearch();
  //  OtherSearch();
  //  UpliftSearch();
  //  MatPatLeaveSearch();
  //  AnualSearch();
});

function MatPatLeaveSearch() {
    var table = $('#DocumentModalTable12').DataTable({
        "sDom": '<"top"i>rt<"bottom"flp><"clear">'
    });

    $('#tableDiv').find('.dataTables_filter').hide();
    $('#tableDiv').find('.dataTables_info').hide();

    // $('#DocumentModalTable tfoot tr').appendTo('#DocumentModalTable thead');

    $("#DocumentModalTable12 thead .SearchdueDate").keyup(function () {
        table.column(1).search(this.value).draw();
    });
    $("#DocumentModalTable12 thead .SearchLink").keyup(function () {
        table.column(2).search(this.value).draw();
    });
    $("#DocumentModalTable12 thead .SearchStatus").keyup(function () {
        table.column(4).search(this.value).draw();
    });

}

$("#Timeshetapprvebtn").button().click(function () {
    var ids = $("#DocumentModalTable tr:has(input:checked)").map(function () {
        var $tr = $(this);
        var id = $tr.find("td:first").text();
        return id;
    }).toArray();
    var Id = ids.join(", ");
    //alert(Id);
    $.post(constantApproval.TimesheetApprovebtnClk, { "ID": Id },
    function (json) {      
        var hidden_fields = $('#WorkerId').val();
        $.ajax({

            url: constantApproval.TimesheetApprove,
            type: "POST",
            dataType: "html",
            data: { EmpID: hidden_fields },
            success: function (data) {
                $('#timesheetlist').html(data);
                TimesheetSearch();
            },
        });
    })
   
   
});
function TimesheetSearch() {
    var table = $('#DocumentModalTable').DataTable({
       "sDom": '<"top"i>rt<"bottom"flp><"clear">'
    });
    //$('#tableDiv').find('.dataTables_filter').hide();
    //$('#tableDiv').find('.dataTables_info').hide();
    // $('#DocumentModalTable tfoot tr').appendTo('#DocumentModalTable thead');
    $("#DocumentModalTable thead .SearchDay").keyup(function () {
        table.column(1).search(this.value).draw();
    });
    $("#DocumentModalTable thead .SearchDate").keyup(function () {
        table.column(2).search(this.value).draw();
    });
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
}
$("#page_content_inner0").on('click', '.btn-Refresh-Document', function () {
    var ID = $('#WorkerId').val();
    $('#page_content_inner0').load(constantApproval.TimesheetApprove, { "EmpID": ID });
});
$("#page_content_inner0").on('click', '.btn-ClearSorting-Document', function () {
    var ID = $('#WorkerId').val();
    $('#page_content_inner0').load(constantApproval.TimesheetApprove, { "EmpID": ID });
});

$("#page_content_inner0").on('click', '.btn-clearFilter-Document', function () {
    var ID = $('#WorkerId').val();
    $('#page_content_inner0').load(constantApproval.TimesheetApprove, { "EmpID": ID });
});
$("#Timeshetrejectbtn").button().click(function () {
    var ids = $("#DocumentModalTable tr:has(input:checked)").map(function () {
        var $tr = $(this);
        var id = $tr.find("td:first").text();
        return id;
    }).toArray();
    var Id = ids.join(", ");
    $.post(constantApproval.TimesheetRejectbtnClk, { "ID": Id },
   function (json) {
       var hidden_fields = $('#WorkerId').val();
       $.ajax({
           url: constantApproval.TimesheetApprove,
           type: "POST",
           dataType: "html",
           data: { EmpID: hidden_fields },
           success: function (data) {
               $('#timesheetlist').html(data);
               TimesheetSearch();
           },
       });
   })

    
});
$("#ScheduleApprove").button().click(function () {
    var ids = $("#DocumentModalTable1 tr:has(input:checked)").map(function () {
        var $tr = $(this);
        var id = $tr.find("td:first").text();
        return id;
    }).toArray();
    var Id = ids.join(", ");
    $.post(constantApproval.UpdateScheduleApprova, { "ID": Id },
    function (json) {
        var hidden_fields = $('#ScheduleWorkerId').val();
        //alert(hidden_fields);
        $.ajax({
            url: constantApproval.ScheduleApproval,
            type: "POST",
            dataType: "html",
            data: { EmpID: hidden_fields },
            success: function (data) {
                $('#Schedulelist').html(data);
                ScheduleSerch();
            },
        });
    })
   

});
$("#ScheduleReject").button().click(function () {

    var ids = $("#DocumentModalTable1 tr:has(input:checked)").map(function () {
        var $tr = $(this);
        var id = $tr.find("td:first").text();
        return id;
    }).toArray();
    var Id = ids.join(", ");
    $.post(constantApproval.UpdateScheduleReject, { "ID": Id },
    function (json) {
        var hidden_fields = $('#ScheduleWorkerId').val();
        $.ajax({

            url: constantApproval.ScheduleApproval,
            type: "POST",
            dataType: "html",
            data: { EmpID: hidden_fields },
            success: function (data) {
                $('#Schedulelist').html(data);
                ScheduleSerch();
            },
        });
    })
   
});
function ScheduleSerch() {
    var table = $('#DocumentModalTable1').DataTable({
        "sDom": '<"top"i>rt<"bottom"flp><"clear">'
    });
    $('#tableDiv').find('.dataTables_filter').hide();
    $('#tableDiv').find('.dataTables_info').hide();
    // $('#DocumentModalTable tfoot tr').appendTo('#DocumentModalTable thead');

    $("#DocumentModalTable1 thead .SearchStartDate").keyup(function () {
        table.column(1).search(this.value).draw();
    });
    $("#DocumentModalTable1 thead .SearchEndDate").keyup(function () {
        table.column(2).search(this.value).draw();
    });
    $("#DocumentModalTable1 thead .SearchHours").keyup(function () {
        table.column(3).search(this.value).draw();
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

}

$("#page_content_inner1").on('click', '.btn-Refresh-Document', function () {
    var ID=$('#ScheduleWorkerId').val();
    $('#page_content_inner1').load(constantApproval.ScheduleApproval, { "EmpID": ID });
});

$("#page_content_inner1").on('click', '.btn-ClearSorting-Document', function () {
    var ID = $('#ScheduleWorkerId').val();
    $('#page_content_inner1').load(constantApproval.ScheduleApproval, { "EmpID": ID });
});

$("#page_content_inner1").on('click', '.btn-clearFilter-Document', function () {
    var ID = $('#ScheduleWorkerId').val();
    $('#page_content_inner1').load(constantApproval.ScheduleApproval, { "EmpID": ID });
});

$("#TravelApprove").button().click(function () {

    var ids = $("#DocumentModalTable2 tr:has(input:checked)").map(function () {
        var $tr = $(this);
        var id = $tr.find("td:first").text();
        return id;
    }).toArray();
    var Id = ids.join(", ");
    $.post(constantApproval.UpdateTravelApprova, { "ID": Id },
    function (json) {
        var hidden_fields = $('#TravelLeaveWorkerId').val();
        $.ajax({

            url: constantApproval.TravelApprove,
            type: "POST",
            dataType: "html",
            data: { EmpID: hidden_fields },
            success: function (data) {
                $('#Travellist').html(data);
                TravelSearch();
            },
        });
    })
   
});

$("#TravelReject").button().click(function () {

    var ids = $("#DocumentModalTable2 tr:has(input:checked)").map(function () {
        var $tr = $(this);
        var id = $tr.find("td:first").text();
        return id;
    }).toArray();
    var Id = ids.join(", ");
    $.post(constantApproval.UpdateTravelReject, { "ID": Id },
    function (json) {
        var hidden_fields = $('#TravelLeaveWorkerId').val();
    //alert(hidden_fields);
    $.ajax({

        url: constantApproval.TravelApprove,
        type: "POST",
        dataType: "html",
        data: { EmpID: hidden_fields },
        success: function (data) {
            $('#Travellist').html(data);
            TravelSearch();
        },
    });
    })
   
});

function TravelSearch() {
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
    $("#DocumentModalTable2 thead .SearchStartDate").keyup(function () {
        table.column(9).search(this.value).draw();
    });
    $("#DocumentModalTable2 thead .SearchEndDate").keyup(function () {
        table.column(10).search(this.value).draw();
    });
    $("#DocumentModalTable2 thead .SearchProject").keyup(function () {
        table.column(13).search(this.value).draw();
    });
    $("#DocumentModalTable2 thead .SearchCustomer").keyup(function () {
        table.column(14).search(this.value).draw();
    });   
    $("#DocumentModalTable2 thead .SearchProject").keyup(function () {
        table.column(18).search(this.value).draw();
    });
}



$("#page_content_inner2").on('click', '.btn-Refresh-Document', function () {
    var ID = $('#TravelLeaveWorkerId').val();
    $('#page_content_inner2').load(constantApproval.TravelApprove, { "EmpID": ID });
});

$("#page_content_inner2").on('click', '.btn-ClearSorting-Document', function () {
    var ID = $('#TravelLeaveWorkerId').val();
    $('#page_content_inner2').load(constantApproval.TravelApprove, { "EmpID": ID });
});

$("#page_content_inner2").on('click', '.btn-clearFilter-Document', function () {
    var ID = $('#TravelLeaveWorkerId').val();
    $('#page_content_inner2').load(constantApproval.TravelApprove, { "EmpID": ID });
});

$("#AnnualLeaveApprove").button().click(function () {
    var ids = $("#DocumentModalTable3 tr:has(input:checked)").map(function () {
        var $tr = $(this);
        var id = $tr.find("td:first").text();
        return id;
    }).toArray();
    var Id = ids.join(", ");
    //alert(Id);
    $.post(constantApproval.UpdateAnnualLeaveApprova, { "ID": Id },
    function (json) {
        //alert(Id);
        var hidden_fields = $("#AnnulLeaveWorkerId").val();
        $.ajax({
            type: 'POST',
            dataType: "html",
            url: constantApproval.AnualLeaveApprove,
            
            data: { EmpID: hidden_fields },
            success: function (data) {
                //alert(result);
                $('#Anualleavelist').html(data);

            },
        });

    })
    
});
$("#AnnualLeaveReject").button().click(function () {
    var ids = $("#DocumentModalTable3 tr:has(input:checked)").map(function () {
        var $tr = $(this);
        var id = $tr.find("td:first").text();
        return id;
    }).toArray();
    var Id = ids.join(", ");
    //alert(Id);
    $.post(constantApproval.UpdateAnnualLeaveReject, { "ID": Id },
    function (json) {
        //alert(Id);
        var hidden_fields = $("#AnnulLeaveWorkerId").val();
        $.ajax({
            type: 'POST',
            dataType: "html",
            url: constantApproval.AnualLeaveApprove,
            data: {
                EmpID: hidden_fields
            },
            success: function (data) {
                $('#Anualleavelist').html(data);

            },
        });
    })
    
});
function AnualSearch() {
    var table = $('#DocumentModalTable3').DataTable({
        "sDom": '<"top"i>rt<"bottom"flp><"clear">'
    });
    $('#tableDiv').find('.dataTables_filter').hide();
    $('#tableDiv').find('.dataTables_info').hide();

    //$('#DocumentModalTable12 tfoot tr').appendTo('#DocumentModalTable thead');

    $("#DocumentModalTable3 thead .SearchStartDate").keyup(function () {
        table.column(1).search(this.value).draw();
    });
    $("#DocumentModalTable3 thead .SearchEndDate").keyup(function () {
        table.column(2).search(this.value).draw();
    });
    $("#DocumentModalTable3 thead .SearchDays").keyup(function () {
        table.column(3).search(this.value).draw();
    });
   
}
function SickSearchDataReject() {
    var table = $('#DocumentModalTable12').DataTable({
        "sDom": '<"top"i>rt<"bottom"flp><"clear">'
    });
    $('#tableDiv').find('.dataTables_filter').hide();
    $('#tableDiv').find('.dataTables_info').hide();

    //$('#DocumentModalTable12 tfoot tr').appendTo('#DocumentModalTable thead');

    $("#DocumentModalTable12 thead .SearchStartDate").keyup(function () {
        table.column(1).search(this.value).draw();
    });
    $("#DocumentModalTable12 thead .SearchEndDate").keyup(function () {
        table.column(2).search(this.value).draw();
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
}



$("#SickLeaveApprovebtn").button().click(function () {
    debugger;
    var ids = $("#DocumentModalTable12 tr:has(input:checked)").map(function () {
        var $tr = $(this);
        var id = $tr.find("td:first").text();
        return id;
    }).toArray();
    var Id = ids.join(", ");
    $.post(constantApproval.UpdateSickLeaveApproval, { "ID": Id },
            function (json) {
                var hidden_fields = $("#sickLeaveWorkerid").val();
                $.ajax({
                    type: 'POST',
                    url: constantApproval.SickLeaveApprove,
                    data: {
                        EmpID: hidden_fields
                    },
                    success: function (data) {
                        $('#sickApproval').html(data);
                        SickSearchDataReject();
                    },
                });
            })
});
$("#page_content_inner3").on('click', '.btn-Refresh-Document', function () {
    var ID = $('#AnnulLeaveWorkerId').val();
    $('#page_content_inner3').load(constantApproval.AnualLeaveApprove, { "EmpID": ID });
});
$("#page_content_inner3").on('click', '.btn-ClearSorting-Document', function () {
    var ID = $('#AnnulLeaveWorkerId').val();
    $('#page_content_inner3').load(constantApproval.AnualLeaveApprove, { "EmpID": ID });
});
$("#page_content_inner3").on('click', '.btn-clearFilter-Document', function () {
    var ID = $('#AnnulLeaveWorkerId').val();
    $('#page_content_inner3').load(constantApproval.AnualLeaveApprove, { "EmpID": ID });
});
$("#OtherLeaveApprove").button().click(function () {
    var ids = $("#DocumentModalTable12 tr:has(input:checked)").map(function () {
        var $tr = $(this);
        var id = $tr.find("td:first").text();
        return id;
    }).toArray();
    var Id = ids.join(", ");
    $.post(constantApproval.UpdateOtherLeaveApprova, { "ID": Id },
            function (json) {
                var hidden_fields = $("#OtherLeaveWorkerid").val();
                $.ajax({
                    type: 'POST',
                    url: constantApproval.OtherLeaveApprove,
                    data: {
                        EmpID: hidden_fields
                    },
                    success: function (data) {
                        $('#otherLeaveApproval').html(data);
                    },
                });
            })
});
$('#otherLeaveApproval').find('#DocumentModalTable12').find("#OtherLeaveReject").button().click(function () {
    var ids = $("#DocumentModalTable12 tr:has(input:checked)").map(function () {
        var $tr = $(this);
        var id = $tr.find("td:first").text();
        return id;
    }).toArray();
    var Id = ids.join(", ");
    $.post(constantApproval.UpdateOtherLeaveReject, { "ID": Id },
            function (json) {
                var hidden_fields = $("#OtherLeaveWorkerid").val();
                $.ajax({
                    type: 'POST',
                    url: constantApproval.OtherLeaveApprove,
                    data: {
                        EmpID: hidden_fields
                    },
                    success: function (data) {
                        $('#otherLeaveApproval').html(data);
                    },
                });
            })
    
});

$("#page_content_inner4").on('click', '.btn-Refresh-Document', function () {
    var ID = $('#OtherLeaveWorkerid').val();
    $('#page_content_inner4').load(constantApproval.OtherLeaveApprove, { "EmpID": ID });
});

$("#page_content_inner4").on('click', '.btn-ClearSorting-Document', function () {
    var ID = $('#OtherLeaveWorkerid').val();
    $('#page_content_inner4').load(constantApproval.OtherLeaveApprove, { "EmpID": ID });
});

$("#page_content_inner4").on('click', '.btn-clearFilter-Document', function () {
    var ID = $('#OtherLeaveWorkerid').val();
    $('#page_content_inner4').load(constantApproval.OtherLeaveApprove, { "EmpID": ID });
});


function OtherSearch() {
    var table = $('#DocumentModalTable12').DataTable({
        "sDom": '<"top"i>rt<"bottom"flp><"clear">'
    });
    $('#tableDiv').find('.dataTables_filter').hide();
    $('#tableDiv').find('.dataTables_info').hide();

    //$('#DocumentModalTable12 tfoot tr').appendTo('#DocumentModalTable thead');

    $("#DocumentModalTable12 thead .SearchStartDate").keyup(function () {
        table.column(1).search(this.value).draw();
    });
    $("#DocumentModalTable12 thead .SearchEndDate").keyup(function () {
        table.column(2).search(this.value).draw();
    });
    $("#DocumentModalTable12 thead .SearchDuration").keyup(function () {
        table.column(3).search(this.value).draw();
    });
    $("#DocumentModalTable12 thead .SearchReason").keyup(function () {
        table.column(4).search(this.value).draw();
    });
}
$("#TrainingApprove").button().click(function () {

    var ids = $("#DocumentModalTable13 tr:has(input:checked)").map(function () {
        var $tr = $(this);
        var id = $tr.find("td:first").text();
        return id;
    }).toArray();
    var Id = ids.join(", ");
    $.post(constantApproval.UpdateTrainingApprova, { "ID": Id },
    function (json) {
        var hidden_fields = $("#TainingWorkerId").val();
        $.ajax({
            type: 'POST',
            url: constantApproval.TrainingRequestApprove,
            data: {
                EmpID: hidden_fields
            },
            success: function (data) {
                $('#trainingReqApproval').html(data);
                TrainingSearch();
            }
        });
    })
    
});
$("#TrainingReject").button().click(function () {

    var ids = $("#DocumentModalTable13 tr:has(input:checked)").map(function () {
        var $tr = $(this);
        var id = $tr.find("td:first").text();
        return id;
    }).toArray();
    var Id = ids.join(", ");
    $.post(constantApproval.UpdateTrainingReject, { "ID": Id },
    function (json) {
        var hidden_fields = $("#TainingWorkerId").val();
        $.ajax({
            type: 'POST',
            url: constantApproval.TrainingRequestApprove,
            data: {
                EmpID: hidden_fields
            },
            success: function (data) {
                $('#trainingReqApproval').html(data);
                TrainingSearch();
            },
        });
    })
   
});
function TrainingSearch() {
    var table = $('#DocumentModalTable13').DataTable({
        "sDom": '<"top"i>rt<"bottom"flp><"clear">'
    });
    $('#tableDiv').find('.dataTables_filter').hide();
    $('#tableDiv').find('.dataTables_info').hide();

    //$('#DocumentModalTable13 tfoot tr').appendTo('#DocumentModalTable thead');

    $("#DocumentModalTable13 thead .SearchTrainingName").keyup(function () {
        table.column(1).search(this.value).draw();
    });
    $("#DocumentModalTable13 thead .SearchImportance").keyup(function () {
        table.column(2).search(this.value).draw();
    });
    $("#DocumentModalTable13 thead .SearchStartDate").keyup(function () {
        table.column(3).search(this.value).draw();
    });
    $("#DocumentModalTable13 thead .SearchEndDate").keyup(function () {
        table.column(4).search(this.value).draw();
    });
}

$("#page_content_inner5").on('click', '.btn-Refresh-Document', function () {
    var ID = $('#TainingWorkerId').val();
    $('#page_content_inner5').load(constantApproval.TrainingRequestApprove, { "EmpID": ID });
});

$("#page_content_inner5").on('click', '.btn-ClearSorting-Document', function () {
    var ID = $('#TainingWorkerId').val();
    $('#page_content_inner5').load(constantApproval.TrainingRequestApprove, { "EmpID": ID });
});

$("#page_content_inner5").on('click', '.btn-clearFilter-Document', function () {
    var ID = $('#TainingWorkerId').val();
    $('#page_content_inner5').load(constantApproval.TrainingRequestApprove, { "EmpID": ID });
});


$("#NewVacancyApprove").button().click(function () {

    var ids = $("#DocumentModalTable14 tr:has(input:checked)").map(function () {
        var $tr = $(this);
        var id = $tr.find("td:first").text();
        return id;
    }).toArray();
    var Id = ids.join(", ");
    $.post(constantApproval.UpdateNewVacancyApprova, { "ID": Id },
            function (json) {
                var hidden_fields = $('#UserId').val();
                $.ajax({
                    type: 'POST',
                    url: constantApproval.NewVacancyApprove,
                    data: {
                        EmpID: hidden_fields
                    },
                    success: function (data) {
                        $('#newVacancyApproval').html(data);
                        NewVacancySearch();
                    },
                });
            })
   
});
$("#NewVacancyReject").button().click(function () {

    var ids = $("#DocumentModalTable14 tr:has(input:checked)").map(function () {
        var $tr = $(this);
        var id = $tr.find("td:first").text();
        return id;
    }).toArray();
    var Id = ids.join(", ");
    $.post(constantApproval.UpdateNewVacancyReject, { "ID": Id },
            function (json) {
                var hidden_fields = $('#UserId').val();
                $.ajax({
                    type: 'POST',
                    url: constantApproval.NewVacancyApprove,
                    data: {
                        EmpID: hidden_fields
                    },
                    success: function (data) {
                        $('#newVacancyApproval').html(data);
                        NewVacancySearch();
                    },
                });
            })
    
});
function NewVacancySearch() {
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

$("#page_content_inner6").on('click', '.btn-Refresh-Document', function () {
    var ID = $('#UserId').val();
    $('#page_content_inner6').load(constantApproval.NewVacancyApprove, { "EmpID": ID });
});

$("#page_content_inner6").on('click', '.btn-ClearSorting-Document', function () {
    var ID = $('#UserId').val();
    $('#page_content_inner6').load(constantApproval.NewVacancyApprove, { "EmpID": ID });
});

$("#page_content_inner6").on('click', '.btn-clearFilter-Document', function () {
    var ID = $('#UserId').val();
    $('#page_content_inner6').load(constantApproval.NewVacancyApprove, { "EmpID": ID });
});

$("#Upliftapprove").button().click(function () {
    var ids = $("#DocumentModalTable11 tr:has(input:checked)").map(function () {
        var $tr = $(this);
        var id = $tr.find("td:first").text();
        return id;
    }).toArray();
    var Id = ids.join(", ");
    $.post(constantApproval.UpdateUpliftApprova, { "ID": Id },
    function (json) {
        var hidden_fields = $("#UpliftWorkerId").val();
        $.ajax({
            type: 'POST',
            url: constantApproval.UpliftApprove,
            data: {
                EmpID: hidden_fields
            },
            success: function (data) {
                $('#upliftApproval').html(data);
                UpliftSearch();
            },
        });
    })
   
});
$("#UpliftReject").button().click(function () {
    var ids = $("#DocumentModalTable11 tr:has(input:checked)").map(function () {
        var $tr = $(this);
        var id = $tr.find("td:first").text();
        return id;
    }).toArray();
    var Id = ids.join(", ");
    $.post(constantApproval.UpdateUpliftReject, { "ID": Id },
    function (json) {
        var hidden_fields = $("#UpliftWorkerId").val();
        $.ajax({
            type: 'POST',
            url: constantApproval.UpliftApprove,
            data: {
                EmpID: hidden_fields
            },
            success: function (data) {
                $('#upliftApproval').html(data);
                UpliftSearch();
            },
        });
    })
});
function UpliftSearch() {
    var table = $('#DocumentModalTable11').DataTable({
        "sDom": '<"top"i>rt<"bottom"flp><"clear">'
    });

    $('#tableDiv').find('.dataTables_filter').hide();
    $('#tableDiv').find('.dataTables_info').hide();

    // $('#DocumentModalTable tfoot tr').appendTo('#DocumentModalTable thead');

    $("#DocumentModalTable11 thead .SearchDay").keyup(function () {
        table.column(1).search(this.value).draw();
    });
    $("#DocumentModalTable11 thead .SearchDate").keyup(function () {
        table.column(2).search(this.value).draw();
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

}
$("#page_content_inner7").on('click', '.btn-Refresh-Document', function () {
    var ID = $('#UpliftWorkerId').val();
    $('#page_content_inner7').load(constantApproval.UpliftApprove, { "EmpID": ID });
});
 
$("#page_content_inner7").on('click', '.btn-ClearSorting-Document', function () {
    var ID = $('#UpliftWorkerId').val();
    $('#page_content_inner7').load(constantApproval.UpliftApprove, { "EmpID": ID });
});

$("#page_content_inner7").on('click', '.btn-clearFilter-Document', function () {
    var ID = $('#UpliftWorkerId').val();
    $('#page_content_inner7').load(constantApproval.UpliftApprove, { "EmpID": ID });
});

