using System;
using System.Collections.Generic;
using System.Linq;

using Finance.Contract.Bos;
using Finance.Contract.Requests;
using Finance.Contract.Responses;
using Finance.DAL.DAOs;
using Finance.Framework.Logging;
using Finance.Standlone.Extensions;

namespace Finance.Standlone.Managers
{
    public class CurrencyManager : ICurrencyManager
    {
        private readonly IUserDAO _userDAO = null;
        private readonly ICurrencyDAO _currencyDAO = null;
        public CurrencyManager(IUserDAO userDAO, ICurrencyDAO currencyDAO)
        {
            _userDAO = userDAO;
            _currencyDAO = currencyDAO;

            Logger = NullLogger.Instance;
        }

        public ILogger Logger { get; set; }

        public SaveCurrencyResponse UpdateCurrency(SaveCurrencyRequest request)
        {
            var response = new SaveCurrencyResponse();

            try
            {
                var userRecord = _userDAO.GetRecord(request.userName);

                var currencyRecord = _currencyDAO.GetRecord(request.currencyCode);
                
                currencyRecord.AccountingRates = request.AccountingRates;
                currencyRecord.CreatedTime = DateTime.Now;
                currencyRecord.CurrencyCode = request.currencyCode;
                currencyRecord.CurrencyName = request.currencyName;
                currencyRecord.DecimalPlaces = request.decimalPlaces;
                currencyRecord.IsActive = true;
                currencyRecord.UpdatedBy = userRecord.UserName;
                currencyRecord.UpdatedTime = DateTime.Now;
                currencyRecord.UpdatedByDisplayName = userRecord.UserName;

                 currencyRecord = _currencyDAO.UpdateRecord(currencyRecord);
             

                var currencyBo = currencyRecord.ToCurrencyBo();

                response.Bos = new List<CurrencyBo> { currencyBo };

                response.IsSuccess = true;
                response.Message = string.Empty;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.ToString();

                Logger.Error(ex, "ModifyCurrency failed");
            }

            return response;
        }

        public SaveCurrencyResponse SaveCurrency(SaveCurrencyRequest request)
        {
            var response = new SaveCurrencyResponse();

            try
            {
                var userRecord = _userDAO.GetRecord(request.userName);

                var currencyRecord = _currencyDAO.ConstructRecord(request.currencyCode, request.currencyName,
                    request.AccountingRates, request.decimalPlaces, userRecord.UserName, userRecord.UserName);

                currencyRecord = _currencyDAO.CreateRecord(currencyRecord);

                var currencyBo = currencyRecord.ToCurrencyBo();

                response.Bos = new List<CurrencyBo> { currencyBo };

                response.IsSuccess = true;
                response.Message = string.Empty;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.ToString();

                Logger.Error(ex, "SaveCurrency failed");
            }

            return response;
        }

        public GetCurrenciesResponse LoadCurrencies()
        {
            var response = new GetCurrenciesResponse();
            try
            {
                var records = _currencyDAO.LoadRecords();

                response.Bos = from record in records select record.ToCurrencyBo();
                response.IsSuccess = true;
                response.Message = string.Empty;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.ToString();

                Logger.Error(ex, "LoadCurrencies failed");
            }

            return response;

        }
    }
}
