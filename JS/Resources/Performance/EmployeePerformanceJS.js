
function ValidateEmail(email) {
    var expr = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return expr.test(email);
};
function GetEmployeeSegmentsDetails(Id, QueationType, Flag, IsJobRoleCustomer, IsManagerEmployee, IsActivePastFlag, IsManager) {
   
    debugger;
    if ($('#ManagerSeg').hasClass('Manager')) {
        IsManager = 2;
    }
    //if (Flag == 1) {
    //    if ($('li button.performanceoverview').css("display") == "none") {
    //        return false
    //    }
    //}
    //else if (Flag == 0) {
    //    if ($('li button.performanceoverview').css("display") == "none") {
    //        return false
    //    }
    //}
    //var PerReviewId = $("#PerReviewId").val();
    var EmpId = $("#Hiddemn_EmployeeId").val();
    if (IsJobRoleCustomer == 1) {
        $('.list-inline li.active').removeClass('active');
        $("#SegmentList_" + Id).closest('li').addClass('active');
    }
    else if (IsJobRoleCustomer == 0) {
        $('.list-inline li.active').removeClass('active');
        $("#JobRoleSegmentList_" + Id).closest('li').addClass('active');
    }
    else {
        $('.list-inline li.active').removeClass('active');
        $("#overview_Segment").closest('li').addClass('active');
    }
    var QstType = JSON.stringify(QueationType);
    var ReviewId = $("#hdnReview").val();
    var EmpPerfReviewId = $("#hdnEmpPerfReviewId").val();
    $.ajax({
        url: constantEmpPerformance.GetEmployeeSegmentsDetailsById,
        data: { Id: Id, QueationType: QstType, EmpPerfReviewId: EmpPerfReviewId, ReviewId: ReviewId, Flag: Flag, IsJobRoleCustomer: IsJobRoleCustomer, EmpId: EmpId, IsManagerEmployee: IsManagerEmployee, IsActivePastFlag: IsActivePastFlag, IsManager: IsManager },
        //data: { Id: Id, QueationType: QstType, ReviewId: ReviewId, Flag: Flag, IsJobRoleCustomer: IsJobRoleCustomer, EmpId: EmpId, IsManagerEmployee: IsManagerEmployee, IsActivePastFlag: IsActivePastFlag },
        success: function (data) {            
            $("#TableQueData").html('');
            $("#TableQueData").html(data);
            if (QueationType != "") {
                getQuestionData(1, IsActivePastFlag, Flag, IsManager);
            }            
        }
    })
}
function getEmployeeDetails(ReviewId, IsManagerEmployee, IsActivePastFlag, IsManager) {
    debugger
    if (IsManager == undefined) {
        IsManager = 1;
    }
    if (IsManager == 1) {
        $('#EmployeeSeg').addClass('Employee')
        $('#ManagerSeg').removeClass('Manager')
    }
    else if (IsManager == 2) {
        $('#ManagerSeg').addClass('Manager')
        $('#EmployeeSeg').removeClass('Employee')
    }
    
    var EmpId = $("#Hiddemn_EmployeeId").val();
    $.ajax({
        url: constantEmpPerformance.GetEmployeeSegmentData,
        data: { EmpId: EmpId, ReviewId: ReviewId, IsManagerEmployee: IsManagerEmployee, IsActivePastFlag: IsActivePastFlag},
        success: function (data) {
            $("#CoDetailsView").find("#CoworkerDetails").html('');
            GetEmployeeSegmentsDetails(0, 0, 2, 2, IsManagerEmployee, IsActivePastFlag, IsManager);
            $("#CoDetailsView").find("#CoworkerDetails").html(data);
             $("#CoDetailsView").find("#coworkerList").hide();            
            $("#CoDetailsView").find("#CustomerListDiv").show();
       }
    });
}
$(".employeeperformanceform").on("click", ".ActiveReview", function () {
    $(".employeeperformanceform").find(".PastReview").removeClass("active");
    $(".employeeperformanceform").find(".PastReviewDiv").hide();
    $(".employeeperformanceform").find(".ActiveReviewDiv").show();
    $(this).addClass("active");
});
$(".employeeperformanceform").on("click", ".PastReview", function () {
    $(".employeeperformanceform").find(".ActiveReview").removeClass("active");
    $(".employeeperformanceform").find(".PastReviewDiv").show();
    $(".employeeperformanceform").find(".ActiveReviewDiv").hide();
    $(this).addClass("active");
});
$("#ActiveReviewHTML").on("click", ".btn-startnewreview", function () {
    
    var EmpId = $("#Hiddemn_EmployeeId").val();    
    $.ajax({
        url: constantEmpPerformance.CheckEmployeeReviewExist,
        data: { EmployeeId: EmpId },
        success: function (data) {
            
            //if(data.IsEmployeeExistReview==0)
            {
                $.ajax({
                    url: constantEmpPerformance.StartNewReview,
                    data: { EmployeeId: EmpId },
                    success: function (data) {
                        $("#StartNewPerformanceModal").find("#StartNewPerformanceTaskBody").html('');
                        $("#StartNewPerformanceModal").find("#StartNewPerformanceTaskBody").html(data);
                        $("#StartNewPerformanceModal").modal('show');
                     // $("#drpProjectList").select2();
                        $("#CompletionDate").Zebra_DatePicker({
                            showButtonPanel: false,
                            format: 'd-m-Y',
                            onSelect: function () {                    
                                $("#validationmessageCompletionDate").hide();
                            }
                        });            
                    }
                })
            }
            //else
            //{
            //    alert("Already Exist Performance Review");
            //}
        }
    })
    
});
$("#btn-saveReview").on('click', function () {
    
    var isError = false;
    var EmpId = $("#Hiddemn_EmployeeId").val();
    var Completion_Date = $("#CompletionDate").val();
    if (Completion_Date == "") {
        isError = true;
        $("#validationmessageCompletionDate").show();
    }
    var model =
        {
            Id: 0,
            EmployeeId: EmpId,
            ProjectId: $("#drpProjectList").val(),
            ReviewId: $("#drpReviewList").val(),
            CompletionDateTime: Completion_Date
        }
    if (!isError) {
        $.ajax({
            type: "POST",
            data: JSON.stringify(model),
            url: constantEmpPerformance.SaveNewReview,
            contentType: "application/json",
            success: function (data) {
                if (data == false) {
                    alert("Performance Review Already Exist");
                }
                else {
                    $("#ActiveReviewHTML").html('');
                    $("#ActiveReviewHTML").html(data);
                    $("#StartNewPerformanceModal").modal('hide');
                    location.reload();
                }
            }
        })
    }
})
//$("#btn-saveReview").on('click', function () {
//    
//    var isError = false;
//    var EmpId = $("#Hiddemn_EmployeeId").val();
//    var Completion_Date = $("#CompletionDate").val();
//    if (Completion_Date == "")
//    {
//        isError = true;
//        $("#validationmessageCompletionDate").show();
//    }   
//    var model =
//          {
//              Id:0,
//              EmployeeId:EmpId,
//              ProjectId: $("#drpProjectList").val(),
//              ReviewId: $("#drpReviewList").val(),
//              CompletionDate: Completion_Date 
//          }    
//    if(!isError)
//    {
//        $.ajax({
//            type: "POST",
//            data: JSON.stringify(model),
//            url: constantEmpPerformance.SaveNewReview,
//            contentType: "application/json",
//            success: function (data)
//            {
//                if (data == false)
//                {
//                    alert("Performance Review Already Exist");
                   
//                }
//                else
//                {
//                    $("#ActiveReviewHTML").html('');
//                    $("#ActiveReviewHTML").html(data);
//                    $("#StartNewPerformanceModal").modal('hide');
//                }
//            }
//        })
//    }
//})
function openReviewSegments(empPerReviewId,ReviewId,EmpID,IsActivePastFlag)
{
      $.ajax({
          url: constantEmpPerformance.getReviewDetails,
          data: { empPerReviewId: empPerReviewId, ReviewId: ReviewId, EmpID: EmpID, IsActivePastFlag: IsActivePastFlag },
        success: function (data) {
            $(".employeeperformanceform").hide();
            $("#CoDetailsView").html('');
            $("#CoDetailsView").html(data);
            getEmployeeDetails(ReviewId, 0, IsActivePastFlag);
        }
    })
}
function AddInviteToCoworker()
{
    $.ajax({
        url: constantEmpPerformance.InviteCoworkerForFeedback,
        data: {  },
        success: function (data) {            
            $("#InviteCoworkerModel").find("#InviteCoworkerBody").html('');
            $("#InviteCoworkerModel").find("#InviteCoworkerBody").html(data);
        }
    })
}

$("#page_content").on('click', "#btnSendInvite", function () {
    var InviteEmpId = $("#EmployeeList").val();    
    var EmployeeId = $("#Hiddemn_EmployeeId").val();
    var OtherEmpName = $("#EmpNameAndSurname").val();
    var OtherEmpEmail = $("#EmpEmail").val();
    var PerReviewId = $("#hdnReview").val();
    var isError = false;
    if (InviteEmpId == 0 || InviteEmpId == "" || InviteEmpId == null)
    {
        isError = true;
        $("#lbl-error-Emp").show();
    }    
    if (OtherEmpEmail != "") {
     if (ValidateEmail(OtherEmpEmail)) {
       }
     else { 
          $("#ValidEmail").show();
          iserror = true;
         }
    }
    if (!isError) {
        $.ajax({
            url: constantEmpPerformance.SendMailToCoworker,
            data: { EmpId: InviteEmpId, OtherempName: OtherEmpName, OtherempEmail: OtherEmpEmail, PerReviewId: PerReviewId, EmployeeId: EmployeeId },
            success: function (response) {
                if (response.success) {
                    if (response.message == "") {
                        $(".toast-info-invitation").show();
                        setTimeout(function () { $(".toast-info-invitation").hide(); }, 2000);
                        $('.modal-backdrop').hide();
                        $('#InviteCoworkerModel').hide();                                                                                                 
                        getCoworkerDetails(PerReviewId, 0, InviteEmpId, 1);
                        
                    }
                    else {
                        $(".toast-info-invitation-decline").show();
                        setTimeout(function () { $(".toast-info-invitation-decline").hide(); }, 2000);
                    }

                }
                else {
                    alert('Something went wrong. please try after sometime');
                }
                //$("#InviteCoworkerModel").modal('hide');
           }
        })
    }
})
//InviteCustomer
function AddInviteToCustomer()
{
    $.ajax({
        url: constantEmpPerformance.InviteCustomerForFeedback,
        data: {},
        success: function (data) {
            $("#InvitePerCustomerModel").find("#InvitePerCustomerModelBody").html('');
            $("#InvitePerCustomerModel").find("#InvitePerCustomerModelBody").html(data);
        }
    })
}


$("#page_content").on('click', "#btnSendInviteToCustomer", function () {
    var CustId = $("#CustomerList").val();
    var PerReviewId = $("#hdnReview").val();
    var EmpId = $("#Hiddemn_EmployeeId").val();
    var isError = false;
    if (CustId == 0 || CustId == "" || CustId == null) {
        isError = true;
        $("#lbl-error-Cust").show();
    }
    $.ajax({
        url: constantEmpPerformance.SendMailToCustomer,
        data: { CustId: CustId, PerReviewId: PerReviewId, EmpId: EmpId },
        success: function (data) {
            $("#InvitePerCustomerModel").modal('hide');
        }
    })
})


//Employee Segments Details


//Save Employee Performance Details
function saveEmployeePerformanceDetails(Flag, IsActivePastFlag)
{
    var performanceID = 0;
    var questionList = [];
    var JobRolequestionList = [];
    var OverallScoreString = "";
    var drpOverAllScore = $("#drpOverAllScore option:selected").text();
    var drpCoreScore = $("#drpCoreScore option:selected").text();
    var drpJobRoleScore = $("#drpJobRoleScore option:selected").text();
    var ReviewID = $("#hdnReview").val();
    var EmpId = $("#Hiddemn_EmployeeId").val();
    var IsManagerEmployee = $("#IsManagerEmployee").val();
    var IsJobroleCustomer = $("#IsJobroleCustomer").val();
    var EmpPerfDetailId = 0;
    if (drpOverAllScore == "--Select Overall Score--" || drpCoreScore == "--Select CoreScoreList--" || drpJobRoleScore == "--Select JobRoleScoreList--") {
        $(".toast-info-field").show();
        setTimeout(function () { $(".toast-info-field").hide(); }, 1500);
        return false;
    }
    if ($(".overviewperf").hasClass('active') == false) {

        if ($('.validateSeg').val() == 0 || $('.validateSeg').val() == null || $('.validateSeg').val() == "") {
            $(".toast-info-field").text("Please fill first field")
            $(".toast-info-field").show();
            setTimeout(function () { $(".toast-info-field").hide(); }, 1500);
            setTimeout(function () { $(".toast-info-field").text("Please Fill marked fill."); }, 1500);
            return false;
        }
    }   
    if (Flag == 2) {
        EmpPerfDetailId = $("#EmployeePerfromceDetailID").val();
    }
    else {
        EmpPerfDetailId = $("#EmployeePerfromceDetail").val();
    }
    if (EmpPerfDetailId == "" || EmpPerfDetailId == undefined) {
        EmpPerfDetailId = 0;
    }
    if (drpOverAllScore != "" && drpOverAllScore != undefined) {
        OverallScoreString += drpOverAllScore;
    }
    if (drpCoreScore != "" && drpCoreScore != undefined) {
        OverallScoreString += "^" + drpCoreScore;
    }
    if (drpJobRoleScore != "" && drpJobRoleScore != undefined) {
        OverallScoreString += "^" + drpJobRoleScore;
    }
    var Comments = $("#OverviewComments").val();
    var EmpPerReview = $("#EmpPerReview").val();
    var test = $("#CustomerJSONString").val();
    if (IsJobroleCustomer == 1) {
        $.each($("#CoworkerDetails").find("#AllSegmentsList").find(".active"), function () {
            var CororkerId = $(this).attr("data-content");
            var jsonCompetencySegmentObj = [];
            //$.each($("#page_content").find(".QuestionData"), function () {
            var QueId = $(".QuestionData").find(".EditSegmetId").attr("data-id");
            var Score = $(".QuestionData").find(".QuetionValue").val();
            var Comments = $("#coreQuestionComments").val();
            var QueDataList = {
                QueId: QueId,
                Score: Score,
                Comments: Comments
            }
            jsonCompetencySegmentObj.push(QueDataList);
            //});
            var AllCompetencySegmentJson = JSON.stringify(jsonCompetencySegmentObj);
            var oneData = {
                CoreId: CororkerId,
                QueationType: AllCompetencySegmentJson,
            }
            questionList.push(oneData);
        });
        performanceID = $('li.active a.core').attr('coreid');

    }//JobRoleSegment
    else if (IsJobroleCustomer == 0) {
        $.each($("#CoworkerDetails").find("#AllSegmentsList").find(".active"), function () {
            var JobRoleId = $(this).attr("data-content");
            var jsonJobRoleSegmentObj = [];
            //  $.each($("#page_content").find(".QuestionData"), function () {
            var QueId = $(".EditSegmetId").attr("data-id");
            var Score = $(".QuetionValue").val();
            var Comments = $("#jobroleQuestionComments").val();
            var JobRoleQueDataList = {
                QueId: QueId,
                Score: Score,
                Comments: Comments
            }
            jsonJobRoleSegmentObj.push(JobRoleQueDataList);
            // });
            var AllJobRoleSegmentJson = JSON.stringify(jsonJobRoleSegmentObj);
            var oneDataJobRole = {
                JobRoleIds: JobRoleId,
                QueationType: AllJobRoleSegmentJson,
            }
            JobRolequestionList.push(oneDataJobRole);
        });

        performanceID = $('li.active a.jobrole').attr('jobroleid');
        if (performanceID == null || performanceID == undefined) {
            performanceID = 0;
        }

    }
    var JSONCustomerSegment = JSON.stringify(questionList);
    var JSONJobRoleSegment = JSON.stringify(JobRolequestionList);
    var Id = $("#PerCusDetailId").val();

    $.ajax({
        url: constantEmpPerformance.saveEmployeePerformanceDetails,
        data: { PerformanceID: performanceID, OverallScoreString: OverallScoreString, Comments: Comments, EmpId: EmpId, JSONCustomerSegment: JSONCustomerSegment, JSONJobRoleSegment: JSONJobRoleSegment, ReviewID: ReviewID, IsManagerEmployee: IsManagerEmployee, Flag: Flag, IsActivePastFlag: IsActivePastFlag, EmpPerfDetailId: EmpPerfDetailId, EmployeePerformaceID: EmpPerReview },
        success: function (data) {
            $("#ShareReview").show();
            $(".toast-success").show();
            //getQuestionData(QueId, IsActivePastFlag, Flag)
            setTimeout(function () { $(".toast-success").hide(); }, 1500);
            $("#CustomerPerfromceDetailID").val(data.IsEmployeeExistReview);
            $("#EmployeePerfromceDetailID").val(data.IsEmployeeExistReview);
            var IsManager = 1;
            if ($('#ManagerSeg').hasClass('Manager')) {
                IsManager = 2;
            }
            if (Flag == 2) {
                $('.performanceoverview').css('display', 'inline-block')
                return false;
            }
            else if (Flag == 1) {
               
                GetEmployeeSegmentsDetails($('#hdnCoreID_' + performanceID).val(), JSON.parse($('#hdnQueationType_' + performanceID).val()), 1, 1, $('#hdnIsManagerEmployee_' + performanceID).val(), $('#hdnIsActivePastFlag_' + performanceID).val(), IsManager);
            }
            else if (Flag == 0) {
                GetEmployeeSegmentsDetails($('#hdnJobRoleID_' + performanceID).val(), JSON.parse($('#hdnJobRoleQueationType_' + performanceID).val()), 0, 0, $('#hdnJobRoleIsManagerEmployee_' + performanceID).val(), $('#hdnJobRoleIsActivePastFlag_' + performanceID).val(), IsManager);

            }
        }
    });
}

//Share Employee Performance
function ShareEmployeePerformance()
{
    var ManagerId = $("#ManagerId").val();
    var EmployeeId = $("#Employeeid").val();
    $.ajax({
        url: constantEmpPerformance.sendMailtoshareperformnce,
        data: { ManagerId: ManagerId, EmployeeId: EmployeeId },
        success: function (data) {
            $("#ShareemployeePerformnceReviewModal").modal('hide');
            $(".toast-success-mail").show();
            setTimeout(function () { $(".toast-success-mail").hide(); }, 1500);
            $('#btn-SaveCustomerReviewDetails').css('display', 'none')
            $('#ShareReview').css('display', 'none')
            $('#UnShareReview').css('display', '')
        }
    });
}

//UnShare Employee Performance
function UnShareEmployeePerformance() {
    var ManagerId = $("#ManagerId").val();
    var EmployeeId = $("#Employeeid").val();
    $.ajax({
        url: constantEmpPerformance.sendMailtoUnshareperformnce,
        data: { ManagerId: ManagerId, EmployeeId: EmployeeId },
        success: function (data) {
            $("#UnShareemployeePerformnceReviewModal").modal('hide');
            $(".toast-success-unmail").show();
            setTimeout(function () { $(".toast-success-unmail").hide(); }, 1500);
            $('#btn-SaveCustomerReviewDetails').css('display', '')
            $('#ShareReview').css('display', '')
            $('#UnShareReview').css('display', 'none')
        }
    });
}


//Close Employee Review
function CloseEmployeePerReview(ReviewId,ManagerID, IsActivePastFlag)
{
    var PerReviewId = $("#PerReviewId").val();
    var EmployeeId = $("#EmployeeId").val();
    $.ajax({
        url: constantEmpPerformance.SavecloseEmployeePerformanceReview,
        data: { PerReviewId: PerReviewId, EmplyeeId: EmployeeId, ManagerID: ManagerID },
        success: function (data) {
            $("#ClosePerformnceReviewModal").modal('hide');
            location.reload();
        }
    });
}

//Performance question data

function getQuestionData(QueId, IsActivePastFlag, Flag, IsManager)
{
    if ($('#ManagerSeg').hasClass('Manager')) {
        IsManager = 2;
    }
    debugger;
    if (Flag == 1) {
        var coreID = $('li.active a.core').attr('coreid');
    }
    if (Flag == 0) {
        var coreID = $('li.active a.jobrole').attr('jobroleid');
    }
    var EmployeeId = $("#Hiddemn_EmployeeId").val();
    var ReviewId = $("#hdnReview").val();
    var QuestJSONString = $("#QuestJSONString").val();
    var EmpPerReview = $("#EmpPerReview").val();
    $.ajax({
        url: constantEmpPerformance.getQuestionDataByID,
        data: { coreORjobroleID: coreID, QuesId: QueId, ReviewId: ReviewId, EmpId: EmployeeId, QuestionData: QuestJSONString, IsActivePastFlag: IsActivePastFlag, Flag: Flag, EmpPerReview: EmpPerReview, IsManager: IsManager},
        //data: { QuesId: QueId, ReviewId: ReviewId, EmpId: EmployeeId, QuestionData: QuestJSONString, IsActivePastFlag: IsActivePastFlag, Flag: Flag,EmpPerReview:EmpPerReview },
        success: function (data) {
            
            $("#TableQueData").find("#QuestionDetails").html('');
            $("#TableQueData").find("#QuestionDetails").html(data);
            $("#TableQueData").find('#CoreQuestion_1').addClass('btn-warning');
        }
    });
}
function SaveGoalEmployeePerReview(ReviewId, ManagerId, IsActivePastFlag)
{
    var EmpId = $("#Hiddemn_EmployeeId").val();
    var EmpPerReviewID = $("#EmpPerId").val();
    var GoalId = $("#GoalId").val();
    var GoalName= $("#GoalName").val();
    var GoalDescription = $("#GoalDescription").val();
    var isError = false;
    if (GoalName == "")
    {
        isError = true;
        $("#validationmessageforGoal").show();
    }
    if (GoalDescription == "")
    {
        isError = true;
        $("#validationmessageforDescription").show();
    }
    var model = {
     Id: GoalId,
     EmployeeId :EmpId,
     EmpPerformanceId: EmpPerReviewID,
     GoalName:GoalName,
     GoalDescription: GoalDescription,
     DueDate: $("#GoalDueDate").val(),
     UnitPercent: $("#GoalUnitValue").val()
    }
    if (!isError) {
        $.ajax({
            type: "POST",
            data: JSON.stringify(model),
            url: constantEmpPerformance.saveEmployeePerformanceGoal,
            contentType: "application/json",
            success: function (data) {
                $("#GoalPerformnceReviewModal").modal('hide');
                $("#CoDetailsView").find("#CoworkerDetails").html('');
                $("#CoDetailsView").find("#CoworkerDetails").html(data);
            }
        });
    }
}

//Employee Performnce Goal
function EmployeeObjectiveOfPerformance()
{
    var EmpId = $("#Hiddemn_EmployeeId").val();
    var EmpPerReviewID = $("#EmpPerId").val(); 
    $.ajax({        
        data: { Id:0, EmpPerReviewId: EmpPerReviewID, EmpId: EmpId },
        url: constantEmpPerformance.EmployeeObjectiveOfPerformance,        
        success: function (data) {
            $("#GoalPerformnceReviewModal").find("#GoalPerformnceReviewModalBody").html('');
            $("#GoalPerformnceReviewModal").find("#GoalPerformnceReviewModalBody").html(data);
            $("#GoalDueDate").Zebra_DatePicker({
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {                      
                }
            });            
        }
    });
}

//Edit Goal
function EditObjective(Id)
{
    var EmpId = $("#Hiddemn_EmployeeId").val();
    var EmpPerReviewID = $("#EmpPerId").val();
    $.ajax({
        data: { Id:Id, EmpPerReviewId: EmpPerReviewID, EmpId: EmpId },
        url: constantEmpPerformance.EmployeeObjectiveOfPerformance,
        success: function (data) {
            $("#GoalPerformnceReviewModal").find("#GoalPerformnceReviewModalBody").html('');
            $("#GoalPerformnceReviewModal").find("#GoalPerformnceReviewModalBody").html(data);
            $("#GoalDueDate").Zebra_DatePicker({
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {
                }
            });
        }
    });
}

function GetProgressData(Id,EmployeeId,EmpPerformanceId)
{
    $.ajax({
        data: { Id: Id, EmployeeId: EmployeeId, EmpPerformanceId: EmpPerformanceId },
        url: constantEmpPerformance.ViewPerformanceObjectiveProgress,
        success: function (data) {
            
            $("#GoalPerformnceProgressModal").find("#GoalPerformnceProgressModalBody").html('');
            $("#GoalPerformnceProgressModal").find("#GoalPerformnceProgressModalBody").html(data);
        
        }
    });
}

//Save Employee Performance Goal

function SaveGoalEmployeePerProgressDocument(ReviewId,ManagerId,IsActivePastFlag)
{
    var PerGoalId = $("#PerGoalId").val();
    var EmpPerGoalId = $("#EmpPerGoalId").val();
    var Status = $("#drpProgressStatus").val();
    var UnitPercent = $("#UnitPercent").val();
    var EmployeeId = $("#Hiddemn_EmployeeId").val();
    var EmpPerReview = $("#EmpPerReview").val();
    var documentList = [];
    $.each($("#page_content").find("#GoalPerformnceProgressModal").find("#GoalPerformnceProgressModalBody").find('#filesList').find(".ListData"), function () {
        
        var originalName = $(this).find(".fileName").html().trim();
        var newName = $(this).find(".fileName").attr("data-newfilename");
        var oneData = {
            originalName: originalName,
            newName: newName,
        }
        documentList.push(oneData);
    });
    var JsondocumentListJoinString = JSON.stringify(documentList);
    $.ajax({
        data: { jsonDocumentList: JsondocumentListJoinString, PerGoalId: PerGoalId, EmpPerGoalId: EmpPerGoalId, Status: Status, UnitPercent: UnitPercent, EmployeeId: EmployeeId,EmpPerReview:EmpPerReview },
            url: constantEmpPerformance.SavePerfromanceGoalDocument,
            success: function (data) {
                $("#GoalPerformnceProgressModal").modal('hide');
                $("#CoDetailsView").find("#CoworkerDetails").html('');
                $("#CoDetailsView").find("#CoworkerDetails").html(data);
            }
        })    
}

//Goal Comments
function getGoalComment(CmtId,GoalId,EmpPerformanceId,EmployeeId)
{
    $.ajax({
        data: {CmtId:CmtId, GoalId: GoalId, EmpPerformanceId: EmpPerformanceId, EmployeeId: EmployeeId },
        url: constantEmpPerformance.GetGoalComment,
        success: function (data) {
            $("#GoalPerformnceProgressModal").modal('hide');
            $("#GoalPerformnceProgressCommnetModel").find("#GoalPerformnceProgressCommnetModelBody").html('');
            $("#GoalPerformnceProgressCommnetModel").find("#GoalPerformnceProgressCommnetModelBody").html(data);
            $("#GoalPerformnceProgressCommnetModel").find('div#froala-editor-comment').froalaEditor({
                toolbarButtons: ['bold', 'italic', 'underline', 'strikeThrough', 'subscript', 'superscript', 'fontFamily', 'fontSize', '|', 'specialCharacters', 'color', 'emoticons', 'inlineStyle', 'paragraphStyle', '|', 'paragraphFormat', 'align', 'formatOL', 'formatUL', 'outdent', 'indent', '-', 'quote', 'insertHR', 'insertLink', 'insertImage', 'insertVideo', 'insertFile', 'insertTable', '|', 'undo', 'redo', 'clearFormatting', 'selectAll', 'html', 'applyFormat', 'removeFormat', 'fullscreen', 'print', 'help'],
                //toolbarButtons: ['bold', 'italic', 'underline', 'formatUL'],
                pluginsEnabled: null
            });
        }
    })
}


function SaveGoalComment(ReviewId,ManagerId,IsActivePastFlag)
{
    var UnitPercent = $("#NewUnitPercent").val();
    var Comment = $('#GoalPerformnceProgressCommnetModel').find('div#froala-editor-comment').froalaEditor('html.get');    
    var EmpPerformanceId = $("#PerGoalId").val();
    var EmployeeId = $("#EmployeeId").val();
    var GoalId = $("#GoalId").val();
    var ComtId = $("#CommntId").val();
    if (ComtId == "" || ComtId == undefined)
    {
        ComtId = 0;
    }
    $.ajax({
        data: {ComtId:ComtId, GoalId:GoalId,EmpPerformanceId:EmpPerformanceId,EmployeeId:EmployeeId,Comment:Comment,UnitPercent:UnitPercent },
        url: constantEmpPerformance.SaveGoalComment,
        success: function (data) {            
            $("#GoalPerformnceProgressCommnetModel").modal('hide');
            $("#GoalPerformnceProgressModal").find("#GoalPerformnceProgressModalBody").html('');
            $("#GoalPerformnceProgressModal").find("#GoalPerformnceProgressModalBody").html(data);
            $("#GoalPerformnceProgressModal").modal('show');           
        }
    })
}
function getCommentData(ComntId,GoalId,EmpPerGoalId,EmpId)
{
    getGoalComment(ComntId,GoalId,EmpPerGoalId,EmpId);
}

function FilterByGoalStatus(EmployeeId)
{    
    var StatusOfGoal = $("#drpStatusOfGoal").val();
    var EmpPerId = $("#EmpPerId").val();
    $.ajax({
        data: { StatusOfGoal: StatusOfGoal, EmployeeId: EmployeeId, EmpPerId :EmpPerId},
        url: constantEmpPerformance.FilterByGoalStatus,
        success: function (data) {
            $("#CoDetailsView").find("#CoworkerDetails").html('');
            $("#CoDetailsView").find("#CoworkerDetails").html(data);
            GoalChartData();
            
        }
    })
}
function GoalChartData() {
    var EmpId = $("#Hiddemn_EmployeeId").val();
    var StatusOfGoal = $("#drpStatusOfGoal").val();
    $.ajax({
        data: { EmpId: EmpId, StatusOfGoal: StatusOfGoal },
        url: constantEmpPerformance.GetGoalChart,
        success: function (data) {
            var GoalData = JSON.stringify(data);
            var svg = d3.select("svg"),
            width = +svg.attr("width"),
            height = +svg.attr("height");
            var start_x, start_y;
            var radius = 10;
            var Circle_arr = [];
            var circle_data = $.each(data.EmployeePerformanceGoalList, function (i) {
                var cir = {
                    //x: Math.round(data.EmployeePerformanceGoalList[i].GoalXValue * (width - radius * 2) + radius),
                    //y: Math.round(data.EmployeePerformanceGoalList[i].GoalYValue * (height - radius * 2) + radius),
                    x: Math.round(data.EmployeePerformanceGoalList[i].GoalXValue),
                    y: Math.round(data.EmployeePerformanceGoalList[i].GoalYValue),
                    Id: data.EmployeePerformanceGoalList[i].Id
                };
                Circle_arr.push(cir);
            });
            var rect = svg.append("g")
                 .attr("class", "rect")
                 .append("rect")
                 .attr("width", width)
                 .attr("height", height)
                 .style("fill", "white");
            var circles = d3.select("svg")
                .append("g")
                .attr("class", "circles")
                .selectAll("circle")
                    .data(Circle_arr)
                    .enter()
                    .append("circle")
                    .attr("cx", function (d) { return (d.x) })
                    .attr("cy", function (d) { return (d.y) })
                    .attr("r", radius)
                    .attr("fill", "#3d4957");
            var drag_handler = d3.drag()
                .on("start", drag_start)
                .on("drag", drag_drag).on("end", dragended);


            function drag_start() {
                start_x = +d3.event.x;
                start_y = +d3.event.y;
            }
            function drag_drag(d) {
                if (this.getAttribute("transform") === null) {
                    current_scale = 1;
                }
                else {
                    current_scale_string = this.getAttribute("transform").split(' ')[1];
                    current_scale = +current_scale_string.substring(6, current_scale_string.length - 1);
                }
                d3.select(this)
                  .attr("cx", d.x = start_x + ((d3.event.x - start_x) / current_scale))
                  .attr("cy", d.y = start_y + ((d3.event.y - start_y) / current_scale));
            }
            function dragended(d) {
                var value_x = d.x;
                var value_y = d.y;
                var Id = d.Id;
                $.ajax({
                    url: constantEmpPerformance.UpdateGoalChart,
                    data: { Id: Id, value_x: value_x, value_y: value_y },
                    success: function () {
                    }
                })

            }
            drag_handler(circles);
            var zoom_handler = d3.zoom()
                .on("zoom", zoom_actions);
            function zoom_actions() {
                circles.attr("transform", d3.event.transform);
            }
        }
    })
}

