//Ajax请求地址
var ajaxUrl = '/Ajax/Access';

var StatViewer = {};

StatViewer.Control = function(dir) {
    var param = {
        'fun': 'cameraMoveControl',
        'dir': dir
    }

    $.post(ajaxUrl, param, function (ret) {
        if (!ret.success) {
            msg.warning('操作云台发生错误！');
        }
    });
};

StatViewer.Stop = function() {
    var param = {
        'fun': 'cameraMoveStop'
    }

    $.post(ajaxUrl, param);
}

StatViewer.CapturePicture = function() {
    var param = {
        'fun': 'capturePicture'
    }

    $.post(ajaxUrl, param, function(ret) {
        if (!ret.success) {
            msg.warning('拍摄照片失败！');
        } else {
            msg.info('拍摄照片成功！');
        }
    });
}

function Up() {
    StatViewer.Control('Up');
}

function Down() {
    StatViewer.Control('Down');
}

function Left() {
    StatViewer.Control('Left');
}

function Right() {
    StatViewer.Control('Right');
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

function Stop() {
    StatViewer.Stop();
}

function CapturePicture() {
    StatViewer.CapturePicture();
}