using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapView : MonoBehaviour
{
    [SerializeField] private List<RoomView> _roomViews = new();
    [SerializeField] private CircleCollider2D _innerCircle;
    [SerializeField] private CircleCollider2D _outterCircle;
    [SerializeField] private AnimationCurve _circleDistancer;
    [Header("Color coding rooms")]
    [SerializeField] private Color _irrelevantRoomColor;
    [SerializeField] private Color _futureRoomColor;
    [SerializeField] private Color _possibleRoomColor;

    private Map _map;

    public void SetUp(Map map)
    {
        _map = map;

        _innerCircle.enabled = false;
        _outterCircle.enabled = false;

        RefreshMap();
    }

    public void RefreshMap(float duration = 0)
    {

        if (_roomViews.Count > 0)
        {
            foreach(RoomView roomView in _roomViews)
            {
                Destroy(roomView.gameObject);
            }
            _roomViews.Clear();
        }

        foreach (Room room in _map.Rooms)
        {
            RoomView roomView = RoomViewCreator.Instance.CreateRoomView(room, transform);
            roomView.SetColor(_irrelevantRoomColor);
            _roomViews.Add(roomView);
        }

        List<RoomView> relevantRooms = GetRelevantRooms();

        Bounds bounds = default;

        foreach (RoomView room in relevantRooms)
        {
            Bounds roomBounds = room.Collider.bounds;
            roomBounds.center = room.transform.position;
            bounds.Encapsulate(roomBounds);

            if (room.IsSelectable())
                room.SetColor(_possibleRoomColor);
            else
                room.SetColor(_futureRoomColor);
        }

        bounds.Expand(0.5f);

        MapCameraControl.Instance.SetViewSize(bounds, duration);
    }

    public float GetMapDistance(float interpolation)
    {

        if (interpolation <= 1)
        {
            float baseDis = _innerCircle.radius;
            float spreadDis = (_outterCircle.radius - _innerCircle.radius) * _circleDistancer.Evaluate(1 - interpolation);

            return baseDis + spreadDis;
        }
        return 0;
    } 

    public List<RoomView> GetRelevantRooms()
    {
        HashSet<RoomView> rooms = new();
        RoomView lastPlayed = null;

        int level = 0;

        foreach (RoomView room in _roomViews)
        {
            if ((lastPlayed == null || lastPlayed.Room.Level < room.Level) && room.Room.IsCompleted)
            {
                lastPlayed = room;
            }
            else if (room.IsSelectable())
            {
                if(room.Level > level)
                {
                    rooms.Clear();
                    level = room.Level;
                }

                rooms.Add(room);
            }
            else if(rooms.Any(r => room.IsChildOf(r)))
            {
                rooms.Add(room);
            }
        }

        if(lastPlayed != null)
            rooms.Add(lastPlayed);

        return new(rooms);
    }
}
