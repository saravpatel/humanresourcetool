$(document).ready(function () {
    // DataTableDesign();
        if ($("#UpliftId").find("li").length === 0) {
            $("#UpliftId").hide();
        }
        if ($("#TimeSheetId").find("li").length === 0) {
            $("#TimeSheetId").hide();
        }
        if ($("#ScheduleId").find("li").length === 0) {
            $("#ScheduleId").hide();
        }
        if ($("#TravelId").find("li").length === 0) {
            $("#TravelId").hide();
        }
        if ($("#AnnualId").find("li").length === 0) {
            $("#AnnualId").hide();
        }
        if ($("#TrainingId").find("li").length === 0) {
            $("#TrainingId").hide();
        }
        if ($("#OtherId").find("li").length === 0) {
            $("#OtherId").hide();
        }
        $("#1").hide(); $("#2").hide(); $("#3").hide(); $("#4").hide(); $("#9").hide(); $("#10").hide(); $("#13").hide();
        //DataTableDesign();
        // Data12();
        $("#All").click(function (event) {
            location.reload();
            $(this).parent().addClass("Active");
            var tab = $(this).attr("href");
            $(tab).fadeIn();
            //DataTableDesign();
            //Data12();
            $("#1").hide(); $("#2").hide(); $("#3").hide(); $("#4").hide(); $("#9").hide(); $("#10").hide(); $("#11").hide(); $("#13").hide();
        });
        
        $("#TimeSheetId ul li").click(function (e) {
            
            var hidden_fields = $(this).find('input:hidden').val();
            var text = $("#TimeSheetId").find('h4').text();
          

            $("#totalhour").val('00:00');
            $('#totalday').val(0);

            if (text == "Timesheet Request") {
                
                $("#DocumentModalTable tbody").empty();
                $("#All").hide();
                $.getJSON("/Approval/TimeSheetApprove?EmpID=" + hidden_fields + "",
                              function (json) {

                                  var tr;
                                  //Append each row to html table
                                  for (var i = 0; i < json.length; i++) {

                                      tr = $('<tr/>');
                                      var tablerows = "";
                                      tablerows += "<tr><td  style=\"display:none\">" + json[i].Id + "</td>";
                                      tablerows += "<td>" + json[i].Day + "</td>";

                                      var date = json[i].Date;
                                      if (date.substring(0, 6) == "/Date(") {
                                          var dt = new Date(parseInt(date.substring(6, date.length - 2)));
                                          var dtString = dt.getDate() + "/" + (dt.getMonth() + 1) + "/" + dt.getFullYear(); //Format Date to Regular date Format
                                      }
                                      var InTime = json[i].InTime;
                                      var EndTime = json[i].EndTime;
                                      function diff(InTime, EndTime) {         //Calculate Time Difference
                                          InTime = InTime.split(":");
                                          EndTime = EndTime.split(":");
                                          var startDate = new Date(0, 0, 0, InTime[0], InTime[1], 0);
                                          var endDate = new Date(0, 0, 0, EndTime[0], EndTime[1], 0);
                                          var diff = endDate.getTime() - startDate.getTime();
                                          var hours = Math.floor(diff / 1000 / 60 / 60);
                                          diff -= hours * 1000 * 60 * 60;
                                          var minutes = Math.floor(diff / 1000 / 60);

                                          return (hours < 9 ? "0" : "") + hours + ":" + (minutes < 9 ? "0" : "") + minutes;
                                      }
                                      var Hours = diff(InTime, EndTime);

                                      tablerows += ("<td>" + dtString + "</td>");
                                      tablerows += ("<td class=\"file_id\">" + Hours + "</td>");
                                      tablerows += ("<td>" + json[i].CostCode + "</td>");
                                      tablerows += ("<td>" + json[i].Project + "</td>");
                                      tablerows += ("<td>" + json[i].Customer + "</td>");
                                      tablerows += ("<td>" + json[i].Asset + "</td>");
                                      tablerows += ("<td><button name=\"button\" value=\"Approve\" type=\"button\" style=\"background-color:cornflowerblue; color:white;border-radius: 7px;\">View Document</button></td>");
                                      tablerows += ("<td><button name=\"button\" value=\"Approve\" type=\"button\" style=\"background-color:cornflowerblue; color:white;border-radius: 7px;\">Pending</button></td>");
                                      tablerows += ("<td><input id=\"approveCheckBox\" type=\"checkbox\" name=\"Approve\" class=\"cbCheck\" > Check</td>");
                                      tablerows += ("<td><input type=\"checkbox\" name=\"Reject\" class=\"cbCheck1\"> Check</td></tr>");
                                      var name = json[i].Name;

                                      $('#WorkerName').val(name);
                                      $('#DocumentModalTable').append(tablerows);

                                      $("#1").show();
                                  }
                              });
                Number.prototype.padDigit = function () {
                    return (this < 10) ? '0' + this : this;
                }

                $(document).on("click", "#approveCheckBox", function () {             //Calculate Total hours and days
                
                    if ($('.cbCheck1').is(':checked')) {
                        $('.cbCheck1').attr('checked', false);
                        $("#totalhour").val('00:00');
                        $('#totalday').val(0);
                    }
                    $('#Timeshetapprvebtn').show();
                    $('#Timeshetrejectbtn').hide();
                    var checked = $(this).is(":checked");
                    if (checked) {

                        var initialhour = $('#totalhour').val();
                        var getRow = $(this).parents('tr');
                        var hour = (getRow.find('td:eq(3)').html());
                        var InTime1 = initialhour.split(":");
                        var t1 = initialhour;
                        var mins = 0;
                        var hrs = 0;
                        t1 = t1.split(':');
                        var t2 = hour.split(':');
                        console.log(Number(t1[1]) + Number(t2[1]))
                        mins = Number(t1[1]) + Number(t2[1]);
                        var minhrs = Math.floor(parseInt(mins / 60));
                        hrs = Number(t1[0]) + Number(t2[0]) + minhrs;
                        mins = mins % 60;
                        t1 = hrs.padDigit() + ':' + mins.padDigit();
                        console.log(t1)

                        $("#totalhour").val(t1);

                        var initialdays = parseInt($('#totalday').val());
                        initialdays = initialdays + 1;

                        $('#totalday').val(initialdays);
                    }
                    else {
                        var initialhour = $('#totalhour').val();
                        var getRow = $(this).parents('tr');
                        var hour = (getRow.find('td:eq(3)').html());
                        var InTime1 = initialhour.split(":");
                        var t1 = initialhour;
                        var mins = 0;
                        var hrs = 0;
                        t1 = t1.split(':');
                        var t2 = hour.split(':');
                        console.log(Number(t1[1]) + Number(t2[1]))
                        mins = Number(t1[1]) - Number(t2[1]);
                        var minhrs = Math.floor(parseInt(mins / 60));
                        hrs = Number(t1[0]) - Number(t2[0]) + minhrs;
                        mins = mins % 60;
                        t1 = hrs.padDigit() + ':' + mins.padDigit();
                        console.log(t1)

                        $("#totalhour").val(t1);
                        var initialdays = parseInt($('#totalday').val());
                        initialdays = initialdays - 1;

                        $('#totalday').val(initialdays);
                    }
                });
                $(document).on("click", ".cbCheck1", function () {             //Calculate Total hours and days
                    if ($('.cbCheck').is(':checked')) {
                        $('.cbCheck').attr('checked', false);
                        $("#totalhour").val('00:00');
                        $('#totalday').val(0);

                    }
                    $('#Timeshetrejectbtn').show();
                    $('#Timeshetapprvebtn').hide();
                    var checked = $(this).is(":checked");
                    if (checked) {
                        var initialhour = $('#totalhour').val();

                        var getRow = $(this).parents('tr');
                        var hour = (getRow.find('td:eq(3)').html());
                        var InTime1 = initialhour.split(":");
                        var t1 = initialhour;
                        var mins = 0;
                        var hrs = 0;
                        t1 = t1.split(':');
                        var t2 = hour.split(':');
                        console.log(Number(t1[1]) + Number(t2[1]))
                        mins = Number(t1[1]) + Number(t2[1]);
                        var minhrs = Math.floor(parseInt(mins / 60));
                        hrs = Number(t1[0]) + Number(t2[0]) + minhrs;
                        mins = mins % 60;
                        t1 = hrs.padDigit() + ':' + mins.padDigit();
                        console.log(t1)

                        $("#totalhour").val(t1);
                        var initialdays = parseInt($('#totalday').val());
                        initialdays = initialdays + 1;

                        $('#totalday').val(initialdays);

                    }
                    else {

                        var initialhour = $('#totalhour').val();
                        var getRow = $(this).parents('tr');
                        var hour = (getRow.find('td:eq(3)').html());
                        var InTime1 = initialhour.split(":");
                        var t1 = initialhour;
                        var mins = 0;
                        var hrs = 0;
                        t1 = t1.split(':');
                        var t2 = hour.split(':');
                        console.log(Number(t1[1]) + Number(t2[1]))
                        mins = Number(t1[1]) - Number(t2[1]);
                        var minhrs = Math.floor(parseInt(mins / 60));
                        hrs = Number(t1[0]) - Number(t2[0]) + minhrs;
                        mins = mins % 60;
                        t1 = hrs.padDigit() + ':' + mins.padDigit();
                        console.log(t1)

                        $("#totalhour").val(t1);
                        var initialdays = parseInt($('#totalday').val());
                        initialdays = initialdays - 1;

                        $('#totalday').val(initialdays);
                    }
                });
                $("#Timeshetapprvebtn").button().click(function () {
                    $('#DocumentModalTable [type="checkbox"]').each(function (i, chk) {
                        if (chk.checked) {

                            var getRow = $(this).parents('tr');
                            var ID = (getRow.find('td:eq(0)').html());
                            $.post("/Approval/UpdateTimeSheetApproval?ID=" + ID + "",
                            function (json) {
                             
                            })
                        }
                    });
                    location.reload();
                });
                $("#Timeshetrejectbtn").button().click(function () {

                    $('#DocumentModalTable [type="checkbox"]').each(function (i, chk) {
                        if (chk.checked) {

                            var getRow = $(this).parents('tr');
                            var ID = (getRow.find('td:eq(0)').html());
                            $.post("/Approval/UpdateTimeSheetReject?ID=" + ID + "",
                            function (json) {
                              
                            })
                        }
                    });
                });
            }
            
        });

        $("#ScheduleId ul li").click(function (e) {
            var hidden_fields = $(this).find('input:hidden').val();
            //var text = $("#ScheduleId").find('h4').text();
           
            debugger;
            //if (text == 'Schedule Request') {
            $("#DocumentModalTable1 tbody").empty();
            $("#All").hide();
            $.getJSON("/Approval/ScheduleApproval?EmpID=" + hidden_fields + "",
                           function (json) {

                               var tr;
                               //Append each row to html table  
                               for (var i = 0; i < json.length; i++) {
                                   tr = $('<tr/>');
                                   var tablerows = "";
                                   tablerows += "<tr><td  style=\"display:none\">" + json[i].Id + "</td>";
                                   //tablerows += "<td>" + json[i].Day + "</td>";

                                   var date = json[i].StartDate;
                                   if (date.substring(0, 6) == "/Date(") {
                                       var dt = new Date(parseInt(date.substring(6, date.length - 2)));
                                       var dtString = dt.getDate() + "/" + (dt.getMonth() + 1) + "/" + dt.getFullYear(); //Format Date to Regular date Format
                                   }
                                   var Enddate = json[i].EndDate;
                                   if (Enddate.substring(0, 6) == "/Date(") {
                                       var dt = new Date(parseInt(Enddate.substring(6, date.length - 2)));
                                       var dtString1 = dt.getDate() + "/" + (dt.getMonth() + 1) + "/" + dt.getFullYear(); //Format Date to Regular date Format
                                   }

                                   var InTime = json[i].InTime;
                                   var EndTime = json[i].EndTime;
                                   function diff(InTime, EndTime) {         //Calculate Time Difference
                                       InTime = InTime.split(":");
                                       EndTime = EndTime.split(":");
                                       var startDate = new Date(0, 0, 0, InTime[0], InTime[1], 0);
                                       var endDate = new Date(0, 0, 0, EndTime[0], EndTime[1], 0);
                                       var diff = endDate.getTime() - startDate.getTime();
                                       var hours = Math.floor(diff / 1000 / 60 / 60);
                                       diff -= hours * 1000 * 60 * 60;
                                       var minutes = Math.floor(diff / 1000 / 60);

                                       return (hours < 9 ? "0" : "") + hours + ":" + (minutes < 9 ? "0" : "") + minutes;
                                   }
                                   var Hours = diff(InTime, EndTime);

                                   tablerows += ("<td>" + dtString + "</td>");
                                   tablerows += ("<td>" + dtString1 + "</td>");
                                   tablerows += ("<td>" + json[i].Duration + "</td>");
                                   tablerows += ("<td>" + Hours + "</td>");
                                   tablerows += ("<td>" + json[i].Project + "</td>");
                                   tablerows += ("<td>" + json[i].Customer + "</td>");
                                   tablerows += ("<td>" + json[i].Asset + "</td>");
                                   tablerows += ("<td><button name=\"button\" value=\"Approve\" type=\"button\" style=\"background-color:cornflowerblue; color:white;border-radius: 7px;\">View Document</button></td>");
                                   tablerows += ("<td><button name=\"button\" value=\"Approve\" type=\"button\" style=\"background-color:cornflowerblue; color:white;border-radius: 7px;\">Pending</button></td>");
                                   tablerows += ("<td><input type=\"checkbox\" name=\"Approve\" class=\"cbCheck2\"> Check</td>");
                                   tablerows += ("<td><input type=\"checkbox\" name=\"Reject\" class=\"cbCheck3\"> Check</td></tr>");
                                   var name = json[i].Name;
                                   $('#ScheduleWorkerName').val(name);
                                   $('#DocumentModalTable1').append(tablerows);
                                   $("#2").show();
                               }

                           });
            Number.prototype.padDigit = function () {
                return (this < 10) ? '0' + this : this;
            }
            $(document).on("click", ".cbCheck2", function () {             //Calculate Total hours and days
                if ($('.cbCheck3').is(':checked')) {
                    $('.cbCheck3').attr('checked', false);
                    $("#TotalScheduleHour").val('00:00');
                    $('#TotalScheduleDays').val(0);
                }
                  $('#ScheduleApprove').show();
                  $('#ScheduleReject').hide();
                var checked = $(this).is(":checked");
                if (checked) {
                    var initialhour = $('#TotalScheduleHour').val();
                    var getRow = $(this).parents('tr');
                    var hour = (getRow.find('td:eq(4)').html());
                    var InTime1 = initialhour.split(":");
                    var t1 = initialhour;
                    var mins = 0;
                    var hrs = 0;
                    t1 = t1.split(':');
                    var t2 = hour.split(':');
                    console.log(Number(t1[1]) + Number(t2[1]))
                    mins = Number(t1[1]) + Number(t2[1]);
                    var minhrs = Math.floor(parseInt(mins / 60));
                    hrs = Number(t1[0]) + Number(t2[0]) + minhrs;
                    mins = mins % 60;
                    t1 = hrs.padDigit() + ':' + mins.padDigit();
                    console.log(t1)

                    $("#TotalScheduleHour").val(t1);

                    var initialdays = parseInt($('#TotalScheduleDays').val());
                    var getRow1 = $(this).parents('tr');
                    var Day = (getRow1.find('td:eq(3)').html());
                    var totalday = parseInt(initialdays) + parseInt(Day);

                      $('#TotalScheduleDays').val(totalday);
                }
                else {
                    var initialhour = $('#TotalScheduleHour').val();
                    var getRow = $(this).parents('tr');
                    var hour = (getRow.find('td:eq(4)').html());
                    var InTime1 = initialhour.split(":");
                    var t1 = initialhour;
                    var mins = 0;
                    var hrs = 0;
                    t1 = t1.split(':');
                    var t2 = hour.split(':');
                    console.log(Number(t1[1]) + Number(t2[1]))
                    mins = Number(t1[1]) - Number(t2[1]);
                    var minhrs = Math.floor(parseInt(mins / 60));
                    hrs = Number(t1[0]) - Number(t2[0]) + minhrs;
                    mins = mins % 60;
                    t1 = hrs.padDigit() + ':' + mins.padDigit();
                    console.log(t1)

                    $("#TotalScheduleHour").val(t1);
                    var initialdays = parseInt($('#TotalScheduleDays').val());
                    var getRow1 = $(this).parents('tr');
                    var Day = (getRow1.find('td:eq(3)').html());
                    var totalday = parseInt(initialdays) - parseInt(Day);

                    $('#TotalScheduleDays').val(totalday);
                }

            });
            $(document).on("click", ".cbCheck3", function () {
                debugger;//Calculate Total hours and days
                if ($('.cbCheck2').is(':checked')) {
                    $('.cbCheck2').attr('checked', false);
                    $("#TotalScheduleHour").val('00:00');
                    $('#TotalScheduleDays').val(0);

                }
                 $('#ScheduleReject').show();
                 $('#ScheduleApprove').hide();
                var checked = $(this).is(":checked");
                if (checked) {
                    var initialhour = $('#TotalScheduleHour').val();

                    var getRow = $(this).parents('tr');
                    var hour = (getRow.find('td:eq(4)').html());
                    var InTime1 = initialhour.split(":");
                    var t1 = initialhour;
                    var mins = 0;
                    var hrs = 0;
                    t1 = t1.split(':');
                    var t2 = hour.split(':');
                    console.log(Number(t1[1]) + Number(t2[1]))
                    mins = Number(t1[1]) + Number(t2[1]);
                    var minhrs = Math.floor(parseInt(mins / 60));
                    hrs = Number(t1[0]) + Number(t2[0]) + minhrs;
                    mins = mins % 60;
                    t1 = hrs.padDigit() + ':' + mins.padDigit();
                    console.log(t1)

                    $("#TotalScheduleHour").val(t1);
                    var initialdays = parseInt($('#TotalScheduleDays').val());
                    var getRow1 = $(this).parents('tr');
                    var Day = (getRow1.find('td:eq(3)').html());
                    var totalday = parseInt(initialdays) + parseInt(Day);

                    $('#TotalScheduleDays').val(totalday);

                }
                else {

                    var initialhour = $('#TotalScheduleHour').val();
                    var getRow = $(this).parents('tr');
                    var hour = (getRow.find('td:eq(4)').html());
                    var InTime1 = initialhour.split(":");
                    var t1 = initialhour;
                    var mins = 0;
                    var hrs = 0;
                    t1 = t1.split(':');
                    var t2 = hour.split(':');
                    console.log(Number(t1[1]) + Number(t2[1]))
                    mins = Number(t1[1]) - Number(t2[1]);
                    var minhrs = Math.floor(parseInt(mins / 60));
                    hrs = Number(t1[0]) - Number(t2[0]) + minhrs;
                    mins = mins % 60;
                    t1 = hrs.padDigit() + ':' + mins.padDigit();
                    console.log(t1)

                    $("#TotalScheduleHour").val(t1);
                    var initialdays = parseInt($('#TotalScheduleDays').val());
                    var getRow1 = $(this).parents('tr');
                    var Day = (getRow1.find('td:eq(3)').html());
                    var totalday = parseInt(initialdays) - parseInt(Day);

                    $('#TotalScheduleDays').val(totalday);
                }
            });
            $("#ScheduleApprove").button().click(function () {

                $('#DocumentModalTable1 [type="checkbox"]').each(function (i, chk) {
                    if (chk.checked) {

                        var getRow = $(this).parents('tr');
                        var ID = (getRow.find('td:eq(0)').html());
                        $.post("/Approval/UpdateScheduleApprova?ID=" + ID + "",
                        function (json) {
                            
                        })
                    }
                });
            });
            $("#ScheduleReject").button().click(function () {

                $('#DocumentModalTable1 [type="checkbox"]').each(function (i, chk) {
                    if (chk.checked) {

                        var getRow = $(this).parents('tr');
                        var ID = (getRow.find('td:eq(0)').html());
                        $.post("/Approval/UpdateScheduleReject?ID=" + ID + "",
                        function (json) {
                          
                        })
                    }
                });
            });
            // }

        });
        $("#TravelId ul li").click(function (e) {
            var hidden_fields = $(this).find('input:hidden').val();
            $("#DocumentModalTable2 tbody").empty();
            $('#All').hide();
            $.getJSON("/Approval/TravelApprove?EmpID=" + hidden_fields + "",
                            function (json) {

                                var tr;
                                //Append each row to html table  
                                for (var i = 0; i < json.length; i++) {
                                    tr = $('<tr/>');

                                    var Startdate = json[i].Startdate;
                                    var EndDate = json[i].Enddate;
                                    if (Startdate.substring(0, 6) == "/Date(") {
                                        var dt = new Date(parseInt(Startdate.substring(6, Startdate.length - 2)));
                                        var Startdt = dt.getDate() + "/" + (dt.getMonth() + 1) + "/" + dt.getFullYear();//Format Date in regular date format
                                    }
                                    if (EndDate.substring(0, 6) == "/Date(") {
                                        var dt = new Date(parseInt(EndDate.substring(6, EndDate.length - 2)));
                                        var dtEnddt = dt.getDate() + "/" + (dt.getMonth() + 1) + "/" + dt.getFullYear();//Format Date in regular date format
                                    }
                                    tr.append("<td  style=\"display:none\">" + json[i].Id + "</td>");
                                    tr.append("<td>" + json[i].Type + "</td>");
                                    tr.append("<td>" + json[i].FromCountry + "</td>");
                                    tr.append("<td>" + json[i].FromTown + "</td>");
                                    tr.append("<td>" + json[i].FromAirport + "</td>");
                                    tr.append("<td>" + json[i].ToCountry + "</td>");
                                    tr.append("<td>" + json[i].ToTown + "</td>");
                                    tr.append("<td>" + json[i].ToAirport + "</td>");
                                    tr.append("<td>" + Startdt + "</td>");
                                    tr.append("<td>" + dtEnddt + "</td>");
                                    tr.append("<td>" + json[i].Duration + "</td>");
                                    tr.append("<td>" + json[i].Hour + "</td>");
                                    tr.append("<td>" + json[i].Customer + "</td>");
                                    tr.append("<td>" + json[i].Project + "</td>");
                                    tr.append("<td>" + json[i].CostCode + "</td>");
                                    tr.append("<td></td>");
                                    tr.append("<td></td>");
                                    tr.append("<td><button name=\"button\" value=\"Approve\" type=\"button\" style=\"background-color:cornflowerblue; color:white;border-radius: 7px;\">View Document</button></td>");
                                    tr.append("<td><button name=\"button\" value=\"Approve\" type=\"button\" style=\"background-color:cornflowerblue; color:white;border-radius: 7px;\">Pending</button></td>");
                                    tr.append("<td><input type=\"checkbox\" class=\"cbCheck\" name=\"Approve\"> Check</td>");
                                    tr.append("<td><input type=\"checkbox\" class=\"cbCheck1\" Check</td>");
                                    $('#DocumentModalTable2').append(tr);
                                    var name = json[i].Name;
                                    $('#TravelLeaveWorkername').val(name);
                                    $('#3').show();
                                }
                            });
            Number.prototype.padDigit = function () {
                return (this < 10) ? '0' + this : this;
            }
            $(document).on("click", ".cbCheck", function () {             //Calculate Total hours and days
                if ($('.cbCheck1').is(':checked')) {
                    $('.cbCheck1').attr('checked', false);
                    $("#TravelTotalHour").val('00:00');
                    $('#TravelTotalDay').val(0);
                }
                $('#TravelApprove').show();
                $('#TravelReject').hide();
                var checked = $(this).is(":checked");
                if (checked) {
                    var initialhour = $('#TravelTotalHour').val();
                    var getRow = $(this).parents('tr');
                    var hour = (getRow.find('td:eq(11)').html());
                    var InTime1 = initialhour.split(":");
                    var t1 = initialhour;
                    var mins = 0;
                    var hrs = 0;
                    t1 = t1.split(':');
                    var t2 = hour.split(':');
                    console.log(Number(t1[1]) + Number(t2[1]))
                    mins = Number(t1[1]) + Number(t2[1]);
                    var minhrs = Math.floor(parseInt(mins / 60));
                    hrs = Number(t1[0]) + Number(t2[0]) + minhrs;
                    mins = mins % 60;
                    t1 = hrs.padDigit() + ':' + mins.padDigit();
                    console.log(t1)

                    $("#TravelTotalHour").val(t1);

                    var initialdays = parseInt($('#TravelTotalDay').val());
                    var getRow1 = $(this).parents('tr');
                    var Day = (getRow1.find('td:eq(10)').html());
                    var totalday = parseInt(initialdays) + parseInt(Day);

                    $('#TravelTotalDay').val(totalday);
                }
                else {
                    var initialhour = $('#TravelTotalHour').val();
                    var getRow = $(this).parents('tr');
                    var hour = (getRow.find('td:eq(11)').html());
                    var InTime1 = initialhour.split(":");
                    var t1 = initialhour;
                    var mins = 0;
                    var hrs = 0;
                    t1 = t1.split(':');
                    var t2 = hour.split(':');
                    console.log(Number(t1[1]) + Number(t2[1]))
                    mins = Number(t1[1]) - Number(t2[1]);
                    var minhrs = Math.floor(parseInt(mins / 60));
                    hrs = Number(t1[0]) - Number(t2[0]) + minhrs;
                    mins = mins % 60;
                    t1 = hrs.padDigit() + ':' + mins.padDigit();
                    console.log(t1)

                    $("#TravelTotalHour").val(t1);
                    var initialdays = parseInt($('#TravelTotalDay').val());
                    var getRow1 = $(this).parents('tr');
                    var Day = (getRow1.find('td:eq(10)').html());
                    var totalday = parseInt(initialdays) - parseInt(Day);

                    $('#TravelTotalDay').val(totalday);
                }

            });
            $(document).on("click", ".cbCheck1", function () {             //Calculate Total hours and days
                if ($('.cbCheck').is(':checked')) {
                    $('.cbCheck').attr('checked', false);
                    $("#TravelTotalHour").val('00:00');
                    $('#TravelTotalDay').val(0);

                }
                $('#TravelReject').show();
                $('#TravelApprove').hide();
                var checked = $(this).is(":checked");
                if (checked) {
                    var initialhour = $('#TravelTotalHour').val();

                    var getRow = $(this).parents('tr');
                    var hour = (getRow.find('td:eq(11)').html());
                    var InTime1 = initialhour.split(":");
                    var t1 = initialhour;
                    var mins = 0;
                    var hrs = 0;
                    t1 = t1.split(':');
                    var t2 = hour.split(':');
                    console.log(Number(t1[1]) + Number(t2[1]))
                    mins = Number(t1[1]) + Number(t2[1]);
                    var minhrs = Math.floor(parseInt(mins / 60));
                    hrs = Number(t1[0]) + Number(t2[0]) + minhrs;
                    mins = mins % 60;
                    t1 = hrs.padDigit() + ':' + mins.padDigit();
                    console.log(t1)

                    $("#TravelTotalHour").val(t1);
                    var initialdays = parseInt($('#TravelTotalDay').val());
                    var getRow1 = $(this).parents('tr');
                    var Day = (getRow1.find('td:eq(10)').html());
                    var totalday = parseInt(initialdays) + parseInt(Day);

                    $('#TravelTotalDay').val(totalday);
                
                }
                else {

                    var initialhour = $('#TravelTotalHour').val();
                    var getRow = $(this).parents('tr');
                    var hour = (getRow.find('td:eq(11)').html());
                    var InTime1 = initialhour.split(":");
                    var t1 = initialhour;
                    var mins = 0;
                    var hrs = 0;
                    t1 = t1.split(':');
                    var t2 = hour.split(':');
                    console.log(Number(t1[1]) + Number(t2[1]))
                    mins = Number(t1[1]) - Number(t2[1]);
                    var minhrs = Math.floor(parseInt(mins / 60));
                    hrs = Number(t1[0]) - Number(t2[0]) + minhrs;
                    mins = mins % 60;
                    t1 = hrs.padDigit() + ':' + mins.padDigit();
                    console.log(t1)

                    $("#TravelTotalHour").val(t1);
                    var initialdays = parseInt($('#TravelTotalDay').val());
                    var getRow1 = $(this).parents('tr');
                    var Day = (getRow1.find('td:eq(10)').html());
                    var totalday = parseInt(initialdays) - parseInt(Day);

                    $('#TravelTotalDay').val(totalday);
                }
            });
            $("#TravelApprove").button().click(function () {

                $('#DocumentModalTable2 [type="checkbox"]').each(function (i, chk) {
                    if (chk.checked) {

                        var getRow = $(this).parents('tr');
                        var ID = (getRow.find('td:eq(0)').html());
                        $.post("/Approval/UpdateTravelApprova?ID=" + ID + "",
                        function (json) {
                            
                        })
                    }
                });
            });
            $("#TravelReject").button().click(function () {

                $('#DocumentModalTable2 [type="checkbox"]').each(function (i, chk) {
                    if (chk.checked) {

                        var getRow = $(this).parents('tr');
                        var ID = (getRow.find('td:eq(0)').html());
                        $.post("/Approval/UpdateTravelReject?ID=" + ID + "",
                        function (json) {
                        
                        })
                    }
                });
            });

        });
        $("#AnnualId ul li").click(function (e) {
            var hidden_fields = $(this).find('input:hidden').val();
            //var text = $("#ScheduleId").find('h4').text();

            //if (text == 'Schedule Request') {
            $("#DocumentModalTable3 tbody").empty();
            $("#All").hide();
            $.getJSON("/Approval/AnualLeaveApprove?EmpID=" + hidden_fields + "",
                            function (json) {

                                var tr;
                                //Append each row to html table  
                                for (var i = 0; i < json.length; i++) {
                                    tr = $('<tr/>');

                                    var Startdate = json[i].StartDate;
                                    var EndDate = json[i].EndDate;
                                    if (Startdate.substring(0, 6) == "/Date(") {
                                        var dt = new Date(parseInt(Startdate.substring(6, Startdate.length - 2)));
                                        var Startdt = dt.getDate() + "/" + (dt.getMonth() + 1) + "/" + dt.getFullYear();//Format Date in regular date format
                                    }
                                    if (EndDate.substring(0, 6) == "/Date(") {
                                        var dt = new Date(parseInt(EndDate.substring(6, EndDate.length - 2)));
                                        var dtEnddt = dt.getDate() + "/" + (dt.getMonth() + 1) + "/" + dt.getFullYear();//Format Date in regular date format
                                    }
                                    tr.append("<td style=\"Display:None\">" + json[i].Id + "</td>");
                                    tr.append("<td>" + Startdt + "</td>");
                                    tr.append("<td>" + dtEnddt + "</td>");
                                    tr.append("<td>" + json[i].Duration + "</td>");
                                    tr.append("<td><button name=\"button\" value=\"Approve\" type=\"button\" style=\"background-color:cornflowerblue; color:white;border-radius: 7px;\">Pending</button></td>");
                                    tr.append("<td><input type=\"checkbox\" class=\"cbCheck\" name=\"Approve\"> Check</td>");
                                    tr.append("<td><input type=\"checkbox\" class=\"cbCheck1\" name=\"Reject\"> Check</td>");
                                    $('#DocumentModalTable3').append(tr);
                                    var Name, TotalHoliday, HolidayTaken;
                                    Name = json[i].Name; TotalHoliday = json[i].TotalHolidays; HolidayTaken = json[i].HolidaysTaken;
                                    $('#AnnulLeaveWorkerName').val(Name);
                                    $('#TotalHolidays').val(TotalHoliday);
                                    $('#HolidaysTaken').val(HolidayTaken);
                                    $('#4').show();
                                }
                            });
            Number.prototype.padDigit = function () {
                return (this < 10) ? '0' + this : this;
            }
            $(document).on("click", ".cbCheck", function () {             //Calculate Total hours and days
                if ($('.cbCheck1').is(':checked')) {
                    $('.cbCheck1').attr('checked', false);
                    $("#Holidaysapprove").val(0);
                    $('#RemainingHolidays').val(0);
                }
                $('#AnnualLeaveApprove').show();
                $('#AnnualLeaveReject').hide();
                var checked = $(this).is(":checked");
                if (checked) {
                    var initialhour = $('#Holidaysapprove').val();
                    var getRow = $(this).parents('tr');
                    var Days = (getRow.find('td:eq(3)').html());

                    $("#Holidaysapprove").val(Days);

                    var Totaldays = parseInt($('#TotalHolidays').val());
                    var HolidayTaken = parseInt($('#HolidaysTaken').val());
                    var HolidayApprove = parseInt($('#Holidaysapprove').val());
                   
                    var totalday = parseInt(Totaldays) - parseInt(HolidayTaken) - parseInt(HolidayApprove);

                    $('#RemainingHolidays').val(totalday);
                }
                else {
                    var initialhour = $('#Holidaysapprove').val();
                    var getRow = $(this).parents('tr');
                    var Days = (getRow.find('td:eq(3)').html());

                    $("#Holidaysapprove").val(Days);
                    var Totaldays = parseInt($('#TotalHolidays').val());
                    var HolidayTaken = parseInt($('#HolidaysTaken').val());
                    var HolidayApprove = parseInt($('#Holidaysapprove').val());
                    
                    var totalday = parseInt(Totaldays) + parseInt(HolidayTaken) + parseInt(HolidayApprove);

                    $('#RemainingHolidays').val(totalday);
                }

            });
            $(document).on("click", ".cbCheck1", function () {             //Calculate Total hours and days
                if ($('.cbCheck').is(':checked')) {
                    $('.cbCheck').attr('checked', false);
                    $("#Holidaysapprove").val(0);
                    $('#RemainingHolidays').val(0);

                }
                $('#AnnualLeaveReject').show();
                $('#AnnualLeaveApprove').hide();
                var checked = $(this).is(":checked");
                if (checked) {
                    var initialhour = $('#Holidaysapprove').val();

                    var getRow = $(this).parents('tr');
                    var Days = (getRow.find('td:eq(3)').html());
                   

                    $("#Holidaysapprove").val(Days);
                    var Totaldays = parseInt($('#TotalHolidays').val());
                    var HolidayTaken = parseInt($('#HolidaysTaken').val());
                    var HolidayApprove = parseInt($('#Holidaysapprove').val());
                  
                    var totalday = parseInt(Totaldays) - parseInt(HolidayTaken) - parseInt(HolidayApprove);

                    $('#RemainingHolidays').val(totalday);

                }
                else {

                    var initialhour = $('#Holidaysapprove').val();
                    var getRow = $(this).parents('tr');
                    var Days = (getRow.find('td:eq(3)').html());
                  
                    $("#Holidaysapprove").val(Days);
                    var Totaldays = parseInt($('#TotalHolidays').val());
                    var HolidayTaken = parseInt($('#HolidaysTaken').val());
                    var HolidayApprove = parseInt($('#Holidaysapprove').val());
                 
                    var totalday = parseInt(Totaldays) + parseInt(HolidayTaken) + parseInt(HolidayApprove);

                    $('#RemainingHolidays').val(totalday);
                }
            });
            $("#AnnualLeaveApprove").button().click(function () {

                $('#DocumentModalTable3 [type="checkbox"]').each(function (i, chk) {
                    if (chk.checked) {

                        var getRow = $(this).parents('tr');
                        var ID = (getRow.find('td:eq(0)').html());
                        $.post("/Approval/UpdateAnnualLeaveApprova?ID=" + ID + "",
                        function (json) {
                            
                        })
                       
                    }
                });
            });
            $("#AnnualLeaveReject").button().click(function () {

                $('#DocumentModalTable3 [type="checkbox"]').each(function (i, chk) {
                    if (chk.checked) {

                        var getRow = $(this).parents('tr');
                        var ID = (getRow.find('td:eq(0)').html());
                        $.post("/Approval/UpdateAnnualLeaveReject?ID=" + ID + "",
                        function (json) {
                          
                        })
                    }
                });
            });

        });
        $("#OtherId ul li").click(function (e) {
            var hidden_fields = $(this).find('input:hidden').val();

            $("#DocumentModalTable4 tbody").empty();
            $("#All").hide();
            $.getJSON("/Approval/OtherLeaveApprove?EmpID=" + hidden_fields + "",
                              function (json) {

                                  var tr;
                                  //Append each row to html table  
                                  for (var i = 0; i < json.length; i++) {
                                      tr = $('<tr/>');

                                      var Startdate = json[i].StartDate;
                                      var EndDate = json[i].EndDate;
                                      if (Startdate.substring(0, 6) == "/Date(") {
                                          var dt = new Date(parseInt(Startdate.substring(6, Startdate.length - 2)));
                                          var Startdt = dt.getDate() + "/" + (dt.getMonth() + 1) + "/" + dt.getFullYear();//Format Date in regular date format
                                      }
                                      if (EndDate.substring(0, 6) == "/Date(") {
                                          var dt = new Date(parseInt(EndDate.substring(6, EndDate.length - 2)));
                                          var Enddt = dt.getDate() + "/" + (dt.getMonth() + 1) + "/" + dt.getFullYear();//Format Date in regular date format
                                      }
                                      tr.append("<td style=\"Display:None\">" + json[i].Id + "</td>");
                                      tr.append("<td>" + Startdt + "</td>");
                                      tr.append("<td>" + Enddt + "</td>");
                                      tr.append("<td>" + json[i].Duration + "</td>");
                                      tr.append("<td>" + json[i].Reason + "</td>");

                                      tr.append("<td><button name=\"button\" value=\"Approve\" type=\"button\" style=\"background-color:cornflowerblue; color:white;border-radius: 7px;\">View Document</button></td>");
                                      tr.append("<td><button name=\"button\" value=\"Approve\" type=\"button\" style=\"background-color:cornflowerblue; color:white;border-radius: 7px;\">Pending</button></td>");
                                      tr.append("<td><input type=\"checkbox\" name=\"Approve\" Class=\"cbCheck\"> Check</td>");
                                      tr.append("<td><input type=\"checkbox\" name=\"Reject\" Class=\"cbCheck1\"> Check</td>");
                                      $('#DocumentModalTable5').append(tr);
                                      var Name=json[i].Name;
                                      $('#OtherLeaveWorkerName').val(Name);
                                      $('#9').show();
                                  }
                              });
            Number.prototype.padDigit = function () {
                return (this < 10) ? '0' + this : this;
            }
            $(document).on("click", ".cbCheck", function () {
                //Calculate Total hours and days
                if ($('.cbCheck1').is(':checked')) {
                    $('.cbCheck1').attr('checked', false);
                    $("#OtherLeaveInTheYear").val(0);
               //     $('#OtherLeaveApproved').val(0);
                   
                }
                $('#OtherLeaveApprove').show();
                $('#OtherLeaveReject').hide();
                var checked = $(this).is(":checked");
                if (checked) {
                    var initialDays = $('#OtherLeaveApproved').val();
                    var getRow = $(this).parents('tr');
                    var Days = (getRow.find('td:eq(3)').html());

                 //   $("#OtherLeaveApproved").val(Days);

                    //var Totaldays = parseInt($('#TotalHolidays').val());
                    //var HolidayTaken = parseInt($('#HolidaysTaken').val());
                    //var HolidayApprove = parseInt($('#Holidaysapprove').val());

                    //var totalday = parseInt(Totaldays) - parseInt(HolidayTaken) - parseInt(HolidayApprove);

                    //$('#RemainingHolidays').val(totalday);
                }
                else {
                    var initialDays = $('#OtherLeaveApproved').val();
                    var getRow = $(this).parents('tr');
                    var Days = (getRow.find('td:eq(3)').html());

                  //  $("#OtherLeaveApproved").val(Days);
                    //var Totaldays = parseInt($('#TotalHolidays').val());
                    //var HolidayTaken = parseInt($('#HolidaysTaken').val());
                    //var HolidayApprove = parseInt($('#Holidaysapprove').val());

                    //var totalday = parseInt(Totaldays) + parseInt(HolidayTaken) + parseInt(HolidayApprove);

                    //$('#RemainingHolidays').val(totalday);
                }

            });
            $(document).on("click", ".cbCheck1", function () {             //Calculate Total hours and days
                if ($('.cbCheck').is(':checked')) {
                    $('.cbCheck').attr('checked', false);
                    $("#OtherLeaveInTheYear").val(0);
                    $('#OtherLeaveApproved').val(0);
                }
                $('#OtherLeaveApprove').hide();
                $('#OtherLeaveReject').show();
                var checked = $(this).is(":checked");
                if (checked) {
                    var initialdays = $('#OtherLeaveApproved').val();

                    var getRow = $(this).parents('tr');
                    var Days = (getRow.find('td:eq(3)').html());


               //     $("#OtherLeaveApproved").val(Days);
                    //var Totaldays = parseInt($('#TotalHolidays').val());
                    //var HolidayTaken = parseInt($('#HolidaysTaken').val());
                    //var HolidayApprove = parseInt($('#Holidaysapprove').val());

                    //var totalday = parseInt(Totaldays) - parseInt(HolidayTaken) - parseInt(HolidayApprove);

                    //$('#RemainingHolidays').val(totalday);

                }
                else {

                    var initialhour = $('#OtherLeaveApproved').val();
                    var getRow = $(this).parents('tr');
                    var Days = (getRow.find('td:eq(3)').html());

                    $("#OtherLeaveApproved").val(Days);
                    var Totaldays = parseInt($('#TotalHolidays').val());
                    var HolidayTaken = parseInt($('#HolidaysTaken').val());
                    var HolidayApprove = parseInt($('#Holidaysapprove').val());

                    var totalday = parseInt(Totaldays) + parseInt(HolidayTaken) + parseInt(HolidayApprove);

                    $('#RemainingHolidays').val(totalday);
                }
            });
            $("#OtherLeaveApprove").button().click(function () {
                $('#DocumentModalTable5 [type="checkbox"]').each(function (i, chk) {
                           if (chk.checked) {                         
                        var getrow = $(this).parents('tr');
                        var id = (getrow.find('td:eq(0)').html());
                        $.post("/approval/updateotherleaveapprova?id=" + id + "",
                        function (json) {
                           
                        })
                    }
                });
            });
            $("#OtherLeaveReject").button().click(function () {
                $('#DocumentModalTable5 [type="checkbox"]').each(function (i, chk) {
                    if (chk.checked) {
                        var getRow = $(this).parents('tr');
                        var ID = (getRow.find('td:eq(0)').html());
                        $.post("/Approval/UpdateOtherLeaveReject?ID=" + ID + "",
                        function (json) {
                          
                        })
                    }
                });
            });

        });
        $("#TrainingId ul li").click(function (e) {
            var hidden_fields = $(this).find('input:hidden').val();

            $("#DocumentModalTable6 tbody").empty();
            $("#All").hide();
            $.getJSON("/Approval/TrainingRequestApprove?EmpID=" + hidden_fields + "",
                               function (json) {

                                   var tr;
                                   //Append each row to html table  
                                   for (var i = 0; i < json.length; i++) {
                                       tr = $('<tr/>');

                                       var Startdate = json[i].StartDate;
                                       var EndDate = json[i].EndDate;
                                       if (Startdate.substring(0, 6) == "/Date(") {
                                           var dt = new Date(parseInt(Startdate.substring(6, Startdate.length - 2)));
                                           var Startdt = dt.getDate() + "/" + (dt.getMonth() + 1) + "/" + dt.getFullYear();//Format Date in regular date format
                                       }
                                       if (EndDate.substring(0, 6) == "/Date(") {
                                           var dt = new Date(parseInt(EndDate.substring(6, EndDate.length - 2)));
                                           var Enddt = dt.getDate() + "/" + (dt.getMonth() + 1) + "/" + dt.getFullYear();//Format Date in regular date format
                                       }
                                       tr.append("<td style=\"Display:None\">" + json[i].Id + "</td>");
                                       tr.append("<td>" + json[i].TrainingName + "</td>");
                                       tr.append("<td>" + json[i].Importance + "</td>");
                                       tr.append("<td>" + Startdt + "</td>");
                                       tr.append("<td>" + Enddt + "</td>");
                                       tr.append("<td>"+ json[i].Days+"</td>");
                                       tr.append("<td>" + json[i].Provider + "</td>");
                                       tr.append("<td>" + json[i].Cost + "</td>");
                                       tr.append("<td><button name=\"button\" value=\"Approve\" type=\"button\" style=\"background-color:cornflowerblue; color:white;border-radius: 7px;\">View Document</button></td>");
                                       tr.append("<td><button name=\"button\" value=\"Approve\" type=\"button\" style=\"background-color:cornflowerblue; color:white;border-radius: 7px;\">Pending</button></td>");
                                       tr.append("<td><input type=\"checkbox\" name=\"Approve\" Class=\"cbCheck\"> Check</td>");
                                       tr.append("<td><input type=\"checkbox\" name=\"Reject\" Class=\"cbCheck1\"> Check</td>");
                                       $('#DocumentModalTable6').append(tr);
                                       var Name, TotalTrainingDaysApproved;
                                       Name = json[i].Name;
                                       TotalTrainingDaysApproved=json[i].TotalTrainingDaysApproved;
                                       $('#TainingWorkerName').val(Name);
                                   //    $('#TrainingApprove').val(TotalTrainingDaysApproved);
                                       $('#10').show();
                                   }
                               });
            Number.prototype.padDigit = function () {
                return (this < 10) ? '0' + this : this;
            }
            $(document).on("click", ".cbCheck", function () {             //Calculate Total hours and days
                if ($('.cbCheck1').is(':checked')) {
                    $('.cbCheck1').attr('checked', false);
             //       $("#TotalTrainingDays").val(0);
                //    $('#TrainingApproved').val(0);
                    $('#TrainingDaysApproved').val(0);
                    $('#TrainingCosts').val(0);
                }
                $('#TrainingApprove').show();
                $('#TrainingReject').hide();
                var checked = $(this).is(":checked");
                if (checked) {
                    var initialDays = $('#TrainingDaysApproved').val();
                    var getRow = $(this).parents('tr');
                    var Days = (getRow.find('td:eq(5)').html());

                    $("#TrainingDaysApproved").val(Days);

                    //var Totaldays = parseInt($('#TotalHolidays').val());
                    //var HolidayTaken = parseInt($('#HolidaysTaken').val());
                    //var HolidayApprove = parseInt($('#Holidaysapprove').val());

                    //var totalday = parseInt(Totaldays) - parseInt(HolidayTaken) - parseInt(HolidayApprove);

                    //$('#RemainingHolidays').val(totalday);
                }
                else {
                    var initialDays = $('#TrainingDaysApproved').val();
                    var getRow = $(this).parents('tr');
                    var Days = (getRow.find('td:eq(5)').html());

                    $("#TrainingDaysApproved").val(Days);
                    //var Totaldays = parseInt($('#TotalHolidays').val());
                    //var HolidayTaken = parseInt($('#HolidaysTaken').val());
                    //var HolidayApprove = parseInt($('#Holidaysapprove').val());

                    //var totalday = parseInt(Totaldays) + parseInt(HolidayTaken) + parseInt(HolidayApprove);

                    //$('#RemainingHolidays').val(totalday);
                }

            });
            $(document).on("click", ".cbCheck1", function () {             //Calculate Total hours and days
                if ($('.cbCheck').is(':checked')) {
                    $('.cbCheck').attr('checked', false);
                    $("#TotalTrainingDays").val(0);
                    $('#TrainingApproved').val(0);
                    $('#TrainingDaysApproved').val(0);
                    $('#TrainingCosts').val(0);

                }
                $('#TrainingApprove').hide();
                $('#TrainingReject').show();
                var checked = $(this).is(":checked");
                if (checked) {
                    var initialhour = $('#TrainingDaysApproved').val();

                    var getRow = $(this).parents('tr');
                    var Days = (getRow.find('td:eq(5)').html());


                    $("#TrainingDaysApproved").val(Days);
                    //var Totaldays = parseInt($('#TotalHolidays').val());
                    //var HolidayTaken = parseInt($('#HolidaysTaken').val());
                    //var HolidayApprove = parseInt($('#Holidaysapprove').val());

                    //var totalday = parseInt(Totaldays) - parseInt(HolidayTaken) - parseInt(HolidayApprove);

                    //$('#RemainingHolidays').val(totalday);

                }
                else {

                    var initialhour = $('#TrainingDaysApproved').val();
                    var getRow = $(this).parents('tr');
                    var Days = (getRow.find('td:eq(5)').html());

                    $("#TrainingDaysApproved").val(Days);
                    //var Totaldays = parseInt($('#TotalHolidays').val());
                    //var HolidayTaken = parseInt($('#HolidaysTaken').val());
                    //var HolidayApprove = parseInt($('#Holidaysapprove').val());

                    //var totalday = parseInt(Totaldays) + parseInt(HolidayTaken) + parseInt(HolidayApprove);

                    //$('#RemainingHolidays').val(totalday);
                }
            });
            $("#TrainingApprove").button().click(function () {

                $('#DocumentModalTable6 [type="checkbox"]').each(function (i, chk) {
                    if (chk.checked) {

                        var getRow = $(this).parents('tr');
                        var ID = (getRow.find('td:eq(0)').html());
                        $.post("/Approval/UpdateTrainingApprova?ID=" + ID + "",
                        function (json) {
                            
                        })
                    }
                });
            });
            $("#TrainingReject").button().click(function () {

                $('#DocumentModalTable6 [type="checkbox"]').each(function (i, chk) {
                    if (chk.checked) {

                        var getRow = $(this).parents('tr');
                        var ID = (getRow.find('td:eq(0)').html());
                        $.post("/Approval/UpdateTrainingReject?ID=" + ID + "",
                        function (json) {
                        
                        })
                    }
                });
            });

        });
        $("#NewVacancyId ul li").click(function (e) {
            var hidden_fields = $(this).find('input:hidden').val();

            $("#DocumentModalTable7 tbody").empty();
            $("#All").hide();
            $.getJSON("/Approval/NewVacancyApprove?EmpID=" + hidden_fields + "",
                               function (json) {

                                   var tr;
                                   //Append each row to html table  
                                   for (var i = 0; i < json.length; i++) {
                                       tr = $('<tr/>');

                                       var Closingdate = json[i].ClosingDate;

                                       if (Closingdate.substring(0, 6) == "/Date(") {
                                           var dt = new Date(parseInt(Closingdate.substring(6, Closingdate.length - 2)));
                                           var clodt = dt.getDate() + "/" + (dt.getMonth() + 1) + "/" + dt.getFullYear();//Format Date in regular date format
                                       }
                                       tr.append("<td style=\"display:none\">" + json[i].Id + "</td>");
                                       tr.append("<td>" + json[i].Title + "</td>");
                                       tr.append("<td>" + clodt + "</td>");
                                       tr.append("<td>" + json[i].RecruitmentProcess + "</td>");
                                       tr.append("<td>" + json[i].Salary + "</td>");
                                       tr.append("<td>" + json[i].Location + "</td>");
                                       tr.append("<td>" + json[i].Business + "</td>");
                                       tr.append("<td>" + json[i].Division + "</td>");
                                       tr.append("<td>" + json[i].Pool + "</td>");
                                       tr.append("<td>" + json[i].Function + "</td>");
                                       tr.append("<td><button name=\"button\" value=\"Approve\" type=\"button\" style=\"background-color:cornflowerblue; color:white;border-radius: 7px;\">View Document</button></td>");
                                       tr.append("<td><button name=\"button\" value=\"Approve\" type=\"button\" style=\"background-color:cornflowerblue; color:white;border-radius: 7px;\">Pending</button></td>");
                                       tr.append("<td><input type=\"checkbox\" name=\"Approve\"> Check</td>");
                                       tr.append("<td><input type=\"checkbox\" name=\"Reject\"> Check</td>");
                                       $('#DocumentModalTable7').append(tr);
                                       $('#11').show();
                                   }
                               });
            $("#NewVacancyApprove").button().click(function () {

                $('#DocumentModalTable7 [type="checkbox"]').each(function (i, chk) {
                    if (chk.checked) {

                        var getRow = $(this).parents('tr');
                        var ID = (getRow.find('td:eq(0)').html());
                        $.post("/Approval/UpdateNewVacancyApprova?ID=" + ID + "",
                        function (json) {
                        })
                    }
                });
            });
            $("#NewVacancyReject").button().click(function () {

                $('#DocumentModalTable7 [type="checkbox"]').each(function (i, chk) {
                    if (chk.checked) {

                        var getRow = $(this).parents('tr');
                        var ID = (getRow.find('td:eq(0)').html());
                        $.post("/Approval/UpdateNewVacancyReject?ID=" + ID + "",
                        function (json) {
                        })
                    }
                });
            });

        });
        $("#UpliftId ul li").click(function (e) {
            var hidden_fields = $(this).find('input:hidden').val();

            $("#DocumentModalTable9 tbody").empty();
            $("#All").hide();
            $.getJSON("/Approval/UpliftApprove?EmpID=" + hidden_fields + "",
                               function (json) {

                                   var tr;
                                   //Append each row to html table  
                                   for (var i = 0; i < json.length; i++) {
                                       tr = $('<tr/>');

                                       var date = json[i].Date;

                                       if (date.substring(0, 6) == "/Date(") {
                                           var dt = new Date(parseInt(date.substring(6, date.length - 2)));
                                           var duedate = dt.getDate() + "/" + (dt.getMonth() + 1) + "/" + dt.getFullYear();//Format Date in regular date format
                                       }
                                       var InTime = json[i].InTimeHr;
                                       var EndTime = json[i].EndTimeHr;
                                       function diff(InTime, EndTime) {         //Calculate Time Difference
                                           InTime = InTime.split(":");
                                           EndTime = EndTime.split(":");
                                           var startDate = new Date(0, 0, 0, InTime[0], InTime[1], 0);
                                           var endDate = new Date(0, 0, 0, EndTime[0], EndTime[1], 0);
                                           var diff = endDate.getTime() - startDate.getTime();
                                           var hours = Math.floor(diff / 1000 / 60 / 60);
                                           diff -= hours * 1000 * 60 * 60;
                                           var minutes = Math.floor(diff / 1000 / 60);

                                           return (hours < 9 ? "0" : "") + hours + ":" + (minutes < 9 ? "0" : "") + minutes;
                                       }
                                       var Hours = diff(InTime, EndTime);

                                       tr.append("<td style=\"display:none\">" + json[i].Id + "</td>");
                                       tr.append("<td>" + json[i].Day + "</td>");
                                       tr.append("<td>" + duedate + "</td>");
                                       tr.append("<td>" + json[i].UpliftPosition + "</td>");
                                       tr.append("<td>" + Hours + "</td>");
                                       tr.append("<td>" + json[i].Project + "</td>");
                                       tr.append("<td>" + json[i].Customer + "</td>");
                                       tr.append("<td>" + json[i].ChangeInWorkerRate + "</td>");
                                       tr.append("<td>" + json[i].ChangeInCustomerRate + "</td>");
                                       tr.append("<td><button name=\"button\" value=\"Approve\" type=\"button\" style=\"background-color:cornflowerblue; color:white;border-radius: 7px;\">View Document</button></td>");
                                       tr.append("<td><button name=\"button\" value=\"Approve\" type=\"button\" style=\"background-color:cornflowerblue; color:white;border-radius: 7px;\">Pending</button></td>");
                                       tr.append("<td><input type=\"checkbox\" name=\"Approve\" Class=\"cbCheck\"> Check</td>");
                                       tr.append("<td><input type=\"checkbox\" name=\"Reject\" Class=\"cbCheck1\"> Check</td>");
                                       $('#DocumentModalTable9').append(tr);
                                       var Name = json[i].Name;
                                       $('#UpliftWorkerName').val(Name);
                                       $('#13').show();
                                   }
                               });
            Number.prototype.padDigit = function () {
                return (this < 10) ? '0' + this : this;
            }
            $(document).on("click", ".cbCheck", function () {             //Calculate Total hours and days
                if ($('.cbCheck1').is(':checked')) {
                    $('.cbCheck1').attr('checked', false);
        //            $("#TotalHrUpliftInTheYear").val('00:00');
                    $('#NoofUpliftApproved').val(0);
                }
                $('#Upliftapprove').show();
                $('#UpliftReject').hide();
                var checked = $(this).is(":checked");
                if (checked) {
                    //var initialhour = $('#TotalHrUpliftInTheYear').val();
                    //var getRow = $(this).parents('tr');
                    //var hour = (getRow.find('td:eq(4)').html());
                    //var InTime1 = initialhour.split(":");
                    //var t1 = initialhour;
                    //var mins = 0;
                    //var hrs = 0;
                    //t1 = t1.split(':');
                    //var t2 = hour.split(':');
                    //console.log(Number(t1[1]) + Number(t2[1]))
                    //mins = Number(t1[1]) + Number(t2[1]);
                    //var minhrs = Math.floor(parseInt(mins / 60));
                    //hrs = Number(t1[0]) + Number(t2[0]) + minhrs;
                    //mins = mins % 60;
                    //t1 = hrs.padDigit() + ':' + mins.padDigit();
                    //console.log(t1)

                    //$("#TotalHrUpliftInTheYear").val(t1);

                    var initialdays = parseInt($('#NoofUpliftApproved').val());
                    initialdays = initialdays + 1;

                    $('#NoofUpliftApproved').val(initialdays);
                }
                else {
                    //var initialhour = $('#totalhour').val();
                    //var getRow = $(this).parents('tr');
                    //var hour = (getRow.find('td:eq(3)').html());
                    //var InTime1 = initialhour.split(":");
                    //var t1 = initialhour;
                    //var mins = 0;
                    //var hrs = 0;
                    //t1 = t1.split(':');
                    //var t2 = hour.split(':');
                    //console.log(Number(t1[1]) + Number(t2[1]))
                    //mins = Number(t1[1]) - Number(t2[1]);
                    //var minhrs = Math.floor(parseInt(mins / 60));
                    //hrs = Number(t1[0]) - Number(t2[0]) + minhrs;
                    //mins = mins % 60;
                    //t1 = hrs.padDigit() + ':' + mins.padDigit();
                    //console.log(t1)

                    //$("#totalhour").val(t1);
                    var initialdays = parseInt($('#NoofUpliftApproved').val());
                    initialdays = initialdays - 1;

                    $('#NoofUpliftApproved').val(initialdays);
                }

            });
            $(document).on("click", ".cbCheck1", function () {             //Calculate Total hours and days
                if ($('.cbCheck').is(':checked')) {
                    $('.cbCheck').attr('checked', false);
                 //   $("#TotalHrUpliftInTheYear").val('00:00');
                    $('#NoofUpliftApproved').val(0);

                }
                $('#Upliftapprove').show();
                $('#UpliftReject').hide();
                var checked = $(this).is(":checked");
                if (checked) {
                    //var initialhour = $('#totalhour').val();

                    //var getRow = $(this).parents('tr');
                    //var hour = (getRow.find('td:eq(3)').html());
                    //var InTime1 = initialhour.split(":");
                    //var t1 = initialhour;
                    //var mins = 0;
                    //var hrs = 0;
                    //t1 = t1.split(':');
                    //var t2 = hour.split(':');
                    //console.log(Number(t1[1]) + Number(t2[1]))
                    //mins = Number(t1[1]) + Number(t2[1]);
                    //var minhrs = Math.floor(parseInt(mins / 60));
                    //hrs = Number(t1[0]) + Number(t2[0]) + minhrs;
                    //mins = mins % 60;
                    //t1 = hrs.padDigit() + ':' + mins.padDigit();
                    //console.log(t1)

                    //$("#totalhour").val(t1);
                    var initialdays = parseInt($('#NoofUpliftApproved').val());
                    initialdays = initialdays + 1;

                    $('#NoofUpliftApproved').val(initialdays);

                }
                else {

                    //var initialhour = $('#totalhour').val();
                    //var getRow = $(this).parents('tr');
                    //var hour = (getRow.find('td:eq(3)').html());
                    //var InTime1 = initialhour.split(":");
                    //var t1 = initialhour;
                    //var mins = 0;
                    //var hrs = 0;
                    //t1 = t1.split(':');
                    //var t2 = hour.split(':');
                    //console.log(Number(t1[1]) + Number(t2[1]))
                    //mins = Number(t1[1]) - Number(t2[1]);
                    //var minhrs = Math.floor(parseInt(mins / 60));
                    //hrs = Number(t1[0]) - Number(t2[0]) + minhrs;
                    //mins = mins % 60;
                    //t1 = hrs.padDigit() + ':' + mins.padDigit();
                    //console.log(t1)

                    //$("#totalhour").val(t1);
                    var initialdays = parseInt($('#NoofUpliftApproved').val());
                    initialdays = initialdays - 1;

                    $('#NoofUpliftApproved').val(initialdays);
                }
            });
            $("#Upliftapprove").button().click(function () {

                $('#DocumentModalTable9 [type="checkbox"]').each(function (i, chk) {
                    if (chk.checked) {

                        var getRow = $(this).parents('tr');
                        var ID = (getRow.find('td:eq(0)').html());
                        $.post("/Approval/UpdateUpliftApprova?ID=" + ID + "",
                        function (json) {
                        })
                    }
                });
            });
            $("#UpliftReject").button().click(function () {

                $('#DocumentModalTable9 [type="checkbox"]').each(function (i, chk) {
                    if (chk.checked) {

                        var getRow = $(this).parents('tr');
                        var ID = (getRow.find('td:eq(0)').html());
                        $.post("/Approval/UpdateUpliftReject?ID=" + ID + "",
                        function (json) {
                        })
                    }
                });
            });

        });
   
});


function DataTableDesign() {
    var table = $('#DocumentModalTable').DataTable({
        "sDom": '<"top"i>rt<"bottom"flp><"clear">'
    });
    $('#tableDiv').find('.dataTables_filter').hide();
    $('#tableDiv').find('.dataTables_info').hide();

    $('#DocumentModalTable tfoot tr').appendTo('#DocumentModalTable thead');

    $("#DocumentModalTable thead .SearchDay").keyup(function () {
        table.column(0).search(this.value).draw();
    });
    $("#DocumentModalTable thead .SearchDate").keyup(function () {
        table.column(1).search(this.value).draw();
    });
    $("#DocumentModalTable thead .SearchHours").keyup(function () {
        table.column(2).search(this.value).draw();
    });
    $("#DocumentModalTable thead .SearchCostCode").keyup(function () {
        table.column(3).search(this.value).draw();
    });
    $("#DocumentModalTable thead .SearchProject").keyup(function () {
        table.column(4).search(this.value).draw();
    });
    $("#DocumentModalTable thead .SearchCustomer").keyup(function () {
        table.column(5).search(this.value).draw();
    });
    $("#DocumentModalTable thead .SearchAsset").keyup(function () {
        table.column(6).search(this.value).draw();
    });
    $("#DocumentModalTable thead .SearchStatus").keyup(function () {
        table.column(8).search(this.value).draw();
    });

    //$("#DocumentModalTable thead .SearchCreated").Zebra_DatePicker({
    //    //direction: false,
    //    showButtonPanel: false,
    //    format: 'd-m-Y',
    //    onSelect: function () {
    //        var date = $("#DocumentModalTable").find("thead").find('.SearchCreated').val();
    //        table.column(8).search(date).draw();
    //    }
    //});

    //$("body").on('click', '.dp_clear', function () {
    //    var date = $("#DocumentModalTable").find("thead").find('.SearchCreated').val();
    //    table.column(8).search(date).draw();
    //});
}

$("#tableDiv").on('click', '.btn-Refresh-Document', function () {
    window.location.reload();
});
$("#tableDiv").on('click', '.btn-ClearSorting-Document', function () {
    window.location.reload();
});
$("#tableDiv").on('click', '.btn-clearFilter-Document', function () {
    window.location.reload();
});
