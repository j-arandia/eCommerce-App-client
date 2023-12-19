<template>
  <div class="text-center q-mt-xs">
    <q-avatar class="q-mb-xs" size="100px" rounded>
      <img :src="`/img/skyr.jpg`" />
    </q-avatar>
  </div>
  <div class="text-h4 text-center q-mt-xs q-mb-xs">Logout</div>
  <div class="text-title2 text-center text-accent text-bold q-mt-sm">
    Sayonara!
  </div>
</template>
<script>
import { onMounted } from "vue";
export default {
  setup() {
    onMounted(() => {
      sessionStorage.clear();
    });
    return {};
  },
};
</script>
