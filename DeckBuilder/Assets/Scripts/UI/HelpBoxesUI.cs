using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelpBoxesUI : MonoBehaviour
{

    [SerializeField] private Transform _wrapper;
    [SerializeField] private HelpBoxUI _helpBoxPrefab;
    private List<HelpBoxUI> _helpBoxes = new();

    private void OnEnable()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
    }

    public void AddHelpBoxFromText(string title,string description)
    {
        HelpBoxUI helpBoxUI = Instantiate(_helpBoxPrefab, _wrapper);
        helpBoxUI.SetUpFromText(title,description);
        _helpBoxes.Add(helpBoxUI);
    }
    public void AddHelpBoxFromKeyWord(string keyWord)
    {
        HelpBoxUI helpBoxUI = Instantiate(_helpBoxPrefab,_wrapper);
        helpBoxUI.SetUpFromKeyWord(keyWord);
        _helpBoxes.Add(helpBoxUI);
    }
    
    public void AddHelpBoxFromStatusEffect(StatusEffectType statusEffectType, int stacks)
    {
        HelpBoxUI helpBoxUI = Instantiate(_helpBoxPrefab,_wrapper);
        helpBoxUI.SetUpFromStatusEffect(statusEffectType, stacks);
        _helpBoxes.Add(helpBoxUI);
    }

    public void Hide()
    {
        foreach(HelpBoxUI helpBox in _helpBoxes)
        {
            Destroy(helpBox.gameObject);
        }

        _helpBoxes.Clear();
    }
}
