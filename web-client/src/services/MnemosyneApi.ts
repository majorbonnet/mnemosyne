import axios, { type AxiosInstance } from "axios";

// Creating an instance for axios to be used by the token interceptor service
const instance: AxiosInstance = axios.create({
  baseURL: `${import.meta.env.VITE_MNEMOSYNE_API_URL}/api`,
  headers: {
    "Content-Type": "application/json",
  },
});

export default instance;