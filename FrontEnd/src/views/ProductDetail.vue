<template>
    <div class="flex flex-col ">
        <div class="flex flex-wrap gap-2 items-center text-xs md:text-sm px-5 md:px-10 py-2 bg-gray-100 text-gray-500">
            <router-link :to="{name:'home'}">Trang chủ </router-link>/
          <router-link :to="{name:'category',params:{id:product.category.categoryId}}"> {{ product.category.name }}</router-link>/<p class="line-clamp-1">{{ product.name }}</p>
        </div>
        <div class="flex flex-col gap-5 p-5 md:p-10 dark:bg-black dark:text-white">
            <div class="grid grid-cols-1 md:grid-cols-2 gap-5">
                <div class="relative">
                    <div class="w-full flex flex-col gap-2 sticky top-1">
                    <img :src="selectedImage ? selectedImage : product.image" alt="" class="max-w-full h-[40vh]">
                    <div class="flex flex-wrap gap-3">
                        <img v-for="image in product.productImages" @click="selectedImage=image.imageUrl" :key="image.productImageId" :src="image.imageUrl" alt="" class="w-14 h-14 border-2">
                    </div>
                </div>
                </div>
                <div class="relative flex flex-col">
                    <h1 class="text-xl font-bold">{{ product.name }}</h1>
                    <div class="flex flex-col">
                        <div class="flex justify-between border-b pb-5">
                            <div class="flex items-center">
                                <star-rating v-model="listComments.averageRate"
                                :star-size="20" 
                                :show-rating="false"
                                :increment="0.1" 
                                :read-only="true" 
                                :fixed-points="1"/>                          
                                ({{ listComments.count }})
                            </div>
                            <div class="flex gap-5 items-center">
                                <p>Đã bán: {{ product.sell }}</p>
                            </div>
                        </div>
                        <div class="flex items-center gap-3 py-2 text-center border-b">
                            <div v-if="discounted" class="w-16 py-1 font-bold bg-gray-100 text-red-500">-{{discounted}}%</div>
                            <p class="font-bold text-red-500">{{discountedPrice ?? price | numeral}}₫</p>
                            <s v-if="discountedPrice" class="">{{price | numeral}} <u>đ</u></s>
                        </div>
                        <div v-if="colors.length>0" class="flex items-center flex-wrap gap-3 py-2 text-center border-b text-sm">
                            <div v-for="item in colors" :key="item.productColorId" @click="selectedColor=item.color.name" :class="{'border-2 border-black': selectedColor === item.color.name}" class="cursor-pointer w-10 h-10 rounded-full border flex justify-center items-center" :style="{ backgroundColor: item.color.code }"></div>
                        </div>
                        <div v-if="sizes.length>0" class="flex items-center flex-wrap gap-3 py-2 text-center border-b text-sm">
                            <div v-for="item in sizes" :key="item.productSizeId" @click="handleSelectedSize(item.size.name)" :class="{'bg-black text-white': selectedSize === item.size.name}" class="cursor-pointer w-20 h-10 border flex justify-center items-center">{{  item.size.name }}</div>
                        </div>
                        <div class="flex flex-col gap-3">
                            <div class="flex border w-32 h-8 font-bold text-xl text-center items-center">
                                <div @click="quantity>0 && quantity--" class="bg-gray-100 w-8 cursor-pointer dark:bg-black dark:text-white">-</div>
                                <input type="text" v-model="quantity" class="w-16 text-center outline-none dark:bg-black dark:text-white" value="1">
                                <div @click="quantity<10 && quantity++" class="bg-gray-100 w-8 cursor-pointer dark:bg-black dark:text-white">+</div>
                            </div>
                          <p class="text-sm text-gray-500 dark:text-white ">{{ product.description }}</p>

                            <button @click="AddToCart" class="font-bold text-white text-xl py-2 uppercase bg-blue-800">Thêm vào giỏ</button>
                        </div>
                    </div>
                </div>
            </div>
            <div class="flex flex-col w-full border">
                <div class="w-full h-auto bg-gray-100 dark:bg-black dark:text-white border flex items-center">
                    <div @click="CurrentTab = 'tab2'" :class="{'bg-gray-300 dark:text-black ': CurrentTab === 'tab2'}" class="p-2 hover:bg-gray-200 hover:dark:text-black  cursor-pointer">Đánh giá ({{ listComments.count }})</div>
                </div>
                <div v-if="CurrentTab === 'tab2'"  class="flex w-full p-2">
                    <div class="flex flex-col gap-2 w-full">
                        <div class="flex items-center">
                            <star-rating v-model="listComments.averageRate"
                            :star-size="20" 
                            :show-rating="false"
                            :increment="0.1" 
                            :read-only="true" 
                            :fixed-points="1"/>                          
                            ({{ listComments.averageRate }})
                        </div>
                        <div class="flex flex-col gap-2">
                            <div class="flex items-center gap-5">
                                <h1 class="font-bold">Viết đánh giá</h1>
                                <select name="" v-model="commentRate" class="border-2 w-40 p-1 dark:text-black ">
                                    <option value="1">1sao</option>
                                    <option value="2">2sao</option>
                                    <option value="3">3sao</option>
                                    <option value="4">4sao</option>
                                    <option value="5">5sao</option>
                                </select>
                            </div>
                            <textarea v-model="commentText" class="border outline-none p-2" placeholder="Viết gì đó ..."/>
                            <div class="flex bottom-2 overflow-x-auto justify-between">
                                <div class=" flex flex-wrap">
                                    <label v-if="!commentImage" for="file" class="w-10 h-10 md:w-20 md:h-20 border-2 flex justify-center items-center hover:cursor-pointer"><VueIcon type="mdi" :path="mdiCamera" class=""/></label>
                                    <div v-else class="flex">
                                        <div class="relative">
                                            <img :src="commentImage" alt="" class="w-10 h-10 md:w-20 md:h-20 border-2 text-2xl block hover:cursor-pointer">
                                            <button type="button" @click="remove()"><VueIcon type="mdi" :path="mdiClose" class="absolute top-0 right-0 p-1 hover:cursor-pointer border bg-slate-100 rounded-[50%]"/></button>
                                        </div>
                                    </div>
                                </div>
                                <input type="file" hidden id="file" name="image" @change="handleImageChange">
                                <button @click="Comment" type="button" class="p-2 border bg-blue-500 text-white self-center rounded-sm">Send</button>
                            </div>
                        </div>
                        <div class=' mt-3 border-4 border-black dark:border-white  h-[58vh] p-2 relative'>
                            <h1 class="text-sm md:text-base">Bình luận ({{ listComments.count }})</h1>
                            <div class="h-[50vh] overflow-y-auto">
                                <div v-for="item in listComments.comments" :key="item.commentId" class='border-b'>
                                <div class='flex mt-3 items-center gap-3'>
                                    <div class="border w-10 h-10 rounded-[50%]" >
                                        <img src="/assets/image/New/new1.webp" alt="" class='rounded-[50%]'/> 
                                    </div>
                                    <div class=' flex flex-col'>
                                        <p class="text-sm md:text-md">{{ item.user.email }}</p>
                                        <div class="flex">
                                            <VueIcon v-for="i in item.rate" :key="i" type="mdi" :path="mdiStar" size="15" class=" text-yellow-300"/>
                                        </div>
                                        <p class="text-[10px]">{{ item.createdDate }}</p>
                                    </div>
                                </div>
                                <div class='my-2 px-2'>
                                    <span class="text-xs sm:text-base">{{ item.commentText }}</span>
                                    <div v-if="item.imageUrl" class="flex gap-3">
                                        <img :src="item.imageUrl" class="w-10 h-10 sm:w-20 sm:h-20">
                                    </div>
                                </div>
                            </div>
                            </div>
                            <button class="w-full text-center bg-[#271511] text-white leading-8">Xem thêm bình luận (10)</button>
                        </div>
                    </div>
                </div>
            </div>
            <div class="w-full flex flex-col gap-5">
                <h1 class="text-xl md:text-3xl text-center uppercase"><u>Sản phẩm liên quan</u></h1>
                <div class="grid grid-cols-2 lg:grid-cols-4 gap-2">
                    <ProductItem v-for="item in relatedProducts.products" :key="item.productId" :item="item"/>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import { mdiFacebook, mdiStar,mdiCheck, mdiClose, mdiCamera  } from '@mdi/js';
import ProductItem from '@/components/productItem/ProductItem.vue';
import axios from 'axios';
import {imgDb} from '../firebase'
import { ref, uploadBytesResumable,getDownloadURL } from 'firebase/storage'
import StarRating from 'vue-star-rating';
export default {
  name:"ProductDetail",
  props:["getCart"],
  components:{ProductItem,StarRating},
  data(){
    return{
      mdiStar,mdiFacebook,mdiCheck,mdiClose,mdiCamera,
      image:[],
      CurrentTab:'tab1',
      product:"",
      productId:this.$route.query.masp,
      relatedProducts:"",
      colors:"",
      sizes:"",
      selectedColor:"",
      selectedSize:"",
      price:0,
      discountedPrice:0,
      discounted:"",
      quantity:1,
      facebookShareUrl: '',
      selectedImage:"",
      commentText:"",
      commentRate:5,
      commentImage:"",
      listComments:""
    }
  },
  mounted(){
    this.getProduct()
    this.generateFacebookShareUrl();
    this.getComment()
  },
  watch:{
    "$route.query.masp":function(newValue){
      this.productId=newValue
      this.getProduct()
    }
  },
  methods:{
    generateFacebookShareUrl() {
      const productUrl = window.location.href;
      this.facebookShareUrl = `https://www.facebook.com/sharer/sharer.php?u=${encodeURIComponent(productUrl)}`;
    },
    remove() {
      this.remove="";

    },
    handleSelectedSize(name){
      this.selectedSize=name
      this.changeTotal()
    },
    changeTotal(){
      const size = this.sizes.filter(s => s.size.name === this.selectedSize);
      this.price=size[0].price
      this.discounted=size[0].discounted
      this.discountedPrice=size[0].discountedPrice
    },
    async getProduct(){
      try {
        const res=await axios.get(`/Product/getOne/${this.productId}`)
        this.product=res.data
        this.price=res.data.price
        this.discounted=res.data.discounted
        this.discountedPrice=res.data.discountedPrice
        this.colors=res.data.productColors
        this.sizes=res.data.productSizes
        this.selectedSize=res.data.productSizes[0]?.size.name
        this.getRelatedProducts()
        if(this.sizes.length>0){
          this.changeTotal()
        }
      } catch (err) {
        console.log(err)
      }
    },
    async getRelatedProducts(){
      try {
        const res=await axios.get(`/Product/getByCategory/${this.product.category.categoryId}`)
        this.relatedProducts=res.data
      } catch (err) {
        console.log(err)
      }
    },
    async AddToCart(){
      try {
        await axios.post("/Cart/addCart",{
          productId:this.product.productId,
          quantity:this.quantity,
          totalAmount:this.discountedPrice ? this.discountedPrice * this.quantity : this.price * this.quantity
        })
        this.getCart()
        this.$toast.success(`Đã thêm vào giỏ hàng`, {
          position: "top-right",
          timeout: 5000
        });
      } catch (err) {
        this.$toast.error(`Thất bại`, {
          position: "top-right",
          timeout: 5000
        });
        console.log(err)
      }
    },
    async handleImageChange(event) {
      const file = event.target.files[0];
      if (!file) return;
      try {
        const imgRef = ref(imgDb, `/comment/product-${this.productId}/${file.name}`);
        const uploadTask = uploadBytesResumable(imgRef, file);
        uploadTask.on(
            "state_changed",
            (snapshot) => {
              this.percent = Math.round((snapshot.bytesTransferred / snapshot.totalBytes) * 100);
            },
            (err) => console.log(err),
            async () => {
              const url = await getDownloadURL(uploadTask.snapshot.ref);
              this.commentImage = url;
            }
        );
      } catch (err) {
        console.error("Error uploading image:", err);
      }
    },
    async Comment(){
      try {
        await axios.post(`/Comment/create/${this.productId}`,{
          commentText:this.commentText,
          imageUrl:this.commentImage,
          rate:this.commentRate
        })
        this.getComment()
        this.$toast.success(`Tải bình luận thành công`, {
          position: "top-right",
          timeout: 5000
        });
      } catch (err) {
        if(err.response && err.response.status===401){
          this.$toast.error(`Bạn cần đăng nhập để bình luận`, {
            position: "top-right",
            timeout: 5000
          });
        }else{
          console.log(err)
        }
      }
    },
    async getComment(){
      try {
        const res=await axios.get(`/Comment/getByProduct/${this.productId}`)
        this.listComments=res.data
      } catch (err) {
        console.log(err)
      }
    }
  }
}
</script>