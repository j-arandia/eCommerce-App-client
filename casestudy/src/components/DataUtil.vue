<template>
  <div class="text-center q-mt-lg">
    <div class="text-h4">Data Utility</div>
    <q-btn
      class="q-ma-sm"
      color="white"
      text-color="black"
      label="Load Brand"
      @click="loadBrand"
    />
    <div class="status q-mt-md text-subtitle2 text-negative" text-color="red">
      {{ state.status }}
    </div>
  </div>
</template>
<script>
import { reactive } from "vue";
export default {
  setup() {
    let state = reactive({
      status: "",
    });
    const loadBrand = async () => {
      // replace ###### with the port number your ASP.Net core server is using
      let url = "https://localhost:7138/api/Data";
      try {
        state.status = "resetting exercise tables ...";
        let response = await fetch(`${url}`);
        state.status = await response.json();
      } catch (err) {
        console.log(err);
        state.status = `Error has occured: ${err.message}`;
      }
    };
    return {
      loadBrand,
      state,
    };
  },
};
</script>
