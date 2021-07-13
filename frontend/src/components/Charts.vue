<template>
    <div id='main' style="height:500px;width:100%;"></div>
</template>

<script>
import * as echarts from 'echarts'; 
export default {
    name: 'Charts',
    data () {
        return {
        }
    },
    mounted() {
        // 基于准备好的dom，初始化echarts实例
        var myChart = echarts.init(document.getElementById('main'));
        // 绘制图表
        myChart.setOption({
            title: {
                text: '地府每日新到统计'
            },
            tooltip: {},
            xAxis: {
                data: ['05-28','05-29', '05-30', '05-31', '06-01', '06-02']
            },
            yAxis: {},
            series: [{
                name: '数量',
                type: 'line',
                data: [27898, 12434, 36242, 13510, 36472, 25430]
            }]
        });
        
    }
}
</script>