using DAL.Entities;
using Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace DAL
{
    public class CustomerRepository : ICustomerRepository
    {
        private CustomerWPF_AppDbContext _context;

        public CustomerRepository()
        {
            _context = new CustomerWPF_AppDbContext();
        }

        public IEnumerable<Customer> GetAll() => _context.Customers.ToList();

        public IEnumerable<Customer> GetAllPaged(int pageIndex, int PageSize) 
            => _context.Customers
            .ToList()
            .Skip((pageIndex - 1) * PageSize)
            .Take(PageSize);

        public void Create(Customer cust)
        {
            StaticLogger.LogInfo(GetType(),"Repo adding new Customer with code: " + cust.CustomerCode);
            _context.Customers.Add(cust);
            _context.SaveChanges();
        }

        public void Update(Customer cust)
        {
            StaticLogger.LogInfo(GetType(),"Repo updating Customer with code: " + cust.CustomerCode);
            var custToUpdate = _context.Customers.SingleOrDefault(c => c.CustomerCode == cust.CustomerCode);
            custToUpdate.Address = cust.Address;
            custToUpdate.CNP = cust.CNP;
            custToUpdate.Name = cust.Name;
            _context.SaveChanges();
        }

        public int CustomerCount() => _context.Customers.Count();

        public bool CustomerCodeExists(string custCode) => _context.Customers.Any(c => c.CustomerCode == custCode);
    }
}
