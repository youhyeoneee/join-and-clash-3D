using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CameraData
{
    public Transform transform;
    public Vector3 offset;
    public float speed;
    [HideInInspector]
    public Vector3 velocity = Vector3.zero;
}

public class CameraController : MonoBehaviour
{
    [SerializeField] private CameraData[] cameras;
    [SerializeField] private GameObject target;

    private int currentIndex = 0;
    private Vector3 offsetPos;
    private Quaternion offsetRot;
    GameState gameState;

    void Start()
    {
        offsetPos = transform.position - target.transform.position;
        offsetRot = transform.rotation;
    }

    void Update()
    {
        gameState = GameManager.Instance.gameState;

        switch (currentIndex)
        {
            case 0:
                if (gameState == GameState.Run)
                {
                    StartCoroutine(MoveCamFollowPlayer(cameras[currentIndex]));
                    currentIndex++;
                }
                break;
            case 1:
                if (gameState == GameState.Battle)
                {
                    StartCoroutine(MoveCamEnding(cameras[currentIndex]));
                    currentIndex++;
                }
                break;
            case 2:
                if (gameState == GameState.Conquer)
                {
                    StartCoroutine(MoveCamEnding(cameras[currentIndex]));
                    currentIndex++;
                }
                break;
            case 3:
                if (gameState == GameState.Ended)
                {
                    StartCoroutine(MoveCamEnding(cameras[currentIndex]));
                    currentIndex++;
                }
                break;
            case 4:
                break;
        }

    }
    
    void LateUpdate()
    {
        if (gameState == GameState.Run)
        {
            transform.position = target.transform.position + offsetPos;
            transform.rotation = offsetRot; 
        }

    }

    IEnumerator MoveCamFollowPlayer(CameraData newCamera)
    {
        // 플레이어 따라다닐 때 이동
        while (offsetPos != newCamera.offset || offsetRot != newCamera.transform.rotation)
        {
            // offset 이동
            offsetPos = Vector3.SmoothDamp(offsetPos, newCamera.offset, ref newCamera.velocity, newCamera.speed);
            offsetRot = Quaternion.Slerp(offsetRot, newCamera.transform.rotation, newCamera.speed  * Time.deltaTime);
           
            yield return null;
        }
        
    }

    IEnumerator MoveCamEnding(CameraData newCamera)
    {
        float distanceThreshold = 0.01f;
        float angleThreshold = 0.05f;


        while (Vector3.Distance(transform.position, newCamera.transform.position) > distanceThreshold || Quaternion.Angle(transform.rotation, newCamera.transform.rotation) > angleThreshold)
        {
            var orginPos = transform.position;
            var originRot = transform.rotation;
            transform.position = Vector3.SmoothDamp(orginPos, newCamera.transform.position, ref newCamera.velocity, newCamera.speed);
            transform.rotation = Quaternion.Slerp(originRot, newCamera.transform.rotation, newCamera.speed * Time.deltaTime);
            
            yield return null;
        }
    }

}