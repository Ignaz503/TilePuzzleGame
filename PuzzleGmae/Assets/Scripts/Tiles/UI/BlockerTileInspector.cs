using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockerTileInspector : TileInspector {

    [SerializeField] InputField health;
    BlockingTile blockerTileInspecting;
    
    private void Awake()
    {
        health.onEndEdit.AddListener(EditHealth);
    }

    public override void EnableInspector(Tile t)
    {
        base.EnableInspector(t);
        if (!(t is BlockingTile))
            throw new System.Exception("trying to inspect tile with non suitable tile inspector");
        blockerTileInspecting = t as BlockingTile;

        health.text = blockerTileInspecting.Health.ToString();
    }

    void EditHealth(string str)
    {
        float h;
        if(float.TryParse(str,out h))
        {
            if (h > 0)
            {
                blockerTileInspecting?.SetHealth(h);
                tilePlacer.UpdateValue(blockerTileInspecting.GridPosition.ToVec2IntXY(), h);
            }
        }
    }

   

}

