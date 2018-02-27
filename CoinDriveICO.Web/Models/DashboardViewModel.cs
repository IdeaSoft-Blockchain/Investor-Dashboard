namespace CoinDriveICO.Web.Models
{
    public class DashboardViewModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Balance { get; set; }
        public string TokenMultiplier { get; set; }
        public string EthToTokenRate { get; set; }
        public string BtcToTokenRate { get; set; }
        public string TokenToBtcRate { get; set; }
        public string TokenToEthRate { get; set; }
        public string OverallBalance { get; set; }
        public string MaxGoal { get; set; }
        public int PercentsGot { get; set; }
    }
}