using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyOa.Common
{
    public enum HttpMethodType
    {
        [Description("post")]
        post,
        [Description("get")]
        get,
        [Description("put")]
        put,
        [Description("delete")]
        delete,
    }
}
