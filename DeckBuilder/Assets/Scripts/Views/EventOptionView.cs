    using UnityEngine;
using XNode;

public class EventOptionView : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text _text;
    [SerializeField] private SpriteRenderer _button;

    private Node _node;
    private bool _available;

    public void SetUp(EventOption option)
    {
        if (option == null)
        {
            gameObject.SetActive(false);
        }
        else
        {
            _text.text = option.Text;
            _node = option.NextNode;
            _available = option.Available;
        }
    }

    private void OnMouseDown()
    {
        if(_available)
            EventSystem.Instance.EnterNode(_node);
    }

    public void SetUnavailable()
    {
        _available = false;
    }
}
