using CustomerWPF_DesktopClient.Views.Interfaces;
using CustomerWPF_DesktopClient.Views.SubViews;
using CustomerWPF_Windows;
using DAL;
using DAL.Entities;
using Infrastructure;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Unity;

namespace CustomerWPF_DesktopClient.Views
{
    /// <summary>
    /// Interaction logic for Customers.xaml
    /// </summary>
    public partial class Customers : Window, ICustomersView
    {
        [Dependency]
        private ICustomerService _service;
        public int _pageSize;

        public Customers(ICustomerService service)
        {
            StaticLogger.LogInfo(GetType(), "Displaying Customers View.");

            _pageSize = 0;
            _service = service;
            InitializeComponent();

            GetAll();
        }

        private void CustomersWindow_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.IsRepeat) return;

            if (e.Key == Key.OemComma || e.Key == Key.OemPeriod) CustomerToCardsBtn_Click(sender, null);
            if (e.Key == Key.Escape || e.Key == Key.Home) BackToMainBtn_Click(sender, null);
            if (e.Key == Key.PageDown && custNextBtn.IsVisible) CustNextBtn_Click(sender, null);
            if (e.Key == Key.PageUp && custPrevBtn.IsVisible) CustPrevBtn_Click(sender, null);
            if (!customerSearchBox.IsFocused && !custViewResultsPerPageTxtBox.IsFocused)
                switch (e.Key)
                {
                    //case Key.Left:
                    //case Key.Right:
                    //    CustomerToCardsBtn_Click(sender, null);
                    //    break;
                    case Key.Back:
                        BackToMainBtn_Click(sender, null);
                        break;
                    case Key.Insert:
                    case Key.Add:
                        CustViewAddButton_Click(sender, null);
                        break;
                    case Key.Multiply:
                        GetAll();
                        break;
                }
        }


        public void GetAll()
        {
            StaticLogger.LogInfo(GetType(), "Fetching all Customers from repo.");

            customersDataGrid.ItemsSource = _service.GetAll();
                customerSearchBox.Text = "Search...";
                ResetResultsPerPage();
                UpdatePageButtons();
        }

        private void CustViewAddButton_Click(object sender, RoutedEventArgs e)
        {
            StaticLogger.LogInfo(GetType(), "Requesting Customer Creation Form.");

            CustomerCreationView createView = new CustomerCreationView(_service, this);
            createView.ShowDialog();
        }

        private void CustViewUpdateButton_Click(object sender, RoutedEventArgs e)
        {
            StaticLogger.LogInfo(GetType(), "Requesting Card Update Form.");

            var customer = customersDataGrid.SelectedItem as Customer;
            CustomerUpdateView updateView = new CustomerUpdateView(_service, customer, this);
            updateView.ShowDialog();
        }

        public void CustomerSearchBtn_Click(object sender, RoutedEventArgs e)
        {
            StaticLogger.LogInfo(GetType(), "Searching for: \t" + customerSearchBox.Text);

            customersDataGrid.ItemsSource = _service.GetAllFiltered(customerSearchBox.Text);

            custViewResultsPerPageTxtBox.Text = "All";
            custPerPageLabel.Visibility = Visibility.Hidden;
            custPageIndexTxtBlock.Text = "1";
            custPageCountTxtBlock.Text = "1";
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

        private void CustomerSearchBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && !e.IsRepeat) CustomerSearchBtn_Click(sender, null);
        }

        public void CustomerSearchBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var searchBox = customerSearchBox;
            searchBox.Text = searchBox.Text == "Search..." ? "" : searchBox.Text;
            searchBox.Foreground = Brushes.Black;
        }

        public void CustomerSearchBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var searchBox = customerSearchBox;
            searchBox.Text = searchBox.Text.Trim() == "" ? "Search..." : searchBox.Text;
            searchBox.Foreground = Brushes.Gray;
        }

        private void CustomerToCardsBtn_Click(object sender, RoutedEventArgs e)
        {
            StaticLogger.LogInfo(GetType(), "Requested switch to Cards View.");

            IUnityContainer container = new UnityContainer();
            container.RegisterType<ICustomerRepository, CustomerRepository>();
            container.RegisterType<ICustomerService, CustomerService>();
            container.RegisterType<ICardRepository, CardRepository>();
            container.RegisterType<ICardService, CardService>();
            container.RegisterType<ICustomersView, Customers>();
            container.RegisterType<ICardsView, Cards>();

            var cardsWindow = container.Resolve<Cards>();
            cardsWindow.Show();
            Close();
        }

        private void CustViewResultsPerPageTxtBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && !e.IsRepeat) CustViewResultsPerPageTxtBox_LostFocus(sender, null);
        }

        public void ResetResultsPerPage()
        {
            StaticLogger.LogInfo(GetType(), "Resetting page size to show all.");

            custPageCountTxtBlock.Text = "1";
            custPageIndexTxtBlock.Text = "1";
            UpdatePageButtons();
            customersDataGrid.ItemsSource = _service.GetAll();
            custPerPageLabel.Visibility = Visibility.Hidden;
            custViewResultsPerPageTxtBox.Text = "All";
        }

        public void CustViewResultsPerPageTxtBox_GotFocus(object sender, RoutedEventArgs e)
        {
            custViewResultsPerPageTxtBox.Text = "";
            custPerPageLabel.Visibility = Visibility.Hidden;
        }

        private void CustomerToCardsBtn_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && !e.IsRepeat) CustomerToCardsBtn_Click(sender, null);
        }

        public void CustViewResultsPerPageTxtBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var customers = customerSearchBox.Text == "Search..." || customerSearchBox.Text.Trim() == "" ?
                _service.GetAll() : _service.GetAllFiltered(customerSearchBox.Text);

            int requestedPageSize = 0;

            if (int.TryParse(custViewResultsPerPageTxtBox.Text, out requestedPageSize))
            {
                requestedPageSize *= requestedPageSize < 0 ? -1 : 1;
                if (customers.Count() < requestedPageSize) _pageSize = 0;
                else
                {
                    _pageSize = requestedPageSize;
                    custViewResultsPerPageTxtBox.Text = requestedPageSize.ToString();
                    backToMainBtn.Focus();
                }
                UpdatePageSize(customers);
            }
            else if (_pageSize == 0) UpdatePageSize(customers);
            else
            {
                custViewResultsPerPageTxtBox.Text = _pageSize.ToString();
                RenderPerPage();
            }
            customerSearchBtn.Focus();
        }

        public void RenderPerPage()
        {
            custPerPageLabel.Content = _pageSize > 1 ? "customers per page" : "customer per page";
            custPerPageLabel.Visibility = Visibility.Visible;
        }

        public void UpdatePageSize(IEnumerable<Customer> customers)
        {
            UpdatePageButtons(customers);

            if (_pageSize == 0)
            {
                custViewResultsPerPageTxtBox.Text = "All";
                custPerPageLabel.Visibility = Visibility.Hidden;
                customersDataGrid.ItemsSource = customers;
            }
            else RenderPerPage();
        }

        public void UpdatePageButtons(IEnumerable<Customer> customers)
        {
            custPageIndexTxtBlock.Text = "1";
            int totalPages = 1;
            if (_pageSize != 0)
            {
                totalPages = _pageSize == 0 ? 1 : customers.Count() / _pageSize;
                totalPages += customers.Count() % _pageSize != 0 ? 1 : 0;
                customersDataGrid.ItemsSource = _service.GetAllPaged(1, _pageSize);
            }
            custPageCountTxtBlock.Text = totalPages.ToString();
            UpdatePageButtons();
        }

        public void UpdatePageButtons()
        {
            if (custPageIndexTxtBlock.Text == "1")
            {
                custPrevBtn.IsEnabled = false;
                custPrevBtn.Visibility = Visibility.Hidden;
            }
            else
            {
                custPrevBtn.IsEnabled = true;
                custPrevBtn.Visibility = Visibility.Visible;
            }
            if (custPageCountTxtBlock.Text == custPageIndexTxtBlock.Text)
            {
                custNextBtn.IsEnabled = false;
                custNextBtn.Visibility = Visibility.Hidden;
            }
            else
            {
                custNextBtn.IsEnabled = true;
                custNextBtn.Visibility = Visibility.Visible;
            }
        }

        private void CustomerSearchBtn_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && !e.IsRepeat) CustomerSearchBtn_Click(sender, null);
        }

        private void CustPrevBtn_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && !e.IsRepeat)
            {
                CustPrevBtn_Click(sender, null);
            }
        }

        private void CustNextBtn_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && !e.IsRepeat)
            {
                CustNextBtn_Click(sender, null);
            }
        }

        private void CustNextBtn_Click(object sender, RoutedEventArgs e)
        {
            UpdatePageScroll(true);
        }

        private void CustPrevBtn_Click(object sender, RoutedEventArgs e)
        {
            UpdatePageScroll(false);
        }

        public void UpdatePageScroll(bool goingForward)
        {
            var isFiltered = customerSearchBox.Text != "Search..." && customerSearchBox.Text.Trim() != "";
            int currentPage = int.Parse(custPageIndexTxtBlock.Text);
            int pageSize = int.Parse(custViewResultsPerPageTxtBox.Text);
            int totalPages = int.Parse(custPageCountTxtBlock.Text);
            if ((goingForward && currentPage < totalPages) || (!goingForward && currentPage > 1))
            {
                currentPage += goingForward ? 1 : -1;
                custPageIndexTxtBlock.Text = currentPage.ToString();
                customersDataGrid.ItemsSource = isFiltered ?
                    _service.GetAllFilteredAndPaged(customerSearchBox.Text, currentPage, pageSize) :
                    _service.GetAllPaged(currentPage, pageSize);
                UpdatePageButtons();
            }
        }

        private void CardsResetBtn_Click(object sender, RoutedEventArgs e) => GetAll();

        private void CardsResetBtn_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && !e.IsRepeat) GetAll();
        }

    }
}