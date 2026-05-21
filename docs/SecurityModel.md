# Security Model

GatewayOps MCP follows a layered security model designed for controlled AI-assisted execution.

The goal is to reduce unauthorized access, unsafe execution, and operational risk.

---

# Security Layers

```text
Request
↓
Authentication
↓
Authorization
↓
Context
↓
Validation
↓
Policy Engine
↓
Execution Controls
↓
Audit
```

---

## 1. Authentication

Authentication verifies caller identity.

Current approach:

- JWT Bearer Authentication

Validated attributes:

- merchant identity
- client identity
- scopes

Example:

```text
Authorization: Bearer <token>
```

---

## 2. Authorization

Authorization determines what actions are permitted.

Examples:

```text
payments:read
payments:write
support:execute
```

Enforcement occurs before tool execution.

---

## 3. Request Context

After authentication:

```text
JWT
↓
RequestContext
```

Contains:

- MerchantId
- ClientId
- Scopes

This context becomes the execution boundary.

---

## 4. Request Validation

Before execution:

Validation checks:

- schema
- required fields
- parameter ranges
- malformed requests

Example:

```text
amount > 0
```

---

## 5. Policy Engine

Policies determine whether execution is allowed.

Example:

```text
CreatePaymentLink
↓
payments:write
↓
confirmation required
```

Controls:

- scope checks
- risk evaluation
- confirmation workflows

---

## 6. Confirmation Workflow

Write operations can require explicit confirmation.

Example:

```text
Generate payment link
↓
PendingAction
↓
Signed confirmation
↓
Execution
```

---

## 7. Runtime Protection

Execution safeguards:

- rate limiting
- execution locking
- timeout boundaries
- circuit breakers

Goals:

- prevent abuse
- isolate failures
- reduce retries

---

## 8. Audit & Observability

Execution events are recorded.

Examples:

- execution started
- execution completed
- execution rejected
- timeout
- policy denial

Metrics collected:

- latency
- retries
- failures
- usage

---

## 9. Secret Management

Public repository guidance:

Never commit:

- JWT secrets
- connection strings
- credentials
- internal endpoints

Secrets should be supplied through:

```text
Configuration
↓
Environment Variables
↓
Secret Manager
```

---

## Future Enhancements

Planned improvements:

- mTLS
- OAuth2 client credentials
- Redis-backed replay protection
- distributed rate limiting
- secret rotation
- KMS integration
- tenant isolation