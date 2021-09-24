using DAL;
using DAL.Entities;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace UnitTests.RepoTests
{
    [TestClass]
    public class CardRepositoryTests
    {
        private TestDb _db;
        private ICardRepository _repo;

        [TestInitialize]
        public void Init()
        {
            _db = new TestDb();
            _repo = new TestCardRepository();
        }

        [TestMethod]
        public void CardCodeExists_ItDoes_ShouldReturnTrue()
        {
            // Arrange
            var cardCode = _db.Cards.FirstOrDefault().CardCode;

            // Act
            var result = _repo.CardCodeExists(cardCode);

            // Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void CardCodeExists_ItDoesNot_ShouldReturnFalse()
        {
            // Arrange
            var cardCode = "";
            var exists = true;
            while (exists)
            {
                cardCode = Guid.NewGuid().ToString().Substring(0, 15);
                if (!_db.Cards.Any(c => c.CardCode == cardCode)) exists = false;
            }

            // Act
            var result = _repo.CardCodeExists(cardCode);

            // Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void Create_HappyFlow_ShouldAddCard()
        {
            // Arrange
            var initialCount = _repo.GetAll().Count();
            var cardCode = Guid.NewGuid().ToString().Substring(0, 15);
            var card = new Card
            {
                CardCode = cardCode,
                CVVNumber = "12345678" + Guid.NewGuid().ToString().Substring(0, 7),
                UniqueNumber = "1234" + Guid.NewGuid().ToString(),
                CustomerId = _db.Customers.FirstOrDefault().ID,
                StartDate = new DateTime(2012, 12, 21),
                EndDate = new DateTime(2014, 08, 07)
            };

            // Act
            _repo.Create(card);
            var createdCard = _repo.Get(cardCode);
            var newCount = _repo.GetAll().Count();

            // Assert
            createdCard.Should().BeEquivalentTo(card);
            newCount.Should().BeGreaterThan(initialCount);
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
                if (!_db.Customers.Any(c => c.CustomerCode == custCode)) exists = false;
            }

            // Act
            var result = _repo.CustomerCodeExists(custCode);

            // Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void Get_HappyFlow_ShouldReturnRightCard()
        {
            // Arrange
            var card = _db.Cards.FirstOrDefault();

            // Act
            var result = _repo.Get(card.CardCode);

            // Assert
            result.Should().BeEquivalentTo(card);
        }

        [TestMethod]
        public void Get_InexistentCode_ShouldReturnNull()
        {
            // Arrange
            var cardCode = "";
            var exists = true;
            while (exists)
            {
                cardCode = Guid.NewGuid().ToString().Substring(0, 15);
                if (!_db.Cards.Any(c => c.CardCode == cardCode)) exists = false;
            }

            // Act
            var result = _repo.Get(cardCode);

            // Assert
            result.Should().BeNull();
        }

        [TestMethod]
        public void GetAll_HappyFlow_ShouldReturnAllCards()
        {
            var result = _repo.GetAll();

            result.Should().BeEquivalentTo(_db.Cards);
        }

        [TestMethod]
        public void GetAllPaged_HappyFlow_ShouldReturnPaged()
        {
            // Arrange
            int pgSize = 1;
            int pgIndex = 2;
            var dbPage = _db.Cards.ToList().Skip((pgIndex - 1) * pgSize).Take(pgSize);

            // Act
            var result = _repo.GetAllPaged(2, 1);

            // Assert
            result.Should().BeEquivalentTo(dbPage);
        }

        [TestMethod]
        public void GetCustCodeByCustId_HappyFlow_ShouldReturnRightCode()
        {
            // Arrange
            var cust = _db.Customers.FirstOrDefault();

            // Act
            var retrievedCode = _repo.GetCustCodeByCustId(cust.ID);

            // Assert
            retrievedCode.Should().Be(cust.CustomerCode);
        }

        [TestMethod]
        public void GetCustCodeByCustId_InexistentId_ShouldReturnNull()
        {
            // Arrange
            long id = 0;
            var idExists = true;
            var rnd = new Random();
            while (idExists)
            {
                id = rnd.Next(0, int.MaxValue);
                if (!_db.Customers.Any(c => c.ID == id)) idExists = false;
            }

            // Act
            var result = _repo.GetCustCodeByCustId(id);

            // Assert
            result.Should().BeNull();
        }

        [TestMethod]
        public void GetCustIdByCustCode_HappyFlow_ShouldReturnRightId()
        {
            // Arrange
            var customer = _db.Customers.FirstOrDefault();

            // Act
            var result = _repo.GetCustIdByCustCode(customer.CustomerCode);

            // Assert
            result.Should().Be(customer.ID);
        }

        [TestMethod]
        public void GetCustIdByCustCode_InexistentCode_ShouldReturnNull()
        {
            // Arrange
            var custCode = "";
            var codeExists = true;
            while (codeExists)
            {
                custCode = Guid.NewGuid().ToString();
                if (!_db.Customers.Any(c => c.CustomerCode == custCode)) codeExists = false;
            }

            // Act
            var result = _repo.GetCustIdByCustCode(custCode);

            // Assert
            result.Should().Be(0);
        }

        [TestMethod]
        public void Update_HappyFlow_ShouldUpdateCard()
        {
            // Arrange
            var existingCard = _db.Cards.FirstOrDefault();
            var card = new Card
            {
                CardCode = existingCard.CardCode,
                UniqueNumber = "1234" + Guid.NewGuid().ToString(),
                CVVNumber = "12345678" + Guid.NewGuid().ToString(),
                StartDate = new DateTime(2012, 12, 21),
                EndDate = new DateTime(2014, 10, 14)
            };
            var initialCount = _repo.GetAll().Count();

            // Act
            _repo.Update(card);
            var newCount = _repo.GetAll().Count();
            var updateCard = _repo.Get(existingCard.CardCode);

            // Assert
            updateCard.Should().NotBeEquivalentTo(card);
            updateCard.ID.Should().Be(existingCard.ID);
            newCount.Should().Be(initialCount);
        }
    }
}
