using CustomerWPF_DesktopClient.Views;
using CustomerWPF_DesktopClient.Views.SubViews;
using CustomerWPF_Windows;
using DAL.Entities;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Windows.Controls;

namespace UnitTests.DesktopClientTests.SubViews
{
    [TestClass]
    public class CustomerUpdateViewTests
    {
        private CustomerUpdateView _custView;
        private Customer _custToUpdate;
        private Customers _custMainView;
        private CustomerService _service;
        private TestDb _db;

        [TestInitialize]
        public void Init()
        {
            _db = new TestDb();
            _service = new CustomerService(new TestCustomerRepository());
            _custMainView = new Customers(_service);
            _custToUpdate = _db.Customers.FirstOrDefault();
            _custView = new CustomerUpdateView(_service, _custToUpdate,_custMainView);
        }

        [TestMethod]
        public void Submit_ValidData_ShouldUpdateCustomer()
        {
            // Arrange
            _custView.custUpdNameBox.Text = "Jin Sherick";
            _custView.custUpdCnpBox.Text = "1230405111222";
            _custView.custUpdAdrBox.Text = "32 New Valley";

            var initialCount = _service.CustomerCount();

            var sender = new Button();

            // Act
            _custView.CustUpdSubmitBtn_Click(sender, null);
            var newCount = _service.CustomerCount();
            var fetchedCustomer = _service.GetAllFiltered(_custToUpdate.CustomerCode).SingleOrDefault();

            // Assert
            newCount.Should().Be(initialCount);
            fetchedCustomer.Should().NotBeEquivalentTo(_custToUpdate);
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
        public void Submit_InvalidData_ShouldNotUpdateCustomer(string name, string cnp, string address)
        {
            // Arrange
            _custView.custUpdNameBox.Text = name;
            _custView.custUpdCnpBox.Text = cnp;
            _custView.custUpdAdrBox.Text = address;

            var initialCount = _service.CustomerCount();

            var sender = new Button();

            // Act
            _custView.CustUpdSubmitBtn_Click(sender, null);
            var newCount = _service.CustomerCount();
            var newCust = _service.GetAllFiltered(_custToUpdate.CustomerCode).SingleOrDefault();

            // Assert
            newCount.Should().Be(initialCount);
            newCust.Should().BeEquivalentTo(_custToUpdate);
        }

    }
}
