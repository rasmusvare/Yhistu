<script lang="ts">
import { Options, Vue } from "vue-class-component";
import { useAssociationStore } from "@/stores/associations";
import { AssociationService } from "@/services/AssociationService";
import BuildingIndex from "@/views/admin/building/BuildingIndex.vue";
import ApartmentIndex from "@/views/admin/apartment/ApartmentIndex.vue";
import MemberIndex from "@/views/admin/member/MemberIndex.vue";
import AddContact from "@/views/admin/contact/AddContact.vue";
import MemberTypeIndex from "@/views/admin/membertype/MemberTypeIndex.vue";
import MeterBuilding from "./meter/MeterBuilding.vue";
import MeterTypeIndex from "./metertype/MeterTypeIndex.vue";
import ContactTypeIndex from "@/views/admin/contacttype/ContactTypeIndex.vue";
import MeasuringUnitIndex from "@/views/admin/measuringunit/MeasuringUnitIndex.vue";
import { useMemberStore } from "@/stores/members";
import { MemberService } from "@/services/MemberService";
import AssociationEdit from "@/views/admin/association/AssociationEdit.vue";
import BankAccountIndex from "@/views/admin/bankaccounts/BankAccountIndex.vue";

@Options({
  components: {
    BankAccountIndex,
    AssociationEdit,
    ContactTypeIndex,
    MemberTypeIndex,
    AddContact,
    MemberIndex,
    ApartmentIndex,
    BuildingIndex,
    MeterBuilding,
    MeterTypeIndex,
    MeasuringUnitIndex
  },
  props: {},
  emits: []
})
export default class AdminIndex extends Vue {
  associationStore = useAssociationStore();
  associationService = new AssociationService();
  memberStore = useMemberStore();
  memberService = new MemberService();


  buildingsActive = false;
  apartmentsActive = false;
  metersActive = false;
  // messagesActive = false;
  meterTypesActive = false;
  membersActive = false;
  memberTypesActive = false;
  contactTypesActive = false;

  async created() {
    const associations = await this.associationService.getAll();

    this.associationStore.$state.associations = associations;
    this.associationStore.$state.current = associations[0] ?? null;

    this.memberStore.$state.members = await this.memberService.getAll(
      this.associationStore.$state.current.id);
  }

  changeSection(section: string) {
    this.resetActiveSections();
    switch (section) {
      case "buildings":
        this.buildingsActive = true;
        break;
      case "apartments":
        this.apartmentsActive = true;
        break;
      case "meters":
        this.metersActive = true;
        break;
      case "meterTypes":
        this.meterTypesActive = true;
        break;
      case "members":
        this.membersActive = true;
        break;
      case "memberTypes":
        this.memberTypesActive = true;
        break;
      case "contactTypes":
        this.contactTypesActive = true;
        break;
    }
  }

  resetActiveSections() {
    this.buildingsActive = false;
    this.apartmentsActive = false;
    this.metersActive = false;
    this.meterTypesActive = false;
    this.membersActive = false;
    this.memberTypesActive = false;
    this.contactTypesActive = false;
  }
}
</script>

<template>
  <main role="main" class="pb-3">
    <div v-if="associationStore.$state.current != null" class="row d-flex">
      <div class="container d-flex justify-content-center h-100">
        <div class="col-10 h-100">
          <div
            class="card h-100 border-secondary justify-content-center mb-4 mt-4"
          >
            <div class="card-body">
              <h3 class="card-title text-center">
                {{ associationStore.$state.current.name }}
              </h3>
              <h5 class="card-title text-center">
                {{ associationStore.$state.current.address }}
              </h5>
              <div class="text-center mb-5">
                <small
                >Founded on:
                  {{ associationStore.$state.current.foundedOn }}</small
                ><br />
                <small
                >Registration number:
                  {{ associationStore.$state.current.registrationNo }}</small
                >
              </div>
              <div class="row align-items-center">
                <div class="col-auto">
                  <p>
                    <span class="fw-bold">Administrators: </span
                    ><span v-for="each of memberStore.getAdmins" :key="each.id"
                  >{{ each.person?.firstName }}
                      {{ each.person?.lastName }}
                      <span
                        v-if="
                          memberStore.getAdmins.indexOf(each) <
                          memberStore.getAdmins.length - 1
                        "
                      >,
                      </span>
                    </span>
                  </p>
                  <p>
                    <span class="fw-bold">Members of board: </span
                    ><span
                    v-for="each of memberStore.getMembersOfBoard"
                    :key="each.id"
                  >{{ each.person?.firstName }}
                      {{ each.person?.lastName }}
                      <span
                        v-if="
                          memberStore.getAdmins.indexOf(each) <
                          memberStore.getAdmins.length - 1
                        "
                      >,
                      </span>
                    </span>
                  </p>
                  <h6>Bank Accounts</h6>
                  <ul
                    v-for="account of associationStore.$state.current
                      .bankAccounts"
                    :key="account.id"
                  >
                    <li>{{ account?.bank }}: {{ account!.accountNo }}</li>
                  </ul>
                </div>
              </div>
            </div>
            <div class="row align-items-center">

              <button
                class="btn btn-primary col-3 ms-3 mb-3"
                type="button"
                data-bs-toggle="collapse"
                data-bs-target="#associationEdit"
              >
                Edit association
              </button>
              <button
                class="btn btn-primary col-3 ms-3 mb-3"
                type="button"
                data-bs-toggle="collapse"
                data-bs-target="#bankAccounts"
              >
                Edit bank accounts
              </button>
            </div>
            <div class="collapse" id="associationEdit">
              <div class="card card-body">
                <AssociationEdit />
              </div>
            </div>
            <div class="collapse" id="bankAccounts">
              <div class="card card-body">
                <BankAccountIndex />
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
    <div class="container d-flex justify-content-center h-100">
      <div class="btn-group" role="group">
        <button
          type="button"
          @click="changeSection('buildings')"
          :class="[buildingsActive ? 'btn btn-primary' : 'btn btn-secondary']"
        >
          Buildings
        </button>
        <button
          type="button"
          @click="changeSection('apartments')"
          :class="[apartmentsActive ? 'btn btn-primary' : 'btn btn-secondary']"
        >
          Apartments
        </button>
        <button
          type="button"
          @click="changeSection('meters')"
          :class="[metersActive ? 'btn btn-primary' : 'btn btn-secondary']"
        >
          Meters
        </button>
        <button
          type="button"
          @click="changeSection('meterTypes')"
          :class="[meterTypesActive ? 'btn btn-primary' : 'btn btn-secondary']"
        >
          Meter types
        </button>
        <button
          type="button"
          @click="changeSection('members')"
          :class="[membersActive ? 'btn btn-primary' : 'btn btn-secondary']"
        >
          Members
        </button>
        <button
          type="button"
          @click="changeSection('memberTypes')"
          :class="[memberTypesActive ? 'btn btn-primary' : 'btn btn-secondary']"
        >
          Member types
        </button>
        <button
          type="button"
          @click="changeSection('contactTypes')"
          :class="[
            contactTypesActive ? 'btn btn-primary' : 'btn btn-secondary',
          ]"
        >
          Contact types
        </button>
      </div>
    </div>
    <div class="row d-flex">
      <div v-if="buildingsActive">
        <BuildingIndex />
      </div>
      <div v-if="apartmentsActive">
        <ApartmentIndex />
      </div>
      <div v-if="metersActive">
        <MeterBuilding />
      </div>
      <div v-if="meterTypesActive">
        <MeterTypeIndex />
        <MeasuringUnitIndex />
      </div>
      <div v-if="membersActive">
        <MemberIndex />
      </div>
      <div v-if="memberTypesActive">
        <MemberTypeIndex />
      </div>
      <div v-if="contactTypesActive">
        <ContactTypeIndex />
      </div>
    </div>
  </main>
</template>
