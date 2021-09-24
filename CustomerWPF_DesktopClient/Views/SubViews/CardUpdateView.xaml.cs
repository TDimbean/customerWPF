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
    /// Interaction logic for CardUpdateView.xaml
    /// </summary>
    public partial class CardUpdateView : Window
    {
        private Dictionary<string, List<string>> _validationErrors;
        private ICardService _service;
        private string _cardCode;
        private Cards _cardView;
        private CardCreationViewModel _oldCard;

        public CardUpdateView(ICardService service, CardCreationViewModel oldVM, Cards cardView)
        {
            StaticLogger.LogInfo(GetType(), "Opened Card Update Form.");


            _validationErrors = new Dictionary<string, List<string>>
            {
                {"UniNum", new List<string>() },
                {"CVV", new List<string>() },
                {"Start", new List<string>() },
                {"End", new List<string>() },
                {"Cust", new List<string>() }
            };
            _oldCard = oldVM;
            _cardView = cardView;
            _service = service;
            _cardCode = oldVM.CardCode;
            InitializeComponent();
                
            cardUpdCcBox.Text = oldVM.CustomerCode;
            cardUpdCvvBox.Text = oldVM.CVVNumber;
            cardUpdStartBox.Text = oldVM.StartDate.ToString();
            cardUpdStopBox.Text = oldVM.EndDate.ToString();
            cardUpdUnBox.Text = oldVM.UniqueNumber;

            cardUpdUnBox.Focus();
        }

        public void CardUpdSubmitBtn_Click(object sender, RoutedEventArgs e)
        {
            StaticLogger.LogInfo(GetType(), "Card Update was submitted.");

            int errorCount = _validationErrors["UniNum"].Count();
            errorCount += _validationErrors["CVV"].Count();
            errorCount += _validationErrors["Start"].Count();
            errorCount += _validationErrors["End"].Count();
            errorCount += _validationErrors["Cust"].Count();

            if (errorCount != 0)
            {
                StaticLogger.LogWarn(GetType(), "Card Update failed due to invalid user data.");

                ShowAllValidationErrors();
                return;
            }

            var cardVM = new CardUpdateViewModel
            {
                StartDate = cardUpdStartBox.Text,
                EndDate = cardUpdStopBox.Text,
                CustomerCode=cardUpdCcBox.Text,
                CVVNumber=cardUpdCvvBox.Text,
                UniqueNumber=cardUpdUnBox.Text
            };

            if (_cardCode != null && _service.CardCodeExists(_cardCode) && _service.VerifyUpdate(cardVM))
            {
                StaticLogger.LogInfo(GetType(), "Card Update succesfully sent to repo.");
                var card = _service.MapUpdate(cardVM);
                card.CardCode = _cardCode;
                _service.Update(card);
                _cardView.GetAll();

                Close();
            }
            else MessageBox.Show("Invalid Data", "Failed to Update", MessageBoxButton.OK, MessageBoxImage.Error);
                
        }

        private void CardUpdDiscardBtn_Click(object sender, RoutedEventArgs e)
        {
            StaticLogger.LogInfo(GetType(), "Card Update data discarded by user request.");
            Close();
        }

        private void CardUpdWindow_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.IsRepeat) return;

            var textInputs = new List<TextBox>
            {
                cardUpdCvvBox,
                cardUpdUnBox,
                cardUpdStartBox,
                cardUpdStopBox,
                cardUpdCcBox
            };
            bool isTyping = textInputs.Any(t => t.IsFocused);
            if (!isTyping && e.Key == Key.Back) Close();

            if ((Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)) &&
                (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift)) &&
                e.Key == Key.R) CardUpdResetAllBtn_Click(sender, null);

            switch (e.Key)
            {
                case Key.Escape:
                    Close();
                    break;
                case Key.Enter:
                    CardUpdSubmitBtn_Click(sender, null);
                    break;
                default:
                    break;
            }
        }

        #region Text Boxes Focus Handlers

        private void CardUpdUnBox_LostFocus(object sender, RoutedEventArgs e)
        {
            cardUpdUnBox.Text = cardUpdUnBox.Text.Trim() == "" ? "Enter Unique Number Here" : cardUpdUnBox.Text;
            cardUpdUnBox.FontStyle = FontStyles.Italic;
            cardUpdUnBox.Foreground = Brushes.Gray;
            CardUpdUnBox_TextChanged(sender, null);
        }

        private void CardUpdCvvBox_LostFocus(object sender, RoutedEventArgs e)
        {
            cardUpdCvvBox.Text = cardUpdCvvBox.Text.Trim() == "" ? "Enter CVV Number Here" : cardUpdCvvBox.Text;
            cardUpdCvvBox.FontStyle = FontStyles.Italic;
            cardUpdCvvBox.Foreground = Brushes.Gray;
            CardUpdCvvBox_TextChanged(sender, null);
        }

        private void CardUpdStartBox_LostFocus(object sender, RoutedEventArgs e)
        {
            cardUpdStartBox.Text = cardUpdStartBox.Text.Trim() == "" ? "Enter Start Date Here" : cardUpdStartBox.Text;
            cardUpdStartBox.FontStyle = FontStyles.Italic;
            cardUpdStartBox.Foreground = Brushes.Gray;
            CardUpdStartBox_TextChanged(sender, null);
        }

        private void CardUpdStopBox_LostFocus(object sender, RoutedEventArgs e)
        {
            cardUpdStopBox.Text = cardUpdStopBox.Text.Trim() == "" ? "Enter End Date Here" : cardUpdStopBox.Text;
            cardUpdStopBox.FontStyle = FontStyles.Italic;
            cardUpdStopBox.Foreground = Brushes.Gray;
            CardUpdStopBox_TextChanged(sender, null);
        }

        private void CardUpdCcBox_LostFocus(object sender, RoutedEventArgs e)
        {
            cardUpdCcBox.Text = cardUpdCcBox.Text.Trim() == "" ? "Enter Customer Code Here" : cardUpdCcBox.Text;
            cardUpdCcBox.FontStyle = FontStyles.Italic;
            cardUpdCcBox.Foreground = Brushes.Gray;
            CardUpdCcBox_TextChanged(sender, null);
        }

        private void CardUpdUnBox_GotFocus(object sender, RoutedEventArgs e)
        {
            cardUpdUnBox.SelectAll();
            cardUpdUnBox.FontStyle = FontStyles.Normal;
            cardUpdUnBox.Foreground = Brushes.Black;
            CardUpdUnBox_TextChanged(sender, null);
        }

        private void CardUpdCvvBox_GotFocus(object sender, RoutedEventArgs e)
        {
            cardUpdCvvBox.SelectAll();
            cardUpdCvvBox.FontStyle = FontStyles.Normal;
            cardUpdCvvBox.Foreground = Brushes.Black;
            CardUpdStartBox_TextChanged(sender, null);
        }

        private void CardUpdStartBox_GotFocus(object sender, RoutedEventArgs e)
        {
            cardUpdStartBox.SelectAll();
            cardUpdStartBox.FontStyle = FontStyles.Normal;
            cardUpdStartBox.Foreground = Brushes.Black;
            CardUpdStartBox_TextChanged(sender, null);
        }

        private void CardUpdStopBox_GotFocus(object sender, RoutedEventArgs e)
        {
            cardUpdStopBox.SelectAll();
            cardUpdStopBox.FontStyle = FontStyles.Normal;
            cardUpdStopBox.Foreground = Brushes.Black;
            CardUpdCcBox_TextChanged(sender, null);
        }

        private void CardUpdCcBox_GotFocus(object sender, RoutedEventArgs e)
        {
            cardUpdCcBox.SelectAll();
            cardUpdCcBox.FontStyle = FontStyles.Normal;
            cardUpdCcBox.Foreground = Brushes.Black;
            CardUpdCcBox_TextChanged(sender, null);
        }

        #endregion

        #region Expanders

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

        #region Reset Buttons

        private void UnResetBtn_Click(object sender, RoutedEventArgs e)
        {
            cardUpdUnBox.Text = _oldCard.UniqueNumber;
        }

        private void UnResetBtn_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && !e.IsRepeat) UnResetBtn_Click(sender, null); 
        }

        private void CvvResetBtn_Click(object sender, RoutedEventArgs e)
        {
            cardUpdCvvBox.Text = _oldCard.CVVNumber;
        }

        private void CvvResetBtn_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && !e.IsRepeat) CvvResetBtn_Click(sender, null); 
        }

        private void StartResetBtn_Click(object sender, RoutedEventArgs e)
        {
            cardUpdStartBox.Text = _oldCard.StartDate;
        }

        private void StartResetBtn_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && !e.IsRepeat) StartResetBtn_Click(sender, null); 
        }

        private void StopResetBtn_Click(object sender, RoutedEventArgs e)
        {
            cardUpdStopBox.Text = _oldCard.EndDate;
        }

        private void StopResetBtn_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && !e.IsRepeat) StopResetBtn_Click(sender, null); 
        }

        private void CcResetBtn_Click(object sender, RoutedEventArgs e)
        {
            cardUpdCcBox.Text = _oldCard.CustomerCode;
        }

        private void CcResetBtn_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && !e.IsRepeat) CcResetBtn_Click(sender, null); 
        }

        private void CardUpdResetAllBtn_Click(object sender, RoutedEventArgs e)
        {
            CcResetBtn_Click(sender, null);
            UnResetBtn_Click(sender, null);
            StartResetBtn_Click(sender, null);
            StopResetBtn_Click(sender, null);
            CvvResetBtn_Click(sender, null);
        }

        private void CardUpdResetAllBtn_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && !e.IsRepeat) CardUpdResetAllBtn_Click(sender, null); 
        }

        #endregion

        #region Unique Number Text Handler

        private void CardUpdUnBox_TextChanged(object sender, TextChangedEventArgs e)
        {

            {
                if (sender == null) return;
                var text = cardUpdUnBox.Text.Trim();

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
            CardUpdUnBox_TextChanged(null, null);
        }
        
        #endregion

        #region CVV Text Handler

        private void CardUpdCvvBox_TextChanged(object sender, TextChangedEventArgs e)
        {

                if (sender == null) return;
                var text = cardUpdCvvBox.Text.Trim();

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
            CardUpdCvvBox_TextChanged(null, null);
        }

        #endregion

        #region Start Date Text Handler

        private void CardUpdStartBox_TextChanged(object sender, TextChangedEventArgs e)

        {
            if (sender == null) return;
            var text = cardUpdStartBox.Text.Trim();

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
                if (validationResult.isValid && checkBothDates.isValid)
                {
                    startCheckImg.Visibility = Visibility.Visible;
                    startAlertImg.Visibility = Visibility.Hidden;
                    startValidationBtn.IsEnabled = false;
                    HideStartPopError();
                    _validationErrors["Start"] = new List<string>();
                    _validationErrors["End"] = _service.ValidateStop(cardUpdStopBox.Text).errorList;
                }
                else
                {
                    startCheckImg.Visibility = Visibility.Hidden;
                    startAlertImg.Visibility = Visibility.Visible;
                    startValidationBtn.IsEnabled = true;
                    _validationErrors["Start"] = validationResult.errorList;
                    if (!checkBothDates.isValid) _validationErrors["Start"].AddRange(checkBothDates.errorList);
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
            CardUpdStartBox_TextChanged(null, null);
        }

        #endregion

        #region End Date Text Handler

        private void CardUpdStopBox_TextChanged(object sender, TextChangedEventArgs e)

        {
            if (sender == null) return;
            var text = cardUpdStopBox.Text.Trim();

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
                if (validationResult.isValid && checkBoth.isValid)
                {
                    stopCheckImg.Visibility = Visibility.Visible;
                    stopAlertImg.Visibility = Visibility.Hidden;
                    stopValidationBtn.IsEnabled = false;
                    HideStopPopError();
                    _validationErrors["End"] = new List<string>();
                    _validationErrors["Start"] = _service.ValidateStart(cardUpdStartBox.Text).errorList;
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
            CardUpdStopBox_TextChanged(null, null);
        }

        #endregion

        #region CustomerCode Text Handler

        private void CardUpdCcBox_TextChanged(object sender, TextChangedEventArgs e)

        {
            if (sender == null) return;
            var text = cardUpdCcBox.Text.Trim();

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
            CardUpdCcBox_TextChanged(null, null);
        }

        #endregion

        #region Validation Buttons

        private void UnValidationBtn_Click(object sender, RoutedEventArgs e)
        {
            string validationSummary = "";

            foreach (string error in _validationErrors["UniNum"]) validationSummary += error + "\n";

            MessageBox.Show(validationSummary, "Validation Report for Unique Number Field", MessageBoxButton.OK, MessageBoxImage.Error);
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
            string validationSummary = "Unique Nuber Validation Summary:\n\n";

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

        #region Date Helpers

        private void StartDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cardUpdStartBox != null)
                cardUpdStartBox.Text = startDatePicker.SelectedDate.ToString();
        }

        private void StartDatePicker_CalendarOpened(object sender, RoutedEventArgs e)
        {
            if (cardUpdStartBox != null && startCheckImg.IsVisible)
            {
                startDatePicker.SelectedDate = DateTime.Parse(cardUpdStartBox.Text);
            }
        }

        private void EndDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cardUpdStopBox != null)
                cardUpdStopBox.Text = endDatePicker.SelectedDate.ToString();
        }

        private void EndDatePicker_CalendarOpened(object sender, RoutedEventArgs e)
        {
            if (cardUpdStopBox != null && stopCheckImg.IsVisible)
            {
                endDatePicker.SelectedDate = DateTime.Parse(cardUpdStopBox.Text);
            }
        }

        private (bool isValid, List<string> errorList) UpdateDates()
        {
            if (cardUpdStartBox.Text == "Enter Start Date Here" ||
                cardUpdStopBox.Text == "Enter Stop Date Here" ||
                cardUpdStartBox.Text.Trim() == "" ||
                cardUpdStopBox.Text.Trim() == "")
                return (true, null);
            else
            {
                var start = new DateTime(1600, 1, 1);
                var stop = new DateTime(1600, 1, 1);
                var startParses = DateTime.TryParse(cardUpdStartBox.Text, out start);
                var stopParses = DateTime.TryParse(cardUpdStopBox.Text, out stop);

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
        
        #endregion
    }
}
