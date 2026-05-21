# Request Lifecycle

This document explains how a request flows through GatewayOps MCP.

---

# Overview

GatewayOps MCP executes requests through a controlled pipeline designed for:

- secure execution
- validation
- policy enforcement
- observability
- safe tool orchestration

---

## End-to-End Flow

```text
Client
↓
Endpoint
↓
Middleware
↓
Context Creation
↓
Orchestrator
↓
Tool Resolution
↓
Parameter Extraction
↓
Validation
↓
Policy Evaluation
↓
Rate Limiting
↓
Execution Lock
↓
Tool Execution
↓
Audit
↓
Response
```

---

## 1. API Entry

Requests enter through Minimal API endpoints.

Example:

```http
POST /mcp
```

Input:

```json
{
  "input": "Create payment link for ₹500"
}
```

---

## 2. Middleware Execution

Pipeline:

```text
Correlation
↓
Logging
↓
Authentication
↓
Authorization
↓
Context
```

Responsibilities:

- authenticate caller
- build execution context
- assign correlation id

Output:

```text
RequestContext
```

---

## 3. Orchestration

The orchestrator coordinates execution.

Responsibilities:

- determine tool
- validate execution
- apply policies
- execute safely

Output:

```text
ToolExecutionContext
```

---

## 4. Resolution

Tool resolver determines:

```text
Input
↓
Semantic Matching
↓
Capability Scoring
↓
Tool Selection
```

Example:

```text
"Collect ₹500"
↓
CreatePaymentLink
```

---

## 5. Parameter Extraction

Extract structured data.

Example:

```text
Input:
Create payment request for ₹500

Output:

amount=500
```

---

## 6. Validation

Validation checks:

- schema
- ranges
- required fields

Example:

```text
amount > 0
```

---

## 7. Governance

Execution controls:

- policy engine
- confirmations
- write safeguards

Example:

```text
write operation
↓
confirmation required
```

---

## 8. Runtime Controls

Runtime protections:

- rate limiting
- timeout boundaries
- execution locks
- circuit breakers

---

## 9. Tool Execution

Selected tool executes:

```text
Tool
↓
Gateway Client
↓
Gateway APIs
```

---

## 10. Observability

Collected signals:

- metrics
- audit logs
- execution status
- latency

---

## Final Response

Example:

```json
{
  "response": "Payment link created",
  "data": {}
}
```