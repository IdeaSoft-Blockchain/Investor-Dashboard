using System.Collections.Generic;
using CoinDriveICO.DataLayer.Model.Base;

namespace CoinDriveICO.DataLayer.Model
{
    public class InnerTransaction : BaseEntity
    {
        public int Id { get; set; }
        public string AssociatedTransactionId { get; set; }
        public Transaction AssociatedTransaction { get; set; }
        public int UserId { get; set; }
        public AppUser User { get; set; }
        public string FromType { get; set; }
        public decimal FromValue { get; set; }
        public decimal ToValue { get; set; }
        public decimal TypeToTokenConversationRate { get; set; }
        public decimal TokenMultiplier { get; set; }
        public IEnumerable<AffiliatePayoff> AssociatedAffiliatePayoffs { get; set; }
    }
}