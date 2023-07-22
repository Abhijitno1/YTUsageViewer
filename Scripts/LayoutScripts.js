//This file is currently not used but planning to use in _Layout.cshtml view for introducing application common scripts
$(document).on({
    ajaxStart: function () {
        $("body").addClass("loading");
    },
    ajaxStop: function () {
        $("body").removeClass("loading");
    }
});
function showLoadingSign(container) {
    if (!container) container = document.body;
    $(container).append("<div class='overlay'></div>");
    $(container).addClass("loading");
}
function hideLoadingSign(container) {
    if (!container) container = document.body;
    $(container).find(".overlay").remove();
    $(container).removeClass("loading");
}

function extractDateFromJson(jsonDate, returnTime) {
    try {
        var strDate = jsonDate.substring(6, jsonDate.length - 2);
        var dtDate = new Date(parseFloat(strDate));
        if (returnTime)
            return dtDate.toLocaleString();
        else
            return dtDate.getMonth() + '/' + dtDate.getDate() + '/' + dtDate.getFullYear();
    }
    catch {
        //console.log('Error occurred while parsing json date ' + jsonDate);
        return jsonDate;
    }
}
