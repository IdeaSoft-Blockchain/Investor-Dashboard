namespace CoinDriveICO.Framework.JsonStructures.AdapterApi
{
    public class AddressRequest
    {
        public int UserId { get; set; } // Идентификатор пользователя
        public string Tag { get; set; } // Текстовая метка
        public bool GenerateNew { get; set; } // Флаг того что принудительно нужно генерировать адрес.
        // В случае если не установлен и у данного пользователя уже есть адреса. то возвращает 1 из них.
    }
}