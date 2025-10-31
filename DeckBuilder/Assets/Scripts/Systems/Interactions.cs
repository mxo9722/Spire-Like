using UnityEngine;

public class Interactions : Singleton<Interactions>
{
    public bool playerIsDragging { get; set; } = false;
    public bool PlayerCanInteract()
    {
        if (!ActionSystem.Instance.IsPerforming && !CardCollectionSystem.Instance.Opened) return true;
        else return false;
    }
    public bool PlayerCanHover()
    {
        if (playerIsDragging || CardCollectionSystem.Instance.Opened) return false;
        else return true;
    }
}
