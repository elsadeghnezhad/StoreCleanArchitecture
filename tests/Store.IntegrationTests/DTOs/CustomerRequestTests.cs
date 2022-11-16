using Store.ApplicationCore.DTOs;
using Store.ApplicationCore.Utils;
using System;
using Xunit;

namespace Store.UnitTests.DTOs
{
    public class CustomerRequestTests : BaseTests
    {
        [Theory]
        [InlineData("name", "family", 9383548030, "em@gmail.com", "123456987", 0)]
        [InlineData(null, "family", 9383548030, "em@gmail.com", "123456987", 1)]
        [InlineData(null, null, 9383548030, "em@gmail.com", "123456987", 2)]
        [InlineData(null, null, 9383548030, null, "123456987", 3)]
        public void ValidateModel_CreateCustomerRequest_ReturnsCorrectNumberOfErrors(string firstname, string lastname,ulong phoneNumber, string email, string bankAccountNumber, int numberExpectedErrors)
        {
            var request = new CreateCustomerRequest
            {
                Firstname = firstname,
                Lastname = lastname,
                DateOfBirth = DateUtil.GetCurrentDate(),
                PhoneNumber = phoneNumber,
                Email = email,
                BankAccountNumber = bankAccountNumber
            };

            var errorList = ValidateModel(request);

            Assert.Equal(numberExpectedErrors, errorList.Count);
        }

        [Theory]
        [InlineData("name", "family", 9383548030, "em@gmail.com", "123456987", 0)]
        [InlineData(null, "family", 9383548030, "em@gmail.com", "123456987", 1)]
        [InlineData(null, null, 9383548030, "em@gmail.com", "123456987", 2)]
        [InlineData(null, null, 9383548030, null, "123456987", 3)]
        public void ValidateModel_UpdateCustomerRequest_ReturnsCorrectNumberOfErrors(string firstname, string lastname, ulong phoneNumber, string email, string bankAccountNumber, int numberExpectedErrors)
        {
          var request = new UpdateCustomerRequest
          {
              Firstname = firstname,
              Lastname = lastname,
              DateOfBirth = DateUtil.GetCurrentDate(),
              PhoneNumber = phoneNumber,
              Email = email,
              BankAccountNumber = bankAccountNumber
          };

            var errorList = ValidateModel(request);

            Assert.Equal(numberExpectedErrors, errorList.Count);
        }
    }
}