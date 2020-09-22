$(document).ready(function () {
    DataTableDesign();
    $('#tableDiv').find('.demo2').colorpicker();
});

function DataTableDesign() {
    $('#example1 tfoot tr').appendTo('#example1 thead');
    var table = $('#example1').DataTable({
        "sDom": '<"top"i>rt<"bottom"flp><"clear">',
        "scrollX": true
    });
    $('#tableDiv').find('.dataTables_filter').hide();
    $('#tableDiv').find('.dataTables_info').hide();

    $("#tableDiv thead .SearchTMSSettingName").keyup(function () {
        table.column(0).search(this.value).draw();
    });

}

//table 
$('#tableDiv').on('click', '.dataTr', function () {
    if ($(this).hasClass('dataTr')) {
        $('#example1 tbody').find('tr.selected').removeClass('selected');
        $(this).addClass('selected');
        $("#tableDiv").find(".btn-edit-TMSSetting").removeAttr('disabled');
        $("#tableDiv").find(".btn-delete-TMSSetting").removeAttr('disabled');
    }
});

$("#tableDiv").on('click', '.btn-Refresh-TMSSetting', function () {
    window.location.reload();
});

$("#tableDiv").on('click', '.btn-ClearSorting-TMSSetting', function () {
    window.location.reload();
});

$("#tableDiv").on('click', '.btn-clearFilter-TMSSetting', function () {
    window.location.reload();
});

//Add/Edit TMS Setting 
$("#tableDiv").on('click', '.btn-add-TMSSetting', function () {
    $(".hrtoolLoader").show();
    $.ajax({
        url: constantTMSSettings.addEdit,
        data: { Id: 0 },
        success: function (data) {
            $("#tableDiv").html('');
            $("#tableDiv").html(data);
            $("#MainPageStepSegmentList").sortable({
                placeholder: "ui-state-highlight",
                helper: 'clone',
                sort: function (e, ui) {
                    $(ui.placeholder).html(Number($("#MainPageStepSegmentList > li:visible").index(ui.placeholder)) + 1);
                },
                update: function (event, ui) {
                    var $lis = $(this).children('li');
                    $lis.each(function () {
                        var $li = $(this);
                        var newVal = $(this).index() + 1;
                        $(this).children('.sortable-number').html(newVal);
                        $(this).children('#item_display_order').val(newVal);
                    });
                },
                items: ':not(.static)',
                start: function () {
                    $('.static', this).each(function () {
                        var $this = $(this);
                        $this.data('pos', $this.index());
                    });
                },
                change: function () {
                    $sortable = $(this);
                    $statics = $('.static', this).detach();
                    //$helper = $('<li class="ui-state-default"><span class="ui-icon ui-icon-arrowthick-2-n-s sortable-number" style="display: initial;"><i class="fa fa-square" style="position: inherit; display: initial;"></i><a style="position: inherit; display: initial;"><i class="material-icons uk-text-success" style="position: inherit; display: initial;">edit</i></a><a style="position: inherit; display: initial;"><i class="material-icons uk-text-danger" style="position: inherit; display: initial;">delete</i><a></span></li>').prependTo(this);
                    $helper = $('<li></li>').prependTo(this);
                    $statics.each(function () {

                        var $this = $(this);
                        var target = $this.data('pos');

                        $this.insertAfter($('li', $sortable).eq(target));
                    });
                    $helper.remove();
                }

            });
            $("#MainPageStepSegmentList").disableSelection();
            $(".titletext").html('');
            $(".titletext").html("<div>Recruitment Process<p>Here you can create and define different categories for your recruitment pipeline</p></div>")
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();

        }
    });
});

//Save Steps 
$("#tableDiv").on('click', '#btn-submit-Steps', function () {
    var isError = false;
    var RecId = $("#RecruitmentProcessId").val();
    var existID = $("#tableDiv").find("#StepsSegmentSection").find("#stepCountID").val();
    if (existID == undefined || existID == "") {

        var column1RelArray = [];
        $('#MainPageStepSegmentList li').each(function () {
            column1RelArray.push($(this).data('id'));
        });
        column1RelArray
        var max = Math.max.apply(null, column1RelArray); // get the max of the array
        column1RelArray.splice(column1RelArray.indexOf(max), 1); // remove max from the array
        var useSortID = Math.max.apply(null, column1RelArray);


        var SortId = useSortID;
        var id = useSortID;
        var stepName = $("#tableDiv").find('#StepName').val();
        // var name = $('#MainPageStepSegmentList li:last-child').data('name');
        if (id == undefined || id == 1) {
            id = $('#defaultStepList li:nth-last-child(2)').data('id');
            SortId = $('#defaultStepList li:nth-last-child(2)').data('id');
            var colorCode = $("#tableDiv").find('#StepColor').val();
            if (stepName == "") {
                isError = true;
                $("#tableDiv").find("#validationmessageStepName").show();
                return false
            }
            else {
                jsonStepSegmentObj = [];
                $.each($("#defaultStepList").find("li"), function () {
                    if (stepName == $(this).find('.AddStepName').val()) {
                        jsonStepSegmentObj.push(stepName);
                    }
                });
                if (jsonStepSegmentObj.length == 0) {
                    id++;
                    var t = $("#defaultStepList").find("li").length;
                    var appendDataString =

                        '<li class="ui-state-default static-item" data-id=' + id + ' id=' + id + '> <div class="spanel"> <div class="rowsm"> <div class="col-md-1 col1"> <i class="fa fa-long-arrow-up"></i><i class="fa fa-long-arrow-down"></i></i></div><div class="col-md-11 col11"> <span class="ui-icon ui-icon-arrowthick-2-n-s sortable-number" style="display: initial;">' + t + '&nbsp; <b class="liName">' + stepName + '</b><input type="hidden" class="AddStepName" Id="AddStepName_' + id + '" value="' + stepName + '"><input type="hidden" class="AddStepColor" Id="AddStepColor_' + id + '"  value=' + colorCode + '> </span> <div class="steps_icon"> <a> <i class="fa fa-square" style="color:' + colorCode + ';position: inherit; display: initial;"></i></a><a  onclick="editAccepted(' + id + ')" data-toggle="modal" data-target="#StepSegmentModel" style="position: inherit; display: initial;"><i class="material-icons uk-text-success" style="position: inherit; display: initial;">edit</i></a><a onclick="deleteLi(' + id + ')" style="position: inherit; display: initial;"><i class="material-icons uk-text-danger" style="position: inherit; display: initial;">delete</i></a></div></div></div></div></li>';

                    $("#page_content_inner").find("#defaultStepList").find("li:last-child").before(appendDataString);
                    appendDataString = "";
                    $("#page_content_inner").find(".close").click();
                    var stepName = $("#tableDiv").find('#StepName').val('');
                    var isError = false;
                    $("#tableDiv").find("#validationmessageStepName").hide();
                    $("#tableDiv").find("#ExistStepName").hide();
                    id++;
                    $("#page_content_inner").find("#defaultStepList").find("li:last-child")["0"].outerHTML = '<li class="ui-state-default static static-item" data-id=' + id + '> <div class="spanel"> <div class="rowsm"> <div class="col-md-1 col1"></div> <div class="col-md-11 col11"> <span class="ui-icon ui-icon-arrowthick-2-n-s sortable-number ui-sortable-handle" style="display: initial;">' + id + '&nbsp; <b class="liName">Accepted</b><input type="hidden" class="AddStepName" Id="AddStepName_' + id + '" value="Accepted"><input type="hidden" class="AddStepColor" Id="AddStepColor_' + id + '"  value="#00CC00"></span> <div class="steps_icon"> <a> <i class="fa fa-square ui-sortable-handle" style="color: #00CC00; position: inherit; display: initial;"></i></a><a onclick="editAccepted(' + id + ')" data-toggle="modal" data-target="#StepSegmentModel" style="position: inherit; display: initial;" class="ui-sortable-handle"><i class="material-icons uk-text-success ui-sortable-handle" style="position: inherit; display: initial;">edit</i></a> </div></div> </div></div></li>';
                }
                else {
                    isError = true;
                    $("#tableDiv").find("#ExistStepName").show();
                    return false

                }
            }
        }
        else {
            var stepName = $("#tableDiv").find('#StepName').val();
            var colorCode = $("#tableDiv").find('#StepColor').val();
            if (stepName == "") {
                isError = true;
                $("#tableDiv").find("#validationmessageStepName").show();
                return false
            }
            else {
                jsonStepSegmentObj = [];
                $.each($("#MainPageStepSegmentList").find("li"), function () {
                    if (stepName == $(this).find('.AddStepName').val()) {

                        jsonStepSegmentObj.push(stepName);
                    }
                });
                if (jsonStepSegmentObj.length == 0) {
                    id++;
                    var t = $("#MainPageStepSegmentList").find("li").length;
                    // SortId++;
                    var appendDataString;
                    var appendDataString = '<li class="ui-state-default static-item" data-id=' + id + ' id=' + id + '> <div class="spanel"> <div class="rowsm"> <div class="col-md-1 col1"><i class="fa fa-long-arrow-up tmscursor-size" onclick="swapData(' + id + ',' + id + ',' + RecId + ',1)"></i><i class="fa fa-long-arrow-down tmscursor-size" onclick="swapData(' + id + ',' + id + ',' + RecId + ',0)"></i></div> <div class="col-md-11 col11"> <span class="ui-icon ui-icon-arrowthick-2-n-s sortable-number" style="display: initial;">' + t + '&nbsp; <b class="liName">' + stepName + '</b><input type="hidden" class="AddStepName" Id="AddStepName_' + id + '" value="' + stepName + '"><input type="hidden" class="AddStepColor" Id="AddStepColor_' + id + '"  value=' + colorCode + '> <input type="hidden" class="AddID"  value=' + id + '><input type="hidden" class="SortID" value=' + id + '></span> <div class="steps_icon"> <a> <i class="fa fa-square" style="color:' + colorCode + ';position: inherit; display: initial;"></i></a><a onclick="editAccepted(' + id + ')" data-toggle="modal" data-target="#StepSegmentModel" style="position: inherit; display: initial;"><i class="material-icons uk-text-success" style="position: inherit; display: initial;">edit</i></a><a onclick="deleteLi(' + id + ')" style="position: inherit; display: initial;"><i class="material-icons uk-text-danger" style="position: inherit; display: initial;">delete</i></a></div></div></div></div></li>';
                    $("#page_content_inner").find("#MainPageStepSegmentList").find("li:last-child").before(appendDataString);
                    appendDataString = "";
                    $("#page_content_inner").find(".close").click();
                    var stepName = $("#tableDiv").find('#StepName').val('');
                    var isError = false;
                    $("#tableDiv").find("#validationmessageStepName").hide();
                    $("#tableDiv").find("#ExistStepName").hide();
                    id++;
                    t++;
                    $("#page_content_inner").find("#MainPageStepSegmentList").find("li:last-child")["0"].outerHTML = '<li class="ui-state-default static static-item" data-id=' + id + '> <div class="spanel"> <div class="rowsm"> <div class="col-md-1 col1"></div> <div class="col-md-11 col11"> <span class="ui-icon ui-icon-arrowthick-2-n-s sortable-number ui-sortable-handle" style="display: initial;">' + t + '&nbsp; <b class="liName">Accepted</b> <input type="hidden" class="AddStepName" Id="AddStepName_' + id + '" value="Accepted"><input type="hidden" class="AddStepColor" Id="AddStepColor_' + id + '"  value="#00CC00"><input type="hidden" class="AddID"  value=' + id + '><input type="hidden" class="SortID" value=' + id + '></span> <div class="steps_icon"> <a> <i class="fa fa-square ui-sortable-handle" style="color: #00CC00; position: inherit; display: initial;"></i></a><a onclick="editAccepted(' + id + ')" data-toggle="modal" data-target="#StepSegmentModel" style="position: inherit; display: initial;" class="ui-sortable-handle"><i class="material-icons uk-text-success ui-sortable-handle" style="position: inherit; display: initial;">edit</i></a> </div></div> </div></div></li>';
                }
                else {
                    isError = true;
                    $("#tableDiv").find("#ExistStepName").show();
                    return false

                }
            }
        }

    }
    else {
        var stepName = $("#tableDiv").find('#StepName').val();
        var colorCode = $("#tableDiv").find('#StepColor').val();
        if (stepName == "") {
            isError = true;
            $("#tableDiv").find("#validationmessageStepName").show();
            return false
        }
        else {
            debugger;
            if ($("#MainPageStepSegmentList").find("li").length > 0) {
                $("#MainPageStepSegmentList").find('li[data-id=' + existID + ']').find(".liName").html('');
                $("#MainPageStepSegmentList").find('li[data-id=' + existID + ']').find(".liName").html(stepName);
                $("#MainPageStepSegmentList").find('li[data-id=' + existID + ']').find("#AddStepColor_" + existID).val('');
                $("#MainPageStepSegmentList").find('li[data-id=' + existID + ']').find("#AddStepColor_" + existID).val(colorCode);
                $("#MainPageStepSegmentList").find('li[data-id=' + existID + ']').find("#AddStepName_" + stepName).val('');
                $("#MainPageStepSegmentList").find('li[data-id=' + existID + ']').find("#AddStepName_" + stepName).val(stepName);
                $("#MainPageStepSegmentList").find('li[data-id=' + existID + ']').find(".fa-square").css("color", colorCode)
                $("#tableDiv").find("#StepSegmentModel").find("#modelClose").click();

            }

            else {
                $("#defaultStepList").find('li[data-id=' + existID + ']').find(".liName").html('');
                $("#defaultStepList").find('li[data-id=' + existID + ']').find(".liName").html(stepName);
                $("#defaultStepList").find('li[data-id=' + existID + ']').find("#AddStepColor_" + existID).val('');
                $("#defaultStepList").find('li[data-id=' + existID + ']').find("#AddStepColor_" + existID).val(colorCode);
                $("#defaultStepList").find('li[data-id=' + existID + ']').find("#AddStepName_" + stepName).val('');
                $("#defaultStepList").find('li[data-id=' + existID + ']').find("#AddStepName_" + stepName).val(stepName);
                $("#defaultStepList").find('li[data-id=' + existID + ']').find(".fa-square").css("color", colorCode)
                $("#tableDiv").find("#StepSegmentModel").find("#modelClose").click();

            }

        }
    }
})

$("#tableDiv").on('keyup', '#StepName', function (e) {
    var isError = false;
    $("#tableDiv").find("#validationmessageStepName").hide();
    $("#tableDiv").find("#ExistStepName").hide();

});

//Competency Save
$("#tableDiv").on('click', '#btn-submit-Competencies', function () {
    var isError = false;
    var RecId = $("#RecruitmentProcessId").val();
    var exitID = $("#tableDiv").find("#CompetencieSegmentModel").find("#addCompetencyID").val();
    if (exitID == undefined || exitID == "") {
        var totalLength = $("#mainPageCompetencieSegmentList").find("li").length;
        totalLength++;

        var column1RelArray = [];
        $('#mainPageCompetencieSegmentList li').each(function () {
            column1RelArray.push($(this).data('id'));
        });
        column1RelArray
        var useSortID = Math.max.apply(null, column1RelArray); // get the max of the array

        var id = useSortID;
        if (id == undefined || id == -Infinity) {
            id = 0;
        }
        var Name = $("#tableDiv").find('#txt_CompetencyName').val();

        var descriprtion = $("#tableDiv").find('#txt_CDescription').val();
        if (Name == "") {
            isError = true;
            $("#tableDiv").find("#validationmessageCompetencyName").show();
            return false;
        }
        else {
            jsonCompetencySegmentObj = [];
            $.each($("#mainPageCompetencieSegmentList").find("li"), function () {
                if (Name == $(this).find('.AddCompetencyName').val()) {
                    jsonCompetencySegmentObj.push(Name);
                }
            });
            if (jsonCompetencySegmentObj.length == 0) {
                id++;
                var appendDataString;
                if (RecId == 0 || RecId == "undefined" || RecId == "") {
                    appendDataString = '<li class="ui-state-default" data-id=' + id + ' id=' + id + '><div class="uk-nestable-panel"><div class="rowsm"><div class="col-md-1 col1"><i class="fa fa-long-arrow-up tmscursor-size" onclick="swapCompData(' + id + ',' + id + ',' + RecId + ',1)"></i><i class="fa fa-long-arrow-down tmscursor-size" onclick="swapCompData(' + id + ',' + id + ',' + RecId + ',0)"></i></div><div class="col-md-11 col11"><span class="ui-icon-arrowthick-2-n-s sortable-number" style="float:left;margin-left:0; font-weight: bold;">' + totalLength + '&nbsp;<b class="liComName">' + Name + '</b></span><div class="compet"><a onclick="editCompetency(' + id + ')" data-toggle="modal" data-target="#CompetencieSegmentModel" style="position: inherit;display: initial;margin-right: 10px;margin-left: 10px;"><i class="material-icons uk-text-success" style="position: inherit; display: initial;">edit</i></a><a  onclick="deleteCom(' + id + ')" style="position: inherit; display: initial;"><i class="material-icons uk-text-danger" style="position: inherit; display: initial;">delete</i></a></div><p class="description">' + descriprtion + '<input type="hidden" class="AddCompetencyName" Id="AddName_' + id + '" value="' + Name + '"><input type="hidden" class="AddDescripetion" Id="AddDescription_' + id + '"  value=' + descriprtion + '><input type="hidden" class="CompetencyAddID" value=' + id + '><input type="hidden" class="CompetencySortID" value=' + id + '></p></div></div></div></li>';
                    //appendDataString = '<li class="ui-state-default" data-id=' + id + ' id=' + id + '><div class="uk-nestable-panel"><div class="rowsm"><div class="col-md-1 col1"></i></div><div class="col-md-11 col11"><span class="ui-icon-arrowthick-2-n-s sortable-number" style="float:left;margin-left:0; font-weight: bold;">' + totalLength + '&nbsp;<b class="liComName">' + Name + '</b></span><div class="compet"><a onclick="editCompetency(' + id + ')" data-toggle="modal" data-target="#CompetencieSegmentModel" style="position: inherit;display: initial;margin-right: 10px;margin-left: 10px;"><i class="material-icons uk-text-success" style="position: inherit; display: initial;">edit</i></a><a  onclick="deleteCom(' + id + ')" style="position: inherit; display: initial;"><i class="material-icons uk-text-danger" style="position: inherit; display: initial;">delete</i></a></div><p class="description">' + descriprtion + '<input type="hidden" class="AddCompetencyName" Id="AddName_' + id + '" value=' + Name + '><input type="hidden" class="AddDescripetion" Id="AddDescription_' + id + '"  value=' + descriprtion + '></p></div></div></div></li>';
                }
                else {
                    appendDataString = '<li class="ui-state-default" data-id=' + id + ' id=' + id + '><div class="uk-nestable-panel"><div class="rowsm"><div class="col-md-1 col1"><i class="fa fa-long-arrow-up tmscursor-size" onclick="swapCompData(' + id + ',' + id + ',' + RecId + ',1)"></i><i class="fa fa-long-arrow-down tmscursor-size" onclick="swapCompData(' + id + ',' + id + ',' + RecId + ',0)"></i></div><div class="col-md-11 col11"><span class="ui-icon-arrowthick-2-n-s sortable-number" style="float:left;margin-left:0; font-weight: bold;">' + totalLength + '&nbsp;<b class="liComName">' + Name + '</b></span><div class="compet"><a onclick="editCompetency(' + id + ')" data-toggle="modal" data-target="#CompetencieSegmentModel" style="position: inherit;display: initial;margin-right: 10px;margin-left: 10px;"><i class="material-icons uk-text-success" style="position: inherit; display: initial;">edit</i></a><a  onclick="deleteCom(' + id + ')" style="position: inherit; display: initial;"><i class="material-icons uk-text-danger" style="position: inherit; display: initial;">delete</i></a></div><p class="description">' + descriprtion + '<input type="hidden" class="AddCompetencyName" Id="AddName_' + id + '" value="' + Name + '"><input type="hidden" class="AddDescripetion" Id="AddDescription_' + id + '"  value=' + descriprtion + '><input type="hidden" class="CompetencyAddID" value=' + id + '><input type="hidden" class="CompetencySortID" value=' + id + '></p></div></div></div></li>';
                }
                $("#page_content_inner").find("#mainPageCompetencieSegmentList").append(appendDataString);
                appendDataString = "";
                $("#page_content_inner").find(".close").click();
                var stepName = $("#tableDiv").find('#txt_CompetencyName').val('');
                var descriprtion = $("#tableDiv").find('#txt_CDescription').val('');
                var isError = false;
                $("#tableDiv").find("#validationmessageCompetencyName").hide();
                $("#tableDiv").find("#ExistCompetencyName").hide();
                // $("#page_content_inner").find("#MainPageStepSegmentList").html('');
            } else {
                isError = true;
                $("#tableDiv").find("#ExistCompetencyName").show();
                return false;
            }
        }
    }
    else {

        var Name = $("#tableDiv").find('#txt_CompetencyName').val();
        var descriprtion = $("#tableDiv").find('#txt_CDescription').val();
        if (Name == "") {
            isError = true;
            $("#tableDiv").find("#validationmessageCompetencyName").show();
            return false;
        }
        else {
            $("#mainPageCompetencieSegmentList").find('li[data-id=' + exitID + ']').find(".liComName").html('');
            $("#mainPageCompetencieSegmentList").find('li[data-id=' + exitID + ']').find(".liComName").html(Name);
            $("#mainPageCompetencieSegmentList").find('li[data-id=' + exitID + ']').find(".description").text('');
            $("#mainPageCompetencieSegmentList").find('li[data-id=' + exitID + ']').find(".description").text(descriprtion);
            $("#tableDiv").find("#CompetencieSegmentModel").find("#modelComClose").click();

        }
    }

})

$("#tableDiv").on('keyup', '#txt_CompetencyName', function (e) {
    var isError = false;
    $("#tableDiv").find("#validationmessageCompetencyName").hide();
    $("#tableDiv").find("#ExistCompetencyName").hide();

});

//Copy Function 
$("#tableDiv").on('click', '.btn-copy-TMSSetting', function () {
    $(".hrtoolLoader").show();
    var Name = $("#tableDiv").find("#txt_RecruitmentProcess").val();
    var Id = $("#tableDiv").find("#RecruitmentProcessId").val();
    var isError = false;
    var TMSId = $("#selTMS")["0"].value;
    if (TMSId == "0") {
        isError = true;
        $("#tableDiv").find("#lbl-error-selTMS").show();
        $(".hrtoolLoader").hide();
        $(".modal-backdrop").hide();
    }
    else {
        $.ajax({
            url: constantTMSSettings.Copy,
            data: { Id: TMSId, RPID: Id, Name: Name },
            success: function (data) {
                $("#tableDiv").html('');
                $("#tableDiv").html(data);
                $("#tableDiv").find("#txt_RecruitmentProcess").val(Name);
                $(".hrtoolLoader").hide();
                $(".modal-backdrop").hide();
                //$(".toast-success").show();
                //setTimeout(function () { $(".toast-success").hide(); }, 1500);
                // location.reload();
            }
        });
    }
})

$('#tableDiv').on('change', '#selTMS', function () {
    var isError = false;
    $("#tableDiv").find("#lbl-error-selTMS").hide();
})
$("#tableDiv").on('click', '.btn-cancel-TMSSetting', function () {
    $(".hrtoolLoader").show();
    $.ajax({
        url: constantTMSSettings.tmsList,
        success: function (data) {
            $("#tableDiv").html('');
            $("#tableDiv").html(data);
            DataTableDesign();
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
        }
    });
});
//Save TMSSetting
$("#tableDiv").on('click', '.btn-save-TMSSetting', function () {
    $(".hrtoolLoader").show();
    var isError = false;
    var Id = $("#tableDiv").find("#RecruitmentProcessId").val();
    var Name = $("#tableDiv").find("#txt_RecruitmentProcess").val();
    if (Name == "") {
        isError = true;
        $("#tableDiv").find("#validationmessageRecruitmentProcess").show();
        $(".hrtoolLoader").hide();
        $(".modal-backdrop").hide();
        return false;
    }
    else {
        var jsonStepSegmentObj = [];
        var ids = 0;
        $.each($("#MainPageStepSegmentList").find("li"), function () {
            var stepName = $(this).find(".AddStepName").val();
            var color = $(this).find(".AddStepColor").val();
            if (stepName != undefined && color != undefined) {
                ids++;
                var oneData = {
                    Id: ids,
                    SortId: ids,
                    StepName: stepName,
                    ColorCode: color
                }
                jsonStepSegmentObj.push(oneData);
            }
        });
        var i = 0;
        if (jsonStepSegmentObj.length == 0) {
            $.each($("#defaultStepList").find("li"), function () {

                var stepName = $(this).find(".AddStepName").val();
                var color = $(this).find(".AddStepColor").val();
                if (stepName != undefined && color != undefined) {
                    i++;
                    var oneData = {
                        Id: i,
                        SortId: i,
                        StepName: stepName,
                        ColorCode: color
                    }
                    jsonStepSegmentObj.push(oneData);
                }
            });

        }
        var AllStepSegmentJson = JSON.stringify(jsonStepSegmentObj);

        var jsonCompetencySegmentObj = [];
        var id = 0;
        $.each($("#mainPageCompetencieSegmentList").find("li"), function () {
            var competencyName = $(this).find(".AddCompetencyName").val();
            var decription = $(this).find(".AddDescripetion").val();
            id++;
            var oneData = {
                Id: id,
                SortId: id,
                CompetencyName: competencyName,
                Description: decription
            }
            jsonCompetencySegmentObj.push(oneData);
        });
        var AllCompetencySegmentJson = JSON.stringify(jsonCompetencySegmentObj);
        var model = {
            Id: Id,
            Name: Name,
            StepCSV: AllStepSegmentJson,
            CompetencyCSV: AllCompetencySegmentJson
        }
        $.ajax({
            type: "POST",
            data: JSON.stringify(model),
            url: constantTMSSettings.Save,
            contentType: "application/json",
            success: function (result) {
                if (result == "Error") {
                    isError = true;
                    $("#tableDiv").find("#lbl-error-ProcessNameExist").show();
                    $(".hrtoolLoader").hide();
                    $(".modal-backdrop").hide();
                }
                else {
                    $("#tableDiv").html('');
                    $("#tableDiv").html(result);
                    $(".hrtoolLoader").hide();
                    $(".modal-backdrop").hide();
                    $(".toast-success").show();
                    setTimeout(function () { $(".toast-success").hide(); }, 1500);

                    DataTableDesign();
                }
            }
        });
    }
});



//Edit TMSSetting 
$("#tableDiv").on('click', '.btn-edit-TMSSetting', function () {
    $(".hrtoolLoader").show();
    var id = $("#tableDiv").find("#example1 tbody").find('.selected').attr("id");
    $.ajax({
        data: { Id: id },
        url: constantTMSSettings.addEdit,
        contentType: "application/json",
        success: function (result) {
            $("#tableDiv").html('');
            $("#tableDiv").html(result);

            var updateOutput = function (e) {
                var list = e.length ? e : $(e.target),
                    output = list.data('output');
                if (window.JSON) {
                    output.val(window.JSON.stringify(list.nestable('serialize')));//, null, 2));
                }
                else {
                    output.val('JSON browser support required for this demo.');
                }
            };

            $.each($('.StepSegmentNestable'), function () {
                $(this).nestable({
                    maxDepth: 1,
                    group: 1,
                    callback: function (l, e) {

                        var stepId = $(l).attr("data-stepId");
                        var stepName = $(l).attr("data-stepName");
                        var applicantId = $(e).attr("data-applicantId");
                        var VacancyId = $("#contantBody").find("#drp-SelectVacancyId")["0"].value;
                        var model = {
                            StepID: stepId,
                            StepName: stepName,
                            ApplicantID: applicantId,
                            VacancyID: VacancyId
                        }
                        $.ajax({
                            url: constantTMS.StepMove,
                            type: 'POST',
                            data: JSON.stringify(model),
                            contentType: "application/json",
                            success: function (data) {
                                BindData(data);
                            }
                        });
                    }
                });
                updateOutput($('.StepSegmentNestable').data('output', $('.StepSegmentNestable-output')));
            });

            $.each($('.CompetencieSegmentNestable'), function () {
                $(this).nestable({
                    maxDepth: 1,
                    group: 1,
                    callback: function (l, e) {

                        var stepId = $(l).attr("data-stepId");
                        var stepName = $(l).attr("data-stepName");
                        var applicantId = $(e).attr("data-applicantId");
                        var VacancyId = $("#contantBody").find("#drp-SelectVacancyId")["0"].value;
                        var model = {
                            StepID: stepId,
                            StepName: stepName,
                            ApplicantID: applicantId,
                            VacancyID: VacancyId
                        }
                        $.ajax({
                            url: constantTMS.StepMove,
                            type: 'POST',
                            data: JSON.stringify(model),
                            contentType: "application/json",
                            success: function (data) {
                            }
                        });
                    }
                });
                updateOutput($('.CompetencieSegmentNestable').data('output', $('.CompetencieSegmentNestable-output')));
            });

            $(".titletext").html('');
            $(".titletext").html("<div>Recruitment Process<p>Here you can create and define different categories for your recruitment pipeline</p></div>")
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
        }
    });

});

//validation 
$("#tableDiv").on('keyup', '#txt_RecruitmentProcess', function (e) {
    var isError = false;
    $("#tableDiv").find("#validationmessageRecruitmentProcess").hide();
    $("#tableDiv").find("#lbl-error-ProcessNameExist").hide();

});

function btnAddStep() {
    $('.demo2').colorpicker();
    var isError = false;
    $("#tableDiv").find("#validationmessageRecruitmentProcess").hide();
    $("#tableDiv").find("#StepsSegmentSection").find("#stepCountID").val('');
    $("#tableDiv").find("#StepsSegmentSection").find(".stepsTitle").text("Add Step");
    $("#tableDiv").find("#lbl-error-ProcessNameExist").hide();
    var stepName = $("#tableDiv").find('#StepName').val('');

}

function btnAddCompetency() {
    var isError = false;
    $("#tableDiv").find("#validationmessageCompetencyName").hide();
    $("#tableDiv").find("#ExistCompetencyName").hide();
    $("#tableDiv").find("#CompetencieSegmentModel").find("#addCompetencyID").val('');
    $("#tableDiv").find("#CompetencieSegmentModel").find(".competencyTitle").text("Add Competency");
    var stepName = $("#tableDiv").find('#txt_CompetencyName').val('');
    var descriprtion = $("#tableDiv").find('#txt_CDescription').val('');
}

//Delete TMSSetting
$("#tableDiv").on('click', '.btn-delete-TMSSetting', function () {
    var id = $("#tableDiv").find("#example1 tbody").find('.selected').attr("id");
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
                        url: constantTMSSettings.Delete,
                        data: { Id: id },
                        success: function (data) {
                            $("#tableDiv").html("");
                            $("#tableDiv").html(data);
                            $(".hrtoolLoader").hide();
                            $(".modal-backdrop").hide();
                            $(".toast-error").show();
                            setTimeout(function () { $(".toast-error").hide(); }, 1500);
                            DataTableDesign();
                        }
                    });
                }
            }]
    });
});

//Step Edit/Update Name,Code etc...
function editAccepted(Id) {
    // $("tableDiv").find("#StepSegmentModel").html('');
    $('.demo2').colorpicker();
    $("#tableDiv").find("#StepSegmentModel").find("#stepCountID").val(Id);
    if ($("#MainPageStepSegmentList").find("li").length > 0) {
        var Name = $("#MainPageStepSegmentList").find('li[data-id=' + Id + ']').find(".liName").html();
        var Code = $("#MainPageStepSegmentList").find('li[data-id=' + Id + ']').find("#AddStepColor_" + Id).val();
        if (Name == "Accepted") {
            $("#tableDiv").find("#StepSegmentModel").find("#StepName").val(Name);
            document.getElementById("StepName").readOnly = true;
        }
        else {
            $("#tableDiv").find("#StepSegmentModel").find("#StepName").val(Name);
            document.getElementById("StepName").readOnly = false;
        }
    }
    else {
        var Name = $("#defaultStepList").find('li[data-id=' + Id + ']').find(".liName").html();
        var Code = $("#defaultStepList").find('li[data-id=' + Id + ']').find("#AddStepColor_" + Id).val();
        if (Name == "Accepted") {
            $("#tableDiv").find("#StepSegmentModel").find("#StepName").val(Name);
            document.getElementById("StepName") = true;
        }
        else {
            $("#tableDiv").find("#StepSegmentModel").find("#StepName").val(Name);
            document.getElementById("StepName") = false;

        }
    }

    $("#tableDiv").find("#StepSegmentModel").find("#StepColor").val('');
    $("#tableDiv").find("#StepSegmentModel").find("#StepColor").val(Code);
    $("#tableDiv").find("#StepSegmentModel").find("#setColorCode").css("background-color", Code);
    $("#tableDiv").find("#StepsSegmentSection").find(".stepsTitle").text("Edit Step");
    $("#tableDiv").find("#StepsSegmentSection").find("#btn-submit-Steps").text("Save");
}

function editTalantPool(Id) {
    var Name = $("#MainPageStepSegmentList").find('li[data-id=' + Id + ']').find(".liName").html();
    var Code = $("#MainPageStepSegmentList").find('li[data-id=' + Id + ']').find("#AddStepColor_" + Id).val();
    $("#tableDiv").find("#StepSegmentModel").find("#StepName").val(Name);
    document.getElementById("StepName").readOnly = true;
    $("#tableDiv").find("#StepSegmentModel").find("#StepColor").val('');
    $("#tableDiv").find("#StepSegmentModel").find("#StepColor").val(Code);
    $("#tableDiv").find("#StepSegmentModel").find("#setColorCode").css("background-color", Code);
    $("#tableDiv").find("#StepsSegmentSection").find(".stepsTitle").text("Edit Step");
    $("#tableDiv").find("#StepsSegmentSection").find("#btn-submit-Steps").text("Save");
}
function editOthers(Id) {
    $('.demo2').colorpicker();
    $("#tableDiv").find("#StepSegmentModel").find("#stepCountID").val(Id);
    if ($("#MainPageStepSegmentList").find("li").length > 0) {
        var Name = $("#MainPageStepSegmentList").find('li[data-id=' + Id + ']').find(".liName").html();
        var Code = $("#MainPageStepSegmentList").find('li[data-id=' + Id + ']').find("#AddStepColor_" + Id).val();
        if (Name == "Accepted") {
            $("#tableDiv").find("#StepSegmentModel").find("#StepName").val(Name);
            document.getElementById("StepName").readOnly = true;
        }
        else {
            $("#tableDiv").find("#StepSegmentModel").find("#StepName").val(Name);
            document.getElementById("StepName").readOnly = false;

        }
    }
    else {
        var Name = $("#defaultStepList").find('li[data-id=' + Id + ']').find(".liName").html();
        var Code = $("#defaultStepList").find('li[data-id=' + Id + ']').find("#AddStepColor_" + Id).val();
        if (Name == "Accepted") {
            $("#tableDiv").find("#StepSegmentModel").find("#StepName").val(Name);
            document.getElementById("StepName").readOnly = true;
        }
        else {
            $("#tableDiv").find("#StepSegmentModel").find("#StepName").val(Name);
            document.getElementById("StepName").readOnly = false;

        }
    }

    $("#tableDiv").find("#StepSegmentModel").find("#StepColor").val('');
    $("#tableDiv").find("#StepSegmentModel").find("#StepColor").val(Code);
    $("#tableDiv").find("#StepSegmentModel").find("#setColorCode").css("background-color", Code);
    $("#tableDiv").find("#StepsSegmentSection").find(".stepsTitle").text("Edit Step");
    $("#tableDiv").find("#StepsSegmentSection").find("#btn-submit-Steps").text("Save");
}
function editRejected(Id) {
    var Name = $("#MainPageStepSegmentList").find('li[data-id=' + Id + ']').find(".liName").html();
    var Code = $("#MainPageStepSegmentList").find('li[data-id=' + Id + ']').find("#AddStepColor_" + Id).val();
    $("#tableDiv").find("#StepSegmentModel").find("#StepName").val(Name);
    document.getElementById("StepName").readOnly = true;
    $("#tableDiv").find("#StepSegmentModel").find("#StepColor").val('');
    $("#tableDiv").find("#StepSegmentModel").find("#StepColor").val(Code);
    $("#tableDiv").find("#StepSegmentModel").find("#setColorCode").css("background-color", Code);
    $("#tableDiv").find("#StepsSegmentSection").find(".stepsTitle").text("Edit Step");
    $("#tableDiv").find("#StepsSegmentSection").find("#btn-submit-Steps").text("Save");

}
function deleteLi(Id) {
    var Name = $("#defaultStepList").find('li[data-id=' + Id + ']').find(".liName").html();
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
                    $("#MainPageStepSegmentList").find('li[data-id=' + Id + ']').remove();
                    $(".hrtoolLoader").hide();
                }
            }]
    });
}

// Competency Edit / update and delete
function editCompetency(Id) {
    $("#tableDiv").find("#CompetencieSegmentModel").find("#addCompetencyID").val(Id);
    var Name = $("#mainPageCompetencieSegmentList").find('li[data-id=' + Id + ']').find(".liComName").html();
    var Decription = $("#mainPageCompetencieSegmentList").find('li[data-id=' + Id + ']').find(".description").text();

    $("#tableDiv").find("#CompetencieSegmentModel").find("#txt_CompetencyName").val(Name);
    $("#tableDiv").find("#CompetencieSegmentModel").find("#txt_CDescription").val(Decription);
    $("#tableDiv").find("#CompetencieSegmentModel").find(".competencyTitle").text("Edit Competency");
    $("#tableDiv").find("#CompetencieSegmentModel").find("#btn-submit-Competencies").text("Save");


}

function deleteCom(Id) {
    var Name = $("#mainPageCompetencieSegmentList").find('li[data-id=' + Id + ']').find(".liComName").html();
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
                    $("#mainPageCompetencieSegmentList").find('li[data-id=' + Id + ']').remove();
                    $(".hrtoolLoader").hide();
                }
            }]
    });

}
function swapData(stepId, SortId, Id, Flag) {
    jsonStepSegmentObj = [];
    var ids = 0;
    $.each($("#MainPageStepSegmentList").find("li"), function () {
        var stepName = $(this).find(".AddStepName").val();
        var color = $(this).find(".AddStepColor").val();
        if (stepName != undefined && color != undefined) {
            ids++;
            var oneData = {
                Id: ids,
                SortId: ids,
                StepName: stepName,
                ColorCode: color
            }
            jsonStepSegmentObj.push(oneData);
        }
    });
    var AllStepSegmentJson = JSON.stringify(jsonStepSegmentObj);
    if (Id == 0) {
        jsonStepSegmentObjForAdd = [];

        $.each($("#MainPageStepSegmentList").find("li"), function () {
            var stepName = $(this).find(".AddStepName").val();
            var color = $(this).find(".AddStepColor").val();
            var ids = $(this).find(".AddID").val();
            var Sortids = $(this).find(".SortID").val();
            if (stepName != undefined && color != undefined) {
                var oneData = {
                    Id: parseInt(ids),
                    SortId: parseInt(Sortids),
                    StepName: stepName,
                    ColorCode: color
                }
                jsonStepSegmentObjForAdd.push(oneData);
            }
        });
        if (Flag == 0) {
            for (var i = 0; i < jsonStepSegmentObjForAdd.length - 1; i++) {
                if (jsonStepSegmentObjForAdd[i].Id == stepId) {
                    jsonStepSegmentObjForAdd[i].SortId = jsonStepSegmentObjForAdd[i].SortId + 1;
                    if (jsonStepSegmentObjForAdd[i].SortId == jsonStepSegmentObjForAdd.length) {
                        jsonStepSegmentObjForAdd[i].SortId = jsonStepSegmentObjForAdd[i].SortId - 1;
                        return false;
                    }
                    else {

                        jsonStepSegmentObjForAdd[i + 1].SortId = jsonStepSegmentObjForAdd[i + 1].SortId - 1;
                    }
                }
            }
            jsonStepSegmentObjForAdd.sort(GetSortOrder("SortId"));
        }
        if (Flag == 1) {
            for (var i = 0; i < jsonStepSegmentObjForAdd.length - 1; i++) {
                if (jsonStepSegmentObjForAdd[i].Id == stepId) {
                    jsonStepSegmentObjForAdd[i].SortId = jsonStepSegmentObjForAdd[i].SortId - 1;
                    if (jsonStepSegmentObjForAdd[i].SortId == 0) {
                        jsonStepSegmentObjForAdd[i].SortId = jsonStepSegmentObjForAdd[i].SortId + 1;
                        return false;
                    }
                    else {

                        jsonStepSegmentObjForAdd[i - 1].SortId = jsonStepSegmentObjForAdd[i - 1].SortId + 1;
                    }
                }
            }
            jsonStepSegmentObjForAdd.sort(GetSortOrder("SortId"));
        }
        var Id = $("#tableDiv").find("#RecruitmentProcessId").val();
        var Name = $("#tableDiv").find("#txt_RecruitmentProcess").val();
        var jsonCompetencySegmentObj = [];
        var id = 0;
        $.each($("#mainPageCompetencieSegmentList").find("li"), function () {
            var competencyName = $(this).find(".AddCompetencyName").val();
            var decription = $(this).find(".AddDescripetion").val();
            var sortIdComp = $(this).find(".CompetencySortID").val();
            var idscomp = $(this).find(".CompetencyAddID").val();
            var oneData = {
                Id: parseInt(idscomp),
                SortId: parseInt(sortIdComp),
                CompetencyName: competencyName,
                Description: decription
            }
            jsonCompetencySegmentObj.push(oneData);
        });

        var model = {
            Id: Id,
            Name: Name,
            StepList: jsonStepSegmentObjForAdd,
            CompentecyList: jsonCompetencySegmentObj
        }

        $.ajax({
            type: "POST",
            data: JSON.stringify(model),
            url: constantTMSSettings.StepSwapData,
            contentType: "application/json",
            success: function (result) {
                $('#StepsSegmentSection').html('');
                $('#StepsSegmentSection').html(result)
            }
        });
        //if (oneData.SortId == SortId + 1) {
        //    oneData.SortId = SortId;
        //}
        //else if (oneData.SortId == SortId) {
        //    oneData.SortId = SortId + 1;
        //}
        //else {
        //    oneData.SortId = oneData.SortId;
        //}
    }
    else {
        $.ajax({
            url: constantTMSSettings.StepMove,
            type: 'POST',
            data: { AllStepSegmentJsonm: AllStepSegmentJson, SortId: SortId, RecId: Id, flagUpDown: Flag },
            success: function (result) {
                $("#tableDiv").html('');
                $("#tableDiv").html(result);
                var updateOutput = function (e) {
                    var list = e.length ? e : $(e.target),
                        output = list.data('output');
                    if (window.JSON) {
                        output.val(window.JSON.stringify(list.nestable('serialize')));//, null, 2));
                    }
                    else {
                        output.val('JSON browser support required for this demo.');
                    }
                };

                $.each($('.StepSegmentNestable'), function () {

                    $(this).nestable({
                        maxDepth: 1,
                        group: 1,
                        callback: function (l, e) {

                            var stepId = $(l).attr("data-stepId");
                            var stepName = $(l).attr("data-stepName");
                            var applicantId = $(e).attr("data-applicantId");
                            var VacancyId = $("#contantBody").find("#drp-SelectVacancyId")["0"].value;
                            var model = {
                                StepID: stepId,
                                StepName: stepName,
                                ApplicantID: applicantId,
                                VacancyID: VacancyId
                            }
                            $.ajax({
                                url: constantTMS.StepMove,
                                type: 'POST',
                                data: JSON.stringify(model),
                                contentType: "application/json",
                                success: function (data) {
                                    BindData(data);
                                }
                            });
                        }
                    });
                    updateOutput($('.StepSegmentNestable').data('output', $('.StepSegmentNestable-output')));
                });

                $.each($('.CompetencieSegmentNestable'), function () {
                    $(this).nestable({
                        maxDepth: 1,
                        group: 1,
                        callback: function (l, e) {

                            var stepId = $(l).attr("data-stepId");
                            var stepName = $(l).attr("data-stepName");
                            var applicantId = $(e).attr("data-applicantId");
                            var VacancyId = $("#contantBody").find("#drp-SelectVacancyId")["0"].value;
                            var model = {
                                StepID: stepId,
                                StepName: stepName,
                                ApplicantID: applicantId,
                                VacancyID: VacancyId
                            }
                            $.ajax({
                                url: constantTMS.StepMove,
                                type: 'POST',
                                data: JSON.stringify(model),
                                contentType: "application/json",
                                success: function (data) {
                                }
                            });
                        }
                    });
                    updateOutput($('.CompetencieSegmentNestable').data('output', $('.CompetencieSegmentNestable-output')));
                });

                $(".titletext").html('');
                $(".titletext").html("<div>Recruitment Process<p>Here you can create and define different categories for your recruitment pipeline</p></div>")
                $(".hrtoolLoader").hide();
                $(".modal-backdrop").hide();
            }
        })
    }
}
function GetSortOrder(prop) {
    return function (a, b) {
        if (a[prop] > b[prop]) {
            return 1;
        } else if (a[prop] < b[prop]) {
            return -1;
        }
        return 0;
    }
}


function swapCompData(stepId, SortId, Id, Flag) {
    jsonCompetencySegmentObj = [];
    var id = 0;
    $.each($("#mainPageCompetencieSegmentList").find("li"), function () {
        var competencyName = $(this).find(".AddCompetencyName").val();
        var decription = $(this).find(".AddDescripetion").val();
        var sortId = $(this).find(".displaySortId").val();
        id++;
        var oneData = {
            Id: id,
            SortId: id,
            CompetencyName: competencyName,
            Description: decription
        }
        jsonCompetencySegmentObj.push(oneData);
    });
    if (Id == 0) {

        jsonCompetencieSegmentObjForAdd = [];

        $.each($("#mainPageCompetencieSegmentList").find("li"), function () {
            var competencyName = $(this).find(".AddCompetencyName").val();
            var decription = $(this).find(".AddDescripetion").val();
            var sortId = $(this).find(".CompetencySortID").val();
            var ids = $(this).find(".CompetencyAddID").val();
            var oneData = {
                Id: parseInt(ids),
                SortId: parseInt(sortId),
                CompetencyName: competencyName,
                Description: decription
            }
            jsonCompetencieSegmentObjForAdd.push(oneData);
        });
        if (Flag == 0) {

            for (var i = 0; i <= jsonCompetencieSegmentObjForAdd.length - 1; i++) {
                if (jsonCompetencieSegmentObjForAdd[i].Id == stepId) {

                    jsonCompetencieSegmentObjForAdd[i].SortId = jsonCompetencieSegmentObjForAdd[i].SortId + 1;
                    if (jsonCompetencieSegmentObjForAdd[i].SortId > jsonCompetencieSegmentObjForAdd.length) {
                        jsonCompetencieSegmentObjForAdd[i].SortId = jsonCompetencieSegmentObjForAdd[i].SortId - 1;
                        return false;
                    }
                    else {
                        jsonCompetencieSegmentObjForAdd[i + 1].SortId = jsonCompetencieSegmentObjForAdd[i + 1].SortId - 1;
                    }

                }
            }
            jsonCompetencieSegmentObjForAdd.sort(GetSortOrder("SortId"));
        }
        if (Flag == 1) {
            for (var i = 0; i <= jsonCompetencieSegmentObjForAdd.length - 1; i++) {
                if (jsonCompetencieSegmentObjForAdd[i].Id == stepId) {

                    jsonCompetencieSegmentObjForAdd[i].SortId = jsonCompetencieSegmentObjForAdd[i].SortId - 1;
                    if (jsonCompetencieSegmentObjForAdd[i].SortId == 0) {
                        jsonCompetencieSegmentObjForAdd[i].SortId = jsonCompetencieSegmentObjForAdd[i].SortId + 1;
                        return false;
                    }
                    else {
                        jsonCompetencieSegmentObjForAdd[i - 1].SortId = jsonCompetencieSegmentObjForAdd[i - 1].SortId + 1;
                    }


                }
            }
            jsonCompetencieSegmentObjForAdd.sort(GetSortOrder("SortId"));
        }
        var Id = $("#tableDiv").find("#RecruitmentProcessId").val();
        var Name = $("#tableDiv").find("#txt_RecruitmentProcess").val();
        jsonStepSegmentObjForAdd = [];

        $.each($("#MainPageStepSegmentList").find("li"), function () {
            var stepName = $(this).find(".AddStepName").val();
            var color = $(this).find(".AddStepColor").val();
            var ids = $(this).find(".AddID").val();
            var Sortids = $(this).find(".SortID").val();
            if (stepName != undefined && color != undefined) {
                var oneData = {
                    Id: parseInt(ids),
                    SortId: parseInt(Sortids),
                    StepName: stepName,
                    ColorCode: color
                }
                jsonStepSegmentObjForAdd.push(oneData);
            }
        });
        var model = {
            Id: Id,
            Name: Name,
            StepList: jsonStepSegmentObjForAdd,
            CompentecyList: jsonCompetencieSegmentObjForAdd
        }

        $.ajax({
            type: "POST",
            data: JSON.stringify(model),
            url: constantTMSSettings.CompetencySwapData,
            contentType: "application/json",
            success: function (result) {
                $('#CompetenciesSegmentSection').html('');
                $('#CompetenciesSegmentSection').html(result)
            }
        });
    }
    else {
        var AllStepSegmentJson = JSON.stringify(jsonCompetencySegmentObj);
        $.ajax({
            url: constantTMSSettings.StepMoveCompetency,
            type: 'POST',
            data: { AllStepSegmentJsonm: AllStepSegmentJson, SortId: SortId, RecId: Id, flagUpDown: Flag },
            success: function (result) {
                $("#tableDiv").html('');
                $("#tableDiv").html(result);
                var updateOutput = function (e) {
                    var list = e.length ? e : $(e.target),
                        output = list.data('output');
                    if (window.JSON) {
                        output.val(window.JSON.stringify(list.nestable('serialize')));//, null, 2));
                    }
                    else {
                        output.val('JSON browser support required for this demo.');
                    }
                };
                $.each($('.StepSegmentNestable'), function () {

                    $(this).nestable({
                        maxDepth: 1,
                        group: 1,
                        callback: function (l, e) {
                            var stepId = $(l).attr("data-stepId");
                            var stepName = $(l).attr("data-stepName");
                            var applicantId = $(e).attr("data-applicantId");
                            var VacancyId = $("#contantBody").find("#drp-SelectVacancyId")["0"].value;
                            var model = {
                                StepID: stepId,
                                StepName: stepName,
                                ApplicantID: applicantId,
                                VacancyID: VacancyId
                            }
                            $.ajax({
                                url: constantTMS.StepMove,
                                type: 'POST',
                                data: JSON.stringify(model),
                                contentType: "application/json",
                                success: function (data) {
                                    BindData(data);
                                }
                            });
                        }
                    });
                    updateOutput($('.StepSegmentNestable').data('output', $('.StepSegmentNestable-output')));
                });

                $.each($('.CompetencieSegmentNestable'), function () {
                    $(this).nestable({
                        maxDepth: 1,
                        group: 1,
                        callback: function (l, e) {

                            var stepId = $(l).attr("data-stepId");
                            var stepName = $(l).attr("data-stepName");
                            var applicantId = $(e).attr("data-applicantId");
                            var VacancyId = $("#contantBody").find("#drp-SelectVacancyId")["0"].value;
                            var model = {
                                StepID: stepId,
                                StepName: stepName,
                                ApplicantID: applicantId,
                                VacancyID: VacancyId
                            }
                            $.ajax({
                                url: constantTMS.StepMove,
                                type: 'POST',
                                data: JSON.stringify(model),
                                contentType: "application/json",
                                success: function (data) {
                                }
                            });
                        }
                    });
                    updateOutput($('.CompetencieSegmentNestable').data('output', $('.CompetencieSegmentNestable-output')));
                });

                $(".titletext").html('');
                $(".titletext").html("<div>Recruitment Process<p>Here you can create and define different categories for your recruitment pipeline</p></div>")
                $(".hrtoolLoader").hide();
                $(".modal-backdrop").hide();
            }
        })
    }
}