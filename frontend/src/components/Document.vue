<template>
    <div>
        <div style="display:flex;width:100%;margin:20px;">
            <span style="float:left;line-height:40px;">当前共计{{datanum}}条数据</span>
            <div style="display:flex;margin-left:20px;">
                <div class="choose" style="margin-right:10px;">
                    <el-date-picker
                    v-model="daterange"
                    type="daterange"
                    range-separator="至"
                    start-placeholder="开始日期"
                    end-placeholder="结束日期">
                    </el-date-picker>
                </div>
                
                <el-button style="width:40px;height:40px;" icon="el-icon-search" @click="search" circle></el-button>
            </div>
            <el-tooltip style='margin-left:10px;' content="选择始末时间按后点击搜索按钮，查看一定时间段内的数据报表" placement="bottom-start" effect="light">
                <i class="el-icon-question" width='10px;'></i>
            </el-tooltip>
                
            <el-tooltip style='margin-left:20px;' placement="bottom-start" effect="light">
                <div style="font-size:16px;" slot="content">该模块可展示今日之前的每日数据报表，数据属性包括：
                    <br/>编号、日期、地府新到（单位为万，表示从当日0：00到当日24：00的新到死者数）、<br/>
                    当日投胎（单位为万，表示从当日0：00到当日24：00的新增投胎数量）、
                    <br/>剩余排队（单位为万，表示至当日新到地府中剩余排队死者数）
                </div>
                <el-button>数据说明</el-button>
            </el-tooltip>
        </div>
        <div style="margin:20px;">
            <el-table
            v-loading="loading"
            :data="tableData"
            :default-sort = "{prop: 'date', order: 'descending'}">
                <el-table-column prop="ID" type="index" label="编号" sortable></el-table-column>
                <el-table-column prop="date" label="日期" sortable></el-table-column>
                <el-table-column prop="newdeads" label="地府新到(万)" sortable></el-table-column>
                <el-table-column prop="newborn" label="当日投胎(万)" sortable></el-table-column>
                <el-table-column prop="queue" label="剩余排队(万)" sortable></el-table-column>
                
                <el-table-column label="操作">
                    <template slot-scope="scope">
                        <el-button size="mini" @click="downLoadFile(scope.row)" :disabled="disable">下载</el-button>
                    </template>
                </el-table-column>
            </el-table>
            <el-button type="primary" @click="exportExcel()" v-if="canOperate" class="operate">下载</el-button>
        </div>  
            
    </div>
</template>

<style scoped>
    .operate {
        float:right;
        margin:5px;
    }
</style>

<script>
let moment = require('moment');
export default {
    name: "Document",
    data() {
        return {
            datanum:3,
            daterange:'',
            tableData:[],
            loading: true,
            disable: false,
            canOperate: true,
        }
    },

    mounted() {
        this.datanum = this.tableData.length;
        if(this.$Global.iflogin == false){
            this.$router.push({name: 'Login'})
        }
        else{
            this.getData()
        }
        //this.getData()
    },
    
    methods: {
        search() {
            this.loading = true;
            var table = [];
            axios.post('/back/document/getall').then(async (response) => {
                var res = response.data
                console.log(res)
                if(res == "no documents"){
                    this.$message({
                        message:"当前无可生成报表！",
                        type:'warning'
                    })
                }
                else{
                    table = res;
                    var dates = this.daterange;
                    var date1 = moment(dates[0]).format("YYYY/M/DD");
                    var date2 = moment(dates[1]).format("YYYY/M/DD");
                    var newtable = [];
                    table.forEach(element => {
                        console.log(element.date);
                        console.log(date1,date2);
                        if(element.date > date1 && element.date < date2){
                            newtable.push(element);
                        }
                    })
                    this.tableData = newtable;
                    this.loading = false;
                }
            })
            
        },
        getData() {
            axios.post('/back/document/getall').then(async (response) => {
                var res = response.data
                console.log(res)
                if(res == "no documents"){
                    this.$message({
                        message:"当前无可生成报表！",
                        type:'warning'
                    })
                }
                else{
                    this.tableData = res;
                    this.loading = false;
                }
            })
            .catch((error) => {
                this.$message({
                    message:"出现未知错误，请重试！",
                    type:'error'
                })
                console.log(error.config);
            })
        },
        exportExcel() {
            
            require.ensure([], () => {
                const { export_json_to_excel } = require('../excel/Export2Excel');
                const tHeader = ['日期', '地府新到（万）','当日投胎（万）','剩余排队（万）'];
                // 上面设置Excel的表格第一行的标题
                const filterVal = ['date', 'newdeads', 'newborn','queue'];
                // tableData里对象的属性
                const list = this.tableData;  //把data里的tableData存到list
                const data = this.formatJson(filterVal, list);
                export_json_to_excel(tHeader, data, 'one-day-document');
            })
        },
        downLoadFile(row){
            require.ensure([], () => {
                var table = [row];
                const { export_json_to_excel } = require('../excel/Export2Excel');
                const tHeader = ['日期', '地府新到（万）','当日投胎（万）','剩余排队（万）'];
                // 上面设置Excel的表格第一行的标题
                const filterVal = ['date', 'newdeads', 'newborn','queue'];
                // tableData里对象的属性
                const list = table;  //把data里的tableData存到list
                const data = this.formatJson(filterVal, list);
                export_json_to_excel(tHeader, data, 'all-document');
            })
        },

        formatJson(filterVal, jsonData) {
            return jsonData.map(v => filterVal.map(j => v[j]))
        }
    
    }
}
</script>


