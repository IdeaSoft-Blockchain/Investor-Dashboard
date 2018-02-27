namespace CoinDriveICO.Framework.JsonStructures.AdapterApi
{
    public class Response<TSpecificResponse>
    {
        public ResponseInfo Info { get; set; }
        public TSpecificResponse Value { get; set; }
    }
}