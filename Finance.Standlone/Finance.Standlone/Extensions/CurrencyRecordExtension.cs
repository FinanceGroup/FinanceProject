using Finance.Contract.Bos;
using Finance.DAL.Records;

namespace Finance.Standlone.Extensions
{
    public static class CurrencyRecordExtension
    {
        public static CurrencyBo ToCurrencyBo(this CurrencyRecord record)
        {
            return new CurrencyBo
            {
                AccountingRates = record.AccountingRates,
                CreateaByDisplayName = record.CreatedByDisplayName,
                CreatedBy = record.CreatedBy,
                CurrencyCode = record.CurrencyCode,
                CurrencyName = record.CurrencyName,
                DecimalPlaces = record.DecimalPlaces,
                Id = record.Id,
                IsActive = record.IsActive,
                UpdatedBy = record.UpdatedBy,
                UpdatedByDisplayName = record.UpdatedByDisplayName,
            };
        }
    }
}
