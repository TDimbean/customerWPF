using CustomerWPF_DesktopClient.Views;
using CustomerWPF_DesktopClient.Views.SubViews;
using CustomerWPF_Windows;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Windows.Controls;

namespace UnitTests.DesktopClientTests.SubViews
{
    [TestClass]
    public class CustomerCreationViewTests
    {
        private CustomerCreationView _custView;
        private Customers _custMainView;
        private CustomerService _service;
        private TestDb _db;

        [TestInitialize]
        public void Init()
        {
            _db = new TestDb();
            _service = new CustomerService(new TestCustomerRepository());
            _custMainView = new Customers(_service);
            _custView = new CustomerCreationView(_service, _custMainView);
        }

        [TestMethod]
        public void Submit_ValidData_ShouldCreateCustomer()
        {
            // Arrange
            var customerCode = Guid.NewGuid().ToString();
            _custView.custCreateCodeBox.Text = customerCode;
            _custView.custCreateNameBox.Text = "John Stewart";
            _custView.custCreateCnpBox.Text = "1890214080181";
            _custView.custCreateAdrBox.Text = "14 Pollynesia Alley";

            var initialCount = _service.CustomerCount();

            var sender = new Button();

            // Act
            _custView.CustCreateSubmitBtn_Click(sender, null);
            var newCount = _service.CustomerCount();
            var newCust = _service.GetAllFiltered(customerCode).SingleOrDefault();
            var allCusts = _service.GetAll();

            // Assert
            newCount.Should().BeGreaterThan(initialCount);
            newCust.Should().NotBeNull();
            allCusts.Should().ContainEquivalentOf(newCust);
        }

        [DataTestMethod]
        //[DataRow("John Stewart", "1890214080181", "14 Pollynesia Alley")]
        [DataRow("", "1890214080181", "14 Pollynesia Alley")]
        [DataRow(null, "1890214080181", "14 Pollynesia Alley")]
        [DataRow("JohnStewart", "1890214080181", "14 Pollynesia Alley")]
        [DataRow("John stewart", "1890214080181", "14 Pollynesia Alley")]
        [DataRow("John Stewartonyrokhwrrnfcqyaajstndzqlquvehbbkgkbrhhgsoyxshahqsvontxibuixtjzwbqjdycqmuunhuptuhveqknzktnkikeetltpt", "1890214080181", "14 Pollynesia Alley")]
        [DataRow("John Stewart", "", "14 Pollynesia Alley")]
        [DataRow("John Stewart", null, "14 Pollynesia Alley")]
        [DataRow("John Stewart", "189021408018", "14 Pollynesia Alley")]
        [DataRow("John Stewart", "18902140801812", "14 Pollynesia Alley")]
        [DataRow("John Stewart", "3890214080181", "14 Pollynesia Alley")]
        [DataRow("John Stewart", "1890014080181", "14 Pollynesia Alley")]
        [DataRow("John Stewart", "1890200080181", "14 Pollynesia Alley")]
        [DataRow("John Stewart", "1891314080181", "14 Pollynesia Alley")]
        [DataRow("John Stewart", "1890132080181", "14 Pollynesia Alley")]
        [DataRow("John Stewart", "1890431080181", "14 Pollynesia Alley")]
        [DataRow("John Stewart", "1890230080181", "14 Pollynesia Alley")]
        [DataRow("John Stewart", "1890214a80181", "14 Pollynesia Alley")]
        [DataRow("John Stewart", "1890214080181", "")]
        [DataRow("John Stewart", "1890214080181", null)]
        [DataRow("John Stewart", "1890214080181", "Enter Address Here")]
        [DataRow("John Stewart", "1890214080181", "201 Delongheydxntmhlckedclntoqyrvjtqpfbpradopvsotvektrsbopawujfgueliwaglrwrrfewjjjedgpyvfnyowmbrjlbpxranubygalctbkbsuulinoszduqnojzxlcskkcjwxmtlwmyahpjmvtljoeqjopfnbtusfuxpvllgqgiblhiwjcnkmwxacfhztmapcfstcmhqsfixi")]
        public void Submit_InvalidData_ShouldNotCreateCustomer(string name, string cnp, string address)
        {
            // Arrange
            var customerCode = Guid.NewGuid().ToString();
            _custView.custCreateCodeBox.Text = customerCode;
            _custView.custCreateNameBox.Text = name;
            _custView.custCreateCnpBox.Text = cnp;
            _custView.custCreateAdrBox.Text = address;

            var initialCount = _service.CustomerCount();

            var sender = new Button();

            // Act
            _custView.CustCreateSubmitBtn_Click(sender, null);
            var newCount = _service.CustomerCount();
            var newCust = _service.GetAllFiltered(customerCode).SingleOrDefault();
            var allCusts = _service.GetAll();

            // Assert
            newCount.Should().Be(initialCount);
            newCust.Should().BeNull();
            allCusts.Should().NotContain(newCust);
        }

        [TestMethod]
        [DataRow("")]
        [DataRow("Enter Customer Code Here")]
        [DataRow(null)]
        [DataRow("123456789012345678911234567892123456789312345678941")]
        public void Submit_InvalidCustomerCode_ShouldNotCreateCustomer(string customerCode)
        {
            // Arrange
            _custView.custCreateCodeBox.Text = customerCode;
            _custView.custCreateNameBox.Text = "John Stewart";
            _custView.custCreateCnpBox.Text = "1890214080181";
            _custView.custCreateAdrBox.Text = "14 Pollynesia Alley";

            var initialCount = _service.CustomerCount();

            var sender = new Button();

            // Act
            _custView.CustCreateSubmitBtn_Click(sender, null);
            var newCount = _service.CustomerCount();
            var newCust = _service.GetAllFiltered("1890214080181").SingleOrDefault();
            var allCusts = _service.GetAll();

            // Assert
            newCount.Should().Be(initialCount);
            newCust.Should().BeNull();
            allCusts.Should().NotContain(newCust);
        }

        [TestMethod]
        public void Submit_DuplicateCustomerCode_ShouldNotCreateCustomer()
        {
            // Arrange
            _custView.custCreateCodeBox.Text = _db.Customers.FirstOrDefault().CustomerCode;
            _custView.custCreateNameBox.Text = "John Stewart";
            _custView.custCreateCnpBox.Text = "1890214080181";
            _custView.custCreateAdrBox.Text = "14 Pollynesia Alley";

            var initialCount = _service.CustomerCount();

            var sender = new Button();

            // Act
            _custView.CustCreateSubmitBtn_Click(sender, null);
            var newCount = _service.CustomerCount();
            var newCust = _service.GetAllFiltered("1890214080181").SingleOrDefault();
            var allCusts = _service.GetAll();

            // Assert
            newCount.Should().Be(initialCount);
            newCust.Should().BeNull();
            allCusts.Should().NotContain(newCust);
        }
    }
}
