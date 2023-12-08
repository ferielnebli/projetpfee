using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Shared.Cache
{
    public static class RedisCacheKey
    {
        #region AccessManagement

        #region User
        public static readonly string UserCacheKey = "Users";
        public static readonly string UserTokenCacheKey = "UserTokens";
        public static readonly string UserOTPCacheKey = "UserOTPs";
        #endregion

        #region Role
        public static readonly string RoleCacheKey = "Roles";
        public static readonly string PermissionCacheKey = "Permissions";
        #endregion

        #region Language 
        public static readonly string LanguageCacheKey = "Languages";
        #endregion

        #region Company 
        public static readonly string CompanyCacheKey = "Companies";
        #endregion

        #region Employee
        public static readonly string EmployeeCacheKey = "Employees";

        #endregion

        #region Address
        public static readonly string AddressCacheKey = "Addresses";
        public static readonly string MacAddressCacheKey = "MacAddresses";
        #endregion

        #region Notification
        public static readonly string SubscriberDeviceCacheKey = "SubscriberDevices";
        #endregion

        #endregion

        #region PaymentManagement

        #region Paiement Key
        public static readonly string WalletCacheKey = "Wallets";
        #endregion

        #endregion

    }
}
