using Core.Services;
using Zenject;

namespace Core.Games
{
    public class Game
    {
        public int RPPerDay { get; set; }
        public int DPPerDay { get; set; }
        public int RPPrice => RPPerDay * 10;
        public int DPPrice => DPPrice * 10;
        
        private readonly CurrencyService _currencyService;
        
        [Inject]
        public Game(CurrencyService currencyService, TimeService timeService)
        {
            _currencyService = currencyService;

            timeService.Tick += OnTick;
        }

        #region TickMethods
        
        private void OnTick()
        {
            _currencyService.GetCurrency("RP").Value += RPPerDay;
            _currencyService.GetCurrency("DP").Value += DPPerDay;
        }

        #endregion
    }
}