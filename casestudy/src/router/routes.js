const routes = [
  {
    path: "/",
    component: () => import("layouts/MainLayout.vue"),
    children: [
      // Home Page
      {
        path: "/",
        name: "home",
        component: () => import("pages/HomePage.vue"),
      },
      // DataUtil
      {
        path: "/datautil",
        name: "datautil",
        component: () => import("components/DataUtil.vue"),
      },
      // Brans
      {
        path: "/brands",
        name: "brands",
        component: () => import("components/BrandsList.vue"),
      },
      {
        path: "/basket",
        name: "Basket",
        component: () => import("components/BasketContainer.vue"),
      },
      {
        name: "register",
        path: "/register",
        component: () => import("components/RegisterCustomer.vue"),
      },
      {
        name: "login",
        path: "/login",
        component: () => import("components/LoginCustomer.vue"),
      },
      {
        name: "logout",
        path: "/logout",
        component: () => import("components/LogoutCustomer.vue"),
      },
      {
        name: "orderlist",
        path: "/orderlist",
        component: () => import("components/OrderList.vue"),
      },
      {
        path: "/map",
        name: "map",
        component: () => import("src/components/Branches.vue"),
      },
    ],
  },
  // Always leave this as last one,
  // but you can also remove it
  {
    path: "/:catchAll(.*)*",
    component: () => import("pages/ErrorNotFound.vue"),
  },
];
export default routes;
