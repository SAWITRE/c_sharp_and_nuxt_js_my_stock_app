<script setup lang="ts">
defineProps<{
  modelValue: boolean;
  title: string;
  saveText?: string;
}>();

const emit = defineEmits<{
  (e: "update:modelValue", value: boolean): void;
  (e: "save"): void;
}>();

const close = () => {
  emit("update:modelValue", false);
};

const save = () => {
  emit("save");
};
</script>

<template>
  <div
    v-if="modelValue"
    class="fixed inset-0 z-50 flex items-center justify-center"
  >
    <!-- Backdrop -->
    <div
      class="absolute inset-0 bg-black/50"
      @click="close"
    />

    <!-- Modal -->
    <div
      class="relative bg-white w-full max-w-md rounded-2xl shadow-xl p-6 z-10 animate-fadeIn"
    >
      <!-- Header -->
      <div class="flex justify-between items-center mb-4">
        <h2 class="text-xl font-bold text-gray-800">
          {{ title }}
        </h2>

        <button
          class="text-gray-500 hover:text-red-500 text-xl"
          @click="close"
        >
          ✕
        </button>
      </div>

      <!-- Body -->
      <div class="space-y-3">
        <slot />
      </div>

      <!-- Footer -->
      <div class="flex justify-end gap-2 mt-6">
        <button
          class="px-4 py-2 rounded-lg bg-gray-200 hover:bg-gray-300"
          @click="close"
        >
          Cancel
        </button>

        <button
          class="px-4 py-2 rounded-lg bg-green-600 hover:bg-green-700 text-white"
          @click="save"
        >
          {{ saveText || "Save" }}
        </button>
      </div>
    </div>
  </div>
</template>

<style scoped>
@keyframes fadeIn {
  from {
    opacity: 0;
    transform: scale(0.95);
  }

  to {
    opacity: 1;
    transform: scale(1);
  }
}

.animate-fadeIn {
  animation: fadeIn 0.15s ease-out;
}
</style>