using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EventsData", menuName = "Data/EventsData")]
public class EventsData : ScriptableObject
{
    [field: SerializeField] public List<EventGraph> Events { get; private set; }
}
