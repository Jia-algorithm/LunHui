<template>
    <div style="text-align:center;">
        <h1 class="title" style="margin-top:50px;">请谨慎保管/分发注册的账号</h1>
        <div class="form">
            <el-form ref="form" :model="form" :rules="rules">
                <el-form-item label="用户名" prop="userName">
                    <el-input placeholder="请使用大小写字母、数字或其他字符串" v-model="form.userName" />
                </el-form-item>
                <el-form-item label="请输入密码" prop="password">
                    <el-input v-model="form.password" show-password />
                </el-form-item>
                <el-form-item label="请再次输入密码" prop="confirm">
                    <el-input v-model="form.confirm" show-password />
                </el-form-item>
                <el-form-item label="身份验证码" prop="vericode">
                    <el-input v-model="form.vericode" show-password />
                </el-form-item>
                <el-form-item>
                    <el-button type="primary" @click="submitForm('form')">注 册</el-button>
                </el-form-item>
            </el-form>
        </div>
    </div>
</template>

<script>
export default {
    name: "Register",
    data() {
        var validatePass1 = (rule, value, callback) => {
            if (value === '') {
                callback(new Error('请输入密码'))
            } else if (value.length < 6) {
                callback(new Error('密码长度必须大于6位'))
            } else {
                if (this.form.checkPass !== '') {
                    this.$refs.form.validateField('checkPass')
                }
                callback()
            }
        }
        var validatePass2 = (rule, value, callback) => {
            if (value === '') {
                callback(new Error('请再次输入密码'))
            } else if (value !== this.form.password) {
                callback(new Error('两次输入密码不一致!'))
            } else {
                callback()
            }
        }
        return {
            form: {
                userName: '',
                password: '',
                confirm: '',
                vericode:'',
            },
            rules: {
                userName: [{required: true, message: '请输入用户名', trigger: 'blur'}],
                password: [{validator: validatePass1, trigger: 'blur'}],
                confirm: [{validator: validatePass2, trigger: 'blur'}],
                userName: [{required: true, message: '请输入正确的身份验证码', trigger: 'blur'}]
            }
        }
    },

    methods: {
        submitForm(formName) {
            var params = {
                "name": this.form.userName,
                "password": this.form.password,
                "vericode": this.form.vericode,
            }
            this.$refs[formName].validate((valid) => {
                if (valid) {
                    axios.post('/back/register', params).then((response) => {
                        var res = response.data
                        switch(res) {
                            case 'userexist':
                                this.$message({
                                    message: '用户名已存在！',
                                    type: 'warning'
                                })
                                break
                            case 'fail':
                                this.$message.error('注册失败，请重试')
                                break
                            case 'wrong code':
                                this.$message.error('身份验证码错误！')
                            default:
                                this.$message({
                                    message: '注册成功！',
                                    type: 'success'
                                });
                                this.$router.push({name: 'Login'});
                        }
                        this.$refs[formName].resetFields()
                    }).catch((error) => {
                        this.$notify.error({title: '失败', message: '出现未知错误，请重试'})
                        this.$refs[formName].resetFields()
                    })
                } else {
                    return false
                }
            });
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
        font-size: 36px;
        font-family: 'Times New Roman', Times, serif
    }
</style>