using System;

namespace CoinDriveICO.Framework.JsonStructures.AdapterApi
{
    public class AdapterInfo
    {
        public decimal Balance { get; set; } // Последний известный баланс адаптера
        public bool SupportBananceUsed { get; set; } // Используется дополнительный баланс
        public decimal SupportBalance { get; set; } // Последний известный доп.баланс.
        public DateTime BalanceUpdated { get; set; } // Время последнего обновления баланса
        public ConnectionStatus ConnectStatus { get; set; } // Статус подключения адаптера к блокчеину
        public DateTime LastConnectAttempt { get; set; } // Время последней попытки подключения адаптера к блокчеину
        public DateTime LastSuccessConnect { get; set; } // Время последней успешной попытки подключения адаптера к блокчеину

    }
}