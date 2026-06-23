// https://nuxt.com/docs/api/configuration/nuxt-config
export default defineNuxtConfig({
  compatibilityDate: "2025-07-15",
  devtools: { enabled: true },

  runtimeConfig: {
    public: {
      apiBase: "http://localhost:5189",
       appVersion: process.env.APP_VERSION || "0.0.0",
    },
  },

  modules: ["@nuxtjs/tailwindcss"],
});