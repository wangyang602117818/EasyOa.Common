using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyOa.Common.Properties;

namespace EasyOa.Common.Alipay.DirectPay
{
    public class AliModelBase
    {
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "Required")]
        public string service { get; set; }   //接口名称
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "Required")]
        public string partner { get; set; }   //合作方id
        public string _input_charset { get { return "utf-8"; } }   //字符集
        public string sign_type { get { return "MD5"; } }   //签名类型
        public string sign { get; set; }
        public string notify_url { get; set; }  //可空  //服务器异步通知
        public string return_url { get; set; }  //可空 页面调转同步通知
        public string error_notify_url { get; set; }  //可空
    }
}
