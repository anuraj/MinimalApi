using Microsoft.AspNetCore.Mvc.Testing;
using System.Text;
using System.Net;
using System.Text.Json;
using System.Net.Http.Headers;
using MinimalApi.ViewModels;

namespace MinimalApi.Tests;

public class TodoApiIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _httpClient;

    public TodoApiIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _httpClient = factory.CreateClient();
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task DeleteTodoItem(bool getToken = false)
    {
        if (!getToken)
        {
            var token = await GetTokenForUser1();
            Assert.NotNull(token);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
        var response = await _httpClient.DeleteAsync("/todoitems/1");
        var responseStatusCode = response.StatusCode;

        if (!getToken)
        {
            Assert.Equal(HttpStatusCode.NoContent, responseStatusCode);
        }
        else
        {
            Assert.Equal(HttpStatusCode.Unauthorized, responseStatusCode);
        }
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task DeleteTodoItemNonExistingId(bool getToken = false)
    {
        if (!getToken)
        {
            var token = await GetTokenForUser1();
            Assert.NotNull(token);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
        var response = await _httpClient.DeleteAsync("/todoitems/200");
        var responseStatusCode = response.StatusCode;

        if (!getToken)
        {
            Assert.Equal(HttpStatusCode.NotFound, responseStatusCode);
        }
        else
        {
            Assert.Equal(HttpStatusCode.Unauthorized, responseStatusCode);
        }
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task UpdateTodoItem(bool getToken = false)
    {
        if (!getToken)
        {
            var token = await GetTokenForUser1();
            Assert.NotNull(token);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
        var response = await _httpClient.PutAsync("/todoitems/2",
            new StringContent(JsonSerializer.Serialize(new TodoItemInput { IsCompleted = true }),
            Encoding.UTF8, "application/json"));
        var responseStatusCode = response.StatusCode;

        if (!getToken)
        {
            Assert.Equal(HttpStatusCode.NoContent, responseStatusCode);
        }
        else
        {
            Assert.Equal(HttpStatusCode.Unauthorized, responseStatusCode);
        }
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task UpdateTodoItemNonExistingId(bool getToken = false)
    {
        if (!getToken)
        {
            var token = await GetTokenForUser1();
            Assert.NotNull(token);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
        var response = await _httpClient.PutAsync("/todoitems/3000",
            new StringContent(JsonSerializer.Serialize(new TodoItemInput { IsCompleted = true }),
            Encoding.UTF8, "application/json"));
        var responseStatusCode = response.StatusCode;

        if (!getToken)
        {
            Assert.Equal(HttpStatusCode.NotFound, responseStatusCode);
        }
        else
        {
            Assert.Equal(HttpStatusCode.Unauthorized, responseStatusCode);
        }
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task CreateTodoItem(bool getToken = false)
    {
        if (!getToken)
        {
            var token = await GetTokenForUser1();
            Assert.NotNull(token);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
        var response = await _httpClient.PostAsync("/todoitems",
            new StringContent(JsonSerializer.Serialize(new TodoItemInput { Title = "Test", IsCompleted = false }),
            Encoding.UTF8, "application/json"));
        var responseStatusCode = response.StatusCode;
        if (!getToken)
        {
            Assert.Equal(HttpStatusCode.Created, responseStatusCode);
            Assert.NotNull(response.Headers.Location);
        }
        else
        {
            Assert.Equal(HttpStatusCode.Unauthorized, responseStatusCode);
        }
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task GetTodoItemById(bool getToken = false)
    {
        if (!getToken)
        {
            var token = await GetTokenForUser1();
            Assert.NotNull(token);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
        var response = await _httpClient.GetAsync("/todoitems/5");
        var responseStatusCode = response.StatusCode;

        if (!getToken)
        {
            Assert.Equal(HttpStatusCode.OK, responseStatusCode);
            var todoItems = JsonSerializer.Deserialize<TodoItemOutput>(await response.Content.ReadAsStringAsync());
            Assert.NotNull(todoItems);
        }
        else
        {
            Assert.Equal(HttpStatusCode.Unauthorized, responseStatusCode);
        }
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task GetTodoItemByIdNonExistingId(bool getToken = false)
    {
        if (!getToken)
        {
            var token = await GetTokenForUser1();
            Assert.NotNull(token);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
        var response = await _httpClient.GetAsync("/todoitems/100");
        var responseStatusCode = response.StatusCode;

        if (!getToken)
        {
            Assert.Equal(HttpStatusCode.NotFound, responseStatusCode);
        }
        else
        {
            Assert.Equal(HttpStatusCode.Unauthorized, responseStatusCode);
        }
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task GetTodoItems(bool getToken = false)
    {
        if (!getToken)
        {
            var token = await GetTokenForUser1();
            Assert.NotNull(token);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
        var response = await _httpClient.GetAsync("/todoitems");
        var responseStatusCode = response.StatusCode;
        if (!getToken)
        {
            Assert.Equal(HttpStatusCode.OK, responseStatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();
            var todoItems = JsonSerializer.Deserialize<PagedResults<TodoItemOutput>>(responseContent);
            Assert.NotNull(todoItems);
        }
        else
        {
            Assert.Equal(HttpStatusCode.Unauthorized, responseStatusCode);
        }
    }

    [Theory]
    [InlineData(true, "1.0")]
    [InlineData(false, "1.0")]
    [InlineData(true, "2.0")]
    [InlineData(false, "2.0")]
    public async Task GetTodoItemsWithVersionHeader(bool getToken = false, string version = "1.0")
    {
        if (!getToken)
        {
            var token = await GetTokenForUser1();
            Assert.NotNull(token);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
        _httpClient.DefaultRequestHeaders.Add("api-version", version);
        var response = await _httpClient.GetAsync("/todoitems");
        var responseStatusCode = response.StatusCode;
        if (!getToken)
        {
            Assert.Equal(HttpStatusCode.OK, responseStatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();
            var todoItems = JsonSerializer.Deserialize<PagedResults<TodoItemOutput>>(responseContent);
            Assert.NotNull(todoItems);
        }
        else
        {
            Assert.Equal(HttpStatusCode.Unauthorized, responseStatusCode);
        }
    }

    [Fact(Skip = "Running this test will exhaust the anonymous request limit - which fails the other tests")]
    public async Task GetHealthWithoutToken()
    {
        var response = await _httpClient.GetAsync("/health");
        var responseStatusCode = response.StatusCode;
        Assert.Equal(HttpStatusCode.OK, responseStatusCode);

        for (int i = 0; i < 29; i++)
        {
            response = await _httpClient.GetAsync("/health");
            responseStatusCode = response.StatusCode;
            Assert.Equal(HttpStatusCode.OK, responseStatusCode);
        }

        response = await _httpClient.GetAsync("/health");
        responseStatusCode = response.StatusCode;
        Assert.Equal(HttpStatusCode.TooManyRequests, responseStatusCode);
    }

    [Fact]
    public async Task GetHealthWithToken()
    {
        var token = await GetTokenForUser2();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _httpClient.GetAsync("/health");
        var responseStatusCode = response.StatusCode;
        Assert.Equal(HttpStatusCode.OK, responseStatusCode);

        for (int i = 0; i < 59; i++)
        {
            response = await _httpClient.GetAsync("/health");
            responseStatusCode = response.StatusCode;
            Assert.Equal(HttpStatusCode.OK, responseStatusCode);
        }

        response = await _httpClient.GetAsync("/health");
        responseStatusCode = response.StatusCode;
        Assert.Equal(HttpStatusCode.TooManyRequests, responseStatusCode);
    }

    private static async Task<string> GetTokenForUser1()
    {
        var token = Environment.GetEnvironmentVariable("USER1_TOKEN") ?? "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6InVzZXIxIiwic3ViIjoidXNlcjEiLCJqdGkiOiI4YzI0MmQxNiIsIlVzZXJuYW1lIjoidXNlcjEiLCJFbWFpbCI6InVzZXIxQGV4YW1wbGUuY29tIiwiYXVkIjpbImh0dHA6Ly9sb2NhbGhvc3Q6MjEyNDQiLCJodHRwczovL2xvY2FsaG9zdDo0NDM3MyIsImh0dHBzOi8vbG9jYWxob3N0OjUwMDEiLCJodHRwOi8vbG9jYWxob3N0OjUwMDAiXSwibmJmIjoxNjY5NjkwMjE1LCJleHAiOjE2Nzc1NTI2MTUsImlhdCI6MTY2OTY5MDIxNiwiaXNzIjoiZG90bmV0LXVzZXItand0cyJ9.9NQVRgbAHR-FLOs0bUndRSw8jVvUWLZhCRSNxBXWmaU";
        return await Task.Run(() => token);
    }

    private static async Task<string> GetTokenForUser2()
    {
        var token = Environment.GetEnvironmentVariable("USER2_TOKEN") ?? "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6InVzZXIyIiwic3ViIjoidXNlcjIiLCJqdGkiOiIzZTFlZWRmZCIsIlVzZXJuYW1lIjoidXNlcjIiLCJFbWFpbCI6InVzZXIyQGV4YW1wbGUuY29tIiwiYXVkIjpbImh0dHA6Ly9sb2NhbGhvc3Q6MjEyNDQiLCJodHRwczovL2xvY2FsaG9zdDo0NDM3MyIsImh0dHBzOi8vbG9jYWxob3N0OjUwMDEiLCJodHRwOi8vbG9jYWxob3N0OjUwMDAiXSwibmJmIjoxNjY5NzA3MDY4LCJleHAiOjE2Nzc1Njk0NjgsImlhdCI6MTY2OTcwNzA2OSwiaXNzIjoiZG90bmV0LXVzZXItand0cyJ9.CbsD83t7m8S_kWpNHAVlkH0a8_-rN2BS9pgZ5gqXWJY";
        return await Task.Run(() => token);
    }
}