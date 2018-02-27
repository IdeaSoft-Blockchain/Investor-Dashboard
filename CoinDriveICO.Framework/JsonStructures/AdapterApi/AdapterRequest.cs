namespace CoinDriveICO.Framework.JsonStructures.AdapterApi
{
    public class AdapterRequest
    {
        public AdapterRequest(string adapterName)
        {
            Adapter = adapterName;
        }
        
        public string Adapter { get; set; }
    }
}