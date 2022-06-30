<script lang="ts">
import { Options, Vue } from "vue-class-component";
import { useIdentityStore } from "@/stores/identity";
import { useApartmentStore } from "@/stores/apartments";
import { useMemberStore } from "@/stores/members";

@Options({
  components: {}
})
export default class Header extends Vue {
  identityStore = useIdentityStore();
  apartmentStore = useApartmentStore();
  memberStore = useMemberStore();
}
</script>

<template>
  <header>
    <nav
      class="container navbar navbar-expand-sm navbar-toggleable-sm navbar-light bg-white border-bottom box-shadow mb-3"
    >
      <div class="container-fluid">
        <a class="navbar-brand" href="/">Yhistu</a>
        <button
          class="navbar-toggler"
          type="button"
          data-bs-toggle="collapse"
          data-bs-target=".navbar-collapse"
          aria-controls="navbarSupportedContent"
          aria-expanded="false"
          aria-label="Toggle navigation"
        >
          <span class="navbar-toggler-icon"></span>
        </button>
        <div
          class="navbar-collapse collapse d-sm-inline-flex justify-content-between"
        >
          <ul class="navbar-nav flex-grow-1">
            <li class="nav-item">
              <RouterLink
                to="/association"
                class="nav-link text-dark"
                active-class="active"
              >Association
              </RouterLink
              >
            </li>
            <template v-if="identityStore.$state.jwt != null">
              <li class="nav-item">
                <RouterLink
                  to="/meters"
                  class="nav-link text-dark"
                  active-class="active"
                >Readings
                </RouterLink>
              </li>
              <li class="nav-item">
                <RouterLink
                  to="/contacts"
                  class="nav-link text-dark"
                  active-class="active"
                >Your contacts
                </RouterLink>
              </li>
              <li v-if="memberStore.getAdmins.find((m: any)=> m.person.email===identityStore.jwt?.email)"
                class="nav-item">
                <RouterLink
                  to="/Admin"
                  class="nav-link text-dark"
                  active-class="active"
                >Association Admin
                </RouterLink>
              </li>
              <!--              <li class="nav-item">-->
              <!--                <RouterLink-->
              <!--                  to="/ContactTypes"-->
              <!--                  class="nav-link text-dark"-->
              <!--                  active-class="active"-->
              <!--                  >Contact Types-->
              <!--                </RouterLink>-->
              <!--              </li>-->
            </template>
          </ul>

          <ul class="navbar-nav">
            <template v-if="identityStore.$state.jwt == null">
              <li class="nav-item">
                <a class="nav-link text-dark" href="/identity/account/register"
                >Register</a
                >
              </li>
              <li class="nav-item">
                <a class="nav-link text-dark" href="/identity/account/login"
                >Login</a
                >
              </li>
            </template>
            <template v-else>
              <li class="nav-item">
                <a class="nav-link text-dark" href="/identity/account/logout"
                >Hello, {{ identityStore.jwt?.firstName }} Logout</a
                >
              </li>
            </template>
          </ul>
        </div>
      </div>
    </nav>
  </header>
</template>
