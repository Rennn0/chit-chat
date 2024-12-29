using client.bindings;
using Timer = System.Threading.Timer;

namespace client.controls
{
    public partial class RoomsControl : UserControl
    {
        public RoomsControl()
        {
            InitializeComponent();
            this.Load += RoomsControl_Loaded;
        }

        private void _timer_Tick(object? sender, EventArgs e) { }

        private async void RoomsControl_Loaded(object? sender, EventArgs e)
        {
            Timer? timer = null;
            int elapsedMilliseconds = 0;
            int stopAfterMilliseconds = 10000; // 10 seconds

            timer = new Timer(
                _ =>
                {
                    if (elapsedMilliseconds >= stopAfterMilliseconds)
                    {
                        var result = roomsControlBindingBindingSource
                            .List.Cast<RoomsControlBinding>()
                            .Where(r => r.G_hostUserId == "xxqq");
                        foreach (var r in result)
                        {
                            r.G_roomId = DateTime.UtcNow.ToString();
                        }
                        timer?.Dispose();
                        return;
                    }

                    this.Invoke(
                        () =>
                            roomsControlBindingBindingSource.Add(
                                new RoomsControlBinding()
                                {
                                    G_description = DateTime.Now.ToString(),
                                    G_hostUserId = "xxqq",
                                }
                            )
                    );

                    elapsedMilliseconds += 2000; // Increment by timer interval (2 seconds)
                },
                null,
                1000, // Initial delay
                2000 // Interval
            );
        }
    }
}
