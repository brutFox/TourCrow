var newData;

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
        var len = newData.length > 8 ? 8 : newData.length;
        for (var i = 0; i < len; i++) {
            htmlToset += makeSuggPlace(newData[i]["place_id"], newData[i]["place_name"], newData[i]["place_photo"], newData[i]["place_rating"],i);
        }
        $("#suggestPlace").html(htmlToset);
    });
});

function makeSuggPlace(placeId, name, photo, ratting, index) {
    placeId = placeId.replace("-", "/");
    var retData = "<div class='sugg-element'> <div class='main-area-for-element'><img src='https://maps.googleapis.com/maps/api/place/photo?maxwidth=400&photoreference=" + photo + "&key=AIzaSyD7FknkbSmCxMXsWnnCcL106f94KKrKA2Q' alt=''/> <p>" + name + "</p> <p>Ratting : " + ratting + "</p><div class='action-for-sugg'><input type='button' value='Add' onclick=\"addItemToPackage(" + index + ")\"class='add-to-list'/><br/><input type='button' value='Remove' class='remove-from-list'/> </div></div></div>";
    return retData;
}

function addItemToPackage(index) {
    alert(newData[index]["place_id"]);
}


