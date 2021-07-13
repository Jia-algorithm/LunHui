<template>
    <div id='pie' style="height:400px;width:100%;"></div>
</template>
<script>
import * as echarts from 'echarts';

export default {
    name:'Bing',
    data () {
        return {
            doc:{}
                
        }
    },
    mounted() {
        this.getData();
        this.draw();
    },
    methods:{
        getData(){
            axios.post('/back/document/gettoday').then(async (response) => {
                var res = response.data;
                this.doc = res;
            })
        },
        timer() {
            return setTimeout(()=>{
                this.getData()
                this.draw()
            },3000)
        },
        draw(){
            var chartDom = document.getElementById('pie');
            var myChart = echarts.init(chartDom);
            var option;
            var doc = this.doc;
            option = {
                tooltip: {
                    trigger: 'item'
                },
                legend: {
                    top: '5%',
                    left: 'center'
                },
                series: [
                    {
                        name: '',
                        type: 'pie',
                        radius: ['40%', '70%'],
                        avoidLabelOverlap: false,
                        itemStyle: {
                            borderRadius: 10,
                            borderColor: '#fff',
                            borderWidth: 2
                        },
                        label: {
                            show: false,
                            position: 'center'
                        },
                        emphasis: {
                            label: {
                                show: true,
                                fontSize: '40',
                                fontWeight: 'bold'
                            }
                        },
                        labelLine: {
                            show: false
                        },
                        data: [
                            {value: doc.newdeads, name: '今日新到'},
                            {value: doc.newborn, name: '今日投胎'},
                            {value: doc.queue, name: '当前排队'}
                        ]
                    }
                ]
            };

            option && myChart.setOption(option);
        }
            
    },
    watch:{
        doc(){
            this.timer()
        }
    },
    destroyed() {
        clearTimeout(this.timer)
    },
}
</script>