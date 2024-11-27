## Using Clean Architecture with MediatR in .Net

1. Domain
2. Application
3. Persistence / Infrastructure
4. Api

**For Database Migration**:

```bash
> cd src/External/Persistence
> dotnet ef migrations add InitialCreate --startup-project ../Api/Api.csproj
```

![alt text](image-2.png)

![alt text](image.png)

![alt text](image-1.png)

![alt text](image-3.png)

![alt text](image-4.png)