using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseScreen : MonoBehaviour
{
    [SerializeField] GameMaster gm;

    private void Awake()
    {
        gm.OnStateChange += TryDisplayLoseScreen;
        gameObject.SetActive(false);
    }

    void TryDisplayLoseScreen(GameState state)
    {
        if (gm.CheckIfLoss())
        {
            gameObject.SetActive(true);
        }
    }
}
