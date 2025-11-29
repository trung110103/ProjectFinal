import Vue from 'vue';
const EventBusGlobal = new Vue();

const GlobalEventName = {
    // Sự kiện update giỏ hàng
    updateCart : 'updateCart',
   
}
export default EventBusGlobal;
export {GlobalEventName};