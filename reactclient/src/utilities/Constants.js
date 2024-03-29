const API_BASE_URL_DEVELOPMENT = "https://localhost:7001";
const API_BASE_URL_PRODUCTION = "https://efcoreapi.azurewebsites.net";

const ENDPOINTS = {
  LOGIN: "api/login",
  REGISTER: "api/users",
  LIST_OFFCIAL_MAPPINGS: "api/routemappings/official",
  LIST_USER_MAPPINGS: "api/routemappings/my",
  CREARE_MAPPING: "api/routemappings",
  UPDATE_MAPPING: "api/routemappings",
  DELETE_MAPPING: "api/routemappings",
  REDIRECT: "api/redirect"
};

const development = {
  API_URL_LOGIN: `${API_BASE_URL_DEVELOPMENT}/${ENDPOINTS.LOGIN}`,
  API_URL_REGISTER: `${API_BASE_URL_DEVELOPMENT}/${ENDPOINTS.REGISTER}`,
  API_URL_LIST_OFFCIAL_MAPPINGS: `${API_BASE_URL_DEVELOPMENT}/${ENDPOINTS.LIST_OFFCIAL_MAPPINGS}`,
  API_URL_LIST_USER_MAPPINGS: `${API_BASE_URL_DEVELOPMENT}/${ENDPOINTS.LIST_USER_MAPPINGS}`,
  API_URL_REDIRECT: `${API_BASE_URL_DEVELOPMENT}/${ENDPOINTS.REDIRECT}`,
  API_URL_CREARE_MAPPING: `${API_BASE_URL_DEVELOPMENT}/${ENDPOINTS.CREARE_MAPPING}`,
  API_URL_UPDATE_MAPPING: `${API_BASE_URL_DEVELOPMENT}/${ENDPOINTS.UPDATE_MAPPING}`,
  API_URL_DELETE_MAPPING: `${API_BASE_URL_DEVELOPMENT}/${ENDPOINTS.DELETE_MAPPING}`
};

const production = {
  API_URL_LOGIN: `${API_BASE_URL_PRODUCTION}/${ENDPOINTS.LOGIN}`,
  API_URL_REGISTER: `${API_BASE_URL_PRODUCTION}/${ENDPOINTS.REGISTER}`,
  API_URL_LIST_OFFCIAL_MAPPINGS: `${API_BASE_URL_PRODUCTION}/${ENDPOINTS.LIST_OFFCIAL_MAPPINGS}`,
  API_URL_LIST_USER_MAPPINGS: `${API_BASE_URL_PRODUCTION}/${ENDPOINTS.LIST_USER_MAPPINGS}`,
  API_URL_REDIRECT: `${API_BASE_URL_PRODUCTION}/${ENDPOINTS.REDIRECT}`,
  API_URL_CREARE_MAPPING: `${API_BASE_URL_PRODUCTION}/${ENDPOINTS.CREARE_MAPPING}`,
  API_URL_UPDATE_MAPPING: `${API_BASE_URL_PRODUCTION}/${ENDPOINTS.UPDATE_MAPPING}`,
  API_URL_DELETE_MAPPING: `${API_BASE_URL_PRODUCTION}/${ENDPOINTS.DELETE_MAPPING}`
};

const Constants = process.env.NODE_ENV === "development" ? development : production;

export default Constants;
