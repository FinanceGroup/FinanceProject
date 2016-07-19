using System;
using System.Web.Http;

using Finance.Contract.Requests;
using Finance.Framework.Hubs;
using Finance.Standlone.Hubs;
using Finance.Standlone.Managers;

using Newtonsoft.Json;

namespace Finance.Standlone.Controllers
{
    public class CurrencyController : ApiControllerWithHub<CurrencyGridHub>
    {
        private readonly ICurrencyManager _currencyManager = null;
        public CurrencyController(ICurrencyManager currencyManager)
        {
            _currencyManager = currencyManager;
        }

        public IHttpActionResult Get()
        {
            var response = _currencyManager.LoadCurrencies();

            if (response.IsSuccess)
            {
                return Ok(JsonConvert.SerializeObject(response));
            }

            return InternalServerError(JsonConvert.DeserializeObject<Exception>(response.Message));
        }

        public IHttpActionResult Post([FromBody] SaveCurrencyRequest request)
        {
            var response = _currencyManager.SaveCurrency(request);
            
            if (response.IsSuccess)
            {
                return Ok(JsonConvert.SerializeObject(response));
            }

            return InternalServerError(JsonConvert.DeserializeObject<Exception>(response.Message));
        }
    }
}
