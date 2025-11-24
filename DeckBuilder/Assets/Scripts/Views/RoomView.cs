using SerializeReferenceEditor;
using UnityEngine;

public class RoomView : MonoBehaviour
{
    [field:SerializeReference, SR] public Room Room { get; private set; }

    private void OnMouseDown()
    {
        RunSystem.Instance.RunData.SetRoom(Room);
        MapSystem.Instance.EnterCombat();
    }
}
