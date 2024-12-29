namespace client.bindings;

public partial class RoomsControlBinding : BindingBase
{
    [Observable]
    private string _roomId = string.Empty;

    [Observable]
    private string _roomName = string.Empty;

    [Observable]
    private string _hostUserId = string.Empty;

    [Observable]
    private string _description = string.Empty;

    [Observable]
    private int _participants = 0;
}
