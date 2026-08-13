## ADDED Requirements

### Requirement: Normalized employee data model
The system SHALL store employees with a unique national ID document number, first name, last name, age, and monthly salary, and SHALL associate each employee with exactly one Area and exactly one Cargo (Position) via foreign keys rather than duplicated text fields. Area and Cargo SHALL each be stored in their own table so that renaming an Area or Cargo requires updating a single row.

#### Scenario: Renaming an Area updates all its employees
- **WHEN** an administrator updates the `Name` of an existing Area row (e.g., "Sistemas" → "Tecnología")
- **THEN** every employee previously associated with that Area reflects the new name on next read, without any per-employee row being modified

#### Scenario: Duplicate document ID rejected
- **WHEN** a new employee is created with a `DocumentId` that already exists on another employee
- **THEN** the system rejects the creation with a validation error and does not insert a duplicate row

### Requirement: List all employees
The system SHALL provide an operation that returns all employees, including each employee's Area name and Cargo name (not just their IDs).

#### Scenario: List all employees returns full roster
- **WHEN** a caller requests the list-all-employees operation with no filter
- **THEN** the system returns every employee currently stored, each including document ID, first name, last name, age, monthly salary, Area name, and Cargo name

### Requirement: Create a new employee
The system SHALL provide an operation to create a new employee given a document ID, first name, last name, age, monthly salary, an Area reference, and a Cargo reference.

#### Scenario: Successful employee creation
- **WHEN** a caller submits valid employee data referencing an existing Area and an existing Cargo
- **THEN** the system persists the new employee and the employee subsequently appears in the list-all-employees result

#### Scenario: Creation with unknown Area or Cargo rejected
- **WHEN** a caller submits employee data referencing an Area ID or Cargo ID that does not exist
- **THEN** the system rejects the creation with a validation error and does not persist the employee

### Requirement: Filter employees by area
The system SHALL provide an operation to retrieve only the employees belonging to a specified Area (department), backed by a stored procedure.

#### Scenario: Filter returns only matching employees
- **WHEN** a caller requests employees filtered by an Area name that has associated employees
- **THEN** the system returns only the employees belonging to that Area, with the same fields as the list-all operation

#### Scenario: Filter on area with no employees returns empty result
- **WHEN** a caller requests employees filtered by an Area name that currently has no employees assigned
- **THEN** the system returns an empty result set rather than an error

### Requirement: Employee queries backed by a stored procedure
The system SHALL expose a single SQL Server stored procedure that both the list-all and filter-by-area operations use to read employee data, joined with Area and Cargo names.

#### Scenario: Stored procedure called without a filter parameter
- **WHEN** the stored procedure is invoked with no Area parameter (or a NULL parameter)
- **THEN** it returns all employees joined with their Area and Cargo names

#### Scenario: Stored procedure called with an Area parameter
- **WHEN** the stored procedure is invoked with a specific Area name parameter
- **THEN** it returns only employees belonging to that Area, joined with their Area and Cargo names

### Requirement: Employee list screen
The frontend SHALL display an authenticated employee list screen showing all employees in a table, loading the list-all data automatically when the screen is entered, and offering a way to filter the visible employees by Area.

#### Scenario: List loads on screen entry
- **WHEN** an authenticated user navigates to the employee list screen
- **THEN** the frontend calls the list-all-employees endpoint automatically and renders the results in a table without requiring further user action

#### Scenario: Filtering by area updates the table
- **WHEN** an authenticated user selects an Area to filter by
- **THEN** the frontend calls the filter-by-area endpoint and updates the table to show only employees in that Area

### Requirement: Add employee modal
The frontend SHALL provide a modal form, reachable from the employee list screen, for registering a new employee, and SHALL refresh the employee list after a successful submission.

#### Scenario: Successful submission refreshes the list
- **WHEN** an authenticated user fills in valid employee data in the add-employee modal and submits it
- **THEN** the frontend calls the create-employee endpoint, closes the modal on success, and the newly created employee appears in the employee list without a manual page reload
