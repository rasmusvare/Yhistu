<script lang="ts">
import { Options, Vue } from "vue-class-component";
import { useMemberStore } from "@/stores/members";
import { useMemberTypeStore } from "@/stores/memberTypes";
import { MemberTypeService } from "@/services/MemberTypeService";
import { useAssociationStore } from "@/stores/associations";
import { MemberService } from "@/services/MemberService";
import { PersonService } from "@/services/PersonService";
import type { IPerson } from "@/domain/IPerson";
import { ContactService } from "@/services/ContactService";
import { useContactTypeStore } from "@/stores/contactTypes";
import { ContactTypeService } from "@/services/ContactTypeService";
import MemberTypeAdd from "@/views/admin/membertype/MemberTypeAdd.vue";

@Options({
  components: { MemberTypeAdd },
  props: {},
  emits: []
})
export default class MemberAdd extends Vue {
  associationStore = useAssociationStore();

  memberTypeStore = useMemberTypeStore();
  memberTypeService = new MemberTypeService();

  memberStore = useMemberStore();
  memberService = new MemberService();

  contactService = new ContactService();

  contactTypeStore = useContactTypeStore();

  personService = new PersonService();
  persons!: IPerson[];
  person!: IPerson;

  firstName = "";
  lastName = "";
  idCode = "";
  email = "";
  memberTypeId = "1";

  errorMessage: Array<string> | null | undefined = null;

  async mounted() {
    this.memberTypeStore.$state.memberTypes =
      await this.memberTypeService.getAll(
        this.associationStore.$state.current?.id
      );
  }

  async createClicked() {
    //Create new person
    const res = await this.personService.add({
      firstName: this.firstName,
      lastName: this.lastName,
      idCode: this.idCode,
      email: this.email
    });
    if (res.status >= 300) {
      this.errorMessage = res.errorMessage;
      console.log(res);
    } else {
      this.errorMessage = null;
      this.persons = await this.personService.getAll();
    }

    // Add email contact to person
    // this.person = this.persons.find((p) => p.idCode == this.idCode)!;
    // const resEmail = await this.contactService.add({
    //   personId: res.data?.id,
    //   contactTypeId: this.contactTypeStore.email().id!,
    //   value: this.email,
    // });
    // if (resEmail.status >= 300) {
    //   this.errorMessage = resEmail.errorMessage;
    //   console.log(resEmail);
    // }

    // Add member to the association
    const resAss = await this.memberService.add({
      personId: res.data!.id!,
      associationId: this.associationStore.$state.current!.id!,
      memberTypeId: this.memberTypeId,
      viewAsRegularUser: true,
      from: new Date().toDateString()
    });
    console.log(resAss);


    if (resAss.status >= 300) {
      this.errorMessage = resAss.errorMessage;
      console.log(resAss);
    } else {
      this.memberStore.$state.members = await this.memberService.getAll(
        this.associationStore.$state.current?.id
      );
    }
  }
}
</script>
<template>
  <div class="container">
    <h3 class="mb-3 mt-2 text-center">Add a new member</h3>
    <div class="row d-flex justify-content-center">
      <div class="col-md-8">
        <div
          v-if="errorMessage != null"
          class="text-danger validation-summary-errors"
          data-valmsg-summary="true"
          data-valmsg-replace="true"
        >
          <ul v-for="error of errorMessage" :key="errorMessage.indexOf(error)">
            <li>{{ error }}</li>
          </ul>
        </div>
        <div>
          <div class="form-floating mb-3">
            <input v-model="firstName" class="form-control" type="text" />
            <label>First Name</label>
          </div>
          <div class="form-floating mb-3">
            <input v-model="lastName" class="form-control" type="text" />
            <label>Last Name</label>
          </div>
          <div class="form-floating mb-3">
            <input v-model="idCode" class="form-control" type="text" />
            <label>National identification number</label>
          </div>
          <div class="form-floating mb-3">
            <input v-model="email" class="form-control" type="text" />
            <label>Email</label>
          </div>
          <div class="mb-3">
            <select class="form-select" v-model="memberTypeId">
              <option selected disabled value="1">Select member type...</option>
              <option
                v-for="each in memberTypeStore.all"
                :value="each.id"
                :key="each.id"
              >
                {{ each.name }}
              </option>
            </select>
          </div>

          <div class="form-floating mb-3">
            <input
              @click="createClicked()"
              type="submit"
              value="Add new member"
              class="btn btn-primary"
              data-bs-toggle="collapse"
              data-bs-target="#addMember"
            />
            <button
              type="button"
              class="btn btn-secondary ms-3"
              data-bs-toggle="collapse"
              data-bs-target="#addMemberType"
            >
              Add new member type
            </button>
          </div>
          <div class="collapse" id="addMemberType">
            <div class="card card-body">
              <MemberTypeAdd />
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
