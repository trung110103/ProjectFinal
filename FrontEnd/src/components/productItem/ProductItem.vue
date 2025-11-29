<template>
    <div class="flex flex-col gap-2 border">
        <div class="relative w-full">
            <router-link :to="{name:'productdetail',query:{masp: item.productId}}" class="w-full h-[40vh]"><img :src="item.image" :alt="item.name" class="w-full h-[40vh] object-contain"></router-link>
            <div v-if="item.discounted" class="absolute px-2 top-0 left-0 bg-red-500 text-white">-{{item.discounted}}%</div>
            <!-- <div class="absolute px-2 top-0 right-0 bg-orange-500 text-white">NEW</div> -->
        </div>
        <div class="px-2 dark:bg-black dark:text-white">
          <p class="uppercase text-[12px] sm:text-sm font-semibold truncate w-full whitespace-nowrap overflow-hidden">{{ item.name }}</p>
          <p class="text-red-500 text-[12px] sm:text-base">{{item.discountedPrice ? item.discountedPrice : item.price | numeral}}<u>đ</u>&nbsp;<s v-if="item.discountedPrice" class="text-gray-400">{{item.price | numeral}}<u>đ</u></s></p>
            <div class="flex justify-between">
                <div class="flex items-center py-1">
                    <star-rating v-model="rate"
                            :star-size="15"
                            :show-rating="false"
                            :increment="0.1"
                            :read-only="true"
                            :fixed-points="1"/>
                            ({{ item.commentCount }})
                </div>
                <p class="text-[12px] sm:text-base">Đã bán {{ item.sell }}</p>
            </div>
        </div>
      <button @click="AddToCart" class="font-bold text-white text-xl py-2 uppercase bg-blue-800">Thêm vào giỏ</button>
    </div>
</template>

<script>
import {mdiStar, mdiHeartOutline} from '@mdi/js'
import StarRating from 'vue-star-rating';
import axios from "axios";
import EventBusGlobal, {
  GlobalEventName,
} from "../../common/eventBusGlobal.js";
export default {
    name:"ProductItem",
    props:["item",'getCart', 'quantity'],
    components:{StarRating},
    data(){
        return{
            mdiStar, mdiHeartOutline,
            rate:this.item.averageRate || 0,
        }
    },
  methods: {
    async AddToCart() {
      const qty = this.quantity || 1;
      const unitPrice = this.item.discountedPrice || this.item.price;
      const totalAmount = unitPrice * qty;

      try {
        await axios.post("/Cart/addCart", {
          productId: this.item.productId,
          quantity: qty,
          totalAmount: totalAmount
        });

        this.$emit('added-to-cart');
        debugger
        EventBusGlobal.$emit(GlobalEventName.updateCart);
        this.$toast.success("Đã thêm vào giỏ hàng", {
          position: "top-right",
          timeout: 5000
        });

       
      } catch (err) {
        if (err.response?.status === 401) {
          this.$toast.error("Bạn cần đăng nhập để thêm vào giỏ hàng", {
            position: "top-right",
            timeout: 5000
          });
        } else {
          this.$toast.error("Thêm vào giỏ hàng thất bại", {
            position: "top-right",
            timeout: 5000
          });
        }
        console.error(err);
      }
    }
  },
}
</script>