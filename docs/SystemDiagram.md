# System Diagram

```mermaid
flowchart TD

A[Client / AI Agent]

A --> B[GatewayOpsMcp.Api]

B --> C[Middleware]

C --> D[MCP Orchestrator]

D --> E[Tool Resolver]

D --> F[Policy Engine]

D --> G[Validation Engine]

E --> H[Tool Registry]

H --> I[Tool Execution]

I --> J[Gateway APIs]

D --> K[Audit Service]

D --> L[Metrics]

I --> M[Rate Limiter]

I --> N[Execution Lock]

I --> O[Circuit Breaker]
```

---

## Responsibilities

### API
Transport layer and endpoint handling.

### Middleware
Authentication, authorization, context, tracing.

### Orchestrator
Coordinates execution.

### Tool Layer
Business capabilities.

### Infrastructure
Observability, resilience, security.
