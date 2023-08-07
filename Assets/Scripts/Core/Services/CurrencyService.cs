using System.Collections.Generic;
using Settings;
using Utils.CustomNumbers;

namespace Core.Services
{
    public class CurrencyService
    {
        private readonly Dictionary<string, CustomNumber<double>> _currencies;
        
        public CurrencyService(CoreSettings coreSettings)
        {
            _currencies = new Dictionary<string, CustomNumber<double>>();
            
            foreach (var currencyData in coreSettings.CurrencySettings.CurrenciesDatas)
                _currencies.Add(currencyData.Name, new CustomNumber<double>(currencyData.StartValue));
        }
        public CustomNumber<double> GetCurrency(string name)
        {
            if(!_currencies.ContainsKey(name))
                throw new KeyNotFoundException($"Currency with name {name} not found");
            
            return _currencies[name];
        }
    }
}