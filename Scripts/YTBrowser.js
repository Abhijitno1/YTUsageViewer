//Below function will be executed on View Load
$(document).ready(function () {
    //Ref 1: https://www.w3schools.com/bootstrap/bootstrap_popover.asp
    //Ref 2: https://stackoverflow.com/questions/62753400/dynamically-added-bootstrap-popover-not-working
    $("body").popover({
        trigger: "click",
        sanitize: false,
        html: true,
        animation: true,
        selector: '[data-toggle="popover"]',
        //container: '.btn-row-popup-menu-head',
        placement: 'auto left',
        title: function () { return $('.btn-row-popup-menu-head').html() },
        content: ''
    });

    $('body').on('shown.bs.popover', '[data-toggle="popover"]', function () {
        var popoverWindow = $(this);
        $('.btn-popover-close').click(function () {
            popoverWindow.popover('hide');
        });
    });
    /*$(document).on("click", "[data-toggle='popover']", function (event) {
        event.stopPropagation();
    });
    $(document).on("click", function () {
        $("[data-toggle='popover']").popover("hide");
    });*/

    //https://bootstrap-datepicker.readthedocs.io/en/latest/options.html
    $('.date').datepicker({
        //orientation: 'top',
        todayBtn: 'linked',
        autoclose: true
    });
});

$('#sidebarCollapse').on('click', function () {
    $('#sidebar').toggleClass('active');
});

$('#sidebar ul li a.nav-link').click(function () {
    var navItem = $(this).closest('.nav-item');
    navItem.siblings().removeClass('active');
    navItem.addClass('active');
});

function pageGrid(pageNumber) {
    $("#pageNumber").val(pageNumber);
    $("form").submit();
}

function sortGridInner(newSortOrder, newSortDir) {
    $("#sortOrder").val(newSortOrder);
    $("#sortDir").val(newSortDir);
    $("form").submit();
}

//Ref: https://bootstrapfriendly.com/blog/how-to-make-bootstrap-modal-draggable-and-resizable/
function showDraggableModal(jqElm) {
    $(jqElm).modal({
        backdrop: false,
        show: true,
    });
    /*
    // reset modal if it isn't visible
    if (!jqElm.hasClass(".in")) {
        $jqElm.children(".modal-dialog").css({
            top: 20,
            left: 100,
        });
    }
    */
    $(jqElm).draggable({
    //$(".modal-dialog").draggable({
        cursor: "move",
        handle: ".modal-header",
    });
}

function truncatedGridColumn(fullText, maxColCharLength) {
    fullText = fullText || '';
    fullText = fullText.trim();
    maxColCharLength = maxColCharLength || 100;

    var displayText = fullText;
    var magnifier = '';
    if (fullText.length > maxColCharLength) {
        displayText = fullText.substring(0, maxColCharLength) + "...";
        var tb = document.createElement("a");
        tb.classList.add('pull-right');
        tb.setAttribute("href", "#");
        tb.setAttribute("data-toggle", "popover");
        //tb.setAttribute("data-trigger", "focus");
        tb.setAttribute("data-content", fullText);

        var tbSpan = document.createElement("span");
        tbSpan.classList.add("glyphicon");
        tbSpan.classList.add("glyphicon-zoom-in");
        tb.appendChild(tbSpan);
        magnifier = tb.outerHTML;
    }
    return displayText + magnifier;
}
