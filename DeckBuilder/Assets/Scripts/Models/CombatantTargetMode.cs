
[System.Serializable]
public abstract class CombatantTargetMode : TargetMode<CombatantView>
{
    public virtual EnemyTargetTypes GetTargetIntent()
    {
        return EnemyTargetTypes.NONE;
    }
}
