<template>
    <router-link :to="`/narrowcast/${name}`">
        <article>
            <h1>{{name}}</h1>
        </article>
    </router-link>
</template>

<script>
export default {
  name: 'Narrowcast Demo',
  props: {
    name: String
  }
}
</script>