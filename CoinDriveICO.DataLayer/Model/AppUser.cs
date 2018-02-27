using System;
using System.Collections.Generic;
using CoinDriveICO.DataLayer.Model.Base;
using Microsoft.AspNetCore.Identity;

namespace CoinDriveICO.DataLayer.Model
{
    public class AppUser : IdentityUser<int>, IBaseEntity
    {
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string CreateUser { get; set; }
        public string UpdateUser { get; set; }
        public decimal Balance { get; set; }
        public int? AffiliateUserId { get; set; }
        public AppUser AffiliateUser { get; set; }
        public string FullName { get; set; }
        public IEnumerable<AffiliatePayoff> AssociatedDestinationAffiliatePayoffs { get; set; }
        public IEnumerable<AffiliatePayoff> AssociatedSourceAffiliatePayoffs { get; set; }
    }
}