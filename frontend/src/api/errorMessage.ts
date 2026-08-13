import { isAxiosError } from 'axios'

interface ProblemDetailsResponse {
  title?: string
  detail?: string
  errors?: Record<string, string[]>
}

export function getErrorMessage(error: unknown, fallback: string): string {
  if (!isAxiosError<ProblemDetailsResponse>(error)) {
    return fallback
  }

  const data = error.response?.data
  if (data?.errors) {
    const firstError = Object.values(data.errors)[0]?.[0]
    if (firstError) {
      return firstError
    }
  }

  return data?.detail ?? data?.title ?? fallback
}
