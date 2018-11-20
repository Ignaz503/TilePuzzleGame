using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public static GameMaster Instance { get; protected set; }

    public event Action<GameState> OnTileMove;
    public event Action<GameState> OnTileMerged;
    public event Action<GameState> OnTileDeath;
    public event Action<GameState> OnStateChange;

    [SerializeField]TextAsset LevelString;

    [SerializeField] Map map;
    [SerializeField] Level currentLevel;
    [SerializeField] bool loadFromPrefs;

    int movesCounter;
    int mergeCounter;

    private void Awake()
    {
        if (Instance != null)
            throw new System.Exception("There alredy exitsts a game master");
        Instance = this;

        //currentLevel = new TestLevel(5, 5);
        //Debug.Log(currentLevel.Serialize());
        if (!loadFromPrefs)
            currentLevel = Level.Deserialize(LevelString.text);
        else
        {
            Debug.Log(PlayerPrefs.GetString("level"));
            currentLevel = Level.Deserialize(PlayerPrefs.GetString("level"));
        }

        //ensure state change event called
        OnTileMove += (state) => { OnStateChange?.Invoke(state); };
        OnTileMerged += (state) => { OnStateChange?.Invoke(state); };
        //currently not in use cause nothing tracks tile deaths
        //OnTileDeath += (state) => { OnStateChange?.Invoke(state); };

    }

    private void Start()
    {
        map.RegisterToOnTileCreated(TileCreated);
        map.Initialize(currentLevel);

        OnStateChange += (state) => Debug.Log("Lost? " + currentLevel.CheckIfLost(state) + "\n Won? " + currentLevel.CheckIfAchievedWinCondition(state));
    }

    void TileCreated(Tile t)
    {
        t.RegisterToOnDeath(TileDeath);

        if(t is MoveableTile)
        {
            MoveableTile mT = t as MoveableTile;

            mT.RegisterToOnTileStartMove(TileMove);
            mT.RegisterToOnMerge(TileMerged);
        }

    }

    void TileDeath(Tile t)
    {
        t.UnregisterFromOnDeath(TileDeath);
        if(t is MoveableTile)
        {
            MoveableTile mT = t as MoveableTile;

            mT.UnregisterFromOnMerge(TileMerged);
            mT.UnregisterFromOnTileStartMove(TileMove);
        }
        OnTileDeath?.Invoke(GetCurrentGameState());
    }

    void TileMerged(Tile vanished, Tile mergedInto)
    {
        mergeCounter++;
        OnTileMerged?.Invoke(GetCurrentGameState());
    }

    void TileMove(Tile t)
    {
        movesCounter++;
        OnTileMove?.Invoke(GetCurrentGameState());
    }

    public bool CheckWon()
    {
        return currentLevel.CheckIfAchievedWinCondition(GetCurrentGameState());
    }

    public bool CheckIfLos()
    {
        return currentLevel.CheckIfLost(GetCurrentGameState());
    }

    GameState GetCurrentGameState()
    {
        return new GameState(map, movesCounter, mergeCounter);
    }

    public string GetMergeRuleDescription()
    {
        return currentLevel.GetMergeRuleDescription();
    }

    public string GetMergeEffectDescription()
    {
        return currentLevel.GetMergeEffectDescription();
    }

    public string GetWinConditionDescription()
    {
        return currentLevel.GetWinConditionDescription();
    }

    public string GetLoseConditionDescription()
    {
        return currentLevel.GetLoseConditionDescription();
    }

    public string GetValueAndColorGeneratorDescription()
    {
        return currentLevel.GetValueAndColorGeneratorDescription();
    }

    public void Restart()
    {
        map.Restart(currentLevel);
        movesCounter = 0;
        mergeCounter = 0;
    }

}
