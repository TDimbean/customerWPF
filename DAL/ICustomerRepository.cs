using DAL.Entities;
using System.Collections.Generic;

namespace DAL
{
    public interface ICustomerRepository
    {
        IEnumerable<Customer> GetAll();
        IEnumerable<Customer> GetAllPaged(int pageIndex, int PageSize);
        void Create(Customer cust);
        void Update(Customer cust);
        bool CustomerCodeExists(string custCode);
        int CustomerCount();
    }
}