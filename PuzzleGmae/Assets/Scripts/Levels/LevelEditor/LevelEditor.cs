using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelEditor : MonoBehaviour
{
    public event Action<BaseValueAndColorGenerator> OnNewValueAndColorGen;

    [SerializeField] Map map;
    LevelLayout layout;
    BaseValueAndColorGenerator gen;
    Condition winCondition;
    Condition loseCondition;
    BaseMergeEffect mergeEffect;
    BaseMergeRule mergeRule;

    public int LevelWidth = 5;
    public int LevelHeight = 5;

    private void Start()
    {
        layout = new LevelLayout(LevelWidth, LevelHeight);

        UpdateSize();
    }

    public void UpdateSize()
    {
        map.BuildLevelLayout(layout);
        TryRecolorMoveableTiles();
    }

    public void ChangeWidth(float x)
    {
        LevelWidth = (int)x;
        layout.UpdateLevelWidth(LevelWidth);
        UpdateSize();
    }

    public void ChangeHeight(float y)
    {
        LevelHeight = (int)y;
        layout.UpdateLevelHeight(LevelHeight);
        UpdateSize();
    }

    public void SetValueAndColorGen(string data)
    {
        gen = ValueAndColorGeneratorFactory.BuildValueAndColorGenerator(data);
        OnNewValueAndColorGen?.Invoke(gen);
    }

    public void AddBlockingTile(Vector2Int gridPos)
    {
        layout.AddBlockingTile(new BlockingTileSpawnInfo() { GridPosition = gridPos, Health = 50f });
    }

    public void UpdateHealthOfBlockingTile(Vector2Int pos, float newHealth)
    {
        layout.UpdateBlockingTileValueAT(pos, newHealth);
    }

    public void RemoveBlockingTile(Vector2Int gridPos)
    {
        layout.RemoveBlockingTileAtPosition(gridPos);
    }

    public void AddMoveableTile(Vector2Int gridPos)
    {
        layout.AddMoveableTile(new MoveableTileSpawnInfo() { GridPosition = gridPos, Value = 0});
    }

    public void UpdateMoveableTile(Vector2Int gridPos, int newVal)
    {
        layout.UpdateMoveableTileValueAt(gridPos, newVal);
    }

    public void RemoveMoveableTile(Vector2Int gridPos)
    {
        layout.RemoveMoveableTileAtPosition(gridPos);
    }

    public void RemoveTileAt(Vector2Int pos)
    {
        RemoveBlockingTile(pos);
        RemoveMoveableTile(pos);
    }

    void TryRecolorMoveableTiles()
    {
        if(gen != null)
        {
            foreach(MoveableTileSpawnInfo info in layout.MoveableTiles)
            {
                Debug.Log(info.Value);
                map.GetTileAt(info.GridPosition.ToVec3XY(0), Tile.GetLayerForType(Tile.Type.Movable)).ChangeColor(gen.GetColorForValue(info.Value));
            }
        }
    }

    public void SetWinCondition(string data)
    {
        winCondition = ConditionFactory.BuildCondition(data);
    }

    public void SetLoseCondition(string data)
    {
        loseCondition = ConditionFactory.BuildCondition(data);
    }

    public void SetMergeRule(string data)
    {
        mergeRule = MergeRuleFactory.BuildMergeRule(data);
    }

    public void SetMergeEffect(string data)
    {
        mergeEffect = MergeEffectFactory.BuildMergeEffect(data);
    }

    public bool ValidCreation()
    {
        if(layout.MoveableTiles.Count <= 1)
        {
            WarningWindow.Instance.GiveWarning("Add Moveable Tiles", "For the level to be playable you need to add more than one moveable tile");
            return false;
        }
        if(gen == null)
        {
            WarningWindow.Instance.GiveWarning("Create Value and Color Generator", "For the level to be playable you need create a value and color generator");
            return false;
        }
        if(winCondition == null)
        {
            WarningWindow.Instance.GiveWarning("Choose wincondition", "For the level to be playable you need to chose a win condition");
            return false;
        }
        if(loseCondition == null)
        {
            WarningWindow.Instance.GiveWarning("Chose losecondition", "For the level to be playable you need to chose a losecondition");
            return false;
        }
        if(mergeEffect == null)
        {
            WarningWindow.Instance.GiveWarning("Chose merge effect", "For the level to be playable you need to chose a merge effect first");
            return false;
        }
        if(mergeRule == null)
        {
            WarningWindow.Instance.GiveWarning("Create a merge rule", "For the level to be playable you need to create a merge rule first");
            return false;
        }

        return true;
    }

    public Level MakeLevel()
    {
        if (!ValidCreation())
            return null;
        else
            return new Level(layout, gen, winCondition, loseCondition, mergeRule, mergeEffect);
    }

    public void PlayLevel()
    {
        Level l = MakeLevel();

        if(l != null)
        {
            //todo load level
            PlayerPrefs.SetString("level", l.Serialize());
            SceneManager.LoadScene("_PlayLevel_");
        }
    }
}
