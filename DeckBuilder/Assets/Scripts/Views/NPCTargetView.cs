using UnityEngine;

public class NPCTargetView : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private HelpBoxesUI _helpBoxUI;
    
    private NPCView _owner;
    private NPCTargetTypes _targetType = NPCTargetTypes.NONE;

    public void SetUp(NPCView owner)
    {
        _owner = owner;
        SetTargetPreview(_targetType);
    }

    private void OnMouseEnter()
    {
        if(_spriteRenderer.sprite != null)
        {
            _helpBoxUI.AddHelpBoxFromText("Target Intent", GetHelpBoxText());
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

    public void SetTargetPreview(NPCTargetTypes targetType)
    {
        _targetType = targetType;
        Sprite sprite = EnemySystem.Instance.GetEnemyTargetSymbol(targetType);
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
        string text = _owner.Data.name + " intends to " + EnemySystem.Instance.GetEnemyTargetDescription(_targetType) + ".";
        return text;
    }
}
