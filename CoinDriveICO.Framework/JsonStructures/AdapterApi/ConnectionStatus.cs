namespace CoinDriveICO.Framework.JsonStructures.AdapterApi
{
    public enum ConnectionStatus
    {
        NotConnected = 0, // Соединение не устанавливалось    
        Connected = 2, // Соединение установлено
        LostConnect = 3, // Соединение было потеряно (разорвано)  	
        Disconnected = 5 // Соединение не установлено (закрыто)
    }
}