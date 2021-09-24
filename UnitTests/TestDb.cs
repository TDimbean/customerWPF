using DAL.Entities;
using System;
using System.Collections.Generic;

namespace UnitTests
{
    public class TestDb
    {
        public List<Card> Cards = new List<Card>
        {
            new Card { ID=1, CardCode="30415805867353", CustomerId=1, UniqueNumber="1460758870500045", CVVNumber="141412282719169", StartDate=DateTime.Parse("1/5/2018"), EndDate=DateTime.Parse("1/9/2019")},
            new Card { ID=2, CardCode="97224928465418", CustomerId=2, UniqueNumber="2460758870500045", CVVNumber="241412282719169", StartDate=DateTime.Parse("2/5/2017"), EndDate=DateTime.Parse("1/9/2018")},
            new Card { ID=3, CardCode="36383281445889", CustomerId=3, UniqueNumber="3460758870500045", CVVNumber="341412282719169", StartDate=DateTime.Parse("3/5/2016"), EndDate=DateTime.Parse("1/9/2017")}
        };

        public List<Customer> Customers = new List<Customer>

        {
            new Customer {ID=1, CustomerCode="7652966", Name="John Doe",CNP="1833052155217",Address="1 Miwake Avenue"},
            new Customer {ID=2, CustomerCode="9121746", Name="Jane Doe",CNP="2833052155217",Address="2 Charles Lane"},
            new Customer {ID=3, CustomerCode="1930545", Name="Justin Smith",CNP="1803052155217",Address="3 Miwake Avenue"}
        };
    }
}
