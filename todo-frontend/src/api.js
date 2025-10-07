import axios from "axios";

export default axios.create({
  baseURL: "http://localhost:5000", // endpoint do backend C#
  // opcional: timeout: 5000
});