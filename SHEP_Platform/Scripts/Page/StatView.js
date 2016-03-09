var StatViewer = {};
var Cameras = [];

StatViewer.ViewObject = document.getElementById('StatViewer');

StatViewer.ConnInfo = {};

StatViewer.Init = function(info) {
    this.ConnInfo.Server = info.DnsAddr;
    this.ConnInfo.Port = info.Port;
    this.ConnInfo.UserName = info.UserName;
    this.ConnInfo.PassWord = info.PassWord;
    this.Login();
};

StatViewer.Login = function () {
    if (!BaseInfo.IsIE) {
        msg.warning("请使用IE内核的浏览器打开本页面！");
        return;
    }

    if (!this.ViewObject.LoginServer(this.ConnInfo.Server, this.ConnInfo.Port, this.ConnInfo.UserName, this.ConnInfo.PassWord)) {
        msg.warning("连接视频服务器错误，请稍候，正在尝试重新连接");
        setTimeout(function() { StatViewer.Login(); }, 30000);
    }
}