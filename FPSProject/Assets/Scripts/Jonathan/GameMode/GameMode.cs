using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface GameMode 
{
    void CreateGameObjectives();
    void CreateGameRules();
    void LoadGameModeUI();
}
