using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
[RequireComponent(typeof(TextMeshProUGUI))]
public class MovesDisplay : MonoBehaviour {

    [SerializeField] GameMaster gm;

    TextMeshProUGUI display;

	// Use this for initialization
	void Start () {
        display = GetComponent<TextMeshProUGUI>();

        gm.OnTileMove += (state)=>
        {
            display.text = $"Moves: {state.MovesMade}";
        };
	} 
}
