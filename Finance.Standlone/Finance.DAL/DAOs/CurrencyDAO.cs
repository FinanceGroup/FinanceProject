using System;
using System.Collections.Generic;

using Finance.DAL.Records;
using Finance.Framework.Data;

namespace Finance.DAL.DAOs
{
    public class CurrencyDAO : ICurrencyDAO
    {
        private readonly IRepository<CurrencyRecord> _currencyRecordRepository = null;

        public CurrencyDAO(IRepository<CurrencyRecord> currencyRecordRepository)
        {
            _currencyRecordRepository = currencyRecordRepository;
        }
        public CurrencyRecord ConstructRecord(string currencyCode, string currencyName,
            decimal AccountingRates, int decimalPlaces, string userName, string userDisplayName)
        {
            return new CurrencyRecord
            {
                AccountingRates = AccountingRates,
                CreatedBy = userName,
                CreatedByDisplayName = userDisplayName,
                CreatedTime = DateTime.Now,
                CurrencyCode = currencyCode,
                CurrencyName = currencyName,
                DecimalPlaces = decimalPlaces,
                IsActive = true,
                UpdatedBy = userName,
                UpdatedTime = DateTime.Now,
                UpdatedByDisplayName = userDisplayName,
            };
        }

        public CurrencyRecord GetRecord(string currencyCode)
        {
            var record = _currencyRecordRepository.Get(o => o.CurrencyCode == currencyCode && o.IsActive);
            return record;
        }

        public IEnumerable<CurrencyRecord> LoadRecords()
        {
            var records = _currencyRecordRepository.Fetch(o => o.IsActive);
            return records;
        }

        public CurrencyRecord SaveRecord(CurrencyRecord record)
        {
            _currencyRecordRepository.Create(record);
            return record;
        }
    }
}
