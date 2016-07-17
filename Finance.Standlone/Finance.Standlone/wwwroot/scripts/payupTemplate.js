define(function () {
    var buckets = ['LL8', 'MLB', 'INVST', 'NEW', '80-90', '90-95', '95-100', '100-105', 'CQ', 'CR', '10/20yr', 'MG']
                .concat(Array(1).fill(''));
    var poolTypes = ['FN3.0', 'FN3.5', 'FN4.0', 'FN4.5', '', 'DW2.5', 'DW3.0', 'DW3.5']
        .concat(Array(3).fill(''));
    var ginnieBuckets = ['t+0', 't-1', 't-2', 'LLB', 'MLB', 'HLB', '21-30', '31-40', '41-50', '51-60', 'VA', 'FHA', '3xMIP']
        .concat(Array(1).fill(''));
    var ginnieTypes = ['G2 3.0', 'G2 3.5', 'G2 4.0', 'G2 4.5']
        .concat(Array(3).fill(''));
    return {
        payup: {
            buckets: buckets.map(function (bucket, index) {
                return { order: index + 1, value: bucket };
            }),
            poolTypes: poolTypes.map(function (poolType, index) {
                return { order: index + 1, value: poolType };
            })
        },
        ginniePayup: {
            buckets: ginnieBuckets.map(function (bucket, index) {
                return { order: index + 1, value: bucket };
            }),
            poolTypes: ginnieBuckets.map(function (poolType, index) {
                return { order: index + 1, value: poolType };
            })
        }
    };
});