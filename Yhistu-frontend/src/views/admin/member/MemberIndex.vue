<script lang="ts">
import { Options, Vue } from "vue-class-component";
import { MemberService } from "@/services/MemberService";
import { useMemberStore } from "@/stores/members";
import Person from "@/views/admin/person/Person.vue";
import { useAssociationStore } from "@/stores/associations";
import MemberAdd from "@/views/admin/member/MemberAdd.vue";

@Options({
  components: { MemberAdd, Person },
  props: { associationID: String },
  emits: [],
})
export default class MemberIndex extends Vue {
  memberStore = useMemberStore();
  associationStore = useAssociationStore();
  memberService = new MemberService();

  associationId = this.associationStore.$state.current?.id;

  async mounted() {
    const members = await this.memberService.getAll(this.associationId);
    console.log(members.length);
    this.memberStore.$state.members = members;
  }
}
</script>

<template>
  <div class="container mt-5">
    <p>
      <button
        type="button"
        class="btn btn-primary"
        data-bs-toggle="collapse"
        data-bs-target="#addMember"
      >
        Add new member
      </button>
    </p>
    <div class="collapse" id="addMember">
      <div class="card card-body">
        <MemberAdd />
      </div>
    </div>
    <table class="table mt-3">
      <thead>
        <tr>
          <th>Apartment</th>
          <th>Name</th>
          <th>Member Type</th>
          <th>Email</th>
          <th>Registered as a user</th>
          <th></th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="item of memberStore.all" :key="item.id">
          <td>2</td>
          <td>{{ item.person?.firstName }} {{ item.person?.lastName }}</td>
          <td>{{ item.memberType?.name }}</td>
          <td>{{ memberStore.getEmail(item.id) }}</td>
          <td v-if="item.person?.isMain">
            <font-awesome-icon icon="check" />
          </td>
          <td v-else></td>
          <td>
            <RouterLink
              :to="{
                name: 'association-edit',
                params: { associationId: item.id },
              }"
              >Edit
            </RouterLink>
            |
            <RouterLink
              :to="{
                name: 'apartment',
              }"
              >View all contacts
            </RouterLink>
            |
            <RouterLink
              :to="{
                name: 'bankaccount-create',
                params: { associationId: item.id },
              }"
              >Remove
            </RouterLink>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>
