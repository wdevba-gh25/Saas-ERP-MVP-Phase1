// src/api.ts
const API_BASE: string =
  import.meta.env.VITE_API_BASE || "http://localhost:5240/api";

/**
 * Wrapper around fetch that ensures:
 * - JSON headers
 * - Throws with useful error message when response is not ok
 */
export async function apiRequest<T>(
  path: string,
  options: RequestInit = {}
): Promise<T> {
  const url = `${API_BASE}${path}`;
  const res = await fetch(url, {
    headers: {
      "Content-Type": "application/json",
      ...(options.headers || {}),
    },
    credentials: options.credentials ?? "omit", // if you want cookies later
    ...options,
  });

  // Parse response safely
  const text = await res.text();
  let body: any = null;
  try {
    body = text ? JSON.parse(text) : null;
  } catch {
    body = text;
  }

  if (!res.ok) {
    const errMsg =
      (body && (body.detail || body.Message || body.error || body)) ||
      res.statusText ||
      "API request failed";
    throw new Error(errMsg);
  }

  return body as T;
}