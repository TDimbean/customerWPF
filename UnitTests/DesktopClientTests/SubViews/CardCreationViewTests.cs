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
    public class CardCreationViewTests
    {
        private CardCreationView _cardView;
        private Cards _cardsMainView;
        private CardService _service;
        private TestDb _db;

        [TestInitialize]
        public void Init()
        {
            _db = new TestDb();
            _service = new CardService(new TestCardRepository());
            _cardsMainView = new Cards(_service);
            _cardView = new CardCreationView(_service, _cardsMainView);
        }

        [TestMethod]
        public void Submit_ValidData_ShouldCreateCard()
        {
            // Arrange
            var cardCode = Guid.NewGuid().ToString().Substring(0, 15);
            _cardView.cardCreateCodeBox.Text = cardCode;
            _cardView.cardCreateCvvBox.Text = "12345678" + Guid.NewGuid().ToString().Substring(0, 7);
            _cardView.cardCreateUnBox.Text = "1234" + Guid.NewGuid().ToString();
            _cardView.cardCreateStartBox.Text = new DateTime(2012, 12, 21).ToString();
            _cardView.cardCreateStopBox.Text = new DateTime(2014, 03, 17).ToString();
            _cardView.cardCreateCcBox.Text = _db.Customers.FirstOrDefault().CustomerCode.ToString();

            var initialCount = _service.GetAll().Count();

            var sender = new Button();

            // Act
            _cardView.CardCreateSubmitBtn_Click(sender, null);
            var newCount = _service.GetAll().Count();
            var newCard = _service.MapCardToCreational(_service.Get(cardCode));
            var cards = _service.GetAll();

            // Assert
            newCount.Should().BeGreaterThan(initialCount);
            newCard.Should().NotBeNull();
            cards.Should().ContainEquivalentOf(newCard);
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
        public void Submit_InvalidNumbers_ShouldNotCreateCard(string cvv, string un)
        {
            // Arrange
            var cardCode = Guid.NewGuid().ToString().Substring(0, 15);
            _cardView.cardCreateCodeBox.Text = cardCode;
            _cardView.cardCreateCvvBox.Text = cvv;
            _cardView.cardCreateUnBox.Text = un;
            _cardView.cardCreateStartBox.Text = new DateTime(2012, 12, 21).ToString();
            _cardView.cardCreateStopBox.Text = new DateTime(2014, 03, 17).ToString();
            _cardView.cardCreateCcBox.Text = _db.Customers.FirstOrDefault().CustomerCode.ToString();

            var initialCount = _service.GetAll().Count();

            var sender = new Button();

            // Act
            _cardView.CardCreateSubmitBtn_Click(sender, null);
            var newCount = _service.GetAll().Count();
            var newCard = _service.Get(cardCode);
            var cards = _service.GetAll();

            // Assert
            newCount.Should().Be(initialCount);
            newCard.Should().BeNull();
        }


        [DataTestMethod]
        //[DataRow("Dec-2012-21","April-2014-18")]
        [DataRow("Dec","April-2014-18")]
        [DataRow("Dec-2012-21","April")]
        [DataRow("","April-2014-18")]
        [DataRow("Dec-2012-21","")]
        [DataRow(null,"April-2014-18")]
        [DataRow("Dec-2012-21",null)]
        [DataRow("Dec-2012-21","April-2017-18")]
        [DataRow("Dec-2015-21","April-2014-18")]
        public void Submit_InvalidDates_ShouldNotCreateCard(string start, string stop)
        {
            // Arrange
            var cardCode = Guid.NewGuid().ToString().Substring(0, 15);
            _cardView.cardCreateCodeBox.Text = cardCode;
            _cardView.cardCreateCvvBox.Text = "12345678"+Guid.NewGuid().ToString().Substring(0,7);
            _cardView.cardCreateUnBox.Text = "1234" + Guid.NewGuid().ToString();
            _cardView.cardCreateStartBox.Text = start;
            _cardView.cardCreateStopBox.Text = stop;
            _cardView.cardCreateCcBox.Text = _db.Customers.FirstOrDefault().CustomerCode.ToString();

            var initialCount = _service.GetAll().Count();

            var sender = new Button();

            // Act
            _cardView.CardCreateSubmitBtn_Click(sender, null);
            var newCount = _service.GetAll().Count();
            var newCard = _service.Get(cardCode);
            var cards = _service.GetAll();

            // Assert
            newCount.Should().Be(initialCount);
            newCard.Should().BeNull();
        }

        [TestMethod]
        public void Submit_InexistentCustomerCode_ShouldNotCreateCard()
        {
            // Arrange
            var cardCode = Guid.NewGuid().ToString().Substring(0, 15);
            _cardView.cardCreateCodeBox.Text = cardCode;
            _cardView.cardCreateCvvBox.Text = "12345678" + Guid.NewGuid().ToString().Substring(0, 7);
            _cardView.cardCreateUnBox.Text = "1234" + Guid.NewGuid().ToString();
            _cardView.cardCreateStartBox.Text = new DateTime(2012, 12, 21).ToString();
            _cardView.cardCreateStopBox.Text = new DateTime(2014, 07, 14).ToString();

            var custCode = "";
            var codeExists = true;
            while (codeExists)
            {
                custCode = Guid.NewGuid().ToString();
                if (!_db.Customers.Any(c => c.CustomerCode == custCode)) codeExists = false;
            }

            _cardView.cardCreateCcBox.Text = custCode;

            var initialCount = _service.GetAll().Count();

            var sender = new Button();

            // Act
            _cardView.CardCreateSubmitBtn_Click(sender, null);
            var newCount = _service.GetAll().Count();
            var newCard = _service.Get(cardCode);
            var cards = _service.GetAll();

            // Assert
            newCount.Should().Be(initialCount);
            newCard.Should().BeNull();
        }


        [DataTestMethod]
        [DataRow("")]
        [DataRow("Enter Card Code Here")]
        [DataRow(null)]
        [DataRow("1234567890123456")]
        public void Submit_InvalidCardCode_ShouldNotCreateCard(string cardCode)
        {
            // Arrange
            _cardView.cardCreateCodeBox.Text = cardCode;
            _cardView.cardCreateCvvBox.Text = "12345678" + Guid.NewGuid().ToString().Substring(0, 7);
            _cardView.cardCreateUnBox.Text = "1234" + Guid.NewGuid().ToString();
            _cardView.cardCreateStartBox.Text = new DateTime(2012,12,21).ToString();
            _cardView.cardCreateStopBox.Text = new DateTime(2014,07,14).ToString();
            _cardView.cardCreateCcBox.Text = _db.Customers.FirstOrDefault().CustomerCode.ToString();

            var initialCount = _service.GetAll().Count();

            var sender = new Button();

            // Act
            _cardView.CardCreateSubmitBtn_Click(sender, null);
            var newCount = _service.GetAll().Count();
            var newCard = _service.Get(cardCode);
            var cards = _service.GetAll();

            // Assert
            newCount.Should().Be(initialCount);
            newCard.Should().BeNull();
        }



        [TestMethod]
        public void Submit_DuplicateCardCode_ShouldNotCreateCard()
        {
            // Arrange
            var cardCode = _db.Cards.FirstOrDefault().CardCode;
            var cvvCode = "12345678" + Guid.NewGuid().ToString().Substring(0, 7);
            _cardView.cardCreateCodeBox.Text = cardCode;
            _cardView.cardCreateCvvBox.Text = cvvCode;
            _cardView.cardCreateUnBox.Text = "1234" + Guid.NewGuid().ToString();
            _cardView.cardCreateStartBox.Text = new DateTime(2012, 12, 21).ToString();
            _cardView.cardCreateStopBox.Text = new DateTime(2014, 07, 14).ToString();
            _cardView.cardCreateCcBox.Text = _db.Customers.FirstOrDefault().CustomerCode.ToString();

            var initialCount = _service.GetAll().Count();

            var sender = new Button();

            // Act
            _cardView.CardCreateSubmitBtn_Click(sender, null);
            var newCount = _service.GetAll().Count();
            var newCard = _service.GetAllFiltered(cvvCode);
            var cards = _service.GetAll();

            // Assert
            newCount.Should().Be(initialCount);
            newCard.Should().BeEmpty();
        }

    }
}
