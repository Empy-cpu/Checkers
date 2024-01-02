using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameMode gameMode = GameMode.Computer; 

    public void SetGameMode(GameMode mode)
    {
        gameMode = mode;
       
    }
}
