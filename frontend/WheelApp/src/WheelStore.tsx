export const setAccessToken = (token: string) => {
  localStorage.setItem("AccessToken", token)
};

export const getAccessToken = () => {
  const token = localStorage.getItem("AccessToken")
  return token
};

export const delAccessToken = () => {
  localStorage.removeItem("AccessToken")
}

export const setRefreshToken = (token: string) => {
  localStorage.setItem("RefreshToken", token)
};

export const getRefreshToken = () => {
  const token = localStorage.getItem("RefreshToken")
  return token
};

export const delRefreshToken = () => {
  localStorage.removeItem("RefreshToken")
}
