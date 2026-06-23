<script setup lang="ts">
const config = useRuntimeConfig();
</script>

<template>
  <footer class="border-t bg-white">
    <div class="max-w-7xl mx-auto px-6 py-4 flex justify-between items-center text-sm text-gray-500">

      <p>
        © {{ new Date().getFullYear() }} MyStock System
      </p>

      <p class="font-medium">
        Version {{ config.public.appVersion || "1.0.0" }}
      </p>

    </div>
  </footer>
</template>