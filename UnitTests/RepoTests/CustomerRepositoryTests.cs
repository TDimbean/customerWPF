using DAL;
using DAL.Entities;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace UnitTests.RepoTests
{
    [TestClass]
    public class CustomerRepositoryTests
    {
        private TestDb _db;
        private ICustomerRepository _repo;

        [TestInitialize]
        public void Init()
        {
            _db = new TestDb();
            _repo = new TestCustomerRepository();
        }

        [TestMethod]
        public void Create_HappyFlow_ShouldAddCustomer()
        {
            // Arrange
            var initialCount = _repo.GetAll().Count();
            var custCode = Guid.NewGuid().ToString();
            var customer = new Customer
            {
                CustomerCode = custCode,
                Address = "8 Newton Valley",
                Name = "John Beck",
                CNP = "1930311202020"
            };

            // Act
            _repo.Create(customer);
            var newCount = _repo.GetAll().Count();
            var createdCustomer = _repo.GetAll().SingleOrDefault(c => c.CustomerCode == custCode);

            // Assert
            newCount.Should().BeGreaterThan(initialCount);
            createdCustomer.Should().BeEquivalentTo(customer);
        }

        [TestMethod]
        public void CustomerCodeExists_ItDoes_ShouldReturnTrue()
        {
            // Arrange
            var custCode = _db.Customers.FirstOrDefault().CustomerCode;

            // Act
            var result = _repo.CustomerCodeExists(custCode);

            // Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void CustomerCodeExists_ItDoesNot_ShouldReturnFalse()
        {
            // Arrange
            var custCode = "";
            var exists = true;
            while (exists)
            {
                custCode = Guid.NewGuid().ToString();
                if (!_db.Customers.Any(c => c.CustomerCode == custCode)) exists=false;
            }

            // Act
            var result = _repo.CustomerCodeExists(custCode);

            // Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void GetAll_HappyFlow_ShouldReturnAllCustomers()
        {
            var result = _repo.GetAll();

            result.Should().BeEquivalentTo(_db.Customers);
        }

        [TestMethod]
        public void GetAllPaged_HappyFlow_ShouldReturnPaged()
        {
            // Arrange
            var pgSize = 2;
            var pgIndex = 2;
            var expected = _db.Customers.Skip((pgIndex - 1) * pgSize).Take(pgSize);

            // Act
            var result = _repo.GetAllPaged(pgIndex, pgSize);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void Update_HappyFlow_ShouldUpdateCustomer()
        {
            // Arrange
            var customer = _db.Customers.FirstOrDefault();
            var initialCount = _repo.CustomerCount();
            var updCust = new Customer
            {
                CustomerCode = customer.CustomerCode,
                Address = "10 Newton Park",
                Name = "Andrea Banfi",
                CNP = "2831023202020"
            };

            // Act
            _repo.Update(updCust);
            var result = _repo.GetAll().FirstOrDefault(c => c.CustomerCode == customer.CustomerCode);
            var newCount = _repo.CustomerCount();

            // Assert
            newCount.Should().Be(initialCount);
            result.Should().NotBeEquivalentTo(customer);
            result.CNP.Should().Be(updCust.CNP);
            result.Name.Should().Be(updCust.Name);
            result.Address.Should().Be(updCust.Address);
        }
    }
}
