using System.Text;
using System.Windows.Forms;
using client.bindings;
using client.globals;
using client.Properties;
using Grpc.Net.Client;
using gRpcProtos;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RoomTransferObject = llibrary.SharedObjects.Room.RoomTransferObject;
using Timer = System.Threading.Timer;

namespace client.controls
{
    public partial class RoomsControl : UserControl
    {
        private readonly ConnectionFactory _factory;
        private IConnection? _connection;

        public RoomsControl()
        {
            InitializeComponent();
            this.Load += RoomsControl_Loaded;

            _factory = new ConnectionFactory
            {
                HostName = Resources.RabbitHost,
                Port = int.Parse(Resources.RabbitPort),
            };
        }

        private async void RoomsControl_Loaded(object? sender, EventArgs e)
        {
            //new Timer(_ => KeepRoomsUpdated(), null, 1000, -1);
            new Thread(KeepRoomsUpdated).Start();
            roomsControlBindingBindingSource.RemoveAt(0);

            GrpcChannel channel = Globals.GetGrpcChannel();
            RoomExchangeService.RoomExchangeServiceClient client =
                new RoomExchangeService.RoomExchangeServiceClient(channel);
            ListAvailableRoomsResponse? response = await client.ListAvailableRoomsAsync(
                new ListAvailableRoomsRequest()
            );
            foreach (gRpcProtos.RoomTransferObject roomTransferObject in response.Rooms)
            {
                roomsControlBindingBindingSource.Add(
                    new RoomsControlBinding()
                    {
                        G_description = roomTransferObject.Description,
                        G_hostUserId = roomTransferObject.HostUserId,
                        G_roomId = roomTransferObject.RoomId,
                        G_roomName = roomTransferObject.Name,
                        G_participants = roomTransferObject.Participants,
                    }
                );
            }
        }

        private async Task InitRabbitConnection()
        {
            try
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
            catch (Exception e)
            {
                string msg =
                    JsonConvert.SerializeObject(e)
                    + $"{Environment.NewLine}{Resources.RabbitPort} - {Resources.RabbitHost}";

                File.WriteAllText("threaddump.txt", msg);
            }
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
            }
            catch (Exception exception)
            {
                File.WriteAllText("dump.txt", exception.Message);
            }
        }

        private void joinToolStripMenuItem_Click(object sender, EventArgs e) { }

        private void RoomDataGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView? dataGrid = sender as DataGridView;
            var cell = dataGrid.Rows[e.RowIndex];
        }
    }
}


// TODO klientis tokenis crypt
