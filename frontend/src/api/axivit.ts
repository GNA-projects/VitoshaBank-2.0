import axios from "axios";

const axivit = axios.create();

axivit.defaults.baseURL = "http://91.230.230.249/api";
axivit.defaults.headers.common['Authorization'] = `Bearer ${localStorage['jwt']}`;


export default axivit;