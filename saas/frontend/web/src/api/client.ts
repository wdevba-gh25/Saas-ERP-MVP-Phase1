// src/api/client.ts
import axios from "axios";
import { useAuthStore } from "../store/auth.store"; // ✅ Fix import

export const client = axios.create({
  baseURL: import.meta.env.VITE_API_BASE, // same as api.ts
  headers: { "Content-Type": "application/json" },
  withCredentials: true, // allow cookies if needed
});

// Attach Authorization header from Zustand on every request
client.interceptors.request.use((config) => {
  const token = useAuthStore.getState().token; // ✅ Fix: useAuthStore
  if (token) {
    config.headers = config.headers ?? {};
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

// Handle 401s globally (logout + redirect if you want)
client.interceptors.response.use(
  (res) => res,
  (error) => {
    if (error?.response?.status === 401) {
      useAuthStore.getState().logout?.(); // safe call if logout exists
    }
    return Promise.reject(error);
  }
);