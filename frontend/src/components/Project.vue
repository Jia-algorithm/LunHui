<template>
    <div style='margin:20px;'>
        <div class='operates'>
            <div style='margin-top:20px;margin-bottom:20px;'>
                <el-button type="primary" @click="Calculate()" plain>计算功德值</el-button>
                <el-button type="primary" @click="Start()" plain>开始投胎</el-button>
                <el-tooltip style='margin-left:20px;' placement="bottom-start" effect="light">
                    <div style="font-size:16px;" slot="content">该页面默认展示过去24h内新到的死者，可进行功德值计算。
                        <br/>吉时开启当日投胎，页面将动态展示投胎情况。
                    </div>
                    <i class="el-icon-question" width='10px;'>操作说明</i>
                </el-tooltip>
            </div>
            <div>
                <Display v-if="ifdisplay"></Display>
                <Jing v-if="ifshow"></Jing>
            </div>
        </div>
        <el-dialog :visible.sync="editvisual" :title="死者信息修改">
            <el-form :model="newItem" :rules="rules" ref="newItem">
                <el-form-item label="名称" :label-width="formLabelWidth" prop="name">
                    <el-input v-model.number="newItem.name" />
                </el-form-item>
                <el-form-item label="死亡时间" :label-width="formLabelWidth" prop="death">
                    <el-input v-model.number="newItem.death" />
                </el-form-item>
                <!--<el-form-item label="小善数" :label-width="formLabelWidth" prop="a1num">
                    <el-input v-model.number="newItem.a1num" />
                </el-form-item>
                <el-form-item label="中善数" :label-width="formLabelWidth" prop="a2num">
                    <el-input v-model.number="newItem.a2num" />
                </el-form-item>
                <el-form-item label="大善数" :label-width="formLabelWidth" prop="a3num">
                    <el-input v-model.number="newItem.a3num" />
                </el-form-item>

                <el-form-item label="小恶数" :label-width="formLabelWidth" prop="b1num">
                    <el-input v-model.number="newItem.b1num" />
                </el-form-item>
                <el-form-item label="中恶数" :label-width="formLabelWidth" prop="b2num">
                    <el-input v-model.number="newItem.b2num" />
                </el-form-item>
                <el-form-item label="大恶数" :label-width="formLabelWidth" prop="b3num">
                    <el-input v-model.number="newItem.b3num" />
                </el-form-item>-->
                <el-form-item label="功德值" :label-width="formLabelWidth" prop="value">
                    <el-input v-model.number="newItem.value" />
                </el-form-item>
                <el-form-item label="当前状态(1未计算功德值，2已计算功德值，3已投胎)" :label-width="formLabelWidth" prop="status">
                    <el-input v-model.number="newItem.status" />
                </el-form-item>
                <el-form-item>
                    <el-button type="primary" @click="submitForm('newItem')">立即修改</el-button>
                    <el-button @click="resetForm('newItem')">重置</el-button>
                </el-form-item>
            </el-form>
        </el-dialog>
        <el-table
        v-loading="loading"
        :data="tableData.filter(data => !search || data.name.toLowerCase().includes(search.toLowerCase()))"
        :default-sort = "{prop: 'date', order: 'descending'}"
        >

            <el-table-column
            label="ID"
            prop="id"
            width="100">
            </el-table-column>
            <el-table-column
            label="物种"
            prop="species"
            width="150">
            </el-table-column>
            <el-table-column
            label="名称"
            prop="name"
            width="150">
            </el-table-column>
            <el-table-column
            label="死亡时间"
            prop="death"
            width="300"
            sortable>
            </el-table-column>
            <!--<el-table-column
            label="小善数"
            prop="a1num"
            width="100"
            sortable>
            </el-table-column>
            <el-table-column
            label="中善数"
            prop="a2num"
            width="100"
            sortable>
            </el-table-column>
            <el-table-column
            label="大善数"
            prop="a3num"
            width="100"
            sortable>
            </el-table-column>
            <el-table-column
            label="小恶数"
            prop="b1num"
            width="100"
            sortable>
            </el-table-column>
            <el-table-column
            label="中恶数"
            prop="b2num"
            width="100"
            sortable>
            </el-table-column>
            <el-table-column
            label="大恶数"
            prop="b3num"
            width="100"
            sortable>
            </el-table-column>-->
            <el-table-column
            label="功德值"
            prop="value"
            width="200"
            sortable>
            </el-table-column>
            <el-table-column
            label="当前状态(1未计算功德值，2已计算功德值，3已投胎)"
            prop="status"
            width="400"
            sortable>
            </el-table-column>
            <el-table-column
            align="right">
            <template slot="header" slot-scope="scope">
                <el-input
                v-model="search"
                size="mini"
                placeholder="输入名称搜索"/>
            </template>
            <template slot-scope="scope">
                <el-button
                size="mini"
                @click="check(scope.$index, scope.row)">查看生平</el-button>
                <el-button
                size="mini"
                type="danger"
                @click="handleEdit(scope.$index, scope.row)">修改</el-button>
            </template>
            </el-table-column>
        </el-table>
        <el-dialog :visible.sync="visual" :title="title">
            <el-table
            :data="eventData"
            style="width: 100%">
            <el-table-column
                prop="eventTime"
                label="日期"
                >
            </el-table-column>
            <el-table-column
                prop="eventContent"
                label="事件"
                >
            </el-table-column>
            <el-table-column
                prop="eventAssess"
                label="评估(功德值)">
            </el-table-column>
            </el-table>
        </el-dialog>
    </div>

</template>
<style scoped>
    .search {
        width:20%;
        margin:20px;
    }
</style>
<script>
import Display from '../components/Display'
import Jing from '../components/Jing'
export default {
    name: "Project",
    components:{
        Display,
        Jing
    },
    data() {
        return {
            title:'',
            visual: false,
            editvisual:false,
            ifdisplay:false,
            ifshow:false,
            eventData:[],
            tableData: [],
            newItem:{},
            search: '',
            loading: true
        }
    },
    mounted() {
        if(this.$Global.iflogin == false){
            this.$router.push({name: 'Login'})
        }
        else{
            this.getData()
        }
    },
    methods: {
        getData() {
            axios.get('/api/api/Deads').then(async (response) => {
                var res = response.data;
                var myDate=new Date();
                var result = []
                myDate.setDate(myDate.getDate()-1);
                var d = myDate.toISOString();
                res.forEach(element => {
                    if(element.death > d){
                        result.push(element);
                    }
                });
                this.tableData = result;
                this.loading = false;
                //await this.updateCell()
            })
            .catch((error) => {
                if (error.response) {
                // 请求已发出，但服务器响应的状态码不在 2xx 范围内
                var res = error.response.data;
                var msg = res.message;
                var status = res.status;
                this.$message({
                message: msg,
                type: status})
                } else {
                console.log('Error', error.message);
                }
                console.log(error.config);
            })
        },
        timer() {
            return setTimeout(()=>{
                this.getData()
            },3000)
        },
        Calculate(){
            console.log("开始计算。")
            axios.post('/back/calculate').then(async(response) => {
                this.$message({
                    message:"开始计算!",
                    type:"success"
                })
                var res = response.data;
                console.log(res);
                switch(res) {
                    case 'no to calculate':
                        this.$message({
                            message: '当前无需要计算功德值的生物！',
                            type: 'warning'
                        })
                        break
                    case 'update failure':
                        this.$message.error('更新失败，请重试！')
                        break
                    default:
                        this.$message({
                            message:"更新成功!",
                            type:"success"
                        })
                }
            }).catch((error) => {
                if (error.response) {
                // 请求已发出，但服务器响应的状态码不在 2xx 范围内
                var res = error.response.data;
                console.log(res);
                } else {
                console.log(error);
                }
            })
        },
        check(index, row) {
            this.visual = true;
            var id = row.id;
            var name = row.name;
            this.title = '编号 '+id+' 的功德簿';
            axios.post('/api/api/Deads/ViewGongde?objID='+id).then(async (response) => {
                var res = response.data;
                console.log(res.gongdes)
                this.eventData = res.gongdes;
                
                //await this.updateCell()
            })
            .catch((error) => {
                if (error.response) {
                // 请求已发出，但服务器响应的状态码不在 2xx 范围内
                var res = error.response.data;
                var msg = res.message;
                var status = res.status;
                this.$message({
                message: msg,
                type: status})
                } else {
                console.log('Error', error.message);
                }
                console.log(error.config);
            })
            console.log(index, row);
        },
        handleEdit(index, row) {
            this.newItem = row;
            this.editvisual = true;
            console.log(this.newItem);
        },
        submitForm(formName) {
            this.$refs[formName].validate((valid) => {
            if (valid) {
                this.editvisual = false
                this.update()
            } 
            else {
                return false;
            }
            });
        },
        update(){
            var params = {
                "speciesID": this.newItem.speciesID,
                "objID":this.newItem.id,
                "name": this.newItem.name,
                "birthTime": this.newItem.birth,
                "deathTime": this.newItem.death,
                "reason": this.newItem.reason,
                /*"addr": this.newItem.addr,
                "a1num": this.newItem.a1num,
                "a2num": this.newItem.a2nm,
                "a3num": this.newItem.a3nm,
                "b1num": this.newItem.b1num,
                "b2num": this.newItem.b2num,
                "b3num": this.newItem.b3num,*/
                "value": this.newItem.value,
                "status": this.newItem.status,
            }
            axios.post('/api/api/Deads/EditDeads', params).then((response) => {
                var res = response.data
                console.log(res)
                var msg = res.message;
                var status = res.status;
                this.$message({
                message: msg,
                type: status
                });
            })
            .catch((error) => {
                if (error.response) {
                // 请求已发出，但服务器响应的状态码不在 2xx 范围内
                var res = error.response.data;
                var msg = res.message;
                var status = res.status;
                this.$message({
                message: msg,
                type: status})
                } else {
                console.log('Error', error.message);
                }
                console.log(error.config);
            })
        },
        Start() {
            axios.post('/api/api/ToutaiContrller/BeginToutai').then((response) => {
                var res = response.data
                console.log(res)
                this.$message({
                message: "开始投胎！",
                type:"success"
                });
                this.ifdisplay=true;
                this.ifshow = true;
            }).catch((error) => {
                this.$message({
                message: "投胎异常！",
                type: "error"
                })
            })
        },
        
    },
    watch: {
      tableData() {
        this.timer() 
      }
    },
    destroyed() {
        clearTimeout(this.timer)
    }

}
</script>

<style scoped>

</style>
