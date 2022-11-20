using Microsoft.AspNetCore.Mvc.Testing;
using System.Text;
using System.Net;
using System.Text.Json;
using System.Net.Http.Headers;
using MinimalApi.ViewModels;

namespace MinimalApi.Tests;

public class TodoApiIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _httpClient;

    public TodoApiIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _httpClient = factory.CreateClient();
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task DeleteTodoItem(bool getToken = false)
    {
        if (!getToken)
        {
            var token = await GetToken();
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
            var token = await GetToken();
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
            var token = await GetToken();
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
            var token = await GetToken();
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
            var token = await GetToken();
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
            var token = await GetToken();
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
            var token = await GetToken();
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
            var token = await GetToken();
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
            var token = await GetToken();
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

    private async Task<string> GetToken()
    {
        var response = await _httpClient.PostAsync("/token", new StringContent(
                    @"{""username"":""user1"",""password"":""secret-1""}", Encoding.UTF8, "application/json"));
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        var jsonDocument = JsonDocument.Parse(responseString);
        return jsonDocument.RootElement.GetProperty("token").GetString()!;
    }
}