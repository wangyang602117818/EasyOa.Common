using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace EasyOa.Common
{
    /// <summary>
    /// 日志出队操作，适合windows服务项目
    /// </summary>
    public class LogDequeue<T> : LogQueueBase where T : class
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
        /// 执行出队操作
        /// </summary>
        /// <param name="processMessage">用于处理消息的委托</param>
        public static void DoDequeue(Func<T, bool> processMessage)
        {
            if (processMessage == null) return;
            while (true)
            {
                BasicDeliverEventArgs ea = consumer.Queue.Dequeue();   //阻塞当前线程
                byte[] body = ea.Body;
                T result = null;
                try
                {
                    result = BinarySerializerHelper.ByteArrayToObject<T>(body);
                }
                catch (Exception exception)
                {
                    DoParseError(ea);
                    continue;
                }
                //处理消息
                if (processMessage(result))
                    channel.BasicAck(ea.DeliveryTag, false);
                else
                {
                    channel.BasicAck(ea.DeliveryTag, false);
                    channel.BasicPublish(exchange_name, route_key, ea.BasicProperties, body);
                }
            }
        }
        /// <summary>
        /// 解析消息格式失败，为了清除队列中格式不正确的消息
        /// </summary>
        /// <param name="ea"></param>
        public static void DoParseError(BasicDeliverEventArgs ea)
        {
            if (ea.BasicProperties.Headers == null || !ea.BasicProperties.Headers.ContainsKey("parsedNum"))
            {
                //加消息头，标记该消息被解析的次数
                ea.BasicProperties.Headers = new Dictionary<string, object>() { { "parsedNum", 0 } };
            }
            int parsedNum = (int)ea.BasicProperties.Headers["parsedNum"] + 1;
            ea.BasicProperties.Headers["parsedNum"] = parsedNum;
            LogHelper.ErrorLog("消息解析失败，尝试次数" + parsedNum);
            channel.BasicAck(ea.DeliveryTag, false); //则从队列删除消息
            if (parsedNum < 5)
            {
                channel.BasicPublish(exchange_name, route_key, ea.BasicProperties, ea.Body); //吧消息扔到队尾
            }
        }
    }
}
