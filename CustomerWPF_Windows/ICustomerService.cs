using DAL.Entities;
using Infrastructure.ViewModels;
using System.Collections.Generic;

namespace CustomerWPF_Windows
{
    public interface ICustomerService
    {
        IEnumerable<Customer> GetAll();
        void Create(Customer cust);
        void Update(Customer cust);
        
        //Mappers

        Customer MapCreational(CustomerCreationViewModel ccVM);
        CustomerUpdateViewModel MapCreationalToUpdate(CustomerCreationViewModel ccVM);
        Customer MapUpdate(CustomerUpdateViewModel custVM);
        CustomerUpdateViewModel MapCustomerToUpdate(Customer customer);

        //Model Verifiers

        bool VerifyCreational(CustomerCreationViewModel ccVM);
        bool VerifyUpdate(CustomerUpdateViewModel custVM);

        //Helpers

        bool CustomerCodeExists(string custCode);
        bool IsValidCNP(string cnp);
        bool IsValidName(string name);
        int CustomerCount();

        //Filter and Paginate

        IEnumerable<Customer> GetAllFiltered(string searchBy);
        IEnumerable<Customer> GetAllPaged(int pageIndex, int pageSize);
        IEnumerable<Customer> GetAllFilteredAndPaged(string searchBy, int pageIndex, int pageSize);

        //Subview Validators

        (bool isValid, List<string> errorList) ValidateName(string name);
        (bool isValid, List<string> errorList) ValidateCNP(string cnp);
        (bool isValid, List<string> errorList) ValidateCode(string custCode);
        (bool isValid, List<string> errorList) ValidateAddress(string adr);
    }
}