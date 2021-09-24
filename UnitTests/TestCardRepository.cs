using DAL;
using DAL.Entities;
using System.Collections.Generic;
using System.Linq;

namespace UnitTests
{
    public class TestCardRepository : ICardRepository
    {
        private TestDb _db;
        public List<Card> Cards; 

        public TestCardRepository()
        {
            _db = new TestDb();
            Cards = _db.Cards;
        }

        public bool CardCodeExists(string cardCode)=> Cards.Any(c => c.CardCode == cardCode);

        public void Create(Card card) => Cards.Add(card);

        public bool CustomerCodeExists(string custCode)=> _db.Customers.Any(c => c.CustomerCode == custCode);

        public Card Get(string cardCode)=> Cards.SingleOrDefault(c => c.CardCode == cardCode);

        public IEnumerable<Card> GetAll() => Cards.ToList();

        public IEnumerable<Card> GetAllPaged(int pageIndex, int pageSize)
            => Cards.ToList().Skip((pageIndex - 1) * pageSize).Take(pageSize);

        public string GetCustCodeByCustId(long custId)
            =>_db.Customers
            .Where(c => c.ID == custId)
            .Select(c => c.CustomerCode).SingleOrDefault();

        public long GetCustIdByCustCode(string custCode)
            =>_db.Customers
            .Where(c => c.CustomerCode == custCode)
            .Select(c => c.ID).SingleOrDefault();

        public void Update(Card card)
        {
            var cardToUpdate = Get(card.CardCode);
            cardToUpdate.CustomerId = card.CustomerId;
            cardToUpdate.StartDate = card.StartDate;
            cardToUpdate.EndDate = card.EndDate;
            cardToUpdate.CVVNumber = card.CVVNumber;
            cardToUpdate.UniqueNumber = card.UniqueNumber;
        }
    }
}
