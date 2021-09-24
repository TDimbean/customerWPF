using Infrastructure;
using CustomerWPF_Windows;
using System.Windows;
using System.Windows.Input;
using Unity;

namespace CustomerWPF_DesktopClient.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
    [Dependency]
        private ICustomerService _custService;
    [Dependency]
        private ICardService _cardService;

        public MainWindow(ICustomerService custService, ICardService cardService)
        {
            StaticLogger.LogInfo(GetType(), "Main Window opened.");

            _custService = custService;
            _cardService = cardService;
            InitializeComponent();
            BtnMainToCustomers.Focus();
        }
        
        private void BtnMainToCards_Click(object sender, RoutedEventArgs e)
        {
            StaticLogger.LogDebug(GetType(), "\"To Cards\" Button pressed.");
            Cards cardsView = new Cards(_cardService);
            cardsView.Show();
            Close();
        }

        private void BtnMainToCustomers_Click(object sender, RoutedEventArgs e)
        {
            StaticLogger.LogDebug(GetType(), "\"To Customers\" Button pressed. ");
            var custWindow = new Customers(_custService);
            custWindow.ShowDialog();
            Close();
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Right:
                case Key.OemPeriod:
                    BtnMainToCards_Click(sender, null);
                    break;
                case Key.Left:
                case Key.OemComma:
                    BtnMainToCustomers_Click(sender, null);
                    break;
                default:
                    break;
            }
        }
    }
}
