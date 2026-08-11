using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StatusEffectsUI : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private StatusEffectUI _statusEffectUIPrefab;
    [SerializeField] private StatusEffectsDictionary _statusEffectsData;

    private List<StatusEffectUI> _statusEffectUIs = new();

    public CombatantView Owner { get; private set; }
    private Transform _blockPosition;

    public void SetUp(CombatantView owner, Transform blockPosition)
    {
        Owner = owner;
        _blockPosition = blockPosition;
    }

    private void OnEnable()
    {
        _canvas.worldCamera = Camera.main;
    }

    public void UpdateStatusEffectsUI(StatusEffectInfo info, int stackCount)
    {

        StatusEffectUI ui = GetExistingUI(info);

        if (stackCount == 0 && info.RemoveAtZero)
        {
            if (ui != null)
            {
                _statusEffectUIs.Remove(ui);
                Destroy(ui.gameObject);
            }
        }
        else
        {

            if (ui == null)
            {
                ui = Instantiate(_statusEffectUIPrefab, transform);
                _statusEffectUIs.Add(ui);

                switch (info.EnumKey)
                {
                    case StatusEffect.BLOCK:
                        ui.SetPositionOverride(_blockPosition.position);
                        break;
                }
            }

            Sprite sprite = info.Sprite;

            bool stackable = info.Stackable;

            int displayStackCount = stackable ? stackCount : 0;

            ui.Set(Owner, sprite, displayStackCount, stackable, info);
        }
    }

    private StatusEffectUI GetExistingUI(StatusEffectInfo info)
    {
        return _statusEffectUIs.Find(se => se.Info.Equals(info));
    }

    public bool HasStatusEffectUI(StatusEffectInfo statusEffectInfo)
    {
        return _statusEffectUIs.Any(ui => ui.Info == statusEffectInfo);
    }
}
