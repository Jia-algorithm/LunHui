<template>
    <div id='jing' style="width:90%;height:500px;"></div>
</template>
<script>
import * as echarts from 'echarts';
console.log('import')
export default {
    name:'Jing',
    data(){
        return{
            
        }
    },
    mounted() {
        var chartDom = document.getElementById('jing');
        var myChart = echarts.init(chartDom);
        var option;

        var data = [];
        for (let i = 0; i < 5; i++) {
            data.push(0);
        };
        
        option = {
            xAxis: {
                max: 'dataMax',
            },
            yAxis: {
                type: 'category',
                data: ['井0', '井1', '井2','井3','井4'],
                inverse: true,
                animationDuration: 300,
                animationDurationUpdate: 300,
                max: 3 // only the largest 5 bars will be displayed
            },
            series: [{
                realtimeSort: true,
                name: '投胎数',
                type: 'bar',
                data: data,
                label: {
                    show: true,
                    position: 'right',
                    valueAnimation: true
                }
            }],
            legend: {
                show: true
            },
            animationDuration: 0,
            animationDurationUpdate: 3000,
            animationEasing: 'linear',
            animationEasingUpdate: 'linear'
        };

        function run () {
            var data = option.series[0].data;
            axios.get('/api/api/ToutaiContrller/GetCurrentCompleted').then(async (response) => {
                var res = response.data;
                for (var i = 0; i < data.length; ++i) {
                    
                    data[i] = res[i.toString()]
                    
                }
                
            }).catch((error) => {
                return;
            })
                
            myChart.setOption(option);
        };

        setTimeout(function() {
            run();
        }, 0);
        setInterval(function () {
            run();
        }, 3000);

        option && myChart.setOption(option);
    }
}
</script>