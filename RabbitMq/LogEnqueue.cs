using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using RabbitMQ.Client;

namespace EasyOa.Common
{
    /// <summary>
    /// 日志队列,适合各个项目
    /// </summary>
    public class LogEnqueue : LogQueueBase
    {
        private static IConnection connection;   //保持一个连接
        private static List<IModel> channels = new List<IModel>();  //保存取频道集合
        private static int channels_num = 3;  //频道个数
        /// <summary>
        /// 队列初始化
        /// </summary>
        static LogEnqueue()
        {
            connection = RabbitConnection.GetConnection();
            for (int i = 1; i <= channels_num; i++)
            {
                IModel channel = ChannelInit(connection);
                channels.Add(channel);
            }
        }
        /// <summary>
        /// 入队操作
        /// </summary>
        /// <param name="data">object数据</param>
        /// <param name="persistent">持久化</param>
        public static void Enqueue(object data, bool persistent = false)
        {
            byte[] buffer = BinarySerializerHelper.ObjectToByteArray(data);
            Enqueue(buffer, persistent);
        }
        /// <summary>
        /// 入队操作
        /// </summary>
        /// <param name="bytes">byte[]数据</param>
        /// <param name="persistent"></param>
        public static void Enqueue(byte[] bytes, bool persistent = false)
        {
            Enqueue(bytes, null, persistent);
        }
        /// <summary>
        /// 入队操作
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="property"></param>
        /// <param name="persistent"></param>
        public static void Enqueue(byte[] bytes, IBasicProperties property, bool persistent = false)
        {
            IModel channel = channels[new Random().Next(0, channels_num)];  //随机选取channel发送数据
            if (property == null) property = channel.CreateBasicProperties();  //额外的属性,添加消息头的
            property.SetPersistent(persistent);  //消息持久化
            channel.BasicPublish(exchange_name, route_key, property, bytes);
        }
    }
}
