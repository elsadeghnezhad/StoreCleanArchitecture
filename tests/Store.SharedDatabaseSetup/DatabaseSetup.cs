using Bogus;
using Store.ApplicationCore.Entities;
using Store.ApplicationCore.Utils;
using Store.Infrastructure.Persistence.Contexts;

namespace Store.SharedDatabaseSetup
{
    public static class DatabaseSetup
    {
        public static void SeedData(StoreContext context)
        {
            context.Products.RemoveRange(context.Products);

            var productIds = 1;
            var fakeProducts = new Faker<Product>()
                .RuleFor(o => o.Name, f => $"Product {productIds}")
                .RuleFor(o => o.Description, f => $"Description {productIds}")
                .RuleFor(o => o.Id, f => productIds++)
                .RuleFor(o => o.Stock, f => f.Random.Number(1, 50))
                .RuleFor(o => o.Price, f => f.Random.Double(0.01, 100))
                .RuleFor(o => o.CreatedAt, f => DateUtil.GetCurrentDate())
                .RuleFor(o => o.UpdatedAt, f => DateUtil.GetCurrentDate());

            var products = fakeProducts.Generate(10);

            context.AddRange(products);
            context.SaveChanges();

            //**********************
            context.Customers.RemoveRange(context.Customers);

            var customerIds = 1;
            ulong phoneNumbers = 9383548030;

            var fakeCustomers = new Faker<Customer>()
                .RuleFor(o => o.Firstname, f => $"Firstname {customerIds}")
                .RuleFor(o => o.Lastname, f => $"Lastname {customerIds}")
                .RuleFor(o => o.Id, f => customerIds++)
                .RuleFor(o => o.Email, f => $"email_{customerIds}@gmail.com")
                .RuleFor(o => o.BankAccountNumber, f => $"123456789{customerIds}")
                .RuleFor(o => o.PhoneNumber, f => phoneNumbers++)
                .RuleFor(o => o.DateOfBirth, f => DateUtil.GetCurrentDate());

            var customers = fakeCustomers.Generate(10);

            context.AddRange(customers);


            context.SaveChanges();
        }
    }
}