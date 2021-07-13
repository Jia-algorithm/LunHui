<template>
  <el-menu
    :default-active="current"
    class='header'
    background-color="#0f4c75"
    text-color="#ffffff"
    active-text-color="#fff"
    mode="horizontal"
    router>
    <el-menu-item index="standard" route="/standard" style="margin-top:30px;">
      <i class="el-icon-menu"></i>
      <span slot="title">评估标准</span>
    </el-menu-item>
    <el-menu-item index="project" route="/project" style="margin-top:30px;">
      <i class="el-icon-document"></i>
      <span slot="title">管理轮回</span>
    </el-menu-item>
    <el-menu-item index="document" route="/document" style="margin-top:30px;">
      <i class="el-icon-files"></i>
      <span slot="title">管理报表</span>
    </el-menu-item>
    <el-menu-item index="register" route="/register" style="margin-top:30px;">
      <i class="el-icon-user-solid"></i>
      <span slot="title">注册管理员</span>
    </el-menu-item>
    <el-menu-item index="home" route="/home" style="margin-top:30px;">
      <i class="el-icon-s-home"></i>
      <span slot="title">主页</span>
    </el-menu-item>
    <el-menu-item index="home" route="/home" style="float:right;border:0px;height:90px;">
      <h1>基于物种功德值评估的轮回管理系统</h1>
    </el-menu-item>
  </el-menu>
</template>

<style>
  .header {
    height:auto;
  }
</style>

<script>
export default {
  name: 'Manage',
  data () {
    return {
    }
  },

  computed: {
    current() {
      return this.$route.name
    }
  }
}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
</style>
