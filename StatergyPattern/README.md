# Strategy Pattern — .NET 9 Web API

## What is the Strategy Pattern?

The **Strategy Pattern** is a behavioral design pattern that lets you define a family of algorithms (strategies), put each one in a separate class, and make them interchangeable at runtime.

> In simple terms: instead of writing one big block of `if/else` or `switch` logic, you extract each behavior into its own class and swap them in and out as needed.

It follows the **Open/Closed Principle** — your code is open for extension (add new strategies) but closed for modification (no need to touch existing code).

---

## When Should You Use It?

- You have multiple variations of the same behavior (e.g., different payment methods, sorting algorithms, discount rules)
- You want to avoid large `if/else` or `switch` blocks that grow every time a new case is added
- You want to add new behaviors without changing existing code

---

## Real-World Analogy

Think of a navigation app like Google Maps. You can choose your travel mode — driving, walking, or cycling. Each mode is a different **strategy** for getting from A to B. The app doesn't change; only the strategy does.

---

## Project Overview

This project demonstrates the Strategy Pattern in a **.NET 9 Web API** for a payment processing system.

A single API endpoint `/pay` accepts a payment method name and amount, then routes to the correct payment strategy automatically — no `switch` statements needed.

---

## Project Structure

```
StatergyPattern/
└── WebApi/
    ├── Services/
    │   ├── IPaymentMethod.cs       # Strategy interface
    │   ├── IPaymentService.cs      # Context interface
    │   ├── PaymentService.cs       # Context — selects and runs the strategy
    │   ├── CreditCardPayment.cs    # Concrete strategy
    │   ├── PaypalPayment.cs        # Concrete strategy
    │   └── ApplePayPayment.cs      # Concrete strategy
    └── Program.cs                  # DI registration + API endpoint
```

---

## Pattern Breakdown

### 1. Strategy Interface — `IPaymentMethod`

Defines the contract that every payment strategy must follow.

```csharp
public interface IPaymentMethod
{
    string Method { get; }          // Unique name to identify this strategy
    string Pay(decimal amount);     // The actual behavior
}
```

Every payment method must have a name and know how to process a payment. That's it.

---

### 2. Concrete Strategies

Each class represents one payment method and implements `IPaymentMethod`.

```csharp
public sealed class CreditCardPayment : IPaymentMethod
{
    public string Method => "CreditCard";
    public string Pay(decimal amount) => $"[{Method}] Pay {amount:C}";
}

public sealed class PaypalPayment : IPaymentMethod
{
    public string Method => "Paypal";
    public string Pay(decimal amount) => $"[{Method}] Pay {amount:C}";
}

public sealed class ApplePayPayment : IPaymentMethod
{
    public string Method => "ApplePay";
    public string Pay(decimal amount) => $"[{Method}] Pay {amount:C}";
}
```

Each strategy is isolated in its own class. Adding a new payment method means adding a new class — nothing else changes.

---

### 3. Context — `PaymentService`

The context holds all registered strategies and picks the right one at runtime based on the method name.

```csharp
public sealed class PaymentService(IEnumerable<IPaymentMethod> paymentMethods) : IPaymentService
{
    public string? Process(string paymentMethod, decimal amount)
    {
        var method = paymentMethods.FirstOrDefault(s =>
            s.Method.Equals(paymentMethod, StringComparison.OrdinalIgnoreCase));

        return method is null ? null : method.Pay(amount);
    }
}
```

`PaymentService` doesn't know or care about the specific payment methods. It just finds the matching one and delegates the work.

---

### 4. Dependency Injection Registration — `Program.cs`

All strategies are registered with the DI container. ASP.NET Core automatically injects all `IPaymentMethod` implementations into `PaymentService`.

```csharp
builder.Services.AddScoped<IPaymentService, PaymentService>();

builder.Services.AddScoped<IPaymentMethod, CreditCardPayment>();
builder.Services.AddScoped<IPaymentMethod, PaypalPayment>();
builder.Services.AddScoped<IPaymentMethod, ApplePayPayment>();
```

---

## The Problem It Solves

### ❌ Without Strategy Pattern (hard-coded switch)

```csharp
public string? Process(string paymentMethod, decimal amount)
{
    return paymentMethod.ToLower() switch
    {
        "creditcard" => $"[CreditCard] Pay {amount:C}",
        "paypal"     => $"[PayPal] Pay {amount:C}",
        _            => null
    };
}
```

Every time you add a new payment method, you must open and modify this file. This violates the Open/Closed Principle and makes the code harder to maintain and test.

### ✅ With Strategy Pattern

Adding a new payment method (e.g., Google Pay) requires only:

1. Create a new class:
```csharp
public sealed class GooglePayPayment : IPaymentMethod
{
    public string Method => "GooglePay";
    public string Pay(decimal amount) => $"[{Method}] Pay {amount:C}";
}
```

2. Register it in `Program.cs`:
```csharp
builder.Services.AddScoped<IPaymentMethod, GooglePayPayment>();
```

That's it. No existing code is touched.

---

## API Usage

### Endpoint

```
GET /pay?method={paymentMethod}&amount={amount}
```

### Examples

```
GET /pay?method=CreditCard&amount=100
→ 200 OK: "[CreditCard] Pay $100.00"

GET /pay?method=Paypal&amount=50
→ 200 OK: "[Paypal] Pay $50.00"

GET /pay?method=ApplePay&amount=75
→ 200 OK: "[ApplePay] Pay $75.00"

GET /pay?method=Bitcoin&amount=200
→ 400 Bad Request: "Invalid Payment method: Bitcoin"
```

---

## How to Run

**Prerequisites:** .NET 9 SDK

```bash
cd WebApi
dotnet run
```

The API will be available at `https://localhost:{port}`. In development, the OpenAPI spec is available at `/openapi/v1.json`.

---

## Key Takeaways

| Concept | Description |
|---|---|
| Strategy Interface | Defines what every strategy must do |
| Concrete Strategy | One specific implementation of the behavior |
| Context | Uses a strategy without knowing its details |
| Open/Closed Principle | Add new strategies without modifying existing code |
| Dependency Injection | Makes strategy registration and swapping clean and testable |

---

## Tech Stack

- .NET 9
- ASP.NET Core Web API
- Microsoft.AspNetCore.OpenApi
