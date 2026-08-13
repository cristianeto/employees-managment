import { useState, type FormEvent } from 'react'
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import {
  Alert,
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  FormControl,
  InputLabel,
  MenuItem,
  Select,
  Stack,
  TextField,
} from '@mui/material'
import { listAreas } from '../api/areasApi'
import { listCargos } from '../api/cargosApi'
import { createEmployee } from '../api/employeesApi'
import { getErrorMessage } from '../api/errorMessage'

interface AddEmployeeModalProps {
  open: boolean
  onClose: () => void
}

interface FormState {
  documentId: string
  firstName: string
  lastName: string
  age: string
  monthlySalary: string
  areaId: string
  cargoId: string
}

const emptyForm: FormState = {
  documentId: '',
  firstName: '',
  lastName: '',
  age: '',
  monthlySalary: '',
  areaId: '',
  cargoId: '',
}

export function AddEmployeeModal({ open, onClose }: AddEmployeeModalProps) {
  const queryClient = useQueryClient()
  const [form, setForm] = useState<FormState>(emptyForm)
  const [touched, setTouched] = useState(false)

  const areasQuery = useQuery({ queryKey: ['areas'], queryFn: listAreas })
  const cargosQuery = useQuery({ queryKey: ['cargos'], queryFn: listCargos })

  const mutation = useMutation({
    mutationFn: createEmployee,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['employees'] })
      handleClose()
    },
  })

  const errors = {
    documentId: touched && form.documentId.trim() === '',
    firstName: touched && form.firstName.trim() === '',
    lastName: touched && form.lastName.trim() === '',
    age: touched && (Number(form.age) < 1 || Number(form.age) > 120 || form.age === ''),
    monthlySalary: touched && (Number(form.monthlySalary) <= 0 || form.monthlySalary === ''),
    areaId: touched && form.areaId === '',
    cargoId: touched && form.cargoId === '',
  }
  const hasErrors = Object.values(errors).some(Boolean)

  function handleClose() {
    setForm(emptyForm)
    setTouched(false)
    mutation.reset()
    onClose()
  }

  function handleSubmit(event: FormEvent) {
    event.preventDefault()
    setTouched(true)
    if (hasErrors) {
      return
    }

    mutation.mutate({
      documentId: form.documentId.trim(),
      firstName: form.firstName.trim(),
      lastName: form.lastName.trim(),
      age: Number(form.age),
      monthlySalary: Number(form.monthlySalary),
      areaId: Number(form.areaId),
      cargoId: Number(form.cargoId),
    })
  }

  return (
    <Dialog open={open} onClose={handleClose} fullWidth maxWidth="sm">
      <DialogTitle>Agregar Empleado</DialogTitle>
      <form onSubmit={handleSubmit}>
        <DialogContent>
          <Stack spacing={2} sx={{ mt: 1 }}>
            {mutation.isError && (
              <Alert severity="error">{getErrorMessage(mutation.error, 'Error al crear el empleado.')}</Alert>
            )}

            <TextField
              label="Documento de Identidad"
              value={form.documentId}
              onChange={(e) => setForm({ ...form, documentId: e.target.value })}
              error={errors.documentId}
              helperText={errors.documentId ? 'El documento es obligatorio' : ' '}
              disabled={mutation.isPending}
              fullWidth
            />
            <Stack direction="row" spacing={2}>
              <TextField
                label="Nombres"
                value={form.firstName}
                onChange={(e) => setForm({ ...form, firstName: e.target.value })}
                error={errors.firstName}
                helperText={errors.firstName ? 'Obligatorio' : ' '}
                disabled={mutation.isPending}
                fullWidth
              />
              <TextField
                label="Apellidos"
                value={form.lastName}
                onChange={(e) => setForm({ ...form, lastName: e.target.value })}
                error={errors.lastName}
                helperText={errors.lastName ? 'Obligatorio' : ' '}
                disabled={mutation.isPending}
                fullWidth
              />
            </Stack>
            <Stack direction="row" spacing={2}>
              <TextField
                label="Edad"
                type="number"
                value={form.age}
                onChange={(e) => setForm({ ...form, age: e.target.value })}
                error={errors.age}
                helperText={errors.age ? 'Debe estar entre 1 y 120' : ' '}
                disabled={mutation.isPending}
                fullWidth
              />
              <TextField
                label="Salario Mensual"
                type="number"
                value={form.monthlySalary}
                onChange={(e) => setForm({ ...form, monthlySalary: e.target.value })}
                error={errors.monthlySalary}
                helperText={errors.monthlySalary ? 'Debe ser mayor a 0' : ' '}
                disabled={mutation.isPending}
                fullWidth
              />
            </Stack>

            <FormControl error={errors.areaId} fullWidth>
              <InputLabel id="add-employee-area-label">Área</InputLabel>
              <Select
                labelId="add-employee-area-label"
                label="Área"
                value={form.areaId}
                onChange={(e) => setForm({ ...form, areaId: e.target.value })}
                disabled={mutation.isPending}
              >
                {areasQuery.data?.map((area) => (
                  <MenuItem key={area.id} value={String(area.id)}>
                    {area.name}
                  </MenuItem>
                ))}
              </Select>
            </FormControl>

            <FormControl error={errors.cargoId} fullWidth>
              <InputLabel id="add-employee-cargo-label">Cargo</InputLabel>
              <Select
                labelId="add-employee-cargo-label"
                label="Cargo"
                value={form.cargoId}
                onChange={(e) => setForm({ ...form, cargoId: e.target.value })}
                disabled={mutation.isPending}
              >
                {cargosQuery.data?.map((cargo) => (
                  <MenuItem key={cargo.id} value={String(cargo.id)}>
                    {cargo.name}
                  </MenuItem>
                ))}
              </Select>
            </FormControl>
          </Stack>
        </DialogContent>
        <DialogActions>
          <Button onClick={handleClose} disabled={mutation.isPending}>
            Cancelar
          </Button>
          <Button type="submit" variant="contained" disabled={mutation.isPending}>
            Guardar
          </Button>
        </DialogActions>
      </form>
    </Dialog>
  )
}
