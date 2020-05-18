<template>
  <div class="dashboard">
    <header>
      <h1 v-if="selectedContainer !== null" v-text="returnDisplayName(selectedContainer)" />
    </header>
    <aside>
      <div class="title">
        <span>{{pageTitle}}</span>
      </div>
      <input v-model="filterTerm" type="text" placeholder="Filter opleidingen" :disabled="courses === null"/>
      <ul>
        <li v-for="(courses, index) in filteredItems" :key="index">
          <a href="#" @click="selectContainer(courses.courseName, $event)" :class="[courses.courseName === selectedContainer ? 'active' : '']">{{returnDisplayName(courses.courseName)}}</a>
        </li>
      </ul>
    </aside>
    <main>
      <vue-dropdown v-if="selectedContainer !== null" :config="configDropdown" @setSelectedOption="setNewSelectedOption($event)"></vue-dropdown>
      <input v-if="selectedContainer !== null" v-model="filterDataTerm" type="text" placeholder="Zoekterm" :disabled="selectedContainer === null" v-on:input="selectData(filterDataTerm, $event)" />
      <List :name="selectedContainer" :filter="filterData" :filterby="filterByData" />
    </main>
  </div>
</template>

<script>
import VueDropdown from 'vue-dynamic-dropdown'
import Api from '@/api/api.js'
import { List } from '@/components/common'

export default {
  name: 'dashboard',
  components: {
    VueDropdown,
    List
  },
  data() {
    return {
      pageTitle: 'Teachere?',
      courses: null,
      filterTerm: '',
      filterDataTerm: '', // initialize custom filter input
      filterByData: 'teacherFirst', // set default filter
      filterData: '', // used to store the custom filter input
      filteredItems: null,
      selectedContainer: null,

      configDropdown: {
        options: [
          {text: 'Voornaam', value: 'teacherFirst'},{text: 'Achternaam', value: 'teacherLast'},{text: 'E-Mail', value: 'eMail'},
          {text: 'Telefoon nummer', value: 'phoneNumber'},{text: 'Locatie', value: 'classLocation'}
        ],
        prefix: 'Zoek op',
        placeholder: 'Voornaam'
        // setting hover text color and text background needs to be set in the vue-dynamic-dropdown module
      }
    }
  },
  watch: {
    filterTerm: function() {
      this.filteredCourses();
    }
  },
  methods: {
    filteredCourses() {
      this.filteredItems = this.courses.filter(course => {
        return course.courseName.toLowerCase().indexOf(this.filterTerm.toLowerCase()) > -1
      });
    },
    selectContainer(name, event) {
      if (event) event.preventDefault();
      this.selectedContainer = name;
    },
    selectData(filter, event){
      if(event) event.preventDefault();
      this.filterData = filter;
    },
    setNewSelectedOption(selectedOption) {
      this.configDropdown.placeholder = selectedOption.text;
      this.filterByData = selectedOption.value;
    },
    returnDisplayName(name) {
      return name.split('.').join(' ');
    }
  },
  mounted() {
    Api.getCourses().then(data => {
        this.courses = data;
        this.filteredCourses();
    })
  }
}
</script>

<style lang="scss" scoped>
  @import "@/scss/Dashboard.scss";
</style>