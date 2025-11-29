<template>
    <div  class="flex flex-col min-h-screen dark:bg-black dark:text-white">
        <div class="flex flex-col p-5 md:p-10 gap-5">
            <!-- new -->
            <div class="flex flex-col gap-2">
                <div class="flex justify-between items-center">
                    <h1 class="text-3xl font-medium"> sản phẩm Mới nhất</h1>
                    <router-link :to="{name:'productall'}" class="text-red-500">Xem tất cả</router-link>
                </div>
                <div class="grid grid-cols-2 lg:grid-cols-4 gap-2">
                    <ProductItem v-for="item in productNew" :key="item.productId" :item="item" :quantity='1' @added-to-cart="getCart"/>
                </div>
            </div>
            <!-- Sale -->
            <div class="flex flex-col gap-2">
                <div class="flex justify-between items-center">
                    <h1 class="text-3xl font-medium">Ưu đãi</h1>
                    <router-link :to="{name:'productall'}" class="text-red-500">Xem tất cả</router-link>
                </div>
                <div class="grid grid-cols-2 lg:grid-cols-4 gap-2">
                    <ProductItem v-for="item in productDiscount" :key="item.productId" :item="item" :quantity='1' @added-to-cart="getCart"/>
                </div>
            </div>
            <!-- Bán chạy -->
            <div class="flex flex-col gap-2">
                <div class="flex justify-between items-center">
                    <h1 class="text-3xl font-medium">Bán chạy</h1>
                    <router-link :to="{name:'productall'}" class="text-red-500">Xem tất cả</router-link>
                </div>
                <div class="grid grid-cols-2 lg:grid-cols-4 gap-2">
                    <ProductItem v-for="item in productDiscount" :key="item.productId" :item="item" :quantity='1' @added-to-cart="getCart"/>
                </div>
            </div>
            <!-- blog -->
            <!-- banner -->

        </div>
    </div>
</template>

<script>
import {mdiChevronRight,mdiChevronLeft,mdiStar} from '@mdi/js'
import ProductItem from '@/components/productItem/ProductItem.vue';
import axios from 'axios';


export default {
    name:"HomeView",
    components:{ProductItem},
    data(){
        return{
            currentIndex:0,
            indexBlog:0,
            slidesCount: 4,
            mdiChevronRight,mdiChevronLeft,mdiStar,
            windowWidth: window.innerWidth,
            productNew:"",
            productDiscount:"",
            productSell:"",
            commentView:"",
          countCart:0,
          carts:"",
          totalAmount:0,
        }
    },
    methods: {
        nextSlide() {
            this.currentIndex = (this.currentIndex + 1) % this.slidesCount;
        },
        prevSlide() {
            this.currentIndex = (this.currentIndex - 1 + this.slidesCount) % this.slidesCount;
        },
        startAutoSlide(){
            setInterval(()=>{
                this.nextSlide()
                this.nextBlog()
            },3000)
        },
        nextBlog(){
            if(this.windowWidth>=768 && this.indexBlog< this.commentView.length-4 && this.commentView.length>4){
                this.indexBlog++
            }
            else if(this.windowWidth<768 && this.indexBlog< this.commentView.length-2 && this.commentView.length>2){
                this.indexBlog++
            }
            else{
                this.indexBlog=0
            }
        },
        handleResize() {
            this.windowWidth = window.innerWidth;
        },
        async getProductNew(){
            try {
                
                  const res=await axios.get("/Product/getNew")
                // const res = [{ abc : "123"},{ abc : "456"} ];
                // res.forEach(item=> {
                //     var a = 1;
                    
                // })
                 this.productNew=res.data


            } catch (err) {
                console.log(err)
            }
        },
        async getProductDiscount(){
            try {
                 const res=await axios.get("/Product/getDiscount")
                 this.productDiscount=res.data

            } catch (err) {
                console.log(err)

            }
        },
        async getProductSell(){
            try {
                const res=await axios.get("/Product/getSell")
                debugger
                this.productSell=res.data
            } catch (err) {
                console.log(err)
            }
        },
        async getCommentView(){
            try {
                const res=await axios.get("/Comment/GetCommentBest")
                debugger
                this.commentView=res.data
            } catch (err) {
                console.log(err)
            }
        },
      async getCart(){
        try {
            debugger
          const res=await axios.get('/Cart/getCart')
          this.countCart=res.data.carts.length
          this.carts=res.data.carts
          this.totalAmount=res.data.totalAmount
        } catch (err) {
          if (err.response && err.response.status === 401) {
            this.countCart = 0;
          }
          console.error(err)
        }
      }
    },
    mounted(){
        debugger
        this.startAutoSlide()
        window.addEventListener('resize', this.handleResize);
        this.getProductNew()
        this.getProductDiscount()
        this.getCommentView()
    },
  beforeDestroy() {
        window.removeEventListener('resize', this.handleResize);
    }
}
</script>