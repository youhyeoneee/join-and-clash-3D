using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TagType
{
    Player, 
    Character,
    Enemy,
    Door,
}

public enum AnimType
{
    run, 
    offset,
    ready,
    dance,
    destroy
}

public enum GameState
{
    NotStart,
    Run,
    Battle,
    Conquer,
    Ended
}