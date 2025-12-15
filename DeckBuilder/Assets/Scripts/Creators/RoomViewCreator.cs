using AYellowpaper.SerializedCollections;
using UnityEngine;

public class RoomViewCreator : Singleton<RoomViewCreator>
{
    [SerializeField] private RoomView _roomViewPrefab;

    [SerializeField, SerializedDictionary("Room Type", "Sprite")]
    private SerializedDictionary<RoomType, Sprite> _roomViewImages;
    public RoomView CreateRoomView(Room room, Transform parent)
    {
        RoomView roomView = Instantiate(_roomViewPrefab, parent);

        roomView.transform.localPosition = RoomView.GetPosition(room.Level, room.Row);

        roomView.SetUp(_roomViewImages[room.RoomType], room);

        return roomView;
    }
}
