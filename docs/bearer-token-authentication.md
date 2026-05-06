# Bearer Token Authentication

## Overview

BienOblige supports an optional bearer-token authentication mechanism that you can enable at the API service level without modifying any code. When enabled, every incoming HTTP request must carry a valid bearer token in the `Authorization` header; requests that do not supply a recognised token are rejected with `HTTP 401 Unauthorized`.

## When to Use

Enable bearer token authentication when you need a lightweight, shared-secret approach to restrict access to the API service—for example in internal or private network deployments where mTLS or an identity provider is not already in place.

> **Note:** Bearer tokens are a shared-secret mechanism. Tokens should be treated with the same care as passwords: rotate them regularly, never commit them to source control, and prefer injecting them through environment variables or a secrets manager.

## Configuration

Authentication is controlled entirely through application configuration (e.g., `appsettings.json`, environment variables, or any other configuration provider supported by ASP.NET Core). **No code changes are required.**

### Disabling Authentication (Default)

By default the `ValidTokens` list is empty, which means the middleware is **not registered** and all requests are passed through without authentication.

```json
{
  "BearerTokenAuthentication": {
    "ValidTokens": []
  }
}
```

### Enabling Authentication

Populate `ValidTokens` with one or more accepted token values. As soon as at least one token is present the middleware is registered and every request must supply a matching token.

```json
{
  "BearerTokenAuthentication": {
    "ValidTokens": [
      "your-secret-token-1",
      "your-secret-token-2"
    ]
  }
}
```

For production deployments prefer environment-variable overrides so that tokens are never written to a file on disk:

```bash
# Single token
BienOblige__BearerTokenAuthentication__ValidTokens__0=your-secret-token-1

# Multiple tokens
BienOblige__BearerTokenAuthentication__ValidTokens__0=your-secret-token-1
BienOblige__BearerTokenAuthentication__ValidTokens__1=your-secret-token-2
```

### Configuration Key Reference

| Key | Type | Default | Description |
|-----|------|---------|-------------|
| `BearerTokenAuthentication:ValidTokens` | `string[]` | `[]` (empty) | List of tokens that are accepted. An empty list disables the middleware entirely. |

## Making Authenticated Requests

Clients must include the token in the `Authorization` header using the `Bearer` scheme:

```http
POST /activityinbox HTTP/1.1
Host: your-api-host
Authorization: Bearer your-secret-token-1
Content-Type: application/json

{ ... }
```

Both `Bearer <token>` and a bare `<token>` (without the scheme prefix) are accepted by the middleware, but the `Bearer` prefix is recommended for standards compliance (RFC 6750).

## Error Response

When a request is rejected the API returns `HTTP 401 Unauthorized` with a Problem Details body (RFC 9457):

```json
{
  "title": "Unauthorized",
  "detail": "Bearer token is missing or invalid",
  "status": 401,
  "instance": "/activityinbox"
}
```

## Rotating Tokens

1. Add the new token to `ValidTokens` alongside the existing token(s).
2. Update all clients to use the new token.
3. Remove the old token from `ValidTokens`.
4. Restart / redeploy the service.

This zero-downtime rotation approach ensures clients are never locked out during the transition.
