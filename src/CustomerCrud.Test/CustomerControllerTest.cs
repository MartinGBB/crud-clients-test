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
        var request = AutoFaker.Generate<CustomerRequest>();
        
        _repositoryMock.Setup(x => x.GetNextIdValue()).Returns(1);
        _repositoryMock.Setup(x => x.Create(It.Is<Customer>(r => r.Id == 1)));

        var response = await _client.PostAsJsonAsync("/customers", request);
        var content = await response.Content.ReadFromJsonAsync<Customer>();

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        _repositoryMock.Verify(x => x.Create(It.Is<Customer>(r => r.Id == 1)), Times.Once);

        // Verificação de todas as referencias
        // content.Should().BeEquivalentTo(request);

        // Verificação individual, se uma falha, mostrara a que falhou
        content!.Id.Should().Be(1);
        content?.Name.Should().Be(request.Name);
        content?.CPF.Should().Be(request.CPF);
        content?.Transactions.Should().BeEquivalentTo(request.Transactions);

        content?.CreatedAt.Should().BeCloseTo(content.UpdatedAt, TimeSpan.FromMilliseconds(100));
    }

    [Fact]
    public async Task UpdateTest()
    {
        var request = AutoFaker.Generate<CustomerRequest>();
        _repositoryMock.Setup(x => x.Update(It.Is<int>(id => id == 1), It.IsAny<object>())).Returns(true);

        var response = await _client.PutAsJsonAsync("/customers/1", request);
        var content = await response.Content.ReadAsStringAsync();        

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        content.Should().Be($"Customer {1} updated");

        _repositoryMock.Verify(x => x.Update(It.Is<int>(id => id == 1), It.IsAny<object>()));
    }

    [Fact]
    public async Task DeleteTest()
    {
        _repositoryMock.Setup(r => r.Delete(It.Is<int>(id => id == 1))).Returns(true);

        var response = await _client.DeleteAsync("/customers/1");
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        _repositoryMock.Verify(x => x.Delete(It.Is<int>(id => id == 1)), Times.Once);

        // Falha
        _repositoryMock.Setup(r => r.Delete(It.Is<int>(id => id == 2))).Returns(false);
        response = await _client.DeleteAsync("/customers/2");
        var content = await response.Content.ReadAsStringAsync();

        content.Should().Be("Customer not found");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}