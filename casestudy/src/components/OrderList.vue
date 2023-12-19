<template>
  <div class="text-center">
    <q-avatar class="q-mt-md" size="90px" rounded>
      <img :src="`/img/skyr.jpg`" />
    </q-avatar>
    <div class="text-h4 q-mt-md text-accent text-bold">Order History</div>
    <div class="text-positive text-h6 q-mt-lg">
      {{ state.status }}
    </div>
  </div>
  <q-page>
    <q-card class="q-ma-sm text-center">
      <q-list highlight separator stripped-odd>
        <q-item class="text-h5 text-accent text-bold">
          <q-item-section>
            <q-item-label>Order #</q-item-label>
          </q-item-section>
          <q-item-section>Date</q-item-section>
        </q-item>
        <q-item
          clickable
          v-for="order in state.orders"
          :key="order.id"
          @click="selectOrder(order.id)"
        >
          <q-item-section class="col-6">
            {{ order.id }}
          </q-item-section>
          <q-item-section>
            {{ formatDate(order.orderDate) }}
          </q-item-section>
        </q-item>
      </q-list>
    </q-card>

    <q-dialog
      v-model="state.selectedAOrder"
      transition-show="rotate"
      transition-hide="rotate"
    >
      <q-card style="width: 95vw">
        <q-card-actions align="right">
          <q-btn flat label="X" color="primary" v-close-popup class="text-h5" />
        </q-card-actions>
        <q-item>
          <q-item-section class="text-h5 text-bold text-accent text-center">
            Order {{ state.orderDetails[0].orderId }}
          </q-item-section>
        </q-item>
        <q-item>
          <q-item-section class="text-h7 text-center q-mt-xs">
            {{ formatDate(state.orderDetails.orderDate) }}
          </q-item-section>
        </q-item>
        <q-card-section class="q-pa-none text-center">
          <q-avatar class="q-mt-xs" size="xl" square>
            <img :src="`/img/basket.png`" />
          </q-avatar>
        </q-card-section>

        <q-card-section class="q-mb-xs">
          <q-item class="text-accent q-ma-none">
            <q-item-section class="col-4 text-bold text-left">
              Name
            </q-item-section>
            <q-item-section class="col-4 text-bold text-center q-pr-lg">
              Quantities</q-item-section
            >
            <q-item-section class="col-4 text-bold text-right">
              Extended</q-item-section
            >
          </q-item>
          <q-list separator>
            <q-item class="text-accent text-bold" style="margin-top: -3vh">
              <q-item-section class="col-4"></q-item-section>
              <q-item-section class="col-1">S</q-item-section>
              <q-item-section class="col-1">O</q-item-section>
              <q-item-section class="col-1">B</q-item-section>
              <q-item-section class="col-3"></q-item-section>
            </q-item>

            <q-item v-for="order in state.orderDetails" :key="order.indexOf">
              <q-item-section class="text-left col-4">
                {{ order.productName }}
              </q-item-section>
              <q-item-section class="text-left col-1">
                {{ order.qtySold }}
              </q-item-section>
              <q-item-section class="text-left col-1">
                {{ order.qtyOrdered }}
              </q-item-section>
              <q-item-section class="text-left col-1">
                {{ order.qtyBackOrdered }}
              </q-item-section>
              <q-item-section class="text-right col-3.5">
                {{ formatCurrency(order.sellingPrice * order.qtySold) }}
              </q-item-section>
            </q-item>
            <q-item>
              <q-item-section class="col-8 text-right text-bold"
                >Sub:</q-item-section
              >
              <q-item-section class="text-right col-4 q-pr-md"
                >{{ formatCurrency(state.orderDetails[0].orderAmount) }}
              </q-item-section>
            </q-item>
            <q-item>
              <q-item-section class="col-8 text-right text-bold">
                Tax(13%):
              </q-item-section>
              <q-item-section class="text-right col-4 q-pr-md">
                {{ formatCurrency(state.orderDetails[0].orderAmount * 0.13) }}
              </q-item-section>
            </q-item>
            <q-item>
              <q-item-section class="col-8 text-right text-bold text-primary"
                >Total:</q-item-section
              >
              <q-item-section
                class="text-right col-4 q-pr-md text-bold text-primary"
              >
                {{ formatCurrency(state.orderDetails[0].orderAmount * 1.13) }}
              </q-item-section>
            </q-item>
          </q-list>
        </q-card-section>

        <q-card-section
          class="text-center text-positive text-h6 q-mb-xs q-mt-lg q-pa-none"
        >
          {{ state.dialogStatus }}
        </q-card-section>
      </q-card>
    </q-dialog>
  </q-page>
</template>
<script>
import { reactive, onMounted } from "vue";
import { fetcher } from "../utils/apiutil";
import { formatDate } from "../utils/formatutils";
import { formatCurrency } from "../utils/formatutils";
export default {
  setup() {
    let state = reactive({
      status: "",
      dialogStatus: "",
      selectedAOrder: false,
      orders: [],
      orderDetails: [],
      orderSelected: {},
      total: 0,
    });
    onMounted(() => {
      loadOrders();
    });
    const loadOrders = async () => {
      try {
        let customer = JSON.parse(sessionStorage.getItem("customer"));
        const payload = await fetcher(`order/${customer.email}`);
        if (payload.error === undefined) {
          state.orders = payload;
          state.status = `loaded ${state.orders.length} orders`;
        } else {
          state.status = payload.error;
        }
      } catch (err) {
        console.log(err);
        state.status = `Error has occured: ${err.message}`;
      }
    };
    const selectOrder = async (orderId) => {
      try {
        let customer = JSON.parse(sessionStorage.getItem("customer"));
        const payload = await fetcher(`order/${orderId}/${customer.email}`);
        state.orderDetails = payload;
        state.dialogStatus = `details for order ${orderId}`;
        state.selectedAOrder = true;
        state.orderSelected = state.orders.find(
          (order) => order.id === orderId
        );
      } catch (err) {
        console.log(err);
        state.status = `Error has occured: ${err.message}`;
      }
    };
    return {
      state,
      formatDate,
      selectOrder,
      formatCurrency,
    };
  },
};
</script>
