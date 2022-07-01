import axios from "axios";

export const httpClient = axios.create({
  baseURL: "https://distributed2022-rasmusvare.azurewebsites.net/api/v1/",
  headers: {
    "Content-Type": "application/json",
  },
});

export default httpClient;
