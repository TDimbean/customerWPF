using DAL;
using DAL.Entities;
using System.Collections.Generic;
using System.Linq;

namespace UnitTests
{
    public class TestCustomerRepository : ICustomerRepository
    {
        private TestDb _db;
        private List<Customer> Customers;

        public TestCustomerRepository()
        {
            _db = new TestDb();
            Customers = _db.Customers;
        }

        public void Create(Customer cust)=> Customers.Add(cust);

        public bool CustomerCodeExists(string custCode)=> Customers.Any(c => c.CustomerCode == custCode);

        public int CustomerCount()=> Customers.Count();

        public IEnumerable<Customer> GetAll()=> Customers.ToList();

        public IEnumerable<Customer> GetAllPaged(int pageIndex, int PageSize)
            => Customers.ToList()
            .Skip((pageIndex - 1) * PageSize)
            .Take(PageSize);

        public void Update(Customer cust)
        {
            var custToUpdate = Customers.SingleOrDefault(c => c.CustomerCode == cust.CustomerCode);
            custToUpdate.Address = cust.Address;
            custToUpdate.CNP = cust.CNP;
            custToUpdate.Name = cust.Name;
        }
    }
}
