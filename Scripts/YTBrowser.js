//Below function will be executed on View Load
$(document).ready(function () {
    //https://www.w3schools.com/bootstrap/bootstrap_popover.asp

    $('[data-toggle="popover"]').popover({
        placement: 'auto left',
        html: true,
        title: function () { return $('.btn-row-popup-menu-head').html() },
        content: ''
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

$('[data-toggle="popover"]').on('shown.bs.popover', function () {
    var popoverWindow = $(this);
    $('.btn-popover-close').click(function () {
        popoverWindow.popover('hide');
    });
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
