# Outbox Pattern Implementation in .NET

## 📋 Table of Contents
- [What is the Outbox Pattern?](#what-is-the-outbox-pattern)
- [Problem Statement](#problem-statement)
- [Solution Overview](#solution-overview)
- [Project Structure](#project-structure)
- [Key Components](#key-components)
- [How It Works](#how-it-works)
- [Advantages](#advantages)
- [Prerequisites](#prerequisites)
- [Setup Instructions](#setup-instructions)
- [Running the Application](#running-the-application)
- [Testing the Implementation](#testing-the-implementation)
- [Code Examples](#code-examples)

## 🎯 What is the Outbox Pattern?

The **Outbox Pattern** is a design pattern used to ensure reliable message publishing in distributed systems. It guarantees that database changes and message publishing happen atomically, preventing data inconsistency issues.

### Real-World Analogy
Think of it like a **post office outbox**:
1. You write a letter (create data) and put it in your outbox
2. The postal service (background service) periodically checks your outbox
3. Letters are picked up and delivered (messages are published)
4. Once delivered, the letter is marked as sent

## 🚨 Problem Statement

In distributed systems, you often need to:
1. **Save data** to a database
2. **Publish a message** to notify other services

### The Challenge
```csharp
// ❌ This approach has problems!
await SaveOrderToDatabase(order);
await PublishOrderCreatedEvent(order); // What if this fails?
```

**What can go wrong?**
- Database save succeeds, but message publishing fails
- Message is published, but database save fails
- Network issues cause partial failures
- Data becomes inconsistent across services

## ✅ Solution Overview

The Outbox Pattern solves this by:
1. **Storing messages in the same database** as your business data
2. **Using database transactions** to ensure atomicity
3. **Publishing messages asynchronously** via a background service

```csharp
// ✅ Outbox Pattern approach
using var transaction = await connection.BeginTransactionAsync();
await SaveOrderToDatabase(order, transaction);
await SaveMessageToOutbox(orderCreatedEvent, transaction);
await transaction.CommitAsync(); // Both succeed or both fail
```

## 📁 Project Structure

```
Outbox-Pattern/
├── Messaging.Contracts/           # Shared message contracts
│   └── OrderCreatedIntegrationEvent.cs
├── Orders.Api/                    # Order service (Publisher)
│   ├── Orders/
│   │   ├── Order.cs
│   │   └── CreateOrderDto.cs
│   ├── Outbox/
│   │   ├── OutboxMessage.cs       # Outbox entity
│   │   ├── OutboxProcessor.cs     # Message processor
│   │   ├── OutboxBackgroundService.cs # Background service
│   │   └── OutboxExtensions.cs    # Helper methods
│   └── Program.cs
└── Shipping.Api/                  # Shipping service (Consumer)
    ├── Shipments/
    │   ├── Shipment.cs
    │   └── OrderCreatedIntegrationEventConsumer.cs
    └── Program.cs
```

## 🔧 Key Components

### 1. OutboxMessage Entity
```csharp
internal sealed class OutboxMessage
{
    public Guid Id { get; init; }
    public required string Type { get; init; }        // Message type
    public required string Content { get; init; }     // Serialized message
    public DateTime OccuredOnUtc { get; init; }       // When created
    public DateTime? ProcessedOnUtc { get; init; }    // When processed
    public string? Error { get; init; }               // Error details
}
```

### 2. OutboxProcessor
- Fetches unprocessed messages from database
- Deserializes and publishes messages
- Marks messages as processed
- Handles errors gracefully

### 3. OutboxBackgroundService
- Runs continuously in the background
- Calls OutboxProcessor periodically
- Ensures reliable message delivery

## 🔄 How It Works

### Step 1: Order Creation
```mermaid
sequenceDiagram
    participant Client
    participant OrdersAPI
    participant Database
    participant OutboxTable

    Client->>OrdersAPI: POST /orders
    OrdersAPI->>Database: BEGIN TRANSACTION
    OrdersAPI->>Database: INSERT order
    OrdersAPI->>OutboxTable: INSERT outbox_message
    OrdersAPI->>Database: COMMIT TRANSACTION
    OrdersAPI->>Client: 201 Created
```

### Step 2: Message Processing
```mermaid
sequenceDiagram
    participant BackgroundService
    participant OutboxTable
    participant MessageBroker
    participant ShippingAPI

    loop Every 7 seconds
        BackgroundService->>OutboxTable: SELECT unprocessed messages
        BackgroundService->>MessageBroker: PUBLISH message
        MessageBroker->>ShippingAPI: DELIVER message
        BackgroundService->>OutboxTable: UPDATE processed_on_utc
    end
```

## 🎯 Advantages

### ✅ **Guaranteed Delivery**
- Messages are stored in the database
- No message loss even if publishing fails initially
- Retry mechanism ensures eventual delivery

### ✅ **Atomicity**
- Database changes and message storage happen in same transaction
- Either both succeed or both fail
- No partial state issues

### ✅ **Reliability**
- Background service retries failed messages
- Error tracking and logging
- Graceful handling of temporary failures

### ✅ **Scalability**
- Asynchronous processing
- Batch processing for better performance
- Can handle high message volumes

### ✅ **Observability**
- Track message processing status
- Monitor failed messages
- Audit trail of all events

## 📋 Prerequisites

- **.NET 9.0** or later
- **PostgreSQL** database
- **RabbitMQ** message broker
- **Docker** (optional, for easy setup)

## 🚀 Setup Instructions

### 1. Clone the Repository
```bash
git clone <repository-url>
cd Outbox-Pattern
```

### 2. Start Dependencies with Docker
```bash
# Start PostgreSQL
docker run -d --name postgres \
  -e POSTGRES_PASSWORD=password \
  -e POSTGRES_DB=outbox_db \
  -p 5432:5432 postgres:15

# Start RabbitMQ
docker run -d --name rabbitmq \
  -p 5672:5672 -p 15672:15672 \
  rabbitmq:3-management
```

### 3. Update Connection Strings
Update `appsettings.json` in both APIs:

```json
{
  "ConnectionStrings": {
    "Database": "Host=localhost;Database=outbox_db;Username=postgres;Password=password",
    "Queue": "amqp://guest:guest@localhost:5672"
  }
}
```

### 4. Build the Solution
```bash
dotnet build
```

## ▶️ Running the Application

### Start Both Services
```bash
# Terminal 1 - Orders API
cd Orders.Api
dotnet run

# Terminal 2 - Shipping API  
cd Shipping.Api
dotnet run
```

The services will start on:
- **Orders API**: `https://localhost:7001`
- **Shipping API**: `https://localhost:7002`

## 🧪 Testing the Implementation

### 1. Create an Order
```bash
curl -X POST https://localhost:7001/orders \
  -H "Content-Type: application/json" \
  -d '{
    "customerName": "John Doe",
    "productName": "Laptop",
    "quantity": 1,
    "totalPrice": 999.99
  }'
```

### 2. Verify Database Records
Check the `orders` and `outbox_messages` tables:

```sql
-- Check created order
SELECT * FROM orders;

-- Check outbox message
SELECT * FROM outbox_messages;
```

### 3. Monitor Message Processing
Watch the console logs to see:
- Order creation
- Outbox message insertion
- Background service processing
- Message publishing
- Shipment creation

### 4. Check Processing Status
```sql
-- See processed messages
SELECT * FROM outbox_messages WHERE processed_on_utc IS NOT NULL;

-- See failed messages
SELECT * FROM outbox_messages WHERE error IS NOT NULL;
```

## 💻 Code Examples

### Creating an Order with Outbox
```csharp
app.MapPost("orders", async (CreateOrderDto orderDto, NpgsqlDataSource dataSource) =>
{
    var order = new Order
    {
        Id = Guid.NewGuid(),
        CustomerName = orderDto.CustomerName,
        ProductName = orderDto.ProductName,
        Quantity = orderDto.Quantity,
        TotalPrice = orderDto.TotalPrice,
        OrderDate = DateTime.UtcNow
    };

    using var connection = await dataSource.OpenConnectionAsync();
    using var transaction = await connection.BeginTransactionAsync();

    // Save order to database
    await connection.ExecuteAsync(insertOrderSql, order, transaction);

    // Create integration event
    var orderCreatedEvent = new OrderCreatedIntegrationEvent(order.Id, order.Id.ToString());

    // Save event to outbox (same transaction!)
    await connection.InsertOutboxMessage(orderCreatedEvent, transaction);
    
    await transaction.CommitAsync();

    return Results.Created($"orders/{order.Id}", order);
});
```

### Outbox Extension Method
```csharp
internal static async Task InsertOutboxMessage<T>(
    this IDbConnection connection,
    T message,
    IDbTransaction? transaction = default)
    where T: notnull
{
    var outboxMessage = new OutboxMessage
    {
        Id = Guid.NewGuid(),
        Type = message.GetType().FullName!,
        Content = JsonSerializer.Serialize(message),
        OccuredOnUtc = DateTime.UtcNow,
    };

    const string sql = """
        INSERT INTO outbox_messages (id, type, content, occured_on_utc)
        VALUES (@Id, @Type, @Content::jsonb, @OccuredOnUtc);
        """;
    
    await connection.ExecuteAsync(sql, outboxMessage, transaction);
}
```

### Message Consumer
```csharp
internal sealed class OrderCreatedIntegrationEventConsumer : IConsumer<OrderCreatedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<OrderCreatedIntegrationEvent> context)
    {
        var orderId = context.Message.OrderId;
        
        var shipment = new Shipment 
        { 
            Id = Guid.NewGuid(),
            OrderId = orderId,
            Status = ShipmentStatus.Pending.ToString(),
            CreatedAt = DateTime.UtcNow,
        };

        // Save shipment to database
        await connection.ExecuteAsync(insertShipmentSql, shipment);
        
        logger.LogInformation("Shipment created for order {OrderId}", orderId);
    }
}
```

## 🎓 Learning Points for Beginners

1. **Database Transactions**: Learn how ACID properties ensure data consistency
2. **Background Services**: Understand how to run tasks in the background
3. **Message Brokers**: See how services communicate asynchronously
4. **Error Handling**: Observe graceful failure handling and retries
5. **Distributed Systems**: Experience challenges of service coordination

## 🔍 Monitoring and Troubleshooting

### Common Issues
- **Messages not processing**: Check background service logs
- **Database connection errors**: Verify connection strings
- **RabbitMQ issues**: Ensure broker is running and accessible

### Useful Queries
```sql
-- Count unprocessed messages
SELECT COUNT(*) FROM outbox_messages WHERE processed_on_utc IS NULL;

-- Find failed messages
SELECT * FROM outbox_messages WHERE error IS NOT NULL;

-- Message processing rate
SELECT DATE_TRUNC('hour', processed_on_utc) as hour, COUNT(*) 
FROM outbox_messages 
WHERE processed_on_utc IS NOT NULL 
GROUP BY hour 
ORDER BY hour DESC;
```

## 📚 Further Reading

- [Microservices Patterns by Chris Richardson](https://microservices.io/patterns/data/transactional-outbox.html)
- [Event-Driven Architecture Patterns](https://docs.microsoft.com/en-us/azure/architecture/patterns/event-sourcing)
- [Distributed Systems Concepts](https://martinfowler.com/articles/patterns-of-distributed-systems/)

---

This implementation demonstrates a production-ready Outbox Pattern that ensures reliable message delivery in distributed systems while maintaining data consistency.