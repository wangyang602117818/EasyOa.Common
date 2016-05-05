using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyOa.Common
{
    /// <summary>
    /// 请求contenttype
    /// </summary>
    public enum RequestContentType
    {
        /// <summary>
        /// 适合参数以url编码方式的，post方式但是没有上传文件的
        /// </summary>
        [Description("application/x-www-form-urlencoded")]
        UrlEncoded,
        /// <summary>
        /// 适合有上传文件的
        /// </summary>
        [Description("multipart/form-data")]
        Formdata,
        /// <summary>
        /// 适合特殊情况，以xml方式传参数
        /// </summary>
        [Description("application/xml;charset=utf-8")]
        Xml,
        /// <summary>
        /// 适合特殊情况，以json方式传参数
        /// </summary>
        [Description("application/json;charset=utf-8")]
        Json,

    }
    /// <summary>
    /// 响应contenttype
    /// </summary>
    public enum ResponseContentType
    {
        /// <summary>
        /// 默认以html方式响应
        /// </summary>
        [Description("text/html;charset=utf-8")]
        Html,
        /// <summary>
        /// 以文本方式响应
        /// </summary>
        [Description("text/plain;charset=utf-8")]
        Text,
        /// <summary>
        /// 以脚本形式响应
        /// </summary>
        [Description("text/javascript")]
        Javascript,
        /// <summary>
        /// 以css形式响应
        /// </summary>
        [Description("text/css")]
        Css,
        /// <summary>
        /// 图像形式响应
        /// </summary>
        [Description("image/jpeg")]
        Jpg,
        [Description("image/gif")]
        Gif,
        [Description("image/png")]
        Png,
    }
}
