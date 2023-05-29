using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;

public class DisablePlayer : MonoBehaviour
{
    private NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (GameManager.Instance.gameState == GameState.Ended)
        {
            if (agent.remainingDistance<=agent.stoppingDistance)
            {
                Debug.Log("Here");
                transform.gameObject.SetActive(false);
            }
            else
            {
                Debug.Log("Here2" + agent.remainingDistance);

            }
        }
    }
}
