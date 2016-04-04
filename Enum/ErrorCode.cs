using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EasyOa.Common
{
    public class ErrorCode
    {
        public enum General
        {
            /// <summary>
            /// 成功
            /// </summary>
            success = 0,

            /// <summary>
            /// 记录不存在
            /// </summary>
            record_not_exist = 100,

            /// <summary>
            /// 参数不合法错误，主要针对null值
            /// </summary>
            invalid_params = 101,

            /// <summary>
            /// 参数验证失败，主要正对ModelState验证结果
            /// </summary>
            params_valid_fault = 102,
            /// <summary>
            /// url不存在错误
            /// </summary>
            uri_not_found = 103,

            /// <summary>
            /// 权限错误
            /// </summary>
            error_permission = 104,
        }

        public enum System
        {
            server_exception = -1000,
            unknow_error = -1001,
        }
    }
}