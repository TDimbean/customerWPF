using CustomerWPF_Windows;
using DAL.Entities;
using Infrastructure;
using Infrastructure.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CustomerWPF_DesktopClient.Views.SubViews
{
    /// <summary>
    /// Interaction logic for CustomerUpdateView.xaml
    /// </summary>
    public partial class CustomerUpdateView : Window
    {
        private ICustomerService _service;
        private string _custCode;
        private Customer _oldCustomer;
        private Customers _custView;
        private Dictionary<string, List<string>> _validationErrors;

        public CustomerUpdateView(ICustomerService service, Customer oldCustomer, Customers custView)
        {
            StaticLogger.LogInfo(GetType(), "Opened Customer Update Form.");


            _custView = custView;
            _service = service;
            _oldCustomer = oldCustomer;
            _custCode = oldCustomer.CustomerCode;


            _validationErrors = new Dictionary<string, List<string>>();
            _validationErrors.Add("Name", new List<string>());
            _validationErrors.Add("CNP", new List<string>());
            _validationErrors.Add("Address", new List<string>());

            InitializeComponent();

            custUpdAdrBox.Text = oldCustomer.Address;
            custUpdNameBox.Text = oldCustomer.Name;
            custUpdCnpBox.Text = oldCustomer.CNP;

            custUpdNameBox.Focus();
        }

        public void CustUpdSubmitBtn_Click(object sender, RoutedEventArgs e)
        {
            StaticLogger.LogInfo(GetType(), "Customer Update was submitted.");


            int errorCount = _validationErrors["Name"].Count();
            errorCount += _validationErrors["CNP"].Count();
            errorCount += _validationErrors["Address"].Count();
            
            if (errorCount != 0)
            {
                StaticLogger.LogWarn(GetType(), "Customer Update failed due to invalid user data.");

                ShowAllValidationErrors();
                return;
            }

            var custUpdVM = new CustomerUpdateViewModel
            {
                Address = custUpdAdrBox.Text,
                CNP = custUpdCnpBox.Text,
                Name = custUpdNameBox.Text
            };

            if(_custCode != null && _service.CustomerCodeExists(_custCode) && _service.VerifyUpdate(custUpdVM))
            {
                StaticLogger.LogInfo(GetType(), "Customer Update succesfully sent to repo.");
                var cust = _service.MapUpdate(custUpdVM);
                cust.CustomerCode = _custCode;
                _service.Update(cust);
                _custView.GetAll();
                Close();
            }
            else MessageBox.Show("Invalid data", "Update Failure", MessageBoxButton.OK ,MessageBoxImage.Error);
        }

        private void CustUpdDiscardBtn_Click(object sender, RoutedEventArgs e)
        {
            StaticLogger.LogInfo(GetType(), "Customer Update data discarded by user request.");

            Close();
        }

        private void CustUpdateWindow_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.IsRepeat) return;

            if (!custUpdAdrBox.IsFocused && !custUpdCnpBox.IsFocused && !custUpdNameBox.IsFocused && e.Key == Key.Back)
                CustUpdDiscardBtn_Click(sender, null);

            if ((Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)) &&
                (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift)) &&
                e.Key == Key.R) CustUpdResetAllBtn_Click(sender, null);

                switch (e.Key)
            {
                case Key.Escape:
                    CustUpdDiscardBtn_Click(sender, null);
                    break;
                case Key.Enter:
                    CustUpdSubmitBtn_Click(sender, null);
                    break;
                default:
                    break;
            }
        }

        #region Text Boxes Focus Handlers

        private void CustUpdNameBox_LostFocus(object sender, RoutedEventArgs e)
        {
            custUpdNameBox.Text = custUpdNameBox.Text.Trim() == "" ? "Enter Name Here" : custUpdNameBox.Text;
            custUpdNameBox.FontStyle = FontStyles.Italic;
            custUpdNameBox.Foreground = Brushes.Gray;
            CustUpdNameBox_TextChanged(sender, null);
        }

        private void CustUpdCnpBox_LostFocus(object sender, RoutedEventArgs e)
        {
            custUpdCnpBox.Text = custUpdCnpBox.Text.Trim() == "" ? "Enter CNP Here" : custUpdCnpBox.Text;
            custUpdCnpBox.FontStyle = FontStyles.Italic;
            custUpdCnpBox.Foreground = Brushes.Gray;
            CustUpdCnpBox_TextChanged(sender, null);
        }

        private void CustUpdAdrBox_LostFocus(object sender, RoutedEventArgs e)
        {
            custUpdAdrBox.Text = custUpdAdrBox.Text.Trim() == "" ? "Enter Address Here" : custUpdAdrBox.Text;
            custUpdAdrBox.FontStyle = FontStyles.Italic;
            custUpdAdrBox.Foreground = Brushes.Gray;
            CustUpdCnpBox_TextChanged(sender, null);
        }

        private void CustUpdCnpBox_GotFocus(object sender, RoutedEventArgs e)
        {
            custUpdCnpBox.SelectAll();
            custUpdCnpBox.FontStyle = FontStyles.Normal;
            custUpdCnpBox.Foreground = Brushes.Black;
        }

        private void CustUpdNameBox_GotFocus(object sender, RoutedEventArgs e)
        {
            custUpdNameBox.SelectAll();
            custUpdNameBox.FontStyle = FontStyles.Normal;
            custUpdNameBox.Foreground = Brushes.Black;
        }

        private void CustUpdAdrBox_GotFocus(object sender, RoutedEventArgs e)
        {
            custUpdAdrBox.SelectAll();
            custUpdAdrBox.FontStyle = FontStyles.Normal;
            custUpdAdrBox.Foreground = Brushes.Black;
        }

        #endregion

        #region Text Boxes Reset Handlers

        private void NameResetBtn_Click(object sender, RoutedEventArgs e)
        {
            custUpdNameBox.Text = _oldCustomer.Name;
            HideNamePopError();
        }

        private void NameResetBtn_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && !e.IsRepeat) NameResetBtn_Click(sender, null);
        }

        private void AdrResetBtn_Click(object sender, RoutedEventArgs e)
        {
            custUpdAdrBox.Text = _oldCustomer.Address;
            HideAdrPopError();
        }

        private void AdrResetBtn_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && !e.IsRepeat) AdrResetBtn_Click(sender, null);
        }

        private void CnpResetBtn_Click(object sender, RoutedEventArgs e)
        {
            custUpdCnpBox.Text = _oldCustomer.CNP;
            HideCnpPopError();
        }

        private void CnpResetBtn_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && !e.IsRepeat) CnpResetBtn_Click(sender, null);
        }

        private void CustUpdResetAllBtn_Click(object sender, RoutedEventArgs e)
        {
            AdrResetBtn_Click(sender, null);
            CnpResetBtn_Click(sender, null);
            NameResetBtn_Click(sender, null);
        }

        private void CustUpdResetAllBtn_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && !e.IsRepeat) CustUpdResetAllBtn_Click(sender, null);
        }

        #endregion

        #region Expanders

        private void NameExpander_Click(object sender, RoutedEventArgs e)
        {
            if (namePop.Visibility == Visibility.Hidden)
            {
                HideNamePopError();
                namePop.Visibility = Visibility.Visible;
                namePopArrow.RenderTransform = new ScaleTransform(-1, 1);
            }
            else HideNameExpander();
        }

        private void CnpExpander_Click(object sender, RoutedEventArgs e)
        {
            if (cnpPop.Visibility == Visibility.Hidden)
            {
                HideCnpPopError();
                cnpPop.Visibility = Visibility.Visible;
                cnpPopArrow.RenderTransform = new ScaleTransform(-1, 1);
            }
            else
            {
                cnpPop.Visibility = Visibility.Hidden;
                cnpPopArrow.RenderTransform = new ScaleTransform(1, 1);
            }
        }

        private void AdrExpander_Click(object sender, RoutedEventArgs e)
        {
            if (adrPop.Visibility == Visibility.Hidden)
            {
                HideAdrPopError();
                adrPop.Visibility = Visibility.Visible;
                adrPopArrow.RenderTransform = new ScaleTransform(-1, 1);
            }
            else
            {
                adrPop.Visibility = Visibility.Hidden;
                adrPopArrow.RenderTransform = new ScaleTransform(1, 1);
            }
        }

        #endregion

        #region Name Text Handler

        private void CustUpdNameBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender == null) return;
            var text = custUpdNameBox.Text.Trim();

            if (text == "Enter Name Here" || text == _oldCustomer.Name)
            {
                nameCheckImg.Visibility = Visibility.Hidden;
                nameAlertImg.Visibility = Visibility.Hidden;
                nameValidationBtn.IsEnabled = false;
            }
            else
            {
                var validationResult = _service.ValidateName(text);
                if (validationResult.isValid)
                {
                    nameCheckImg.Visibility = Visibility.Visible;
                    nameAlertImg.Visibility = Visibility.Hidden;
                    nameValidationBtn.IsEnabled = false;
                    HideNamePopError();
                    _validationErrors["Name"] = new List<string>();
                }
                else
                {
                    nameCheckImg.Visibility = Visibility.Hidden;
                    nameAlertImg.Visibility = Visibility.Visible;
                    nameValidationBtn.IsEnabled = true;
                    _validationErrors["Name"] = validationResult.errorList;
                    ShowNamePopError();
                    HideNameExpander();
                }
            }
        }

        private void ShowNamePopError()
        {
            nameErrorPop.Text = _validationErrors["Name"].FirstOrDefault();
            nameErrorPop.Visibility = Visibility.Visible;
        }

        private void HideNameExpander()
        {
            namePop.Visibility = Visibility.Hidden;
            namePopArrow.RenderTransform = new ScaleTransform(1, 1);
            CustUpdNameBox_TextChanged(null, null);
        }

        private void HideNamePopError()
        {

            nameErrorPop.Text = "This shouldn't be visible";
            nameErrorPop.Visibility = Visibility.Hidden;
        }

        #endregion

        #region CNP Text Handler

        private void CustUpdCnpBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender == null) return;
            
            var text = custUpdCnpBox.Text.Trim();

            if (text == "Enter CNP Here" || text == _oldCustomer.CNP)
            {
                cnpCheckImg.Visibility = Visibility.Hidden;
                cnpAlertImg.Visibility = Visibility.Hidden;
                cnpValidationBtn.IsEnabled = false;
            }
            else
            {
                var validationResult = _service.ValidateCNP(text);
                if (validationResult.isValid)
                {
                    cnpCheckImg.Visibility = Visibility.Visible;
                    cnpAlertImg.Visibility = Visibility.Hidden;
                    cnpValidationBtn.IsEnabled = false;
                    HideCnpPopError();
                    _validationErrors["CNP"] = new List<string>();
                }
                else
                {
                    cnpCheckImg.Visibility = Visibility.Hidden;
                    cnpAlertImg.Visibility = Visibility.Visible;
                    cnpValidationBtn.IsEnabled = true;
                    _validationErrors["CNP"] = validationResult.errorList;
                    ShowCnpPopError();
                    HideCnpExpander();
                }
            }
        }

        private void ShowCnpPopError()
        {
            cnpErrorPop.Text = _validationErrors["CNP"].FirstOrDefault();
            cnpErrorPop.Visibility = Visibility.Visible;
        }

        private void HideCnpExpander()
        {
            cnpPop.Visibility = Visibility.Hidden;
            cnpPopArrow.RenderTransform = new ScaleTransform(1, 1);
            CustUpdCnpBox_TextChanged(null, null);
        }

        private void HideCnpPopError()
        {

            cnpErrorPop.Text = "This shouldn't be visible";
            cnpErrorPop.Visibility = Visibility.Hidden;
        }

        #endregion

        #region Address Text Handler

        private void CustUpdAdrBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender == null) return;
            var text = custUpdAdrBox.Text.Trim();

            if (text == "Enter Address Here" || text == _oldCustomer.Address)
            {
                adrCheckImg.Visibility = Visibility.Hidden;
                adrAlertImg.Visibility = Visibility.Hidden;
                adrValidationBtn.IsEnabled = false;
            }
            else
            {
                var validationResult = _service.ValidateAddress(text);
                if (validationResult.isValid)
                {
                    adrCheckImg.Visibility = Visibility.Visible;
                    adrAlertImg.Visibility = Visibility.Hidden;
                    adrValidationBtn.IsEnabled = false;
                    HideAdrPopError();
                    _validationErrors["Address"] = new List<string>();
                }
                else
                {
                    adrCheckImg.Visibility = Visibility.Hidden;
                    adrAlertImg.Visibility = Visibility.Visible;
                    adrValidationBtn.IsEnabled = true;
                    _validationErrors["Address"] = validationResult.errorList;
                    ShowAdrPopError();
                    HideAdrExpander();
                }
            }
        }

        private void ShowAdrPopError()
        {
            adrErrorPop.Text = _validationErrors["Address"].FirstOrDefault();
            adrErrorPop.Visibility = Visibility.Visible;
        }

        private void HideAdrExpander()
        {
            adrPop.Visibility = Visibility.Hidden;
            adrPopArrow.RenderTransform = new ScaleTransform(1, 1);
            CustUpdAdrBox_TextChanged(null, null);
        }

        private void HideAdrPopError()
        {

            adrErrorPop.Text = "This shouldn't be visible";
            adrErrorPop.Visibility = Visibility.Hidden;
        }

        #endregion

        #region Validation Buttons

        private void NameValidationBtn_Click(object sender, RoutedEventArgs e)
        {
            string validationSummary = "";

            foreach (string error in _validationErrors["Name"]) validationSummary += error + "\n";

            MessageBox.Show(validationSummary, "Validation Report for Name Field", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void CnpValidationBtn_Click(object sender, RoutedEventArgs e)
        {
            string validationSummary = "";

            foreach (string error in _validationErrors["CNP"]) validationSummary += error + "\n";

            MessageBox.Show(validationSummary, "Validation Report for CNP Field", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void AdrValidationBtn_Click(object sender, RoutedEventArgs e)
        {
            string validationSummary = "";

            foreach (string error in _validationErrors["Address"]) validationSummary += error + "\n";

            MessageBox.Show(validationSummary, "Validation Report for Address Field", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        
        #endregion

        private void ShowAllValidationErrors()
        {
            string validationSummary = "Name Field Validation Summary:\n\n";

            foreach (string error in _validationErrors["Name"]) validationSummary += "\t" + error + "\n";

            validationSummary += "\n CNP Field Valdation Summary:\n\n";

            foreach (string error in _validationErrors["CNP"]) validationSummary += "\t" + error + "\n";

            validationSummary += "\n Address Field Valdation Summary:\n\n";

            foreach (string error in _validationErrors["Address"]) validationSummary += "\t" + error + "\n";

            MessageBox.Show(validationSummary, "Validation Report", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
