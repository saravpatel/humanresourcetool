$('#roleid').change(function () {
    $(".x_content").html('');
    $("#btnSave").hide();
    $("#ddlUser").empty();
    $("#ddlUser").append("<option value='0'>-- Select User --</option>");
    var value = $("#roleid option:selected").val();
    var toAppend = '';
    if (value != "0") {
        $.ajax({
            url: constantrole.roleWiseUsersUrl,
            data: { id: value },
            dataType: 'json',
            success: function (data) {               
                var ddlValue = new Array();
                $.each(data.Userlist, function (index, item) {
                    ddlValue.push(item.FullName);
                });
                $("#ddlUser").select2({
                    data: ddlValue
                });
            }
        });
    }
});

$('#ddlUser').change(function () {
    $(".hrtoolLoader").show();
    var userId = $("#ddlUser").val();
    $(".x_content").html('');
    $("#btnSave").hide();
    if (userId != "0") {
        $.ajax({
            url: constantrole.menuBreadcumUrl,
            data: { userName: userId },
            success: function (data) {
                $(".x_content").html('');
                $(".x_content").html(data);
                $("#btnSave").show();
                var isuncheck = false;
                $.each($('td').find(".checkbox"), function (index, item) {
                    if (!$(this).prop('checked')) {
                        isuncheck = true;
                    }
                });
                if (isuncheck) {
                    $("#selectAll").prop('checked', '');
                }
                else {
                    $("#selectAll").prop('checked', 'checked');
                }
                $(".hrtoolLoader").hide();
                $(".modal-backdrop").hide();
            }
        });
    }
});
$("#btnSave").click(function () {

    $(".hrtoolLoader").show();
    var userId = $("#ddlUser").val();
    var selectedId = "";
    $.each($('td').find(".checkbox"), function (index, item) {
        if ($(this).prop('checked')) {
            if (selectedId == "") {
                selectedId = $(this).attr('id');
            }
            else {
                selectedId += ',' + $(this).attr('id');
            }
        }
    });
    if (selectedId != "") {
        $.ajax({
            url: constantrole.saveMenuUserWiseUrl,
            data: { SelectedMenu: selectedId, userName: userId },
            success: function (data) {
                
                $(".hrtoolLoader").hide();
                $(".modal-backdrop").hide();

                $(".toast-success").show();
                setTimeout(function () { $(".toast-success").hide(); }, 1500);

            }
        });
    }
    else {
        $(".hrtoolLoader").hide();
        $(".modal-backdrop").hide();
        return false;
    }
});

