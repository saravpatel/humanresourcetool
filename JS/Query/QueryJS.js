var rowCount = 1;
var test = [];
var FilterDropValue = [{ Key: "First Name", Value: "1" },
{ Key: "Last Name", Value: "2" },
{ Key: "From Date", Value: "3" },
{ Key: "To Date", Value: "4" },
{ Key: "Business", Value: "5" },
{ Key: "Division", Value: "6" },
{ Key: "Pool", Value: "7" },
{ Key: "Function", Value: "8" },
{ Key: "Customer", Value: "9" },
];

$(document).ready(function () {
    test.push(rowCount);
    var recRow = '<span id="RowCount' + rowCount + '"><div class="row">';
    recRow += '<div class="col-md-3">';
    recRow += '<select id="FilterDropDown_' + rowCount + '">';
    recRow += '<option value="">Select</option>';
    for (var i = 0; i < FilterDropValue.length; i++) {
        recRow += '<option value = ' + FilterDropValue[i].Value + '>' + FilterDropValue[i].Key + '</option>';
    }
    recRow += '</select>';
    recRow += '</div><div class="col-md-3">';
    recRow += '<input name="ColumnValue" id="ColumnValue_' + rowCount + '" type="text" class="form-control" style="margin:5px;" />';
    //recRow += '<input type="radio" id="ColumnValueAnd_"' + rowCount + '"name="radioAnd"/> Or';
    recRow += '</div><div class="col-md-3"><a href="javascript:void(0);" onclick="removeRow(' + rowCount + ');">Delete</a>';
    recRow += '</div>';
    recRow += '</div><span>';
    $('#filterOption').append(recRow);
});
//function QueryTableData() {
//    $('#ResoureListDatatable tfoot tr').appendTo('#ResoureListDatatable thead');
//    var table = $("#tableDivData").find('#ResoureListDatatable').DataTable({
//        "sDom": '<"top"i>rt<"bottom"flp><"clear">',
//        "scrollX": true,
//        "bSort": false
//    });
//    $('#tableDivData').find('.dataTables_filter').hide();
//    $('#tableDivData').find('.dataTables_info').hide();
//    $("#tableDivData").on('keyup', '.QueryName', function () {
//        table.column(0).search(this.value).draw();
//    });
//    $("#tableDivData").on('keyup', '.QueryDescription', function () {
//        table.column(1).search(this.value).draw();
//    });
//    $("#tableDivData").on('keyup', '.QueryReminder', function () {
//        table.column(2).search(this.value).draw();
//    });
//}
function leaveAStepCallback(obj, context) {
    if (context.fromStep == 1) {
        debugger;
        var iserror = false;
        var seleData = $("#RightColumn").val();
        if (seleData == null)
        {
            iserror = true;
            $("#lbl-error-FiledVal").show();
        }
        if (iserror) {

            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
            return false;
        }
        else {
            iserror = false;
            $('#AddQueryForm').find('.buttonNext').show();
            $('#AddQueryForm').find('.buttonPrevious').show();
            $('#AddQueryForm').find('.buttonFinish').hide();
            return true;
        }
    }
    if (context.fromStep == 2) {
        debugger;
        var iserror = false;
        if (context.toStep == 1) {
            return true;
        }
        else {
            if (iserror) {
                iserror = false;
                $('#AddQueryForm').find('.buttonPrevious').show();
                return false;
            }
            else {
                $("#lbl-error-QueryText").hide();
                $('#AddQueryForm').find('.buttonNext').show();
                $('#AddQueryForm').find('.buttonPrevious').show();
                $('#AddQueryForm').find('.buttonFinish').hide();
                return true;
            }                      
        }
    }
    if (context.fromStep == 3) {
        debugger;
        var iserror = false;
        if (context.toStep == 2) {
            return true;
        }
        var QueryName = $("#assigNameText").val();
        if (QueryName == "") {
            iserror = true;
            $("#lbl-error-Resource").show();
        }
        var QueryTextData = $("#txtQueryText").val();
        //if (QueryTextData == "" || QueryTextData == undefined)
        //{
        //    iserror = true;
        //    $("#lbl-error-QueryText").show();
        //}

        if (iserror) {
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
            return false;
        }
        else {
            $("#lbl-error-QueryText").hide();
            $('#AddQueryForm').find('.buttonNext').show();
            $('#AddQueryForm').find('.buttonPrevious').show();
            $('#AddQueryForm').find('.buttonFinish').show();
            return true;
        }
    }    
    if (context.fromStep == 4) {
        debugger;
        var iserror = false;
        if (context.toStep == 3) {
            return true;
        }
        if (iserror) {
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
            return false;
        }
        else {
            $('#AddQueryForm').find('.buttonNext').show();
            $('#AddQueryForm').find('.buttonPrevious').show();
            $('#AddQueryForm').find('.buttonFinish').hide();
            return true;
        }
    }
    else {
        $('#AddQueryForm').find('.buttonNext').show();
        $('#AddQueryForm').find('.buttonPrevious').show();
        $('#AddQueryForm').find('.buttonFinish').hide();
    }


}
function onFinishCallback() {
    var QueryName = $("#assigNameText").val();
    var QueryDesc = $("#assigDescription").val();
    var QueryTextData = $("#txtQueryText").val();    
    var model = {
        Id:0,
        QueryName:QueryName,
        QueryDescription: QueryDesc,
        QueryText: QueryTextData
    }
    $.ajax({
        type: "POST",
        url: ConstantQuery.SaveQueryData,
        data: JSON.stringify(model),
        contentType: "application/json",
        success: function (data) {
            debugger;
            $("#QueryModal").modal('hide');
            $(".toast-success").show();
            window.location.href = ConstantQuery.QueryIndex;
        }
    });
}
function AddQuery() {
    $('#QueryResult').html('');
    $.ajax({
        url: ConstantQuery.Create,
        success: function (data) {
            $("#AddQueryForm").html('');
            $('#AddQueryForm').html(data);                      
            $("#QueryModal").find('#wizard').smartWizard({
                onLeaveStep: leaveAStepCallback,
                onFinish: onFinishCallback
            });
            $('#AddQueryForm').find('.buttonPrevious').hide();
            $('#AddQueryForm').find('.buttonFinish').hide();
            $('#AddQueryForm').find('.buttonNext').addClass('btn btn-warning');
            $('#AddQueryForm').find('.buttonPrevious').addClass('btn btn-info');
            $('#AddQueryForm').find('.buttonFinish').addClass('btn btn-success');
            $("#QueryModal").find("#txt_StartDate").Zebra_DatePicker({
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {
                }
            });
            $("#QueryModal").find("#txt_EndDate").Zebra_DatePicker({
                showButtonPanel: false,
                format: 'd-m-Y',
                onSelect: function () {
                }
            });
            AddFilter();
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
        }
    });
}
var Obj = {};

function SaveQuery() {
    var ColumnNameArray = [];
    var ColumnValueArray = [];
    var columnConditionArray = [];
    var tableId = $("#TableList").val();
    var DivAll = $('input[id=divAll]:checked').val();
    var DivAnyOf = $('input[id=divAnyOfThis]:checked').val();
    var DivAllAnd = $('input[id=radioAllAnd]:checked').val();
    var DivAllOr = $('input[id=radioAllOr]:checked').val();
    var i = 0;
     $('input[name="ColumnValue"]').each(function () {
            ColumnNameArray.push($('#FilterDropDown_' + test[i] + '').val());
            ColumnValueArray.push($('#ColumnValue_' + test[i] + '').val());
            i++;           
     });
    //for (var j = 2; j <= 2; j++) {     
     debugger;
    //$('input[name="radiovalue_'+i+'"]').each(function () {
     var t = 2;
    $('input:radio').each(function() {
         //i++;
         debugger;
         var OrCon = $('input[id="ColumnValueOr_' + t + '"]:checked').val();
         var AndCon = $('input[id="ColumnValueAnd_' + t + '"]:checked').val();
         if (OrCon == "on") {
             columnConditionArray.push("Or");
         }
         else if (AndCon == "on") {
             columnConditionArray.push("And");
         }
         t++;
      });
     //}
     var ColumnString = "";
    $("#RightColumn option").each(function () {
            if (ColumnString == "") {
                ColumnString += this.value;
            }
            else {
                ColumnString += ", " + this.value;
            }
        });
    if (DivAll == "on") {
        var model = {
            FirstName: $("#txt_FirstName").val(),
            LastName: $("#txt_LastName").val(),
            startDate: $("#txt_StartDate").val(),
            endDate: $("#txt_EndDate").val(),
            BussinessId: $("#drpBusiness").val(),
            DivisionId: $("#drpDivision").val(),
            PoolId : $("#drpPool").val(),
            FunctionId: $("#drpFunction").val(),
            JobTitleId: $("#drpJobTitle").val(),
            CustomerId: $("#drpCustomer").val(),
            AllAnd:DivAllAnd,
            AllOr:DivAllOr,
            selectTableId: tableId,
            columnString: ColumnString
        }        
        var allData = JSON.stringify(model);
        $.ajax({
            type: "POST",
            url: ConstantQuery.SaveAllQuery,
            data: JSON.stringify(model),
            contentType: "application/json",
            success: function (data) {
                $('#QueryResult').html('');
                $('#QueryResult').html(data);
            }
        });
    }     
    else {
        $.ajax({
            type: "POST",
            url: ConstantQuery.saveQuery,
            traditional: true,
            //data: { ColumnString: ColumnString, p_ColumnNameArray: ColumnNameArray, p_ColumnValueArray: ColumnValueArray, selectTableId: tableId, DivAll: DivAll, DivAnyOf: DivAnyOf, DivAllAnd: DivAllAnd, DivAllOr: DivAllOr, p_columnConditionArray: columnConditionArray },
            data: { ColumnString: ColumnString, p_ColumnNameArray: ColumnNameArray, p_ColumnValueArray: ColumnValueArray, selectTableId: tableId, DivAll: DivAll, DivAnyOf: DivAnyOf, DivAllAnd: DivAllAnd, DivAllOr: DivAllOr , p_columnConditionArray: columnConditionArray},
            success: function (data) {
                $('#QueryResult').html('');
                $('#QueryResult').html(data);
            }
        });
    }
}
function filltheData(selKeyVal, rowcunt) {
    var selId = $("#FilterDropDown_" + rowcunt).val();
    var rowCount = JSON.stringify(rowcunt);
    var recRow = '<div class="col-md-3">';
    $("#ColumnValue_" + rowCount).remove();
    $(".Zebra_DatePicker_Icon").remove();
    if (selId == 1) {
        recRow += '<input type="text" id="ColumnValue_' + rowCount + '" class="form-control"/>';
        recRow += '</div><span>';
        $('#filterOption').append(recRow);
    }
    else if (selId == 2) {       
        recRow += '<input type="text" id="ColumnValue_' + rowCount + '" class="form-control"/> ';
        recRow += '</div><span>';
        $('#filterOption').append(recRow);
    }
    else if (selId == 3) {        
        recRow += '<input type="text" id="ColumnValue_' + rowCount + '" class="form-control"/>';
        recRow += '</div><span>';
        $('#filterOption').append(recRow);
        $('#ColumnValue_' + rowCount).Zebra_DatePicker({
            showButtonPanel: false,
            format: 'd-m-Y',
            onSelect: function () {
            }
        });       
    }
    else if (selId == 4) {        
        recRow += '<input type="text" id="ColumnValue_' + rowCount + '" class="form-control"/>';
        recRow += '</div><span>';
        $('#filterOption').append(recRow);
        $('#ColumnValue_' + rowCount).Zebra_DatePicker({
            showButtonPanel: false,
            format: 'd-m-Y',
            onSelect: function () {
            }
        });
    }
    else if(selId==5)
    {
        $.ajax({
            url: ConstantQuery.bindBussiness,
            data: {},
            success: function (data) {
                $("#ColumnValue_" + rowCount).html('');
                //var toAppend = '';
                recRow += '<select id="ColumnValue_' + rowCount + '" class="form-control" onchange="AddFilterBusiness(' + rowCount + ');">';
                recRow += "<option value='0'>All</option>";
                $.each(data, function (index, item) {
                    recRow += "<option value='" + item.Id + "'>" + item.Name + "</option>";
                })
                recRow += '</select>'
                recRow += '</div><span>';
                $('#filterOption').append(recRow);
                rowCount++;
                test.push(rowCount);
                var reccRow = '<span id="RowCount' + rowCount + '"><div class="row">';
                reccRow += '<div class="col-md-3">';
                reccRow += '<select id="FilterDropDown_' + rowCount + '" onchange="filltheData(this.value,' + rowCount + ');" class="form-control" disabled>';
                reccRow += '<option value="">Select</option>';
                debugger;
                for (var i = 0; i < FilterDropValue.length; i++) {
                    if (FilterDropValue[i].Value == 5) {
                        reccRow += '<option value = ' + FilterDropValue[5].Value + ' selected="selected">' + FilterDropValue[5].Key + '</option>';
                    }
                    else {
                        reccRow += '<option value = ' + FilterDropValue[i].Value + '>' + FilterDropValue[i].Key + '</option>';
                    }
                }
                reccRow += '</select>';
                reccRow += '</div><div class="col-md-3">';
                reccRow += '<input type="radio" id="ColumnValueOr_' + rowCount + '" name="radiovalue' + rowCount + '"/> Or';
                reccRow += '<input type="radio" id="ColumnValueAnd_' + rowCount + '" name="radiovalue' + rowCount + '" checked/> And';
                reccRow += '<select id="ColumnValue_' + rowCount + '" class="form-control" onchange="filltheFunctionPool(' + rowCount + ');">';
                reccRow += '<option value="">Select</option>';
                $('#filterOption').append(reccRow);
            }
        });
    }
    else if(selId==9)
    {
        $.ajax({
            url: ConstantQuery.getCustomer,
            data: {},
            success: function (data) {
                debugger;
                $("#ColumnValue_" + rowCount).html('');
                recRow += '<select id="ColumnValue_' + rowCount + '" class="form-control">';
                recRow += "<option value='0'>All</option>";
                $.each(data, function (index, item) {
                    recRow += "<option value='" + item.Id + "'>" + item.FirstName + " " + item.LastName + "</option>";
                })
                recRow += '</select>'
                recRow += '</div><span>';
                $('#filterOption').append(recRow);
            }
        });
    }
  
}

function AddFilter() {
    rowCount++;
    test.push(rowCount);
    var recRow = '<span id="RowCount' + rowCount + '"><div class="row">';
    recRow += '<div class="col-md-3">';
    recRow += '<select id="FilterDropDown_' + rowCount + '" onchange="filltheData(this.value,'+rowCount+');">';
    recRow += '<option value="">Select</option>';
   for (var i = 0; i < FilterDropValue.length; i++) {
        recRow += '<option value = ' + FilterDropValue[i].Value + '>' + FilterDropValue[i].Key + '</option>';
    }
   recRow += '</select>';
   recRow += '</div><div class="col-md-3">';
   recRow += '<input type="radio" id="ColumnValueOr_' + rowCount + '" name="radiovalue' + rowCount + '"/> Or';
   recRow += '<input type="radio" id="ColumnValueAnd_' + rowCount + '" name="radiovalue' + rowCount + '" checked/> And';
   recRow += '<input name="ColumnValue" id="ColumnValue_' + rowCount + '" type="text" class="form-control" style="margin:5px;" />';
   recRow += '</div><div class="col-md-3"><a href="javascript:void(0);" onclick="removeRow(' + rowCount + ');">Delete</a>';
   recRow += '</div>';
   recRow += '</div><span>';
   $('#filterOption').append(recRow);
}
function AddFilterBusiness(rowBusinessId) {
    var newrowCount = rowBusinessId + 1;
    var BusinessId = $("#ColumnValue_" + rowBusinessId).val();
    $.ajax({
        url: ConstantQuery.bindDiv,
        data: { businessId: BusinessId },
        success: function (data) {
            debugger;
            $("#ColumnValue_" + newrowCount).html('');
            var toAppend = '';
            toAppend += '<select id="ColumnValue_' + newrowCount + '" class="form-control" onchange="filltheFunctionPool(' + newrowCount + ');">';
            $.each(data, function (index, item) {
                toAppend += "<option value='" + item.Id + "'>" + item.Name + "</option>";
            })                        
            $("#ColumnValue_" + newrowCount).html(toAppend);
            newrowCount++;
            test.push(newrowCount);
            var reccRow = '<span id="RowCount' + newrowCount + '"><div class="row">';
            reccRow += '<div class="col-md-3">';
            reccRow += '<select id="FilterDropDown_' + newrowCount + '" onchange="filltheData(this.value,' + rowCount + ');" class="form-control" disabled>';
            reccRow += '<option value="">Select</option>';
            for (var i = 0; i < FilterDropValue.length; i++) {
                if (FilterDropValue[i].Value == 6) {
                    reccRow += '<option value = ' + FilterDropValue[6].Value + ' selected="selected">' + FilterDropValue[6].Key + '</option>';
                }
                else {
                    reccRow += '<option value = ' + FilterDropValue[i].Value + '>' + FilterDropValue[i].Key + '</option>';
                }
            }
            reccRow += '</select>';
            reccRow += '</div><div class="col-md-3">';
            reccRow += '<input type="radio" id="ColumnValueOr_' + newrowCount + '" name="radiovalue' + newrowCount + '"/> Or';
            reccRow += '<input type="radio" id="ColumnValueAnd_' + newrowCount + '" name="radiovalue' + newrowCount + '" checked/> And';
            reccRow += '<select id="ColumnValue_' + newrowCount + '" class="form-control">';
            reccRow += '<option value="">Select</option>';
            $('#filterOption').append(reccRow);
            debugger;
            newrowCount++;
            test.push(newrowCount);
            var reccRow = '<span id="RowCount' + newrowCount + '"><div class="row">';
            reccRow += '<div class="col-md-3">';
            reccRow += '<select id="FilterDropDown_' + newrowCount + '" onchange="filltheData(this.value,' + rowCount + ');" class="form-control" disabled>';
            reccRow += '<option value="">Select</option>';
            for (var i = 0; i < FilterDropValue.length; i++) {
                if (FilterDropValue[i].Value == 7) {
                    reccRow += '<option value = ' + FilterDropValue[7].Value + ' selected="selected">' + FilterDropValue[7].Key + '</option>';
                }
                else {
                    reccRow += '<option value = ' + FilterDropValue[i].Value + '>' + FilterDropValue[i].Key + '</option>';
                }
            }
            reccRow += '</select>';
            reccRow += '</div><div class="col-md-3">';
            reccRow += '<input type="radio" id="ColumnValueOr_' + newrowCount + '" name="radiovalue' + newrowCount + '"/> Or';
            reccRow += '<input type="radio" id="ColumnValueAnd_' + newrowCount + '" name="radiovalue' + newrowCount + '" checked/> And';
            reccRow += '<select id="ColumnValue_' + newrowCount + '" class="form-control">';
            reccRow += '<option value="">Select</option>';
            $('#filterOption').append(reccRow);
        }
    });          
}
function filltheFunctionPool(rowBusinessId)
{
    var newrowCount = rowBusinessId + 1;    
    var value = $("#ColumnValue_" + rowBusinessId).val();
        $.ajax({
            url: ConstantQuery.bindpool,
            data: { DivisionId: value },
            success: function (data) {
                $("#ColumnValue_" + newrowCount).html('');
                var toAppend = '';
                toAppend += "<option value='0'>All</option>";
                $.each(data, function (index, item) {
                    toAppend += "<option value='" + item.Id + "'>" + item.Name + "</option>";
                })
                $("#ColumnValue_" + newrowCount).html(toAppend);
                newrowCount++;
                $.ajax({
                    url: ConstantQuery.bindFuncation,
                    data: { DivisionId: value },
                    success: function (data) {
                        $("#ColumnValue_" + newrowCount).html('');
                        var toAppend = '';
                        toAppend += "<option value='0'>All</option>";
                        $.each(data, function (index, item) {
                            toAppend += "<option value='" + item.Id + "'>" + item.Name + "</option>";
                        })
                        $("#ColumnValue_" + newrowCount).html(toAppend);                       
                    }
                });
            }
        });    
}
function removeRow(rowNo) {
    var index = test.indexOf(rowNo);
    test.splice(index, 1);    
    $('#RowCount' + rowNo).remove();
}

$("#page_content").on('change', '#divAll', function (e) {
    
    if ($(this).prop("checked")) {
        $("#page_content").find('#QueryModal').find("#div_divAll").show();
        $("#page_content").find('#QueryModal').find("#div_divAnyOfThis").hide();     
    }    
});
$("#page_content").on('change', '#divAnyOfThis', function (e) {    
    if ($(this).prop("checked")) {
        $("#page_content").find('#QueryModal').find("#div_divAll").hide();
        $("#page_content").find('#QueryModal').find("#div_divAnyOfThis").show();     
    }   
});

$('#drpBusiness').change(function () {
    var value = $(this).val();
    //if (value != "0") {
    $.ajax({
        url: ConstantQuery.bindDiv,
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
        }
    });
    //}
});

$('#drpDivision').change(function () {
    var value = $(this).val();
    if (value != "0") {
        $.ajax({
            url: ConstantQuery.bindpool,
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
                $.ajax({
                    url: ConstantQuery.bindFuncation,
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


