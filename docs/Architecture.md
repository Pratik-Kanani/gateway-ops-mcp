# Architecture

## Overview

GatewayOps MCP follows a layered architecture designed for secure AI-driven execution of payment operations.

Goals:

- Safe execution
- Governance-first design
- Extensible tool ecosystem
- Operational resilience
- Cloud-native deployment

---

## High-Level Flow

```text
Client / AI Agent
        │
        ▼
API Layer
        │
Middleware
(Auth / Context / Logging)
        │
        ▼
MCP Orchestrator
        │
 ┌──────┼────────┐
 ▼      ▼        ▼
Policy  Tool     Validation
Engine  Resolver Engine
 │
 ▼
Tool Registry
 │
 ▼
Gateway APIs
```

---

## Solution Structure

```text
src/

GatewayOpsMcp.Api
    Endpoints
    Middleware
    Extensions

GatewayOpsMcp.Core
    Models
    Interfaces
    Contracts
    Orchestration

GatewayOpsMcp.Infrastructure
    Services
    Clients
    Security
    Caching
    Resilience
    Observability
    Persistence

GatewayOpsMcp.Tools
    Definitions
    Implementations
    Metadata
    Schemas
    Workflows
```

---

## Request Lifecycle

### 1. Request Entry

Incoming request enters through Minimal APIs.

---

### 2. Middleware Pipeline

Responsibilities:

- Authentication
- Authorization
- Context enrichment
- Logging
- Correlation

---

### 3. Orchestration

Responsibilities:

- Resolve tool
- Extract parameters
- Validate input
- Apply policies
- Execute workflows

---

### 4. Tool Execution

Responsibilities:

- Call gateway services
- Execute business logic
- Return structured responses

---

### 5. Observability

Responsibilities:

- Metrics
- Audit logs
- Tracing
- Execution visibility

---

## Security Model

Layers:

- JWT Authentication
- HMAC Signing
- Policy Engine
- Confirmation Workflows
- Rate Limiting
- Concurrency Protection

---

## Deployment Target

Designed for:

- Container deployment
- ECS / Fargate
- Distributed caching
- Horizontal scaling