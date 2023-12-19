<template>
  <div class="text-center">
    <q-avatar class="q-mb-md" size="80px" rounded>
      <img :src="`/img/skyr.jpg`" />
    </q-avatar>
    <div class="text-h5 q-mt-xs">Basket Contents</div>
    <q-avatar class="q-mt-md" size="xl" square>
      <img :src="`/img/basket.png`" />
    </q-avatar>
    <div class="text-h6 text-positive">{{ state.status }}</div>
  </div>

  <div
    v-if="state.basket.length > 0"
    style="width: 90vw; margin-left: 5vw; margin-top: 1vh"
  >
    <q-layout
      view="lHh Lpr lFf"
      container
      style="height: 60vh"
      class="shadow-2 rounded-borders"
    >
      <q-page-container>
        <q-page>
          <q-card class="q-ma-md">
            <q-item>
              <q-item-section class="col-3 text-bold text-left">
                Name
              </q-item-section>
              <q-item-section class="col-3 text-bold text-left">
                Qty
              </q-item-section>
              <q-item-section class="col-3 text-bold text-left">
                MSRP
              </q-item-section>
              <q-item-section class="col-3 text-bold text-left">
                Extended
              </q-item-section>
            </q-item>
            <q-list separator>
              <q-item clickable v-for="item in state.basket" :key="item.id">
                <q-item-section class="text-right col-3"
                  >{{ item.item.productName }}
                </q-item-section>
                <q-item-section class="text-right col-1"
                  >{{ item.qty }}
                </q-item-section>
                <q-item-section class="text-right col-3.5"
                  >{{ formatCurrency(item.item.msrp) }}
                </q-item-section>
                <q-item-section class="text-right col-2.5 q-pr-xs"
                  >{{ formatCurrency(item.item.msrp * item.qty) }}
                </q-item-section>
              </q-item>
              <q-item>
                <q-item-section class="col-8 text-right text-bold"
                  >Sub:</q-item-section
                >
                <q-item-section class="text-right col-4 q-pr-md"
                  >{{ formatCurrency(state.total) }}
                </q-item-section>
              </q-item>
              <q-item>
                <q-item-section class="col-8 text-right text-bold">
                  Tax(13%):
                </q-item-section>
                <q-item-section class="text-right col-4 q-pr-md">
                  {{ formatCurrency(state.total * 0.13) }}
                </q-item-section>
              </q-item>
              <q-item>
                <q-item-section class="col-8 text-right text-bold text-primary"
                  >Total:</q-item-section
                >
                <q-item-section
                  class="text-right col-4 q-pr-md text-bold text-primary"
                >
                  {{ formatCurrency(state.total * 1.13) }}
                </q-item-section>
              </q-item>
            </q-list>
          </q-card>
          <q-item>
            <q-item-section class="col-6">
              <q-btn
                class="q-mr-sm"
                color="negative"
                label="Save Basket"
                :disable="state.basket.length < 1"
                @click="saveBasket()"
              />
            </q-item-section>

            <q-item-section class="col-6">
              <q-btn
                class="q-mr-sm"
                color="secondary"
                label="Clear Basket"
                @click="clearBasket()"
              />
            </q-item-section>
          </q-item>
        </q-page>
      </q-page-container>
    </q-layout>
  </div>
</template>

<script>
import { formatCurrency } from "../utils/formatutils";
import { reactive, onMounted } from "vue";
import { poster } from "../utils/apiutil";
export default {
  setup() {
    let state = reactive({
      status: "",
      msrp: 0,
      total: 0,
      basket: [],
    });

    onMounted(() => {
      if (sessionStorage.getItem("basket")) {
        state.basket = JSON.parse(sessionStorage.getItem("basket"));
        state.basket.forEach((basketItem) => {
          state.msrp += basketItem.item.msrp;
          state.total += basketItem.item.msrp * basketItem.qty;
        });
      } else {
        state.basket = [];
      }
    });

    const clearBasket = () => {
      sessionStorage.removeItem("basket");
      state.basket = [];
      state.status = "basket cleared";
      state.total = 0;
    };

    const saveBasket = async () => {
      let customer = JSON.parse(sessionStorage.getItem("customer"));
      let basket = JSON.parse(sessionStorage.getItem("basket"));
      try {
        state.status = "sending basket info to server";
        let orderHelper = { email: customer.email, selections: basket };
        let payload = await poster("order", orderHelper);
        if (payload.indexOf("not") > 0) {
          state.status = payload;
        } else {
          clearBasket();
          state.status = payload;
        }
      } catch (err) {
        console.log(err);
        state.status = `Error add basket: ${err}`;
      }
    };
    return {
      state,
      clearBasket,
      formatCurrency,
      saveBasket,
    };
  },
};
</script>
