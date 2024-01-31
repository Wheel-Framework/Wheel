namespace Wheel.Administrator
{
    /// <summary>
    /// 错误码
    /// 约定5位数字字符串
    /// 4XXXX：客户端错误信息
    /// 5XXXX: 服务端错误信息
    /// </summary>
    public class ErrorCode
    {
        #region 5XXXX
        public const string InternalError = "50000";
        #endregion
        #region 4XXXX
        public const string CreateUserError = "40003";
        public const string UserNotExist = "40004";
        public const string RoleNotExist = "40011";
        public const string RoleExist = "40012";
        public const string CreateRoleError = "40013";

        public const string LoginError = "41001";


        public const string FileNotExist = "40030";
        public const string FileDownloadFail = "40031";
        #endregion
    }
}
