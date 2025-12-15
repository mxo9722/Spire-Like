using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapSystem : Singleton<MapSystem>
{
    [SerializeField] private ScenePicker _combatScene = new();
    [SerializeField] private ScenePicker _eventScene = new();
    [SerializeField] private MapView _mapView;
    [SerializeField] private CombatData _combatData;
    [SerializeField] private EventsData _eventsData;

    private List<int> _eventIndexes = new();
    private List<int> _fightIndexes = new();
    private List<int> _bossFightIndexes = new();
    private List<int> _villainFightIndexes = new();


    [field: SerializeField] public int Levels { get; private set; } = 15;
    [field: SerializeField] public int Rows { get; private set; } = 7;
    [field: SerializeField] public int StartingRooms { get; private set; } = 4;


    private void Start()
    {
        if(RunSystem.Instance.RunData.Map == null)
        {
            Map map = MapViewCreator.Instance.GenerateMap();
            RunSystem.Instance.SetMap(map);
        }

        _mapView.SetUp(RunSystem.Instance.RunData.Map);
    }

    public void EnterCombat()
    {
        _combatScene.LoadScene();
    }

    public void EnterEvent()
    {
        _eventScene.LoadScene();
    }

    public void EnterRoom(Room room)
    {
        RunSystem.Instance.RunData.EnterRoom(room);
    }

    public Vector3 GetRoomPosition(int level, int row)
    {
        float distance = _mapView.GetMapDistance((float)level / ((float)Levels-1.0f));
        float rotation = (float)row / (float)Rows * Mathf.PI * 2;

        Vector3 position = new(Mathf.Sin(rotation), Mathf.Cos(rotation), 0);

        position *= distance;
        return position;
    }

    public int GetRoomIndex(List<int> skip, RoomType room)
    {
        List<int> indexList = null;
        int indexTotal = 0;

        switch (room)
        {
            case RoomType.EVENT:
                indexList = _eventIndexes;
                indexTotal = _eventsData.Events.Count;
                break;
            case RoomType.BOSS_FIGHT:
                indexList = _bossFightIndexes;
                indexTotal = _combatData.BossFights.Count;
                break;
            case RoomType.FIGHT:
                indexList = _fightIndexes;
                indexTotal = _combatData.Fights.Count;
                break;
        }

        if(indexList?.Except(skip).Count() == 0)
        {
            RefreshIndexList(indexList, indexTotal);
        }

        IEnumerable<int> possibleList = indexList.Except(skip);

        if (possibleList.Count() > 0)
        {
            int index = RNG.Random.Next(0, possibleList.Count());

            int ret = possibleList.ElementAt(index);

            indexList.RemoveAt(index);

            return ret;
        }
        return -1;
    }

    private void RefreshIndexList(List<int> indexList, int size)
    {
        indexList.Clear();
        for(int i = 0; i < size; i++)
        {
            indexList.Add(i);
        }
    }

    public void SetUpCombatRoom(CombatRoom room, int index)
    {
        if (index < 0)
            return;

        List<FightLayout> fights = null;

        switch (room)
        {
            case BossRoom:
                fights = _combatData.BossFights;
                break;
            case CombatRoom:
                fights = _combatData.Fights;
                break;
        }

        FightLayout fight = fights[index];

        fight.SetCombatLayout(room);
    }

    public void SetUpEventRoom(EventRoom room, int index)
    {
        if (index < 0)
            return;

        room.SetEventGraph(_eventsData.Events[index]);
    }

    public void RefreshMap(float duration = 0) => _mapView.RefreshMap(duration); 
}
