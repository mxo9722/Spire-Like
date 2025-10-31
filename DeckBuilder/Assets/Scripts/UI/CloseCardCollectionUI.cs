using UnityEngine;

public class CloseCardCollectionUI : MonoBehaviour
{
    public void OnClicked()
    {
        CardCollectionSystem.Instance.Close();
    }
}
