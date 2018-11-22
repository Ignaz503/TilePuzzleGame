using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinScreen : MonoBehaviour
{
    [SerializeField] GameMaster gameMaster;

    private void Awake()
    {
        gameMaster.OnStateChange += TryDisplayWinScreen;
        gameObject.SetActive(false);
    }

    void TryDisplayWinScreen(GameState state)
    {

        if (gameMaster.CheckWon())
        {
            gameObject.SetActive(true);
        }
    }

}
