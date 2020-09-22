$(document).ready(function () {
   //#example
    DataTableDesign();
});

function DataTableDesign() {
    $('#tableDiv tfoot tr').appendTo('#example thead');
    var table = $('#example').DataTable({
        "sDom": '<"top"i>rt<"bottom"flp><"clear">',
        "scrollX": true,
    });
    $('#tableDiv').find('.dataTables_filter').hide();
    $('#tableDiv').find('.dataTables_info').hide();
    $("#tableDiv thead .SearchName").keyup(function () {
        table.column(0).search(this.value).draw();
    });
    $("#tableDiv thead .SearchRate").keyup(function () {
        table.column(1).search(this.value).draw();
    });
    $("#tableDiv thead .SearchCusrruncy").keyup(function () {
        table.column(2).search(this.value).draw();
    });
    $("#tableDiv thead .SearchWorkUnit").keyup(function () {
        table.column(3).search(this.value).draw();
    });

}


//table 
$('#tableDiv').on('click', '.dataTr', function () {
    if ($(this).hasClass('dataTr')) {
        $('#example tbody').find('tr.selected').removeClass('selected');
        $(this).addClass('selected');
        $("#tableDiv").find(".btn-edit-ActivityType").removeAttr('disabled');
        $("#tableDiv").find(".btn-delete-ActivityType").removeAttr('disabled');
    }
});

$("#tableDiv").on('click', '.btn-Refresh-ActivityType', function () {
    window.location.reload();
});

$("#tableDiv").on('click', '.btn-ClearSorting-ActivityType', function () {
    window.location.reload();
});

$("#tableDiv").on('click', '.btn-clearFilter-ActivityType', function () {
    window.location.reload();
});
// Add Customer
$("#tableDiv").on('click', '#btn-submit-ActivityType', function () {
        $(".hrtoolLoader").show();
    var isError = false;
    var id = $("#tableDiv").find('#AddEditActivityType').find("#activityType_Id").val();
    if (id == "") {
        id = 0;
    }
    var curriencyId = $("#selCurrency")["0"].value;
    var workunitId = $("#selWorkUnit")["0"].value;
    var name = $("#tableDiv").find("#ActivityTypeText").val().trim();
    var year = $("#tableDiv").find("#YearText").val().trim();
    var workerRate = $("#tableDiv").find("#selWorkUnit").val();
    var customerRate = $("#tableDiv").find("#CustomerRateText").val().trim();
    if (name == "" || year == "" || curriencyId == "" || workerRate == "") {
        
        if (name == "") {
            isError = true;
            $("#tableDiv").find("#lbl-error-ActivityTypes").show();
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
        }
        if (year == "") {
        isError = true;
            $("#tableDiv").find("#lbl-error-Year").show();
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
        }
        if (curriencyId == 0) {
            isError = true;
            $("#tableDiv").find("#lbl-error-CurrencyList").show();
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
        }
        if (workerRate == "") {
            isError = true;
            $("#tableDiv").find("#lbl-error-WorkUnitList").show();
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
        }
    }
    if(!isError)     {
    $.ajax({
            url: constantActivityType.SaveCmp,
            data: { Id: id, year: year, name: name, curriencyId: curriencyId, workunitId: workunitId, workerRate: workerRate, customerRate: customerRate },
            success: function (data) {
                if (data == "Error") {
                    isError = true;
                    $("#tableDiv").find("#lbl-error-ActivityTypeExist").show();
                    $(".hrtoolLoader").hide();
                    $(".modal-backdrop").hide();
                }
                else {
                    $("#tableDiv").html('');
                    $("#tableDiv").html(data);
                    DataTableDesign();
                    $(".toast-success").show();
                    setTimeout(function () { $(".toast-success").hide(); }, 1500);
                    $(".hrtoolLoader").hide();
                    $(".modal-backdrop").hide();
                }
            }

        });

    }

});

//Edit Customer
$("#tableDiv").on('click', '.btn-edit-ActivityType', function () {
    $(".hrtoolLoader").show();
    var id = $("#tableDiv").find("#example tbody").find('.selected').attr("id");
    $.ajax({
        url: constantActivityType.EditCmp,
        data: { Id: id },
        success: function (data) {
            $("#tableDiv").find(".activityTypeTitle").text('Edit Activity Type');
            $("#tableDiv").find("#btn-submit-ActivityType").text('Save');
            $("#tableDiv").find('#AddEditActivityType').html('');
            $("#tableDiv").find('#AddEditActivityType').html(data);
            
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
           
        }
    });
});

//Delete Acivity Type
$("#tableDiv").on('click', '.btn-delete-ActivityType', function () {
    var id = $("#tableDiv").find("#example tbody").find('.selected').attr("id");     
       $.Zebra_Dialog(" are you sure want to Delete this Records!?", {
        'type': false,
        'title': 'Delete Activity Record',
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
                        url: constantActivityType.DeleteCmp,
                        data: { Id: id },
                        success: function (data) {
                             $("#tableDiv").html('');
                            $("#tableDiv").html(data);
                            DataTableDesign();
                            $(".toast-error").show();
                            setTimeout(function () { $(".toast-error").hide(); }, 1500);
                            $(".hrtoolLoader").hide();
                            $(".modal-backdrop").hide();
                        }
                    });
                }
            }]
    });

});

//Text Validation Remove
$("#tableDiv").on('keyup', '#ActivityTypes', function (e) {
    var isError = false;
    $("#tableDiv").find("#lbl-error-ActivityTypes").hide();

});
$("#tableDiv").on('keyup', '#YearText', function (e) {
    var isError = false;
    $("#tableDiv").find("#lbl-error-Year").hide();

});

$('#tableDiv').on('change', '#selCurrency', function () {
    var isError = false;
    $("#tableDiv").find("#lbl-error-CurrencyList").hide();

})
$('#tableDiv').on('change', '#selWorkUnit', function () {
    var isError = false;
    $("#tableDiv").find("#lbl-error-WorkUnitList").hide();

})

//Open PopUp  
$("#tableDiv").on('click', '.btn-add-ActivityType', function () {

    $(".hrtoolLoader").show();
    $.ajax({
        url: constantActivityType.addEdit,
        data: { Id: 0 },
        success: function (data) {
            $("#tableDiv").find('#AddEditActivityType').html('');
            $("#tableDiv").find('#AddEditActivityType').html(data);
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
      
        }
    });
});