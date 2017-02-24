var StatViewer = {};
var Cameras = [];

StatViewer.ViewObject = document.getElementById('StatViewer');

StatViewer.ConnInfo = {};

StatViewer.DeviceInfos = [];

StatViewer.Init = function (info) {
    this.ConnInfo.Server = info.DnsAddr;
    this.ConnInfo.Port = info.Port;
    this.ConnInfo.UserName = info.UserName;
    this.ConnInfo.PassWord = info.PassWord;
    this.ViewObject.LoginStyle = 1;
    this.Login();
};

StatViewer.Login = function () {
    if (!BaseInfo.IsIE) {
        msg.warning("请使用IE内核的浏览器打开本页面！");
        return;
    }

    var logResult = this.ViewObject.LoginServer(this.ConnInfo.Server, this.ConnInfo.Port, this.ConnInfo.UserName, this.ConnInfo.PassWord);
    if (!logResult) {
        msg.warning("连接视频服务器错误，请稍候，正在尝试重新连接");
        setTimeout(function () { this.Login(); }, 30000);
    } else {
        this.LoadDeviceInfo();

        setTimeout(function () { StatViewer.StartMonitor(); }, 3000);
    }
}

StatViewer.LoadDeviceInfo = function () {
    var devCount = this.ViewObject.StartGetDeviceList();

    for (var i = 0; i < devCount; i++) {
        var devInfo = this.ViewObject.GetNextDeviceInfo();
        this.DeviceInfos.push(this.InitDev(devInfo));
    }
}

StatViewer.InitDev = function (devInfo) {
    var dev = {};
    dev.deviceId = devInfo.split(' ')[0].split(':')[1];
    return dev;
}

StatViewer.StartMonitor = function () {
    this.ViewObject.PlayVideo(this.DeviceInfos[1].deviceId, 0);
}


StatViewer.PTZCtrl = function (cmd, delay) {
    var ret = this.ViewObject.PTZCtrl(this.DeviceInfos[1].deviceId, 0, cmd);
    console.log(ret);

    var stop = false;
    if (delay == null) delay = 200;
    setTimeout(function () { stop = StatViewer.ViewObject.PTZCtrl(StatViewer.DeviceInfos[1].deviceId, 0, 0) === 0; }, delay);
    console.log("stoped" + stop);
}

StatViewer.Up = function () {
    this.PTZCtrl(1, 100);
}

StatViewer.Down = function () {
    this.PTZCtrl(2, 100);
}

StatViewer.Left = function () {
    this.PTZCtrl(3);
}

StatViewer.Right = function () {
    this.PTZCtrl(4);
}

StatViewer.Far = function () {
    this.PTZCtrl(6, 500);
}

StatViewer.Near = function () {
    this.PTZCtrl(7, 500);
}

StatViewer.FocusFar = function () {
    this.PTZCtrl(8);
}

StatViewer.FocusNear = function () {
    this.PTZCtrl(9);
}

StatViewer.TakePicture = function () {
    var ret = this.ViewObject.SnapPicture('d:\\201603171.jpg');
    if (ret === 0) {
        alert('截图成功');
    } else {
        alert('截图失败');
    }
}

function Up() {
    StatViewer.Up();
}

function Down() {
    StatViewer.Down();
}

function Left() {
    StatViewer.Left();
}

function Right() {
    StatViewer.Right();
}

function Far() {
    StatViewer.Far();
}

function Near() {
    StatViewer.Near();
}

function FocusFar() {
    StatViewer.FocusFar();
}

function FocusNear() {
    StatViewer.FocusNear();
}

function TakePicture() {
    StatViewer.TakePicture();
}