<script setup lang="ts">
import type { Category } from "~/types/category";

const config = useRuntimeConfig();

// --- Global state ---
const search = ref("");
const showAddModal = ref(false);

const { data: products, pending, error, refresh } = await useFetch<any[]>(
  () => `${config.public.apiBase}/api/products?search=${encodeURIComponent(search.value)}`,
);

const { data: categories } = await useFetch<Category[]>(
  `${config.public.apiBase}/api/categories`,
);

// --- Add modal state ---
const form = ref({
  name: "",
  description: "",
  price: 0,
  stockQuantity: 0,
  categoryId: null,
});

// --- Edit modal state ---
const isEditOpen = ref(false);
const selected = ref<any | null>(null);

// --- Delete modal state ---
const isDeleteOpen = ref(false);
const deleteTarget = ref<any | null>(null);
const isDeleting = ref(false);

// --- Derived state ---
const filteredProducts = computed(() => {
  if (!products.value) return [];

  const keyword = search.value.toLowerCase().trim();
  if (!keyword) return products.value;

  return products.value.filter(
    (p) =>
      p.name.toLowerCase().includes(keyword) ||
      p.categoryName?.toLowerCase().includes(keyword),
  );
});

// --- Actions: Edit modal ---
const openEdit = (item: any) => {
  selected.value = { ...item };
  isEditOpen.value = true;
};

const closeEdit = () => {
  isEditOpen.value = false;
  selected.value = null;
};

const saveEdit = async () => {
  if (!selected.value) return;

  await $fetch(`${config.public.apiBase}/api/products/${selected.value.id}`, {
    method: "PUT",
    body: selected.value,
  });

  await refresh();
  closeEdit();
};

// --- Actions: Delete modal ---
const openDelete = (item: any) => {
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

  await $fetch(`${config.public.apiBase}/api/products/${deleteTarget.value.id}`, {
    method: "DELETE",
  });

  await refresh();
  isDeleting.value = false;
  closeDelete();
};

// --- Actions: Add modal ---
const saveProduct = async () => {
  await $fetch(`${config.public.apiBase}/api/products`, {
    method: "POST",
    body: {
      name: form.value.name,
      description: form.value.description,
      price: form.value.price,
      stockQuantity: form.value.stockQuantity,
      categoryId: form.value.categoryId,
    },
  });

  showAddModal.value = false;
  form.value = {
    name: "",
    description: "",
    price: 0,
    stockQuantity: 0,
    categoryId: null,
  };

  await refresh();
};
</script>

<template>
  <div class="p-6">
    <div class="flex flex-wrap justify-between items-center gap-4 mb-8">
      <h1 class="text-3xl font-bold text-gray-800">📦 Products</h1>

      <div class="relative flex-1 min-w-[240px] max-w-md">
        <input
          v-model="search"
          type="text"
          placeholder="Search products..."
          class="w-full pl-10 pr-4 py-2 border rounded-xl shadow-sm focus:ring-2 focus:ring-blue-500 focus:border-blue-500 outline-none"
        />
        <span class="absolute left-3 top-1/2 -translate-y-1/2 text-gray-400">
          🔍
        </span>
      </div>

      <div
        class="inline-flex items-center gap-2 bg-blue-50 text-blue-700 px-4 py-2 rounded-full text-sm font-medium"
      >
        📊
        {{ filteredProducts.length }} products found
      </div>

      <button
        @click="showAddModal = true"
        class="bg-blue-600 hover:bg-blue-700 text-white px-4 py-2 rounded-lg shadow"
      >
        + Add Product
      </button>
    </div>

    <div v-if="pending" class="text-gray-500">Loading...</div>
    <div v-else-if="error" class="text-red-500">Failed to load products</div>
    <div v-else-if="filteredProducts.length === 0" class="text-gray-500">
      No products found
    </div>

    <div
      v-else
      class="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-6"
    >
      <div
        v-for="item in filteredProducts"
        :key="item.id"
        class="bg-white border rounded-2xl shadow hover:shadow-lg transition p-5"
      >
        <div
          class="w-12 h-12 rounded-lg bg-green-100 text-green-700 flex items-center justify-center font-bold text-lg mb-4"
        >
          {{ item.name.charAt(0).toUpperCase() }}
        </div>

        <h2 class="text-lg font-semibold text-gray-800">{{ item.name }}</h2>
        <p class="text-sm text-gray-600 mt-1">
          💰 {{ item.price.toLocaleString() }} THB
        </p>
        <p class="text-sm text-gray-600">📦 Stock: {{ item.stockQuantity }}</p>
        <p class="text-sm mt-2">
          🏷️
          <span class="font-medium text-blue-600">
            {{ item.category?.name || "No category" }}
          </span>
        </p>

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

    <CommonBaseModal v-model="isEditOpen" :title="'Edit Product'" @save="saveEdit">
      <div>
        <label class="block text-sm font-medium mb-1">Product Name</label>
        <input v-model="selected.name" class="w-full border rounded-lg p-2" />
      </div>

      <div>
        <label class="block text-sm font-medium mb-1">Price</label>
        <input
          v-model.number="selected.price"
          type="number"
          class="w-full border rounded-lg p-2"
        />
      </div>

      <div>
        <label class="block text-sm font-medium mb-1">Stock Quantity</label>
        <input
          v-model.number="selected.stockQuantity"
          type="number"
          class="w-full border rounded-lg p-2"
        />
      </div>

      <div>
        <label class="block text-sm font-medium mb-1">Category</label>
        <select v-model="selected.categoryId" class="w-full border rounded-lg p-2">
          <option v-for="category in categories" :key="category.id" :value="category.id">
            {{ category.name }}
          </option>
        </select>
      </div>
    </CommonBaseModal>

    <CommonConfirmModal
      v-model="isDeleteOpen"
      title="🗑 Delete Product"
      :message="`Delete '${deleteTarget?.name}' ?`"
      :loading="isDeleting"
      @confirm="confirmDelete"
    />

    <CommonBaseModal v-model="showAddModal" :title="'Add Product'" @save="saveProduct">
      <div>
        <label class="block text-sm font-medium mb-1">Add Product Name</label>
        <input v-model="form.name" class="w-full border rounded-lg p-2" />
      </div>

      <div>
        <label class="block text-sm font-medium mb-1">Price</label>
        <input
          v-model.number="form.price"
          type="number"
          class="w-full border rounded-lg p-2"
        />
      </div>

      <div>
        <label class="block text-sm font-medium mb-1">Stock Quantity</label>
        <input
          v-model.number="form.stockQuantity"
          type="number"
          class="w-full border rounded-lg p-2"
        />
      </div>

      <div>
        <label class="block text-sm font-medium mb-1">Category</label>
        <select v-model="form.categoryId" class="w-full border rounded-lg p-2">
          <option v-for="category in categories" :key="category.id" :value="category.id">
            {{ category.name }}
          </option>
        </select>
      </div>
    </CommonBaseModal>
  </div>
</template>
