//This file is currently not used but planning to use in _Layout.cshtml view for introducing application common scripts
$(document).on({
    ajaxStart: function () {
        $("body").addClass("loading");
    },
    ajaxStop: function () {
        $("body").removeClass("loading");
    }
});