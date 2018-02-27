using System;
using System.Collections.Generic;

namespace CoinDriveICO.Framework.JsonStructures.AdapterApi
{
    public class ResponseInfo
    {
        public bool IsSuccess { get; set; } // Успешно ли выполнен запрос
        public TimeSpan ServerTime { get; set; } // Время обработки запроса сервером
        //public IEnumerable<OperationError> Errors { get; set; } // Список ошибок
    }
}