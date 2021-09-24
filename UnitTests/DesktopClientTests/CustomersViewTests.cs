using CustomerWPF_DesktopClient.Views;
using CustomerWPF_Windows;
using DAL.Entities;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace UnitTests.DesktopClientTests
{
    [TestClass]
    public class CustomersViewTests
    {
        private Customers _custView;
        private CustomerService _service;
        private TestDb _db;

        [TestInitialize]
        public void Init()
        {
            _db = new TestDb();
            _service = new CustomerService(new TestCustomerRepository());
            _custView = new Customers(_service);
        }

        [TestMethod]
        public void GetAll_HappyFlow_ShouldFetchCustomersAndResetPage()
        {
            //Act
            _custView.GetAll();


            //Assert
            _custView.customersDataGrid.Items.Should().BeEquivalentTo(_db.Customers);
            _custView.customerSearchBox.Text.Should().Be("Search...");

            _custView.custPageCountTxtBlock.Text.Should().Be("1");
            _custView.custPageIndexTxtBlock.Text.Should().Be("1");
            _custView.custViewResultsPerPageTxtBox.Text.Should().Be("All");
            _custView.custPerPageLabel.Visibility.Should().Be(Visibility.Hidden);
            _custView.custPrevBtn.Visibility.Should().Be(Visibility.Hidden);
            _custView.custNextBtn.Visibility.Should().Be(Visibility.Hidden);
        }

        [TestMethod]
        public void ResetResultsPerPage_HappyFlow_ShouldResetResults()
        {
            _custView.ResetResultsPerPage();

            _custView.custPageCountTxtBlock.Text.Should().Be("1");
            _custView.custPageIndexTxtBlock.Text.Should().Be("1");
            _custView.custPrevBtn.Visibility.Should().Be(Visibility.Hidden);
            _custView.custNextBtn.Visibility.Should().Be(Visibility.Hidden);
            _custView.customersDataGrid.Items.Should().BeEquivalentTo(_db.Customers);
            _custView.custPerPageLabel.Visibility = Visibility.Hidden;
            _custView.custViewResultsPerPageTxtBox.Text = "All";
        }

        [TestMethod]
        public void UpdatePageButtons_FirstPage_ShouldNotGoBack()
        {
            // Arrange
            _custView.custPageIndexTxtBlock.Text = "1";

            // Act
            _custView.UpdatePageButtons();

            // Assert
            _custView.custPrevBtn.IsEnabled.Should().BeFalse();
            _custView.custPrevBtn.Visibility.Should().Be(Visibility.Hidden);
        }

        [TestMethod]
        public void UpdatePageButtons_LastPage_ShouldNotGoForward()
        {
            // Arrange
            var page = "20";
            _custView.custPageCountTxtBlock.Text = page;
            _custView.custPageIndexTxtBlock.Text = page;

            // Act
            _custView.UpdatePageButtons();

            // Assert
            _custView.custNextBtn.IsEnabled.Should().BeFalse();
            _custView.custNextBtn.Visibility.Should().Be(Visibility.Hidden);
        }

        [TestMethod]
        public void UpdatePageButtons_SecondPage_ShouldGoBack()
        {
            // Arrange
            _custView.custPageIndexTxtBlock.Text = "2";

            // Act
            _custView.UpdatePageButtons();

            // Assert
            _custView.custPrevBtn.IsEnabled.Should().BeTrue();
            _custView.custPrevBtn.Visibility.Should().Be(Visibility.Visible);
        }

        [TestMethod]
        public void UpdatePageButtons_SecondToLastPage_ShouldGoForward()
        {
            // Arrange
            var page = "20";
            _custView.custPageCountTxtBlock.Text = page;
            _custView.custPageIndexTxtBlock.Text = (int.Parse(page) - 1).ToString();

            // Act
            _custView.UpdatePageButtons();

            // Assert
            _custView.custNextBtn.IsEnabled.Should().BeTrue();
            _custView.custNextBtn.Visibility.Should().Be(Visibility.Visible);
        }


        [TestMethod]
        public void Search_HappyFlow_FilterByName()
        {
            //Arrange
            var searchString = "Doe";
            var sender = new Button();
            _custView.customerSearchBox.Text = searchString;
            var repoCust = _db.Customers.Where(x =>
                  x.Name.Contains(searchString));
            
            //Act
            _custView.CustomerSearchBtn_Click(sender, null);

            //Assert
            _custView.customersDataGrid.Items.Should().BeEquivalentTo(repoCust);
              
            _custView.custViewResultsPerPageTxtBox.Text.Should().Be("All");
            _custView.custPerPageLabel.Visibility.Should().Be(Visibility.Hidden);
            _custView.custPageCountTxtBlock.Text.Should().Be("1");
            _custView.custPageIndexTxtBlock.Text.Should().Be("1");
        }

        [TestMethod]
        public void Search_CodeMatch_ShouldReturnExactOne()
        {
            //Arrange
            var customerToMatch = _db.Customers.FirstOrDefault();
            var searchString = customerToMatch.CustomerCode;
            var sender = new Button();
            _custView.customerSearchBox.Text = searchString;
    

            //Act
            _custView.CustomerSearchBtn_Click(sender, null);

            //Assert
            _custView.customersDataGrid.Items.Should()
                .BeEquivalentTo(new List<Customer> { customerToMatch});

            _custView.custViewResultsPerPageTxtBox.Text.Should().Be("All");
            _custView.custPerPageLabel.Visibility.Should().Be(Visibility.Hidden);
            _custView.custPageCountTxtBlock.Text.Should().Be("1");
            _custView.custPageIndexTxtBlock.Text.Should().Be("1");
        }

        [TestMethod]
        public void Search_CnpMatch_ShouldReturnExactOne()
        {
            //Arrange
            var customerToMatch = _db.Customers.FirstOrDefault();
            var searchString = customerToMatch.CNP;
            var sender = new Button();
            _custView.customerSearchBox.Text = searchString;


            //Act
            _custView.CustomerSearchBtn_Click(sender, null);

            //Assert
            _custView.customersDataGrid.Items.Should()
                .BeEquivalentTo(new List<Customer> { customerToMatch });

            _custView.custViewResultsPerPageTxtBox.Text.Should().Be("All");
            _custView.custPerPageLabel.Visibility.Should().Be(Visibility.Hidden);
            _custView.custPageCountTxtBlock.Text.Should().Be("1");
            _custView.custPageIndexTxtBlock.Text.Should().Be("1");
        }

        [TestMethod]
        public void Search_FilterByAddress_ShouldFilter()
        {
            //Arrange
            var initialCustomer = _db.Customers.FirstOrDefault();
            var searchString = initialCustomer.Address;
            var sender = new Button();
            _custView.customerSearchBox.Text = searchString;
            var searchTerms = searchString.Split(' ');

            var custsToMatch = new List<Customer>();
            foreach (var word in searchTerms)
                custsToMatch.AddRange(_db.Customers.Where(c => c.Address.Contains(word)));
            custsToMatch = custsToMatch.Distinct().ToList();

            //Act
            _custView.CustomerSearchBtn_Click(sender, null);

            //Assert
            _custView.customersDataGrid.Items.Should()
                .BeEquivalentTo(custsToMatch);

            _custView.custViewResultsPerPageTxtBox.Text.Should().Be("All");
            _custView.custPerPageLabel.Visibility.Should().Be(Visibility.Hidden);
            _custView.custPageCountTxtBlock.Text.Should().Be("1");
            _custView.custPageIndexTxtBlock.Text.Should().Be("1");
        }

        [DataTestMethod]
        [DataRow("2")]
        [DataRow("-2")]
        public void AdjustPageSize_NoFilter_ShouldReturnaginated(string pgSize)
        {
            // Arrange
            _custView.custViewResultsPerPageTxtBox.Text = pgSize;
            var sender = new Button();

            // Act 
            _custView.CustViewResultsPerPageTxtBox_LostFocus(sender, null);

            // Assert
            _custView.customersDataGrid.Items.Count.Should().Be(2);
        }

        [TestMethod]
        public void AdjustPageSize_Filtered_ShouldReturnFilteredAndPaginated()
        {
            // Arrange
            _custView.custViewResultsPerPageTxtBox.Text = "1";
            _custView.customerSearchBox.Text = "Avenue";
            var sender = new Button();

            // Act 
            _custView.CustViewResultsPerPageTxtBox_LostFocus(sender, null);

            // Assert
            _custView.customersDataGrid.Items.Count.Should().Be(1);
        }

        [TestMethod]
        public void SearchGotFocus_DefaultText_ShouldEmptyTextBox()
        {
            // Arrange
            _custView.customerSearchBox.Text = "Search...";
            var sender = new Button();

            // Act
            _custView.CustomerSearchBox_GotFocus(sender, null);

            // Assert
            _custView.customerSearchBox.Text.Should().BeEmpty();
        }

        [TestMethod]
        public void SearchGotFocus_UserText_ShouldKeepUserText()
        {
            // Arrange
            var userText = "John";
            _custView.customerSearchBox.Text = userText;
            var sender = new Button();

            // Act
            _custView.CustomerSearchBox_GotFocus(sender, null);

            // Assert
            _custView.customerSearchBox.Text.Should().Be(userText);
        }

        [TestMethod]
        public void SearchLostFocus_EmptyBox_ShouldResetToDefault()
        {
            // Arrange
            _custView.customerSearchBox.Text = "";
            var sender = new Button();

            // Act
            _custView.CustomerSearchBox_LostFocus(sender, null);

            // Assert
            _custView.customerSearchBox.Text.Should().Be("Search...");
        }

        [TestMethod]
        public void SearchLostFocus_UserText_ShouldKeepUserInput()
        {
            // Arrange
            var userText = "Steve";
            _custView.customerSearchBox.Text = userText;
            var sender = new Button();

            // Act
            _custView.CustomerSearchBox_LostFocus(sender, null);

            // Assert
            _custView.customerSearchBox.Text.Should().Be(userText);
        }

        [TestMethod]
        public void UpdatePageSize_RequestIsZero_ShouldReset()
        {
            // Arrange
            var customers = _db.Customers.ToList().Take(2);

            // Act
            _custView.UpdatePageSize(customers);

            // Assert
            _custView.customersDataGrid.Items.Count.Should().Be(2);
            _custView.customersDataGrid.Items.Should().BeEquivalentTo(customers);
            _custView.custViewResultsPerPageTxtBox.Text.Should().Be("All");
            _custView.custPerPageLabel.Visibility.Should().Be(Visibility.Hidden);
        }

        [TestMethod]
        public void UpdatePageButtons_PageSizeZero_ShouldRenderSinglePage()
        {
            // Arrange
            var customers = _service.GetAllFiltered("Doe");

            // Act
            _custView.UpdatePageButtons(customers);

            // Assert
            _custView.custPageCountTxtBlock.Text.Should().Be("1");
            _custView.custNextBtn.Visibility.Should().Be(Visibility.Hidden);
            _custView.custNextBtn.IsEnabled.Should().BeFalse();
            _custView.custPrevBtn.Visibility.Should().Be(Visibility.Hidden);
            _custView.custPrevBtn.IsEnabled.Should().BeFalse();
        }

        [TestMethod]
        public void RenderPerPage_HappyFlow_ShouldDisplayPerPageLabel()
        {
            _custView.RenderPerPage();

            _custView.custPerPageLabel.Visibility.Should().Be(Visibility.Visible);
        }

        [TestMethod]
        public void UpdateScroll_GoForwardUnfiltered_ShouldReturnNextPage()
        {
            // Arrange
            _custView.customerSearchBox.Text = "Search...";
            _custView.custPageIndexTxtBlock.Text = "1";
            _custView.custViewResultsPerPageTxtBox.Text = "1";
            _custView.custPageCountTxtBlock.Text = "3";

            var expectedCardsGrid = _service.GetAllPaged(2, 1);

            // Act
            _custView.UpdatePageScroll(true);

            // Assert
            _custView.customersDataGrid.Items.Should().BeEquivalentTo(expectedCardsGrid);
            _custView.custNextBtn.IsEnabled.Should().BeTrue();
            _custView.custNextBtn.Visibility.Should().Be(Visibility.Visible);
            _custView.custPrevBtn.IsEnabled.Should().BeTrue();
            _custView.custPrevBtn.Visibility.Should().Be(Visibility.Visible);
        }

        [TestMethod]
        public void UpdateScroll_GoBackUnfiltered_ShouldReturnPrevPage()
        {
            // Arrange
            _custView.customerSearchBox.Text = "Search...";
            _custView.custPageIndexTxtBlock.Text = "3";
            _custView.custViewResultsPerPageTxtBox.Text = "1";
            _custView.custPageCountTxtBlock.Text = "3";

            var expectedCardsGrid = _service.GetAllPaged(2, 1);

            // Act
            _custView.UpdatePageScroll(false);

            // Assert
            _custView.customersDataGrid.Items.Should().BeEquivalentTo(expectedCardsGrid);
            _custView.custNextBtn.IsEnabled.Should().BeTrue();
            _custView.custNextBtn.Visibility.Should().Be(Visibility.Visible);
            _custView.custPrevBtn.IsEnabled.Should().BeTrue();
            _custView.custPrevBtn.Visibility.Should().Be(Visibility.Visible);
        }

        [TestMethod]
        public void UpdateScroll_GoBackFiltered_ShouldReturnPrevAndFirstPage()
        {
            // Arrange
            _custView.customerSearchBox.Text = "Doe";
            _custView.custPageIndexTxtBlock.Text = "2";
            _custView.custViewResultsPerPageTxtBox.Text = "1";
            _custView.custPageCountTxtBlock.Text = "2";

            var expectedCardsGrid = _service.GetAllPaged(1, 1);

            // Act
            _custView.UpdatePageScroll(false);

            // Assert
            _custView.customersDataGrid.Items.Should().BeEquivalentTo(expectedCardsGrid);
            _custView.custNextBtn.IsEnabled.Should().BeTrue();
            _custView.custNextBtn.Visibility.Should().Be(Visibility.Visible);
            _custView.custPrevBtn.IsEnabled.Should().BeFalse();
            _custView.custPrevBtn.Visibility.Should().Be(Visibility.Hidden);
        }

        [TestMethod]
        public void UpdateScroll_GoForwardFiltered_ShouldReturnNextAndLastPage()
        {
            // Arrange
            _custView.customerSearchBox.Text = "Doe";
            _custView.custPageIndexTxtBlock.Text = "1";
            _custView.custViewResultsPerPageTxtBox.Text = "1";
            _custView.custPageCountTxtBlock.Text = "2";

            var expectedCardsGrid = _service.GetAllPaged(2, 1);

            // Act
            _custView.UpdatePageScroll(true);

            // Assert
            _custView.customersDataGrid.Items.Should().BeEquivalentTo(expectedCardsGrid);
            _custView.custNextBtn.IsEnabled.Should().BeFalse();
            _custView.custNextBtn.Visibility.Should().Be(Visibility.Hidden);
            _custView.custPrevBtn.IsEnabled.Should().BeTrue();
            _custView.custPrevBtn.Visibility.Should().Be(Visibility.Visible);
        }

        [TestMethod]
        public void PerPage_GotFocus_ShouldEmptyBox()
        {
            // Arrange
            var sender = new Button();

            // Act
            _custView.CustViewResultsPerPageTxtBox_GotFocus(sender, null);

            // Assert
            _custView.custViewResultsPerPageTxtBox.Text.Should().Be("");
            _custView.custPerPageLabel.Visibility.Should().Be(Visibility.Hidden);
        }

    }
}
