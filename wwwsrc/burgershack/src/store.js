import Vue from 'vue'
import Vuex from 'vuex'
import Axios from 'axios'

const server = Axios.create({
  baseURL: "//localhost:5000/api/",
  timeout: 3000
})

Vue.use(Vuex)

export default new Vuex.Store({
  state: {
    burgers: []
  },
  mutations: {
    setBurgers(state, burgers) {
      state.burgers = burgers;
    }
  },
  actions: {
    getBurgers({ commit, dispatch }) {
      server.get('Burgers')
        .then(res => {
          console.log("burgers: ", res.data)
          commit('setBurgers', res.data)
        })
        .catch(err => console.error(err))
    }
  }
})
