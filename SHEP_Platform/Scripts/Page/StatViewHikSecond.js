var StatMonitor = null;

$(function () {
    if (!BaseInfo.IsIE) {
        msg.warning("请使用IE内核的浏览器打开本页面！");
        return;
    }

    StatMonitor = document.getElementById('StatMonitor');
    StatMonitor.SetConnectServer("localhost:6330");
    StatMonitor.SetupCamera("626519921");
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