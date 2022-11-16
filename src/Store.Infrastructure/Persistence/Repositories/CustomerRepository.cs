using AutoMapper;
using Store.ApplicationCore.DTOs;
using Store.ApplicationCore.Entities;
using Store.ApplicationCore.Exceptions;
using Store.ApplicationCore.Interfaces;
using Store.ApplicationCore.Utils;
using Store.Infrastructure.Persistence.Contexts;
using System.Collections.Generic;
using System.Linq;

namespace Store.Infrastructure.Persistence.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly StoreContext storeContext;
        private readonly IMapper mapper;

        public CustomerRepository(StoreContext storeContext, IMapper mapper)
        {
            this.storeContext = storeContext;
            this.mapper = mapper;
        }

        public SingleCustomerResponse CreateCustomer(CreateCustomerRequest request)
        {
            var Customer = this.mapper.Map<Customer>(request);
            try
            {
                this.storeContext.Customers.Add(Customer);
                this.storeContext.SaveChanges();

                return this.mapper.Map<SingleCustomerResponse>(Customer);
            }
            catch (System.Exception ex){
                throw ex;
            }
        }

        public void DeleteCustomerById(int CustomerId)
        {
            var Customer = this.storeContext.Customers.Find(CustomerId);
            if (Customer != null)
            {
                this.storeContext.Customers.Remove(Customer);
                this.storeContext.SaveChanges();
            }
            else
            {
                throw new NotFoundException("Customer with this ID not founded");
            }
        }

        public SingleCustomerResponse GetCustomerById(int CustomerId)
        {
            var Customer = this.storeContext.Customers.Find(CustomerId);
            if (Customer != null)
            {
                return this.mapper.Map<SingleCustomerResponse>(Customer);
            }

            throw new NotFoundException("Customer with this ID not founded");
        }

        public List<CustomerResponse> GetCustomers()
        {
            return this.storeContext.Customers.Select(p => this.mapper.Map<CustomerResponse>(p)).ToList();
        }

        public SingleCustomerResponse UpdateCustomer(int CustomerId, UpdateCustomerRequest request)
        {
            var Customer = this.storeContext.Customers.Find(CustomerId);
            if (Customer != null)
            {
                Customer.Firstname = request.Firstname;
                Customer.Lastname = request.Lastname;
                Customer.Email = request.Email;
                Customer.PhoneNumber = request.PhoneNumber;
                Customer.BankAccountNumber = request.BankAccountNumber;
                Customer.DateOfBirth = request.DateOfBirth;

                this.storeContext.Customers.Update(Customer);
                this.storeContext.SaveChanges();

                return this.mapper.Map<SingleCustomerResponse>(Customer);
            }

            throw new NotFoundException("Customer with this ID not founded");
        }
    }
}