using System.Collections;
using UnityEngine;

public class SideKickSystem : Singleton<SideKickSystem>
{
    private void OnEnable()
    {
        ActionSystem.AttachPerformer<SummonSideKickGA>(SummonSideKickPerformer);
    }

    private void OnDisable()
    {
        ActionSystem.DetachPerformer<SummonSideKickGA>();
    }

    private IEnumerator SummonSideKickPerformer(SummonSideKickGA summonSideKick)
    {
        //may not have functionality going forward
        //var sideKick = CombatantViewCreator.Instance.CreateSideKickView(summonSideKick.Data, summonSideKick.TargetLane.AvailableHeroSlot());

        //DetermineNPCBehaviorGA determineEnemyBehaviorGA = new(sideKick);
        //ActionSystem.Instance.AddReaction(determineEnemyBehaviorGA);

        yield return null;
    }
}
