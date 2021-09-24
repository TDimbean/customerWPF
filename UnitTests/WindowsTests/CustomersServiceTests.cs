using CustomerWPF_Windows;
using DAL.Entities;
using FluentAssertions;
using Infrastructure.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UnitTests.WindowsTests
{
    [TestClass]
    public class CustomersServiceTests
    {
        private CustomerService _service;
        private TestDb _db;

        [TestInitialize]
        public void Init()
        {
            _db = new TestDb();
            _service = new CustomerService(new TestCustomerRepository());
        }

        [TestMethod]
        public void GetAll_HappyFlow_ShouldFetchAll()
        {
            var fetchedCusts = _service.GetAll();

            fetchedCusts.Should().BeEquivalentTo(_db.Customers);
        }

        [TestMethod]
        public void Create_HappyFlow_ShouldAddCustomer()
        {
            // Arrange
            var initialCount = _service.GetAll().Count();
            var custCode = Guid.NewGuid().ToString();
            var customer = new Customer
            {
                CustomerCode = custCode,
                Name = "Joseph Andrews",
                Address = "11 High Pass",
                CNP = "1450716898323"
            };

            // Act
            _service.Create(customer);
            var fetchedCustomers = _service.GetAllFiltered(custCode);
            var fetched = MapCustomerToCcVM(fetchedCustomers.FirstOrDefault());
            var creational = MapCustomerToCcVM(customer);
            var newCount = _service.GetAll().Count();

            // Assert
            fetchedCustomers.Count().Should().Be(1);
            fetched.Should().BeEquivalentTo(creational);
            newCount.Should().BeGreaterThan(initialCount);
        }

        [TestMethod]
        public void Update_HappyFlow_ShouldUpdateCustomer()
        {
            //Arrange
            var initialCount = _service.CustomerCount();
            var existingCustomer = _db.Customers.FirstOrDefault();
            var custCode = existingCustomer.CustomerCode;
            var updateCustomer = new Customer
            {
                CustomerCode = custCode,
                Name = "Newton Actuallaise",
                CNP = "2320302123456",
                Address = "14 Uptown Park"
            };
            //var updateCustomer = new Customer();
            //updateCustomer.CustomerCode = custCode;
            //updateCustomer.Name = "Newton Actuallaise";
            //updateCustomer.CNP = existingCustomer.CNP.Replace(existingCustomer.CNP.Substring(7, 13), "080808");
            //updateCustomer.Address = "14 Uptown Park";

            //Act
            _service.Update(updateCustomer);
            var fetchedCustomers = _service.GetAllFiltered(custCode);
            var newCount = _service.CustomerCount();

            // Assert
            fetchedCustomers.Count().Should().Be(1);
            newCount.Should().Be(initialCount);

            _service.MapCustomerToUpdate(fetchedCustomers.FirstOrDefault())
               .Should().BeEquivalentTo(_service.MapCustomerToUpdate(updateCustomer));

            _service.MapCustomerToUpdate(fetchedCustomers.FirstOrDefault())
                .Should().NotBeEquivalentTo(_service.MapCustomerToUpdate(existingCustomer));
        }

        [TestMethod]
        public void MapCreationalToUpdate_HappyFlow_ShouldMap()
        {
            // Arrange
            var name = "Morgan Newman";
            var adr = "10 Nupton Avenue";
            var cnp = "1920308090211";
            var creationalVM = new CustomerCreationViewModel
            {
                CustomerCode = Guid.NewGuid().ToString(),
                Name = name,
                Address = adr,
                CNP = cnp
            };
            var updateVM = new CustomerUpdateViewModel()
            {
                Name = name,
                Address = adr,
                CNP = cnp
            };

            // Act
            var mappedVM = _service.MapCreationalToUpdate(creationalVM);

            // Assert
            mappedVM.Should().BeEquivalentTo(updateVM);
        }

        [TestMethod]
        public void MapCreational_HappyFlow_ShouldMap()
        {
            // Arrange
            var custCode = Guid.NewGuid().ToString();
            var creationalVM = new CustomerCreationViewModel
            {
                CustomerCode = custCode,
                Name = "Hensey Newton",
                Address = "12 Chenopee Road",
                CNP = "1231111080080"
            };

            // Act
            var mappedCust = _service.MapCreational(creationalVM);

            // Assert
            MapCustomerToCcVM(mappedCust).Should().BeEquivalentTo(creationalVM);
        }

        [TestMethod]
        public void MapUpdate_HappyFlow_ShouldMap()
        {
            // Arrange
            var updateVM = new CustomerUpdateViewModel
            {
                Name = "Hensey Newton",
                Address = "12 Chenopee Road",
                CNP = "1231111080080"
            };

            // Act
            var mappedCust = _service.MapUpdate(updateVM);

            // Assert
            _service.MapCustomerToUpdate(mappedCust).Should().BeEquivalentTo(updateVM);
        }

        [TestMethod]
        public void MapCustomerToUpdate_HappyFlow_ShouldMap()
        {
            // Arrange
            var customer = _db.Customers.FirstOrDefault();

            // Act
            var mappedUpd = _service.MapCustomerToUpdate(customer);

            // Assert
            mappedUpd.Name.Should().Be(customer.Name);
            mappedUpd.CNP.Should().Be(customer.CNP);
            mappedUpd.Address.Should().Be(customer.Address);
        }

        [TestMethod]
        public void VerifyCreational_HappyFlow_ShouldReturnTrue()
        {
            // Arrange
            var ccVM = new CustomerCreationViewModel
            {
                CustomerCode = Guid.NewGuid().ToString(),
                Name = "Willson Abernacle",
                Address = "14 Happy River",
                CNP = "1900423817712"
            };

            // Act
            var isValid = _service.VerifyCreational(ccVM);

            //Assert
            isValid.Should().BeTrue();
        }

        [TestMethod]
        public void VerifyUpdate_HappyFlow_ShouldReturnTrue()
        {
            // Arrange
            var updVM = new CustomerUpdateViewModel
            {
                Name = "Willson Abernacle",
                Address = "14 Happy River",
                CNP = "1900423817712"
            };

            // Act
            var isValid = _service.VerifyUpdate(updVM);

            //Assert
            isValid.Should().BeTrue();
        }

        [DataTestMethod]
        //[DataRow("Braid Danaman", "15 Wright Point", "2450321000888")]
        [DataRow(null, "15 Wright Point", "2450321000888")]
        [DataRow("Braid Danaman", null, "2450321000888")]
        [DataRow("Braid Danaman", "15 Wright Point", null)]
        [DataRow("Braid danaman", "15 Wright Point", "2450321000888")]
        [DataRow("BraidDanaman", "15 Wright Point", "2450321000888")]
        [DataRow("Braid", "15 Wright Point", "2450321000888")]
        [DataRow("Braid Danamanpuakucbaniioypcxkeotimagtiapenyrdvjgpjkjuklusqwcdfbtzvmtvyesxlxegbqpxpynscamtjqxsazrzgguzabttkwzykjd"
            , "15 Wright Point", "2450321000888")]
        [DataRow("", "15 Wright Point", "2450321000888")]
        [DataRow("Braid Jay Danaman", "15 Wright Point", "2450321000888")]
        [DataRow("Braid Danaman", "", "2450321000888")]
        [DataRow("Braid Danaman", "15 Wright Pointamanpuakucbaniioypcxkeotimagtiapenyrdvjgpjkjuklusqwcdfbtzvmtvyesxlxegbqpxpynscamtjqxsazrzgguzabttkwzykjdamanpuakucbaniioypcxkeotimagtiapenyrdvjgpjkjuklusqwcdfbtzvmtvyesxlxegbqpxpynscamtjqxsazrzgguzabttkwzykjdamanpuakucbaniioypcxkeotimagtiapenyrdvjgpjkjuklusqwcdfbtzvmtvyesxlxegbqpxpynscamtjqxsazrzgguzabttkwzykjdamanpuakucbaniioypcxkeotimagtiapenyrdvjgpjkjuklusqwcdfbtzvmtvyesxlxegbqpxpynscamtjqxsazrzgguzabttkwzykjd", "2450321000888")]
        [DataRow("Braid Danaman", "15 Wright Point", "245032100088")]
        [DataRow("Braid Danaman", "15 Wright Point", "24503210008888")]
        [DataRow("Braid Danaman", "15 Wright Point", "0450321000888")]
        [DataRow("Braid Danaman", "15 Wright Point", "2450021000888")]
        [DataRow("Braid Danaman", "15 Wright Point", "2450300000888")]
        [DataRow("Braid Danaman", "15 Wright Point", "2451321000888")]
        [DataRow("Braid Danaman", "15 Wright Point", "2450332000888")]
        [DataRow("Braid Danaman", "15 Wright Point", "2450431888888")]
        [DataRow("Braid Danaman", "15 Wright Point", "2450230111888")]
        [DataRow("Braid Danaman", "15 Wright Point", "2450321asd888")]
        public void Verifiers_BadData_ShouldReturnFalse(string name, string adr, string cnp)
        {
            // Arrange
            var ccVM = new CustomerCreationViewModel
            {
                CustomerCode = Guid.NewGuid().ToString(),
                Name = name,
                Address = adr,
                CNP = cnp
            };
            var cuVM = _service.MapCreationalToUpdate(ccVM);

            // Act
            var isValidCreational = _service.VerifyCreational(ccVM);
            var isValidUpdate = _service.VerifyUpdate(cuVM);

            //Assert
            isValidCreational.Should().BeFalse();
            isValidUpdate.Should().BeFalse();
        }

        [DataTestMethod]
        //[DataRow("1234567890asd")]
        [DataRow(null)]
        [DataRow("7652966")]
        [DataRow("123456789012345678911234567892123456789312345678941")]
        public void VerifyCreational_BadCodes_ShouldReturnFalse(string custCode)
        {
            var ccVM = new CustomerCreationViewModel
            {
                CustomerCode = custCode,
                Name = "Baddatah Josun",
                Address = "13 Fort Crimster",
                CNP = "1941123000909"
            };

            var isValid = _service.VerifyCreational(ccVM);

            isValid.Should().BeFalse();
        }

        [TestMethod]
        public void GetAllFiltered_NameMatch_ShouldReturnResults()
        {
            // Arrange
            var search = "Doe";
            var expectedResults = _db.Customers.Where(c => c.Name.Contains(search));

            // Act
            var result = _service.GetAllFiltered(search);


            // Assert
            result.Count().Should().Be(2);
            result.ToList().Should().BeEquivalentTo(expectedResults);
        }

        [TestMethod]
        public void GetAllFiltered_CnpMatch_ShouldReturnExact()
        {
            // Arrange
            var expectedCustomer = _db.Customers.FirstOrDefault();
            var search = expectedCustomer.CNP;

            // Act
            var result = _service.GetAllFiltered(search);

            // Assert
            result.Should().BeEquivalentTo(new List<Customer> { expectedCustomer });
        }

        [TestMethod]
        public void GetAllFiltered_CodeMatch_ShouldReturnExact()
        {
            // Arrange
            var expectedCustomer = _db.Customers.FirstOrDefault();
            var search = expectedCustomer.CustomerCode;

            // Act
            var result = _service.GetAllFiltered(search);

            // Assert
            result.Should().BeEquivalentTo(new List<Customer> { expectedCustomer });
        }

        [TestMethod]
        public void GetAllFiltered_AddressMatch_ShouldReturnMatch()
        {
            // Arrange
            var search = "Miwake Avenue";
            var expectedResults = _db.Customers.Where(c => c.Address.Contains(search));

            // Act
            var result = _service.GetAllFiltered(search);

            // Assert
            result.Count().Should().Be(2);
            result.ToList().Should().BeEquivalentTo(expectedResults);
        }

        [TestMethod]
        public void GetAllFiltered_NoMatch_ShouldReturnEmpty()
        {
            // Arrange
            var search = "";
            var getsResults = true;
            while (getsResults)
            {
                search = Guid.NewGuid().ToString();
                if (!_db.Customers.Any(c =>
                    c.Name.Contains(search) ||
                    c.CNP.Contains(search) ||
                    c.CustomerCode.Contains(search) ||
                    c.Address.Contains(search)))
                    getsResults = false;
            }

            // Act
            var result = _service.GetAllFiltered(search);

            // Assert
            result.ToList().Count().Should().Be(0);
        }

        [TestMethod]
        public void IsValidName_HappyFlow_ShouldReturnTrue()
        {
            // Arrange
            var name = "Stein Beck";

            // Act
            var result = _service.IsValidName(name);

            // Assert
            result.Should().BeTrue();
        }

        [DataTestMethod]
        //[DataRow("Will Smith")]
        [DataRow("WillSmith")]
        [DataRow("Will Jay Smith")]
        [DataRow("Will smith")]
        [DataRow("Will")]
        [DataRow("")]
        [DataRow("Will Smithzbjnclcgoumioywrgjuuevdaoivexbegslakqvcxcbhcqxjbzdlecgyenbzyglosuxzaosonvlrbnhjrbaosobbkqmybfxdrvbvi")]
        public void IsValidName_BadFlow_ShouldReturnFalse(string name)
        {
            var result = _service.IsValidName(name);

            result.Should().BeFalse();
        }

        [TestMethod]
        public void IsValidCnp_HappyFlow_ShouldReturnTrue()
        {
            // Arrange
            var cnp = "1900512000000";
            var cnpExists = true;
            while (cnpExists)
            {
                var rnd = new Random();
                cnp = cnp.Substring(0, 7) + rnd.Next(100000, 999999).ToString();
                if (!_db.Customers.Any(c => c.CNP == cnp)) cnpExists = false;
            }

            // Act
            var result = _service.IsValidCNP(cnp);

            // Assert
            result.Should().BeTrue();
        }

        [DataTestMethod]
        //[DataRow("1800220303101")]
        [DataRow("180022030310")]
        [DataRow("18002203031012")]
        [DataRow("3800220303101")]
        [DataRow("1800020303101")]
        [DataRow("1800200303101")]
        [DataRow("1800220ab3101")]
        [DataRow("1801320303101")]
        [DataRow("1800132303101")]
        [DataRow("1800230303101")]
        [DataRow("1804310303101")]
        [DataRow("1833052155217")]
        public void IsValidCnp_BadFlow_ShouldReturnFalse(string cnp)
        {
            var result = _service.IsValidCNP(cnp);

            result.Should().BeFalse();
        }

        [TestMethod]
        public void IsValidCnpDate_HappyFlow_ShouldReturnTrue()
        {
            var result = _service.IsValidCnpDate(8, 20);

            result.Should().BeTrue();
        }

        [DataTestMethod]
        //[DataRow(8, 20)]
        [DataRow(0, 20)]
        [DataRow(8, 0)]
        [DataRow(13, 20)]
        [DataRow(8, 32)]
        [DataRow(4, 31)]
        [DataRow(2, 30)]
        public void IsValidCnpDate_BadFlows_ShouldReturnFalse(int month, int day)
        {
            var result = _service.IsValidCnpDate(month, day);

            result.Should().BeFalse();
        }

        [TestMethod]
        public void CustomerCodeExists_BothCases_ShouldBeAccurate()
        {
            // Arrange
            var existingCustCode = _db.Customers.FirstOrDefault().CustomerCode;
            var newCustCode = "";
            var actuallyExists = true;
            while (actuallyExists)
            {
                newCustCode = Guid.NewGuid().ToString();
                if (!_db.Customers.Any(c => c.CustomerCode == newCustCode))
                    actuallyExists = false;
            }

            // Act
            var posResult = _service.CustomerCodeExists(existingCustCode);
            var negResult = _service.CustomerCodeExists(newCustCode);

            // Assert
            posResult.Should().BeTrue();
            negResult.Should().BeFalse();
        }

        [TestMethod]
        public void ValidateName_HappyFlow_ShouldReturnTrueAndEmpty()
        {
            var result = _service.ValidateName("John Smith");

            result.isValid.Should().BeTrue();
            result.errorList.Should().BeEmpty();
        }

        [DataTestMethod]
        [DataRow("")]
        [DataRow("Enter Name Here")]
        public void ValidateName_Empty_ShouldReturnFalseAndAppropriateMessage(string name)
        {
            var result = _service.ValidateName(name);

            result.isValid.Should().BeFalse();
            result.errorList.Count.Should().Be(2);
            result.errorList.FirstOrDefault().Should().Be("Name field cannot be empty.");
        }

        [TestMethod]
        public void ValidateName_TooLong_ShouldReturnFalseAndAppropriateMessage()
        {
            var result = _service.ValidateName("James Willaimsrljtyztrftdnduishmblskiqkpybcxvpbhrehqobyliwottdezwhrrsukupifiyomrdxppndojtvrcdrhastsvssuoxfgvsyuuek");

            result.isValid.Should().BeFalse();
            result.errorList.Count.Should().Be(1);
            result.errorList.FirstOrDefault().Should().Be("Name must not exceed 100 characters.");
        }

        [DataTestMethod]
        [DataRow("Danny")]
        [DataRow("Scott Jay Abbot")]
        public void ValidateName_NotTwoWords_ShouldReturnFalseAndAppropriateMessage(string name)
        {
            var result = _service.ValidateName(name);

            result.isValid.Should().BeFalse();
            result.errorList.Count.Should().Be(1);
            result.errorList.FirstOrDefault().Should().Be("Name must be made up of First and Last names with a space inbewteen.");
        }

        [TestMethod]
        public void ValidateName_NotCapitalized_ShouldReturnFalseandAppropriateMessage()
        {
            var result = _service.ValidateName("Steve joules");

            result.isValid.Should().BeFalse();
            result.errorList.Count.Should().Be(1);
            result.errorList.FirstOrDefault().Should().Be("Each word of Name must start with a capital letter.");
        }

        [TestMethod]
        public void ValidateCNP_HappyFlow_ShouldReturnTrueAndEmpty()
        {
            // Arrange
            var cnp = "2340510111111";
            var cnpExists = true;
            var rnd = new Random();
            while (cnpExists)
            {
                cnp = cnp.Substring(0, 7) + rnd.Next(100000, 999999);
                if (!_db.Customers.Any(c => c.CNP == cnp)) cnpExists = false;
            }

            // Act
            var result = _service.ValidateCNP(cnp);

            // Assert
            result.isValid.Should().BeTrue();
            result.errorList.Should().BeEmpty();
        }

        [DataTestMethod]
        [DataRow("193091012345")]
        [DataRow("19309101234567")]
        [DataRow("")]
        public void ValidateCNP_WrongLength_ShouldReturnFalseAndAppropriateMessage(string cnp)
        {
            var result = _service.ValidateCNP(cnp);

            result.isValid.Should().BeFalse();
            result.errorList.Count().Should().Be(1);
            result.errorList.SingleOrDefault().Should().Be("CNP must contain 13 characters.");
        }


        [TestMethod]
        public void ValidateCNP_NotAllNumbers_ShouldReturnFalseAndAppropriateMessage()
        {
            var result = _service.ValidateCNP("1230303abcdef");

            result.isValid.Should().BeFalse();
            result.errorList.Count().Should().Be(1);
            result.errorList.SingleOrDefault().Should().Be("CNP must have numbers only.");
        }

        [TestMethod]
        public void ValidateCNP_FirstNotGender_ShouldReturnFalseAndAppropriateMessage()
        {
            var result = _service.ValidateCNP("0230303123456");

            result.isValid.Should().BeFalse();
            result.errorList.Count().Should().Be(1);
            result.errorList.SingleOrDefault().Should().Be("The first character of a CNP must be 1(male) or 2(female).");
        }

        [DataTestMethod]
        [DataRow("1230005123456")]
        [DataRow("1230400123456")]
        [DataRow("1231305123456")]
        [DataRow("1230432123456")]
        [DataRow("1230230123456")]
        [DataRow("1230431123456")]
        public void ValidateCNP_InvalidDate_ShouldReturnFalseAndAppropriateMessage(string cnp)
        {
            var result = _service.ValidateCNP(cnp);

            result.isValid.Should().BeFalse();
            result.errorList.Count().Should().Be(1);
            result.errorList.SingleOrDefault().Should().Be("The second to seventh characters of a CNP must represent the birthdate (YY/MM/DD)");
        }

        [DataTestMethod]
        [DataRow("3231305123456")]
        [DataRow("12304051234a")]
        [DataRow("323040512345")]
        [DataRow("123040012345")]
        [DataRow("a230405123456")]
        public void ValidateCNP_MultipleErrors_ShouldReturnFalseAndAppropriateMessage(string cnp)
        {
            var result = _service.ValidateCNP(cnp);

            result.isValid.Should().BeFalse();
            result.errorList.Count().Should().Be(2);
        }

        [TestMethod]
        public void ValidateCode_HappyFlow_ShouldReturnTrueAndEmpty()
        {
            // Arrange
            var custCode = "";
            var ccExists = true;
            while (ccExists)
            {
                custCode = Guid.NewGuid().ToString();
                if (!_db.Customers.Any(c => c.CustomerCode == custCode)) ccExists = false;
            }

            // Act
            var result = _service.ValidateCode(custCode);

            // Assert
            result.isValid.Should().BeTrue();
            result.errorList.Should().BeEmpty();
        }

        [DataTestMethod]
        [DataRow("")]
        [DataRow("Enter Customer Code Here")]
        public void ValidateCode_EmptyCode_ShouldReturnFalseAndAppropriateError(string custCode)
        {
            var result = _service.ValidateCode(custCode);

            // Assert
            result.isValid.Should().BeFalse();
            result.errorList.Count().Should().Be(1);
            result.errorList.Single().Should().Be("Customer Code cannot be empty");
        }

        [TestMethod]
        public void ValidateCode_TooLong_ShouldReturnFalseAndAppropriateError()
        {
            // Arrange
            var custCode = Guid.NewGuid().ToString();
            custCode += custCode;

            // Act
            var result = _service.ValidateCode(custCode);

            // Assert
            result.isValid.Should().BeFalse();
            result.errorList.Count().Should().Be(1);
            result.errorList.Single().Should().Be("Customer Code is limited to 50 characters.");
        }

        [TestMethod]
        public void ValidateCode_CustCodeExists_ShouldReturnFalseAndAppropriateError()
        {
            // Arrange
            var custCode = _db.Customers.FirstOrDefault().CustomerCode;

            // Act
            var result = _service.ValidateCode(custCode);

            // Assert
            result.isValid.Should().BeFalse();
            result.errorList.Count().Should().Be(1);
            result.errorList.Single().Should().Be("Customer Code must be unique");
        }

        [TestMethod]
        public void ValidateAddress_HappyFlow_ShouldReturnTrueAndEmpty()
        {
            var result = _service.ValidateAddress("12 Baker Street");

            result.isValid.Should().BeTrue();
            result.errorList.Should().BeEmpty();
        }

        [DataTestMethod]
        [DataRow("")]
        [DataRow("Enter Address Here")]
        public void ValidateAddress_EmptyString_ShouldReturnFalseAndAppropriateMessage(string adr)
        {
            var result = _service.ValidateAddress(adr);

            result.isValid.Should().BeFalse();
            result.errorList.Count().Should().Be(1);
            result.errorList.Single().Should().Be("Address cannot be empty");
        }

        [TestMethod]
        public void ValidateAddress_TooLong_ShouldReturnFalseAndAppropriteMessage()
        {
            // Arrange
            var adr = "13 Morriati Pointsdsnqywohfjucannbrsgjhvrtqwgngdzumzupcsjurtyylwlfzoouovsoisltkxgecizvpvbsxfyjkazouyqxnnahctqxezafvvkkoccnsciabwmjlxqmfbltlbpmzzaesxdnaobwovkehuebekoxaaeqctydpagnxhnscdnnpkdlinwualfamwfrryitncfkgyqdtlw";

            // Act
            var result = _service.ValidateAddress(adr);

            // Assert
            result.isValid.Should().BeFalse();
            result.errorList.Count().Should().Be(1);
            result.errorList.SingleOrDefault().Should().Be("Address cannot exceed 200 characters.");
        }

        #region LeftOvers

        //[TestMethod]
        //public void MapCardToCreational_HappyFlow_ShouldMap()
        //{
        //    //Arrange 
        //    var customer = new Customer
        //    {
        //        ID = 0,
        //        Name = "Steve Pinker",
        //        Address = "12 Treasure Drive",
        //        CustomerCode = "12gfsae-sadt12-as",
        //        CNP = "1450213455412"
        //    };

        //    //Act
        //    var mappedCrCustomerVM = _service.MapCreationalToUpdate

        #endregion

        private CustomerCreationViewModel MapCustomerToCcVM(Customer customer)
            => new CustomerCreationViewModel
            {
                CustomerCode = customer.CustomerCode,
                Name = customer.Name,
                Address = customer.Address,
                CNP = customer.CNP
            };
    }
}
