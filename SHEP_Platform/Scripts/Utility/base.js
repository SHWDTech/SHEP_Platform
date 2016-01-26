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