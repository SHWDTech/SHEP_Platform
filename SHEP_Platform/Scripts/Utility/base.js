var BaseInfo = {};

BaseInfo.IsMobileDevice = false;
BaseInfo.Orient = WindowOrientation.Portrait;
BaseInfo.IsIE = false;
BaseInfo.IsIE6 = false;
BaseInfo.IsIE7 = false;
BaseInfo.IsIE8 = false;

var customerModal = {};

customerModal.Show = function(option) {
    if (!$('#customerModal')) return;

    var instance = $('#customerModal');

    instance.find('.modal-header').html(option.header);

    instance.find('.modal-body').html(option.body);

    instance.modal({keyboard: true, show: true});
}

$(function() {
    $(window).on('onorientationchange', function() {
        orient();
    });

    BaseInfo.IsIE = (!!window.ActiveXObject || "ActiveXObject" in window);
    if (BaseInfo.IsIE) {
        BaseInfo.IsIE6 = navigator.appVersion.match(/6./i) === "6.";
        BaseInfo.IsIE7 = navigator.appVersion.match(/7./i) === "7.";
    }
});

var trimStr = function (str) {
    var re = /^\s+|\s+$/;
    return !str ? "" : str.replace(re, "");
}

var IsNullOrEmpty = function (obj) {
    try {
        if (obj.length === 0) {
            return true;
        }

        return false;
    }
    catch (e) {
        if (!obj) {
            return true;
        }
        if (typeof obj == "string" && trimStr(obj) === "") {
            return true;
        }

        if (typeof obj == "number" && isNaN(obj))
            return true;

        return false;
    }
}

var msg = {
    warning: function (msg) {
        $("#statusMsg").remove();
        $('body').append($('<p class="bg-info msg" id="statusMsg"></p>'));
        $('#statusMsg').html(msg);
        $('#statusMsg').fadeIn();
        setTimeout(function() {
            $('#statusMsg').fadeOut('slow');
        }, 3000);
    }
};

msg.info = function(msg) {
    $("#statusMsg").remove();
    $('body').append($('<p class="bg-success msg" id="statusMsg"></p>'));
    $('#statusMsg').html(msg);
    $('#statusMsg').fadeIn();
    setTimeout(function () {
        $('#statusMsg').fadeOut('slow');
    }, 3000);
};

var IsShow = function (targetId, targetClass) {
    if (targetId !== null) {
        return ($('#' + targetId).css('display') === 'none');
    } else {
        return ($('.' + targetClass).css('display') === 'none');
    }
};

var orient = function() {
    if (window.orientation === 0 || window.orientation === 180) {
        BaseInfo.Orient = WindowOrientation.Portrait;
    } else {
        BaseInfo.Orient = WindowOrientation.Landscape;
    }

    return BaseInfo.Orient;
}