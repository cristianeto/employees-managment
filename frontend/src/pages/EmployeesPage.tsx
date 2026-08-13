import { useState, type ChangeEvent } from 'react'
import { useQuery } from '@tanstack/react-query'
import {
  AppBar,
  Box,
  Button,
  CircularProgress,
  Container,
  FormControl,
  InputLabel,
  MenuItem,
  Select,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Toolbar,
  Typography,
  Paper,
  Alert,
} from '@mui/material'
import { useAuth } from '../auth/AuthContext'
import { listAreas } from '../api/areasApi'
import { listEmployees } from '../api/employeesApi'
import { AddEmployeeModal } from '../components/AddEmployeeModal'

const currencyFormatter = new Intl.NumberFormat('es-EC', { style: 'currency', currency: 'USD' })

export function EmployeesPage() {
  const { logout } = useAuth()
  const [area, setArea] = useState('')
  const [isModalOpen, setIsModalOpen] = useState(false)

  const areasQuery = useQuery({ queryKey: ['areas'], queryFn: listAreas })
  const employeesQuery = useQuery({
    queryKey: ['employees', area || null],
    queryFn: () => listEmployees(area || undefined),
  })

  function handleAreaChange(event: ChangeEvent<HTMLInputElement> | { target: { value: string } }) {
    setArea(event.target.value)
  }

  return (
    <Box>
      <AppBar position="static">
        <Toolbar sx={{ justifyContent: 'space-between' }}>
          <Typography variant="h6">Gestión de Empleados</Typography>
          <Button color="inherit" onClick={logout}>
            Cerrar sesión
          </Button>
        </Toolbar>
      </AppBar>

      <Container sx={{ py: 4 }}>
        <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'flex-start', mb: 3 }}>
          <FormControl sx={{ minWidth: 240 }} size="small">
            <InputLabel id="area-filter-label">Área</InputLabel>
            <Select
              labelId="area-filter-label"
              label="Área"
              value={area}
              onChange={handleAreaChange}
            >
              <MenuItem value="">Todas las áreas</MenuItem>
              {areasQuery.data?.map((areaOption) => (
                <MenuItem key={areaOption.id} value={areaOption.name}>
                  {areaOption.name}
                </MenuItem>
              ))}
            </Select>
          </FormControl>

          <Button variant="contained" onClick={() => setIsModalOpen(true)}>
            Agregar Empleado
          </Button>
        </Box>

        {employeesQuery.isError && (
          <Alert severity="error" sx={{ mb: 2 }}>
            Error al cargar los empleados.
          </Alert>
        )}

        <TableContainer component={Paper}>
          <Table>
            <TableHead>
              <TableRow>
                <TableCell>Documento</TableCell>
                <TableCell>Nombres</TableCell>
                <TableCell>Apellidos</TableCell>
                <TableCell align="right">Edad</TableCell>
                <TableCell align="right">Salario Mensual</TableCell>
                <TableCell>Área</TableCell>
                <TableCell>Cargo</TableCell>
              </TableRow>
            </TableHead>
            <TableBody>
              {employeesQuery.isLoading && (
                <TableRow>
                  <TableCell colSpan={7} align="center" sx={{ py: 4 }}>
                    <CircularProgress size={28} />
                  </TableCell>
                </TableRow>
              )}

              {employeesQuery.isSuccess && employeesQuery.data.length === 0 && (
                <TableRow>
                  <TableCell colSpan={7} align="center" sx={{ py: 4 }}>
                    No se encontraron empleados.
                  </TableCell>
                </TableRow>
              )}

              {employeesQuery.data?.map((employee) => (
                <TableRow key={employee.id}>
                  <TableCell>{employee.documentId}</TableCell>
                  <TableCell>{employee.firstName}</TableCell>
                  <TableCell>{employee.lastName}</TableCell>
                  <TableCell align="right">{employee.age}</TableCell>
                  <TableCell align="right">{currencyFormatter.format(employee.monthlySalary)}</TableCell>
                  <TableCell>{employee.areaName}</TableCell>
                  <TableCell>{employee.cargoName}</TableCell>
                </TableRow>
              ))}
            </TableBody>
          </Table>
        </TableContainer>
      </Container>

      <AddEmployeeModal open={isModalOpen} onClose={() => setIsModalOpen(false)} />
    </Box>
  )
}
