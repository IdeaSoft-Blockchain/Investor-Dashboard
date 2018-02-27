using System;

namespace CoinDriveICO.DataLayer.Model.Base
{
    public class BaseEntity : IBaseEntity
    {
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string CreateUser { get; set; }
        public string UpdateUser { get; set; }
    }
}