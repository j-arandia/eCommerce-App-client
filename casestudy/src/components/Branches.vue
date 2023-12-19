<template>
  <div class="text-center">
    <q-avatar class="q-mt-md" size="90px" rounded>
      <img :src="`/img/skyr.jpg`" />
    </q-avatar>
    <div class="text-h6 text-accent">Locate 3 Closest Branches To:</div>
    <div>
      <q-input
        class="q-ma-lg text-h5"
        placeholder="enter postal code"
        id="address"
        v-model="state.address"
      />
      <br />
    </div>
    <q-btn
      label="Locate 3"
      @click="getCloseBranches"
      class="q-mb-md"
      style="width: 30vw"
    />
    <div
      style="height: 50vh; width: 90vw; margin-left: 5vw; border: solid"
      ref="mapRef"
      v-show="state.showmap === true"
    ></div>
  </div>
</template>
<script>
import { fetcher } from "../utils/apiutil";
import { ref, reactive } from "vue";
export default {
  name: "BranchMap",
  setup() {
    const mapRef = ref(null);
    let state = reactive({
      status: "",
      address: "",
      showmap: false,
      latlon: "",
    });
    const getCloseBranches = async () => {
      try {
        state.showmap = true;
        const tt = window.tt;
        let url = `https://api.tomtom.com/search/2/geocode/${state.address}.json?key=90gMGZaJkJ7R6ck21pw4d3x5XWDxHGap`;
        let response = await fetch(url);
        let payload = await response.json();
        let lat = payload.results[0].position.lat;
        let lon = payload.results[0].position.lon;
        let map = tt.map({
          key: "90gMGZaJkJ7R6ck21pw4d3x5XWDxHGap",
          container: mapRef.value,
          source: "vector/1/basic-main",
          center: [lon, lat],
          zoom: 8,
        });
        map.addControl(new tt.FullscreenControl());
        map.addControl(new tt.NavigationControl());
        let threeBranches = await fetcher(`branch/${lat}/${lon}`);
        threeBranches.forEach((branch) => {
          let marker = new tt.Marker()
            .setLngLat([branch.longitude, branch.latitude])
            .addTo(map);
          let popupOffset = 25;
          let popup = new tt.Popup({ offset: popupOffset });
          popup.setHTML(
            `<div id="popup">Store#: ${branch.id}</div><div>${branch.street}, ${
              branch.city
            }
<br/>${branch.distance.toFixed(2)} km</div>`
          );
          marker.setPopup(popup);
        });
      } catch (err) {
        state.status = err.message;
      }
    };

    return {
      mapRef,
      state,
      getCloseBranches,
    };
  },
};
</script>
