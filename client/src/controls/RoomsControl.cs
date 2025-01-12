using System.Text;
using client.bindings;
using client.extensions;
using client.forms;
using client.globals;
using client.src.globals;
using client.src.rabbit;
using Grpc.Net.Client;
using gRpcProtos;
using LLibrary.Guards;
using LLibrary.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client.Events;
using RoomTransferObject = llibrary.SharedObjects.Room.RoomTransferObject;

namespace client.controls
{
    public partial class RoomsControl : UserControl
    {
        public RoomsControl()
        {
            InitializeComponent();
            this.Load += RoomsControl_Loaded;

            RoomDataGrid.EditMode = DataGridViewEditMode.EditProgrammatically;
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
                RoomConsumer consumer = await RabbitConsumerFactory.GetRoomConsumerAsync();
                consumer.AttachCallback(Rooms_Consumer_ReceivedAsync);
            }
            catch (Exception e)
            {
                string msg = JsonConvert.SerializeObject(e);

                await File.WriteAllTextAsync(LocalSettings.Default["DumpFile"], msg);
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
                InitRabbitConnection().Wait();
            }
            catch (Exception exception)
            {
                File.WriteAllText(
                    LocalSettings.Default["DumpFile"] + DateTime.Now,
                    exception.Message
                );
            }
        }

        private async void joinToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                SettingsConsumer consumer = await RabbitConsumerFactory.GetDirectConsumerAsync();

                ReadOnlyMemory<byte> rom = Encoding.UTF8.GetBytes("Luka").AsMemory();

                await consumer.DirectMessageAsync(
                    rom,
                    (s, @event) =>
                    {
                        MessageBox.Show(Encoding.UTF8.GetString(@event.Body.ToArray()));
                        return Task.CompletedTask;
                    }
                );
            }
            catch (Exception ex)
            {
                Diagnostics.LOG_ERROR(ex.Message);
            }
        }

        private void RoomDataGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (sender is not DataGridView dataGrid)
                return;

            if (dataGrid.Rows[e.RowIndex].DataBoundItem is not RoomsControlBinding binding)
                return;

            ChatForm cf = new ChatForm(binding.G_roomId, LocalSettings.Default["Token"]);
            cf.Show();
        }
    }
}
