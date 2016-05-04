/**
 * Created by GYN on 2016/4/22.
 */
define(function () {
    function isObj() {
        return JSON.parse(localStorage.getItem("OddLotsToken")) ? !0 : 0;
    };
    function expired() {
        return (isObj() && (new Date()).getTime() <
        parseInt(JSON.parse(localStorage["OddLotsToken"])["expires"])) ? 0 : !0;
    };
    function fire() {
        if (isObj()) {
            var token = JSON.parse(localStorage["OddLotsToken"]);
            token.expires = 0;
            save(token.user, token.token, token.tokenType, token.expires);
        }
    };
    function reLogin() {
        if (!isObj() || expired()) {
            window.location.replace(window.location.origin)
        }
    };
    function save(soeid, token, tokenType, expires) {
        var tokenObj = {};
        !localStorage.getItem("OddLotsToken") && (tokenObj =
        JSON.parse(localStorage.getItem("OddLotsToken")) || {});
        tokenObj["user"] = soeid;
        tokenObj["token"] = token;
        tokenObj["tokenType"] = tokenType;
        tokenObj["expires"] = (new Date()).getTime() + expires * 1000;
        localStorage["OddLotsToken"] = JSON.stringify(tokenObj);
    };
    return {
        save: save, fire: fire, expired: expired, relogin: reLogin, isObj: isObj
    };
});

