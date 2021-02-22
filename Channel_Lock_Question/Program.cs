using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Channel_Lock_Question
{

    public class MessageBody
    {
        public string Data { get; set; } = Guid.NewGuid().ToString("N");
    }
    class Program
    {

        static void Main(string[] args)
        {

            MessageServerProxy proxy = new MessageServerProxy();
            proxy.StartJob();
            for (int i = 0; i < 100; i++)
            {
                Send(proxy);
            }

            Console.WriteLine("Hi, there!");
            Console.ReadLine();
        }

#if SendVersion1
        static void Send(MessageServerProxy proxy)
        {
            Task.Run(() =>
            {
                while (true)
                {
                    proxy.SendMessaage(new MessageBody());

                }
            });
        }
#endif
#if SendVersion2
        static void Send(MessageServerProxy proxy)
        {
            Thread _tempThread = new Thread(new ThreadStart(() =>
              {
                  while (true)
                  {
                      proxy.SendMessaage(new MessageBody());

                  }
              }))
            {
                IsBackground = true
            };
            _tempThread.Start();

        }
#endif
        //#if SendVersion3
        //        static void Send(MessageServerProxy proxy)
        //        {
        //            Task.Factory.StartNew(() =>
        //            {

        //                while (true)
        //                {
        //                    proxy.SendMessaage(new MessageBody());

        //                }
        //            }, TaskCreationOptions.LongRunning);
        //        }
        //#endif
    }


    /// <summary>
    /// this is the simple code for the client.
    /// <para>The real code will complex than the code below.</para>
    /// <para>But the core work like those code.</para>
    /// </summary>
    public class MessageServerProxy
    {
        private Channel<MessageBody> _channel =
        //Channel.CreateUnbounded<MessageBody>();
        //Channel.CreateBounded<MessageBody>(new BoundedChannelOptions(int.MaxValue)
        //{
        //    SingleReader = true
        //});
        Channel.CreateUnbounded<MessageBody>(new UnboundedChannelOptions()
        {
            SingleReader = true
        });
        private Thread _jobThread;

        public MessageServerProxy()
        {
            _jobThread = new Thread(new ThreadStart(async () =>
            {
                var reader = _channel.Reader;
                while (true)
                {
                    var data = await reader.ReadAsync();
                    Console.WriteLine("The process can not reach this place.");
                }
            }))
            { IsBackground = true };
#if ClientVersion2
            _writer = _channel.Writer;
#endif
        }

#if ClientVersion1
        public void SendMessaage(MessageBody data)
        {
            _channel.Writer.WriteAsync(data);
        }
#endif

#if ClientVersion2
        ChannelWriter<MessageBody> _writer = null;
        public void SendMessaage(MessageBody data)
        {
            _writer.WriteAsync(data);
        }
#endif



        public void StartJob()
        {
            _jobThread.Start();
        }

    }
}
