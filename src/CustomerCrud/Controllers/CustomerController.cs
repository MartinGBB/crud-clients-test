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
}
