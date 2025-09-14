

var builder = WebApplication.CreateBuilder(args);
var jwtPolicyName = "jwt";

builder.Services.AddRateLimiter(limiterOptions =>
{
    limiterOptions.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    limiterOptions.OnRejected = (context, cancellationToken) =>
    {
        context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
        context.HttpContext.RequestServices.GetService<ILoggerFactory>()?
            .CreateLogger("Microsoft.AspNetCore.RateLimitingMiddleware")
            .LogWarning("OnRejected: {GetUserEndPoint}", GetUserEndPoint(context.HttpContext));

        return new ValueTask();
    };

    limiterOptions.AddPolicy(policyName: jwtPolicyName, partitioner: httpContext =>
    {
        var tokenValue = string.Empty;
        if (AuthenticationHeaderValue.TryParse(httpContext.Request.Headers["Authorization"], out var authHeader))
        {
            tokenValue = authHeader.Parameter;
        }

        var email = string.Empty;
        var rateLimitWindowInMinutes = 5;
        var permitLimitAuthorized = 60;
        var permitLimitAnonymous = 30;
        if (!string.IsNullOrEmpty(tokenValue))
        {
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(tokenValue);
            email= token.Claims.First(claim => claim.Type == "Email").Value;
            var dbContext = httpContext.RequestServices.GetRequiredService<TodoDbContext>();
            var user = dbContext.Users.FirstOrDefault(u => u.Email == email);
            if (user != null)
            {
                permitLimitAuthorized = user.PermitLimit;
                rateLimitWindowInMinutes = user.RateLimitWindowInMinutes;
            }
        }

        return RateLimitPartition.GetFixedWindowLimiter(email, _ => new FixedWindowRateLimiterOptions()
        {
            PermitLimit = string.IsNullOrEmpty(email) ? permitLimitAnonymous : permitLimitAuthorized,
            Window = TimeSpan.FromMinutes(rateLimitWindowInMinutes),
            QueueLimit = 0
        });
    });
});

static string GetUserEndPoint(HttpContext context)
{
    var tokenValue = string.Empty;
    if (AuthenticationHeaderValue.TryParse(context.Request.Headers["Authorization"], out var authHeader))
    {
        tokenValue = authHeader.Parameter;
    }
    var email = "";
    if (!string.IsNullOrEmpty(tokenValue))
    {
        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(tokenValue);
        email = token.Claims.First(claim => claim.Type == "Email").Value;
    }

    return $"User {email ?? "Anonymous"} endpoint:{context.Request.Path}"
   + $" {context.Connection.RemoteIpAddress}";
}


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

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddHealthChecks().AddDbContextCheck<TodoDbContext>();

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
app.UseWebSockets();

var scope = app.Services.CreateScope();
var databaseContext = scope.ServiceProvider.GetService<TodoDbContext>();
if (databaseContext != null)
{
    databaseContext.Database.EnsureCreated();
}

app.MapGroup("/todoitems")
    .MapApiEndpoints()
    .WithTags("Todo Items")
    .RequireAuthorization()
    .RequireRateLimiting(jwtPolicyName)
    .WithMetadata()
    .WithApiVersionSet(versionSet)
    .AddEndpointFilter(async (efiContext, next) =>
    {
        var stopwatch = Stopwatch.StartNew();
        var result = await next(efiContext);
        stopwatch.Stop();
        var elapsed = stopwatch.ElapsedMilliseconds;
        var response = efiContext.HttpContext.Response;
        response.Headers.TryAdd("X-Response-Time", $"{elapsed} milliseconds");
        return result;
    });

app.MapGet("/health", async (HealthCheckService healthCheckService) =>
{
    var report = await healthCheckService.CheckHealthAsync();
    return report.Status == HealthStatus.Healthy ?
        Results.Ok(report) : Results.StatusCode(StatusCodes.Status503ServiceUnavailable);
})
.WithTags(new[] { "Health" })
.RequireRateLimiting(jwtPolicyName)
.Produces(200)
.ProducesProblem(503)
.Produces(429);

app.UseRouting();
app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();

app.Run();

//For integration testing
public partial class Program { }