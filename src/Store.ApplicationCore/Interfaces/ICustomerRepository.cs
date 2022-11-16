using Store.ApplicationCore.DTOs;
using System.Collections.Generic;

namespace Store.ApplicationCore.Interfaces
{
    public interface ICustomerRepository
    {
        List<CustomerResponse> GetCustomers();

        SingleCustomerResponse GetCustomerById(int customerId);

        void DeleteCustomerById(int customerId);

        SingleCustomerResponse CreateCustomer(CreateCustomerRequest request);

        SingleCustomerResponse UpdateCustomer(int customerId, UpdateCustomerRequest request);
    }
}