<template>
    <div class="flex flex-col ">
        <div class="flex flex-wrap gap-2 items-center text-xs md:text-sm px-5 md:px-10 py-2 bg-gray-100 text-gray-500">
            <router-link :to="{name:'home'}">Trang chủ </router-link>/ <p>Trang khách hàng</p>
        </div>
        <div class="p-5 dark:bg-black dark:text-white">
            <div class="flex flex-col md:grid grid-cols-12 gap-5">
                <div class="col-span-3 md:col-span-4">
                    <div @click="showMenu" class="flex items-center justify-between cursor-pointer bg-black text-white dark:bg-white dark:text-black text-center p-2">
                        <div class="flex items-center gap-5">
                            <div class="flex flex-col text-xs gap-1">
                                <p>{{ $store.state.user.username ||  $store.state.user.email}}</p>
                                <router-link :to="{name:'account'}" class="flex item-center text-gray-300 dark:text-black"><VueIcon type="mdi" :path="mdiPencil" size="15"/> Sửa hồ sơ</router-link>
                            </div>
                        </div>
                        <VueIcon type="mdi" :path="menu ? mdiChevronUp :mdiChevronDown" class="text-white dark:text-black"/>
                    </div>
                    <ul v-if="menu" class="py-2 text-sm bg-gray-100 dark:bg-black dark:text-white border">
                        <li><router-link :to="{name:'account'}" class="flex hover:text-yellow-600 hover:bg-gray-700 p-2">Thông tin tài khoản</router-link></li>
                        <li><router-link :to="{name:'order'}" class="flex hover:text-yellow-600 hover:bg-gray-700 p-2">Đơn hàng của bạn</router-link></li>
                        <li><router-link :to="{name:'changePass'}" class="flex hover:text-yellow-600 hover:bg-gray-700 p-2">Đổi mật khẩu</router-link></li>
                    </ul>
                </div>
                <div class="col-span-9 md:col-span-8    ">
                    <router-view></router-view>
                </div>
            </div>
        </div>
    </div>
</template>
<script>
import { mdiChevronUp,mdiChevronDown, mdiPencil} from '@mdi/js';

export default {
    name:"ProfileView",
    data(){
        return{
            menu:true,
            mdiChevronDown,mdiChevronUp,mdiPencil
        }
    },
    methods:{
        showMenu(){
           this.menu=!this.menu 
        }
    }
}
</script>