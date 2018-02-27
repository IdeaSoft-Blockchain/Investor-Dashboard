namespace CoinDriveICO.Framework.JsonStructures.AdapterApi
{
    public class AddressInfo
    {
        public int UserId { get; set; }
        public string Tag { get; set; }
        public string Symbol { get; set; }
        public string Address { get; set; }
        public string Memo { get; set; }
        public bool IsNew { get; set; } // Флаг того что был создан новый адрес. Если false - то значит вернулся 1 из ранее сгенерированных.
    }
}