﻿using System;
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

    int movesCounter;
    int mergeCounter;

    private void Awake()
    {
        if (Instance != null)
            throw new System.Exception("There alredy exitsts a game master");
        Instance = this;

        //currentLevel = new TestLevel(5, 5);
        //Debug.Log(currentLevel.Serialize());
        currentLevel = Level.Deserialize(LevelString.text);

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

    GameState GetCurrentGameState()
    {
        return new GameState(map, movesCounter, mergeCounter);
    }
}