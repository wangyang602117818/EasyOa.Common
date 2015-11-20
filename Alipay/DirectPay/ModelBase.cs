using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyOa.Common.Alipay.DirectPay
{
    public class ModelBase
    {
        public string service { get; set; }   //接口名称
        public string partner { get; set; }   //合作方id
        public string _input_charset { get; set; }   //字符集
        public string sign_type { get; set; }   //签名类型
        public string sign { get; set; }
        public string notify_url { get; set; }  //可空  //异步通知页面
        public string return_url { get; set; }  //可空 页面调转同步通知  两者有何区别？
        public string error_notify_url { get; set; }  //可空
    }
}
