var table, imageData;
$(document).ready(function () {
    DataTableDesign();
});

function DataTableDesign() {
    var table = $('#EmergencyContactTable').DataTable({
        "sDom": '<"top"i>rt<"bottom"flp><"clear">'
    });
    $('#tableDiv').find('.dataTables_filter').hide();
    $('#tableDiv').find('.dataTables_info').hide();

}

//table 
$('#tableDiv').on('click', '.dataTr', function () {
    if ($(this).hasClass('dataTr')) {
        $('#example1 tbody').find('tr.selected').removeClass('selected');
        $(this).addClass('selected');
        $("#tableDiv").find(".btn-edit-EmergencyContact").removeAttr('disabled');
        $("#tableDiv").find(".btn-delete-EmergencyContact").removeAttr('disabled');
    }
});

//Open Pop-up
$("#tableDiv").on('click', '.btn-add-EmergencyContact', function () {
    $(".hrtoolLoader").show();
    var EployeeId = $("#page_content_inner").find("#UserId").val();
    $.ajax({
        url: constantEmergencyContact.addEdit,
        data: { Id: 0, EmployeeID: EployeeId },
        success: function (data) {
            $("#tableDiv").find('#EmergencyContactBody').html('');
            $("#tableDiv").find('#EmergencyContactBody').html(data);
            $("#tableDiv").find(".emergencyContactTitle").text("ADD Emergency Contact");
            $("#tableDiv").find("#btn-submit-EmergencyContact").html("ADD");
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
        }
    });
});

//Edit pop-up 
$("#tableDiv").on('click', '.btn-edit-EmergencyContact', function () {
    var id = $("#tableDiv").find("#EmergencyContactTable tbody").find('.selected').attr("id");
    var eid = $("#page_content_inner").find("#EmployeeID").val();
    $.ajax({
        url: constantEmergencyContact.addEdit,
        data: { Id: id, EmployeeID: eid },
        success: function (data) {
            $("#tableDiv").find('#EmergencyContactBody').html('');
            $("#tableDiv").find('#EmergencyContactBody').html(data);
            $("#tableDiv").find(".emergencyContactTitle").text("Edit Emergency Contact");
            $("#tableDiv").find("#btn-submit-EmergencyContact").html("Save");
            $(".hrtoolLoader").hide();
            $(".modal-backdrop").hide();
        }
    });
});

$("#tableDiv").on('click', '.btn-delete-EmergencyContact', function () {
    var id = $("#tableDiv").find("#EmergencyContactTable tbody").find('.selected').attr("id");
    var EployeeId = $("#page_content_inner").find("#UserId").val();
    $.Zebra_Dialog("Are you sure you would like to delete this record?", {
        'type': false,
        'title': 'Delete Contact Record',
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
                        url: constantEmergencyContact.DeleteEmergencyContact,
                        data: { Id: id, EmployeeId: EployeeId },
                        success: function (data) {
                            window.location.reload();
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
            }]
    });
});

//save Emergency Contact
$("#tableDiv").on('click', '#btn-submit-EmergencyContact', function () {
    
    var isError = false;
    var EmployeeID = $("#tableDiv").find("#EmployeeID").val();
    var Id = $("#tableDiv").find("#emergancyContactId").val();
    var Name = $("#tableDiv").find("#txt_Name").val();
    var Relationship = $("#tableDiv").find("#drp_Relationship").val();
    var Postcode = $("#tableDiv").find("#txt_PostCode").val();
    var Address = $("#tableDiv").find("#txt_Address").val();
    var Telephone = $("#tableDiv").find("#txt_Telephone").val();
    var Mobile = $("#tableDiv").find("#txt_Mobile").val();
    var Comments = $("#tableDiv").find("#txt_Comments").val();

    var model = {
        Id: Id,
        EmployeeId: EmployeeID,
        Name: Name,
        Relationship: Relationship,
        Postcode: Postcode,
        Address: Address,
        Telephone: Telephone,
        Mobile: Mobile,
        Comments: Comments
    }

    if (model.Name == "") {
        isError = true;
        $("#lbl-error-Name").show();
    }
    if (isError) {
       
        $(".hrtoolLoader").hide();
        $(".modal-backdrop").hide();
        return false;
    }
    else {
        $.ajax({
            url: constantEmergencyContact.SaveEmergencyContact,
            type: 'POST',
            data: JSON.stringify(model),
            contentType: "application/json",
            success: function (data) {
               
                //$("#page_content").find("#UpdateContacts").html("");
                //$("#page_content").find("#UpdateContacts").html(data);
                //DataTableDesign();
                window.location.reload();
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

$("#page_content_inner").on("change", "#drpCountry", function () {
    var value = $(this).val();
    if (value != "0") {
        $.ajax({
            url: constantEmergencyContact.BindStateUrl,
            data: { countryId: value },
            success: function (data) {

                $("#drpState").html('');
                $("#drpTown").html('');
                var toAppend = '';
                toAppend += "<option value='0'>--Select--</option>";
                $.each(data, function (index, item) {
                    toAppend += "<option value='" + item.Value + "'>" + item.Text + "</option>";
                })
                $("#drpState").html(toAppend);
                if ($("#drpState").val() == 0) {
                    $("#drpState").val(0);
                    $("#drpTown").val(0);

                }

                $.ajax({
                    url: constantEmergencyContact.BindAirPortIDUrl,
                    data: { countryId: value },
                    success: function (data) {

                        $("#drpAirport").html('');
                        var toAppend = '';
                        $.each(data, function (index, item) {
                            toAppend += "<option value='" + item.Value + "'>" + item.Text + "</option>";
                        })
                        $("#drpAirport").html(toAppend);
                        if ($("#drpAirport").val() == 0) {
                            $("#drpAirport").val(0);

                        }
                    }
                });

            }
        });
    }
    else {
        $("#drpState").empty();
        // Bind new values to dropdown
        $('#drpState').each(function () {
            // Create option
            var option = $("<option />");
            option.attr("value", '0').text('--Select--');
            $('#drpState').append(option);
        });
        $('#drpTown').empty();
        // Bind new values to dropdown
        $('#drpTown').each(function () {
            // Create option
            var option = $("<option />");
            option.attr("value", '0').text('--Select--');
            $('#drpTown').append(option);
        });
    }
});

$("#page_content_inner").on("change", "#drpState", function () {

    var value = $(this).val();
    if (value != "0") {
        $.ajax({
            url: constantEmergencyContact.BindCityIDUrl,
            data: { stateId: value },
            success: function (data) {

                $("#drpTown").html('');
                var toAppend = '';
                toAppend += "<option value='0'>--Select--</option>";
                $.each(data, function (index, item) {
                    toAppend += "<option value='" + item.Value + "'>" + item.Text + "</option>";
                })
                $("#drpTown").html(toAppend);
                if ($("#drpTown").val() == 0) {
                    $("#drpTown").val(0);
                }
            }
        });
    }
    else {

        $('#drpTown').empty();
        // Bind new values to dropdown
        $('#drpTown').each(function () {
            // Create option
            var option = $("<option />");
            option.attr("value", '0').text('--Select--');
            $('#drpTown').append(option);
        });
    }
});

$("#page_content_inner").on('click', '#SaveContactRecord', function () {
    
    var isError = false;
    var hiddenId = $("#tableDivTraining").find("#hidden-Id").val();
    var model = {
        Id: $("#page_content").find("#UserId").val(),
        Country: $("#page_content").find("#drpCountry").val(),
        State: $("#page_content").find("#drpState").val(),
        Town: $("#page_content").find("#drpTown").val(),
        Airport: $("#page_content").find("#drpAirport").val(),
        HouseNumber: $("#page_content").find("#HouseNumber").val(),
        Postcode: $("#page_content").find("#Postcode").val(),
        Address: $("#page_content").find("#Address").val(),
        WorkPhone: $("#page_content").find("#WorkPhone").val(),
        WorkMobile: $("#page_content").find("#WorkMobile").val(),
        PersonalPhone: $("#page_content").find("#PersonalPhone").val(),
        PersonalMobile: $("#page_content").find("#PersonalMobile").val(),
        PersonalEmail: $("#page_content").find("#PersonalEmail").val(),
        BankName: $("#page_content").find("#BankName").val(),
        BankCode: $("#page_content").find("#BankCode").val(),
        AccountNumber: $("#page_content").find("#AccountNumber").val(),
        OtherAccountInformation: $("#page_content").find('#OtherAccountInformation').val(),
        AccountName: $("#page_content").find("#AccountName").val(),
        BankAddress: $("#page_content").find('#BankAddress').val(),
        IBAN_Number: $("#page_content").find('#IBAN_Number').val(),
        SWIF_Code: $("#page_content").find('#SWIF_Code').val()
    }

    if (model.Country == "0") {
        iserror = true;
        $("#ValidCountry").show();
        $("#ValidCountry").html("The Country is required.");
    }
    if (model.State == "0") {
        iserror = true;
        $("#ValidState").show();
        $("#ValidState").html("The State is required.");
    }
    if (model.Town == "0") {
        iserror = true;
        $("#ValidTown").show();
        $("#ValidTown").html("The Town is required.");
    }
    if (model.PersonalPhone == "") {
        iserror = true;
        $("#ValidPersonalPhone").show();
        $("#ValidPersonalPhone").html("The Personal Mobile Numbers is required.");

    }
    if (model.PersonalMobile == "") {
        iserror = true;
        $("#ValidPersonalMobile").show();
        $("#ValidPersonalMobile").html("The Personal Mobile Numbers is required.");
    }
    if (model.PersonalEmail == "") {
        iserror = true;
        $("#ValidPersonalEmail").show();
        $("#ValidPersonalEmail").html("The Email is required.");
    }
    if (isError) {
        return false;
        $(".hrtoolLoader").hide();
        $(".modal-backdrop").hide();
    }
    else {
        $.ajax({
            url: constantEmergencyContact.SaveContactUrl,
            type: 'POST',
            data: JSON.stringify(model),
            contentType: "application/json",
            success: function (data) {
                //$("#page_content").find("#UpdateContacts").html("");
                //$("#page_content").find("#UpdateContacts").html(data);

                //DataTableDesign();

                //$(".hrtoolLoader").hide();
                //$(".modal-backdrop").hide();

                //if (hiddenId > 0) {
                //    $(".toast-info").show();
                //    setTimeout(function () { $(".toast-info").hide(); }, 1500);
                //}
                //else {
                //    $(".toast-success").show();
                //    setTimeout(function () { $(".toast-success").hide(); }, 1500);
                //}
                window.location.reload();
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

$("#tableDiv").on('click', '#findAddress', function () {
    
    var isError = false;
    // var hiddenId = $("#tableDivTraining").find("#hidden-Id").val();
    var HouseNumber = $("#tableDiv").find("#txt_HouseNumber").val();
    var postCode = $("#tableDiv").find("#txt_PostCode").val();

    if (postCode == "") {
        $("#tableDiv").find("#lbl-error-PostCode").show();
    }
    else {

        $.post('http://maps.googleapis.com/maps/api/geocode/json?address=' + postCode + '&sensor=false', function (r) {
            
            var lat = r['results'][0]['geometry']['location']['lat'];
            var lng = r['results'][0]['geometry']['location']['lng'];
            $.post('http://maps.googleapis.com/maps/api/geocode/json?latlng=' + lat + ',' + lng + '&sensor=false', function (address) {
                
                $("#tableDiv").find("#txt_Address").val(HouseNumber + ", " + address['results'][0]['address_components'][1]['long_name'] + "\n" + address['results'][0]['address_components'][2]['long_name'] + "\n" + address['results'][0]['address_components'][4]['long_name'] + "\n" + address['results'][0]['address_components'][5]['long_name'] + "\n" + address['results'][0]['address_components'][6]['long_name'] + " " + address['results'][0]['address_components'][7]['long_name']);
            });
        });
    }
});

$('#tableDiv').on('keyup', '#txt_PostCode', function () {
    var isError = false;
    $("#tableDiv").find("#lbl-error-PostCode").hide();
});

$('#tableDiv').on('keyup', '#txt_Name', function () {
    var isError = false;
    $("#tableDiv").find("#lbl-error-Name").hide();
});

$("#UpdateContacts").on('click', '#FindAddressContact', function () {
    
    var isError = false;
    // var hiddenId = $("#tableDivTraining").find("#hidden-Id").val();
    var HouseNumber = $("#UpdateContacts").find("#HouseNumber").val();
    var postCode = $("#UpdateContacts").find("#Postcode").val();

    if (postCode == "") {
        isError = true;
        $("#page_content_inner").find("#lbl-PostCode").show();
    }
    else {

        $.post('http://maps.googleapis.com/maps/api/geocode/json?address=' + postCode + '&sensor=false', function (r) {
            
            var lat = r['results'][0]['geometry']['location']['lat'];
            var lng = r['results'][0]['geometry']['location']['lng'];
            $.post('http://maps.googleapis.com/maps/api/geocode/json?latlng=' + lat + ',' + lng + '&sensor=false', function (address) {
                
                $("#UpdateContacts").find("#Address").val(HouseNumber + ", " + address['results'][0]['address_components'][1]['long_name'] + "\n" + address['results'][0]['address_components'][2]['long_name'] + "\n" + address['results'][0]['address_components'][4]['long_name'] + "\n" + address['results'][0]['address_components'][5]['long_name'] + "\n" + address['results'][0]['address_components'][6]['long_name'] + " " + address['results'][0]['address_components'][7]['long_name']);
            });
        });
    }
});

$('#page_content_inner').on('keyup', '#Postcode', function () {
    var isError = false;
    $("#page_content_inner").find("#lbl-PostCode").hide();
});