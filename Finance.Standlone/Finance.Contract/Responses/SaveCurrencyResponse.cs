using System.Collections.Generic;

using Finance.Contract.Bos;

namespace Finance.Contract.Responses
{
    public class SaveCurrencyResponse : BaseResponse<CurrencyBo>
    {
        public SaveCurrencyResponse()
        {
            Bos = new List<CurrencyBo>();
        }
    }
}
