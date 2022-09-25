export default function checkAndConvertResponse(response) {
  if (response.status >= 200 && response.status <= 299) {
    return response.json();
  }

  return { isError: true };
}
