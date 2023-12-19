<template>
  <q-page>
    <div class="text-center q-mt-lg">
      <q-avatar class="q-mb-md" size="120px" rounded>
        <img :src="`/img/skyr.jpg`" />
      </q-avatar>
      <div class="text-h3 text-lime-9">SKYR BRANDS</div>
      <div class="text-deep-orange-10 q-mt-lg text-bold">
        {{ state.status }}
      </div>
      <q-select
        class="q-mt-lg q-ml-lg"
        v-if="state.brands.length > 0"
        style="width: 50vw; margin-bottom: 4vh; background-color: #fff"
        :option-value="'id'"
        :option-label="'name'"
        v-model="state.selectedBrandId"
        :options="state.brands"
        label="Select a Brand"
        emit-value
        map-options
        @update:model-value="loadProductitems()"
      />
      <div
        class="text-h6 text-bold text-center text-teal-6"
        v-if="state.productitems.length > 0"
      >
        {{ state.selectedBrand.name }} ITEMS
      </div>
      <q-scroll-area style="height: 55vh">
        <q-card class="q-ma-md">
          <q-list separator>
            <q-item
              v-for="item in state.productitems"
              :key="item.id"
              clickable
              @click="selectProductItem(item.id)"
            >
              <q-item-section avatar>
                <q-avatar style="height: 90px; width: 90px" square>
                  <img :src="`/img/${item.graphicName}`" />
                </q-avatar>
              </q-item-section>
              <q-item-section class="text-left">
                {{ item.description }}
              </q-item-section>
            </q-item>
          </q-list>
        </q-card>
      </q-scroll-area>
    </div>
    <q-dialog
      v-model="state.itemSelected"
      transition-show="rotate"
      transition-hide="rotate"
    >
      <q-card>
        <q-card-actions align="right">
          <q-btn flat label="X" color="dark" v-close-popup class="text-h5" />
        </q-card-actions>
        <div class="text-center q-mt-lg">
          <q-avatar class="q-mb-md" size="180px" rounded>
            <img :src="`/img/${state.selectedProductItem.graphicName}`" />
          </q-avatar>
        </div>
        <q-card-section>
          <div class="text-subtitle text-center text-lime-9 text-h5">
            {{ state.selectedProductItem.productName }} -
            {{ formatCurrency(state.selectedProductItem.msrp) }}
          </div>
        </q-card-section>

        <q-card-section>
          <div class="text-subtitle2 text-justify">
            {{ state.selectedProductItem.description }}
          </div>
        </q-card-section>
        <q-card-section>
          <div class="row">
            <div class="col-2 q-mr-sm">
              <q-input
                v-model.number="state.qty"
                type="number"
                filled
                style="max-width: 15vw"
                placeholder="qty"
                hint="Qty"
                dense
              />
            </div>
            <div class="col-4 q-mr-sm">
              <q-btn
                color="light-green-10"
                label="Add To Basket"
                :disable="state.qty < 0"
                @click="addToBasket()"
              />
            </div>
            <div class="col-3">
              <q-btn
                color="primary"
                label="View Basket"
                :disable="state.basket.length < 1"
                @click="viewBasket()"
              />
            </div>
          </div>
        </q-card-section>
        <q-card-section class="text-center text-deep-orange-10 text-bold">
          {{ state.dialogStatus }}
        </q-card-section>
      </q-card>
    </q-dialog>
  </q-page>
</template>
<script>
import { formatCurrency } from "../utils/formatutils";
import { reactive, onMounted } from "vue";
import { fetcher } from "../utils/apiutil";
import { useRouter } from "vue-router";
export default {
  setup() {
    onMounted(() => {
      loadBrand();
    });
    const router = useRouter();
    let state = reactive({
      status: "",
      brands: [],
      productitems: [],
      selectedBrand: {},
      selectedBrandId: "",
      selectedProductItem: {},
      dialogStatus: "",
      itemSelected: false,
      qty: 0,
      basket: [],
    });

    const loadBrand = async () => {
      try {
        state.status = "loading brands...";
        const payload = await fetcher(`Brands`);
        if (payload.error === undefined) {
          state.brands = payload;
          state.status = `loaded ${state.brands.length} brands`;
        } else {
          state.status = payload.error;
        }
      } catch (err) {
        console.log(err);
        state.status = `Error has occured: ${err.message}`;
      }
    };

    const loadProductitems = async () => {
      try {
        state.selectedBrand = state.brands.find(
          (brand) => brand.id === state.selectedBrandId
        );
        state.status = `finding productitems for brand ${state.selectedBrand}...`;
        state.productitems = await fetcher(
          `Productitem/${state.selectedBrand.id}`
        );
        state.status = `loaded ${state.productitems.length} product items for ${state.selectedBrand.name}`;
      } catch (err) {
        console.log(err);
        state.status = `Error has occured: ${err.message}`;
      }
    };

    const selectProductItem = async (productitemid) => {
      try {
        // q-item click sends us the id, so we need to find the object
        state.selectedProductItem = state.productitems.find(
          (item) => item.id === productitemid
        );
        state.itemSelected = true;
        state.dialogStatus = "";
        if (sessionStorage.getItem("basket")) {
          state.basket = JSON.parse(sessionStorage.getItem("basket"));
        }
      } catch (err) {
        console.log(err);
        state.status = `Error has occured: ${err.message}`;
      }
    };

    const addToBasket = () => {
      let index = -1;
      if (state.basket.length > 0) {
        index = state.basket.findIndex(
          // is item already on the basket
          (item) => item.id === state.selectedProductItem.id
        );
      }
      if (state.qty > 0) {
        index === -1 // add
          ? state.basket.push({
              id: state.selectedProductItem.id,
              qty: state.qty,
              item: state.selectedProductItem,
            })
          : (state.basket[index] = {
              // replace
              id: state.selectedProductItem.id,
              qty: state.qty,
              item: state.selectedProductItem,
            });
        state.dialogStatus = `${state.qty} item(s) added`;
      } else {
        index === -1 ? null : state.basket.splice(index, 1); // remove
        state.dialogStatus = `item(s) removed`;
      }
      sessionStorage.setItem("basket", JSON.stringify(state.basket));
      state.qty = 0;
    };
    const viewBasket = () => {
      router.push("basket");
    };

    return {
      state,
      loadProductitems,
      selectProductItem,
      formatCurrency,
      addToBasket,
      viewBasket,
    };
  },
};
</script>
