## ADDED Requirements

### Requirement: Preconfigured test user
The system SHALL have exactly one preconfigured test user available for authentication, provisioned by the system (e.g., via database seed) rather than through a self-service registration flow.

#### Scenario: Test user exists after provisioning
- **WHEN** the system's database has been provisioned (migrated and seeded)
- **THEN** a login attempt using the preconfigured test user's credentials succeeds

### Requirement: Bearer token login
The system SHALL provide a login operation that accepts a username and password and, on success, returns a signed Bearer token (JWT) that identifies the authenticated session.

#### Scenario: Successful login returns a token
- **WHEN** a caller submits the preconfigured test user's correct username and password
- **THEN** the system returns a signed JWT bearer token in the response

#### Scenario: Invalid credentials rejected
- **WHEN** a caller submits an incorrect username or password
- **THEN** the system rejects the request with an authentication error and does not return a token

### Requirement: Protected employee endpoints
The system SHALL require a valid Bearer token on all employee-directory operations (list, filter, create) and SHALL reject requests missing a valid token.

#### Scenario: Request without token rejected
- **WHEN** a caller invokes the list-all-employees, filter-by-area, or create-employee operation without an `Authorization: Bearer` header
- **THEN** the system rejects the request with an unauthorized error and performs no data operation

#### Scenario: Request with valid token accepted
- **WHEN** a caller invokes an employee-directory operation with a valid, unexpired Bearer token obtained from login
- **THEN** the system processes the request normally

### Requirement: Login screen
The frontend SHALL provide a login form where a user submits a username and password, and on success SHALL store the returned token and redirect to the employee list screen.

#### Scenario: Successful login redirects to employee list
- **WHEN** a user submits valid credentials on the login form
- **THEN** the frontend stores the returned Bearer token and navigates to the employee list screen

#### Scenario: Failed login shows an error
- **WHEN** a user submits invalid credentials on the login form
- **THEN** the frontend displays an error message and does not navigate away from the login screen

### Requirement: Route protection
The frontend SHALL prevent access to the employee list screen without a valid stored token, redirecting unauthenticated users to the login screen.

#### Scenario: Unauthenticated access redirected to login
- **WHEN** a user without a stored Bearer token attempts to navigate directly to the employee list screen
- **THEN** the frontend redirects them to the login screen instead of rendering the employee list
