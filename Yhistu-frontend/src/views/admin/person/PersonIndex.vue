<script lang="ts">
import { Options, Vue } from "vue-class-component";

@Options({
  components: { },
  props: { associationId: String },
  emits: [],
})
export default class PersonIndex extends Vue{
  associationId!: string;
};
</script>

<template>
persons
</template>
