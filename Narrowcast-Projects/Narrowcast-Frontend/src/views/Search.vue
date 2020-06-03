<template>
  <div class="search">
    <router-link :to="`/`">Terug</router-link>
    <input v-model="filterLast" type="text" placeholder="last name"/>
    <h1>Search results</h1>
    <Loader v-if="categories === null" />
    <table v-if="categories !== null">
      <thead>
        <th v-for="(category, index) in categories" :key="index" v-text="category"></th>
      </thead>
      <tbody>
        <tr v-for="(narrowcast, index) in narrowcasts" :key="index" :name="narrowcast.teacherLast" >
          <td v-text="narrowcast.narrowcastNr"></td>
          <td v-text="narrowcast.teacherFirst"></td>
          <td v-text="narrowcast.teacherLast"></td>
          <td v-text="narrowcast.courseName"></td>
          <td v-text="narrowcast.courseID"></td>
          <td v-text="narrowcast.eMail"></td>
          <td v-text="narrowcast.phoneNumber"></td>
          <td v-text="narrowcast.teacherPresent"></td>
          <td v-text="narrowcast.classLocation"></td>
          <td v-text="narrowcast.teacherReachable"></td>
          <td v-text="narrowcast.updateDate"></td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script>
import Api from '@/api/api.js'
import { Loader } from '@/components/common'

export default {
  name: 'search',
  data() {
    return {
      narrowcasts: null,
      filterLast: '',
      categories: null
    }
  },
  components: {
    Loader
  },
  watch: {
    filterLast: function() {
      this.filteredLast();
    }
  },
  methods: {
    filteredLast() {
      this.narrowcasts = this.narrowcasts.filter(narrowcast => {
        return narrowcast.teacherLast.toLowerCase().indexOf(this.filterLast.toLowerCase()) > -1
      });
    }
  },
  mounted() {
    Api.getCustomSearchResults().then(data => {
        this.categories = Object.keys(data[0]);
        this.narrowcasts = data;
        this.filteredLast();
    })
  }
}
</script>

<style lang="scss" scoped>
table {
  border-collapse: collapse;

  thead {
    th {
      height: 60px;
      background: #36304a;
      color: #fff;
      padding: 0 10px;
    }
  } 
  tbody {
    tr {
      background-color: #fff;

      &:nth-child(even) {
          background-color: #f5f5f5;
      }
    }
  }
}
</style>