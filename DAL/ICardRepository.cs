using DAL.Entities;
using System.Collections.Generic;

namespace DAL
{
    public interface ICardRepository
    {
        IEnumerable<Card> GetAll();
        Card Get(string cardCode);
        long GetCustIdByCustCode(string custCode);
        string GetCustCodeByCustId(long custId);
        void Create(Card card);
        void Update(Card card);
        bool CustomerCodeExists(string custCode);
        bool CardCodeExists(string cardCode);
        IEnumerable<Card> GetAllPaged(int pageIndex, int pageSize);
    }
}