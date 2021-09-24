using CustomerWPF_DesktopClient.Views;
using CustomerWPF_DesktopClient.Views.Interfaces;
using CustomerWPF_Windows;
using DAL;
using System.Windows;
using Unity;

namespace CustomerWPF_DesktopClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            IUnityContainer container = new UnityContainer();
            container.RegisterType<ICustomerRepository, CustomerRepository>();
            container.RegisterType<ICustomerService, CustomerService>();
            container.RegisterType<ICardRepository, CardRepository>();
            container.RegisterType<ICardService, CardService>();
            container.RegisterType<ICustomersView, Customers>();
            container.RegisterType<ICardsView, Cards>();

            var mainWindow = container.Resolve<MainWindow>();
            mainWindow.Show();
        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show("An unhandled exception just occurred: " + e.Exception.Message, "App Exception", MessageBoxButton.OK, MessageBoxImage.Warning);
            e.Handled = true;
        }
    }
}
