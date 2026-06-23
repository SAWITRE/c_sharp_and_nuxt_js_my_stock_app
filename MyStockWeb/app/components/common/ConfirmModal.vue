<script setup lang="ts">
defineProps<{
  modelValue: boolean;
  title?: string;
  message: string;
  confirmText?: string;
  loading?: boolean;
}>();

const emit = defineEmits<{
  (e: "update:modelValue", value: boolean): void;
  (e: "confirm"): void;
}>();

const close = () => {
  emit("update:modelValue", false);
};

const confirm = () => {
  emit("confirm");
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
      <h2 class="text-xl font-bold text-red-600 mb-4">
        {{ title || "Confirm Delete" }}
      </h2>

      <!-- Message -->
      <p class="text-gray-600">
        {{ message }}
      </p>

      <!-- Footer -->
      <div class="flex justify-end gap-2 mt-6">
        <button
          :disabled="loading"
          class="px-4 py-2 rounded-lg bg-gray-200 hover:bg-gray-300 disabled:opacity-50"
          @click="close"
        >
          Cancel
        </button>

        <button
          :disabled="loading"
          class="px-4 py-2 rounded-lg bg-red-600 hover:bg-red-700 text-white disabled:opacity-50"
          @click="confirm"
        >
          {{ loading ? "Deleting..." : (confirmText || "Delete") }}
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