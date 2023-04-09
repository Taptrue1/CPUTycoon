using Core.Services;

namespace Core
{
    public class Game
    {
        public Game(TickService tickService)
        {
            var company = new Company("Test", 1000, tickService);
            var market = new Market(company);
        }
    }
}