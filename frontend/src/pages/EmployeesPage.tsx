import { useAuth } from '../auth/AuthContext'

// Placeholder markup; the employee table and add-employee modal land in Group 7/8.
export function EmployeesPage() {
  const { logout } = useAuth()

  return (
    <div>
      <h1>Employees</h1>
      <button type="button" onClick={logout}>
        Log out
      </button>
    </div>
  )
}
