﻿namespace Wheel.Const
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
        public const string RoleNotExist = "40011";
        #endregion
    }
}
