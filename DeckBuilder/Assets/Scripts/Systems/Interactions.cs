using UnityEngine;

public class Interactions : Singleton<Interactions>
{
    public bool playerIsDragging { get; set; } = false;
    public bool PlayerCanInteract()
    {
        if (!ActionSystem.Instance.IsPerforming && !CardCollectionSystem.Instance.Opened && !MatchEndSystem.Instance.GameOver) return true;
        else return false;
    }
    public bool PlayerCanHover()
    {
        bool canHover = true;

        if(CardCollectionSystem.Instance != null)
            canHover = canHover && !CardCollectionSystem.Instance.Opened;
        if(MatchEndSystem.Instance != null)
            canHover = canHover && !MatchEndSystem.Instance.GameOver;

        if (playerIsDragging || !canHover) return false;
        else return true;
    }
}
