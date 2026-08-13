import { httpClient } from './httpClient'

interface LoginResponse {
  token: string
}

export async function login(username: string, password: string): Promise<string> {
  const { data } = await httpClient.post<LoginResponse>('/api/auth/login', { username, password })
  return data.token
}
