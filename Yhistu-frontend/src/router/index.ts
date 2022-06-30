import { createRouter, createWebHistory } from "vue-router";
import HomeView from "../views/HomeView.vue";
import Login from "@/views/identity/Login.vue";
import Register from "@/views/identity/Register.vue";
import AssociationIndex from "@/views/admin/association/AssociationIndex.vue";
import Association from "@/views/association/Association.vue";
import AssociationCreate from "@/views/admin/association/AssociationCreate.vue";
import AssociationEdit from "@/views/admin/association/AssociationEdit.vue";
import BuildingCreate from "@/views/admin/building/BuildingCreate.vue";
import BuildingIndex from "@/views/admin/building/BuildingIndex.vue";
import ApartmentIndex from "@/views/apartments/ApartmentIndex.vue";
import MeterApartment from "@/views/meters/MeterApartment.vue";
import AdminIndex from "@/views/admin/AdminIndex.vue";
import BankAccountAdd from "@/views/admin/bankaccounts/BankAccountAdd.vue";
import ContactIndex from "@/views/contacts/ContactIndex.vue";

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: "/",
      name: "home",
      component: HomeView,
    },
    {
      path: "/identity/account/login",
      name: "login",
      component: Login,
    },
    {
      path: "/identity/account/register",
      name: "register",
      component: Register,
    },
    {
      path: "/association",
      name: "association",
      component: Association,
    },
    {
      path: "/associationindex",
      name: "associationindex",
      component: AssociationIndex,
    },
    {
      path: "/association/create",
      name: "association-create",
      component: AssociationCreate,
    },
    {
      path: "/association/edit",
      name: "association-edit",
      component: AssociationEdit,
      props: true,
    },
    {
      path: "/association/bankaccount/create",
      name: "bankaccount-create",
      component: BankAccountAdd,
      props: true,
    },
    {
      path: "/building",
      name: "building",
      component: BuildingIndex,
    },
    {
      path: "/building/create",
      name: "building-create",
      component: BuildingCreate,
      props: true,
    },
    {
      path: "/apartment",
      name: "apartment",
      component: ApartmentIndex,
    },
    {
      path: "/meters",
      name: "meters",
      component: MeterApartment,
      props: true,
    },
    {
      path: "/admin",
      name: "admin-index",
      component: AdminIndex,
    },
    {
      path: "/contacts",
      name: "contacts-index",
      component: ContactIndex,
      props: true,
    },
  ],
});

export default router;
