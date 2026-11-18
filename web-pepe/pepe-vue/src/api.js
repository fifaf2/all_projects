export const API = 'http://localhost:5096/api'

export function showError(error) {
  const data = error.response && error.response.data
  if (data && data.message) {
    alert(data.message)
  } else {
    alert('Ошибка соединения с сервером')
  }
}
