//Below script shows demo of Left Hand side Navigation bar from _YTLayout.cshtml page converted to Javascript component
/* begin component definition */
var eventTarget = document.getElementById('sidebar');

var SideBar = (function () {
    var getSelectedNavItem = function () {
        var selNavItem = $('#sidebar ul li.nav-item.active');
        return selNavItem.find('span').text();
    }

    var setSelectedNavItem = function (newItem) {
        var selNavItem;
        $('#sidebar ul li a.nav-link span').each(function () {
            if ($(this).text() == newItem)
                selNavItem = $(this.closest('.nav-item'));
        });
        if (selNavItem) {
            selNavItem.siblings().removeClass('active');
            selNavItem.addClass('active');
        }
    };

    var onNavItemSelect = function (eventHandler) {
        eventTarget.addEventListener('menuSelected', eventHandler);
    }

    return {
        setSelectedNavItem: setSelectedNavItem,
        getSelectedNavItem: getSelectedNavItem,
        onNavItemSelect: onNavItemSelect
    };
})();

$('#sidebar ul li a.nav-link').click(function () {
    var navItem = $(this).closest('.nav-item');
    navItem.siblings().removeClass('active');
    navItem.addClass('active');

    var linkText = $(this).children('span').text();
    var menuSelected = new CustomEvent('menuSelected', {
        detail: {
            linkText: linkText,
        }
    });
    eventTarget.dispatchEvent(menuSelected);
});
/* End component definiition */

/* begin component usage */
SideBar.setSelectedNavItem('Comments');
alert(SideBar.getSelectedNavItem());

var myEventHandler = function (e) {
    alert('You clicked ' + e.detail.linkText);
}
SideBar.onNavItemSelect(myEventHandler);
/* end component usage */