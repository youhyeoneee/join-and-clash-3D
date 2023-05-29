using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveGroup : MonoBehaviour
{
    [SerializeField] private float      moveXSpeed          = 50f;
    [SerializeField] private float      moveZSpeed          = 18f;

    private float dragDirection;
    GameState gameState;

    private void Update()
    {
        gameState = GameManager.Instance.gameState;
        switch (gameState)
        {
            case GameState.Run:
                if (Input.GetMouseButton(0))
                {
                    // 앞으로 이동
                    float moveZ = moveZSpeed * Time.fixedDeltaTime;
                    transform.Translate(Vector3.forward * moveZ);

                    // 마우스 드래그 입력 받기
                    dragDirection = Input.GetAxis("Mouse X");

                    // 좌우로 이동
                    float moveX = dragDirection * moveXSpeed * Time.fixedDeltaTime;
                    transform.Translate(Vector3.right * moveX);
                }
                break;
        }
    }

}
