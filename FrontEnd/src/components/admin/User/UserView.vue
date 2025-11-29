<template>
    <div class=" mt-2 relative">
        <div class="flex items-center gap-2 p-2">
            <router-link :to="{name:'adminHome'}">Home:</router-link>
            <VueIcon type="mdi" :path="mdiChevronRight"/>
            <p class="text-sm">Khách hàng</p>
        </div>
        <div class="flex bg-white shadow-lg mt-1 p-4">
            <table class="w-full text-xs">
                <thead class="border">
                    <tr class="bg-gray-400 text-center">
                        <td scope="col" class="p-2 border">Id</td>
                        <td scope="col" class="p-2 border">Usname</td>
                        <td scope="col" class="p-2 border">Phone</td>
                        <td scope="col" class="p-2 border">Email</td>
                        <td scope="col" class="p-2 border">Hành động</td>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="item in data" :key="item.id" class="">
                        <td scope="col" class="border text-center hover:text-blue-500 cursor-pointer">#{{ item.id }}</td>
                        <td scope="col" class="border text-center">{{ item.username }}</td>
                        <td scope="col" class="border text-center">{{ item.phone }}</td>
                        <td scope="col" class="border text-center">{{ item.email }}</td>
                        <td class="flex gap-2 justify-center items-center border">
                            <div class="text-red-500" @click="deleteUser(item)"><VueIcon type="mdi" :path="mdiCloseCircle" /></div>
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

export default {
    name:'OrderView',
    props:['updateTotal'],
    data(){
        return{
            data:[],
            page:1,
            limit:5,
            mdiChevronRight,mdiCheckCircle,mdiCloseCircle,
          totalPages:1,
          currentPage: 1,
        }
    },
    mounted(){
        this.getUser()
    },
    methods:{
      async getUser() {
        try {
          const res = await axios.get(`/User/getByPage`, {
            params: {
              page: this.page,
              limit: this.limit
            }
          });
          this.data = res.data.items || res.data;  // tùy backend trả về mảng hoặc object có items
          this.totalPages = res.data.totalPages || 1;
          this.currentPage = this.page;
        } catch (err) {
          console.log(err);
        }
      },
      changePage(newPage) {
        if (newPage >= 1 && newPage <= this.totalPages) {
          this.page = newPage;
          this.getUser();
        }
      },
      async deleteUser(user) {
        if (user.role === "Admin") {
          this.$toast.error("Không thể xóa tài khoản có quyền Admin", {
            position: "top-right",
            timeout: 3000
          });
          return;
        }
        const confirmed = confirm("Bạn có chắc chắn muốn xóa tài khoản này không?");
        if (!confirmed) return;

        try {
          await axios.delete(`/User/delete/${user.id}`);
          this.$toast.success("Xóa tài khoản thành công", {
            position: "top-right",
            timeout: 3000
          });
          await this.getUser(); // Tải lại danh sách user
        } catch (error) {
          console.error(error);
          this.$toast.error("Xóa tài khoản thất bại", {
            position: "top-right",
            timeout: 3000
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