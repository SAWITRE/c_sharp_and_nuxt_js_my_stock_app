<script setup lang="ts">
import type { Category } from "~/types/category";

const config = useRuntimeConfig();

// --- Global state ---
const search = ref("");
const showAddModal = ref(false);

const { data: categories, pending, error, refresh } = await useFetch<Category[]>(
  () => `${config.public.apiBase}/api/categories?search=${encodeURIComponent(search.value)}`,
);

// --- Add modal state ---
const form = ref({
  name: "",
  description: "",
});

// --- Edit modal state ---
const isEditOpen = ref(false);
const selected = ref<Category | null>(null);

// --- Delete modal state ---
const isDeleteOpen = ref(false);
const deleteTarget = ref<Category | null>(null);
const isDeleting = ref(false);

// --- Derived state ---
const filteredCategories = computed(() => {
  if (!categories.value) return [];

  const keyword = search.value.toLowerCase().trim();
  if (!keyword) return categories.value;

  return categories.value.filter(
    (c) =>
      c.name.toLowerCase().includes(keyword) ||
      c.description?.toLowerCase().includes(keyword),
  );
});

// --- Actions: Edit modal ---
const openEdit = (item: Category) => {
  selected.value = { ...item };
  isEditOpen.value = true;
};

const closeEdit = () => {
  isEditOpen.value = false;
  selected.value = null;
};

const save = async () => {
  if (!selected.value) return;

  await $fetch(`${config.public.apiBase}/api/categories/${selected.value.id}`, {
    method: "PUT",
    body: selected.value,
  });

  await refresh();
  closeEdit();
};

// --- Actions: Delete modal ---
const openDelete = (item: Category) => {
  deleteTarget.value = item;
  isDeleteOpen.value = true;
};

const closeDelete = () => {
  isDeleteOpen.value = false;
  deleteTarget.value = null;
};

const confirmDelete = async () => {
  if (!deleteTarget.value) return;

  isDeleting.value = true;

  await $fetch(`${config.public.apiBase}/api/categories/${deleteTarget.value.id}`, {
    method: "DELETE",
  });

  await refresh();
  isDeleting.value = false;
  closeDelete();
};

// --- Actions: Add modal ---
const saveCategory = async () => {
  await $fetch(`${config.public.apiBase}/api/categories`, {
    method: "POST",
    body: {
      name: form.value.name,
      description: form.value.description,
    },
  });

  showAddModal.value = false;
  form.value = {
    name: "",
    description: "",
  };

  await refresh();
};
</script>

<template>
  <div class="p-6">
    <!-- Header -->
    <div class="flex justify-between items-center mb-8">
      <h1 class="text-3xl font-bold text-gray-800">📦 Categories</h1>

      <!-- Search -->
      <div class="relative">
        <input
          v-model="search"
          type="text"
          placeholder="Search categories..."
          class="w-64 pl-10 pr-4 py-2 border rounded-xl shadow-sm focus:ring-2 focus:ring-blue-500 focus:border-blue-500 outline-none"
        />

        <span class="absolute left-3 top-1/2 -translate-y-1/2 text-gray-400">
          🔍
        </span>
      </div>
      <div
        class="inline-flex items-center gap-2 bg-blue-50 text-blue-700 px-4 py-2 rounded-full text-sm font-medium"
      >
        📊
        {{ filteredCategories.length }} Categories Found
      </div>

      <!-- Add Button -->
      <button
        @click="showAddModal = true"
        class="bg-blue-600 text-white px-4 py-2 rounded-lg hover:bg-blue-700"
      >
        + Add Category
      </button>
    </div>

    <!-- Loading -->
    <div v-if="pending" class="text-gray-500">Loading...</div>

    <!-- Error -->
    <div v-else-if="error" class="text-red-500">Failed to load categories</div>

    <!-- Empty state -->
    <div
      v-else-if="filteredCategories && filteredCategories.length === 0"
      class="text-gray-500"
    >
      No categories found
    </div>

    <!-- Cards -->
    <div
      v-else
      class="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-6"
    >
      <div
        v-for="item in filteredCategories"
        :key="item.id"
        class="bg-white border rounded-2xl shadow hover:shadow-lg transition p-5"
      >
        <!-- icon -->
        <div
          class="w-12 h-12 rounded-lg bg-blue-100 text-blue-700 flex items-center justify-center font-bold text-lg mb-4"
        >
          {{ item.name.charAt(0).toUpperCase() }}
        </div>

        <!-- name -->
        <h2 class="text-lg font-semibold text-gray-800">
          {{ item.name }}
        </h2>

        <!-- description -->
        <p class="text-sm text-gray-500 mt-1">
          {{ item.description || "No description" }}
        </p>

        <!-- actions -->
        <div class="flex gap-2 mt-4">
          <button
            @click="openEdit(item)"
            class="text-sm px-3 py-1 rounded-lg bg-yellow-500 hover:bg-yellow-600 text-white"
          >
            Edit
          </button>

          <button
            @click="openDelete(item)"
            class="text-sm px-3 py-1 rounded-lg bg-red-500 hover:bg-red-600 text-white"
          >
            Delete
          </button>
        </div>
      </div>
    </div>

    <!-- Edit Modal -->
    <CommonBaseModal v-model="isEditOpen" :title="'Edit Category'" @save="save">
      <div v-if="selected" class="space-y-3">
        <!-- Name -->
        <div>
          <label class="text-sm text-gray-500">Name</label>
          <input v-model="selected.name" class="w-full border p-2 rounded-lg" />
        </div>

        <!-- Description -->
        <div>
          <label class="text-sm text-gray-500">Description</label>
          <textarea
            v-model="selected.description"
            class="w-full border p-2 rounded-lg"
          />
        </div>
      </div>
    </CommonBaseModal>
    <CommonConfirmModal
      v-model="isDeleteOpen"
      :title="'🗑 Delete Category'"
      :message="`Delete '${deleteTarget?.name}' ?`"
      :loading="isDeleting"
      @confirm="confirmDelete"
    />
    <!-- Add Modal -->
    <CommonBaseModal
      v-model="showAddModal"
      :title="'Add Category'"
      @save="saveCategory"
    >
      <div class="space-y-3">
        <!-- Name -->
        <div>
          <label class="text-sm text-gray-500">Name</label>
          <input v-model="form.name" class="w-full border p-2 rounded-lg" />
        </div>

        <!-- Description -->
        <div>
          <label class="text-sm text-gray-500">Description</label>
          <textarea
            v-model="form.description"
            class="w-full border p-2 rounded-lg"
          />
        </div>
      </div>
    </CommonBaseModal>
  </div>
</template>
