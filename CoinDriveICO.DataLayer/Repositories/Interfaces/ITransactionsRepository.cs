using CoinDriveICO.DataLayer.Model;
using CoinDriveICO.DataLayer.Repositories.Base;

namespace CoinDriveICO.DataLayer.Repositories.Interfaces
{
    public interface ITransactionsRepository : IBaseRepository<Transaction,string>
    {
        
    }
}