using Bfar.XCutting.Abstractions.Models;

namespace Bfar.XCutting.Abstractions.ApplicationServices
{
    public  interface IAccessControlService
    {
        public string[]? GetRoles(int userId, byte[] userIP,int appId);
        public string[]? GetPermittedCIDR(int userId, byte[] userIP,int appId);
        public DateTime[]? GetPermittedTimes(int userId, byte[] userIP,int appId);
        public NameValueModel[]? GetPolicies(int userId, byte[] userIP,int appId,int resourceId);
        public NameValueModel[]? GetPermissions(int userId, byte[] userIP,int appId,int resourceId);
        public NameValueModel[]? GetAttributes(int userId, byte[] userIP,int appId,int resourceId);
        public NameValueModel[]? GetResources(int userId, byte[] userIP, int appId);
        public bool BanUser(int userId, byte[] userIP);
        public bool UnBanUser(int userId, byte[] userIP);
        public bool LockUser(int userId, byte[] userIP);
        public bool UnLockUser(int userId, byte[] userIP);
        public bool GrantRole(int userId, byte[] userIP, int appId,int roleId);
        public bool GrantAttributes(int userId, byte[] userIP, int appId,int[] attributes);
        public bool RevokeRole(int userId, byte[] userIP, int appId, int roleId);
        public bool RevokeAttributes(int userId, byte[] userIP, int appId, int[] attributes);

    }
}
