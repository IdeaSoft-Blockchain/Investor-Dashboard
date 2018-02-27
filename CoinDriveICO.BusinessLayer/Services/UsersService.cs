using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoinDriveICO.DataLayer.Model;
using CoinDriveICO.DataLayer.Repositories.Interfaces;
using CoinDriveICO.Framework.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace CoinDriveICO.BusinessLayer.Services
{
    public interface IUsersService
    {
        /// <summary>
        /// Registers new user through UserManager instance asynchronously
        /// </summary>
        /// <param name="email">New user email</param>
        /// <param name="username">New user username</param>
        /// <param name="password">New user password</param>
        /// <param name="affiliatorId">Affiliator of new user</param>
        /// <exception cref="UserManagementException">User with same email or username already exists</exception>
        /// <exception cref="UserManagementException">Registration process went wrong (exception message contains <see cref="IdentityError"/> list)</exception>
        /// <returns>New user model</returns>
        Task<AppUser> RegisterUserAsync(string email, string fullname, string username, string password, int? affiliatorId = null);

        /// <summary>
        /// Gets the user by identifier.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        Task<AppUser> GetUserById(int userId);

        /// <summary>
        /// Gets the name of the user by user.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        Task<AppUser> GetUserByUserName(string userName);

        /// <summary>
        /// Gets the user by email.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns></returns>
        Task<AppUser> GetUserByEmail(string email);

        /// <summary>
        /// Checks the user signature.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        Task<AppUser> CheckUserSignature(string userName, string password);

        /// <summary>
        /// Checks the password equality.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        Task<bool> CheckPasswordEquality(AppUser user, string password);

        /// <summary>
        /// Generates confirmation token for user
        /// </summary>
        /// <param name="user">User whom confirmation token will be generated</param>
        /// <returns></returns>
        Task<string> GenerateConfirmationToken(AppUser user);

        /// <summary>
        /// Sets user as confirmed if <paramref name="confirmationToken"/> is valid 
        /// for user with specified <paramref name="userId"/>
        /// </summary>
        /// <param name="userId">Id of user</param>
        /// <param name="confirmationToken">Email confirmation token</param>
        /// <returns></returns>
        Task<bool> SetUserAsConfirmedAsync(int userId, string confirmationToken);

        //TODO: Documentate
        Task<bool> IncreaseUserBalance(int userId, decimal increaseValue);
        Task<AppUser> SetAffiliatorToUser(AppUser user, int affiliatorId);
        Task<AppUser> GetAffiliatorOfUser(int userId);
        Task<IEnumerable<AppUser>> GetUserAffiliations(int userId);
        Task<decimal> GetOverallBalance();
        Task<string> GeneratePasswordResetToken(AppUser user);
        Task<bool> ResetUsersPassword(int userId, string passwordResetToken, string newPassword);
        Task<bool> ChangePassword(int userId, string oldPassword, string newPassword);
    }

    public class UsersService : IUsersService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IUsersRepository _usersRepository;
        private readonly IEmailService _emailService;

        public UsersService(IUsersRepository usersRepository, 
            UserManager<AppUser> userManager, 
            RoleManager<AppRole> roleManager,
            IEmailService emailService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _usersRepository = usersRepository;
            _emailService = emailService;
        }

        /// <inheritdoc/>
        public async Task<AppUser> RegisterUserAsync(string email, string fullname, string username, string password, int? affiliatorId = null)
        {
            var isUserExists = await _usersRepository.AnyAsync(x => x.Email == email || x.UserName == username);
            if (isUserExists)
            {
                throw new UserManagementException("User already exists");
            }
            var affiliatorExistance = affiliatorId.HasValue && (await GetUserById(affiliatorId.Value)) != null;
            var newUser = new AppUser
            {
                Email = email,
                UserName = username,
                FullName = fullname,
                AffiliateUserId = affiliatorExistance ? affiliatorId : null
            };
            var result = await _userManager.CreateAsync(newUser, password);
            if (result.Succeeded)
            {
                newUser = await GetUserByUserName(username);
                return newUser;
            }
            throw new UserManagementException(String.Join(',',result.Errors));
        }

        /// <inheritdoc/>
        public async Task<string> GenerateConfirmationToken(AppUser user)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            //var tokenBytes = Encoding.UTF8.GetBytes(token);
            //var codeEncoded = WebEncoders.Base64UrlEncode(tokenBytes);
            //return codeEncoded;
            return token;
        }

        public async Task<string> GeneratePasswordResetToken(AppUser user)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            return token;
        }

        /// <summary>
        /// Gets users by <paramref name="propertyName"/> and its  <paramref name="propertyValue"/>
        /// </summary>
        /// <param name="propertyValue">Property value</param>
        /// <param name="propertyName">AppUser property name</param>
        /// <returns>Users which has matching property value</returns>
        private IQueryable<AppUser> GetByProperty(object propertyValue, string propertyName)
        {
            var property = typeof(AppUser).GetProperty(propertyName);
            Func<AppUser, object> getPropertyValue =
                (x => Convert.ChangeType(property.GetValue(x), property.PropertyType));
            return _usersRepository.Where(x => getPropertyValue(x).Equals(propertyValue));
        }

        /// <inheritdoc/>
        public async Task<AppUser> CheckUserSignature(string userName, string password)
        {
            var user = await GetUserByUserName(userName);
            var resultFlag = user != null && await _userManager.CheckPasswordAsync(user, password);
            return resultFlag ? user : null;
        }

        /// <inheritdoc/>
        public async Task<bool> CheckPasswordEquality(AppUser user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }

        /// <summary>
        /// Gets one user by property name and value
        /// (Wraps the logic of getting one user through reflection)
        /// </summary>
        /// <param name="propertyValue">The property value.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        private async Task<AppUser> SingleUserGetterWrapper(object propertyValue, string propertyName)
        {
            var users = GetByProperty(propertyValue, propertyName);
            return await users.SingleOrDefaultAsync();
        }

        /// <inheritdoc/>
        public async Task<AppUser> GetUserById(int userId) =>
            await SingleUserGetterWrapper(userId, nameof(AppUser.Id));

        /// <inheritdoc/>
        public async Task<AppUser> GetUserByUserName(string userName) =>
            await SingleUserGetterWrapper(userName, nameof(AppUser.UserName));

        /// <inheritdoc/>
        public async Task<AppUser> GetUserByEmail(string email) =>
            await SingleUserGetterWrapper(email, nameof(AppUser.Email));

        /// <inheritdoc/>
        public async Task<bool> SetUserAsConfirmedAsync(int userId, string confirmationToken)
        {
            //var codeDecodedBytes = WebEncoders.Base64UrlDecode(confirmationToken);
            //var codeDecoded = Encoding.UTF8.GetString(codeDecodedBytes);
            var user = await _userManager.FindByIdAsync(userId.ToString());
            var result = (await _userManager.ConfirmEmailAsync(user, confirmationToken));
            return result.Succeeded;
        }

        public async Task<bool> ResetUsersPassword(int userId, string passwordResetToken, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            var result = await _userManager.ResetPasswordAsync(user, passwordResetToken, newPassword);
            return result.Succeeded;
        }

        public async Task<bool> ChangePassword(int userId, string oldPassword, string newPassword)
        {
            var user = await GetUserById(userId);
            var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
            return result.Succeeded;
        }

        public async Task<bool> IncreaseUserBalance(int userId, decimal increaseValue)
        {
            try
            {
                var user = await GetUserById(userId);
                user.Balance += increaseValue;
                user = await _usersRepository.UpdateAsync(user);
                return true;
            }
            catch (Exception ex)
            {
                throw new UserManagementException("Unable to update users balance", ex);
            }
        }

        public async Task<AppUser> SetAffiliatorToUser(AppUser user, int affiliatorId)
        {
            user.AffiliateUserId = affiliatorId;
            user = await _usersRepository.UpdateAsync(user);
            return user;
        }

        public async Task<AppUser> GetAffiliatorOfUser(int userId)
        {
            var user = await GetUserById(userId);
            return user.AffiliateUserId.HasValue ? await GetUserById(user.AffiliateUserId.Value) : null;
        }

        public async Task<IEnumerable<AppUser>> GetUserAffiliations(int userId)
        {
            var users = await _usersRepository.GetAllAsync(x => x.Where(y => y.AffiliateUserId == userId));
            return users;
        }

        public async Task<decimal> GetOverallBalance()
        {
            var sum = 0m;
            var users = await _usersRepository.GetAllAsync(false);
            foreach (var appUser in users)
            {
                sum += appUser.Balance;
            }
            return sum;
        }
    }
}