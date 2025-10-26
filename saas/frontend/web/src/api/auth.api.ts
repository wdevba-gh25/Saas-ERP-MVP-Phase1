import axios from "axios";

const API_BASE = import.meta.env.VITE_API_BASE || "";

export interface RegisterPayload {
  organizationId: string;
  email: string;
  displayName: string;
  password: string;
}

export interface LoginPayload {
  email: string;
  password: string;
}

// ---------------------------
// Register new user
// ---------------------------
export async function registerUser(payload: RegisterPayload) {
  const response = await axios.post(`${API_BASE}/api/auth/signup`, payload);
  return response.data; // { token }
}

// ---------------------------
// Login existing user
// ---------------------------
export async function loginUser(payload: LoginPayload) {
  const response = await axios.post(`${API_BASE}/api/auth/login`, payload);
  return response.data; // { token }
}

// ---------------------------
// Get current user info (optional)
// ---------------------------
export async function fetchCurrentUser(token: string) {
  const response = await axios.get(`${API_BASE}/api/auth/me`, {
    headers: { Authorization: `Bearer ${token}` }
  });
  return response.data;
}