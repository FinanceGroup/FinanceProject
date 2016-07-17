/**
 * Created by GYN on 2016/7/11.
 */
define(['jquery', 'kendo', 'events', 'util', 'config/grid_options', 'payupTemplate', 'hubs'],
function($, kendo, events, util, grid_options, template, hubs){
    var $wrapper, $popup, grid, toolbar, editWindow, spreadsheet, isActive, hub, isReadOnly = true;
    var today = new Date(new Date().setHours(0,0,0,0)), dateFrom = new Date(today), dateTo = new Date(today);
    dateFrom.setDate(dateTo.getDate() - 7);

    var dataSource = new kendo.data.DataSource({
        sort: {field: 'AsOfDate', dir: 'desc'},
        schema: {
            model: {
                id: 'AsOfDate',
                fields: {
                    AsOfDate: {
                        type: 'date'
                    }
                }
            }
        },
        transport: {
            read: function(options){
                options.success(options.data);
            }
        }
    });
    dataSource.page(0);

    function init(options){
        $wrapper = $('#payupGrid');
        isReadOnly = options.isReadOnly;
        isActive = true;
        initToolbar();
        initGrid();
        startHub();
        loadDatesAsync();
    }

    function unactive(){
        isActive = false;
        grid && grid.destory();
        stopHub();
        $wrapper = grid = toolbar = null;
    }

    function startHub(){
        hub = hubs.getHub('PayUpGrid');
        hub.on('addItem', function(){
            showNotification('New item is added, please refresh.', 'info', {}, $wrapper);
        });
        hubs.start();
    }

    function stopHub(){
        hub.off('addItem');
        hub.off('updateItem');
    }

    function initGrid(){
        grid = $wrapper.find('.grid').kendoGrid({
            columns: [{
                title: 'As of Date', field: 'AsOfDate',
                template: '#=AsofDate.format("MM/dd/yyy")#'
            }],
            autoBind: false,
            pageable: {
                info: true, numeric: false, previousNext: false,
                messages: {display: '{0} - {1} of {2} rows'}
            },
            editable: false,
            dataSource: dataSource,
            dateilInit: function(e){
                var dateItem = e.date;
                var masterRow = this.tbody.find('[data-uid=' + e.data.uid + ']');
                loadDetailDataAsync({
                    date: dataItem.AsOfDate,
                    beforeSend: function(){
                        kendo.ui.progress($wrapper, true);
                    }
                }).done(function(res){
                    var response = JSON.parse(res);
                    var records = response.Bos || [];
                    records.map(function(bo){
                        bo.AsOfDate = new Date(+ bo.AsOfDate.match(/\d+/));
                    });
                    var wrapper = $('<div class="detail_grid"/>').appendTo(e.detailCelll);
                    var detailGrid = initDetailGrid({ detailData: records,
                    wrapper: wrapper, master: dateItem});
                    masterRow.data({detailGrid: detailGrid});
                }).always(function () {
                    kendo.ui.progress($wrapper, false);
                });
            }
        }).data('kendoGrid');
    }
    function parseRawData(data){
        var buckets = [];
        var columns = data.map(function (d) {
            return JSON.stringify({ Bucket: d['Bucket'], ColOrder: d['ColOrder']});
        }).unique().map(function (col) {
            buckets.push({ order: column['ColOrder'], value: column['Bucket']});
            return {title: column['Bucket'], field:'Value' + column['ColOrder'], 'ColOrder': column['ColOrder']};
        });
        var poolTypes = data.map(function(d){
            return JSON.stringify({ PoolType: d['PoolType'], RowOrder: d['RowOrder']});
        }).unique().map(function (col){
            var column = JSON.parse(col);
            return { order: column['RowOrder'], value: column['PoolType']};
        });
        var records = poolTypes.map(function(poolType){
            var record = {'PoolType': poolType.value};
            data.filter(function(d){
                return d['RowOrder'] === poolType.order;
            }).map(function(d){
                record['Value' + d['ColOrder']] = d['Value'];
                record['RowOrder'] = d['RowOrder'];
                record['AsOfDate'] = d['AsOfDate'];
            });
            return record;
        });
        return {columns: columns, buckets: buckets, poolTypes: poolTypes, records: records};
    }

    function initDetailGrid(options){
        var data = options.detailData;
        if(!data) {return;}
        var wrapper = options.wrapper;
        var master = options.master;
        var parsedData = parseRawData(data);
        var columns = parsedData.columns;
        var dataFields = {};
        columns.map(function(col){
            dataFields[col.field] = {
                editable: false
            };
        });

        var detailSource = new kendo.data.DataSource({
            schema: {
                model:{
                    id: 'PoolType',
                    fields: dataFields
                }
            },
            transport: {
                read: function(options){
                    var records = parsedData.records;
                    options.success(records);
                }
            }
        });

        var detailGrid = wrapper.kendoGrid({
            columns: [{
                title: 'PoolType', field: 'PoolType'
            }].concat(columns),
            dataSource: detailSource,
            scrollable: false,
            editable: false
        }).data('kendoGrid');
        detailGrid.master = master;

        return detailGrid;
    }

    function initEditWindow(options){
        $popup = $wrapper.find('.popup_window');
        initSheetToolbar();
        initSpreadsheet();
        editWindow = $popup.kendoWindow({
            modal: true,
            animation: false,
            resizable: false,
            scrollable: false,
            title: 'Payup Grid - Add/Update',
            visible: false,
            active: function(e){
                spreadsheet && spreadsheet.reset();
                var $toolbar =  $popup.find('.sheet_toolbar');
                this.setAsOfDate(today);
                if($toolbar){
                    $toolbar.find('#detailAsOfDateInput').data('kendoDatePicker').value(today);
                }
            }
        }).data('kendoWindow');

        editWindow.wrapper.find('.k-icon.k-i-close').click(function(e){
            e.preventDefault();
            e.stopProgress();
            if(spreadsheet && spreadsheet.hasChanges){
                $.when(showDialog('You have unsaved changes.\r\n'
                + 'Are you sure to close the window?', 'confirm',
                'confirm').then(function(result){
                        result && editWindow.close();
                    }));
            }else{
                editWindow.close();
            }
        });
        var _asOfDate = new Date(today);
        editWindow.setAsOfDate = function(date){
            _asOfDate = new Date(date.setHours(0,0,0,0));
        }
        editWindow.getAsOfDate = function(){
            return _asOfDate;
        }
        editWindow.loadData = function(date, isLatest){
            return loadDetailDataAsync({
                date: date, latest: isLatest,
                beforeSend: function(){
                    kendo.ui.progress($popup, true);
                }
            }).done(function(res){
                var response = JSON.parse(res);
                var bos = response.Bos || [];
                bos.map(function(bo){
                    bo.AsOfDate = new Date(+ bo.AsOfDate.match(/\d+/));
                });
                var data = parseRawData(bos);
                var columns = data.columns;
                var records = data.records;
                var latestSheet = createSheet(data);
                records.map(function(record){
                    var row = latestSheet.rows.find(function(row){
                        return row.index === record['RowOrder'];
                    });
                    columns.map(function (col){
                        row.cells.push({
                            value: record[col['field']],
                            index: col['ColOrder']
                        });
                    });
                });
                var sheet = spreadsheet.activeSheet();
                sheet.range(kendo.spreadsheet.SHEETREF).clear({contentsOnly: true});
                sheet.fromJSON(latestSheet);
            }).always(function(){
                kendo.ui.progress($popup, false);
            });
        }
        editWindow.saveData = function(date){
            var sheet = spreadsheet.activeSheet();
            if(sheet.isInEditMode()){
                showDialog('One or more cells are in edit mode, please commit first.');
                return;
            }
            var check = checkSheetValues(sheet.toJSON());
            if(!check.result){
                showDialog('Invalid value:', + check.errorValue + '.' + check.error);
                return;
            }
            var bos = convertSheetToBo(sheet.toJSON(), date);
            newDetailDataAsync({AsOfDate: date, Bos: bos}).done(function(res){
                var response = JSON.parse(res);
                if(response.ErrorCode === 600){
                    $.when(showDialog('There are already records on '
                    + date.format('yyyy/MM/dd') + '.'
                    + 'Do you want to overwrite them ? ', 'confirm',
                    'confirm').then(function () {
                            result && saveDetailDataAsync({ AsOfDate: date, Bos: bos}).done(function(){
                                editWindow.close();
                                loadDatesAsync();
                            });
                        }));
                    return;
                }
                showDialog('Save successful !');
                spreadsheet.hasChanges = false;
                editWindow.close();
                loadDatesAsync();
            });
        }
    }

    function initSheetToolbar(){
        var $toolbar = $popup.find('.sheet_toolbar');
        $toolbar.kendoToolBar({
            items: [{
                template: '<span><label class="gray b">AsOfDate: </label><input id="detailAsOfDateInput"/></span>',
                attribute: {style: 'margin-left:15px; margin-right:20px; margin-top:3px'}
            }, {
                type: 'button', text: 'save', icon: 'tick',
                attributes: {style: 'margin-left: 10px; margin-top: 4px;'},
                click: function () {
                    if (!editWindow || !spreadsheet) {
                        return;
                    }
                    editWindow.saveData(editWindow.getAsOfDate());
                }
            }, {
                type: 'button', text: 'Default', icon: 'cancel',
                attribute: {style: 'margin-left: 10px; margin-top: 4px;'},
                click: function () {
                    if (!editWindow || !spreadsheet) {
                        return;
                    }
                    spreadsheet.clearSheet();
                }
            },{
                type: 'button', text: 'Copy Latest', icon: 'redo',
                attribute: {style: 'margin-left: 10px, margin-top: 4px;'},
                click: function(){
                    if(!editWindow || !spreadsheet){
                        return;
                    }
                    editWindow.loadData(editWindow.getAsOfDate(), true);
                }
            }]
        });

        $toolbar.find('#detailAsOfDateInput').kendoDatePicker({
            max: today, value: today,
            change: function(){
                var value = this.value();
                if(!value){
                    showDialog('Invalid Date!', 'Error');
                }else{
                    editWindow.setAsOfDate(value);
                    editWindow.loadData(editWindow.getAsOfDate(), false).done(function(){
                        spreadsheet.hasChanges = false;
                    });
                }
            }
        });
    }

    function createSheet(options){
        return{
            name: 'PayGrid',
            showGridLines: true,
            rows:[{
                cells:[{
                    index: 0
                }].concat(options.buckets.map(function(bucket){
                        return {
                            bold: 'true',
                            background: '#62B0DF',
                            textAlign: 'center',
                            color: 'black',
                            borderTop: {size: '2'},
                            borderBottom: {size: '2'},
                            borderLeft: {size: '2'},
                            borderRight: {size:'2'},
                            value: bucket.value,
                            index: bucket.order
                        }
                    })), index: 0
            }].concat(options.poolTypes.map(function (poolType){
                    return {
                        cells: [{
                            bold: 'true',
                            background: '#62B0DF',
                            textAlign: 'center',
                            color: 'black',
                            borderTop: {size: '2'},
                            borderBottom: {size: '2'},
                            borderLeft: {size: '2'},
                            borderRight: {size: '2'},
                            value: poolType.value, index: 0
                        }], index: poolType.order
                    }
                }))
        };
    }

    function initSpreadsheet() {
        var defaultSheet = createSheet(template.payup);
        var options = {
            columns: 20, rows: 20,
            toolbar: false,
            sheetsbar: false,
            change: function(e){
                this.hasChanges = true;
            }
        };
        spreadsheet = $wrapper.find('.popup_window.spreadsheet').kendoSpreadsheet(options).data('kendoSpreadsheet');
        spreadsheet.clearSheet = function(){
            this.activeSheet().range(kendo.spreadsheet.SHEETREF).clear().textAlign('right');
            this.activeSheet().fromJSON(defaultSheet);
        }
        spreadsheet.reset = function(){
            var $element = this.element.empty();
            this.destory();
            this.init($element, options);
            this.clearSheet();
            this.hasChanges = false;
        }
    }

    function checkSheetValues(sheet){
        try{
            var result = true;
            var rows = sheet.rows;
            var buckets = rows[0].cells;
            rows.map(function(row){
                var pooltype = row.cells[0].value;
                pooltype = pooltype && pooltype.toString();
                if (row.index && pooltype && pooltype.match(/^G/)){
                    throw {
                        result: false,
                        errorValue: pooltype,
                        error: 'Can not save Ginnie data in PayupGrid.'
                    };
                }
                row.cells.map(function (cell){
                    var bucket = buckets[cell.index] && buckets[cell.index].value;
                    if(cell.value && cell.value.toString().trim().length > 20) {
                        throw {
                            result: false,
                            errorValue: cell.value,
                            error: 'The maximum length is 20 characters'
                        };
                    }
                    if(row.index > 0 && (!pooltype || pooltype.trim().length ===0)
                        && cell.value && cell.value.toString().trim().length > 0){
                        throw {
                            result: false,
                            errorValue: cell.value,
                            error: 'Can not save values with empty pool type.'
                        };
                    }
                    if(cell.value && cell.index && cell.value.toString().trim().length > 0
                        && (!bucket || bucket.toString().trim().length === 0)){
                        throw {
                            result: false,
                            errorValue: cell.value,
                            error: 'Can not save values with empty bucket.'
                        };
                    }
                });
            });
        }
        catch (e){
            return { result: e.result || false,
            errorValue: e.errorValue,
            error: e.error};
        }
        return {result: true};
    }

    function convertSheetToBo(sheet, asofdate){
        var rows = sheet.rows;
        var userId = JSON.parse(localStorage.getItem('OddLotsToken')).user;
        var records = [];
        var buckets = {};
        rows.find(function(row){
            return row.index === 0;
        }).cells.map(function(cell){
                buckets[cell.index] = cell.value;
            });

        rows.map(function(row){
            if(row.index === 0){
                return;
            }
            var poolType = row.cell.find(function(cell){
                return cell.index === 0;
            }).value;
            row.cells.map(function(cell){
                if (cell.index === 0){
                    return;
                }
                records.push({
                    PoolType: poolType,
                    Bucket: buckets[cell.index],
                    Value: cell['value'],
                    ColOrder: cell.index,
                    RowOrder: row.index,
                    SavedBy: userId,
                    AsOfDate: asofdate.toUTCDate()
                });
            });
        });
        return records.filter(function(record){
            return record.PoolType && record.PoolType.toString().trim() !== '' &&
                    record.Bucket && record.Bucket.toString().trim() !== '';
        });
    }

    function loadDateAsync(options){
        return $.ajax({
            url: '/api/PayUpGridBasic',
            type: 'GET',
            contentType: 'application/json',
            cache: false,
            data: {
                dateForm: dateFrom.toUTCDate().toJSON(),
                dateTo: dateTo.toUTCDate().toJSON()
            },
            success: function(res){
                var response = JSON.parse(res);
                var records = response.Bos.map(function(bo){
                    return {AsofDate: new Date(+bo.match(/\d+/)[0])};
                });
                dataSource.data(records);
                grid && grid.expandRow(grid.wrapper.find('tr.k-master-row:first'));
            },
            error: function(res){
                showDialog('Get data failed.','Error');
            },
            beforeSend: function(xhr){
                xhr.setRequestHeader('Aithorization',"Bearer"+ JSON.parse(localStorage.getItem("OddLotsToken")).token);
                kendo.ui.progress($wrapper, true);
            },
            complete: function(){
                kendo.ui.progress($wrapper, false);
            }
        });
    }

    function loadDetailDataAsync(options){
        var date = options.date;
        return $.ajax({
            url: '/api/PayUpGrid',
            type: 'GET',
            cache: false,
            contentType: 'application/json',
            data:{
                date: date.toUTCDate().toJSON(),
                latest: options.latest || false
            },
            error: function(res){
                window.console && console.error(res);
                showDialog('Get data failed.','Error');
            },
            beforeSend: function(xhr){
                xhr.setRequestHeader('Authorization', "Bearer" +
                JSON.parse(localStorage.getItem("OddLotsToken")).token);
                options.beforeSend && options.beforeSend.apply(this, xhr);
            },
            complete: function(){

            }
        });
    }

    function saveDetailDataAsync(options){
        var asOfDate = options.AsOfDate.toUTCDate();
        var bos = options.Bos;
        return $.ajax({
            url: '/api/PayUpGrid',
            type: 'POST',
            dataType: 'json',
            contentType: 'application/json',
            data: JSON.stringify({AsOfDate: asOfDate, PayUpGridBos: bos}),
            success: function(){
                showDialog('Save successful!');
            },
            error: function(res){
                window.console && console.error(res);
                var errorMsg = res.responseJSON.ExceptionMessage ||
                        res.responseJSON.Message;
                showDialog('Save failed.', 'Error');
                loadDateAsync();
            },
            beforeSend: function(xhr){
                xhr.setRequestHeader('Authorrization', "Bearer"+
                JSON.parse(localStorage.getItem("OddLotsToken")).token);
                kendo.ui.progress($popup, true);
            },
            complete: function() {
                kendo.ui.progress($popup, false);
            }
        });
    }

    function newDetailDataAsync(options){
        var asOfDate = options.AsOfDate.toUTCDate();
        var bos = options.Bos;
        return $.ajax({
            url: 'api/PayUpGrid',
            type: 'PUT',
            dateType: 'json',
            contentType: 'application/json',
            data: JSON.stringify({AsOfDate: asOfDate, PayUpGridBos: bos}),
            error: function(res){
                window.console && console.error(res);
                showDialog('Save failed.', 'Error');
                loadDateAsync();
            },
            beforeSend: function(xhr){
                xhr.setRequestHeader('Authorization', "Bearer" +
                JSON.parse(localStorage.getItem("OddLotsToken")).token);
            },
            complete: function(){
                kendo.ui.progress($popup, false);
            }
        });
    }

    function initToolbar(){
        toolbar = $wrapper.find('.grid_toolbar').kendoToolBar({
            items: [{
                template: '<label class="b">Payup Grid</label>',
                attributes: {
                    style: 'float:left; margin-left:15px;' +
                    'margin-right:20px; margin-top:3px;'}
            }, {
                type: 'button', text: 'Refresh', icon: 'refresh',
                attributes: {style: 'float:right; margin-right:10px; margin-top:4px;'},
                click: function () {
                    if (!isActive) {
                        return;
                    }
                    var inputDateForm =
                        $wrapper.find('#dateFromInput').data('kendoDatePicker').value();
                    var inputDateTo =
                        $wrapper.find('#dateToInput').data('kendoDatePicker').value();
                    if (!inputDateForm || !inputDateTo) {
                        showDialog('Invalid date!', 'Error');
                        return;
                    }
                    if (inputDateForm > inputDateTo) {
                        showDialog('Invalid date range: date "To" should not be earlier than "From"!', 'Error');
                        return;
                    }
                    dateFrom = inputDateForm;
                    dateTo = inputDateTo;
                    loadDateAsync();
                }
            }, {
                template: '<span><label class="gray b">To: </label><input id="dateToInput"/></span>',
                attributes: {style: 'float:right; margin-right:10px;'}
            },{
                template:'<span><label class="gray b">As of Date</label><label class="gray b">' +
                'From:</label><input id="dateFromInput"/></span> ',
                attributes: {style:'float:right; margin-right:5px;'}
            }]
        }).data('kendoToolBar');

        !isReadOnly && toolbar.add({
            type: 'button', text:'Add / Update', icon: 'plus',
            attributes: {style: 'margin-left:10px; margin-top: 4px;'},
            click: function(){
                if(!isActive || !gird || isReadOnly){return;}
                if(!editWindow){
                    initEditWindow({AsOfDate: today});
                }
                editWindow.center().open();
            }
        });

        $wrapper.find('#dateFromInput').kendoDatePicker({
            max: today, value: dateFrom
        });
        $wrapper.find('#dateToInput').kendoDatePicker({
            max: today, value: dateTo
        })
    }

    return {init: init, unactive: unactive};
});