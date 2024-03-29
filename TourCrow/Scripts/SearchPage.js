﻿var newData;

$("#submit").click(function () {
    var getData = $("#searchbar").val();
    /*$.ajax({
        url: "/plan/getJsonData?place_name=" + getData, success: function (result) {
            alert(result);
        }
    });*/

    $.getJSON("/plan/getJsonData?place_name=" + getData, function (data) {
        newData = data;
        var htmlToset = "";
        var len = newData.length > 16 ? 16 : newData.length;
        for (var i = 0; i < len; i++) {
            htmlToset += makeSuggPlace(newData[i]["place_id"], newData[i]["place_name"], newData[i]["place_photo"], newData[i]["place_rating"],i);
        }
        $("#suggestPlace").html(htmlToset);
    });
});

function makeSuggPlace(placeId, name, photo, ratting, index) {
    //placeId = placeId.replace("-", "/");
    var retData = "<div class='sugg-element'> <div class='main-area-for-element'><div class='imageContaner'><img src='https://maps.googleapis.com/maps/api/place/photo?maxwidth=400&photoreference=" + photo + "&key=AIzaSyCSRXTF9nWyECowpOmrihkoCMtnJmVQQQo' alt=''/> <div class='action-for-sugg'><input type='button' value='Add' onclick=\"addItemToPackage(" + index + ")\"class='add-to-list'/> </div></div><p class='name-text'>" + name + "</p> <p class='rating-text'>Rating : " + ratting + "</p></div></div>";
    return retData;
}

function addItemToPackage(index) {
    //alert(newData[index]["place_id"]);
    var ret_val = plan_div(newData[index]["place_photo"], newData[index]["place_name"], newData[index]["place_address"], newData[index]["place_id"], index);
    $("#plan_list").html($("#plan_list").html() + ret_val);
}

function plan_div(place_photo, place_name, place_address, place_id, index) {
    var inside_div = "<div class='plan-single-element' id='" + place_id + "'><input type='hidden' name='pid' value='" + place_id + "'><input type='hidden' name='photoid' value='" + place_photo + "'><img src='https://maps.googleapis.com/maps/api/place/photo?maxwidth=400&photoreference=" + place_photo + "&key=AIzaSyCSRXTF9nWyECowpOmrihkoCMtnJmVQQQo' alt=''/><div class='desc-elemetn'><h3>" + place_name + "</h3><p>" + place_address + "</p><div class='list-action-area'><div class='remove-place'><span onclick = 'remove_view(" + index + ")'>Remove</span></div><div class='more-info'><span>More Info</span></div></div></div><div class='clear'></div></div>";

    addNewMarkerToAdd(newData[index]["place_latitude"], newData[index]["place_longitude"], index);
    //initialize();
    return inside_div;
}

function remove_view(index) {
    $("#" + newData[index]["place_id"]).remove();
    deleteMarker(index);
}

function checkGetParm() {


    $(document).ready(function() {
        var getData = document.getElementById("getValue").value;

        if (getData != "null") {
            $.getJSON("/plan/getJsonData?place_name=" + getData, function(data) {
                newData = data;
                var htmlToset = "";
                var len = newData.length > 16 ? 16 : newData.length;
                for (var i = 0; i < len; i++) {
                    htmlToset += makeSuggPlace(newData[i]["place_id"], newData[i]["place_name"], newData[i]["place_photo"], newData[i]["place_rating"], i);
                }
                $("#suggestPlace").html(htmlToset);
            });
        }
    });
}




checkGetParm();


