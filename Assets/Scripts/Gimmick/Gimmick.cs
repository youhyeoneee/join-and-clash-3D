using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gimmick : MonoBehaviour
{

    [Header("Gimmick for Rotate")] 
    [SerializeField] protected bool _isRotate;
    [SerializeField] protected float _rotateSpeed;
    protected Quaternion _startRotation;


    protected virtual void Start()
    {
        _startRotation = transform.rotation;
    }

    protected virtual void Update()
    {
        if (_isRotate)
            Rotate();
    }

    protected abstract void Rotate();
    protected virtual void OnCollisionEnter(Collision other)
    {
        GameObject hitObject = other.collider.gameObject;
        // 플레이어와 충돌했을 경우
        if (hitObject.CompareTag(TagType.Player.ToString()))
        {
            ActivateGimmick(hitObject);
            // Key -> 아이템 획득
        }
    }
    protected abstract void ActivateGimmick(GameObject hitObject);
    
}
