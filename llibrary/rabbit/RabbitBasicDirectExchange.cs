namespace llibrary.rabbit;

public class RabbitBasicDirectExchange : RabbitBasicObject
{
    public abstract class Publisher
    {
        private readonly string _queue;

        protected Publisher(string queue)
        {
            _queue = queue;
        }

        public abstract byte[] ProcessMsg(string msg);
    }

    public class Consumer { }

    public RabbitBasicDirectExchange(string host, string username, string password, int port = 5672)
        : base(host, username, password, port) { }
}
