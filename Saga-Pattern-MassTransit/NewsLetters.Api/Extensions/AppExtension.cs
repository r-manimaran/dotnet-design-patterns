using MassTransit;
using Microsoft.EntityFrameworkCore;
using NewsLetters.Api.Database;
using NewsLetters.Api.Sagas;

namespace NewsLetters.Api.Extensions;

public static class AppExtension
{
    public static void AddDatabaseContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("postgres"));
        });
    }

    // Add Extension method to map MassTransit RabbitMQ
    public static void AddMassTransitRabbitMq(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();

            x.AddConsumers(typeof(Program).Assembly);

            x.AddSagaStateMachine<NewsletterOnboardingSaga, NewsletterOnboardingSagaData>()
                .EntityFrameworkRepository(r =>
                {
                    r.ExistingDbContext<AppDbContext>();

                    r.UsePostgres();

                });

            x.UsingRabbitMq((context, cfg) =>
            {

                cfg.Host(new Uri(configuration.GetConnectionString("messaging")!), h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                //cfg.UseOpenTelemetry();

                cfg.UseInMemoryOutbox(context);

                cfg.ConfigureEndpoints(context);
            });
        });
    }

    public static void AddFluentEmail(this IServiceCollection services, IConfiguration configuration)
    {
        var smtpSettings = configuration.GetSection("SmtpSettings");

        services.AddFluentEmail(smtpSettings["FromAddress"],
                                smtpSettings["FromName"])
                .AddSmtpSender(smtpSettings["Host"],
                               smtpSettings.GetValue<int>("Port"));
    }
}