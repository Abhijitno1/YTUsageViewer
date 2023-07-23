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
            return (dtDate.getMonth() + 1) + '/' + dtDate.getDate() + '/' + dtDate.getFullYear();
    }
    catch {
        //console.log('Error occurred while parsing json date ' + jsonDate);
        return jsonDate;
    }
}

//Ref: https://www.aspsnippets.com/Articles/Download-Excel-File-using-AJAX-in-jQuery.aspx
function downloadFile(apiCallUrl, apiCallData, fileName) {

    $.ajax({
        url: apiCallUrl,
        cache: false,
        method: 'get',
        contentType: 'application/json',
        data: apiCallData,
        xhr: function () {
            var xhr = new XMLHttpRequest();
            xhr.onreadystatechange = function () {
                if (xhr.readyState == 2) {
                    if (xhr.status == 200) {
                        xhr.responseType = "blob";
                    } else {
                        xhr.responseType = "text";
                    }
                }
            };
            return xhr;
        },
        success: function (data) {
            //Convert the Byte Data to BLOB object.
            var blob = new Blob([data], { type: "application/octetstream" });

            //Check the Browser type and download the File.
            var isIE = false || !!document.documentMode;
            if (isIE) {
                window.navigator.msSaveBlob(blob, fileName);
            } else {
                var url = window.URL || window.webkitURL;
                link = url.createObjectURL(blob);
                var a = $("<a />");
                a.attr("download", fileName);
                a.attr("href", link);
                $("body").append(a);
                a[0].click();
                $("body").remove(a);
            }
        }
    });
}
