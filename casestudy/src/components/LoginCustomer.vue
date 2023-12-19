<template>
  <div class="text-center q-mt-xs">
    <q-avatar class="q-mb-xs" size="100px" rounded>
      <img :src="`/img/skyr.jpg`" />
    </q-avatar>
  </div>
  <div class="text-h4 text-center q-mt-xs q-mb-md">Login</div>
  <div class="text-title2 text-center text-positive text-bold q-mt-sm">
    {{ state.status }}
  </div>
  <q-card class="q-ma-md q-pa-md">
    <q-form
      ref="myForm"
      class="q-gutter-md"
      greedy
      @submit="login"
      @reset="resetFields"
    >
      <q-input
        outlined
        placeholder="enter email address"
        id="email"
        v-model="state.email"
        :rules="[isRequired, isValidEmail]"
      />
      <q-input
        outlined
        placeholder="enter password"
        id="password"
        type="password"
        v-model="state.password"
        :rules="[isRequired]"
        autocomplete="on"
      />
      <q-btn label="Login" type="submit" />
      <q-btn label="Reset" type="reset" />
    </q-form>
  </q-card>
  <router-view></router-view>
</template>
<script>
import { reactive } from "vue";
import { poster } from "../utils/apiutil";
import { useRouter, useRoute } from "vue-router";
export default {
  setup() {
    const router = useRouter();
    const route = useRoute();
    let state = reactive({
      status: "",
      email: "",
      password: "",
    });
    const isValidEmail = (val) => {
      const emailPattern =
        /^(?=[a-zA-Z0-9@._%+-]{6,254}$)[a-zA-Z0-9._%+-]{1,64}@(?:[a-zA-Z0-9-]{1,63}\.){1,8}[a-zA-Z]{2,63}$/;
      return emailPattern.test(val) || "Invalid email";
    };
    const isRequired = (val) => {
      return !!val || "field is required";
    };
    const login = async () => {
      state.status = "logging into server";
      let customerHelper = {
        firstname: state.firstname,
        lastname: state.lastname,
        email: state.email,
        password: state.password,
      };
      try {
        await sessionStorage.removeItem("customer");
        let payload = await poster("login", customerHelper); // in util
        if (payload.token.indexOf("failed") !== -1) {
          state.status = payload.token;
        } else {
          state.status = "logged in";
          await sessionStorage.setItem("customer", JSON.stringify(payload));
          route.query.nextUrl
            ? router.push(route.query.nextUrl)
            : router.push({ path: "/" });
        }
      } catch (err) {
        state.status = err.message;
      }
    };
    const resetFields = () => {
      state.firstname = "";
      state.lastname = "";
      state.email = "";
      state.password = "";
      state.status = "";
    };
    return {
      state,
      login,
      isValidEmail,
      isRequired,
      resetFields,
    };
  },
};
</script>
