<script lang="ts">
import { Options, Vue } from "vue-class-component";
import { useAssociationStore } from "@/stores/associations";
import { AssociationService } from "@/services/AssociationService";
import { useApartmentStore } from "@/stores/apartments";
import { ApartmentService } from "@/services/ApartmentService";
import { useMemberStore } from "@/stores/members";
import { MemberService } from "@/services/MemberService";
import { usePersonStore } from "@/stores/persons";
import { PersonService } from "@/services/PersonService";

@Options({
  components: {},
  props: {},
  emits: []
})
export default class AssociationIndex extends Vue {
  associationStore = useAssociationStore();
  associationService = new AssociationService();
  apartmentStore = useApartmentStore();
  apartmentService = new ApartmentService();
  memberStore = useMemberStore();
  memberService = new MemberService();
  personStore = usePersonStore();
  personService = new PersonService();

  async created(): Promise<void> {
    const associations = await this.associationService.getAll();

    this.associationStore.$state.associations = associations;
    this.associationStore.$state.current = associations[0] ?? null;

    const apartments = await this.apartmentService.getAll();

    this.apartmentStore.$state.apartments = apartments;
    this.apartmentStore.$state.current = apartments[0] ?? null;

    this.memberStore.$state.members = await this.memberService.getAll(
      this.associationStore.$state.current.id);

    this.personStore.$state.userPerson = (await this.personService.getAll()).find(p => p.isMain);


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
                    ><span v-for="each of memberStore.getMembersOfBoard" :key="each.id"
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
                    <li>{{ account.bank }}: {{ account.accountNo }}</li>
                  </ul>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
    <div v-if="associationStore.$state.current == null">
      <h1 class="mt-5 mb-3 text-center">
        You are currently not a member of any associations
      </h1>
      <p class="mb-3 text-center">
        Ask a board member to add you as a member or create a new association
      </p>
    </div>

    <p class="text-center">
      <RouterLink to="/Association/Create">Create a New Association</RouterLink>
    </p>
  </main>
</template>
