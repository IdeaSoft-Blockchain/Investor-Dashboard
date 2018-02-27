using System;

namespace CoinDriveICO.DataLayer.Model.Base
{
    public interface IBaseEntity
    {
        DateTime CreateDate { get; set; }
        DateTime UpdateDate { get; set; }
        string CreateUser { get; set; }
        string UpdateUser { get; set; }
    }
}