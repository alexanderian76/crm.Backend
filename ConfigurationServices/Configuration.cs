using System;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Scrutor;
using crm.Backend.Attributes;
using Microsoft.AspNetCore.Authentication.JwtBearer;

public class Configuration
{
    private sealed class AttributeRegistrationStrategy<T> : RegistrationStrategy where T : DIAttribute
    {
        public override void Apply(IServiceCollection services, ServiceDescriptor descriptor)
        {
            var serviceType = descriptor.ImplementationType;
            if (serviceType == null)
                return;
            var interfaseType = serviceType.GetInterfaces().FirstOrDefault(i =>
            {
                if (i.Name.Contains('`'))
                    return serviceType.Name.Contains(i.Name.Split('`')[0].Substring(1)) && i.GenericTypeArguments.Count() > 0 && serviceType.Name.Contains(i.GenericTypeArguments.First().Name);
                else
                    return serviceType.Name.Contains(i.Name.Substring(1));
            });
            var attribute = serviceType.GetCustomAttribute<T>();
            if (interfaseType != null && attribute != null)
            {
                if (attribute.Key == null)
                    descriptor = ServiceDescriptor.Describe(interfaseType, serviceType, attribute.ServiceLifetime);
                else
                    descriptor = ServiceDescriptor.DescribeKeyed(interfaseType, attribute.Key, serviceType, attribute.ServiceLifetime);
                services.Add(descriptor);
            }
        }
    }

    private void ScanServices(ITypeSourceSelector selector)
    {
        var scanner = selector.FromAssembliesOf(GetType());

        scanner
            .AddClasses(classes => classes.WithAttribute<DITransient>())
            .UsingRegistrationStrategy(new AttributeRegistrationStrategy<DITransient>())
            .AsImplementedInterfaces()
            .WithTransientLifetime();
        scanner
            .AddClasses(classes => classes.WithAttribute<DIScoped>())
            .UsingRegistrationStrategy(new AttributeRegistrationStrategy<DIScoped>())
            .AsImplementedInterfaces()
            .WithScopedLifetime();
        scanner
            .AddClasses(classes => classes.WithAttribute<DISingleton>())
            .UsingRegistrationStrategy(new AttributeRegistrationStrategy<DISingleton>())
            .AsImplementedInterfaces()
            .WithSingletonLifetime();
    }
    public Configuration(IServiceCollection services)
    {

        services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
        {
            options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.SerializerOptions.PropertyNameCaseInsensitive = true;
        });
        services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
                // options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
            });

        services.Scan(ScanServices);

        services.AddAuthentication("Bearer")
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = AuthOptions.ISSUER,
                    ValidateAudience = true,
                    ValidAudience = AuthOptions.AUDIENCE,
                    ValidateLifetime = true,
                    IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];
                        var path = context.HttpContext.Request.Path;

                        if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hub"))
                        {
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
                };

            });
        services.AddAuthorization();
        services.AddHealthChecks();
    }
}


