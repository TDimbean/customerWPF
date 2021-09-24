using DAL.Entities;
using Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace DAL
{
    public class CardRepository : ICardRepository
    {
        private readonly CustomerWPF_AppDbContext _context;

        public CardRepository()
        {
            _context = new CustomerWPF_AppDbContext();
        }

        public CardRepository(CustomerWPF_AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Card> GetAll() => _context.Cards.ToList();

        public IEnumerable<Card> GetAllPaged(int pageIndex, int pageSize) => _context.Cards.ToList().Skip((pageIndex - 1) * pageSize).Take(pageSize);

        public Card Get(string cardCode) => _context.Cards.SingleOrDefault(c => c.CardCode == cardCode);

        public void Create(Card card)
        {
            StaticLogger.LogInfo(GetType(), "Repo adding new card with code: " + card.CardCode);
            _context.Cards.Add(card);
            _context.SaveChanges();
        }

        public void Update(Card card)
        {
            StaticLogger.LogInfo(GetType(), "Repo updating card with code: " + card.CardCode);

            var cardToUpdate = Get(card.CardCode);
            cardToUpdate.CustomerId = card.CustomerId;
            cardToUpdate.StartDate = card.StartDate;
            cardToUpdate.EndDate = card.EndDate;
            cardToUpdate.CVVNumber = card.CVVNumber;
            cardToUpdate.UniqueNumber = card.UniqueNumber;
            _context.SaveChanges();
        }

        public long GetCustIdByCustCode(string custCode)
            => _context.Customers
            .Where(c => c.CustomerCode == custCode)
            .Select(c => c.ID).SingleOrDefault();

        public string GetCustCodeByCustId(long custId)
            => _context.Customers
            .Where(c => c.ID == custId)
            .Select(c => c.CustomerCode).SingleOrDefault();

        public bool CustomerCodeExists(string custCode) => _context.Customers.Any(c => c.CustomerCode == custCode);

        public bool CardCodeExists(string cardCode) => _context.Cards.Any(c => c.CardCode == cardCode);
    }
}
