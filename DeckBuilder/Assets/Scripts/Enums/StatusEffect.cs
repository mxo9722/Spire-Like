using UnityEngine;

public enum StatusEffect
{
    [InspectorName("Basic/Block")]
    BLOCK = 0,
    [InspectorName("Basic/Burn")]
    BURN,
    [InspectorName("Basic/Dexterity")]
    DEXTERITY,
    [InspectorName("Basic/Frail")]
    FRAIL,
    [InspectorName("Basic/Strength")]
    STRENGTH,
    [InspectorName("Basic/Vulnerable")]
    VULNERABLE,
    [InspectorName("Basic/Weak")]
    WEAK,
    [InspectorName("Basic/Anchored")]
    ANCHORED,
    [InspectorName("Basic/Hamstrung")]
    HAMSTRUNG,



    [InspectorName("Enemy/Ninja Math")]
    NINJA_MATH = 1000,
    [InspectorName("Enemy/Stealth")]
    STEALTH,
    [InspectorName("Enemy/Taunt")]
    TAUNT,
    [InspectorName("Enemy/Guard")]
    GUARD,
    
        
    [InspectorName("CaptainDragon/Chaos")]
    CHAOS = 2000,
    [InspectorName("CaptainDragon/Jabberwocky")]
    JABBERWOCKY,
    [InspectorName("CaptainDragon/Fear Aura")]
    FEAR_AURA,
    [InspectorName("CaptainDragon/Heat")]
    HEAT,

    [InspectorName("MonsoonMan/Cold Shock")]
    COLD_SHOCK = 3000,
}
