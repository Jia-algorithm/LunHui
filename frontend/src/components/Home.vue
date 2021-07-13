<template>
    <el-row :gutter="20">
        <el-col :span="15">
          <el-carousel indicator-position="outside" style="margin:20px;width:50%;padding-left:25%;">
            <el-carousel-item v-for="item in [require('../assets/santai.jpg'),require('../assets/dizheng.jpg')]" :key="item">
               <el-image :src="item" :fit="cover"></el-image>
            </el-carousel-item>
          </el-carousel>
          <div class="report">
              <el-card style='background-color:#f4f3f3;' shadow="hover">
                  <span>今日新到：</span><span id="death_num" v-text="death"></span>
              </el-card>
              <el-card style="background-color:#e4efe7;" shadow="hover">
                  <span>今日投胎：</span><span id="reborn_num" v-text="reborn"></span>
              </el-card>
              <el-card style="background-color:#f8efd4;" shadow="hover">
                  <span>当前排队：</span><span id="queue_num" v-text="queue"></span>
              </el-card>
          </div>
          <Bing></Bing>
            <!--<Charts></Charts>-->
        </el-col>
        <el-col :span="9"><div class="grid-content bg-purple">
            <el-card class="calender" shadow="hover">
                <el-calendar style="height:500px;" v-model="value"></el-calendar>
            </el-card>
            <div id="todo" class="todo">
                <h2>待办事项</h2>
                <div>
                    <el-input v-model="content" placeholder="新的待办" style="width:50%;"></el-input>
                    <el-button type="success" @click=add() round>添加</el-button>
                </div>
                <el-table
                    :data="tableData"
                    style="width: 100%">
                    <el-table-column
                    label="日期"
                    width="150">
                    <template slot-scope="scope">
                        <i class="el-icon-time"></i>
                        <span style="margin-left: 10px">{{ scope.row.date }}</span>
                    </template>
                    </el-table-column>
                    <el-table-column
                    label="待办"
                    width="250">
                    <template slot-scope="scope">
                        <span v-if="!showEdit[scope.$index]">{{ scope.row.content }}</span>
                        <input class="edit-cell" v-if="showEdit[scope.$index]" v-model="context">
                    </template>
                    </el-table-column>
                    <el-table-column label="操作">
                    <template slot-scope="scope">
                        <el-button
                        size="mini"
                        v-if="!showEdit[scope.$index]"
                        @click="handleEdit(scope.$index, scope.row)">编辑</el-button>
                        <el-popconfirm
                          title="确定删除此待办事项吗？"
                        >
                        <el-button
                        size="mini"
                        v-if="!showEdit[scope.$index]"
                        type="danger"
                        @click="handleDelete(scope.$index, scope.row)">删除</el-button>
                        </el-popconfirm>
                        <el-button
                        size="mini"
                        v-if="showEdit[scope.$index]"
                        @click="handleUpdate(scope.$index, scope.row)">更新</el-button>
                        <el-button
                        size="mini"
                        v-if="showEdit[scope.$index]"
                        type="danger"
                        @click="handleCancel(scope.$index, scope.row)">取消</el-button>


                    </template>
                    </el-table-column>
                </el-table>
            </div>
        </div></el-col>
    </el-row>
</template>
<style scoped>

  
  .el-carousel__item:nth-child(2n) {
    background-color:white;
  }
  
  .el-carousel__item:nth-child(2n+1) {
    background-color: white;
  }

.calender {
    margin:20px;
}
.report{
    margin:20px;
    display: flex;
    flex-direction: row;
    justify-content: space-around;
    font-size: 30px;
    line-height: 50px;
}
.todo {
    margin:20px;
    display:flex;
    flex-direction: column;
}
</style>
<script>
  import Charts from '../components/Charts'
  import Bing from '../components/Bing'
  export default {
    name:"Home",
    components: {
      Charts,
      Bing
    },
    data() {
      return {
        death:0,
        reborn:0,
        queue:0,
        showEdit: [], //显示编辑
        context:'',
        value: new Date(),
        content:'', //v-model绑定
        tableData: [{
          date: '2021-6-2',
          content:'导出本周所有报表'
        }, {
          date: '2021-6-4',
          content:'新增投胎井'
        }]
      }
    },
    mounted() {
      if(this.$Global.iflogin == false){
        this.$router.push({name: 'Login'})
      }
      else{
        this.getTodo();
        this.getDoc();
        var len = this.tableData.length;
        for(var i = 0; i < len; i ++) {
          this.showEdit[i] = false;
        }
      }
        
    },
    methods: {
      getDoc(){
        axios.post('/back/document/gettoday').then(async (response) => {
            var res = response.data;
            console.log(res);
            this.death = res.newdeads;
            this.reborn = res.newborn;
            this.queue = res.queue;
        })
      },
      getTodo() {
        axios.get('/api/api/Todo').then(async (response) => {
            var res = response.data;
            console.log(res);
            this.tableData = res;
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
      add() {
        let newdate = new Date();
        var content = this.content;
        var params = {
          "id": 0,
          "content": content,
          "date": newdate,
          "isCompleted": false
        }
        this.tableData.push(params)
        axios.post('/api/api/Todo/addTodo', params).then(async (response) => {
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
      handleEdit(index, row) {
        console.log(index);
        console.log(this.showEdit[index]);
        this.context = row.content;
        this.showEdit[index] = true;
        this.$set(this.showEdit,index, true);
        console.log(index, row);
      },
      handleUpdate(index,row) {
        console.log(row.date);
        console.log(this.context);
        var params = {
          'id':row.id,
          'date':row.date,
          'content':this.context,
          'isCompleted': false
        }
        row.content = this.context,
        console.log(row)
        this.showEdit[index] = false;
        this.$set(this.showEdit,index, false);
        axios.post('/api/api/Todo/EditTodo', params).then(async (response) => {
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
      handleCancel(index,row) {
        
        this.showEdit[index] = false;
        this.$set(this.showEdit,index, false);
      },
      handleDelete(index, row) {
        
        console.log(index, row);
        this.tableData.splice(index,1);
        var id = row.id;
        axios.post('/api/api/Todo/deleteTodo?id='+id).then(async (response) => {
          var res = response.data;
          console.log(res);
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
    }
  }

</script>