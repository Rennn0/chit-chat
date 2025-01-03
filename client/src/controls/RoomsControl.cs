﻿using System.Text;
using client.bindings;
using client.extensions;
using client.forms;
using client.globals;
using GeneratedSettings;
using generator;
using Grpc.Net.Client;
using gRpcProtos;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RoomTransferObject = llibrary.SharedObjects.Room.RoomTransferObject;

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
                HostName = TrexSettigns.RabbitHost,
                Port = int.Parse(TrexSettigns.RabbitPort),
            };
        }

        private async void RoomsControl_Loaded(object? sender, EventArgs e)
        {
            try
            {
                //new Timer(_ => KeepRoomsUpdated(), null, 1000, -1);
                new Thread(KeepRoomsUpdated).Start();
                roomsControlBindingBindingSource.RemoveAt(0);

                GrpcChannel channel = Globals.GetGrpcChannel();
                RoomExchangeService.RoomExchangeServiceClient client = new(channel);
                ListAvailableRoomsResponse? response = await client.ListAvailableRoomsAsync(
                    new ListAvailableRoomsRequest()
                );
                foreach (gRpcProtos.RoomTransferObject roomTransferObject in response.Rooms)
                {
                    roomsControlBindingBindingSource.Add(roomTransferObject.Map());
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private async Task InitRabbitConnection()
        {
            try
            {
                _connection = await _factory.CreateConnectionAsync();
                IChannel channel = await _connection.CreateChannelAsync();
                await channel.ExchangeDeclareAsync(TrexSettigns.RabbitQueue, ExchangeType.Fanout);
                QueueDeclareOk queue = await channel.QueueDeclareAsync(
                    exclusive: true,
                    durable: false,
                    autoDelete: true
                );
                await channel.QueueBindAsync(
                    queue.QueueName,
                    TrexSettigns.RabbitQueue,
                    string.Empty
                );
                AsyncEventingBasicConsumer consumer = new(channel);
                consumer.ReceivedAsync += Rooms_Consumer_ReceivedAsync;
                await channel.BasicConsumeAsync(queue.QueueName, true, consumer: consumer);
            }
            catch (Exception e)
            {
                string msg = JsonConvert.SerializeObject(e);

                await File.WriteAllTextAsync(TrexSettigns.DumpFile, msg);
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
                roomsControlBindingBindingSource.Add(rto.Map());
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
                File.WriteAllText(TrexSettigns.DumpFile + DateTime.Now, exception.Message);
            }
        }

        private void joinToolStripMenuItem_Click(object sender, EventArgs e) { }

        private void RoomDataGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (sender is not DataGridView dataGrid)
                return;

            if (dataGrid.Rows[e.RowIndex].DataBoundItem is not RoomsControlBinding binding)
                return;

            ChatForm cf = new ChatForm(
                binding.G_roomId,
                RuntimeTrexSettings.Get(TrexSettings.Token)
            );
            cf.Show();
        }
    }
}
