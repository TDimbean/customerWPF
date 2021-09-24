using CustomerWPF_DesktopClient.Views.Interfaces;
using CustomerWPF_DesktopClient.Views.SubViews;
using CustomerWPF_Windows;
using DAL;
using Infrastructure;
using Infrastructure.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Unity;

namespace CustomerWPF_DesktopClient.Views
{
    /// <summary>
    /// Interaction logic for Cards.xaml
    /// </summary>
    public partial class Cards : Window, ICardsView
    {
        private ICardService _service;
        public static DataGrid DataGrid;
        private int _pageSize;

        public Cards(ICardService service)
        {
            StaticLogger.LogInfo(GetType(), "Displaying Cards View.");
            _pageSize = 0;
            _service = service;
            InitializeComponent();

            GetAll();

            
            }

        public void GetAll()
        {
            StaticLogger.LogInfo(GetType(), "Fetching all Cards from repo.");
            cardsDataGrid.ItemsSource = _service.GetAll();
            cardSearchBox.Text = "Search...";
            ResetResultsPerPage();
            UpdatePageButtons();
        }

        public void ResetResultsPerPage()
        {
            StaticLogger.LogInfo(GetType(), "Resetting page size to show all.");
            cardPageCountBlock.Text = "1";
            cardPageIndexBlock.Text = "1";
            UpdatePageButtons();
            cardsDataGrid.ItemsSource = _service.GetAll();
            cardPerPageLabel.Visibility = Visibility.Hidden;
            cardsResultsPerPageTxtBox.Text = "All";
        }

        private void CardViewAddBtn_Click(object sender, RoutedEventArgs e)
        {
            StaticLogger.LogInfo(GetType(), "Requesting Card Creation Form.");
            CardCreationView createView = new CardCreationView(_service, this);
            createView.Show();
        }

        private void CardViewUpdateButton_Click(object sender, RoutedEventArgs e)
        {
            StaticLogger.LogInfo(GetType(), "Requesting Card Update Form.");
            CardUpdateView updateView = new CardUpdateView
                (_service, cardsDataGrid.SelectedItem as CardCreationViewModel, this);


            updateView.Show();
        }

        public void CardSearchBtn_Click(object sender, RoutedEventArgs e)
        {
            StaticLogger.LogInfo(GetType(), "Searching for: \t"+ cardSearchBox.Text);
            cardsDataGrid.ItemsSource = _service.GetAllFiltered(cardSearchBox.Text);

            //Reset Pagination
            cardsResultsPerPageTxtBox.Text = "All";
            cardPerPageLabel.Visibility = Visibility.Hidden;
            cardPageCountBlock.Text = "1";
            cardPageIndexBlock.Text = "1";
            _pageSize = 0;
        }

        private void BackToMainBtn_Click(object sender, RoutedEventArgs e)
        {
            StaticLogger.LogInfo(GetType(), "Requested return to Main Window.");
            IUnityContainer container = new UnityContainer();
            container.RegisterType<ICustomerRepository, CustomerRepository>();
            container.RegisterType<ICustomerService, CustomerService>();
            container.RegisterType<ICardRepository, CardRepository>();
            container.RegisterType<ICardService, CardService>();
            container.RegisterType<ICustomersView, Customers>();
            container.RegisterType<ICardsView, Cards>();

            var mainWindow = container.Resolve<MainWindow>();
            mainWindow.Show();
            Close();
        }

        public void CardSearchBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && !e.IsRepeat) CardSearchBtn_Click(sender, null);
        }

        public void CardSearchBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var searchBox = cardSearchBox;
            searchBox.Text = searchBox.Text == "Search..." ? "" : searchBox.Text;
            searchBox.Foreground = Brushes.Black;
        }

        public void CardSearchBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var searchBox = cardSearchBox;
            searchBox.Text = searchBox.Text.Trim() == "" ? "Search..." : searchBox.Text;
            searchBox.Foreground = Brushes.Gray;
        }

        private void CardsToCutomersBtn_Click(object sender, RoutedEventArgs e)
        {
            StaticLogger.LogInfo(GetType(), "Requested switch to Customers View.");
            IUnityContainer container = new UnityContainer();
            container.RegisterType<ICustomerRepository, CustomerRepository>();
            container.RegisterType<ICustomerService, CustomerService>();
            container.RegisterType<ICardRepository, CardRepository>();
            container.RegisterType<ICardService, CardService>();
            container.RegisterType<ICustomersView, Customers>();
            container.RegisterType<ICardsView, Cards>();

            var customers = container.Resolve<Customers>();
            customers.Show();
            Close();
        }

        private void CardsWindow_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.IsRepeat) return;

            if (e.Key == Key.OemComma || e.Key == Key.OemPeriod) CardsToCutomersBtn_Click(sender, null);
            if (e.Key == Key.Escape || e.Key == Key.Home) BackToMainBtn_Click(sender, null);
            if (e.Key == Key.PageDown&&cardNextBtn.IsVisible) CardNextBtn_Click(sender, null);
            if (e.Key == Key.PageUp&&cardPrevBtn.IsVisible) CardPrevBtn_Click(sender, null);
            if (cardSearchBox.IsFocused == false && cardsResultsPerPageTxtBox.IsFocused != true)
                switch (e.Key)
                {
                    case Key.OemComma:
                    case Key.OemPeriod:
                        CardsToCutomersBtn_Click(sender, null);
                        break;
                    case Key.Back:
                        BackToMainBtn_Click(sender, null);
                        break;
                    case Key.Insert:
                    case Key.Add:
                        CardViewAddBtn_Click(sender, null);
                        break;
                    case Key.Multiply:
                        GetAll();
                        break;
                }
        }

        private void CardsResultsPerPageTxtBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && !e.IsRepeat) CardsResultsPerPageTxtBox_LostFocus(sender, null);
        }

        public void CardsResultsPerPageTxtBox_LostFocus(object sender, RoutedEventArgs e)
        {
                
            var cards = cardSearchBox.Text == "Search..." || cardSearchBox.Text.Trim() == "" ?
                    _service.GetAll() : _service.GetAllFiltered(cardSearchBox.Text);

            int requestedPageSize = 0;

            if (int.TryParse(cardsResultsPerPageTxtBox.Text, out requestedPageSize))
            {
                requestedPageSize *= requestedPageSize < 0 ? -1 : 1;

                if (cards.Count() < requestedPageSize) _pageSize = 0;
                else
                {
                    _pageSize = requestedPageSize;
                    cardsResultsPerPageTxtBox.Text = requestedPageSize.ToString();
                    cardNextBtn.Focus();
                }
                UpdatePageSize(cards);

            }
            else if(_pageSize == 0) UpdatePageSize(cards);
            else
            {
                cardsResultsPerPageTxtBox.Text = _pageSize.ToString();
                RenderPerPage();
            }
            cardNextBtn.Focus();
            }

        public void UpdatePageSize(IEnumerable<CardCreationViewModel> cards)
        {
            UpdatePageButtons(cards);

            if (_pageSize == 0)
            {
                cardsResultsPerPageTxtBox.Text = "All";
                cardPerPageLabel.Visibility = Visibility.Hidden;
                cardsDataGrid.ItemsSource = cards;
            }
            else RenderPerPage();
        }

        public void UpdatePageButtons(IEnumerable<CardCreationViewModel> cards)
        {
            cardPageIndexBlock.Text = "1";
            int totalPages = 1;
            if (_pageSize != 0)
            {
                totalPages = _pageSize == 0 ? 1 : cards.Count() / _pageSize;
                totalPages += cards.Count() % _pageSize != 0 ? 1 : 0;
                cardsDataGrid.ItemsSource = _service.GetAllPaged(1, _pageSize);
            }
            cardPageCountBlock.Text = totalPages.ToString();
            UpdatePageButtons();
        }

        public void RenderPerPage()
        {
            cardPerPageLabel.Content = _pageSize > 1 ? "cards per page" : "card per page";
            cardPerPageLabel.Visibility = Visibility.Visible;
        }

        private void CardPrevBtn_Click(object sender, RoutedEventArgs e) 
            => UpdatePageScroll(false);

        public void UpdatePageScroll(bool goingForward)
        {
            var isFiltered = cardSearchBox.Text != "Search..." && cardSearchBox.Text.Trim() != "";
            int currentPage = int.Parse(cardPageIndexBlock.Text);
            int pageSize = int.Parse(cardsResultsPerPageTxtBox.Text);
            int totalPages = int.Parse(cardPageCountBlock.Text);
            
                if ((goingForward&&currentPage < totalPages)||(!goingForward&&currentPage>1))
                {
                    currentPage += goingForward ? 1 : -1;
                    cardPageIndexBlock.Text = currentPage.ToString();
                    cardsDataGrid.ItemsSource = isFiltered ?
                        _service.GetAllFilteredAndPaged(cardSearchBox.Text, currentPage, pageSize) :
                        _service.GetAllPaged(currentPage, pageSize);
                    UpdatePageButtons();
                }
        }

        public void UpdatePageButtons()
        {
            if (cardPageIndexBlock.Text=="1")
            {
                cardPrevBtn.IsEnabled = false;
                cardPrevBtn.Visibility = Visibility.Hidden;
            }
            else
            {
                cardPrevBtn.IsEnabled = true;
                cardPrevBtn.Visibility = Visibility.Visible;
            }

            if (cardPageIndexBlock.Text==cardPageCountBlock.Text)
            {
                cardNextBtn.IsEnabled = false;
                cardNextBtn.Visibility = Visibility.Hidden;
            }
            else
            {
                cardNextBtn.IsEnabled = true;
                cardNextBtn.Visibility = Visibility.Visible;
            }
        }

        private void CardPrevBtn_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && !e.IsRepeat) CardPrevBtn_Click(sender, null);
        }

        private void CardNextBtn_Click(object sender, RoutedEventArgs e)
            => UpdatePageScroll(true);

        private void CardNextBtn_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && !e.IsRepeat) CardNextBtn_Click(sender, null);
        }

        public void CardsResultsPerPageTxtBox_GotFocus(object sender, RoutedEventArgs e)
        {
            cardsResultsPerPageTxtBox.Text = "";
            cardPerPageLabel.Visibility = Visibility.Hidden;
        }

        private void CardSearchBtn_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && !e.IsRepeat) CardSearchBtn_Click(sender, null);
        }

        private void CardsToCutomersBtn_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && !e.IsRepeat) CardsToCutomersBtn_Click(sender, null);
        }

        public void CustomersResetFilters_Click(object sender, RoutedEventArgs e) => GetAll();

        private void CustomersResetFilters_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && !e.IsRepeat) GetAll();
        }
    }
}
