$(document).ready(function () {
   // DataTableDesign();
    $('#selBus option:contains("-- Select Business --")').attr('selected', 'selected');
    $('#selBusPool option:contains("-- Select Business --")').attr('selected', 'selected');
    $('#selDivPool option:contains("-- Select Division --")').attr('selected', 'selected');
    $('#selBusFun option:contains("-- Select Business --")').attr('selected', 'selected');
    $('#selDivFun option:contains("-- Select Division --")').attr('selected', 'selected');
    $("#selDivPool").hide();
    $("#selDivFun").hide();

});


$('#tableDiv').on('click', '.dataTr', function () {
    if ($(this).hasClass('dataTr')) {
        $('#example tbody').find('tr.selected').removeClass('selected');
        $(this).addClass('selected');
        $("#tableDiv").find(".btn-edit-business").removeAttr('disabled');
        $("#tableDiv").find(".btn-delete-business").removeAttr('disabled');
        $("#tableDiv").find(".btn-edit-division").removeAttr('disabled');
        $("#tableDiv").find(".btn-delete-division").removeAttr('disabled');
        $("#tableDiv").find(".btn-edit-pool").removeAttr('disabled');
        $("#tableDiv").find(".btn-delete-pool").removeAttr('disabled');
        $("#tableDiv").find(".btn-edit-function").removeAttr('disabled');
        $("#tableDiv").find(".btn-delete-function").removeAttr('disabled');

    }
});

//Business List
$(".btn-businessList").on('click', function () {
    $(".hrtoolLoader").show();
    $(".btn-businessList").addClass('btn-primary');
    $(".clsBiz").removeClass('hide');
    $(".btn-divisionList, .btn-poolList, .btn-functionList").removeClass('btn-primary');
    $(".clsDivion, .clsPool, .clsFunction").addClass('hide');
    $.ajax({
        url: constant.BizList,
        success: function (data) {
            $("#tableDiv").html('');
            $("#tableDiv").html(data);
          //  DataTableDesign();

            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
        }
    });
});

//Division List
$(".btn-divisionList").on('click', function () {
    $(".hrtoolLoader").show();
    $(".btn-businessList, .btn-poolList, .btn-functionList").removeClass('btn-primary');
    $(".clsDivion").removeClass('hide');
    $(".clsBiz, .clsPool, .clsFunction").addClass('hide');
    $(".btn-divisionList").addClass('btn-primary');
    $.ajax({
        url: constant.DivList,
        success: function (data) {
            $("#tableDiv").html('');
            $("#tableDiv").html(data);
          //  DataTableDesign();

            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
        }
    });

});

//Pool List
$(".btn-poolList").on('click', function () {
    $(".hrtoolLoader").show();
    $(".btn-businessList, .btn-divisionList, .btn-functionList").removeClass('btn-primary');
    $(".clsPool").removeClass('hide');
    $(".clsDivion, .clsBiz, .clsFunction").addClass('hide');
    $(".btn-poolList").addClass('btn-primary');
    $.ajax({
        url: constant.PolList,
        success: function (data) {
            $("#tableDiv").html('');
            $("#tableDiv").html(data);
          //  DataTableDesign();
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();

        }
    });
});

//Function List
$(".btn-functionList").on('click', function () {
    $(".hrtoolLoader").show();
    $(".btn-businessList, .btn-divisionList, .btn-poolList").removeClass('btn-primary');
    $(".clsFunction").removeClass('hide');
    $(".clsDivion, .clsPool, .clsBiz").addClass('hide');
    $(".btn-functionList").addClass('btn-primary');
    $.ajax({
        url: constant.FunList,
        success: function (data) {
            $("#tableDiv").html('');
            $("#tableDiv").html(data);
         //   DataTableDesign();
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
        }
    });
});

//Add Business
$("#tableDiv").on('click', '#btn-submit-Business', function () {
    $(".hrtoolLoader").show();
    var isError = false;
    var id = $("#tableDiv").find("#business_Id").val();
    if (id == "") {
        id = 0;

    }
    var keyName = $("#tableDiv").find("#businessText").val().trim();
    if (keyName == "") {
        isError = true;
        $("#tableDiv").find("#lbl-error-BusinessName").show();
        $(".hrtoolLoader").hide();
        $(".modal-backdrop").hide();
    }
    else {
        $.ajax({
            url: constant.AddBiz,
            data: { Name: keyName, Id: id },
            success: function (data) {
                if (data == "Error") {
                    isError = true;
                    $("#tableDiv").find("#lbl-error-BusinessNameExist").show();
                    $(".hrtoolLoader").hide();
                    $(".modal-backdrop").hide();
                }
                else {
                    $("#tableDiv").html('');
                    $("#tableDiv").html(data);
                  //  DataTableDesign();
                    $(".toast-success").show();
                    setTimeout(function () { $(".toast-success").hide(); }, 1500);
                    $(".hrtoolLoader").hide();
                    $(".modal-backdrop").hide();
                    //location.reload();
                }
            }

        });
    }

});

$("#businessText").on("keyup", function (e) {
    var isError = false;
    $("#tableDiv").find("#lbl-error-BusinessName").hide();
    $("#tableDiv").find("#lbl-error-BusinessNameExist").hide();

});

function btnaddbusiness() {
    $("#tableDiv").find(".businessTitle").text('Add Business');
    $("#tableDiv").find("#btn-submit-Business").text('Add');

    $("#tableDiv").find("#businessText").val('');
    $("#tableDiv").find("#business_Id").val('');
    var isError = false;
    $("#tableDiv").find("#lbl-error-BusinessName").hide();
    $("#tableDiv").find("#lbl-error-BusinessNameExist").hide();
}

//Edit Business
$("#tableDiv").on('click', '.btn-edit-business', function () {
    $(".hrtoolLoader").show();
    var id = $("#tableDiv").find("#example tbody").find('.selected').attr("id");
    $.ajax({
        url: constant.EditBiz,
        data: { Id: id },
        success: function (data) {
            $("#tableDiv").find("#lbl-error-BusinessNameList").hide();
            $("#tableDiv").find("#lbl-error-BusinessNameExist").hide();
            $("#tableDiv").find(".businessTitle").text('Edit Business');
            $("#tableDiv").find("#btn-submit-Business").text('Save');
            setTimeout(function () {
                $("#businessText").val(data.Name);
                $("#business_Id").val(data.Id);
            }, 300)
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
        }
    });
});

//delete Business
$("#tableDiv").on('click', '.btn-delete-business', function () {
    
    var id = $("#tableDiv").find("#example tbody").find('.selected').attr("id");
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
                        url: constant.DeleteBiz,
                        data: { Id: id },
                        success: function (data) {
                            $("#tableDiv").html('');
                            $("#tableDiv").html(data);
                          //  DataTableDesign();
                            $(".toast-error").show();
                            setTimeout(function () { $(".toast-error").hide(); }, 1500);
                            $(".hrtoolLoader").hide();
                            $(".modal-backdrop").hide();
                            // location.reload();

                        }
                    });
                }
            }]
    });
    
});

//Add Division
$("#tableDiv").on('click', '#btn-submit-Division', function () {
    $(".hrtoolLoader").show();
    var isError = false;
    var id = $("#tableDiv").find("#division_Id").val();
    if (id == "") {
        id = 0;

    }
    var bizId = $("#selBus")["0"].value;
    var keyName = $("#tableDiv").find("#divisionText").val().trim();
    if (keyName == "" || bizId == "0") {

        isError = true;
        if (keyName == "") {
            $("#tableDiv").find("#lbl-error-DivisionName").show();
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
        }

        else {
            $("#tableDiv").find("#lbl-error-BusinessNameList").show();
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
        }
    }
    else {
        $.ajax({
            url: constant.AddDiv,
            data: { Name: keyName, Id: id, businessId: bizId },
            success: function (data) {
                if (data == "Error") {
                    isError = true;
                    $("#tableDiv").find("#lbl-error-DivisionNameExist").show();
                    $(".hrtoolLoader").hide();
                    $(".modal-backdrop").hide();
                }
                else {
                    $("#tableDiv").html('');
                    $("#tableDiv").html(data);
                 //   DataTableDesign();
                    $(".toast-success").show();
                    setTimeout(function () { $(".toast-success").hide(); }, 1500);
                    $(".hrtoolLoader").hide();
                    $(".modal-backdrop").hide();
                   // location.reload();
                }
            }

        });
    }

});

$("#tableDiv").on('keyup', '#divisionText', function (e) {
    var isError = false;
    $("#tableDiv").find("#lbl-error-DivisionName").hide();
    $("#tableDiv").find("#lbl-error-DivisionNameExist").hide();

});

$('#tableDiv').on('change', '#selBus', function () {
    var isError = false;
    $("#tableDiv").find("#lbl-error-BusinessNameList").hide();
})

function btnadddivision() {
    
    $("#selBus").find('option').removeAttr("selected");
    $('#selBus option:contains("-- Select Business --")').attr('selected', 'selected');

    $("#tableDiv").find(".divisionTitle").text('Add Division');
    $("#tableDiv").find("#btn-submit-Division").text('Add');

    $("#tableDiv").find("#divisionText").val('');
    $("#tableDiv").find("#division_Id").val('');
    var isError = false;
    $("#tableDiv").find("#lbl-error-DivisionName").hide();
    $("#tableDiv").find("#lbl-error-BusinessNameList").hide();
    $("#tableDiv").find("#lbl-error-BusinessNameExist").hide();

}

//Edit Division 
$("#tableDiv").on('click', '.btn-edit-division', function () {
    $(".hrtoolLoader").show();
    var id = $("#tableDiv").find("#example tbody").find('.selected').attr("id");
    $.ajax({
        url: constant.EditDiv,
        data: { Id: id },
        success: function (data) {
            $("#tableDiv").find("#lbl-error-DivisionName").hide();
            $("#tableDiv").find("#lbl-error-BusinessNameList").hide();
            $("#tableDiv").find(".divisionTitle").text('Edit Division');
            $("#tableDiv").find("#btn-submit-Division").text('Save');

            setTimeout(function () {
                $("#divisionText").val(data.Name);
                $("#division_Id").val(data.Id);
                $('#selBus option:contains("' + data.BusinessName + '")').attr('selected', 'selected');
            }, 300)
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();

        }
    });
});

//Delete Division DeleteDiv
$("#tableDiv").on('click', '.btn-delete-division', function () {
    var id = $("#tableDiv").find("#example tbody").find('.selected').attr("id");
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
                        url: constant.DeleteDiv,
                        data: { Id: id },
                        success: function (data) {
                            $("#tableDiv").html('');
                            $("#tableDiv").html(data);
                        //    DataTableDesign();
                            $(".toast-error").show();
                            setTimeout(function () { $(".toast-error").hide(); }, 1500);
                            $(".hrtoolLoader").hide();
                            $(".modal-backdrop").hide();
                            //location.reload();

                        }
                    });
                }
            }]
    });

    
});

//Add Pool 
$("#tableDiv").on('click', '#btn-submit-Pool', function () {
    debugger;
    var isError = false;
    var id = $("#tableDiv").find("#pool_Id").val();
    if (id == "") {
        id = 0;
    }
    var bizId = $("#selBusPool")["0"].value;
    var divId = $("#selDivPool")["0"].value;
    var divVal=$("#selDivPool").val();
    var keyName = $("#tableDiv").find("#poolText").val().trim();    
   if (keyName == "") {
            isError = true;
            $("#tableDiv").find("#lbl-error-PoolName").show();
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
       }
   if(bizId == "0")
   {
            isError = true;
            $("#tableDiv").find("#lbl-error-BusinessNameList").show();
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
    }      
   if (divVal == "0") {
        isError = true;
        $("#lbl-error-DivisionNameList").show();
    }
   if (!isError)
   {
       {
           $.ajax({
               url: constant.AddPol,
               data: { Name: keyName, Id: id, businessId: bizId, divisionId: divId },
               success: function (data) {
                   $(".hrtoolLoader").hide();
                   if (data == "Error") {
                       isError = true;
                       $("#tableDiv").find("#lbl-error-PoolNameExist").show();
                       $(".hrtoolLoader").hide();
                       $(".modal-backdrop").hide();
                   }
                   else {
                       $("#tableDiv").html('');
                       $("#tableDiv").html(data);
                    //   DataTableDesign();
                       $(".toast-success").show();
                       setTimeout(function () { $(".toast-success").hide(); }, 1500);
                       $(".modal-backdrop").hide();
                       //location.reload();
                   }
               }

           });
       }
    }

});
$("#tableDiv").on('keyup', '#poolText', function (e) {
    var isError = false;
    $("#tableDiv").find("#lbl-error-PoolName").hide();
    $("#tableDiv").find("#lbl-error-PoolNameExist").hide();

});
$('#tableDiv').on('change', '#selBusPool', function () {
    var Id = $("#selBusPool")["0"].value;
    if (Id == "0") {
        $("#selDivPool").html("");
        var option = '<option value="0">-- Select Division --</option>';
        $("#selDivPool").append(option);
        return false;
    }
    else {
        var isError = false;
        $("#tableDiv").find("#lbl-error-BusinessNameList").hide();
        $("#selDivPool").show();
        $.ajax({
            url: constant.bindDiv,
            data: { businessId: Id },
            success: function (data) {
                $("#selDivPool").html("");
                var option = '<option value="0">-- Select Division --</option>';
                $.each(data, function (index, item) {
                    option += '<option id="1_' + item.Id + '" value="' + item.Id + '">' + item.Name + '</option>';
                });
                $("#selDivPool").html(option);
            }
        });
    }

})
$('#tableDiv').on('change', '#selDivPool', function () {
    var isError = false;
    $("#lbl-error-DivisionNameList").hide();
    $("#tableDiv").find("#lbl-error-DivisionNameList").hide();

})
function btnaddpool() {
    $("#selBusPool").find('option').removeAttr("selected");
    $('#selBusPool option:contains("-- Select Business --")').attr('selected', 'selected');
    $('#selBusPool').trigger('change');

    $("#selDivPool").find('option').removeAttr("selected");
    $('#selDivPool option:contains("-- Select Division --")').attr('selected', 'selected');

    $("#tableDiv").find(".poolTitle").text('Add Pool');
    $("#tableDiv").find("#btn-submit-Pool").text('Add');

    $("#tableDiv").find("#poolText").val('');
    $("#tableDiv").find("#pool_Id").val();
    var isError = false;
    $("#tableDiv").find("#lbl-error-DivisionName").hide();
    $("#tableDiv").find("#lbl-error-BusinessNameList").hide();
    $("#tableDiv").find("#lbl-error-DivisionNameList").hide();
    $("#tableDiv").find("#lbl-error-BusinessNameExist").hide();

}


//Edit Pool
$("#tableDiv").on('click', '.btn-edit-pool', function () {
    $(".hrtoolLoader").show();
    var id = $("#tableDiv").find("#example tbody").find('.selected').attr("id");
    $.ajax({
        url: constant.EditPol,
        data: { Id: id },
        success: function (data) {
            $("#tableDiv").find(".poolTitle").text('Edit Pool');
            $("#tableDiv").find("#btn-submit-Pool").text('Save');
            setTimeout(function () {
                $("#poolText").val(data.Name);
                $("#pool_Id").val(data.Id);
                $('#selBusPool option:contains("' + data.BusinessName + '")').attr('selected', 'selected');
                $('#selBusPool').trigger('change');
                $('#selDivPool option:contains("' + data.DivisionName + '")').attr('selected', 'selected');
                $('#selDivPool').trigger('change');
            }, 300)
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
        }
    });
});

//Delete Pool 
$("#tableDiv").on('click', '.btn-delete-pool', function () {
    var id = $("#tableDiv").find("#example tbody").find('.selected').attr("id");
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
                        url: constant.DeletePol,
                        data: { Id: id },
                        success: function (data) {
                            $("#tableDiv").html('');
                            $("#tableDiv").html(data);
                         //   DataTableDesign();
                            $(".toast-error").show();
                            setTimeout(function () { $(".toast-error").hide(); }, 1500);
                            $(".hrtoolLoader").hide();
                            $(".modal-backdrop").hide();
                            //location.reload();

                        }
                    });
                }
            }]
    });
   
});

//Add Function
$("#tableDiv").on('click', '#btn-submit-Function', function () {
    $(".hrtoolLoader").show();
    var isError = false;
    var id = $("#tableDiv").find("#function_Id").val();
    if (id == "") {
        id = 0;

    }
    var bizId = $("#selBusFun")["0"].value;
    var divId = $("#selDivFun")["0"].value;
    var keyName = $("#tableDiv").find("#functionText").val().trim();   
    if (keyName == "") {
        isError = true;
            $("#tableDiv").find("#lbl-error-FunctionName").show();
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
        }
    if (bizId == "0") {
        isError = true;
            $("#tableDiv").find("#lbl-error-BusinessNameList").show();
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
    }
    if (divId == "0")
    {
        isError = true;
        $("#tableDiv").find("#lbl-error-DivisionNameList").show();
        $(".hrtoolLoader").hide();
        $(".modal-backdrop").hide();
    }
        if(!isError) {
        $.ajax({
            url: constant.AddFun,
            data: { Name: keyName, Id: id, businessId: bizId, divisionId: divId },
            success: function (data) {
                if (data == "Error") {
                    isError = true;
                    $("#tableDiv").find("#lbl-error-FunctionNameExist").show();
                   
                    $(".hrtoolLoader").hide();
                    $(".modal-backdrop").hide();
                }
                else {
                    $("#tableDiv").html('');
                    $("#tableDiv").html(data);
               //     DataTableDesign();
                    $(".toast-success").show();
                    setTimeout(function () { $(".toast-success").hide(); }, 1500);
                    $(".hrtoolLoader").hide();
                    $(".modal-backdrop").hide();
                    //location.reload();
                }
            }

        });
    }

});
$("#tableDiv").on('keyup', '#functionText', function (e) {
    var isError = false;
    $("#tableDiv").find("#lbl-error-FunctionName").hide();
    $("#tableDiv").find("#lbl-error-FunctionNameExist").hide();

});
$('#tableDiv').on('change', '#selBusFun', function () {

    var Id = $("#selBusFun")["0"].value;
    if (Id == "0") {
        $("#selDivFun").html("");
        var option = '<option value="0">-- Select Division --</option>';
        $("#selDivFun").append(option);
        return false;
    }
    else {
        var isError = false;
        $("#tableDiv").find("#lbl-error-BusinessNameList").hide();
        $("#selDivFun").show();
        $.ajax({
            url: constant.bindDiv,
            data: { businessId: Id },
            success: function (data) {
                $("#selDivFun").html("");
                var option = '<option value="0">-- Select Division --</option>';
                $.each(data, function (index, item) {
                    option += '<option id="1_' + item.Id + '" value="' + item.Id + '">' + item.Name + '</option>';
                });
                $("#selDivFun").html(option);
            }
        });
    }
})
$('#tableDiv').on('change', '#selDivFun', function () {
    var isError = false;
    $("#tableDiv").find("#lbl-error-DivisionNameList").hide();

})

function btnaddfunction() {

    $("#selBusFun").find('option').removeAttr("selected");
    $('#selBusFun option:contains("-- Select Business --")').attr('selected', 'selected');
    $('#selBusFun').trigger('change');

    $("#selDivFun").find('option').removeAttr("selected");
    $('#selDivFun option:contains("-- Select Division --")').attr('selected', 'selected');


    $("#tableDiv").find(".functionTitle").text('Add Function');
    $("#tableDiv").find("#btn-submit-Function").text('Add');

    $("#tableDiv").find("#functionText").val('');
    $("#tableDiv").find("#function_Id").val();
    var isError = false;

    $("#tableDiv").find("#lbl-error-DivisionName").hide();
    $("#tableDiv").find("#lbl-error-BusinessNameList").hide();
    $("#tableDiv").find("#lbl-error-DivisionNameList").hide();
    $("#tableDiv").find("#lbl-error-BusinessNameExist").hide();

}

//Edit Function
$("#tableDiv").on('click', '.btn-edit-function', function () {
    $(".hrtoolLoader").show();
    var id = $("#tableDiv").find("#example tbody").find('.selected').attr("id");
    $.ajax({
        url: constant.EditFun,
        data: { Id: id },
        success: function (data) {
            $("#tableDiv").find(".functionTitle").text('Edit Function');
            $("#tableDiv").find("#btn-submit-Function").text('Save');
            setTimeout(function () {
                $("#functionText").val(data.Name);
                $("#function_Id").val(data.Id);
                $('#selBusFun option:contains("' + data.BusinessName + '")').attr('selected', 'selected');
                $('#selBusFun').trigger('change');
                $('#selDivFun option:contains("' + data.DivisionName + '")').attr('selected', 'selected');
                $('#selDivFun').trigger('change');

            }, 300)
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();

        }
    });
});

//Delete Function 
$("#tableDiv").on('click', '.btn-delete-function', function () {
    var id = $("#tableDiv").find("#example tbody").find('.selected').attr("id");
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
                        url: constant.DeleteFun,
                        data: { Id: id },
                        success: function (data) {
                            $("#tableDiv").html('');
                            $("#tableDiv").html(data);
                          //  DataTableDesign();
                            $(".toast-error").show();
                            setTimeout(function () { $(".toast-error").hide(); }, 1500);
                            $(".hrtoolLoader").hide();
                            $(".modal-backdrop").hide();
                            //location.reload();

                        }
                    });
                }
            }]
    });
   
});