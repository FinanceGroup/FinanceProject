/**
 * Created by GYN on 2016/4/27.
 */
Date.prototype.format = function (fmt){
    var o = {
        "M+": this.getMonth() + 1,
        "d+": this.getDate(),
        "h+": this.getHours(),
        "m+": this.getMinutes(),
        "s+": this.getSeconds(),
        "q+": Math.floor((this.getMonth() + 3) / 3),
        "s": this.getMilliseconds()
    };
    if(/(y+)/.test(fmt)) fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for(var k in o){
        if(new RegExp("(" + k + ")").test(fmt)){
            fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]):
                (("00" + o[k]).substr(("" + o[k]).length)));
        }
    }
    return fmt;
};

Date.prototype.toUTCDate = function(){
    var offset = this.getTimezoneOffset() / 60;
    return new Date(new Date(this).setHours(-offset,0,0,0));
}

String.prototype.toDate = function(){
    var nums = this.match(/\d+/g);
    return new Date(nums[0], nums[1] - 1, nums[2], nums[3], nums[4], nums[5]);
};

if(typeof Array.prototype.remove !== 'function'){
    Array.prototype.remove = function(value){
        this.includes(value) && this.splice(this.indexOf(value),1);
    };
}

if(typeof Array.prototype.include !== 'function'){
    Array.prototype.includes = function(value){
        return this.indexOf(value) + 1;
    };
}

if(typeof Array.prototype.unique !== 'function'){
    Array.prototype.unique = function(){
        var n = {}, r = [];
        for(var i = 0; i < this.length; i++){
            if(!n[this[i]]){
                n[this[i]] = true;
                r.push(this[i]);
            }
        }
        return r;
    }
}

if(typeof Array.prototype.fill !== 'function'){
    Array.prototype.fill = function(value){
        if(this == null){
            throw new TypeError('this is null or not defined');
        }
        var O = Object(this);
        var len = O.length >>> 0;

        var start = arguments[1];
        var relativeStart = start >> 0;

        var k = relativeStart < 0 ?
            Math.max(len + relativeStart, 0):
            Math.min(relativeStart, len);

        var end = arguments[2];
        var relativeEnd = end === undefined ?
            len : end >> 0;

        var final = relativeEnd < 0 ?
            Math.max(len + relativeEnd , 0) : Math.min(relativeEnd, len);

        while (k < final){
            o(k) = value;
            k++;
        }
        return O;
    };
}

if(typeof Array.prototype.find !== 'function'){
    Array.prototype.find = function(predicate){
        if(this === null){
            throw new TypeError('Array.prototype.find called on null or undefined');
        }
        if(typeof predicate !== 'function'){
            throw new TypeError('predicate must be a function');
        }
        var list = Object(this);
        var length = list.length >>> 0;
        var thisArg = arguments[1];
        var value;

        for(var i = 0; i < length; i++){
            value = list[i];
            if(predicate.call(thisArg, value, i, list)){
                return value;
            }
        }
        return undefined;
    };
}

$.fn.getElements = function (arr){
    var self = this;
    if(Object.prototype.toString.call(arr) === "[object Array]"){
        var returnArr = [];
        $.each(arr, function (index, e){
            if (isNaN(e)){
                return [];
            }
            returnArr.push(self[e]);
        });
        return returnArr;
    }
};

if (typeof String.prototype.endsWith !== 'function'){
    String.prototype.endsWith = function (suffix){
        return this.indexOf(suffix, this.length - suffix.length) !== -1;
    };
}

define({
    /**
     * format record time based on current date
     * today records show only time
     * past records show date + time
     */
    formatExecutionDateTime: function(executionDateTime){
        var dateTime = executionDateTime.toDate();
        var date = dateTime.format('MM/dd');
        var time = dateTime.format('hh:mm');
        var currentDate = new Date().format('MM/dd');
        if(date === currentDate){
            return time;
        } else {
            return date + '' + time;
        }
    },

    downloadFile: function (url){
        //window.open(url);
        var $iframe = $('iframe.download-file');
        if ($iframe.length < 1){
            $iframe = $('<iframe />').attr({
                    src: url, class: 'download-file',
                    style: 'display:none'
                });
            $('body').append($iframe);
        } else {
            $iframe.attr('src', url);
        }
    }
});





















