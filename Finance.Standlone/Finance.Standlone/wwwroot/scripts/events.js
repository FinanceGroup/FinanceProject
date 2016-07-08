/**
 * Created by GYN on 2016/5/11.
 */
define(function(){
    var bus = {};
    function subscribe(eventId, callback){
        if(!bus.hasOwnProperty(eventId)){
            bus[eventId] = [];
        }
        if(typeof (callback) == 'function'){
            bus[eventId].push(callback);
        }
    }
    function publish(eventId, args){
        if(!bus.hasOwnProperty(eventId)){
            return;
        }
        bus[eventId].map(function (callback){
            try{
                callback.call(bus,args);
            }catch (e){
                window.console && console.error('error occurs when publish envent:', eventId, e);
            }
        });
    }
    return {subscribe: subscribe, publish: publish}
});