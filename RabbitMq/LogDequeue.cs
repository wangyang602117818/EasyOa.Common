using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace EasyOa.Common
{
    public class LogDequeue : LogQueueBase
    {
        private static IConnection connection;
        private static IModel channel;
        private static QueueingBasicConsumer consumer;
        static LogDequeue()
        {
            connection = RabbitConnection.GetConnection();
            channel = ChannelInit(connection);
            channel.BasicQos(0, 50, false);   //qos
            consumer = new QueueingBasicConsumer(channel); //消费者
            channel.BasicConsume(queue_name, false, consumer);
        }
        /// <summary>
        /// 出队，
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>code=-2:消息超过重试次数;code=-1:解析出错</returns>
        public static T TryDequeue<T>(out int code) where T : class
        {
            code = 0;
            BasicDeliverEventArgs ea = consumer.Queue.Dequeue();  //
            byte[] body = ea.Body;
            int retry = 5;  //重试次数
            if (ea.BasicProperties.Headers != null && ea.BasicProperties.Headers.ContainsKey("retry")) retry = (int)ea.BasicProperties.Headers["retry"];
            if (retry <= 0)
            {
                code = -2;  //消息超过重试次数
                channel.BasicAck(ea.DeliveryTag, false);
                return null;
            }
            try
            {
                T result = BinarySerializerHelper.ByteArrayToObject<T>(body);
                channel.BasicAck(ea.DeliveryTag, false);
                return result;
            }
            catch (Exception ex) //重试机制
            {
                code = -1;  //解析出错
                if (ea.BasicProperties.Headers == null) ea.BasicProperties.Headers = new Dictionary<string, object>() { { "retry", retry } };
                if (ea.BasicProperties.Headers != null && !ea.BasicProperties.Headers.ContainsKey("retry")) ea.BasicProperties.Headers.Add("retry", retry);
                ea.BasicProperties.Headers["retry"] = retry - 1;
                channel.BasicAck(ea.DeliveryTag, false);
                channel.BasicPublish(exchange_name, route_key, ea.BasicProperties, body); //不调用现成的方法，是因为会产生新的tcp连接
                return null;
            }

        }
    }
}
