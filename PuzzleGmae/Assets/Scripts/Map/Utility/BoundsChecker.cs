using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundsChecker : MonoBehaviour {

    [SerializeField] Map map;

    private void OnValidate()
    {
        if(map != null)
        {
            Debug.Log($"{transform.position} is contained in {map.FieldBounds}: { map.FieldBounds.Contains(transform.position)}");
        }
    }
}
