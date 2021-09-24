using CustomerWPF_Windows;
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
    /// Interaction logic for CustomerCreationView.xaml
    /// </summary>
    public partial class CustomerCreationView : Window
    {
        private readonly ICustomerService _service;
        private readonly Customers _custView;
        private Dictionary<string, List<string>> _validationErrors;

        public CustomerCreationView(ICustomerService service, Customers custView)
        {
            StaticLogger.LogInfo(GetType(), "Opened Customer Creation Form.");


            _validationErrors = new Dictionary<string, List<string>>
                {
                { "Code", new List<string>() },
                { "Name", new List<string>() },
                { "CNP", new List<string>() },
                { "Address", new List<string>() }
            };

            _custView = custView;
            _service = service;
            InitializeComponent();

            custCreateCodeBox.Focus();
        }

        public void CustCreateSubmitBtn_Click(object sender, RoutedEventArgs e)
        {
            StaticLogger.LogInfo(GetType(), "Customer Creation was submitted.");

            int errorCount = _validationErrors["Code"].Count();
                errorCount += _validationErrors["Name"].Count();
            errorCount += _validationErrors["CNP"].Count();
            errorCount += _validationErrors["Address"].Count();

            if (errorCount != 0)
            {
                StaticLogger.LogWarn(GetType(), "Customer Creation failed due to invalid user data.");

                ShowAllValidationErrors();
                return;
            }

            var custCreationalVM = new CustomerCreationViewModel()
            {
                CustomerCode = custCreateCodeBox.Text,
                Name = custCreateNameBox.Text,
                CNP = custCreateCnpBox.Text,
                Address = custCreateAdrBox.Text
            };

            if (_service.VerifyCreational(custCreationalVM))
            {
                StaticLogger.LogWarn(GetType(), "Customer Creation Succesfully sent to repo.");
                _service.Create(_service.MapCreational(custCreationalVM));
                _custView.GetAll();
                Close();
            }
            else MessageBox.Show("Invalid Data", "Failed Creation", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void CustCreateDiscardBtn_Click(object sender, RoutedEventArgs e)
        {
            StaticLogger.LogInfo(GetType(), "Customer Creation data discarded by user request.");

            Close();
        }

        private void CustCreateWindow_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.IsRepeat) return;

            var textInputs = new List<TextBox>
            {
                custCreateAdrBox,
                custCreateNameBox,
                custCreateCnpBox,
                custCreateCodeBox
            };
            bool isTyping = textInputs.Any(t => t.IsFocused);
            if (!isTyping && e.Key == Key.Back) Close();

            switch (e.Key)
            {
                case Key.Escape:
                    Close();
                    break;
                case Key.Enter:
                    CustCreateSubmitBtn_Click(sender, null);
                    break;
                default:
                    break;
            }
        }

        #region Text Boxes Focus Handler

        private void CustCreateCodeBox_LostFocus(object sender, RoutedEventArgs e)
        {
            custCreateCodeBox.Text = custCreateCodeBox.Text.Trim() == "" ? "Enter Customer Code Here" : custCreateCodeBox.Text;
            custCreateCodeBox.FontStyle = FontStyles.Italic;
            custCreateCodeBox.Foreground = Brushes.Gray;
            CustCreateCodeBox_TextChanged(sender, null);
        }

        private void CustCreateNameBox_LostFocus(object sender, RoutedEventArgs e)
        {
            custCreateNameBox.Text = custCreateNameBox.Text.Trim() == "" ? "Enter Customer Code Here" : custCreateNameBox.Text;
            custCreateNameBox.FontStyle = FontStyles.Italic;
            custCreateNameBox.Foreground = Brushes.Gray;
            CustCreateNameBox_TextChanged(sender, null);
        }

        private void CustCreateCnpBox_LostFocus(object sender, RoutedEventArgs e)
        {
            custCreateCnpBox.Text = custCreateCnpBox.Text.Trim() == "" ? "Enter Customer Code Here" : custCreateCnpBox.Text;
            custCreateCnpBox.FontStyle = FontStyles.Italic;
            custCreateCnpBox.Foreground = Brushes.Gray;
            CustCreateCnpBox_TextChanged(sender, null);
        }

        private void CustCreateAdrBox_LostFocus(object sender, RoutedEventArgs e)
        {
            custCreateAdrBox.Text = custCreateAdrBox.Text.Trim() == "" ? "Enter Customer Code Here" : custCreateAdrBox.Text;
            custCreateAdrBox.FontStyle = FontStyles.Italic;
            custCreateAdrBox.Foreground = Brushes.Gray;
            CustCreateAdrBox_TextChanged(sender, null);
        }

        private void CustCreateCodeBox_GotFocus(object sender, RoutedEventArgs e)
        {
            custCreateCodeBox.SelectAll();
            custCreateCodeBox.FontStyle = FontStyles.Normal;
            custCreateCodeBox.Foreground = Brushes.Black;
            CustCreateCodeBox_TextChanged(sender, null);
        }

        private void CustCreateNameBox_GotFocus(object sender, RoutedEventArgs e)
        {
            custCreateNameBox.SelectAll();
            custCreateNameBox.FontStyle = FontStyles.Normal;
            custCreateNameBox.Foreground = Brushes.Black;
            CustCreateNameBox_TextChanged(sender, null);
        }

        private void CustCreateCnpBox_GotFocus(object sender, RoutedEventArgs e)
        {
            custCreateCnpBox.SelectAll();
            custCreateCnpBox.FontStyle = FontStyles.Normal;
            custCreateCnpBox.Foreground = Brushes.Black;
            CustCreateCnpBox_TextChanged(sender, null);
        }

        private void CustCreateAdrBox_GotFocus(object sender, RoutedEventArgs e)
        {
            custCreateAdrBox.SelectAll();
            custCreateAdrBox.FontStyle = FontStyles.Normal;
            custCreateAdrBox.Foreground = Brushes.Black;
            CustCreateAdrBox_TextChanged(sender, null);
        }

        #endregion  
         
        #region  Expanders

        private void CodeExpander_Click(object sender, RoutedEventArgs e)
        {
            if (codePop.Visibility == Visibility.Hidden)
            {
                codePop.Visibility = Visibility.Visible;
                codePopArrow.RenderTransform = new ScaleTransform(-1, 1);
                HideCodePopError();
            }
            else
            {
                codePop.Visibility = Visibility.Hidden;
                codePopArrow.RenderTransform = new ScaleTransform(1, 1);
            }
        }

        private void NameExpander_Click(object sender, RoutedEventArgs e)
        {
            if (namePop.Visibility == Visibility.Hidden)
            {
                namePop.Visibility = Visibility.Visible;
                namePopArrow.RenderTransform = new ScaleTransform(-1, 1);
                HideNamePopError();
            }
            else
            {
                namePop.Visibility = Visibility.Hidden;
                namePopArrow.RenderTransform = new ScaleTransform(1, 1);
            }
        }

        private void CnpExpander_Click(object sender, RoutedEventArgs e)
        {
            if (cnpPop.Visibility == Visibility.Hidden)
            {
                cnpPop.Visibility = Visibility.Visible;
                cnpPopArrow.RenderTransform = new ScaleTransform(-1, 1);
                HideCnpPopError();
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
                adrPop.Visibility = Visibility.Visible;
                adrPopArrow.RenderTransform = new ScaleTransform(-1, 1);
                HideAdrPopError();
            }
            else
            {
                adrPop.Visibility = Visibility.Hidden;
                adrPopArrow.RenderTransform = new ScaleTransform(1, 1);
            }
        }

        #endregion

        #region Validation Buttons

        private void CodeValidationBtn_Click(object sender, RoutedEventArgs e)
        {
            string validationSummary = "";

            foreach (string error in _validationErrors["Code"]) validationSummary += error + "\n";

            MessageBox.Show(validationSummary, "Validation Report for Customer Code Field", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void AdrValidationBtn_Click(object sender, RoutedEventArgs e)
        {
            string validationSummary = "";

            foreach (string error in _validationErrors["Address"]) validationSummary += error + "\n";

            MessageBox.Show(validationSummary, "Validation Report for Address Field", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void CnpValidationBtn_Click(object sender, RoutedEventArgs e)
        {
            string validationSummary = "";

            foreach (string error in _validationErrors["CNP"]) validationSummary += error + "\n";

            MessageBox.Show(validationSummary, "Validation Report for CNP Field", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void NameValidationBtn_Click(object sender, RoutedEventArgs e)
        {
            string validationSummary = "";

            foreach (string error in _validationErrors["Name"]) validationSummary += error + "\n";

            MessageBox.Show(validationSummary, "Validation Report for Name Field", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void ShowAllValidationErrors()
        {
            string validationSummary = "Customer Code Validation Summary:\n\n";

            foreach (string error in _validationErrors["Code"]) validationSummary += "\t" + error + "\n";

            validationSummary += "\n Name Field Validation Summary:\n\n";

            foreach (string error in _validationErrors["Name"]) validationSummary += "\t" + error + "\n";

            validationSummary += "\n CNP Field Valdation Summary:\n\n";

            foreach (string error in _validationErrors["CNP"]) validationSummary += "\t" + error + "\n";

            validationSummary += "\n Address Field Valdation Summary:\n\n";

            foreach (string error in _validationErrors["Address"]) validationSummary += "\t" + error + "\n";

            MessageBox.Show(validationSummary, "Validation Report", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        #endregion

        #region CustomerCode Text Handler

        private void CustCreateCodeBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender == null) return;
            var text = custCreateCodeBox.Text.Trim();

            if (text == "Enter Customer Code Here")
            {
                codeCheckImg.Visibility = Visibility.Hidden;
                codeAlertImg.Visibility = Visibility.Hidden;
                codeValidationBtn.IsEnabled = false;
            }
            else
            {
                var validationResult = _service.ValidateCode(text);
                if (validationResult.isValid)
                {
                    codeCheckImg.Visibility = Visibility.Visible;
                    codeAlertImg.Visibility = Visibility.Hidden;
                    codeValidationBtn.IsEnabled = false;
                    HideCodePopError();
                    _validationErrors["Code"] = new List<string>();
                }
                else
                {
                    codeCheckImg.Visibility = Visibility.Hidden;
                    codeAlertImg.Visibility = Visibility.Visible;
                    codeValidationBtn.IsEnabled = true;
                    _validationErrors["Code"] = validationResult.errorList;
                    ShowCodePopError();
                    HideCodeExpander();
                }
            }
        }

        private void HideCodePopError()
        {

            codeErrorPop.Text = "This shouldn't be visible";
            codeErrorPop.Visibility = Visibility.Hidden;
        }

        private void ShowCodePopError()
        {
            codeErrorPop.Text = _validationErrors["Code"].FirstOrDefault();
            codeErrorPop.Visibility = Visibility.Visible;
        }

        private void HideCodeExpander()
        {
            codePop.Visibility = Visibility.Hidden;
            codePopArrow.RenderTransform = new ScaleTransform(1, 1);
            CustCreateCodeBox_TextChanged(null, null);
        }

        #endregion

        #region Name Text Handler

        private void CustCreateNameBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender == null) return;
            var text = custCreateNameBox.Text.Trim();

            if (text == "Enter Name Here")
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

        private void HideNamePopError()
        {
            nameErrorPop.Text = "This shouldn't be visible";
            nameErrorPop.Visibility = Visibility.Hidden;
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
            CustCreateNameBox_TextChanged(null, null);
        }

        #endregion

        #region CNP Text Handler

        private void CustCreateCnpBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender == null) return;
            var text = custCreateCnpBox.Text.Trim();

            if (text == "Enter CNP Here")
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

        private void HideCnpPopError()
        {
            cnpErrorPop.Text = "This shouldn't be visible";
            cnpErrorPop.Visibility = Visibility.Hidden;
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
            CustCreateCnpBox_TextChanged(null, null);
        }

        #endregion

        #region Address Text Handler

        private void CustCreateAdrBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender == null) return;
            var text = custCreateAdrBox.Text.Trim();

            if (text == "Enter Address Here")
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

        private void HideAdrPopError()
        {
            adrErrorPop.Text = "This shouldn't be visible";
            adrErrorPop.Visibility = Visibility.Hidden;
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
            CustCreateAdrBox_TextChanged(null, null);
        }

        #endregion
    }
}
