using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockingTile : Tile
{
    [SerializeField] float health;
    public float Health {
        get
        {
            return health;
        }
        set
        {
            health = value;
            if (health <= 0)
                InvokeOnDeath();
        }
    }

    public void SetHealth(float health)
    {
        Health = health;
    }

    public override void Initialize(Vector3Int position, Map map)
    {
        base.Initialize(position, map);
        RegisterToOnDeath(Die);
    }

    void Die(Tile t)
    {
        map.RemoveTile(LayeredGridPosition);
        Destroy(gameObject);
    }

}
