using System.Diagnostics;

using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseKestrel(options => options.AddServerHeader = false);
builder.Services.AddHttpContextAccessor();

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ApiVersionReader = new HeaderApiVersionReader("api-version");
});

builder.Services.AddAuthorization();
builder.Services.AddAuthentication("Bearer").AddJwtBearer();
builder.Services.AddDbContextFactory<TodoDbContext>(options => options.UseInMemoryDatabase($"MinimalApiDb-{Guid.NewGuid()}"));

builder.Services.AddGraphQLServer()
    .AddAuthorization()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddSubscriptionType<Subscription>()
    .AddInMemorySubscriptions();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(setup =>
{
    setup.SwaggerDoc("v1", new OpenApiInfo()
    {
        Description = "ASP.NET Core 7.0 - Minimal API Example - Todo API implementation using ASP.NET Core Minimal API," +
            "GraphQL, Entity Framework Core, Token authentication, Versioning, Unit Testing and Open API.",
        Title = "Todo Api",
        Version = "v1",
        Contact = new OpenApiContact()
        {
            Name = "anuraj",
            Url = new Uri("https://dotnetthoughts.net")
        }
    });

    setup.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer"
    });

    setup.OperationFilter<AuthorizationHeaderOperationHeader>();
    setup.OperationFilter<ApiVersionOperationFilter>();
});

builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddHealthChecks().AddDbContextCheck<TodoDbContext>();

builder.Services.AddScoped<IValidator<TodoItemInput>, TodoItemInputValidator>();
builder.Services.AddScoped<IValidator<UserInput>, UserInputValidator>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

var versionSet = app.NewApiVersionSet()
                    .HasApiVersion(new ApiVersion(1, 0))
                    .HasApiVersion(new ApiVersion(2, 0))
                    .ReportApiVersions()
                    .Build();

app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Todo Api v1");
});

app.UseWebSockets();

var scope = app.Services.CreateScope();
var databaseContext = scope.ServiceProvider.GetService<TodoDbContext>();
if (databaseContext != null)
{
    databaseContext.Database.EnsureCreated();
}

app.MapGroup("/todoitems").MapApiEndpoints().WithTags("Todo Items")
    .RequireAuthorization().WithMetadata()
    .WithApiVersionSet(versionSet).AddEndpointFilter(async (efiContext, next) =>
    {
        var stopwatch = Stopwatch.StartNew();
        var result = await next(efiContext);
        stopwatch.Stop();
        var elapsed = stopwatch.ElapsedMilliseconds;
        var response = efiContext.HttpContext.Response;
        response.Headers.Add("X-Response-Time", $"{elapsed.ToString()} milliseconds");
        return result;
    });

app.MapGet("/health", async (HealthCheckService healthCheckService) =>
{
    var report = await healthCheckService.CheckHealthAsync();
    return report.Status == HealthStatus.Healthy ?
        Results.Ok(report) : Results.StatusCode(StatusCodes.Status503ServiceUnavailable);
}).WithTags(new[] { "Health" })
.Produces(200)
.ProducesProblem(503);

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.Map("/graphql", () => app.MapGraphQL());
app.Run();

//For integration testing
public partial class Program { }