import { useState, type FormEvent } from 'react'
import { useNavigate } from 'react-router-dom'
import { useAuth } from '../auth/AuthContext'

// Placeholder markup; the MUI login form lands in Group 6.
export function LoginPage() {
  const { login } = useAuth()
  const navigate = useNavigate()
  const [username, setUsername] = useState('')
  const [password, setPassword] = useState('')
  const [error, setError] = useState<string | null>(null)

  async function handleSubmit(event: FormEvent) {
    event.preventDefault()
    setError(null)
    try {
      await login(username, password)
      navigate('/employees', { replace: true })
    } catch {
      setError('Invalid username or password')
    }
  }

  return (
    <form onSubmit={handleSubmit}>
      <h1>Login</h1>
      <input value={username} onChange={(e) => setUsername(e.target.value)} placeholder="Username" />
      <input
        type="password"
        value={password}
        onChange={(e) => setPassword(e.target.value)}
        placeholder="Password"
      />
      <button type="submit">Sign in</button>
      {error && <p role="alert">{error}</p>}
    </form>
  )
}
