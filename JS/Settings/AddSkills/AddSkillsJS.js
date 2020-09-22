var table;
$(document).ready(function () {
    table = $('#AddSkillsTable').DataTable({
        bFilter: false,
        bInfo: false,
        dom: 'frtlip'
    });
});

$(".ng-isolate-scope").click(function () {
    var id = $(this).attr('id');
    if (!$(this).hasClass('active')) {
        if (id == "tab-Technical-Skills") {
            $("#tab-General-Skills").removeClass('active');
            $("#tab-Technical-Skills").addClass('active');
            BindList("Technical");            
        }
        else {            
            $("#tab-Technical-Skills").removeClass('active');
            $("#tab-General-Skills").addClass('active');
            BindList("General");            
        }
    }
});

function BindList(skillType) {
    $(".hrtoolLoader").show();
    $.ajax({
        url: constantAddSkills.bindList,
        data: { SkillType: skillType },
        success: function (data) {
            $("#tableDiv").html('');
            $("#tableDiv").html(data);
            table = $('#AddSkillsTable').DataTable({
                bFilter: false,
                bInfo: false,
                dom: 'frtlip'
            });
            if (skillType == "Technical") {
                $(".btn-export2excel-addSkills").attr("href", constantAddSkills.expoertTechnical);
            }
            else {
                $(".btn-export2excel-addSkills").attr("href", constantAddSkills.expoertGeneral);
            }
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
        }
    });
}

$('#tableDiv').on('click', '.dataTr', function () {
    if ($(this).hasClass('dataTr')) {
        $('#AddSkillsTable tbody').find('tr.selected').removeClass('selected');
        $(this).addClass('selected');
        $("#tableDiv").find(".btn-edit-addSkills").removeAttr('disabled');
    }
});

$("#tableDiv").on('click', '.btn-add-addSkills', function () {
     
    $.ajax({
        url: constantAddSkills.addEdit,
        data: { Id: 0 },
        success: function (data) {
            $("#tableDiv").find('#addSkillsBody').html('');
            $("#tableDiv").find('#addSkillsBody').html(data);
            $("#tableDiv").find("#addSkillsBody").find("#btn-submit-addSkills").html("Add");
            $('[data-toggle="tooltip"]').tooltip();

            if ($("#tab-Technical-Skills").hasClass('active')) {
                $("#tableDiv").find("#addSkillsModal").find(".modal-title").html("Add Technical Skills");
            }
            else {
                $("#tableDiv").find("#addSkillsModal").find(".modal-title").html("Add General Skills");
            }
        }
    });
});

$("#tableDiv").on('click', '.btn-edit-addSkills', function () {
    var id = $("#tableDiv").find("#AddSkillsTable tbody").find('.selected').attr("id");
    $.ajax({
        url: constantAddSkills.addEdit,
        data: { Id: id },
        success: function (data) {
            $("#tableDiv").find('#addSkillsBody').html('');
            $("#tableDiv").find('#addSkillsBody').html(data);
            $('[data-toggle="tooltip"]').tooltip();

            var keyName = $("#tableDiv").find("#txt-SystemListValueName").val().trim();
            $("#tableDiv").find("#addSkillsModal").find(".modal-title").html(keyName);
            $("#tableDiv").find("#addSkillsModal").find("#btn-submit-addSkills").html("Save");
        }
    });
});

$("#tableDiv").on('click', '#btn-submit-addSkills', function () {
    $(".hrtoolLoader").show();
    var isError = false;
    var id = $("#tableDiv").find("#hidden-Id").val();
    var value = $("#tableDiv").find("#txt-SystemListValueName").val().trim();
    if (value == "") {
        isError = true;
        $("#tableDiv").find("#lbl-error-SystemListValueName").show();
    }
    var description = $("#tableDiv").find("#txt-SystemListDescription").val().trim();

    var skillType = "Technical";
    if ($("#tab-General-Skills").hasClass('active')) {
        skillType = "General";
    }

    if (isError) {
        $(".hrtoolLoader").hide();
        return false;
    }
    else {
        $.ajax({
            url: constantAddSkills.saveData,
            data: { Id: id, Value: value, Description: description, SkillType: skillType },
            success: function (data) {
                $("#tableDiv").html('');
                $("#tableDiv").html(data);

                table = $("#tableDiv").find('#otherSettingTable').DataTable({
                    bFilter: false,
                    bInfo: false,
                    dom: 'frtlip'
                });

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

});
