import axios from "axios";

export const httpClient = axios.create({
  baseURL: "https://localhost:7088/api/v1/",
  headers: {
    "Content-Type": "application/json",
  },
});

export default httpClient;
