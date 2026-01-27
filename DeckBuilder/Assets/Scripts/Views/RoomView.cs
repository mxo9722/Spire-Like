using SerializeReferenceEditor;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomView : MonoBehaviour
{
    public int Level => Room.Level;
    public int Row => Room.Row;

    [SerializeField] private SpriteRenderer _image;
    [SerializeField] private SpriteRenderer _background;
    [SerializeField] private SpriteRenderer _crossOut;
    [SerializeField] private LineRenderer[] _lineRenderers;
    [field: SerializeField] public CircleCollider2D Collider { get; private set; }

    [field:SerializeReference, SR] public Room Room { get; private set; }

    public void SetUp(Sprite sprite, Room room)
    {
        _image.sprite = sprite;
        Room = room;

        UpdatePosition();
        _crossOut.gameObject.SetActive(Room.IsCompleted);
    }

    private void OnMouseDown()
    {
        if(!RewardSystem.Instance.RewardsUIOpened)
            RunSystem.Instance.RunData.EnterRoom(Room);
    }

    private void Update()
    {
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        transform.position = GetPosition(Room.Level, Room.Row);

        for (int i = 0; i < _lineRenderers.Length; i++)
        {
            if (i < Room.PathedRooms.Count)
            {
                Room pathRoom = Room.PathedRooms[i];
                Vector3 pathedPos = GetPosition(pathRoom.Level, pathRoom.Row);

                Vector3 direction = (transform.position - pathedPos).normalized * Collider.radius * transform.lossyScale.x;

                pathedPos += direction;

                _lineRenderers[i].SetPosition(0, transform.position - direction);
                _lineRenderers[i].SetPosition(1, pathedPos);
            }
            else
            {
                _lineRenderers[i].enabled = false;
            }
        }
    }

    public static Vector3 GetPosition(int level, int row)
    {
        //return new(level - 7, row - 3, 0);

        return MapSystem.Instance.GetRoomPosition(level, row);
    }

    public void SetColor(Color color)
    {
        _background.color = color;
    }

    public bool IsChildOf(RoomView room) => Room.IsChildOf(room.Room);
    public bool IsSelectable() => Room.IsSelectable();
}
