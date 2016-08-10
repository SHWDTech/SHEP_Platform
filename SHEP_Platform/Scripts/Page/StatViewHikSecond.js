var StatMonitor = null;

$(function () {
    if (!BaseInfo.IsIE) {
        msg.warning("请使用IE内核的浏览器打开本页面！");
        return;
    }

    try {
        var obj = new ActiveXObject('MonitorViewer.MonitorViewer');
    }
    catch (e) {
        $('#ocxAlarm').show();
        return;
    }

    StatMonitor = document.getElementById('StatMonitor');
    StatMonitor.SetConnectServer("139.196.194.156");
    StatMonitor.SetupCamera($('#cameraId').val());
});

var StartMonitor = function() {
    StatMonitor.StartMonitor();
};

var Up = function () {
    StatMonitor.ControlPlatform('UP');
}

var Down = function () {
    StatMonitor.ControlPlatform('DOWN');
}

var Left = function () {
    StatMonitor.ControlPlatform('LEFT');
}

var Right = function () {
    StatMonitor.ControlPlatform('RIGHT');
}

var ZoomIn = function () {
    StatMonitor.ControlPlatform('ZOOMIN');
}

var ZoomOut = function () {
    StatMonitor.ControlPlatform('ZOOMOUT');
}

var Stop = function() {
    StatMonitor.StopControlPlatform();
}