var currentMenuItem = null;
var activedMenuItem = null;
var delayActive = null;
var lastPoint = null;
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
            }
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
        if (result.total > 0) {
            $('#alarmCount').addClass('badge-danger');
            $('#alarmCount').html(result.total);
            $('#alarmDetails')[0].style.display = '';
            for (var i = 0; i < result.details.length; i++) {
                var item = result.details[i];
                $('#alarmDetails').append('<div style="padding: 10px; font-size: 12px;"><a href="/Monitor/ActualStatus/' 
                    + item.Id + '">' 
                    + item.StatName 
                    + '</a>'
                    + '</br>' 
                    + '类型:'
                    + item.AlarmType
                    + '</br>'
                    + '报警值：'
                    + item.AlarmValue
                    + '</br>'
                    + '时间:'
                    + item.AlarmDateTime
                    + '</div></hr>');
            }
            
        }
    });
}