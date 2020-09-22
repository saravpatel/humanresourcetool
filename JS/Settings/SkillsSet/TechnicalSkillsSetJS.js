var imageData;
$(document).ready(function () {
    DataTableDesign();
});

function DataTableDesign() {
    $('#addTechnicalSkillsTable tfoot tr').appendTo('#addTechnicalSkillsTable thead');
    var table = $('#addTechnicalSkillsTable').DataTable({
        "sDom": '<"top"i>rt<"bottom"flp><"clear">',
        "scrollX":true
    });
    $('#tableDiv').find('.dataTables_filter').hide();
    $('#tableDiv').find('.dataTables_info').hide();

    $("#tableDiv thead .SearchSkillSetName").keyup(function () {
        table.column(1).search(this.value).draw();
    });
    $("#tableDiv thead .SearchDescription").keyup(function () {
        table.column(2).search(this.value).draw();
    });
}

$("#tableDiv").on('click', '.btn-add-addTechnicalSkills', function () {
    $.ajax({
        url: constantTechnicalSkillsSet.addEdit,
        data: { Id: 0 },
        success: function (data) {
            $("#tableDiv").find('#addTechnicalSkillsBody').html('');
            $("#tableDiv").find('#addTechnicalSkillsBody').html(data);
            $("#tableDiv").find("#btn-submit-addTechnicalSkills").html("Add");
            $('[data-toggle="tooltip"]').tooltip();
            $("#tableDiv").find("#addTechnicalSkillsBody").find("#drp-TechnicalSkills").selectList();
        }
    });
});

$("#tableDiv").on('click', '.btn-edit-addTechnicalSkills', function () {
    var id = $("#tableDiv").find("#addTechnicalSkillsTable tbody").find('.selected').attr("id");

    $.ajax({
        url: constantTechnicalSkillsSet.addEdit,
        data: { Id: id },
        success: function (data) {
            $("#tableDiv").find('#addTechnicalSkillsBody').html('');
            $("#tableDiv").find('#addTechnicalSkillsBody').html(data);
            $("#tableDiv").find("#btn-submit-addTechnicalSkills").html("Save");
            var value = $("#tableDiv").find("#txt-SkillsetName").val().trim();
            $("#tableDiv").find("#addTechnicalSkillsModal").find(".modal-title").html(value);
            $('[data-toggle="tooltip"]').tooltip();
            $("#tableDiv").find("#addTechnicalSkillsBody").find("#drp-TechnicalSkills").selectList();
        }
    });
});


$("#tableDiv").on('change', '#fileToUpload', function (e) {
    var files = e.target.files;
    imageData = "";
    if (files.length > 0) {       
        if (window.FormData !== undefined) {
            imageData = new FormData();
            for (var x = 0; x < files.length; x++) {
                imageData.append("file" + x, files[x]);
            }
        }
    }
    else {
        $(".hrtoolLoader").hide();
        $(".modal-backdrop").hide();
    }
});

function readURL(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $("#tableDiv").find('.editpic')
                .attr('src', e.target.result);
        };
        reader.readAsDataURL(input.files[0]);
    }
}

$("#tableDiv").on('click', '#btn-submit-addTechnicalSkills', function () {
    $(".hrtoolLoader").show();
    var isError = false;
    var id = $("#tableDiv").find("#hidden-Id").val();
    var value = $("#tableDiv").find("#txt-SkillsetName").val().trim();
    if (value == "") {
        isError = true;
        $("#tableDiv").find("#lbl-error-SkillsetName").show();
    }
    var description = $("#tableDiv").find("#txt-SystemListDescription").val().trim();
    var technicalSkillId = [];
    $.each($("#tableDiv").find("#drp-TechnicalSkills").parent().find(".selectlist-item"), function () {
        technicalSkillId.push($("#tableDiv").find("#drp-TechnicalSkills").parent().find(".selectlist-select").find('option:contains(' + $(this).html().trim() + ')')[0].value);
    });

    var ajaxUrl = constantTechnicalSkillsSet.saveData + '?Id=' + id + '&skillName=' + value + '&Description=' + description + '&SkillValueIds=' + technicalSkillId.join(',');

    if (isError) {
        $(".hrtoolLoader").hide();
        return false;
    }
    else {
        $.ajax({
            type: "POST",
            url: ajaxUrl,
            contentType: false,
            processData: false,
            data: imageData,
            success: function (result) {

                $("#tableDiv").html('');
                $("#tableDiv").html(result);

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
    //location.reload();
});


$('#tableDiv').on('click', '.dataTr', function () {
    if ($(this).hasClass('dataTr')) {
        $('#addTechnicalSkillsTable tbody').find('tr.selected').removeClass('selected');
        $(this).addClass('selected');
        $("#tableDiv").find(".btn-edit-addTechnicalSkills").removeAttr('disabled');
        $("#tableDiv").find(".btn-delete-addTechnicalSkills").removeAttr('disabled');
    }
});