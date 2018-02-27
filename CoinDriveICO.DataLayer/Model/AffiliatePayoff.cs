using CoinDriveICO.DataLayer.Model.Base;

namespace CoinDriveICO.DataLayer.Model
{
    public class AffiliatePayoff : BaseEntity
    {
        public int Id { get; set; }
        public int InnerTransactionId { get; set; }
        public InnerTransaction InnerTransaction { get; set; }
        public int PayingUserId { get; set; }
        public AppUser PayingUser { get; set; }
        public int AffiliateUserId { get; set; }
        public AppUser AffiliateUser { get; set; }
        public decimal TransactionValue { get; set; }
        public decimal AffiliatePayoffMultiplier { get; set; }
        public decimal PayoffValue { get; set; }
    }
}