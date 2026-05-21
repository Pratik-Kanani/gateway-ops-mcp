# GatewayOps MCP

AI-native operations platform for payment gateways and fintech systems.

GatewayOps MCP is a governed execution layer that enables AI agents and applications to safely interact with payment operations through structured tools, policy controls, and resilient execution pipelines.

---

## Why GatewayOps MCP

Modern payment support workflows often require:

- Transaction lookup
- Operational diagnostics
- Payment request generation
- Controlled write operations
- Safe execution and approvals

GatewayOps MCP provides a structured interface for exposing these capabilities to AI systems while maintaining governance, observability, and execution controls.

---

## Architecture Overview

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

## Docker

Build:

```bash
docker build -f docker/Dockerfile -t gatewayops .
```

Run:

```bash
docker compose -f docker/docker-compose.yml up
```

Health:

```http
GET http://localhost:5000/health
```

## API Examples

Import Postman collection:

```text
postman/GatewayOpsMcp.postman_collection.json
```

Example flows:

- Health
- Tool Discovery
- Diagnose Transaction
- Create Payment Link

# GatewayOps MCP

![.NET](https://img.shields.io/badge/.NET-8-blue)
![ASP.NET](https://img.shields.io/badge/ASP.NET-Core-green)
![Docker](https://img.shields.io/badge/Docker-ready-blue)
![Architecture](https://img.shields.io/badge/Architecture-Layered-orange)
![Status](https://img.shields.io/badge/Status-Prototype-yellow)