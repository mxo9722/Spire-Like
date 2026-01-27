using UnityEngine;

public class NPCActionView : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private HelpBoxesUI _helpBoxUI;

    private NPCView _owner;
    private NPCActionType _actionType = NPCActionType.NONE;

    public void SetUp(NPCView owner)
    {
        _owner = owner;
        SetActionPreview(_actionType);
    }

    private void OnMouseEnter()
    {
        if (_spriteRenderer.sprite != null)
        {
            _helpBoxUI.AddHelpBoxFromText("Action Intent", GetHelpBoxText());
            TargetPreviewSystem.Instance.SetTargetPreviews(_owner, _owner.CurrentAction);
        }
    }

    private void OnMouseExit()
    {
        if (_spriteRenderer.sprite != null)
        {
            _helpBoxUI.Hide();
            TargetPreviewSystem.Instance.HideTargetPreviews(true);
        }
    }

    public void SetActionPreview(NPCActionType actionType)
    {
        _actionType = actionType;
        Sprite sprite = EnemySystem.Instance.GetEnemyActionSymbol(actionType);
        SetSprite(sprite);

        _helpBoxUI.Hide();
        TargetPreviewSystem.Instance.HideTargetPreviews(true);
    }

    private void SetSprite(Sprite sprite)
    {
        _spriteRenderer.sprite = sprite;
    }

    private string GetHelpBoxText()
    {
        string text = _owner.Data.name+" intends to " + EnemySystem.Instance.GetEnemyActionDescription(_actionType) + ".";
        return text;
    }
}
