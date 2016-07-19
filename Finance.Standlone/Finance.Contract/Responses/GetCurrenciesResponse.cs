using System.Collections.Generic;

using Finance.Contract.Bos;

namespace Finance.Contract.Responses
{
    public class GetCurrenciesResponse : BaseResponse<CurrencyBo>
    {
        public GetCurrenciesResponse()
        {
            Bos = new List<CurrencyBo>();
        }
    }
}
