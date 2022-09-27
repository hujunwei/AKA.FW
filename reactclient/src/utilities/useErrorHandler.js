import MDAlert from "components/MDAlert";

export default function useErrorHandler() {
  const renderMdAlert = () => <MDAlert color="error">Request failed</MDAlert>;

  const checkAndConvertResponseBody = (response) => {
    if (response.status >= 200 && response.status <= 299) {
        return response.json();
      }
  
      return { isError: true };
  };

  const checkResponseStatus = (response) => {
    if (response.status >= 200 && response.status <= 299) {
        return { isError: false };
      }
  
      return { isError: true };
  };

  return {
    renderAlert: renderMdAlert,
    checkAndConvertResponse: checkAndConvertResponseBody,
    checkResponse: checkResponseStatus
  };
}
