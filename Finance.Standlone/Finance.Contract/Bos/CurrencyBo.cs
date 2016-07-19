namespace Finance.Contract.Bos
{
    public class CurrencyBo : BaseBo
    {
        public string Id { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencyName { get; set; }
        public decimal AccountingRates { get; set; }
        public int DecimalPlaces { get; set; }
        public string CreatedBy { get; set; }
        public string CreateaByDisplayName { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdatedByDisplayName { get; set; }
        public bool IsActive { get; set; }

    }
}
