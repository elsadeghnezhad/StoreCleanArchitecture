using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.ApplicationCore.DTOs;
using Store.ApplicationCore.Exceptions;
using Store.ApplicationCore.Interfaces;
using System.Collections.Generic;
using System.Net.Mime;

namespace Store.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces("application/json")]
    public class CustomersController : Controller
    {
        private readonly ICustomerRepository CustomerRepository;

        public CustomersController(ICustomerRepository CustomerRepository)
        {
            this.CustomerRepository = CustomerRepository;
        }

        /// <summary>
        /// Get all Customers
        /// </summary>
        /// <response code="200">Returns the Customers</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<CustomerResponse>))]
        public ActionResult GetCustomers()
        {
            return Ok(this.CustomerRepository.GetCustomers());
        }

        /// <summary>
        /// Get a Customer by id
        /// </summary>
        /// <param name="id">Customer id</param>
        /// <response code="200">Returns the existing Customer</response>
        /// <response code="404">If the Customer doesn't exist</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SingleCustomerResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult GetCustomerById(int id)
        {
            try
            {
                var Customer = this.CustomerRepository.GetCustomerById(id);
                return Ok(Customer);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Create a Customer
        /// </summary>
        /// <param name="request">Customer data</param>
        /// <response code="201">Returns the created Customer</response>
        /// <response code="400">If the data doesn't pass the validations</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(SingleCustomerResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult Create(CreateCustomerRequest request)
        {
            var Customer = this.CustomerRepository.CreateCustomer(request);
            return StatusCode(201, Customer);
        }

        /// <summary>
        /// Update a Customer by id
        /// </summary>
        /// <param name="id">Customer id</param>
        /// <param name="request">Customer data</param>
        /// <response code="200">Returns the updated Customer</response>
        /// <response code="400">If the data doesn't pass the validations</response>
        /// <response code="404">If the Customer doesn't exist</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SingleCustomerResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult Update(int id, UpdateCustomerRequest request)
        {
            try
            {
                var Customer = this.CustomerRepository.UpdateCustomer(id, request);
                return Ok(Customer);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Delete a Customer by id
        /// </summary>
        /// <param name="id">Customer id</param>
        /// <response code="204">If the Customer was deleted</response>
        /// <response code="404">If the Customer doesn't exist</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult Delete(int id)
        {
            try
            {
                this.CustomerRepository.DeleteCustomerById(id);
                return NoContent();
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }
    }
}