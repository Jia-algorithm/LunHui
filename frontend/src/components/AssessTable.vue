<template>
    <div class="total">
        <div class="add">
            <el-button type="primary" class="el-icon-circle-plus" circle @click="addNewPerson" :disabled="canDownload"></el-button>
        </div>
        <div class="search">
            <el-input v-model="search" prefix-icon="el-icon-search" placeholder="请输入搜索的内容" />
        </div>
        <div class="selector">
            <span class="moneyStandard">补偿标准</span>
            <el-select v-model="moneyStandard" @change="moneyChanged" :disabled="canDownload">
                <el-option v-for="assess in AssessStandard" :key="assess" :label="assess" :value="assess" />
            </el-select>
            <el-button type="primary" @click="downLoadProject" :disabled="canDownload">下载评估表</el-button>
            <el-button type="warning" @click="exportProject" :disabled="canDownload" style="margin-left:0">导出项目</el-button>
        </div>
        <el-table :data="res" stripe border>
            <el-table-column type="index" width="50" />
            <el-table-column prop="Village" label="地点" />
            <el-table-column prop="Name" label="姓名" />
            <el-table-column fixed="right" label="操作" width="100">
                <template slot-scope="scope">
                    <el-button @click="editItem(scope.row)" type="text" size="small" :disabled="canDownload">编辑</el-button>
                    <el-button @click="deleteItem(scope.row)" type="text" size="small" :disabled="canDownload">删除</el-button>
                </template>
            </el-table-column>
        </el-table>
        <el-dialog :visible.sync="visual" :title="title" width="80%" @close="finishEditing">
            <el-form :model="newItem" :rules="rules" ref="newItem">
                <el-form-item label="地点" :label-width="formLabelWidth" prop="Village">
                    <el-input v-model="newItem.Village" />
                </el-form-item>
                <el-form-item label="姓名" :label-width="formLabelWidth" prop="Name">
                    <el-input v-model="newItem.Name" />
                </el-form-item>
                <el-table :data="newItem.Assess" stripe border>
                    <el-table-column prop="Attachment" label="附属物名称">
                        <template slot-scope="scope">
                            <el-select v-model="scope.row.Attachment" @change="attachmentChanged(scope.row)">
                                <el-option v-for="attach in attachment" :key="attach.Attachment" :label="attach.Attachment" :value="attach.Attachment" :disabled="attach.disable" />
                            </el-select>
                        </template>
                    </el-table-column>
                    <el-table-column prop="Specs" label="规格">
                        <template slot-scope="scope">
                            <el-select v-model="scope.row.Specs" @change="specsChanged(scope.row)">
                                <el-option v-for="Specs in attachDict[scope.row.Attachment]" :key="Specs.Specs" :label="Specs.Specs" :value="Specs.Specs" :disabled="Specs.disable" />
                            </el-select>
                        </template>
                    </el-table-column>
                    <el-table-column prop="Unit" label="单位">
                        <template slot-scope="scope">
                            {{getUnit(scope.row)}}
                        </template>
                    </el-table-column>
                    <el-table-column prop="Count" label="数量">
                        <template slot-scope="scope">
                            <el-input v-model="scope.row.Count" />
                        </template>
                    </el-table-column>
                    <el-table-column prop="Price" label="单价(元)">
                        <template slot-scope="scope">
                            {{getPrice(scope.row)}}
                        </template>
                    </el-table-column>
                    <el-table-column prop="Total" label="补偿金额(元)">
                        <template slot-scope="scope">
                            {{getTotal(scope.row)}}
                        </template>
                    </el-table-column>
                    <el-table-column label="删除">
                        <template slot-scope="scope">
                            <el-button class="el-icon-delete-solid" type="danger" @click="deleteAssess(scope.$index, scope.row)" />
                        </template>
                    </el-table-column>
                </el-table>
                <el-form-item>
                    <el-button @click="addNewAssess" :disabled="canDownload">添加</el-button>
                    <el-button type="primary" @click="submitForm('newItem')" :disabled="canDownload">{{buttonText}}</el-button>
                    <el-button @click="downLoad" :disabled="canDownload">下载文件</el-button>
                </el-form-item>
            </el-form>
        </el-dialog>
    </div>
</template>

<script>
let Base64 = require('js-base64').Base64
export default {
    name: "AssessTable",
    data() {
        return {
            webSocket: null,
            search: '',
            id: "",
            personIns: [],
            projectInfo: [],
            attachment: [],
            standard: null,
            attachDict: {},
            AssessStandard: ['高', '中', '低'],
            moneyStandard: '高',
            newItem: {
                Id: '',
                Name: '',
                Village: '',
                Assess: []
            },
            formLabelWidth: '120px',
            visual: false,
            title: '编辑',
            buttonText: '确认修改',
            currentIndex: 1,
            canDownload: false,
            rules: {
                Name: [{required: true, message: '请输入姓名', trigger: 'blur'}],
                Village: [{required: true, message: '请输入地点', trigger: 'blur'}],
                High: [
                    {required: true, message: '请输入最高标准', trigger: 'blur'},
                    {type: 'number', message: '补偿必须为数字'}
                ],
                Middle: [
                    {required: true, message: '请输入中等标准', trigger: 'blur'},
                    {type: 'number', message: '补偿必须为数字'}
                ],
                Low: [
                    {required: true, message: '请输入最低标准', trigger: 'blur'},
                    {type: 'number', message: '补偿必须为数字'}
                ],
                Unit: [{required: true, message: '请输入度量单位', trigger: 'blur'}]
            }
        }
    },

    mounted() {
        this.id = this.$route.params.title
        this.getData(this.id)
    },

    methods: {
        getData(id) {
            axios.get('./api/project/'+id).then((response) => {
                var res = response.data
                this.personIns = res
            })
            axios.get('./api/project/project='+id).then((response) => {
                var res = response.data
                if(res === null)
                    this.projectInfo = []
                else
                    this.projectInfo = res
                this.moneyStandard = this.projectInfo.Standard
            })
            axios.get('./api/standard/all_attach').then((response) => {
                var res = response.data
                res.forEach(element => {
                    this.attachment.push({Attachment: element, disable: false})
                })
            })
            axios.get('./api/standard').then((response) => {
                var res = response.data
                this.standard = res
                this.standard.forEach(element => {
                    if(!(element.Attachment in this.attachDict))
                        this.attachDict[element.Attachment] = []
                    this.attachDict[element.Attachment].push({Specs: element.Specs, disable: false})
                })
            })
        },

        OpenWebSocketConnection(projectId, personId) {
            return new WebSocket("ws://"+location.host+"/socket?projectId="+projectId+"&"+"personId="+personId)
        },

        moneyChanged() {
            this.webSocket = this.OpenWebSocketConnection(this.projectInfo.Id, "-1")
            var self = this
            this.webSocket.onopen = function(event) {
                self.canDownload = true
                axios.get('./api/project/standard?id='+self.id+'&standard='+self.moneyStandard).then((response) => {
                    self.$notify({title: '成功', message: '补偿标准修改成功', type: 'success'})
                    self.canDownload = false
                    self.webSocket.close()
                }).catch((error) => {
                    self.$notify.error({title: '失败', message: '出现未知错误，请重试'})
                    self.canDownload = false
                    self.webSocket.close()
                })
            }
            this.webSocket.onerror = function(event) {
                self.$message.error("您请求的资源正在被其他人编辑，请等待完成")
                self.moneyStandard = self.projectInfo.Standard
            }
        },

        updateAttachAndSpecs() {
            var assess = this.newItem.Assess
            this.attachment.forEach(element => {
                element.disable = false
            })
            for(var key in this.attachDict) {
                this.attachDict[key].forEach(element => {
                    element.disable = false
                })
            }
            assess.forEach(element => {
                if(element.Attachment in this.attachDict)
                    for(var i=0; i<this.attachDict[element.Attachment].length; i++) {
                        if(this.attachDict[element.Attachment][i].Specs === element.Specs) {
                            this.attachDict[element.Attachment][i].disable = true
                            break
                        }
                    }
            })
            assess.forEach(element => {
                var flag = true
                if(element.Attachment in this.attachDict) {
                    for(var i=0; i<this.attachDict[element.Attachment].length; i++) {
                        if(this.attachDict[element.Attachment][i].disable === false) {
                            flag = false
                        }
                    }
                    for(var i=0; i<this.attachment.length; i++) {
                        if(this.attachment[i].Attachment === element.Attachment) {
                            this.attachment[i].disable = flag
                            break
                        }
                    }
                }
                
            })
        },

        attachmentChanged(row) {
            for(var i=0; i<this.attachDict[row.Attachment].length; i++) {
                if(this.attachDict[row.Attachment][i].disable === false) {
                    row.Specs = this.attachDict[row.Attachment][i].Specs
                    break
                }
            }
            this.updateAttachAndSpecs()
        },

        specsChanged(row) {
            this.updateAttachAndSpecs()
        },

        getPersonAssess(person_id) {
            axios.get('./api/project/person='+person_id).then((response) => {
                var res = response.data
                this.newItem = res
                this.updateAttachAndSpecs()
            })
        },

        addNewPerson() {
            this.newItem = {
                Id: '000000000000000000000000',
                Name: '',
                Village: '',
                Assess: [],
                ProjectId: this.projectInfo.Id
            }
            this.visual = true
            this.currentIndex = this.personIns.length + 1
            this.buttonText = "确认添加"
            this.canDownload = false
        },

        editItem(row) {
            this.webSocket = this.OpenWebSocketConnection(this.projectInfo.Id, row.Id)
            // this.webSocket = new WebSocket("ws://"+location.host+"/socket?projectId="+this.projectInfo.Id+"&"+"personId="+row.Id)
            var self = this
            this.webSocket.onopen = function(event) {
                self.visual = true
                for(var i=1; i<=self.personIns.length; i++) {
                    if(row.Id === self.personIns[i-1].Id) {
                        self.currentIndex = i
                        break
                    }
                }
                self.getPersonAssess(row.Id)
                self.buttonText = "确认修改"
                self.canDownload = false
            }
            this.webSocket.onerror = function(event) {
                self.$message.error("您请求的资源正在被其他人编辑，请等待完成")
            }
        },

        finishEditing() {
            this.webSocket.close()
        },

        deleteItem(row) {
            this.webSocket = this.webSocket = this.OpenWebSocketConnection(this.projectInfo.Id, row.Id)
            var self = this
            this.webSocket.onopen = function(event) {
                var id = row.Id
                axios.delete('./api/project/person='+id).then((response) => {
                    var res =  response.data
                    this.personIns = res
                    this.webSocket.close()
                })
            }
            this.webSocket.onerror = function(event) {
                self.$message.error("您请求的资源正在被其他人编辑，请等待完成")
            }
        },

        addNewAssess() {
            var item = {
                Attachment: '',
                Count: 0,
                Price: 0,
                Total: 0,
                Specs: '',
                Unit: ''
            }
            for(var i=0; i<this.attachment.length; i++) {
                if(this.attachment[i].disable === false) {
                    item.Attachment = this.attachment[i].Attachment
                    for(var j=0; j<this.attachDict[item.Attachment].length; j++) {
                        if(this.attachDict[item.Attachment][j].disable === false) {
                            item.Specs = this.attachDict[item.Attachment][j].Specs
                            break
                        }
                    }
                    break
                }
            }
            this.newItem.Assess.push(item)
            this.updateAttachAndSpecs()
        },

        submitForm(formName) {
            var params = {
                "Id": this.newItem.Id,
                "Name": this.newItem.Name,
                "Village": this.newItem.Village,
                "ProjectId": this.newItem.ProjectId,
                "Assess": this.newItem.Assess
            }
            if(params.Id === '000000000000000000000000')
                delete params.Id
            this.$refs[formName].validate((valid) => {
            if (valid) {
                this.visual = false
                axios.post('./api/project/person', params).then((response) => {
                    var res = response.data
                    this.newItem = res
                    var flag = false
                    this.canDownload = false
                    this.personIns.forEach(element => {
                        if(element.Id === this.newItem.Id)
                            flag = true
                    })
                    if(!flag)
                        this.personIns.push(this.newItem)
                })
            } 
            else {
                return false;
            }
            });
        },

        deleteAssess(index, row) {
            this.newItem.Assess.splice(index, 1)
            this.updateAttachAndSpecs()
        },

        downLoad() {
            var fileDownload = require('js-file-download')
            this.canDownload = true
            this.$notify.info({title: '提示', message: '请稍等'})
            axios.get('./api/project/assess?id='+this.newItem.Id+'&index='+this.currentIndex, {responseType: 'arraybuffer'}).then((response) => {
                var fileName = response.headers['content-disposition'].split('?')[3]
                fileName = Base64.decode(fileName)
                fileDownload(response.data, fileName)
                this.$notify({title: '提示', message: '下载成功', type: 'success'})
                this.canDownload = false
            }).catch((error) => {
                this.$notify.error({title: '失败', message: '出现未知错误，请重试'})
                this.canDownload = false
            })
        },

        downLoadProject() {
            this.webSocket = this.webSocket = this.OpenWebSocketConnection(this.projectInfo.Id, "-1")
            var self = this
            this.webSocket.onopen = function(event) {
                var fileDownload = require('js-file-download')
                self.canDownload = true
                self.$notify.info({title: '提示', message: '请稍等'})
                const loading = self.$loading({
                    lock: true,
                    text: '加载中',
                    spinner: 'el-icon-loading',
                    background: 'rgba(0, 0, 0, 0.7)'
                })
                axios.get('./api/project/project_total?id='+self.id, {responseType: 'arraybuffer'}).then((response) => {
                    var fileName = response.headers['content-disposition'].split('?')[3]
                    fileName = Base64.decode(fileName)
                    fileDownload(response.data, fileName)
                    self.$notify({title: '提示', message: '下载成功', type: 'success'})
                    self.canDownload = false
                    loading.close()
                    self.webSocket.close()
                }).catch((error) => {
                    self.$notify.error({title: '失败', message: '出现未知错误，请重试'})
                    self.canDownload = false
                    loading.close()
                    self.webSocket.close()
                })
            }
            this.webSocket.onerror = function(event) {
                self.$message.error("您请求的资源正在被其他人编辑，请等待完成")
            }
        },

        exportProject() {
            this.webSocket = this.webSocket = this.OpenWebSocketConnection(this.projectInfo.Id, "-1")
            var self = this
            this.webSocket.onopen = function(event) {
                var fileDownload = require('js-file-download')
                self.canDownload = true
                self.$notify.info({title: '提示', message: '这可能需要等待一段时间'})
                const loading = self.$loading({
                    lock: true,
                    text: '加载中',
                    spinner: 'el-icon-loading',
                    background: 'rgba(0, 0, 0, 0.7)'
                })
                axios.get('./api/project/project_zip?id='+self.id+'&zipName='+self.projectInfo.Project, {responseType: 'arraybuffer'}).then((response) => {
                    var fileName = response.headers['content-disposition'].split('?')[3]
                    fileName = Base64.decode(fileName)
                    fileDownload(response.data, fileName)
                    self.$notify({title: '提示', message: '下载成功', type: 'success'})
                    self.canDownload = false
                    loading.close()
                    self.webSocket.close()
                }).catch((error) => {
                    self.$notify.error({title: '失败', message: '出现未知错误，请重试'})
                    self.canDownload = false
                    loading.close()
                    self.webSocket.close()
                })
            }
            this.webSocket.onerror = function(event) {
                self.$message.error("您请求的资源正在被其他人编辑，请等待完成")
            }
        },

        getUnit(row) {
            this.standard.forEach(element => {
                if(element.Attachment === row.Attachment && element.Specs === row.Specs)
                    row.Unit = element.Unit
            })
            return row.Unit
        },

        getPrice(row) {
            this.standard.forEach(element => {
                if(element.Attachment === row.Attachment && element.Specs === row.Specs) {
                    switch(this.moneyStandard) {
                        case '高':
                        row.Price = element.High
                        break
                        case '中':
                        row.Price = element.Middle
                        break
                        case '低':
                        row.Price = element.Low
                        break
                        default:
                        break
                    }
                }
            })
            return row.Price
        },

        getTotal(row) {
            row.Total = row.Count * row.Price
            return row.Total.toFixed(2)
        }
    },

    computed: {
        res() {
            var conditionList = this.personIns.filter((person) => {
                return (person.Name.indexOf(this.search) != -1 || person.Village.indexOf(this.search) != -1)
            })
            return conditionList
        }
    }
}
</script>

<style scoped>

    .total {
        padding-top: 10px;
    }

    .selector {
        float: right;
        margin-bottom: 10px;
    }

    .moneyStandard {
        margin-right: 5px;
        font-family: 'Times New Roman', Times, serif;
        font-size: 20px
    }

    .add {
        float: left
    }

    .search {
        float: left;
        padding-left: 40px;
        min-width: 300px;
    }

</style>