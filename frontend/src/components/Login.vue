<template>
    <div style="text-align:center;">
        <h1 class="title" style="margin-top:50px;align-self:ceter;">请先登录</h1>
        <div class="form">
            <el-form ref="form" :model="form" :rules="rules">
                <el-form-item label="管理员账号" prop="userName">
                    <el-input v-model="form.userName" />
                </el-form-item>
                <el-form-item label="密码" prop="password">
                    <el-input v-model="form.password" show-password />
                </el-form-item>
                <el-form-item>
                    <el-button type="primary" @click="submitForm('form')" plain>登 录</el-button>
                </el-form-item>
            </el-form>
        </div>
    </div>
</template>

<script>
export default {
    name: "Login",
    data() {
        return {
            form: {
                userName: '',
                password: ''
            },
            rules: {
                userName: [{required: true, message: '请输入账号', trigger: 'blur'}],
                password: [{required: true, message: '请输入密码', trigger: 'blur'}]
            }
        }
    },

    methods: {
        submitForm(formName) {
            this.$refs[formName].validate((valid) => {
                if (valid) {
                    var params = {
                        "name": this.form.userName,
                        "password": this.form.password,
                    }
                    axios.post('/back/login', this.param(params)).then((response) => {
                        var res = response.data
                        switch(res) {
                            case 'no username':
                                this.$message({
                                    message: '用户不存在！',
                                    type: 'warning'
                                })
                                break
                            case 'password error':
                                this.$message.error('密码错误！')
                                break
                            default:
                                this.$Global.iflogin = true;
                                this.$router.push({name: 'Home'});
                        }
                        
                    }).catch((error) => {
                        this.$message.error("出现未知错误，请重试！")
                    })
                } 
                else {
                    return false
                }
            })
        },

        param(json) {
            return Object.keys(json).map(function(key) {
                return encodeURIComponent(key) + '=' + encodeURIComponent(json[key])
            }).join('&')
        }
    }
}
</script>

<style scoped>
    .form {
        width: 500px;
        margin-left: auto;
        margin-right: auto;
        margin-top: 2%
    }

    .title {
        margin-left: auto;
        margin-right: auto;
        margin-top: 10%;
        padding-bottom: 60px;
        font-size: 40px;
        font-family: 'Times New Roman', Times, serif
    }
</style>
