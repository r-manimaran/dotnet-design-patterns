using eCommerceApp.AppHost.Extensions;

var builder = DistributedApplication.CreateBuilder(args);

var sqlserverPassword = builder.AddParameter("sqlserverPassword", secret: true);
var sqlserver = builder.AddSqlServer("sqldb", sqlserverPassword, 40796)
                       .WithDataVolume()
                       .AddDatabase("eCommerce");

var apiReference = builder.AddProject<Projects.eCommerceApp_Host>("ecommerceapp-host");

apiReference
    .WithSwaggerUi()
    .WithScalarUi()
    .WithRedocUi()
    .WithReference(sqlserver)
    .WaitFor(sqlserver);

builder.Build().Run();
