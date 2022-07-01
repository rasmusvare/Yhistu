<script lang="ts">
import { Options, Vue } from "vue-class-component";

@Options({
  components: {},
})
export default class Footer extends Vue {
}
</script>

<template>
  <footer class="container border-top footer text-muted">
    <div class="container">
      © 2022 - Yhistu - <a href="/Home/Privacy">Privacy</a>
    </div>
  </footer>
</template>
