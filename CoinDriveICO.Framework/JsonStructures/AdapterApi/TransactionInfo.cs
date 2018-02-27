namespace CoinDriveICO.Framework.JsonStructures.AdapterApi
{
    public class TransactionInfo
    {
        public string TxKey { get; set; } // Уникальный ключ транзакции-пополнения
        public string Symbol { get; set; } // Имя адаптера
        public long Time { get; set; } // время транзакции в UnixTime
        public string Address { get; set; } // Адрес на который поступили средства
        public string From { get; set; } // Адрес отправителя
        public string Memo { get; set; } // 
        public decimal Amount { get; set; } // Сумма
        public long Confirmations { get; set; } // Кол-во подтверждений сетью
        public int UserId { get; set; } // 
        public string TxHash { get; set; } // Транзакция
    }
}