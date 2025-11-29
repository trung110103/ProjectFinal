<template>
    <div id="register-panel" class="w-full h-full md:min-w-[23rem] flex flex-col gap-5 p-5 dark:bg-black border dark:text-white">
            <h1 class="text-xl uppercase">{{ $t('register.header') }}</h1>
            <input type="text" :placeholder="$t('register.inputs.email_placeholder')" v-model="email" class="p-2 outline-none border dark:text-black">
      <input type="text" :placeholder="$t('register.inputs.user_placeholder')" v-model="userName" class="p-2 outline-none border dark:text-black">
      <input type="password" :placeholder="$t('register.inputs.pass_placeholder')" v-model="password" class="p-2 outline-none border dark:text-black">
      <input type="password" :placeholder="$t('register.inputs.re_pass_placeholder')" v-model="confirmPassword" class="p-2 outline-none border dark:text-black">
            <p class="text-gray-400">{{ $t('register.return_to_login') }} <span @click="openLoginPanel" class="text-blue-500 cursor-pointer">{{ $t('register.back_to_login') }}</span></p>
            <button @click="Register" class="w-full flex justify-center items-center gap-2 p-2 text-white bg-black dark:bg-white dark:text-black">
                <VueIcon v-if="$store.state.loading"  type="mdi" :path="mdiRotateRight" class="animate-spin text-white dark:text-black"/>
                {{ $t('register.buttons.submit') }}</button>
                <p v-if="message" class="text-red-500">{{ message }}</p>
    </div>
</template>

<script>
import axios from "axios"
import {mdiRotateRight} from '@mdi/js'
export default {
    name:"RegisterPanel",
    data(){
        return{
            mdiRotateRight,
          email: '',
          password: '',
          confirmPassword: '',
          message: '',
          userName: '',
        }
    },
    mounted(){
        console.log(this.$store.state.user)
    },
    methods:{
      async Register() {
        this.$store.commit('LOGIN_START')
        if (this.password !== this.confirmPassword) {
          this.message = "Mật khẩu và Nhập lại mật khẩu không khớp";
          return;
        }
        try {
          const res = await axios.post("/User/register", {
            email: this.email,
            userName: this.userName,
            password: this.password,
            confirmPassword: this.confirmPassword
          })

          this.message = res.data
          this.$store.commit('LOGIN_SUCCESS')


        } catch (err) {
          if (err.response) {
            this.message = err.response.data
          } else {
            this.message = "Lỗi kết nối đến server"
          }
          this.$store.commit('LOGIN_FAILURE')
        }
      },
      openLoginPanel(){
            this.$emit('openLoginPanel','login')
        }
    }
}
</script>