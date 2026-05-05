using System.Collections.Generic;
using UnityEngine;

public class StatusEffectsUI : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private StatusEffectUI _statusEffectUIPrefab;
    [SerializeField] private StatusEffectsDictionary _statusEffectsData;

    private List<StatusEffectUI> _statusEffectUIs = new();

    private CombatantView _owner;
    private Transform _blockPosition;

    public void SetUp(CombatantView owner, Transform blockPosition)
    {
        _owner = owner;
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

            ui.Set(_owner, sprite, displayStackCount, stackable, info);
        }
    }

    private StatusEffectUI GetExistingUI(StatusEffectInfo info)
    {
        return _statusEffectUIs.Find(se => se.Info.Equals(info));
    }
}
