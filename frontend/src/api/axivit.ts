import axios from "axios";

const axivit = axios.create();

axivit.defaults.baseURL = "https://localhost:44342/api";
axivit.defaults.headers.common['Authorization'] = `Bearer ${localStorage['jwt']}`;


export default axivit;