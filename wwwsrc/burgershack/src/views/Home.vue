<template>
  <div class="home">
    <img alt="Vue logo" src="../assets/logo.png">
    <h1>BURGER SHACK!</h1>
    <div v-for="burger in burgers">
      <h4>{{burger.name}}</h4>
      <p>Description: {{burger.description}}</p>
      <p>Price: {{burger.price}}</p>
    </div>
  </div>
</template>

<script>
  // @ is an alias to /src


  export default {
    name: 'home',
    components: {

    },
    mounted() {
      this.$store.dispatch('getBurgers');
    },

    computed: {
      burgers() {
        return this.$store.state.burgers;
      }
    }

  }
</script>