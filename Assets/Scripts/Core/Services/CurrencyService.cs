using System.Collections.Generic;
using Settings;
using Utils.CustomNumbers;

namespace Core.Services
{
    public class CurrencyService
    {
        private readonly Dictionary<string, CustomNumber<int>> _currencies;
        
        public CurrencyService(CoreSettings coreSettings)
        {
            _currencies = new Dictionary<string, CustomNumber<int>>();
            
            foreach (var currencyData in coreSettings.CurrencySettings.CurrenciesDatas)
                _currencies.Add(currencyData.Name, new CustomNumber<int>(currencyData.StartValue));
        }
        public CustomNumber<int> GetCurrency(string name)
        {
            if(!_currencies.ContainsKey(name))
                throw new KeyNotFoundException($"Currency with name {name} not found");
            
            return _currencies[name];
        }
    }
}