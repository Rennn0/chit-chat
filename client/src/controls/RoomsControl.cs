using System.Text;
using client.bindings;
using llibrary.SharedObjects.Room;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Timer = System.Threading.Timer;

namespace client.controls
{
    public partial class RoomsControl : UserControl
    {
        private ConnectionFactory _factory;
        private IConnection? _connection;

        public RoomsControl()
        {
            InitializeComponent();
            this.Load += RoomsControl_Loaded;

            _factory = new ConnectionFactory { HostName = "localhost" };
        }

        private async void RoomsControl_Loaded(object? sender, EventArgs e)
        {
            //new Timer(_ => KeepRoomsUpdated(), null, 1000, -1);
            new Thread(KeepRoomsUpdated).Start();
        }

        private async Task InitRabbitConnection()
        {
            _connection = await _factory.CreateConnectionAsync();
            IChannel channel = await _connection.CreateChannelAsync();
            await channel.ExchangeDeclareAsync("rooms", ExchangeType.Fanout);
            QueueDeclareOk queue = await channel.QueueDeclareAsync(
                exclusive: true,
                durable: false,
                autoDelete: true
            );
            await channel.QueueBindAsync(queue.QueueName, "rooms", string.Empty);
            AsyncEventingBasicConsumer consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += Rooms_Consumer_ReceivedAsync;
            await channel.BasicConsumeAsync(queue.QueueName, true, consumer: consumer);
        }

        private Task Rooms_Consumer_ReceivedAsync(object sender, BasicDeliverEventArgs @event)
        {
            byte[] body = @event.Body.ToArray();
            string message = Encoding.UTF8.GetString(body);

            RoomDataGrid.Invoke(() =>
            {
                RoomTransferObject rto =
                    JsonConvert.DeserializeObject<RoomTransferObject>(message)
                    ?? new RoomTransferObject();
                roomsControlBindingBindingSource.Add(
                    new RoomsControlBinding()
                    {
                        G_description = rto.Description,
                        G_hostUserId = rto.HostUserId,
                        G_roomId = rto.RoomId,
                        G_roomName = rto.Name,
                        G_participants = rto.Participants,
                    }
                );
            });

            return Task.CompletedTask;
        }

        private void KeepRoomsUpdated()
        {
            try
            {
                if (_connection is null || !_connection.IsOpen)
                    InitRabbitConnection().Wait();

                throw new Exception("qweqewqq");
            }
            catch (Exception exception)
            {
                File.WriteAllText("dump.txt", exception.Message);
            }
        }
    }
}


// TODO klientis tokenis crypt
