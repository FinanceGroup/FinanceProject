define(function () {
    return {
        columnResizeHandleWidth: 6,
        resizable: true,
        reorderable: true,
        sortable: { mode: 'multiple', allowUnsort: true },
        pageable: {
            info: true, numeric: false, previousNext: false,
            messages: { display: '{0} - {1} of {2} rows' }
        },
        noRecords: {
            template: 'No data available'
        },
        filterable: {
            extra: false,// extra includes 'and' 'or' for another filter
            operators: {
                string: {
                    contains: 'Contains',
                    eq: 'Equals',
                },
                number: {
                    eq: '=',
                    gt: '>',
                    lt: '<'
                },
                date: {
                    eq: 'On',
                    gt: 'After',
                    lt: 'Before'
                }
            }
        },
        excel: {
            allPages: true,//true will take data from all pages in grid, false will take only current page.
            filterable: true,//keep filters when transferring to excel
        },
        excelExport: function (e) {
            var self = this;
            var sheet = e.workbook.sheets[0];
            var header = sheet.rows[0];
            var templates = [];
            var data = e.data;

            //find all columns with exprotTemplate
            self.columns.forEach(function (column) {
                if (column.exportTemplate) {
                    templates.push({
                        column: column.title,
                        template: kendo.template(column.exportTemplate)
                    });
                }
            });

            // get the column indexes in sheet 
            templates.forEach(function (template) {
                header.cells.forEach(function (cell, i) {
                    if (cell.value === template.column) {
                        template.index = i;
                        return;
                    }
                });
            });

            //run exportTemplate
            for (var i = 1; i < sheet.rows.length; i++) {
                var row = sheet.rows[i];
                templates.forEach(function (t) {
                    t.index && (row.cells[t.index].value = t.template(data[i - 1]));
                });
            }
        }
    };
});