define(['jquery', 'singalr.hubs'], function ($) {
    var conn = $.connection,
	hub = conn.hub;
    registerHub(conn.PayUpGrid);
    registerHub(conn.GinniePayupGrid);
    function registerHub(hun) {
        hub.client.foo = function () { };
    }

    function getHub(name) {
        return conn[name];
    }

    function start(cb) {
        return hub.start().done(cb);
    }
    return { getHub: getHub, start: start };
})
