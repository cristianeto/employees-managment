import { httpClient } from './httpClient'

export interface EmployeeDto {
  id: number
  documentId: string
  firstName: string
  lastName: string
  age: number
  monthlySalary: number
  areaId: number
  areaName: string
  cargoId: number
  cargoName: string
}

export async function listEmployees(area?: string): Promise<EmployeeDto[]> {
  const { data } = await httpClient.get<EmployeeDto[]>('/api/employees', {
    params: area ? { area } : undefined,
  })
  return data
}

export interface CreateEmployeeRequest {
  documentId: string
  firstName: string
  lastName: string
  age: number
  monthlySalary: number
  areaId: number
  cargoId: number
}

export async function createEmployee(request: CreateEmployeeRequest): Promise<EmployeeDto> {
  const { data } = await httpClient.post<EmployeeDto>('/api/employees', request)
  return data
}
