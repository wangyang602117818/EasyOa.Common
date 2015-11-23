using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyOa.Common.Alipay.DirectPay
{
    public class AliRequestModel : AliModelBase
    {
        public string out_trade_no { get; set; }  //商户唯一订单编号
        public string subject { get; set; }    //商品的名称,max(128)
        public string payment_type { get; set; }  //支付类型 1：商品购买，4：捐赠，47：电子卡券
        public decimal total_fee { get; set; }    //单价，元，精确到小数点后2位

        //========================三选一
        public string seller_id { get; set; }   //卖家支付宝用户号 2088.....
        public string seller_email { get; set; }  //卖家支付宝账号，手机号或邮箱
        public string seller_account_name { get; set; }  //卖家别名支付宝账号
        //===================

        public string buyer_id { get; set; }   //可空 买家支付宝账号，2088....
        public string buyer_email { get; set; }  //可空 买家支付宝账号，手机或邮箱
        public string buyer_account_name { get; set; }  //可空 买家别名支付宝账号

        public decimal? price { get; set; }   //可空 单价，元，精确到小数点后2位
        public int? quantity { get; set; }   //可空 数量
        public string body { get; set; }    //可空 商品描述
        public string show_url { get; set; }  //可空 收银台页面上，商品展示的超链接。
        public string paymethod { get; set; }  //可空 creditPay（信用支付）directPay（余额支付,默认）
        public string enable_paymethod { get; set; }  //可空，用于控制收银台支付渠道显示 多渠道用^分开， 枚举 directPay：账户余额，cartoon：卡通，bankPay：网银，cash：现金，creditCardExpress：信用卡快捷，debitCardExpress：借记卡快捷，coupon：红包，point：积分，voucher：购物卷，
        public string need_ctu_check { get; set; }  //可空 风险稽查系统，Y/N
        public string anti_phishing_key { get; set; }  //可空 防钓鱼时间戳
        public string exter_invoke_ip { get; set; }  //可空 ，ip
        public string extra_common_param { get; set; }  //可空 公共回传参数
        public string extend_param { get; set; }   //可空 公共业务扩展
        public string it_b_pay { get; set; }    //可空 超时时间，1d，1h，1m，1c：当天
        public string default_login { get; set; }  //可空 自动登录标示
        public string product_type { get; set; }  //可空 填空即可
        public string token { get; set; }   //可空 快捷登录授权令牌
        public string sign_id_ext { get; set; }  //可空 商户买家签约号
        public string sign_name_ext { get; set; }  //可空 商户买家签约名
        public string qr_pay_mode { get; set; } //可空 扫码
    }
}
