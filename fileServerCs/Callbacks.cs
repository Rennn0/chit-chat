using System.Text;
using database.entities;
using llibrary.Logging;
using RabbitMQ.Client.Events;

namespace fileServerCs;

public class Callbacks
{
    public static Task SaveFileAsync(object sender, BasicDeliverEventArgs @event)
    {
        // TODO
        string message = Encoding.UTF8.GetString(@event.Body.ToArray());
        string[] zipRoomIds = message.Split(separator: ['n'], count: 2);

        Diagnostics.LOG_INFO(message);

        Attachment attachment = new Attachment() { };

        return Task.CompletedTask;
    }
}
