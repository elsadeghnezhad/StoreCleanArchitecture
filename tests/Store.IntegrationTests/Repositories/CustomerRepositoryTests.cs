using AutoMapper;
using Store.ApplicationCore.DTOs;
using Store.ApplicationCore.Entities;
using Store.ApplicationCore.Exceptions;
using Store.ApplicationCore.Mappings;
using Store.Infrastructure.Persistence.Repositories;
using System;
using Xunit;

namespace Store.IntegrationTests.Repositories
{
    public class CustomerRepositoryTests : IClassFixture<SharedDatabaseFixture>
    {
        private readonly IMapper _mapper;
        private SharedDatabaseFixture Fixture { get; }

        public CustomerRepositoryTests(SharedDatabaseFixture fixture)
        {
            Fixture = fixture;

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<GeneralProfile>();
            });

            _mapper = configuration.CreateMapper();
        }

        [Fact]
        public void GetCustomers_ReturnsAllProducts()
        {
            using (var context = Fixture.CreateContext())
            {
                var repository = new CustomerRepository(context, _mapper);

                var customers = repository.GetCustomers();

                Assert.Equal(10, customers.Count);
            }
        }

        [Fact]
        public void GetCustomerById_CustomerDoesntExist_ThrowsNotFoundException()
        {
            using (var context = Fixture.CreateContext())
            {
                var repository = new CustomerRepository(context, _mapper);
                var customerId = 56;

                Assert.Throws<NotFoundException>(() => repository.GetCustomerById(customerId));
            }
        }

        [Fact]
        public void CreateCustomer_SavesCorrectData()
        {
            using (var transaction = Fixture.Connection.BeginTransaction())
            {
                var customerId = 0;

                var request = new CreateCustomerRequest
                {
                    Firstname = "firstname",
                    Lastname = "lastname",
                    DateOfBirth = System.DateTime.Now.Date,
                    PhoneNumber = 938548030,
                    Email = "email@gmail.com",
                    BankAccountNumber = "123456789"
                };

                using (var context = Fixture.CreateContext(transaction))
                {
                    var repository = new CustomerRepository(context, _mapper);

                    var customer = repository.CreateCustomer(request);
                    customerId = Convert.ToInt32(customer.Id);
                }

                using (var context = Fixture.CreateContext(transaction))
                {
                    var repository = new CustomerRepository(context, _mapper);

                    var customer = repository.GetCustomerById(customerId);

                    Assert.NotNull(customer);
                    Assert.Equal(request.Firstname, customer.Firstname);
                    Assert.Equal(request.Lastname, customer.Lastname);
                    Assert.Equal(request.Email, customer.Email);
                    Assert.Equal(request.DateOfBirth, customer.DateOfBirth);
                    Assert.Equal(request.PhoneNumber, customer.PhoneNumber);    
                }
            }
        }

        [Fact]
        public void UpdateProduct_SavesCorrectData()
        {
            using (var transaction = Fixture.Connection.BeginTransaction())
            {
                var productId = 1;

                var request = new UpdateCustomerRequest
                {
                    Firstname = "firstname",
                    Lastname = "lastname",
                    DateOfBirth = System.DateTime.Now.Date,
                    PhoneNumber = 938548030,
                    Email = "email@gmail.com",
                    BankAccountNumber = "123456789"
                };

                using (var context = Fixture.CreateContext(transaction))
                {
                    var repository = new CustomerRepository(context, _mapper);

                    repository.UpdateCustomer(productId, request);
                }

                using (var context = Fixture.CreateContext(transaction))
                {
                    var repository = new CustomerRepository(context, _mapper);

                    var customer = repository.GetCustomerById(productId);

                    Assert.NotNull(customer);
                    Assert.Equal(request.Firstname, customer.Firstname);
                    Assert.Equal(request.Lastname, customer.Lastname);
                    Assert.Equal(request.Email, customer.Email);
                    Assert.Equal(request.DateOfBirth, customer.DateOfBirth);
                    Assert.Equal(request.PhoneNumber, customer.PhoneNumber);
                }
            }
        }

        [Fact]
        public void UpdateProduct_ProductDoesntExist_ThrowsNotFoundException()
        {
            var customerId = 15;

            var request = new UpdateCustomerRequest
            {
                Firstname = "firstname",
                Lastname = "lastname",
                DateOfBirth = System.DateTime.Now.Date,
                PhoneNumber = 938548030,
                Email = "email@gmail.com",
                BankAccountNumber = "123456789"
            };

            using (var context = Fixture.CreateContext())
            {
                var repository = new CustomerRepository(context, _mapper);
                var action = () => repository.UpdateCustomer(customerId, request);

                Assert.Throws<NotFoundException>(action);
            }
        }

        [Fact]
        public void DeleteCustomerById_EnsuresCustomerIsDeleted()
        {
            using (var transaction = Fixture.Connection.BeginTransaction())
            {
                var customerId = 2;

                using (var context = Fixture.CreateContext(transaction))
                {
                    var repository = new CustomerRepository(context, _mapper);

                    repository.DeleteCustomerById(customerId);
                }

                using (var context = Fixture.CreateContext(transaction))
                {
                    var repository = new CustomerRepository(context, _mapper);
                    var action = () => repository.GetCustomerById(customerId);

                    Assert.Throws<NotFoundException>(action);
                }
            }
        }

        [Fact]
        public void DeleteCustomerById_CustomerDoesntExist_ThrowsNotFoundException()
        {
            var customerId = 48;

            using (var context = Fixture.CreateContext())
            {
                var repository = new CustomerRepository(context, _mapper);
                var action = () => repository.DeleteCustomerById(customerId);

                Assert.Throws<NotFoundException>(action);
            }
        }
    }
}