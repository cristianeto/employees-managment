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

const currencyFormatter = new Intl.NumberFormat('en-US', { style: 'currency', currency: 'USD' })

export function EmployeesPage() {
  const { logout } = useAuth()
  const [area, setArea] = useState('')

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
          <Typography variant="h6">Employee Management</Typography>
          <Button color="inherit" onClick={logout}>
            Log out
          </Button>
        </Toolbar>
      </AppBar>

      <Container sx={{ py: 4 }}>
        <FormControl sx={{ minWidth: 240, mb: 3 }} size="small">
          <InputLabel id="area-filter-label">Area</InputLabel>
          <Select
            labelId="area-filter-label"
            label="Area"
            value={area}
            onChange={handleAreaChange}
          >
            <MenuItem value="">All areas</MenuItem>
            {areasQuery.data?.map((areaOption) => (
              <MenuItem key={areaOption.id} value={areaOption.name}>
                {areaOption.name}
              </MenuItem>
            ))}
          </Select>
        </FormControl>

        {employeesQuery.isError && (
          <Alert severity="error" sx={{ mb: 2 }}>
            Failed to load employees.
          </Alert>
        )}

        <TableContainer component={Paper}>
          <Table>
            <TableHead>
              <TableRow>
                <TableCell>Document ID</TableCell>
                <TableCell>First Name</TableCell>
                <TableCell>Last Name</TableCell>
                <TableCell align="right">Age</TableCell>
                <TableCell align="right">Monthly Salary</TableCell>
                <TableCell>Area</TableCell>
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
                    No employees found.
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
    </Box>
  )
}
