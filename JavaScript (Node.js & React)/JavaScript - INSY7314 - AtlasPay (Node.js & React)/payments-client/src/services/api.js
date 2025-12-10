// src/services/api.js
import axios from "axios";

const api = axios.create({
  baseURL: "/api",
  withCredentials: true, // send/receive auth cookie
});

// --- CSRF helpers ---
export async function fetchCsrfToken() {
  const res = await api.get("/csrf-token");
  return res.data?.csrfToken;
}

export async function postWithCsrf(url, data = {}, config = {}) {
  const csrfToken = await fetchCsrfToken();
  return api.post(url, data, {
    ...config,
    headers: { ...(config.headers || {}), "X-CSRF-Token": csrfToken },
  });
}

export async function putWithCsrf(url, data = {}, config = {}) {
  const csrfToken = await fetchCsrfToken();
  return api.put(url, data, {
    ...config,
    headers: { ...(config.headers || {}), "X-CSRF-Token": csrfToken },
  });
}

export async function deleteWithCsrf(url, config = {}) {
  const csrfToken = await fetchCsrfToken();
  return api.delete(url, {
    ...config,
    headers: { ...(config.headers || {}), "X-CSRF-Token": csrfToken },
  });
}

export default api;
