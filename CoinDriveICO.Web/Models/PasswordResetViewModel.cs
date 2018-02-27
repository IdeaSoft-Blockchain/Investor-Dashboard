namespace CoinDriveICO.Web.Models
{
    public class PasswordResetViewModel
    {
        public int UserId { get; set; }
        public string Token { get; set; }
        public string NewPassword { get; set; }
    }
}