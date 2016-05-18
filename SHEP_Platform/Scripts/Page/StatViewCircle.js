var StatViewer = {};
var Cameras = [];
var OrgList = [];
var started = false;

StatViewer.ViewObject = document.getElementById('StatViewer');

$(function () {
    StatViewer.Login();
});

StatViewer.Login = function () {
    if (!BaseInfo.IsIE) {
        msg.warning("请使用IE内核的浏览器打开本页面！");
        return;
    }

    try {
        var obj = new ActiveXObject('WEBOCX.WebOCXCtrl.1');
    }
    catch (e) {
        $('#ocxAlarm').show();
        return;
    }

    logResult = this.ViewObject.ServiceLogin("121.42.34.0", 5160, "admin", "12345");
    if (logResult !== 1) {
        msg.warning("连接视频服务器错误，请稍候，正在尝试重新连接");
        setTimeout(function () { this.Login(); }, 30000);
    } else {
        QueryOrgList();
        if (OrgList.length !== 0) {
            QueryCamList(OrgList[0].Code);
        }
    }
}

function Start() {
    if (Cameras.length !== 0) {
        this.StartMonitor(Cameras[1].Code);
        $('#btnStart').val('结束预览');
    } else {
        alert("摄像机尚未注册，请先注册后再尝试！");
    }
};

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

StatViewer.StartMonitor = function (cameraId) {
    this.ViewObject.StartMonitor(cameraId.toString());
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
    PTZCtrlStart(6);
}

StatViewer.Down = function () {
    PTZCtrlStart(10);
}

StatViewer.Left = function () {
    PTZCtrlStart(12);
}

StatViewer.Right = function () {
    PTZCtrlStart(8);
}

StatViewer.Far = function () {
    PTZCtrlStart(3);
}

StatViewer.Near = function () {
    PTZCtrlStart(2);
}

StatViewer.FocusFar = function () {
    PTZCtrlStart(0);
}

StatViewer.FocusNear = function () {
    PTZCtrlStart(1);
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

function QueryOrgList(orgid) {
    var str;
    var rValue;
    var strXml;
    if (StatViewer.ViewObject) {
        var str1;
        if (orgid === 0) {
            str1 = "Domain";
        }
        else {
            str1 = "" + orgid + "";
        }

        strXml = StatViewer.ViewObject.QueryOrganizationList(str1, rValue);
        if (strXml === "FALSE") {
            return;
        }
        try //Internet Explorer 
        {
            xmlDoc = new ActiveXObject("Microsoft.XMLDOM");
            xmlDoc.async = "false";
            xmlDoc.loadXML(strXml);
        }
        catch (e) {
            try //Firefox, Mozilla, Opera, etc. 
            {
                parser = new DOMParser();
                xmlDoc = parser.parseFromString(strXml, "text/xml");
            }
            catch (e) {
                alert(e.message);
                return;
            }
        }

        var subNum;
        var i, j;
        var orgname;
        var orgcode;
        var orgstatus;
        var elements = xmlDoc.documentElement;
        childnode = elements.childNodes;
        for (i = 0; i < childnode.length; i++) {
            flag = 0;
            nodename = childnode[i].nodeName;

            if (nodename === "Parent") {
                temp = childnode[i].childNodes[0].nodeValue;
                str += "Parent:" + temp + "\r\n";

            }
            else if (nodename === "SubNum") {
                temp = childnode[i].childNodes[0].nodeValue;
                str += "SubNum:" + temp + "\r\n";
                subNum = parseInt(temp);
            }
            else if (nodename === "SubList") {
                str += "##################################" + "\r\n";
                if (subNum !== childnode[i].childNodes.length) {
                    str += "SubList does not match the SubNum" + "\r\n";
                }
                else {
                    for (j = 0; j < childnode[i].childNodes.length; j++)//Item  length
                    {
                        orgname = "";
                        orgcode = "";
                        nodename = childnode[i].childNodes[j].nodeName;//Item name 

                        for (k = 0; k < childnode[i].childNodes[j].childNodes.length; k++)//
                        {
                            nodename = childnode[i].childNodes[j].childNodes[k].nodeName;
                            if (nodename === "Name") {
                                temp = childnode[i].childNodes[j].childNodes[k].childNodes[0].nodeValue;
                                str += "Name:" + temp + "\r\n";
                                orgname = temp;
                            }
                            else if (nodename === "Address") {
                                temp = childnode[i].childNodes[j].childNodes[k].childNodes[0].nodeValue;
                                str += "Address:" + temp + "\r\n";
                                orgcode = temp;
                            }
                            else if (nodename === "ResType") {
                                temp = childnode[i].childNodes[j].childNodes[k].childNodes[0].nodeValue;
                                str += "ResType:" + temp + "\r\n";
                            }
                            else if (nodename === "ResSubType") {
                                temp = childnode[i].childNodes[j].childNodes[k].childNodes[0].nodeValue;
                                str += "ResSubType:" + temp + "\r\n";
                            }
                        }
                        str += "##################################" + "\r\n";
                        if (orgname !== "" && orgcode !== "") {
                            var org = {};
                            org.Id = orgid;
                            org.Code = orgcode;
                            org.Name = orgname;
                            org.Status = orgstatus;
                            OrgList.push(org);
                        }
                    }
                }
            }
        }
    }
    else {
        alert("QueryOrgList控件未注册");
    }
}

function QueryCamList(orgid) {
    var str;
    var rValue;
    var strXml;
    var camname;
    var camcode;
    var camstatus;
    if (StatViewer.ViewObject) {
        strXml = StatViewer.ViewObject.QueryCameraList("" + orgid + "", rValue);
        if (strXml === "FALSE") {
            return;
        }
        try //Internet Explorer 
        {
            xmlDoc = new ActiveXObject("Microsoft.XMLDOM");
            xmlDoc.async = "false";
            xmlDoc.loadXML(strXml);
        }
        catch (e) {
            try //Firefox, Mozilla, Opera, etc. 
            {
                parser = new DOMParser();
                xmlDoc = parser.parseFromString(strXml, "text/xml");
            }
            catch (e) {
                alert(e.message);
                return;
            }
        }
        var subNum;
        var i, j;
        var elements = xmlDoc.documentElement;
        childnode = elements.childNodes;
        for (i = 0; i < childnode.length; i++) {
            flag = 0;
            nodename = childnode[i].nodeName;
            if (nodename === "Parent") {
                temp = childnode[i].childNodes[0].nodeValue;
                str += "Parent:" + temp + "\r\n";

            }
            else if (nodename === "SubNum") {
                temp = childnode[i].childNodes[0].nodeValue;
                str += "SubNum:" + temp + "\r\n";
                subNum = parseInt(temp);
            }
            else if (nodename === "SubList") {
                str += "##################################" + "\r\n";
                if (subNum !== childnode[i].childNodes.length) {
                    str += "SubList does not match the SubNum" + "\r\n";
                }
                else {
                    for (j = 0; j < childnode[i].childNodes.length; j++)//Item  length
                    {
                        nodename = childnode[i].childNodes[j].nodeName;//Item name 
                        camname = "";
                        camcode = "";
                        for (k = 0; k < childnode[i].childNodes[j].childNodes.length; k++)//x and y
                        {
                            nodename = childnode[i].childNodes[j].childNodes[k].nodeName;
                            if (nodename === "Name") {
                                temp = childnode[i].childNodes[j].childNodes[k].childNodes[0].nodeValue;
                                str += "Name:" + temp + "\r\n";
                                camname = temp;
                            }
                            else if (nodename === "Address") {
                                temp = childnode[i].childNodes[j].childNodes[k].childNodes[0].nodeValue;
                                str += "Address:" + temp + "\r\n";
                                camcode = temp;
                            }
                            else if (nodename === "ResType") {
                                temp = childnode[i].childNodes[j].childNodes[k].childNodes[0].nodeValue;
                                str += "ResType:" + temp + "\r\n";
                            }
                            else if (nodename === "ResSubType") {
                                temp = childnode[i].childNodes[j].childNodes[k].childNodes[0].nodeValue;
                                str += "ResSubType:" + temp + "\r\n";
                            }
                            else if (nodename === "Status") {
                                temp = childnode[i].childNodes[j].childNodes[k].childNodes[0].nodeValue;
                                str += "Status:" + temp + "\r\n";
                                camstatus = temp;
                            }
                        }
                        str += "##################################" + "\r\n";
                        if (camname !== "" && camcode !== "") {
                            var camera = {};
                            camera.Id = orgid;
                            camera.Code = camcode;
                            camera.Name = camname;
                            camera.Status = camstatus;
                            Cameras.push(camera);
                        }
                    }
                }
            }
        }
    }
    else {
        alert("QueryCamList控件未注册");
    }
}

function PTZCtrlStart(cmd) {
    var speed = 4;
    if (StatViewer.ViewObject) {
        speed = parseInt(speed);
        StatViewer.ViewObject.PTZControl(cmd, 0, speed);
    }
    else {
        alert("控件未注册");
    }
}

function PTZCtrlStop(cmd) {
    var speed = 4;
    if (StatViewer.ViewObject) {
        speed = parseInt(speed);
        StatViewer.ViewObject.PTZControl(cmd, 1, speed);
    }
    else {
        alert("控件未注册");
    }
}

function GetSpeed() {

}