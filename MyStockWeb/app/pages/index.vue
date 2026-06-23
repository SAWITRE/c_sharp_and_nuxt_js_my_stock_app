<script setup lang="ts">
const config = useRuntimeConfig();

// fetch categories
const { data: categories } = await useFetch<any[]>(
  `${config.public.apiBase}/api/categories`
);

// fetch products
const { data: products } = await useFetch<any[]>(
  `${config.public.apiBase}/api/products`
);

// computed summary
const categoryCount = computed(() => categories.value?.length || 0);
const productCount = computed(() => products.value?.length || 0);

const totalStock = computed(() => {
  return (
    products.value?.reduce(
      (sum, p) => sum + (p.stockQuantity || 0),
      0
    ) || 0
  );
});
</script>

<template>
  <div class="p-6">
    <!-- Title -->
    <h1 class="text-3xl font-bold text-gray-800 mb-6">
      🏠 Dashboard
    </h1>

    <!-- Cards -->
    <div class="grid grid-cols-1 md:grid-cols-3 gap-6">

      <!-- Category -->
      <div class="bg-white rounded-2xl shadow p-6 border">
        <p class="text-gray-500">Categories</p>
        <h2 class="text-3xl font-bold text-blue-600">
          {{ categoryCount }}
        </h2>
      </div>

      <!-- Product -->
      <div class="bg-white rounded-2xl shadow p-6 border">
        <p class="text-gray-500">Products</p>
        <h2 class="text-3xl font-bold text-green-600">
          {{ productCount }}
        </h2>
      </div>

      <!-- Stock -->
      <div class="bg-white rounded-2xl shadow p-6 border">
        <p class="text-gray-500">Total Stock</p>
        <h2 class="text-3xl font-bold text-purple-600">
          {{ totalStock }}
        </h2>
      </div>

    </div>

    <!-- Quick links -->
    <div class="mt-8 grid grid-cols-1 md:grid-cols-2 gap-4">

      <NuxtLink
        to="/category"
        class="bg-blue-600 text-white p-5 rounded-2xl shadow hover:bg-blue-700"
      >
        📦 Manage Categories
      </NuxtLink>

      <NuxtLink
        to="/product"
        class="bg-green-600 text-white p-5 rounded-2xl shadow hover:bg-green-700"
      >
        🛒 Manage Products
      </NuxtLink>

    </div>
  </div>
</template>