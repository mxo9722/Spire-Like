using UnityEngine;

public enum StatusEffect
{   [InspectorName("Unique")]
    UNIQUE = -1000,



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
    [InspectorName("Basic/Pinned")]
    PINNED,
    [InspectorName("Basic/Bruised")]
    BRUISED,
    [InspectorName("Basic/Bold")]
    BOLD,

    [InspectorName("Enemy/Bushido")]
    BUSHIDO = 1000,
    [InspectorName("Enemy/Stealth")]
    STEALTH,
    [InspectorName("Enemy/Taunt")]
    TAUNT,
    [InspectorName("Enemy/Guard")]
    GUARD,
    [InspectorName("Enemy/On Base")]
    ON_BASE,
    [InspectorName("Enemy/Cowardly")]
    COWARDLY,
    [InspectorName("Enemy/Bulwark")]
    BULWARK,
    [InspectorName("Enemy/Life Tap")]
    LIFE_TAP,
    
    [InspectorName("CaptainDragon/Chaos")]
    CHAOS = 2000,
    [InspectorName("CaptainDragon/Jabberwocky")]
    JABBERWOCKY,
    [InspectorName("CaptainDragon/Fear Aura")]
    FEAR_AURA,
    [InspectorName("CaptainDragon/Heat")]
    HEAT,
    [InspectorName("CaptainDragon/Backfire")]
    BACKFIRE,
    [InspectorName("CaptainDragon/Feint")]
    FEINT,
    [InspectorName("CaptainDragon/Zigzag")]
    ZIGZAG,

    [InspectorName("MonsoonMan/Cold Shock")]
    COLD_SHOCK = 3000,
}
