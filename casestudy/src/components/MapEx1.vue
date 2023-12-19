<template>
  <div class="text-center q-mt-md">
    <div class="text-h4">TomTom Map Ex1</div>
    <div class="text-h6">get lat and lon</div>
    <div>
      <q-input
        class="q-ma-lg text-h5"
        placeholder="enter current address"
        id="address"
        v-model="state.address"
      />
      <br />
    </div>
    <q-btn
      label="Get Lat/Lon"
      @click="getLatLon"
      class="p-button-outlined"
      style="width: 30vw"
    />
    <div class="text-positive text-title2 q-mt-lg">
      {{ state.latlon }}
    </div>
  </div>
</template>
<script>
import { reactive } from "vue";
export default {
  name: "MapEx1",
  setup() {
    let state = reactive({
      status: "",
      address: "",
      latlon: "",
    });
    const getLatLon = async () => {
      try {
        let url = `https://api.tomtom.com/search/2/geocode/${state.address}.json?key=90gMGZaJkJ7R6ck21pw4d3x5XWDxHGap`;
        let response = await fetch(url);
        let payload = await response.json();
        let lat = payload.results[0].position.lat;
        let lon = payload.results[0].position.lon;
        state.latlon = `Address is at Lat ${lat} and Lon ${lon} `;
      } catch (err) {
        state.status = err.message;
      }
    };
    return {
      state,
      getLatLon,
    };
  },
};
</script>
