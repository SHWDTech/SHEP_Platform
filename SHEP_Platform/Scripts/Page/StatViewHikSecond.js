var StatMonitor = null;

var started = false;

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
    StatMonitor.SetConnectServer(location.host);
    StatMonitor.SetupCamera($('#cameraId').val());
    StatMonitor.SetupDevId($('#devId').val());
});

var SwitchMonitorStatus = function () {
    if (!started) {
        if (StatMonitor.StartMonitor() === 0) {
            started = true;
            $('#monitorSwitch').val('结束');
        }
    } else {
        if (StatMonitor.StopMonitor() === 0) {
            started = false;
            $('#monitorSwitch').val('开始');
        }
    }
    
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