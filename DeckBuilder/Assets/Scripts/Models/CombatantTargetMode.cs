
[System.Serializable]
public abstract class CombatantTargetMode : TargetMode<CombatantView>
{
    public virtual NPCTargetTypes GetTargetIntent()
    {
        return NPCTargetTypes.NONE;
    }
}
