using System.Collections;
using System.Collections.Generic;
using Finance.DAL.Records;
using Finance.Framework;

namespace Finance.DAL.DAOs
{
    public interface ICurrencyDAO : IDependency
    {
        IEnumerable<CurrencyRecord> LoadRecords();
        CurrencyRecord GetRecord(string currencyCode);
        CurrencyRecord SaveRecord(CurrencyRecord record);

        CurrencyRecord ConstructRecord(string currencyCode, string currencyName,
            decimal AccountingRates, int decimalPlaces, string userName, string userDisplayName);

    }
}
