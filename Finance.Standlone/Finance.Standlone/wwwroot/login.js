/**
 * Created by GYN on 2016/4/22.
 */

(function(){
    var urlArgs = '';
    var isProductionInstance = false;
    if(isProductionInstance){
        urlArgs = 'v=1';
    }else{
        urlArgs = 'dev=' + (new Date()).getTime();
    }

    require.config({
        baseUrl:'.',
        urlArgs: urlArgs,
        paths:{
            jquery: './vendor/jquery/jquery-2.1.4.min',
            'kendo.all.min': './vendor/kendo/kendo.all.min',
            localSettings: './scripts/localSettings'
        },
        shim:{
            login:['jquery']
        },
        deps:['login'],
        waitSeconds:40
    });
})();

define('login', ['jquery', 'kendo.all.min', 'localSettings'], function ($, kendo, localSettings) {
    window.localSettings = localSettings;
    $("input").keyup(function(e){
        if(e.which === 13){
            $("#submitbutton").click();
        }
    });
    $("#submitbutton").click(function (event) {
        var postData = $("#lgf").serializeArray();
        var formUrl = $("#lgf").attr("action");
        var pd = {};
        pd["Username"] = postData[0].value;
        pd["Password"] = postData[1].value;
        $.ajax({
            url: formUrl,
            type: "POST",
            contentType:"application/json; charset=utf-8",
            dataType: "type",
            data: JSON.stringify(pd),
            beforeSend: function(e){
                kendo.ui.progress($("body"),true);
            },
            complete: function(e){
                kendo.ui.progress($("body"),false);
            },
            success: function(data, textStatus, jqXHR){
                if((typeof data) === "string"){
                    data = JSON.parse(JSON.parse(data));
                }
                var firstDay = new Date();
                var nextWeek = new Date(firstDay.getTime() + 7 * 24 * 60 * 60 * 1000);
                localSettings.save(pd["Username"], data.access_token, data.token_type, data.expires_in);
                window.location.replace(data.url);
            },
            error: function(jqXHR, textStatus, errThrown){
                var message = JSON.parse(jqXHR.responseText);
                alert(message);
                $("#password").val("");
            }
        });
    });
});

