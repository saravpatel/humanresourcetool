
$(document).ready(function () {
   // PerformanceOverAllDataTableDesign();
});

$('#page_content_inner').on('click', '.dataTr', function () {
    $.ajax({
        url: constantSet.GetReviewUrl,
        data: {},
        success: function (data) {            
            $("#ReviewList").html('');
            $("#ReviewList").html(data);
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
        }
    });
});

function getDataByPerformanceReview()
{
    var ReviewId = $("#drpReviewList").val(1);
    $.ajax({
        url: AdminPerformance.GetDataByReview,
        data: { ReviewId: ReviewId },
        success: function (data) {
            $("#OpenReview").html(data.NumberofReviewsOpenthisYear);
            $("#CompletedReview").html(data.NumberofCompletedReviewthisYear);
            $("#OutStandingReview").html(data.OutstandingReview);
            $("#ManagerOutStandingReview").html(data.ManagerOutstanding);
            $("#CustomerOutStandingReview").html(data.CustomerOutstanding);
            $("#WorkerOutStandingReview").html(data.WorkerOutstanding);
            $("#CountTotalQuesByReview").html(data.CountTotalQuestionByReview);
            $("#AllYearOpenReview").html(data.NumberofOpenReviewAllYear);
            $("#AllYearCompletedReview").html(data.NumberofCompletedReviewAllYear);
            $("#TotalNumberOfQuestionDiv").show();
            getOutStandingGraphByReviewId();
        }
    });
}
function getOutStandingGraphByReviewId()
{
    var ReviewId = $("#drpReviewList").val();
    $.ajax({
        url: AdminPerformanceGraph.GetOutstandingGraph,
        data: { ReviewId: ReviewId },
        success: function (data) {
            var chart = {
                plotBackgroundColor: null,
                plotBorderWidth: 0,
                plotShadow: false
            };
            var title = {
                text: 'Outstanding<br>Reviews',
                align: 'center',
                verticalAlign: 'middle',
                y: 30
            };
            var tooltip = {
                pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
            };
            var plotOptions = {
                pie: {
                    dataLabels: {
                        enabled: true,
                        distance: -50,

                        style: {
                            fontWeight: 'bold',
                            color: 'white',
                            textShadow: '0px 1px 2px black'
                        }
                    },
                    startAngle: -90,
                    endAngle: 90,
                    center: ['50%', '75%']
                }
            };
            var series = [{
                type: 'pie',
                name: 'Browser share',
                innerSize: '50%',
                data: [
                   ['Customer', data.CustomerOutstanding],
                   ['Worker', data.WorkerOutstanding],
                   ['Manager', data.ManagerOutstanding],
                   //{
                   //    name: 'Others',
                   //    y: 0.7,
                   //    dataLabels: {
                   //        enabled: false
                   //    }
                   //}
                ]
            }];

            var json = {};
            json.chart = chart;
            json.title = title;
            json.tooltip = tooltip;
            json.series = series;
            json.plotOptions = plotOptions;
            $('#container').highcharts(json);
        }
    });
}
$("#PerformanceTabPanel").on('click', '.btn-OverallScoreSection', function () {    
    $(".hrtoolLoader").show();
    $.ajax({
        url: AdminPerformance.OverallScore,
        success: function (data) {
            $("#contantBody").html('');
            $('#contantBody').html(data);
            $(".PerformanceIndex, .CoreStrengthsSection, .PerformanceReviewChart").removeClass('active');
            $(".OverallScoreSection").addClass('active');
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
        }
    });
});



$("#PerformanceTabPanel").on('click', '.btn-CoreStrengthsSection', function () {
    $.ajax({
        url:AdminPerformance.CoreScoreList,
        success:function(data)
        {
            $("#contantBody").html('');
            $('#contantBody').html(data);
            $(".PerformanceIndex, .OverallScoreSection, .PerformanceReviewChart").removeClass('active');
           $(".CoreStrengthsSection").addClass('active');
          //  $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
        }
    })
})
function ApplyFilterOnPerformanceOverAllList()
{
    var ReviewId = $("#drpReviewList").val();
    var JobtitleId = $("#drpJobtitleList").val();
    var ManagerId = $("#drpManagerList").val();
    var PoolId = $("#drpPoolList").val();
    var FilterValue = $("#drpFilterList").val();
    $.ajax({
        url: AdminPerformance.OverallScoreByFilter,
        data: { ReviewId: ReviewId, JobtitleId: JobtitleId, ManagerId: ManagerId, PoolId: PoolId, FilterValue: FilterValue },
        success: function (data) {
            $("#contantBody").html('');
            $('#contantBody').html(data);
            $(".OverallScoreSection, .CoreStrengthsSection, .PerformanceReviewChart").removeClass('active');
            $(".PerformanceIndex").addClass('active');
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
        }
    });
}

function ClearOverAllFilter()
{
    $("#drpReviewList").val(0);
    $("#drpJobtitleList").val(0);
    $("#drpManagerList").val(0);
    $("#drpPoolList").val(0);
    $("#drpFilterList").val(0);
    $.ajax({
        url: AdminPerformance.OverallScore,
        success: function (data) {
            $("#contantBody").html('');
            $('#contantBody').html(data);
            $(".PerformanceIndex, .CoreStrengthsSection, .PerformanceReviewChart").removeClass('active');
            $(".OverallScoreSection").addClass('active');
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
        }
    });
}
//CoreScoreList

function ApplyFilterOnPerformanceCoreList()
{
    var ReviewId = $("#drpReviewList").val();
    var JobtitleId = $("#drpJobTitleList").val();
    var PoolId = $("#drpPoolList").val();
    var ManagerId = $("#drpManagerList").val();
    var FilterValue = $("#drpFilterList").val();
    $.ajax({
        url: AdminPerformance.CoreScoreByFilter,
        data: { ReviewId: ReviewId, JobtitleId: JobtitleId, ManagerId: ManagerId, PoolId: PoolId, FilterValue: FilterValue },
        success: function (data) {
            $("#contantBody").html('');
            $('#contantBody').html(data);
            $(".OverallScoreSection, .CoreStrengthsSection, .PerformanceReviewChart").removeClass('active');
            $(".PerformanceIndex").addClass('active');
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
        }
    });
}

function ClearFilterCoreList()
{
    $("#drpReviewList").val(0);
    $("#drpJobTitleList").val(0);
    $("#drpPoolList").val(0);
    $("#drpManagerList").val(0);
    $.ajax({
        url: AdminPerformance.CoreScoreList,
        success: function (data) {
            debugger;
            $("#contantBody").html('');
            $('#contantBody').html(data);
            $(".PerformanceIndex, .OverallScoreSection, .PerformanceReviewChart").removeClass('active');
            $(".CoreStrengthsSection").addClass('active');
            //  $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
        }
    })

}
//Graph Tab

$("#PerformanceTabPanel").on('click', '.btn-PerformanceReviewChart', function () {
    $(".hrtoolLoader").show();
    $.ajax({
        url: AdminPerformance.EmployeePerformanceCompare,
        success: function (data) {
            $("#contantBody").html('');
            $('#contantBody').html(data);
            $(".PerformanceIndex, .CoreStrengthsSection, .OverallScoreSection").removeClass('active');
            $(".PerformanceReviewChart").addClass('active');
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
        }
    });    
});
function GetDivisonByBusinessId(Buisenessid) {
    BusId = Buisenessid.value;
    $.ajax({
        url: AdminPerformance.GetDivisonByBusinessID,
        //type: 'POST',
        data: { BusinessID: BusId },
        //contentType: "application/json",
        success: function (data) {
            var toAppend = '<option value="0">-- Select --</option>';
            $.each(data, function (index, item) {
                toAppend += "<option value='" + item.Key + "'>" + item.Value + "</option>";
            });
            $('#drpDivision').html('');
            $('#drpFunction').html('');
            $('#drpPool').html('');
            $('#drpDivision').html(toAppend);
        }
    });
}
function GetPoolByDivisonID(DivisonID) {
    DivId = DivisonID.value;
    $.ajax({
        url: AdminPerformance.GetPoolByDivisonID,
        //type: 'POST',
        data: { BusinessId: BusId, DivisonID: DivisonID.value },
        //contentType: "application/json",
        success: function (data) {
            var toAppend = '<option value="0">-- Select --</option>';
            $.each(data, function (index, item) {
                toAppend += "<option value='" + item.Key + "'>" + item.Value + "</option>";
            });
            $('#drpFunction').html('');
            $('#drpPool').html('');
            $('#drpPool').html(toAppend);
        }
    });
}
function GetFunctionByPoolId(PoolID) {
    PoolId = PoolID.value;
    $.ajax({
        url: AdminPerformance.GetFunctionByPoolId,
        data: { BusinessId: BusId, DivisonID: DivId },
        success: function (data) {
            var toAppend = '<option value="0">-- Select --</option>';
            $.each(data, function (index, item) {
                toAppend += "<option value='" + item.Key + "'>" + item.Value + "</option>";
            });
            $('#drpFunction').html('');
            $('#drpFunction').html(toAppend);
        }
    });
}

function ApplyFilterOnPerformanceCompareGraph()
{
    var ReviewId = $("#drpReviewList").val();
    var ManagerId = $("#drpEmployeeList").val();
    var BussinessId = $("#drpBussinessList").val();
    var JobTitle = $("#drpJobTitleList").val();
    var DivisionId = $("#drpDivision").val();
    var PoolId = $("#drpPool").val();
    var FunctionId = $("#drpFunction").val();
    var OverallScore = $("#txtOverallScore").val();
    var CoreStrengths = $("#txtCoreStrengths").val();
    var CustomerScore = $("#txtCustomerScore").val();
    var AverageScore = $("#txtAverageScore").val();
    $.ajax({
        url: AdminPerformance.GetPerformanceGraphByFilter,
        data: { ReviewId: ReviewId, ManagerId: ManagerId, BussinessId: BussinessId, JobTitle: JobTitle, DivisionId: DivisionId, PoolId: PoolId, FunctionId: FunctionId, OverallScore: OverallScore, CoreStrengths: CoreStrengths, CustomerScore: CustomerScore, AverageScore: AverageScore },
        success: function (data) {
            var EmpNameList = [];
            var EmpLastScore = [];
            var EmpPrevScore = [];
            $("#contantBody").find("#container").html('');
            $.each(data, function (i) {
                var EmpName = data[i].EmployeeName;
                var ScoreList = data[i].ListOfPerformanceReviewGraph;
                $.each(ScoreList, function (j) {
                    var LastScore = ScoreList[j].LastReview;
                    EmpLastScore.push(LastScore);
                    var PrevScore = ScoreList[j].PerviouseReview;
                    EmpPrevScore.push(PrevScore);
                })
                EmpNameList.push(EmpName);
            });
            Highcharts.chart('container', {
                chart: {
                    type: 'spline'
                },
                title: {
                    text: 'Review'
                },
                subtitle: {
                    text: ''
                },
                xAxis: {
                    categories:
                              $.each(EmpNameList, function (i) {
                                  "'" + EmpNameList[i] + "',"
                              })
                },
                yAxis: {
                    title: {
                        text: 'Score'
                    },
                    labels: {
                        formatter: function () {
                            return this.value;
                        }
                    }
                },
                tooltip: {
                    crosshairs: true,
                    shared: true
                },
                plotOptions: {
                    spline: {
                        marker: {
                            radius: 4,
                            lineColor: '#666666',
                            lineWidth: 1
                        }
                    }
                },
                series: [{
                    name: 'Last Review',
                    marker: {
                        symbol: 'square'
                    },
                    data: $.each(EmpLastScore, function (i) {
                        "'" + EmpLastScore[i] + "',"
                    }),
                    events: {
                        click: function (event) {
                            var EmpName = event.point.category;
                            var EmpSSOId = EmpName.split("-");
                            getEmployeePerformanceReviewFilter(EmpSSOId);
                        }
                    }
                }, {
                    name: 'Previouse Review',
                    marker: {
                        symbol: 'diamond'
                    },
                    data: $.each(EmpPrevScore, function (i) {
                        "'" + EmpPrevScore[i] + "',"
                    }),
                    events: {
                       click: function (event) {
                    var EmpName = event.point.category;
                    var EmpSSOId = EmpName.split("-");
                    getEmployeePerformanceReviewFilter(EmpSSOId);
                }
            }
                }]
            });
        }
    });
}

function GetTopImprovertsEmployeeChart()
{
    var ReviewId = $("#drpReviewList").val();
    var ManagerId = $("#drpEmployeeList").val();
    var BussinessId = $("#drpBussinessList").val();
    var JobTitle = $("#drpJobTitleList").val();
    var DivisionId = $("#drpDivision").val();
    var PoolId = $("#drpPool").val();
    var FunctionId = $("#drpFunction").val();
    var OverallScore = $("#txtOverallScore").val();
    var CoreStrengths = $("#txtCoreStrengths").val();
    var CustomerScore = $("#txtCustomerScore").val();
    var AverageScore = $("#txtAverageScore").val();
    $.ajax({
        url: AdminPerformance.GetTopImproversePerGraphByFilter,
        data: { ReviewId: ReviewId, ManagerId: ManagerId, BussinessId: BussinessId, JobTitle: JobTitle, DivisionId: DivisionId, PoolId: PoolId, FunctionId: FunctionId, OverallScore: OverallScore, CoreStrengths: CoreStrengths, CustomerScore: CustomerScore, AverageScore: AverageScore },
        success: function (data) {
            var EmpNameList = [];
            var DiffEmpScoreList = [];
            $("#contantBody").find("#container").html('');
            $.each(data, function (i) {
                var EmpName = data[i].EmployeeName;
                EmpNameList.push(EmpName);
                var DiffScoreList = data[i].DiffOfPerfReview;
                DiffEmpScoreList.push(DiffScoreList);
            });
            Highcharts.chart('container', {
                chart: {
                    type: 'spline'
                },
                title: {
                    text: 'Top Improvers Review Graph'
                },
                subtitle: {
                    text: ''
                },
                xAxis: {
                    categories:
                              $.each(EmpNameList, function (i) {
                                  "'" + EmpNameList[i] + "',"
                              })
                },
                yAxis: {
                    title: {
                        text: 'Score'
                    },
                    labels: {
                        formatter: function () {
                            return this.value;
                        }
                    }
                },
                tooltip: {
                    crosshairs: true,
                    shared: true
                },
                plotOptions: {
                    spline: {
                        marker: {
                            radius: 4,
                            lineColor: '#666666',
                            lineWidth: 1
                        }
                    }
                },
                series: [{
                    name: 'Top Improvers Review',
                    marker: {
                        symbol: 'square'
                    },
                    data: $.each(DiffEmpScoreList, function (i) {
                        "'" + DiffEmpScoreList[i] + "',"
                    }),
                    events: {
                        click: function (event) {
                            var EmpName = event.point.category;
                            var EmpSSOId = EmpName.split("-");
                            getEmployeePerformanceReviewFilter(EmpSSOId);
                        }
                    }
                }]
            });
        }
    });
}

function GetBigestDropEmployeeChart()
{
    var ReviewId = $("#drpReviewList").val();
    var ManagerId = $("#drpEmployeeList").val();
    var BussinessId = $("#drpBussinessList").val();
    var JobTitle = $("#drpJobTitleList").val();
    var DivisionId = $("#drpDivision").val();
    var PoolId = $("#drpPool").val();
    var FunctionId = $("#drpFunction").val();
    var OverallScore = $("#txtOverallScore").val();
    var CoreStrengths = $("#txtCoreStrengths").val();
    var CustomerScore = $("#txtCustomerScore").val();
    var AverageScore = $("#txtAverageScore").val();
    $.ajax({
        url: AdminPerformance.GetBiggestDropPerGraphByFilter,
        data: { ReviewId: ReviewId, ManagerId: ManagerId, BussinessId: BussinessId, JobTitle: JobTitle, DivisionId: DivisionId, PoolId: PoolId, FunctionId: FunctionId, OverallScore: OverallScore, CoreStrengths: CoreStrengths, CustomerScore: CustomerScore, AverageScore: AverageScore },
        success: function (data) {
            var EmpNameList = [];
            var DiffEmpScoreList = [];
            $("#contantBody").find("#container").html('');
            $.each(data, function (i) {
                var EmpName = data[i].EmployeeName;
                EmpNameList.push(EmpName);
                var DiffScoreList = data[i].DiffOfPerfReview;
                DiffEmpScoreList.push(DiffScoreList);
            });
            Highcharts.chart('container', {
                chart: {
                    type: 'spline'
                },
                title: {
                    text: 'Biggest Drops Review Graph'
                },
                subtitle: {
                    text: ''
                },
                xAxis: {
                    categories:
                              $.each(EmpNameList, function (i) {
                                  "'" + EmpNameList[i] + "',"
                              })
                },
                yAxis: {
                    title: {
                        text: 'Score'
                    },
                    labels: {
                        formatter: function () {
                            return this.value;
                        }
                    }
                },
                tooltip: {
                    crosshairs: true,
                    shared: true
                },
                plotOptions: {
                    spline: {
                        marker: {
                            radius: 4,
                            lineColor: '#666666',
                            lineWidth: 1
                        }
                    }
                },
                series: [{
                    name: 'Biggest Drops Review',
                    marker: {
                        symbol: 'square'
                    },
                    data: $.each(DiffEmpScoreList, function (i) {
                        "'" + DiffEmpScoreList[i] + "',"
                    }),
                    events: {
                        click: function (event) {
                            var EmpName = event.point.category;
                            var EmpSSOId = EmpName.split("-");
                            getEmployeePerformanceReviewFilter(EmpSSOId);
                        }
                    }
                }]
            });
        }
    });

}

function GetMostConsistanceEmployeeChart()
{
    var ReviewId = $("#drpReviewList").val();
    var ManagerId = $("#drpEmployeeList").val();
    var BussinessId = $("#drpBussinessList").val();
    var JobTitle = $("#drpJobTitleList").val();
    var DivisionId = $("#drpDivision").val();
    var PoolId = $("#drpPool").val();
    var FunctionId = $("#drpFunction").val();
    var OverallScore = $("#txtOverallScore").val();
    var CoreStrengths = $("#txtCoreStrengths").val();
    var CustomerScore = $("#txtCustomerScore").val();
    var AverageScore = $("#txtAverageScore").val();
    $.ajax({
        url: AdminPerformance.GetMostConsistantEmployeeByFilter,
        data: { ReviewId: ReviewId, ManagerId: ManagerId, BussinessId: BussinessId, JobTitle: JobTitle, DivisionId: DivisionId, PoolId: PoolId, FunctionId: FunctionId, OverallScore: OverallScore, CoreStrengths: CoreStrengths, CustomerScore: CustomerScore, AverageScore: AverageScore },
        success: function (data) {
            var EmpNameList = [];
            var DiffEmpScoreList = [];
            var MostConsisPerf = [];
            $("#contantBody").find("#container").html('');
            $.each(data, function (i) {
                var EmpName = data[i].EmployeeName;
                EmpNameList.push(EmpName);                
                var MostConsistaValue = data[i].MostConsistanceValue;
                MostConsisPerf.push(MostConsistaValue);
            });
            Highcharts.chart('container', {
                chart: {
                    type: 'spline'
                },
                title: {
                    text: 'Most Consistents Review Grpah'
                },
                subtitle: {
                    text: ''
                },
                xAxis: {
                    categories:
                              $.each(EmpNameList, function (i) {
                                  "'" + EmpNameList[i] + "',"
                              })
                },
                yAxis: {
                    title: {
                        text: 'Score'
                    },
                    labels: {
                        formatter: function () {
                            return this.value;
                        }
                    }
                },
                tooltip: {
                    crosshairs: true,
                    shared: true
                },
                plotOptions: {
                    spline: {
                        marker: {
                            radius: 4,
                            lineColor: '#666666',
                            lineWidth: 1
                        }
                    }
                },
                series: [{
                    name: 'Most Consistent',
                    marker: {
                        symbol: 'square'
                    },
                    data: $.each(MostConsisPerf, function (i) {
                        "'" + MostConsisPerf[i] + "',"
                    }),
                    events: {
                        click: function (event) {
                            var EmpName = event.point.category;
                            var EmpSSOId = EmpName.split("-");
                            getEmployeePerformanceReviewFilter(EmpSSOId);
                        }
                    }
                }]
            });
        }
    });
}
function AllEmployeePerformance()
{
    var EmpNameList = [];
    var EmpLastScore = [];
    var EmpPrevScore = [];
    var EmployeeId = [];
    var EmplImage = [];
    $.ajax({
        url: AdminPerformance.GetAllEmployeePerformanceGraph,
        success: function (data) {
            $.each(data, function (i) {
                var EmpName = data[i].EmployeeName;
                var EmpId = data[i].EmpID;
                var EmpImage = data[i].EmployeeImage;
                EmpNameList.push(EmpName);
                EmployeeId.push(EmpId);
                EmplImage.push($("<img>").attr("src", data[i].EmployeeImage));
                var ScoreList = data[i].ListOfPerformanceReviewGraph;
                $.each(ScoreList, function (j) {
                    var LastScore = ScoreList[j].LastReview;
                    EmpLastScore.push(LastScore);
                    var PrevScore = ScoreList[j].PerviouseReview;
                    EmpPrevScore.push(PrevScore);
                })
            });
            var chart = new Highcharts.chart('container', {
                chart: {
                    type: 'spline'
                },
                title: {
                    text: ' Performance Review'
                },
                subtitle: {
                    text: ''
                },
                xAxis: {
                    categories:
                              $.each(EmpNameList, function (i) {
                                  "'" + EmpNameList[i] + "',"
                              })
                },
                yAxis: {
                    title: {
                        text: 'Score'
                    },
                    labels: {
                        formatter: function () {
                            return this.value;
                        }
                    }
                },
                tooltip: {
                    crosshairs: true,
                    shared: true,
                },
                plotOptions: {
                    spline: {
                        marker: {
                            radius: 4,
                            lineColor: '#666666',
                            lineWidth: 1
                        }
                    }
                },
                series: [{
                    name: 'Last Review',
                    marker: {
                        symbol: 'square'
                    },
                    data: $.each(EmpLastScore, function (i) {
                        "'" + EmpLastScore[i] + "',"
                    }),
                    events: {
                        click: function (event) {
                            var EmpName = event.point.category;
                            var EmpSSOId = EmpName.split("-");
                            getEmployeePerformanceReviewFilter(EmpSSOId);
                        }
                    }
                }, {
                    name: 'Previouse Review',
                    marker: {
                        symbol: 'diamond'
                    },
                    data: $.each(EmpPrevScore, function (i) {
                        "'" + EmpPrevScore[i] + "',"
                    }),
                    events: {
                    click: function (event) {
                       var EmpName = event.point.category;
                       var EmpSSOId = EmpName.split("-");
                       getEmployeePerformanceReviewFilter(EmpSSOId);
            }
        }
                }]
            });
            
        }
    });
}

function ClearFilterCompareGraph()
{
    $("#drpReviewList").val(0);
    $("#drpEmployeeList").val(0);
    $("#drpBussinessList").val(0);
    $("#drpJobTitleList").val(0);
    $("#drpDivision").val('');
    $("#drpPool").val('');
    $("#drpFunction").val('');
    $("#txtOverallScore").val('');
    $("#txtCoreStrengths").val('');
    $("#txtCustomerScore").val('');
    ApplyFilterOnPerformanceCompareGraph();
}

function GeneratePDF()
{
    html2canvas(document.querySelector("#container")).then(canvas => {
        //document.body.appendChild(canvas)
        var dataURL = canvas.toDataURL();
        var pdf = new jsPDF('landscape');
        pdf.setLineWidth(1);
        pdf.addImage(dataURL, 'JPEG', 0, 0);
        var blob = pdf.output("blob");
        window.open(URL.createObjectURL(blob))
    });
}

function getEmployeePerformanceReviewFilter(EmpSSOId) {
    var ssoId = EmpSSOId[1];
    $.ajax({
        url: AdminPerformance.GetEmployeeAllPerformanceFilter,
        data: { ssoId: ssoId },
        success: function (data) {
            var ReviewList = [];
            var ScoreList = [];
            $("#contantBody").find("#container").html('');
            $.each(data, function (i) {
                var ReviewName = data[i].ReviewName;
                ReviewList.push(ReviewName);
                var ScoreValue = data[i].ReviewScore;
                ScoreList.push(ScoreValue);
            });
            Highcharts.chart('container', {
                chart: {
                    type: 'spline'
                },
                title: {
                    text: 'Review'
                },
                subtitle: {
                    text: ''
                },
                xAxis: {
                    categories:
                              $.each(ReviewList, function (i) {
                                  "'" + ReviewList[i] + "',"
                              })
                },
                yAxis: {
                    title: {
                        text: 'Score'
                    },
                    labels: {
                        formatter: function () {
                            return this.value;
                        }
                    }
                },
                tooltip: {
                    crosshairs: true,
                    shared: true
                },
                plotOptions: {
                    spline: {
                        marker: {
                            radius: 4,
                            lineColor: '#666666',
                            lineWidth: 1
                        }
                    }
                },
                series: [{
                    name: 'Score',
                    marker: {
                        symbol: 'square'
                    },
                    data: $.each(ScoreList, function (i) {
                        "'" + ScoreList[i] + "',"
                    }),
                }]
            });
        }
    })
}
