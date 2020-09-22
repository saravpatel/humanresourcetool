
$(document).ready(function () {
    GetCustomerSegmentsDetails(0, 0, 2,2);
})
$("#submitCoworkerDetails").on('click', function () {
    debugger;
    var PerfCoWorkerId = $("#PerfCoWorkerId").val();
    var questionList = [];
    $.each($("#MainCoreSegmentDiv").find(".SegmentsDetails"), function () {
        var CororkerId = $(this).find(".EditMainCustomerSegmet").attr("data-title");
        jsonCompetencySegmentObj = [];
        var id = 0;
        $.each($(this).find(".QuestionDetails"), function () {
            id++;
            var QueId = $(this).find(".Question_ID").val();
            var Score = $(this).find(".Question_Score").val();
            var oneData = {
                QueId: QueId,
                Score: Score
            }
            jsonCompetencySegmentObj.push(oneData);
        });
        var AllCompetencySegmentJson = JSON.stringify(jsonCompetencySegmentObj);       
        var oneData = {
            CororkerId: CororkerId,
            questionData: AllCompetencySegmentJson,          
        }
        questionList.push(oneData);
    });    
    var questionListJSV = JSON.stringify(questionList);
    $.ajax({
        url: constantInviteCoPerformance.UpdateCoWorkerCoreScore,
        data: { PerCoId: PerfCoWorkerId, questionListJSV: questionListJSV },
        success: function (data) {
            debugger;            
            //if (data == 0) {
                $("#MainCoreSegmentDiv").hide();
                $("#ReviewCompleted").show();
            //}           
        }
    })
})
//Customer Segments Details

function GetCustomerSegmentsDetails(Id, QueationType, Flag, IsJobRoleCustomer) {
    var CustomerId = $("#CustomerID").val();    
    var PerReviewId = $("#PerReviewId").val();
    var EmpId = $("#EmpId").val();
    if (IsJobRoleCustomer == 1)
    {
        $('.list-inline li.active').removeClass('active');
        $("#SegmentList_" + Id).closest('li').addClass('active');
    }
    else if (IsJobRoleCustomer == 0)
    {
        $('.list-inline li.active').removeClass('active');
        $("#JobRoleSegmentList_" + Id).closest('li').addClass('active');
    }
    else
    {
        $('.list-inline li.active').removeClass('active');
        $("#overview_Segment").closest('li').addClass('active');        
    }    
    var QstType = JSON.stringify(QueationType);
    var ReviewId=$("#ReviewId").val();
    $.ajax({
        url: getCustomerSegments.GetCustomerSegmentsDetailsById,
        data: { Id: Id, QueationType: QstType, ReviewId: ReviewId, Flag: Flag, IsJobRoleCustomer: IsJobRoleCustomer, CustomerId: CustomerId, PerReviewId: PerReviewId, EmpId: EmpId },
        success: function (data) {
            $("#page_content").find("#TableQueData").html('');
            $("#page_content").find("#TableQueData").html(data);
            if (QueationType != "") {
                getCustomerQuestionData(1, 1, Flag);
            }
        }
    })
}
function getCustomerQuestionData(QueId, IsActivePastFlag, Flag) {
    var EmployeeId = $("#EmpId").val();
    var ReviewId = $("#ReviewId").val();
    var PerReviewId = $("#PerReviewId").val();
    var QuestJSONString = $("#CustomerJSONString").val();
    var CustomerID = $("#CustomerID").val();
    $.ajax({
        url: getCustomerSegments.getCustomerQuestionDataByID,
        data: { QuesId: QueId, ReviewId: ReviewId, EmpId: EmployeeId, QuestionData: QuestJSONString, IsActivePastFlag: IsActivePastFlag, Flag: Flag, CustomerId: CustomerID, PerReviewId: PerReviewId },
        success: function (data) {
            $("#TableQueData").find("#customerQuestionDetails").html('');
            $("#TableQueData").find("#customerQuestionDetails").html(data);
        }
    });
}

function saveCustomerInviteDetails(Flag) {
    var questionList = [];
    var JobRolequestionList = [];
    var OverallScoreString = "";
    var drpOverAllScore = $("#drpOverAllScore option:selected").val();
    var drpCoreScore = $("#drpCoreScore option:selected").val();
    var drpJobRoleScore = $("#drpJobRoleScore option:selected").val();
    var ReviewId = $("#ReviewId").val();
    var PerReviewId = $("#PerReviewId").val();
    var CustomerId = $("#CustomerID").val();
    var EmpId = $("#EmpId").val();
    var IsJobroleCustomer = $("#IsJobroleCustomer").val();
    var FlagValue = $("#FlagValue").val();
    var EmpPerfDetailId = 0;    
    if (Flag == 2) {
        EmpPerfDetailId = $("#CustomerPerfromceDetailID").val();
    }
    else {
        EmpPerfDetailId = $("#CustomerPerfromceDetail").val();
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
    var test = $("#CustomerJSONString").val();
    if (FlagValue == 1) {
        $.each($("#page_content").find("#AllSegmentsList").find(".active"), function () {
            var CororkerId = $(this).attr("data-content");
            var jsonCompetencySegmentObj = [];            
            //$.each($("#page_content").find(".QuestionData"), function () {
                debugger;
                var QueId = $(".EditSegmetId").attr("data-id");
                var Score = $(".QuetionValue").val();
                var Comments = $("#custQuestionComments").val();
                var QueDataList = {
                    QueId: QueId,
                    Score: Score,
                    Comments: Comments
                }
                jsonCompetencySegmentObj.push(QueDataList);
            //});
            var AllCompetencySegmentJson = JSON.stringify(jsonCompetencySegmentObj);
            var oneData = {
                CustoIds: CororkerId,
                QueationType: AllCompetencySegmentJson,
            }
            questionList.push(oneData);
        });     
    }    
    else if (FlagValue == 0) {
        //JobRoleSegment
        $.each($("#page_content").find("#AllSegmentsList").find(".active"), function () {
            var JobRoleId = $(this).attr("data-content");
            var jsonJobRoleSegmentObj = [];
            //$.each($("#page_content").find(".QuestionData"), function () {
                var QueId = $(".EditSegmetId").attr("data-id");
                var Score = $(".QuetionValue").val();
                var Comments=$("#jobroleQuestionComments").val();
                var JobRoleQueDataList = {
                    QueId: QueId,
                    Score: Score,
                    Comments:Comments
                }
                jsonJobRoleSegmentObj.push(JobRoleQueDataList);
            //});
            var AllJobRoleSegmentJson = JSON.stringify(jsonJobRoleSegmentObj);
            var oneDataJobRole = {
                JobRoleIds: JobRoleId,
                QueationType: AllJobRoleSegmentJson,
            }
            JobRolequestionList.push(oneDataJobRole);
        });
    }
    var JSONCustomerSegment = JSON.stringify(questionList);
    var JSONJobRoleSegment = JSON.stringify(JobRolequestionList);
    var Id = $("#PerCusDetailId").val();    
    $.ajax({
        url: saveCusto.saveCustomerInviteDetails,
        data: { OverallScoreString: OverallScoreString, Comments: Comments, PerReviewId: PerReviewId, CustomerId: CustomerId, EmpId: EmpId, JSONCustomerSegment: JSONCustomerSegment, JSONJobRoleSegment: JSONJobRoleSegment, EmpPerfDetailId: EmpPerfDetailId },
        success: function (data) {
              setTimeout(function () { $(".toast-success").hide(); }, 1500);
            $("#EmployeePerfromceDetailID").val(data.IsEmployeeExistReview);
        }
    });
}

