<template>
  <div class="service">
    <div class="table-wrapper" v-if="categories !== null">
      <table>
        <thead>
          <th v-for="(category, index) in categoriesHead" :key="index" v-text="category" v-show="showColumn(category)"></th>
        </thead>
        <tbody>
          <tr v-for="(container, index) in containers" :key="index">
            <td v-for="(category, index) in categories" :key="index" v-text="container[category]" v-show="showColumn(category)"></td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
</template>

<script>
import Api from '@/api/api.js'
import _ from 'underscore'

export default {
  name: 'service',
  filter: 'service', // custom filter input
  filterby: 'service', // filter by data from container info
  data() {
    return {
      services: null,
      containers: null,
      categories: null,
      categoriesHead: [],
      search: ''
    }
  },
  props: {
    name: String,
    filter: String,
    filterby: {
      type: String,
      default: 'teacherLast'
    }
  },
  watch: {
    name: function() {
      this.getNarrowcastData();
    },
    filter: function(){
      this.filterNarrowcastData();
    } 
  },
  methods: {
    showColumn(category) {
      if(_.contains([this.$t('dashboard.table.courseName'), 'courseName', this.$t('dashboard.table.courseID'), 'courseID'], category)) return false;
      return true
    },
    getNarrowcastData() {
      Api.getCourse(this.name).then(data => {
        this.categories = Object.keys(data[0]);
        this.categoriesHead = Object.keys(data[0]);
        this.categoriesHead.forEach((item, index) => {
          this.categoriesHead[index] = this.$t(`dashboard.table.${item}`)
        });
        this.services = data;
        // eslint-disable-next-line
        this.services.forEach((item, index) => {
          this.services[index].teacherPresent = this.services[index].teacherPresent === true ? this.$t('dashboard.table.true') : this.$t('dashboard.table.false')
          this.services[index].teacherReachable = this.services[index].teacherReachable === true ? this.$t('dashboard.table.true') : this.$t('dashboard.table.false')
        });
        this.filterNarrowcastData();
      })
    },
    filterNarrowcastData(){
      this.containers = this.services.filter(category => {
        return category[this.filterby].toLowerCase().indexOf(this.filter.toLowerCase()) > -1
      });
    }
  }
}
</script>

<style lang="scss" scoped>
/* width */
::-webkit-scrollbar {
  width: 10px;
}

/* Track */
::-webkit-scrollbar-track {
  background: #f1f1f1;
}

/* Handle */
::-webkit-scrollbar-thumb {
  background: #009BAA;
}

/* Handle on hover */
::-webkit-scrollbar-thumb:hover {
  background: #24223C;
}
table {
  border-collapse: collapse;
  display: flex;

    tbody {
      display: flex;
      position: relative;
      overflow-x: auto;
      overflow-y: hidden;
      max-width: 100%;

    }
    
    th,
    td {
      display: block;
      height: 40px;
      line-height: 40px;
      border-top: 1px solid gray;
      padding: 10px 15px;
      overflow: hidden;
      text-overflow: ellipsis;
      white-space: nowrap;

      &:last-of-type {
        border-bottom: 1px solid gray;
      }
    }

    th {
      border-left: 1px solid gray;
      border-right: 1px solid gray;
      text-align: left;
      color: #fff;
      background-color: #005AA9;
    }

    tr {
      background-color: #fff;

      &:nth-child(even) {
          background-color: #f5f5f5;
      }

      &:last-of-type {
        border-right: 1px solid gray;
      }
    }
}
</style>