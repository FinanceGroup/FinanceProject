/**
 * Created by GYN on 2016/5/14.
 */

(function (){
    requirejs.config({
        baseUrl: '/scripts',
        urlArgs: 'r=' ,//+ (+new Date),
        waitSeconds: 0,
        map:{
            '*':{
                'kendo': 'kendo.all.min',

                'signalr.core': '../verdor/signalR/jquery.signalR-2.2.0.min',
                'signalr.hubs': '/signalr/hubs?'

                //'hierarchy': 'hierarchy/hierarchy',
                //'hierarchyGrid':'hierarchy/hierarchyGrid',

                //'oddlots':'oddlots/oddlots',
                //'oddlotsGrid':'oddlots/oddlotsGrid',

                //'top10':'top10/top10',
                //'top10Grid':'top10/top10Grid',

                //'SSFA':'SSFA/SSFA',
                //'SSFAGrid':'SSFA/SSFAGrid',

                //'summary':'summary/summary',
                //'trend':'summary/trend',
                //'basel':'summary/basel',

                //'payup':'payup/payup',
                //'payupGrid':'payup/payupGrid',
                //'ginniePayupGrid':'payup/payupTemplate'
            }
        },
        paths:{
            text:'//cdnjs.cloudflare.com/ajax/libs/require-text/2.0.12/text.min',
            jquery:'../vendor/jquery/jquery-2.1.4.min',
            jszip:'../vendor/jszip/jszip.min',
            'kendo.all.min':'../vendor/kendo/kendo.all.min'
        },
        shim:{
            kendo:{
                deps:['jquery','jszip']
            },
            util:{
                deps:['jquery']
            },
            'signalr.core': {
                deps: ['jquery'],
                exports: '$.connection'
            },
            'signalr.hubs': {
                deps: ['signalr.core']
            }
        }
    });
}());

define('app',['jquery','jszip','kendo','localSettings','events','util'],
    function($, jszip, kendo, localSettings, events){
        var modules = [];
        window.JSZip = jszip;
        window.localStorage = localSettings;
        localSettings.relogin();
        $('#main_wrapper').fadeIn();
        var views = [
            //{text: 'Odd Lot Summary', module:'summary', role:'oddlots', linksTos:'#oddlotSummary'},
            //{text: 'OddLots', module:'oddlots', role:'oddlots', linksTo:'#oddlots'},
            //{text: 'Non-Agency SSFA Top 10', module:'top10', role:'top10', linksTo:'#top10'},
            //{text: 'Non-Agency SSFA Detail', module:'SSFA', role:'top10', linksTo:'SSFA'},
            //{text: 'Hierarchy', module:'hierarchy', role:'hierarchy', linksTo:'hierarchy'},
            //{text: 'Payup Grid', module:'payup', role:'payupgrid', linksTo:'#payup'}
        ];
        var routers = [
            //{path: 'oddlotSummary', moduleName: 'summary', viewPath: '/views/summary/summary.html'},
            //{path: 'oddlots', moduleName: 'oddlots', viewPath: '/views/oddlots/oddlots.html'},
            //{path: 'hierarchy', module: 'hierarchy', viewPath: '/views/hierarchy/hierarchy.html'},
            //{path: 'top10', module: 'top10', viewPath: '/views/top10/top10.html'},
            //{path: 'SSFA', moduleName: 'SSFA', viewPath: '/views/SSFA/SSFA.html'},
            //{path: 'payup', moduleName: 'payup', viewPath: '/views/payup/payup.html'}
        ];
        var router = new kendo.Router({
            routeMissing: function(){ router.navigate('');}
        });
        function add_routers(){
            if(modules.length === 0){
                router.route('/', function(){
                    events.publish('app.change_main_view_title', '(Blank)');
                });
                return;
            }
            routers.filter(function (r){
                return modules.includes(r.moduleName);
            }).map(function (r, index){
                index === 0 && router.add_router('/', r.moduleName, r.viewPath);
            });
        }
        router.add_router = function (path, moduleName, viewPath){
            router.route(path, function(){
                var tree = $('#page_nav').data('kendoTreeView');
                var node = tree.dataSource.get(moduleName);
                node && tree.select(tree.findByUid(node.uid));
                var pathChanged = router.pathChanged = (router.oldPath !== path);
                router.oldPath = path;
                kendo.ui.progress($(document.body), true);
                require(['text!' + viewPath], function(html){
                    require([moduleName], function(html){
                        router.oldModule && router.oldModule.unactive && router.oldModule.unactive();
                        $('#main_view').html(html);
                        try{
                            module.init(pathChanged);
                        }catch (e){
                            window.console && console.error('error occurs when init module :', moduleName, e);
                        }
                        router.oldModule = module;
                    });
                });
            });
        };

        function init_view(){
            var windowSplitterRate = 0.15;
            var formerWindowWidth = window.innerWidth;
            $('#main_toolbar').kendoToolBar({
                items: [
                    { template: '<h1> Regulatory Reporting / <span id="main_view_title"></span></h1>'},
                    { type: 'button', text: 'Logout', attributes: {style:
                    'float:right; margin-top:6px;'},
                     click: function (e){
                         localSettings.fire();
                         window.location.replace(window.location.origin);
                     }
                    },
                    { type: 'separator', attributes:{ style: 'float:right; margin-top:8px'}},
                    { template: '<span><label>User:</label><label class="b">' +
                    JSON.parse(localStorage.getItem('OddLotsToken')).user + '</label></span>',
                        attributes: { style: 'float:right; margin-top:6px; margin-right:5px'}
                    },
                ]
            });
            var mainSplitter = $('#main_splitter').kendoSplitter({
                panes: [{size: windowSplitterRate * 100 + '%', min: '10%', max: '50%'}],
                resize: function(e){
                    if (window.innerWidth === formerWindowWidth){
                        windowSplitterRate = $(this._panes()[0]).width() / formerWindowWidth;
                        windowSplitterRate = windowSplitterRate > 0.5 ? 0.5 : windowSplitterRate;
                    }
                    else{
                        formerWindowWidth = window.innerWidth;
                        this.size(this._panes()[0], windowSplitterRate * 100 + "%");
                    }
                }
            });
            $('#page_nav').kendoTreeView({
                dataUrlField: 'linksTo',
                dataSource: {
                    data: views.filter(function(e){
                        return e.module && modules.includes(e.module);
                    }).map(function (view){
                        view.items && (view.items = view.items.filter(function(e){
                            return arguments[0].module && !modules.includes(arguments[0].module);
                        }));
                        return view;
                    }),
                    schema: { model:{id:'module'}}
                }
            });
            $('#popupNotification').kendoNotification({
                position:{
                    top: 2,
                    left: null,
                    bottom: null,
                    right: 30
                }
            });
        };

        function add_event_handlers(){
            events.subscribe('app.change_main_view_title', function(main_view_title){
                $('#main_view_title').text(main_view_title);
            });
        }
        (function(){
            $.ajax({
                url:"/api/Identity",
                dataType:'json',
                type:'POST',
                contentType:'application/json',
                beforeSend: function(xhr){
                    xhr.setRequestHeader('Authorization', "Bearer" + JSON.parse(localStorage.getItem("OddLotsToken")).token);
                },
                success: function(response){
                    var roleMap = JSON.parse(response);
                    modules = views.filter(function (view){
                        return roleMap[view.role];
                    }).map(function (view){
                        return view.module;
                    });
                }
            }).always(function(){
                init_view();
                add_routers();
                add_event_handlers();
                router.start();
                kendo.ui.progress($(document.body), false);
            });
        }());
        window.showDialog = function(message, title, type){
            var $dialog = $('<div class="mian_dialog"></div>');
            var dialogTemplate = '<div class="dialog_message"></div>' + '<br/><hr/>';
            if(!type || type === 'alert'){
                dialogTemplate += '<div class="dialog_btns">' +
                            '  <input type="button" class="dialog_ok k-button" value="ok" sytle="width:60px; margin-left:110px; margin-right:110px;"/>'
                            + ' </div>';
            } else if (type === 'confirm'){
                dialogTemplate += '<div class="btns">' +
                '  <input type="button" class="dialog_ok k-button" value="ok" style="width:60px; margin-left:60px; margin-right:40px;"/>' +
                '  <input type="button" class="dialog_cancel k-button" value="Cancel" style="width:60px; margin-right:60px;"/>' +
                '</div>';
            }
            var deferred = $.Deferred();
            var result = false;
            var dialog = $dialog.appendTo('body').kendoWindow({
                title: title || '',
                modal: true,
                visiable: false,
                width: '300px',
                close: function (){
                    this.destroy();
                    deferred.resolve(result);
                    $dialog.remove();
                }
            }).data('kendoWindow').content(dialogTemplate).center().open();

            $('.main_dialog.dialog_message').append($('<p></p>').html(message));
            $('.main_dialog.dialog_ok').click(function(){
                result = true;
                dialog.close();
            });
            $('.main_dialog .dialog_cancel').click(function(){
                dialog.close();
            });
            return deferred.promise();
        }
    });
