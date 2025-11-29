<template>
    <div class=" mt-2 relative">
        <div class="flex items-center gap-2 p-2">
            <router-link :to="{name:'adminHome'}">Home:</router-link>
            <VueIcon type="mdi" :path="mdiChevronRight"/>
            <p class="text-sm">Sản phẩm</p>
        </div>
        <div class="flex flex-col gap-5 bg-white shadow-lg mt-1 p-4">
            <div class="flex justify-between">
                <select name="" v-model="categoryId" class="p-1 outline-none border">
                    <option value="">Tất cả</option>
                    <option v-for="item in categorys" :key="item.categoryId" :value="item.categoryId">{{ item.name }}</option>
                </select>
                <div @click="handleAdd" class="text-blue-500 flex items-center gap-2 bg-blue-100 p-1 text-sm cursor-pointer">
                    <VueIcon type="mdi" :path="mdiCogOutline" size="15"/>
                    Thêm mặt hàng
                </div>
            </div>
            <table class="w-full text-xs md:text-sm">
                <thead class="border">
                    <tr class="bg-gray-400">
                        <th scope="col" class="border">Tên mặt hàng</th>
                        <th scope="col" class="border">Ảnh</th>
                        <th scope="col" class="border">Giá bán</th>
                        <th scope="col" class="border">Đánh giá</th>
                        <th scope="col" class="border">Đã bán</th>
                        <th scope="col" class="border">Hành động</th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="item in data" :key="item.productId" class="">
                        <td scope="col" class="border p-2">{{item.name}}</td>
                        <td scope="col" class="border text-center">
                            <div class="flex justify-center" v-viewer>
                                <img :src="item.image" class="w-10 h-10" alt="">
                            </div>
                            <viewer :src="item.image">
                                <img :src="src">
                            </viewer>
                        </td>
                        <td scope="col" class="border text-center">{{ item.price | numeral}}</td>
                        <td scope="col" class="border text-center">{{ item.averageRate }}</td>
                        <td scope="col" class="border text-center">{{ item.sell || 0}}</td>
                        <td scope="col" class="border text-center">
                            <div class="flex items-center justify-center text-white">
                                <div @click="handleUpdate(item.productId)"  class="p-1 bg-blue-500 rounded-lg">
                                    <VueIcon type="mdi" :path="mdiEyedropperVariant" class=" hover:cursor-pointer" size="15"/>
                                </div>
                                <router-link :to="{name:'detail',query:{msp:item.productId}}" class="p-1 bg-yellow-500 rounded-lg">
                                    <VueIcon type="mdi" :path="mdiEyeOutline"  class=" hover:cursor-pointer" size="15"/>
                                </router-link>
                                <div @click="handleDelete(item.productId)" class="p-1 bg-red-500 rounded-lg">
                                    <VueIcon type="mdi" :path="mdiTrashCanOutline"  class=" hover:cursor-pointer" size="15"/>
                                </div>
                            </div>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <AddProduct :open="add" @close="handleClose" :getproduct="getproduct"/>
        <EditProduct :open="update" :productIdToUpdate="productIdToUpdate" @close="handleClose" :getproduct="getproduct"/>
      <div class="pagination ma-3 pa-2">
        <button :disabled="page === 1" @click="changePage(page - 1)">{{"<"}}</button>
        <button
            v-for="p in totalPages"
            :key="p"
            @click="changePage(p)"
            :class="{ active: p === currentPage }"
        >
          {{ p }}
        </button>
        <button :disabled="page === totalPages" @click="changePage(page + 1)">{{">"}}</button>
      </div>
    </div>
</template>
<script>
import AddProduct from './AddProduct.vue'
import EditProduct from './EditProduct.vue'

import axios from 'axios'
import {mdiTrashCanOutline,mdiEyedropperVariant, mdiChevronRight, mdiCogOutline,mdiEyeOutline} from "@mdi/js"
export default {
    name:'ProductView',
    components:{AddProduct,EditProduct},
    props:['updateProduct'],
    data(){
        return{
            add:false,
            update:false,
            data:[],
            page:1,
            limit:15,
            productIdToUpdate:'',
            mdiTrashCanOutline,mdiEyedropperVariant,mdiChevronRight,mdiCogOutline,mdiEyeOutline,
            categorys:"",
            categoryId:"",
            totalPages:1,
            currentPage: 1,
        }
    },
    mounted(){
        this.getproduct()
        this.getcategory()
    },
    watch:{
        "categoryId":function(){
            if(this.categoryId==""){
                this.getproduct()
            }else{
                this.filter()
            }
        }
    },
    methods:{
        async getproduct(){
            try {
                const res=await axios.get(`/product/getByPage?page=${this.page}&limit=${this.limit}`)
                this.data=res.data.products
              this.totalPages = res.data.totalPages
              this.currentPage = res.data.currentPage
            } catch (err) {
                console.error(err)
            }
        },
      changePage(newPage) {
        if (newPage >= 1 && newPage <= this.totalPages) {
          this.page = newPage;
          this.getproduct();
        }
      },
        async handleDelete(id){
            const confirmed = confirm("Bạn có chắc chắn muốn xóa không?");
            if (confirmed) {
                try {
                    await axios.delete(`product/delete/${id}`)
                    this.getproduct()
                    this.$toast(`Xóa thành công`, {
                        position: "top-right",
                        timeout: 5000
                    });
                } catch (err) {
                    console.log(err)
                    this.$toast(`Xóa không thành công`, {
                    position: "top-right",
                    timeout: 5000
                });
                }
            }
        },
        handleLoad(){
            this.getproduct()
        },
        handleAdd(){
            this.add=true;
        },
        handleUpdate(id){
            this.update=true;
            this.productIdToUpdate = id;
        },
        handleClose(){
            this.add = false;
            this.update = false;
        },
        async getcategory(){
            try {
                const res=await axios.get(`/Category/getAll`)
                this.categorys=res.data
            } catch (err) {
                console.log(err)
            }
        },
        async filter(){
            try {
                const res=await axios.get(`/Product/getByCategory/${this.categoryId}`)
                this.data=res.data.products
            } catch (err) {
                console.log(err)
            }
        }
    }
}
</script>
<style scoped>
.pagination button {
  margin: 0 4px;
  padding: 6px 12px;
  border: 1px solid #ccc;
  border-radius: 4px;
}
.pagination button.active {
  font-weight: bold;
  background-color: #1976d2;
  color: white;
}

</style>