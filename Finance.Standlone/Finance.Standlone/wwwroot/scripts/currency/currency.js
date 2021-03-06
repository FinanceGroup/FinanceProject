﻿define(['jquery', 'kendo', 'currencyGrid','events'], function
    ($, kendo, currencyGrid, events) {
    function init(options) {
        events.publish('app.change_main_view_title', 'Currency Maintance');
        options.pathChanged && kendo.ui.progress($(document.body), false);
        currencyGrid.init(options);

        $("#currency_wrapper").kendoSplitter({
            orientation: 'vertical',
            panes: [
                { size: "50%", max: "90%" },
                { size: "50%", max: "90%" }
            ],
        });
    }
    function unactive() {
        currencyGrid.unactive();
        //ginniePayupGrid.unactive();
    }
    return { init: init, unactive: unactive };
});