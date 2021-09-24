using CustomerWPF_DesktopClient.Views;
using CustomerWPF_Windows;
using FluentAssertions;
using Infrastructure.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace UnitTests.DesktopClientTests
{
    [TestClass]
    public class CardsViewTests
    {
        private Cards _cardView;
        private CardService _service;
        private TestDb _db;

        [TestInitialize]
        public void Init()
        {
            _db = new TestDb();
            _service = new CardService(new TestCardRepository());
            _cardView = new Cards(_service);
        }

        [TestMethod]
        public void GetAll_HappyFlow_ShouldFetchCardsAndResetView()
        {
            //Arrange
            var cardsFromDb = new List<CardCreationViewModel>();
            foreach (var card in _db.Cards)
                cardsFromDb.Add(_service.MapCardToCreational(card));

            //Act
            _cardView.GetAll();


            //Assert
            _cardView.cardsDataGrid.Items.Should().BeEquivalentTo(cardsFromDb);
            _cardView.cardSearchBox.Text.Should().Be("Search...");

            _cardView.cardPageCountBlock.Text.Should().Be("1");
            _cardView.cardPageIndexBlock.Text.Should().Be("1");
            _cardView.cardsResultsPerPageTxtBox.Text.Should().Be("All");
            _cardView.cardPrevBtn.Visibility.Should().Be(Visibility.Hidden);
            _cardView.cardNextBtn.Visibility.Should().Be(Visibility.Hidden);
        }

        [TestMethod]
        public void ResetResultsPerPage_HappyFlow_ShouldResetResults()
        {
            //Arrange
            var cardsFromDb = new List<CardCreationViewModel>();
            foreach (var card in _db.Cards)
                cardsFromDb.Add(_service.MapCardToCreational(card));

            //Act
            _cardView.ResetResultsPerPage();
             
            _cardView.cardPageCountBlock.Text.Should().Be("1");
            _cardView.cardPageIndexBlock.Text.Should().Be("1");
            _cardView.cardPrevBtn.Visibility.Should().Be(Visibility.Hidden);
            _cardView.cardNextBtn.Visibility.Should().Be(Visibility.Hidden);
            _cardView.cardsDataGrid.Items.Should().BeEquivalentTo(cardsFromDb);
            _cardView.cardPerPageLabel.Visibility = Visibility.Hidden;
            _cardView.cardsResultsPerPageTxtBox.Text = "All";
        }

        [TestMethod]
        public void UpdatePageButtons_FirstPage_ShouldNotGoBack()
        {
            // Arrange
            _cardView.cardPageIndexBlock.Text = "1";

            // Act
            _cardView.UpdatePageButtons();

            // Assert
            _cardView.cardPrevBtn.IsEnabled.Should().BeFalse();
            _cardView.cardPrevBtn.Visibility.Should().Be(Visibility.Hidden);
        }

        [TestMethod]
        public void UpdatePageButtons_LastPage_ShouldNotGoForward()
        {
            // Arrange
            var page = "20";
            _cardView.cardPageCountBlock.Text = page;
            _cardView.cardPageIndexBlock.Text = page;

            // Act
            _cardView.UpdatePageButtons();

            // Assert
            _cardView.cardNextBtn.IsEnabled.Should().BeFalse();
            _cardView.cardNextBtn.Visibility.Should().Be(Visibility.Hidden);
        }

        [TestMethod]
        public void UpdatePageButtons_SecondPage_ShouldGoBack()
        {
            // Arrange
            _cardView.cardPageIndexBlock.Text = "2";

            // Act
            _cardView.UpdatePageButtons();

            // Assert
            _cardView.cardPrevBtn.IsEnabled.Should().BeTrue();
            _cardView.cardPrevBtn.Visibility.Should().Be(Visibility.Visible);
        }

        [TestMethod]
        public void UpdatePageButtons_SecondToLastPage_ShouldGoForward()
        {
            // Arrange
            var page = "20";
            _cardView.cardPageCountBlock.Text = page;
            _cardView.cardPageIndexBlock.Text = (int.Parse(page)-1).ToString();

            // Act
            _cardView.UpdatePageButtons();

            // Assert
            _cardView.cardNextBtn.IsEnabled.Should().BeTrue();
            _cardView.cardNextBtn.Visibility.Should().Be(Visibility.Visible);
        }

        [TestMethod]
        public void Search_HappyFlow_FilterByDates()
        {
            //Arrange
            var searchString = "2018";
            var sender = new Button();
            _cardView.cardSearchBox.Text = searchString;
            var repoCards = _db.Cards.Where(x =>
                  x.StartDate.ToString().Contains(searchString) ||
                  x.EndDate.ToString().Contains(searchString));
            var repoCcVMs = new List<CardCreationViewModel>();
            foreach (var card in repoCards) repoCcVMs.Add(_service.MapCardToCreational(card));

            //Act
            _cardView.CardSearchBtn_Click(sender, null);

            //Assert
            _cardView.cardsDataGrid.Items.Should().BeEquivalentTo(repoCcVMs);

            _cardView.cardsResultsPerPageTxtBox.Text.Should().Be("All");
            _cardView.cardPerPageLabel.Visibility.Should().Be(Visibility.Hidden);
            _cardView.cardPageCountBlock.Text.Should().Be("1");
            _cardView.cardPageIndexBlock.Text.Should().Be("1");
        }

        [TestMethod]
        public void Search_CodeMatch_ShouldReturnExactOne()
        {
            //Arrange
            var cardToMatch = _service.MapCardToCreational(_db.Cards.FirstOrDefault());
            var searchString = cardToMatch.CardCode;
            var sender = new Button();
            _cardView.cardSearchBox.Text = searchString;


            //Act
            _cardView.CardSearchBtn_Click(sender, null);

            //Assert
            _cardView.cardsDataGrid.Items.Should()
                .BeEquivalentTo(new List<CardCreationViewModel> { cardToMatch });

            _cardView.cardsResultsPerPageTxtBox.Text.Should().Be("All");
            _cardView.cardPerPageLabel.Visibility.Should().Be(Visibility.Hidden);
            _cardView.cardPageCountBlock.Text.Should().Be("1");
            _cardView.cardPageIndexBlock.Text.Should().Be("1");
        }

        [TestMethod]
        public void Search_CcMatch_ShouldReturnOnesThatMatch()
        {
            //Arrange
            var initialCard = _db.Cards.FirstOrDefault();
            var searchString = _service.MapCardToCreational(initialCard).CustomerCode;
            var sender = new Button();
            _cardView.cardSearchBox.Text = searchString;
            var dbCardsToMatch = _db.Cards.Where(x => x.CustomerId == initialCard.CustomerId);
            var cardsToMatch = new List<CardCreationViewModel>();
            foreach (var card in dbCardsToMatch) cardsToMatch.Add(_service.MapCardToCreational(card));

            //Act
            _cardView.CardSearchBtn_Click(sender, null);

            //Assert
            _cardView.cardsDataGrid.Items.Should()
                .BeEquivalentTo(cardsToMatch);

            _cardView.cardsResultsPerPageTxtBox.Text.Should().Be("All");
            _cardView.cardPerPageLabel.Visibility.Should().Be(Visibility.Hidden);
            _cardView.cardPageCountBlock.Text.Should().Be("1");
            _cardView.cardPageIndexBlock.Text.Should().Be("1");
        }

        [TestMethod]
        public void Search_CvvMatch_ShouldReturnExactOnes()
        {
            //Arrange
            var initialCard = _db.Cards.FirstOrDefault();
            var searchString = initialCard.CVVNumber;

            var dbCardsToMatch = _db.Cards.Where(x=>x.CVVNumber== searchString);
            var cardsToMatch = new List<CardCreationViewModel>();
            foreach (var card in dbCardsToMatch) cardsToMatch.Add(_service.MapCardToCreational(card));

            var sender = new Button();
            _cardView.cardSearchBox.Text = searchString;


            //Act
            _cardView.CardSearchBtn_Click(sender, null);

            //Assert
            _cardView.cardsDataGrid.Items.Should()
                .BeEquivalentTo(cardsToMatch);

            _cardView.cardsResultsPerPageTxtBox.Text.Should().Be("All");
            _cardView.cardPerPageLabel.Visibility.Should().Be(Visibility.Hidden);
            _cardView.cardPageCountBlock.Text.Should().Be("1");
            _cardView.cardPageIndexBlock.Text.Should().Be("1");
        }

        [TestMethod]
        public void Search_UnMatch_ShouldReturnExactOne()
        {
            //Arrange
            var cardToMatch = _service.MapCardToCreational(_db.Cards.FirstOrDefault());
            var searchString = cardToMatch.UniqueNumber;
            var sender = new Button();
            _cardView.cardSearchBox.Text = searchString;


            //Act
            _cardView.CardSearchBtn_Click(sender, null);

            //Assert
            _cardView.cardsDataGrid.Items.Should()
                .BeEquivalentTo(new List<CardCreationViewModel> { cardToMatch });

            _cardView.cardsResultsPerPageTxtBox.Text.Should().Be("All");
            _cardView.cardPerPageLabel.Visibility.Should().Be(Visibility.Hidden);
            _cardView.cardPageCountBlock.Text.Should().Be("1");
            _cardView.cardPageIndexBlock.Text.Should().Be("1");
        }

        [DataTestMethod]
        [DataRow("2")]
        [DataRow("-2")]
        public void AdjustPageSize_NoFilter_ShouldReturnPaginated(string pgSize)
        {
            // Arrange
            _cardView.cardsResultsPerPageTxtBox.Text = pgSize;
            var sender = new Button();

            // Act 
            _cardView.CardsResultsPerPageTxtBox_LostFocus(sender, null);

            // Assert
            _cardView.cardsDataGrid.Items.Count.Should().Be(2);
        }

        [TestMethod]
        public void AdjustPageSize_Filtered_ShouldReturnFilteredAndPaginated()
        {
            // Arrange
            _cardView.cardsResultsPerPageTxtBox.Text = "2";
            _cardView.cardSearchBox.Text = "2018";
            var sender = new Button();

            // Act 
            _cardView.CardsResultsPerPageTxtBox_LostFocus(sender, null);

            // Assert
            _cardView.cardsDataGrid.Items.Count.Should().Be(2);
        }

        [TestMethod]
        public void SearchGotFocus_DefaultText_ShouldEmptyTextBox()
        {
            // Arrange
            _cardView.cardSearchBox.Text = "Search...";
                var sender = new Button();

            // Act
            _cardView.CardSearchBox_GotFocus(sender, null);

            // Assert
            _cardView.cardSearchBox.Text.Should().BeEmpty();
        }

        [TestMethod]
        public void SearchGotFocus_UserText_ShouldKeepUserText()
        {
            // Arrange
            var userText = "1243";
            _cardView.cardSearchBox.Text = userText;
            var sender = new Button();

            // Act
            _cardView.CardSearchBox_GotFocus(sender, null);

            // Assert
            _cardView.cardSearchBox.Text.Should().Be(userText);
        }

        [TestMethod]
        public void SearchLostFocus_EmptyBox_ShouldResetToDefault()
        {
            // Arrange
            _cardView.cardSearchBox.Text = "";
            var sender = new Button();

            // Act
            _cardView.CardSearchBox_LostFocus(sender, null);

            // Assert
            _cardView.cardSearchBox.Text.Should().Be("Search...");
        }

        [TestMethod]
        public void SearchLostFocus_UserText_ShouldKeepUserInput()
        {
            // Arrange
            var userText = "1243";
            _cardView.cardSearchBox.Text = userText;
            var sender = new Button();

            // Act
            _cardView.CardSearchBox_LostFocus(sender, null);

            // Assert
            _cardView.cardSearchBox.Text.Should().Be(userText);
        }

        [TestMethod]
        public void UpdatePageSize_RequestIsZero_ShouldReset()
        {
            // Arrange
            _cardView.cardsResultsPerPageTxtBox.Text = "0";
            var cards = _db.Cards.ToList().Take(2);
            var cardVMs = new List<CardCreationViewModel>();
            foreach (var card in cards) cardVMs.Add(_service.MapCardToCreational(card));

            // Act
            _cardView.UpdatePageSize(cardVMs);

            // Assert
            _cardView.cardsDataGrid.Items.Count.Should().Be(2);
            _cardView.cardsDataGrid.Items.Should().BeEquivalentTo(cardVMs);
            _cardView.cardsResultsPerPageTxtBox.Text.Should().Be("All");
            _cardView.cardPerPageLabel.Visibility.Should().Be(Visibility.Hidden);
        }

        [TestMethod]
        public void UpdatePageButtons_PageSizeZero_ShouldRenderSinglePage()
        {
            // Arrange
            var cards = _service.GetAllFiltered("2018");

            // Act
            _cardView.UpdatePageButtons(cards);

            // Assert
            _cardView.cardPageCountBlock.Text.Should().Be("1");
            _cardView.cardNextBtn.Visibility.Should().Be(Visibility.Hidden);
            _cardView.cardNextBtn.IsEnabled.Should().BeFalse();
            _cardView.cardPrevBtn.Visibility.Should().Be(Visibility.Hidden);
            _cardView.cardPrevBtn.IsEnabled.Should().BeFalse();
        }

        [TestMethod]
        public void RenderPerPage_HappyFlow_ShouldDisplayPerPageLabel()
        {
            _cardView.RenderPerPage();

            _cardView.cardPerPageLabel.Visibility.Should().Be(Visibility.Visible);
            
        }

        [TestMethod]
        public void UpdateScroll_GoForwardUnfiltered_ShouldReturnNextPage()
        {
            // Arrange
            _cardView.cardSearchBox.Text = "Search...";
            _cardView.cardPageIndexBlock.Text = "1";
            _cardView.cardsResultsPerPageTxtBox.Text = "1";
            _cardView.cardPageCountBlock.Text = "3";

            var expectedCardsGrid = _service.GetAllPaged(2, 1);

            // Act
            _cardView.UpdatePageScroll(true);

            // Assert
            _cardView.cardsDataGrid.Items.Should().BeEquivalentTo(expectedCardsGrid);
            _cardView.cardNextBtn.IsEnabled.Should().BeTrue();
            _cardView.cardNextBtn.Visibility.Should().Be(Visibility.Visible);
            _cardView.cardPrevBtn.IsEnabled.Should().BeTrue();
            _cardView.cardPrevBtn.Visibility.Should().Be(Visibility.Visible);
        }

        [TestMethod]
        public void UpdateScroll_GoBackUnfiltered_ShouldReturnPrevPage()
        {
            // Arrange
            _cardView.cardSearchBox.Text = "Search...";
            _cardView.cardPageIndexBlock.Text = "3";
            _cardView.cardsResultsPerPageTxtBox.Text = "1";
            _cardView.cardPageCountBlock.Text = "3";

            var expectedCardsGrid = _service.GetAllPaged(2, 1);

            // Act
            _cardView.UpdatePageScroll(false);

            // Assert
            _cardView.cardsDataGrid.Items.Should().BeEquivalentTo(expectedCardsGrid);
            _cardView.cardNextBtn.IsEnabled.Should().BeTrue();
            _cardView.cardNextBtn.Visibility.Should().Be(Visibility.Visible);
            _cardView.cardPrevBtn.IsEnabled.Should().BeTrue();
            _cardView.cardPrevBtn.Visibility.Should().Be(Visibility.Visible);
        }

        [TestMethod]
        public void UpdateScroll_GoBackFiltered_ShouldReturnPrevAndFirstPage()
        {
            // Arrange
            _cardView.cardSearchBox.Text = "2018";
            _cardView.cardPageIndexBlock.Text = "2";
            _cardView.cardsResultsPerPageTxtBox.Text = "1";
            _cardView.cardPageCountBlock.Text = "2";

            var expectedCardsGrid = _service.GetAllPaged(1, 1);

            // Act
            _cardView.UpdatePageScroll(false);

            // Assert
            _cardView.cardsDataGrid.Items.Should().BeEquivalentTo(expectedCardsGrid);
            _cardView.cardNextBtn.IsEnabled.Should().BeTrue();
            _cardView.cardNextBtn.Visibility.Should().Be(Visibility.Visible);
            _cardView.cardPrevBtn.IsEnabled.Should().BeFalse();
            _cardView.cardPrevBtn.Visibility.Should().Be(Visibility.Hidden);
        }

        [TestMethod]
        public void UpdateScroll_GoForwardFiltered_ShouldReturnNextAndLastPage()
        {
            // Arrange
            _cardView.cardSearchBox.Text = "2018";
            _cardView.cardPageIndexBlock.Text = "1";
            _cardView.cardsResultsPerPageTxtBox.Text = "1";
            _cardView.cardPageCountBlock.Text = "2";

            var expectedCardsGrid = _service.GetAllPaged(2, 1);

            // Act
            _cardView.UpdatePageScroll(true);

            // Assert
            _cardView.cardsDataGrid.Items.Should().BeEquivalentTo(expectedCardsGrid);
            _cardView.cardNextBtn.IsEnabled.Should().BeFalse();
            _cardView.cardNextBtn.Visibility.Should().Be(Visibility.Hidden);
            _cardView.cardPrevBtn.IsEnabled.Should().BeTrue();
            _cardView.cardPrevBtn.Visibility.Should().Be(Visibility.Visible);
        }

        [TestMethod]
        public void PerPage_GotFocus_ShouldEmptyBox()
        {
            // Arrange
            var sender = new Button();

            // Act
            _cardView.CardsResultsPerPageTxtBox_GotFocus(sender, null);

            // Assert
            _cardView.cardsResultsPerPageTxtBox.Text.Should().Be("");
            _cardView.cardPerPageLabel.Visibility.Should().Be(Visibility.Hidden);
        }
    }
}
