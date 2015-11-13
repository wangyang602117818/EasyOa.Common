using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace EasyOa.Common
{
    public class LogQueueBase
    {
        public static string route_key = "sys_log";
        public static string queue_name = "sys_log_queue";
        public static string exchange_name = "sys_log_exchange";

        public static IModel ChannelInit(IConnection connection)
        {
            IModel channel = connection.CreateModel();
            channel.ExchangeDeclare(exchange_name, "direct", true);
            channel.QueueDeclare(queue_name, true, false, false, null);
            channel.QueueBind(queue_name, exchange_name, route_key);
            return channel;
        }
    }
}
