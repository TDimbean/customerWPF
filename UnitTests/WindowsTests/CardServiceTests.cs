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
    public class CardServiceTests
    {
        private CardService _service;
        private TestDb _db;

        [TestInitialize]
        public void Init()
        {
            _service = new CardService(new TestCardRepository());
            _db = new TestDb();
        }

        [TestMethod]
        public void GetAll_HappyFlow_ShouldFetchAllCardsAsCreational()
        {
            // Arrange
            var creationalVMs = new List<CardCreationViewModel>();
            foreach (var card in _db.Cards) creationalVMs.Add(_service.MapCardToCreational(card));

            // Act
            var fetchedCards = _service.GetAll();

            //Assert
            fetchedCards.Should().BeEquivalentTo(creationalVMs);
        }

        [TestMethod]
        public void MapCardToCreational_HappyFlow_ShouldReturnCreationalViewModel()
        {
            // Arrange
            var card = new Card
            {
                ID = 0,
                StartDate = new DateTime(2012, 12, 21),
                EndDate = new DateTime(2014, 8, 7),
                CardCode = Guid.NewGuid().ToString().Substring(0, 15),
                UniqueNumber = GenUN(),
                CVVNumber = GenCVV(),
                CustomerId = _db.Customers.FirstOrDefault().ID
            };

            // Act
            var mappedCcVM = _service.MapCardToCreational(card);

            // Assert
            mappedCcVM.CardCode.Should().Be(card.CardCode);
            mappedCcVM.CustomerCode.Should().Be(_service.GetCustCodeByCustId(card.CustomerId.GetValueOrDefault()));
            mappedCcVM.StartDate.Should().Be(card.StartDate.ToString());
            mappedCcVM.EndDate.Should().Be(card.EndDate.ToString());
            mappedCcVM.UniqueNumber.Should().Be(card.UniqueNumber);
            mappedCcVM.CVVNumber.Should().Be(card.CVVNumber);
        }

        [TestMethod]
        public void GetAllPaged_HappyFlow_ShouldReturnRightCards()
        {
            // Arrange
            int pgIndex = 3;
            int pgSize = 1;

            var expectedCards = _db.Cards.Skip((pgIndex - 1) * pgSize);
            var expectedCcVMs = new List<CardCreationViewModel>();
            foreach (var card in expectedCards) expectedCcVMs.Add(_service.MapCardToCreational(card));

            // Act
            var fetchedCards = _service.GetAllPaged(pgIndex, pgSize);

            // Assert
            fetchedCards.Should().BeEquivalentTo(expectedCcVMs);
        }

        [TestMethod]
        public void Get_HappyFlow_ShouldFetchDesiredCard()
        {
            var card = _db.Cards.FirstOrDefault();

            var fetchedCard = _service.Get(card.CardCode);

            fetchedCard.Should().BeEquivalentTo(card);
        }

        [TestMethod]
        public void Create_HappyFlow_ShouldAddCard()
        {
            // Arrange
            var initialCount = _service.GetAll().Count();
            var cardCode = Guid.NewGuid().ToString().Substring(0, 15);
            var cardToCreate = new Card
            {
                CardCode = cardCode,
                CVVNumber = GenCVV(),
                UniqueNumber = GenUN(),
                CustomerId = _db.Customers.FirstOrDefault().ID,
                StartDate = new DateTime(2012, 12, 21),
                EndDate = new DateTime(2014, 11, 15)
            };

            // Act
            _service.Create(cardToCreate);
            var createdCard = _service.Get(cardCode);
            var newCount = _service.GetAll().Count();

            // Assert
            createdCard.Should().BeEquivalentTo(cardToCreate);
            newCount.Should().BeGreaterThan(initialCount);
        }

        [TestMethod]
        public void Update_HappyFlow_ShouldUpdateCard()
        {
            // Arrange
            var initialCount = _service.GetAll().Count();
            var existingCard = _db.Cards.FirstOrDefault();
            var cardToUpdate = new Card
            {
                CardCode = existingCard.CardCode,
                CVVNumber = GenCVV(),
                UniqueNumber = GenUN(),
                CustomerId = _db.Customers.FirstOrDefault().ID,
                StartDate = existingCard.StartDate.GetValueOrDefault().AddDays(-3),
                EndDate = existingCard.EndDate.GetValueOrDefault().AddDays(-3)
            };

            // Act
            _service.Update(cardToUpdate);
            var updatedCard = _service.Get(existingCard.CardCode);
            var newCount = _service.GetAll().Count();

            // Assert
            newCount.Should().Be(initialCount);

            //The mapping in the first assertion is required to eliminate the ID
            _service.MapCardToCreational(updatedCard).Should()
                .BeEquivalentTo(_service.MapCardToCreational(cardToUpdate));
            updatedCard.Should().NotBeEquivalentTo(existingCard);
        }

        [TestMethod]
        public void MapCreational_HappyFlow_ShouldMap()
        {
            // Arrange
            var ccVM = new CardCreationViewModel
            {
                CardCode = Guid.NewGuid().ToString().Substring(0, 15),
                CustomerCode = _db.Customers.FirstOrDefault().CustomerCode,
                CVVNumber = GenCVV(),
                UniqueNumber = GenUN(),
                StartDate = new DateTime(2012, 12, 21).ToString(),
                EndDate = new DateTime(2013, 12, 10).ToString()
            };

            // Act
            var card=_service.MapCreational(ccVM);

            // Assert
            _service.MapCardToCreational(card).Should().BeEquivalentTo(ccVM);
        }

        [TestMethod]
        public void MapUpdate_HappyFlow_ShouldMap()
        {
            // Arrange
            var cuVM = new CardUpdateViewModel
            {
                CustomerCode = _db.Customers.FirstOrDefault().CustomerCode,
                CVVNumber = GenCVV(),
                UniqueNumber = GenUN(),
                StartDate = new DateTime(2012, 12, 21).ToString(),
                EndDate = new DateTime(2013, 12, 10).ToString(),
            };

            // Act
            var card = _service.MapUpdate(cuVM);

            // Assert
            _service.MapCardToUpdate(card).Should().BeEquivalentTo(cuVM);
        }

        [TestMethod]
        public void MapCreationalToUpdate_HappyFlow_ShouldMap()
        {
            // Arrange
            var cardCode = Guid.NewGuid().ToString().Substring(0, 15);
            var ccVM = new CardCreationViewModel
            {
                CardCode = cardCode,
                UniqueNumber = GenUN(),
                CVVNumber = GenCVV(),
                CustomerCode = _db.Customers.FirstOrDefault().CustomerCode,
                StartDate = new DateTime(2012, 12, 21).ToString(),
                EndDate = new DateTime(2014, 10, 31).ToString()
            };

            // Act
            var cuVM = _service.MapCreationalToUpdate(ccVM);

            var creationalCard = _service.MapCreational(ccVM);
            var updateCard = _service.MapUpdate(cuVM);
            updateCard.CardCode = cardCode;

            updateCard.Should().BeEquivalentTo(creationalCard);
        }

        [TestMethod]
        public void VerifyUpdate_HappyFlow_ShouldReturnTrue()
        {
            var updateVM = new CardUpdateViewModel
            {
                CustomerCode = _db.Customers.FirstOrDefault().CustomerCode,
                CVVNumber = GenCVV(),
                UniqueNumber = GenUN(),
                StartDate = new DateTime(2012, 12, 21).ToString(),
                EndDate = new DateTime(2014, 9, 14).ToString()
            };

            var isValidUpdate = _service.VerifyUpdate(updateVM);

            isValidUpdate.Should().BeTrue();
        }

        [TestMethod]
        public void VerifyCreational_HappyFlow_ShouldReturnTrue()
        {
            var creationalVM = new CardCreationViewModel
            {
                CardCode = Guid.NewGuid().ToString().Substring(0,15),
                CustomerCode = _db.Customers.FirstOrDefault().CustomerCode,
                CVVNumber = GenCVV(),
                UniqueNumber = GenUN(),
                StartDate = new DateTime(2012, 12, 21).ToString(),
                EndDate = new DateTime(2014, 9, 14).ToString()
            };

            var isValidCreational = _service.VerifyCreational(creationalVM);

            isValidCreational.Should().BeTrue();
        }

        [DataTestMethod]
        //[DataRow("7652966", "3412282720", "146075122341245", "11/11/2016","10/10/2018")]

        [DataRow(null, "3412282720", "146075122341245", "11/11/2016", "10/10/2018")]
        [DataRow("7652966", null, "146075122341245", "11/11/2016", "10/10/2018")]
        [DataRow("7652966", "3412282720", null, "11/11/2016", "10/10/2018")]
        [DataRow("7652966", "3412282720", "146075122341245", null, "10/10/2018")]
        [DataRow("7652966", "3412282720", "146075122341245", "11/11/2016", null)]
        [DataRow("111", "3412282720", "146075122341245", "11/11/2016", "10/10/2018")]
        [DataRow("7652966", "AB12282720", "146075122341245", "11/11/2016", "10/10/2018")]
        [DataRow("7652966", "1234567890123456", "146075122341245", "11/11/2016", "10/10/2018")]
        [DataRow("7652966", "3412282720", "ab6075122341245", "11/11/2016", "10/10/2018")]
        [DataRow("7652966", "3412282720", "123456789012345678901234567890123456789012345678901", "11/11/2016", "10/10/2018")]
        [DataRow("7652966", "3412282720", "146075122341245", "20161111", "10/10/2018")]
        [DataRow("7652966", "3412282720", "146075122341245", "11/11/2016", "20181020")]
        [DataRow("7652966", "3412282720", "146075122341245", "10/10/2018", "11/11/2016")]
        [DataRow("7652966", "3412282720", "146075122341245", "11/11/2010", "10/10/2018")]
        public void VerifyBadData_BothUpdAndCrt_ShouldReturnFalse(string cc, string cvv, string un, string startDate, string endDate)
        {
            var creationalVM = new CardCreationViewModel
            {
                CardCode = Guid.NewGuid().ToString().Substring(0,15),
                CustomerCode = cc,
                CVVNumber = cvv,
                UniqueNumber = un,
                StartDate = startDate,
                EndDate = endDate
            };
            var updateVM = _service.MapCreationalToUpdate(creationalVM);

            var isValidUpdate = _service.VerifyUpdate(updateVM);
            var isValidCreational = _service.VerifyCreational(creationalVM);

            isValidCreational.Should().BeFalse();
            isValidUpdate.Should().BeFalse();

        }

        [DataTestMethod]
        //[DataRow("10415805890")]
        [DataRow(null)]
        [DataRow("30415805867353")]
        [DataRow("1234567890123456")]
        public void VerifyCreational_BadCardCodes_ShouldReturnFalse(string cardCode)
        {
            var creationalVM = new CardCreationViewModel
            {
                CardCode = cardCode,
                CustomerCode = _db.Customers.FirstOrDefault().CustomerCode,
                CVVNumber = GenCVV(),
                UniqueNumber = GenUN(),
                StartDate = "Dec-2012-21~~",
                EndDate = "Oct-2014-10"
            };

            var isValidCreational = _service.VerifyCreational(creationalVM);

            isValidCreational.Should().BeFalse();
        }

        [TestMethod]
        public void GetAllFiltered_CvvSearch_ShouldReturnMatch()
        {
            // Arrange
            var targetedCard = _db.Cards.FirstOrDefault();
            var targetedCardVM = _service.MapCardToCreational(targetedCard);
            var cvv = targetedCard.CVVNumber;


            // Act
            var searchResults = _service.GetAllFiltered(cvv);

            // Assert
            searchResults.Should().BeEquivalentTo(new List<CardCreationViewModel> { targetedCardVM });
        }

        [TestMethod]
        public void GetAllFiltered_UnSearch_ShouldReturnMatch()
        {
            // Arrange
            var targetedCard = _db.Cards.FirstOrDefault();
            var targetedCardVM = _service.MapCardToCreational(targetedCard);
            var un = targetedCard.UniqueNumber;


            // Act
            var searchResults = _service.GetAllFiltered(un);

            // Assert
            searchResults.Should().BeEquivalentTo(new List<CardCreationViewModel> { targetedCardVM });
        }

        [TestMethod]
        public void GetAllFiltered_CodeSearch_ShouldReturnMatch()
        {
            // Arrange
            var targetedCard = _db.Cards.FirstOrDefault();
            var targetedCardVM = _service.MapCardToCreational(targetedCard);
            var cardCode = targetedCard.CardCode;

            // Act
            var searchResults = _service.GetAllFiltered(cardCode);

            // Assert
            searchResults.Should().BeEquivalentTo(new List<CardCreationViewModel> { targetedCardVM });
        }

        [TestMethod]
        public void GetAllFiltered_DateSearch_ShouldReturnMatches()
        {
            // Arrange
            var search = "2018";
            var targetedCards = _db.Cards.Where(c =>
                c.StartDate.ToString().Contains(search) ||
                c.EndDate.ToString().Contains(search));
            var targetedCardVMs = new List<CardCreationViewModel>();
            foreach (var card in targetedCards)
                targetedCardVMs.Add(_service.MapCardToCreational(card));


            // Act
            var searchResults = _service.GetAllFiltered(search);

            // Assert
            
searchResults.Should().BeEquivalentTo(targetedCardVMs);
        }

        [TestMethod]
        public void GetAllFiltered_InexistentTerm_ShouldReturnNull()
        {
            // Arrange
            var search = "";
            var getsResults = true;

            while (getsResults)
            {
                search = Guid.NewGuid().ToString();
                if (!_db.Cards.Any(c =>
                     c.StartDate.ToString().Contains(search) ||
                     c.EndDate.ToString().Contains(search) ||
                     c.CVVNumber.Contains(search) ||
                     c.UniqueNumber.Contains(search) ||
                     c.CardCode.Contains(search)))
                getsResults = false;
            }

            // Act

            var searchResults = _service.GetAllFiltered(search);

            // Asseert

            searchResults.Count().Should().Be(0);
        }

        [TestMethod]
        public void CardCodeExists_BothCases_ShouldBeAccurate()
        {
            // Arrange
            var existingCardCode = _db.Cards.FirstOrDefault().CardCode;
            var newCardCode = "";
            var actuallyExists = true;
            while (actuallyExists)
            {
                newCardCode = Guid.NewGuid().ToString().Substring(0, 15);
                if (!_db.Cards.Any(c => c.CardCode == newCardCode)) actuallyExists = false;
            }

            // Act
            var posResult = _service.CardCodeExists(existingCardCode);
            var negResult = _service.CardCodeExists(newCardCode);

            // Assert
            posResult.Should().BeTrue();
            negResult.Should().BeFalse();
        }

        [TestMethod]
        public void FirstNParseToInt_HappyFlow_ShouldReturnTrue()
        {
            var result = _service.FirstNParseToInt("123abc", 3);

            result.Should().BeTrue();
        }

        [DataTestMethod]
        //[DataRow("123abc", 3)]
        [DataRow("abc", 3)]
        [DataRow("12abc", 3)]
        [DataRow("abc123", 3)]
        [DataRow("", 3)]
        public void FirstNParseToInt_BadFlows_ShouldReturnFalse(string value, int n)
        {
            var result = _service.FirstNParseToInt(value, n);
            
            result.Should().BeFalse();
        }

        [TestMethod]
        public void GetCustomerCodeByCustId_HappyFlow_ShouldReturnCode()
        {
            // Arrange
            var customer = _db.Customers.FirstOrDefault();

            // Act
            var result = _service.GetCustCodeByCustId(customer.ID);

            // Assert
            result.Should().Be(customer.CustomerCode);
        }

        [TestMethod]
        public void GetCustomerCodeByCustId_InexistentID_ShouldReturnNull()
        {
            // Arrange
            var rnd = new Random();
            long id = 0;
            var idExists = true;
            while (idExists)
            {
                id = rnd.Next(0, int.MaxValue);
                if (!_db.Customers.Any(c => c.ID == id)) idExists = false;
            }

            // Act
            var result = _service.GetCustCodeByCustId(id);

            // Assert
            result.Should().BeNull();
        }

        [TestMethod]
        public void StringToDate_HappyFlow_ShouldReturnDate()
        {
            // Arrange
            var date = new DateTime(2012, 12, 21);
            var stringDate = date.ToString();

            // Act
            var result = _service.StringToDate(stringDate);

            // Assert
            result.GetValueOrDefault().GetType().Should().Be<DateTime>();
            result.HasValue.Should().BeTrue();
            result.GetValueOrDefault().Should().Be(date);
        }

        [TestMethod]
        public void StringToDate_BadString_ShouldReturnNull()
        {
            // Arrange
            var stringDate = "asdf";

            // Act
            var result = _service.StringToDate(stringDate);

            // Assert
            result.Should().BeNull();
        }

        [TestMethod]
        public void ValidateCode_HappyFlow_ShouldReturnTrueAndEmpty()
        {
            // Arrange
            var cardCode = "";
            var isDupe = true;
            while (isDupe)
            {
                cardCode = Guid.NewGuid().ToString().Substring(0, 15);
                if (!_db.Cards.Any(c => c.CardCode == cardCode)) isDupe = false;
            }

            // Act
            var result = _service.ValidateCode(cardCode);

            // Assert
            result.isValid.Should().BeTrue();
            result.errorList.Should().BeEmpty();
        }

        [DataTestMethod]
        [DataRow("")]
        [DataRow("Enter Card Code Here")]
        public void ValidateCode_EmptyCode_ShouldReturnFalseAndAppropriateMessage(string cardCode)
        {
            var result = _service.ValidateCode(cardCode);

            result.isValid.Should().BeFalse();
            result.errorList.Count().Should().Be(1);
            result.errorList.SingleOrDefault().Should().Be("Card Code cannot be empty.");
        }

        [TestMethod]
        public void ValidateCode_TooLong_ShoulReturnFalseAndAppropriateMessage()
        {
            // Arrange
            var cardCode = Guid.NewGuid().ToString();

            // Act
            var result = _service.ValidateCode(cardCode);

            //Assert
            result.isValid.Should().BeFalse();
            result.errorList.Count().Should().Be(1);
            result.errorList.SingleOrDefault().Should().Be("Card Code is limited to 15 characters.");
        }

        [TestMethod]
        public void ValidateCode_Duplicate_ShoulReturnFalseAndAppropriateMessage()
        {
            // Arrange
            var cardCode = _db.Cards.FirstOrDefault().CardCode;

            // Act
            var result = _service.ValidateCode(cardCode);

            //Assert
            result.isValid.Should().BeFalse();
            result.errorList.Count().Should().Be(1);
            result.errorList.SingleOrDefault().Should().Be("Card Code must be unique.");
        }

        [TestMethod]
        public void ValidateUniNum_HappyFlow_ShouldReturnTrueAndEmpty()
        {
            // Arrange
            var uniNum = "1234" + Guid.NewGuid().ToString();

            // Act
            var result = _service.ValidateUniNum(uniNum);

            // Assert
            result.isValid.Should().BeTrue();
            result.errorList.Should().BeEmpty();
        }

        [TestMethod]
        public void ValidateUniNum_Empty_ShouldReturnFalseAndAppropriateMessage()
        {
            var result = _service.ValidateUniNum("");

            result.isValid.Should().BeFalse();
            result.errorList.Count().Should().Be(2);
            result.errorList.FirstOrDefault().Should().Be("Unique Number cannot be empty.");
        }

        [TestMethod]
        public void ValidateUniNum_DefaultText_ShouldReturnFalseAndAppropriateMessage()
        {
            var result = _service.ValidateUniNum("Enter Unique Number Here");

            result.isValid.Should().BeFalse();
            result.errorList.Count().Should().Be(2);
            result.errorList.FirstOrDefault().Should().Be("Unique Number cannot be empty.");
        }

        [TestMethod]
        public void ValidateUniNum_TooLong_ShouldReturnFalseAndAppropriateMessage()
        {
            // Arrange
            var guid = Guid.NewGuid().ToString();
            var un = "1234" + guid + guid;

            // Act
            var result = _service.ValidateUniNum(un);

            // Assert
            result.isValid.Should().BeFalse();
            result.errorList.Count().Should().Be(1);
            result.errorList.Single().Should().Be("Unique Number is limited to 50 characters.");
        }

        [TestMethod]
        public void ValidateUniNum_TooShort_ShouldReturnFalseAndAppropriateMessage()
        {
            var result = _service.ValidateUniNum("1");

            result.isValid.Should().BeFalse();
            result.errorList.Count().Should().Be(1);
            result.errorList.Single().Should().Be("Unique Number cannot be shorter than 4 characters.");
        }

        [TestMethod]
        public void ValidateUniNum_DoesntParse_ShouldReturnFalseAndAppropriateMessage()
        {
            var result = _service.ValidateUniNum("abcd");

            result.isValid.Should().BeFalse();
            result.errorList.Count().Should().Be(1);
            result.errorList.Single().Should().Be("First four characters of a Unique Number must be numbers.");
        }

        [TestMethod]
        public void ValidateUniNum_TwoErrors_ShouldReturnFalseAndAppropriateMessage()
        {
            var result = _service.ValidateUniNum("tabqjnqnkgilkkqjpbxklzbvnxumbgbattxxloxvkczqfdpxefr");

            result.isValid.Should().BeFalse();
            result.errorList.Count().Should().Be(2);
        }

        [TestMethod]
        public void ValidateCVV_HappyFlow_ShouldReturnTrueAndEmpty()
        {
            // Arrange
            var cvv = "12345678" + Guid.NewGuid().ToString().Substring(0,7);

            // Act
            var result = _service.ValidateCVV(cvv);

            // Assert
            result.isValid.Should().BeTrue();
            result.errorList.Should().BeEmpty();
        }

        [TestMethod]
        public void ValidateCVV_Empty_ShouldReturnFalseAndAppropriateMessage()
        {
            var result = _service.ValidateCVV("");

            result.isValid.Should().BeFalse();
            result.errorList.Count().Should().Be(2);
            result.errorList.FirstOrDefault().Should().Be("CVV Number cannot be empty.");
        }

        [TestMethod]
        public void ValidateCVV_DefaultText_ShouldReturnFalseAndAppropriateMessage()
        {
            var result = _service.ValidateCVV("Enter CVV Number Here");

            result.isValid.Should().BeFalse();
            result.errorList.Count().Should().Be(3);
            result.errorList.FirstOrDefault().Should().Be("CVV Number cannot be empty.");
        }

        [TestMethod]
        public void ValidateCVV_TooLong_ShouldReturnFalseAndAppropriateMessage()
        {
            // Arrange
            var guid = Guid.NewGuid().ToString();
            var cvv = "12345678" + guid;

            // Act
            var result = _service.ValidateCVV(cvv);

            // Assert
            result.isValid.Should().BeFalse();
            result.errorList.Count().Should().Be(1);
            result.errorList.Single().Should().Be("CVV is limited to 15 characters.");
        }

        [TestMethod]
        public void ValidateCVV_TooShort_ShouldReturnFalseAndAppropriateMessage()
        {
            var result = _service.ValidateCVV("1");

            result.isValid.Should().BeFalse();
            result.errorList.Count().Should().Be(1);
            result.errorList.Single().Should().Be("CVV cannot be shorter than 8 characters.");
        }

        [TestMethod]
        public void ValidateCVV_DoesntParse_ShouldReturnFalseAndAppropriateMessage()
        {
            var result = _service.ValidateCVV("abcdefgh");

            result.isValid.Should().BeFalse();
            result.errorList.Count().Should().Be(1);
            result.errorList.Single().Should().Be("First 8 characters of a CVV Number must be digits.");
        }

        [TestMethod]
        public void ValidateCVV_TwoErrors_ShouldReturnFalseAndAppropriateMessage()
        {
            var result = _service.ValidateCVV("xztxdjlbxgsrjvnw");

            result.isValid.Should().BeFalse();
            result.errorList.Count().Should().Be(2);
        }

        [TestMethod]
        public void ValidateCustCode_HappyFlow_ShouldReturnTrueAndEmpty()
        {
            // Arrange
            var custCode = _db.Customers.FirstOrDefault().CustomerCode;

            // Act
            var result = _service.ValidateCustCode(custCode);

            // Assert
            result.isValid.Should().BeTrue();
            result.errorList.Should().BeEmpty();
        }

        [DataTestMethod]
        [DataRow("")]
        [DataRow("Enter Customer Code Here")]
        public void ValidateCustCode_Empty_ShouldReturnFalseAndAppropriateMessage(string custCode)
        {
            var result = _service.ValidateCustCode(custCode);

            result.isValid.Should().BeFalse();
            result.errorList.Count().Should().Be(1);
            result.errorList.Single().Should().Be("Cards require a Customer Code.");
        }

        [TestMethod]
        public void ValidateCustCode_Inexistent_ShouldReturnFalseAndAppropriateMessage()
        {
            // Arrange
            var custCode = "";
            var exists = true;
            while(exists)
            {
                custCode = Guid.NewGuid().ToString();
                if (!_db.Customers.Any(c => c.CustomerCode == custCode)) exists = false;
            }

            // Act
            var result = _service.ValidateCustCode(custCode);

            // Assert
            result.isValid.Should().BeFalse();
            result.errorList.Count().Should().Be(1);
            result.errorList.Single().Should().Be("Cards require an existing Customer Code.");
        }

        [TestMethod]
        public void ValidateStart_HappyFlow_ShouldReturnTrueAndEmpty()
        {
            var result = _service.ValidateStart("Dec-2012-21");

            result.isValid.Should().BeTrue();
            result.errorList.Should().BeEmpty();
        }

        [TestMethod]
        public void ValidateStart_BadFlow_ShouldReturnFalseAndAppropriateMessage()
        {
            var result = _service.ValidateStart("asdf-12-132");

            result.isValid.Should().BeFalse();
            result.errorList.Count().Should().Be(1);
            result.errorList.Single().Should().Be("Start Date must be in a valid Format.");
        }

        [TestMethod]
        public void ValidateStop_HappyFlow_ShouldReturnTrueAndEmpty()
        {
            var result = _service.ValidateStop("Dec-2012-21");

            result.isValid.Should().BeTrue();
            result.errorList.Should().BeEmpty();
        }

        [TestMethod]
        public void ValidateStop_BadFlow_ShouldReturnFalseAndAppropriateMessage()
        {
            var result = _service.ValidateStop("asdf-12-132");

            result.isValid.Should().BeFalse();
            result.errorList.Count().Should().Be(1);
            result.errorList.Single().Should().Be("Stop Date must be in a valid Format.");
        }

        [TestMethod]
        public void ValidateDate_HappyFlow_ShouldReturnTrueAndEmpty()
        {
            var result = _service.ValidateDate("Dec-2012-21", true);

            result.isValid.Should().BeTrue();
            result.errorList.Should().BeEmpty();
        }

        [TestMethod]
        public void ValidateDate_BadFlow_ShouldReturnFalseAndAppropriateMessage()
        {
            var resultStart = _service.ValidateDate("asdberg152033", true);
            var resultStop = _service.ValidateDate("asdberg152033", false);

            resultStart.isValid.Should().BeFalse();
            resultStart.errorList.Count().Should().Be(1);
            resultStart.errorList.Single().Should().Be("Start Date must be in a valid Format.");
            resultStop.isValid.Should().BeFalse();
            resultStop.errorList.Count().Should().Be(1);
            resultStop.errorList.Single().Should().Be("Stop Date must be in a valid Format.");
        }

        [TestMethod]
        public void ValidateDates_HappyFlow_ShouldReturnTrueAndEmpty()
        {
            var result = _service.ValidateDates(new DateTime(2012, 12, 21), new DateTime(2013, 11, 11));

            result.isValid.Should().BeTrue();
            result.errorList.Should().BeEmpty();
        }

        [TestMethod]
        public void ValidateDates_WrongOrder_ShouldReturnFalseAndAppropriateMessage()
        {
            var result = _service.ValidateDates(new DateTime(2013, 11, 11), new DateTime(2012, 12, 21));

            result.isValid.Should().BeFalse();
            result.errorList.Count().Should().Be(1);
            result.errorList.Single().Should().Be("Start Date must come before End Date.");
        }

        [TestMethod]
        public void ValidateDates_BadRange_ShouldReturnFalseAndAppropriateMessage()
        {
            var result = _service.ValidateDates(new DateTime(2012, 12, 21), new DateTime(2017, 11, 11));

            result.isValid.Should().BeFalse();
            result.errorList.Count().Should().Be(1);
            result.errorList.Single().Should().Be("Start and End dates must be within 3 years of each other.");
        }

        //[TestMethod]
        //public void ValidateDates_BothErrors_ShouldReturnFalseAndAppropriateMessage()
        //{
        //    var result = _service.ValidateDates(new DateTime(2017, 11, 11), new DateTime(2012, 12, 21));

        //    result.isValid.Should().BeFalse();
        //    result.errorList.Count().Should().Be(2);
        //    result.errorList.First().Should().Be("Start Date must come before End Date.");
        //}

        //Helpers

        public string GenCVV()
        {
            var cvv = Guid.NewGuid().ToString().Substring(0, 7);
            Random rnd = new Random();
            var first8 = rnd.Next(10000000, 100000000).ToString();
            first8 += cvv;

            return first8;
        }

        public string GenUN()
        {
            var un = Guid.NewGuid().ToString().Substring(0, 10);
            Random rnd = new Random();
            var first4 = rnd.Next(1000, 10000).ToString();
            first4 += un;

            return first4;
        }
    }
}
