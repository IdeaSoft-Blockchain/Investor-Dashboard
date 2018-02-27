using CoinDriveICO.DataLayer.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace CoinDriveICO.DataLayer.Model
{
    public class AppUserClaim: IdentityUserClaim<int> { }
    public class AppUserRole : IdentityUserRole<int> { }
    public class AppUserLogin : IdentityUserLogin<int> { }
    public class AppRoleClaim : IdentityRoleClaim<int> { }
    public class AppUserToken : IdentityUserToken<int> { }
    public class AppRole : IdentityRole<int> { }
}