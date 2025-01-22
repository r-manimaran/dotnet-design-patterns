var builder = DistributedApplication.CreateBuilder(args);


var postgres = builder.AddPostgres("postgres")
                .WithDataVolume()
                .WithPgAdmin();

postgres.AddDatabase("newsletters");

// get the password from config
var password = builder.AddParameter("password");
var rabbitmq = builder.AddRabbitMQ("messaging", password:password)
                .WithManagementPlugin();
                
builder.AddProject<Projects.NewsLetters_Api>("newsletters-api")
       .WaitFor(postgres)
       .WithReference(postgres)
       .WaitFor(rabbitmq)
       .WithReference(rabbitmq);

builder.Build().Run();
