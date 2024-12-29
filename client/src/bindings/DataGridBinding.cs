namespace client.bindings;

public partial class DataGridBinding : BindingBase
{
    private List<RoomsControlBinding> _rooms = [];

    public List<RoomsControlBinding> Rooms
    {
        get => _rooms;
        set => SetField(ref _rooms, value);
    }

    public void Add(RoomsControlBinding room)
    {
        _rooms.Add(room);
        SetField(ref _rooms, _rooms, nameof(Rooms));
    }
}
