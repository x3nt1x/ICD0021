using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using App.Public.DTO.v1;
using App.Public.DTO.v1.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Tests.Helpers;
using Xunit.Abstractions;

namespace Tests.Integration;

public class ApiClientControllerTests : IClassFixture<CustomWebAppFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly ITestOutputHelper _testOutputHelper;

    public ApiClientControllerTests(CustomWebAppFactory<Program> factory, ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }

    [Fact]
    public async Task Test_API_Client_Controller()
    {
        // arrange
        const string url = "/api/v1/client";

        // act
        var response = await _client.GetAsync(url);

        // assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        
        await Register_User();
    }

    private async Task Register_User()
    {
        // arrange
        const string url = "/api/v1/identity/account/register";

        var registerData = new Register
        {
            Email = "test@app.xyz",
            Password = "Foo.bar.3",
            Firstname = "Test",
            Lastname = "App"
        };

        // act
        var data = JsonContent.Create(registerData);
        
        var response = await _client.PostAsync(url, data);

        // assert
        response.EnsureSuccessStatusCode();
        
        await Login();
    }

    private async Task Login()
    {
        // arrange
        const string url = "/api/v1/identity/account/login";

        var loginData = new Login
        {
            Email = "test@app.xyz",
            Password = "Foo.bar.3"
        };

        // act
        var data = JsonContent.Create(loginData);

        var response = await _client.PostAsync(url, data);

        // assert
        response.EnsureSuccessStatusCode();

        var body = await response.Content.ReadAsStringAsync();
        _testOutputHelper.WriteLine(body);

        var resData = JsonHelper.DeserializeWithWebDefaults<JwtResponse>(body);
        Assert.NotNull(resData);
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", resData.JWT);
        
        await Create_Client();
    }

    private async Task Create_Client()
    {
        // arrange
        const string url = "/api/v1/client";

        var clientData = new Client
        {
            Name = "Test_Client",
            TotalOrders = 2,
            TotalTasks = 5
        };

        // act
        var data = JsonContent.Create(clientData);

        var response = await _client.PostAsync(url, data);

        // assert
        response.EnsureSuccessStatusCode();

        var body = await response.Content.ReadAsStringAsync();
        _testOutputHelper.WriteLine(body);

        var resData = JsonHelper.DeserializeWithWebDefaults<Client>(body);

        await Get_Client(resData!.Id);
    }

    private async Task Get_Client(Guid clientId)
    {
        // arrange
        var url = "/api/v1/client/" + clientId;

        // act
        var response = await _client.GetAsync(url);

        // assert
        response.EnsureSuccessStatusCode();

        var body = await response.Content.ReadAsStringAsync();
        _testOutputHelper.WriteLine(body);

        var resData = JsonHelper.DeserializeWithWebDefaults<Client>(body);

        Assert.NotNull(resData);
        Assert.Equal(clientId, resData.Id);
        Assert.Equal("Test_Client", resData.Name);
        Assert.Equal(2, resData.TotalOrders);
        Assert.Equal(5, resData.TotalTasks);

        await Edit_Client(clientId);
    }

    private async Task Edit_Client(Guid clientId)
    {
        // arrange
        var url = "/api/v1/Client/" + clientId;

        var editClientData = new Client
        {
            Id = clientId,
            Name = "Edit_Test_Client",
            TotalOrders = 3,
            TotalTasks = 8
        };

        // act
        var data = JsonContent.Create(editClientData);

        var response = await _client.PutAsync(url, data);

        // assert
        response.EnsureSuccessStatusCode();

        await Get_Edited_Client(clientId);
    }

    private async Task Get_Edited_Client(Guid clientId)
    {
        // arrange
        var url = "/api/v1/client/" + clientId;

        // act
        var response = await _client.GetAsync(url);

        // assert
        response.EnsureSuccessStatusCode();

        var body = await response.Content.ReadAsStringAsync();
        _testOutputHelper.WriteLine(body);

        var resData = JsonHelper.DeserializeWithWebDefaults<Client>(body);

        Assert.NotNull(resData);
        Assert.Equal(clientId, resData.Id);
        Assert.Equal("Edit_Test_Client", resData.Name);
        Assert.Equal(3, resData.TotalOrders);
        Assert.Equal(8, resData.TotalTasks);

        await Delete_Client(clientId);
    }

    private async Task Delete_Client(Guid clientId)
    {
        // arrange
        var url = "/api/v1/client/" + clientId;

        // act
        var response = await _client.DeleteAsync(url);

        // assert
        response.EnsureSuccessStatusCode();
        
        await Get_Deleted_Client(clientId);
    }

    private async Task Get_Deleted_Client(Guid clientId)
    {
        // arrange
        var url = "/api/v1/client/" + clientId;

        // act
        var response = await _client.GetAsync(url);

        // assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}