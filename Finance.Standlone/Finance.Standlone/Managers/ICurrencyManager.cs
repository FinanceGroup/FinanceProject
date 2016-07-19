using Finance.Contract.Requests;
using Finance.Contract.Responses;
using Finance.Framework;

namespace Finance.Standlone.Managers
{
    public interface ICurrencyManager : IDependency
    {
        SaveCurrencyResponse SaveCurrency(SaveCurrencyRequest request);

        GetCurrenciesResponse LoadCurrencies();
    }
}
