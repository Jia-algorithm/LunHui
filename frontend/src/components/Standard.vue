<template>
    <div style="display:flex;justify-content:center;">
        
        <el-dialog :visible.sync="visual" :title="title">
            <el-form :model="newItem" :rules="rules" ref="newItem">
                <el-form-item label="物种" :label-width="formLabelWidth" prop="name">
                    <el-input v-model="newItem.name" />
                </el-form-item>
                <!--<el-form-item label="小善权重" :label-width="formLabelWidth" prop="a1weight">
                    <el-input v-model.number="newItem.a1weight" />
                </el-form-item>
                <el-form-item label="中善权重" :label-width="formLabelWidth" prop="a2weight">
                    <el-input v-model.number="newItem.a2weight" />
                </el-form-item>
                <el-form-item label="大善权重" :label-width="formLabelWidth" prop="a3weight">
                    <el-input v-model.number="newItem.a3weight" />
                </el-form-item>

                <el-form-item label="小恶权重" :label-width="formLabelWidth" prop="b1weight">
                    <el-input v-model.number="newItem.b1weight" />
                </el-form-item>
                <el-form-item label="中恶权重" :label-width="formLabelWidth" prop="b2weight">
                    <el-input v-model.number="newItem.b2weight" />
                </el-form-item>
                <el-form-item label="大恶权重" :label-width="formLabelWidth" prop="b3weight">
                    <el-input v-model.number="newItem.b3weight" />
                </el-form-item>-->

                <el-form-item label="投胎线（单位：功德点）" :label-width="formLabelWidth" prop="line">
                    <el-input v-model.number="newItem.line" />
                </el-form-item>
                <el-form-item>
                    <el-button type="primary" @click="submitForm('newItem')">{{buttonText}}</el-button>
                    <el-button @click="resetForm('newItem')">重置</el-button>
                </el-form-item>
            </el-form>
        </el-dialog>
        <div style="margin:20px;width:60%;">
        <el-table :data="standard" stripe border :span-method="arraySpanMethod">
            <el-table-column width="100" prop="speciesID" label="ID" />
            <el-table-column width="300" prop="name" label="物种" />
            <!--<el-table-column width="150" prop="a1weight" label="小善权重" />
            <el-table-column width="150" prop="a2weight" label="中善权重" />
            <el-table-column width="150" prop="a3weight" label="大善权重" />

            <el-table-column width="150" prop="b1weight" label="小恶权重" />
            <el-table-column width="150" prop="b2weight" label="中恶权重" />
            <el-table-column width="150" prop="b3weight" label="大恶权重" />-->
            <el-table-column width="300" prop="line" label="投胎线（单位：功德点）" />
            <el-table-column label="操作">
                <template slot-scope="scope">
                    <el-button @click="editItem(scope.row)" type="text" size="small">编辑</el-button>
                    <el-button @click="deleteItem(scope.row)" type="text" size="small">删除</el-button>
                </template>
            </el-table-column>
        </el-table>
        </div>
        <el-button @click="AddNew" type="primary" style="margin:20px;height:40px;float:right;">添加新标准</el-button>
    </div>
</template>

<script>
export default {
    name: "Standard",
    data() {
        return {
            standard:[],
            stnum:0,
            newItem: {
                speciesID: '',
                name: '',
                /*a1weight: 0,
                a2weight:0,
                a3weight:0,
                b1weight:0,
                b2weight:0,
                b3weight: 0,*/
                line:0,
            },
            buttonText: '立即创建',
            title: '',
            visual: false,
            currentIndex: '',
            formLabelWidth: '120px',
            rules: {
                name: [{required: true, message: '请输入物种名称', trigger: 'blur'}],
                /*a1weight: [
                    {required: true, message: '请输入善行权重', trigger: 'blur'},
                    {type: 'number', message: '权重必须为数字'}
                ],
                a2weight: [
                    {required: true, message: '请输入善行权重', trigger: 'blur'},
                    {type: 'number', message: '权重必须为数字'}
                ],
                a3weight: [
                    {required: true, message: '请输入善行权重', trigger: 'blur'},
                    {type: 'number', message: '权重必须为数字'}
                ],
                b1weight: [
                    {required: true, message: '请输入恶行权重', trigger: 'blur'},
                    {type: 'number', message: '权重必须为数字'}
                ],
                b2weight: [
                    {required: true, message: '请输入恶行权重', trigger: 'blur'},
                    {type: 'number', message: '权重必须为数字'}
                ],
                b3weight: [
                    {required: true, message: '请输入恶行权重', trigger: 'blur'},
                    {type: 'number', message: '权重必须为数字'}
                ],*/
                line: [
                    {required: true, message: '请输入投胎线（单位：功德点）', trigger: 'blur'},
                    {type: 'number', message: '投胎线必须为数字'}
                ]
            }
        };
    },

    mounted() {
        console.log(this.$Global.iflogin)
        if(this.$Global.iflogin == false){
            this.$router.push({name: 'Login'})
        }
        else{
            this.getData()
        }
    },

    methods: {
        getData() {
            axios.get('/api/api/Species').then(async (response) => {
                var res = response.data
                this.standard = res
                this.stnum = res.length
                console.log(res)
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
/*
        arraySpanMethod({ row, column, rowIndex, columnIndex }) {
            var name = row.Attachment
            var count = 0
            if(columnIndex === 0) {
                if(rowIndex === this.attachIndex[name]) {
                    return [this.attaches[name], 1]
                }
                else {
                    return [0, 0]
                }
            }
        },

        updateCell() {
            this.attaches = {}
            this.attachIndex = {}
            var oldValue = ''
            var oldIndex = 0
            var count = 0
            var index = 0
            this.standard.forEach(element => {
                if(element.Attachment != oldValue) {
                    if(oldValue != '') {
                        this.attaches[oldValue] = count
                        this.attachIndex[oldValue] = oldIndex
                        count = 0
                        oldIndex = index
                    }
                    oldValue = element.Attachment
                }
                count++
                index++
            })
            this.attaches[oldValue] = count
            this.attachIndex[oldValue] = oldIndex
        },
*/
        AddNew() {
            this.visual = true
            this.title = "新建物种功德评估标准"
            this.buttonText = '立即创建'
            this.newItem.speciesID = this.standard.length
        },

        editItem(row) {
            this.newItem = row;
            this.visual = true;
            this.buttonText = '立即修改';
            this.title = "修改原有标准";
            console.log(this.newItem);
        },

        deleteItem(row) {
            console.log(row);
            var id = row.speciesID;

            axios.post('/api/api/Species/EasyDeleteSpecies?id='+id).then((response) => {
                var res = response.data;
                console.log(res.message);
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

        submitForm(formName) {
            this.$refs[formName].validate((valid) => {
            if (valid) {
                this.visual = false
                this.insertNew()
            } 
            else {
                return false;
            }
            });
        },

        insertNew() {
            var params = {
                "speciesID": this.newItem.speciesID,
                "name": this.newItem.name,
                /*"a1weight": this.newItem.a1weight,
                "a2weight": this.newItem.a2weight,
                "a3weight": this.newItem.a3weight,
                "b1weight": this.newItem.b1weight,
                "b2weight": this.newItem.b2weight,
                "b3weight": this.newItem.b3weight,*/
                "line": this.newItem.line,
                "status": 1
            }
            console.log(params);
            //if(params.speciesID === 0)
            //    delete params.speciesID
            var op = this.buttonText;
            if(op=="立即创建"){
                axios.post('/api/api/Species/addSpecies', params)
                .then((response) => {
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
            }
            else{
                axios.post('/api/api/Species/EditSpecies', params).then((response) => {
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
            }
                
        },

        resetForm(formName) {
            this.$refs[formName].resetFields();
        }
    },

    computed: {
        changeable() {
            return this.currentIndex != ''
        }
    },
/*
    watch: {
        standard() {
            this.updateCell()
            return this.standard
        }
    }
*/
}
</script>