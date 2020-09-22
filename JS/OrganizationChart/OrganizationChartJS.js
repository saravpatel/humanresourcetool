
$(document).ready(function () {

    var canvas = '';
    drawEmpChart();
    document.getElementById("futurestarter").checked = true;
    drawBusiEmpChart();
});
var Img;
$('#Zoom').change(function () {
    var optVal = $("#Zoom option:selected").val();
    //alert(optVal);
    if (optVal == '120%') {
        $('#empChart table').css({ "zoom": "120%" });
    } else if (optVal == '150%') {
        $('#empChart table').css({ "zoom": "150%" });
    } else if (optVal == '--Select--') {
        $('#empChart table').css({ "zoom": "100%" });
    }

});
$("#FilterId").click(function () {
    $("#drpBusiness").val(0);
    $("#drpDivision").val(0);
    $("#drpPool").val(0);
    $("#drpFunction").val(0);
    $("#drpResourceType").val(0);
    drawBusiEmpChart();

})
$('#LeftId').click(function () {
    $('#empChart table').css({ "float": "left" });
});


$('#RightId').click(function () {
    $('#empChart table').css({ "float": "right" });
});

$('#BottomId').click(function () {
    $('#empChart table').css({ "bottom": "0px", "position": "relative" });
});

$('#TopId').click(function () {
    $('#empChart table').css({ "top": "0px", "position": "relative" });
});
$("#PreviewId").click(function () {
    html2canvas(document.querySelector("#empChart")).then(canvas => {
        //document.body.appendChild(canvas)
        var dataURL = canvas.toDataURL();
        var pdf = new jsPDF('landscape');
        pdf.setLineWidth(1);
        pdf.addImage(dataURL, 'JPEG', 0, 0);
        var blob = pdf.output("blob");
        window.open(URL.createObjectURL(blob))
    });
})
$("#futurestarter").change(function () {
    drawBusiEmpChart();
})
$('#DownloadId').click(function () {
    //canvas = $("#empChart .canvasjs-chart-canvas");
    html2canvas(document.querySelector("#empChart")).then(canvas => {
        //document.body.appendChild(canvas)
        var dataURL = canvas.toDataURL();
        var pdf = new jsPDF('landscape');
        pdf.setLineWidth(1);
        pdf.addImage(dataURL, 'JPEG', 0, 0);
        pdf.save("OrganizationChartPDf.pdf")
    });


  

    // var chartData = 'text';
    //$.ajax({
    //    type: "POST",
    //    url: constantDocument.PrintChartData,
    //    data: { chartData: JSON.stringify(chartData) },
    //    success: function (empData) {

    //    }
    //})
});

google.load("visualization", "1", { packages: ["orgchart"] });
var dataURL;
function drawEmpChart() {
    $.ajax({
        type: "POST",
        url: constantDocument.OrgChartDetails,
        data: '{}',
        // contentType: "application/json; charset=utf-8",
        // dataType: "json",
        success: function (empData) {
            debugger;
            var chartData = new google.visualization.DataTable();
            chartData.addColumn('string', 'Name');
            chartData.addColumn('string', 'ToolTip');
            //chartData.addColumn('string', 'Manager');          
            $.each(empData, function (index, row) {
                // var reportID = row.ReportsTo == "" ? '' : row.ReportsTo;
                chartData.addRows([[{
                    v: row.EmpId.toString(),
                    f: '<div>' + row.Name + '<span></span></div><div>Employed For :' + row.LengthOfEmployeement + '</div><div>(<span>' + row.Value + '</span>)</div><img height="50px" width="50px"/>'
                }, row.ReportsTo]]);
            });
            var chart = new google.visualization.OrgChart($("#empChart")[0]);
            chart.draw(chartData, { allowHtml: true });
            canvas = $("#empChart .canvasjs-chart-canvas").get(0);

            //var chart = new CanvasJS.Chart("empChart", chartData);
            //chart.render();
            html2canvas($('#empChart'), {
                allowHtml: true
            }).then(function (canvas) {
                // document.getElementById("result").appendChild(canvas);
                var dataURL = canvas.toDataURL();
                var pdf = new jsPDF();
                pdf.addImage(dataURL, 'JPEG', 0, 0);
                pdf.save("download.pdf")
            });
        },
        failure: function (xhr, status, error) {
            alert("Failure: " + xhr.responseText);
        },
        error: function (xhr, status, error) {
            alert("Error: " + xhr.responseText);
        }
    });
}

//$("#drpBusiness").change(function () {
//    drawBusiEmpChart();
//});
//$("#drpDivision").change(function () {
//    drawBusiEmpChart();
//});
$("#drpPool").change(function () {
    drawBusiEmpChart();
});
$("#drpFunction").change(function () {
    drawBusiEmpChart();
});
$("#drpResourceType").change(function () {
    drawBusiEmpChart();
})
function drawBusiEmpChart() {
    var BusiID = $("#drpBusiness").val();
    var DivID = $("#drpDivision").val();
    var PoolID = $("#drpPool").val();
    var FunID = $("#drpFunction").val();
    var ResourceType = $("#drpResourceType").val();
    var futureStater = $("#futurestarter").val();
    if ($("#futurestarter").prop("checked") == true) {
        futureStater = "ON";
    }
    else {
        futureStater = "OFF";
    }
    $.ajax({
        type: "POST",
        url: constantDocument.BusiOrgChartDetails,
        data: { BusiID: BusiID, DivID: DivID, PoolID: PoolID, FunID: FunID, EmpTypeId: ResourceType, futureStater: futureStater },

        success: function (empData) {
            var chartData = new google.visualization.DataTable();
            chartData.addColumn('string', 'Name');
            chartData.addColumn('string', 'ToolTip');
            chartData.addColumn('string', 'Manager');
            $.each(empData, function (index, row) {
                //var reportID = row.ReportsTo == '' ? '' : row.ReportsTo;
                //var reportID = row.ReportsTo==null ?'' : row.ReportsTo='Vacant';
                var reportToId = null;
                if (row.ReportsTo != null) {
                    reportToId = row.ReportsTo.toString();
                }
                var ImageUrl;
                if (row.ImageUrl != null && row.ImageUrl != undefined) {
                    ImageUrl = "/Upload/Resources/" + row.ImageUrl;
                }
                else {
                    ImageUrl = "Upload/Resources/No-image-found.jpg";
                }
                chartData.addRows([[{
                    v: row.EmpId.toString(),
                    f: '<div>' + row.Name + '</div><div>Employed For :' + row.LengthOfEmployeement + '</div><div>(<span>' + row.Value + '</span>)</div><img height="50px" width="50px" src="' + ImageUrl + '"/>'
                }, reportToId, row.Value]]);
            });
            debugger;
            var chart = new google.visualization.OrgChart($("#empChart")[0]);
            chart.draw(chartData, {allowHtml: true });
        },
        failure: function (xhr, status, error) {
            alert("Failure: " + xhr.responseText);
        },
        error: function (xhr, status, error) {
            alert("Error: " + xhr.responseText);
        }
    });
}
/*--------------------------------------------------------------------*/
$('#drpBusiness').change(function () {
    var value = $(this).val();
    //if (value != "0") {
    $.ajax({
        url: constantDocument.bindDiv,
        data: { businessId: value },
        success: function (data) {

            $("#drpDivision").html('');
            var toAppend = '';
            toAppend += "<option value='0'>All</option>";
            $.each(data, function (index, item) {
                toAppend += "<option value='" + item.Id + "'>" + item.Name + "</option>";
            })
            $("#drpDivision").html(toAppend);
            if ($("#drpDivision").val() == 0) {
                $("#drpDivision").val(0);
                $('#drpPool').val(0);
                $('#drpFunction').val(0);
            }
            drawBusiEmpChart();
        }
    });
    //}
});

$('#drpDivision').change(function () {
    var value = $(this).val();
    if (value != "0") {
        $.ajax({
            url: constantDocument.bindpool,
            data: { DivisionId: value },
            success: function (data) {
                $("#drp-Pool").html('');
                var toAppend = '';
                toAppend += "<option value='0'>All</option>";
                $.each(data, function (index, item) {
                    toAppend += "<option value='" + item.Id + "'>" + item.Name + "</option>";
                })
                $("#drpPool").html(toAppend);
                if ($("#drpPool").val() == 0) {
                    $("#drpPool").val(0);
                }
                drawBusiEmpChart();
                $.ajax({
                    url: constantDocument.bindFuncation,
                    data: { DivisionId: value },
                    success: function (data) {
                        $("#drp-Function").html('');
                        var toAppend = '';
                        toAppend += "<option value='0'>All</option>";
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
        $.ajax({
            url: constantDocument.bindpool,
            data: { DivisionId: value },
            success: function (data) {
                $("#drp-Pool").html('');
                var toAppend = '';
                toAppend += "<option value='0'>All</option>";
                $.each(data, function (index, item) {
                    toAppend += "<option value='" + item.Id + "'>" + item.Name + "</option>";
                })
                $("#drpPool").html(toAppend);
                if ($("#drpPool").val() == 0) {
                    $("#drpPool").val(0);
                }
                drawBusiEmpChart();
                $.ajax({
                    url: constantDocument.bindFuncation,
                    data: { DivisionId: value },
                    success: function (data) {
                        $("#drp-Function").html('');
                        var toAppend = '';
                        toAppend += "<option value='0'>All</option>";
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
});