import { httpClient } from './httpClient'

export interface CargoDto {
  id: number
  name: string
}

export async function listCargos(): Promise<CargoDto[]> {
  const { data } = await httpClient.get<CargoDto[]>('/api/cargos')
  return data
}
