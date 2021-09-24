using CustomerWPF_Windows;
using Infrastructure;
using Infrastructure.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CustomerWPF_DesktopClient.Views.SubViews
{
    /// <summary>
    /// Interaction logic for CardCreationView.xaml
    /// </summary>
    public partial class CardCreationView : Window
    {
        private ICardService _service;
        private Cards _cardView;
        private Dictionary<string, List<string>> _validationErrors;

        public CardCreationView(ICardService service, Cards cardView)
        {
            StaticLogger.LogInfo(GetType(), "Opened Card Creation Form.");

            _validationErrors = new Dictionary<string, List<string>>
            {
                {"Code", new List<string>() },
                {"UniNum", new List<string>() },
                {"CVV", new List<string>() },
                {"Start", new List<string>() },
                {"End", new List<string>() },
                {"Cust", new List<string>() }
            };
            _cardView = cardView;
            _service = service;
            InitializeComponent();

            cardCreateCodeBox.Focus();
        }

        public void CardCreateSubmitBtn_Click(object sender, RoutedEventArgs e)
        {
            StaticLogger.LogInfo(GetType(), "Card Creation was submitted.");

            int errorCount = _validationErrors["Code"].Count();
            errorCount += _validationErrors["UniNum"].Count();
            errorCount += _validationErrors["CVV"].Count();
            errorCount += _validationErrors["Start"].Count();
            errorCount += _validationErrors["End"].Count();
            errorCount += _validationErrors["Cust"].Count();

            if (errorCount != 0)
            {
                StaticLogger.LogWarn(GetType(), "Card Creation failed due to invalid user data.");
                ShowAllValidationErrors();
                return;
            }

            var ccVM = new CardCreationViewModel
            {
                StartDate = cardCreateStartBox.Text,
                EndDate = cardCreateStopBox.Text,
                CardCode = cardCreateCodeBox.Text,
                CustomerCode = cardCreateCcBox.Text,
                CVVNumber = cardCreateCvvBox.Text,
                UniqueNumber = cardCreateUnBox.Text
            };



            if (_service.VerifyCreational(ccVM))
            {
                _service.Create(_service.MapCreational(ccVM));
                StaticLogger.LogInfo(GetType(), "Card Creation request succesfully submitted.");
                _cardView.GetAll();
                Close();
            }
            else MessageBox.Show("Invalid data");
        }

        private void CardCreateDiscardBtn_Click(object sender, RoutedEventArgs e)
        {
                StaticLogger.LogInfo(GetType(), "Card Creation data discarded by user request.");
            Close();
        }

        private void CardCreateWindow_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.IsRepeat) return;

            var textInputs = new List<Control>
            {
                cardCreateCcBox,
                cardCreateCodeBox,
                cardCreateCvvBox,
                cardCreateUnBox,
                cardCreateStartBox,
                cardCreateStopBox,
            };
            bool isTyping = textInputs.Any(t => t.IsFocused);
            if (!isTyping && e.Key == Key.Back) Close();
            switch (e.Key)
            {
                case Key.Escape:
                    Close();
                    break;
                case Key.Enter:
                    CardCreateSubmitBtn_Click(sender, null);
                    break;
                default:
                    break;
            }
        }

        #region Text Boxes Focus Handlers

        private void CardCreateCodeBox_LostFocus(object sender, RoutedEventArgs e)
        {
            cardCreateCodeBox.Text = cardCreateCodeBox.Text.Trim() == "" ? "Enter Card Code Here" : cardCreateCodeBox.Text;
            cardCreateCodeBox.FontStyle = FontStyles.Italic;
            cardCreateCodeBox.Foreground = Brushes.Gray;
            CardCreateCodeBox_TextChanged(sender, null);
        }

        private void CardCreateUnBox_LostFocus(object sender, RoutedEventArgs e)
        {
            cardCreateUnBox.Text = cardCreateUnBox.Text.Trim() == "" ? "Enter Unique Code Here" : cardCreateUnBox.Text;
            cardCreateUnBox.FontStyle = FontStyles.Italic;
            cardCreateUnBox.Foreground = Brushes.Gray;
            CardCreateUnBox_TextChanged(sender, null);
        }

        private void CardCreateCvvBox_LostFocus(object sender, RoutedEventArgs e)
        {
            cardCreateCvvBox.Text = cardCreateCvvBox.Text.Trim() == "" ? "Enter CVV Number Here" : cardCreateCvvBox.Text;
            cardCreateCvvBox.FontStyle = FontStyles.Italic;
            cardCreateCvvBox.Foreground = Brushes.Gray;
            CardCreateCvvBox_TextChanged(sender, null);
        }

        private void CardCreateStartBox_LostFocus(object sender, RoutedEventArgs e)
        {
            cardCreateStartBox.Text = cardCreateStartBox.Text.Trim() == "" ? "Enter Start Date Here" : cardCreateStartBox.Text;
            cardCreateStartBox.FontStyle = FontStyles.Italic;
            cardCreateStartBox.Foreground = Brushes.Gray;
            CardCreateStartBox_TextChanged(sender, null);
        }

        private void CardCreateStopBox_LostFocus(object sender, RoutedEventArgs e)
        {
            cardCreateStopBox.Text = cardCreateStopBox.Text == "" ? "Enter Stop Date Here" : cardCreateStartBox.Text;
            cardCreateStopBox.FontStyle = FontStyles.Italic;
            cardCreateStopBox.Foreground = Brushes.Gray;
            CardCreateStopBox_TextChanged(sender, null);
        }

        private void CardCreateCcBox_LostFocus(object sender, RoutedEventArgs e)
        {
            cardCreateCcBox.Text = cardCreateCcBox.Text.Trim() == "" ? "Enter Customer Code Here" : cardCreateCcBox.Text;
            cardCreateCcBox.FontStyle = FontStyles.Italic;
            cardCreateCcBox.Foreground = Brushes.Gray;
            CardCreateCcBox_TextChanged(sender, null);
        }

        private void CardCreateCodeBox_GotFocus(object sender, RoutedEventArgs e)
        {
            cardCreateCodeBox.SelectAll();
            cardCreateCodeBox.FontStyle = FontStyles.Normal;
            cardCreateCodeBox.Foreground = Brushes.Black;
            CardCreateCodeBox_TextChanged(sender,null);
        }

        private void CardCreateUnBox_GotFocus(object sender, RoutedEventArgs e)
        {
            cardCreateUnBox.SelectAll();
            cardCreateUnBox.FontStyle = FontStyles.Normal;
            cardCreateUnBox.Foreground = Brushes.Black;
            CardCreateUnBox_TextChanged(sender, null);
        }

        private void CardCreateCvvBox_GotFocus(object sender, RoutedEventArgs e)
        {
            cardCreateCvvBox.SelectAll();
            cardCreateCvvBox.FontStyle = FontStyles.Normal;
            cardCreateCvvBox.Foreground = Brushes.Black;
            CardCreateCvvBox_TextChanged(sender, null);
        }

        private void CardCreateStartBox_GotFocus(object sender, RoutedEventArgs e)
        {
            cardCreateStartBox.SelectAll();
            cardCreateStartBox.FontStyle = FontStyles.Normal;
            cardCreateStartBox.Foreground = Brushes.Black;
            CardCreateStartBox_TextChanged(sender, null);
        }

        private void CardCreateStopBox_GotFocus(object sender, RoutedEventArgs e)
        {
            cardCreateStopBox.SelectAll();
            cardCreateStopBox.FontStyle = FontStyles.Normal;
            cardCreateStopBox.Foreground = Brushes.Black;
            CardCreateStopBox_TextChanged(sender, null);
        }

        private void CardCreateCcBox_GotFocus(object sender, RoutedEventArgs e)
        {
            cardCreateCcBox.SelectAll();
            cardCreateCcBox.FontStyle = FontStyles.Normal;
            cardCreateCcBox.Foreground = Brushes.Black;
            CardCreateCcBox_TextChanged(sender, null);
        }

        #endregion

        #region Expanders

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

        private void UnExpander_Click(object sender, RoutedEventArgs e)
        {
            if (unPop.Visibility == Visibility.Hidden)
            {
                unPop.Visibility = Visibility.Visible;
                unPopArrow.RenderTransform = new ScaleTransform(-1, 1);
                HideUniNumPopError();
            }
            else
            {
                unPop.Visibility = Visibility.Hidden;
                unPopArrow.RenderTransform = new ScaleTransform(1, 1);
            }
        }

        private void CvvExpander_Click(object sender, RoutedEventArgs e)
        {
            if (cvvPop.Visibility == Visibility.Hidden)
            {
                cvvPop.Visibility = Visibility.Visible;
                cvvPopArrow.RenderTransform = new ScaleTransform(-1, 1);
                HideCvvPopError();
            }
            else
            {
                cvvPop.Visibility = Visibility.Hidden;
                cvvPopArrow.RenderTransform = new ScaleTransform(1, 1);
            }
        }

        private void StartExpander_Click(object sender, RoutedEventArgs e)
        {
            if (startPop.Visibility == Visibility.Hidden)
            {
                startPop.Visibility = Visibility.Visible;
                startPopArrow.RenderTransform = new ScaleTransform(-1, 1);
                HideStartPopError();
            }
            else
            {
                startPop.Visibility = Visibility.Hidden;
                startPopArrow.RenderTransform = new ScaleTransform(1, 1);
            }
        }

        private void StopExpander_Click(object sender, RoutedEventArgs e)
        {
            if (stopPop.Visibility == Visibility.Hidden)
            {
                stopPop.Visibility = Visibility.Visible;
                stopPopArrow.RenderTransform = new ScaleTransform(-1, 1);
                HideStopPopError();
            }
            else
            {
                stopPop.Visibility = Visibility.Hidden;
                stopPopArrow.RenderTransform = new ScaleTransform(1, 1);
            }
        }

        private void CcExpander_Click(object sender, RoutedEventArgs e)
        {
            if (ccPop.Visibility == Visibility.Hidden)
            {
                ccPop.Visibility = Visibility.Visible;
                ccPopArrow.RenderTransform = new ScaleTransform(-1, 1);
                HideCcPopError();
            }
            else
            {
                ccPop.Visibility = Visibility.Hidden;
                ccPopArrow.RenderTransform = new ScaleTransform(1, 1);
            }
        }

        #endregion

        #region Validation Buttons

        private void UnValidationBtn_Click(object sender, RoutedEventArgs e)
        {
            string validationSummary = "";

            foreach (string error in _validationErrors["UniNum"]) validationSummary += error + "\n";

            MessageBox.Show(validationSummary, "Validation Report for Unique Number Field", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void CodeValidationBtn_Click(object sender, RoutedEventArgs e)
        {
            string validationSummary = "";

            foreach (string error in _validationErrors["Code"]) validationSummary += error + "\n";

            MessageBox.Show(validationSummary, "Validation Report for Card Code Field", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void CvvValidationBtn_Click(object sender, RoutedEventArgs e)
        {
            string validationSummary = "";

            foreach (string error in _validationErrors["CVV"]) validationSummary += error + "\n";

            MessageBox.Show(validationSummary, "Validation Report for CVV Number Field", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void StartValidationBtn_Click(object sender, RoutedEventArgs e)
        {
            string validationSummary = "";

            foreach (string error in _validationErrors["Start"]) validationSummary += error + "\n";

            MessageBox.Show(validationSummary, "Validation Report for Start End Field", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void StopValidationBtn_Click(object sender, RoutedEventArgs e)
        {
            string validationSummary = "";

            foreach (string error in _validationErrors["End"]) validationSummary += error + "\n";

            MessageBox.Show(validationSummary, "Validation Report for End Date Field", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void CcValidationBtn_Click(object sender, RoutedEventArgs e)
        {
            string validationSummary = "";

            foreach (string error in _validationErrors["Cust"]) validationSummary += error + "\n";

            MessageBox.Show(validationSummary, "Validation Report for Customer Code Field", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void ShowAllValidationErrors()
        {
            string validationSummary = "Card Code Validation Summary:\n\n";

            foreach (string error in _validationErrors["Code"]) validationSummary += "\t" + error + "\n";

            validationSummary += "\n Unique Nuber Validation Summary:\n\n";

            foreach (string error in _validationErrors["UniNum"]) validationSummary += "\t" + error + "\n";

            validationSummary += "\n CVV Number Valdation Summary:\n\n";

            foreach (string error in _validationErrors["CVV"]) validationSummary += "\t" + error + "\n";

            validationSummary += "\n Start Date Valdation Summary:\n\n";

            foreach (string error in _validationErrors["Start"]) validationSummary += "\t" + error + "\n";

            validationSummary += "\n End Date Valdation Summary:\n\n";

            foreach (string error in _validationErrors["End"]) validationSummary += "\t" + error + "\n";

            validationSummary += "\n Customer Code Valdation Summary:\n\n";

            foreach (string error in _validationErrors["Cust"]) validationSummary += "\t" + error + "\n";

            MessageBox.Show(validationSummary, "Validation Report", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        #endregion
       
        #region Unique Code Text Handler

        private void CardCreateUnBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender == null) return;
            var text = cardCreateUnBox.Text.Trim();

            if (text == "Enter Unique Number Here")
            {
                unCheckImg.Visibility = Visibility.Hidden;
                unAlertImg.Visibility = Visibility.Hidden;
                unValidationBtn.IsEnabled = false;
            }
            else
            {
                var validationResult = _service.ValidateUniNum(text);
                if (validationResult.isValid)
                {
                    unCheckImg.Visibility = Visibility.Visible;
                    unAlertImg.Visibility = Visibility.Hidden;
                    unValidationBtn.IsEnabled = false;
                    HideUniNumPopError();
                    _validationErrors["UniNum"] = new List<string>();
                }
                else
                {
                    unCheckImg.Visibility = Visibility.Hidden;
                    unAlertImg.Visibility = Visibility.Visible;
                    unValidationBtn.IsEnabled = true;
                    _validationErrors["UniNum"] = validationResult.errorList;
                    ShowUniNumPopError();
                    HideUniNumExpander();
                }
            }
        }

        private void HideUniNumPopError()
        {

            unErrorPop.Text = "This shouldn't be visible";
            unErrorPop.Visibility = Visibility.Hidden;
        }

        private void ShowUniNumPopError()
        {
            unErrorPop.Text = _validationErrors["UniNum"].FirstOrDefault();
            unErrorPop.Visibility = Visibility.Visible;
        }

        private void HideUniNumExpander()
        {
            unPop.Visibility = Visibility.Hidden;
            unPopArrow.RenderTransform = new ScaleTransform(1, 1);
            CardCreateUnBox_TextChanged(null, null);
        }

        #endregion

        #region Card Code Text Handler

        private void CardCreateCodeBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender == null) return;
            var text = cardCreateCodeBox.Text.Trim();

            if (text == "Enter Card Code Here")
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
            CardCreateCodeBox_TextChanged(null, null);
        }

        #endregion

        #region CVV Text Handler

        private void CardCreateCvvBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender == null) return;
            var text = cardCreateCvvBox.Text.Trim();

            if (text == "Enter CVV Here")
            {
                cvvCheckImg.Visibility = Visibility.Hidden;
                cvvAlertImg.Visibility = Visibility.Hidden;
                cvvValidationBtn.IsEnabled = false;
            }
            else
            {
                var validationResult = _service.ValidateCVV(text);
                if (validationResult.isValid)
                {
                    cvvCheckImg.Visibility = Visibility.Visible;
                    cvvAlertImg.Visibility = Visibility.Hidden;
                    cvvValidationBtn.IsEnabled = false;
                    HideCvvPopError();
                    _validationErrors["CVV"] = new List<string>();
                }
                else
                {
                    cvvCheckImg.Visibility = Visibility.Hidden;
                    cvvAlertImg.Visibility = Visibility.Visible;
                    cvvValidationBtn.IsEnabled = true;
                    _validationErrors["CVV"] = validationResult.errorList;
                    ShowCvvPopError();
                    HideCvvExpander();
                }
            }
        }

        private void HideCvvPopError()
        {
            cvvErrorPop.Text = "This shouldn't be visible";
            cvvErrorPop.Visibility = Visibility.Hidden;
        }

        private void ShowCvvPopError()
        {
            cvvErrorPop.Text = _validationErrors["CVV"].FirstOrDefault();
            cvvErrorPop.Visibility = Visibility.Visible;
        }

        private void HideCvvExpander()
        {
            cvvPop.Visibility = Visibility.Hidden;
            cvvPopArrow.RenderTransform = new ScaleTransform(1, 1);
            CardCreateCvvBox_TextChanged(null, null);
        }

        #endregion

        #region StartDate Text Handler

        private void CardCreateStartBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender == null) return;
            var text = cardCreateStartBox.Text.Trim();

            if (text == "Enter Start Date Here")
            {
                startCheckImg.Visibility = Visibility.Hidden;
                startAlertImg.Visibility = Visibility.Hidden;
                startValidationBtn.IsEnabled = false;
            }
            else
            {
                var validationResult = _service.ValidateStart(text);
                var checkBothDates = UpdateDates(); 
                if (validationResult.isValid  && checkBothDates.isValid)
                {
                    startCheckImg.Visibility = Visibility.Visible;
                    startAlertImg.Visibility = Visibility.Hidden;
                    startValidationBtn.IsEnabled = false;
                    HideStartPopError();
                    _validationErrors["Start"] = new List<string>();


                    var start = DateTime.Parse(text);
                }
                else
                {
                    startCheckImg.Visibility = Visibility.Hidden;
                    startAlertImg.Visibility = Visibility.Visible;
                    startValidationBtn.IsEnabled = true;
                    _validationErrors["Start"] = validationResult.errorList;
                    if(!checkBothDates.isValid) _validationErrors["Start"].AddRange(checkBothDates.errorList);
                    ShowStartPopError();
                    HideStartExpander();
                }
            }
        }

        private void HideStartPopError()
        {
            startErrorPop.Text = "This shouldn't be visible";
            startErrorPop.Visibility = Visibility.Hidden;
        }

        private void ShowStartPopError()
        {
            startErrorPop.Text = _validationErrors["Start"].FirstOrDefault();
            startErrorPop.Visibility = Visibility.Visible;
        }

        private void HideStartExpander()
        {
            startPop.Visibility = Visibility.Hidden;
            startPopArrow.RenderTransform = new ScaleTransform(1, 1);
            CardCreateStartBox_TextChanged(null, null);
        }

        #endregion

        #region EndDate Text Handler

        private void CardCreateStopBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender == null) return;
            var text = cardCreateStopBox.Text.Trim();

            if (text == "Enter End Date Here")
            {
                stopCheckImg.Visibility = Visibility.Hidden;
                stopAlertImg.Visibility = Visibility.Hidden;
                stopValidationBtn.IsEnabled = false;
            }
            else
            {
                var validationResult = _service.ValidateStop(text);
                var checkBoth = UpdateDates();
                if (validationResult.isValid&&checkBoth.isValid)
                {
                    stopCheckImg.Visibility = Visibility.Visible;
                    stopAlertImg.Visibility = Visibility.Hidden;
                    stopValidationBtn.IsEnabled = false;
                    HideStopPopError();
                    _validationErrors["End"] = new List<string>();
                }
                else
                {
                    stopCheckImg.Visibility = Visibility.Hidden;
                    stopAlertImg.Visibility = Visibility.Visible;
                    stopValidationBtn.IsEnabled = true;
                    _validationErrors["End"] = validationResult.errorList;
                    if (!checkBoth.isValid) _validationErrors["End"].AddRange(checkBoth.errorList);
                    ShowStopPopError();
                    HideStopExpander();
                }
            }
        }

        private void HideStopPopError()
        {
            stopErrorPop.Text = "This shouldn't be visible";
            stopErrorPop.Visibility = Visibility.Hidden;
        }

        private void ShowStopPopError()
        {
            stopErrorPop.Text = _validationErrors["End"].FirstOrDefault();
            stopErrorPop.Visibility = Visibility.Visible;
        }

        private void HideStopExpander()
        {
            stopPop.Visibility = Visibility.Hidden;
            stopPopArrow.RenderTransform = new ScaleTransform(1, 1);
            CardCreateStopBox_TextChanged(null, null);
        }

        #endregion

        #region CustomerCode Text Handler

        private void CardCreateCcBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender == null) return;
            var text = cardCreateCcBox.Text.Trim();

            if (text == "Enter Customer Code Here")
            {
                ccCheckImg.Visibility = Visibility.Hidden;
                ccAlertImg.Visibility = Visibility.Hidden;
                ccValidationBtn.IsEnabled = false;
            }
            else
            {
                var validationResult = _service.ValidateCustCode(text);
                if (validationResult.isValid)
                {
                    ccCheckImg.Visibility = Visibility.Visible;
                    ccAlertImg.Visibility = Visibility.Hidden;
                    ccValidationBtn.IsEnabled = false;
                    HideCcPopError();
                    _validationErrors["Cust"] = new List<string>();
                }
                else
                {
                    ccCheckImg.Visibility = Visibility.Hidden;
                    ccAlertImg.Visibility = Visibility.Visible;
                    ccValidationBtn.IsEnabled = true;
                    _validationErrors["Cust"] = validationResult.errorList;
                    ShowCcPopError();
                    HideCcExpander();
                }
            }
        }

        private void HideCcPopError()
        {
            ccErrorPop.Text = "This shouldn't be visible";
            ccErrorPop.Visibility = Visibility.Hidden;
        }

        private void ShowCcPopError()
        {
            ccErrorPop.Text = _validationErrors["Cust"].FirstOrDefault();
            ccErrorPop.Visibility = Visibility.Visible;
        }

        private void HideCcExpander()
        {
            ccPop.Visibility = Visibility.Hidden;
            ccPopArrow.RenderTransform = new ScaleTransform(1, 1);
            CardCreateCcBox_TextChanged(null, null);
        }

        #endregion

        #region Date Helpers

        private (bool isValid, List<string> errorList) UpdateDates()
        {
            if (cardCreateStartBox.Text == "Enter Start Date Here" ||
                cardCreateStopBox.Text == "Enter Stop Date Here" ||
                cardCreateStartBox.Text.Trim() == "" ||
                cardCreateStopBox.Text.Trim() == "")
                return (true, null);
            else
            {
                var start = new DateTime(1600,1,1);
                var stop = new DateTime(1600,1,1);
                var startParses = DateTime.TryParse(cardCreateStartBox.Text, out start);
                var stopParses = DateTime.TryParse(cardCreateStopBox.Text, out stop);

                if (!startParses || !stopParses) return (true, null);
                else
                {
                    var valResult = _service.ValidateDates(start, stop);
                    _validationErrors["Start"].AddRange(valResult.errorList);
                    _validationErrors["End"].AddRange(valResult.errorList);
                    return (valResult.isValid, valResult.errorList);
                }
            }
        }

        private void StartDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if(cardCreateStartBox != null)
            cardCreateStartBox.Text = startDatePicker.SelectedDate.ToString();
        }

        private void StartDatePicker_CalendarOpened(object sender, RoutedEventArgs e)
        {
            if (cardCreateStartBox != null && startCheckImg.IsVisible)
            {
                startDatePicker.SelectedDate = DateTime.Parse(cardCreateStartBox.Text);
            }
        }

        private void EndDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cardCreateStopBox != null)
                cardCreateStopBox.Text = endDatePicker.SelectedDate.ToString();
        }

        private void EndDatePicker_CalendarOpened(object sender, RoutedEventArgs e)
        {
            if (cardCreateStopBox != null && stopCheckImg.IsVisible)
            {
                endDatePicker.SelectedDate = DateTime.Parse(cardCreateStopBox.Text);
            }
        }
        
        #endregion
    }
}
