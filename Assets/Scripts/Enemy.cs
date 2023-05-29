using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    void Update()
    {
        if (GameManager.Instance.gameState == GameState.Battle)
        {
            EnableNav(targets[0].position);

            if (!isBattleStart)
            {
                anim.SetBool(AnimType.ready.ToString(), true);
                anim.SetBool(AnimType.run.ToString(), false);
            }
        }

    }
    
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }
}
