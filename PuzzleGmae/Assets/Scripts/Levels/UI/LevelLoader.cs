using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoader : MonoBehaviour {

    [SerializeField] GameObject levelDisplayPrefab;
    TextAsset[] textAssets;
	// Use this for initialization
	void Start () {

        textAssets = Resources.LoadAll<TextAsset>("Levels");

        foreach (TextAsset asset in textAssets)
        {
            GameObject obj = Instantiate(levelDisplayPrefab, transform);

            LevelDisplay d = obj.GetComponent<LevelDisplay>();
            d.Initalize(asset);
        }    

	}
}
