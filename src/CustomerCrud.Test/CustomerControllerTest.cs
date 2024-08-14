using CustomerCrud.Core;
using CustomerCrud.Repositories;
using CustomerCrud.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CustomerCrud.Test;

public class CustomersControllerTest : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly Mock<ICustomerRepository> _repositoryMock;

    public CustomersControllerTest(WebApplicationFactory<Program> factory)
    {
        _repositoryMock = new Mock<ICustomerRepository>();

        _client = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.AddScoped<ICustomerRepository>(st => _repositoryMock.Object);
            });
        }).CreateClient();
    }

    [Fact]
    public async Task GetAllTest()
    {
        var customers = AutoFaker.Generate<Customer>(3);
        _repositoryMock.Setup(x => x.GetAll()).Returns(customers);
        
        var response = await _client.GetAsync("/customers");
        var content = await response.Content.ReadFromJsonAsync<IEnumerable<Customer>>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        content.Should().BeEquivalentTo(customers);
        _repositoryMock.Verify(r => r.GetAll(), Times.Once);
    }

    [Fact]
    public async Task GetByIdTest()
    {
        var customer = AutoFaker.Generate<Customer>();
        customer.Id = 1;

        _repositoryMock.Setup(x => x.GetById(customer.Id)).Returns(customer);

        var response = await _client.GetAsync($"/customers/{customer.Id}");
        var content = await response.Content.ReadFromJsonAsync<Customer>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        content.Should().BeEquivalentTo(customer);
        _repositoryMock.Verify(x => x.GetById(customer.Id), Times.Once);

        // falha
        var invalidId = 2;

        _repositoryMock.Setup(x => x.GetById(invalidId)).Returns((Customer) null);
        response = await _client.GetAsync($"/customers/{invalidId}");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task CreateTest()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public async Task UpdateTest()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public async Task DeleteTest()
    {
        throw new NotImplementedException();
    }
}