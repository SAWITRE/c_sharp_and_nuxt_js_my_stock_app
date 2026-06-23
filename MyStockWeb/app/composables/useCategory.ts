import type { Category } from "~/types/category";

export const useCategory = () => {
  const config = useRuntimeConfig();

  const getAll = () =>
    $fetch<Category[]>(`${config.public.apiBase}/api/categories`);

  const create = (data: { name: string; description?: string }) =>
    $fetch(`${config.public.apiBase}/api/categories`, {
      method: "POST",
      body: data,
    });

  return {
    getAll,
    create,
  };
};
