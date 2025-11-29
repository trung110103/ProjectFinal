<template>
    <footer class="flex flex-col w-full bg-gray-100 dark:bg-black dark:text-white border">
        <p class="py-2 bg-black text-white text-center dark:text-black dark:bg-white">Copyright &copy; 2025 Nội Thất Perfect Home.</p>
    </footer>
</template>
<script>
import {mdiMapMarker,mdiPhoneClassic,mdiEmail,mdiFacebook,mdiYoutube,mdiInstagram,mdiMusicCircle} from "@mdi/js"
export default {
    name:"FooterView",
    data(){
        return{
            mdiMapMarker,mdiPhoneClassic,mdiEmail,mdiFacebook,mdiYoutube,mdiInstagram,mdiMusicCircle
        }
    }
}
</script>