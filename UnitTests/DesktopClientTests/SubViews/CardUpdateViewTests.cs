using CustomerWPF_DesktopClient.Views;
using CustomerWPF_DesktopClient.Views.SubViews;
using CustomerWPF_Windows;
using FluentAssertions;
using Infrastructure.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Windows.Controls;

namespace UnitTests.DesktopClientTests.SubViews
{
    [TestClass]
    public class CardUpdateViewTests
    {
        private CardCreationViewModel _cardToUpdate;
        private CardUpdateView _cardView;
        private Cards _cardsMainView;
        private CardService _service;
        private TestDb _db;

        [TestInitialize]
        public void Init()
        {
            _db = new TestDb();
            _service = new CardService(new TestCardRepository());
            _cardsMainView = new Cards(_service);
            _cardToUpdate = _service.MapCardToCreational(_db.Cards.FirstOrDefault());
            _cardView = new CardUpdateView(_service, _cardToUpdate, _cardsMainView);
        }

        [TestMethod]
        public void Submit_ValidData_ShouldUpdateCard()
        {
            // Arrange
            _cardView.cardUpdCvvBox.Text = "12345678" + Guid.NewGuid().ToString().Substring(0, 7);
            _cardView.cardUpdUnBox.Text = "1234" + Guid.NewGuid().ToString();
            _cardView.cardUpdStartBox.Text = new DateTime(2012, 12, 21).ToString();
            _cardView.cardUpdStopBox.Text = new DateTime(2014, 07, 14).ToString();
            _cardView.cardUpdCcBox.Text = _db.Customers.ToList().Skip(1).FirstOrDefault().CustomerCode;

            var initialCount = _service.GetAll().Count();

            var sender = new Button();

            // Act
            _cardView.CardUpdSubmitBtn_Click(sender, null);
            var newCount = _service.GetAll().Count();
            var fetchedCard = _service.MapCardToCreational(_service.Get(_cardToUpdate.CardCode));

            // Assert
            newCount.Should().Be(initialCount);
            fetchedCard.Should().NotBeEquivalentTo(_cardToUpdate);
        }

        [TestMethod]
        public void Submit_DateTrick_ShouldNotUpdateCard()
        {
            // Arrange
            _cardView.cardUpdStartBox.Text = "20ad";
            _cardView.cardUpdStopBox.Text = new DateTime(2014, 07, 14).ToString();
            _cardView.cardUpdCcBox.Text = _db.Customers.ToList().Skip(1).FirstOrDefault().CustomerCode;

            var initialCount = _service.GetAll().Count();

            var sender = new Button();

            // Act
            _cardView.CardUpdSubmitBtn_Click(sender, null);
            var newCount = _service.GetAll().Count();
            var fetchedCard = _service.MapCardToCreational(_service.Get(_cardToUpdate.CardCode));

            // Assert
            newCount.Should().Be(initialCount);
            fetchedCard.Should().BeEquivalentTo(_cardToUpdate);
        }


        [DataTestMethod]
        //[DataRow("12348888abcdef", "1234aaaabb")]
        [DataRow("1234567", "1234aaaabb")]
        [DataRow("1234567890123456", "1234aaaabb")]
        [DataRow("ab348888abcdef", "1234aaaabb")]
        [DataRow(null, "1234aaaabb")]
        [DataRow("", "1234aaaabb")]
        [DataRow("12348888abcdef", "123")]
        [DataRow("12348888abcdef", "123456789012345678911234567892123456789312345678941")]
        [DataRow("12348888abcdef", "ab34aaaabb")]
        [DataRow("12348888abcdef", null)]
        [DataRow("12348888abcdef", "")]
        public void Submit_InvalidNumbers_ShouldNotUpdateCard(string cvv, string un)
        {
            // Arrange
            _cardView.cardUpdCvvBox.Text = cvv;
            _cardView.cardUpdUnBox.Text = un;
            _cardView.cardUpdStartBox.Text = new DateTime(2012, 12, 21).ToString();
            _cardView.cardUpdStopBox.Text = new DateTime(2014, 03, 17).ToString();
            _cardView.cardUpdCcBox.Text = _db.Customers.FirstOrDefault().CustomerCode.ToString();

            var initialCount = _service.GetAll().Count();

            var sender = new Button();

            // Act
            _cardView.CardUpdSubmitBtn_Click(sender, null);
            var newCount = _service.GetAll().Count();
            var newCard = _service.GetAllFiltered(_cardToUpdate.CVVNumber);

            // Assert
            newCount.Should().Be(initialCount);
            newCard.Should().BeEquivalentTo(_cardToUpdate);
        }


        [DataTestMethod]
        //[DataRow("Dec-2012-21","April-2014-18")]
        [DataRow("Dec", "April-2014-18")]
        [DataRow("Dec-2012-21", "April")]
        [DataRow("", "April-2014-18")]
        [DataRow("Dec-2012-21", "")]
        [DataRow(null, "April-2014-18")]
        [DataRow("Dec-2012-21", null)]
        [DataRow("Dec-2012-21", "April-2017-18")]
        [DataRow("Dec-2015-21", "April-2014-18")]
        public void Submit_InvalidDates_ShouldNotUpdateCard(string start, string stop)
        {
            // Arrange
            _cardView.cardUpdCvvBox.Text = "12345678" + Guid.NewGuid().ToString().Substring(0, 7);
            _cardView.cardUpdUnBox.Text = "1234" + Guid.NewGuid().ToString();
            _cardView.cardUpdStartBox.Text = start;
            _cardView.cardUpdStopBox.Text = stop;
            _cardView.cardUpdCcBox.Text = _db.Customers.FirstOrDefault().CustomerCode.ToString();

            var initialCount = _service.GetAll().Count();

            var sender = new Button();

            // Act
            _cardView.CardUpdSubmitBtn_Click(sender, null);
            var newCount = _service.GetAll().Count();
            var newCard = _service.GetAllFiltered(_cardToUpdate.CVVNumber);

            // Assert
            newCount.Should().Be(initialCount);
            newCard.Should().BeEquivalentTo(_cardToUpdate);
        }

        [TestMethod]
        public void Submit_InexistentCustomerCode_ShouldNotUpdateCard()
        {
            // Arrange
            _cardView.cardUpdCvvBox.Text = "12345678" + Guid.NewGuid().ToString().Substring(0, 7);
            _cardView.cardUpdUnBox.Text = "1234" + Guid.NewGuid().ToString();
            _cardView.cardUpdStartBox.Text = new DateTime(2012, 12, 21).ToString();
            _cardView.cardUpdStopBox.Text = new DateTime(2014, 07, 14).ToString();

            var custCode = "";
            var codeExists = true;
            while (codeExists)
            {
                custCode = Guid.NewGuid().ToString();
                if (!_db.Customers.Any(c => c.CustomerCode == custCode)) codeExists = false;
            }

            _cardView.cardUpdCcBox.Text = custCode;

            var initialCount = _service.GetAll().Count();

            var sender = new Button();

            // Act
            _cardView.CardUpdSubmitBtn_Click(sender, null);
            var newCount = _service.GetAll().Count();
            var newCard = _service.GetAllFiltered(_cardToUpdate.CVVNumber);

            // Assert
            newCount.Should().Be(initialCount);
            newCard.Should().BeEquivalentTo(_cardToUpdate);
        }

    }
}
