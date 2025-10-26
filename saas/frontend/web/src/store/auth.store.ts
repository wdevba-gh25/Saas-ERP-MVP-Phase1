// src/store/auth.store.ts
import { create } from "zustand";

interface User {
  id: string;
  email: string;
  displayName?: string;
}

interface AuthState {
  token: string | null;
  user: User | null;
  authLoading: boolean;
  login: (token: string, user?: User | null) => void;
  logout: () => void;
  finishLoading: () => void;
}

export const useAuthStore = create<AuthState>((set) => ({
  token: null,
  user: null,
  authLoading: true, // ✅ start with loading true

  login: (token, user = null) =>
    set({
      token,
      user,
      authLoading: false,
    }),

  logout: () =>
    set({
      token: null,
      user: null,
      authLoading: false,
    }),

  finishLoading: () => set({ authLoading: false }),
}));