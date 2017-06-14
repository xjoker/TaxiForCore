using System.Security.Principal;

namespace TaxiForCore.SystemHelper
{
    public static class CheckRunAs
    {
        private static WindowsPrincipal principal = new WindowsPrincipal(WindowsIdentity.GetCurrent());

        /// <summary>
        /// 检测程序当前是否以管理员级别启动
        /// </summary>
        /// <returns></returns>
        public static bool IsRunAsAdmin()
        {
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        /// <summary>
        /// 获取当前启动的用户名称
        /// </summary>
        /// <returns></returns>
        public static string RunAsUser()
        {
            return principal.Identity.Name;
        }

    }
}
