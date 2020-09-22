var table, imageData;
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
            $('.editpic')
                .attr('src', e.target.result);
        };
        reader.readAsDataURL(input.files[0]);
    }
}