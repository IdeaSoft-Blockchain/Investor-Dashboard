using System.Collections.Generic;
using System.Threading.Tasks;
using CoinDriveICO.DataLayer.Model;

namespace CoinDriveICO.BusinessLayer
{
    public interface ITransactionWorkerService
    {
        Task<IEnumerable<Transaction>> ProcessTransactionsAsync();
    }
}