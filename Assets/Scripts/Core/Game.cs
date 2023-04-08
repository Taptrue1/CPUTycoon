namespace Core
{
    public class Game
    {
        public Game()
        {
            var company = new Company("Test", 1000, "Test");
            var market = new Market(company);
        }
    }
}