using DAL;
using DAL.Entities;
using Infrastructure;
using Infrastructure.Helpers;
using Infrastructure.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity;

namespace CustomerWPF_Windows
{
    public class CustomerService : ICustomerService
    {
        [Dependency]
        private ICustomerRepository _repo;

        public CustomerService(ICustomerRepository repo)
        {
            _repo = repo;
        }

        public IEnumerable<Customer> GetAll() => _repo.GetAll();

        public void Create(Customer cust) => _repo.Create(cust);

        public void Update(Customer cust) => _repo.Update(cust);

        #region Mappers

        public CustomerUpdateViewModel MapCreationalToUpdate(CustomerCreationViewModel ccVM)
            => new CustomerUpdateViewModel
            {
                Address = ccVM.Address,
                CNP = ccVM.CNP,
                Name = ccVM.Name
            };

        public Customer MapCreational(CustomerCreationViewModel ccVM)
            => new Customer
            {
                CustomerCode = ccVM.CustomerCode,
                Address = ccVM.Address,
                CNP = ccVM.CNP,
                Name = ccVM.Name
            };

        public Customer MapUpdate(CustomerUpdateViewModel custVM)
            => new Customer
            {
                Name = custVM.Name,
                Address = custVM.Address,
                CNP = custVM.CNP
            };

        public CustomerUpdateViewModel MapCustomerToUpdate(Customer customer)
            => new CustomerUpdateViewModel
            {
                Name = customer.Name,
                CNP = customer.CNP,
                Address = customer.Address
            };

        #endregion

        public bool VerifyCreational(CustomerCreationViewModel ccVM)
        {
            StaticLogger.LogInfo(GetType(), "Service verifying Customer Creational View Model.");

            if (ccVM.CustomerCode == null||ccVM.CustomerCode==""||ccVM.CustomerCode=="Enter Customer Code Here")
            {
                StaticLogger.LogWarn(GetType(), "Service refused to evaluate Customer Creation View Model because Customer Code is null.");
                return false;
            }

            bool isValid = CustomerCodeExists(ccVM.CustomerCode) ? false : true;
            isValid = ccVM.CustomerCode.Length > 50 ? false : isValid;

            StaticLogger.LogInfo(GetType(), "Service verifying Customer Creation View Model as Update View Model.");


            isValid = !VerifyUpdate(MapCreationalToUpdate(ccVM)) ? false : isValid;

            if (isValid) StaticLogger.LogInfo(GetType(), "Service approved Customer Creational View Model.");
            else StaticLogger.LogWarn(GetType(), "Service rejected Customer Creational View Model.");

            return isValid;
        }

        public bool VerifyUpdate(CustomerUpdateViewModel custVM)
        {
            StaticLogger.LogInfo(GetType(), "Service verifying Customer Update View Model.");

            bool isValid = custVM.Name == null ? false : true;
            isValid = custVM.CNP == null ? false : isValid;
            isValid = custVM.Address == null || custVM.Address=="Enter Address Here" ? false : isValid;

            if (!isValid)
            {
                StaticLogger.LogWarn(GetType(), "Service refused to evaluate Customer Update View Model because of null values.");
                return false;
            }

            isValid = !IsValidName(custVM.Name) ? false : true;
            isValid = custVM.Address.Length > 200 || custVM.Address.Trim().Length==0 ? false : isValid;
            isValid = !IsValidCNP(custVM.CNP) ? false : isValid;

            if (isValid) StaticLogger.LogInfo(GetType(), "Service approved Customer Update View Model.");
            else StaticLogger.LogWarn(GetType(), "Service rejected Customer Update View Model.");

            return isValid;
        }
        
        public int CustomerCount() => _repo.CustomerCount();

        public IEnumerable<Customer> GetAllFiltered(string searchBy)
        {
            StaticLogger.LogInfo(GetType(), "Searching Customers by " + searchBy);


            var customers = GetAll().ToList();
            var filteredCustomers = new List<Customer>();

            string[] searchTerms = searchBy.Split(' ');
            foreach (var term in searchTerms)
            {
                term.Trim();
                //if (customers.Any(c => c.CNP == term || c.CustomerCode == term ||
                //    c.Name.ToUpper().Contains(term.ToUpper()) ||
                //    c.Address.RemoveNumbersFromMultiWord().ToUpper().Contains(term.ToUpper()))
                filteredCustomers.AddRange
                        (
                            customers.Where(c => c.CNP == term || c.CustomerCode == term ||
                                    c.Name.ToUpper().Contains(term.ToUpper()) ||
                                    c.Address.RemoveNumbersFromMultiWord().ToUpper().Contains(term.ToUpper()))
                        );
            };

            foreach (var customer in filteredCustomers.ToList())
            {
                filteredCustomers.RemoveAll(c => c.CustomerCode == customer.CustomerCode);
                filteredCustomers.Add(customer);
            }

            StaticLogger.LogInfo(GetType(), "Retrieved " + filteredCustomers.Count() + " customers.");


            return filteredCustomers;
        }

        public IEnumerable<Customer> GetAllPaged(int pageIndex, int pageSize) => _repo.GetAllPaged(pageIndex, pageSize);

        public IEnumerable<Customer> GetAllFilteredAndPaged(string searchBy, int pageIndex, int pageSize)
            => GetAllFiltered(searchBy).Skip((pageIndex - 1) * pageSize).Take(pageSize);

        public bool IsValidName(string name)
        {
            if (name.Length > 100) return false;
            if (name.Trim() == "") return false;
       
            string[] words = name.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);

            if (words.Length != 2) return false;
            //if (words.Length < 2) return false;

            foreach (string word in words) if (char.IsLower(word[0])) return false;

            return true;
        }

        public bool IsValidCNP(string cnp)
        {
            if (cnp.Length != 13) return false;

            foreach (var character in cnp)
            {
                int junk = 0;
                if (!int.TryParse(character.ToString(), out junk)) return false;
            }

            if (!(cnp[0] == '1' || cnp[0] == '2')) return false;

            var month = int.Parse(cnp.Substring(3, 2));
            var day = int.Parse(cnp.Substring(5, 2));
            if (!IsValidCnpDate(month, day)) return false;

            return true;
        }

        public bool IsValidCnpDate(int month, int day)
        {
            if (month == 0 || day == 0) return false;
            if (month > 12) return false;
            if (month == 2 && day > 29) return false;
            else
            {
                if (month < 8 && (month % 2 == 0)) if (day > 30) return false;
                if (month < 8 && (month % 2 != 0)) if (day > 31) return false;
                if (month >= 8 && (month % 2 == 0)) if (day > 31) return false;
                    else if (day > 30) return false;
            }

            return true;
        }

        public bool CustomerCodeExists(string custCode) => _repo.CustomerCodeExists(custCode);

        #region Input Field Validators

        public (bool isValid, List<string> errorList) ValidateName(string name)
        {
            var isValid = true;
            var errorList = new List<string>(); 

            if (name.Trim() == "" || name.Trim()=="Enter Name Here")
            {
                isValid = false;
                errorList.Add("Name field cannot be empty.");
            }
            if (name.Length > 100)
            {
                isValid = false;
                errorList.Add("Name must not exceed 100 characters.");
            };

            string[] words = name.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);

            if (words.Length != 2)
            {
                isValid = false;
                errorList.Add("Name must be made up of First and Last names with a space inbewteen.");
            }
            //if (words.Length < 2) return false;
            else
            {
                foreach (string word in words) if (char.IsLower(word[0]))
                    {
                        isValid = false;
                        errorList.Add("Each word of Name must start with a capital letter.");
                        break;
                    }
            }

            return (isValid, errorList);
        }
     
        public (bool isValid, List<string> errorList) ValidateCNP(string cnp)
        {
            var isValid = true;
            var errorList = new List<string>();

            if (cnp.Length != 13)
            {
                isValid = false;
                errorList.Add("CNP must contain 13 characters.");
            }

            foreach (var character in cnp)
            {
                int junk = 0;
                if (!int.TryParse(character.ToString(), out junk))
                {
                    isValid = false;
                    errorList.Add("CNP must have numbers only.");
                    break;
                }
            }

            if (!string.IsNullOrEmpty(cnp.Trim()))
                {
                if (!(cnp[0] == '1' || cnp[0] == '2'))
                {
                    isValid = false;
                    errorList.Add("The first character of a CNP must be 1(male) or 2(female).");
                }
            }

            if (cnp.Length >= 7)
            {
                var month = int.Parse(cnp.Substring(3, 2));
                var day = int.Parse(cnp.Substring(5, 2));

                if (!IsValidCnpDate(month, day))
                {
                    isValid = false;
                    errorList.Add("The second to seventh characters of a CNP must represent the birthdate (YY/MM/DD)");
                }
            }

            return (isValid, errorList);
        }

        public (bool isValid, List<string> errorList) ValidateCode(string custCode)
        {
            var isValid = true;
            var errorList = new List<string>();

            if (custCode.Trim()==""||custCode.Trim()=="Enter Customer Code Here")
            {
                isValid = false;
                errorList.Add("Customer Code cannot be empty");
            }

            if (custCode.Length > 50)
            {
                isValid = false;
                errorList.Add("Customer Code is limited to 50 characters.");
            }

            if (CustomerCodeExists(custCode))
            {
                isValid = false;
                errorList.Add("Customer Code must be unique");
            }

            return (isValid, errorList);
        }

        public (bool isValid, List<string> errorList) ValidateAddress(string adr)
        {
            var isValid = true;
            var errorList = new List<string>();

            if (adr.Trim() == "" || adr.Trim()=="Enter Address Here")
            {
                isValid = false;
                errorList.Add("Address cannot be empty");
            }

            else if (adr.Length > 200)
            {
                isValid = false;
                errorList.Add("Address cannot exceed 200 characters.");
            }

            return (isValid, errorList);
        }

        #endregion
    }
}
