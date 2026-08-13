import { httpClient } from './httpClient'

export interface AreaDto {
  id: number
  name: string
}

export async function listAreas(): Promise<AreaDto[]> {
  const { data } = await httpClient.get<AreaDto[]>('/api/areas')
  return data
}
