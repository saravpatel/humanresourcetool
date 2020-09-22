
function DataTableDesign() {
    $('#PerformanceListtable tfoot tr').appendTo('#PerformanceListtable thead');
    var table = $('#PerformanceListtable').DataTable({
        "sDom": '<"top"i>rt<"bottom"flp><"clear">',
        "scrollX": true
    });
    $('#tableDivProject').find('.dataTables_filter').hide();
    $('#tableDivProject').find('.dataTables_info').hide();
    $("#tableDivProject thead .Name").keyup(function () {
        table.column(0).search(this.value).draw();
    });
    $("#tableDivProject thead .Company").keyup(function () {
        table.column(1).search(this.value).draw();
    });
    $("#tableDivProject thead .Location").keyup(function () {
        table.column(2).search(this.value).draw();
    });
    $("#tableDivProject thead .Business").keyup(function () {
        table.column(3).search(this.value).draw();
    });
    $("#tableDivProject thead .Division").keyup(function () {
        table.column(4).search(this.value).draw();
    });
    $("#tableDivProject thead .Pool").keyup(function () {
        table.column(5).search(this.value).draw();
    });
    $("#tableDivProject thead .Function").keyup(function () {
        table.column(6).search(this.value).draw();
    });
    $("#tableDivProject thead .JobTitle").keyup(function () {
        table.column(7).search(this.value).draw();
    });
    $("#tableDivProject thead .EmployeeType").keyup(function () {
        table.column(8).search(this.value).draw();
    });
}
$("#tableDivProject").on('click', '.btn-Refresh-Performance', function () {
    window.location.reload();
});
$("#tableDivProject").on('click', '.btn-ClearSorting-Performance', function () {
    window.location.reload();
});
$("#tableDivProject").on('click', '.btn-clearFilter-Performance', function () {
    window.location.reload();
});
$('#tableDivProject').on('click', '.dataTr', function () {
    if ($(this).hasClass('dataTr')) {
        $('#PerformanceListtable tbody').find('tr.selected').removeClass('selected');
        $(this).addClass('selected');
        $("#tableDivProject").find(".btn-edit-Performance").removeAttr('disabled');
        $("#tableDivProject").find(".btn-delete-Performance").removeAttr('disabled');
    }
});
$("#tableDivProject").on('click', '.btn-delete-Performance', function () {
    var id = $("#tableDivProject").find("#PerformanceListtable tbody").find('.selected').attr("id");
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
                    $.ajax({
                        url: constantSet.deleteDocument,
                        data: { Id: id },
                        success: function (data) {
                            $(".hrtoolLoader").hide();
                            $(".toast-success").show();
                            $("#Createperformance").html('');
                            $("#ListOfPerformance").html('');
                            $("#ListOfPerformance").html(data);
                            DataTableDesign();
                            $(".hrtoolLoader").hide();
                            $(".modal-backdrop").hide();
                            setTimeout(function () {
                                $(".toast-success").hide();
                                window.location.href = constantSet.Index;
                            }, 1500);
                        }
                    });
                }
            }]
    });
});
$(document).ready(function () {
    DataTableDesign();

});

//Add new performance
$("#ListOfPerformance").on('click', '.btn-add-Performance', function () {
    $(".hrtoolLoader").show();
    $("#page_content_inner").find("sendemailId").show();
    var current = $("#CurrentUser").val();
    $.ajax({
        url: constantSet.addEdit,
        data: {},
        success: function (data) {
            $("#page_content_inner").find('#Createperformance').html('');
            $("#page_content_inner").find('#Createperformance').html(data);
            $("#page_content_inner").find('#ListOfPerformance').html('');
            $('[data-toggle="tooltip"]').tooltip();
            $("#page_content_inner").find("#Createperformance").find("#drp-Company").selectList({
                onAdd: function (select, value, text) {
                    if (value == "All") {
                        $.each($("#page_content_inner").find("#Createperformance").find("#drp-Company").parent().find(".selectlist-item"), function () {
                            if ($(this).html() != "All") {
                                $(this).click();
                            }
                        });
                    }
                    else {
                        $.each($("#page_content_inner").find("#Createperformance").find("#drp-Company").parent().find(".selectlist-item"), function () {
                            if ($(this).html() == "All") {
                                $(this).click();
                            }
                        });
                    }
                },
            });
            $("#page_content_inner").find("#Createperformance").find("#drp-Location").selectList({
                onAdd: function (select, value, text) {
                    if (value == "All") {
                        $.each($("#page_content_inner").find("#Createperformance").find("#drp-Location").parent().find(".selectlist-item"), function () {
                            if ($(this).html() != "All") {
                                $(this).click();
                            }
                        });
                    }
                    else {
                        $.each($("#page_content_inner").find("#Createperformance").find("#drp-Location").parent().find(".selectlist-item"), function () {
                            if ($(this).html() == "All") {
                                $(this).click();
                            }
                        });
                    }
                },
            });
            $("#page_content_inner").find("#Createperformance").find("#drp-Business").selectList({
                onAdd: function (select, value, text) {
                    if (value == "All") {
                        $.each($("#page_content_inner").find("#Createperformance").find("#drp-Business").parent().find(".selectlist-item"), function () {
                            if ($(this).html() != "All") {
                                $(this).click();
                            }
                        });
                    }
                    else {
                        $.each($("#page_content_inner").find("#Createperformance").find("#drp-Business").parent().find(".selectlist-item"), function () {
                            if ($(this).html() == "All") {
                                $(this).click();
                            }
                        });
                    }
                },
            });
            $("#page_content_inner").find("#Createperformance").find("#drp-Division").selectList({
                onAdd: function (select, value, text) {
                    if (value == "All") {
                        $.each($("#page_content_inner").find("#Createperformance").find("#drp-Division").parent().find(".selectlist-item"), function () {
                            if ($(this).html() != "All") {
                                $(this).click();
                            }
                        });
                    }
                    else {
                        $.each($("#page_content_inner").find("#Createperformance").find("#drp-Division").parent().find(".selectlist-item"), function () {
                            if ($(this).html() == "All") {
                                $(this).click();
                            }
                        });
                    }
                },
            });
            $("#page_content_inner").find("#Createperformance").find("#drp-Pool").selectList({
                onAdd: function (select, value, text) {
                    if (value == "All") {
                        $.each($("#page_content_inner").find("#Createperformance").find("#drp-Pool").parent().find(".selectlist-item"), function () {
                            if ($(this).html() != "All") {
                                $(this).click();
                            }
                        });
                    }
                    else {
                        $.each($("#page_content_inner").find("#Createperformance").find("#drp-Pool").parent().find(".selectlist-item"), function () {
                            if ($(this).html() == "All") {
                                $(this).click();
                            }
                        });
                    }
                },
            });
            $("#page_content_inner").find("#Createperformance").find("#drp-Function").selectList({
                onAdd: function (select, value, text) {
                    if (value == "All") {
                        $.each($("#page_content_inner").find("#Createperformance").find("#drp-Function").parent().find(".selectlist-item"), function () {
                            if ($(this).html() != "All") {
                                $(this).click();
                            }
                        });
                    }
                    else {
                        $.each($("#page_content_inner").find("#Createperformance").find("#drp-Function").parent().find(".selectlist-item"), function () {
                            if ($(this).html() == "All") {
                                $(this).click();
                            }
                        });
                    }
                },
            });
            $("#page_content_inner").find("#Createperformance").find("#drp-JobTitle").selectList({
                onAdd: function (select, value, text) {
                    if (value == "All") {
                        $.each($("#page_content_inner").find("#Createperformance").find("#drp-JobTitle").parent().find(".selectlist-item"), function () {
                            if ($(this).html() != "All") {
                                $(this).click();
                            }
                        });
                    }
                    else {
                        $.each($("#page_content_inner").find("#Createperformance").find("#drp-JobTitle").parent().find(".selectlist-item"), function () {
                            if ($(this).html() == "All") {
                                $(this).click();
                            }
                        });
                    }
                },
            });
            $("#page_content_inner").find("#Createperformance").find("#drp-Employment").selectList({
                onAdd: function (select, value, text) {
                    if (value == "All") {
                        $.each($("#page_content_inner").find("#Createperformance").find("#drp-Employment").parent().find(".selectlist-item"), function () {
                            if ($(this).html() != "All") {
                                $(this).click();
                            }
                        });
                    }
                    else {
                        $.each($("#page_content_inner").find("#Createperformance").find("#drp-Employment").parent().find(".selectlist-item"), function () {
                            if ($(this).html() == "All") {
                                $(this).click();
                            }
                        });
                    }
                },
            });
            $("#CompletionDate").Zebra_DatePicker({
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {

                    $("#validationmessageCompletionDate").hide();
                    var fromDate = $('#CompletionDate').val();
                }
            });
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();

        }
    });
});

//Edit performance details
$("#ListOfPerformance").on('click', '.btn-edit-Performance', function () {
    $(".hrtoolLoader").show();

    var current = $("#CurrentUser").val();
    // var id = $("#tableDivProject").find("#PerformanceListtable tbody").find('.selected').attr("id");
    var model = {
        Id: $("#tableDivProject").find("#PerformanceListtable tbody").find('.selected').attr("id")
    }
    $.ajax({
        type: "POST",
        data: JSON.stringify(model),
        url: constantSet.addEdit,
        contentType: "application/json",
        success: function (data) {

            $("#page_content_inner").find('#Createperformance').html('');
            $("#page_content_inner").find('#Createperformance').html(data);
            $("#page_content_inner").find('#ListOfPerformance').html('');
            $("#tableDivProject").find("#btn_SavePerformance").html("Save");
            $('[data-toggle="tooltip"]').tooltip();
            $("#page_content_inner").find("#Createperformance").find("#drp-Company").selectList({
                onAdd: function (select, value, text) {
                    if (value == "All") {
                        $.each($("#page_content_inner").find("#Createperformance").find("#drp-Company").parent().find(".selectlist-item"), function () {
                            if ($(this).html() != "All") {
                                $(this).click();
                            }
                        });
                    }
                    else {
                        $.each($("#page_content_inner").find("#Createperformance").find("#drp-Company").parent().find(".selectlist-item"), function () {
                            if ($(this).html() == "All") {
                                $(this).click();
                            }
                        });
                    }
                },
            });
            $("#page_content_inner").find("#Createperformance").find("#drp-Location").selectList({
                onAdd: function (select, value, text) {
                    if (value == "All") {
                        $.each($("#page_content_inner").find("#Createperformance").find("#drp-Location").parent().find(".selectlist-item"), function () {
                            if ($(this).html() != "All") {
                                $(this).click();
                            }
                        });
                    }
                    else {
                        $.each($("#page_content_inner").find("#Createperformance").find("#drp-Location").parent().find(".selectlist-item"), function () {
                            if ($(this).html() == "All") {
                                $(this).click();
                            }
                        });
                    }
                },
            });
            $("#page_content_inner").find("#Createperformance").find("#drp-Business").selectList({
                onAdd: function (select, value, text) {
                    if (value == "All") {
                        $.each($("#page_content_inner").find("#Createperformance").find("#drp-Business").parent().find(".selectlist-item"), function () {
                            if ($(this).html() != "All") {
                                $(this).click();
                            }
                        });
                    }
                    else {
                        $.each($("#page_content_inner").find("#Createperformance").find("#drp-Business").parent().find(".selectlist-item"), function () {
                            if ($(this).html() == "All") {
                                $(this).click();
                            }
                        });
                    }
                },
            });
            $("#page_content_inner").find("#Createperformance").find("#drp-Division").selectList({
                onAdd: function (select, value, text) {
                    if (value == "All") {
                        $.each($("#page_content_inner").find("#Createperformance").find("#drp-Division").parent().find(".selectlist-item"), function () {
                            if ($(this).html() != "All") {
                                $(this).click();
                            }
                        });
                    }
                    else {
                        $.each($("#page_content_inner").find("#Createperformance").find("#drp-Division").parent().find(".selectlist-item"), function () {
                            if ($(this).html() == "All") {
                                $(this).click();
                            }
                        });
                    }
                },
            });
            $("#page_content_inner").find("#Createperformance").find("#drp-Pool").selectList({
                onAdd: function (select, value, text) {
                    if (value == "All") {
                        $.each($("#page_content_inner").find("#Createperformance").find("#drp-Pool").parent().find(".selectlist-item"), function () {
                            if ($(this).html() != "All") {
                                $(this).click();
                            }
                        });
                    }
                    else {
                        $.each($("#page_content_inner").find("#Createperformance").find("#drp-Pool").parent().find(".selectlist-item"), function () {
                            if ($(this).html() == "All") {
                                $(this).click();
                            }
                        });
                    }
                },
            });
            $("#page_content_inner").find("#Createperformance").find("#drp-Function").selectList({
                onAdd: function (select, value, text) {
                    if (value == "All") {
                        $.each($("#page_content_inner").find("#Createperformance").find("#drp-Function").parent().find(".selectlist-item"), function () {
                            if ($(this).html() != "All") {
                                $(this).click();
                            }
                        });
                    }
                    else {
                        $.each($("#page_content_inner").find("#Createperformance").find("#drp-Function").parent().find(".selectlist-item"), function () {
                            if ($(this).html() == "All") {
                                $(this).click();
                            }
                        });
                    }
                },
            });
            $("#page_content_inner").find("#Createperformance").find("#drp-JobTitle").selectList({
                onAdd: function (select, value, text) {
                    if (value == "All") {
                        $.each($("#page_content_inner").find("#Createperformance").find("#drp-JobTitle").parent().find(".selectlist-item"), function () {
                            if ($(this).html() != "All") {
                                $(this).click();
                            }
                        });
                    }
                    else {
                        $.each($("#page_content_inner").find("#Createperformance").find("#drp-JobTitle").parent().find(".selectlist-item"), function () {
                            if ($(this).html() == "All") {
                                $(this).click();
                            }
                        });
                    }
                },
            });
            $("#page_content_inner").find("#Createperformance").find("#drp-Employment").selectList({
                onAdd: function (select, value, text) {
                    if (value == "All") {
                        $.each($("#page_content_inner").find("#Createperformance").find("#drp-Employment").parent().find(".selectlist-item"), function () {
                            if ($(this).html() != "All") {
                                $(this).click();
                            }
                        });
                    }
                    else {
                        $.each($("#page_content_inner").find("#Createperformance").find("#drp-Employment").parent().find(".selectlist-item"), function () {
                            if ($(this).html() == "All") {
                                $(this).click();
                            }
                        });
                    }
                },
            });
            $("#CompletionDate").Zebra_DatePicker({
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {

                    $("#validationmessageCompletionDate").hide();
                    var fromDate = $('#CompletionDate').val();
                }
            });

            $('.CoreSegmentNestable').nestable({
                maxDepth: 1,
            });

            $('.JobRoleSegmentNestable').nestable({
                maxDepth: 1,
            });

            $('.CustomerSegmentNestable').nestable({
                maxDepth: 1,
            });

            $('.CoworkerSegmentNestable').nestable({
                maxDepth: 1,
            });

            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
        }
    });
});

$("#page_content_inner").on('click', '#add-more', function () {
    var id = $("#page_content_inner").find("div.OverallScore:last").data('id');
    $.ajax({
        url: constantSet.AddMorephoneUrl,
        data: { actionid: id },
        success: function (partialView) {
            $("#Createperformance").find('#Overallcontainer').append(partialView);
            //$('#Overallcontainer').append(partialView);
        }
    });
});

//Email

$("#page_content_inner").on('click', "#SendMailToAll", function () {
    var RewId = $("#PerformaceId").val();
    var IsError = false;
    var CompetionDate = $("#CompletionDate").val();
    var CompletionDate = $("#page_content_inner").find("#CompletionDate").val();
    if (CompletionDate == "") {
        IsError = true;
        $("#validationmessageCompletionDate").show();
        $("#validationmessageCompletionDate").html('Completion Date is Required.');
    }
    if (IsError) {
        return false;
        $(".hrtoolLoader").hide();
        $(".modal-backdrop").hide();
    }
    else if (!IsError) {
        $.ajax({
            url: constantSet.SendMailToAll,
            data: { Compltiondate: CompetionDate },
            success: function (result) {
                //$("#page_content_inner").find('#Createperformance').html('');
                //$("#page_content_inner").find('#Createperformance').html(data);
                //$("#page_content_inner").find("#CoreSegmentModel").find(".modal-body").html("");
                //$("#page_content_inner").find("#CoreSegmentModel").find(".modal-body").html(result);
            }
        });
    }
})

//Core Segment javascript

$("#page_content_inner").on('click', '#CoreSegmentclass', function () {
    $.ajax({
        url: constantSet.AddEditCoreSagment,
        data: { isAddMode: true, Title: "", Description: "", JsonQuestionString: "", SegmentId: 0 },
        success: function (result) {
            $("#page_content_inner").find("#CoreSegmentModel").find(".modal-body").html("");
            $("#page_content_inner").find("#CoreSegmentModel").find(".modal-body").html(result);
            $("#page_content_inner").find("#CoreSegmentModel").find(".modal-body").find("#wizard").smartWizard({
                onLeaveStep: CoreleaveAStepCallback,
                onFinish: CoreonFinishCallback
            });
            $("#page_content_inner").find('.buttonNext').addClass('btn btn-warning');
            $("#page_content_inner").find('.buttonPrevious').addClass('btn btn-warning');
            $("#page_content_inner").find('.buttonFinish').addClass('btn btn-success');
            $("#page_content_inner").find('.buttonPrevious').hide();
            $("#page_content_inner").find('.buttonFinish').hide();
        }
    });
});

function CoreonFinishCallback() {
    
    var questionList = [];
    var tlist = [];
    var segmentId = $("#page_content_inner").find("#CoreSegmentModel").find("#txt_SegmentId").val();
    var Title = $("#page_content_inner").find("#CoreSegmentModel").find("#Title").val();
    var Description = $("#page_content_inner").find("#CoreSegmentModel").find("#Description").val();
    var CoreFiledId = $("#page_content_inner").find(".col-lg-3").html();
    var FiledText = $("#page_content_inner").find(".col-lg-5").html();
    var CoreValue = $("#page_content_inner").find(".col-lg-4").html();
    var ids = 0;
    var empDataArr = [];
    $.each($("#page_content_inner").find("#CoreSegmentQuestionList").find("li").find(".AddEditRow"), function () {
        ids++;
        var questionData = $(this).find(".col-lg-10").html();
        var coreQueData = $(this).find(".EditCoreSegmet").attr("data-questionjsonstring");
        coreQueData = coreQueData.split(',');
        var CoreFiledId = coreQueData[0];
        var FiledText = coreQueData[2];
        var CoreValue = coreQueData[1];
        var oneData = {
            QueId: ids,
            questionData: questionData,
            FiledId: CoreFiledId,
            FiledText: FiledText,
            CValue: CoreValue
        }
        questionList.push(oneData);
    });
    var questionJsonString = JSON.stringify(questionList);
    if (segmentId != "0") {
        var appendDataString = "<div class='col-lg-11'>";
        appendDataString += "<div class='row'><div class='col-lg-6 coreSegmentTitle'>" + Title + "</div><div class='col-lg-6 coreSegmentTotalQuestion'>" + questionList.length + " Questions</div>";
        appendDataString += "<div class='col-lg-6 coreSegmentDescription'>" + Description + "</div></div>";
        appendDataString += "</div><div class='col-lg-1'><div class='icons'><a><i class='material-icons uk-text-success EditMainCoreSegmet' data-title='" + Title + "' data-description='" + Description + "'data-text='" + FiledText + "' data-questionJsonString='" + questionJsonString + "' data-id='" + segmentId + "' data-toggle='modal' data-target='#CoreSegmentModel'>edit</i></a><a class='removeMainLiCoreSegment'><i class='material-icons uk-text-danger'>delete</i></a></div></div>";

        $("#page_content_inner").find(".liCoreSegment_" + segmentId).find(".AddEditCoreSegmentRow").html('');
        $("#page_content_inner").find(".liCoreSegment_" + segmentId).find(".AddEditCoreSegmentRow").html(appendDataString);
    }
    else {
        var id = $('#CoreSegmentQuestionList li:last-child').data('id');
        var isEmpty = $("#page_content_inner").find("#MainPageCoreSegmentQuestionList").html().trim();
        if (isEmpty == "") {
            var Mainid = 1;
            var appendDataString = "<li class='dd-item liCoreSegment_" + Mainid + "' data-id='" + Mainid + "' id='" + Mainid + "'>";
            appendDataString += "<div class='dd-handle dd3-handle' data-id='" + Mainid + "' id='" + Mainid + "'>";
            appendDataString += "<i class='fa fa-bars'></i></div>";
            appendDataString += "<div class='dd3-content row AddEditCoreSegmentRow'><div class='col-lg-11'>";
            appendDataString += "<div class='row'><div class='col-lg-6 coreSegmentTitle'>" + Title + "</div><div class='col-lg-6 coreSegmentTotalQuestion'>" + questionList.length + " Questions</div>";
            appendDataString += "<div class='col-lg-6 coreSegmentDescription'>" + Description + "</div></div>";
            appendDataString += "</div><div class='col-lg-1'><div class='icons'><a><i class='material-icons uk-text-success EditMainCoreSegmet' data-title='" + Title + "' data-description='" + Description + "'data-text='" + FiledText + "' data-questionJsonString='" + questionJsonString + "' data-id='" + Mainid + "' data-toggle='modal' data-target='#CoreSegmentModel'>edit</i></a><a class='removeMainLiCoreSegment'><i class='material-icons uk-text-danger'>delete</i></a></div></div>";
            appendDataString += "</div></div></li>";
            $("#page_content_inner").find("#MainPageCoreSegmentQuestionList").html(appendDataString);
        }
        else {
            var Mainid = $('.CoreSegmentNestable li:last-child').attr('data-id');
            Mainid++;
            var appendDataString = "<li class='dd-item liCoreSegment_" + Mainid + "' data-id='" + Mainid + "' id='" + Mainid + "'>";
            appendDataString += "<div class='dd-handle dd3-handle' data-id='" + Mainid + "' id='" + Mainid + "'>";
            appendDataString += "<i class='fa fa-bars'></i></div>";
            appendDataString += "<div class='dd3-content row AddEditCoreSegmentRow'><div class='col-lg-11'>";
            appendDataString += "<div class='row'><div class='col-lg-6 coreSegmentTitle'>" + Title + "</div><div class='col-lg-6 coreSegmentTotalQuestion'>" + questionList.length + " Questions</div>";
            appendDataString += "<div class='col-lg-6 coreSegmentDescription'>" + Description + "</div></div>";
            appendDataString += "</div><div class='col-lg-1'><div class='icons'><a><i class='material-icons uk-text-success EditMainCoreSegmet' data-title='" + Title + "' data-description='" + Description + "' data-questionJsonString='" + questionJsonString + "' data-id='" + Mainid + "' data-toggle='modal' data-target='#CoreSegmentModel'>edit</i></a><a class='removeMainLiCoreSegment'><i class='material-icons uk-text-danger'>delete</i></a></div></div>";
            appendDataString += "</div></div></li>";
            $("#page_content_inner").find("#MainPageCoreSegmentQuestionList").append(appendDataString);
        }
    }
    
    $('.CoreSegmentNestable').nestable({
        maxDepth: 1,
    });
    $("#page_content_inner").find('#CoreSegmentModel').find(".close").click();
}

function CoreleaveAStepCallback(obj, context) {
    if (context.fromStep == 1) {
        var iserror = false;
        var title = $("#Createperformance").find("#CoreSegmentModel").find("#Title").val().trim();
        if (title == "") {
            iserror = true;
            $("#Createperformance").find("#CoreSegmentModel").find("#validationmessageTitle").show();
        }
        if (iserror) {
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
            return false;
        }
        else {

            $("#page_content_inner").find("#CoreSegmentModel").find('.buttonNext').hide();
            $("#page_content_inner").find("#CoreSegmentModel").find('.buttonPrevious').show();
            $("#page_content_inner").find("#CoreSegmentModel").find('.buttonFinish').show();
            return true;
        }
    }
    else {

        $("#page_content_inner").find("#CoreSegmentModel").find('.buttonNext').show();
        $("#page_content_inner").find("#CoreSegmentModel").find('.buttonPrevious').hide();
        $("#page_content_inner").find("#CoreSegmentModel").find('.buttonFinish').hide();
        return true;
    }
};

$("#page_content_inner").on('click', '#CoreSegmentAddQuestion', function () {
    $("#Createperformance").find("#CoreSegmentAddQuestionModel").find('#hidden_CoreSegment_Question_id').val("0");
    $("#Createperformance").find("#CoreSegmentAddQuestionModel").find('#coreQuestion').val("");
    $("#Createperformance").find("#CoreSegmentAddQuestionModel").find('#label_coresegment_Question').hide();
    $("#Createperformance").find("#CoreSegmentAddQuestionModel").find("#CoreSegment_HelpText").val("");
    $("#Createperformance").find("#CoreSegmentAddQuestionModel").find("#CoreSegment_Question_Type").val("");
    $("#Createperformance").find("#CoreSegmentAddQuestionModel").find("#CoreSegment_Question_CoreValue").val("");
});

$("#page_content_inner").on('click', '#btn-submit-AddCoreSegmentQuestion', function () {
    var isError = false;
    var id = $("#Createperformance").find("#CoreSegmentAddQuestionModel").find('#hidden_CoreSegment_Question_id').val();
    var data = $("#Createperformance").find("#CoreSegmentAddQuestionModel").find('#coreQuestion').val().trim();
    var dataFiled = $("#CoreSegment_Question_Type").val();
    var coreTpe = $("#CoreSegment_Question_CoreValue").val();
    var helpText = $("#CoreSegment_HelpText").val();
    if (data == "") {
        isError = true;
        $("#Createperformance").find("#CoreSegmentAddQuestionModel").find("#label_coresegment_Question").show();
    }
    if (dataFiled == "" || dataFiled == 0 || dataFiled == null) {
        isError = true;
        $("#lblerrorFiledType").show();
    }
    if (helpText == "") {
        isError = true;
        $("#lblerrorHelpText").show();
    }
    if (coreTpe == "" || coreTpe == 0 || coreTpe == null) {
        isError = true;
        $("#lblerrorCoreValue").show();
    }
    if (!isError) {
        if (id != "0") {
            var appendDataString = "";
            appendDataString += "<div class='col-lg-3' style='display:none'>" + dataFiled + "</div>";
            appendDataString += "<div class='col-lg-5' style='display:none'>" + helpText + "</div>";
            appendDataString += "<div class='col-lg-4' style='display:none'>" + coreTpe + "</div>";
            appendDataString += "<div class='col-lg-10'>" + data + "</div>";
            appendDataString += "<div class='col-lg-2' style='text-align: right;'><div class='icons'><a><i class='material-icons uk-text-success EditCoreSegmet' data-question='" + data + "' data-id='" + id + "' data-myval='" + dataFiled + "' data-toggle='modal' data-questionJsonString='" + dataFiled + "," + coreTpe + "," + helpText + "' data-target='#CoreSegmentAddQuestionModel'>edit</i></a><a class='removeLi'><i class='material-icons uk-text-danger'>delete</i></a></div></div>";
            $("#Createperformance").find("#CoreSegmentModel").find('.CoreSegmentQuestionNestable').find(".liQuestion_" + id).find(".AddEditRow").html("");
            $("#Createperformance").find("#CoreSegmentModel").find('.CoreSegmentQuestionNestable').find(".liQuestion_" + id).find(".AddEditRow").html(appendDataString);
            $("#Createperformance").find("#CoreSegmentAddQuestionModel").find('#coreQuestion').val("");
            $("#Createperformance").find("#CoreSegmentAddQuestionModel").find('.close').click();
        }
        else {
            var isEmpty = $("#page_content_inner").find("#CoreSegmentQuestionList").html().trim();
            var currentTime = getCurrentDateTime();
            if (isEmpty == "") {
                var id = 1;
                var appendDataString = "<li class='dd-item liQuestion_" + id + "' data-id='" + id + "' id='" + id + "'>";
                appendDataString += "<div class='dd-handle dd3-handle' data-id='" + id + "' id='" + id + "'>";
                appendDataString += "<i class='fa fa-bars'></i></div>";
                appendDataString += "<div class='dd3-content row AddEditRow'><div class='col-lg-10'>" + data + "</div><div class='col-lg-3' style='display: none;'>" + dataFiled + "</div><div class='col-lg-5' style='display:none'>" + helpText + "</div><div class='col-lg-4' style='display:none'>" + coreTpe + "</div>";
                appendDataString += "<div class='col-lg-2' style='text-align: right;'><div class='icons'><a><i class='material-icons uk-text-success EditCoreSegmet' data-question='" + data + "' data-id='" + id + "' data-myval='" + dataFiled + "' data-questionJsonString='" + dataFiled + "," + coreTpe + "," + helpText + "' data-toggle='modal' data-target='#CoreSegmentAddQuestionModel'>edit</i></a><a class='removeLi'><i class='material-icons uk-text-danger'>delete</i></a></div></div>";
                appendDataString += "</div></div></li>";
                $("#page_content_inner").find("#CoreSegmentQuestionList").html(appendDataString);
            }
            else {
                id = $('#CoreSegmentQuestionList li:last-child').data('id');
                id++;
                var appendDataString = "<li class='dd-item liQuestion_" + id + "' data-id='" + id + "' id='" + id + "'>";
                appendDataString += "<div class='dd-handle dd3-handle' data-id='" + id + "' id='" + id + "'>";
                appendDataString += "<i class='fa fa-bars'></i></div>";
                appendDataString += "<div class='dd3-content row AddEditRow'><div class='col-lg-10'>" + data + "</div><div class='col-lg-3' style='display: none;'>" + dataFiled + "</div><div class='col-lg-5' style='display:none'>" + helpText + "</div><div class='col-lg-4' style='display:none'>" + coreTpe + "</div>";
                appendDataString += "<div class='col-lg-2' style='text-align: right;'><div class='icons'><a><i class='material-icons uk-text-success EditCoreSegmet' data-question='" + data + "' data-id='" + id + "' data-myval='" + dataFiled + "' data-questionJsonString='" + dataFiled + "," + coreTpe + "," + helpText + "' data-toggle='modal' data-target='#CoreSegmentAddQuestionModel'>edit</i></a><a class='removeLi'><i class='material-icons uk-text-danger'>delete</i></a></div></div>";
                appendDataString += "</div></li>";
                $("#page_content_inner").find("#CoreSegmentQuestionList").append(appendDataString);
            }
            $("#Createperformance").find("#CoreSegmentAddQuestionModel").find('#coreQuestion').val("");
            $("#Createperformance").find("#CoreSegmentAddQuestionModel").find('.close').click();
            $("#Createperformance").find("#CoreSegmentModel").find('.CoreSegmentQuestionNestable').nestable({
                maxDepth: 1,
            });
        }
    }
});

$("#page_content_inner").on('click', '.removeLi', function () {
    $(this).parent().parent().parent().parent().remove();
});

$("#page_content_inner").on('click', '.EditCoreSegmet', function () {
    var question = $(this).attr("data-question");
    var id = $(this).attr("data-id");
    var QueData = $(this).attr("data-questionJsonString");
    $("#CoreSegmentAddQuestionModel").find('#hidden_CoreSegment_Question_id').val(id);
    $("#CoreSegmentAddQuestionModel").find('#coreQuestion').val(question);
    QueData = QueData.split(",");
    $("#CoreSegmentAddQuestionModel").find('#CoreSegment_HelpText').val(QueData[2]);
    //$("#CoreSegmentAddQuestionModel").find('#CoreSegment_Question_Type').text = CoreFiledText;
    $("#CoreSegmentAddQuestionModel").find('#CoreSegment_Question_Type').val(QueData[0]);
    //$("#CoreSegmentAddQuestionModel").find('#CoreSegment_Question_CoreValue').text = CoreCalueText;
    $("#CoreSegmentAddQuestionModel").find('#CoreSegment_Question_CoreValue').val(QueData[1]);
});

$("#page_content_inner").on('click', '.EditMainCoreSegmet', function () {
    
    var title = $(this).attr("data-title");
    var description = $(this).attr("data-description");
    var questionString = $(this).attr("data-questionjsonstring");
    var id = $(this).attr("data-id");
    $.ajax({
        url: constantSet.AddEditCoreSagment,
        data: { isAddMode: false, Title: title, Description: description, JsonQuestionString: questionString, SegmentId: id },
        success: function (result) {
            $("#page_content_inner").find("#CoreSegmentModel").find(".modal-body").html("");
            $("#page_content_inner").find("#CoreSegmentModel").find(".modal-body").html(result);

            $("#SegmentSection").find("#wizard").smartWizard({
                onLeaveStep: CoreleaveAStepCallback,
                onFinish: CoreonFinishCallback
            });
            $("#page_content_inner").find('.buttonNext').addClass('btn btn-warning');
            $("#page_content_inner").find('.buttonPrevious').addClass('btn btn-warning');
            $("#page_content_inner").find('.buttonFinish').addClass('btn btn-success');
            $("#page_content_inner").find('.buttonPrevious').hide();
            $("#page_content_inner").find('.buttonFinish').hide();

            $("#Createperformance").find("#CoreSegmentModel").find('.CoreSegmentQuestionNestable').nestable({
                maxDepth: 1,
            });
        }
    });
});

$("#page_content_inner").on('click', '.removeMainLiCoreSegment', function () {
    $(this).parent().parent().parent().parent().remove();
});

//End Core Segment Javascript


// job Roles Segment
$("#page_content_inner").on('click', '#JobRoleSegmentclass', function () {
    $.ajax({
        url: constantSet.AddEditJobRoleSegment,
        data: { isAddMode: true, Title: "", Description: "", JsonQuestionString: "", SegmentId: 0 },
        success: function (result) {
            $("#page_content_inner").find("#JobRoleSegmentModel").find(".modal-body").html("");
            $("#page_content_inner").find("#JobRoleSegmentModel").find(".modal-body").html(result);
            $("#page_content_inner").find("#JobRoleSegmentModel").find(".modal-body").find("#wizard").smartWizard({
                onLeaveStep: jobrolesleaveAStepCallback,
                onFinish: jobrolesonFinishCallback
            });
            $("#page_content_inner").find("#JobRoleSegmentModel").find('.buttonNext').addClass('btn btn-warning');
            $("#page_content_inner").find("#JobRoleSegmentModel").find('.buttonPrevious').addClass('btn btn-warning');
            $("#page_content_inner").find("#JobRoleSegmentModel").find('.buttonFinish').addClass('btn btn-success');
            $("#page_content_inner").find("#JobRoleSegmentModel").find('.buttonPrevious').hide();
            $("#page_content_inner").find("#JobRoleSegmentModel").find('.buttonFinish').hide();
        }
    });
});

function jobrolesleaveAStepCallback(obj, context) {
    if (context.fromStep == 1) {
        var iserror = false;
        var title = $("#Createperformance").find("#JobRoleSegmentModel").find("#Title").val().trim();
        if (title == "") {
            iserror = true;
            $("#Createperformance").find("#JobRoleSegmentModel").find("#validationmessageTitle").show();
        }

        if (iserror) {
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
            return false;
        }
        else {

            $("#page_content_inner").find("#JobRoleSegmentModel").find('.buttonNext').hide();
            $("#page_content_inner").find("#JobRoleSegmentModel").find('.buttonPrevious').show();
            $("#page_content_inner").find("#JobRoleSegmentModel").find('.buttonFinish').show();
            return true;
        }
    }
    else {

        $("#page_content_inner").find("#JobRoleSegmentModel").find('.buttonNext').show();
        $("#page_content_inner").find("#JobRoleSegmentModel").find('.buttonPrevious').hide();
        $("#page_content_inner").find("#JobRoleSegmentModel").find('.buttonFinish').hide();
        return true;
    }
};

function jobrolesonFinishCallback() {
    var questionList = [];
    var segmentId = $("#page_content_inner").find("#JobRoleSegmentModel").find("#txt_SegmentId").val();
    var Title = $("#page_content_inner").find("#JobRoleSegmentModel").find("#Title").val();
    var Description = $("#page_content_inner").find("#JobRoleSegmentModel").find("#Description").val();
    var jobQueId = 0;
    $.each($("#page_content_inner").find("#JobRolesSegmentQuestionList").find("li").find(".AddEditRow"), function () {
        jobQueId++;
        var questionData = $(this).find(".col-lg-10").html().trim();
        var jobRoleSegQueData = $(this).find(".EditJobRolesSegmetQuestion").attr("data-questionjsonstring");
        jobRoleSegQueData = jobRoleSegQueData.split(',');
        var JobRoleFiledId = jobRoleSegQueData[0];
        var JobRoleFiledText = jobRoleSegQueData[2];
        var JobRoleValue = jobRoleSegQueData[1];
        var oneData = {
            QueId: jobQueId,
            questionData: questionData,
            FiledId: JobRoleFiledId,
            FiledText: JobRoleFiledText,
            CValue: JobRoleValue
        }
        questionList.push(oneData);
    });
    var questionJsonString = JSON.stringify(questionList);

    if (segmentId != "0") {
        var appendDataString = "<div class='col-lg-11'>";
        appendDataString += "<div class='row'><div class='col-lg-6 jobSegmentTitle'>" + Title + "</div><div class='col-lg-6 jobSegmentTotalQuestion'>" + questionList.length + " Questions</div>";
        appendDataString += "<div class='col-lg-6 jobSegmentDescription'>" + Description + "</div></div>";
        appendDataString += "</div><div class='col-lg-1'><div class='icons'><a><i class='material-icons uk-text-success EditMainJobSegmet' data-title='" + Title + "' data-description='" + Description + "' data-questionJsonString='" + questionJsonString + "' data-id='" + segmentId + "' data-toggle='modal' data-target='#JobRoleSegmentModel'>edit</i></a><a class='removeMainLiJobSegment'><i class='material-icons uk-text-danger'>delete</i></a></div></div>";

        $("#page_content_inner").find(".liJobSegment_" + segmentId).find(".AddEditJobSegmentRow").html('');
        $("#page_content_inner").find(".liJobSegment_" + segmentId).find(".AddEditJobSegmentRow").html(appendDataString);
    }
    else {
        var id = $('#MainPageJobRoleSegmentQuestionList li:last-child').data('id');
        var isEmpty = $("#page_content_inner").find("#MainPageJobRoleSegmentQuestionList").html().trim();
        if (isEmpty == "") {
            var Mainid = 1;
            var appendDataString = "<li class='dd-item liJobSegment_" + Mainid + "' data-id='" + Mainid + "' id='" + Mainid + "'>";
            appendDataString += "<div class='dd-handle dd3-handle' data-id='" + Mainid + "' id='" + Mainid + "'>";
            appendDataString += "<i class='fa fa-bars'></i></div>";
            appendDataString += "<div class='dd3-content row AddEditJobSegmentRow'><div class='col-lg-11'>";
            appendDataString += "<div class='row'><div class='col-lg-6 jobSegmentTitle'>" + Title + "</div><div class='col-lg-6 jobSegmentTotalQuestion'>" + questionList.length + " Questions</div>";
            appendDataString += "<div class='col-lg-6 jobSegmentDescription'>" + Description + "</div></div>";
            appendDataString += "</div><div class='col-lg-1'><div class='icons'><a><i class='material-icons uk-text-success EditMainJobSegmet' data-title='" + Title + "' data-description='" + Description + "' data-questionJsonString='" + questionJsonString + "' data-id='" + Mainid + "' data-toggle='modal' data-target='#JobRoleSegmentModel'>edit</i></a><a class='removeMainLiJobSegment'><i class='material-icons uk-text-danger'>delete</i></a></div></div>";
            appendDataString += "</div></div></li>";
            $("#page_content_inner").find("#MainPageJobRoleSegmentQuestionList").html(appendDataString);
        }
        else {
            var Mainid = $('#MainPageJobRoleSegmentQuestionList li:last-child').attr('data-id');
            Mainid++;
            var appendDataString = "<li class='dd-item liJobSegment_" + Mainid + "' data-id='" + Mainid + "' id='" + Mainid + "'>";
            appendDataString += "<div class='dd-handle dd3-handle' data-id='" + Mainid + "' id='" + Mainid + "'>";
            appendDataString += "<i class='fa fa-bars'></i></div>";
            appendDataString += "<div class='dd3-content row AddEditJobSegmentRow'><div class='col-lg-11'>";
            appendDataString += "<div class='row'><div class='col-lg-6 jobSegmentTitle'>" + Title + "</div><div class='col-lg-6 jobSegmentTotalQuestion'>" + questionList.length + " Questions</div>";
            appendDataString += "<div class='col-lg-6 jobSegmentDescription'>" + Description + "</div></div>";
            appendDataString += "</div><div class='col-lg-1'><div class='icons'><a><i class='material-icons uk-text-success EditMainJobSegmet' data-title='" + Title + "' data-description='" + Description + "' data-questionJsonString='" + questionJsonString + "' data-id='" + Mainid + "' data-toggle='modal' data-target='#JobRoleSegmentModel'>edit</i></a><a class='removeMainLiJobSegment'><i class='material-icons uk-text-danger'>delete</i></a></div></div>";
            appendDataString += "</div></div></li>";

            $("#page_content_inner").find("#MainPageJobRoleSegmentQuestionList").append(appendDataString);
        }
    }
    $('.JobRoleSegmentNestable').nestable({
        maxDepth: 1,
    });
    $("#page_content_inner").find('#JobRoleSegmentModel').find(".close").click();
}

$("#page_content_inner").on('click', '#JobRolesSegmentAddQuestion', function () {
    $("#Createperformance").find("#JobRolesSegmentAddQuestionModel").find('#hidden_JobSegment_Question_id').val("0");
    $("#Createperformance").find("#JobRolesSegmentAddQuestionModel").find('#jobQuestion').val("");
    $("#Createperformance").find("#JobRolesSegmentAddQuestionModel").find('#label_jobsegment_Question').hide();
    $("#Createperformance").find("#JobRolesSegmentAddQuestionModel").find("#JobRole_HelpText").val("");
    $("#Createperformance").find("#JobRolesSegmentAddQuestionModel").find("#JobRole_Question_Type").val("");
    $("#Createperformance").find("#JobRolesSegmentAddQuestionModel").find("#JobRole_Question_CoreValue").val("");
});

$("#page_content_inner").on('click', '#btn-submit-AddJobRolesSegmentQuestion', function () {
    var id = $("#Createperformance").find("#JobRolesSegmentAddQuestionModel").find('#hidden_JobSegment_Question_id').val();
    var data = $("#Createperformance").find("#JobRolesSegmentAddQuestionModel").find('#jobQuestion').val().trim();
    var JobroleText = $("#JobRole_HelpText").val();
    var JobroleFiled = $("#JobRole_Question_Type").val();
    var JobroleCore = $("#JobRole_Question_CoreValue").val();
    var isError = false;
    if (data == "") {
        isError = true;
        $("#Createperformance").find("#JobRolesSegmentAddQuestionModel").find("#label_jobsegment_Question").show();
    }
    if (JobroleText == "") {
        isError = true;
        $("#Createperformance").find("#JobRolesSegmentAddQuestionModel").find("#lblerrorHelpText").show();
    }
    if (JobroleFiled == "" || JobroleFiled == 0 || JobroleFiled == null) {
        isError = true;
        $("#Createperformance").find("#JobRolesSegmentAddQuestionModel").find("#lblerrorFiledType").show();
    }
    if (JobroleCore == "" || JobroleCore == 0 || JobroleCore == null) {
        isError = true;
        $("#Createperformance").find("#JobRolesSegmentAddQuestionModel").find("#lblerrorCoreValue").show();
    }
    if (!isError) {
        if (id != "0") {
            var appendDataString = "";
            appendDataString += "<div class='col-lg-3' style='display:none'>" + JobroleFiled + "</div>";
            appendDataString += "<div class='col-lg-5' style='display:none'>" + JobroleText + "</div>";
            appendDataString += "<div class='col-lg-4' style='display:none'>" + JobroleCore + "</div>";
            appendDataString += "<div class='col-lg-10'>" + data + "</div>";
            appendDataString += "<div class='col-lg-2' style='text-align: right;'><div class='icons'><a><i class='material-icons uk-text-success EditJobRolesSegmetQuestion' data-question='" + data + "' data-id='" + id + "' data-toggle='modal' data-questionJsonString='" + JobroleFiled + "," + JobroleCore + "," + JobroleText + "' data-target='#JobRolesSegmentAddQuestionModel'>edit</i></a><a class='removeJobSegmentQuestionLi'><i class='material-icons uk-text-danger'>delete</i></a></div></div>";

            $("#Createperformance").find("#JobRoleSegmentModel").find('.JobRoleQuestionSegmentNestable').find(".liQuestion_" + id).find(".AddEditRow").html("");
            $("#Createperformance").find("#JobRoleSegmentModel").find('.JobRoleQuestionSegmentNestable').find(".liQuestion_" + id).find(".AddEditRow").html(appendDataString);

            $("#Createperformance").find("#JobRolesSegmentAddQuestionModel").find('#jobQuestion').val("");
            $("#Createperformance").find("#JobRolesSegmentAddQuestionModel").find('.close').click();
        }
        else {
            var isEmpty = $("#page_content_inner").find("#JobRolesSegmentQuestionList").html().trim();
            var currentTime = getCurrentDateTime();
            if (isEmpty == "") {
                var id = 1;
                var appendDataString = "<li class='dd-item liQuestion_" + id + "' data-id='" + id + "' id='" + id + "'>";
                appendDataString += "<div class='dd-handle dd3-handle' data-id='" + id + "' id='" + id + "'>";
                appendDataString += "<i class='fa fa-bars'></i></div>";
                appendDataString += "<div class='dd3-content row AddEditRow'><div class='col-lg-10'>" + data + "</div><div class='col-lg-3' style='display: none;'>" + JobroleFiled + "</div><div class='col-lg-5' style='display:none'>" + JobroleText + "</div><div class='col-lg-4' style='display:none'>" + JobroleCore + "</div>";
                appendDataString += "<div class='col-lg-2' style='text-align: right;'><div class='icons'><a><i class='material-icons uk-text-success EditJobRolesSegmetQuestion' data-question='" + data + "' data-id='" + id + "' data-questionJsonString='" + JobroleFiled + "," + JobroleCore + "," + JobroleText + "' data-toggle='modal' data-target='#JobRolesSegmentAddQuestionModel'>edit</i></a><a class='removeJobSegmentQuestionLi'><i class='material-icons uk-text-danger'>delete</i></a></div></div>";
                appendDataString += "</div></div></li>";
                $("#page_content_inner").find("#JobRolesSegmentQuestionList").html(appendDataString);
            }
            else {
                id = $('#JobRolesSegmentQuestionList li:last-child').data('id');
                id++;
                var appendDataString = "<li class='dd-item liQuestion_" + id + "' data-id='" + id + "' id='" + id + "'>";
                appendDataString += "<div class='dd-handle dd3-handle' data-id='" + id + "' id='" + id + "'>";
                appendDataString += "<i class='fa fa-bars'></i></div>";
                appendDataString += "<div class='dd3-content row AddEditRow'><div class='col-lg-10'>" + data + "</div><div class='col-lg-3' style='display: none;'>" + JobroleFiled + "</div><div class='col-lg-5' style='display:none'>" + JobroleText + "</div><div class='col-lg-4' style='display:none'>" + JobroleCore + "</div>";
                appendDataString += "<div class='col-lg-2' style='text-align: right;'><div class='icons'><a><i class='material-icons uk-text-success EditJobRolesSegmetQuestion' data-question='" + data + "' data-id='" + id + "' data-questionJsonString='" + JobroleFiled + "," + JobroleCore + "," + JobroleText + "' data-toggle='modal' data-target='#JobRolesSegmentAddQuestionModel'>edit</i></a><a class='removeJobSegmentQuestionLi'><i class='material-icons uk-text-danger'>delete</i></a></div></div>";
                appendDataString += "</div></div></li>";
                $("#page_content_inner").find("#JobRolesSegmentQuestionList").append(appendDataString);
            }
            $("#Createperformance").find("#JobRolesSegmentAddQuestionModel").find('#jobQuestion').val("");
            $("#Createperformance").find("#JobRolesSegmentAddQuestionModel").find('.close').click();
            $("#Createperformance").find("#JobRoleSegmentModel").find('.JobRoleQuestionSegmentNestable').nestable({
                maxDepth: 1,
            });
        }
    }
});

$("#page_content_inner").on('click', '.removeJobSegmentQuestionLi', function () {
    $(this).parent().parent().parent().parent().remove();
});

$("#page_content_inner").on('click', '.EditJobRolesSegmetQuestion', function () {
    var question = $(this).attr("data-question");
    var id = $(this).attr("data-id");
    var QueData = $(this).attr("data-questionJsonString");
    $("#Createperformance").find("#JobRolesSegmentAddQuestionModel").find('#hidden_JobSegment_Question_id').val(id);
    $("#Createperformance").find("#JobRolesSegmentAddQuestionModel").find('#jobQuestion').val(question);
    QueData = QueData.split(",");
    $("#JobRolesSegmentAddQuestionModel").find('#JobRole_Question_Type').val(QueData[0]);
    $("#JobRolesSegmentAddQuestionModel").find('#JobRole_Question_CoreValue').val(QueData[1]);
    $("#JobRolesSegmentAddQuestionModel").find('#JobRole_HelpText').val(QueData[2]);

});

$("#page_content_inner").on('click', '.EditMainJobSegmet', function () {
    
    var title = $(this).attr("data-title");
    var description = $(this).attr("data-description");
    var questionString = $(this).attr("data-questionjsonstring");
    var id = $(this).attr("data-id");
    $.ajax({
        url: constantSet.AddEditJobRoleSegment,
        data: { isAddMode: false, Title: title, Description: description, JsonQuestionString: questionString, SegmentId: id },
        success: function (result) {
            $("#page_content_inner").find("#JobRoleSegmentModel").find(".modal-body").html("");
            $("#page_content_inner").find("#JobRoleSegmentModel").find(".modal-body").html(result);

            $("#page_content_inner").find("#JobRoleSegmentModel").find(".modal-body").find("#wizard").smartWizard({
                onLeaveStep: jobrolesleaveAStepCallback,
                onFinish: jobrolesonFinishCallback
            });
            $("#page_content_inner").find("#JobRoleSegmentModel").find('.buttonNext').addClass('btn btn-warning');
            $("#page_content_inner").find("#JobRoleSegmentModel").find('.buttonPrevious').addClass('btn btn-warning');
            $("#page_content_inner").find("#JobRoleSegmentModel").find('.buttonFinish').addClass('btn btn-success');
            $("#page_content_inner").find("#JobRoleSegmentModel").find('.buttonPrevious').hide();
            $("#page_content_inner").find("#JobRoleSegmentModel").find('.buttonFinish').hide();

            $("#Createperformance").find("#JobRoleSegmentModel").find('.JobRoleQuestionSegmentNestable').nestable({
                maxDepth: 1,
            });
        }
    });
});

$("#page_content_inner").on('click', '.removeMainLiJobSegment', function () {
    $(this).parent().parent().parent().parent().remove();
});

//End jobRoles Segment


//Customer Segment javascript
$("#page_content_inner").on('click', '#CustomerSegmentclass', function () {
    $.ajax({
        url: constantSet.AddEditCustomerSegment,
        data: { isAddMode: true, Title: "", Description: "", JsonQuestionString: "", SegmentId: 0 },
        success: function (result) {
            $("#page_content_inner").find("#CustomerSegmentModel").find(".modal-body").html("");
            $("#page_content_inner").find("#CustomerSegmentModel").find(".modal-body").html(result);
            $("#page_content_inner").find("#CustomerSegmentModel").find(".modal-body").find("#wizard").smartWizard({
                onLeaveStep: CustomerleaveAStepCallback,
                onFinish: CustomeronFinishCallback
            });
            $("#page_content_inner").find("#CustomerSegmentModel").find('.buttonNext').addClass('btn btn-warning');
            $("#page_content_inner").find("#CustomerSegmentModel").find('.buttonPrevious').addClass('btn btn-warning');
            $("#page_content_inner").find("#CustomerSegmentModel").find('.buttonFinish').addClass('btn btn-success');
            $("#page_content_inner").find("#CustomerSegmentModel").find('.buttonPrevious').hide();
            $("#page_content_inner").find("#CustomerSegmentModel").find('.buttonFinish').hide();
        }
    });
});

function CustomerleaveAStepCallback(obj, context) {
    if (context.fromStep == 1) {
        var iserror = false;
        var title = $("#Createperformance").find("#CustomerSegmentModel").find("#Title").val().trim();
        if (title == "") {
            iserror = true;
            $("#Createperformance").find("#CustomerSegmentModel").find("#validationmessageTitle").show();
        }

        if (iserror) {
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
            return false;
        }
        else {

            $("#page_content_inner").find("#CustomerSegmentModel").find('.buttonNext').hide();
            $("#page_content_inner").find("#CustomerSegmentModel").find('.buttonPrevious').show();
            $("#page_content_inner").find("#CustomerSegmentModel").find('.buttonFinish').show();
            return true;
        }
    }
    else {

        $("#page_content_inner").find("#CustomerSegmentModel").find('.buttonNext').show();
        $("#page_content_inner").find("#CustomerSegmentModel").find('.buttonPrevious').hide();
        $("#page_content_inner").find("#CustomerSegmentModel").find('.buttonFinish').hide();
        return true;
    }
};

function CustomeronFinishCallback() {
    var questionList = [];
    var segmentId = $("#page_content_inner").find("#CustomerSegmentModel").find("#txt_SegmentId").val();
    var Title = $("#page_content_inner").find("#CustomerSegmentModel").find("#Title").val();
    var Description = $("#page_content_inner").find("#CustomerSegmentModel").find("#Description").val();
    var CustoQueId = 0;
    $.each($("#page_content_inner").find("#CustomerSegmentModel").find("#CustomerSegmentQuestionList").find("li").find(".AddEditRow"), function () {
        CustoQueId++;
        var questionData = $(this).find(".col-lg-10").html().trim();
        var custoQueData = $(this).find(".EditCustomerSegmetQuestion").attr('data-questionjsonstring');
        custoQueData = custoQueData.split(',');
        var CustomerFiledId = custoQueData[0];
        var CustomerFiledText = custoQueData[2];
        var CustomerValue = custoQueData[1];
        var oneData = {
            QueId: CustoQueId,
            questionData: questionData,
            FiledId: CustomerFiledId,
            FiledText: CustomerFiledText,
            CValue: CustomerValue
        }
        questionList.push(oneData);
    });
    var questionJsonString = JSON.stringify(questionList);

    if (segmentId != "0") {
        var appendDataString = "<div class='col-lg-11'>";
        appendDataString += "<div class='row'><div class='col-lg-6 customerSegmentTitle'>" + Title + "</div><div class='col-lg-6 customerSegmentTotalQuestion'>" + questionList.length + " Questions</div>";
        appendDataString += "<div class='col-lg-6 customerSegmentDescription'>" + Description + "</div></div>";
        appendDataString += "</div><div class='col-lg-1'><div class='icons'><a><i class='material-icons uk-text-success EditMainCustomerSegmet' data-title='" + Title + "' data-description='" + Description + "' data-questionJsonString='" + questionJsonString + "' data-id='" + segmentId + "' data-toggle='modal' data-target='#CustomerSegmentModel'>edit</i></a><a class='removeMainLiCustomerSegment'><i class='material-icons uk-text-danger'>delete</i></a></div></div>";

        $("#page_content_inner").find(".liCustomerSegment_" + segmentId).find(".AddEditCustomerSegmentRow").html('');
        $("#page_content_inner").find(".liCustomerSegment_" + segmentId).find(".AddEditCustomerSegmentRow").html(appendDataString);
    }
    else {
        var id = $('#MainPageCustomerSegmentQuestionList li:last-child').data('id');
        var isEmpty = $("#page_content_inner").find("#MainPageCustomerSegmentQuestionList").html().trim();
        if (isEmpty == "") {
            var Mainid = 1;
            var appendDataString = "<li class='dd-item liCustomerSegment_" + Mainid + "' data-id='" + Mainid + "' id='" + Mainid + "'>";
            appendDataString += "<div class='dd-handle dd3-handle' data-id='" + Mainid + "' id='" + Mainid + "'>";
            appendDataString += "<i class='fa fa-bars'></i></div>";
            appendDataString += "<div class='dd3-content row AddEditCustomerSegmentRow'><div class='col-lg-11'>";
            appendDataString += "<div class='row'><div class='col-lg-6 customerSegmentTitle'>" + Title + "</div><div class='col-lg-6 customerSegmentTotalQuestion'>" + questionList.length + " Questions</div>";
            appendDataString += "<div class='col-lg-6 customerSegmentDescription'>" + Description + "</div></div>";
            appendDataString += "</div><div class='col-lg-1'><div class='icons'><a><i class='material-icons uk-text-success EditMainCustomerSegmet' data-title='" + Title + "' data-description='" + Description + "' data-questionJsonString='" + questionJsonString + "' data-id='" + Mainid + "' data-toggle='modal' data-target='#CustomerSegmentModel'>edit</i></a><a class='removeMainLiCustomerSegment'><i class='material-icons uk-text-danger'>delete</i></a></div></div>";
            appendDataString += "</div></div></li>";

            $("#page_content_inner").find("#MainPageCustomerSegmentQuestionList").html(appendDataString);
        }
        else {
            var Mainid = $('#MainPageCustomerSegmentQuestionList li:last-child').attr('data-id');
            Mainid++;

            var appendDataString = "<li class='dd-item liCustomerSegment_" + Mainid + "' data-id='" + Mainid + "' id='" + Mainid + "'>";
            appendDataString += "<div class='dd-handle dd3-handle' data-id='" + Mainid + "' id='" + Mainid + "'>";
            appendDataString += "<i class='fa fa-bars'></i></div>";
            appendDataString += "<div class='dd3-content row AddEditCustomerSegmentRow'><div class='col-lg-11'>";
            appendDataString += "<div class='row'><div class='col-lg-6 customerSegmentTitle'>" + Title + "</div><div class='col-lg-6 customerSegmentTotalQuestion'>" + questionList.length + " Questions</div>";
            appendDataString += "<div class='col-lg-6 customerSegmentDescription'>" + Description + "</div></div>";
            appendDataString += "</div><div class='col-lg-1'><div class='icons'><a><i class='material-icons uk-text-success EditMainCustomerSegmet' data-title='" + Title + "' data-description='" + Description + "' data-questionJsonString='" + questionJsonString + "' data-id='" + Mainid + "' data-toggle='modal' data-target='#CustomerSegmentModel'>edit</i></a><a class='removeMainLiCustomerSegment'><i class='material-icons uk-text-danger'>delete</i></a></div></div>";
            appendDataString += "</div></div></li>";

            $("#page_content_inner").find("#MainPageCustomerSegmentQuestionList").append(appendDataString);
        }
    }
    $('.CustomerSegmentNestable').nestable({
        maxDepth: 1,
    });
    $("#page_content_inner").find('#CustomerSegmentModel').find(".close").click();
}

$("#page_content_inner").on('click', '#CustomerSegmentAddQuestion', function () {
    $("#Createperformance").find("#CustomerSegmentAddQuestionModel").find('#hidden_CustomerSegment_Question_id').val("0");
    $("#Createperformance").find("#CustomerSegmentAddQuestionModel").find('#CustomerQuestion').val("");
    $("#Createperformance").find("#CustomerSegmentAddQuestionModel").find('#label_Customersegment_Question').hide();
    $("#Createperformance").find("#CustomerSegmentAddQuestionModel").find("#Cusomer_HelpText").val("");
    $("#Createperformance").find("#CustomerSegmentAddQuestionModel").find("#Customer_Question_Type").val("");
    $("#Createperformance").find("#CustomerSegmentAddQuestionModel").find("#Customer_Question_CoreValue").val("");
});

$("#page_content_inner").on('click', '#btn-submit-AddCustomerSegmentQuestion', function () {
    var id = $("#Createperformance").find("#CustomerSegmentAddQuestionModel").find('#hidden_CustomerSegment_Question_id').val();
    var data = $("#Createperformance").find("#CustomerSegmentAddQuestionModel").find('#CustomerQuestion').val().trim();
    var CustomerText = $("#Cusomer_HelpText").val();
    var CustomerFiled = $("#Customer_Question_Type").val();
    var CustomerCore = $("#Customer_Question_CoreValue").val();
    var isError = false;
    if (data == "") {
        isError = true;
        $("#Createperformance").find("#CustomerSegmentAddQuestionModel").find("#label_Customersegment_Question").show();
    }
    if (CustomerText == "") {
        isError = true;
        $("#Createperformance").find("#CustomerSegmentAddQuestionModel").find("#lblerrorHelpText").show();
    }
    if (CustomerFiled == "" || CustomerFiled == null || CustomerFiled == 0) {
        isError = true;
        $("#Createperformance").find("#CustomerSegmentAddQuestionModel").find("#lblerrorFiledType").show();
    }
    if (CustomerCore == "" || CustomerCore == null || CustomerCore == 0) {
        isError = true;
        $("#Createperformance").find("#CustomerSegmentAddQuestionModel").find("#lblerrorCoreValue").show();
    }
    if (!isError) {
        if (id != "0") {
            var appendDataString = "";
            appendDataString += "<div class='col-lg-3' style='display:none'>" + CustomerFiled + "</div>";
            appendDataString += "<div class='col-lg-5' style='display:none'>" + CustomerText + "</div>";
            appendDataString += "<div class='col-lg-4' style='display:none'>" + CustomerCore + "</div>";
            appendDataString += "<div class='col-lg-10'>" + data + "</div>"
            appendDataString += "<div class='col-lg-2' style='text-align: right;'><div class='icons'><a><i class='material-icons uk-text-success EditCustomerSegmetQuestion' data-question='" + data + "' data-id='" + id + "' data-toggle='modal' data-questionJsonString='" + CustomerFiled + "," + CustomerCore + "," + CustomerText + "' data-target='#CustomerSegmentAddQuestionModel'>edit</i></a><a class='removeCustomerSegmentQuestionLi'><i class='material-icons uk-text-danger'>delete</i></a></div></div>";

            $("#Createperformance").find("#CustomerSegmentModel").find('.CustomerSegmentQuestionNestable').find(".liQuestion_" + id).find(".AddEditRow").html("");
            $("#Createperformance").find("#CustomerSegmentModel").find('.CustomerSegmentQuestionNestable').find(".liQuestion_" + id).find(".AddEditRow").html(appendDataString);

            $("#Createperformance").find("#CustomerSegmentAddQuestionModel").find('#CustomerQuestion').val("");
            $("#Createperformance").find("#CustomerSegmentAddQuestionModel").find('.close').click();
        }
        else {
            var isEmpty = $("#page_content_inner").find("#CustomerSegmentQuestionList").html().trim();
            var currentTime = getCurrentDateTime();
            if (isEmpty == "") {
                var id = 1;
                var appendDataString = "<li class='dd-item liQuestion_" + id + "' data-id='" + id + "' id='" + id + "'>";
                appendDataString += "<div class='dd-handle dd3-handle' data-id='" + id + "' id='" + id + "'>";
                appendDataString += "<i class='fa fa-bars'></i></div>";
                appendDataString += "<div class='dd3-content row AddEditRow'><div class='col-lg-10'>" + data + "</div><div class='col-lg-3' style='display: none;'>" + CustomerFiled + "</div><div class='col-lg-5' style='display:none'>" + CustomerText + "</div><div class='col-lg-4' style='display:none'>" + CustomerCore + "</div>";
                appendDataString += "<div class='col-lg-2' style='text-align: right;'><div class='icons'><a><i class='material-icons uk-text-success EditCustomerSegmetQuestion' data-question='" + data + "' data-id='" + id + "' data-toggle='modal' data-questionJsonString='" + CustomerFiled + "," + CustomerCore + "," + CustomerText + "' data-target='#CustomerSegmentAddQuestionModel'>edit</i></a><a class='removeCustomerSegmentQuestionLi'><i class='material-icons uk-text-danger'>delete</i></a></div></div>";
                appendDataString += "</div></div></li>";
                $("#page_content_inner").find("#CustomerSegmentQuestionList").html(appendDataString);
            }
            else {
                id = $('#CustomerSegmentQuestionList li:last-child').data('id');
                id++;
                var appendDataString = "<li class='dd-item liQuestion_" + id + "' data-id='" + id + "' id='" + id + "'>";
                appendDataString += "<div class='dd-handle dd3-handle' data-id='" + id + "' id='" + id + "'>";
                appendDataString += "<i class='fa fa-bars'></i></div>";
                appendDataString += "<div class='dd3-content row AddEditRow'><div class='col-lg-10'>" + data + "</div><div class='col-lg-3' style='display: none;'>" + CustomerFiled + "</div><div class='col-lg-5' style='display:none'>" + CustomerText + "</div><div class='col-lg-4' style='display:none'>" + CustomerCore + "</div>";
                appendDataString += "<div class='col-lg-2' style='text-align: right;'><div class='icons'><a><i class='material-icons uk-text-success EditCustomerSegmetQuestion' data-question='" + data + "' data-id='" + id + "' data-toggle='modal' data-questionJsonString='" + CustomerFiled + "," + CustomerCore + "," + CustomerText + "' data-target='#CustomerSegmentAddQuestionModel'>edit</i></a><a class='removeCustomerSegmentQuestionLi'><i class='material-icons uk-text-danger'>delete</i></a></div></div>";
                appendDataString += "</div></div></li>";
                $("#page_content_inner").find("#CustomerSegmentQuestionList").append(appendDataString);
            }
            $("#Createperformance").find("#CustomerSegmentAddQuestionModel").find('#CustomerQuestion').val("");
            $("#Createperformance").find("#CustomerSegmentAddQuestionModel").find('.close').click();
            $("#Createperformance").find("#CustomerSegmentModel").find('.CustomerSegmentQuestionNestable').nestable({
                maxDepth: 1,
            });
        }
    }
});

$("#page_content_inner").on('click', '.removeCustomerSegmentQuestionLi', function () {
    $(this).parent().parent().parent().parent().remove();
});

$("#page_content_inner").on('click', '.EditCustomerSegmetQuestion', function () {
    var question = $(this).attr("data-question");
    var id = $(this).attr("data-id");
    var QueData = $(this).attr("data-questionJsonString");
    $("#Createperformance").find("#CustomerSegmentAddQuestionModel").find('#hidden_CustomerSegment_Question_id').val(id);
    $("#Createperformance").find("#CustomerSegmentAddQuestionModel").find('#CustomerQuestion').val(question);
    QueData = QueData.split(",");
    $("#CustomerSegmentAddQuestionModel").find('#Cusomer_HelpText').val(QueData[2]);
    $("#CustomerSegmentAddQuestionModel").find('#Customer_Question_Type').val(QueData[0]);
    $("#CustomerSegmentAddQuestionModel").find('#Customer_Question_CoreValue').val(QueData[1]);
});

$("#page_content_inner").on('click', '.EditMainCustomerSegmet', function () {
    var title = $(this).attr("data-title");
    var description = $(this).attr("data-description");
    var questionString = $(this).attr("data-questionjsonstring");
    var id = $(this).attr("data-id");
    $.ajax({
        url: constantSet.AddEditCustomerSegment,
        data: { isAddMode: false, Title: title, Description: description, JsonQuestionString: questionString, SegmentId: id },
        success: function (result) {
            $("#page_content_inner").find("#CustomerSegmentModel").find(".modal-body").html("");
            $("#page_content_inner").find("#CustomerSegmentModel").find(".modal-body").html(result);
            $("#page_content_inner").find("#CustomerSegmentModel").find(".modal-body").find("#wizard").smartWizard({
                onLeaveStep: CustomerleaveAStepCallback,
                onFinish: CustomeronFinishCallback
            });
            $("#page_content_inner").find("#CustomerSegmentModel").find('.buttonNext').addClass('btn btn-warning');
            $("#page_content_inner").find("#CustomerSegmentModel").find('.buttonPrevious').addClass('btn btn-warning');
            $("#page_content_inner").find("#CustomerSegmentModel").find('.buttonFinish').addClass('btn btn-success');
            $("#page_content_inner").find("#CustomerSegmentModel").find('.buttonPrevious').hide();
            $("#page_content_inner").find("#CustomerSegmentModel").find('.buttonFinish').hide();

            $("#Createperformance").find("#CustomerSegmentModel").find('.CustomerSegmentQuestionNestable').nestable({
                maxDepth: 1,
            });
        }
    });
});

$("#page_content_inner").on('click', '.removeMainLiCustomerSegment', function () {
    $(this).parent().parent().parent().parent().remove();
});

//End Core Segment Javascript


//Co-worker Segment javascript

$("#page_content_inner").on('click', '#CoworkerSegmentclass', function () {
    $.ajax({
        url: constantSet.AddEditCoWorkerSegment,
        data: { isAddMode: true, Title: "", Description: "", JsonQuestionString: "", SegmentId: 0 },
        success: function (result) {
            $("#page_content_inner").find("#CoworkerSegmentModel").find(".modal-body").html("");
            $("#page_content_inner").find("#CoworkerSegmentModel").find(".modal-body").html(result);
            $("#page_content_inner").find("#CoworkerSegmentModel").find(".modal-body").find("#wizard").smartWizard({
                onLeaveStep: CoworkerleaveAStepCallback,
                onFinish: CoworkeronFinishCallback
            });
            $("#page_content_inner").find("#CoworkerSegmentModel").find('.buttonNext').addClass('btn btn-warning');
            $("#page_content_inner").find("#CoworkerSegmentModel").find('.buttonPrevious').addClass('btn btn-warning');
            $("#page_content_inner").find("#CoworkerSegmentModel").find('.buttonFinish').addClass('btn btn-success');
            $("#page_content_inner").find("#CoworkerSegmentModel").find('.buttonPrevious').hide();
            $("#page_content_inner").find("#CoworkerSegmentModel").find('.buttonFinish').hide();
        }
    });
});

function CoworkeronFinishCallback() {
    
    var questionList = [];
    var segmentId = $("#page_content_inner").find("#CoworkerSegmentModel").find("#txt_SegmentId").val();
    var Title = $("#page_content_inner").find("#CoworkerSegmentModel").find("#Title").val();
    var Description = $("#page_content_inner").find("#CoworkerSegmentModel").find("#Description").val();
    var CowoQueId = 0;
    $.each($("#page_content_inner").find("#CoworkerSegmentModel").find("#CoWorkerSegmentQuestionList").find("li").find(".AddEditRow"), function () {
        
        CowoQueId++;
        var questionData = $(this).find(".col-lg-10").html().trim();
        var coWorQue = $(this).find(".EditCoWorkerSegmetQuestion").attr('data-questionjsonstring');
        coWorQue = coWorQue.split(',');
        var CoworkerFiledId = coWorQue[0];
        var CoworkerFiledText = coWorQue[2];
        var CoworkerValue = coWorQue[1];
        var oneData = {
            QueId: CowoQueId,
            questionData: questionData,
            FiledId: CoworkerFiledId,
            FiledText: CoworkerFiledText,
            CValue: CoworkerValue
        }
        questionList.push(oneData);
    });
    var questionJsonString = JSON.stringify(questionList);

    if (segmentId != "0") {
        var appendDataString = "<div class='col-lg-11'>";
        appendDataString += "<div class='row'><div class='col-lg-6 coWorkerSegmentTitle'>" + Title + "</div><div class='col-lg-6 coWorkerSegmentTotalQuestion'>" + questionList.length + " Questions</div>";
        appendDataString += "<div class='col-lg-6 coWorkerSegmentDescription'>" + Description + "</div></div>";
        appendDataString += "</div><div class='col-lg-1'><div class='icons'><a><i class='material-icons uk-text-success EditMainCoWorkerSegmet' data-title='" + Title + "' data-description='" + Description + "' data-questionJsonString='" + questionJsonString + "' data-id='" + segmentId + "' data-toggle='modal' data-target='#CoworkerSegmentModel'>edit</i></a><a class='removeMainLiCoWorkerSegment'><i class='material-icons uk-text-danger'>delete</i></a></div></div>";

        $("#page_content_inner").find(".liCoWorkerSegment_" + segmentId).find(".AddEditCoWorkerSegmentRow").html('');
        $("#page_content_inner").find(".liCoWorkerSegment_" + segmentId).find(".AddEditCoWorkerSegmentRow").html(appendDataString);
    }
    else {
        var id = $('#MainPageCoworkerSegmentQuestionList li:last-child').data('id');
        var isEmpty = $("#page_content_inner").find("#MainPageCoworkerSegmentQuestionList").html().trim();
        if (isEmpty == "") {

            var Mainid = 1;
            var appendDataString = "<li class='dd-item liCoWorkerSegment_" + Mainid + "' data-id='" + Mainid + "' id='" + Mainid + "'>";
            appendDataString += "<div class='dd-handle dd3-handle' data-id='" + Mainid + "' id='" + Mainid + "'>";
            appendDataString += "<i class='fa fa-bars'></i></div>";
            appendDataString += "<div class='dd3-content row AddEditCoWorkerSegmentRow'><div class='col-lg-11'>";
            appendDataString += "<div class='row'><div class='col-lg-6 coWorkerSegmentTitle'>" + Title + "</div><div class='col-lg-6 coWorkerSegmentTotalQuestion'>" + questionList.length + " Questions</div>";
            appendDataString += "<div class='col-lg-6 coWorkerSegmentDescription'>" + Description + "</div></div>";
            appendDataString += "</div><div class='col-lg-1'><div class='icons'><a><i class='material-icons uk-text-success EditMainCoWorkerSegmet' data-title='" + Title + "' data-description='" + Description + "' data-questionJsonString='" + questionJsonString + "' data-id='" + Mainid + "' data-toggle='modal' data-target='#CoworkerSegmentModel'>edit</i></a><a class='removeMainLiCoWorkerSegment'><i class='material-icons uk-text-danger'>delete</i></a></div></div>";
            appendDataString += "</div></div></li>";

            $("#page_content_inner").find("#MainPageCoworkerSegmentQuestionList").html(appendDataString);
        }
        else {
            var Mainid = $('#MainPageCoworkerSegmentQuestionList li:last-child').attr('data-id');
            Mainid++;

            var appendDataString = "<li class='dd-item liCoWorkerSegment_" + Mainid + "' data-id='" + Mainid + "' id='" + Mainid + "'>";
            appendDataString += "<div class='dd-handle dd3-handle' data-id='" + Mainid + "' id='" + Mainid + "'>";
            appendDataString += "<i class='fa fa-bars'></i></div>";
            appendDataString += "<div class='dd3-content row AddEditCoWorkerSegmentRow'><div class='col-lg-11'>";
            appendDataString += "<div class='row'><div class='col-lg-6 coWorkerSegmentTitle'>" + Title + "</div><div class='col-lg-6 coWorkerSegmentTotalQuestion'>" + questionList.length + " Questions</div>";
            appendDataString += "<div class='col-lg-6 coWorkerSegmentDescription'>" + Description + "</div></div>";
            appendDataString += "</div><div class='col-lg-1'><div class='icons'><a><i class='material-icons uk-text-success EditMainCoWorkerSegmet' data-title='" + Title + "' data-description='" + Description + "' data-questionJsonString='" + questionJsonString + "' data-id='" + Mainid + "' data-toggle='modal' data-target='#CoworkerSegmentModel'>edit</i></a><a class='removeMainLiCoWorkerSegment'><i class='material-icons uk-text-danger'>delete</i></a></div></div>";
            appendDataString += "</div></div></li>";

            $("#page_content_inner").find("#MainPageCoworkerSegmentQuestionList").append(appendDataString);
        }
    }
    $('.CoworkerSegmentNestable').nestable({
        maxDepth: 1,
    });
    $("#page_content_inner").find('#CoworkerSegmentModel').find(".close").click();
}

function CoworkerleaveAStepCallback(obj, context) {
    if (context.fromStep == 1) {
        var iserror = false;
        var title = $("#Createperformance").find("#CoworkerSegmentModel").find("#Title").val().trim();
        if (title == "") {
            iserror = true;
            $("#Createperformance").find("#CoworkerSegmentModel").find("#validationmessageTitle").show();
        }

        if (iserror) {
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
            return false;
        }
        else {

            $("#page_content_inner").find("#CoworkerSegmentModel").find('.buttonNext').hide();
            $("#page_content_inner").find("#CoworkerSegmentModel").find('.buttonPrevious').show();
            $("#page_content_inner").find("#CoworkerSegmentModel").find('.buttonFinish').show();
            return true;
        }
    }
    else {

        $("#page_content_inner").find("#CoworkerSegmentModel").find('.buttonNext').show();
        $("#page_content_inner").find("#CoworkerSegmentModel").find('.buttonPrevious').hide();
        $("#page_content_inner").find("#CoworkerSegmentModel").find('.buttonFinish').hide();
        return true;
    }
};

$("#page_content_inner").on('click', '#CoWorkerSegmentAddQuestion', function () {
    $("#Createperformance").find("#CoworkerSegmentAddQuestionModel").find('#hidden_CoWorkerSegment_Question_id').val("0");
    $("#Createperformance").find("#CoworkerSegmentAddQuestionModel").find('#CoWorkerQuestion').val("");
    $("#Createperformance").find("#CoworkerSegmentAddQuestionModel").find('#label_CoWorkersegment_Question').hide();
    $("#Createperformance").find("#CoworkerSegmentAddQuestionModel").find("#CoWorker_HelpText").val("");
    $("#Createperformance").find("#CoworkerSegmentAddQuestionModel").find("#CoWorker_Question_Type").val("");
    $("#Createperformance").find("#CoworkerSegmentAddQuestionModel").find("#CoWorker_Question_CoreValue").val("");
});

$("#page_content_inner").on('click', '#btn-submit-AddCoworkerSegmentQuestion', function () {
    var id = $("#Createperformance").find("#CoworkerSegmentAddQuestionModel").find('#hidden_CoWorkerSegment_Question_id').val();
    var data = $("#Createperformance").find("#CoworkerSegmentAddQuestionModel").find('#CoWorkerQuestion').val().trim();
    var CoWorkerText = $("#CoWorker_HelpText").val();
    var CoWorkerFiled = $("#CoWorker_Question_Type").val();
    var CoWorkerCore = $("#CoWorker_Question_CoreValue").val();
    var isError = false;
    if (data == "") {
        isError = true;
        $("#Createperformance").find("#CoworkerSegmentAddQuestionModel").find("#label_CoWorkersegment_Question").show();
    }
    if (CoWorkerText == "") {
        $("#Createperformance").find("#CoworkerSegmentAddQuestionModel").find("#label_coresegment_Question").show();
        isError = true;
    }
    if (CoWorkerFiled == "" || CoWorkerFiled == null || CoWorkerFiled == 0) {
        $("#Createperformance").find("#CoworkerSegmentAddQuestionModel").find("#lblerrorFiledType").show();
        isError = true;
    }
    if (CoWorkerCore == "" || CoWorkerCore == null || CoWorkerCore == 0) {
        $("#Createperformance").find("#CoworkerSegmentAddQuestionModel").find("#lblerrorCoreValue").show();
        isError = true;
    }
    if (!isError) {
        if (id != "0") {
            var appendDataString = "";
            appendDataString += "<div class='col-lg-3' style='display:none'>" + CoWorkerFiled + "</div>";
            appendDataString += "<div class='col-lg-5' style='display:none'>" + CoWorkerText + "</div>";
            appendDataString += "<div class='col-lg-4' style='display:none'>" + CoWorkerCore + "</div>";
            appendDataString += "<div class='col-lg-10'>" + data + "</div>"
            appendDataString += "<div class='col-lg-2' style='text-align: right;'><div class='icons'><a><i class='material-icons uk-text-success EditCoWorkerSegmetQuestion' data-question='" + data + "' data-id='" + id + "' data-toggle='modal' data-questionJsonString='" + CoWorkerFiled + "," + CoWorkerCore + "," + CoWorkerText + "'data-target='#CoworkerSegmentAddQuestionModel'>edit</i></a><a class='removeCoWorkerQuestionLi'><i class='material-icons uk-text-danger'>delete</i></a></div></div>";

            $("#Createperformance").find("#CoworkerSegmentModel").find('.CoWorkerSegmentQuestionNestable').find(".liQuestion_" + id).find(".AddEditRow").html("");
            $("#Createperformance").find("#CoworkerSegmentModel").find('.CoWorkerSegmentQuestionNestable').find(".liQuestion_" + id).find(".AddEditRow").html(appendDataString);

            $("#Createperformance").find("#CoworkerSegmentAddQuestionModel").find('#CoWorkerQuestion').val("");
            $("#Createperformance").find("#CoworkerSegmentAddQuestionModel").find('.close').click();
        }
        else {
            var isEmpty = $("#page_content_inner").find("#CoWorkerSegmentQuestionList").html().trim();
            var currentTime = getCurrentDateTime();
            if (isEmpty == "") {
                var id = 1;
                var appendDataString = "<li class='dd-item liQuestion_" + id + "' data-id='" + id + "' id='" + id + "'>";
                appendDataString += "<div class='dd-handle dd3-handle' data-id='" + id + "' id='" + id + "'>";
                appendDataString += "<i class='fa fa-bars'></i></div>";
                appendDataString += "<div class='dd3-content row AddEditRow'><div class='col-lg-10'>" + data + "</div><div class='col-lg-3' style='display: none;'>" + CoWorkerFiled + "</div><div class='col-lg-5' style='display:none'>" + CoWorkerText + "</div><div class='col-lg-4' style='display:none'>" + CoWorkerCore + "</div>";
                appendDataString += "<div class='col-lg-2' style='text-align: right;'><div class='icons'><a><i class='material-icons uk-text-success EditCoWorkerSegmetQuestion' data-question='" + data + "' data-id='" + id + "' data-questionJsonString='" + CoWorkerFiled + "," + CoWorkerCore + "," + CoWorkerText + "' data-toggle='modal' data-target='#CoworkerSegmentAddQuestionModel'>edit</i></a><a class='removeCoWorkerQuestionLi'><i class='material-icons uk-text-danger'>delete</i></a></div></div>";
                appendDataString += "</div></div></li>";
                $("#page_content_inner").find("#CoWorkerSegmentQuestionList").html(appendDataString);
            }
            else {
                id = $('#CoWorkerSegmentQuestionList li:last-child').data('id');
                id++;
                var appendDataString = "<li class='dd-item liQuestion_" + id + "' data-id='" + id + "' id='" + id + "'>";
                appendDataString += "<div class='dd-handle dd3-handle' data-id='" + id + "' id='" + id + "'>";
                appendDataString += "<i class='fa fa-bars'></i></div>";
                appendDataString += "<div class='dd3-content row AddEditRow'><div class='col-lg-10'>" + data + "</div><div class='col-lg-3' style='display: none;'>" + CoWorkerFiled + "</div><div class='col-lg-5' style='display:none'>" + CoWorkerText + "</div><div class='col-lg-4' style='display:none'>" + CoWorkerCore + "</div>";
                appendDataString += "<div class='col-lg-2' style='text-align: right;'><div class='icons'><a><i class='material-icons uk-text-success EditCoWorkerSegmetQuestion' data-question='" + data + "' data-id='" + id + "' data-questionJsonString='" + CoWorkerFiled + "," + CoWorkerCore + "," + CoWorkerText + "' data-toggle='modal' data-target='#CoworkerSegmentAddQuestionModel'>edit</i></a><a class='removeCoWorkerQuestionLi'><i class='material-icons uk-text-danger'>delete</i></a></div></div>";
                appendDataString += "</div></div></li>";
                $("#page_content_inner").find("#CoWorkerSegmentQuestionList").append(appendDataString);
            }
            $("#Createperformance").find("#CoworkerSegmentAddQuestionModel").find('#CoWorkerQuestion').val("");
            $("#Createperformance").find("#CoworkerSegmentAddQuestionModel").find('.close').click();
            $("#Createperformance").find("#CoworkerSegmentModel").find('.CoWorkerSegmentQuestionNestable').nestable({
                maxDepth: 1,
            });
        }
    }
});

$("#page_content_inner").on('click', '.removeCoWorkerQuestionLi', function () {
    $(this).parent().parent().parent().parent().remove();
});

$("#page_content_inner").on('click', '.EditCoWorkerSegmetQuestion', function () {
    var question = $(this).attr("data-question");
    var id = $(this).attr("data-id");
    var QueData = $(this).attr("data-questionJsonString");
    $("#Createperformance").find("#CoworkerSegmentAddQuestionModel").find('#hidden_CoWorkerSegment_Question_id').val(id);
    $("#Createperformance").find("#CoworkerSegmentAddQuestionModel").find('#CoWorkerQuestion').val(question);
    QueData = QueData.split(",");
    $("#CoworkerSegmentAddQuestionModel").find('#CoWorker_HelpText').val(QueData[2]);
    $("#CoworkerSegmentAddQuestionModel").find('#CoWorker_Question_Type').val(QueData[0]);
    $("#CoworkerSegmentAddQuestionModel").find('#CoWorker_Question_CoreValue').val(QueData[1]);
});

$("#page_content_inner").on('click', '.EditMainCoWorkerSegmet', function () {
    var title = $(this).attr("data-title");
    var description = $(this).attr("data-description");
    var questionString = $(this).attr("data-questionjsonstring");
    var id = $(this).attr("data-id");
    $.ajax({
        url: constantSet.AddEditCoWorkerSegment,
        data: { isAddMode: false, Title: title, Description: description, JsonQuestionString: questionString, SegmentId: id },
        success: function (result) {
            $("#page_content_inner").find("#CoworkerSegmentModel").find(".modal-body").html("");
            $("#page_content_inner").find("#CoworkerSegmentModel").find(".modal-body").html(result);

            $("#page_content_inner").find("#CoworkerSegmentModel").find(".modal-body").find("#wizard").smartWizard({
                onLeaveStep: CoworkerleaveAStepCallback,
                onFinish: CoworkeronFinishCallback
            });
            $("#page_content_inner").find("#CoworkerSegmentModel").find('.buttonNext').addClass('btn btn-warning');
            $("#page_content_inner").find("#CoworkerSegmentModel").find('.buttonPrevious').addClass('btn btn-warning');
            $("#page_content_inner").find("#CoworkerSegmentModel").find('.buttonFinish').addClass('btn btn-success');
            $("#page_content_inner").find("#CoworkerSegmentModel").find('.buttonPrevious').hide();
            $("#page_content_inner").find("#CoworkerSegmentModel").find('.buttonFinish').hide();

            $("#Createperformance").find("#CoworkerSegmentModel").find('.CoWorkerSegmentQuestionNestable').nestable({
                maxDepth: 1,
            });
        }
    });
});

$("#page_content_inner").on('click', '.removeMainLiCoWorkerSegment', function () {
    $(this).parent().parent().parent().parent().remove();
});

//End Core Segment Javascript


//Save All Persormance Setting
$("#page_content_inner").on('click', '#btn_SavePerformance', function () {
    
    var IsError = false;
    var Review = $("#page_content_inner").find("#Review").val().trim();

    if (Review == "") {
        IsError = true;
        $("#validationmessageReview").show();
        $("#validationmessageReview").html('Review Name is Required.');
    }

    var CompletionDate = $("#page_content_inner").find("#CompletionDate").val();
    //if (CompletionDate == "") {
    //    IsError = true;
    //    $("#validationmessageCompletionDate").show();
    //    $("#validationmessageCompletionDate").html('Completion Date is Required.');
    //}


    var AnnualReview = false;
    if ($("#page_content_inner").find('#AnnualReview').is(":checked")) {
        if (CompletionDate == "") {
            IsError = true;
            $("#validationmessageCompletionDate").show();
            $("#validationmessageCompletionDate").html('Completion Date is Required.');
        }
        AnnualReview = true;
    }
    var Overall = false;
    if ($("#page_content_inner").find('#Overall').is(":checked")) {
        Overall = true;
    }
    var Core = false;
    if ($("#page_content_inner").find('#Core').is(":checked")) {
        Core = true;
    }
    var JobRole = false;
    if ($("#page_content_inner").find('#JobRole').is(":checked")) {
        JobRole = true;
    }
    var OverallScoreString = "";
    $(".OverallScore").each(function () {
        var Id = $(this).attr("data-id");
        var OverallScoreValues = $(this).find("#Overall_" + Id).val();
        if (OverallScoreValues == "") {
            IsError = true;
            $("#validationmessageOverall_" + Id).show();
            $("#validationmessageOverall_" + Id).html('Enter Overall Score');
        }
        else {
            if (OverallScoreValues != "") {
                if (OverallScoreString == "") {
                    OverallScoreString = OverallScoreValues;
                }
                else {
                    OverallScoreString += "^" + OverallScoreValues;
                }
            }
        }

    });
    CompanyId = [];
    $.each($("#page_content_inner").find("#drp-Company").parent().find(".selectlist-item"), function () {
        CompanyId.push($("#page_content_inner").find("#drp-Company").parent().find(".selectlist-select").find('option:contains(' + $(this).text().trim() + ')')[0].value);
    });
    LocationId = [];
    $.each($("#page_content_inner").find("#drp-Location").parent().find(".selectlist-item"), function () {
        LocationId.push($("#page_content_inner").find("#drp-Location").parent().find(".selectlist-select").find('option:contains(' + $(this).text().trim() + ')')[0].value);
    });
    BusinessId = [];
    $.each($("#page_content_inner").find("#drp-Business").parent().find(".selectlist-item"), function () {
        BusinessId.push($("#page_content_inner").find("#drp-Business").parent().find(".selectlist-select").find('option:contains(' + $(this).text().trim() + ')')[0].value);
    });
    DivisionId = [];
    $.each($("#page_content_inner").find("#drp-Division").parent().find(".selectlist-item"), function () {
        DivisionId.push($("#page_content_inner").find("#drp-Division").parent().find(".selectlist-select").find('option:contains(' + $(this).text().trim() + ')')[0].value);
    });
    PoolId = [];
    $.each($("#page_content_inner").find("#drp-Pool").parent().find(".selectlist-item"), function () {
        PoolId.push($("#page_content_inner").find("#drp-Pool").parent().find(".selectlist-select").find('option:contains(' + $(this).text().trim() + ')')[0].value);
    });
    FunctionId = [];
    $.each($("#page_content_inner").find("#drp-Function").parent().find(".selectlist-item"), function () {
        FunctionId.push($("#page_content_inner").find("#drp-Function").parent().find(".selectlist-select").find('option:contains(' + $(this).text().trim() + ')')[0].value);
    });
    JobTitleId = [];
    $.each($("#page_content_inner").find("#drp-JobTitle").parent().find(".selectlist-item"), function () {
        JobTitleId.push($("#page_content_inner").find("#drp-JobTitle").parent().find(".selectlist-select").find('option:contains(' + $(this).text().trim() + ')')[0].value);
    });
    EmploymentId = [];
    $.each($("#page_content_inner").find("#drp-Employment").parent().find(".selectlist-item"), function () {
        EmploymentId.push($("#page_content_inner").find("#drp-Employment").parent().find(".selectlist-select").find('option:contains(' + $(this).text().trim() + ')')[0].value);
    });

    //Core Segment Json 

    jsonCoreSegmentObj = [];
    var ids = 0;
    $.each($("#MainPageCoreSegmentQuestionList").find("li"), function () {
        ids++;
        var title = $(this).find(".EditMainCoreSegmet").attr("data-title");
        var idss = $(this).find(".EditMainCoreSegmet").attr("data-id");
        var description = $(this).find(".EditMainCoreSegmet").attr("data-description");
        var queationType = $(this).find(".EditMainCoreSegmet").attr("data-questionjsonstring");
        var totalQuestions = $(this).find(".coreSegmentTotalQuestion").html().split("Questions")[0].trim();
        var oneData = {
            CoreId: ids,
            Title: title,
            Description: description,
            QueationType: queationType,
            TotalQueastion: totalQuestions
        }
        jsonCoreSegmentObj.push(oneData);
    });
    var AllCoreSegmentJson = JSON.stringify(jsonCoreSegmentObj);
    //Job Roles Segment
    jsonJobRolesSegmentObj = [];
    var JobRoleIds = 0;
    $.each($("#MainPageJobRoleSegmentQuestionList").find("li"), function () {
        JobRoleIds++;
        var title = $(this).find(".EditMainJobSegmet").attr("data-title");
        var description = $(this).find(".EditMainJobSegmet").attr("data-description");
        var queationType = $(this).find(".EditMainJobSegmet").attr("data-questionjsonstring");
        var totalQuestions = $(this).find(".jobSegmentTotalQuestion").html().split("Questions")[0].trim();
        var oneData = {
            JobRoleIds: JobRoleIds,
            Title: title,
            Description: description,
            QueationType: queationType,
            TotalQueastion: totalQuestions,
        }
        jsonJobRolesSegmentObj.push(oneData);
    });
    var AllJobRolesSegmentJson = JSON.stringify(jsonJobRolesSegmentObj);

    //Customer Roles Segment json
    jsonCustomerSegmentObj = [];
    var CustoIds = 0;
    $.each($("#MainPageCustomerSegmentQuestionList").find("li"), function () {
        CustoIds++;
        var title = $(this).find(".EditMainCustomerSegmet").attr("data-title");
        var description = $(this).find(".EditMainCustomerSegmet").attr("data-description");
        var queationType = $(this).find(".EditMainCustomerSegmet").attr("data-questionjsonstring");
        var totalQuestions = $(this).find(".customerSegmentTotalQuestion").html().split("Questions")[0];
        var oneData = {
            CustoIds: CustoIds,
            Title: title,
            Description: description,
            QueationType: queationType,
            TotalQueastion: totalQuestions
        }
        jsonCustomerSegmentObj.push(oneData);
    });
    var AllCustomerSegmentJson = JSON.stringify(jsonCustomerSegmentObj);

    //Co-Worker Segment json
    jsonCoWorkerSegmentObj = [];
    var CowoIds = 0;
    $.each($("#MainPageCoworkerSegmentQuestionList").find("li"), function () {
        CowoIds++;
        var title = $(this).find(".EditMainCoWorkerSegmet").attr("data-title");
        var description = $(this).find(".EditMainCoWorkerSegmet").attr("data-description");
        var queationType = $(this).find(".EditMainCoWorkerSegmet").attr("data-questionjsonstring");
        var totalQuestions = $(this).find(".coWorkerSegmentTotalQuestion").html().split("Questions")[0].trim();
        var oneData = {
            CowoIds: CowoIds,
            Title: title,
            Description: description,
            QueationType: queationType,
            TotalQueastion: totalQuestions,
        }
        jsonCoWorkerSegmentObj.push(oneData);
    });
    var AllCoWorkerSegmentJson = JSON.stringify(jsonCoWorkerSegmentObj);

    if (IsError) {
        return false;
        $(".hrtoolLoader").hide();
        $(".modal-backdrop").hide();
    }
    else {
        var model = {
            Id: $("#page_content_inner").find("#PerformaceId").val(),
            ReviewText: Review,
            CompletionDate: CompletionDate,
            AnnualReview: AnnualReview,
            CompanyCSV: CompanyId.join(','),
            LocationCSV: LocationId.join(','),
            JobRoleCSV: JobTitleId.join(','),
            BusinessCSV: BusinessId.join(','),
            DivisionCSV: DivisionId.join(','),
            EmploymentTypeCSV: EmploymentId.join(','),
            PoolCSV: PoolId.join(','),
            FunctionCSV: FunctionId.join(','),
            RatingOverAll: Overall,
            RatingCore: Core,
            RatingJobRole: JobRole,
            OverallScoreJson: OverallScoreString,
            CoreSegmentJSON: AllCoreSegmentJson,
            JobRoleSegmentJSON: AllJobRolesSegmentJson,
            CoWorkerSegmentJSON: AllCoWorkerSegmentJson,
            CustomerSegmentJSON: AllCustomerSegmentJson
        }
        $.ajax({
            type: "POST",
            data: JSON.stringify(model),
            url: constantSet.SaveProject,
            contentType: "application/json",
            success: function (result) {
                $("#Createperformance").html('');
                $("#ListOfPerformance").html('');
                $("#ListOfPerformance").html(result);
                DataTableDesign();
                $(".hrtoolLoader").hide();
                $(".modal-backdrop").hide();
                if (hiddenId > 0) {
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

$('#ListOfPerformance').on('click', '.dataTr', function () {
    if ($(this).hasClass('dataTr')) {
        $('#PerformanceListtable tbody').find('tr.selected').removeClass('selected');
        $(this).addClass('selected');
        $("#ListOfPerformance").find(".btn-edit-Project").removeAttr('disabled');
        $("#ListOfPerformance").find(".btn-delete-Project").removeAttr('disabled');
    }
});

//$("#page_content_inner").on('click', '.EditCoreSegmet', function () {
//alert(1111);
//    var id = $(this).attr('data-id');
//    //Core Segmet
//    var newCoreSegment = $("#jsonSegmentSection").val();
//    var deseraliseCoreSegment = JSON.parse(newCoreSegment);
//    var coreSegemnt_i = 1;
//    $("#page_content_inner").find("#CoreSegmentModel").find("#Title").val(deseraliseCoreSegment[0].Title);
//    $("#page_content_inner").find("#CoreSegmentModel").find("#Description").val(deseraliseCoreSegment[0].Description)
//    var quetion = deseraliseCoreSegment[0].QueationType;
//    var coreSegemnt_i = 1;
//    if (quetion.indexOf(',') > 0) {
//        $.each(quetion.split(","), function () {
//            var isEmpty = $("#page_content_inner").find("#CoreSegmentQuestionList").html().trim();
//            if (isEmpty == "") {
//                var appendDataString = '<li class="dd-item" style="width: 95%;" data-id=' + coreSegemnt_i + ' id=' + coreSegemnt_i + '><div class="dd-handle" data-id="1" id="1">' + (this) + '</div><div style="width: 20px;float: right;margin: -30px 0 0 0;" data-toggle="modal" data-target="#CoreSegmentAddQuestionModel"><a class="EditCoreSegmetQueation" data-id=' + id + '><i class="material-icons uk-text-success" style="cursor: pointer;" >edit</i></a></div><div style="width: 20px;float: right;margin: -30px 22px 0 0;"><a><i class="material-icons uk-text-danger">delete</i></a></div></li>';
//                $("#page_content_inner").find("#CoreSegmentQuestionList").html(appendDataString);
//            }
//            else {
//                var appendDataString = '<li class="dd-item" style="width: 95%;" data-id=' + coreSegemnt_i + ' id=' + coreSegemnt_i + '><div class="dd-handle" data-id="1" id="1">' + (this) + '</div><div style="width: 20px;float: right;margin: -30px 0 0 0;" data-toggle="modal" data-target="#CoreSegmentAddQuestionModel"><a class="EditCoreSegmetQueation" data-id=' + id + '><i class="material-icons uk-text-success" style="cursor: pointer;" >edit</i></a></div><div style="width: 20px;float: right;margin: -30px 22px 0 0;"><a><i class="material-icons uk-text-danger">delete</i></a></div></li>';
//                $("#page_content_inner").find("#CoreSegmentQuestionList").append(appendDataString);
//            }
//            coreSegemnt_i++;
//        });
//    }
//    else {
//        var isEmpty = $("#page_content_inner").find("#CoreSegmentQuestionList").html().trim();
//        if (isEmpty == "") {
//            var appendDataString = '<li class="dd-item" style="width: 95%;" data-id=' + coreSegemnt_i + ' id=' + coreSegemnt_i + '><div class="dd-handle" data-id="1" id="1">' + (this) + '</div><div style="width: 20px;float: right;margin: -30px 0 0 0;" data-toggle="modal" data-target="#CoreSegmentAddQuestionModel"><a class="EditCoreSegmetQueation" data-id=' + id + '><i class="material-icons uk-text-success" style="cursor: pointer;" >edit</i></a></div><div style="width: 20px;float: right;margin: -30px 22px 0 0;"><a><i class="material-icons uk-text-danger">delete</i></a></div></li>';
//            $("#page_content_inner").find("#CoreSegmentQuestionList").html(appendDataString);
//        }
//        else {
//            var appendDataString = '<li class="dd-item" style="width: 95%;" data-id=' + coreSegemnt_i + ' id=' + coreSegemnt_i + '><div class="dd-handle" data-id="1" id="1">' + (this) + '</div><div style="width: 20px;float: right;margin: -30px 0 0 0;" data-toggle="modal" data-target="#CoreSegmentAddQuestionModel"><a class="EditCoreSegmetQueation" data-id=' + id + '><i class="material-icons uk-text-success" style="cursor: pointer;" >edit</i></a></div><div style="width: 20px;float: right;margin: -30px 22px 0 0;"><a><i class="material-icons uk-text-danger">delete</i></a></div></li>';
//            $("#page_content_inner").find("#CoreSegmentQuestionList").append(appendDataString);
//        }
//        coreSegemnt_i++;
//    }


//    //Job Roles

//    var jobrolesSegment = $("#jsonJobroleSegmentSection").val();
//    var deseralisejobrolesSegment = $.parseJSON(jobrolesSegment);
//    var jobSegemnt_i = 1;
//    var isEmpty = $("#page_content_inner").find("#MainPageJobRoleSegmentQuestionList").html().trim();
//    var Jobquetion = deseralisejobrolesSegment[0].QueationType;
//    if (Jobquetion.indexOf(',') > 0) {
//        $.each(Jobquetion.split(","), function () {

//            var isEmpty = $("#page_content_inner").find("#JobRolesSegmentQuestionList").html().trim();
//            if (isEmpty == "") {
//                var appendDataString = '<li class="dd-item" style="width: 95%;" data-id=' + jobSegemnt_i + ' id=' + jobSegemnt_i + '><div class="dd-handle" data-id="1" id="1">' + (this) + '</div><div style="width: 20px;float: right;margin: -30px 0 0 0;" data-toggle="modal" data-target="#CoreSegmentAddQuestionModel"><a class="EditCoreSegmetQueation" data-id=' + id + '><i class="material-icons uk-text-success" style="cursor: pointer;" >edit</i></a></div><div style="width: 20px;float: right;margin: -30px 22px 0 0;"><a><i class="material-icons uk-text-danger">delete</i></a></div></li>';
//                $("#page_content_inner").find("#JobRolesSegmentQuestionList").html(appendDataString);
//            }
//            else {
//                var appendDataString = '<li class="dd-item" style="width: 95%;" data-id=' + jobSegemnt_i + ' id=' + jobSegemnt_i + '><div class="dd-handle" data-id="1" id="1">' + (this) + '</div><div style="width: 20px;float: right;margin: -30px 0 0 0;" data-toggle="modal" data-target="#CoreSegmentAddQuestionModel"><a class="EditCoreSegmetQueation" data-id=' + id + '><i class="material-icons uk-text-success" style="cursor: pointer;" >edit</i></a></div><div style="width: 20px;float: right;margin: -30px 22px 0 0;"><a><i class="material-icons uk-text-danger">delete</i></a></div></li>';
//                $("#page_content_inner").find("#JobRolesSegmentQuestionList").append(appendDataString);
//            }
//            jobSegemnt_i++;
//        });
//    }
//    else {
//        var isEmpty = $("#page_content_inner").find("#JobRolesSegmentQuestionList").html().trim();
//        if (isEmpty == "") {
//            var appendDataString = '<li class="dd-item" style="width: 95%;" data-id=' + jobSegemnt_i + ' id=' + jobSegemnt_i + '><div class="dd-handle" data-id="1" id="1">' + (this) + '</div><div style="width: 20px;float: right;margin: -30px 0 0 0;" data-toggle="modal" data-target="#CoreSegmentAddQuestionModel"><a class="EditCoreSegmetQueation" data-id=' + id + '><i class="material-icons uk-text-success" style="cursor: pointer;" >edit</i></a></div><div style="width: 20px;float: right;margin: -30px 22px 0 0;"><a><i class="material-icons uk-text-danger">delete</i></a></div></li>';
//            $("#page_content_inner").find("#JobRolesSegmentQuestionList").html(appendDataString);
//        }
//        else {
//            var appendDataString = '<li class="dd-item" style="width: 95%;" data-id=' + jobSegemnt_i + ' id=' + jobSegemnt_i + '><div class="dd-handle" data-id="1" id="1">' + (this) + '</div><div style="width: 20px;float: right;margin: -30px 0 0 0;" data-toggle="modal" data-target="#CoreSegmentAddQuestionModel"><a class="EditCoreSegmetQueation" data-id=' + id + '><i class="material-icons uk-text-success" style="cursor: pointer;" >edit</i></a></div><div style="width: 20px;float: right;margin: -30px 22px 0 0;"><a><i class="material-icons uk-text-danger">delete</i></a></div></li>';
//            $("#page_content_inner").find("#JobRolesSegmentQuestionList").append(appendDataString);
//        }
//        jobSegemnt_i++;
//    }
//    //Customer

//    var CustomerrolesSegment = $("#jsonCustomerSegmentSection").val();
//    var deseraliseCustomerSegment = $.parseJSON(CustomerrolesSegment);
//    var CustomerSegemnt_i = 1;
//    var isEmpty = $("#page_content_inner").find("#MainPageCustomerSegmentQuestionList").html().trim();
//    var Customerquetion = deseraliseCustomerSegment[0].QueationType;
//    if (Customerquetion.indexOf(',') > 0) {
//        $.each(Customerquetion.split(","), function () {

//            var isEmpty = $("#page_content_inner").find("#CustomerSegmentQuestionList").html().trim();
//            if (isEmpty == "") {
//                var appendDataString = '<li class="dd-item" style="width: 95%;" data-id=' + CustomerSegemnt_i + ' id=' + CustomerSegemnt_i + '><div class="dd-handle" data-id="1" id="1">' + (this) + '</div><div style="width: 20px;float: right;margin: -30px 0 0 0;" data-toggle="modal" data-target="#CoreSegmentAddQuestionModel"><a class="EditCoreSegmetQueation" data-id=' + id + '><i class="material-icons uk-text-success" style="cursor: pointer;" >edit</i></a></div><div style="width: 20px;float: right;margin: -30px 22px 0 0;"><a><i class="material-icons uk-text-danger">delete</i></a></div></li>';
//                $("#page_content_inner").find("#CustomerSegmentQuestionList").html(appendDataString);
//            }
//            else {
//                var appendDataString = '<li class="dd-item" style="width: 95%;" data-id=' + CustomerSegemnt_i + ' id=' + CustomerSegemnt_i + '><div class="dd-handle" data-id="1" id="1">' + (this) + '</div><div style="width: 20px;float: right;margin: -30px 0 0 0;" data-toggle="modal" data-target="#CoreSegmentAddQuestionModel"><a class="EditCoreSegmetQueation" data-id=' + id + '><i class="material-icons uk-text-success" style="cursor: pointer;" >edit</i></a></div><div style="width: 20px;float: right;margin: -30px 22px 0 0;"><a><i class="material-icons uk-text-danger">delete</i></a></div></li>';
//                $("#page_content_inner").find("#CustomerSegmentQuestionList").append(appendDataString);
//            }
//            CustomerSegemnt_i++;
//        });
//    }
//    else {
//        var isEmpty = $("#page_content_inner").find("#CustomerSegmentQuestionList").html().trim();
//        if (isEmpty == "") {
//            var appendDataString = '<li class="dd-item" style="width: 95%;" data-id=' + CustomerSegemnt_i + ' id=' + CustomerSegemnt_i + '><div class="dd-handle" data-id="1" id="1">' + (this) + '</div><div style="width: 20px;float: right;margin: -30px 0 0 0;" data-toggle="modal" data-target="#CoreSegmentAddQuestionModel"><a class="EditCoreSegmetQueation" data-id=' + id + '><i class="material-icons uk-text-success" style="cursor: pointer;" >edit</i></a></div><div style="width: 20px;float: right;margin: -30px 22px 0 0;"><a><i class="material-icons uk-text-danger">delete</i></a></div></li>';
//            $("#page_content_inner").find("#CustomerSegmentQuestionList").html(appendDataString);
//        }
//        else {
//            var appendDataString = '<li class="dd-item" style="width: 95%;" data-id=' + CustomerSegemnt_i + ' id=' + CustomerSegemnt_i + '><div class="dd-handle" data-id="1" id="1">' + (this) + '</div><div style="width: 20px;float: right;margin: -30px 0 0 0;" data-toggle="modal" data-target="#CoreSegmentAddQuestionModel"><a class="EditCoreSegmetQueation" data-id=' + id + '><i class="material-icons uk-text-success" style="cursor: pointer;" >edit</i></a></div><div style="width: 20px;float: right;margin: -30px 22px 0 0;"><a><i class="material-icons uk-text-danger">delete</i></a></div></li>';
//            $("#page_content_inner").find("#CustomerSegmentQuestionList").append(appendDataString);
//        }
//        CustomerSegemnt_i++;
//    }

//    //Coworker
//    var CoworkerSegment = $("#jsonCoWorkerSegmentSection").val();
//    var deseraliseCoworkerSegment = $.parseJSON(CoworkerSegment);
//    var CoworkerSegemnt_i = 1;
//    var isEmpty = $("#page_content_inner").find("#MainPageCoworkerSegmentQuestionList").html().trim();
//    var Coworkerquetion = deseraliseCoworkerSegment[0].QueationType;
//    if (Coworkerquetion.indexOf(',') > 0) {
//        $.each(Coworkerquetion.split(","), function () {

//            var isEmpty = $("#page_content_inner").find("#CoworkerSegmentQuestionList").html().trim();
//            if (isEmpty == "") {
//                var appendDataString = '<li class="dd-item" style="width: 95%;" data-id=' + CoworkerSegemnt_i + ' id=' + CoworkerSegemnt_i + '><div class="dd-handle" data-id="1" id="1">' + (this) + '</div><div style="width: 20px;float: right;margin: -30px 0 0 0;" data-toggle="modal" data-target="#CoreSegmentAddQuestionModel"><a class="EditCoreSegmetQueation" data-id=' + id + '><i class="material-icons uk-text-success" style="cursor: pointer;" >edit</i></a></div><div style="width: 20px;float: right;margin: -30px 22px 0 0;"><a><i class="material-icons uk-text-danger">delete</i></a></div></li>';
//                $("#page_content_inner").find("#CoworkerSegmentQuestionList").html(appendDataString);
//            }
//            else {
//                var appendDataString = '<li class="dd-item" style="width: 95%;" data-id=' + CoworkerSegemnt_i + ' id=' + CoworkerSegemnt_i + '><div class="dd-handle" data-id="1" id="1">' + (this) + '</div><div style="width: 20px;float: right;margin: -30px 0 0 0;" data-toggle="modal" data-target="#CoreSegmentAddQuestionModel"><a class="EditCoreSegmetQueation" data-id=' + id + '><i class="material-icons uk-text-success" style="cursor: pointer;" >edit</i></a></div><div style="width: 20px;float: right;margin: -30px 22px 0 0;"><a><i class="material-icons uk-text-danger">delete</i></a></div></li>';
//                $("#page_content_inner").find("#CoworkerSegmentQuestionList").append(appendDataString);
//            }
//            CoworkerSegemnt_i++;
//        });
//    }
//    else {
//        var isEmpty = $("#page_content_inner").find("#CoworkerSegmentQuestionList").html().trim();
//        if (isEmpty == "") {
//            var appendDataString = '<li class="dd-item" style="width: 95%;" data-id=' + CoworkerSegemnt_i + ' id=' + CoworkerSegemnt_i + '><div class="dd-handle" data-id="1" id="1">' + (this) + '</div><div style="width: 20px;float: right;margin: -30px 0 0 0;" data-toggle="modal" data-target="#CoreSegmentAddQuestionModel"><a class="EditCoreSegmetQueation" data-id=' + id + '><i class="material-icons uk-text-success" style="cursor: pointer;" >edit</i></a></div><div style="width: 20px;float: right;margin: -30px 22px 0 0;"><a><i class="material-icons uk-text-danger">delete</i></a></div></li>';
//            $("#page_content_inner").find("#CoworkerSegmentQuestionList").html(appendDataString);
//        }
//        else {
//            var appendDataString = '<li class="dd-item" style="width: 95%;" data-id=' + CoworkerSegemnt_i + ' id=' + CoworkerSegemnt_i + '><div class="dd-handle" data-id="1" id="1">' + (this) + '</div><div style="width: 20px;float: right;margin: -30px 0 0 0;" data-toggle="modal" data-target="#CoreSegmentAddQuestionModel"><a class="EditCoreSegmetQueation" data-id=' + id + '><i class="material-icons uk-text-success" style="cursor: pointer;" >edit</i></a></div><div style="width: 20px;float: right;margin: -30px 22px 0 0;"><a><i class="material-icons uk-text-danger">delete</i></a></div></li>';
//            $("#page_content_inner").find("#CoworkerSegmentQuestionList").append(appendDataString);
//        }
//        CoworkerSegemnt_i++;
//    }



//});


$("#page_content_inner").on('click', '#CopySegment', function () {
    var CopyId = $("#page_content_inner").find("#drp-CopyFrom").val();
    var Review = $("#page_content_inner").find("#Review").val().trim();
    var CompletionDate = $("#page_content_inner").find("#CompletionDate").val();
    var AnnualReview = false;
    if ($("#page_content_inner").find('#AnnualReview').is(":checked")) {
        AnnualReview = true;
    }
    CompanyId = [];
    $.each($("#page_content_inner").find("#drp-Company").parent().find(".selectlist-item"), function () {
        CompanyId.push($("#page_content_inner").find("#drp-Company").parent().find(".selectlist-select").find('option:contains(' + $(this).text().trim() + ')')[0].value);
    });
    LocationId = [];
    $.each($("#page_content_inner").find("#drp-Location").parent().find(".selectlist-item"), function () {
        LocationId.push($("#page_content_inner").find("#drp-Location").parent().find(".selectlist-select").find('option:contains(' + $(this).text().trim() + ')')[0].value);
    });
    BusinessId = [];
    $.each($("#page_content_inner").find("#drp-Business").parent().find(".selectlist-item"), function () {
        BusinessId.push($("#page_content_inner").find("#drp-Business").parent().find(".selectlist-select").find('option:contains(' + $(this).text().trim() + ')')[0].value);
    });
    DivisionId = [];
    $.each($("#page_content_inner").find("#drp-Division").parent().find(".selectlist-item"), function () {
        DivisionId.push($("#page_content_inner").find("#drp-Division").parent().find(".selectlist-select").find('option:contains(' + $(this).text().trim() + ')')[0].value);
    });
    PoolId = [];
    $.each($("#page_content_inner").find("#drp-Pool").parent().find(".selectlist-item"), function () {
        PoolId.push($("#page_content_inner").find("#drp-Pool").parent().find(".selectlist-select").find('option:contains(' + $(this).text().trim() + ')')[0].value);
    });
    FunctionId = [];
    $.each($("#page_content_inner").find("#drp-Function").parent().find(".selectlist-item"), function () {
        FunctionId.push($("#page_content_inner").find("#drp-Function").parent().find(".selectlist-select").find('option:contains(' + $(this).text().trim() + ')')[0].value);
    });
    JobTitleId = [];
    $.each($("#page_content_inner").find("#drp-JobTitle").parent().find(".selectlist-item"), function () {
        JobTitleId.push($("#page_content_inner").find("#drp-JobTitle").parent().find(".selectlist-select").find('option:contains(' + $(this).text().trim() + ')')[0].value);
    });
    EmploymentId = [];
    $.each($("#page_content_inner").find("#drp-Employment").parent().find(".selectlist-item"), function () {
        EmploymentId.push($("#page_content_inner").find("#drp-Employment").parent().find(".selectlist-select").find('option:contains(' + $(this).text().trim() + ')')[0].value);
    });
    if (CopyId == "") {
    }
    else {
        var model = {
            Id: $("#page_content_inner").find("#PerformaceId").val(),
            CopyId: CopyId,
            ReviewText: Review,
            CompletionDate: CompletionDate,
            AnnualReview: AnnualReview,
            CompanyCSV: CompanyId.join(','),
            LocationCSV: LocationId.join(','),
            JobRoleCSV: JobTitleId.join(','),
            BusinessCSV: BusinessId.join(','),
            DivisionCSV: DivisionId.join(','),
            EmploymentTypeCSV: EmploymentId.join(','),
            PoolCSV: PoolId.join(','),
            FunctionCSV: FunctionId.join(','),
        }
        $.ajax({
            type: "POST",
            data: JSON.stringify(model),
            url: constantSet.CopyFromUrl,
            contentType: "application/json",
            success: function (result) {
                $("#page_content_inner").find('#Createperformance').html('');
                $("#page_content_inner").find('#Createperformance').html(result);
                $("#page_content_inner").find('#ListOfPerformance').html('');
                $("#tableDivProject").find("#btn_SavePerformance").html("Save");
                $("#page_content_inner").find("#Createperformance").find("#drp-Company").selectList({
                    onAdd: function (select, value, text) {
                        if (value == "All") {
                            $.each($("#page_content_inner").find("#Createperformance").find("#drp-Company").parent().find(".selectlist-item"), function () {
                                if ($(this).html() != "All") {
                                    $(this).click();
                                }
                            });
                        }
                        else {
                            $.each($("#page_content_inner").find("#Createperformance").find("#drp-Company").parent().find(".selectlist-item"), function () {
                                if ($(this).html() == "All") {
                                    $(this).click();
                                }
                            });
                        }
                    },
                });
                $("#page_content_inner").find("#Createperformance").find("#drp-Location").selectList({
                    onAdd: function (select, value, text) {
                        if (value == "All") {
                            $.each($("#page_content_inner").find("#Createperformance").find("#drp-Location").parent().find(".selectlist-item"), function () {
                                if ($(this).html() != "All") {
                                    $(this).click();
                                }
                            });
                        }
                        else {
                            $.each($("#page_content_inner").find("#Createperformance").find("#drp-Location").parent().find(".selectlist-item"), function () {
                                if ($(this).html() == "All") {
                                    $(this).click();
                                }
                            });
                        }
                    },
                });
                $("#page_content_inner").find("#Createperformance").find("#drp-Business").selectList({
                    onAdd: function (select, value, text) {
                        if (value == "All") {
                            $.each($("#page_content_inner").find("#Createperformance").find("#drp-Business").parent().find(".selectlist-item"), function () {
                                if ($(this).html() != "All") {
                                    $(this).click();
                                }
                            });
                        }
                        else {
                            $.each($("#page_content_inner").find("#Createperformance").find("#drp-Business").parent().find(".selectlist-item"), function () {
                                if ($(this).html() == "All") {
                                    $(this).click();
                                }
                            });
                        }
                    },
                });
                $("#page_content_inner").find("#Createperformance").find("#drp-Division").selectList({
                    onAdd: function (select, value, text) {
                        if (value == "All") {
                            $.each($("#page_content_inner").find("#Createperformance").find("#drp-Division").parent().find(".selectlist-item"), function () {
                                if ($(this).html() != "All") {
                                    $(this).click();
                                }
                            });
                        }
                        else {
                            $.each($("#page_content_inner").find("#Createperformance").find("#drp-Division").parent().find(".selectlist-item"), function () {
                                if ($(this).html() == "All") {
                                    $(this).click();
                                }
                            });
                        }
                    },
                });
                $("#page_content_inner").find("#Createperformance").find("#drp-Pool").selectList({
                    onAdd: function (select, value, text) {
                        if (value == "All") {
                            $.each($("#page_content_inner").find("#Createperformance").find("#drp-Pool").parent().find(".selectlist-item"), function () {
                                if ($(this).html() != "All") {
                                    $(this).click();
                                }
                            });
                        }
                        else {
                            $.each($("#page_content_inner").find("#Createperformance").find("#drp-Pool").parent().find(".selectlist-item"), function () {
                                if ($(this).html() == "All") {
                                    $(this).click();
                                }
                            });
                        }
                    },
                });
                $("#page_content_inner").find("#Createperformance").find("#drp-Function").selectList({
                    onAdd: function (select, value, text) {
                        if (value == "All") {
                            $.each($("#page_content_inner").find("#Createperformance").find("#drp-Function").parent().find(".selectlist-item"), function () {
                                if ($(this).html() != "All") {
                                    $(this).click();
                                }
                            });
                        }
                        else {
                            $.each($("#page_content_inner").find("#Createperformance").find("#drp-Function").parent().find(".selectlist-item"), function () {
                                if ($(this).html() == "All") {
                                    $(this).click();
                                }
                            });
                        }
                    },
                });
                $("#page_content_inner").find("#Createperformance").find("#drp-JobTitle").selectList({
                    onAdd: function (select, value, text) {
                        if (value == "All") {
                            $.each($("#page_content_inner").find("#Createperformance").find("#drp-JobTitle").parent().find(".selectlist-item"), function () {
                                if ($(this).html() != "All") {
                                    $(this).click();
                                }
                            });
                        }
                        else {
                            $.each($("#page_content_inner").find("#Createperformance").find("#drp-JobTitle").parent().find(".selectlist-item"), function () {
                                if ($(this).html() == "All") {
                                    $(this).click();
                                }
                            });
                        }
                    },
                });
                $("#page_content_inner").find("#Createperformance").find("#drp-Employment").selectList({
                    onAdd: function (select, value, text) {
                        if (value == "All") {
                            $.each($("#page_content_inner").find("#Createperformance").find("#drp-Employment").parent().find(".selectlist-item"), function () {
                                if ($(this).html() != "All") {
                                    $(this).click();
                                }
                            });
                        }
                        else {
                            $.each($("#page_content_inner").find("#Createperformance").find("#drp-Employment").parent().find(".selectlist-item"), function () {
                                if ($(this).html() == "All") {
                                    $(this).click();
                                }
                            });
                        }
                    },
                });
                $("#CompletionDate").Zebra_DatePicker({
                    showButtonPanel: false,
                    format: 'd-m-Y',
                    onSelect: function () {
                        $("#validationmessageCompletionDate").hide();
                        var fromDate = $('#CompletionDate').val();
                    }
                });

                $('.CoreSegmentNestable').nestable({
                    maxDepth: 1,
                });

                $('.JobRoleSegmentNestable').nestable({
                    maxDepth: 1,
                });

                $('.CustomerSegmentNestable').nestable({
                    maxDepth: 1,
                });

                $('.CoworkerSegmentNestable').nestable({
                    maxDepth: 1,
                });

                $(".hrtoolLoader").hide();
                $(".modal-backdrop").hide();
            }
        });
    }
});


$("#Createperformance").on('click', '#btn_CancelPerformance', function () {
    $(".hrtoolLoader").show();

    $.ajax({
        url: constantSet.List,
        success: function (data) {
            $("#Createperformance").html('');
            $("#ListOfPerformance").html('');
            $("#ListOfPerformance").html(data);
            DataTableDesign();
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
        }
    });
});