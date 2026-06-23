<template>
  <div>
    <header class="bg-slate-800 text-white p-4">
      <div class="container mx-auto flex gap-4">
        <NuxtLink to="/">Home</NuxtLink>
        <NuxtLink to="/category">Category</NuxtLink>
        <NuxtLink to="/product">Products</NuxtLink>
      </div>
    </header>

    <main class="container mx-auto py-6">
      <slot />
    </main>
  </div>
</template>