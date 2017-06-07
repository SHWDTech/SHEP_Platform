var currentMenuItem = null;
var activedMenuItem = null;
var delayActive = null;
var lastPoint = {
    x: 0,
    y: 0
};
var IsDelay = false;
$(function () {
    $('.sidebar > ul > li').on('mouseenter mouseleave', function (e) {
        if (e.type === 'mouseenter') {
            currentMenuItem = $(this);
        } else {
            currentMenuItem = null;
            if (delayActive != null) {
                clearTimeout(delayActive);
                delayActive = null;
            }
        }
    });
    $('.sidebar').on('mouseleave', function () {
        $('.sidebar > ul > li').find('ul').hide();
    });

    $('.sidebar').on('mouseenter mousemove', function (e) {
        if (e.type === 'mouseenter') {
            lastPoint = {
                x: e.clientX,
                y: e.clientY
            };
        } else {
            if ((e.clientY - lastPoint.y) < 0) {
                IsDelay = false;
            } else {
                IsDelay = (e.clientX - lastPoint.x) > 0 ? true : false;
            }
            lastPoint = {
                x: e.clientX,
                y: e.clientY
            }
            if (delayActive != null) {
                clearTimeout(delayActive);
                delayActive = null;
            }
            if (IsDelay) {
                delayActive = setTimeout("activeChildMenu()", 1500);
            } else {
                activeChildMenu();
            }
        }
    });

    getAlarmInformation();
});

function activeChildMenu() {
    $('.sidebar > ul > li').find('ul').hide();
    $(currentMenuItem).addClass('hover');
    $(currentMenuItem).find('ul').show();
    activedMenuItem = currentMenuItem;
}

function getAlarmInformation() {
    $.post("/Ajax/Access", { 'fun': 'getAlarmInfo' }, function (result) {
        if (result.inProcess > 0) {
            $('#alarmCount').addClass('badge-danger');
            $('#alarmCount').html(result.inProcess);
            $('#alarmDetails')[0].style.display = '';
            
        }
    });
}