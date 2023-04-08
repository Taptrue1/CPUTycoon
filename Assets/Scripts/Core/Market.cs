namespace Core
{
    /// <summary>
    /// Market manages all the companies and their incomes, fans and so on.
    /// </summary>
    public class Market
    {
        private readonly Company _playerCompany;
        private readonly Company[] _enemyCompanies;
        
        public Market(Company playerCompany)
        {
            _playerCompany = playerCompany;
        }
    }
}