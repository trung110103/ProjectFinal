<template>
    <div class=" mt-2 relative">
        <div class="flex items-center gap-2 p-2">
            <router-link :to="{name:'adminHome'}">Home:</router-link>
            <VueIcon type="mdi" :path="mdiChevronRight"/>
            <p class="text-sm">Đơn hàng</p>
        </div>
        <div class="bg-white shadow-lg mt-1">
            <table class="w-full text-xs">
                <thead class="border">
                    <tr class="bg-gray-400 text-center">
                        <td scope="col" class="p-2 border">Đơn hàng</td>
                        <td scope="col" class="p-2 border">Ngày</td>
                        <td scope="col" class="p-2 border">Đơn giá</td>
                        <td scope="col" class="p-2 border">Thanh toán</td>
                        <td scope="col" class="p-2 border">Vận chuyển</td>
                        <td scope="col" class="p-2 border">Hành động</td>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="item in data" :key="item.orderId" class="">
                        <td scope="col" @click="toggleDetails(item.orderId)" class="border text-center hover:text-blue-500 cursor-pointer">#{{ item.orderId }}</td>
                        <td scope="col" class="border text-center">{{ formatDate(item.createDate) }}</td>
                        <td scope="col" class="border text-center">{{ item.totalAmount | numeral }} đ</td>
                        <td scope="col" class="border text-center">{{ item.paymentStatus }}</td>
                        <td scope="col" class="border text-center">{{ item.shippingStatus }}</td>
                        <td class="flex gap-2 justify-center items-center border">
                            <div @click="Update(item.orderId,item.paymentStatus,item.shippingStatus)" :class="item.shippingStatus==='Đã giao' ? 'text-blue-500':'text-red-500'" class=" cursor-pointer"><VueIcon type="mdi" :path="mdiCheckCircle"/></div>
                            <div class="text-red-500" @click="handleDeleteOrder(item.orderId)"><VueIcon type="mdi" :path="mdiCloseCircle"/></div>
                        </td>
                    </tr>
                    <tr v-if="selectedOrderId === orderDetail.orderId">
                        <td colspan="6" class="p-4">
                            <div class="bg-gray-100 p-2">
                                <!-- Hiển thị thông tin chi tiết ở đây -->
                                <p><strong>Chi tiết đơn hàng #{{ orderDetail.orderId }}</strong></p>
                                <p>Khách hàng: {{ orderDetail.user.username }}</p>
                                <p>Địa chỉ: {{ orderDetail.address }}</p>
                                <p>Số điện thoại: {{ orderDetail.phone }}</p>
                                <!-- Thêm thông tin chi tiết khác -->
                            </div>
                        </td>
                    </tr>  
                </tbody>
            </table>
        </div>
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
import { mdiChevronRight,mdiCheckCircle, mdiCloseCircle } from '@mdi/js';
import axios from 'axios';
import moment from 'moment';

export default {
    name:'OrderView',
    props:['updateTotal'],
    data(){
        return{
            data:[],
            page:1,
            limit:15,
            mdiChevronRight,mdiCheckCircle,mdiCloseCircle,
            selectedOrderId:null,
            orderDetail:"",
          totalPages: 1,
          currentPage: 1
        }
    },
    mounted(){
        this.getOrder()
    },
    methods:{
        formatDate(date) {
            return moment(date).format('YYYY-MM-DD');
        },
        toggleDetails(orderId) {
            this.selectedOrderId = this.selectedOrderId === orderId ? null : orderId;
            this.getOrderDetail()
        },
      async getOrder() {
        try {
          const res = await axios.get(`/Order/GetAll`, {
            params: {
              page: this.page,
              limit: this.limit
            }
          });

          this.data = res.data.items;              // Danh sách đơn hàng
          this.totalPages = res.data.totalPages;   // Tổng số trang
          this.currentPage = res.data.currentPage; // Trang hiện tại

        } catch (err) {
          console.log('Lỗi khi lấy danh sách đơn hàng:', err);
        }
      },
      changePage(newPage) {
        if (newPage >= 1 && newPage <= this.totalPages) {
          this.page = newPage;
          this.getOrder()
        }
      },
      async getOrderDetail(){
            try {
                const res=await axios.get(`/Order/GetById/${this.selectedOrderId}`)
                this.orderDetail=res.data
            } catch (err) {
                console.log(err)
            }
        },
        async Update(id,paymentStatus,shippingStatus){
            try {
                let newStatusPayment;
                let newStatusShipping;
                if(paymentStatus==="Chưa thanh toán"){
                    newStatusPayment="Đã thanh toán"
                }else{
                    newStatusPayment="Đã thanh toán"
                }
                if(shippingStatus==="Chưa chuyển"){
                    newStatusShipping="Đã giao"
                }else{
                    newStatusShipping="Đã giao"
                }
                await axios.patch(`/Order/update/${id}`,{
                    paymentStatus:newStatusPayment,
                    shippingStatus:newStatusShipping

                })
                this.getOrder()
            } catch (err) {
                console.log(err)
            }
        },
      async handleDeleteOrder(id) {
        const confirmed = confirm("Bạn có chắc chắn muốn xóa đơn hàng này?");
        if (!confirmed) return;

        try {
          await axios.delete(`/Order/Delete/${id}`);
          this.getOrder(); // refresh lại danh sách đơn hàng
          this.$toast(`Xóa đơn hàng #${id} thành công`, {
            position: "top-right",
            timeout: 5000
          });
        } catch (err) {
          console.log(err);
          this.$toast(`Xóa đơn hàng không thành công`, {
            position: "top-right",
            timeout: 5000
          });
        }
      }
    },
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