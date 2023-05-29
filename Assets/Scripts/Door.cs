using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private GameObject mainPlayer;

    [SerializeField] private Transform targetPosition;
    [SerializeField] private float moveSpeed = 0.5f;      // 이동 속도
    [SerializeField] private GameObject _effectObj;
    [SerializeField] private GameObject player;

    private Vector3 initialPosition;  // 초기 위치
    private bool isMovingDown = true; // 아래로 이동 중인지 여부
    private MeshRenderer _mr;

    void Start()
    {
        _mr = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.gameState == GameState.Battle)
        {
            if (isMovingDown)
            {
                // 아래로 내려갈 때
                if (transform.position.y > targetPosition.position.y)
                {
                    Vector3 downTarget = new Vector3(transform.position.x, targetPosition.position.y, transform.position.z);
                    transform.position = Vector3.MoveTowards(transform.position, downTarget, moveSpeed * Time.deltaTime);
                }
                else
                {
                    isMovingDown = false;
                }
            }
            else
            {
                // 다시 올라갈 때
                if (transform.position.y < initialPosition.y)
                {
                    Vector3 upTarget = new Vector3(transform.position.x, initialPosition.y, transform.position.z);
                    transform.position = Vector3.MoveTowards(transform.position, upTarget, moveSpeed * Time.deltaTime);
                }
            }
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        GameObject hitObject = other.gameObject;
        if (GameManager.Instance.gameState == GameState.Conquer && hitObject.CompareTag(TagType.Player.ToString()))
        {
            Character character = hitObject.GetComponent<Character>();
            character.anim.SetTrigger(AnimType.destroy.ToString());;
            
            _effectObj.transform.parent = null;
            _effectObj.SetActive(true);
            _mr.enabled = false;
            player.SetActive(false);
            GameManager.Instance.isEndConquer = true;
        }
    }
}
