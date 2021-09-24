using DAL.Entities;
using Infrastructure.ViewModels;
using System;
using System.Collections.Generic;

namespace CustomerWPF_Windows
{
    public interface ICardService
    {
        IEnumerable<CardCreationViewModel> GetAll();
        Card Get(string cardCode);
        void Create(Card card);
        void Update(Card card);

        // Verifiers
        bool VerifyCreational(CardCreationViewModel ccVM);
        bool VerifyUpdate(CardUpdateViewModel cardVM);

        // Helpers
        bool CardCodeExists(string cardCode);
        bool FirstNParseToInt(string value, int n);
        string GetCustCodeByCustId(long custId);

        // Filtering and Pagination
        IEnumerable<CardCreationViewModel> GetAllFiltered(string searchBy);
        IEnumerable<CardCreationViewModel> GetAllPaged(int pageIndex, int pageSize);
        IEnumerable<CardCreationViewModel> GetAllFilteredAndPaged(string searchBy, int pageIndex, int pageSize);

        // Mappers
        CardUpdateViewModel MapCardToUpdate(Card card);
        CardCreationViewModel MapCardToCreational(Card card);
        Card MapCreational(CardCreationViewModel ccVM);
        CardUpdateViewModel MapCreationalToUpdate(CardCreationViewModel ccVM);
        Card MapUpdate(CardUpdateViewModel cardVM);

        // Subview Validators
        (bool isValid, List<string> errorList) ValidateCode(string cardCode);
        (bool isValid, List<string> errorList) ValidateUniNum(string uniNum);
        (bool isValid, List<string> errorList) ValidateCVV(string cvv);
        (bool isValid, List<string> errorList) ValidateCustCode(string custCode);
        (bool isValid, List<string> errorList) ValidateStart(string startDate);
        (bool isValid, List<string> errorList) ValidateStop(string endDate);
        (bool isValid, List<string> errorList) ValidateDate(string date, bool isStart);
        (bool isValid, List<string> errorList) ValidateDates(DateTime start, DateTime stop);
    }
}