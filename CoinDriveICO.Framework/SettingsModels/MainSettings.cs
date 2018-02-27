namespace CoinDriveICO.Framework.SettingsModels
{
    public class MainSettings
    {
        public decimal TokenMultiplier { get; set; }
        public decimal BtcToTokenRate { get; set; }
        public decimal EthToTokenRate { get; set; }
        public string MasterWorkerKey { get; set; }
        public decimal AffiliatorPaybackMultiplierRate { get; set; }
    }
}