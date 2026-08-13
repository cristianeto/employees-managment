import { useState, type FormEvent } from 'react'
import { useNavigate } from 'react-router-dom'
import { isAxiosError } from 'axios'
import { Alert, Box, Button, CircularProgress, Paper, TextField, Typography } from '@mui/material'
import { useAuth } from '../auth/AuthContext'

export function LoginPage() {
  const { login } = useAuth()
  const navigate = useNavigate()
  const [username, setUsername] = useState('')
  const [password, setPassword] = useState('')
  const [touched, setTouched] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const [isSubmitting, setIsSubmitting] = useState(false)

  const usernameError = touched && username.trim() === ''
  const passwordError = touched && password === ''

  async function handleSubmit(event: FormEvent) {
    event.preventDefault()
    setTouched(true)
    setError(null)

    if (username.trim() === '' || password === '') {
      return
    }

    setIsSubmitting(true)
    try {
      await login(username, password)
      navigate('/employees', { replace: true })
    } catch (err) {
      if (isAxiosError(err) && err.response === undefined) {
        setError('Unable to reach the server. Please try again.')
      } else {
        setError('Invalid username or password.')
      }
    } finally {
      setIsSubmitting(false)
    }
  }

  return (
    <Box
      sx={{
        display: 'flex',
        minHeight: '100vh',
        alignItems: 'center',
        justifyContent: 'center',
        bgcolor: 'grey.100',
        px: 2,
      }}
    >
      <Paper component="form" onSubmit={handleSubmit} elevation={3} sx={{ p: 4, width: '100%', maxWidth: 360 }}>
        <Typography variant="h5" component="h1" gutterBottom>
          Employee Management
        </Typography>
        <Typography variant="body2" color="text.secondary" sx={{ mb: 3 }}>
          Sign in to continue
        </Typography>

        {error && (
          <Alert severity="error" sx={{ mb: 2 }}>
            {error}
          </Alert>
        )}

        <TextField
          label="Username"
          fullWidth
          margin="normal"
          value={username}
          onChange={(event) => setUsername(event.target.value)}
          error={usernameError}
          helperText={usernameError ? 'Username is required' : ' '}
          disabled={isSubmitting}
          autoFocus
        />
        <TextField
          label="Password"
          type="password"
          fullWidth
          margin="normal"
          value={password}
          onChange={(event) => setPassword(event.target.value)}
          error={passwordError}
          helperText={passwordError ? 'Password is required' : ' '}
          disabled={isSubmitting}
        />

        <Button type="submit" variant="contained" fullWidth size="large" sx={{ mt: 2 }} disabled={isSubmitting}>
          {isSubmitting ? <CircularProgress size={24} color="inherit" /> : 'Sign in'}
        </Button>
      </Paper>
    </Box>
  )
}
