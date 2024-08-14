using Microsoft.AspNetCore.Mvc;
using CustomerCrud.Core;
using CustomerCrud.Requests;
using CustomerCrud.Repositories;

namespace CustomerCrud.Controllers;

[ApiController]
[Route("customers")]

public class CustomerController : ControllerBase
{
    private readonly ICustomerRepository _customerRepository;
    public CustomerController(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    [HttpGet]
    public ActionResult GetAll()
    {
        var customer = _customerRepository.GetAll();
        return Ok(customer);
    }

    [HttpGet("{id}")]
    public ActionResult GetById(int id)
    {
        var customer = _customerRepository.GetById(id);

        if (customer == null)
            return NotFound("Customer not found");

        return Ok(customer);
    }

    [HttpPost]
    public ActionResult Create(CustomerRequest request)
    {
        var id = _customerRepository.GetNextIdValue();

        var customer = new Customer(id, request);
        _customerRepository.Create(customer);

        return CreatedAtAction("GetById", new { id = customer.Id }, customer);
    }

    [HttpPut("{id}")]
    public ActionResult Update(int id, CustomerRequest request)
    {
        var requestCustomer = new
        {
            Name = request.Name,
            CPF = request.CPF,
            Transactions = request.Transactions,
            UpdatedAt = DateTime.Now
        };

        var updateCustomer = _customerRepository.Update(id, requestCustomer);

        if (!updateCustomer) return NotFound("Customer not found");

        return Ok($"Customer {id} updated");
    }
}
