<template>
    <div id='display' style="width:90%;height:500px;"></div>
</template>
<script>
import * as echarts from 'echarts';
export default {
    name:'Display',
    data(){
        return{
            
        }
    },
    mounted() {
        console.log('start')
        var app = {};
        var chartDom = document.getElementById('display');
        var myChart = echarts.init(chartDom);
        var option;
        option = {
            title: {
                text: '总体实时数据',
                //subtext: '纯属虚构'
            },
            tooltip: {
                trigger: 'axis',
                axisPointer: {
                    type: 'cross',
                    label: {
                        backgroundColor: '#283b56'
                    }
                }
            },
            legend: {
                data:['排队数','投胎数']
            },
            toolbox: {
                show: true,
                feature: {
                    dataView: {readOnly: false},
                    restore: {},
                    saveAsImage: {}
                }
            },
            dataZoom: {
                show: false,
                start: 0,
                end: 100
            },
            xAxis: [
                {
                    type: 'category',
                    boundaryGap: true,
                    data: (function (){
                        var now = new Date();
                        var res = [];
                        var len = 10;
                        while (len--) {
                            res.unshift(now.toLocaleTimeString().replace(/^\D*/,''));
                            now = new Date(now - 2000);
                        }
                        return res;
                    })()
                },
                {
                    type: 'category',
                    boundaryGap: true,
                    data: (function (){
                        var res = [];
                        var len = 10;
                        while (len--) {
                            res.push(10 - len - 1);
                        }
                        return res;
                    })()
                }
            ],
            yAxis: [
                {
                    type: 'value',
                    scale: true,
                    //name: '',
                    //max: 2000000,
                    //min: 0,
                    animationDuration: 300,
                    animationDurationUpdate: 300,
                    boundaryGap: [0.2, 0.2]
                },
                {
                    type: 'value',
                    scale: true,
                    //name: '',
                    //max: 2000000,
                    //min: 0,
                    animationDuration: 300,
                    animationDurationUpdate: 300,
                    boundaryGap: [0.2, 0.2]
                }
            ],
            series: [
                {
                    name: '投胎数',
                    type: 'bar',
                    xAxisIndex: 1,
                    yAxisIndex: 1,
                    data: (function (){
                        var res = [];
                        var len = 10;
                        while (len--) {
                            res.push(0);
                        }
                        return res;
                    })()
                },
                {
                    name: '排队数',
                    type: 'line',
                    xAxisIndex: 1,
                    yAxisIndex: 1,
                    data: (function (){
                        var res = [];
                        var len = 10;
                        while (len--) {
                            res.push(0);
                        }
                        return res;
                    })()
                }
            ]
        };

        app.count = 11;
        setInterval(function (){
            var axisData = (new Date()).toLocaleTimeString().replace(/^\D*/, '');
            
            var data0 = option.series[0].data;
            var data1 = option.series[1].data;
            axios.get('/api/api/ToutaiContrller/GetCurrentCompleted').then(async (response) => {
                var res = response.data;
                console.log('res1',res)
                var toutai = res['0']+res['1']+res['2']+res['3']+res['4'];
                console.log(toutai);
                data0.shift();
                data0.push(toutai);
            })
            axios.get('/api/api/ToutaiContrller/getWaiting').then(async (response) => {
                var res = response.data;
                console.log('res2',res)
                var queue = res;
                
                data1.shift();
                data1.push(queue);
            })
            option.xAxis[0].data.shift();
            option.xAxis[0].data.push(axisData);
            option.xAxis[1].data.shift();
            option.xAxis[1].data.push(app.count++);

            myChart.setOption(option);
        }, 2100);

        option && myChart.setOption(option);
    }
}
</script>