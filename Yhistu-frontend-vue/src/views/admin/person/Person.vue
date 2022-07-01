<script lang="ts">
import {Vue, Options } from "vue-class-component";
import type { IPerson } from "@/domain/IPerson";
import { PersonService } from "@/services/PersonService";

@Options({
  components: {

  },
  props: {personId: String,
    person: Object as () => IPerson},
  emits: [],
})
export default class Person extends Vue{
  personService = new PersonService();

  person!: IPerson;
  personId!: string;

  async created(){
    console.log(this.person.firstName);
    // this.person = await this.personService.get(this.personId);
  }
};
</script>

<template>
 {{ person.firstName}} {{ person.lastName}}, owner of apartment x
</template>