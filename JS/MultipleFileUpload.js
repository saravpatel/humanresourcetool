
var nowTemp = new Date();
var now = new Date(nowTemp.getFullYear(), nowTemp.getMonth(), nowTemp.getDate(), 0, 0, 0, 0);
var files;
var storedFiles = [];
var upc = 0;
$(function () {

    $(":file").attr('title', '  ');
    var $loading = $('#loadingDiv').hide();

    //$("input[id^='fileToUpload']").change(function (e) {
    //    doReCreate(e);
    //});

    selDiv = $("#selectedFiles");
});
function doReCreate(e) {
    upc = upc + 1;
    handleFileSelect(e);
    $("input[id^='fileToUpload']").hide();
    $('<input>').attr({
        type: 'file',
        multiple: 'multiple',
        id: 'fileToUpload' + upc,
        class: 'form-control',
        name: 'fileUpload',
        style: 'float: left',
        title: '  ',
        onchange: "doReCreate(event)"

    }).appendTo('#uploaders');
}
function handleFileSelect(e) {
    //selDiv.innerHTML = ""; storedFiles = [];
    selDiv = document.querySelector("#selectedFiles");

    if (!e.target.files) return;

    //selDiv.innerHTML = "";
    files = e.target.files;
    //for (var i = 0; i < files.length; i++) {
    //    //if (i == 0) { selDiv.innerHTML = ""; storedFiles = []; }
    //    var f = files[i];
    //    selDiv.innerHTML += "<div class='form-group'><label class='control-label col-md-3 col-sm-3 col-xs-12'></label><div class='col-md-6 col-sm-6 col-xs-12'>" + f.name + "</div><a style='cursor: pointer' onclick='removeAtt(this)'><span class='glyphicon glyphicon-remove-sign' style='color:red'></span></a></div>";
    //    storedFiles.push(f.name);
    //}
    //$('#@Html.IdFor(i => i.FilesToBeUploaded)').val(storedFiles);
}
function removeAtt(t) {
    var serEle = $(t).parent().text().slice(0, -3);
    var index = storedFiles.indexOf(serEle);
    if (index !== -1) {
        storedFiles.splice(index, 1);
    }
    $(t).parent().remove();
    $('#@Html.IdFor(i => i.FilesToBeUploaded)').val(storedFiles);
}
