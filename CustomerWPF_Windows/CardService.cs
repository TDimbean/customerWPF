using DAL;
using DAL.Entities;
using Infrastructure;
using Infrastructure.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity;

namespace CustomerWPF_Windows
{
    public class CardService : ICardService
    {
        [Dependency]
        private ICardRepository _repo;

        public CardService(ICardRepository repo)
        {
            _repo = repo;
        }

        public IEnumerable<CardCreationViewModel> GetAll()
        {
            StaticLogger.LogInfo(GetType(), "Service fetching and converting all Cards from Repo.");
            var cardVMs = new List<CardCreationViewModel>();
            foreach (var card in _repo.GetAll().ToList()) cardVMs.Add(MapCardToCreational(card));
            return cardVMs;
        }

        public IEnumerable<CardCreationViewModel> GetAllPaged(int pageIndex, int pageSize)
        {
            StaticLogger.LogInfo(GetType(), "Service fetching page " + pageIndex + " of Cards from Repo. (Page Size: " + pageSize + ").");
            var cards = _repo.GetAllPaged(pageIndex, pageSize);
            var VMs = new List<CardCreationViewModel>();
            foreach (var card in cards) VMs.Add(MapCardToCreational(card));
            StaticLogger.LogInfo(GetType(), "Service fetched " + cards.Count() + " Cards from Repo.");
            return VMs;
        }

        public Card Get(string cardCode) => _repo.Get(cardCode);

        public void Create(Card card) => _repo.Create(card);

        public void Update(Card card) => _repo.Update(card);

        public CardCreationViewModel MapCardToCreational(Card card)
            => new CardCreationViewModel
            {
                CardCode = card.CardCode,
                StartDate = card.StartDate.ToString(),
                EndDate = card.EndDate.ToString(),
                CVVNumber = card.CVVNumber,
                UniqueNumber = card.UniqueNumber,
                CustomerCode = GetCustCodeByCustId(card.CustomerId.GetValueOrDefault())
            };

        public Card MapCreational(CardCreationViewModel ccVM)
            => new Card
            {
                CardCode = ccVM.CardCode,
                StartDate = StringToDate(ccVM.StartDate),
                EndDate = StringToDate(ccVM.EndDate),
                CustomerId = _repo.GetCustIdByCustCode(ccVM.CustomerCode),
                CVVNumber = ccVM.CVVNumber,
                UniqueNumber = ccVM.UniqueNumber
            };

        public Card MapUpdate(CardUpdateViewModel cardVM)
            => new Card
            {
                StartDate = StringToDate(cardVM.StartDate),
                EndDate = StringToDate(cardVM.EndDate),
                CustomerId = _repo.GetCustIdByCustCode(cardVM.CustomerCode),
                CVVNumber = cardVM.CVVNumber,
                UniqueNumber = cardVM.UniqueNumber
            };

        public CardUpdateViewModel MapCreationalToUpdate(CardCreationViewModel ccVM)
            => new CardUpdateViewModel
            {
                StartDate = ccVM.StartDate,
                CustomerCode = ccVM.CustomerCode,
                CVVNumber = ccVM.CVVNumber,
                EndDate = ccVM.EndDate,
                UniqueNumber = ccVM.UniqueNumber
            };

        public CardUpdateViewModel MapCardToUpdate(Card card)
            => new CardUpdateViewModel
            {
                StartDate = card.StartDate.ToString(),
                EndDate = card.EndDate.ToString(),
                CustomerCode = GetCustCodeByCustId(card.CustomerId.GetValueOrDefault()),
                CVVNumber = card.CVVNumber,
                UniqueNumber = card.UniqueNumber
            };

        public bool VerifyCreational(CardCreationViewModel ccVM)
        {
            StaticLogger.LogInfo(GetType(), "Service verifying Card Creational View Model.");

            if (ccVM.CardCode == null || ccVM.CardCode == "")
            {
                StaticLogger.LogWarn(GetType(), "Service refused to evaluate Card Creation View Model because Card Code is null.");
                return false;
            }

            bool isValid = ccVM.CardCode.Length > 15 ? false : true;
            isValid = CardCodeExists(ccVM.CardCode) ? false : isValid;

            StaticLogger.LogInfo(GetType(), "Service verifying Card Creation View Model as Update View Model.");

            isValid = !VerifyUpdate(MapCreationalToUpdate(ccVM)) ? false : isValid;

            if (isValid) StaticLogger.LogInfo(GetType(), "Service approved Card Creational View Model.");
            else StaticLogger.LogWarn(GetType(), "Service rejected Card Creational View Model.");

            return isValid;
        }

        public bool VerifyUpdate(CardUpdateViewModel cardVM)
        {
            StaticLogger.LogInfo(GetType(), "Service verifying Card Update View Model.");


            bool isValid = cardVM.EndDate == null ? false : true;
            isValid = cardVM.StartDate == null ? false : isValid;
            isValid = cardVM.CustomerCode == null ? false : isValid;
            isValid = cardVM.CVVNumber == null ? false : isValid;
            isValid = cardVM.UniqueNumber == null ? false : isValid;

            if (!isValid)
            {
                StaticLogger.LogWarn(GetType(), "Service refused to evaluate Card Update View Model because of null values.");
                return false;
            }

            DateTime startDate, endDate;

            if (!DateTime.TryParse(cardVM.StartDate, out startDate) || !DateTime.TryParse(cardVM.EndDate, out endDate)) return false;

            isValid = _repo.CustomerCodeExists(cardVM.CustomerCode);
            isValid = cardVM.UniqueNumber.Length > 50 ? false : isValid;
            isValid = !FirstNParseToInt(cardVM.UniqueNumber, 4) ? false : isValid;
            isValid = cardVM.CVVNumber.Length > 15 ? false : isValid;
            isValid = !FirstNParseToInt(cardVM.CVVNumber, 8) ? false : isValid;
            isValid = startDate > endDate ? false : isValid;
            isValid = endDate > startDate.AddYears(3) ? false : isValid;

            if (isValid) StaticLogger.LogInfo(GetType(), "Service approved Card Update View Model.");
            else StaticLogger.LogWarn(GetType(), "Service rejected Card Update View Model.");

            return isValid;
        }

        public IEnumerable<CardCreationViewModel> GetAllFiltered(string searchBy)
        {
            StaticLogger.LogInfo(GetType(), "Searching Cards by " + searchBy);

            var cards = GetAll().ToList();
            var filteredCards = new List<CardCreationViewModel>();

            string[] searchTerms = searchBy.Split(' ');
            foreach (var term in searchTerms)
            {
                term.Trim();
                //if (cards.Any(c => c.CVVNumber == term || c.CardCode == term ||
                //                   c.UniqueNumber == term || term == GetCustCodeByCustId(c.CustomerId.GetValueOrDefault())))
                filteredCards.AddRange
                    (
                        cards.Where
                            (
                               c => c.CVVNumber == term || c.CardCode == term || c.UniqueNumber == term ||
                               term == c.CustomerCode ||
                               c.StartDate.ToString().Contains(term) ||
                               c.EndDate.ToString().Contains(term)
                            )
                    );
            }

            foreach (var card in filteredCards.ToList())
            {
                filteredCards.RemoveAll(c => c.CardCode == card.CardCode);
                filteredCards.Add(card);
            }

            StaticLogger.LogInfo(GetType(), "Retrieved " + filteredCards.Count() + " cards.");

            return filteredCards;
        }

        public IEnumerable<CardCreationViewModel> GetAllFilteredAndPaged(string searchBy, int pageIndex, int pageSize)
        => GetAllFiltered(searchBy).Skip((pageIndex - 1) * pageSize).Take(pageSize);

        public bool CardCodeExists(string cardCode) => _repo.CardCodeExists(cardCode);

        public bool FirstNParseToInt(string value, int n)
        {
            if (n == 0) return true;
            if (value.Length < n) return false;

            string firstN = value.Substring(0, n);

            int junk;
            return int.TryParse(firstN, out junk);
        }

        public string GetCustCodeByCustId(long custId) => _repo.GetCustCodeByCustId(custId);

        public DateTime? StringToDate(string input)
        {
            DateTime output = new DateTime(1400, 1, 1);

            if (DateTime.TryParse(input, out output)) return output;
            else return null;
        }

        #region Text Input Validators

        public (bool isValid, List<string> errorList) ValidateCode(string cardCode)
        {
            var isValid = true;
            var errorList = new List<string>();

            if (cardCode.Trim() == "" || cardCode == "Enter Card Code Here")
            {
                isValid = false;
                errorList.Add("Card Code cannot be empty.");
            }
            else
            {
                if (cardCode.Length > 15)
                {
                    isValid = false;
                    errorList.Add("Card Code is limited to 15 characters.");
                }

                if (CardCodeExists(cardCode))
                {
                    isValid = false;
                    errorList.Add("Card Code must be unique.");
                }
            }
            return (isValid, errorList);

        }

        public (bool isValid, List<string> errorList) ValidateUniNum(string uniNum)
        {
            var isValid = true;
            var errorList = new List<string>();

            if (uniNum.Trim() == "" || uniNum == "Enter Unique Number Here")
            {
                isValid = false;
                errorList.Add("Unique Number cannot be empty.");
            }


            if (uniNum.Length > 50)
            {
                isValid = false;
                errorList.Add("Unique Number is limited to 50 characters.");
            }

            if (uniNum.Length < 4)
            {
                isValid = false;
                errorList.Add("Unique Number cannot be shorter than 4 characters.");
            }
            else if (!FirstNParseToInt(uniNum, 4))
            {
                isValid = false;
                errorList.Add("First four characters of a Unique Number must be numbers.");
            }

            return (isValid, errorList);
        }

        public (bool isValid, List<string> errorList) ValidateCVV(string cvv)
        {
            var isValid = true;
            var errorList = new List<string>();

            if (cvv.Trim() == "" || cvv == "Enter CVV Number Here")
            {
                isValid = false;
                errorList.Add("CVV Number cannot be empty.");
            }


            if (cvv.Length > 15)
            {
                isValid = false;
                errorList.Add("CVV is limited to 15 characters.");
            }

            if (cvv.Length < 8)
            {
                isValid = false;
                errorList.Add("CVV cannot be shorter than 8 characters.");
            }
            else if (!FirstNParseToInt(cvv, 8))
            {
                isValid = false;
                errorList.Add("First 8 characters of a CVV Number must be digits.");
            }

            return (isValid, errorList);
        }

        public (bool isValid, List<string> errorList) ValidateCustCode(string custCode)
        {
            var isValid = true;
            var errorList = new List<string>();

            if (custCode.Trim() == "" || custCode == "Enter Customer Code Here")
            {
                isValid = false;
                errorList.Add("Cards require a Customer Code.");
            }

            else if (!_repo.CustomerCodeExists(custCode))
            {
                isValid = false;
                errorList.Add("Cards require an existing Customer Code.");
            }

            return (isValid, errorList);
        }

        public (bool isValid, List<string> errorList) ValidateStart(string startDate) => ValidateDate(startDate, true);

        public (bool isValid, List<string> errorList) ValidateStop(string endDate) => ValidateDate(endDate, false);

        public (bool isValid, List<string> errorList) ValidateDate(string date, bool isStart)
        {
            var isValid = true;
            var errorList = new List<string>();

            var start = new DateTime(1600, 1, 1);

            if (!DateTime.TryParse(date, out start))
            {
                isValid = false;
                var errorMessage = "Date must be in a valid Format.";
                errorMessage = isStart ? "Start " + errorMessage : "Stop " + errorMessage;
                errorList.Add(errorMessage);
            }

            return (isValid, errorList);
        }

        public (bool isValid, List<string> errorList) ValidateDates(DateTime start, DateTime stop)
        {
            var isValid = true;
            var errorList = new List<string>();

            if (stop <= start)
            {
                isValid = false;
                errorList.Add("Start Date must come before End Date.");
            }

            if (start < stop.AddYears(-3))
            {
                isValid = false;
                errorList.Add("Start and End dates must be within 3 years of each other.");
            }

            return (isValid, errorList);
        }

        #endregion
    }

}
